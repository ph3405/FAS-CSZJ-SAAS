using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseDocListSearch : ResponseSearchBase<DocList>
    {
        /// <summary>
        /// 核算单位
        /// </summary>
        public string AccountName { get; set; }
    }

    public class ResponseDocListAttachment: ResponseBase
    {
        public List<string> ImgUrl { get; set; }
    }
}
