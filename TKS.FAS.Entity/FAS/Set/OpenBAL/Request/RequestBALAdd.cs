using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 辅助核算期初
    /// </summary>
    public class RequestBALAdd:RequestBase
    {
        public TKS_FAS_FGLBalanceExt Data { get; set; }
        public List<TKS_FAS_FGLBalance> lst { get; set; }
    }
}
