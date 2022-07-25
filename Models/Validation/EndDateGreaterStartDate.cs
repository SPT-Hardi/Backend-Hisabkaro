using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Validation
{
    public class EndDateGreaterStartDate : ValidationAttribute
    {
        public DateTime? StartDate { get; set; }
        public override bool IsValid(object value)
        {
            if (value == null) 
            {
                return true;
            }
            var d = Convert.ToDateTime(value);
            if (StartDate == null)
            {
                return true;
            }
            else if (StartDate < d) 
            {
                return true;
            }
            return false;
        }
    }
}
