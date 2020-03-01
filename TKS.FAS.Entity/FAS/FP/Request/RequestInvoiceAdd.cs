using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class RequestInvoiceAdd:RequestBase
    {
        public TKS_FAS_Invoice Data { get; set; }
        public string AccountId  { get; set; }
        public string UserId { get; set; }
        public string Detail { get; set; }
        public string SFDetail { get; set; }
        public string InvoiceInfo { get; set; }
        public string Ids { get; set; }
    }

    public class InvoiceAttach
    {
        public string Title { get; set; }
        public string Path { get; set; }
    }

    public class SFDetail
    {
        public string SFDate { get; set; }
        public string SFMoney { get; set; }
        public string SFRemark { get; set; }
        public string SFStatus { get; set; }
        
        public string Seq { get; set; }
    }
}
