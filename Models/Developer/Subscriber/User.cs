using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Developer.Subscriber
{
    public class User
    {
        [Validation.Pair_IntegerString(ErrorMessage ="Language is required!")]
        public IntegerString Language { get; set; } = new IntegerString();


        [Required(ErrorMessage ="Mobilenumber is required!")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Only 10 digit allowed and startfrom 6,7,8,9 !")]
        public string MobileNumber { get; set; }


        [Required(ErrorMessage ="DeviceToken is required!")]
        public string DeviceToken { get; set; }
    }
}
