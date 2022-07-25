using HIsabKaro.Models.Common.Contact;
using HIsabKaro.Models.Common.Shift;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization
{
    public class OrganizationProfile
    {
        [JsonIgnore]
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string LogoFGUId { get; set; }
        [Required(ErrorMessage = "Shift Time Is Required!")]
        public List<ShitTime> ShiftTime { get; set; } = new List<ShitTime>();        
        public int AddressId { get; set; }
        public Common.IntegerNullString OwnershipType { get; set; } = new Common.IntegerNullString();
        public List<Partner> Partners { get; set; } = new List<Partner>();
    }
    
    public class Partner
    {
        [JsonIgnore]
        public int? PartnerId { get; set; }
        public string Email { get; set; }
        public string Mobilenumber { get; set; }
    }
}
