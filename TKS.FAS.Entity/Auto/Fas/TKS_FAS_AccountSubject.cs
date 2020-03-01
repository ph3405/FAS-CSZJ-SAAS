using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //会计科目表
    public class TKS_FAS_AccountSubject
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
        /// 账套ID
        /// </summary>		
        public string AccountId
        {
            get;
            set;
        }
        /// <summary>
        /// 编码
        /// </summary>		
        public string Code
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
        public string ShowTitle { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>		
        public string ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 一级科目代码
        /// </summary>
        public string RootCode { get; set; }
        /// <summary>
        /// 科目类别
        /// </summary>		
        public string Category
        {
            get;
            set;
        }
        /// <summary>
        /// 余额方向，0,借方，1贷方
        /// </summary>		
        public int Credit_Debit
        {
            get;
            set;
        }
        /// <summary>
        /// 是否使用
        /// </summary>		
        public int IsUse
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>		
        public int IsValid
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用辅助核算
        /// </summary>		
        public int IsCalHelperValid
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用数量核算
        /// </summary>		
        public int IsQuantityValid
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用外币核算
        /// </summary>		
        public int IsCurrencyValid
        {
            get;
            set;
        }
        /// <summary>
        /// 是否叶子节点
        /// </summary>		
        public int IsLeaf
        {
            get;
            set;
        }
        /// <summary>
        /// 阶级一共4级
        /// </summary>		
        public int SLevel
        {
            get;
            set;
        }
        
        /// <summary>
        /// 辅助核算项目值
        /// </summary>		
        public string CalHelperValue
        {
            get;
            set;
        }
        /// <summary>
        /// 数量计量单位
        /// </summary>		
        public string QuantityValue
        {
            get;
            set;
        }
        /// <summary>
        /// 币种代码
        /// </summary>		
        public string CurrencyValue
        {
            get;
            set;
        }

        public int IsCustom { get; set; }

        

       

    }

    public class Subject
    {
        
        public string Code
        {
            get;
            set;
        }
        public string ParentCode
        {
            get;
            set;
        }
        public int SLevel
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Category
        {
            get;
            set;
        }
    }
}