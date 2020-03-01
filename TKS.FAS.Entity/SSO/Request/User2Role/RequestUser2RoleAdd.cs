using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO 
{
    public class RequestUser2RoleAdd:RequestBase
    {
        public string RoleId { get; set; }
        public List<string> UserIds { get; set; }
    }
}
