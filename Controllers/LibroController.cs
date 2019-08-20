using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Capi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private DataContext context;

        public LibroController(DataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Entities.Libro>> todos()
        {
            var l = context.Libros.Include(x => x.Autor).ToList();
            return l;
        }
        [HttpPost]
        public ActionResult<Libro> crear([FromBody] Libro libro)
        {
            var insertar = context.Libros.Add(libro);
            context.SaveChanges();
            return new CreatedAtRouteResult("creado", new { id = libro.Id }, libro);
        }
    }
}