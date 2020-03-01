using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    /// <summary>
    /// 预付款
    /// </summary>
    public class RequestICRegisterPrePay : RequestBase
    {
        public string OrderNo { get; set; }

        public decimal Money { get; set; }
    }
}
