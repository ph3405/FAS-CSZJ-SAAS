using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseInvoiceDataGet : ResponseBase
    {
        public List <TKS_FAS_DATA> Data { get; set; }
    }
}
