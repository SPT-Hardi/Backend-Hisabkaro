using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Loan
{
    public class LoanDetail
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal PrincipalAmount { get; set; }

        public decimal? Monthlypay { get; set; }

        public string Description { get; set; }

        public string InterestRate { get; set; }

        //public decimal? TotalAmount { get; set; }
    }
}
