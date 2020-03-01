using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class RequestAccountListSearch:RequestSearchBase
    {
        public TKS_FAS_AccountInfo Data { get; set; }
    }
    public class RequestBackUpSearch : RequestSearchBase
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
