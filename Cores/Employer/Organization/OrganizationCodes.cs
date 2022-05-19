using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Employer.Organization
{
    public class OrganizationCodes
    {
        public Result Get(object URId) 
        {
            using (DBContext c = new DBContext())
            {
                var oid = (int)(c.SubUserOrganisations.Where(x => x.URId == (int)URId).SingleOrDefault().OId);
                if (oid == 0) 
                {
                    throw new ArgumentException("Organization not exist!");
                }
               
                var orgcode = c.DevOrganisations.Where(x => x.OId == oid).SingleOrDefault();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Organization code get successfully!",
                    Data =orgcode.OrgCode,
                };
            }
        }
    }
}
