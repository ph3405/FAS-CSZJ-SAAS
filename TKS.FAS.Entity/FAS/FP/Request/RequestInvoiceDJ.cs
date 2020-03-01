using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 递交财务
    /// </summary>
   public class RequestInvoiceDJ:RequestBase
    {
        public TKS_FAS_Invoice Data { get; set; }
        public string Id { get; set; }
        public string UserId { get; set; }
    }
}
