using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public  class TKS_FAS_ZCFZReport:TKS_FAS_ReportDetailTPL
    {
        /// <summary>
        /// 期末余额
        /// </summary>
        public decimal EndBalance { get; set; }

        /// <summary>
        /// 年初余额
        /// </summary>
        public decimal YearStartBalance { get; set; }
    }
}
