﻿using HIsabKaro.Cores.Employer.Organization.Staff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIsabKaro.Controllers.Employer.Organization.Staff
{
    [Route("Employer/Organization/Staff")]
    [ApiController]
    public class StaffOrganizationsController : ControllerBase
    {
        [HttpGet]
        [Route("StaffOrganization")]
        public IActionResult Get() 
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new StaffOrganizations().Get(URId));
        }
    }
}