using HIsabKaro.Models.Common;
using HisabKaroDBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Cores.Developer.Schema
{
    public class Controllers
    {
        public Models.Developer.Schema.Controller One(int id)
        {
            using (var c = new DBContext())
            {
                var q = (from x in c.DevControllers
                         where x.CId == id
                         select new Models.Developer.Schema.Controller()
                         {
                             CId = x.CId,
                             ControllerName = x.ControllerName,
                             ControllerType = new IntegerString() { Id = x.ControllerTypeId, Text = x.DevControllerType.ControllerType },
                             ParentController = new IntegerNullString() { Id = x.ParentControllerId, Text = x.DevController_ParentControllerId.ControllerName },
                             Status = x.Status,
                             IsVisible = x.IsVisible,
                             NeedAuthorisation = x.NeedAuthorisation,
                             NeedParentAuthorisation = x.NeedParentAuthorisation,
                             NeedLogin = x.NeedLogin,
                             LoginType = new IntegerNullString() { Id = x.LoginTypeId, Text = x.SubFixedLookup.FixedLookup },
                             Ordinal = x.Ordinal,
                             DisplayText = x.DisplayText,
                             URL = x.URL,
                             SqlCommand = x.SqlCommand,
                             SqlCommandKeys = x.SqlCommandKeys,
                             EditController = x.EditController,
                             CreateController = x.CreateController,
                             SqlCommandOrderBy = x.SqlCommandOrderBy,
                             ChildControllers = x.ChildControllers,
                             ControlController = x.ControlController,

                         }).SingleOrDefault();
                return q;
            }
        }

        public Models.Developer.Schema.Controller One(string con)
        {
            using (var c = new DBContext())
            {
                var q = (from x in c.DevControllers
                         where x.ControllerName == con
                         select new Models.Developer.Schema.Controller()
                         {
                             CId = x.CId,
                             ControllerName = x.ControllerName,
                             ControllerType = new IntegerString() { Id = x.ControllerTypeId, Text = x.DevControllerType.ControllerType },
                             ParentController = new IntegerNullString() { Id = x.ParentControllerId, Text = x.DevController_ParentControllerId.ControllerName },
                             Status = x.Status,
                             IsVisible = x.IsVisible,
                             NeedAuthorisation = x.NeedAuthorisation,
                             NeedParentAuthorisation = x.NeedParentAuthorisation,
                             NeedLogin = x.NeedLogin,
                             LoginType = new IntegerNullString() { Id = x.LoginTypeId, Text = x.SubFixedLookup.FixedLookup },
                             Ordinal = x.Ordinal,
                             DisplayText = x.DisplayText,
                             URL = x.URL,
                             SqlCommand = x.SqlCommand,
                             SqlCommandKeys = x.SqlCommandKeys,
                             EditController = x.EditController,
                             CreateController = x.CreateController,
                             SqlCommandOrderBy = x.SqlCommandOrderBy,
                             ChildControllers = x.ChildControllers,
                             ControlController = x.ControlController,

                         }).SingleOrDefault();
                return q;
            }
        }
    }
}
