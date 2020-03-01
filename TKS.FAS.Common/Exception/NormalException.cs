using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Common
{
    public class NormalException : ApplicationException
    {
       

        public NormalException(string message):base(message)
        {
           
        }
    }
}
