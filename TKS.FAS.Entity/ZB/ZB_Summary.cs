using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class ZB_Summary
    {

        public string Code { get; set; }

        public string Name { get; set; }



        #region 借方

        public decimal NUM1 { get; set; }

      

        public decimal Money1 { get; set; }

        #endregion

        #region 贷方

        public decimal NUM2 { get; set; }

      

        public decimal Money2 { get; set; }

        #endregion
    }
}
