using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Capi.Helpers;

namespace Capi.Entities
{
    public class Autor
    {
        public int Id { get; set; }
        [Required]
        [PrimeraLetraMAttribute]
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }
    }
}