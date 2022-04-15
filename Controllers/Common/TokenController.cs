using HIsabKaro.Cores.Common;
using HIsabKaro.Models.Common;
using HIsabKaro.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Common
{
    [Microsoft.AspNetCore.Components.Route("Token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenServices _tokenService;

        public TokenController(ITokenServices tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(Token value)
        {
            return Ok(new Tokens(_tokenService).RefreshToken(value));   
        }

        /*[Authorize]
        [HttpPost("Revoke")]
        public async Task<IActionResult> Revoke()
        {
            using (ProductInventoryDataContext _context = new ProductInventoryDataContext())
            {
                var emailaddress = User.Identity.Name;

                var user = _context.Users.SingleOrDefault(u => u.EmailAddress == emailaddress);
                if (user == null)
                    return BadRequest();

                var _user = _context.UserRefreshTokens.SingleOrDefault(id => id.UserID == user.UserID);
                var Token = _context.RefreshTokens.SingleOrDefault(id => id.RefreshID == _user.RefreshID);

                Token.RToken = null;
                _context.SubmitChanges();

                return NoContent();
            }
        }*/
        }
}
