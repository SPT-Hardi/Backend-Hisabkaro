﻿using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class Educations
    {
        public Result Add(string UID,Models.Employee.Resume.Education value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    //24-10th
                    //25-12th
                    //26-UG
                    //27-PG
                    //28-ITI
                    //1-salesman
                    //2-electrician
                    var check = c.SubUsers.Where(x => x.UId.ToString() == UID).SingleOrDefault();
                    if (check == null) 
                    {
                        throw new ArgumentException("User Doesnt Exist!,(enter valid token)");
                    }
                    foreach (var item in value.educationlist)
                    {
                        var edu = c.EmpResumeEducations.Where(x => x.UId.ToString() == UID && x.EducationNameId == item.EducationName.ID).SingleOrDefault();
                        if (edu != null) 
                        {
                            throw new ArgumentException($"Users {edu.SubFixedLookup.FixedLookup} education details already exist!");
                        }
                    }
                    var education = (from obj in value.educationlist
                                     select new EmpResumeEducation()
                                     {
                                         EducationNameId=obj.EducationName.ID,
                                         //EducationSteamName=obj.EducationStreamName,
                                         InstituteName=obj.InstituteName,
                                         UId=int.Parse(UID),
                                         StartDate=obj.StartDate,
                                         EndDate=obj.EndDate,
                                     }).ToList();
                    c.EmpResumeEducations.InsertAllOnSubmit(education);
                    c.SubmitChanges();
                    var res = (from obj in education
                               select new 
                               {
                                   EmpResumeEducationId=obj.EmpResumeEducationId,
                                   EducationName=new IntegerNullString() { ID=(int)obj.EducationNameId,Text=obj.SubFixedLookup.FixedLookup},
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
                using (DBContext c = new DBContext())
                {
                    var res = (from obj in c.EmpResumeEducations
                               where obj.UId.ToString() == UID
                               select new Models.Employee.Resume.EducationDetail()
                               {
                                   EmpResumeEducationId = obj.EmpResumeEducationId,
                                   EducationName = new IntegerNullString() { ID = obj.EducationNameId, Text = obj.SubFixedLookup.FixedLookup },
                                   //EducationStreamName = obj.EducationSteamName,
                                   InstituteName = obj.InstituteName,
                                   StartDate = Convert.ToDateTime(obj.StartDate),
                                   EndDate = Convert.ToDateTime(obj.EndDate),
                               }).ToList();
                    if (!res.Any())
                    {
                        return new Result()
                        {
                            Status = Result.ResultStatus.success,
                            Message = "Employee Resume-Educations Data get successfully!",
                            Data = res,
                        };
                    }
                    else
                    {
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
        public Result Update(int Id, string UID,Models.Employee.Resume.EducationDetail value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    
                    var education = c.EmpResumeEducations.Where(x => x.UId.ToString() == UID && x.EmpResumeEducationId == Id).SingleOrDefault();
                    if (education == null) 
                    {
                        throw new ArgumentException("Enter valid EducationId");
                    }
                    if (education.EducationNameId == value.EducationName.ID)
                    {

                       // education.EducationSteamName = value.EducationStreamName;
                        education.InstituteName = value.InstituteName;
                        education.EndDate = value.EndDate;
                        education.StartDate = value.StartDate;
                        c.SubmitChanges();
                    }
                    else 
                    {
                        throw new ArgumentException($"You only update {education.SubFixedLookup.FixedLookup} details by this request.");
                    }


                    var text =education.SubFixedLookup.FixedLookup;
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-Educations Data Update successfully!",
                        Data = new 
                        {
                            EmpResumeEducationId = education.EmpResumeEducationId,
                            EducationName = new IntegerNullString() { ID = education.EducationNameId, Text = text },
                            //EducationStreamName = education.EducationSteamName,
                        }
                    };
                }
            }
        }
    }
}
