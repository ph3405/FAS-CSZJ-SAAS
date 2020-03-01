using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //TKS_FAS_Token
    public class TKS_FAS_Token
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
        /// Token
        /// </summary>		
        public string Token
        {
            get;
            set;
        }
        /// <summary>
        /// UserId
        /// </summary>		
        public string UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 激活时间
        /// </summary>		
        public DateTime? ActiveTime
        {
            get;
            set;
        }
        public string Source
        {
            get;
            set;
        }
    }
}