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

namespace TKS.FAS.BLL.FAS.Report
{
    /// <summary>
    /// 经营报表
    /// </summary>
    public class ReportBLL3 : CommonBase
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
        /// <summary>
        /// 本月客户收款金额
        /// </summary>
        /// <param name="period"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        List<SubjectMoney> CurMonthKHSK(TKS_FAS_MonthPeriodInfo period, IDbTransaction ts)
        {
            List<SubjectMoney> res = new List<SubjectMoney>();
            //本月的凭证
            string sql = @"select * from tks_fas_doc where periodId=@PeriodId";

            var lsDoc = cnn.Query<TKS_FAS_Doc>(sql, new {PeriodId=period.Id }, ts).ToList();

            sql = @"SELECT A.ParentId,A.AccountId,A.SubjectCode,sum(A.Money_Credit) as Money_Credit ,
                    sum(A.Money_Debit) as Money_Debit,A.CalItem1,A.CalValue1 
                    FROM  TKS_FAS_DocDetail A 
                    where  periodId=@PeriodId 
					group by A.ParentId,A.AccountId,A.SubjectCode,A.CalItem1,A.CalValue1  ";

            var lsDocDetail = cnn.Query<TKS_FAS_DocDetailExt>(sql, new {PeriodId=period.Id }, ts).ToList();

            sql = "select * from TKS_FAS_CaculateHelperDetail where accountId=@AccountId";

            var calHelper = cnn.Query<TKS_FAS_CaculateHelperDetail>(sql, 
                new { AccountId=period.AccountId }, ts).ToList ();


            Dictionary<string, SubjectMoney> dicRes = new Dictionary<string, SubjectMoney>();
            foreach(var doc in lsDoc)
            {
                var detail = lsDocDetail.Where(p => p.ParentId == doc.Id).ToList();

                var flag1 = detail.Any(p => { return p.SubjectCode == "1002" && p.Credit_Debit == 0; });

                var flag2 = detail.Any(p => { return p.SubjectCode == "1131" && p.Credit_Debit == 1&&p.CalItem1!=null; });

                if (flag1 && flag2)
                {
                    var d = detail.Where(p => { return p.SubjectCode == "1131" && p.Credit_Debit == 1; }).ToList ();
                    if (d.Count ()==0) continue;

                    foreach (var item in d)
                    {
                        if (dicRes.ContainsKey(item.CalValue1))
                        {
                            dicRes[item.CalValue1].Money += item.Money_Credit;//累计贷方金额
                        }
                        else
                        {
                            var h = calHelper.Where(p => p.Code == item.CalValue1).SingleOrDefault();
                            item.Name =h==null ?"": h.Name;

                            dicRes.Add(item.CalValue1, new SubjectMoney
                            {
                                Subject = "客户收款-" + item.Name,
                                Item = "本月客户收款金额",
                                Credit_Debit = "贷",
                                Money = item.Money_Credit
                            });


                        }
                    }
                   
                }
 
            }

            foreach (var item in dicRes)
            {
                res.Add(dicRes[item.Key]);
            }

            res.Sort();
            return res;
        }

