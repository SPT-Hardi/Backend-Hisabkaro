using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Resume
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
        [Required]
        public IntegerNullString EducationName { get; set; }

        public string  EducationStreamName { get; set; }
        [Required]
        public string InstituteName { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

    }
}
