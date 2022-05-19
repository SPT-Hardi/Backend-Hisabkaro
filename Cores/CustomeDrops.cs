using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores
{
    public class CustomeDrops
    {
        public class RoleDrop 
        {
            public Result Get()
            {
                using (DBContext c = new DBContext())
                {
                    var fixedlookup = (from x in c.SubFixedLookups where x.FixedLookupType.ToLower() =="logintype" && x.FixedLookupId!=23 select new { x.FixedLookupId, x.FixedLookup }).ToList();
                    List<IntegerNullString> logintype = new List<IntegerNullString>();
                    foreach (var item in fixedlookup)
                    {
                        logintype.Add(new IntegerNullString()
                        {
                            Id=item.FixedLookupId,
                            Text=item.FixedLookup,
                        });
                    }
                    if (logintype == null) 
                    {
                        throw new ArgumentException("Not logintype");
                    }
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Role drop get successfully!",
                        Data =logintype,
                    };
                }
            }
        }
    }
}
