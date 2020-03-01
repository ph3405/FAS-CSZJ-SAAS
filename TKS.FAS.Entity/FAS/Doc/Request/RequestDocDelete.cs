using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestDocDelete : RequestBase
    {
        public TKS_FAS_Doc Data { get; set; }
        public string TKS_FAS_FixedAssetsChange_Id { get; set; }
        /// <summary>
        /// 固定资产ID
        /// </summary>
        public string TKS_FAS_FixedAssets_Id { get; set; }
    }
}
