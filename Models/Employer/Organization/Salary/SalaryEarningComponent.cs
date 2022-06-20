using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Salary
{
    public class SalaryEarningComponent : Models.Employer.Organization.Salary.SalaryComponent
    {
        public DateTime Date { get; set; }
        public Common.IntegerNullString PaymentType { get; set; } = new Common.IntegerNullString();
        public string Description { get; set; }
    }
}
