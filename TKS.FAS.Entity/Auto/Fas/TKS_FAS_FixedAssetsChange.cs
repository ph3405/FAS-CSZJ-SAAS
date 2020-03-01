using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TKS.FAS.Entity
{
		/// <summary>
 	///固定资产变更记录
 	/// </summary>	
		public class TKS_FAS_FixedAssetsChange
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
		/// ParentId
        /// </summary>		
        public string ParentId
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
		/// 关联凭证
        /// </summary>		
        public string DocId
        {
            get;
            set;
        }        
		/// <summary>
		/// 凭证字
        /// </summary>		
        public string DocPZZ
        {
            get;
            set;
        }        
		/// <summary>
		/// 变动类型 1 新增 2 原值调整 3 累计折旧调整 4 使用年限调整 5 部门转移 6 折旧方法调整 7 状态修改 8 报废
        /// </summary>		
        public int ChangeType
        {
            get;
            set;
        }        
		/// <summary>
		/// 资产编号
        /// </summary>		
        public string DocNo
        {
            get;
            set;
        }        
		/// <summary>
		/// 资产名称
        /// </summary>		
        public string AssetsName
        {
            get;
            set;
        }        
		/// <summary>
		/// PreContent
        /// </summary>		
        public string PreContent
        {
            get;
            set;
        }        
		/// <summary>
		/// CurContent
        /// </summary>		
        public string CurContent
        {
            get;
            set;
        }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}