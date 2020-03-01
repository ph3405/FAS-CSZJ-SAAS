using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO
{
    /// <summary>
    /// 账户充值
    /// </summary>
    public class RequestNodeAccountAdd : RequestBase
    {
        public decimal Money { get; set; }
    }
}
