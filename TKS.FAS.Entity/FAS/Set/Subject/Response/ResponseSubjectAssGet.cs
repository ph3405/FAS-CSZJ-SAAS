using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public  class ResponseSubjectAssGet:ResponseBase
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public bool IsCalHelperValid { get; set; }

        public bool IsQuantityValid { get; set; }

        public bool IsCurrencyValid { get; set; }

        public string QuantityValue { get; set; }

        public List<SubjectAssSource> CalHelper { get; set; }

        public List<SubjectAssSource> Currency { get; set; }
    }
}
