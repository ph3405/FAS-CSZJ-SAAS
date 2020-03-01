using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///账套
    /// </summary>	
    public class TKS_FAS_AccountInfo
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
        /// QY_Name
        /// </summary>		
        public string QY_Name
        {
            get;
            set;
        }
        /// <summary>
        /// 社会信用代码
        /// </summary>		
        public string QY_CreditCode
        {
            get;
            set;
        }
        /// <summary>
        /// QY_Address
        /// </summary>		
        public string QY_Address
        {
            get;
            set;
        }
        /// <summary>
        /// 行业类型
        /// </summary>		
        public string QY_Type
        {
            get;
            set;
        }
        /// <summary>
        /// 代帐信用代码
        /// </summary>		
        public string DZ_CreditCode
        {
            get;
            set;
        }
        /// <summary>
        /// DL_CreditCode
        /// </summary>		
        public string DL_CreditCode
        {
            get;
            set;
        }
        /// <summary>
        /// WB_CreditCode
        /// </summary>		
        public string WB_CreditCode
        {
            get;
            set;
        }
        /// <summary>
        /// WB_Status
        /// </summary>		
        public int WB_Status
        {
            get;
            set;
        }
        /// <summary>
        /// 会计ID
        /// </summary>		
        public string AccountantId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否有效账套
        /// </summary>		
        public int IsActive
        {
            get;
            set;
        }
        /// <summary>
        /// 账套启用年月
        /// </summary>		
        public DateTime? StartYearMonth
        {
            get;
            set;
        }
        /// <summary>
        /// 是否外包
        /// </summary>		
        public int IsOutSource
        {
            get;
            set;
        }
        /// <summary>
        /// 会计准则，1：小企业会计准则 2：企业会计准则
        /// </summary>		
        public int AccountantRule
        {
            get;
            set;
        }
        /// <summary>
        /// 增值税类型，1：小规模纳税人 2：一般纳税人
        /// </summary>		
        public int AddedValueTaxType
        {
            get;
            set;
        }
        /// <summary>
        /// 是否需要审核，1：需要 0：不需要
        /// </summary>		
        public int IsNeedReviewed
        {
            get;
            set;
        }
        /// <summary>
        /// 邀请码
        /// </summary>		
        public string InvitationCode
        {
            get;
            set;
        }
        /// <summary>
        /// TaxRate
        /// </summary>		
        public decimal TaxRate
        {
            get;
            set;
        }
        public decimal LandTax
        {
            get;
            set;
        }
        /// <summary>
        /// IsInvitation
        /// </summary>	
        public int IsInvitation
        {
            get;
            set;
        }
        /// <summary>
        /// BillTarget
        /// </summary>		
        public string BillTarget
        {
            get;
            set;
        }
        /// <summary>
        /// InvitationQYCode
        /// </summary>		
        public string InvitationQYCode
        {
            get;
            set;
        }
        /// <summary>
        /// InvitationQYName
        /// </summary>		
        public string InvitationQYName
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

    }
}