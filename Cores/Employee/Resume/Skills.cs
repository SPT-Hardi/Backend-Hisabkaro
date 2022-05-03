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
        public Result Add(object UID,Models.Employee.Resume.Skill value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = c.SubUsers.Where(x => x.UId == (int)UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User doesnt Exist!");
                    }
                    var skills = (from obj in value.skillDetails
                                  select new EmpResumeSkill
                                  {
                                      UId = (int)UID,
                                      SkillName=obj.SkillName,
                                  }).ToList();
                    c.EmpResumeSkills.InsertAllOnSubmit(skills);
                    c.SubmitChanges();
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
        public Result View(object UID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var skills = c.EmpResumeSkills.Where(x => x.UId == (int)UID).ToList();
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
        public Result Update(int Id,object UID,Models.Employee.Resume.SkillDetails value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var skills = c.EmpResumeSkills.Where(x => x.UId == (int)UID && x.EmpResumeSkillId == Id).SingleOrDefault();
                    if (skills == null) 
                    {
                        throw new ArgumentException("No Skills details for this id,(enter valid token)");
                    }
                    skills.SkillName = value.SkillName;
                    c.SubmitChanges();
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
        public Result Delete(object UId,int Id) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var skill = c.EmpResumeSkills.Where(x => x.UId == (int)UId && x.EmpResumeSkillId == Id).SingleOrDefault();
                    if (skill == null) 
                    {
                        throw new ArgumentException("There is no any skill details for current Id!");
                    }
                    c.EmpResumeSkills.DeleteOnSubmit(skill);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"Employee's {skill.SkillName} skill details deleted successfully!",
                        Data = new
                        {
                            EmpResumeSkillId =skill.EmpResumeSkillId,
                            Skill =skill.SkillName,
                        }
                    };
                }
            }
        }
    }
}
