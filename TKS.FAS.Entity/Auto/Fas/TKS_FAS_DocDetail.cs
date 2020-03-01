using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //TKS_FAS_DocDetail
    public class TKS_FAS_DocDetail
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
        /// 会计期间
        /// </summary>
        public string PeriodId
        {
            get;
            set;
        }

       
        /// <summary>
        /// 行号
        /// </summary>		
        public int LineNo
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
        /// 年份
        /// </summary>		
        public int Year
        {
            get;
            set;
        }
        /// <summary>
        /// 科目代码
        /// </summary>		
        public string SubjectCode
        {
            get;
            set;
        }
        /// <summary>
        /// 科目描述
        /// </summary>		
        public string SubjectDescription
        {
            get;
            set;
        }
        /// <summary>
        /// 摘要
        /// </summary>		
        public string Summary
        {
            get;
            set;
        }
        /// <summary>
        /// 余额方向，0,借方，1贷方
        /// </summary>		
        public int Credit_Debit
        {
            get;
            set;
        }
        /// <summary>
        /// 借方金额
        /// </summary>		
        public decimal Money_Debit
        {
            get;
            set;
        }
        /// <summary>
        /// 贷方金额
        /// </summary>		
        public decimal Money_Credit
        {
            get;
            set;
        }
        /// <summary>
        /// 业务伙伴，（客户）代码
        /// </summary>		
        public string PartnerCode
        {
            get;
            set;
        }
        /// <summary>
        /// 计量单位
        /// </summary>		
        public string Unit
        {
            get;
            set;
        }
        /// <summary>
        /// 货币代码
        /// </summary>		
        public string CurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 核算项1
        /// </summary>		
        public string CalItem1
        {
            get;
            set;
        }
        /// <summary>
        /// 核算项1的值
        /// </summary>		
        public string CalValue1
        {
            get;
            set;
        }
        /// <summary>
        /// CalItem2
        /// </summary>		
        public string CalItem2
        {
            get;
            set;
        }
        /// <summary>
        /// CalValue2
        /// </summary>		
        public string CalValue2
        {
            get;
            set;
        }
        /// <summary>
        /// CalItem3
        /// </summary>		
        public string CalItem3
        {
            get;
            set;
        }
        /// <summary>
        /// CalValue3
        /// </summary>		
        public string CalValue3
        {
            get;
            set;
        }
        /// <summary>
        /// CalItem4
        /// </summary>		
        public string CalItem4
        {
            get;
            set;
        }
        /// <summary>
        /// CalValue4
        /// </summary>		
        public string CalValue4
        {
            get;
            set;
        }
        /// <summary>
        /// CalItem5
        /// </summary>		
        public string CalItem5
        {
            get;
            set;
        }
        /// <summary>
        /// CalValue5
        /// </summary>		
        public string CalValue5
        {
            get;
            set;
        }
        /// <summary>
        /// 汇率
        /// </summary>		
        public decimal Rate
        {
            get;
            set;
        }
        /// <summary>
        /// 数量
        /// </summary>		
        public decimal Quantity
        {
            get;
            set;
        }
        /// <summary>
        /// 单价
        /// </summary>		
        public decimal Price
        {
            get;
            set;
        }
        /// <summary>
        /// 是否设置外币
        /// </summary>		
        public int IsCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// IsQuantity
        /// </summary>		
        public int IsQuantity
        {
            get;
            set;
        }
        /// <summary>
        /// IsCalHelper
        /// </summary>		
        public int IsCalHelper
        {
            get;
            set;
        }
        /// <summary>
        /// 余额
        /// </summary>		
        public decimal Balance
        {
            get;
            set;
        }
        /// <summary>
        /// 原币
        /// </summary>
        public int YB
        {
            get; set;
        }
       
        
       
        

        public string Source { get; set; }
        public int Seq { get; set; }
        public decimal StartBAL
        {
            get;
            set;
        }
        public decimal EndBAL
        {
            get;
            set;
        }
    }
}