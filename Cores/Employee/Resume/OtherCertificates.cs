using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Employee.Resume
{
    public class OtherCertificates
    {
       

        public Result Add(string UID,Models.Employee.Resume.OtherCertificate value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    
                    var user = context.SubUsers.Where(x => x.UId.ToString() == UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User doesnt exist,(enter valid token)");
                    }
                    var othercertificate = (from obj in value.OtherCertificateDetails
                                            select new EmpResumeOtherCertificate()
                                            {
                                                UId=int.Parse(UID),
                                                CertificateName=obj.CertificateName,
                                                StartDate=obj.StartDate,
                                                EndDate=obj.EndDate,
                                                
                                            }).ToList();
                    context.EmpResumeOtherCertificates.InsertAllOnSubmit(othercertificate);
                    context.SubmitChanges();
                    var res = (from obj in othercertificate
                               select new 
                               {
                                   EmpResumeOtherCertificateId=obj.EmpResumeOtherCertificateId,
                                   CertificateName=obj.CertificateName,
                                   
                               }).ToList();
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-OtherCertificates added successfully!",
                        Data = res,
                    };
                }
            }
        }
       
        public Result UploadCertificate(int Id,string UID,Models.Employee.Resume.Certificate value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var certificate = context.EmpResumeOtherCertificates.Where(x => x.UId.ToString() == UID && x.EmpResumeOtherCertificateId == Id).SingleOrDefault();
                    if (certificate == null) 
                    {
                        throw new ArgumentException($"There are no details for OtherCertificateId:{Id}");
                    }

                    var file = context.CommonFiles.Where(x => x.FGUID == value.CertificateFGUID).SingleOrDefault();
                    if (file == null) 
                    {
                        throw new ArgumentException("File not exist!");
                    }
                    certificate.CertificateFileId = file.FileId;
                    context.SubmitChanges();
                   
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-OtherCertificates File added successfully!",
                        Data = new 
                        {
                            CertificateFileId=certificate.CertificateFileId,
                            CertificateName=certificate.CertificateName,
                        },
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
                    
                    var res = (from obj in context.EmpResumeOtherCertificates
                               where obj.UId.ToString()==UID
                               select new 
                               {
                                   EmpResumeOtherCertificateId = obj.EmpResumeOtherCertificateId,
                                   CertificateName = obj.CertificateName,
                                   StartDate = Convert.ToDateTime(obj.StartDate),
                                   EndDate = Convert.ToDateTime(obj.EndDate),
                                   CertificateFilePath =obj.CertificateFileId==null ? null : obj.CommonFile.FilePath ,

                               }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-OtherCertificates added successfully!",
                        Data = res,
                    };
                }
            }
        }
        public Result Update(int Id, string UID,Models.Employee.Resume.OtherCertificateDetails value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var othercertificates = context.EmpResumeOtherCertificates.Where(x => x.UId.ToString() == UID && x.EmpResumeOtherCertificateId == Id).SingleOrDefault();
                    if (othercertificates == null) 
                    {
                        throw new ArgumentException("No othercertificate details for this Id,(enter valid token)");
                    }

                    othercertificates.CertificateName = value.CertificateName;
                    othercertificates.EndDate =value.EndDate;
                    othercertificates.StartDate =value.StartDate;

                    context.SubmitChanges();
                    var res = new 
                    {
                        EmpResumeOtherCertificateId = othercertificates.EmpResumeOtherCertificateId,
                        CertificateName = othercertificates.CertificateName,
                        


                    };
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-OtherCertificates added successfully!",
                        Data = res,
                    };
                }
            }
        }
    }
}
