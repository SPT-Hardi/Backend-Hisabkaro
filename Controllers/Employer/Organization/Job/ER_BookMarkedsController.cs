using HIsabKaro.Cores.Employer.Organization.Job;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Job
{
    [Route("Employer/Organization/Job")]
    [ApiController]
    public class ER_BookMarkedsController : ControllerBase
    {
        [Route("ViewBookMark/{Jid}")]
        [HttpGet]
        public IActionResult One(int Jid)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new ER_BookMarkeds().Get(URId,Jid));
        }
    }
}
