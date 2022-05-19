using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Staff
{
    public class SatffEmploymentDetails
    {
        public Result One(object URId)
        {
            using (DBContext c = new DBContext())
            {
                var _User = c.SubUserOrganisations.SingleOrDefault(u => u.URId == (int)URId);
                if (_User is null)
                {
                    throw new ArgumentException("User Does Not Exits!");
                }
                var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(u => u.URId == (int)URId);
                if (_Staff is null)
                {
                    throw new ArgumentException("Staff Does Not Exits!");
                }

                var _Org = (from x in c.DevOrganisationsStaffsEmploymentDetails
                            where x.StaffEmpId == _Staff.StaffEmpId
                            select new
                            {
                                URId=URId,
                                DOJ=x.DOJ,
                                Designation = x.Designation,
                                Department = x.Department,
                                UAN = x.UAN,
                                ESI = x.ESI,
                                PAN = x.PAN,
                            }).ToList();

                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = string.Format("Success"),
                    Data = _Org,

                };
            }
        }

        public Result Create(object URId, Models.Employee.Staff.SatffEmploymentDetail value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var _User = c.SubUserOrganisations.SingleOrDefault(u => u.URId == (int)URId);
                    if (_User is null)
                    {
                        throw new ArgumentException("User Does Not Exits!");
                    }
                    var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(u => u.URId == (int)URId);
                    if (_Staff is null)
                    {
                        throw new ArgumentException("Staff Does Not Exits!");
                    }

                    if (_Staff.StaffEmpId is null)
                    {
                        var _StaffEmp = new DevOrganisationsStaffsEmploymentDetail()
                        {
                            DOJ=value.DOJ,
                            Designation = value.Designation,
                            Department = value.Department,
                            UAN = value.UAN,
                            ESI = value.ESI,
                            PAN = value.PAN,
                        };
                        c.DevOrganisationsStaffsEmploymentDetails.InsertOnSubmit(_StaffEmp);
                        c.SubmitChanges();

                        var id = _StaffEmp.StaffEmpId;
                        _Staff.StaffEmpId = _StaffEmp.StaffEmpId;
                        c.SubmitChanges();
                    }
                    else
                    {
                        var _StaffEmployer = Update((int)_Staff.StaffEmpId, value);
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

                    _StaffEmp.DOJ = value.DOJ;
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
