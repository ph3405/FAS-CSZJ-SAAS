using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 结转生产成本
    /// </summary>
    public class RequestSCCBGen : RequestBase
    {
        /// <summary>
        /// 生产成本科目
        /// </summary>
        public string SCCBCode { get; set; }
    }
}
