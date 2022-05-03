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
    public class BulkStaffDetailsController : ControllerBase
    {
        [HttpPost]
        [Route("BulkStaffDetails/Create")]
        public IActionResult Create([FromBody] Models.Employer.Organization.Staff.BulkStaffDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new BulkStaffDetails().Create(URId,value));
        }                                                                     
    }
}
