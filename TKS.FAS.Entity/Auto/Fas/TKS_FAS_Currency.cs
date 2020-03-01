using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //币制
    public class TKS_FAS_Currency
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
        /// Code
        /// </summary>		
        public string Code
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
        /// 是否本位币，1是0否
        /// </summary>		
        public int IsBaseCurrency
        {
            get;
            set;
        }
        /// <summary>
        /// 汇率
        /// </summary>		
        public decimal ExchangeRate
        {
            get;
            set;
        }

    }
}