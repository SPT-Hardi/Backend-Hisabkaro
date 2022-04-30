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
            using (HisabKaroDBContext.DBContext c = new HisabKaroDBContext.DBContext()) 
            {
                var oid = c.SubUserOrganisations.Where(x => x.URId == (int)URId).SingleOrDefault();
                var org = c.DevOrganisations.Where(x => x.OId ==(oid==null ? null : oid.OId)).SingleOrDefault();
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Staff organization details get successfully!",
                    Data = new IntegerNullString()
                    {
                        Id = org == null ? 0 : org.OId,
                        Text = org == null ? null : org.OrganisationName,
                    }
                };
            }
        }
    }
}
