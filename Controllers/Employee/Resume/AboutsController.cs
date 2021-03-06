using HIsabKaro.Cores.Employee.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Resume
{
    [Route("Employee/User/Resume")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        [HttpPost]
        [Route("About")]
        public IActionResult Post(Models.Employee.Resume.About value) 
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new Abouts().Add(UID.ToString(),value));
        }

        [HttpGet]
        [Route("About")]
        public IActionResult Get()
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new Abouts().View(UID.ToString()));
        }
    }
}
