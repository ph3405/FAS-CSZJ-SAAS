using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class ResponseInvoiceGet:ResponseBase
    {
        public TKS_FAS_Invoice Data { get; set; }
        public List<TKS_FAS_InvoiceAttach> lstAttach { get; set; }
        public List<TKS_FAS_InvoiceSFDetail> lstSFDetail { get; set; }
    }
}
