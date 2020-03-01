using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseAccountGet : ResponseBase
    {
        public TKS_FAS_AccountInfoExt Data { get; set; }
    }
}
