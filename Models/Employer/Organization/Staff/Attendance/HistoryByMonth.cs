using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Attendance
{
    public class HistoryByMonth
    {
        public int URId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string MobileNumber { get; set; }
        public string AttendanceMonth { get; set; }
        public string TotalWorkingHourPerMonth { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int WeeklyOff { get; set; }
        public int OverTime { get; set; }
        public List<AttendanceHistory> attendanceHistory { get; set; } = new List<AttendanceHistory>();

    }
    public class AttendanceHistory 
    {
        public int URId { get; set; }
        public string AttendanceDate { get; set; }
        public DateTime Date { get; set; }
        public string TotalWorkingHourPerDay { get; set; }
        public string Status { get; set; }
        public string CheckIN { get; set; }
        public string CheckOUT { get; set; }
        public string LateBy { get; set; }
    }
   
}

