using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public enum PZType
    {
        /// <summary>
        /// 期末检查
        /// </summary>
        QM,

        /// <summary>
        /// 发票
        /// </summary>
        FP,
        /// <summary>
        /// 固定资产变动类型
        /// </summary>
        CHANGE,

        /// <summary>
        /// 固定资产，计提折旧
        /// </summary>
        GD
    }
}
