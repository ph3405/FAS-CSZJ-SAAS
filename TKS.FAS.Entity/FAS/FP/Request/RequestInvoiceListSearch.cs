using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 发票查询
    /// </summary>
    public   class RequestInvoiceListSearch:RequestSearchBase
    {
        public string Status { get; set; }
        public string AccountId { get; set; }
        public string IsAll { get; set; }
        public string InvoiceNo { get; set; }
        public string BasicDataName { get; set; }
        public string StartSFDate { get; set; }
        public string EndSFDate { get; set; }
    }
}
