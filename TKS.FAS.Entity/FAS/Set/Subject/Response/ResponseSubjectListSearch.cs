using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseSubjectListSearch : ResponseSearchBase<TKS_FAS_AccountSubject>
    {
        public string CheckReadonly { get; set; }
        public List<TKS_FAS_AccountSubjectExt> lst { get; set; }
    }
    public class TKS_FAS_AccountSubjectExt : TKS_FAS_AccountSubject
    {
        public string IsTree;
    }
}


