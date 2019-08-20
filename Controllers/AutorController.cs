using System;
using System.Collections.Generic;
using System.Linq;
using Capi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly DataContext context;
        public AutorController(DataContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Entities.Autor>> Get()
        {
            return context.Autores.ToList();
        }
        [HttpGet("{id}", Name = "ire")]
        public ActionResult<Autor> Ir(int id)
        {
            var elid = context.Autores.FirstOrDefault(x => x.Id == id);
            if (elid == null)
            {
                return NotFound();
            }
            return elid;
        }

        [HttpPost]
        public ActionResult<Autor> Insertar([FromBody] Autor autor)
        {
            context.Autores.Add(autor);
            context.SaveChanges();
            return new CreatedAtRouteResult("ire", new { id = autor.Id }, autor);
        }

        [HttpPut("{id}")]
        public ActionResult<Autor> Actulizar(int id, [FromBody] Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest();
            }

            context.Entry(autor).State = EntityState.Modified;
            context.SaveChanges();
            return Ok("200");
        }
        [HttpDelete("{id}")]
        public ActionResult<Autor> borrar(int id)
        {
            var autor = context.Autores.SingleOrDefault(x => x.Id == id);
            context.Autores.Remove(autor);
            context.SaveChanges();
            return autor;
        }
    }
}