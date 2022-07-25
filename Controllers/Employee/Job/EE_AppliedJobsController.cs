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
        [Route("Apply/{Id}")]
        [HttpPost]
        public IActionResult Apply_Toggle([FromRoute]int Id)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new EE_AppliedJobs().Applied_Toggle(UId,Id));
        }

        [Route("Apply_List")]
        [HttpGet]
        public IActionResult List()
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new EE_AppliedJobs().Applied_List(UId));
        }
    }
}
