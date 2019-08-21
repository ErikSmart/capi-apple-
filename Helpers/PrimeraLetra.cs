using System;
using System.ComponentModel.DataAnnotations;

namespace Capi.Helpers
{
    public class PrimeraLetra : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
    }
}