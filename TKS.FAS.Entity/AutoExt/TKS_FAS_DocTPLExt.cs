using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public  class TKS_FAS_DocTPLExt:TKS_FAS_DocTPL
    {
        /// <summary>
        /// 是否已经生成凭证 0 否 1是
        /// </summary>
        public int IsGenPZ { get; set; }

        /// <summary>
        /// 凭证ID
        /// </summary>
        public string PZId { get; set; }

        /// <summary>
        /// 显示金额
        /// </summary>
        public decimal Money { get; set; }
    }
}
