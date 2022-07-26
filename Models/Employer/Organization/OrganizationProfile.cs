using HIsabKaro.Models.Common;
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
        public string LogoFGUId { get; set; } = null;
        [Required(ErrorMessage ="Organization name is required!")]
        [RegularExpression("^[a-z A-Z 0-9]{1,50}$",ErrorMessage = "Organization name is required!,Allowed_Value : Alphanumeric only ,Max_Length : 50")]
        public string OrganizationName { get; set; }
        public int? AddressId { get; set; } = null;
        public List<IntegerString> WeekOff { get; set; } = new List<IntegerString>();
        public List<ShitTime> ShiftTime { get; set; } = new List<ShitTime>();
        [Validation.Pair_NullIntegerNullString(ErrorMessage ="Sector is required!, Allowed_Value: [Id:0 or any digit][Text:any], NotAllowed_Value:[Id:null]")]
        public Common.IntegerNullString Sector { get; set; } = new Common.IntegerNullString();
        public List<Branches> Branches { get; set; } = new List<Branches>();
        public List<Partner> Partners { get; set; } = new List<Partner>();
        public OrgInformation OrgInformation { get; set; } = new OrgInformation();
    }
    
    public class Partner
    {
        [JsonIgnore]
        public int? PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string Mobilenumber { get; set; }
    }
    public class Branches
    {
        [JsonIgnore]
        public int BranchId{get;set;}
        public string BranchName { get; set; } = null;
        public int? AddressId { get; set; } = null;
    }
    public class OrgInformation 
    {
        public string GSTNumber { get; set; }
        public string GSTFGUId { get; set; }
        public string PANNumber { get; set; }
        public string PANFGUId { get; set; }
    }
}
