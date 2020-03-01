using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 财务立账
    /// </summary>
   public class RequestInvoiceLZ:RequestBase
    {
        public TKS_FAS_Invoice Data { get; set; }
    }
}
