using HIsabKaro.Models.Common;
using HIsabKaro.Models.Common.Contact;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Models.Employee.Resume
{
    public class Contact
    {
        [JsonIgnore]
        public string MobileNumber { get; set; }
        [Required]
        public string AMobileNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int AddressId { get; set; }
        [Required]
        public Address Address { get; set; } = new Address();
       
    }
}
