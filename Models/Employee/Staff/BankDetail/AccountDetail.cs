using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Staff.BankDetail
{
    public class AccountDetail
    {
        public string Name { get; set; }
        [RegularExpression(@"^[0-9]{9,18}$", ErrorMessage = "Only Numabers or 9 to 18 Digit Number!")]
        public string AccountNumber { get; set; }
        public string ConAccountNumber { get; set; }
        public string IFSC { get; set; }
    }
}
