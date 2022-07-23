using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Resume
{
    public class OtherCertificate
    {
        public List<OtherCertificateDetails> OtherCertificateDetails { get; set; } = new List<OtherCertificateDetails>();
    }
    public class OtherCertificateDetails 
    {
        [JsonIgnore]
        public int EmpResumeOtherCertificateId { get; set; }
        [JsonIgnore]
        public int CertificateFileId { get; set; }


        [Required(ErrorMessage ="Certificatename field is required!")]
        [RegularExpression(@"^.{1,50}$",ErrorMessage ="Max 50 characters are allowed!")]
        public string CertificateName { get; set; }


        [Required(ErrorMessage ="StartDate is required!")]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage ="EndDate is required!")]
        public DateTime EndDate { get; set; }

        public string CertificateFGUID { get; set; }
    }
    public class Certificate
    {
        [Required(ErrorMessage ="CertificateId is required!")]
        public int CertificateId { get; set; }
        public string FileGUID { get;set; }
    }
    
}
