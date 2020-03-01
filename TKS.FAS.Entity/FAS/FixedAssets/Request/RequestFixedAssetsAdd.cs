using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 固定资产新增
    /// </summary>
    public class RequestFixedAssetsAdd : RequestBase
    {
        public TKS_FAS_FixedAssets Data { get; set; }
    }
}
