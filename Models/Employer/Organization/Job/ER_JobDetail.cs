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
        public Models.Common.IntegerNullString Organisation { get; set; } = new Models.Common.IntegerNullString();

        public Models.Common.IntegerNullString Branch { get; set; } = new Models.Common.IntegerNullString();

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string MinSalary { get; set; }

        public string MaxSalary { get; set; }
        public IntegerNullString SalaryType { get; set; } = new IntegerNullString();
        public string MinIncentive { get; set; }
        public string MaxIncentive { get; set; }
        public IntegerNullString IncentiveType { get; set; }
        public IntegerNullString JobShiftTime { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string EnglishLevel { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
        public List<JobType> jobType { get; set; } = new List<JobType>();
        public List<JobSkill> jobSkill { get; set; } = new List<JobSkill>();

        [Required(ErrorMessage = "Last Apply date required")]
        public DateTime EndDate { get; set; }
        public List<ExperienceLevel> ExperienceLevels { get; set; } = new List<ExperienceLevel>();
    }

    public class JobSkill
    {
        public string skill { get; set; } 
    }
    public class ExperienceLevel 
    {
        public string level { get; set; }
    }
    public class JobType
    {
        public bool status { get; set; }
        public Models.Common.IntegerNullString type { get; set; } = new Models.Common.IntegerNullString();
    }
    public class Org_and_Branches 
    {
        public string Name { get; set; }
        public IntegerNullString Organization { get; set; } = new IntegerNullString();
        public IntegerNullString Branch { get; set; } = new IntegerNullString();
    }
}
