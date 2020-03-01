using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestFixedAssetsDelete : RequestBase
    {
        public TKS_FAS_FixedAssets Data { get; set; }
    }
}
