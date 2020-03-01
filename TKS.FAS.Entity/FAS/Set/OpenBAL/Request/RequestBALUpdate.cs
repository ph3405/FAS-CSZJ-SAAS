using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class RequestBALUpdate : RequestBase
    {

        public string Id { get; set; }

        /// <summary>
        /// 1:正常，2：辅助核算
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string InputType { get; set; }

        /// <summary>
        ///  金额
        /// </summary>
        public decimal Money { get; set; }

        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
    }
}
