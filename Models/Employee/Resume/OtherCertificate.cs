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
        [Required]
        public string CertificateName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
    public class Certificate
    {
       
        public int EmpResumeOtherCertificateId { get; set; }
     
        public string CertificateFGUID { get;set; }
    }
    
}
