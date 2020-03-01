using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Entity.FAS 
{
    public class ResponseJYGet:ResponseBase
    {
        public string Account { get; set; }

        /// <summary>
        /// 本月开票收入
        /// </summary>
        public SubjectMoney KPMoney { get; set; }
        /// <summary>
        /// 本季度开票收入
        /// </summary>
        public SubjectMoney Quarter_KPMoney { get; set; }
        /// <summary>
        /// 首页银行存款余额
        /// </summary>
        public SubjectMoney BankDepositBal_Main { get; set; }
        

        /// <summary>
        /// 本年累计开票收入
        /// </summary>
        public SubjectMoney KPMoneyYear { get; set; }

        /// <summary>
        /// 本月银行存款余额
        /// </summary>
        public List<SubjectMoney > BankDepositBal { get; set; }

        /// <summary>
        /// 本月应收账款余额
        /// </summary>
        public List<SubjectMoney> ARAccountBal { get; set; }
        /// <summary>
        /// 首页应收账款余额
        /// </summary>
        public SubjectMoney ARAccountBal_Main { get; set; }
        

        /// <summary>
        /// 本月客户收款金额
        /// </summary>
        public List<SubjectMoney> KHMoney { get; set; }

        /// <summary>
        /// 本月应付账款余额
        /// </summary>
        public List<SubjectMoney> APAccountBal { get; set; }
        /// <summary>
        /// 首页应付账款余额
        /// </summary>
        public SubjectMoney APAccountBal_Main { get; set; }
        

        /// <summary>
        /// 本月支付供应商款项金额
        /// </summary>
        public List<SubjectMoney> PaySupplierMoney { get; set; }

        /// <summary>
        /// 本月供应商采购金额
        /// </summary>
        public List<SubjectMoney> CGSupplierMoney { get; set; }
    }

    public class SubjectMoney
    {
        public string Item { get; set; }
        public string Subject { get; set; }

        public string Credit_Debit { get; set; }

        public decimal Money { get; set; }
    }
}
