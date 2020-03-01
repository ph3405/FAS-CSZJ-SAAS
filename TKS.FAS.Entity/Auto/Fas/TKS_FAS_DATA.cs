using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///基础资料表
    /// </summary>	
    public class TKS_FAS_DATA
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
        /// Code
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
        /// <summary>
        /// ParentId
        /// </summary>		
        public string ParentId
        {
            get;
            set;
        }

    }

    
}