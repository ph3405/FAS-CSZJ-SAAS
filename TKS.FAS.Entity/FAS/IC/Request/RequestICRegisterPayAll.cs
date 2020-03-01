using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    /// <summary>
    /// 付全款
    /// </summary>
    public class RequestICRegisterPayAll : RequestBase
    {
        public string OrderNo { get; set; }

        public string Money { get; set; }
    }
}
