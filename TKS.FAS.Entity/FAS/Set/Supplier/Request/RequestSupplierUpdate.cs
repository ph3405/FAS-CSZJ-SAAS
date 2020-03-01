using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity 
{
   public class RequestSupplierUpdate:RequestBase
    {
       public TKS_FAS_Supplier Data { get; set; }
    }
}
