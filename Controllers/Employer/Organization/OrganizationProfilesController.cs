using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Cores.Common.Shift;
using HIsabKaro.Cores.Employer.Organization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization
{
    [Route("Employer/Organization")]
    [ApiController]
    public class OrganizationProfilesController : ControllerBase
    {
        private readonly OrganizationProfiles _organizationProfiles;
        private readonly ContactAddress _contactAddress;
        private readonly ShiftTimes _shiftTimes;

        public OrganizationProfilesController(OrganizationProfiles organizationProfiles,ContactAddress contactAddress,ShiftTimes shiftTimes )
        {
            _organizationProfiles = organizationProfiles;
            _contactAddress = contactAddress;
            _shiftTimes = shiftTimes;
        }

        [HttpGet]
        [Route("OrganizationProfiles/One")]
        public IActionResult One([FromQuery] int OId)
        {
            return Ok(_organizationProfiles.One(OId));
        }
        [HttpPost]
        [Route("OrganizationProfiles/Create")]

        public IActionResult Create([FromBody] Models.Employer.Organization.OrganizationProfile value)
        {
            //int UserID = (int)HttpContext.Items["UserID"];
            int UserID = 50000001;
            return Ok(_organizationProfiles.Create(UserID, value ));
        }
    }
}
