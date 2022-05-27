using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common
{
    [Route("AddSkills")]
    [ApiController]
    public class AddSkillsController : ControllerBase
    {
        [HttpPost]
        [Route("{Id}")]
        public IActionResult Post(int Id,Skills value )
        {
            return Ok(new AddSkills().Add(Id,value));
        }

        [HttpGet]
        [Route("Search/{keyword}")]
        public IActionResult Get(string keyword)
        {
            return Ok(new AddSkills().SearchSkillList(keyword));
        }
    }
}
