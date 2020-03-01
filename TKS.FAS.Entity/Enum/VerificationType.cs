using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public enum VerificationType
    {
        /// <summary>
        /// 客户端注册
        /// </summary>
        C_Regist,

        /// <summary>
        /// 微信小程序登录注册
        /// </summary>
        WX_Regist,
        /// <summary>
        /// 邀请码
        /// </summary>
        Invitation
    }
}
