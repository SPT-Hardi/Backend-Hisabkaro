using HIsabKaro.Models.Common;
using HisabKaroContext;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Common
{
    public class AddSkills
    {
        public Result Add(int Id, Skills value)
        {
            using (DBContext c = new DBContext())
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var skills = (from x in value.SkillName
                                  select new SubLookup()
                                  {
                                      LookupTypeId = Id,
                                      Lookup = x
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
                               Id = x.LookupId,
                               Text = x.Lookup
                           }).ToList().Take(10);
                return new Result()
                {
                    Status = Result.ResultStatus.success,
                    Message = "Skill list get successfully!",
                    Data = res
                };
            }
        }
        public List<string> AddJobPostName()
        {
            using (TransactionScope scope = new TransactionScope())
            {

                using (DBContext c = new DBContext())
                {
                    List<string> joblist = new List<string>();
                    var filePath = @"C:\Users\RajMashruwala\Desktop\Requirement info.xlsx";
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Workbook wb;
                    Worksheet ws;
                    wb = excel.Workbooks.Open(filePath);
                    ws = wb.Worksheets[1];

                    Microsoft.Office.Interop.Excel.Range cell = ws.Range["B3:B59"];
                    foreach (string job in cell.Value)
                    {
                        SubFixedLookup fixedLookup= new SubFixedLookup();
                        fixedLookup.FixedLookupType = "JobType/Role";
                        fixedLookup.FixedLookup = job.Trim();
                        c.SubFixedLookups.InsertOnSubmit(fixedLookup);
                        c.SubmitChanges();
                        joblist.Add(job);
                    }
                    scope.Complete();
                    return joblist;
                };
            }
        }
    }
}
