using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 明细账
    /// </summary>
    public class RequestDetailListSearch : RequestSearchBase
    {
        /// <summary>
        /// 期间
        /// </summary>
        public string PeriodId { get; set; }

        /// <summary>
        /// 科目代码
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// normal 一般查询 more 更多查询
        /// </summary>
        public string Type { get; set; }

        public DetailMore More { get; set; }
        /// <summary>
        /// 账套ID集合字符串，用于打印工具,例(xxx,xxx,xxx)
        /// </summary>
        public string AccountList { get; set; }
        public string allPZ { get; set; }
    }

    public class DetailMore
    {
        public string Period_S { get; set; }

        public string Period_E { get; set; }

        public string Code { get; set; }

        public string Code_S { get; set; }

        public string Code_E { get; set; }
    }
}
