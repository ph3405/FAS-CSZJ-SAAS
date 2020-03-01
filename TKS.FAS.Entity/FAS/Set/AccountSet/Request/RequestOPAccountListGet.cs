using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 获取可以操作的账套
    /// </summary>
    public class RequestOPAccountListGet : RequestBase
    {
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}
