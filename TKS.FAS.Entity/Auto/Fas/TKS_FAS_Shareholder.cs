using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///TKS_FAS_Shareholder
    /// </summary>	
    public class TKS_FAS_Shareholder
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
        /// 订单号
        /// </summary>		
        public string OrderNo
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
        /// Stock_Percent
        /// </summary>		
        public string Stock_Percent
        {
            get;
            set;
        }
        /// <summary>
        /// Front_Pic
        /// </summary>		
        public string Front_Pic
        {
            get;
            set;
        }
        /// <summary>
        /// Back_Pic
        /// </summary>		
        public string Back_Pic
        {
            get;
            set;
        }

    }


}