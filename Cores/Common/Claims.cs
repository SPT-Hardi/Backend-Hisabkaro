using HisaabKaro.Services;
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

namespace HisaabKaro.Cores.Common
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
        public Models.Common.Token Add(string UID, string DToken)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
                {
                    var authclaims = new List<Claim>
                        {
                     new Claim(ClaimTypes.Sid,UID.ToString()),
                     new Claim(ClaimTypes.Name,DToken.ToString()),
                     new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString ()),
                         };
                    var jwtToken = _tokenServices.GenerateAccessToken(authclaims);
                    var refreshToken = _tokenServices.GenerateRefreshToken();

                    var check = context.SubUserTokens.Where(x => x.UId.ToString() == UID && x.DeviceToken == DToken).SingleOrDefault();

                    //RefreshToken refreshToken1 = new RefreshToken();
                    if (check == null)
                    {
                        
                    }
                    else
                    {
                        check.Token = refreshToken;
                        context.SubmitChanges();
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
