using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO 
{
    public  class ResponseUserGet:ResponseBase
    {
        public TKS_FAS_User Data { get; set; }
    }
}
