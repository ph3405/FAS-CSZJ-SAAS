using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO
{
    public class ResponseUserLogin : ResponseBase
    {
        public string Token { get; set; }

        public string UserName { get; set; }
        public string TrueName { get; set; }

        public string Sex { get; set; }

        public string Id { get; set; }

        public string NodeName { get; set; }
        
    }

    public class ResponseUserLogin_WX : ResponseBase
    {
        public string Token { get; set; }
        public TKS_FAS_User user { get; set; }

    }
}
