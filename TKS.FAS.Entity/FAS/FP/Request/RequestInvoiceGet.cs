using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public  class RequestInvoiceGet:RequestBase
    {
        public TKS_FAS_Invoice Data { get; set; }
        public string Id { get; set; }
    }
}
