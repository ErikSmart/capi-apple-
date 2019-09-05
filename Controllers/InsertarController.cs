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
            var insertar = new Producto();
            insertar.nomproducto = "Cepillo suave";
            insertar.precio = 343.21;

            context.Add(insertar);
            context.SaveChanges();
            return Ok();

        }

    }
}