using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class ResponseOPAccountListGet : ResponseBase
    {
        public List<TKS_FAS_AccountInfoExt> Data { get; set; }
        public TKS_FAS_AccountInfo SelectAccount { get; set; }

        public string UserCreditCode { get; set; }

        public bool IsSelected { get; set; }
        /// <summary>
        /// 会计期间，年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 会计期间，月
        /// </summary>
        public int Month { get; set; }

        public string PeriodId { get; set; }
    }
}
