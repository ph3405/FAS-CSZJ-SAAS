using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 经营报表
    /// </summary>
    public  class RequestJYGet:RequestBase
    {
        public string PeriodId { get; set; }
        public string AccountList { get; set; }
        public string AccountId { get; set; }
        public string UserId { get; set; }
    }
}
