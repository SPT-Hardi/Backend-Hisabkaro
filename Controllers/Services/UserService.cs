using System;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;


namespace HIsabKaro.Controllers.Services
{
    /*public interface IUserService
    {
        TeamIN.Core.Models.Common.Result Authenticate(LogonRequest model);
        TeamIN.Core.Models.Common.Ids GetById(int UId);
    }*/

    public class UserService /*: IUserService*/
    {
       /* private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }*/

      /*  public TeamIN.Core.Models.Common.Result Authenticate(LogonRequest model)
        {
            var r = new TeamIN.Core.Common.Subscriber.Authentication().Validate(model.username, model.password, model.scope);

            if (r.Status == TeamIN.Core.Models.Common.Result.ResultStatus.success)
            {
                var token = generateJwtToken(r.Data.UId);

                var data = new TeamIN.Core.Models.Common.Subscriber.Authentication.LogonResponse()
                {
                    access_token = token,
                    client_id = model.client_id,
                    loginType = r.Data.LoginType,
                    guid = r.Data.UGUID,
                    refresh_token = "",
                    token_type = "JWT"
                };

                var result = new TeamIN.Core.Models.Common.Result()
                {
                    Message = "Login successfull!",
                    Status = TeamIN.Core.Models.Common.Result.ResultStatus.success,
                    Data = data
                };
                return result;
            }
            else
            {
                return r;
            }
        }*/

        public Models.Common.Ids GetByUId(int UId)
        {
            return new Cores.Common.Subscriber.Users.User().ForUIdIds(UId);
        }
        public Models.Common.Ids GetByURId(int URId)
        {
            return new Cores.Common.Subscriber.Users.User().ForURIdIds(URId);
        }
        // helper methods

        /* private string generateJwtToken(int UId)
         {
             // generate token that is valid for 7 days
             var tokenHandler = new JwtSecurityTokenHandler();
             var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
             var tokenDescriptor = new SecurityTokenDescriptor
             {
                 Subject = new ClaimsIdentity(new[] { new Claim("UId", UId.ToString()) }),
                 Expires = DateTime.UtcNow.AddDays(7),
                 SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
             };
             var token = tokenHandler.CreateToken(tokenDescriptor);
             return tokenHandler.WriteToken(token);
         }*/
    }
}
