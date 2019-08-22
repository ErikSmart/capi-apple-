using System;
using System.ComponentModel.DataAnnotations;

namespace Capi.Modelos
{
    public class LibroDTO
    {
        public int Id { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public int AutorId { get; set; }
    }
}