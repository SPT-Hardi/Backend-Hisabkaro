using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class Skills
    {
        public Result Add(string UID,Models.Employee.Resume.Skill value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var user = context.SubUsers.Where(x => x.UId.ToString() == UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User doesnt Exist!");
                    }
                    var skills = (from obj in value.skillDetails
                                  select new EmpResumeSkill
                                  {
                                      
                                      SkillName=obj.SkillName,
                                  }).ToList();
                    var res = (from obj in skills
                               select new Models.Employee.Resume.SkillDetails()
                               {
                                   EmpResumeSkillId=obj.EmpResumeSkillId,
                                   SkillName=obj.SkillName,
                               }).ToList();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-Skills Details Added Successfully!",
                        Data = res,
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
                    var skills = context.EmpResumeSkills.Where(x => x.UId.ToString() == UID).ToList();
                    var res = (from obj in skills
                               select new Models.Employee.Resume.SkillDetails()
                               {
                                   EmpResumeSkillId = obj.EmpResumeSkillId,
                                   SkillName = obj.SkillName,
                               }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-Skills Details Added Successfully!",
                        Data = res,
                    };
                }
            }
        }
        public Result Update(int Id, string UID,Models.Employee.Resume.SkillDetails value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var skills = context.EmpResumeSkills.Where(x => x.UId.ToString() == UID && x.EmpResumeSkillId == Id).SingleOrDefault();
                    if (skills == null) 
                    {
                        throw new ArgumentException("No Skills details for this id,(enter valid token)");
                    }
                    skills.SkillName = value.SkillName;
                    context.SubmitChanges();
                    var res = new Models.Employee.Resume.SkillDetails()
                    {
                        EmpResumeSkillId = skills.EmpResumeSkillId,
                        SkillName = skills.SkillName,
                    };
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-Skills Details Added Successfully!",
                        Data = res,
                    };
                }
            }
        }
    }
}
