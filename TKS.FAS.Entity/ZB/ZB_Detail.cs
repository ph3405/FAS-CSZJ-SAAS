using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    /// <summary>
    /// 账簿 明细账
    /// </summary>
    public class ZB_Detail : TKS_FAS_GLBalance
    {
        //public string PeriodId { get; set; }
        //public string Year { get; set; }
        public string Month { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime DetailDate { get; set; }

        /// <summary>
        /// 凭证字
        /// </summary>
        public string PZZ { get; set; }



        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary { get; set; }


        #region 借方

        public decimal NUM1 { get; set; }

        public decimal Unit_Price1 { get; set; }

        public decimal Money1 { get; set; }

        #endregion

        #region 贷方

        public decimal NUM2 { get; set; }

        public decimal Unit_Price2 { get; set; }

        public decimal Money2 { get; set; }

        #endregion

        /// <summary>
        /// 余额方向 0 借 1 贷方
        /// </summary>
        public int Show_Credit_Debit { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Show_Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal Show_Unit_Price { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Show_Money { get; set; }

        public decimal StartBAL { get; set; }
    }

    public class ZB_DetailHead
    {
        public string PeriodId { get; set; }
        public string Year { get; set; }

        public string Month { get; set; }
        public List<ZB_Detail> Data { get; set; }
    }
}
