using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Resume
{
    public class DashboardView
    {
        public int UId { get; set; }
        public About about { get; set; } = new About();
        public List<ViewExperience> experiences { get; set; } = new List<ViewExperience>();
        public List<EduDetail> educations { get; set; } = new List<EduDetail>();
        public List<SkillDetails> skills { get; set; } = new List<SkillDetails>();
        public ViewContact contact { get; set; } = new ViewContact();
        public List<ViewOtherCertificate> otherCertificates { get; set; } = new List<ViewOtherCertificate>();
    }

    public class ViewExperience
    {
        public int EmpResumeWorkExperienceId { get; set; }
        public string JobTitle { get; set; }
        public string OrganizationName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TotalDuration { get; set; }
        public string WorkFrom { get; set; }
    }

    public class ViewOtherCertificate
    {
        public string CertificateName { get; set; }
        public int EmpResumeOtherCertificateId { get; set; }
        public string Duration { get; set; }
    }

    public class ViewContact
    {
        public int UId { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }

    public class EduDetail
    {
        public int EmpResumeEducationId { get; set; }
        public IntegerNullString EducationName { get; set; }
        public string EducationStreamName { get; set; }
        public string InstituteName { get; set; }
        public string TotalDuration { get; set; }
    }

    public class OverTime
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? Hours { get; set; }
    }
}
