using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO
{
    public class ResponseUserMenusGet : ResponseBase
    {
        public List<TKS_FAS_MenuItem> Data { get; set; }
        public string FuncId { get; set; }
    }
}
