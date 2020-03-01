using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TKS.FAS.Entity
{
		/// <summary>
 	///固定资产折旧记录
 	/// </summary>	
		public class TKS_FAS_FixedAssetsLog
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
		/// 变更的期间
        /// </summary>		
        public string PeriodId
        {
            get;
            set;
        }        
		/// <summary>
		/// 固定资产ID
        /// </summary>		
        public string FixedId
        {
            get;
            set;
        }        
		/// <summary>
		/// 
        /// </summary>		
        public decimal Amount
        {
            get;
            set;
        }        
      
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}