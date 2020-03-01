using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //TKS_FAS_Role2Permission
    public class TKS_FAS_Role2Permission
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
        /// RoleId
        /// </summary>		
        public string RoleId
        {
            get;
            set;
        }
        /// <summary>
        /// Permission
        /// </summary>		
        public string Permission
        {
            get;
            set;
        }
        /// <summary>
        /// 级别
        /// </summary>
        public int PLevel { get; set; }

    }
}