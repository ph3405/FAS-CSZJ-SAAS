using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestInvoiceUpdate : RequestBase
    {
        public TKS_FAS_Invoice Data { get; set; }
        public string AccountId { get; set; }
        public string UserId { get; set; }
        public string Detail { get; set; }
        public string SFDetail { get; set; }
        
        public string InvoiceInfo { get; set; }
        public string Id { get; set; }
    }
}
