using HisaabKaro.Models.Common;
using HisaabKaro.Services;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HisaabKaro.Cores.Employee.Resume
{
    public class OtherCertificates
    {
        public Result Add(Models.Employee.Resume.OtherCertificate value,string UID) 
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
                               select new Models.Employee.Resume.OtherCertificateDetails()
                               {
                                   EmpResumeOtherCertificateId=obj.EmpResumeOtherCertificateId,
                                   CertificateName=obj.CertificateName,
                                   StartDate=Convert.ToDateTime(obj.StartDate),
                                   EndDate=Convert.ToDateTime(obj.EndDate),
                                   CertificateFileId=(int)obj.CertificateFileId,
                                   
                                   
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
        public Result UploadCertificate(Models.Common.File.Upload objFile,int Id, string UID, IWebHostEnvironment Environment) 
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
                    FileUploadServices upload = new FileUploadServices(Environment);
                    var res = upload.Upload(objFile);
                    
                    certificate.CertificateFileId = res.Data;
                    context.SubmitChanges();
                   
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-OtherCertificates File added successfully!",
                        Data = new 
                        {
                            CertificateFileId=certificate.CertificateFileId,
                            CertificateFilePath=certificate.CommonFile.FilePath,
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
                    var othercertificate = context.EmpResumeOtherCertificates.Where(x => x.UId.ToString() == UID).ToList();
                    var res = (from obj in othercertificate
                               select new Models.Employee.Resume.OtherCertificateDetails()
                               {
                                   EmpResumeOtherCertificateId = obj.EmpResumeOtherCertificateId,
                                   CertificateName = obj.CertificateName,
                                   StartDate = Convert.ToDateTime(obj.StartDate),
                                   EndDate = Convert.ToDateTime(obj.EndDate),
                                   CertificateFileId = (int)obj.CertificateFileId,

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
        public Result Update(Models.Employee.Resume.OtherCertificateDetails value,int Id,string UID)
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
                    var res = new Models.Employee.Resume.OtherCertificateDetails()
                    {
                        EmpResumeOtherCertificateId = othercertificates.EmpResumeOtherCertificateId,
                        CertificateName = othercertificates.CertificateName,
                        StartDate = Convert.ToDateTime(othercertificates.StartDate),
                        EndDate = Convert.ToDateTime(othercertificates.EndDate),
                        CertificateFileId = (int)othercertificates.CertificateFileId,


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
