using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //TKS_FAS_Function
    public class TKS_FAS_Function
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
        /// Name
        /// </summary>		
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// URL
        /// </summary>		
        public string URL
        {
            get;
            set;
        }
        /// <summary>
        /// Img
        /// </summary>		
        public string Img
        {
            get;
            set;
        }
        /// <summary>
        /// ParentId
        /// </summary>		
        public string ParentId
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

        public int Seq
        {
            get;set;
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