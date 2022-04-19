using HIsabKaro.Models.Employer.Organization.Staff.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Dashboard.Attendance
{
    public class Report
    {
        public Statistics statistics { get; set; } = new Statistics();
        public List<AttendanceList> attendanceLists { get; set; } = new List<AttendanceList>();
    }
}
