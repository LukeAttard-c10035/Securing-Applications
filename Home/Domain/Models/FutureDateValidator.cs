using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class FutureDateValidator : ValidationAttribute
    {   
        public override bool IsValid(object value)  
        {
            if (value == null) return true;
            try
            {
                 DateTime currentTime = DateTime.Now;
                 DateTime dateInput = Convert.ToDateTime(value);

                return dateInput > currentTime;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
