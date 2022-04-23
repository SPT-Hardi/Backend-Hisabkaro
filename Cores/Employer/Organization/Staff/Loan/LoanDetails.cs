using HIsabKaro.Models.Common;
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

                    //decimal MontlyPay = PrincipalAmt / month;
                    //var t = (value.EndDate.Year - value.StartDate.Year);
                    //double totaldays =(value.EndDate - value.StartDate).TotalDays;
                    ////month = (int)Math.Floor(totaldays / 30);
                    //////var total = Convert.ToDouble(value.Amount) * totaldays;
                    //////var interest = (total / int.Parse(value.Interest.Text)) +  (total % int.Parse(value.Interest.Text));
                    //////var monthlypay = total + interest;
                    //var interestRate = 0.12;
                    //////var i = //Convert.ToDecimal(int.Parse(value.Interest.Text) / 100);
                    ////var interest = Convert.ToDouble(value.Amount) * (totaldays / 365) * interestRate;
                    ////var monthlypay = (interest + Convert.ToDouble(value.Amount)) / totaldays ;

                    int totalMonth = 12 * (value.StartDate.Year - value.EndDate.Year) + value.StartDate.Month - value.EndDate.Month;
                    int month = (Math.Abs(totalMonth));
                    var d = $"{(month / 12)}year{(month % 12)}month";
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
                        Duration = d,
                        MonthlyPay = (decimal)(value.Monthlypay == null ? (decimal)monthlypay : value.Monthlypay),
                        Description = value.Description,
                        URId = StaffId,
                        InterestId = value.Interest.Id,
                        PrincipalAmt = PrincipalAmt
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
                            Name = "name"
                        },
                    };
                }
            }
        }

        public Result GetStaffLoan(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't exist");
                }

                var loan = (from x in c.OrgStaffsLoanDetails
                            where x.URId == user.URId
                            select new
                            {
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                Amount = x.Amount,
                                MontlyPay = x.MonthlyPay,
                                Duration = x.Duration
                            }).ToList();
                var name = user.SubUser.SubUsersDetail.FullName;
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Loan List",
                    Data = loan
                };
            }
        }

        public Result GetOrgLoan(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't exist");
                }

                var loan = (from x in c.OrgStaffsLoanDetails
                            where x.SubUserOrganisation.OId == user.OId
                            select new
                            {
                                UserName = x.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                Amount = x.Amount,
                                MontlyPay = x.MonthlyPay,
                                Duration = x.Duration
                            }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Loan List",
                    Data = loan
                };
            }
        }
    }
}
