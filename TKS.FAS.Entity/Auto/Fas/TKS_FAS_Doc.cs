using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //凭证主表
    public class TKS_FAS_Doc
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
        /// 凭证字
        /// </summary>		
        public string PZZ
        {
            get;
            set;
        }
        /// <summary>
        /// 凭证字号
        /// </summary>		
        public int PZZNO
        {
            get;
            set;
        }
        /// <summary>
        /// 会计期间ID
        /// </summary>		
        public string PeriodId
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
        /// 凭证年份
        /// </summary>		
        public int Year
        {
            get;
            set;
        }
        /// <summary>
        /// 凭证日期
        /// </summary>		
        public DateTime? PZDate
        {
            get;
            set;
        }
        /// <summary>
        /// 附件数量
        /// </summary>		
        public int AppendNum
        {
            get;
            set;
        }
        /// <summary>
        /// 审核人
        /// </summary>		
        public string CheckMan
        {
            get;
            set;
        }
        /// <summary>
        /// 审核标识位
        /// </summary>		
        public int CheckStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 借方金额合计
        /// </summary>		
        public decimal AMT_DBT
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Memo
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
        /// <summary>
        /// CreateUser
        /// </summary>		
        public string UpdateUser
        {
            get;
            set;
        }
        /// <summary>
        /// CreateDate
        /// </summary>		
        public DateTime? UpdateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 来源 ： FP ，LR，JZ
        /// </summary>
        public string Source
        {
            get;set;
        }

    }
}