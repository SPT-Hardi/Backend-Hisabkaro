using HIsabKaro.Cores.Employer.Organization.Staff.Loan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Employer.Organization.Staff.Loan
{
    [Route("Employer/Organization/Staff")]
    [ApiController]
    public class LoanDetailsController : ControllerBase
    {
        [Route("loan/{StaffId}")]
        [HttpPost]
        public IActionResult Create([FromRoute]int StaffId,[FromBody]Models.Employer.Organization.Staff.Loan.LoanDetail value)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new LoanDetails().Create(URId,StaffId, value));
        }

        [Route("Orgloandetail")]
        [HttpGet]
        public IActionResult GetOrg()
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new LoanDetails().GetOrgLoan(URId));
        }

        [Route("Staffloandetail/{LoanId}")]
        [HttpGet]
        public IActionResult GetStaff([FromRoute]int LoanId)
        {
            int URId = (int)HttpContext.Items["URId"];
            return Ok(new LoanDetails().Get(URId,LoanId));
        }
    }
}
