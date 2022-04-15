using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class Abouts
    {
        public Result Add( string UID,Models.Employee.Resume.About value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                   var qs = context.EmpResumeAbouts.Where(x => x.UId.ToString()==UID).SingleOrDefault();
                    if (qs == null)
                    {
                        EmpResumeAbout about = new EmpResumeAbout();
                        about.UId = int.Parse(UID);
                        about.About = value.AboutText;
                        context.EmpResumeAbouts.InsertOnSubmit(about);
                        context.SubmitChanges();
                        value.EmpResumeAboutId = about.EmpResumeAboutId;
                    }
                    else 
                    {
                        qs.About = value.AboutText;
                        context.SubmitChanges();
                        value.EmpResumeAboutId = qs.EmpResumeAboutId;
                    }
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "User Resume-About Details added successfully!",
                        Data = value,
                    };
                }
            }
        }
        public Result View(string UID) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var about = context.EmpResumeAbouts.Where(x => x.UId.ToString() == UID).SingleOrDefault();

                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "User Resume-About Details get successfully!",
                        Data = new Models.Employee.Resume.About()
                        {
                            AboutText = about == null ? null : about.About,
                            EmpResumeAboutId = about == null ? 0: about.EmpResumeAboutId
                            
                        }
                    };
                }
            }
        }
    }
}
