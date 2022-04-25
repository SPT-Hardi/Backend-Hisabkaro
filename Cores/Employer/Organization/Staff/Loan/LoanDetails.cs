﻿using HIsabKaro.Models.Common;
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
                        URId = StaffId,
                        InterestId = (int)value.Interest.Id,
                        PrincipalAmt = PrincipalAmt,
                        RemainingAmt = PrincipalAmt,
                        Status = "Approved"
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

        public Result GetStaffLoan(int URId)
        {
            using (DBContext c = new DBContext())
            {
                LoanDetail loanDetail = new LoanDetail();
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't exist");
                }

                var loan = (from x in c.OrgStaffsLoanDetails
                            where x.URId == user.URId
                            select new
                            {
                                LoanId = x.LoanId,
                                Intrestrate = x.SubFixedLookup.FixedLookup,
                                DueOn = x.EndDate,
                                PrincipalAmount = x.PrincipalAmt,
                                MontlyPay = x.MonthlyPay,
                                Duration = x.Duration,
                                InstallmentPaid = (Math.Abs(DateTime.Now.Month - x.StartDate.Month)),
                                RemainingAmount = x.PrincipalAmt - x.Amount,
                                Payment = (from y in c.OrgStaffsLoanDetails
                                           where y.URId == x.URId
                                           select new
                                           {
                                               month = y.StartDate.Month,
                                               pay = y.MonthlyPay
                                           }),
                            }).ToList();

                var Payment = c.OrgStaffsLoanDetails.Where(x => x.URId == URId ).ToList();
                foreach (var item in Payment)
                {
                    var totalmonth = item.StartDate.Month - item.EndDate.Month;
                    var l = (new LoanDetail()
                    {
                      
                    });
                }
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
                                LoanId = x.LoanId,
                                UserName = x.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                                StartDate = x.StartDate,
                                EndDate = x.EndDate,
                                PrincipalAmount = x.PrincipalAmt,
                                MontlyPay = x.MonthlyPay,
                                RemainingAmount = x.RemainingAmt,
                                Duration = x.Duration,
                                InstallmentPaid = (Math.Abs(DateTime.Now.Month - x.StartDate.Month)),
                            }).ToList();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Loan List",
                    Data = loan
                };
            }
        }

        public Result Get(int URId,int LoanId)
        {
            using (DBContext c = new DBContext())
            {
                LoanView loanView = new LoanView();
                var user = c.SubUserOrganisations.SingleOrDefault(x => x.URId == URId);
                if (user == null)
                {
                    throw new ArgumentException("User Doesn't exist");
                }

                var loan = c.OrgStaffsLoanDetails.Where(x => x.URId == URId && x.LoanId == LoanId).ToList();
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
                        InstallmentPaid = (Math.Abs(DateTime.Now.Month - item.StartDate.Month)),
                    });
                }
              
                var payment = c.OrgStaffsLoanDetails.Where(x => x.URId == URId && x.LoanId == LoanId).SingleOrDefault();
                for (DateTime dt = payment.StartDate.AddMonths(1) ; dt <= payment.EndDate; dt = dt.AddMonths(1))
                {
                    var m = (Math.Abs(dt.Month - payment.EndDate.Month));
                    var duration = (payment.EndDate.Month - payment.StartDate.Month) - 1;
                    var r = (decimal)payment.PrincipalAmt - ((payment.MonthlyPay) * duration);
                    decimal v = ((decimal)(m == 0 ? r : payment.MonthlyPay));
                    loanView.payment.Add(new PaymentView()
                    {
                        month = dt.ToString("MMM"),
                        amount = v,
                        InstallmentPaid = ((DateTime.Now.Month - dt.Month) <= 0 ? "Unpaid" : "Paid").ToString() 
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
