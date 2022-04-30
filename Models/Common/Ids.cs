using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIsabKaro.Models.Common
{
    public class Ids
    {
        public Ids()
        {
            _UGUId = Guid.NewGuid();
            _RequestId = Guid.NewGuid();
        }

        public int? UId { get; set; }
        public int RId { get; set; }
        public int OId { get; set; }

        private Guid _UGUId;

        public Guid UGUId
        {
            get
            {
                return _UGUId;
            }

            set
            {
                _UGUId = value;
            }
        }

        private Guid _RequestId;

        public Guid RequestId
        {
            get
            {
                return _RequestId;
            }

            set
            {
                _RequestId = value;
            }
        }

        public int LId { get; set; }
        public int? CId { get; set; }
    
}
}
