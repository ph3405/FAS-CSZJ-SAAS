using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public class ResponseInvoiceCX:ResponseBase
    {
        /// <summary>
        /// 相对路径
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 图片原名称
        /// </summary>
        public string Title { get; set; }
    }
}
