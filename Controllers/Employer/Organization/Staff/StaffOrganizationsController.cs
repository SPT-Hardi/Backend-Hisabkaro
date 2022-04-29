using HIsabKaro.Cores.Employer.Organization.Staff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new StaffOrganizations().Get(URId));
        }
    }
}
