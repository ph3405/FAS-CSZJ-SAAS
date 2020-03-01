using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestTPLMGet:RequestBase
    {
        public TKS_FAS_DocTPL Data { get; set; }

        /// <summary>
        /// 填充的金额
        /// </summary>
        public decimal Money { get; set; }

        public string Key { get; set; }

        /// <summary>
        /// 税金 发票专用
        /// </summary>
        public decimal Tax { get; set; }
    }
}
