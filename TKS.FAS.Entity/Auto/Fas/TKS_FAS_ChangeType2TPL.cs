using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///固定资产变更类型对应模板ID
    /// </summary>	
    public class TKS_FAS_ChangeType2TPL
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
        /// 变更类型 1 新增 2 原值调整 3 累计折旧调整  
        /// </summary>		
        public int ChangeType
        {
            get;
            set;
        }
        /// <summary>
        /// 模板ID
        /// </summary>		
        public string TPLId
        {
            get;
            set;
        }

    }
}