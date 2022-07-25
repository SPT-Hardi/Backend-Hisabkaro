using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Resume
{
    [Route("Employee/Resume")]
    [ApiController]
    public class CreateResumesController : ControllerBase
    {
        [HttpPost]
        [Route("CreateResume")]
        public IActionResult CreateResume([FromBody]Models.Employee.Resume.CreateResume value) 
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new Cores.Employee.Resume.CreateResumes().Add(UId,value));
        }
    }
}
