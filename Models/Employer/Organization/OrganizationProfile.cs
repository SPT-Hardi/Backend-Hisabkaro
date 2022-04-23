using HIsabKaro.Models.Common.Contact;
using HIsabKaro.Models.Common.Shift;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization
{
    public class OrganizationProfile
    {
        public string LogoFile { get; set; }
        [RegularExpression(@"^[0-9]{2}[A-Z]{5}\d{4}[A-Z]{1}\d{1}[A-Z]{1}\d{1}$", ErrorMessage = "Invalid GST Number!")]
        public string GSTNumber { get; set; }
        public string GST { get; set; }
        public List<ShitTime> ShiftTime { get; set; } = new List<ShitTime>();        
        public Address Address { get; set; }
        [Required(ErrorMessage = "Pan Card Number Is Required!")]
        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]{1}$", ErrorMessage = "Invalid Pan Card Number!")] 
        public string PanCardNumber { get; set; }
        [Required(ErrorMessage = "Pan Card Is Required!")]
        public string PanCard { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public List<Partner> Partners { get; set; } = new List<Partner>();
    }
    
    public class Partner
    {
        public string Email { get; set; }
        public string Mobilenumber { get; set; }
        public Common.IntegerNullString OwnershipTypeID { get; set; } = new Common.IntegerNullString();
    }
}
