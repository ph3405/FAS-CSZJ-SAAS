using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public class MM_UserInfo
    {
        public string AccountId { get; set; }

      
        public TKS_FAS_User User { get; set; }

        public TKS_FAS_Node Node { get; set; }

        public List<TKS_FAS_Role> Roles { get; set; }
    }
}
