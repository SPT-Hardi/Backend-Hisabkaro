using HisaabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Employer.Organization.Staff.Leave
{
    public class Submit
    {
        [Required]
        public int RId { get; set; }
        [Required]
        public int StaffRId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        //public IntegerNullString LeaveReason { get; set; } = new IntegerNullString();

        public string Reason { get; set; }
        [Required]
        public bool IsPaidLeave { get; set; }
       
  
    }
}
