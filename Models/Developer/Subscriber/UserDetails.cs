
using HIsabKaro.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Developer.Subscriber
{
    public class UserDetails
    {
        public IntegerNullString role { get; set; } = new IntegerNullString();
        public UserPersonalDetails userdetails { get; set; } = new UserPersonalDetails();
    }
 
    public class UserPersonalDetails
    {
        public string ProfilePhotoFGUID { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        public string AMobileNumber { get; set; }


    }
}
