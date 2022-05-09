using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common
{
    [Microsoft.AspNetCore.Components.Route("Token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenServices _tokenService;
        private readonly IConfiguration _configuration;

        public TokenController(ITokenServices tokenService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("Refresh/{Id}")]
        public async  Task<IActionResult> RefreshURId(int URId,Token value)
        {
            return Ok(new Tokens(_tokenService,_configuration).RefreshToken(URId,value));   
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshUId(Token value)
        {
            var URId = 0;
            return Ok(new Tokens(_tokenService, _configuration).RefreshToken(URId, value));
        }

        /*[Authorize]
        [HttpPost("Revoke")]
        public async Task<IActionResult> Revoke()
        {
            using (ProductInventoryDataContext _c new ProductInventoryDataContext())
            {
                var emailaddress = User.Identity.Name;

                var user = _c.Users.SingleOrDefault(u => u.EmailAddress == emailaddress);
                if (user == null)
                    return BadRequest();

                var _user = _c.UserRefreshTokens.SingleOrDefault(id => id.UserID == user.UserID);
                var Token = _c.RefreshTokens.SingleOrDefault(id => id.RefreshID == _user.RefreshID);

                Token.RToken = null;
                _c.SubmitChanges();

                return NoContent();
            }
        }*/
    }
}
