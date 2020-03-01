using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestCalHelperAdd:RequestBase
    {
        public TKS_FAS_CaculateHelperItem Head { get; set; }

        public List<TKS_FAS_CustomColDes> CustomDes { get; set; }
    }
}
