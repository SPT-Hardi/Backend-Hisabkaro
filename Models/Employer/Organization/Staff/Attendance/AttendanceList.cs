using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Attendance
{
    public class AttendanceList
    {
      public int URId { get; set; }
      public string AttendanceDate { get; set; }
      public string ImagePath { get; set; }
      public string Name { get; set; }
      public string Status { get; set; }
      public string CheckIN { get; set; }
      public string CheckOUT { get; set; }
      public string LateBy { get; set; }
    }
}
