using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //辅助核算项目
    public class TKS_FAS_CaculateHelperItem
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
        /// 账套ID，一个账套一套辅助核算
        /// </summary>		
        public string AccountId
        {
            get;
            set;
        }
        /// <summary>
        /// Title
        /// </summary>		
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// 是否自定义
        /// </summary>		
        public int IsCustom
        {
            get;
            set;
        }

    }
}