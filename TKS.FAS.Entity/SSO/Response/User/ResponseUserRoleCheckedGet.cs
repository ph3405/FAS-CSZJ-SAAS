using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO 
{
    public  class ResponseUserRoleCheckedGet:ResponseBase
    {
        public List<TKS_FAS_RoleExt> PTRoles { get; set; }


        /// <summary>
        /// 企业角色
        /// </summary>
        public List<TKS_FAS_RoleExt> QYRoles { get; set; }
    }
}
