using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Filters.Custom
{
    public class HaveAuthority
    {
        public void AccessStaff(object Ids) 
        {
            using (DBContext c = new DBContext()) 
            {
                var ids = (Models.Common.Ids)Ids;
                var admin = (from x in c.SubUserOrganisations where x.URId == ids.URId && x.SubRole.RoleName.ToLower() == "admin" select x).FirstOrDefault();
                if (admin is null)
                {

                    var authorized = (from x in c.SubURIdControllers where x.URId == ids.URId && x.CId == ids.CId select x).FirstOrDefault();
                    if (authorized is null)
                    {
                        throw new ArgumentException("Not authorized!");
                    }
                }
            }
            
        }
    }
}
