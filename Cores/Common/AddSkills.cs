using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Common
{
    public class AddSkills
    {
        public Result Add(int Id,Skills value) 
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var skills = (from x in value.SkillName
                                  select new SubLookup()
                                  {
                                      LookupTypeId=Id,
                                      Lookup=x
                                  }).ToList();

                    c.SubLookups.InsertAllOnSubmit(skills);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Skills added successfully!",
                    };
                }
            }
        }
    }
}
