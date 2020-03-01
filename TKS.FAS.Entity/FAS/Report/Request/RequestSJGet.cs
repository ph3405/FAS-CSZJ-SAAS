using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 税金报表
    /// </summary>
    public class RequestSJGet:RequestBase
    {
        public string AccountList { get; set; }

        public string AccountId { get; set; }

        public string UserId { get; set; }

        public string PeriodId { get; set; }
    }
}
