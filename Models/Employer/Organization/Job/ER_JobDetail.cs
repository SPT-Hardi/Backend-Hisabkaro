using HIsabKaro.Models.Common;
using HIsabKaro.Models.Common.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization.Job
{
    public class ER_JobDetail
    {
        [Validation.Pair_RequiredIntegerNullString(ErrorMessage ="Organization Id is required!,Value_Allowed : any valid Integer,Value_NotAllowed : 0 or null")]
        public Models.Common.IntegerNullString Organisation { get; set; } = new Models.Common.IntegerNullString();
        [Validation.Pair_NullIntegerNullString(ErrorMessage = "Branch Id is required!,Value_Allowed: 0 or valid Integer,Value_NotAllowed:null")]
        public Models.Common.IntegerNullString Branch { get; set; } = new Models.Common.IntegerNullString();

        [Required(ErrorMessage = "Title is required")]
        [RegularExpression("^.{1,50}$", ErrorMessage = "Value_Allowed: Any, Max_Length:50 character")]
        public string Title { get; set; }
        [RegularExpression("^[0-9]{1,10}$",ErrorMessage ="Value_Allowed:Only digits are allowed!, Max_Length:10 character")]
        public string MinSalary { get; set; }
        [RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Value_Allowed:Only digits are allowed!, Max_Length:10 character")]
        public string MaxSalary { get; set; }
        [Validation.Pair_RequiredIntegerNullString(ErrorMessage = "SalaryType Id is required!,Value_Allowed : any valid Integer,Value_NotAllowed : 0 or null")]
        public IntegerNullString SalaryType { get; set; } = new IntegerNullString();
        [RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Value_Allowed:Only digits are allowed!, Max_Length:10 character")]
        public string MinIncentive { get; set; }
        [RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Value_Allowed:Only digits are allowed!, Max_Length:10 character")]
        public string MaxIncentive { get; set; }
        [Validation.Pair_RequiredIntegerNullString(ErrorMessage = "IncentiveType Id is required!,Value_Allowed : any valid Integer,Value_NotAllowed : 0 or null")]
        public IntegerNullString IncentiveType { get; set; } = new IntegerNullString();
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "Time must be between 00:00 to 23:59")]
        public TimeSpan? JobStartTime { get; set; }
        [RegularExpression(@"((([0-1][0-9])|(2[0-3]))(:[0-5][0-9])(:[0-5][0-9])?)", ErrorMessage = "Time must be between 00:00 to 23:59")]
        public TimeSpan? JobEndDate { get; set; }
        [RegularExpression("^.{1,300}$", ErrorMessage = "Value_Allowed: Any, Max_Length:300 character")]
        public string Description { get; set; }
        [RegularExpression("^.{1,150}$", ErrorMessage = "Value_Allowed: Any, Max_Length:150 character")]
        public string Comment { get; set; }
        [RegularExpression("^[0-9]{1,10}$", ErrorMessage = "Value_Allowed:Only digits are allowed!, Max_Length:10 character")]
        public string MobileNumber { get; set; }
        [EmailAddress(ErrorMessage ="Enter valid email address!,Value_Allowed: Any, Max_Length:100 characters")]
        public string Email { get; set; }
        public int AddressId { get; set; }
        public List<JobType> jobType { get; set; } = new List<JobType>();
        public List<JobSkill> jobSkill { get; set; } = new List<JobSkill>();

        [Required(ErrorMessage = "Last Apply date required")]
        public DateTime EndDate { get; set; }
        public List<ExperienceLevel> ExperienceLevels { get; set; } = new List<ExperienceLevel>();
        public List<EnglishLevel> EnglishLevels { get; set; } = new List<EnglishLevel>();
        public List<OtherLanguage> OtherLanguages { get; set; } = new List<OtherLanguage>();
    }
    public class Applied_Bookmarked_ShortListed_List
    {
        public int Total { get; set; }
        public List<Applicants> Applicants { get; set; } = new List<Applicants>();
    }
    public class Applicants
    {
        public int Id { get; set; }
        public int UId { get; set; }
        public string Name { get; set; }
        public string ImageFGUID{get;set;}
        public string MobileNumber { get; set; }
        public DateTime Date { get; set; }
        //public dynamic WorkExperience { get; set; }
        public bool IsApplied { get; set; }
        public bool IsBookmarked { get; set; }
        public bool IsShortListed { get; set; }
        public List<JobSkill> skills { get; set; } = new List<JobSkill>();
    }
    public class JobSkill
    {
        [RegularExpression("^.{1,50}$", ErrorMessage = "Value_Allowed: Any, Max_Length:50 character")]
        public string skill { get; set; } 
    }
    public class OtherLanguage 
    {
        [RegularExpression("^.{1,50}$", ErrorMessage = "Value_Allowed: Any, Max_Length:50 character")]
        public string language { get; set; }
    }
    public class EnglishLevel
    {
        [RegularExpression("^.{1,50}$", ErrorMessage = "Value_Allowed: Any, Max_Length:50 character")]
        public string level { get; set; }
    }
    public class ExperienceLevel 
    {
        [RegularExpression("^.{1,50}$", ErrorMessage = "Value_Allowed: Any, Max_Length:50 character")]
        public string level { get; set; }
    }
    public class JobType
    {
        public bool status { get; set; }
        [RegularExpression("^.{1,50}$", ErrorMessage = "Value_Allowed: Any, Max_Length:50 character")]
        public string type { get; set; } 
    }
    public class Org_and_Branches 
    {
        public string Name { get; set; }
        public IntegerNullString Organization { get; set; } = new IntegerNullString();
        public IntegerNullString Branch { get; set; } = new IntegerNullString();
    }
}
