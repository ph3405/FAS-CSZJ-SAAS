using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
   
    public class TKS_FAS_BasicData
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
        /// 
        /// </summary>		
        public string UserId
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>		
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 客户：Customer  供应商：Vendor
        /// </summary>		
        public string DataType
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>		
        public string Remark
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