using HIsabKaro.Models.Common;
using HIsabKaro.Models.Employer.Organization.Staff.Loan;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employer.Organization.Staff.Loan
{
    public class LoanDetails
    {
        //public Result Create(object URId,int StaffId,Models.Employer.Organization.Staff.Loan.LoanDetail value)
        //{
        //    using(TransactionScope scope = new TransactionScope())
        //    {
        //        using (DBContext c = new DBContext())
        //        {
        //            var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId ==(int)URId);
        //            if(user == null)
        //            {
        //                throw new ArgumentException("User Doesn't exist");
        //            }

        //            var staff = c.SubUserOrganisations.SingleOrDefault(x => x.URId == StaffId);
        //            if (staff == null)
        //            {
        //                throw new ArgumentException("Staff Doesn't exist");
        //            }

        //            if (value.StartDate > value.EndDate)
        //            {
        //                throw new ArgumentException("Start Date Can't be After End Date.");
        //            }

        //            var org_staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.OId == user.OId && x.URId == staff.URId);
        //            if (org_staff == null)
        //            {
        //                throw new ArgumentException("Staff Doesn't exist in org");
        //            }

        //            int totalMonth = 12 * (value.StartDate.Year - value.EndDate.Year) + value.StartDate.Month - value.EndDate.Month;
        //            int month = (Math.Abs(totalMonth));
        //            var t = (month / 12)  == 0 ? 0 : (month / 12);
        //            decimal principal = value.Amount;
        //            decimal rate = decimal.Parse(value.Interest.Text) / 12;
        //            double no = Math.Round(((float)month / 12), 2);
        //            decimal interestPaid = principal * (rate / 100) * month;//* Convert.ToDecimal(no);
        //            decimal PrincipalAmt = principal + interestPaid;
        //            decimal monthlypay = PrincipalAmt / month;
        //            var loan = new OrgStaffsLoanDetail()
        //            {
        //                StartDate = value.StartDate.ToLocalTime(),
        //                EndDate = value.EndDate.ToLocalTime(),
        //                Amount = value.Amount,
        //                Duration = (t == 0 ? $"{(month % 12)}month" : $"{(month / 12)}year{(month % 12)}month"),
        //                MonthlyPay = (decimal)(value.Monthlypay == null ? (decimal)monthlypay : value.Monthlypay),
        //                Description = value.Description,
        //                URId = (int)URId,
        //                StaffURId = staff.URId,
        //                InterestId = (int)value.Interest.Id,
        //                PrincipalAmt = PrincipalAmt,
        //                RemainingAmt = PrincipalAmt,
        //                IsLoanPending = true
        //            };
        //            c.OrgStaffsLoanDetails.InsertOnSubmit(loan);
        //            c.SubmitChanges();
        //            var name = staff.SubUser.SubUsersDetail.FullName;
        //            scope.Complete();
        //            return new Result()
        //            {
        //                Status = Result.ResultStatus.success,
        //                Message = "Loan details added successfully!",
        //                Data = new
        //                {
        //                    Id = loan.LoanId,
        //                    Name = name
        //                },
        //            };
        //        }
        //    }
        //}

        public Result GetOrgLoan(object URId)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
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
                            orderby x.LoanId descending
                            select new
                            {
                                LoanId = x.LoanId,
                                Name = x.SubUserOrganisation_StaffURId.DevOrganisationsStaffs.Select(y => y.NickName).FirstOrDefault(),
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                PrincipalAmount = x.PrincipalAmt,
                                MontlyPay = x.MonthlyPay,
                                RemainingAmount = x.RemainingAmt,
                                Duration = x.Duration,
                                InstallmentPaid = (DateTime.Now.ToLocalTime().Month - x.StartDate.Month) <= 0 ? 0 : (DateTime.Now.ToLocalTime().Month - x.StartDate.Month),
                                Status = x.IsLoanPending == true ? "Pending" : "Completed" 
                            }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Loan List",
                    Data = loan
                };
            }
        }

        public Result GetStaffLoan(object URId,int LoanId)
        {
            var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
            using (DBContext c = new DBContext())
            {
                LoanView loanView = new LoanView();
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't exist");
                }

                var loan = c.OrgStaffsLoanDetails.Where(x => x.StaffURId == (int)URId && x.LoanId == LoanId).ToList();
                if (loan.Count() == 0)
                {
                    throw new ArgumentException("Loan Doesn't exist");
                }
                foreach (var item in loan)
                {
                    loanView.loan.Add(new LoanDetailView()
                    {
                        LoanId = item.LoanId,
                        LoanAmount = item.PrincipalAmt,
                        Duration = item.Duration,
                        Interestrate = item.InterestRate.ToString(),
                        DueOn = item.EndDate,
                        PayableAmt = item.PrincipalAmt,
                        InterestAmt = (decimal)item.InterestAmt,
                        InstallmentPaid = (DateTime.Now.ToLocalTime().Month - item.StartDate.Month) <= 0 ? 0 : (DateTime.Now.ToLocalTime().Month - item.StartDate.Month)
                    });
                }
              
                var payment = c.OrgStaffsLoanDetails.Where(x => x.StaffURId == (int)URId && x.LoanId == LoanId).SingleOrDefault();
                for (DateTime dt = payment.StartDate ; dt < payment.EndDate; dt = dt.AddMonths(1))
                {
                    var m = (Math.Abs(dt.Month - payment.EndDate.Month));
                    var duration = (payment.EndDate.Month - payment.StartDate.Month) - 1;
                    var r = (decimal)payment.PayableAmt - ((payment.MonthlyPay) * duration);

                    loanView.payment.Add(new PaymentView()
                    {
                        month = dt.ToString("dd-MMM-yyyy"),
                        amount = dt.AddMonths(1) == payment.EndDate ? r : payment.MonthlyPay,
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

        public Result Create(object URId, int StaffId, Models.Employer.Organization.Staff.Loan.LoanDetail value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                    if (user == null)
                    {
                        throw new ArgumentException("User Doesn't exist");
                    }

                    var staff = c.SubUserOrganisations.SingleOrDefault(x => x.URId == StaffId);
                    if (staff == null)
                    {
                        throw new ArgumentException("Staff Doesn't exist");
                    }

                    if (value.StartDate > value.EndDate)
                    {
                        throw new ArgumentException("Start Date Can't be After End Date.");
                    }

                    var org_staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.OId == user.OId && x.URId == staff.URId);
                    if (org_staff == null)
                    {
                        throw new ArgumentException("Staff Doesn't exist in org");
                    }

                    
                    int totalMonth = Math.Abs(12 *(value.StartDate.Year - value.EndDate.Year) + value.StartDate.Month - value.EndDate.Month);
                    var t = (totalMonth / 12) == 0 ? 0 : (totalMonth / 12);
                    decimal principal = value.PrincipalAmount;
                    decimal rate = decimal.Parse(value.InterestRate) / 12 / 100;
                    decimal u = (decimal)Math.Pow((double)rate + 1, totalMonth);
                    decimal monthlypay = principal * rate * (u / (u - 1));
                    decimal PayableAmt = monthlypay * totalMonth;

                    //if (value.TotalAmount != Math.Round(PayableAmt,2))
                    //{
                    //    throw new ArgumentException("Error in calculating");
                    //}

                    var loan = new OrgStaffsLoanDetail()
                    {
                        StartDate = value.StartDate.ToLocalTime(),
                        EndDate = value.EndDate.ToLocalTime(),
                        PrincipalAmt  = value.PrincipalAmount,
                        Duration = (t == 0 ? $"{(totalMonth % 12)}month" : $"{(totalMonth / 12)}year{(totalMonth % 12)}month"),
                        MonthlyPay = (decimal)(value.Monthlypay == null ? (decimal)monthlypay : value.Monthlypay),
                        Description = value.Description,
                        URId = (int)URId,
                        StaffURId = staff.URId,
                        InterestRate = (int?)decimal.Parse(value.InterestRate),
                        InterestAmt = PayableAmt - principal,
                        PayableAmt = PayableAmt,
                        RemainingAmt = PayableAmt,
                        IsLoanPending = true
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
    }
}
