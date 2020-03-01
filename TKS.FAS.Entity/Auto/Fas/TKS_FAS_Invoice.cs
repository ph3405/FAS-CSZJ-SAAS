using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //发票管理
    public class TKS_FAS_Invoice
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
            get;set;
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
        /// 发票日期
        /// </summary>		
        public string InvoiceDate
        {
            get;
            set;
        }
        /// <summary>
        /// 发票编号
        /// </summary>		
        public string InvoiceNo
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
        /// 是否增值税 0 否 1是
        /// </summary>		
        public int IsVAT
        {
            get;
            set;
        }
        /// <summary>
        /// 收付状态  0 未收付 1  已收付
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
        /// <summary>
        /// 发票张数
        /// </summary>		
        public int InvoiceNum
        {
            get;
            set;
        }
        /// <summary>
        /// 发票金额（含税）
        /// </summary>		
        public decimal Money
        {
            get;
            set;
        }
        /// <summary>
        /// 税金金额
        /// </summary>		
        public decimal TaxMoney
        {
            get;
            set;
        }
        /// <summary>
        /// 发票状态 0 草稿  1  递交财务  2  财务立账
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
        /// 备注
        /// </summary>		
        public string Memo
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用税金预知功能，0不启用，1启用
        /// </summary>
        public string IsTaxYZ
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用应收应付管理功能，0不启用，1启用
        /// </summary>
        public string IsUse
        {
            get;
            set;
        }
        /// <summary>
        /// 收付类型（应收/应付）
        /// </summary>
        public string SFType
        {
            get;
            set;
        }
        /// <summary>
        /// 客户或供应商ID
        /// </summary>
        public string BasicDataId
        {
            get;
            set;
        }
        /// <summary>
        /// 坏账
        /// </summary>
        public decimal BadMoney
        {
            get;
            set;
        }
    }

}