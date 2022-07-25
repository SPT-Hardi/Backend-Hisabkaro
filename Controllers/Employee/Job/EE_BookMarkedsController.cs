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
        [HttpPost]
        [Route("Bookmarke/{Id}")]
        public IActionResult Bookmark_Toggle([FromRoute]int Id)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new EE_BookmarkedJobs().BookMarkedJob_Toggle(UId,Id));
        }

        [HttpGet]
        [Route("Bookmarked_List")]
        public IActionResult List()
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new EE_BookmarkedJobs().BookmarkedJob_List(UId));
        }

    }
}
