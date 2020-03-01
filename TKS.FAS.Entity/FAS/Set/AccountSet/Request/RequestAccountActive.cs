using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestAccountActive : RequestBase
    {
        public TKS_FAS_AccountInfo Data { get; set; }
        public string AccountId { get; set; }
    }
}
