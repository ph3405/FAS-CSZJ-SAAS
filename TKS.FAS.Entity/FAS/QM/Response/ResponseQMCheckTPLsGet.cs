using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 期末检查模板
    /// </summary>
    public class ResponseQMCheckTPLsGet : ResponseBase
    {
        public List<TKS_FAS_DocTPLExt> Data { get; set; }
    }
}
