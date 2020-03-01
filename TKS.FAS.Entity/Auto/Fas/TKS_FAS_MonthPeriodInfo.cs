using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //计算日期区间信息,月度
    public class TKS_FAS_MonthPeriodInfo
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
        /// 账套ID
        /// </summary>		
        public string AccountId
        {
            get;
            set;
        }

        

        /// <summary>
        /// 期间值，月
        /// </summary>		
        public int Month
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
        /// Title
        /// </summary>		
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 开始日期
        /// </summary>		
        public DateTime? StartDate
        {
            get;
            set;
        }
        /// <summary>
        /// 结束日期
        /// </summary>		
        public DateTime? EndDate
        {
            get;
            set;
        }
       
        /// <summary>
        /// 是否结账
        /// </summary>		
        public int IsPay
        {
            get;
            set;
        }
        /// <summary>
        /// 当前激活的期间（会计）
        /// </summary>		
        public int IsActive
        {
            get;
            set;
        }
        public string ParentId { get; set; }
    }
}