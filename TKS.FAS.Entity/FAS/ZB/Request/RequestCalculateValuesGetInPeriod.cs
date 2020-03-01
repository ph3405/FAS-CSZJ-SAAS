using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestCalculateValuesGetInPeriod : RequestBase
    {
        public string Period_S { get; set; }

        public string Period_E { get; set; }

        public string CalculateItemId { get; set; }
    }
}
