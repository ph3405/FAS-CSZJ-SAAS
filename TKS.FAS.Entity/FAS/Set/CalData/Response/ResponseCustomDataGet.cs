using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseCustomDataGet : ResponseBase
    {
        public TKS_FAS_CaculateHelperItem Head { get; set; }

        public List<TKS_FAS_CustomColDesExt> CustomDes { get; set; }

        public TKS_FAS_CaculateHelperDetail Data { get; set; }
     
    }
}
