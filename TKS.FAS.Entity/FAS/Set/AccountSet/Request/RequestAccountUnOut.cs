using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 撤销外包
    /// </summary>
    public class RequestAccountUnOut : RequestBase
    {
        public TKS_FAS_AccountInfo Data { get; set; }
    }
}
