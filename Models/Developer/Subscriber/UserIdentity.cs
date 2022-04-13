using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HisaabKaro.Models.Developer.Subscriber
{
    public class UserIdentity
    {
        
        public int UserIdentityId { get; set; }
        public string PanNumber { get; set; }
        public int PanFileId { get; set; }
        public string AadharNumber { get; set; }
        public int AadharFrontFileId { get; set; }
        public int AadharBackFileId { get; set; }
        
    }
}
