using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public class TKS_FAS_TreeItem
    {
        public string id { get; set; }
        public string text { get; set; }

        public string state { get; set; }

        public bool @checked {get;set;}

        public string checkState { get; set; }
        public List<TKS_FAS_TreeItem> children { get; set; }
    }
}
