using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 获取指定核算项的可选项集合
    /// </summary>
    public class RequestCalculateValuesGet:RequestBase
    {
        /// <summary>
        /// 期间ID
        /// </summary>
        public string PeriodId { get; set; }
        /// <summary>
        /// 核算项ID
        /// </summary>
        public string CalculateItemId { get; set; }
    }
}
