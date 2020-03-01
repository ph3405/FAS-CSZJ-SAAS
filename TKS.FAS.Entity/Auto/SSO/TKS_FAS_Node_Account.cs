using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace TKS.FAS.Entity
{
    /// <summary>
    ///用户账户，挂在所属机构下
    /// </summary>	
    public class TKS_FAS_Node_Account
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
        /// 机构ID
        /// </summary>		
        public string NodeId
        {
            get;
            set;
        }
        /// <summary>
        /// 账户余额
        /// </summary>		
        public decimal Balance
        {
            get;
            set;
        }

    }

  
}