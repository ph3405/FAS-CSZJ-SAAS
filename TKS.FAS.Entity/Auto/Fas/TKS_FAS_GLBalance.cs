using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //余额表
    public class TKS_FAS_GLBalance
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
        /// AccountId
        /// </summary>		
        public string AccountId
        {
            get;
            set;
        }

        /// <summary>
        /// 科目ID
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 科目代码
        /// </summary>		
        public string SubjectCode
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
        /// Name
        /// </summary>		
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 期间代码
        /// </summary>		
        public string PeriodId
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
        /// 货币代码
        /// </summary>		
        public string CurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 期初余额方向，0,借方，1贷方
        /// </summary>		
        public int SCredit_Debit
        {
            get;
            set;
        }
        /// <summary>
        /// 期初余额，数量
        /// </summary>		
        public decimal NUMStartBAL
        {
            get;
            set;
        }
        /// <summary>
        /// 期初余额，本位币
        /// </summary>		
        public decimal BWBStartBAL
        {
            get;
            set;
        }
        /// <summary>
        /// 期初余额，原币
        /// </summary>		
        public decimal YBStartBAL
        {
            get;
            set;
        }
        /// <summary>
        /// 借方累计，数量
        /// </summary>		
        public decimal NUMDebitTotal
        {
            get;
            set;
        }
        /// <summary>
        /// 年度借方累计，数量
        /// </summary>		
        public decimal NUMDebitTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// 借方累计，本位币
        /// </summary>		
        public decimal BWBDebitTotal
        {
            get;
            set;
        }
        /// <summary>
        /// 年度借方余额，本位币
        /// </summary>		
        public decimal BWBDebitTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// 借方累计，原币
        /// </summary>		
        public decimal YBDebitTotal
        {
            get;
            set;
        }
        /// <summary>
        /// YBDebitTotal_Y
        /// </summary>		
        public decimal YBDebitTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// 贷方累计，数量
        /// </summary>		
        public decimal NUMCreditTotal
        {
            get;
            set;
        }
        /// <summary>
        /// 年度贷方累计，数量
        /// </summary>		
        public decimal NUMCreditTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// 贷方累计，本位币
        /// </summary>		
        public decimal BWBCreditTotal
        {
            get;
            set;
        }
        /// <summary>
        /// 年度贷方累计,本位币
        /// </summary>		
        public decimal BWBCreditTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// 贷方累计，原币
        /// </summary>		
        public decimal YBCreditTotal
        {
            get;
            set;
        }
        /// <summary>
        /// 年度贷方累计，原币
        /// </summary>		
        public decimal YBCreditTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// 期末余额方向
        /// </summary>		
        public int ECredit_Debit
        {
            get;
            set;
        }
        /// <summary>
        /// 期末余额，数量
        /// </summary>		
        public decimal NUMEndBAL
        {
            get;
            set;
        }
        /// <summary>
        /// 期末余额，本位币
        /// </summary>		
        public decimal BWBEndBAL
        {
            get;
            set;
        }
        /// <summary>
        /// 期末余额，原币
        /// </summary>		
        public decimal YBEndBAL
        {
            get;
            set;
        }
        /// <summary>
        /// Category
        /// </summary>		
        public string Category
        {
            get;
            set;
        }
        /// <summary>
        /// 计量单位代码
        /// </summary>		
        public string Unit
        {
            get;
            set;
        }
        /// <summary>
        /// 年初余额数量
        /// </summary>		
        public decimal YearStartNumBAL
        {
            get;
            set;
        }
        /// <summary>
        /// 年初余额，原币
        /// </summary>		
        public decimal YearStartYBBAL
        {
            get;
            set;
        }
        /// <summary>
        /// 年初余额，本位币
        /// </summary>		
        public decimal YearStartBWBBAL
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
        /// 是否启用了辅助核算
        /// </summary>
        public int IsCalHelperValid { get; set; }

        /// <summary>
        /// 是否启用了数量
        /// </summary>
        public int IsQuantityValid { get; set; }

        /// <summary>
        /// 是否是本位币
        /// </summary>
        public int IsDefaultCurrency { get; set; }

        /// <summary>
        /// 是否叶子节点
        /// </summary>
        public int IsLeaf { get; set; }

      

    }
}