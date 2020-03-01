using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public class RequestInvoiceCX:RequestBase
    {
        public TKS_FAS_Invoice Data { get; set; }
    }
}
