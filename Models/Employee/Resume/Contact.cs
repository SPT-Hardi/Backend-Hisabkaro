using HIsabKaro.Models.Common;
using HIsabKaro.Models.Common.Contact;
using HisabKaroContext;
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

        //[Required(ErrorMessage ="Alternate mobile number field is required!")]
        //[RegularExpression(@"^[6-9][0-9]{9}$", ErrorMessage = "Only 10 digit allowed and startfrom 6,7,8,9 !")]
        public string AMobileNumber { get; set; }

     
        [EmailAddress(ErrorMessage ="Enter valid email-address!")]
        public string Email { get; set; }

/*
        [Required(ErrorMessage ="AddressId is required!")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Enter only digits!")]
        public int AddressId { get; set; }*/


        //[Required(ErrorMessage ="Address is required!")]
        public Address Address { get; set; } = new Address();
       
    }
}
