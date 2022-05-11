using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Salary
{
    public class SalarySlip
    {
        public Organisation organisation { get; set; } = new Organisation();
        public EmployeeDetail employeeDetail { get; set; } = new EmployeeDetail();
        public AttendanceDetail attendanceDetail { get; set; } = new AttendanceDetail();
        public Earning earning { get; set; } = new Earning();
        public Deduction deduction { get; set; } = new Deduction();
        public decimal NetPay { get; set; }
    }

    public class Organisation
    {
        public int OId { get; set; }
        public string OrganizationName { get; set; }
        public string Logo { get; set; }
        public string GST { get; set; }
        public string PAN { get; set; }
    }

    public class EmployeeDetail
    {
        public int EmployeeId { get; set; }
        public string HolderName { get; set; }
        public string AccountNo { get; set; }
        public string IFSC { get; set; }
        public string PAN { get; set; }
        public string AadharCard { get; set; }
    }

    public class AttendanceDetail
    {
        public int Workingdays { get; set; }
        public int Present { get; set; }
        public int WeeklyOff { get; set; }
        public int Absent { get; set; }
        public int PaidLeave { get; set; }
    }

    public class Earning
    {
        public decimal Salary { get; set; }
        public decimal Overtime { get; set; }
        public decimal Bonus { get; set; }
        public decimal TotalEarning { get; set; }
    }

    public class Deduction
    {
        public decimal Loan { get; set; }
        public decimal Advance { get; set; }
        public decimal Leave { get; set; }
        public decimal TotalDeduction { get; set; }
    }
}
