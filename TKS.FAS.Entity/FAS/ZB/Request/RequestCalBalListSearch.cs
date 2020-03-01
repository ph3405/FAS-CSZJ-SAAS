using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    /// <summary>
    /// 核算项目余额表
    /// </summary>
    public class RequestCalBalListSearch : RequestSearchBase
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
        /// normal 一般查询  more 更多查询
        /// </summary>
        public string Type { get; set; }

        public CalBalMore More { get; set; }

    }

    public class CalBalMore
    {
        public string Period_S { get; set; }

        public string Period_E { get; set; }
    }
}
