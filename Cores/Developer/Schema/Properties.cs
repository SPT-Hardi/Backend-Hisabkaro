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
                    var Ids = Cores.Common.Context.Current.Ids;
                    if (Ids is object)
                    {
                        return c.dev_fn_sc_Controller_Property_Value(ControllerName, PropertyName, Ids.UId, Ids.RId, Ids.OId);
                    }
                    else
                    {
                        return c.dev_fn_sc_Controller_Property_Value(ControllerName, PropertyName, 0, 0, 0);
                    }
                }
            }
        }
    }
}
