using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class RequestNewsSearch : RequestSearchBase
    {
        public string Title { get; set; }
        public string Type { get; set; }
    }
}
