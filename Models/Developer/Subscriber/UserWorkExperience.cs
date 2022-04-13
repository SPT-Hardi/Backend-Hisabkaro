using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Developer.Subscriber
{
    public class UserWorkExperience
    {
        [JsonIgnore]
        public int UserWorkExperienceId { get; set; }
        public Duration Duration = new Duration();
        public string Description { get; set; }
        public bool LookingForJob { get; set; }
        public bool CurrentlyWorking { get; set; }

    }
    public class Duration 
    {
        public string Year { get; set; }
        public string Month { get; set; }
    }
}
