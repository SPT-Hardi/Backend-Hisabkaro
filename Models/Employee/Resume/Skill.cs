using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Employee.Resume
{
    public class Skill
    {
        public List<SkillDetails> skillDetails { get; set; } = new List<SkillDetails>();
    }
    public class SkillDetails 
    {
        [JsonIgnore]
        public int EmpResumeSkillId { get; set; }
        public string SkillName { get; set; }
    }
}
