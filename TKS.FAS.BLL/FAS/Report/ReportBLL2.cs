using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TKS.FAS.Entity;
using TKS.FAS.Common;
using TKS.FAS.Entity.FAS;
using Dapper;
using DapperExtensions;
using System.Data;

namespace TKS.FAS.BLL.FAS
{
    /// <summary>
    /// 税金报表
    /// </summary>
    public class ReportBLL2 : CommonBase
    {
        public int GetQuarterByMonth(int month)
        {
            int m = 0;
            if (month >= 1 && month <= 3) m = 1;
            if (month >= 4 && month <= 6) m = 2;
            if (month >= 7 && month <= 9) m = 3;
            if (month >= 10 && month <= 12) m = 4;
            return m;
        }
        List<TKS_FAS_GLBalanceExt> GetData(string prePeriodId, string curPeriodId, string accountId, IDbTransaction ts)
        {


            string sql = @" 
with MM as (
select K.*,J.RootCode, j.IsLeaf from (

		select SubjectCode,
		sum(BWBDebitTotal_Y) as BWBDebitTotal_Y, 
        sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
		AccountId
from(
			select SubjectCode, 	 
			sum(BWBDebitTotal_Y) as BWBDebitTotal_Y, 
            sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
			AccountId
			 
		from TKS_FAS_GLBalance
			where PeriodId=@prePeriodId and AccountId=@AccountId
		group by SubjectCode,AccountId
union all  
			select SubjectCode,
			sum(BWBDebitTotal_Y) as BWBDebitTotal_Y,
			 sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
		 AccountId
		from TKS_FAS_FGLBalance
		 where PeriodId=@prePeriodId and AccountId=@AccountId
		group by SubjectCode,AccountId

union all
		select 
 subjectCode,
 sum(Money_Debit),
 sum(Money_Credit),
 accountId
from TKS_FAS_DocDetail where PeriodId=@curPeriodId and AccountId=@AccountId
group by SubjectCode,accountId

) A
 group by SubjectCode ,accountId

) K
  left join
  TKS_FAS_AccountSubject J
   on K.SubjectCode=J.Code and K.AccountId=J.AccountId
 ) 
 
select
SubjectCode,
BWBDebitTotal_Y,
BWBCreditTotal_Y from MM where
  IsLeaf = 1
 union

select

rootCode, 
BWBDebitTotal_Y,
BWBCreditTotal_Y from (
		select RootCode, 
       
		sum( BWBDebitTotal_Y  ) as BWBDebitTotal_Y, 
		sum( BWBCreditTotal_Y  ) as BWBCreditTotal_Y from MM 
		 
	 group by RootCode
) K
";
            List<TKS_FAS_GLBalanceExt> balance = cnn.Query<TKS_FAS_GLBalanceExt>(sql,
                new { prePeriodId = prePeriodId, curPeriodId = curPeriodId, AccountId = accountId }, ts).ToList();

            return balance;
        }

