using HisaabKaro.Cores.Employee.Resume;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Controllers.Employee.Resume
{
    [Route("Employee/Resume")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        [HttpPost]
        [Route("Skills")]
        public IActionResult Add(Models.Employee.Resume.Skill value) 
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new Skills().Add(value,UID.ToString()));
        }

        [HttpGet]
        [Route("Skills")]
        public IActionResult Get()
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new Skills().View(UID.ToString()));
        }

        [HttpPatch]
        [Route("Skills/{Id}")]
        public IActionResult Update(Models.Employee.Resume.SkillDetails value,int Id)
        {
            var UID = HttpContext.Items["UserID"];
            return Ok(new Skills().Update(value,Id,UID.ToString()));
        }
    }
}
