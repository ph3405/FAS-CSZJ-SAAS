using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    /// <summary>
    /// 辅助核算数据源
    /// </summary>
    public class SubjectAssSource
    {
        public SubjectAssSource()
        {
            Item = new Entity.CodeValue();
            Source = new List<Entity.CodeValue>();
        }
        public CodeValue Item { get; set; }

        public decimal Rate { get; set; }

        public List<CodeValue> Source { get; set; }
    }
}
