using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class RequestSubjectListSearch:RequestSearchBase
    {
        public TKS_FAS_AccountSubject Data { get; set; }
    }
}
