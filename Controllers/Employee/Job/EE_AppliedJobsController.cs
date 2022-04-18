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
    public class EE_AppliedJobsController : ControllerBase
    {
        [Route("apply/{Jid}")]
        [HttpPost]
        public IActionResult Create(int Jid)
        {
            int Uid = 50000002;
            return Ok(new EE_AppliedJobs().Create(Uid, Jid));
        }

        [Route("apply")]
        [HttpGet]
        public IActionResult One()
        {
            int Uid = 50000002;
            return Ok(new EE_AppliedJobs().One(Uid));
        }
    }
}
