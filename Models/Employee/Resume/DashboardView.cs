﻿using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Resume
{
    public class DashboardView
    {
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
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TotalDuration { get; set; }
        public string WorkFrom { get; set; }
    }

    public class ViewOtherCertificate
    {
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
}