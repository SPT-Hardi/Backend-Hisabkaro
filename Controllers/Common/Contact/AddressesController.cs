using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common.Contact
{
    [Route("Common/Contact/Address")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        [HttpGet]
        [Route("Organiztion/{Id}")]
        public IActionResult Org_Address([FromRoute]int Id) 
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new Cores.Common.Contact.ContactAddress().GetOrgAddress(UId,Id));
        }

        [HttpGet]
        [Route("Branch/{Id}")]
        public IActionResult Branch_Address([FromRoute] int Id)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new Cores.Common.Contact.ContactAddress().GetBranchAddress(UId, Id));
        }
    }
}
