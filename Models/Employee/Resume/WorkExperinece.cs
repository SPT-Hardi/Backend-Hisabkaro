using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Resume
{
    public class WorkExperinece
    {
        public List<WorkExperienceDetails> workExperienceDetails { get; set; } = new List<WorkExperienceDetails>();
    }
    public class WorkExperienceDetails 
    {
        [JsonIgnore]
        public int EmpResumeWorkExperienceId { get; set; }
        public string JobTitle { get; set; }
        public string OrganizationName { get; set; }
        public string WorkFrom { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
