using HIsabKaro.Cores.Employee.Resume;
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
    public class SkillsController : ControllerBase
    {
        [HttpGet]
        [Route("Skills")]
        public IActionResult Get()
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Skills().View(UID));
        }

        [HttpPut]
        [Route("Skills")]
        public  IActionResult Update([FromBody] Models.Employee.Resume.List_Skills value)
        {
            var UID = HttpContext.Items["UId"];
            return Ok(new Cores.Employee.Resume.Skills().Update_Skills(UID, value));
        }
    }
}
