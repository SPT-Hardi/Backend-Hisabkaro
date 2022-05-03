using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Developer.Schema
{
    public class Properties
    {
        public class Property 
        {
            public string Value(string ControllerName, string PropertyName)
            {
                using (var c = new DBContext())
                {
                    var Ids = Common.Contact.Current.Ids;
                        return (from x in c.DevControllers where x.ControllerName == ControllerName select 
                                
                                PropertyName == "URL" ? x.URL :
                                PropertyName == "SqlCommand" ? x.SqlCommand :
                                PropertyName == "SqlCommandKeys" ? x.SqlCommandKeys :
                                PropertyName == "SqlCommandOrderBy" ? x.SqlCommandOrderBy :
                                PropertyName == "DisplayText" ? x.DisplayText :
                                PropertyName == "EditController" ? x.EditController
                                : ""
                                
                                ).FirstOrDefault();
                }
            }
        }
    }
}
