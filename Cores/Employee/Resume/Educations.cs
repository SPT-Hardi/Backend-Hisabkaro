using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class Educations
    {
       /* public Result Add(object UID,Models.Employee.Resume.Education value) 
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
                    var check = c.SubUsers.Where(x => x.UId== (int)UID).SingleOrDefault();
                    if (check == null) 
                    {
                        throw new ArgumentException("User Doesnt Exist!,(enter valid token)");
                    }
                    foreach (var item in value.educationlist)
                    {
                        var edu = c.EmpResumeEducations.Where(x => x.UId == (int)UID && x.EducationNameId == item.EducationName.Id).SingleOrDefault();
                        if (edu != null) 
                        {
                            throw new ArgumentException($"Users {edu.SubFixedLookup.FixedLookup} education details already exist!");
                        }
                    }
                    var education = (from obj in value.educationlist
                                     select new EmpResumeEducation()
                                     {
                                         EducationNameId=(int)obj.EducationName.Id,
                                         UId=(int)UID,
                                     }).ToList();
                    c.EmpResumeEducations.InsertAllOnSubmit(education);
                    c.SubmitChanges();
                    var res = (from obj in education
                               select new 
                               {
                                   EmpResumeEducationId=obj.EmpResumeEducationId,
                                   EducationName=new IntegerNullString() { Id=(int)obj.EducationNameId,Text=obj.SubFixedLookup.FixedLookup},
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
                    var user = (from x in c.SubUsers where x.UId == (int)UID select x).FirstOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User not exist!");
                    }
                    var Profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    var Education = (from x in c.EmpResumeEducations
                                     where x.UId == (int)UID && x.ProfileId == Profile.ProfileId
                                     select new IntegerNullString()
                                     {
                                         Id=x.SubFixedLookup.FixedLookupId,
                                         Text=x.SubFixedLookup.FixedLookup
                                     });
                    return new Result()
                    {
                        Status=Result.ResultStatus.success,
                        Message="User education details viewed successfully!",
                        Data=Education
                    };
                    
                }
            }
        }
        public Result Update(object UID,Models.Employee.Resume.Educations value) 
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
                        throw new ArgumentException("User not exist!");
                    }
                    var Profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    if (Profile.EmpResumeEducations.ToList().Any())
                    {
                        c.EmpResumeEducations.DeleteAllOnSubmit(Profile.EmpResumeEducations.ToList());
                        c.SubmitChanges();
                    }
                    var Education = new EmpResumeEducation()
                    {
                        EducationNameId=value.HighestEducation.Id,
                        ProfileId=Profile.ProfileId,
                        UId=user.UId
                    };
                    c.EmpResumeEducations.InsertOnSubmit(Education);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "User education details updated successfully!",
                        Data = value
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
                    var education = c.EmpResumeEducations.Where(x => x.UId == (int)UId && x.EmpResumeEducationId == Id).SingleOrDefault();
                    if (education == null) 
                    {
                        throw new ArgumentException("There is no any education details for current Id!,(enter valid token)");
                    }
                    c.EmpResumeEducations.DeleteOnSubmit(education);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"Employee's {education.SubFixedLookup.FixedLookup} education details deleted successfully!",
                        Data = new
                        {
                            EmpResumeEducationId =education.EmpResumeEducationId,
                            EducationName =education.SubFixedLookup.FixedLookup,

                        }
                    };
                }
            }
        }
    }
}
