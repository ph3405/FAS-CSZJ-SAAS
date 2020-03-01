using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //发票收付明细
    public class TKS_FAS_InvoiceSFDetail
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
        /// InvoiceId
        /// </summary>
        public string InvoiceId
        {
            get;set;
        }
        /// <summary>
        /// SFDate
        /// </summary>
        public string SFDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>		
        public decimal SFMoney
        {
            get;
            set;
        }
        /// <summary>
        /// SFRemark
        /// </summary>		
        public string SFRemark
        {
            get;
            set;
        }
        public string SFStatus
        {
            get;
            set;
        }
        /// <summary>
        /// Seq
        /// </summary>		
        public int Seq
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
        /// CreateUser
        /// </summary>		
        public string CreateUser
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