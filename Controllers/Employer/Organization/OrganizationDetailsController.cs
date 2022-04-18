using HIsabKaro.Cores.Employer.Organization;
using HIsabKaro.Services;
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
    public class OrganizationDetailsController : ControllerBase
    {
        private readonly ITokenServices _tokenService;

        public OrganizationDetailsController(ITokenServices tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost]
        [Route("OrganizationDetails/Create")]
        public IActionResult Create([FromBody] Models.Employer.Organization.OrganizationDetail value)
        {
            int UserID = (int)HttpContext.Items["UserID"];
            return Ok(new OrganizationDetails(_tokenService).Create(UserID,value));
        }
    }
}
