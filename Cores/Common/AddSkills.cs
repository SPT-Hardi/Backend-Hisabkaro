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
        public Result SearchSkillList(string keyword) 
        {
            using (DBContext c = new DBContext())
            {

                var res = (from x in c.SubLookups where x.LookupTypeId == 58 && x.Lookup.Contains(keyword)
                           select new IntegerNullString()
                           {
                               Id=x.LookupId,
                               Text=x.Lookup
                           }).ToList();
            return new Result()
            {
                Status = Result.ResultStatus.success,
                Message = "Skill list get successfully!",
                Data =res
            };
            }
        }
    }
}
