using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Attendance
{
    public class AttendanceList
    {
      public int URId { get; set; }
      public DateTime AttendanceDate { get; set; }
      public string ImagePath { get; set; }
      public string Name { get; set; }
      public string Status { get; set; }
      public string CheckIN { get; set; }
      public string CheckOUT { get; set; }
      public string LateBy { get; set; }
        public bool IsPresent { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsWeeklyOff { get; set; }
        public bool IsLate { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsOverTime { get; set; }
    }
}
