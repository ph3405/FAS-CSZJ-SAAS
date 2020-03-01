using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
   public class TKS_FAS_FixedAssetsExt :TKS_FAS_FixedAssets
    {
        /// <summary>
        /// 资产净值
        /// </summary>
        public decimal ZCJZ { get; set; }
        public decimal Amount { get; set; }
    }
}
