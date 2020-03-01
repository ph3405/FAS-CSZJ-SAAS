using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class TKS_FAS_FixedAssetsChangeExt : TKS_FAS_FixedAssetsChange
    {
        /// <summary>
        /// 变动期间
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public string TPLId { get; set; }

        /// <summary>
        /// 录入当期是否折旧
        /// </summary>
        public int IsStartPeriodDepreciation { get; set; }

        /// <summary>
        /// 增加方式
        /// </summary>
        public string AddType { get; set; }

        /// <summary>
        /// 是否处置
        /// </summary>
        public string ISCHUZHI { get; set; }
    }
}
