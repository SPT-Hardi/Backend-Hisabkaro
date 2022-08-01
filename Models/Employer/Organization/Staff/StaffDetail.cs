using HIsabKaro.Models.Common;
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

        [Required(ErrorMessage = "Salary Is Required!")]
        public decimal BasicSalary { get; set; }
        [Validation.Pair_IntegerString(ErrorMessage ="SalaryType is required!, Allow_Value:[Id:any digit][Text:Any], NotAllow_Value:[Id:0 or null][Text:null]")]
        public IntegerString SalaryType { get; set; } = new IntegerString();
        public decimal HRA { get; set; }
        public decimal TotalSalaryAmount { get; set; }

        [Required(ErrorMessage = "Mobile Number Is Required!")]
        public string MobileNumber { get; set; }
        public Common.IntegerNullString Organization { get; set; } = new Common.IntegerNullString();
        public List<Advances> Advances { get; set; } = new List<Advances>();

    }
    public class StaffDetailList 
    {
        public List<StaffDetail> StaffDetails { get; set; } = new List<StaffDetail>();
    }
    public class Advances 
    {
        public decimal? AdvanceAmount { get; set; } = 0;
        public decimal InterestRate { get; set; }
        public int TotalMonths { get; set; }
        public bool IsEMI { get; set; }
        public decimal FinalAmountWithInterest { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public List<EMIPerMonth> EMIPerMonths { get; set; } = new List<EMIPerMonth>();
    }
    public class EMIPerMonth
    {
        public decimal? EMIAmount { get; set; } = 0;
        public decimal InstallmentAmount { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public decimal TotalRemainAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int InstallmentMonth { get; set; }
    }
   /* public class Daily
    {
        public int SalaryAmount { get; set; }

    }*/
 /*   public class Monthly
    {
        public int SalaryAmount { get; set; }
        public decimal HRA { get; set; }

    }*/
    //public class StaffDetail
    //{
    //    [Required(ErrorMessage = "Name Is Required!")]
    //    public string Name { get; set; }
    //    public string Email { get; set; }
    //    [Required(ErrorMessage = "Mobile Number Is Required!")]
    //    public string MobileNumber { get; set; }
    //    public string AMobileNumber { get; set; }
    //    public Common.IntegerNullString Organization { get; set; } = new Common.IntegerNullString();
    //    public Common.IntegerNullString ShiftTiming { get; set; } = new Common.IntegerNullString();
    //    public bool IsOpenWeek { get; set; }
    //    public Common.IntegerNullString WeekOff1 { get; set; } = new Common.IntegerNullString();
    //    public Common.IntegerNullString WeekOff2 { get; set; } = new Common.IntegerNullString();
    //    [Required(ErrorMessage = "Salary Is Required!")]
    //    public float Salary { get; set; }
    //}
    //public class JoinOrganizationCreate
    //{    
    //    public string Name { get; set; }
    //    public string OrgCode { get; set; }
    //    public string MobileNumber { get; set; }
    //}

}
