using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capi.Modelos
{
    public class AutorDTO
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Identificacion { get; set; }
        public List<LibroDTO> Libros { get; set; }
    }
}