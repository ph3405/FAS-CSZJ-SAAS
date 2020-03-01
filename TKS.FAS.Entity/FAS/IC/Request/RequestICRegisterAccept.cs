using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    /// <summary>
    /// 受理
    /// </summary>
   public class RequestICRegisterAccept:RequestBase
    {
        public string OrderNo { get; set; }
    }
}
