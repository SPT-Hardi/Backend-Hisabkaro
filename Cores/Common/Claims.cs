using HIsabKaro.Services;
using HisabKaroDBContext;
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
        public Models.Common.Token Add(int UID, string DToken,int URId)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                using (DBContext c = new DBContext())
                {
                    var authclaims = new List<Claim>
                        {
                     new Claim(ClaimTypes.Sid,UID.ToString()),
                     new Claim(ClaimTypes.Name,DToken.ToString()),
                     new Claim(ClaimTypes.Role,URId==0 ? "null" : URId.ToString()),
                     new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString ()),
                         };
                    var jwtToken = _tokenServices.GenerateAccessToken(authclaims);
                    var refreshToken = _tokenServices.GenerateRefreshToken();

                    var check = c.SubUserTokens.Where(x => x.UId == UID && x.DeviceToken == DToken).SingleOrDefault();

                    //RefreshToken refreshToken1 = new RefreshToken();
                    if (check == null)
                    {
                        
                    }
                    else
                    {
                        check.Token = refreshToken;
                        c.SubmitChanges();
                        scope.Complete();
                       
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
}
