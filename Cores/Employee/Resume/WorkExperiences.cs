using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class WorkExperiences
    {
        public Result Add(int UID,Models.Employee.Resume.WorkExperinece value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = c.SubUsers.Where(x => x.UId == UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User doesnt exist!,(enter valid token)");
                    }
                    var workexperience = (from obj in value.workExperienceDetails
                                          select new EmpResumeWorkExperience()
                                          {
                                              UId=UID,
                                              StartDate=obj.StartDate,
                                              EndDate=obj.EndDate,
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
        public Result View(int UID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var user = c.SubUsers.Where(x => x.UId == UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User Doesnt Exist!,(Enter valid token)");
                    }
                    var workExperience = c.EmpResumeWorkExperiences.Where(x => x.UId== UID).ToList();
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
        public Result Update(int Id,int UID,Models.Employee.Resume.WorkExperienceDetails value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var workExperience = c.EmpResumeWorkExperiences.Where(x => x.UId == UID && x.EmpResumeWorkExperienceId == Id).SingleOrDefault();
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
    }
}
