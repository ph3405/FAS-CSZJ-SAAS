using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TKS.FAS.Entity;

namespace TKS.FAS.BLL
{
    public class GenComparer : IEqualityComparer<ZB_GenAccount>
    {
        public bool Equals(ZB_GenAccount x, ZB_GenAccount y)
        {
            return x.SubjectCode == y.SubjectCode;
        }

        public int GetHashCode(ZB_GenAccount obj)
        {
            return obj.ToString ().GetHashCode();
        }
    }
    public class ZB_KMBALComparer : IEqualityComparer<ZB_KMBAL>
    {
        public bool Equals(ZB_KMBAL x, ZB_KMBAL y)
        {
            return x.PeriodId == y.PeriodId;
        }

        public int GetHashCode(ZB_KMBAL obj)
        {
            return obj.ToString().GetHashCode();
        }
    }

    public class ZB_DetailComparer : IEqualityComparer<ZB_Detail>
    {
        public bool Equals(ZB_Detail x, ZB_Detail y)
        {
            return x.PeriodId == y.PeriodId;
        }

        public int GetHashCode(ZB_Detail obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
    public class CodeComparer : IEqualityComparer<CodeValue>
    {
        public bool Equals(CodeValue x, CodeValue y)
        {
            return x.Code == y.Code;
        }

        public int GetHashCode(CodeValue obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
