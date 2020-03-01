using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 接受外包
    /// </summary>
    public class RequestAccountAcceptOut : RequestBase
    {
        public TKS_FAS_AccountInfo Data { get; set; }
    }
}
