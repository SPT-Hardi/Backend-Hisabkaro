using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Staff.BankDetail
{
    public class UPIDetail
    {
        [RegularExpression(@"^[a-z0-9._]+@[a-z0-9-]+[a-z]{2,}$", ErrorMessage = "Only Numaber, 12 Digit Number!")]
        public string UPI { get; set; }
    }
}
