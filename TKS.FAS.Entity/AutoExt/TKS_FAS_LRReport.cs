using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    /// <summary>
    /// 利润表
    /// </summary>
    public class TKS_FAS_LRReport:TKS_FAS_ReportDetailTPL
    {
        public decimal Money_Year { get; set;}

        public decimal Money_Month { get; set; }

    }
}
