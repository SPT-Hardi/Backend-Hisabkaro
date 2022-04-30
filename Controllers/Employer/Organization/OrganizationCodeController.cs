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
    public class OrganizationCodeController : ControllerBase
    {
        [HttpGet]
        [Route("Code")]
        public  IActionResult Get()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new OrganizationCodes().Get(URId));
        }
    }
}
