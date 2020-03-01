using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public class TKS_FAS_InvoiceExt:TKS_FAS_Invoice
    {
        public string PZZ { get; set; }

        public string Name { get; set; }
        public string BasicDataName { get; set; }

        public string DataType { get; set; }
    }

    public class InvoiceList
    {
        public TKS_FAS_InvoiceExt Head { get; set; }

        public List<TKS_FAS_InvoiceSFDetail> Detail { get; set; }
    }
}
