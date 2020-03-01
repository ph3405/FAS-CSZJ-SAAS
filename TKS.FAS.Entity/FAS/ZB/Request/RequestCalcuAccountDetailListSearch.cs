using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 核算项目明细账
    /// </summary>
    public class RequestCalcuAccountDetailListSearch:RequestSearchBase
    {
        /// <summary>
        /// 期间
        /// </summary>
        public string PeriodId { get; set; }

        /// <summary>
        /// 核算项
        /// </summary>
        public string CalculateItem { get; set; }

        /// <summary>
        /// 核算项值
        /// </summary>
        public string CalculateValue { get; set; }

        public string Type { get; set; }

        public CalDetailMore More { get; set; }
        public string allPZ { get; set; }
    }

    public class CalDetailMore
    {
        public string Period_S { get; set; }

        public string Period_E { get; set; }
    }
}
