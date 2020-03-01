using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //科目辅助表
    public class TKS_FAS_SubjectAss
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
        /// ParentId
        /// </summary>		
        public string ParentId
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
        /// Code
        /// </summary>		
        public string Code
        {
            get;
            set;
        }
        /// <summary>
        /// Value
        /// </summary>		
        public string Value
        {
            get;
            set;
        }

        /// <summary>
        /// 1币别，2辅助核算
        /// </summary>
        public int Type
        {
            get;set;
        }

    }
}