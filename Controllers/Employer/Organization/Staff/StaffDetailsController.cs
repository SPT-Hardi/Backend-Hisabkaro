using HIsabKaro.Controllers.Filters;
using HIsabKaro.Cores.Employer.Organization.Staff;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using HisabKaroContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        //    [HttpGet]
        //    [Route("StaffDetails/One")]
        //    public IActionResult One([FromQuery] int OId)
        //    {
        //        return Ok(_staffDetails.One(OId));
        //    }

        [HttpPost]
        [Route("StaffDetails/Create")]
        [ValidateEmployerToken]
        public IActionResult Create([FromBody]Models.Employer.Organization.Staff.StaffDetailList value)
        {
            //var s =JsonConvert.DeserializeObject<List<Models.Employer.Organization.Staff.StaffDetail>>(value.ToString());

            var URId = HttpContext.Items["URId"];
            return Ok(new StaffDetails().Create(URId, value));
        }

        //    //Angular
        //    [HttpPost]
        //    [Route("StaffDetails/BulkCreate")]
        //    public IActionResult BulkCreate([FromBody] List<Models.Employer.Organization.Staff.StaffDetail> value)
        //    {
        //        var URId = HttpContext.Items["URId"];
        //        return Ok(_staffDetails.BulkCreate(URId, value));
        //    }

        //    [HttpGet]
        //    [Route("StaffDetails/Drop/{Id}")]
        //    public List<IntegerNullString> Drop([FromRoute]int Id)
        //    {
        //        var URId = HttpContext.Items["URId"];
        //        using (DBContext c = new DBContext())
        //        {
        //            return (from x in c.DevOrganisationsShiftTimes
        //                    where x.OId ==Id  //c.SubUserOrganisations.Where(x=>x.URId==(int)URId).FirstOrDefault().OId
        //                    select new IntegerNullString()
        //                    {
        //                        Id = x.ShiftTimeId,
        //                        Text = Convert.ToDateTime(x.StartTime.ToString()).ToString("hh:mm tt")
        //                    }).ToList();
        //        }
        //    }

        //    [HttpPost]
        //    [Route("StaffDetails/JoinOrganization")]

        //    public IActionResult JoinOrganizationCreate([FromBody] Models.Employer.Organization.Staff.JoinOrganizationCreate value)
        //    {
        //        return Ok(_staffDetails.JoinOrganizationCreate(value,_configuration,_tokenService));
        //    }
    }
}
