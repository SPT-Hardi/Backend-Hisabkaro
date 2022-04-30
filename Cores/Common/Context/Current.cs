using HisabKaroDBContext;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Common.Context
{
    public class Current
    {
        private static IHttpContextAccessor _HttpContextAccessor;

        public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        {
            _HttpContextAccessor = accessor;
        }
        public static HttpContext httpContext
        {
            get
            {
                return _HttpContextAccessor.HttpContext;
            }
        }
        public static Models.Common.Ids Ids
        {
            get
            {
                return (Models.Common.Ids)httpContext.Items["Ids"];
            }
        }
        public static bool IsLoggedIn
        {
            get
            {
                return Ids == null ? false : true;
            }
        }
        public static bool SetCId(int? CId)
        {
            Models.Common.Ids CurrentIds = httpContext.Items["Ids"] as Models.Common.Ids;
            CurrentIds.CId = CId;
            httpContext.Items["Ids"] = CurrentIds;
            return true;
        }
        public static bool IsAuthorised(string Con, string ParentCon = null)
        {
            using (var c = new DBContext())
            {
                var cc = Ids;
                if (c.IsAuthorised(Con, ParentCon, cc.UId, cc.RId, cc.OId, cc.LId) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



    }
}
