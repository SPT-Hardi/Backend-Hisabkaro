﻿using HIsabKaro.Models.Employer.Organization.Staff.Leave;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Leave
{
    [Route("Employer/Organization/Staff/Leave")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        [HttpPost]
        [Route("Request")]
        public IActionResult Create(Models.Employer.Organization.Staff.Leave.Request value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new Requests().Create(URId, value));
        }

        [HttpGet]
        [Route("Request/View")]
        public IActionResult Get()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new Requests().Get(URId));
        }
    }
}
