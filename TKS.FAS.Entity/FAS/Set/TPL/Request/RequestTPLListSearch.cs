using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestTPLListSearch : RequestSearchBase
    {
        /// <summary>
        /// 模板类型
        /// </summary>
        public string Type { get; set; }
    }
}
