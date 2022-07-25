using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Common.Contact
{
    public class Address
    {
        [JsonIgnore]
        public int AddressId { get; set; }
        //[Required(ErrorMessage ="Address is required!")]
        //[RegularExpression("^.{1,100}$",ErrorMessage ="Maximum 100 character allowed!")]
        public string AddressLine1 { get; set; } = null;

        /* [RegularExpression("^.{1,100}$", ErrorMessage = "Maximum 100 character allowed!")]*/
        public string AddressLine2 { get; set; } = null;


        /* [Required(ErrorMessage = "Cityname is required!")]
         [RegularExpression("^[a-zA-Z\s]{1,50}$", ErrorMessage = "Maximum 50 character allowed containing only alphabet!")]*/
        public string City { get; set; } 


       /* [Required(ErrorMessage = "Statename is required!")]
        [RegularExpression("^[a-zA-Z\s]{1,50}$", ErrorMessage = "Maximum 50 character allowed containing only alphabet!")]*/
        public string State { get; set; }


        /*[Required(ErrorMessage = "Pincode is required!")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Maximum 6 digits are allowed!")]*/
        public int PinCode { get; set; }

      /*  [RegularExpression("^.{1,50}$", ErrorMessage = "Maximum 50 character allowed!")]*/
        public string? LandMark { get; set; }
    }
}
