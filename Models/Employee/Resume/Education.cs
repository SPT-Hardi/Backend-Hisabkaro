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


        [Required(ErrorMessage = "Education name is required!")]
        public IntegerNullString EducationName { get; set; }


        [RegularExpression(@"^[a-z A-Z]{1,50}$",ErrorMessage ="Only alphabates contains space,Max 50 characters are allowed!")]
        public string EducationStreamName { get; set; }


        [Required(ErrorMessage ="Institute name is required!")]
        [RegularExpression(@"^[a-z A-Z .]{1,100}$", ErrorMessage = "Only alphabates contains space and dot,Max 100 characters are allowed!")]
        public string InstituteName { get; set; }


        [Required(ErrorMessage ="StartDate is required!")]
        public DateTime StartDate { get; set; }


        [Required(ErrorMessage ="EndDate is required!")]
        public DateTime EndDate { get; set; }

    }
}
