using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 税金报表
    /// </summary>
    public class ResponseSJGet : ResponseBase
    {
        /// <summary>
        /// 账套
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 本月销项税
        /// </summary>
        public SubjectMoney XXTax { get; set; }

        /// <summary>
        /// 未入账销项税
        /// </summary>
        public SubjectMoney UnCal_XXTax { get; set; }
        /// <summary>
        /// 销项税合计
        /// </summary>
        public SubjectMoney XXTax_TOTAL { get; set; }
        /// <summary>
        /// 本月进项税
        /// </summary>
        public SubjectMoney JXTax { get; set; }

        /// <summary>
        /// 未入账进项税
        /// </summary>
        public SubjectMoney UnCal_JXTax { get; set; }
        public SubjectMoney JXTax_TOTAL { get; set; }

        public SubjectMoney Pre_ZZTax { get; set; }
        /// <summary>
        /// 留抵
        /// </summary>
        public SubjectMoney Pre_LiuDi { get; set; }

        /// <summary>
        /// 本月应交地方税金
        /// </summary>
        public SubjectMoney LocalSJ { get; set; }

        /// <summary>
        /// 本年已入账的累计应交所得税
        /// </summary>
        public SubjectMoney Cal_TotalSDTax { get; set; }

        /// <summary>
        /// 本月税金合计
        /// </summary>
        public SubjectMoney TotalTax { get; set; }
        /// <summary>
        /// 本年累计增值税合计
        /// </summary>
        public SubjectMoney TotalZZTax { get; set; }
        /// <summary>
        /// 本年累计地税合计
        /// </summary>
        public SubjectMoney TotalLocalSJ { get; set; }

        /// <summary>
        /// 本年累计已交税金合计
        /// </summary>
        public SubjectMoney TotalYearTax { get; set; }
        
        /// <summary>
        /// 本年累计已交所得税
        /// </summary>
        public SubjectMoney Total_Deliver_SDTax { get; set; }

        /// <summary>
        /// 本月预计应交所得税
        /// </summary>
        public SubjectMoney Pre_SDTax { get; set; }
        /// <summary>
        /// 本季度已交所得税
        /// </summary>
        public SubjectMoney Quarter_TotalSDTax { get; set; }
        /// <summary>
        /// 本季度已交增值税
        /// </summary>
        public SubjectMoney Quarter_VaTax { get; set; }
        /// <summary>
        /// 本季度已交地方税金
        /// </summary>
        public SubjectMoney Quarter_LocalSJ { get; set; }
    }
}
