using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Staff
{
    public class SatffEmploymentDetail
    {    
        public DateTime DOJ { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Only Numaber, 12 Digit Number!")]
        public string UAN { get; set; }
        [RegularExpression(@"^[0-9]{17}$", ErrorMessage = "Only Numaber, 17 Digit Number!")]
        public string ESI { get; set; }

        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]{1}$", ErrorMessage = "Invalid Pan Card Number!")]
        public string PAN { get; set; }
    }
}
