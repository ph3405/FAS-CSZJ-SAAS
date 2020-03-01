using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public class RequestKMBalListSearch:RequestSearchBase
    {
        public string PeriodId { get; set; }

        public string Type { get; set; }

        public KMBalListMore More { get; set; }

        public string AccountList { get; set; }
        public string allPZ { get; set; }
        public string IsFuZhu { get; set; }
    }

    public class KMBalListMore {
        public string Period_S { get; set; }

        public string Period_E { get; set; }

        public string Code_S { get; set; }

        public string Code_E { get; set; }
    }
}
