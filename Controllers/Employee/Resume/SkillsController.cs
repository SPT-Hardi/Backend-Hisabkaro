﻿using HIsabKaro.Cores.Employee.Resume;
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
        [HttpPost]
        [Route("Skills")]
        public IActionResult Add(Models.Employee.Resume.Skill value) 
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new Skills().Add(UID,value));
        }

        [HttpGet]
        [Route("Skills")]
        public IActionResult Get()
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new Skills().View(UID));
        }

        [HttpPatch]
        [Route("Skills/{Id}")]
        public IActionResult Update(Models.Employee.Resume.SkillDetails value,int Id)
        {
            int UID = (int)HttpContext.Items["UserID"];
            return Ok(new Skills().Update(Id, UID,value));
        }

        [HttpDelete]
        [Route("Skills/{Id}")]
        public IActionResult Update([FromRoute]int Id)
        {
            int UId = (int)HttpContext.Items["UserID"];
            return Ok(new Skills().Delete(UId,Id));
        }
    }
}
