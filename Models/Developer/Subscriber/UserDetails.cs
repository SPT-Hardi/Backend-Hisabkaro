using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Developer.Subscriber
{
    public class UserDetails
    {
        public int PhotoID { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string AMobileNumber { get; set; }

    }
}
