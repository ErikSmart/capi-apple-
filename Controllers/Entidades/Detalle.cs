using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capi.Entities
{
    public class Detalle
    {
        public int Id { get; set; }
        [Required]
        public DateTime fecha { get; set; }
        [Required]
        public int cantidadcompra { get; set; }
        [Required]
        //Crea la relacion FOREIGN KEY de Productos (sin esta no se relaciona)
        public Producto producto { get; set; }
        //Crea la relacion FOREIGN KEY de Cliente (sin esta no se relaciona)
        public Cliente cliente { get; set; }
        public int productoId { get; set; }
    }

    //Herencia de tablas

    public class Entregado : Detalle
    {

    }

    public class Cancelado : Detalle
    {

    }
    public class Preparando : Detalle
    {
        public string enpaqueteria { get; set; }
        public string sinenviar { get; set; }
    }
}