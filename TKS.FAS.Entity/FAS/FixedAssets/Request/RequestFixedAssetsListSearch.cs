using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestFixedAssetsListSearch : RequestSearchBase
    {
       

        /// <summary>
        /// 资产名称
        /// </summary>
        public string Name { get; set; }
        public string PeriodDate { get; set; }
        public string PeriodId { get; set; }
    }
}
