using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Staff.Loan
{
    public class LoanView
    {
        public List<LoanDetailView> loan { get; set; } = new List<LoanDetailView>();
        public List<PaymentView> payment { get; set; } = new List<PaymentView>();
    }

    public class LoanDetailView
    {
        public int LoanId { get; set; }
        public decimal PayableAmt { get; set; }
        public DateTime DueOn { get; set; }
        public decimal LoanAmount { get; set; }
        public string Duration { get; set; }
        public int InstallmentPaid { get; set; }
        public decimal InterestAmt { get; set; }
        public string Interestrate { get; set; }
    }

    public class PaymentView
    {
        public string month { get; set; }
        public decimal amount { get; set; }
        public string InstallmentPaid { get; set; }
    }
}
