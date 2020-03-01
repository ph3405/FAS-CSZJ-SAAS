using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //用户当前选择的账套，由于一个账套可能被多个用户看到，所以激活的状态需要从表记录
    public class TKS_FAS_UserCurrentAccount
    {

        /// <summary>
        /// Id
        /// </summary>		
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// 账套ID
        /// </summary>		
        public string AccountId
        {
            get;
            set;
        }
        /// <summary>
        /// 用户ID
        /// </summary>		
        public string UserId
        {
            get;
            set;
        }

    }
}