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
    public class EE_ViewJobsController : ControllerBase
    {
        [Route("ViewJobByCompany")]
        [HttpPost]
        public IActionResult Search(Models.Employee.Job.EE_ViewJob value)
        {
            int Uid = 1;
            return Ok(new EE_ViewJobs().Create( Uid,value));
        }

        [Route("ViewJobs")]
        [HttpGet]
        public IActionResult Get()
        {
            int Uid = 5;
            return Ok(new EE_ViewJobs().Get(Uid));
        }
    }
}
