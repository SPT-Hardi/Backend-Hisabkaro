﻿using HIsabKaro.Cores.Employee.Job;
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
        [Route("save/{Jid}")]
        [HttpPost]
        public IActionResult Create(int Jid)
        {
            var UserId = HttpContext.Items["UId"];
            return Ok(new EE_BookmarkedJobs().Create(UserId, Jid));
        }

        [Route("save")]
        [HttpGet]
        public IActionResult One()
        {
            var UserId = HttpContext.Items["UId"];
            return Ok(new EE_BookmarkedJobs().One(UserId));
        }

        [Route("save/{SaveId}")]
        [HttpDelete]
        public IActionResult Delete(int SaveId)
        {
            var UserId = HttpContext.Items["UId"];
            return Ok(new EE_BookmarkedJobs().Remove(UserId, SaveId));
        }
    }
}
