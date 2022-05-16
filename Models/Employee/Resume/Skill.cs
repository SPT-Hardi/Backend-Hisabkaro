using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Resume
{
    public class Skill
    {
        public List<SkillDetails> skillDetails { get; set; } = new List<SkillDetails>();
    }
    public class SkillDetails 
    {
        [JsonIgnore]
        public int EmpResumeSkillId { get; set; }

        [Required(ErrorMessage ="Skillname is required!")]
        public string SkillName { get; set; }
    }
}
