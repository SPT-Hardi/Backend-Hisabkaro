using HisaabKaro.Services;
using HisabKaroDBContext;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HisaabKaro.Cores.Common
{
    public class Tokens
    {
        private readonly ITokenServices _tokenService;

        public Tokens(ITokenServices tokenService) 
        {
            _tokenService = tokenService;
        }
        public Models.Common.Token RefreshToken(Models.Common.Token value) 
        {
            string token = value.JWT;
            string refreshToken = value.RToken;
            using (HisabKaroDBDataContext context = new HisabKaroDBDataContext())
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
                var RoleID = principal.Claims.First(x => x.Type == ClaimTypes.Role).Value;
                var UserID = principal.Claims.First(x => x.Type == ClaimTypes.Sid).Value;
                var user = context.SubUserTokens.Where(x => x.UId.ToString() == UserID).SingleOrDefault();
                if (user == null)
                {
                    throw new ArgumentException("Bad Request!!");
                }
                var userrefreshtoken = context.SubUserTokens.Where(x => x.Token == refreshToken && x.UId.ToString() == UserID).SingleOrDefault();


                if (user == null || userrefreshtoken == null)
                    throw new ArgumentException("Bad Request!");

                var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                if (newJwtToken is null)
                {
                    throw new ArgumentException("Error While Generating New Token");
                }
                if (newRefreshToken is null)
                {
                    throw new ArgumentException("Error While Generating New Refresh Token");
                }
                return new Models.Common.Token()
                {
                    JWT = newJwtToken,
                    RToken = newRefreshToken
                };
            }
        }
    }
}
