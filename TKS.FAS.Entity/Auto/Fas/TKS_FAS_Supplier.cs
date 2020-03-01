using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///供应商维护，用以账套外包时候，选择外包机构
    /// </summary>	
    public class TKS_FAS_Supplier
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
        /// SupplierId
        /// </summary>		
        public string SupplierId
        {
            get;
            set;
        }
        /// <summary>
        /// 供应商机构名称
        /// </summary>		
        public string SupplierName
        {
            get;
            set;
        }
        /// <summary>
        /// 所属机构ID
        /// </summary>		
        public string OrgId
        {
            get;
            set;
        }
        /// <summary>
        /// 所属机构名称
        /// </summary>		
        public string OrgName
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