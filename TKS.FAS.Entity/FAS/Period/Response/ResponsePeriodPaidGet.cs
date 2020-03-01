using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponsePeriodPaidGet : ResponseBase
    {
        public TKS_FAS_AccountInfo Account { get; set; }
        public List<TKS_FAS_MonthPeriodInfo> Data { get; set; }
    }
}
