using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponsePZZTotalGet : ResponseBase
    {
        public List<TKS_FAS_CertificateWord> Data { get; set; }

        /// <summary>
        /// 凭证字号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 当前期间的最后一天
        /// </summary>
        public string DefaultDate { get; set; }

        /// <summary>
        /// 核算单位
        /// </summary>
        public string AccountName { get; set; }
    }
}
