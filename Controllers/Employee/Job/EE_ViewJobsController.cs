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
        [Route("View_ListofJobs")]
        [HttpGet]
        public IActionResult View_ListofJobs()
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new EE_ViewJobs().List(UId));
        }

        [Route("View_Job/{Id}")]
        [HttpGet]
        public IActionResult Get([FromRoute]int Id)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new EE_ViewJobs().One(UId,Id));
        }

        [Route("Search_Job/ByLocation")]
        [HttpGet]
        public IActionResult Search_ByLocation([FromQuery]string keyword)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new EE_ViewJobs().Search_ByLocation(UId,keyword));
        }

        [Route("Search_Job")]
        [HttpGet]
        public IActionResult Search([FromQuery]string keyword)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new EE_ViewJobs().Search_BySector_Company_JobTitle(UId,keyword));
        }
    }
}
