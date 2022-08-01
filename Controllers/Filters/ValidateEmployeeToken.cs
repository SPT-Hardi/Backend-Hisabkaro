using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateEmployeeToken :Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext c) 
        {
            var UId = c.HttpContext.Items["UId"];
            var DeviceToken = c.HttpContext.Items["DeviceToken"];
            if (UId==null || DeviceToken==null)
            {
                var access_token = c.HttpContext.Request.Headers["Authorization"].ToString();
                if (String.IsNullOrEmpty(access_token))
                {
                    throw new HttpResponseException() { Status = 401, Value = "invalid_token" };
                }
                else
                {
                    throw new HttpResponseException() { Status = 401, Value = "token_expired" };
                }
            }
        }
    }
}
