using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //自定义列描述
    public class TKS_FAS_CustomColDes
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
        /// 自定义列代码
        /// </summary>		
        public string ColumnCode
        {
            get;
            set;
        }
        /// <summary>
        /// 自定义列名称
        /// </summary>		
        public string ColumnName
        {
            get;
            set;
        }

    }
}