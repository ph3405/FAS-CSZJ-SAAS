using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public  class ResponseDetailListSearch:ResponseSearchBase<ZB_Detail>
    {
        public List<ZB_DetailHead> PrintData { get; set; }
    }
}
