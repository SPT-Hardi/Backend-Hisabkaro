﻿using HIsabKaro.Cores.Common.Contact;
using HIsabKaro.Cores.Common.Shift;
using HIsabKaro.Cores.Employer.Organization.Branch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Branch
{
    [Route("Employer/Organization")]
    [ApiController]
    public class BranchDetailsController : ControllerBase
    {
        private readonly ContactAddress _contactAddress;
        private readonly BranchDetails _branchDetails;
        private readonly ShiftTimes _shiftTimes;

        public BranchDetailsController(ContactAddress contactAddress,BranchDetails branchDetails, ShiftTimes shiftTimes)
        {
            _contactAddress = contactAddress;
            _branchDetails = branchDetails;
            _shiftTimes = shiftTimes;
        }

        [Route("Branch")]
        [HttpPost]
        public IActionResult Create([FromBody]Models.Employer.Organization.Branch.BranchDetail value)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_branchDetails.Create(URId, value));
        }

        [Route("Branch/{BId}")]
        [HttpPut]
        public IActionResult Update([FromBody]Models.Employer.Organization.Branch.BranchDetail value,int BId)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_branchDetails.Update(URId, BId,value));
        }

        [Route("OrgBranch")]
        [HttpGet]
        public IActionResult GetOrgBranch()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_branchDetails.GetOrg(URId));
        }

        [Route("Branch/{Bid}")]
        [HttpGet]
        public IActionResult GetBranch([FromRoute]int Bid)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(_branchDetails.GetBranch(Bid, URId));
        }

        [Route("date")]
        [HttpGet]
        public IActionResult Date()
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            DateTime ISTDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), INDIAN_ZONE);
            DateTime localDate = DateTime.Now;
            DateTime utcDate = DateTime.UtcNow;
            //DateTime localTime = DateTime.Now.ToLocalTime();

            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(localDate);
            var db = Convert.ToDateTime(utcDate);
            return Ok(localDate + " , " + utcDate + " , " + ISTDate + " , " + db);
        }
    }
}