        /// <summary>
        /// 本年累计已交所得税
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        decimal DeliverSDTaxYear(TKS_FAS_MonthPeriodInfo curPeriod, IDbTransaction ts)
        {
            decimal res = 0;

            AccountSetBLL bll = new AccountSetBLL(cnn);
            //var curPeriod = bll.GetActivePeriod(user.AccountId, ts);



            string sql = @"select A.* from TKS_FAS_Doc A 
                        where A.pzDate>='" + curPeriod.Year + "-01-01'  and A.pzDate<='" + curPeriod.EndDate.Value.ToString("yyyy-MM-dd") + @"' 
                        and A.accountId=@AccountId   ";

            var lsDoc = cnn.Query<TKS_FAS_Doc>(sql, new { AccountId = curPeriod.AccountId }, ts).ToList();
            //add by Hero.Zhang 获取1002科目下的所有科目
            var sub = cnn.Query<TKS_FAS_AccountSubject>(@"select * from TKS_FAS_AccountSubject where AccountId=@AccountId and RootCode='1002'", new { AccountId = curPeriod.AccountId }, ts).ToArray();
            if (sub.Length > 0)
            {
                string[] code = new string[sub.Length];
                for (int i = 0; i < sub.Count(); i++)
                {
                    code[i] = sub[i].Code;
                }
                foreach (var doc in lsDoc)
                {
                    sql = "select * from tks_fas_docDetail where parentId=@ParentId ";
                    var detail = cnn.Query<TKS_FAS_DocDetail>(sql, new { ParentId = doc.Id }, ts);

                    var flag1 = detail.Any(p => { return (code).Contains(p.SubjectCode) && p.Credit_Debit == 1; });
                    //var flag1 = detail.Any(p => { return p.SubjectCode == "1002" && p.Credit_Debit == 1; });

                    var flag2 = detail.Any(p => { return p.SubjectCode == "2171006" && p.Credit_Debit == 0; });

                    if (flag1 && flag2)
                    {
                        var d = detail.Where<TKS_FAS_DocDetail>(p => { return p.SubjectCode == "2171006" && p.Credit_Debit == 0; }).FirstOrDefault();
                        if (d == null)
                            continue;
                        res += d.Money_Debit;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 本月已交所得税
        /// </summary>
        /// <param name="curPeriod"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        decimal DeliverSDTaxMonth(TKS_FAS_MonthPeriodInfo curPeriod, IDbTransaction ts)
        {
            decimal res = 0;

            AccountSetBLL bll = new AccountSetBLL(cnn);
            string sql = @"select A.* from TKS_FAS_Doc A 
                        where A.pzDate>='" + curPeriod.StartDate.Value.ToString("yyyy-MM-dd") + "'  and A.pzDate<='" + curPeriod.EndDate.Value.ToString("yyyy-MM-dd") + @"' 
                        and A.accountId=@AccountId   ";

            var lsDoc = cnn.Query<TKS_FAS_Doc>(sql, new { AccountId = curPeriod.AccountId }, ts).ToList();
            //add by Hero.Zhang 获取1002科目下的所有科目
            var sub = cnn.Query<TKS_FAS_AccountSubject>(@"select * from TKS_FAS_AccountSubject where AccountId=@AccountId and RootCode='1002'", new { AccountId = curPeriod.AccountId }, ts).ToArray();
            if (sub.Length > 0)
            {
                string[] code = new string[sub.Length];
                for (int i = 0; i < sub.Count(); i++)
                {
                    code[i] = sub[i].Code;
                }


                foreach (var doc in lsDoc)
                {
                    sql = "select * from tks_fas_docDetail where parentId=@ParentId ";
                    var detail = cnn.Query<TKS_FAS_DocDetail>(sql, new { ParentId = doc.Id }, ts);

                    var flag1 = detail.Any(p => { return (code).Contains(p.SubjectCode) && p.Credit_Debit == 1; });
                    //var flag1 = detail.Any(p => { return  p.SubjectCode == "1002" && p.Credit_Debit == 1; });
                    var flag2 = detail.Any(p => { return p.SubjectCode == "2171006" && p.Credit_Debit == 0; });

                    if (flag1 && flag2)
                    {
                        var d = detail.Where<TKS_FAS_DocDetail>(p => { return p.SubjectCode == "2171006" && p.Credit_Debit == 0; }).FirstOrDefault();
                        if (d == null)
                            continue;
                        res += d.Money_Debit;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 本月已交增值税
        /// </summary>
        /// <param name="curPeriod"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        decimal VaTaxMonth(TKS_FAS_MonthPeriodInfo curPeriod, IDbTransaction ts)
        {
            decimal res = 0;

            AccountSetBLL bll = new AccountSetBLL(cnn);
            string sql = @"select A.* from TKS_FAS_Doc A 
                        where A.pzDate>='" + curPeriod.StartDate.Value.ToString("yyyy-MM-dd") + "'  and A.pzDate<='" + curPeriod.EndDate.Value.ToString("yyyy-MM-dd") + @"' 
                        and A.accountId=@AccountId   ";

            var lsDoc = cnn.Query<TKS_FAS_Doc>(sql, new { AccountId = curPeriod.AccountId }, ts).ToList();
            //add by Hero.Zhang 获取1002科目下的所有科目
            var sub = cnn.Query<TKS_FAS_AccountSubject>(@"select * from TKS_FAS_AccountSubject where AccountId=@AccountId and RootCode='1002'", new { AccountId = curPeriod.AccountId }, ts).ToArray();
            if (sub.Length > 0)
            {
                string[] code = new string[sub.Length];
                for (int i = 0; i < sub.Count(); i++)
                {
                    code[i] = sub[i].Code;
                }


                foreach (var doc in lsDoc)
                {
                    sql = "select * from tks_fas_docDetail where parentId=@ParentId ";
                    var detail = cnn.Query<TKS_FAS_DocDetail>(sql, new { ParentId = doc.Id }, ts);

                    var flag1 = detail.Any(p => { return (code).Contains(p.SubjectCode) && p.Credit_Debit == 1; });
                    //var flag1 = detail.Any(p => { return  p.SubjectCode == "1002" && p.Credit_Debit == 1; });
                    var flag2 = detail.Any(p => { return p.SubjectCode == "2171001" && p.Credit_Debit == 0; });

                    if (flag1 && flag2)
                    {
                        var d = detail.Where<TKS_FAS_DocDetail>(p => { return p.SubjectCode == "2171001" && p.Credit_Debit == 0; }).FirstOrDefault();
                        if (d == null)
                            continue;
                        res += d.Money_Debit;
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// 本年累计增值税合计
        /// </summary>
        /// <param name="curPeriod"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        decimal GetTotalZZTax(TKS_FAS_MonthPeriodInfo curPeriod, IDbTransaction ts, string AccountId)
        {
            decimal res = 0;

            AccountSetBLL bll = new AccountSetBLL(cnn);
            string sql = @"select A.* from TKS_FAS_MonthPeriodInfo A 
                        where A.Year='" + curPeriod.Year + "'  and A.StartDate<='" + curPeriod.EndDate.Value.ToString("yyyy-MM-dd") + @"' 
                        and A.accountId=@AccountId   ";

            var lstPeriod = cnn.Query<TKS_FAS_MonthPeriodInfo>(sql, new { AccountId = curPeriod.AccountId }, ts).ToList();
            sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
                            where accountId=@AccountId and  pzid is null and type=25 ";
            //销售发票
            var unCalXXTaxData = cnn.ExecuteScalar(sql, new { AccountId = AccountId }, ts);
            decimal unCalXXTax = 0;
            if (unCalXXTaxData != null)
            {
                unCalXXTax = decimal.Parse(unCalXXTaxData.ToString());
            }
            sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
                            where accountId=@AccountId and  pzid is null and type<>25 "; //除销售发票
            var unCalJXTaxData = cnn.ExecuteScalar(sql, new { AccountId = AccountId }, ts);
            decimal unCalJXTax = 0;
            if (unCalJXTaxData != null)
            {
                unCalJXTax = decimal.Parse(unCalJXTaxData.ToString());
            }

            foreach (var period in lstPeriod)
            {
                //销项税
                sql = @"select sum(Money_Credit) Money_Credit,sum(Money_Debit) Money_Debit
                                    from tks_fas_docDetail where subjectCode='217100105' 
                                  and periodId=@PeriodId";
                var xxData = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
                    new { PeriodId = period.Id }, ts);

                decimal xxTax = xxData.Money_Credit - xxData.Money_Debit;//贷方 - 借方
                                                                         //进项税
                sql = @"select sum(Money_Credit) Money_Credit,sum(Money_Debit) Money_Debit
                                from tks_fas_docDetail where subjectCode='217100101' 
                                  and periodId=@PeriodId";
                var jxData = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
                    new { PeriodId = period.Id }, ts);
                decimal jxTax = jxData.Money_Debit - jxData.Money_Credit;//借方 - 贷方


                decimal preZZTax = xxTax + unCalXXTax - jxTax - unCalJXTax;//销项汇总-进项汇总
                decimal LiuDi = GetLiuDi(AccountId, curPeriod, ts);
                decimal VaTax = preZZTax - LiuDi;//update by Hero.Zhang
                if (VaTax < 0)
                {
                    VaTax = 0;
                }
                res += VaTax;
            }

            return res;
        }
        decimal WX_DeliverSDTax(string AccountId, IDbTransaction ts)
        {
            decimal res = 0;

            AccountSetBLL bll = new AccountSetBLL(cnn);
            var curPeriod = bll.GetActivePeriod(AccountId, ts);



            string sql = @"select A.* from TKS_FAS_Doc A 
                        where A.pzDate>='" + curPeriod.Year + "-01-01'  and A.pzDate<='" + curPeriod.EndDate.Value.ToString("yyyy-MM-dd") + @"' 
                        and A.accountId=@AccountId   ";

            var lsDoc = cnn.Query<TKS_FAS_Doc>(sql, new { AccountId = AccountId }, ts).ToList();

            foreach (var doc in lsDoc)
            {
                sql = "select * from tks_fas_docDetail where parentId=@ParentId ";
                var detail = cnn.Query<TKS_FAS_DocDetail>(sql, new { ParentId = doc.Id }, ts);

                var flag1 = detail.Any(p => { return p.SubjectCode == "1002" && p.Credit_Debit == 1; });

                var flag2 = detail.Any(p => { return p.SubjectCode == "2171006" && p.Credit_Debit == 0; });

                if (flag1 && flag2)
                {
                    var d = detail.Where<TKS_FAS_DocDetail>(p => { return p.SubjectCode == "2171006" && p.Credit_Debit == 0; }).FirstOrDefault();
                    if (d == null)
                        continue;
                    res += d.Money_Debit;
                }
            }

            return res;
        }
        /// <summary>
        /// 税金（当期凭证）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseSJGet SJGet(RequestSJGet request)
        {
            var res = new ResponseSJGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    AccountSetBLL bll = new AccountSetBLL(cnn);
                    PeriodBLL periodBLL = new PeriodBLL(cnn);
                    //var account = bll.AccountInfoGet(request.AccountList ?? user.AccountId, ts);

                    var account = bll.AccountInfoGet(user.AccountId, ts);
                    //string sql_Period = "select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId and Id=@PeriodId order by startDate";
                    //var curPeriod = cnn.Query<TKS_FAS_MonthPeriodInfo>(sql_Period, new { AccountId = user.AccountId, PeriodId = request.PeriodId }, ts).FirstOrDefault();
                    var curPeriod = periodBLL.GetPeriod(request.PeriodId, ts);

                    //var curPeriod = bll.GetActivePeriod(request.AccountList ?? user.AccountId, ts);
                    var prePeriod = periodBLL.GetPrePeriod(curPeriod, ts);
                    //if (prePeriod == null)
                    //{
                    //    if (!string.IsNullOrEmpty(request.AccountList))
                    //    {
                    //        res.IsSuccess = false;
                    //        res.Message = "加载完成";

                    //        return res;
                    //    }
                    //    else
                    //    {
                    //        throw new NormalException("当前账套无上一个期间");
                    //    }
                    //}
                    string prePeriodId = "";
                    if (prePeriod != null)
                    {
                        prePeriodId = prePeriod.Id;
                    }
                    string curPeriodId = curPeriod.Id;
                    //销项税
                    string sql = @"select sum(Money_Credit) Money_Credit,sum(Money_Debit) Money_Debit
                                    from tks_fas_docDetail where subjectCode='217100105' 
                                  and periodId=@PeriodId";
                    var xxData = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
                        new { PeriodId = curPeriodId }, ts);

                    decimal xxTax = xxData.Money_Credit - xxData.Money_Debit;//贷方 - 借方

                    //update by Hero.Zhang 原“销售发票”类型为25，修改为0
                    //sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
                    //        where accountId=@AccountId and  pzid is null and type=25 "; //销售发票
                    sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
                            where accountId=@AccountId and  pzid is null and type=0 "; //销售发票
                    var unCalXXTaxData = cnn.ExecuteScalar(sql, new { AccountId = request.AccountList ?? user.AccountId }, ts);
                    decimal unCalXXTax = 0;
                    if (unCalXXTaxData != null)
                    {
                        unCalXXTax = decimal.Parse(unCalXXTaxData.ToString());
                    }

                    //进项税
                    sql = @"select sum(Money_Credit) Money_Credit,sum(Money_Debit) Money_Debit
                                from tks_fas_docDetail where subjectCode='217100101' 
                                  and periodId=@PeriodId and AccountId=@AccountId";
                    var jxData = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
                        new { PeriodId = curPeriodId, AccountId = request.AccountList ?? user.AccountId }, ts);
                    decimal jxTax = jxData.Money_Debit - jxData.Money_Credit;//借方 - 贷方

                    //update by Hero.Zhang 原“销售发票”类型为25，修改为0
                    //sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
                    //        where accountId=@AccountId and  pzid is null and type<>25 "; //除销售发票
                    sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
                            where accountId=@AccountId and  pzid is null and type<>0 "; //除销售发票
                    var unCalJXTaxData = cnn.ExecuteScalar(sql, new { AccountId = request.AccountList ?? user.AccountId }, ts);
                    decimal unCalJXTax = 0;
                    if (unCalJXTaxData != null)
                    {
                        unCalJXTax = decimal.Parse(unCalJXTaxData.ToString());
                    }

                    decimal preZZTax = xxTax + unCalXXTax - jxTax - unCalJXTax;//销项汇总-进项汇总
                    decimal LiuDi = GetLiuDi(request.AccountList ?? user.AccountId, curPeriod, ts);
                    //decimal localSJ = (preZZTax + LiuDi) * account.TaxRate / 100m;
                    //decimal localSJ = (preZZTax + LiuDi) * (account.LandTax == 0 ? 12 : account.LandTax) / 100m;
                    //update by Hero.Zhang
                    decimal localSJ = (preZZTax - LiuDi) * (account.LandTax == 0 ? 12 : account.LandTax) / 100m;
                    if (localSJ < 0)
                    {
                        localSJ = 0;
                    }
                    var balance = GetData(prePeriodId, curPeriodId, curPeriod.AccountId, ts);

                    sql = "select * from TKS_FAS_ReportDetailTPL where accountId=@AccountId order by seq";
                    //模板
                    List<TKS_FAS_LRReport> tpl = cnn.Query<TKS_FAS_LRReport>(sql,
                        new
                        {
                            AccountId = request.AccountList ?? user.AccountId

                        }, ts).ToList();
                    sql = "select * from tks_fas_formula where accountId=@AccountId";

                    List<TKS_FAS_Formula> formula = cnn.Query<TKS_FAS_Formula>(sql, new { AccountId = request.AccountList ?? user.AccountId }, ts).ToList();

                    List<TKS_FAS_LRReport> lrReport = GenLRReport(tpl, balance, formula, ts);

                    var sdTax = lrReport.Where(p => p.Category == 22).FirstOrDefault();
                    decimal calTotalSDTax = 0;
                    if (sdTax != null)
                    {
                        calTotalSDTax = sdTax.Money_Year;
                    }


                    //本年累计已交所得税
                    decimal total_Deliver_SDTax = DeliverSDTaxYear(curPeriod, ts);

                    //sql = @"select * from TKS_FAS_AccountInfo where id=@Id";

                    // var account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(sql, new {Id=user.AccountId }, ts);



                    if (xxTax < 0)
                    {
                        xxTax = 0;
                    }

                    if (unCalJXTax < 0)
                    {
                        unCalJXTax = 0;
                    }

                    if (jxTax < 0)
                    {
                        jxTax = 0;
                    }

                    if (unCalJXTax < 0)
                    {
                        unCalJXTax = 0;
                    }

                    //decimal VaTax = preZZTax + LiuDi;
                    decimal VaTax = preZZTax - LiuDi;//update by Hero.Zhang
                    if (VaTax < 0)
                    {
                        VaTax = 0;
                    }

                    var yearTaxTotal = calTotalSDTax * account.TaxRate / 100m;
                    if (yearTaxTotal < 0)
                    {
                        yearTaxTotal = 0;
                    }

                    if (total_Deliver_SDTax < 0)
                    {
                        total_Deliver_SDTax = 0;
                    }

                    //update by Hero.zhang 20180827
                    //逻辑修改：本月预计应交所得税=本年已入账的累计应交所得税-本年累计已交所得税
                    //var preSDTax = calTotalSDTax * account.TaxRate / 100m - total_Deliver_SDTax;
                    var preSDTax = yearTaxTotal - total_Deliver_SDTax;
                    if (preSDTax < 0)
                    {
                        preSDTax = 0;
                    }
                    if (account.AddedValueTaxType == 1)
                    {
                        //add by Hero.Zhang 小规模类型的账套
                        decimal quarter_VaTax = 0;//本季度已交增值税
                        decimal quarter_SDTax = 0;//本季度已交所得税
                        var selPeriod = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                @"select * from TKS_FAS_MonthPeriodInfo where Id=@Id",
                new { Id = request.PeriodId }, ts);
                        var lstPeriod = cnn.Query<TKS_FAS_MonthPeriodInfo>(@"select * from TKS_FAS_MonthPeriodInfo where 
  AccountId=@AccountId and Year=@Year", new { AccountId = user.AccountId, Year = curPeriod.Year }, ts).ToList();
                        List<TKS_FAS_MonthPeriodInfo> lstFirst = new List<TKS_FAS_MonthPeriodInfo>();
                        List<TKS_FAS_MonthPeriodInfo> lstSecond = new List<TKS_FAS_MonthPeriodInfo>();
                        List<TKS_FAS_MonthPeriodInfo> lstThird = new List<TKS_FAS_MonthPeriodInfo>();
                        List<TKS_FAS_MonthPeriodInfo> lstFour = new List<TKS_FAS_MonthPeriodInfo>();
                        foreach (var item in lstPeriod)
                        {
                            if (item.Month <= 3)
                            {
                                lstFirst.Add(item);
                            }
                            else if (item.Month > 3 && item.Month <= 6)
                            {
                                lstSecond.Add(item);
                            }
                            else if (item.Month > 6 && item.Month <= 9)
                            {
                                lstThird.Add(item);
                            }
                            else if (item.Month > 9 && item.Month <= 12)
                            {
                                lstFour.Add(item);
                            }
                        }
                        var month = selPeriod.Month;
                        var Quarter = GetQuarterByMonth(month);

                        decimal total_Deliver_SDTax_Month = VaTaxMonth(curPeriod, ts);
                        if (Quarter == 1)
                        {
                            foreach (var item in lstFirst)
                            {
                                quarter_VaTax += VaTaxMonth(item, ts);
                                quarter_SDTax += DeliverSDTaxMonth(item, ts);
                            }

                        }
                        if (Quarter == 2)
                        {
                            foreach (var item in lstSecond)
                            {
                                quarter_VaTax += VaTaxMonth(item, ts);
                                quarter_SDTax += DeliverSDTaxMonth(item, ts);
                            }
                        }
                        if (Quarter == 3)
                        {
                            foreach (var item in lstThird)
                            {
                                quarter_VaTax += VaTaxMonth(item, ts);
                                quarter_SDTax += DeliverSDTaxMonth(item, ts);
                            }
                        }
                        if (Quarter == 4)
                        {
                            foreach (var item in lstFour)
                            {
                                quarter_VaTax += VaTaxMonth(item, ts);
                                quarter_SDTax += DeliverSDTaxMonth(item, ts);
                            }
                        }
                        res.Quarter_VaTax = new SubjectMoney { Subject = "本季度已交增值税", Credit_Debit = "", Money = quarter_VaTax };
                        decimal quarter_LocalSJ = quarter_VaTax * (account.LandTax == 0 ? 12 : account.LandTax) / 100m;
                        if (quarter_LocalSJ < 0)
                        {
                            quarter_LocalSJ = 0;
                        }
                        res.Quarter_LocalSJ = new SubjectMoney { Subject = "本季度已交地方税金", Credit_Debit = "", Money = quarter_LocalSJ };
                        res.Quarter_TotalSDTax = new SubjectMoney { Subject = "本季度已交所得税", Credit_Debit = "", Money = quarter_SDTax };
                        decimal total_ZZTax = GetTotalZZTax(curPeriod, ts, user.AccountId);
                        res.TotalZZTax = new SubjectMoney { Subject = "本年累计已交增值税", Credit_Debit = "", Money = total_ZZTax };
                        decimal total_LocalSJ = total_ZZTax * (account.LandTax == 0 ? 12 : account.LandTax) / 100m;
                        res.TotalLocalSJ = new SubjectMoney { Subject = "本年累计已交地方税金", Credit_Debit = "", Money = total_LocalSJ };
                        res.Total_Deliver_SDTax = new SubjectMoney { Subject = "本年累计已交所得税", Credit_Debit = "", Money = total_Deliver_SDTax };
                        res.TotalYearTax = new SubjectMoney { Subject = "本年累计已交税金合计", Credit_Debit = "", Money = total_ZZTax + total_LocalSJ + total_Deliver_SDTax };
                    }
                    else
                    {
                        if (curPeriod.IsPay == 1)
                        {
                            res.XXTax_TOTAL = new SubjectMoney { Subject = "本月销项税合计", Credit_Debit = "贷", Money = xxTax + unCalJXTax };
                            res.JXTax_TOTAL = new SubjectMoney { Subject = "本月进项税合计", Credit_Debit = "贷", Money = jxTax + unCalJXTax };
                            res.Pre_LiuDi = new SubjectMoney { Subject = "上期留抵", Credit_Debit = "", Money = Math.Abs(LiuDi) };
                            res.Pre_ZZTax = new SubjectMoney { Subject = "本月增值税", Credit_Debit = "", Money = VaTax };
                            res.LocalSJ = new SubjectMoney { Subject = "本月地方税金", Credit_Debit = "贷", Money = localSJ };
                            decimal total_Deliver_SDTax_Month = DeliverSDTaxMonth(curPeriod, ts);
                            res.Cal_TotalSDTax = new SubjectMoney { Subject = "本月企业所得税", Credit_Debit = "", Money = total_Deliver_SDTax_Month };

                            //add by Hero.Zhang 20180827 添加四个栏位
                            res.TotalTax = new SubjectMoney { Subject = "本月税金合计", Credit_Debit = "", Money = VaTax + localSJ + total_Deliver_SDTax_Month };
                            decimal total_ZZTax = GetTotalZZTax(curPeriod, ts, user.AccountId);
                            res.TotalZZTax = new SubjectMoney { Subject = "本年累计已交增值税", Credit_Debit = "", Money = total_ZZTax };
                            decimal total_LocalSJ = total_ZZTax * (account.LandTax == 0 ? 12 : account.LandTax) / 100m;
                            res.TotalLocalSJ = new SubjectMoney { Subject = "本年累计已交地方税金", Credit_Debit = "", Money = total_LocalSJ };
                            res.Total_Deliver_SDTax = new SubjectMoney { Subject = "本年累计已交所得税", Credit_Debit = "", Money = total_Deliver_SDTax };
                            res.TotalYearTax = new SubjectMoney { Subject = "本年累计已交税金合计", Credit_Debit = "", Money = total_ZZTax + total_LocalSJ + total_Deliver_SDTax };
                        }
                        else
                        {
                            res.XXTax = new SubjectMoney { Subject = "本月已入账销项税", Credit_Debit = "贷", Money = xxTax };
                            res.UnCal_XXTax = new SubjectMoney { Subject = "本月未入账销项税", Credit_Debit = "贷", Money = unCalXXTax };
                            res.XXTax_TOTAL = new SubjectMoney { Subject = "本月账销项税合计", Credit_Debit = "贷", Money = xxTax + unCalJXTax };
                            res.JXTax = new SubjectMoney { Subject = "本月已入账进项税", Credit_Debit = "借", Money = jxTax };
                            res.UnCal_JXTax = new SubjectMoney { Subject = "本月未入账的进项税", Credit_Debit = "借", Money = unCalJXTax };
                            res.JXTax_TOTAL = new SubjectMoney { Subject = "本月账进项税合计", Credit_Debit = "贷", Money = jxTax + unCalJXTax };
                            //add by Hero.Zhang 添加留抵项
                            res.Pre_LiuDi = new SubjectMoney { Subject = "上期留抵", Credit_Debit = "", Money = Math.Abs(LiuDi) };
                            res.Pre_ZZTax = new SubjectMoney { Subject = "本月预计应交增值税", Credit_Debit = "贷", Money = VaTax };
                            res.LocalSJ = new SubjectMoney { Subject = "本月预计应交地方税金", Credit_Debit = "贷", Money = localSJ };
                            res.Cal_TotalSDTax = new SubjectMoney { Subject = "本年已入账的累计应交所得税", Credit_Debit = "", Money = yearTaxTotal };
                            res.Total_Deliver_SDTax = new SubjectMoney { Subject = "本年累计已交所得税", Credit_Debit = "", Money = total_Deliver_SDTax };
                            res.Pre_SDTax = new SubjectMoney { Subject = "本月预计应交所得税", Credit_Debit = "", Money = preSDTax };

                        }
                    }
                    ts.Commit();
                    res.Account = account.QY_Name;
                    res.IsSuccess = true;
                    res.Message = "计算完成";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseSJGet;
                }
            }
        }

        //public ResponseSJGet WX_SJGet(RequestSJGet request)
        //{
        //    var res = new ResponseSJGet();
        //    using (cnn = GetConnection())
        //    {
        //        var ts = cnn.BeginTransaction();
        //        try
        //        {
        //            AccountSetBLL bll = new AccountSetBLL(cnn);
        //            PeriodBLL periodBLL = new FAS.PeriodBLL(cnn);
        //            var account = bll.AccountInfoGet(request.AccountId, ts);
        //            //string sql_Period = "select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId and Id=@PeriodId order by startDate";
        //            //var curPeriod = cnn.Query<TKS_FAS_MonthPeriodInfo>(sql_Period, new { AccountId = request.AccountId, PeriodId = request.PeriodId }, ts).FirstOrDefault();
        //            var curPeriod = periodBLL.GetPeriod(request.PeriodId, ts);

        //            //var curPeriod = bll.GetActivePeriod(request.AccountId, ts);

        //            var prePeriod = periodBLL.GetPrePeriod(curPeriod, ts);
        //            if (prePeriod == null)
        //            {
        //                res.IsSuccess = false;
        //                res.Message = "当前账套无上一个期间";

        //                return res;
        //            }
        //            string prePeriodId = "";
        //            if (prePeriod != null)
        //            {
        //                prePeriodId = prePeriod.Id;
        //            }
        //            string curPeriodId = curPeriod.Id;
        //            //销项税
        //            string sql = @"select sum(Money_Credit) Money_Credit,sum(Money_Debit) Money_Debit
        //                            from tks_fas_docDetail where subjectCode='217100105' 
        //                          and periodId=@PeriodId";
        //            var xxData = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
        //                new { PeriodId = curPeriodId }, ts);

        //            decimal xxTax = xxData.Money_Credit - xxData.Money_Debit;//贷方 - 借方

        //            sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
        //                    where accountId=@AccountId and  pzid is null and type=25 "; //销售发票
        //            var unCalXXTaxData = cnn.ExecuteScalar(sql, new { AccountId = request.AccountId }, ts);
        //            decimal unCalXXTax = 0;
        //            if (unCalXXTaxData != null)
        //            {
        //                unCalXXTax = decimal.Parse(unCalXXTaxData.ToString());
        //            }

        //            //进项税
        //            sql = @"select sum(Money_Credit) Money_Credit,sum(Money_Debit) Money_Debit
        //                        from tks_fas_docDetail where subjectCode='217100101' 
        //                          and periodId=@PeriodId";
        //            var jxData = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
        //                new { PeriodId = curPeriodId }, ts);
        //            decimal jxTax = jxData.Money_Debit - jxData.Money_Credit;//借方 - 贷方

        //            sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
        //                    where accountId=@AccountId and  pzid is null and type<>25 "; //除销售发票
        //            var unCalJXTaxData = cnn.ExecuteScalar(sql, new { AccountId = request.AccountId }, ts);
        //            decimal unCalJXTax = 0;
        //            if (unCalJXTaxData != null)
        //            {
        //                unCalJXTax = decimal.Parse(unCalJXTaxData.ToString());
        //            }

        //            decimal preZZTax = xxTax + unCalXXTax - jxTax - unCalJXTax;//销项汇总-进项汇总
        //            decimal LiuDi = GetLiuDi(request.AccountId, curPeriod, ts);
        //            decimal localSJ = (preZZTax + LiuDi) * account.TaxRate / 100m;
        //            if (localSJ < 0)
        //            {
        //                localSJ = 0;
        //            }
        //            var balance = GetData(prePeriodId, curPeriodId, ts);

        //            sql = "select * from TKS_FAS_ReportDetailTPL where accountId=@AccountId order by seq";
        //            //模板
        //            List<TKS_FAS_LRReport> tpl = cnn.Query<TKS_FAS_LRReport>(sql,
        //                new
        //                {
        //                    AccountId = request.AccountId

        //                }, ts).ToList();
        //            sql = "select * from tks_fas_formula where accountId=@AccountId";

        //            List<TKS_FAS_Formula> formula = cnn.Query<TKS_FAS_Formula>(sql, new { AccountId = request.AccountId }, ts).ToList();

        //            List<TKS_FAS_LRReport> lrReport = GenLRReport(tpl, balance, formula, ts);

        //            var sdTax = lrReport.Where(p => p.Category == 22).FirstOrDefault();
        //            decimal calTotalSDTax = 0;
        //            if (sdTax != null)
        //            {
        //                calTotalSDTax = sdTax.Money_Year;
        //            }


        //            //本年累计已交所得税
        //            decimal total_Deliver_SDTax = WX_DeliverSDTax(request.AccountId, ts);

        //            //sql = @"select * from TKS_FAS_AccountInfo where id=@Id";

        //            // var account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(sql, new {Id=user.AccountId }, ts);

        //            ts.Commit();
        //            res.XXTax = new SubjectMoney { Subject = "本月销项税", Credit_Debit = "贷", Money = xxTax };
        //            res.UnCal_XXTax = new SubjectMoney { Subject = "已提交发票未入账的销项税", Credit_Debit = "贷", Money = unCalXXTax };
        //            res.JXTax = new SubjectMoney { Subject = "本月进项税", Credit_Debit = "借", Money = jxTax };
        //            res.UnCal_JXTax = new SubjectMoney { Subject = "已提交发票未入账的进项税", Credit_Debit = "借", Money = unCalJXTax };
        //            //add by Hero.Zhang 添加留抵项
        //            res.Pre_LiuDi = new SubjectMoney { Subject = "上期留抵", Credit_Debit = "", Money = Math.Abs(LiuDi) };
        //            res.Pre_ZZTax = new SubjectMoney { Subject = "本月预计应交增值税", Credit_Debit = "贷", Money = preZZTax + LiuDi };
        //            res.LocalSJ = new SubjectMoney { Subject = "本月应交地方税金", Credit_Debit = "贷", Money = localSJ };
        //            res.Cal_TotalSDTax = new SubjectMoney { Subject = "本年已入账的累计应交所得税", Credit_Debit = "", Money = calTotalSDTax * account.TaxRate / 100m };

        //            res.Total_Deliver_SDTax = new SubjectMoney { Subject = "本年累计已交所得税", Credit_Debit = "", Money = total_Deliver_SDTax };
        //            var preSDTax = calTotalSDTax * account.TaxRate / 100m - total_Deliver_SDTax;
        //            if (preSDTax < 0)
        //            {
        //                preSDTax = 0;
        //            }
        //            res.Pre_SDTax = new SubjectMoney { Subject = "本月预计应交所得税", Credit_Debit = "", Money = preSDTax };

        //            res.Account = account.QY_Name;
        //            res.IsSuccess = true;
        //            res.Message = "计算完成";
        //            return res;
        //        }
        //        catch (Exception ex)
        //        {
        //            ts.Rollback();
        //            return this.DealException(res, ex) as ResponseSJGet;
        //        }
        //    }
        //}

        public decimal GetLiuDi(string accountId, TKS_FAS_MonthPeriodInfo curPeriod, IDbTransaction ts)
        {
            decimal value = 0;
            PeriodBLL periodBLL = new PeriodBLL(ts.Connection);
            var period = periodBLL.GetPrePeriods(accountId, curPeriod.Year, curPeriod.Month, ts);
            //var period = cnn.Query<TKS_FAS_MonthPeriodInfo>(
            //    @"select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId and IsPay=1 order by StartDate",
            //    new { AccountId = accountId }, ts).ToList();
            if (period.Count > 0)
            {
                foreach (var item in period)
                {
                    string curPeriodId = item.Id;
                    //销项税
                    string sql = @"select sum(Money_Credit) Money_Credit,sum(Money_Debit) Money_Debit
                                    from tks_fas_docDetail where subjectCode='217100105' 
                                  and periodId=@PeriodId";
                    var xxData = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
                        new { PeriodId = curPeriodId }, ts);

                    decimal xxTax = xxData.Money_Credit - xxData.Money_Debit;//贷方 - 借方

                    sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
                            where accountId=@AccountId and  pzid is null and type=25 "; //销售发票
                    var unCalXXTaxData = cnn.ExecuteScalar(sql, new { AccountId = accountId }, ts);
                    decimal unCalXXTax = 0;
                    if (unCalXXTaxData != null)
                    {
                        unCalXXTax = decimal.Parse(unCalXXTaxData.ToString());
                    }

                    //进项税
                    sql = @"select sum(Money_Credit) Money_Credit,sum(Money_Debit) Money_Debit
                                from tks_fas_docDetail where subjectCode='217100101' 
                                  and periodId=@PeriodId";
                    var jxData = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
                        new { PeriodId = curPeriodId }, ts);
                    decimal jxTax = jxData.Money_Debit - jxData.Money_Credit;//借方 - 贷方

                    sql = @"select sum(taxMoney) as taxMoney from TKS_FAS_Invoice 
                            where accountId=@AccountId and  pzid is null and type<>25 "; //除销售发票
                    var unCalJXTaxData = cnn.ExecuteScalar(sql, new { AccountId = accountId }, ts);
                    decimal unCalJXTax = 0;
                    if (unCalJXTaxData != null)
                    {
                        unCalJXTax = decimal.Parse(unCalJXTaxData.ToString());
                    }

                    decimal preZZTax = jxTax + unCalJXTax - (xxTax + unCalXXTax);//进项汇总 - 销项汇总
                    value += preZZTax;
                    //if (preZZTax > 0)
                    //{
                    //    value += preZZTax;
                    //}
                    if (value <= 0)
                    {
                        value = 0;
                    }
                }
            }
            else
            {
                string sql = @"select sum(case SCredit_Debit when  0 then BWBStartBAL else -1*BWBStartBAL end) liudi from TKS_FAS_GLBalance where SubjectCode in (
select Code from TKS_FAS_AccountSubject where ParentId in(select  Id from TKS_FAS_AccountSubject where Code ='2171001' and AccountId=@AccountId) and AccountId=@AccountId) and periodId='' and AccountId=@AccountId";
                var data = cnn.ExecuteScalar(sql, new { AccountId = accountId }, ts);

                value = decimal.Parse(data.ToString());
                if (value <= 0)
                {
                    value = 0;
                }
            }
            return value;
        }
        private List<TKS_FAS_LRReport> GenLRReport(List<TKS_FAS_LRReport> data,
            List<TKS_FAS_GLBalanceExt> balance, List<TKS_FAS_Formula> formula, IDbTransaction ts)
        {


            for (var i = 0; i < data.Count; i++)
            {

                if (data[i].SourceType == 0)
                {

                    data[i].Money_Month = CalculateMoneyMonth(data[i].SourceValue, data[i].Id, balance, formula, ts);

                    data[i].Money_Year = CalculateMoneyYear(data[i].SourceValue, data[i].Id, balance, formula, ts);
                }
            }


            for (var i = 0; i < data.Count; i++)
            {

                if (data[i].SourceType == 1)
                {
                    data[i].Money_Month = SumMoneyMonth(data[i].Category, data);

                    data[i].Money_Year = SumMoneyYear(data[i].Category, data);
                }
            }

            return data;
        }

        private decimal CalculateMoneyYear(string type, string id, List<TKS_FAS_GLBalanceExt> balance,
            List<TKS_FAS_Formula> formula, IDbTransaction ts)
        {
            SubjectBLL bll = new SubjectBLL(cnn);
            var fun = formula.Where(p => p.ReportDetailTPLId == id).ToList();
            decimal result = 0;
            foreach (var item in fun)
            {
                var bal = balance.Where(p => p.SubjectCode == item.SubjectCode).First();
                if (bal == null)
                    continue;
                decimal val = 0;

                var subject = bll.GetSubject(item.SubjectCode, item.AccountId, ts);
                if (subject.Credit_Debit == 1)
                {
                    val = bal.BWBCreditTotal_Y - bal.BWBDebitTotal_Y;// 贷方减去借方
                }
                else
                {
                    val = bal.BWBDebitTotal_Y - bal.BWBCreditTotal_Y;// 用借方减去贷方
                }
                if (item.OperatorCharacter == "+")
                {

                    result += val;
                }
                else if (item.OperatorCharacter == "-")
                {
                    result -= val;
                }
            }

            return result;
        }

        private decimal CalculateMoneyMonth(string type, string id, List<TKS_FAS_GLBalanceExt> balance,
            List<TKS_FAS_Formula> formula, IDbTransaction ts)
        {
            SubjectBLL bll = new SubjectBLL(cnn);

            var fun = formula.Where(p => p.ReportDetailTPLId == id).ToList();
            decimal result = 0;
            foreach (var item in fun)
            {
                var bal = balance.Where(p => p.SubjectCode == item.SubjectCode).First();

                if (bal == null)
                    continue;
                decimal val = 0;

                var subject = bll.GetSubject(item.SubjectCode, item.AccountId, ts);
                if (subject.Credit_Debit == 1)
                {
                    val = bal.BWBCreditTotal - bal.BWBDebitTotal;// 贷方减去借方
                }
                else
                {
                    val = bal.BWBDebitTotal - bal.BWBCreditTotal;// 用借方减去贷方
                }

                if (item.OperatorCharacter == "+")
                {

                    result += val;
                }
                else if (item.OperatorCharacter == "-")
                {
                    result -= val;
                }
            }

            return result;
        }


        private decimal SumMoneyYear(int category, List<TKS_FAS_LRReport> data)
        {
            List<TKS_FAS_LRReport> fd = new List<TKS_FAS_LRReport>();
            //根据category分别取计算列
            if (category == 21)
            {
                fd = data.Where(p => p.Category == 20 && p.SourceType == 0).ToList();
            }
            else if (category == 22)
            {
                fd = (from item in data
                      where item.SourceType == 0 && (item.Category == 20 || item.Category == 21)
                      select item).ToList();
            }
            else if (category == 23)
            {
                fd = (from item in data
                      where item.SourceType == 0 && (item.Category == 20 || item.Category == 21 || item.Category == 22)
                      select item).ToList();
            }

            decimal result = 0;
            foreach (var item in fd)
            {
                if (item.OperatorCharacter == "+")
                    result += item.Money_Year;
                else if (item.OperatorCharacter == "-")
                    result -= item.Money_Year;


            }
            return result;
        }

        private decimal SumMoneyMonth(int category, List<TKS_FAS_LRReport> data)
        {
            List<TKS_FAS_LRReport> fd = new List<TKS_FAS_LRReport>();
            //根据category分别取计算列
            if (category == 21)
            {
                fd = data.Where(p => p.Category == 20 && p.SourceType == 0).ToList();
            }
            else if (category == 22)
            {
                fd = (from item in data
                      where item.SourceType == 0 && (item.Category == 20 || item.Category == 21)
                      select item).ToList();
            }
            else if (category == 23)
            {
                fd = (from item in data
                      where item.SourceType == 0 && (item.Category == 20 || item.Category == 21 || item.Category == 22)
                      select item).ToList();
            }

            decimal result = 0;
            foreach (var item in fd)
            {
                if (item.OperatorCharacter == "+")
                    result += item.Money_Month;
                else if (item.OperatorCharacter == "-")
                    result -= item.Money_Month;

            }
            return result;
        }
    }
}
