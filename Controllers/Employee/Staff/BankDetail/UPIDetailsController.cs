using HIsabKaro.Cores.Employee.Staff.BankDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Staff.BankDetail
{
    [Route("Employee/Staff/BankDetail")]
    [ApiController]
    public class UPIDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("UPIDetails/One")]
        public IActionResult One()
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new UPIDetails().One(URId));
        }

        [HttpPost]
        [Route("UPIDetails/Create")]
        public IActionResult Create([FromBody] Models.Employee.Staff.BankDetail.UPIDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new UPIDetails().Create(URId, value));
        }
    }
}
