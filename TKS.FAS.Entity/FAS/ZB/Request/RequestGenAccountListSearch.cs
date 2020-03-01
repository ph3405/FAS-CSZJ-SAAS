using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 总帐
    /// </summary>
    public class RequestGenAccountListSearch:RequestSearchBase
    {
        public string PeriodId { get; set; }

        /// <summary>
        /// normal 一般查询 more 更多查询
        /// </summary>
        public string Type { get; set; }

        public GenAccountMore More { get; set; }
        public string AccountList { get; set; }
    }

    public class GenAccountMore
    {
        public string Period_S { get; set; }

        public string Period_E { get; set; }

        public string Code_S { get; set; }

        public string Code_E { get; set; }

    }
}
