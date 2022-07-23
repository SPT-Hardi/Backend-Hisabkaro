using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Resume
{
    public class CreateResume
    {
        public PersonalInfo PersonalInfo { get; set; } = new PersonalInfo();
        public List<WorkExperiences> WorkExperiences { get; set; } = new List<WorkExperiences>();
        public List<Skills> Skills { get; set; } = new List<Skills>();
        public IntegerString HighestEducation { get; set; } = new IntegerString();
        public List<Certificates> Certificates { get; set; } = new List<Certificates>();
    }
    public class PersonalInfo 
    {
        public int ProfileId { get; set; }
        [Required(ErrorMessage ="Name is required!")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int? AddressId { get; set; }=null;
        public Models.Common.Contact.Address Address { get; set; } = new Common.Contact.Address();
        public string CurrentSalary { get; set; }
        public IntegerNullString SalaryType { get; set; } = new IntegerNullString();
        public bool IsVisibleToBusinessOwner { get; set; }
        public IntegerNullString EnglishLevel { get; set; } = new IntegerNullString();
        public List<OtherLanguages> OtherLanguages { get; set; } = new List<OtherLanguages>();

    }
    public class WorkExperiences
    {
        [JsonIgnore]
        public int WorkExperienceId { get; set; }
        public string JobTitle { get; set; }
        public string CopanyName { get; set; }
        public IntegerNullString Sector { get; set; } = new IntegerNullString();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class List_WorkExperienxes 
    {
        public List<WorkExperiences> WorkExperienceList { get; set; } = new List<WorkExperiences>();
    }
    public class Skills 
    {  
        public string skill { get; set; }
    }
    public class List_Skills 
    {
        public List<Skills> SkillList { get; set; } = new List<Skills>();
    }
    public class Certificates 
    {
        public int CertificateId { get; set; }
        public string CertificateName { get; set; }
        public string FileGUId { get; set; }
    }
    public class Educations 
    {
        public IntegerString HighestEducation { get; set; } = new IntegerString();
    }
    public class List_Certificates 
    {
        public List<Certificates> CertificateList { get; set; } = new List<Certificates>();
    }
    public class OtherLanguages 
    {
        public string language { get; set; }
    }
    public class List_OtherLanguages
    {
        public List<OtherLanguages> OtherLanguageList { get; set; } = new List<OtherLanguages>();
    }
}
