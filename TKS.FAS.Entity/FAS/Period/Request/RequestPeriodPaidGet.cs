using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 获取已经结转的年月
    /// </summary>
    public class RequestPeriodPaidGet : RequestBase
    {
        public string AccountId { get; set; }
    }
}
