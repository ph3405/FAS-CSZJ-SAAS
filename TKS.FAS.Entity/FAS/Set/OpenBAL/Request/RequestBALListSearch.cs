using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class RequestBALListSearch:RequestSearchBase
    {
        public string  Category { get; set; }

        public string CurrencyCode { get; set; }

        public int IsCalHelper { get; set; }
        public string PeriodId { get; set; }
    }
}
