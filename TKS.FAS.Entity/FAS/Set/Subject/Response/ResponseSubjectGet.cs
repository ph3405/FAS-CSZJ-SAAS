using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseSubjectGet : ResponseBase
    {
        public TKS_FAS_AccountSubject Data { get; set; }

        public List<TKS_FAS_CurrencyExt> Currency { get; set; }

        public List<TKS_FAS_CaculateHelperItemExt> CalItem { get; set; }
    }
}
