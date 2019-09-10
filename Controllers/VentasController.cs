using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capi.Controllers
{
    [ApiController]
    [Route("/")]
    public class VentasController : ControllerBase
    {
        private DataContext context;
        public VentasController(DataContext context)
        {
            this.context = context;
        }
        [HttpGet("/")]
        public async Task<ActionResult<IEnumerable<Entities.Producto>>> ir()
        {
            var vista = await context.productos.Include(x => x.detalle).ThenInclude(y => y.cliente).ToListAsync();

            //var herencia = await context.detalles.OfType<Cancelado>().ToListAsync();

            return vista;
        }
        [HttpGet("/abrir")]
        public async Task<ActionResult<IEnumerable<Entities.Detalle>>> LaHerencia()
        {
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