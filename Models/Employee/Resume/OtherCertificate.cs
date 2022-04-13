using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Employee.Resume
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
        public string CertificateName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class Certificate 
    {
        public int EmpResumeOtherCertificateId { get; set; }
        public int CertificateFileId { get;set; }
    }
}
