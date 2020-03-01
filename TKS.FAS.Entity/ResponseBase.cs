using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
    public class ResponseBase
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}
