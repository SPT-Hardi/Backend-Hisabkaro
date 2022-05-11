using HIsabKaro.Services;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Common
{
    public class Tokens
    {
        private readonly ITokenServices _tokenService;
        private readonly IConfiguration _configuration;

        public Tokens(ITokenServices tokenService, IConfiguration configuration) 
        {
            _tokenService = tokenService;
            _configuration = configuration;
        }
        public Models.Common.Token RefreshToken(int Id, Models.Common.Token value) 
        {
            string token = value.JWT;
            string refreshToken = value.RToken;
            using (DBContext c = new DBContext())
            {
                if (token is null)
                {
                    throw new ArgumentException("Token Not Found.");
                }
                if (refreshToken is null)
                {
                    throw new ArgumentException("RefreshToken Not Found.");
                }
                var principal = _tokenService.GetPrincipalFromExpiredToken(token);
                //int URID = int.Parse(principal.Claims.First(x => x.Type == ClaimTypes.Role).Value);
                int UserID = int.Parse(principal.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
                string DeviceToken = principal.Claims.First(x => x.Type == ClaimTypes.Name).Value;
               
                var userrefreshtoken = c.SubUserTokens.Where(x => x.Token == refreshToken && x.UId == UserID && x.DeviceToken==DeviceToken).SingleOrDefault();


                if (userrefreshtoken == null)
                    throw new ArgumentException("Bad Request!");
                var claim = new Claims(_configuration, _tokenService);
                var res =claim.Add(UserID.ToString(), DeviceToken, Id.ToString());
                //var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
                //var newRefreshToken = _tokenService.GenerateRefreshToken();

                if (res.JWT is null)
                {
                    throw new ArgumentException("Error While Generating New Token");
                }
                if (res.RToken is null)
                {
                    throw new ArgumentException("Error While Generating New Refresh Token");
                }
                return new Models.Common.Token()
                {
                    JWT = res.JWT,
                    RToken = res.RToken,

                };
            }
        }
    }
}
