using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    /// <summary>
    /// auto complete data
    /// </summary>
    public class SubjectAutoData
    {
        public string Id { get; set; }
        public string label { get; set; }
        
        public string value { get; set; }
        
        public int Credit_Debit { get; set; }

        public int IsCalHelperValid { get; set; }

        public int IsQuantityValid { get; set; }

        public int IsCurrencyValid { get; set; }

}
}
