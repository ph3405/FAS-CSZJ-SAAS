using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseDocGet : ResponseBase
    {
        public TKS_FAS_Doc Head { get; set; }

        public List<TKS_FAS_DocDetail> Detail { get; set; }
    }
}
