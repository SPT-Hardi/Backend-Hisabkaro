using HisaabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HisaabKaro.Cores.Employee.Resume
{
    public class Educations
    {
        public Result Add(Models.Employee.Resume.Education value,string UID) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var check = context.SubUsers.Where(x => x.UId.ToString() == UID).SingleOrDefault();
                    if (check == null) 
                    {
                        throw new ArgumentException("User Doesnt Exist!,(enter valid token)");
                    }
                    var education = (from obj in value.educationlist
                                     select new EmpResumeEducation()
                                     {
                                         EducationNameId=obj.EducationName.ID,
                                         EducationSteamId=obj.EducationStream.ID,
                                         InstituteName=obj.InstituteName,
                                         UId=int.Parse(UID),
                                         StartDate=obj.StartDate,
                                         EndDate=obj.EndDate,
                                     }).ToList();
                    context.EmpResumeEducations.InsertAllOnSubmit(education);
                    context.SubmitChanges();
                    var res = (from obj in education
                               select new Models.Employee.Resume.EducationDetail()
                               {
                                   EmpResumeEducationId=obj.EmpResumeEducationId,
                                   EducationName=new IntegerNullString() { ID=(int)obj.EducationNameId,Text=obj.SubFixedLookup.FixedLookup},
                                   EducationStream=new IntegerNullString() { ID=(int)obj.EducationSteamId,Text=obj.SubLookup.Lookup},
                                   InstituteName=obj.InstituteName,
                                   StartDate=Convert.ToDateTime(obj.StartDate),
                                   EndDate=Convert.ToDateTime(obj.EndDate),
                                   
                               }).ToList();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-Educations Add successfully!",
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
                    var qs = context.EmpResumeEducations.Where(x => x.UId.ToString() == UID).ToList();
                   if (qs == null)
                    {
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "Employee Resume-Educations Data get successfully!",
                            Data = qs,
                        };
                    }
                    else
                    {
                        var res = (from obj in qs
                        select new Models.Employee.Resume.EducationDetail()
                        {
                            EmpResumeEducationId = obj.EmpResumeEducationId,
                            EducationName = new IntegerNullString() { ID = (int)obj.EducationNameId, Text = obj.SubFixedLookup.FixedLookup },
                            EducationStream = new IntegerNullString() { ID = (int)obj.EducationSteamId, Text = obj.SubLookup.Lookup },
                            InstituteName = obj.InstituteName,
                            StartDate = Convert.ToDateTime(obj.StartDate),
                            EndDate = Convert.ToDateTime(obj.EndDate),

                        }).ToList();
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "Employee Resume-Educations Data get successfully!",
                            Data = res,
                        };
                    }
                    
                }
            }
        }
        public Result Update(Models.Employee.Resume.EducationDetail value,int Id,string UID) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var education = context.EmpResumeEducations.Where(x => x.UId.ToString() == UID && x.EmpResumeEducationId == Id).SingleOrDefault();
                    if (education == null) 
                    {
                        throw new ArgumentException("Enter valid EducationId");
                    }
                    education.EducationNameId = value.EducationName.ID;
                    education.EducationSteamId = value.EducationStream.ID;
                    education.InstituteName = value.InstituteName;
                    education.EndDate = value.EndDate;
                    education.StartDate = value.StartDate;
                    context.SubmitChanges();
                    
                    var res =  new Models.Employee.Resume.EducationDetail()
                               {
                                   EmpResumeEducationId = value.EmpResumeEducationId,
                                   EducationName = new IntegerNullString() { ID = (int)education.EducationNameId, Text = education.SubFixedLookup.FixedLookup },
                                   EducationStream = new IntegerNullString() { ID = (int)education.EducationSteamId, Text = education.SubLookup.Lookup },
                                   InstituteName = education.InstituteName,
                                   StartDate = Convert.ToDateTime(education.StartDate),
                                   EndDate = Convert.ToDateTime(education.EndDate),
                                };

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-Educations Data Update successfully!",
                        Data = res,
                    };
                }
            }
        }
    }
}
