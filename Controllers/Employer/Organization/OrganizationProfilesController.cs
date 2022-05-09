using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Cores.Common.Shift;
using HIsabKaro.Cores.Employer.Organization;
using HIsabKaro.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly ITokenServices _tokenService;
        private readonly ContactAddress _contactAddress;
        private readonly ShiftTimes _shiftTimes;
        private readonly IConfiguration _configuration;

        public OrganizationProfilesController(OrganizationProfiles organizationProfiles, ITokenServices tokenService, ContactAddress contactAddress,ShiftTimes shiftTimes, IConfiguration configuration)
        {
            _organizationProfiles = organizationProfiles;
            _tokenService = tokenService;
            _contactAddress = contactAddress;
            _shiftTimes = shiftTimes;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("OrganizationProfiles/One")]
        public IActionResult One([FromQuery] int OId)
        {
            return Ok(_organizationProfiles.One(OId));
        }

        [HttpPost]
        [Route("OrganizationProfiles/Create/{OId}")]
        public IActionResult Create([FromRoute] int OId,[FromBody] Models.Employer.Organization.OrganizationProfile value)
        {
            var UserId = HttpContext.Items["UserID"];
            //int UserId = 50000333;
            return Ok(_organizationProfiles.Create(UserId,OId,value,_configuration,_tokenService));
        }
    }
}
