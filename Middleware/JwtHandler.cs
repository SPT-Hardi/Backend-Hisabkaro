using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HIsabKaro.Middleware
{
    public class JwtHandler
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtHandler(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext c)
        {
            var token = c.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                getUserDataFromToken(c, token);
            }
            await _next(c);
        }

        private void getUserDataFromToken(HttpContext c, string token)
        {
            try
            {
                var tokenhandler = new JwtSecurityTokenHandler();
                tokenhandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("Thisismysecretkey")),
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                }, out SecurityToken validatedToken
                   );
                var jwtToken = (JwtSecurityToken)validatedToken;
               
                string DeviceToken = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                c.Items["DeviceToken"] = DeviceToken;
                int UserID = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.Sid).Value);
                c.Items["UserID"] = UserID;
                int URId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value);
                c.Items["URId"] = URId;

            }
            catch (Exception)
            { }
        }
    }
}
