using HIsabKaro.Models.Employer.Organization.Staff.Leave;
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
            int Uid = 50000012;
            int Rid = 1000001;
            return Ok(new Requests().Create(Uid, Rid, value));
        }
    }
}
