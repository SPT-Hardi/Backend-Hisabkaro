using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Common
{
    public class ProfileDrop
    {
        public List<ProfileDropOrg> Profiles { get; set; } = new List<ProfileDropOrg>(); 
    }
    public class ProfileDropOrg 
    {
        public string OrgName { get; set; }
        public List<IntegerNullString> Orgrolelist { get; set; } = new List<IntegerNullString>();
    }
}
