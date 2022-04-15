
using HIsabKaro.Models.Common.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Developer.Subscriber
{
    public class UserCompleteProfile
    {
        public Address address { get; set; } = new Address();
        public TotalWorkExperience totalWorkExperience { get; set; } = new TotalWorkExperience();
    }
    public class TotalWorkExperience 
    {
        [JsonIgnore]
        public int UserWorkExperienceId { get; set; }

        public Duration Duration { get; set; } = new Duration();
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
