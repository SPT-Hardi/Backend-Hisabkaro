using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroContext;
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
        public Result Add(object UID,Models.Employee.Resume.OtherCertificate value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    
                    var user = c.SubUsers.Where(x => x.UId == (int)UID).SingleOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User doesnt exist,(enter valid token)");
                    }
                    var othercertificate = (from obj in value.OtherCertificateDetails
                                            select new EmpResumeOtherCertificate()
                                            {
                                                UId = (int)UID,
                                                CertificateName = obj.CertificateName,
                                                StartDate = (obj.EndDate < obj.StartDate) ? throw new ArgumentException($"Enter valid daterange for certificate:{obj.CertificateName}") : obj.StartDate,
                                                EndDate = obj.EndDate,
                                                CertificateFileId = obj.CertificateFGUID == null ? null : ((from x in c.CommonFiles where x.FGUID == obj.CertificateFGUID select x.FileId).FirstOrDefault() == 0 ? throw new ArgumentException("Enter valid FGUID") : (from x in c.CommonFiles where x.FGUID == obj.CertificateFGUID select x.FileId).FirstOrDefault())
                                            }).ToList();
                    c.EmpResumeOtherCertificates.InsertAllOnSubmit(othercertificate);
                    c.SubmitChanges();

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
       
        public Result UploadCertificate(int Id,object UID,Models.Employee.Resume.Certificate value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var certificate = c.EmpResumeOtherCertificates.Where(x => x.UId == (int)UID && x.EmpResumeOtherCertificateId == Id).SingleOrDefault();
                    if (certificate == null) 
                    {
                        throw new ArgumentException($"There are no details for OtherCertificateId:{Id}");
                    }

                    var file = c.CommonFiles.Where(x => x.FGUID == value.CertificateFGUID).SingleOrDefault();
                    if (file == null) 
                    {
                        throw new ArgumentException("File not exist!");
                    }
                    certificate.CertificateFileId = file.FileId;
                    c.SubmitChanges();
                   
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
        public Result View(object UID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    
                    var res = (from obj in c.EmpResumeOtherCertificates
                               where obj.UId==(int)UID
                               select new 
                               {
                                   EmpResumeOtherCertificateId = obj.EmpResumeOtherCertificateId,
                                   CertificateName = obj.CertificateName,
                                   StartDate = Convert.ToDateTime(obj.StartDate),
                                   EndDate = Convert.ToDateTime(obj.EndDate),
                                   CertificateFilePath =obj.CertificateFileId==null ? null : obj.CommonFile.FGUID ,

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
        public Result Update(int Id, object UID,Models.Employee.Resume.OtherCertificateDetails value)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var othercertificates = c.EmpResumeOtherCertificates.Where(x => x.UId == (int)UID && x.EmpResumeOtherCertificateId == Id).SingleOrDefault();
                    if (othercertificates == null) 
                    {
                        throw new ArgumentException("No othercertificate details for this Id,(enter valid token)");
                    }

                    othercertificates.CertificateName = value.CertificateName;
                    othercertificates.EndDate =value.EndDate;
                    othercertificates.StartDate =value.StartDate;
                    othercertificates.CertificateFileId = value.CertificateFGUID == null ? null : ((from x in c.CommonFiles where x.FGUID == value.CertificateFGUID select x.FileId).FirstOrDefault() == 0 ? throw new ArgumentException("Enter valid FGUID") : (from x in c.CommonFiles where x.FGUID == value.CertificateFGUID select x.FileId).FirstOrDefault());

                    c.SubmitChanges();
                    var res = new 
                    {
                        EmpResumeOtherCertificateId = othercertificates.EmpResumeOtherCertificateId,
                        CertificateName = othercertificates.CertificateName,
                        


                    };
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-OtherCertificates updated successfully!",
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
                    var othercertificate = c.EmpResumeOtherCertificates.Where(x => x.UId == (int)UId && x.EmpResumeOtherCertificateId == Id).SingleOrDefault();
                    if (othercertificate == null) 
                    {
                        throw new ArgumentException("There are no any data for current ID!,(enter valid token)");
                    }
                    c.EmpResumeOtherCertificates.DeleteOnSubmit(othercertificate);
                    c.SubmitChanges();

                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = $"Employee's {othercertificate.CertificateName} cetificate details deleted successfully!",
                        Data = new
                        {
                            EmpResumeOtherCertificateId =othercertificate.EmpResumeOtherCertificateId,
                            CertificateName =othercertificate.CertificateName,
                        }
                    };
                }
            }
        }
    }
}
