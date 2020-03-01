using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestCustomDataGet:RequestBase
    {
        public TKS_FAS_CaculateHelperDetail Data { get; set; }
    }
}
