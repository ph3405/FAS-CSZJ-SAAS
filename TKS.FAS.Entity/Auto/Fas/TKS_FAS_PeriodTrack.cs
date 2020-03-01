using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///TKS_FAS_PeriodTrack
    /// </summary>	
    public class TKS_FAS_PeriodTrack
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
        /// FromId
        /// </summary>		
        public string FromId
        {
            get;
            set;
        }
        /// <summary>
        /// ToId
        /// </summary>		
        public string ToId
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
        /// FromYear
        /// </summary>		
        public string FromYear
        {
            get;
            set;
        }
        /// <summary>
        /// ToYear
        /// </summary>		
        public string ToYear
        {
            get;
            set;
        }
        /// <summary>
        /// FromMonth
        /// </summary>		
        public string FromMonth
        {
            get;
            set;
        }
        /// <summary>
        /// ToMonth
        /// </summary>		
        public string ToMonth
        {
            get;
            set;
        }

    }
}