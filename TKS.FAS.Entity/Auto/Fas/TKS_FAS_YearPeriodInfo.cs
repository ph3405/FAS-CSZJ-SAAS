using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //年度会计期间
    public class TKS_FAS_YearPeriodInfo
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
        /// Year
        /// </summary>		
        public int Year
        {
            get;
            set;
        }
        /// <summary>
        /// StartDate
        /// </summary>		
        public DateTime? StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// EndDate
        /// </summary>		
        public DateTime? EndDate
        {
            get;
            set;
        }
        /// <summary>
        /// 月 期间 数量
        /// </summary>		
        public int PeriodNUM
        {
            get;
            set;
        }
        /// <summary>
        /// 是否结账
        /// </summary>		
        public int IsClose
        {
            get;
            set;
        }
        /// <summary>
        /// 是否当前激活
        /// </summary>		
        public int IsActive
        {
            get;
            set;
        }

    }
}