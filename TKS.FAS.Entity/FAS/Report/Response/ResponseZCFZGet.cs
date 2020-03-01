using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public  class ResponseZCFZGet:ResponseBase
    {
       
        public List<TKS_FAS_ZCFZReport> FLDZC { get; set; }
        public List<TKS_FAS_ZCFZReport> LDZC { get; set; }
        public List<TKS_FAS_ZCFZReport> SYZQYHJ { get; set; }
        public List<TKS_FAS_ZCFZReport> SYZQY { get; set; }
        public List<TKS_FAS_ZCFZReport> ZCHJ { get; set; }
        public List<TKS_FAS_ZCFZReport> FZHJ { get; set; }
        public List<TKS_FAS_ZCFZReport> FLDFZ { get; set; }
        public List<TKS_FAS_ZCFZReport> LDFZ { get; set; }
    }

    public class ZCFZGetData
    {
        public string Year { get; set; }
        public string Month { get; set; }
        public ResponseZCFZGet Data { get; set; }
    }
    public class PrintZCFZGet:ResponseBase
    {
        public List<ZCFZGetData> PrintData { get; set; }
    }
}

