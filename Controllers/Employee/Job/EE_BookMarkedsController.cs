using HIsabKaro.Cores.Employee.Job;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Job
{
    [Route("Employee/Job")]
    [ApiController]
    public class EE_BookMarkedsController : ControllerBase
    {
        [Route("save/{Jid}")]
        [HttpPost]
        public IActionResult Create(int Jid)
        {
            int UserId = (int)HttpContext.Items["UserID"];
            return Ok(new EE_BookmarkedJobs().Create(UserId, Jid));
        }

        [Route("save")]
        [HttpGet]
        public IActionResult One()
        {
            int UserId = (int)HttpContext.Items["UserID"];
            return Ok(new EE_BookmarkedJobs().One(UserId));
        }
    }
}
