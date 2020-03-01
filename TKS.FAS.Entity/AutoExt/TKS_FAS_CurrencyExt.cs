using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class TKS_FAS_CurrencyExt : TKS_FAS_Currency
    {
        /// <summary>
        /// 是否被选中，1是，0否
        /// </summary>
        public int IsChecked { get; set; }
    }
}
