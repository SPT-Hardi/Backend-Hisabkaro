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
        private readonly ITokenServices _tokenService;
        private readonly IConfiguration _configuration;
        public OrganizationProfilesController(ITokenServices tokenService,IConfiguration configuration)
        {
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("OrganizationProfiles/Create")]
        public IActionResult Create([FromBody] Models.Employer.Organization.OrganizationProfile value)
        {
            var UId = HttpContext.Items["UId"];
            var DeviceToken = HttpContext.Items["DeviceToken"];
            return Ok(new Cores.Employer.Organization.OrganizationProfiles().Create(UId, DeviceToken, value, _tokenService, _configuration));
        }

        [HttpGet]
        [Route("OrganizationProfiles/One/{OId}")]
        public IActionResult One([FromRoute] int OId)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new Cores.Employer.Organization.OrganizationProfiles().One(URId,OId));
        }
    }
}
