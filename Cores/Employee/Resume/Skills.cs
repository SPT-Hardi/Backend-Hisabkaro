using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class Skills
    {
       /* public Result Add(object UID,Models.Employee.Resume.List_Skills value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    if (UID == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var user = c.SubUsers.Where(x => x.UId == (int)UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User doesnt Exist!");
                    }
                    var profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    if (profile == null)
                    {
                        throw new ArgumentException("user resume not created yet!");
                    }
                    var skills = (from obj in value.SkillList
                                  select new EmpResumeSkill()
                                  {
                                      UId = (int)UID,
                                      ProfileId=profile.ProfileId,
                                      SkillName=obj.skill,
                                  }).ToList();
                    c.EmpResumeSkills.InsertAllOnSubmit(skills);
                    c.SubmitChanges();
                    var res = (from obj in skills
                               select new Models.Employee.Resume.Skills()
                               {
                                   skill=obj.SkillName,
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
        }*/
        public Result View(object UID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    if (UID == null)
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var user = c.SubUsers.Where(x => x.UId == (int)UID).SingleOrDefault();
                    if (user == null)
                    {
                        throw new ArgumentException("User doesnt Exist!");
                    }
                    var profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    if (profile == null) 
                    {
                        throw new ArgumentException("User resume yet not created!");
                    }
                    var skills = c.EmpResumeSkills.Where(x => x.UId == (int)UID && x.ProfileId==profile.ProfileId).ToList();
                    var res = (from obj in skills
                               select new Models.Employee.Resume.Skills()
                               {
                                   skill=obj.SkillName,
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
        public Result Update_Skills(object UID,Models.Employee.Resume.List_Skills value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    if (UID == null)
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var user = c.SubUsers.Where(x => x.UId == (int)UID).SingleOrDefault();
                    if (user == null)
                    {
                        throw new ArgumentException("User doesnt Exist!");
                    }
                    var profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    if (profile == null) 
                    {
                        throw new ArgumentException("user resume not created yet!");
                    }
                    if (profile.EmpResumeSkills.ToList().Any()) 
                    {
                        c.EmpResumeSkills.DeleteAllOnSubmit(profile.EmpResumeSkills.ToList());
                        c.SubmitChanges();
                    }
                    var skills = (from obj in value.SkillList
                                  select new EmpResumeSkill()
                                  {
                                      UId = (int)UID,
                                      ProfileId = profile.ProfileId,
                                      SkillName = obj.skill,
                                  }).ToList();
                    c.EmpResumeSkills.InsertAllOnSubmit(skills);
                    c.SubmitChanges();
                    var res = (from obj in skills
                               select new Models.Employee.Resume.Skills()
                               {
                                   skill = obj.SkillName,
                               }).ToList();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-Skills Details Updated Successfully!",
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
