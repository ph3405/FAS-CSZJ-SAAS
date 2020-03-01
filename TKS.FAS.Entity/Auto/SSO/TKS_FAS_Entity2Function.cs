using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //TKS_FAS_Entity2Function
    public class TKS_FAS_Entity2Function
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
        /// U:user,R:role
        /// </summary>		
        public string Type
        {
            get;
            set;
        }
        /// <summary>
        /// EntityId
        /// </summary>		
        public string EntityId
        {
            get;
            set;
        }
        /// <summary>
        /// FunctionId
        /// </summary>		
        public string FunctionId
        {
            get;
            set;
        }

    

        public string CheckState
        {
            get;
            set;
        }
    }
}