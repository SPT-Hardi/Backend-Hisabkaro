﻿using HIsabKaro.Cores.Employer.Organization.Staff;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Controllers.Employer.Organization.Staff
{
    [Route("Employer/Organization/Staff")]
    [ApiController]
    public class StaffDetailsController : ControllerBase
    {
        private readonly StaffDetails _staffDetails;
        private readonly ITokenServices _tokenService;

        public StaffDetailsController(StaffDetails staffDetails,ITokenServices tokenService)
        {
            _staffDetails = staffDetails;
            _tokenService = tokenService;
        }
        [HttpGet]
        [Route("StaffDetails/One")]
        public IActionResult One([FromQuery] int OId)
        {
            return Ok(_staffDetails.One(OId));
        }

        [HttpPost]
        [Route("StaffDetails/Create")]
        public IActionResult Create([FromBody] Models.Employer.Organization.Staff.StaffDetail value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_staffDetails.Create(URId, value));
        }

        //Angular
        [HttpPost]
        [Route("StaffDetails/BulkCreate")]
        public IActionResult BulkCreate([FromBody] List<Models.Employer.Organization.Staff.StaffDetail> value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_staffDetails.BulkCreate(URId, value));
        }

        //CSV
        //[HttpPost]
        [HttpPost]
        [Route("StaffDetails/BulkStaffDetails/Create")]
        public IActionResult Create([FromBody] Models.Employer.Organization.Staff.BulkStaffDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(_staffDetails.Create(URId, value));
        }

        [HttpGet]
        [Route("StaffDetails/Drop")]
        public List<IntegerNullString> Drop()
        {
            var URId = HttpContext.Items["URId"];
            using (DBContext c = new DBContext())
            {
                return (from x in c.DevOrganisationsShiftTimes
                        where x.OId == c.SubUserOrganisations.Where(x=>x.URId==(int)URId).FirstOrDefault().OId
                        select new IntegerNullString()
                        {
                            Id = x.ShiftTimeId,
                            Text = Convert.ToDateTime(x.StartTime.ToString()).ToString("hh:mm tt")
                        }).ToList();
            }
        }

        [HttpPost]
        [Route("StaffDetails/JoinOrganization")]

        public IActionResult JoinOrganizationCreate([FromBody] Models.Employer.Organization.Staff.JoinOrganizationCreate value)
        {
            return Ok(_staffDetails.JoinOrganizationCreate(value));
        }
    }
}
