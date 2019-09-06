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

    }
}