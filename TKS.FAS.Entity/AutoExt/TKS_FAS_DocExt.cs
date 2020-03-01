using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class TKS_FAS_DocExt : TKS_FAS_Doc
    {
        public string PZZName { get; set; }
        public string RowNum { get; set; }
        public string AttachmentCount { get; set; }
    }


    public class DocList
    {
        public TKS_FAS_DocExt Head { get; set; }

        public List<TKS_FAS_DocDetail> Detail { get; set; }
    }
}
