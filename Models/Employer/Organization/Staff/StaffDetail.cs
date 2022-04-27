using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff
{
    public class StaffDetail
    {
        [Required(ErrorMessage = "Name Is Required!")]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile Number Is Required!")]
        public string MobileNumber { get; set; }
        public string AMobileNumber { get; set; }
        public Common.IntegerNullString Organization { get; set; } = new Common.IntegerNullString();
        public Common.IntegerNullString ? Branch { get; set; } = new Common.IntegerNullString();
        public Common.IntegerNullString ShiftTiming { get; set; } = new Common.IntegerNullString();
        public bool IsOpenWeek { get; set; }
        public Common.IntegerNullString WeekOff1 { get; set; } = new Common.IntegerNullString();
        public Common.IntegerNullString WeekOff2 { get; set; } = new Common.IntegerNullString();
        [Required(ErrorMessage = "Salary Is Required!")]
        public float Salary { get; set; }
    }
    public class JoinOrganizationCreate
    {    
        public string Name { get; set; }
        public string OrgCode { get; set; }
        public string MobileNumber { get; set; }
    }
}
