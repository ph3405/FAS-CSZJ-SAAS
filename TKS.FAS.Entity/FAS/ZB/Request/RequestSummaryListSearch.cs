using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    /// <summary>
    /// 科目汇总表
    /// </summary>
    public class RequestSummaryListSearch:RequestSearchBase
    {
        /// <summary>
        /// 期间
        /// </summary>
        public string PeriodId { get; set; }

        /// <summary>
        /// normal 一般查询 more 更多查询
        /// </summary>
        public string Type { get; set; }

        public SummaryMore More { get; set; }
        public string allPZ { get; set; }
    }

    public class SummaryMore
    {
        public string Period_S { get; set; }

        public string Period_E { get; set; }

        public string Code_S { get; set; }

        public string Code_E { get; set; }
    }
}
