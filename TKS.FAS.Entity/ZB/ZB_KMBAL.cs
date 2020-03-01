using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    /// <summary>
    /// 科目余额表
    /// </summary>
    public class ZB_KMBAL
    {
        public string IsCal { get; set; }
        public string calCode { get; set; }
        public string CalItem { get; set; }
        public string CalItem1 { get; set; }
        public string CalItem2 { get; set; }
        public string CalItem3 { get; set; }
        public string CalItem4 { get; set; }
        public string CalItem5 { get; set; }
        public string PeriodId { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public decimal NumStart_J { get; set; }

     

        public decimal BWBStart_J { get; set; }

        public decimal NumStart_D { get; set; }
        public decimal BWBStart_D { get; set; }
    
        public decimal Num_CJ { get; set; }

     

        public decimal BWB_CJ { get; set; }

        public decimal NUM_CD { get; set; }

    

        public decimal BWB_CD { get; set; }

        public decimal Num_YJ { get; set; }

       

        public decimal BWB_YJ { get; set; }

        public decimal Num_YD { get; set; }

   

        public decimal BWB_YD { get; set; }

        public decimal NumEnd_J { get; set; }

    

        public decimal BWBEnd_J { get; set; }

        public decimal NumEnd_D { get; set; }

     

        public decimal BWBEnd_D { get; set; }

        /// <summary>
        /// 上级科目
        /// </summary>
        public string ParentCode { get; set; }
        public string Category { get; set; }
        public int SLevel { get; set; }

        /// <summary>
        /// 是否最小子科目
        /// </summary>
        public string IsChild { get; set; }
    }

    public class ZB_KMBALHead
    {
        public string PeriodId { get; set; }
        public string Year { get; set; }

        public string Month { get; set; }
        public List<ZB_KMBAL> Data { get; set; }
    }

    /// <summary>
    /// 序时账
    /// </summary>
    public class DocDetailAccount
    {
        public DateTime? PZDate
        {
            get;
            set;
        }
        public string PZZ { get; set; }
        public string PZZNO { get; set; }
        public string SubjectCode { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Credit_Debit { get; set; }
        public string amt { get; set; }
    }
}
