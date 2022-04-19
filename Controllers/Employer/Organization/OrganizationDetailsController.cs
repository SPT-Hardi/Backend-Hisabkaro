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
        [HttpPost]
        [Route("OrganizationDetails/Create")]
        public IActionResult Create([FromBody] Models.Employer.Organization.OrganizationDetail value)
        {
            int UserID = (int)HttpContext.Items["UserID"];
            return Ok(new OrganizationDetails().Create(UserID,value));
        }
    }
}
