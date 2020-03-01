using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseInvoiceLZ : ResponseBase
    {
        /// <summary>
        /// 含税金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 税金金额
        /// </summary>
        public decimal Tax { get; set; }
        /// <summary>
        /// 获取的模板ID
        /// </summary>
        public string TPLId { get; set; }
    }
}
