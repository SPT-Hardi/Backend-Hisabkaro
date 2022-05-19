using HIsabKaro.Cores.Employee.Profile;
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
                var oid = c.DevOrganisationsStaffs.Where(x => x.URId == (int)URId).SingleOrDefault();

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
                        StaffId = oid.SId,
                        JoiningDate = oid.CreateDate,
                        IsActive=oid.Status,
                        WorkExperice= new ViewProfiles().Get(oid.SubUserOrganisation.SubUser.UId).Data,

                    }
                };
            }
        }
    }
}
