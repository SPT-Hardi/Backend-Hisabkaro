using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Salary
{
    public class SalaryComponent
    {
        public List<StaffList> StaffLists { get; set; } = new List<StaffList>();
        public decimal Amount { get; set; }
    }
    public class StaffList
    {
        public Boolean Status { get; set; }
        public Common.IntegerNullString Staff { get; set; } = new Common.IntegerNullString();
    }
    public class View
    {
        public Boolean Status { get; set; } = false;
        public Common.IntegerNullString Staff { get; set; } = new Common.IntegerNullString();
        public string Profile { get; set; }                                                           
        public string MobileNumber { get; set; }                                                     
        public string Hours { get; set; }                                                       
    }    
}
