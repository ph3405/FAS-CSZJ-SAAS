using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 发票界面下拉框联动基础数据获取
    /// </summary>
    public class RequestInvoiceDataGet:RequestBase
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 发票类型
        /// </summary>
        public decimal  FPType { get; set; }

        /// <summary>
        /// 是否增票
        /// </summary>
        public decimal IsVAT { get; set; }

        /// <summary>
        /// 收付状态
        /// </summary>
        public decimal  RPStatus { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public decimal PayMode { get; set; }

        public string AccountId { get; set; }
    }
}
