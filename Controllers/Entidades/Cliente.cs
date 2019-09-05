using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capi.Entities
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public string dni { get; set; }
        [Required]
        public string nombre { get; set; }
        // crea detalleId en la tabla detalles  
        public List<Detalle> detalles { get; set; }
    }
}