using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 发票附件
    /// </summary>
    public class RequestFPFJAdd:RequestBase
    {
        public TKS_FAS_InvoiceAttach Data { get; set; }
 
    }
}
