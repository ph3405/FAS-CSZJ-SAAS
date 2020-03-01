using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///账户流水
    /// </summary>	
    public class TKS_FAS_Node_AccountDetailList
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
        /// 交易号
        /// </summary>		
        public string TradeNo
        {
            get;
            set;
        }

        /// <summary>
        /// 机构ID
        /// </summary>
        public string NodeId
        {
            get;set;
        }
       
        /// <summary>
        /// 分类   0 账套扣款
        /// </summary>		
        public int Category
        {
            get;
            set;
        }
        /// <summary>
        /// 收费对象
        /// </summary>		
        public string TargetName
        {
            get;
            set;
        }
        /// <summary>
        /// 收费对象ID
        /// </summary>		
        public string TargetId
        {
            get;
            set;
        }
        /// <summary>
        /// 收费标题
        /// </summary>		
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 金额
        /// </summary>		
        public string Money
        {
            get;
            set;
        }
        /// <summary>
        /// 0 出 1 进
        /// </summary>		
        public int Direction
        {
            get;
            set;
        }
        /// <summary>
        /// 1  成功  0  失败
        /// </summary>		
        public int Status
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