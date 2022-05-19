using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Developer.Subscriber
{
    public class UserMobile
    {
        public string DeviceProfile { get; set; }


        [Required(ErrorMessage ="DeviceToken is required!")]
        public string DeviceToken { get; set; }


        [Required(ErrorMessage ="MobileNumber is required!")]
        public string MobileNumber { get; set; }


        [Required(ErrorMessage ="OTP is required!")]
        public string OTP { get; set; }


        public int OTPID { get; set; }
    }
}
