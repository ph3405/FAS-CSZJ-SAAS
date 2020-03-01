using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///资讯
    /// </summary>	
    public class TKS_FAS_News
    {

        /// <summary>
        /// id
        /// </summary>		
        public string id
        {
            get;
            set;
        }
        /// <summary>
        /// from_Id
        /// </summary>		
        public string from_Id
        {
            get;
            set;
        }
        /// <summary>
        /// title
        /// </summary>		
        public string title
        {
            get;
            set;
        }
        /// <summary>
        /// summary
        /// </summary>		
        public string summary
        {
            get;
            set;
        }
        /// <summary>
        /// cover
        /// </summary>		
        public string cover
        {
            get;
            set;
        }
        /// <summary>
        /// author_name
        /// </summary>		
        public string author_name
        {
            get;
            set;
        }
        /// <summary>
        /// published_at
        /// </summary>		
        public DateTime? published_at
        {
            get;
            set;
        }
        /// <summary>
        /// post_id
        /// </summary>		
        public string post_id
        {
            get;
            set;
        }
        /// <summary>
        /// created_at
        /// </summary>		
        public DateTime? created_at
        {
            get;
            set;
        }
        /// <summary>
        /// news
        /// </summary>		
        public string type
        {
            get;
            set;
        }
        /// <summary>
        /// content
        /// </summary>		
        public string content
        {
            get;
            set;
        }

        public decimal status { get; set; }
        public int Sort { get; set; }
    }


}