using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class ResponseLRGet : ResponseBase
    {
        public List<TKS_FAS_LRReport> Data { get; set; }
        public List<LRGetData> PrintData { get; set; }
    }
    public class LRGetData
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public List<TKS_FAS_LRReport> Data { get; set; }
    }
}
