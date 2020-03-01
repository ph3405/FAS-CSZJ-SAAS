using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS
{
    public class ResponseBALCalculate : ResponseBase
    {
        public decimal startBWB1 { get; set; }//期初借方本位币合计 资产
        public decimal startBWB2 { get; set; }//期初贷方本位币合计 负债
        public decimal startBWB3 { get; set; }//期初贷方本位币合计 权益
        public decimal startBWB4 { get; set; }//期初借方本位币合计 成本
        public decimal startBWB5 { get; set; }//期初借方本位币合计 损益

        public decimal deLJBWB1 { get; set; }//借方累计合计 资产 
        public decimal deLJBWB2 { get; set; }//借方累计合计 负债 
        public decimal deLJBWB3 { get; set; }//借方累计合计 权益 
        public decimal deLJBWB4 { get; set; }//借方累计合计 成本 
        public decimal deLJBWB5 { get; set; }//借方累计合计 损益 

        public decimal crLJBWB1 { get; set; }//贷方累计合计 资产 
        public decimal crLJBWB2 { get; set; }//贷方累计合计 负债 
        public decimal crLJBWB3 { get; set; }//贷方累计合计 权益 
        public decimal crLJBWB4 { get; set; }//贷方累计合计 成本 
        public decimal crLJBWB5 { get; set; }//贷方累计合计 损益 
    }
}
