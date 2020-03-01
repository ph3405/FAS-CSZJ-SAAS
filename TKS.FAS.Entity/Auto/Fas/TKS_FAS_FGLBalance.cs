using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //辅助核算余额表
    public class TKS_FAS_FGLBalance
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
        /// 所属科目代码
        /// </summary>		
        public string ParentId
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
        /// SubjectCode
        /// </summary>		
        public string SubjectCode
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
        /// PeriodId
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
        /// CalItem1
        /// </summary>		
        public string CalItem1
        {
            get;
            set;
        }
        /// <summary>
        /// CalValue1
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
        /// 货币代码
        /// </summary>		
        public string CurrencyCode
        {
            get;
            set;
        }
        /// <summary>
        /// 期初余额方向
        /// </summary>		
        public int SCredit_Debit
        {
            get;
            set;
        }
        /// <summary>
        /// NUMStartBAL
        /// </summary>		
        public decimal NUMStartBAL
        {
            get;
            set;
        }
        /// <summary>
        /// BWBStartBAL
        /// </summary>		
        public decimal BWBStartBAL
        {
            get;
            set;
        }
        /// <summary>
        /// 期初余额,原币
        /// </summary>		
        public decimal YBStartBAL
        {
            get;
            set;
        }
        /// <summary>
        /// NUMDebitTotal
        /// </summary>		
        public decimal NUMDebitTotal
        {
            get;
            set;
        }
        /// <summary>
        /// NUMDebitTotal_Y
        /// </summary>		
        public decimal NUMDebitTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// BWBDebitTotal
        /// </summary>		
        public decimal BWBDebitTotal
        {
            get;
            set;
        }
        /// <summary>
        /// BWBDebitTotal_Y
        /// </summary>		
        public decimal BWBDebitTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// 外币本期借方累计
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
        /// NUMCreditTotal
        /// </summary>		
        public decimal NUMCreditTotal
        {
            get;
            set;
        }
        /// <summary>
        /// NUMCreditTotal_Y
        /// </summary>		
        public decimal NUMCreditTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// BWBCreditTotal
        /// </summary>		
        public decimal BWBCreditTotal
        {
            get;
            set;
        }
        /// <summary>
        /// BWBCreditTotal_Y
        /// </summary>		
        public decimal BWBCreditTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// YBCreditTotal
        /// </summary>		
        public decimal YBCreditTotal
        {
            get;
            set;
        }
        /// <summary>
        /// YBCreditTotal_Y
        /// </summary>		
        public decimal YBCreditTotal_Y
        {
            get;
            set;
        }
        /// <summary>
        /// ECredit_Debit
        /// </summary>		
        public int ECredit_Debit
        {
            get;
            set;
        }
        /// <summary>
        /// NUMEndBAL
        /// </summary>		
        public decimal NUMEndBAL
        {
            get;
            set;
        }
        /// <summary>
        /// BWBEndBAL
        /// </summary>		
        public decimal BWBEndBAL
        {
            get;
            set;
        }
        /// <summary>
        /// YBEndBAL
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
        /// Unit
        /// </summary>		
        public string Unit
        {
            get;
            set;
        }
        /// <summary>
        /// YearStartNumBAL
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
        /// 年初余额本位币
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
        /// 是否是本位币
        /// </summary>		
        public int IsDefaultCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// IsQuantityValid
        /// </summary>		
        public int IsQuantityValid
        {
            get;
            set;
        }

        /// <summary>
        /// 是否是期初录入 0否  1 是
        /// </summary>
        public int IsFirstPeriodAdd { get; set; }
    }
}