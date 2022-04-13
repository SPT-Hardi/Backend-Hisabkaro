using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Developer.Subscriber
{
    public class User
    {

        [Required]
        public int DefaultLanguageID { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string DeviceToken { get; set; }
        public string DeviceProfile { get; set; }
    }
}
