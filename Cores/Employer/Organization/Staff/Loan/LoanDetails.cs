using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Loan;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Loan
{
    public class LoanDetails
    {
        public Result Create(int URId,int StaffId,Models.Employer.Organization.Staff.Loan.LoanDetail value)
        {
            using(TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                    if(user == null)
                    {
                        throw new ArgumentException("User Doesn't exist");
                    }

                    var staff = c.SubUserOrganisations.SingleOrDefault(x => x.URId == StaffId);
                    if (staff == null)
                    {
                        throw new ArgumentException("Staff Doesn't exist");
                    }

                    var org_staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.OId == user.OId && x.URId == staff.URId);
                    if (org_staff == null)
                    {
                        throw new ArgumentException("Staff Doesn't exist in org");
                    }

                    int totalMonth = 12 * (value.StartDate.Year - value.EndDate.Year) + value.StartDate.Month - value.EndDate.Month;
                    int month = (Math.Abs(totalMonth));
                    var t = (month / 12)  == 0 ? 0 : (month / 12);
                    decimal principal = value.Amount;
                    decimal rate = decimal.Parse(value.Interest.Text);
                    double no = Math.Round(((float)month / 12), 2);
                    decimal interestPaid = principal * (rate / 100) * Convert.ToDecimal(no);
                    decimal PrincipalAmt = principal + interestPaid;
                    decimal monthlypay = PrincipalAmt / month;
                    var loan = new OrgStaffsLoanDetail()
                    {
                        StartDate = value.StartDate.ToLocalTime(),
                        EndDate = value.EndDate.ToLocalTime(),
                        Amount = value.Amount,
                        Duration = (t == 0 ? $"{(month % 12)}month" : $"{(month / 12)}year{(month % 12)}month"),
                        MonthlyPay = (decimal)(value.Monthlypay == null ? (decimal)monthlypay : value.Monthlypay),
                        Description = value.Description,
                        URId = URId,
                        StaffURId = staff.URId,
                        InterestId = (int)value.Interest.Id,
                        PrincipalAmt = PrincipalAmt,
                        RemainingAmt = PrincipalAmt,
                        Status = true
                    };
                    c.OrgStaffsLoanDetails.InsertOnSubmit(loan);
                    c.SubmitChanges();
                    var name = staff.SubUser.SubUsersDetail.FullName;
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Loan details added successfully!",
                        Data = new
                        {
                            Id = loan.LoanId,
                            Name = name
                        },
                    };
                }
            }
        }

        public Result GetOrgLoan(int URId)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't exist");
                }

                if (user.SubRole.RoleName.ToLower() != "admin")
                {
                    throw new ArgumentException("Access not allow!!");
                }

                var loan = (from x in c.OrgStaffsLoanDetails
                            where x.SubUserOrganisation_URId.OId == user.OId
                            select new
                            {
                                LoanId = x.LoanId,
                                UserName = x.SubUserOrganisation_StaffURId.SubUser.SubUsersDetail.FullName,
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                PrincipalAmount = x.PrincipalAmt,
                                MontlyPay = x.MonthlyPay,
                                RemainingAmount = x.RemainingAmt,
                                Duration = x.Duration,
                                InstallmentPaid = (DateTime.Now.ToLocalTime().Month - x.StartDate.Month) <= 0 ? 0 : (DateTime.Now.ToLocalTime().Month - x.StartDate.Month),
                            }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Loan List",
                    Data = loan
                };
            }
        }

        public Result GetStaffLoan(int URId,int LoanId)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                LoanView loanView = new LoanView();
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't exist");
                }

                var loan = c.OrgStaffsLoanDetails.Where(x => x.StaffURId == URId && x.LoanId == LoanId).ToList();
                if (loan.Count() == 0)
                {
                    throw new ArgumentException("Loan Doesn't exist");
                }
                foreach (var item in loan)
                {
                    int totalMonth = 12 * (item.StartDate.Year - item.EndDate.Year) + item.StartDate.Month - item.EndDate.Month;
                    int month = (Math.Abs(totalMonth));
                    decimal principal = item.Amount;
                    decimal rate = decimal.Parse(item.SubFixedLookup.FixedLookup);
                    decimal no = (decimal)Math.Round(((float)month / 12), 2);
                    decimal interestPaid = principal * (rate / 100) * (no);
                    loanView.loan.Add(new LoanDetailView()
                    {
                        LoanId = item.LoanId,
                        LoanAmount = item.Amount,
                        Duration = item.Duration,
                        Interestrate = item.SubFixedLookup.FixedLookup,
                        DueOn = item.EndDate,
                        PrincipalAmount = item.PrincipalAmt,
                        Interest = interestPaid,
                        lastMonth = item.MonthlyPay,
                        InstallmentPaid = (DateTime.Now.ToLocalTime().Month - item.StartDate.Month) <= 0 ? 0 : (DateTime.Now.ToLocalTime().Month - item.StartDate.Month),
                    });
                }
              
                var payment = c.OrgStaffsLoanDetails.Where(x => x.StaffURId == URId && x.LoanId == LoanId).SingleOrDefault();
                for (DateTime dt = payment.StartDate ; dt < payment.EndDate; dt = dt.AddMonths(1))
                {
                    var m = (Math.Abs(dt.Month - payment.EndDate.Month));
                    var duration = (payment.EndDate.Month - payment.StartDate.Month) - 1;
                    var r = (decimal)payment.PrincipalAmt - ((payment.MonthlyPay) * duration);
                    decimal v = ((decimal)(m == 0 ? r : payment.MonthlyPay));
                    loanView.payment.Add(new PaymentView()
                    {
                        month = dt.ToString("MMM"),
                        amount = v,
                        InstallmentPaid = ((ISDT.Month - dt.Month) <= 0 ? "Unpaid" : "Paid").ToString() 
                    });
                }
                var name = user.SubUser.SubUsersDetail.FullName;
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Loan List",
                    Data = loanView
                };
            }
        }
    }
}
