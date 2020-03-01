using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 固定资产更新
    /// </summary>
    public class RequestFixedAssetsUpdate:RequestBase
    {
        /// <summary>
        /// 更新的类型
        /// </summary>
        public string Type { get; set; }
        public TKS_FAS_FixedAssets Data { get; set; }
    }
}
