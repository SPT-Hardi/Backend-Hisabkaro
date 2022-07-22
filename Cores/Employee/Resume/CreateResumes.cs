using HIsabKaro.Models.Common;
using HisabKaroContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class CreateResumes
    {
        public Result Add(object UId,Models.Employee.Resume.CreateResume value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    if (UId == null) 
                    {
                        throw new ArgumentException("token not found or expired!");
                    }
                    var user = (from x in c.SubUsers where x.UId == (int)UId select x).FirstOrDefault();
                    //can be deleted if multi resume feature added in future
                    if (user.EmpResumeProfiles.ToList().Count() > 0) 
                    {
                        throw new ArgumentException("Profile alredy exist!");
                    }
                   
                    //create new user profile
                    var userProfile = new EmpResumeProfile()
                    {
                        AMobileNumber=user.SubUsersDetail.AMobileNumber,
                        CurrentSalary=value.PersonalInfo.CurrentSalary,
                        Email=value.PersonalInfo.Email,
                        EnglishLevelId=value.PersonalInfo.EnglishLevel.Id,
                        FullName=value.PersonalInfo.Name,
                        IsVisibleToBussinessOwner=value.PersonalInfo.IsVisibleToBusinessOwner,
                        MobileNumber=value.PersonalInfo.MobileNumber,
                        UId=user.UId,
                        SalaryTypeId=value.PersonalInfo.SalaryType.Id,
                    };
                    //create address for profile
                    if (value.PersonalInfo.AddressId == null)
                    {
                        var address = new CommonContactAddress()
                        {
                            AddressLine1 = value.PersonalInfo.Address.AddressLine1,
                            AddressLine2 = "",
                            City = value.PersonalInfo.Address.City,
                            Landmark = value.PersonalInfo.Address.LandMark,
                            PinCode = value.PersonalInfo.Address.PinCode,
                            State = value.PersonalInfo.Address.State,
                        };
                        c.CommonContactAddresses.InsertOnSubmit(address);
                        c.SubmitChanges();
                        userProfile.AddressId = address.ContactAddressId;

                    }
                    else 
                    {
                        userProfile.AddressId = value.PersonalInfo.AddressId;
                    }
                    c.EmpResumeProfiles.InsertOnSubmit(userProfile);
                    c.SubmitChanges();


                    //create new user_Resume_OtherLanguages
                    c.EmpResumeOtherLanguages.InsertAllOnSubmit(value.PersonalInfo.OtherLanguages.Where(x => x.language != null).Select(x => new EmpResumeOtherLanguage()
                    {
                        OtherLanguage=x.language,
                        ProfileId=userProfile.ProfileId,
                        UId=user.UId
                    }));
                    c.SubmitChanges();

                    //create new user_Resume_WorkExperience
                    c.EmpResumeWorkExperiences.InsertAllOnSubmit(value.WorkExperiences.Where(x => x.JobTitle != null).Select(x => new EmpResumeWorkExperience()
                    {
                        CompanyName=x.CopanyName,
                        EndDate=x.EndDate,
                        JobTitle=x.JobTitle,
                        SectorId=x.Sector.Id,
                        StartDate=x.StartDate,
                        UId=user.UId,
                        ProfileId=userProfile.ProfileId,
                    }));
                    c.SubmitChanges();

                    //create new user_Resume_Skills
                    c.EmpResumeSkills.InsertAllOnSubmit(value.Skills.Where(x => x.skill != null).Select(x => new EmpResumeSkill()
                    {
                        SkillName=x.skill,
                        UId=user.UId,
                        ProfileId=userProfile.ProfileId,
                    }));
                    c.SubmitChanges();

                    //create new user_Resume_Education
                    if (value.HighestEducation.Id != 0 || value.HighestEducation.Text != null)
                    {

                        c.EmpResumeEducations.InsertOnSubmit(new EmpResumeEducation()
                        {
                            UId =user.UId,
                            EducationNameId =value.HighestEducation.Id,
                            ProfileId =userProfile.ProfileId,
                        });
                        c.SubmitChanges();
                    }

                    //create new user_Resume_Certificates
                    c.EmpResumeOtherCertificates.InsertAllOnSubmit(value.Certificates.Where(x => x.CertificateName != null).Select(x => new EmpResumeOtherCertificate()
                    {
                        CertificateFileId=(from y in c.CommonFiles where y.FGUID==x.FileGUId select y.FileId).FirstOrDefault(),
                        CertificateName=x.CertificateName,
                        ProfileId=userProfile.ProfileId,
                        UId=user.UId,
                    }));
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "User resume details added successfully!",
                        Data = ""
                    };
                }
            }
        }
    }
}
