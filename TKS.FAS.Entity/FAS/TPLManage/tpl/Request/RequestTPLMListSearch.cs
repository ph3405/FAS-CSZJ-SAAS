using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestTPLMListSearch : RequestSearchBase
    {

        public string Title { get; set; }
        /// <summary>
        /// 模板类型
        /// </summary>
        public int TPLTarget { get; set; }

        /// <summary>
        /// 是否可结转
        /// </summary>
        public int IsCarry { get; set; }


    }
}
