using HIsabKaro.Cores.Employer.Organization.Staff;
using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff
{
    [Route("Employer/Organization/Staff")]
    [ApiController]
    public class StaffDetailsController : ControllerBase
    {
        [HttpGet]
        [Route("StaffDetails/One")]
        public IActionResult One([FromQuery] int OId)
        {
            return Ok(new StaffDetails().One(OId));
        }

        [HttpPost]
        [Route("StaffDetails/Create")]

        public IActionResult Create([FromBody] Models.Employer.Organization.Staff.StaffDetail value)
        {
            return Ok(new StaffDetails().Create(value));
        }

        [HttpGet]
        [Route("StaffDetails/Drop")]
        public List<IntegerNullString> Drop([FromQuery] int OId)
        {
            using (DBContext c = new DBContext())
            {
                return (from x in c.DevOrganisationsShiftTimes
                        where x.OId == OId
                        select new IntegerNullString()
                        {
                            ID = x.ShiftTimeId,
                            Text = x.StartTime.ToString(),
                        }).ToList();
            }

        }
    }
}
