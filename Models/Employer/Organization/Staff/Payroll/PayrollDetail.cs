using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Payroll
{
    public class PayrollDetail
    {
        public List<StaffPayroll> StaffLists { get; set; } = new List<StaffPayroll>();
        public decimal Amount { get; set; }
    }
    public class StaffPayroll
    {
        public Boolean Status { get; set; }
        public Common.IntegerNullString Staff { get; set; } = new Common.IntegerNullString(); 
    }
    public class View
    {
        public Boolean Status { get; set; } = false;
        public IntegerNullString Staffset { get; set; } = new IntegerNullString();
        public string Profile { get; set; }
        public string MobileNumber { get; set; }
        public string Hours { get; set; }
    }
}
