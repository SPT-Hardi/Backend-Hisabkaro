using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Developer.Subscriber
{
    public class UserEducation
    {
        public List<Education> educations { get; set; } = new List<Education>();
        public List<OtherCertificate> otherCertificates { get; set; } = new List<OtherCertificate>();
        public List<Skill> skills { get; set; } = new List<Skill>();
    }
    public class Education 
    {
       
        public int UserEducationId { get; set; }
        public int EducationNameId { get; set; }
        public int EducationCertificateFileId { get; set; }
    }
    public class OtherCertificate 
    {
        
        public int UserOtherCertificateId { get; set; }
        public string OtherCertificateName { get; set; }
        public DateTime Duration { get; set; }
    }
    public class Skill 
    {
        
        public int UserSkillId { get; set; }
        public string SkillName { get; set; }
        public int SkillCertificateFileId { get; set; }
    }
}
