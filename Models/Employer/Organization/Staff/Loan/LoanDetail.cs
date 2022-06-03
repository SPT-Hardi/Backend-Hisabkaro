using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Loan
{
    public class LoanDetail
    {
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }


        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter only digit!")]
        public int month { get; set; }


        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter only digit!")]
        public decimal PrincipalAmount { get; set; }


        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter only digit!")]
        public decimal Monthlypay { get; set; }


        public string Description { get; set; }


        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Please Enter only digit!")]
        public decimal? InterestRate { get; set; }
    }
}
