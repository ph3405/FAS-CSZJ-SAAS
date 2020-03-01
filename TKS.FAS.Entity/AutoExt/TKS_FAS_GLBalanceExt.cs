using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class TKS_FAS_GLBalanceExt : TKS_FAS_GLBalance
    {
        /// <summary>
        /// 1 基础科目  2  自定义的辅助科目，允许删除
        /// </summary>
        public int Type { get; set; }


        public decimal YearStartBWBBAL_J { get; set; }

        public decimal YearStartBWBBAL_D { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal BWBEndBAL_J { get; set; }

        public decimal BWBEndBAL_D { get; set; }
        public decimal BWBStartBAL_J { get; set; }

        public decimal BWBStartBAL_D { get; set; }

        /// <summary>
        /// credit debit 
        /// </summary>
        public string CD { get; set; }

        public int SLevel { get; set; }
        public string RootCode { get; set; }
        public string ParentCode { get; set; }
        public string CalItem1 { get; set; }
        public string CalValue1 { get; set; }
        public string IsHelper { get; set; }
        public string calCode { get; set; }
        public string calName { get; set; }
    }
}
