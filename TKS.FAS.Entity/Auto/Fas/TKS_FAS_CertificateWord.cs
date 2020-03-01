using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //凭证字
    public class TKS_FAS_CertificateWord
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
        /// PZZ
        /// </summary>		
        public string PZZ
        {
            get;
            set;
        }
        /// <summary>
        /// ShowTitle
        /// </summary>		
        public string ShowTitle
        {
            get;
            set;
        }
        /// <summary>
        /// 是否默认，1是，0否
        /// </summary>		
        public int IsDefault
        {
            get;
            set;
        }

    }
}