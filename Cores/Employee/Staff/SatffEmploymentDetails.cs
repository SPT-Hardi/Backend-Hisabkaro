using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Staff
{
    public class SatffEmploymentDetails
    {
        public Result One(int URId)
        {
            using (DBContext c = new DBContext())
            {
                var _SId = c.DevOrganisationsStaffs.SingleOrDefault(o => o.URId == URId);
                if (_SId is null)
                {
                    throw new ArgumentException("Satff Does Not Exits!");
                }

                var _Org = (from x in c.DevOrganisationsStaffs
                            where x.URId == URId
                            select new
                            {
                                URId = URId,
                                DOB = x.DOB,
                                Gender = x.Gender,
                                Address = (from y in c.CommonContactAddresses
                                           where y.ContactAddressId == x.SubUserOrganisation.SubUser.SubUsersDetail.AddressID
                                           select new
                                           {
                                               AddressLine1 = y.AddressLine1,
                                               AddressLine2 = y.AddressLine2,
                                               City = y.City,
                                               State = y.State,
                                               PinCode = y.PinCode,
                                               LandMark = y.Landmark
                                           }).FirstOrDefault(),
                            }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Success"),
                    Data = _Org,

                };
            }
        }

        public Result Create(int URId, Models.Employee.Staff.SatffEmploymentDetail value)
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

                    if (_Staff.StaffEmpId is null)
                    {
                        var _staffEmp = new DevOrganisationsStaffsEmploymentDetail()
                        {
                            Designation = value.Designation,
                            Department = value.Department,
                            UAN = value.UAN,
                            ESI = value.ESI,
                            PAN = value.PAN,
                        };
                        c.DevOrganisationsStaffsEmploymentDetails.InsertOnSubmit(_staffEmp);
                        c.SubmitChanges();

                        var id = _staffEmp.StaffEmpId;

                        var _OrgStaff = c.DevOrganisationsStaffs.SingleOrDefault(u => u.URId == URId);
                        
                        _OrgStaff.StaffEmpId = id;
                        c.SubmitChanges();
                    }
                    else
                    {
                        var s = Update((int)_Staff.StaffEmpId, value);
                    }
                    
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Personal Detail Add Successfully!"),
                    };
                }
            }
        }

        public Result Update(int StaffEmpId, Models.Employee.Staff.SatffEmploymentDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _StaffEmp = c.DevOrganisationsStaffsEmploymentDetails.SingleOrDefault(x => x.StaffEmpId == StaffEmpId);
                    if (_StaffEmp is null)
                    {
                        throw new ArgumentException("Staff Employment Detail Does Not Exits!");
                    }

                    _StaffEmp.Designation = value.Designation;
                    _StaffEmp.Department = value.Department;
                    _StaffEmp.UAN = value.UAN;
                    _StaffEmp.ESI = value.ESI;
                    _StaffEmp.PAN = value.PAN;
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = string.Format($"Personal Detail Edit Successfully!"),
                        Data=_StaffEmp.StaffEmpId,
                    };
                }
            }
        }
    }
}
