using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employee.Resume
{
    public class About
    {
        [JsonIgnore]
        public int EmpResumeAboutId { get; set; }

        [Required(ErrorMessage ="AboutText field is required")]
        [RegularExpression(@"^.{1,150}$",ErrorMessage ="AnyValue,Max 150 characters are allowed!")]
        public string AboutText { get; set; }
    }
}
