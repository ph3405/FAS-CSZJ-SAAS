using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO 
{
    public class RequestUser2RoleListSearch :RequestSearchBase
    {
        public string UserName { get; set; }

        public string TrueName { get; set; }
        public string RoleId { get; set; }
    }
}
