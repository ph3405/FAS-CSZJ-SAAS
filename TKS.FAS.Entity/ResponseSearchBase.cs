using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class ResponseSearchBase<T>:ResponseBase where T:class
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 查询数据
        /// </summary>
        public List<T> Data { get; set; }

        public List<TKS_FAS_Doc> lstAll { get; set; }
    }
}
