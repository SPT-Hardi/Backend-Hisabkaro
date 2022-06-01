using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Loan
{
    public class LoanDetail
    {
        public Models.Common.IntegerNullString paymentType { get; set; } = new Models.Common.IntegerNullString();

        public DateTime StartDate { get; set; }

        public int month { get; set; }

        public decimal PrincipalAmount { get; set; }

        public decimal Monthlypay { get; set; }

        public string Description { get; set; }

        public decimal? InterestRate { get; set; }

        //public decimal? TotalAmount { get; set; }
    }
}
