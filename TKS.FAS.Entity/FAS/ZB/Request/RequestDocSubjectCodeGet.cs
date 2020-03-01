using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 获取凭证使用到的科目代码
    /// </summary>
    public class RequestDocSubjectCodeGet : RequestBase
    {
        /// <summary>
        /// 期间
        /// </summary>
        public string PeriodId { get; set; }
    }
}
