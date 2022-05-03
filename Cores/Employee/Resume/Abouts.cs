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
        public Result Add(object UID,Models.Employee.Resume.About value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                   var qs = c.EmpResumeAbouts.Where(x => x.UId==(int)UID).SingleOrDefault();
                    if (qs == null)
                    {
                        EmpResumeAbout about = new EmpResumeAbout();
                        about.UId = (int)UID;
                        about.About = value.AboutText;
                        c.EmpResumeAbouts.InsertOnSubmit(about);
                        c.SubmitChanges();
                        value.EmpResumeAboutId = about.EmpResumeAboutId;
                    }
                    else 
                    {
                        qs.About = value.AboutText;
                        c.SubmitChanges();
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
        public Result View(object UID) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var about = c.EmpResumeAbouts.Where(x => x.UId == (int)UID).SingleOrDefault();

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
