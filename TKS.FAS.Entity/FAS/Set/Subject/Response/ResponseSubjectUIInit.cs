using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseSubjectUIInit:ResponseBase
    {
        /// <summary>
        /// 币制
        /// </summary>
        public List<TKS_FAS_CurrencyExt> Currency { get; set; }

        /// <summary>
        /// 辅助科目
        /// </summary>
        public List<TKS_FAS_CaculateHelperItemExt> CalItem { get; set; }


        public TKS_FAS_AccountSubject ParentSubject { get; set; }
    }
}
