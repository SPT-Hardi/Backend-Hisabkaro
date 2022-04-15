using HIsabKaro.Cores.Common.Contact;
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
            int Uid = 1;
            return Ok(_branchDetails.Create(Uid,value));
        }

        [Route("Branch/{BId}")]
        [HttpPut]
        public IActionResult Update([FromBody]Models.Employer.Organization.Branch.BranchDetail value,int BId)
        {
            int Uid = 1;
            return Ok(_branchDetails.Update(Uid, BId,value));
        }

        [Route("OrgBranch/{Oid}")]
        [HttpGet]
        public IActionResult GetOrg([FromRoute]int Oid)
        {
            int Uid = 1;
            return Ok(_branchDetails.GetOrg(Oid,Uid));
        }

        [Route("Branch/{Bid}")]
        [HttpGet]
        public IActionResult GetBranch([FromRoute]int Bid)
        {
            int Uid = 1;
            return Ok(_branchDetails.GetBranch(Bid,Uid));
        }
    }
}
