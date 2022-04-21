using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Staff.BankDetail
{
    public class UPIDetails
    {
        public Result One(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var _User = c.SubUserOrganisations.SingleOrDefault(u => u.URId == URId);
                if (_User is null)
                {
                    throw new ArgumentException("User Does Not Exits!");
                }
                var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(u => u.URId == URId);
                if (_Staff is null)
                {
                    throw new ArgumentException("Staff Does Not Exits!");
                }

                var _Org = (from x in c.DevOrganisationsStaffsBankDetails
                            where x.BankDetailId == _Staff.BankDetailId
                            select new
                            {
                                URId = URId,
                                UPI=x.UPI,
                            }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Success"),
                    Data = _Org,

                };
            }
        }

        public Result Create(int URId, Models.Employee.Staff.BankDetail.UPIDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUserOrganisations.SingleOrDefault(u => u.URId == URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(u => u.URId == URId);
                    if (_Staff is null)
                    {
                        throw new ArgumentException("Staff Does Not Exits!");
                    }

                    if (_Staff.BankDetailId is null)
                    {
                        var _StaffBank = new DevOrganisationsStaffsBankDetail()
                        {
                            UPI=value.UPI,
                        };
                        c.DevOrganisationsStaffsBankDetails.InsertOnSubmit(_StaffBank);
                        c.SubmitChanges();

                        var id = _StaffBank.BankDetailId;
                        _Staff.BankDetailId = _StaffBank.BankDetailId;
                        c.SubmitChanges();
                    }
                    else
                    {
                        var _StaffBank = Update((int)_Staff.BankDetailId, value);
                    }
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Bank Detail Add Successfully!"),
                    };
                }
            }
        }

        public Result Update(int BankDetailId, Models.Employee.Staff.BankDetail.UPIDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _StaffBank = c.DevOrganisationsStaffsBankDetails.SingleOrDefault(x => x.BankDetailId == BankDetailId);
                    if (_StaffBank is null)
                    {
                        throw new ArgumentException("Staff Bank Detail Does Not Exits!");
                    }

                    _StaffBank.UPI = value.UPI;
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Bank Detail Add Successfully!"),
                        Data = _StaffBank.BankDetailId,
                    };
                }
            }
        }
    }
}