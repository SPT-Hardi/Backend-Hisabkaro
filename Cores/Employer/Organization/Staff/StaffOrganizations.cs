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
        public Result Get(object URId) 
        {
            using (HisabKaroContext.DBContext c = new HisabKaroContext.DBContext())
            {
                var ISDT = new Common.ISDT().GetISDT(DateTime.Now);

                var oid = c.DevOrganisationsStaffs.Where(x => x.URId == (int)URId).SingleOrDefault();
                var r = oid.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile == null ? "" : oid.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath;
                var attedanace = new HistoryByMonths().Get(URId, 0, ISDT);
                return new Result()
                {

                    Status = Result.ResultStatus.success,
                    Message = "Staff organization details get successfully!",
                    Data = new
                    {
                        OrgId = oid.DevOrganisation.OId,
                        OrgName = oid.DevOrganisation.OrganisationName,
                        ORString = oid.DevOrganisation.QRString,
                        UserName = oid.SubUserOrganisation.SubUser.SubUsersDetail.FullName,
                        Profile=oid.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile ==null ? "": oid.SubUserOrganisation.SubUser.SubUsersDetail.CommonFile.FilePath,
                        StaffId = oid.SId,
                        JoiningDate = oid.CreateDate,
                        IsActive = oid.Status,
                        WorkExperice = new ViewProfiles().Get(oid.SubUserOrganisation.SubUser.UId).Data,
                        TotalAdvance = (from x in c.OrgStaffsAdvanceDetails
                                        where x.StaffURId == (int)URId
                                        select new { Total = x.Amount }).Sum(x => x.Total),
                        Present = attedanace.Data.Present,
                        Absent = attedanace.Data.Absent,
                        Late = attedanace.Data.Late,
                        WeeklyOff = attedanace.Data.WeeklyOff,
                        Salary = (from x in c.OrgStaffsSalaryDetails
                                  where x.StaffURId == (int)URId
                                  select new { Salary=x.ASalary,Payment = x.Salary }).ToList(),                    }
                };
            }
        }
    }
}
