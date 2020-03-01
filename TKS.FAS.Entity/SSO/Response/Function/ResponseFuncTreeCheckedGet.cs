using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO 
{
    public class ResponseFuncTreeCheckedGet:ResponseBase
    {
        public List<TKS_FAS_TreeItem> Data { get; set; }
    }
}
