using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Employer.Organization
{
    public class OrganizationDetail
    {
        public string? Image { get; set; }
        [Required(ErrorMessage = "Organization Name Is Required!")]
        public string OrgName { get; set; }
        public Common.IntegerNullString InudstrySector { get; set; } = new Common.IntegerNullString();
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
    }
}
