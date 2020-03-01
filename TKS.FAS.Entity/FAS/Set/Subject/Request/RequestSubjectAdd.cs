using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestSubjectAdd:RequestBase
    {
        public TKS_FAS_AccountSubject Data { get; set; }

        public List<string> CalItem { get; set; }

        public List<string> Currency { get; set; }

        public string checkSub { get; set; }
    }
}
