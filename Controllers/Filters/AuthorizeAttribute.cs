using HIsabKaro.Models.Common;
using Microsoft.AspNetCore.Mvc.Filters;
using System;


namespace HIsabKaro.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticateAttribute : Attribute, IAuthorizationFilter
    {

        private string _cn;

        public string cn
        {
            get { return _cn; }
            set { _cn = value; }
        }


        public AuthenticateAttribute(string cn)
        {
            _cn = cn;
        }

        public void OnAuthorization(AuthorizationFilterContext c)
        {
            var isAuthenticated = (Ids)c.HttpContext.Items["Ids"];

            var exists = new HIsabKaro.Cores.Developer.Schema.Controllers().One(_cn);
            if (exists is null)
            {
                throw new HttpResponseException() { Status = 404, Value = "Not found" };
            }

            if (exists.Status == false)
            {
                throw new HttpResponseException() { Status = 503, Value = "Service unavailable!" };
            }

            if (exists.NeedLogin)
            {
                if (isAuthenticated == null)
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
            Cores.Common.Contact.Current.SetCId(exists.CId);
            if (exists.NeedLogin == true)
            {
                if (!HIsabKaro.Cores.Common.Contact.Current.IsAuthorised(_cn))
                {
                    throw new HttpResponseException() { Status = 403, Value = "Access to this feature is denied!" };
                }
            }
        }
    }
}
