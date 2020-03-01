using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity
{
    public class ResponseICRegisterGetByUser:ResponseBase
    {
        public List<TKS_FAS_ICRegisterOrder> Data { get; set; }
    }
}
