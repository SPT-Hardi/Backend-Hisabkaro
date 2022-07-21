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
        public Result Add(object UID,Models.Employee.Resume.WorkExperinece value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = c.SubUsers.Where(x => x.UId == (int)UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User doesnt exist!,(enter valid token)");
                    }
                    var workexperience = (from obj in value.workExperienceDetails
                                          select new EmpResumeWorkExperience()
                                          {
                                              UId=(int)UID,
                                              StartDate=obj.StartDate,
                                              EndDate=(obj.EndDate < obj.StartDate)? throw new ArgumentException($"Enter valid daterange for jobtitle:{obj.JobTitle}"):obj.EndDate,
                                              JobTitle=obj.JobTitle,
                                              OrganizationName=obj.OrganizationName,
                                              WorkFrom=obj.WorkFrom,
                                          }).ToList();
                    c.EmpResumeWorkExperiences.InsertAllOnSubmit(workexperience);
                    c.SubmitChanges();
                    var res = (from obj in workexperience
                              select new Models.Employee.Resume.WorkExperienceDetails()
                              {
                                  EmpResumeWorkExperienceId=obj.EmpResumeWorkExperienceId,
                                  JobTitle=obj.JobTitle,
                                  OrganizationName=obj.OrganizationName,
                                  WorkFrom=obj.WorkFrom,
                                  StartDate=Convert.ToDateTime(obj.StartDate),
                                  EndDate=Convert.ToDateTime(obj.EndDate),
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
                    var user = c.SubUsers.Where(x => x.UId == (int)UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User Doesnt Exist!,(Enter valid token)");
                    }
                    var workExperience = c.EmpResumeWorkExperiences.Where(x => x.UId== (int)UID).ToList();
                    var res= (from obj in workExperience
                              select new Models.Employee.Resume.WorkExperienceDetails()
                              {
                                  EmpResumeWorkExperienceId = obj.EmpResumeWorkExperienceId,
                                  JobTitle = obj.JobTitle,
                                  OrganizationName = obj.OrganizationName,
                                  WorkFrom = obj.WorkFrom,
                                  StartDate = Convert.ToDateTime(obj.StartDate),
                                  EndDate = Convert.ToDateTime(obj.EndDate),
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
        public Result Update(int Id,object UID,Models.Employee.Resume.WorkExperienceDetails value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var workExperience = c.EmpResumeWorkExperiences.Where(x => x.UId == (int)UID && x.EmpResumeWorkExperienceId == Id).SingleOrDefault();
                    if (workExperience == null) 
                    {
                        throw new ArgumentException("Enter valid WorkExperienceId,(enter valid token)");
                    }
                    workExperience.JobTitle =value.JobTitle;
                    workExperience.EndDate =value.EndDate;
                    workExperience.OrganizationName =value.OrganizationName;
                    workExperience.StartDate =value.StartDate;
                    workExperience.WorkFrom =value.WorkFrom;
                    c.SubmitChanges();

                    var res = new Models.Employee.Resume.WorkExperienceDetails()
                    {
                        EmpResumeWorkExperienceId = workExperience.EmpResumeWorkExperienceId,
                        JobTitle = workExperience.JobTitle,
                        OrganizationName = workExperience.OrganizationName,
                        WorkFrom = workExperience.WorkFrom,
                        StartDate = Convert.ToDateTime(workExperience.StartDate),
                        EndDate = Convert.ToDateTime(workExperience.EndDate),
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
        public TimeSpan TotalExperience(int UId) 
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
        }
    }
}
