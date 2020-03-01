using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestDocAdd:RequestBase
    {
        public TKS_FAS_Doc Head { get; set; }

        public List<TKS_FAS_DocDetail> Detail { get; set; }

        /// <summary>
        /// 来源类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public string TPLId { get; set; }

        /// <summary>
        /// 通用Key
        /// </summary>
        public string Key { get; set; }
    }
}
