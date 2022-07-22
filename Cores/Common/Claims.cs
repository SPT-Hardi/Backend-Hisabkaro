using HIsabKaro.Services;
using HisabKaroContext;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;

namespace HIsabKaro.Cores.Common
{
    public class Claims
    {
        private readonly IConfiguration _configuration;
        private readonly ITokenServices _tokenServices;
        public Claims(IConfiguration configuration, ITokenServices tokenServices)
        {
            _configuration = configuration;
            _tokenServices = tokenServices;
        }
        public Models.Common.Token Add(int UID, string DToken,int? URId)
        {
            using (DBContext c = new DBContext())
            {
                if (UID == 0 || DToken == null)
                {
                    throw new ArgumentException("Not Allowed!");
                }
                var authclaims = new List<Claim>
                    {
                         new Claim(ClaimTypes.Sid,UID.ToString()),
                         new Claim(ClaimTypes.Name,DToken),
                         new Claim(ClaimTypes.Role,URId==null ? "0" : URId.ToString()),
                         new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString ()),
                    };
                var jwtToken = _tokenServices.GenerateAccessToken(authclaims);
                var refreshToken = _tokenServices.GenerateRefreshToken();
                var check = c.SubUserTokens.Where(x => x.UId == UID && x.CommonDeviceToken.DeviceToken == DToken).SingleOrDefault();
                //RefreshToken refreshToken1 = new RefreshToken();
                if (check == null)
                {
                }
                else
                {
                    check.Token = refreshToken;
                    c.SubmitChanges();
                }
                return new Models.Common.Token()
                {
                    JWT = jwtToken,
                    RToken = refreshToken,
                };


            }
        }
    }
        
}
