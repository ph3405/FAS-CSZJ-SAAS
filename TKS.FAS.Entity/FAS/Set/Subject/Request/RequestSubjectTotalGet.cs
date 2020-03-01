using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestSubjectTotalGet : RequestBase
    {
        public string Code { get; set; }

        public string Category { get; set; }
        public string ShowTitle { get; set; }
    }
}
