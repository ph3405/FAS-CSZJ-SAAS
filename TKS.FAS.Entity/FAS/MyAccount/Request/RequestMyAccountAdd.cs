using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestMyAccountAdd:RequestBase
    {
        public string InvitationCode { get; set; }
        public string UserId { get; set; }
    }
}
