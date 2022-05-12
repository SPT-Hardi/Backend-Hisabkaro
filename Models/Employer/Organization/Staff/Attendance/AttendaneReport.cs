using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Attendance
{
    public class AttendaneReport
    {
        public int URId { get; set; }
        public string Name { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int Overtime { get; set; }
        public int WeekOff { get; set; }
        public string TotalWorkingHours { get; set; }
    }
}
