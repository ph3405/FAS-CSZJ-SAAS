using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 快速凭证模板
    /// </summary>
    public class ResponseKSPZTPLsGet : ResponseBase
    {
        public List<TKS_FAS_DocTPLExt> Data { get; set; }
    }
}
