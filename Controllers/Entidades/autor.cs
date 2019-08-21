using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Capi.Entities
{
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public List<Libro> Libros { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();
                if (primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe ser mayuscula", new string[] { nameof(Nombre) });
                }
            }

        }

    }



}