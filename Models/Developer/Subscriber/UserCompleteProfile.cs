
using HIsabKaro.Models.Common.Contact;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [RegularExpression("^.{1,100}$",ErrorMessage ="Maximum 100 character are allowed!")]
        public string Description { get; set; }

        
        public bool LookingForJob { get; set; }

       
        public bool CurrentlyWorking { get; set; }

    }
    public class Duration
    {
        [Required(ErrorMessage ="Years are required!")]
        [RegularExpression("^[0-9]+$",ErrorMessage ="Only digits are allowed!")]
        public string Year { get; set; }


        [Required(ErrorMessage = "Months are required!")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Only digits are allowed!")]
        public string Month { get; set; }
    }
}
