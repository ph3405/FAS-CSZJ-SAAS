using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class RequestCustomDataListSearch:RequestSearchBase
    {
        public TKS_FAS_CaculateHelperItem Head { get; set; }

        public TKS_FAS_CaculateHelperDetail Data { get; set; }
    }
}
