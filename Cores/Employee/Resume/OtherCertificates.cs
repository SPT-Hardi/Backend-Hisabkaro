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
        public Result Add(object UID,Models.Employee.Resume.List_Certificates value) 
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
                        throw new ArgumentException("User doesnt exist,(enter valid token)");
                    }
                    var profile=user.EmpResumeProfiles.ToList().FirstOrDefault();
                    if (profile == null) 
                    {
                        throw new ArgumentException("user resume not created yet!");
                    }
                    var othercertificates = (from obj in value.CertificateList
                                            select new EmpResumeOtherCertificate()
                                            {
                                                UId = (int)UID,
                                                CertificateName = obj.CertificateName,
                                                ProfileId=profile.ProfileId,
                                                CertificateFileId=(from x in c.CommonFiles where x.FGUID==obj.FileGUId select x).FirstOrDefault()?.FileId,
                                            }).ToList();
                    c.EmpResumeOtherCertificates.InsertAllOnSubmit(othercertificates);
                    c.SubmitChanges();

                    var res = (from obj in othercertificates
                               select new 
                               {
                                   CertificateId=obj.EmpResumeOtherCertificateId,
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
       
        public Result UploadCertificate(object UID,Models.Employee.Resume.Certificate value) 
        {
            using (TransactionScope scope = new TransactionScope())
            {
                using (DBContext c = new DBContext())
                {
                    var certificate = c.EmpResumeOtherCertificates.Where(x => x.UId == (int)UID && x.EmpResumeOtherCertificateId == value.CertificateId).SingleOrDefault();
                    if (certificate == null) 
                    {
                        throw new ArgumentException($"There are no details for OtherCertificateId:{value.CertificateId}");
                    }

                    var file = c.CommonFiles.Where(x => x.FGUID == value.FileGUID).SingleOrDefault();
                    if (file == null) 
                    {
                        throw new ArgumentException("File not exist!");
                    }
                    certificate.CertificateFileId = file.FileId;
                    c.SubmitChanges();

                    var res = new
                    {
                        CertificateId = certificate.EmpResumeOtherCertificateId,
                        CertificateName = certificate.CertificateName,
                    };
                    scope.Complete();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-OtherCertificates File added successfully!",
                        Data = res
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
                    var user = (from x in c.SubUsers where x.UId == (int)UID select x).FirstOrDefault();
                    if (user == null) 
                    {
                        throw new ArgumentException("User not exist!");
                    }
                    var profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    var res = (from obj in c.EmpResumeOtherCertificates
                               where obj.UId==(int)UID && obj.ProfileId==profile.ProfileId
                               select new Models.Employee.Resume.Certificates()
                               {
                                   CertificateName = obj.CertificateName,
                                   CertificateId=obj.EmpResumeOtherCertificateId,
                                   FileGUId=obj.CommonFile.FGUID,
                               }).ToList();
                    return new Result()
                    {
                        Status = Result.ResultStatus.success,
                        Message = "Employee Resume-OtherCertificates details get successfully!",
                        Data = res,
                    };
                }
            }
        }
        public Result Update(int Id, object UID,Models.Employee.Resume.Certificates value)
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
                        throw new ArgumentException("user not exist!");
                    }
                    var profile = user.EmpResumeProfiles.ToList().FirstOrDefault();
                    var othercertificates = c.EmpResumeOtherCertificates.Where(x => x.UId == (int)UID && x.EmpResumeOtherCertificateId == Id ).SingleOrDefault();
                    if (othercertificates == null) 
                    {
                        throw new ArgumentException("No othercertificate details for this Id,(enter valid token)");
                    }

                    othercertificates.CertificateName = value.CertificateName;
                    othercertificates.CertificateFileId =(from x in c.CommonFiles where x.FGUID==value.FileGUId select x).FirstOrDefault()?.FileId;

                    c.SubmitChanges();
                    var res = new 
                    {
                        CertificateId = othercertificates.EmpResumeOtherCertificateId,
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
