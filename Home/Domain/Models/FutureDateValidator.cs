using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class FutureDateValidator : ValidationAttribute
    {   
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)  
        {
            if (value == null) return ValidationResult.Success;
            try
            {
                 DateTime currentTime = DateTime.Now;
                 DateTime dateInput = Convert.ToDateTime(value);

                if(dateInput <= currentTime)
                {
                    var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                    return new ValidationResult(errorMessage);
                }
                return ValidationResult.Success;
            }
            catch (Exception)
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }

        }
    }
}
