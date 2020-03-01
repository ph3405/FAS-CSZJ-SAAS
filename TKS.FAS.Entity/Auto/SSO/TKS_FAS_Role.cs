using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //TKS_FAS_Role
    public class TKS_FAS_Role
    {

        /// <summary>
        /// id
        /// </summary>		
        public string Id
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
        /// Memo
        /// </summary>		
        public string Memo
        {
            get;
            set;
        }
        /// <summary>
        /// 1 平台级，2代账企业 3 雇主企业
        /// </summary>		
        public int Type
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
        /// <summary>
        /// UpdateUser
        /// </summary>		
        public string UpdateUser
        {
            get;
            set;
        }
        /// <summary>
        /// UpdateDate
        /// </summary>		
        public DateTime? UpdateDate
        {
            get;
            set;
        }

    }
}