using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestCurrencyGet:RequestBase
    {
        public TKS_FAS_Currency Data { get; set; }
    }
}
