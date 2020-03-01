using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseCalculateValuesGetInPeriod : ResponseBase
    {
        public List<CodeValue> Data { get; set; }
    }
}
