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
        [Required]
        public string DeviceToken { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string OTP { get; set; }
        public int OTPID { get; set; }
    }
}
