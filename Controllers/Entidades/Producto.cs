using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capi.Entities
{
    public class Producto
    {
        public int Id { get; set; }
        [Required]
        public string nomproducto { get; set; }
        [Required]
        public double precio { get; set; }
        [Required]
        // Crea productoId en la tabla detalles  
        public List<Detalle> detalle { get; set; }
    }
}