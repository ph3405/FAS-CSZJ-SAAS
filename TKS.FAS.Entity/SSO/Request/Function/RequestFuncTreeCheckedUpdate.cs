using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO 
{
    public class RequestFuncTreeCheckedUpdate:RequestBase
    {
        public string RoleId { get; set; }
        public List<TKS_FAS_TreeItem> Data { get; set; }
    }
}
