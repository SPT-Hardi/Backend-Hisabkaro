
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

        [Required(ErrorMessage = "Fullname is required!")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only character are allowed!")]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage ="Enter valid emailaddress!")]
        [Required(ErrorMessage ="Email address is required!")]
        public string Email { get; set; }


        [Required(ErrorMessage ="Mobile number is required!")]
        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Only 10 digit allowed and startfrom 6,7,8,9 !")]
        public string MobileNumber { get; set; }


        [RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Only 10 digit allowed and startfrom 6,7,8,9 !")]
        public string AMobileNumber { get; set; }
    }
}
