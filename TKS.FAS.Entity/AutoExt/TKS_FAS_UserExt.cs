using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public  class TKS_FAS_UserExt:TKS_FAS_User
    {
        public string NodeId { get; set; }

        public string NodeName { get; set; }

        public string NodeType { get; set; }
    }
}
