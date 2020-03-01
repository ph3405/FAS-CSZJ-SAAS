using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO 
{
    public class ResponseRoleGet:ResponseBase
    {
        public RoleInfo Data { get; set; }
        //public List<TKS_FAS_Role2PermissionExt> lstRole2Permission { get; set; }
    }

    public class RoleInfo
    {
        public TKS_FAS_Role Data { get; set; }
        public List<TKS_FAS_Role2PermissionExt> lstRole2Permission { get; set; }
    }
}
