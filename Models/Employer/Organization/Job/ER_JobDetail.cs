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

        public string Location { get; set; }

        public List<JobType> jobType { get; set; } = new List<JobType>();

        public string MinSalary { get; set; }

        public string MaxSalary { get; set; }

        public List<JobSkill> jobSkill { get; set; } = new List<JobSkill>();

        public string Roles { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Last Apply date required")]
        public DateTime Enddate { get; set; }
    }

    public class JobSkill
    {
        public string skill { get; set; } 
    }

    public class JobType
    {
        public bool status { get; set; }
        public Models.Common.IntegerNullString type { get; set; } = new Models.Common.IntegerNullString();
    }
}
