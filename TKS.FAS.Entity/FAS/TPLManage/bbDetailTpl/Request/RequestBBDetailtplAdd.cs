using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    /// <summary>
    /// 报表模板列明细新增
    /// </summary>
    public class RequestBBDetailtplAdd:RequestBase
    {
        public TKS_FAS_ReportDetailTPL Data { get; set; }
    }
}
