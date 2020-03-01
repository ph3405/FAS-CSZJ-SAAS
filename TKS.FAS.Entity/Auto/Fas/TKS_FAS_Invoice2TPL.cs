using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///发票选项对应的模板
    /// </summary>	
    public class TKS_FAS_Invoice2TPL
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
        /// TPLID
        /// </summary>		
        public string TPLID
        {
            get;
            set;
        }
        /// <summary>
        /// 发票类型
        /// </summary>		
        public int Type
        {
            get;
            set;
        }
        /// <summary>
        /// 是否增票 0 否 1 是
        /// </summary>		
        public int IsVAT
        {
            get;
            set;
        }
        /// <summary>
        /// 收付状态 0 未收付 1  已收付
        /// </summary>		
        public int RPStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 支付方式 0 现金 1 转账
        /// </summary>		
        public int PayMode
        {
            get;
            set;
        }

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
        /// <summary>
        /// UpdateUser
        /// </summary>		
        public string UpdateUser
        {
            get;
            set;
        }
        /// <summary>
        /// UpdateDate
        /// </summary>		
        public DateTime? UpdateDate
        {
            get;
            set;
        }

        public string AccountId
        {
            get;set;
        }

    }
}