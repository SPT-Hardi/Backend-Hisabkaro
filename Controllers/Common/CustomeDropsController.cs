using HIsabKaro.Cores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common
{
    [Route("Common/CustomDrop")]
    [ApiController]
    public class CustomeDropsController : ControllerBase
    {
        [HttpGet]
        [Route("General/Data/LoginType")]
        public IActionResult Get() 
        {
            return Ok(new CustomeDrops.RoleDrop().Get());
        }


        [HttpGet]
        [Route("General/Data/Organizations")]
        public IActionResult Org_Drop()
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new CustomeDrops.Org_Drop().Get(UId));
        }

        [HttpGet]
        [Route("General/Data/Branches/{Id}")]
        public IActionResult Branches_drop([FromRoute]int Id)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new CustomeDrops.Branches_Drop().Get(UId,Id));
        }

        [HttpGet]
        [Route("General/Data/Organizations_Branches")]
        public IActionResult Orgs_Branches_drop([FromQuery]int? Id)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new CustomeDrops.Orgs_Branches_Drop().Get(UId,Id));
        }

        [HttpGet]
        [Route("General/Data/Salary_Type")]
        public IActionResult Salary_Type_drop()
        {
            return Ok(new CustomeDrops.Salary_Type_Drop().Get());
        }

        [HttpGet]
        [Route("General/Data/Organization_ShiftTime/{Id}")]
        public IActionResult Organization_ShiftTime_Drop([FromRoute]int Id)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new CustomeDrops.Org_Shift_Type().Get(UId,Id));
        }

        [HttpGet]
        [Route("General/Data/Branch_ShiftTime/{Id}")]
        public IActionResult Branch_ShiftTime_Drop([FromRoute]int Id)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new CustomeDrops.Branch_Shift_Type().Get(UId,Id));
        }

        [HttpGet]
        [Route("General/Data/Incentive_Type")]
        public IActionResult Incentive_Type_drop([FromQuery] int? Id)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new CustomeDrops.Incentive_Type_Drop().Get());
        }

        [HttpGet]
        [Route("General/Data/Search_Skill")]
        public IActionResult Search_Skill([FromQuery]string keyword)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new CustomeDrops.Skill_Search().Get(keyword));
        }

        [HttpGet]
        [Route("General/Data/Search_Language")]
        public IActionResult Search_Language([FromQuery] string keyword)
        {
            var UId = HttpContext.Items["UId"];
            return Ok(new CustomeDrops.Language_Search().Get(keyword));
        }

        [HttpGet]
        [Route("General/Data/English_Level")]
        public IActionResult English_Level_Drop()
        {
            return Ok(new CustomeDrops.EnglishLevel_Drop().Get());
        }

        [HttpGet]
        [Route("General/Data/Job_Types")]
        public IActionResult Job_Types_Drop()
        {
            return Ok(new CustomeDrops.Job_Type().Get());
        }

        [HttpGet]
        [Route("General/Data/Login_Types")]
        public IActionResult Login_Types_Drop()
        {
            return Ok(new CustomeDrops.LoginType_Drop().Get());
        }

        [HttpGet]
        [Route("General/Data/Languages")]
        public IActionResult Languages_Drop()
        {
            return Ok(new CustomeDrops.Languages().Get());
        }

        [HttpGet]
        [Route("General/Data/IndustrySector")]
        public IActionResult IndustrySector_Drop()
        {
            return Ok(new CustomeDrops.Industry_Sector_Drop().Get());
        }

        [HttpGet]
        [Route("General/Data/HighestEducation")]
        public IActionResult HighestEducation_Drop()
        {
            return Ok(new CustomeDrops.HighestEducation_Drop().Get());
        }
    }
}
