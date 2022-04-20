using HIsabKaro.Cores.Common.Contact;
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
            int URId = (int)HttpContext.Items["URId"];
            return Ok(_staffPersonalDetails.One(URId));
        }

        [HttpPost]
        [Route("SatffPersonalDetails/Create")]
        public IActionResult Create([FromBody] Models.Employee.Staff.StaffPersonalDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(_staffPersonalDetails.Create(URId,value));
        }
    }
}
