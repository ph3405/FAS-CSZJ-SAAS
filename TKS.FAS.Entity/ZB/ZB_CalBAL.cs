using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class ZB_CalBAL
    {
        public int No { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal NumStart_J { get; set; }

        public decimal PriceStart_J { get; set; }

        public decimal BWBStart_J { get; set; }

        public decimal NumStart_D { get; set; }
        public decimal BWBStart_D { get; set; }
        public decimal PriceStart_D { get; set; }
        public decimal Num_CJ { get; set; }

        public decimal Price_CJ { get; set; }

        public decimal BWB_CJ { get; set; }

        public decimal NUM_CD { get; set; }

        public decimal Price_CD { get; set; }

        public decimal BWB_CD { get; set; }

        public decimal Num_YJ { get; set; }

        public decimal Price_YJ { get; set; }

        public decimal BWB_YJ { get; set; }

        public decimal Num_YD { get; set; }

        public decimal Price_YD { get; set; }

        public decimal BWB_YD { get; set; }

        public decimal NumEnd_J { get; set; }

        public decimal PriceEnd_J { get; set; }

        public decimal BWBEnd_J { get; set; }

        public decimal NumEnd_D { get; set; }

        public decimal PriceEnd_D { get; set; }

        public decimal BWBEnd_D { get; set; }

    }
}
