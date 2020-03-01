using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class RequestDocListSearch:RequestSearchBase
    {
        public string PZZ_S { get; set; }

        public string PZZ_E { get; set; }

        public string PZZ { get; set; }

        public string Period { get; set; }

        /// <summary>
        /// 查询的类型 normal 普通 more 更多
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 账套ID集合字符串，用于打印工具,例(xxx,xxx,xxx)
        /// </summary>
        public string AccountList { get; set; }

        public DocListMore More { get; set; }

    }

    public class DocListMore
    {
        public string PZZ_S { get; set; }

        public string PZZ_E { get; set; }

        public string PZZ { get; set; }

        public string Period_S { get; set; }

        public string Period_E { get; set; }
    }
}
