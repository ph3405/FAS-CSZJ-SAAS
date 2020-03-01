using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 辅助核算起初删除
    /// </summary>
    public class RequestBALDelete : RequestBase
    {
        public string Id { get; set; }

        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }

    }
}
