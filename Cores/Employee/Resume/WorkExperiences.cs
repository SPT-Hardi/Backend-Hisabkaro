using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class WorkExperiences
    {
        public Result Add(object UID,Models.Employee.Resume.List_WorkExperienxes value) 
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
                        throw new ArgumentException("User doesnt exist!,(enter valid token)");
                    }
                    var Profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    if (Profile == null) 
                    {
                        throw new ArgumentException("user resume yet not created!");
                    }
                    var workexperiences = (from x in value.WorkExperienceList
                                          select new EmpResumeWorkExperience()
                                          {
                                              CompanyName=x.CopanyName,
                                              EndDate=x.EndDate,
                                              JobTitle=x.JobTitle,
                                              ProfileId=Profile.ProfileId,
                                              SectorId=x.Sector.Id,
                                              StartDate=x.StartDate,
                                              UId=user.UId,
                                          }).ToList();
                    c.EmpResumeWorkExperiences.InsertAllOnSubmit(workexperiences);
                    c.SubmitChanges();

                    var res = (from obj in workexperiences
                              select new Models.Employee.Resume.WorkExperiences()
                              {
                                  WorkExperienceId=obj.EmpResumeWorkExperienceId,
                                  JobTitle=obj.JobTitle,
                                  StartDate=Convert.ToDateTime(obj.StartDate),
                                  EndDate=Convert.ToDateTime(obj.EndDate),
                                  CopanyName=obj.CompanyName,
                                  Sector=new IntegerNullString() { Id=obj.SubFixedLookup.FixedLookupId,Text=obj.SubFixedLookup.FixedLookup,},
                              }).ToList();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-WorkExperiences added successfully!",
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
                    if (UID == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var user = c.SubUsers.Where(x => x.UId == (int)UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User Doesnt Exist!,(Enter valid token)");
                    }
                    var profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    var workExperience = c.EmpResumeWorkExperiences.Where(x => x.UId== (int)UID && x.ProfileId==profile.ProfileId).ToList();
                    var res= (from obj in workExperience
                              select new Models.Employee.Resume.WorkExperiences()
                              {
                                  WorkExperienceId = obj.EmpResumeWorkExperienceId,
                                  JobTitle = obj.JobTitle,
                                  CopanyName = obj.CompanyName,
                                  StartDate = Convert.ToDateTime(obj.StartDate),
                                  EndDate = Convert.ToDateTime(obj.EndDate),
                                  Sector=new IntegerNullString() { Id=obj.SubFixedLookup.FixedLookupId,Text=obj.SubFixedLookup.FixedLookup,},
                              }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-WorkExperiences added successfully!",
                        Data = res,
                    };
                }
            }
        }
        public Result Update(int Id,object UID,Models.Employee.Resume.WorkExperiences value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    if (UID == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var user = (from x in c.SubUsers where x.UId == (int)UID select x).FirstOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User not found!");
                    }
                    var profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    var workExperience = c.EmpResumeWorkExperiences.Where(x => x.UId == (int)UID && x.EmpResumeWorkExperienceId == Id && x.ProfileId==profile.ProfileId).SingleOrDefault();
                    if (workExperience == null) 
                    {
                        throw new ArgumentException($"No record found for given Id:{Id}");
                    }
                    workExperience.JobTitle =value.JobTitle;
                    workExperience.EndDate =value.EndDate;
                    workExperience.CompanyName =value.CopanyName;
                    workExperience.StartDate =value.StartDate;
                    workExperience.SectorId = value.Sector.Id;
                    c.SubmitChanges();

                    var res = new Models.Employee.Resume.WorkExperiences()
                    {
                        WorkExperienceId = workExperience.EmpResumeWorkExperienceId,
                        JobTitle = workExperience.JobTitle,
                        CopanyName = workExperience.CompanyName,
                        StartDate = Convert.ToDateTime(workExperience.StartDate),
                        EndDate = Convert.ToDateTime(workExperience.EndDate),
                        Sector=new IntegerNullString() { Id=workExperience.SubFixedLookup.FixedLookupId,Text=workExperience.SubFixedLookup.FixedLookup,},
                    };
                    
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-WorkExperiences added successfully!",
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
                    var workexperience = c.EmpResumeWorkExperiences.Where(x => x.UId == (int)UId && x.EmpResumeWorkExperienceId == Id).SingleOrDefault();
                    if (workexperience == null) 
                    {
                        throw new ArgumentException("Employee workexperience not exist for current Id!,(enter valid token)");
                    }
                    c.EmpResumeWorkExperiences.DeleteOnSubmit(workexperience);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"User's {workexperience.JobTitle} workexperience deleted successfully!",
                        Data = new
                        {
                            EmpResumeWorkExperienceId =workexperience.EmpResumeWorkExperienceId,
                            JobTitle =workexperience.JobTitle,
                        }
                    };
                }
            }
        }
   /*     public TimeSpan TotalExperience(int UId) 
        {
            using (DBContext c = new DBContext())
            {
                var user = (from x in c.SubUsers where x.UId == UId select x).FirstOrDefault();
                if (user == null) 
                {
                    throw new ArgumentException("User not found!");
                }
                List<DateTime> dates = new List<DateTime>();
                foreach (var x in user.EmpResumeWorkExperiences.ToList()) 
                {
                    dates.Add(x.StartDate);
                    dates.Add(x.EndDate);
                }
                var MaxDate = dates.Max();
                var MinDate = dates.Min();
                var Duration = MaxDate - MinDate;
                return Duration;
            }
        }*/
    }
}
