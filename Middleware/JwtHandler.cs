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

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                getUserDataFromToken(context, token);
            }
            await _next(context);
        }

        private void getUserDataFromToken(HttpContext context, string token)
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
                context.Items["DeviceToken"] = DeviceToken;
                string UserID = jwtToken.Claims.First(x => x.Type == ClaimTypes.Sid).Value;
                context.Items["UserID"] = UserID;

            }
            catch (Exception)
            { }
        }
    }
}
