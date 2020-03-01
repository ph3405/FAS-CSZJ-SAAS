using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseMyAccountGet : ResponseBase
    {
        public TKS_FAS_MyAccount Data { get; set; }
    }
}
