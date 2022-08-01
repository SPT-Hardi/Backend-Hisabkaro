using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Loan
{
    public class LoanInstallments
    {
        /*public Result Create(int LoanId, DBContext c)
        {
            var loan = c.OrgStaffsLoanDetails.Where(x => x.LoanId == LoanId).FirstOrDefault();
            if (loan == null)
            {
                throw new ArgumentException("Loan Doesn't exist");
            }

            DateTime month = loan.StartDate.AddMonths((int)loan.TotalMonth);
            for (DateTime dt = loan.StartDate; dt < month; dt = dt.AddMonths(1))
            {
                var duration = (month.Month - loan.StartDate.Month) - 1;
                var r = (decimal)loan.PayableAmt - ((loan.MonthlyPay) * duration) == 0 ? loan.MonthlyPay : (decimal)loan.PayableAmt - ((loan.MonthlyPay) * duration);

                var Installment = new OrgStaffLoanInstallmentDetail()
                {
                    LoanId = LoanId,
                    Month = dt.ToString("MMMM"),
                    Year = dt.Year.ToString(),
                    IsInstallmentCompleted = false,
                    MonthlyPay = dt.AddMonths(1) == month ? r : loan.MonthlyPay
                };
                c.OrgStaffLoanInstallmentDetails.InsertOnSubmit(Installment);
            }
            c.SubmitChanges();
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = "Loan details added successfully!",
            };
        }*/
    }
}
