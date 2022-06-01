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
        public enum PaymentType
        {
            FullAmount = 59,
            EMI = 60
        }

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

                //var loan = (from x in c.OrgStaffsLoanDetails
                //            where x.SubUserOrganisation_URId.OId == user.OId
                //            orderby x.LoanId descending
                //            select new
                //            {
                //                LoanId = x.LoanId,
                //                Name = x.SubUserOrganisation_StaffURId.DevOrganisationsStaffs.Select(y => y.NickName).FirstOrDefault(),
                //                StartDate = x.StartDate,
                //                PrincipalAmount = x.PrincipalAmt,
                //                MontlyPay = x.MonthlyPay,
                //                RemainingAmount = x.RemainingAmt,
                //                Duration = x.TotalMonth,
                //                InstallmentPaid = (DateTime.Now.ToLocalTime().Month - x.StartDate.Month) <= 0 ? 0 : (DateTime.Now.ToLocalTime().Month - x.StartDate.Month),
                //                Status = x.IsLoanPending == true ? "Pending" : "Completed" 
                //            }).ToList();

                var loan = (from x in c.OrgStaffsLoanDetails
                            where x.SubUserOrganisation_URId.OId == user.OId
                            orderby x.LoanId descending
                            select new
                            {
                                LoanId = x.LoanId,
                                Name = x.SubUserOrganisation_StaffURId.DevOrganisationsStaffs.Select(y => y.NickName).FirstOrDefault(),
                                StartDate = x.StartDate,
                                PrincipalAmount = x.PrincipalAmt,
                                PayableAmount = x.PayableAmt,
                                MontlyPay = x.MonthlyPay,
                                RemainingAmount = x.RemainingAmt,
                                Duration = x.TotalMonth,
                                InstallmentPaid = (from y in c.OrgStaffLoanInstallmentDetails
                                                   where y.LoanId == x.LoanId && y.IsInstallmentCompleted == true
                                                   select y).Count(),
                                                   //(DateTime.Now.ToLocalTime().Month - x.StartDate.Month) <= 0 ? 0 : (DateTime.Now.ToLocalTime().Month - x.StartDate.Month),
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

        public Result Create(object URId, int StaffId, Models.Employer.Organization.Staff.Loan.LoanDetail value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                decimal totalmonth = 0;
                decimal monthlypay = 0;
                decimal rate = 0;
                decimal InterestPaid = 0;
                int _days = 0;

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

                    var org_staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.OId == user.OId && x.URId == StaffId);
                    if (org_staff == null)
                    {
                        throw new ArgumentException("Staff Doesn't exist in org");
                    }

                    if(value.paymentType.Id == (int)PaymentType.FullAmount)
                    {
                        throw new ArgumentException("Paymnet Type not valid here");
                    }

                    var month = value.StartDate.AddMonths(value.month);
                    for (DateTime dt = value.StartDate; dt < month; dt = dt.AddMonths(1))
                    {
                        _days = _days + DateTime.DaysInMonth(dt.Year, dt.Month);
                    }
                    int totalDays = Math.Abs((_days - value.StartDate.Day)) + 1;

                    if (value.InterestRate == 0 || value.InterestRate == null)
                    {
                        if (value.Monthlypay != 0 && value.PrincipalAmount != 0)
                        {
                            totalmonth = Math.Ceiling(value.PrincipalAmount / value.Monthlypay);
                            monthlypay = value.Monthlypay;
                        }
                        else
                        {
                            monthlypay = value.PrincipalAmount / value.month;
                            totalmonth = value.month;
                        }
                    }
                    else
                    {
                        rate = value.InterestRate == 0 ? 0 : (decimal)(value.InterestRate / 365);
                        InterestPaid = (value.PrincipalAmount * rate * totalDays) / 100;
                        monthlypay = (value.PrincipalAmount + InterestPaid) / value.month;
                        totalmonth = value.month;
                        if (value.Monthlypay > monthlypay)
                        {
                            throw new ArgumentException("Monthly Pay can't be greater than calculate EMI!!");
                        }
                    }

                    var _loan = new OrgStaffsLoanDetail()
                    {
                        StartDate = value.StartDate.ToLocalTime(),
                        EndDate = DateTime.Now,
                        PrincipalAmt = value.PrincipalAmount,
                        MonthlyPay = monthlypay,
                        Description = value.Description,
                        URId = (int)URId,
                        StaffURId = staff.URId,
                        InterestRate = (int?)value.InterestRate,
                        InterestAmt = InterestPaid,
                        PayableAmt = value.PrincipalAmount + InterestPaid,
                        RemainingAmt = value.PrincipalAmount + InterestPaid,
                        IsLoanPending = true,
                        TotalMonth = totalmonth
                    };
                    c.OrgStaffsLoanDetails.InsertOnSubmit(_loan);
                    c.SubmitChanges();

                    var installment = new Cores.Employer.Organization.Staff.Loan.LoanInstallments().Create(_loan.LoanId, c);
                    var name = staff.SubUser.SubUsersDetail.FullName;
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Loan details added successfully!",
                        Data = new
                        {
                            Id = _loan.LoanId,
                            Name = name
                        },
                    };
                }
            }
        }

        public Result GetStaffLoan(object URId, int LoanId)
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
                        Duration = (decimal)item.TotalMonth,
                        Interestrate = item.InterestRate.ToString(),
                        PayableAmt = item.PayableAmt,
                        InterestAmt = (decimal)item.InterestAmt,
                        InstallmentPaid = (DateTime.Now.ToLocalTime().Month - item.StartDate.Month) <= 0 ? 0 : (DateTime.Now.ToLocalTime().Month - item.StartDate.Month)
                    });
                }

                //var payment = c.OrgStaffsLoanDetails.Where(x => x.StaffURId == (int)URId && x.LoanId == LoanId).SingleOrDefault();
                //for (DateTime dt = payment.StartDate; dt < payment.EndDate; dt = dt.AddMonths(1))
                //{
                //    var m = (Math.Abs(dt.Month - payment.EndDate.Month));
                //    var duration = (payment.EndDate.Month - payment.StartDate.Month) - 1;
                //    var r = (decimal)payment.PayableAmt - ((payment.MonthlyPay) * duration);

                //    loanView.payment.Add(new PaymentView()
                //    {
                //        month = dt.ToString("dd-MMM-yyyy"),
                //        amount = dt.AddMonths(1) == payment.EndDate ? r : payment.MonthlyPay,
                //        InstallmentPaid = ((ISDT.Month - dt.Month) <= 0 ? "Unpaid" : "Paid").ToString()
                //    });
                //}

                var payment = c.OrgStaffLoanInstallmentDetails.Where(x => x.LoanId == LoanId).ToList();
                foreach (var item in payment)
                {
                    loanView.payment.Add(new PaymentView()
                    {
                        month = item.Month + "-" + item.Year,
                        amount = (decimal)item.MonthlyPay,
                        InstallmentPaid = item.IsInstallmentCompleted == true ? "Paid" : "Unpaid"
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