        /// <summary>
        /// 本月支付供应商款项金额
        /// </summary>
        /// <param name="period"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        List<SubjectMoney> PaySupplier(TKS_FAS_MonthPeriodInfo period,IDbTransaction ts)
        {
            //贷方，银行存款
            //借方，应付账款
            List<SubjectMoney> res = new List<SubjectMoney>();
            //本月的凭证
            string sql = @"select * from tks_fas_doc where periodId=@PeriodId";

            var lsDoc = cnn.Query<TKS_FAS_Doc>(sql, new { PeriodId = period.Id }, ts).ToList();


            sql = @"SELECT A.ParentId,A.AccountId,A.SubjectCode,sum(A.Money_Credit) as Money_Credit ,
                    sum(A.Money_Debit) as Money_Debit,A.CalItem1,A.CalValue1 
                    FROM  TKS_FAS_DocDetail A 
                    where  periodId=@PeriodId 
					group by A.ParentId,A.AccountId,A.SubjectCode,A.CalItem1,A.CalValue1  ";

            var lsDocDetail = cnn.Query<TKS_FAS_DocDetailExt>(sql, new { PeriodId = period.Id }, ts).ToList();

            sql = "select * from TKS_FAS_CaculateHelperDetail where accountId=@AccountId";

            var calHelper = cnn.Query<TKS_FAS_CaculateHelperDetail>(sql,
                new { AccountId = period.AccountId }, ts).ToList();

            Dictionary<string, SubjectMoney> dicRes = new Dictionary<string, SubjectMoney>();
            foreach (var doc in lsDoc)
            {
                var detail = lsDocDetail.Where(p => p.ParentId == doc.Id).ToList();

                var flag1 = detail.Any(p => { return p.SubjectCode == "1002" && p.Credit_Debit == 1; });

                var flag2 = detail.Any(p => { return p.SubjectCode == "2121" && p.Credit_Debit == 0 && p.CalItem1 != null; });

                if (flag1 && flag2)
                {
                    var d = detail.Where(p => { return p.SubjectCode == "2121" && p.Credit_Debit == 0; }).ToList ();
                    if (d.Count () == 0) continue;

                    foreach (var item in d)
                    {
                        if (dicRes.ContainsKey(item.CalValue1))
                        {
                            dicRes[item.CalValue1].Money += item.Money_Debit;//累计借方金额
                        }
                        else
                        {
                            var h = calHelper.Where(p => p.Code == item.CalValue1).SingleOrDefault();
                            item.Name = h == null ? "" : h.Name;
                            dicRes.Add(item.CalValue1, new SubjectMoney
                            {
                                Subject = "支付账款-" + item.Name,
                                Item = "本月支付供应商款项金额",
                                Credit_Debit = "借",
                                Money = item.Money_Debit
                            });


                        }
                    }
                 
                }

            }

            foreach (var item in dicRes)
            {
                res.Add(dicRes[item.Key]);
            }

            res.Sort();
            return res;

        }

        /// <summary>
        /// 本月供应商采购金额
        /// </summary>
        /// <param name="period"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        List<SubjectMoney> CG(TKS_FAS_MonthPeriodInfo period,IDbTransaction ts)
        {
            //贷方，不为银行存款
            //应付账款贷方-应付账款借方
            List<SubjectMoney> res = new List<SubjectMoney>();
            //本月的凭证
            string sql = @"select * from tks_fas_doc where periodId=@PeriodId";

            var lsDoc = cnn.Query<TKS_FAS_Doc>(sql, new { PeriodId = period.Id }, ts).ToList();


            sql = @"SELECT A.ParentId,A.AccountId,A.SubjectCode,sum(A.Money_Credit) as Money_Credit ,
                    sum(A.Money_Debit) as Money_Debit,A.CalItem1,A.CalValue1 
                    FROM  TKS_FAS_DocDetail A 
                    where  periodId=@PeriodId 
					group by A.ParentId,A.AccountId,A.SubjectCode,A.CalItem1,A.CalValue1  ";

            var lsDocDetail = cnn.Query<TKS_FAS_DocDetailExt>(sql, new { PeriodId = period.Id }, ts).ToList();

            sql = "select * from TKS_FAS_CaculateHelperDetail where accountId=@AccountId";

            var calHelper = cnn.Query<TKS_FAS_CaculateHelperDetail>(sql,
                new { AccountId = period.AccountId }, ts).ToList();


            Dictionary<string, SubjectMoney> dicRes = new Dictionary<string, SubjectMoney>();
            foreach (var doc in lsDoc)
            {
                var detail = lsDocDetail.Where(p => p.ParentId == doc.Id).ToList();

                var flag1 = detail.Any(p => { return p.SubjectCode != "1002" && p.Credit_Debit == 1; });

                var flag2 = detail.Any(p => { return (p.SubjectCode == "2121" && p.Credit_Debit == 0)||
                    (p.SubjectCode == "2121" && p.Credit_Debit == 1); });

                if (flag1 && flag2)
                {
                    var d = detail.Where(p => { return
                        (p.SubjectCode == "2121" && p.Credit_Debit == 0) ||
                        (p.SubjectCode == "2121" && p.Credit_Debit == 1);
                    }).ToList ();
                    if (d.Count () == 0) continue;

                    foreach (var item in d)
                    {
                        if (dicRes.ContainsKey(item.CalValue1))
                        {
                            dicRes[item.CalValue1].Money += item.Money_Credit - item.Money_Debit;//累计借方金额
                        }
                        else
                        {
                            var h = calHelper.Where(p => p.Code == item.CalValue1).SingleOrDefault();
                            item.Name = h == null ? "" : h.Name;
                            dicRes.Add(item.CalValue1, new SubjectMoney
                            {
                                Subject = "采购-" + item.Name,
                                Item = "本月供应商采购金额",
                                Credit_Debit = "贷",
                                Money = item.Money_Credit - item.Money_Debit
                            });


                        }
                    }
                   
                }

            }

            foreach (var item in dicRes)
            {
                res.Add(dicRes[item.Key]);
            }

            res.Sort();
            return res;
        }

