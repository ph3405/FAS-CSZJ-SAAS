using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    ///  
    /// </summary>
    public class RequestFormulaListSearch : RequestSearchBase
    {
        /// <summary>
        /// 模板列ID
        /// </summary>
        public string ReportDetailTPLId { get; set; }
    }
}
