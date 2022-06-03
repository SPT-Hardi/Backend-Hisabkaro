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
        [Route("Orgloandetail")]
        [HttpGet]
        public IActionResult GetOrg()
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new LoanDetails().GetOrgLoan(URId));
        }

        [Route("Staffloandetail/{LoanId}")]
        [HttpGet]
        public IActionResult GetStaff([FromRoute]int LoanId)
        {
            var URId = HttpContext.Items["URId"];
            return Ok(new LoanDetails().GetStaffLoan(URId,LoanId));
        }
    }
}
