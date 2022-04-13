using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Developer.Subscriber
{
    public class OTP
    {
        [JsonIgnore]
        public int OTPId { get; set; }
        public string Otp { get; set; }
        public int DeviceId { get; set; }
        public DateTime ExpiryDate{ get; set; }
        public int UId { get; set; }
        public int IsUsed { get; set; }

    }
}
