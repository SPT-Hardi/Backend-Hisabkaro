using HIsabKaro.Cores.Employee.Profile;
using HIsabKaro.Cores.Employer.Organization.Staff.Attendance;
using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization.Staff
{
    public class StaffOrganizations
    {
       /* public Result Get(object URId) 
        {
            using (HisabKaroContext.DBContext c = new HisabKaroContext.DBContext())
            {
                var ISDT = new Common.ISDT().GetISDT(DateTime.Now);
                
                var _User = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId);
                if (_User is null)
                {
                    throw new ArgumentException("User Does Not Exits!");
                }

                var _URId = c.SubUserOrganisations.SingleOrDefault(x => x.URId == (int)URId && x.SubRole.RoleName == "staff");
                if (_URId is null)
                {
                    throw new ArgumentException("Unathorized!");
                }

                var _Staff = c.DevOrganisationsStaffs.SingleOrDefault(x => x.URId == (int)URId);
                if (_Staff is null)
                {
                    throw new ArgumentException("Staff Does Not Exits!");
                }
                                
                return new Result()
                {

                    Status = Result.ResultStatus.success,
                    Message = "Staff organization details get successfully!",
                    Data = new
                    {
                        OrgId = _Staff.DevOrganisation.OId,
                        OrgName = _Staff.DevOrganisation.OrganisationName,
                        ORString = _Staff.DevOrganisation.QRString,
                        UserName = _Staff.NickName,
                        Profile= _Staff.SubUserOrganisation.SubUser.SubUsersDetail.FileId ==null ? null: _Staff.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FGUID,
                        StaffId = _Staff.SId,
                        Designation = _Staff.DevOrganisationsStaffsEmploymentDetail==null?"": _Staff.DevOrganisationsStaffsEmploymentDetail.Designation,
                        JoiningDate = _Staff.CreateDate,
                        IsActive = _Staff.Status,
                        WorkExperice = new ViewProfiles().Get(_Staff.SubUserOrganisation.SubUser.UId).Data,
                        TotalAdvance = (from x in c.OrgStaffsAdvanceDetails
                                        where x.StaffURId == (int)URId
                                        select new { Total = x.Amount }).Sum(x => x.Total),
                        Salary = (from x in c.OrgStaffsSalaryDetails
                                  where x.StaffURId == (int)URId
                                  select new { PayDate=x.Date,Salary=x.ASalary,Payment = x.Salary }).ToList(),         
                    }
                };
            }
        }*/
    }
}
