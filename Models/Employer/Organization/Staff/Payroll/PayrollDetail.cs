using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Payroll
{
    public class PayrollDetail
    {
        public Boolean Status { get; set; }
        public Common.IntegerNullString Staff { get; set; } = new Common.IntegerNullString();
    }
}
