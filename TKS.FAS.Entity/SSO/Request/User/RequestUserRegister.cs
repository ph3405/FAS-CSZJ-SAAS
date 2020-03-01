using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO 
{
    public class RequestUserRegister : RequestBase
    {
        public TKS_FAS_User User{get;set;}

        public TKS_FAS_Node Node { get; set; }
    }
}
