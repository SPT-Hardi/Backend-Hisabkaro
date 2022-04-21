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
    public class AccountDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("AccountDetails/One")]
        public IActionResult One()
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new AccountDetails().One(URId));
        }

        [HttpPost]
        [Route("AccountDetails/Create")]
        public IActionResult Create([FromBody] Models.Employee.Staff.BankDetail.AccountDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new AccountDetails().Create(URId, value));
        }
    }
}
