using HIsabKaro.Cores.Common.Context;
using HIsabKaro.Cores.Employee.Staff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employee.Staff
{
    [Route("Employee/Staff")]
    [ApiController]
    public class SatffPersonalDetailsController : ControllerBase
    {
        private readonly StaffPersonalDetails _staffPersonalDetails;
        private readonly ContactAddress _contactAddress;

        public SatffPersonalDetailsController(StaffPersonalDetails staffPersonalDetails, ContactAddress contactAddress)
        {
            _staffPersonalDetails = staffPersonalDetails;
            _contactAddress = contactAddress;
        }

        [HttpGet]
        [Route("SatffPersonalDetails/One")]
        public IActionResult One()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_staffPersonalDetails.One(URId));
        }

        [HttpPost]
        [Route("SatffPersonalDetails/Create")]
        public IActionResult Create([FromBody] Models.Employee.Staff.StaffPersonalDetail value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_staffPersonalDetails.Create(URId,value));
        }
        [HttpGet]
        [Route("SatffPersonalDetails/StaffProfile/One")]
        public IActionResult GetOne()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_staffPersonalDetails.GetOne(URId));
        }

        [HttpPost]
        [Route("SatffPersonalDetails/StaffProfile/Create")]
        public IActionResult GetPut([FromBody] Models.Employee.Staff.StaffProfile value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_staffPersonalDetails.GetPut(URId, value));
        }
    }
}
