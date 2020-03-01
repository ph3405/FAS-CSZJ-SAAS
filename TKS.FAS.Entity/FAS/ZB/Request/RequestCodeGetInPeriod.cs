using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestCodeGetInPeriod:RequestBase
    {
        public string PeriodS { get; set; }

        public string PeriodE { get; set; }
    }
}
