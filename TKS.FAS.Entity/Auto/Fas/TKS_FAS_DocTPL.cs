using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //凭证主表模板
    public class TKS_FAS_DocTPL
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
        /// Title
        /// </summary>		
        public string Title
        {
            get;
            set;
        }

        public int Type { get; set; }
        /// <summary>
        /// 账套ID
        /// </summary>		
        public string AccountId
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
        /// <summary>
        /// 是否自定义 0 否 1 是
        /// </summary>
        public int IsCustom
        {
            get;set;
        }

        /// <summary>
        /// 模板用处 1  快速凭证  2 发票上传  3 固定资产 4 期末检查  5 期间费用结转 6 利润结转 7 固定资产变动类型
        /// </summary>
        public int TPLTarget { get; set; }


        /// <summary>
        /// 是否结转类型 0 否 1 是
        /// </summary>
        public int IsCarry { get; set; }


    }
}