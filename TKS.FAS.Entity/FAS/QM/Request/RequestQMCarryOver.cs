using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestQMCarryOver:RequestBase
    {
        /// <summary>
        /// 结转生产成本科目对应的科目
        /// </summary>
        public string SCCBCode { get; set; }
    }
}
