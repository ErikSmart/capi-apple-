using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capi.Controllers
{
    [ApiController]
    [Route("insertar")]
    public class InsertarController : ControllerBase
    {
        private DataContext context;
        public InsertarController(DataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entities.Producto>>> ver()
        {
            var idproducto = await context.productos.Select(x => x.Id).FirstOrDefaultAsync();
            var insertar = new Cancelado();
            insertar.cantidadcompra = 50;
            insertar.fecha = DateTime.Now;
            insertar.productoId = 2;
            context.Add(insertar);
            context.SaveChanges();

            var Prepa = new Preparando();
            Prepa.cantidadcompra = 2;
            Prepa.fecha = DateTime.Now;
            Prepa.productoId = 3;
            Prepa.sinenviar = "si";
            Prepa.enpaqueteria = "no";
            context.Add(Prepa);
            context.SaveChanges();
            return Ok();

        }

    }
}