using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.SSO
{
    public class RequestUserLogin : RequestBase
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public string Mobile { get; set; }
        public string VerCode { get; set; }
    }
}
