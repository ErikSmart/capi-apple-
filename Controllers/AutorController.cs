using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Capi.Entities;
using Capi.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AutorController : ControllerBase
    {
        private readonly DataContext context;
        private readonly IMapper mapper;

        public AutorController(DataContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<AutorDTO>> Get()
        {
            var autores = context.Autores.ToList();
            var autoresdto = mapper.Map<List<AutorDTO>>(autores);
            return autoresdto;
        }

        [HttpGet("{id}", Name = "ire")]
        public async Task<ActionResult<AutorDTO>> Ir(int id)
        {
            var elid = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);
            if (elid == null)
            {
                return NotFound();
            }

            var autordto = mapper.Map<AutorDTO>(elid);
            return autordto;
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