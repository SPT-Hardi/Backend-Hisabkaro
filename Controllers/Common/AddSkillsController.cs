using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace HIsabKaro.Controllers.Common
{
    [Route("AddSkills")]
    [ApiController]
    public class AddSkillsController : ControllerBase
    {
        [HttpPost]
        [Route("{Id}")]
        public IActionResult Post(int Id, Skill_Default value)
        {
            return Ok(new AddSkills().Add(Id, value));
        }

        [HttpGet]
        [Route("Search")]
        public IActionResult Get([FromQuery] string keyword)
        {
            return Ok(new AddSkills().SearchSkillList(keyword));
        }

        [HttpGet]
        [Route("AddJobList")]
        public IActionResult AddJobList()
        {
            return Ok(new AddSkills().AddJobPostName());
        }
    }
}
