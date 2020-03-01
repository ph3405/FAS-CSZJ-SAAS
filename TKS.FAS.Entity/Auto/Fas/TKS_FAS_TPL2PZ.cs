using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //模板凭证对应关系
    public class TKS_FAS_TPL2PZ
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
        /// 凭证ID
        /// </summary>		
        public string PZId
        {
            get;
            set;
        }
        /// <summary>
        /// 模板ID
        /// </summary>		
        public string TPLId
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
        /// PeriodId
        /// </summary>		
        public string PeriodId
        {
            get;
            set;
        }
        /// <summary>
        /// CreateUser
        /// </summary>		
        public string CreateUser
        {
            get;
            set;
        }
        /// <summary>
        /// CreateDate
        /// </summary>		
        public DateTime? CreateDate
        {
            get;
            set;
        }

    }
}