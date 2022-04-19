using HIsabKaro.Cores.Employer.Organization.Staff.Leave;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Leave
{
    [Route("Employer/Organization/Staff")]
    [ApiController]
    public class SubmitsController : ControllerBase
    {
        [HttpPost]
        [Route("Leave/Submit/{URId}")]
        public IActionResult Post([FromRoute] int URId, [FromBody] Models.Employer.Organization.Staff.Leave.Submit value)
        {
            int Uid = 50000003;
            int Rid = 1000002;
            return Ok(new Submits().Create(URId,Uid,Rid,value));
        }
    }
}
