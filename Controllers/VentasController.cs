using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Capi.Entities;
using Capi.Hubs;
using Capi.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Capi.Controllers
{
    [EnableCors("PermitirApiRequest")]
    [ApiController]
    [Route("/")]
    public class VentasController : ControllerBase
    {
        private DataContext context;
        private IMapper mapper;
        private SqlConnection conn;
        private IHubContext<ChatHub> _hub;
        private readonly IConfiguration configuration;
        private void Conectar()
        {
            string connString = configuration.GetConnectionString("Conexion");
            var conn = new SqlConnection(connString);
        }
        public VentasController(DataContext context, IMapper mapper, IHubContext<ChatHub> hub, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
            _hub = hub;
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        //[Authorize(Roles = "e@g.com, Admin")]
        //[Authorize(Roles = "Dios")]
        [HttpGet("/")]
        public async Task<ActionResult<IEnumerable<Entities.Producto>>> ir()
        {



            var vista = await context.productos.Include(x => x.detalle).ThenInclude(y => y.cliente).ToListAsync();

            //Prueba para el tiempo 

            //await Task.Delay(10000);

            return vista;
            // Trer solo los regitros de la Herencia 

            //var herencia = await context.detalles.OfType<Cancelado>().ToListAsync();
        }
        [HttpGet("/ver")]
        public ActionResult ver()
        {
            string connString = configuration.GetConnectionString("Conexion");
            var conn = new SqlConnection(connString);




            //SqlDataReader
            conn.Open();
            List<Producto> teacherList = new List<Producto>();

            string sql = "Select * From productos"; SqlCommand command = new SqlCommand(sql, conn);
            SqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                Producto teacher = new Producto();
                teacher.Id = Convert.ToInt32(dataReader["Id"]);
                teacher.nomproducto = Convert.ToString(dataReader["nomproducto"]);
                teacherList.Add(teacher);
            }


            conn.Close();

            /* string connString = configuration.GetConnectionString("DefaultConnection");
            using (var conn = new SqlConnection(connString))
            {
                conn.Open();
                //No funciona si usas Select *
                using (var cmd = new SqlCommand("SELECT Nombre FROM [dbo].Personas", conn))
                {
                    return cmd.ExecuteReader().ToString(); // Hay que correr el query
                }
            } */
            var lista = context.productos.Select(r => new { r.nomproducto, r.precio });
            var lista2 = context.productos.ToList();

            var resultado = context.productos.FromSql("select top(20) percent  id, precio, nomproducto from productos").IgnoreQueryFilters().Select(x => new { x.Id, x.nomproducto });

            var json = JsonConvert.SerializeObject(resultado.ToArray());
            var jsona = JsonConvert.SerializeObject(lista.ToArray());
            var jsonaa = JsonConvert.SerializeObject(teacherList.ToArray());
            var deserializedProduct = JsonConvert.DeserializeObject(json);
            var deserializedProducta = JsonConvert.DeserializeObject(jsona);
            var deserializedProductaa = JsonConvert.DeserializeObject(jsonaa);

            return Ok(deserializedProductaa);

        }

        [HttpGet("/dto")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> mostrarmiDTO()
        {
            var buscar = await context.productos.ToListAsync();
            var pDTO = mapper.Map<List<ProductoDTO>>(buscar);
            return Ok(pDTO);
        }

        [HttpGet("/abrir")]
        public async Task<ActionResult<IEnumerable<Entities.Detalle>>> LaHerencia()
        {
            // Para OfType es para traer los datos de la herencia
            var herencia = await context.detalles.OfType<Preparando>().ToListAsync();
            return herencia;
        }
        [HttpPost]
        public async Task<ActionResult<Producto>> agregar([FromBody] Producto producto)
        {
            context.Add(producto);
            await context.SaveChangesAsync();
            var timerManager = _hub.Clients.All.SendAsync("transferchartdata", producto);
            return Ok(producto);
        }
        //Crear registro DTO con auto mapper revisar Startup.cs y agregar services.AddAutoMapper(options => { options.CreateMap<CrearProductoDTO, Producto>(); }); 
        [HttpPost("/dto")]
        public async Task<ActionResult> insertar([FromBody] CrearProductoDTO crearProductoDTO)
        {
            var productodto = mapper.Map<Producto>(crearProductoDTO);
            await context.AddAsync(productodto);
            await _hub.Clients.All.SendAsync("transferchartdata", productodto);
            await context.SaveChangesAsync();
            var devolver = mapper.Map<ProductoDTO>(productodto);
            return Ok(devolver);

        }
        //Crear registro DTO con auto mapper revisar Startup.cs y agregar services.AddAutoMapper(options => { options.CreateMap<ActualizarProductoDTO, Producto>(); }); 
        [HttpPut("{id}")]
        public async Task<ActionResult> actulizar(int id, [FromBody] ActualizarProductoDTO crearProductoDTO)
        {
            /* var productodto = mapper.Map<ProductoDTO>(productos);
            var devolver = mapper.Map<ActualizarProductoDTO>(productodto); */
            var productodto = mapper.Map<Producto>(crearProductoDTO);
            if (id != productodto.Id)
            {
                return BadRequest();
            }
            await _hub.Clients.All.SendAsync("transferchartdata", productodto);
            context.Entry(productodto).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Ok("producto");
        }
        //Crear registro DTO con auto mapper revisar Startup.cs y agregar services.AddAutoMapper(options => { options.CreateMap<CrearProductoDTO, Producto>(); }); 
        //Actulizacion Parcial con patch RFC 6902 {"op": "replace","path": "/nomproducto", "value":"Lo que se quiere replazar" }
        [HttpPatch("{id}")]
        public async Task<IActionResult> parcial(int id, [FromBody] JsonPatchDocument<ActualizarProductoDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var miproducto = await context.productos.FirstOrDefaultAsync(x => x.Id == id);
            if (miproducto == null)
            {
                return NotFound();
            }


            ActualizarProductoDTO productoDTO = mapper.Map<ActualizarProductoDTO>(miproducto);
            //mapper.Map(productoDTO, ModelState);

            patchDocument.ApplyTo(productoDTO, ModelState);

            var esvalido = TryValidateModel(productoDTO);

            if (!esvalido)
            {
                return BadRequest(ModelState);
            }
            await context.SaveChangesAsync();



            return Ok(productoDTO);

        }
        /* [Authorize(Roles = "Dios")] */
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> detalle(int id)
        {
            var unico = await context.productos.FirstOrDefaultAsync(x => x.Id == id);
            if (unico == null)
            {
                return NotFound();
            }

            return unico;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Producto>> eliminar(int id)
        {
            var sinoexiste = await context.productos.FirstOrDefaultAsync(x => x.Id == id);
            if (sinoexiste == null)
            {
                return NotFound();
            }
            var encontrar = await context.productos.FirstAsync(x => x.Id == id);
            context.productos.Remove(encontrar);
            await _hub.Clients.All.SendAsync("transferchartdata", encontrar);
            await context.SaveChangesAsync();
            return Ok(encontrar);
        }

    }
}