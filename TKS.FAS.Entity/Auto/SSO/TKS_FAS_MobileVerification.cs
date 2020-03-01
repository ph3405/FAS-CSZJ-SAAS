using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //TKS_FAS_MobileVerification
    public class TKS_FAS_MobileVerification
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
        /// Mobile
        /// </summary>		
        public string Mobile
        {
            get;
            set;
        }
        /// <summary>
        /// VerCode
        /// </summary>		
        public string VerCode
        {
            get;
            set;
        }
        public string CodeType
        {
            get;
            set;
        }
        public string Status
        {
            get;
            set;
        }
        /// <summary>
        /// CreateDate
        /// </summary>		
        public DateTime? CreateDate
        {
            get;
            set;
        }

    }
}