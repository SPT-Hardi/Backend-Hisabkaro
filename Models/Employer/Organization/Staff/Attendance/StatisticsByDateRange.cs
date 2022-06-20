using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Attendance
{
    public class StatisticsByDateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int URId { get; set; }
        public string Name { get; set; }
        public int TotalDays { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int WeeklyOff { get; set; }
        public int Late { get; set; }
        public int PaidLeave { get; set; }
        public int FullOverTime { get; set; }
        public int HalfOverTime { get; set; }
        public List<Status> Status { get; set; } = new List<Status>();
    }
    public class Status 
    {
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsWeeklyOff { get; set; }
        public bool IsLate { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsOvertimeFull { get; set; }
        public bool IsOvertimeHalf { get; set; }
        public string StatusString { get; set; }
    }
}
