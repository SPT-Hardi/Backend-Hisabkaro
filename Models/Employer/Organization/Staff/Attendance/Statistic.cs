using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Attendance
{
    public class Statistic
    {
        public int TotalEmployee { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int WeeklyOff { get; set; }
    }
    
}
