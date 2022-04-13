using HisaabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Employee.Resume
{
    public class Education
    {
        //public int UID { get; set; }
        public List<EducationDetail> educationlist { get; set; } = new List<EducationDetail>();
    }
    public class EducationDetail
    {
        [JsonIgnore]
        public int EmpResumeEducationId { get; set; }
        public IntegerNullString EducationName { get; set; }
        public IntegerNullString EducationStream { get; set; }
        public string InstituteName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
