using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class ResponseKMBalListSearch : ResponseSearchBase<ZB_KMBAL>
    {
        //public List<ZB_KMBALHead> PrintData { get; set; }
        public List<ZB_KMBAL> PrintData { get; set; }
    }
}
