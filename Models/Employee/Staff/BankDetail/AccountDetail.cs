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
        public string AccountNumber { get; set; }
        public string ConAccountNumber { get; set; }
        public string IFSC { get; set; }
    }
}
