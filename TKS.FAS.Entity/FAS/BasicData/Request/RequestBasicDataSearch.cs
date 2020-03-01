using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    /// <summary>
    /// 查询
    /// </summary>
    public   class RequestBasicDataSearch : RequestSearchBase
    {
        public string Id { get; set; }
        public string DataType { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        
    }
}