        public ResponseJYGet JYGet(RequestJYGet request)
        {
            var res = new ResponseJYGet();

            using (this.cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    AccountSetBLL accountBLL = new AccountSetBLL(cnn);
                    var curPeriod = accountBLL.GetActivePeriod(request.AccountList ?? user.AccountId, ts);
                    curPeriod.Id = request.PeriodId;
                    
                    //本月开票收入
                    //string sql = @"SELECT  (sum(Money_Credit)-sum(Money_Debit)) as money 
                    //                FROM     TKS_FAS_DocDetail where 
                    //                periodId=@PeriodId and subjectCode='5101' 
                    string sql = @"SELECT CASE 
		                                    WHEN SCredit_Debit = 0 THEN ABS(BWBDebitTotal - BWBCreditTotal)
		                                    ELSE ABS(BWBCreditTotal - BWBDebitTotal)
	                                    END AS money
                                    FROM tks_fas_glBalance
                                    WHERE periodId = @PeriodId
	                                    AND subjectCode = '5101'";
                    var kp = cnn.ExecuteScalar(sql, new { PeriodId = curPeriod.Id }, ts);
                    
                    var kpMoney = decimal.Parse(kp==null?"0":kp.ToString());
                    //add by Hero.Zhang 小规模类型的账套，本季度开票收入
                    #region
                    decimal quarter_kpMoney = 0;
                    var data_Account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(
                @"select * from TKS_FAS_AccountInfo where Id=@Id",
                new { Id = user.AccountId }, ts);
                    if (data_Account.AddedValueTaxType==1)
                    {
                        var selPeriod = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                @"select * from TKS_FAS_MonthPeriodInfo where Id=@Id",
                new { Id = request.PeriodId }, ts);

                        var lstPeriod = cnn.Query<TKS_FAS_MonthPeriodInfo>(@"select * from TKS_FAS_MonthPeriodInfo where 
  AccountId=@AccountId and Year=@Year", new { AccountId = user.AccountId, Year = curPeriod .Year}, ts).ToList();
                        List<string> lstFirst = new List<string>();
                        List<string> lstSecond = new List<string>();
                        List<string> lstThird= new List<string>();
                        List<string> lstFour = new List<string>();
                        foreach (var item in lstPeriod)
                        {
                            if (item.Month<=3)
                            {
                                lstFirst.Add(item.Id);
                            }
                            else if (item.Month > 3&& item.Month<=6)
                            {
                                lstSecond.Add(item.Id);
                            }
                            else if (item.Month > 6 && item.Month <= 9)
                            {
                                lstThird.Add(item.Id);
                            }
                            else if (item.Month > 9 && item.Month <= 12)
                            {
                                lstFour.Add(item.Id);
                            }
                        }
                        var month = selPeriod.Month;
                        var Quarter = GetQuarterByMonth(month);
                        string sql_quarter = @"SELECT CASE 
		                                    WHEN SCredit_Debit = 0 THEN ABS(BWBDebitTotal - BWBCreditTotal)
		                                    ELSE ABS(BWBCreditTotal - BWBDebitTotal)
	                                    END AS money
                                    FROM tks_fas_glBalance
                                    WHERE periodId = @PeriodId
	                                    AND subjectCode = '5101'";
                        if (Quarter == 1)
                        {
                            foreach (var item in lstFirst)
                            {                             
                                var kp_quarter = cnn.ExecuteScalar(sql_quarter, new { PeriodId = item }, ts);

                                var kpMoney_quarter = decimal.Parse(kp_quarter == null ? "0" : kp_quarter.ToString());
                                quarter_kpMoney += kpMoney_quarter;
                            }
                        }
                        if (Quarter == 2)
                        {
                            foreach (var item in lstSecond)
                            {
                                var kp_quarter = cnn.ExecuteScalar(sql_quarter, new { PeriodId = item }, ts);

                                var kpMoney_quarter = decimal.Parse(kp_quarter == null ? "0" : kp_quarter.ToString());
                                quarter_kpMoney += kpMoney_quarter;
                            }
                        }
                        if (Quarter == 3)
                        {
                            foreach (var item in lstThird)
                            {
                                var kp_quarter = cnn.ExecuteScalar(sql_quarter, new { PeriodId = item }, ts);

                                var kpMoney_quarter = decimal.Parse(kp_quarter == null ? "0" : kp_quarter.ToString());
                                quarter_kpMoney += kpMoney_quarter;
                            }
                        }
                        if (Quarter == 4)
                        {
                            foreach (var item in lstFour)
                            {
                                var kp_quarter = cnn.ExecuteScalar(sql_quarter, new { PeriodId = item }, ts);

                                var kpMoney_quarter = decimal.Parse(kp_quarter == null ? "0" : kp_quarter.ToString());
                                quarter_kpMoney += kpMoney_quarter;
                            }
                        }
                    }
                    #endregion
                    PeriodBLL periodBLL = new PeriodBLL(cnn);

                    //本年累计开票收入

                    decimal kpMoneyYear = 0;
                    sql = @"SELECT CASE 
		                            WHEN SCredit_Debit = 0 THEN ABS(BWBDebitTotal_Y - BWBCreditTotal_Y)
		                             ELSE ABS(BWBCreditTotal_Y - BWBDebitTotal_Y)
	                                 END AS money
                                    FROM tks_fas_glBalance
                                    WHERE periodId = @PeriodId
	                                    AND subjectCode = '5101'";
                    var preKp = cnn.ExecuteScalar(sql, new { PeriodId = curPeriod.Id }, ts);

                    var preKPMoney = decimal.Parse(preKp == null ? "0" : preKp.ToString());
                    //kpMoneyYear = preKPMoney + kpMoney;
                    kpMoneyYear = preKPMoney;
                    //var prePeriod = periodBLL.GetPrePeriod(curPeriod, ts);
                    //if (prePeriod!=null)
                    //{
                    //    //sql = @"select  BWBCreditTotal from tks_fas_glBalance
                    //    //    where subjectCode='5101' and periodId=@PeriodId";
                    //    sql = @"select  BWBEndBAL from tks_fas_glBalance
                    //        where subjectCode='5101' and periodId=@PeriodId";
                    //    var preKp = cnn.ExecuteScalar(sql, new { PeriodId = prePeriod.Id }, ts);

                    //    var preKPMoney = decimal.Parse(preKp == null ? "0" : preKp.ToString());
                    //    //kpMoneyYear = preKPMoney + kpMoney;
                    //    kpMoneyYear = preKPMoney;
                    //}
                    //else
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


                    //sql = @"select K1.SubjectCode,K1.name,( K1.BWBDebitTotal+K2.money) as BWBEndBAL  from (
                    //        SELECT  A.SubjectCode,B.name,  A.BWBDebitTotal
                    //          FROM  TKS_FAS_GLBalance A left join TKS_FAS_AccountSubject B 
                    //          on A.SubjectCode=B.Code and A.AccountId=B.AccountId

                    //           where A.PeriodId=@PrePeriodId and B.SLevel=2
                    //          and B.RootCode='1002') K1

                    //          left join
                    //          (
                    //          select A.SubjectCode,B.Name,(sum(Money_Debit)- sum(Money_Credit)) as money  from TKS_FAS_DocDetail A left join TKS_FAS_AccountSubject B 
                    //          on A.SubjectCode=B.Code and A.AccountId=B.AccountId  
                    //          where B.RootCode='1002' and B.SLevel=2  and A.PeriodId=@CurPeriodId
                    //          group by A.SubjectCode,B.Name) K2 on K1.SubjectCode=K2.SubjectCode";
                    sql = @"SELECT  A.SubjectCode,B.name,  A.BWBEndBAL
                              FROM  TKS_FAS_GLBalance A left join TKS_FAS_AccountSubject B 
                              on A.SubjectCode=B.Code and A.AccountId=B.AccountId

                               where A.PeriodId=@CurPeriodId and B.SLevel=2
                              and B.RootCode='1002'";

                    var lsBank = cnn.Query<TKS_FAS_GLBalance>(sql,
                        new { CurPeriodId = curPeriod.Id }, ts).ToList();
                    List<SubjectMoney> banks = new List<SubjectMoney>();
                    decimal Bank_Main = 0;
                    foreach (var bank in lsBank)
                    {
                        banks.Add(new SubjectMoney
                        {
                            Item = "本月银行存款余额",
                            Credit_Debit = "借",
                            Money = bank.BWBEndBAL,
                            Subject = bank.SubjectCode+" "+bank.Name
                        });
                        Bank_Main += bank.BWBEndBAL;
                    }
                    //首页银行存款余额

                    var Bank_MainMoney = Bank_Main;
                    //本月应收账款余额
                    #region 本月应收账款余额
                    //sql = @"select k1.subjectCode,k1.CalValue1,k3.Name   , 
                    //        (k1.bwbEndBal_J+(case when k2.bwbEndBal is null then 0 else k2.bwbEndBal end )) as bwbEndBal  from (
                    //        select SubjectCode ,CalItem1,CalValue1, 

                    //        sum(case when ECredit_Debit=0 then bwbEndBal else 0 end) bwbEndBal_J,
                    //        sum(case when ECredit_Debit=1 then BWBEndBAL else 0 end) bwbEndBal_D

                    //        from TKS_FAS_FGLBalance where periodId=@PrePeriodId and SubjectCode='1131'

                    //        group by SubjectCode ,CalItem1,CalValue1) k1

                    //        left join (

                    //        select SubjectCode,CalItem1,CalValue1,

                    //        sum(Money_Debit)-sum(Money_Credit) as bwbEndBal

                    //        from TKS_FAS_DocDetail 

                    //        where periodId=@CurPeriodId and SubjectCode='1131'
                    //        group by SubjectCode ,CalItem1,CalValue1) k2
                    //        on k1.CalValue1=k2.CalValue1
                    //        left join TKS_FAS_CaculateHelperDetail k3
                    //        on ( k1.CalValue1=k3.Code) and k1.CalItem1=k3.parentId order by k1.CalValue1";
                    sql = @"select k1.subjectCode,k1.CalValue1,k3.Name   , 
                            (k1.bwbEndBal_J-k1.bwbEndBal_D) as bwbEndBal  from (
                            select SubjectCode ,CalItem1,CalValue1, 

                            sum(case when ECredit_Debit=0 then bwbEndBal else 0 end) bwbEndBal_J,
                            sum(case when ECredit_Debit=1 then BWBEndBAL else 0 end) bwbEndBal_D

                            from TKS_FAS_FGLBalance where periodId=@CurPeriodId and SubjectCode like'1131%'

                            group by SubjectCode ,CalItem1,CalValue1) k1

                            left join TKS_FAS_CaculateHelperDetail k3
                            on ( k1.CalValue1=k3.Code) and k1.CalItem1=k3.parentId order by k1.CalValue1";
                    var ls1131 = cnn.Query<TKS_FAS_FGLBalance>(sql, 
                        new {CurPeriodId=curPeriod.Id }, ts).ToList ();
                    List<SubjectMoney> ar = new List<SubjectMoney>();
                    decimal ar_Main = 0;
                    foreach (var item in ls1131)
                    {
                        ar.Add(new SubjectMoney {
                             Credit_Debit="借",
                             Item= "本月应收账款余额",
                             Subject="应收账款 "+ "-"+item.Name,
                             Money=item.BWBEndBAL
                        });
                        ar_Main += item.BWBEndBAL;
                    }
                    //首页应收账款余额

                    var ar_MainMoney = ar_Main;
                    #endregion


                    #region 本月客户收款金额
                    //借方，银行存款
                    //贷方，应收账款客户
                    var lsKHMoney = CurMonthKHSK(curPeriod, ts);

                    #endregion

                    #region 本月应付账款余额

                    //sql = @"select k1.subjectCode,k1.CalValue1,k3.Name   , 
                    //        (k1.bwbEndBal_D+(case when k2.bwbEndBal is null then 0 else k2.bwbEndBal end )) as bwbEndBal  from (
                    //        select SubjectCode ,CalItem1,CalValue1, 

                    //        sum(case when ECredit_Debit=0 then bwbEndBal else 0 end) bwbEndBal_J,
                    //        sum(case when ECredit_Debit=1 then BWBEndBAL else 0 end) bwbEndBal_D

                    //        from TKS_FAS_FGLBalance where periodId=@PrePeriodId and SubjectCode='2121'

                    //        group by SubjectCode ,CalItem1,CalValue1) k1

                    //        left join (

                    //        select SubjectCode,CalItem1,CalValue1,

                    //       sum(Money_Credit)- sum(Money_Debit) as bwbEndBal

                    //        from TKS_FAS_DocDetail 

                    //        where periodId=@CurPeriodId and SubjectCode='2121'
                    //        group by SubjectCode ,CalItem1,CalValue1) k2
                    //        on k1.CalValue1=k2.CalValue1
                    //        left join TKS_FAS_CaculateHelperDetail k3
                    //        on ( k1.CalValue1=k3.Code) and k1.CalItem1=k3.parentId order by k1.CalValue1";

                    sql = @"select k1.subjectCode,k1.CalValue1,k3.Name   , 
                            (k1.bwbEndBal_D-bwbEndBal_J) as bwbEndBal  from (
                            select SubjectCode ,CalItem1,CalValue1, 

                            sum(case when ECredit_Debit=0 then bwbEndBal else 0 end) bwbEndBal_J,
                            sum(case when ECredit_Debit=1 then BWBEndBAL else 0 end) bwbEndBal_D

                            from TKS_FAS_FGLBalance where periodId=@CurPeriodId and SubjectCode like'2121%'

                            group by SubjectCode ,CalItem1,CalValue1) k1

                            left join TKS_FAS_CaculateHelperDetail k3
                            on ( k1.CalValue1=k3.Code) and k1.CalItem1=k3.parentId order by k1.CalValue1";
                    var ls2121 = cnn.Query<TKS_FAS_FGLBalance>(sql,
                        new { CurPeriodId = curPeriod.Id }, ts).ToList();
                    List<SubjectMoney> ap = new List<SubjectMoney>();
                    decimal ap_Main = 0;
                    foreach (var item in ls2121)
                    {
                        ap.Add(new SubjectMoney
                        {
                            Credit_Debit = "贷",
                            Item = "本月应付账款余额",
                            Subject =   "应付账款-" + item.Name,
                            Money = item.BWBEndBAL
                        });
                        ap_Main += item.BWBEndBAL;
                    }
                    //首页应付账款余额


                    var ap_MainMoney = ap_Main;
                    #endregion


                    #region 本月支付供应商款项金额

                    var lsPaySupplier = PaySupplier(curPeriod, ts);
                    #endregion

                    #region 本月供应商采购金额
                    var lsCG = CG(curPeriod, ts);
                    #endregion


                    sql = @"select * from TKS_FAS_AccountInfo where id=@Id";

                    var account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(sql, new { Id = request.AccountList ?? user.AccountId }, ts);


                    ts.Commit();
                    res.Account = account.QY_Name;
                    

                    res.KPMoney = new SubjectMoney { Subject = "本月开票收入", Credit_Debit = "贷", Money = kpMoney };
                    if (data_Account.AddedValueTaxType == 1)
                    {
                        //小规模
                        res.Quarter_KPMoney = new SubjectMoney { Subject = "本季度开票收入", Credit_Debit = "贷", Money = quarter_kpMoney };
                    }
                    res.KPMoneyYear = new SubjectMoney { Subject = "本年累计开票收入", Credit_Debit = "贷", Money = kpMoneyYear };
                    res.BankDepositBal = banks;
                    res.ARAccountBal = ar;
                    res.KHMoney = lsKHMoney;
                    res.APAccountBal = ap;
                    res.PaySupplierMoney = lsPaySupplier;
                    res.CGSupplierMoney = lsCG;
                    res.BankDepositBal_Main = new SubjectMoney { Subject = "银行存款", Money = Bank_MainMoney };
                    res.ARAccountBal_Main = new SubjectMoney { Subject = "应收账款", Money = ar_MainMoney };
                    res.APAccountBal_Main = new SubjectMoney { Subject = "应付账款", Money = ap_MainMoney };
                    res.IsSuccess = true;
                    res.Message = "加载完成";
                
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseJYGet;
                }
            }
        }
       
        //public ResponseJYGet WX_JYGet(RequestJYGet request)
        //{
        //    var res = new ResponseJYGet();

        //    using (this.cnn = GetConnection())
        //    {
        //        var ts = cnn.BeginTransaction();
        //        try
        //        {
        //            AccountSetBLL accountBLL = new AccountSetBLL(cnn);
        //            var curPeriod = accountBLL.GetActivePeriod(request.AccountId, ts);
        //            curPeriod.Id = request.PeriodId;
        //            //本月开票收入
        //            string sql = @"SELECT CASE 
        //                              WHEN SCredit_Debit = 0 THEN ABS(BWBDebitTotal - BWBCreditTotal)
        //                              ELSE ABS(BWBCreditTotal - BWBDebitTotal)
        //                             END AS money
        //                            FROM tks_fas_glBalance
        //                            WHERE periodId = @PeriodId
        //                             AND subjectCode = '5101'";
        //            var kp = cnn.ExecuteScalar(sql, new { PeriodId = curPeriod.Id }, ts);

        //            var kpMoney = decimal.Parse(kp == null ? "0" : kp.ToString());
        //            PeriodBLL periodBLL = new PeriodBLL(cnn);

        //            //本年累计开票收入

        //            decimal kpMoneyYear = 0;
        //            sql = @"SELECT CASE 
        //                      WHEN SCredit_Debit = 0 THEN ABS(BWBDebitTotal_Y - BWBCreditTotal_Y)
        //                       ELSE ABS(BWBCreditTotal_Y - BWBDebitTotal_Y)
        //                          END AS money
        //                            FROM tks_fas_glBalance
        //                            WHERE periodId = @PeriodId
        //                             AND subjectCode = '5101'";
        //            var preKp = cnn.ExecuteScalar(sql, new { PeriodId = curPeriod.Id }, ts);

        //            var preKPMoney = decimal.Parse(preKp == null ? "0" : preKp.ToString());
        //            //kpMoneyYear = preKPMoney + kpMoney;
        //            kpMoneyYear = preKPMoney;

        //            sql = @"SELECT  A.SubjectCode,B.name,  A.BWBEndBAL
        //                      FROM  TKS_FAS_GLBalance A left join TKS_FAS_AccountSubject B 
        //                      on A.SubjectCode=B.Code and A.AccountId=B.AccountId

        //                       where A.PeriodId=@CurPeriodId and B.SLevel=2
        //                      and B.RootCode='1002'";

        //            var lsBank = cnn.Query<TKS_FAS_GLBalance>(sql,
        //                new { CurPeriodId = curPeriod.Id }, ts).ToList();
        //            List<SubjectMoney> banks = new List<SubjectMoney>();
        //            foreach (var bank in lsBank)
        //            {
        //                banks.Add(new SubjectMoney
        //                {
        //                    Item = "本月银行存款余额",
        //                    Credit_Debit = "借",
        //                    Money = bank.BWBEndBAL,
        //                    Subject = bank.SubjectCode + " " + bank.Name
        //                });
        //            }

        //            //本月应收账款余额
        //            #region 本月应收账款余额
        //            sql = @"select k1.subjectCode,k1.CalValue1,k3.Name   , 
        //                    (k1.bwbEndBal_J) as bwbEndBal  from (
        //                    select SubjectCode ,CalItem1,CalValue1, 

        //                    sum(case when ECredit_Debit=0 then bwbEndBal else 0 end) bwbEndBal_J,
        //                    sum(case when ECredit_Debit=1 then BWBEndBAL else 0 end) bwbEndBal_D

        //                    from TKS_FAS_FGLBalance where periodId=@CurPeriodId and SubjectCode='1131'

        //                    group by SubjectCode ,CalItem1,CalValue1) k1

        //                    left join TKS_FAS_CaculateHelperDetail k3
        //                    on ( k1.CalValue1=k3.Code) and k1.CalItem1=k3.parentId order by k1.CalValue1";
        //            var ls1131 = cnn.Query<TKS_FAS_FGLBalance>(sql,
        //                new { CurPeriodId = curPeriod.Id }, ts).ToList();
        //            List<SubjectMoney> ar = new List<SubjectMoney>();
        //            foreach (var item in ls1131)
        //            {
        //                ar.Add(new SubjectMoney
        //                {
        //                    Credit_Debit = "借",
        //                    Item = "本月应收账款余额",
        //                    Subject = "应收账款 " + "-" + item.Name,
        //                    Money = item.BWBEndBAL
        //                });
        //            }

        //            #endregion


        //            #region 本月客户收款金额
        //            //借方，银行存款
        //            //贷方，应收账款客户
        //            var lsKHMoney = CurMonthKHSK(curPeriod, ts);

        //            #endregion

        //            #region 本月应付账款余额

        //            sql = @"select k1.subjectCode,k1.CalValue1,k3.Name   , 
        //                    (k1.bwbEndBal_D) as bwbEndBal  from (
        //                    select SubjectCode ,CalItem1,CalValue1, 

        //                    sum(case when ECredit_Debit=0 then bwbEndBal else 0 end) bwbEndBal_J,
        //                    sum(case when ECredit_Debit=1 then BWBEndBAL else 0 end) bwbEndBal_D

        //                    from TKS_FAS_FGLBalance where periodId=@CurPeriodId and SubjectCode='2121'

        //                    group by SubjectCode ,CalItem1,CalValue1) k1

        //                    left join TKS_FAS_CaculateHelperDetail k3
        //                    on ( k1.CalValue1=k3.Code) and k1.CalItem1=k3.parentId order by k1.CalValue1";
        //            var ls2121 = cnn.Query<TKS_FAS_FGLBalance>(sql,
        //                new { CurPeriodId = curPeriod.Id }, ts).ToList();
        //            List<SubjectMoney> ap = new List<SubjectMoney>();
        //            foreach (var item in ls2121)
        //            {
        //                ap.Add(new SubjectMoney
        //                {
        //                    Credit_Debit = "贷",
        //                    Item = "本月应付账款余额",
        //                    Subject = "应付账款-" + item.Name,
        //                    Money = item.BWBEndBAL
        //                });
        //            }

        //            #endregion


        //            #region 本月支付供应商款项金额

        //            var lsPaySupplier = PaySupplier(curPeriod, ts);
        //            #endregion

        //            #region 本月供应商采购金额
        //            var lsCG = CG(curPeriod, ts);
        //            #endregion


        //            sql = @"select * from TKS_FAS_AccountInfo where id=@Id";

        //            var account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(sql, new { Id = request.AccountId }, ts);


        //            ts.Commit();
        //            res.Account = account.QY_Name;

        //            res.KPMoney = new SubjectMoney { Subject = "本月开票收入", Credit_Debit = "贷", Money = kpMoney };
        //            res.KPMoneyYear = new SubjectMoney { Subject = "本年累计开票收入", Credit_Debit = "贷", Money = kpMoneyYear };
        //            res.BankDepositBal = banks;

        //            res.ARAccountBal = ar;

        //            res.KHMoney = lsKHMoney;
        //            res.APAccountBal = ap;

        //            res.PaySupplierMoney = lsPaySupplier;
        //            res.CGSupplierMoney = lsCG;

        //            res.IsSuccess = true;
        //            res.Message = "加载完成";

        //            return res;
        //        }
        //        catch (Exception ex)
        //        {
        //            ts.Rollback();
        //            return this.DealException(res, ex) as ResponseJYGet;
        //        }
        //    }
        //}
    }
}
