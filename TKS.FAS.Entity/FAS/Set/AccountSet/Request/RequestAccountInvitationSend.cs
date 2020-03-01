using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 邀请码发送
    /// </summary>
    public class RequestAccountInvitationSend:RequestBase
    {
        /// <summary>
        /// 邀请码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 企业称呼
        /// </summary>
        public string QYName { get; set; }
    }
}
