using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.Auto.Fas
{
    /// <summary>
    /// OCR配置表
    /// </summary>
    public class TKS_FAS_OCRInfor
    {
        public int id { get; set; }
        public string host { get; set; }
        public string path { get; set; }
        public string method { get; set; }
        public string appcode { get; set; }
        public string appsecret { get; set; }
        public string imgpath { get; set; }
    }
}
