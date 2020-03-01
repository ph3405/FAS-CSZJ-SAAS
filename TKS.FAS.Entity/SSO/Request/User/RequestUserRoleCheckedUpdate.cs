using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO
{
    public class RequestUserRoleCheckedUpdate : RequestBase
    {
        public string UserId { get; set; }

        public List <string> RoleIds { get; set; }
    }
}
