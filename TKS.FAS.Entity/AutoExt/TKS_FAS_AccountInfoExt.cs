using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class TKS_FAS_AccountInfoExt : TKS_FAS_AccountInfo
    {
        /// <summary>
        /// 1:激活，0：未激活
        /// </summary>
        public int Active { get; set; }

        /// <summary>
        /// 制单员真实姓名
        /// </summary>
        public string TrueName { get; set; }

     
        public string SupplierName { get; set; }

        /// <summary>
        /// 是否查询者创建 0 否  1是
        /// </summary>
        public string CreateByMe { get; set; }
    }
}
