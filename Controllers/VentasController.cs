using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Capi.Entities;
using Capi.Modelos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capi.Controllers
{
    [EnableCors("PermitirApiRequest")]
    [ApiController]
    [Route("/")]
    public class VentasController : ControllerBase
    {
        private DataContext context;
        private IMapper mapper;
        public VentasController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
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
            return Ok(producto);
        }
        //Crear registro DTO con auto mapper revisar Startup.cs y agregar services.AddAutoMapper(options => { options.CreateMap<CrearProductoDTO, Producto>(); }); 
        [HttpPost("/dto")]
        public async Task<ActionResult> insertar([FromBody] CrearProductoDTO crearProductoDTO)
        {
            var productodto = mapper.Map<Producto>(crearProductoDTO);
            await context.AddAsync(productodto);
            await context.SaveChangesAsync();
            var devolver = mapper.Map<ProductoDTO>(productodto);
            return Ok(devolver);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Producto>> actulizar(int id, [FromBody] Producto producto)
        {

            if (id != producto.Id)
            {
                return BadRequest();
            }
            context.Entry(producto).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok("producto");
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
            await context.SaveChangesAsync();
            return Ok(encontrar);
        }

    }
}