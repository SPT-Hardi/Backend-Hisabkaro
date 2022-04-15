using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Resume
{
    public class About
    {
        [JsonIgnore]
        public int EmpResumeAboutId { get; set; }
        public string AboutText { get; set; }
    }
}
