using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class TKS_FAS_CustomColDesExt:TKS_FAS_CustomColDes
    {
        /// <summary>
        /// 对应的明细的实际值
        /// </summary>
        public string Value { get; set; }
    }
}
