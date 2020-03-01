using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Common;
using System.Data;

namespace TKS.FAS.BLL.FAS
{
    /// <summary>
    /// 账簿的查询后续需要重构，最小粒度去查，以便更多筛选
    /// </summary>
    public class AccountBookBLL : CommonBase
    {
        /// <summary>
        /// 01 获取总账
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseGenAccountListSearch GenAccountListSearch(RequestGenAccountListSearch request)
        {
            ResponseGenAccountListSearch res = new ResponseGenAccountListSearch();
            using (cnn = GetConnection())
            {
                try
                {
                    var ts = cnn.BeginTransaction();

                    var user = this.UserInfoGet(request.Token, ts);

                    #region table
                    string table = @"     
(
select   convert(varchar, B.Year)+'年'+convert(varchar,B.Month)+'期' as period, B.StartDate as DetailDate,'' as PZZ,A.subjectCode ,C.showTitle as name ,  '期初余额' as summary,

0 as num1,0 as Unit_Price1 ,0 as Money1,
0 as num2,0 as Unit_Price2 ,0 as Money2,

 A.SCredit_Debit as show_credit_debit,A.NUMStartBAL as Show_Quantity,A.BWBStartBAL as show_money
 
from 
TKS_FAS_GLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
 where {0}

 

union
select  convert(varchar, B.Year)+'年'+convert(varchar,B.Month)+'期' as period, B.EndDate as DetailDate,''as PZZ,A.subjectCode ,C.showTitle as  name ,'本期合计' as summary ,

A.NUMDebitTotal as num1,0 as Unit_Price1 ,A.BWBDebitTotal as Money1,
A.NUMCreditTotal as num2,0 as Unit_Price2 ,A.BWBCreditTotal as Money2,

A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money

 
from 
TKS_FAS_GLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
 where {0}

 union

 select convert(varchar, B.Year)+'年'+convert(varchar,B.Month)+'期' as period, B.EndDate as DetailDate,''as PZZ,A.subjectCode ,C.showTitle as name ,'本年累计' as summary ,
 A.NUMDebitTotal_Y as num1,0 as Unit_Price1 ,A.BWBDebitTotal_Y as Money1,
A.NUMCreditTotal_Y as num2,0 as Unit_Price2 ,A.BWBCreditTotal_Y as Money2,

A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money
 
from 
TKS_FAS_GLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
 where {0} 
) A";

                    #endregion

                    if (request.Type == "normal")
                    {
                        List<CodeValue> subjectCode = GetDocSubjectCode(request.PeriodId, ts, user, "");

                        string where = string.Empty;

                        for (var i = 0; i < subjectCode.Count; i++)
                        {
                            if (i != 0)
                            {
                                where += "or A.subjectCode='" + subjectCode[i].Code + "' ";
                            }
                            else
                            {
                                where += " A.subjectCode='" + subjectCode[i].Code + "' ";
                            }
                        }
                        where = "(" + where + ")";

                        table = string.Format(table, " A.PeriodId='" + request.PeriodId + "'");
                        table += " where " + where;
                    }
                    else
                    {
                        #region
                        var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                    @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                    new { Id = request.More.Period_S }, ts);
                        //结束期间
                        var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                         new { Id = request.More.Period_E }, ts);
                        if (periodS == null)
                        {
                            return new ResponseGenAccountListSearch { IsSuccess = false, Message = "会计期间没有选择" };
                        }
                        if (periodS.EndDate > periodE.EndDate)
                        {
                            throw new NormalException("开始期间不能大于结束期间");
                        }
                        //获取期间集合
                        List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                            @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE ",
                            new
                            {
                                StartDateS = periodS.StartDate.ToString(),
                                StartDateE = periodE.StartDate.ToString(),
                                AccountId = request.AccountList == null ? user.AccountId : request.AccountList
                            }, ts).ToList();

                        string periodWhere = string.Empty;//期间条件

                        List<CodeValue> subjectCode = new List<CodeValue>();
                        int j = 0;
                        foreach (var period in periods)
                        {
                            if (j > 0)
                            {
                                periodWhere += " or ";
                            }
                            var _subjectCode = GetDocSubjectCode(period.Id, ts, user, "");
                            subjectCode.AddRange(_subjectCode);
                            periodWhere += "  A.periodId='" + period.Id + "'";
                            j++;
                        }


                        List<CodeValue> qcCode = GetQCCode(user, ts);
                        subjectCode.AddRange(qcCode);
                        List<CodeValue> fzCode = GetFZCode(user, ts);
                        subjectCode.AddRange(fzCode);


                        string where = string.Empty;
                        for (var i = 0; i < subjectCode.Count; i++)
                        {
                            if (i != 0)
                            {
                                where += "or A.SubjectCode='" + subjectCode[i].Code + "' ";
                            }
                            else
                            {
                                where += " A.SubjectCode='" + subjectCode[i].Code + "' ";
                            }
                        }
                        //科目条件没有获取到则过滤所有
                        where = "   (" + (string.IsNullOrEmpty(where) ? "1=2" : where) + ")";

                        periodWhere = "  (" + periodWhere + ")";

                        if (!string.IsNullOrEmpty(request.More.Code_S))
                        {
                            where += " and A.SubjectCode>='" + request.More.Code_S + "'";
                        }

                        if (!string.IsNullOrEmpty(request.More.Code_E))
                        {
                            where += " and A.SubjectCode<='" + request.More.Code_E + "'";
                        }

                        table = string.Format(table, periodWhere);
                        table += " where " + where;
                        #endregion
                    }




                    string sql = this.GetPageSql("A.*", table,
                        " A.subjectCode",
                        request.PageSize,
                        request.PageIndex);

                    List<ZB_GenAccount> data = cnn.Query<ZB_GenAccount>(sql, null, ts).ToList();

                    string countSql = @"select count(1) from  " + table;

                    int total = int.Parse(cnn.ExecuteScalar(countSql, null, ts).ToString());


                    ts.Commit();


                    List<ZB_GenAccountHead> lsHead = new List<ZB_GenAccountHead>();
                    GenComparer comparer = new BLL.GenComparer();
                    var lsData = data.Distinct(comparer);

                    lsHead = (from item in lsData
                              select new ZB_GenAccountHead
                              {
                                  Code = item.SubjectCode,
                                  Name = item.Name,
                                  Data = data.Where(p => p.SubjectCode == item.SubjectCode).ToList()
                              }).Distinct().OrderBy(p => p.Code).ToList();



                    res.IsSuccess = true;
                    res.Message = "加载完毕";
                    res.PageIndex = request.PageIndex;
                    res.Data = lsHead;
                    res.Total = total;
                    return res;
                }
                catch (Exception ex)
                {
                    return this.DealException(res, ex) as ResponseGenAccountListSearch;
                }
            }
        }


        /// <summary>
        /// 02 明细账
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseDetailListSearch DetailListSearch(RequestDetailListSearch request)
        {
            ResponseDetailListSearch res = new ResponseDetailListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string StartPeriod = "";//期初期间
                    #region table
                    string table = @"( 
 select 0 as id, A.PeriodId,B.Year,B.Month,A.subjectCode, B.StartDate as DetailDate,'' as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,  '期初余额' as summary,

0 as num1,0 as Unit_Price1 ,0 as Money1,
0 as num2,0 as Unit_Price2 ,0 as Money2,

 A.SCredit_Debit as show_credit_debit,A.NUMStartBAL as Show_Quantity,A.BWBStartBAL as show_money
 ,'' as CreateDate
from 
TKS_FAS_GLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
 where {0} and A.BWBStartBAL!=0

 union all

 select row_number() OVER ( ORDER BY  B.CreateDate) id, A.PeriodId,D.Year,D.Month,A.subjectCode, B.PZDate as DetailDate ,C.PZZ+cast(B.PZZNO as varchar) as pzz,B.PZZNO,A.Seq,A.SubjectDescription as name,
A.Summary,
 
A.Quantity as num1,0 as Unit_Price1 ,A.Money_Debit as Money1,
A.Quantity as num2,0 as Unit_Price2 ,A.Money_Credit as Money2,
  case when A.EndBAL is null or A.EndBAL=0 then -1
  else sub.Credit_Debit end  as show_credit_debit,0 as show_quantity,isnull(A.EndBAL,0) as show_money
,B.CreateDate
 from 
 TKS_FAS_Doc B left join  TKS_FAS_DocDetail A on B.Id=A.ParentId
 left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
LEFT JOIN TKS_FAS_MonthPeriodInfo D ON A.PeriodId = D.id
 left join TKS_FAS_AccountSubject sub on sub.Code=A.SubjectCode and sub.AccountId=A.AccountId
 where 
    {0}  and B.source<>'JZ' and (A.Money_Debit!=0 OR A.Money_Credit!=0 )

union
select 0 as id, A.PeriodId,B.Year,B.Month,A.subjectCode, B.EndDate as DetailDate,''as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,'本期合计' as summary ,

A.NUMDebitTotal as num1,0 as Unit_Price1 ,A.BWBDebitTotal as Money1,
A.NUMCreditTotal as num2,0 as Unit_Price2 ,A.BWBCreditTotal as Money2,

A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money

 ,'' as CreateDate
from 
TKS_FAS_GLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
left join tks_fas_accountSubject C on A.subjectCode=C.code  and A.accountId=C.accountId
 where {0} and A.BWBEndBAL!=0

 union

 select 0 as id, A.PeriodId,B.Year,B.Month,A.subjectCode, B.EndDate as DetailDate,''as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,'本年累计' as summary ,
 A.NUMDebitTotal_Y as num1,0 as Unit_Price1 ,A.BWBDebitTotal_Y as Money1,
A.NUMCreditTotal_Y as num2,0 as Unit_Price2 ,A.BWBCreditTotal_Y as Money2,

A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money
 ,'' as CreateDate
from 
TKS_FAS_GLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
 where {0}and A.BWBEndBAL!=0
) A where  1=1 ";
                    if (!string.IsNullOrEmpty(request.allPZ))
                    {
                        table = @"(
select  A.PeriodId,B.Year,B.Month,A.subjectCode, B.StartDate as DetailDate,'' as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,  '期初余额' as summary,

0 as num1,0 as Unit_Price1 ,0 as Money1,
0 as num2,0 as Unit_Price2 ,0 as Money2,

 A.SCredit_Debit as show_credit_debit,A.NUMStartBAL as Show_Quantity,A.BWBStartBAL as show_money
 
from 
TKS_FAS_GLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
 where {0}

 union all

 select  A.PeriodId,D.Year,D.Month,A.subjectCode, B.PZDate as DetailDate ,C.PZZ+cast(B.PZZNO as varchar) as pzz,B.PZZNO,A.Seq,A.SubjectDescription as name,
A.Summary,
 
A.Quantity as num1,0 as Unit_Price1 ,A.Money_Debit as Money1,
A.Quantity as num2,0 as Unit_Price2 ,A.Money_Credit as Money2,
 0 as show_credit_debit,0as show_quantity,0 as show_money

 from 
 TKS_FAS_Doc B left join  TKS_FAS_DocDetail A on B.Id=A.ParentId
 left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
LEFT JOIN TKS_FAS_MonthPeriodInfo D ON A.PeriodId = D.id
 where 
    {0} 

union
select  A.PeriodId,B.Year,B.Month,A.subjectCode, B.EndDate as DetailDate,''as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,'本期合计' as summary ,

A.NUMDebitTotal as num1,0 as Unit_Price1 ,A.BWBDebitTotal as Money1,
A.NUMCreditTotal as num2,0 as Unit_Price2 ,A.BWBCreditTotal as Money2,

A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money

 
from 
TKS_FAS_GLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
left join tks_fas_accountSubject C on A.subjectCode=C.code  and A.accountId=C.accountId
 where {0}

 union

 select  A.PeriodId,B.Year,B.Month,A.subjectCode, B.EndDate as DetailDate,''as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,'本年累计' as summary ,
 A.NUMDebitTotal_Y as num1,0 as Unit_Price1 ,A.BWBDebitTotal_Y as Money1,
A.NUMCreditTotal_Y as num2,0 as Unit_Price2 ,A.BWBCreditTotal_Y as Money2,

A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money
 
from 
TKS_FAS_GLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
 where {0}
) A";
                    }
                    #endregion

                    List<ZB_Detail> data = new List<ZB_Detail>();
                    List<ZB_DetailHead> lsHead = new List<ZB_DetailHead>();
                    int total = 0;
                    if (request.Type == "normal")
                    {
                        #region 一般查询
                        table = string.Format(table, "A.PeriodId=@PeriodId and A.SubjectCode=@SubjectCode");
                        string sql = this.GetPageSql("A.*",
                                            table,
                                            "A.subjectCode  ",
                                            request.PageSize,
                                            request.PageIndex);

                        object predicate = new { PeriodId = request.PeriodId, SubjectCode = request.SubjectCode };

                        data = cnn.Query<ZB_Detail>(sql, predicate
                     , ts).ToList();
                        string countSql = @"select count(1) from " + table;

                        total = int.Parse(cnn.ExecuteScalar(countSql, predicate, ts).ToString());
                        #endregion
                    }
                    else
                    {
                        #region 更多查询
                        //开始期间

                        var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                       @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                       new { Id = request.More.Period_S }, ts);
                        //结束期间
                        var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                         new { Id = request.More.Period_E }, ts);
                        if (periodS == null)
                        {
                            return new ResponseDetailListSearch { IsSuccess = false, Message = "会计期间没有选择" };
                        }
                        if (periodS.EndDate > periodE.EndDate)
                        {
                            throw new NormalException("开始期间不能大于结束期间");
                        }
                        //获取期间集合
                        List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                            @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE order by Year,Month",
                            new
                            {
                                StartDateS = periodS.StartDate.ToString(),
                                StartDateE = periodE.StartDate.ToString(),
                                AccountId = user.AccountId
                            }, ts).ToList();
                        string orderby = "A.subjectCode,A.DetailDate,A.PZZNO,A.Seq";
                        if (!string.IsNullOrEmpty(request.AccountList))
                        {
                            orderby = "A.PeriodId,A.subjectCode,A.PZZNO,A.Seq";
                            //打印工具专用
                            periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                            string.Format(@"select * from TKS_FAS_MonthPeriodInfo where AccountId in{0} 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE ", request.AccountList),
                            new
                            {
                                StartDateS = periodS.StartDate.ToString(),
                                StartDateE = periodE.StartDate.ToString()
                            }, ts).ToList();
                        }
                        //获取科目存在的期间,不用

                        //    var existPeriods = cnn.Query<TKS_FAS_DocDetail>(
                        //        @"select * from TKS_FAS_DocDetail 
                        //where subjectCode=@SubjectCode and accountId=@AccountId",
                        //            new { SubjectCode = request.More.Code, AccountId = user.AccountId }, ts)
                        //            .Select(p => p.PeriodId).ToList();

                        string periodWhere = string.Empty;//期间条件


                        int j = 0;
                        foreach (var period in periods)
                        {

                            //if (!existPeriods.Contains(period.Id))
                            //{
                            //    continue;
                            //}
                            if (j > 0)
                            {
                                periodWhere += " or ";
                            }

                            periodWhere += "  A.periodId='" + period.Id + "'";
                            j++;
                        }

                        string where = "(" + periodWhere + ")";

                        if (!string.IsNullOrEmpty(request.More.Code_S) && !string.IsNullOrEmpty(request.More.Code_E))
                        {
                            //where += " and A.SubjectCode>='" + request.More.Code_S + "' and A.subjectCode<='" + request.More.Code_E + "'";

                            var subjects = cnn.Query<TKS_FAS_AccountSubject>(
                                @"select * from TKS_FAS_AccountSubject where AccountId =@AccountId
                                and Code >=@SCode and  Code <=@ECode 
                                union 
                                select * from TKS_FAS_AccountSubject where ParentId in (
                                select id from TKS_FAS_AccountSubject where AccountId =@AccountId
                                and Code >=@SCode and  Code <=@ECode )
                                union
                                select * from TKS_FAS_AccountSubject where ParentId in (
                                select id from TKS_FAS_AccountSubject where ParentId in (
                                select id from TKS_FAS_AccountSubject where AccountId =@AccountId
                                and Code >=@SCode and  Code <=@ECode))
                                union
                                 select * from TKS_FAS_AccountSubject where ParentId in (
                                select id from TKS_FAS_AccountSubject where ParentId in (
                                select id from TKS_FAS_AccountSubject where ParentId in (
                                select id from TKS_FAS_AccountSubject where AccountId =@AccountId
                                and Code >=@SCode and  Code <=@ECode)))
                                ", new { AccountId = user.AccountId, SCode = request.More.Code_S, ECode = request.More.Code_E }, ts);
                            if (subjects.Count() > 0)
                            {
                                where += " and A.SubjectCode in(" + string.Join(",", subjects.Select(p => "'" + p.Code + "'").ToList()) + ")";
                            }
                            else
                            {
                                where += " and A.SubjectCode = ''";
                            }
                        }
                        StartPeriod = periods[0].Id;//期初期间
                        string start_sql = string.Format(@"
        select  A.PeriodId,B.Year,B.Month,A.subjectCode, B.StartDate as DetailDate,''as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,'期初余额' as summary ,
         A.NUMDebitTotal_Y as num1,0 as Unit_Price1 ,0 as Money1,
        A.NUMCreditTotal_Y as num2,0 as Unit_Price2 ,0 as Money2,

        A.SCredit_Debit as show_credit_debit,A.NUMStartBAL as show_quantity, A.BWBStartBAL as show_money
 
        from 
        TKS_FAS_GLBalance A 
        left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
        left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
         where  A.accountId = '{0}' and A.periodId='{1}' union  
", user.AccountId, StartPeriod);

                        table = string.Format(table, where);

                        //string sql = this.GetPageSql("A.*",
                        //                  table,
                        //                  orderby,
                        //                  request.PageSize,
                        //                  request.PageIndex);

                        //object predicate = new { SubjectCode = request.More.Code };
                        List<ZB_Detail> QC = new List<ZB_Detail>();//期初
                        List<ZB_Detail> PZDetail = new List<ZB_Detail>();//凭证明细
                        List<ZB_Detail> SumPeriod = new List<ZB_Detail>();//本期合计
                        List<ZB_Detail> SumYear = new List<ZB_Detail>();//本年累计
                        string sql_QC = string.Format(@" 
    select 0 as id, A.PeriodId,B.Year,B.Month,A.subjectCode, B.StartDate as DetailDate,'' as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,  '期初余额' as summary,

    0 as num1,0 as Unit_Price1 ,0 as Money1,
    0 as num2,0 as Unit_Price2 ,0 as Money2,

     A.SCredit_Debit as show_credit_debit,A.NUMStartBAL as Show_Quantity,A.BWBStartBAL as show_money
     ,0 as StartBAL
    from 
    TKS_FAS_GLBalance A 
    left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
    left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
     where {0} and A.BWBStartBAL!=0", where);
                        QC = cnn.Query<ZB_Detail>(sql_QC, null
                    , ts).ToList();
                        string sql_PZDetail = string.Format(@"
     select row_number() OVER (ORDER BY D.Year,D.Month, A.subjectCode, B.CreateDate,A.PeriodId,A.Seq) id, A.PeriodId,D.Year,D.Month,A.subjectCode, B.PZDate as DetailDate ,C.PZZ+cast(B.PZZNO as varchar) as pzz,B.PZZNO,A.Seq,A.SubjectDescription as name,
    A.Summary,
 
    A.Quantity as num1,0 as Unit_Price1 ,A.Money_Debit as Money1,
    A.Quantity as num2,0 as Unit_Price2 ,A.Money_Credit as Money2,
      case when A.EndBAL is null or A.EndBAL=0 then -1
      else sub.Credit_Debit end  as show_credit_debit,0 as show_quantity,isnull(A.EndBAL,0) as show_money
    ,B.CreateDate,isnull(A.StartBAL,0) as StartBAL
     from 
     TKS_FAS_Doc B left join  TKS_FAS_DocDetail A on B.Id=A.ParentId
     left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
    LEFT JOIN TKS_FAS_MonthPeriodInfo D ON A.PeriodId = D.id
     left join TKS_FAS_AccountSubject sub on sub.Code=A.SubjectCode and sub.AccountId=A.AccountId
     where 
        {0}  and B.source<>'JZ' and (A.Money_Debit!=0 OR A.Money_Credit!=0 )

", where);
                        PZDetail = cnn.Query<ZB_Detail>(sql_PZDetail, null
                    , ts).ToList();
                        string sql_SumPeriod = string.Format(@"
    select 0 as id, A.PeriodId,B.Year,B.Month,A.subjectCode, B.EndDate as DetailDate,''as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,'本期合计' as summary ,

    A.NUMDebitTotal as num1,0 as Unit_Price1 ,A.BWBDebitTotal as Money1,
    A.NUMCreditTotal as num2,0 as Unit_Price2 ,A.BWBCreditTotal as Money2,
    case when A.BWBEndBAL is null or A.BWBEndBAL=0 then -1
      else A.ECredit_Debit end  as show_credit_debit
    ,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money
,0 as StartBAL
    
    from 
    TKS_FAS_GLBalance A 
    left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
    left join tks_fas_accountSubject C on A.subjectCode=C.code  and A.accountId=C.accountId
     where {0} 

", where);
                        SumPeriod = cnn.Query<ZB_Detail>(sql_SumPeriod, null
                  , ts).ToList();
                        string sql_SumYear = string.Format(@"
     select 0 as id, A.PeriodId,B.Year,B.Month,A.subjectCode, B.EndDate as DetailDate,''as PZZ,'' as PZZNO,'' as Seq,A.subjectCode +C.showTitle as name,'本年累计' as summary ,
     A.NUMDebitTotal_Y as num1,0 as Unit_Price1 ,A.BWBDebitTotal_Y as Money1,
    A.NUMCreditTotal_Y as num2,0 as Unit_Price2 ,A.BWBCreditTotal_Y as Money2,

       case when A.BWBEndBAL is null or A.BWBEndBAL=0 then -1
      else A.ECredit_Debit end  as show_credit_debit
    ,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money
    ,0 as StartBAL
    from 
    TKS_FAS_GLBalance A 
    left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
    left join tks_fas_accountSubject C on A.subjectCode=C.code and A.accountId=C.accountId
     where {0}

", where);
                        SumYear = cnn.Query<ZB_Detail>(sql_SumYear, null
                 , ts).ToList();
                        int flag = 0;
                        foreach (var pz in PZDetail)
                        {
                            ZB_Detail z_QC = QC.FirstOrDefault(s => s.PeriodId == pz.PeriodId && s.SubjectCode == pz.SubjectCode);
                            if (z_QC != null)
                            {
                                data.Add(z_QC);
                                QC.Remove(z_QC);
                            }
                            data.Add(pz);
                            int count = PZDetail.Where(p => p.PeriodId == pz.PeriodId && p.SubjectCode == pz.SubjectCode).Count();
                            if (count >= 1)
                            {
                                flag++;
                            }
                            if (count == flag)
                            {
                                ZB_Detail z_SumPeriod = SumPeriod.FirstOrDefault(s => s.PeriodId == pz.PeriodId && s.SubjectCode == pz.SubjectCode);
                                if (z_SumPeriod != null)
                                {
                                    data.Add(z_SumPeriod);

                                }
                                ZB_Detail z_SumYear = SumYear.FirstOrDefault(s => s.PeriodId == pz.PeriodId && s.SubjectCode == pz.SubjectCode);
                                if (z_SumYear != null)
                                {
                                    data.Add(z_SumYear);

                                }
                                flag = 0;
                            }
                        }
                        //   data = cnn.Query<ZB_Detail>(sql, null
                        //, ts).ToList();

                        #region 打印数据处理

                        ZB_DetailComparer comparer = new BLL.ZB_DetailComparer();
                        var lsData = data.Distinct(comparer);

                        lsHead = (from item in lsData
                                  select new ZB_DetailHead
                                  {
                                      Year = item.Year.ToString(),
                                      Month = item.Month,
                                      Data = data.Where(p => p.PeriodId == item.PeriodId).ToList()
                                  }).Distinct().OrderBy(p => p.PeriodId).ToList();
                        #endregion

                        string countSql = @"select count(1) from " + table;

                        //total = int.Parse(cnn.ExecuteScalar(countSql, null, ts).ToString());
                        #endregion
                    }

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "加载完毕";
                    res.PageIndex = request.PageIndex;
                    res.Data = data.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
                    res.PrintData = lsHead;//打印数据
                    //res.Total = total;
                    res.Total = data.Count;
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseDetailListSearch;
                }
            }
        }

        /// <summary>
        /// 03 科目余额表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseKMBalListSearch BALAccountSearch(RequestKMBalListSearch request)
        {
            ResponseKMBalListSearch response = new ResponseKMBalListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string where = string.Empty;
                    string where1 = string.Empty;
                    string StartPeriod = "";//期初期间
                    string EndPeriod = "";//期末期间
                    if (request.Type == "normal")
                    {
                        #region 普通查询
                        List<CodeValue> subjectCode = GetDocSubjectCode(request.PeriodId, ts, user, request.allPZ);
                        List<CodeValue> qcCode = GetQCCode(user, ts);
                        subjectCode.AddRange(qcCode);
                        List<CodeValue> fzCode = GetFZCode(user, ts);
                        subjectCode.AddRange(fzCode);



                        for (var i = 0; i < subjectCode.Count; i++)
                        {
                            if (i != 0)
                            {
                                where += "or A.SubjectCode='" + subjectCode[i].Code + "' ";
                            }
                            else
                            {
                                where += " A.SubjectCode='" + subjectCode[i].Code + "' ";
                            }
                        }
                        //科目条件没有获取到则过滤所有
                        where = " and (" + (string.IsNullOrEmpty(where) ? "1=2" : where) + ")";

                        where += " and A.periodId='" + request.PeriodId + "'";
                        #endregion
                    }
                    else
                    {
                        //开始期间
                        var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                       @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                       new { Id = request.More.Period_S }, ts);
                        //结束期间
                        var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                         new { Id = request.More.Period_E }, ts);
                        if (periodS == null)
                        {
                            return new ResponseKMBalListSearch { IsSuccess = false, Message = "会计期间没有选择" };
                        }
                        if (periodS.EndDate > periodE.EndDate)
                        {
                            throw new NormalException("开始期间不能大于结束期间");
                        }
                        //获取期间集合
                        List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                            @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE 
                                order by Year,Month",
                            new
                            {
                                StartDateS = periodS.StartDate.ToString(),
                                StartDateE = periodE.StartDate.ToString(),
                                AccountId = request.AccountList == null ? user.AccountId : request.AccountList
                            }, ts).ToList();

                        string periodWhere = string.Empty;//期间条件
                        StartPeriod = periods[0].Id;//期初期间
                        EndPeriod = periods[periods.Count - 1].Id;//期末期间
                        List<CodeValue> subjectCode = new List<CodeValue>();
                        int j = 0;
                        foreach (var period in periods)
                        {
                            if (j > 0)
                            {
                                periodWhere += " or ";
                            }
                            var _subjectCode = GetDocSubjectCode(period.Id, ts, user, request.allPZ);
                            subjectCode.AddRange(_subjectCode);
                            periodWhere += "  A.periodId='" + period.Id + "'";
                            j++;
                        }


                        List<CodeValue> qcCode = GetQCCode(user, ts);
                        subjectCode.AddRange(qcCode);
                        List<CodeValue> fzCode = GetFZCode(user, ts);
                        subjectCode.AddRange(fzCode);


                        //update by Hero.Zhang 取消筛选选择期间内发生的科目
                        //for (var i = 0; i < subjectCode.Count; i++)
                        //{
                        //    if (i != 0)
                        //    {
                        //        where += "or A.SubjectCode='" + subjectCode[i].Code + "' ";
                        //    }
                        //    else
                        //    {
                        //        where += " A.SubjectCode='" + subjectCode[i].Code + "' ";
                        //    }
                        //}
                        //科目条件没有获取到则过滤所有
                        //where = " and (" + (string.IsNullOrEmpty(where) ? "1=2" : where) + ")";



                        if (!string.IsNullOrEmpty(request.More.Code_S))
                        {
                            where += " and A.SubjectCode>='" + request.More.Code_S + "'";
                        }

                        if (!string.IsNullOrEmpty(request.More.Code_E))
                        {
                            where += " and A.SubjectCode<='" + request.More.Code_E + "'";
                        }

                        where += " and (" + periodWhere + ")";
                        where1 = where;
                    }


                    string A = @" with A as (select 'N' as IsCal,";
                    //if (!string.IsNullOrEmpty(request.AccountList))
                    //{
                    //A += "PeriodId,TKS_FAS_MonthPeriodInfo.Year,TKS_FAS_MonthPeriodInfo.Month,";
                    //}
                    A += @"SubjectCode as code
  , isnull(MAX(CASE 
				WHEN RootName = 1 THEN A.Name
			END),TKS_FAS_AccountSubject.Name) AS name,
 sum( (case when SCredit_Debit = 0 then NUMStartBAL else 0 end)) as NumStart_J,   --期初借方数量
  sum( (case when SCredit_Debit = 0 then  A.BWBStartBAL else 0 end)) as BWBStart_J, --期初本位币

   sum(   (case when SCredit_Debit = 1 then NUMStartBAL else 0 end)) as NumStart_D,   --期初借方数量
    sum( (case when SCredit_Debit = 1 then  A.BWBStartBAL else 0 end)) as BWBStart_D, --期初本位币
       

  sum( A.NUMDebitTotal)as Num_CJ ,--本期借方发生额数量


  sum( A.BWBDebitTotal) as BWB_CJ,  --本期借方发生额本位币

  sum( A.NUMCreditTotal) as NUM_CD,
   
   sum(A.BWBCreditTotal) as BWB_CD,--本期贷方发生额本位币

   sum(A.NUMDebitTotal_Y) as Num_YJ,--本年累计发生额数量


   sum(A.BWBDebitTotal_Y) as BWB_YJ,--本年累计发生额本位币

   sum(A.NUMCreditTotal_Y) Num_YD,--本年贷方累计发生额数量


   sum(A.BWBCreditTotal_Y) as BWB_YD,--本年贷方累计发生额本位币

   sum((case when ECredit_Debit = 0 then A.NUMEndBAL else 0 end)) as NumEnd_J,   --期末借方数量
   sum((case when ECredit_Debit = 0 then  A.BWBEndBAL else 0 end)) as BWBEnd_J, --期末本位币

     sum( (case when ECredit_Debit = 1 then A.NUMEndBAL else 0 end)) as NumEnd_D,   --期末借方数量
    sum( (case when ECredit_Debit = 1 then  A.BWBEndBAL else 0 end)) as BWBEnd_D --期末本位币

from (
  select a.AccountId,A.subjectId,a.Category,a.PeriodId,a.SubjectCode,a.Name,a.CurrencyCode,a.SCredit_Debit,
a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
a.isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,a.isLeaf, 1 as Type
,a.NUMEndBAL,a.YBEndBAL,a.BWBEndBAL,a.ECredit_Debit ,1 as RootName
  from TKS_FAS_GLBalance A 
  union all

  select a.AccountId,'' as subjectId,a.Category,a.PeriodId,a.SubjectCode,a.Name,a.CurrencyCode,a.SCredit_Debit,
a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
0 as  isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,1 as isLeaf, 2 as Type
,a.NUMEndBAL,a.YBEndBAL,a.BWBEndBAL,a.ECredit_Debit , 0 as RootName
  from TKS_FAS_FGLBalance A ) A ";
                    //if (!string.IsNullOrEmpty(request.AccountList))
                    //{
                    A += " left join TKS_FAS_MonthPeriodInfo on TKS_FAS_MonthPeriodInfo.Id=A.PeriodId";
                    A += @" left join TKS_FAS_AccountSubject on TKS_FAS_AccountSubject.Code=A.SubjectCode 

            and TKS_FAS_AccountSubject.AccountId = A.AccountId";
                    //}
                    A += "  where A.accountId=@AccountId   " + where + @" group by SubjectCode,TKS_FAS_AccountSubject.Name ";

                    A += " ) ";

                    string table = @" (select ";
                    //if (!string.IsNullOrEmpty(request.AccountList))
                    //{
                    //table += "PeriodId,Year,Month,";
                    //}
                    table += @"IsCal,code,
   name,
  NumStart_J,   --期初借方数量
   ( case when  (abs(BWBStart_J)-abs(BWBStart_D))>0 then BWBStart_J-BWBStart_D else 0 end)    BWBStart_J, --期初本位币
   NumStart_D,   --期初借方数量
    ( case when  (abs(BWBStart_J)-abs(BWBStart_D))<0 then BWBStart_D-BWBStart_J else 0 end)  BWBStart_D, --期初本位币
  Num_CJ ,--本期借方发生额数量
   BWB_CJ,  --本期借方发生额本位币
   NUM_CD,
  BWB_CD,--本期贷方发生额本位币
     Num_YJ,--本年累计发生额数量
    BWB_YJ,--本年累计发生额本位币
     Num_YD,--本年贷方累计发生额数量
    BWB_YD,--本年贷方累计发生额本位币
   (case when (NumEnd_J-abs(NumEnd_D)) >0 then  NumEnd_J-NumEnd_D else 0 end)  NumEnd_J,   --期末借方数量
 ( case when  (abs(BWBEnd_J)-abs(BWBEnd_D))>0 then BWBEnd_J-BWBEnd_D else 0 end) BWBEnd_J, --期末本位币
   (case when (NumEnd_J-abs(NumEnd_D)) <0 then  NumEnd_D-NumEnd_J else 0 end)  NumEnd_D,   --期末借方数量
    ( case when  (abs(BWBEnd_J)-abs(BWBEnd_D))<0 then BWBEnd_D-BWBEnd_J else 0 end) BWBEnd_D --期末本位币

	from A where not (a.BWBStart_J =0 and a.BWBStart_D=0 and a.BWB_CJ=0 and a.BWB_CD=0 and a.BWBEnd_J= 0and a.BWBEnd_D=0)
                          
                          
                          ) M";
                    string orderby = @" M.Code ";
                    //if (!string.IsNullOrEmpty(request.AccountList))
                    //{
                    //orderby = "M.PeriodId, M.Code";
                    //}
                    //string sql = this.GetPageSql(" M.* ",
                    //        table, orderby,
                    //        request.PageSize, request.PageIndex);
                    string sql = this.GetPageSql(" M.* ",
                            table, orderby);
                    sql = A + sql;
                    string start_sql = string.Format(@"
		SELECT 'N' AS IsCal, SubjectCode AS code, isnull(MAX(CASE 
				WHEN RootName = 1 THEN A.Name
			END),TKS_FAS_AccountSubject.Name) AS name
			, SUM(CASE 
				WHEN SCredit_Debit = 0 THEN NUMStartBAL
				ELSE 0
			END) AS NumStart_J, SUM(CASE 
				WHEN SCredit_Debit = 0 THEN A.BWBStartBAL
				ELSE 0
			END) AS BWBStart_J
			, SUM(CASE 
				WHEN SCredit_Debit = 1 THEN NUMStartBAL
				ELSE 0
			END) AS NumStart_D, SUM(CASE 
				WHEN SCredit_Debit = 1 THEN A.BWBStartBAL
				ELSE 0
			END) AS BWBStart_D
			, SUM(A.NUMDebitTotal) AS Num_CJ, SUM(A.BWBDebitTotal) AS BWB_CJ
			, SUM(A.NUMCreditTotal) AS NUM_CD, SUM(A.BWBCreditTotal) AS BWB_CD
			, SUM(A.NUMDebitTotal_Y) AS Num_YJ, SUM(A.BWBDebitTotal_Y) AS BWB_YJ
			, SUM(A.NUMCreditTotal_Y) AS Num_YD, SUM(A.BWBCreditTotal_Y) AS BWB_YD
			, SUM(CASE 
				WHEN ECredit_Debit = 0 THEN A.NUMEndBAL
				ELSE 0
			END) AS NumEnd_J, SUM(CASE 
				WHEN ECredit_Debit = 0 THEN A.BWBEndBAL
				ELSE 0
			END) AS BWBEnd_J
			, SUM(CASE 
				WHEN ECredit_Debit = 1 THEN A.NUMEndBAL
				ELSE 0
			END) AS NumEnd_D, SUM(CASE 
				WHEN ECredit_Debit = 1 THEN A.BWBEndBAL
				ELSE 0
			END) AS BWBEnd_D
		FROM (
			SELECT a.AccountId, A.subjectId, a.Category, a.PeriodId, a.SubjectCode
				, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal, a.NUMDebitTotal_Y
				, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y, a.NUMCreditTotal
				, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal, a.YBCreditTotal_Y
				, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL, a.YearStartBWBBAL
				, a.YearStartYBBAL, a.isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid, a.isLeaf
				, 1 AS Type, a.NUMEndBAL, a.YBEndBAL, a.BWBEndBAL, a.ECredit_Debit
				, 1 AS RootName
			FROM TKS_FAS_GLBalance A
			UNION ALL
			SELECT a.AccountId, '' AS subjectId, a.Category, a.PeriodId, a.SubjectCode
				, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal, a.NUMDebitTotal_Y
				, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y, a.NUMCreditTotal
				, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal, a.YBCreditTotal_Y
				, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL, a.YearStartBWBBAL
				, a.YearStartYBBAL, 0 AS isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid, 1 AS isLeaf
				, 2 AS Type, a.NUMEndBAL, a.YBEndBAL, a.BWBEndBAL, a.ECredit_Debit
				, 0 AS RootName
			FROM TKS_FAS_FGLBalance A
		) A
			LEFT JOIN TKS_FAS_MonthPeriodInfo ON TKS_FAS_MonthPeriodInfo.Id = A.PeriodId
            left join TKS_FAS_AccountSubject on TKS_FAS_AccountSubject.Code=A.SubjectCode 
			 and TKS_FAS_AccountSubject.AccountId=A.AccountId
		WHERE A.accountId = @AccountId
			{0} and A.periodId=@StartPeriod
		GROUP BY SubjectCode,TKS_FAS_AccountSubject.Name
", where1);
                    List<ZB_KMBAL> StartData = cnn.Query<ZB_KMBAL>(start_sql,
                        new
                        {
                            AccountId = request.AccountList == null ? user.AccountId : request.AccountList,//账套
                            StartPeriod = StartPeriod

                        }, ts).ToList();
                    List<ZB_KMBAL> EndData = cnn.Query<ZB_KMBAL>(start_sql,
                        new
                        {
                            AccountId = request.AccountList == null ? user.AccountId : request.AccountList,//账套
                            StartPeriod = EndPeriod

                        }, ts).ToList();
                    List<ZB_KMBAL> newData = new List<ZB_KMBAL>();
                    List<ZB_KMBAL> data = cnn.Query<ZB_KMBAL>(sql,
                        new
                        {
                            AccountId = request.AccountList == null ? user.AccountId : request.AccountList//账套


                        }, ts).ToList();
                    for (var i = 0; i < data.Count; i++)
                    {
                        //string name = GetNameByCode(data[i].Code, request.AccountList == null ? user.AccountId : request.AccountList, ts);
                        //data[i].Name = name == string.Empty ? data[i].Name : name;

                        var lstStart = StartData.Where(s => s.Code == data[i].Code).ToList();
                        if (lstStart.Count > 0)
                        {
                            data[i].NumStart_J = lstStart[0].NumStart_J;
                            data[i].BWBStart_J = lstStart[0].BWBStart_J;
                            data[i].NumStart_D = lstStart[0].NumStart_D;
                            data[i].BWBStart_D = lstStart[0].BWBStart_D;

                            if (data[i].BWBStart_J > data[i].BWBStart_D)
                            {
                                data[i].BWBStart_J = data[i].BWBStart_J - data[i].BWBStart_D;
                                data[i].BWBStart_D = 0;
                            }
                            else
                            {
                                data[i].BWBStart_D = data[i].BWBStart_D - data[i].BWBStart_J;
                                data[i].BWBStart_J = 0;
                            }
                        }
                        else
                        {
                            data[i].NumStart_J = 0;
                            data[i].BWBStart_J = 0;
                            data[i].NumStart_D = 0;
                            data[i].BWBStart_D = 0;
                        }
                        var lstEnd = EndData.Where(s => s.Code == data[i].Code).ToList();
                        if (lstEnd.Count > 0)
                        {
                            data[i].NumEnd_J = lstEnd[0].NumEnd_J;
                            data[i].BWBEnd_J = lstEnd[0].BWBEnd_J;
                            data[i].NumEnd_D = lstEnd[0].NumEnd_D;
                            data[i].BWBEnd_D = lstEnd[0].BWBEnd_D;

                            if (data[i].BWBEnd_J > data[i].BWBEnd_D)
                            {
                                data[i].BWBEnd_J = data[i].BWBEnd_J - data[i].BWBEnd_D;
                                data[i].BWBEnd_D = 0;
                            }
                            else
                            {
                                data[i].BWBEnd_D = data[i].BWBEnd_D - data[i].BWBEnd_J;
                                data[i].BWBEnd_J = 0;
                            }
                        }
                        else
                        {
                            data[i].NumEnd_J = 0;
                            data[i].BWBEnd_J = 0;
                            data[i].NumEnd_D = 0;
                            data[i].BWBEnd_D = 0;
                        }
                        if (data[i].Code.IndexOf("5") == 0)
                        {
                            //损益类
                            //if (data[i].BWB_CJ > 0)
                            //{
                            //    data[i].BWB_CD = data[i].BWB_CJ;
                            //    data[i].BWB_CJ = 0;
                            //}
                            //else
                            //{
                            //    data[i].BWB_CJ = data[i].BWB_CD;
                            //    data[i].BWB_CD = 0;
                            //}
                            data[i].BWBStart_J = 0;
                            data[i].BWBStart_D = 0;
                            data[i].BWB_YJ = 0;
                            data[i].BWB_YD = 0;
                            data[i].BWBEnd_J = 0;
                            data[i].BWBEnd_D = 0;
                        }
                    }
                    if (string.IsNullOrEmpty(request.IsFuZhu))
                    {
                        newData = data;
                    }
                    else
                    {
                        string sql_cal = string.Format(@"
	SELECT  'Y' as IsCal,calDetail.Code as calCode, SubjectCode AS code, calDetail.Name
            ,CONCAT(A.CalItem1,A.CalItem2,A.CalItem3,A.CalItem4,A.CalItem5) as CalItem
			, SUM(CASE 
				WHEN SCredit_Debit = 0 THEN NUMStartBAL
				ELSE 0
			END) AS NumStart_J, SUM(CASE 
				WHEN SCredit_Debit = 0 THEN A.BWBStartBAL
				ELSE 0
			END) AS BWBStart_J
			, SUM(CASE 
				WHEN SCredit_Debit = 1 THEN NUMStartBAL
				ELSE 0
			END) AS NumStart_D, SUM(CASE 
				WHEN SCredit_Debit = 1 THEN A.BWBStartBAL
				ELSE 0
			END) AS BWBStart_D
			, SUM(A.NUMDebitTotal) AS Num_CJ, SUM(A.BWBDebitTotal) AS BWB_CJ
			, SUM(A.NUMCreditTotal) AS NUM_CD, SUM(A.BWBCreditTotal) AS BWB_CD
			, SUM(A.NUMDebitTotal_Y) AS Num_YJ, SUM(A.BWBDebitTotal_Y) AS BWB_YJ
			, SUM(A.NUMCreditTotal_Y) AS Num_YD, SUM(A.BWBCreditTotal_Y) AS BWB_YD
			, SUM(CASE 
				WHEN ECredit_Debit = 0 THEN A.NUMEndBAL
				ELSE 0
			END) AS NumEnd_J, SUM(CASE 
				WHEN ECredit_Debit = 0 THEN A.BWBEndBAL
				ELSE 0
			END) AS BWBEnd_J
			, SUM(CASE 
				WHEN ECredit_Debit = 1 THEN A.NUMEndBAL
				ELSE 0
			END) AS NumEnd_D, SUM(CASE 
				WHEN ECredit_Debit = 1 THEN A.BWBEndBAL
				ELSE 0
			END) AS BWBEnd_D
		FROM (
			SELECT a.AccountId, '' AS subjectId, a.Category, a.PeriodId, a.SubjectCode
				, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal, a.NUMDebitTotal_Y
				, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y, a.NUMCreditTotal
				, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal, a.YBCreditTotal_Y
				, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL, a.YearStartBWBBAL
				, a.YearStartYBBAL, 0 AS isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid, 1 AS isLeaf
				, 2 AS Type, a.NUMEndBAL, a.YBEndBAL, a.BWBEndBAL, a.ECredit_Debit
				, a.CalItem1,a.CalValue1,a.CalItem2,a.CalValue2,a.CalItem3,a.CalValue3,a.CalItem4,a.CalValue4
				,a.CalItem5,a.CalValue5
			FROM TKS_FAS_FGLBalance A
		) A
			LEFT JOIN TKS_FAS_MonthPeriodInfo ON TKS_FAS_MonthPeriodInfo.Id = A.PeriodId
			left join TKS_FAS_CaculateHelperDetail calDetail
			on calDetail.ParentId in(A.CalItem1,A.CalItem2,A.CalItem3,A.CalItem4,A.CalItem5)
			and calDetail.Code in(A.CalValue1,A.CalValue2,A.CalValue3,A.CalValue4,A.CalValue5)
		WHERE A.accountId = @AccountId
			{0}
		GROUP BY SubjectCode,calDetail.Name,calDetail.Code,A.CalItem1,A.CalItem2,A.CalItem3,A.CalItem4,A.CalItem5
       order by calCode
", where1);
                        List<ZB_KMBAL> calHelper = cnn.Query<ZB_KMBAL>(sql_cal,
                       new
                       {
                           AccountId = request.AccountList == null ? user.AccountId : request.AccountList//账套


                       }, ts).ToList();
                        start_sql = string.Format(@"
	SELECT  'Y' as IsCal,calDetail.Code as calCode, SubjectCode AS code, calDetail.Name
,CONCAT(A.CalItem1,A.CalItem2,A.CalItem3,A.CalItem4,A.CalItem5) as CalItem
			, SUM(CASE 
				WHEN SCredit_Debit = 0 THEN NUMStartBAL
				ELSE 0
			END) AS NumStart_J, SUM(CASE 
				WHEN SCredit_Debit = 0 THEN A.BWBStartBAL
				ELSE 0
			END) AS BWBStart_J
			, SUM(CASE 
				WHEN SCredit_Debit = 1 THEN NUMStartBAL
				ELSE 0
			END) AS NumStart_D, SUM(CASE 
				WHEN SCredit_Debit = 1 THEN A.BWBStartBAL
				ELSE 0
			END) AS BWBStart_D
			, SUM(A.NUMDebitTotal) AS Num_CJ, SUM(A.BWBDebitTotal) AS BWB_CJ
			, SUM(A.NUMCreditTotal) AS NUM_CD, SUM(A.BWBCreditTotal) AS BWB_CD
			, SUM(A.NUMDebitTotal_Y) AS Num_YJ, SUM(A.BWBDebitTotal_Y) AS BWB_YJ
			, SUM(A.NUMCreditTotal_Y) AS Num_YD, SUM(A.BWBCreditTotal_Y) AS BWB_YD
			, SUM(CASE 
				WHEN ECredit_Debit = 0 THEN A.NUMEndBAL
				ELSE 0
			END) AS NumEnd_J, SUM(CASE 
				WHEN ECredit_Debit = 0 THEN A.BWBEndBAL
				ELSE 0
			END) AS BWBEnd_J
			, SUM(CASE 
				WHEN ECredit_Debit = 1 THEN A.NUMEndBAL
				ELSE 0
			END) AS NumEnd_D, SUM(CASE 
				WHEN ECredit_Debit = 1 THEN A.BWBEndBAL
				ELSE 0
			END) AS BWBEnd_D
		FROM (
			SELECT a.AccountId, '' AS subjectId, a.Category, a.PeriodId, a.SubjectCode
				, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal, a.NUMDebitTotal_Y
				, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y, a.NUMCreditTotal
				, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal, a.YBCreditTotal_Y
				, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL, a.YearStartBWBBAL
				, a.YearStartYBBAL, 0 AS isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid, 1 AS isLeaf
				, 2 AS Type, a.NUMEndBAL, a.YBEndBAL, a.BWBEndBAL, a.ECredit_Debit
				, a.CalItem1,a.CalValue1,a.CalItem2,a.CalValue2,a.CalItem3,a.CalValue3,a.CalItem4,a.CalValue4
				,a.CalItem5,a.CalValue5
			FROM TKS_FAS_FGLBalance A
		) A
			LEFT JOIN TKS_FAS_MonthPeriodInfo ON TKS_FAS_MonthPeriodInfo.Id = A.PeriodId
			left join TKS_FAS_CaculateHelperDetail calDetail
			on calDetail.ParentId in(A.CalItem1,A.CalItem2,A.CalItem3,A.CalItem4,A.CalItem5)
			and calDetail.Code in(A.CalValue1,A.CalValue2,A.CalValue3,A.CalValue4,A.CalValue5)
		WHERE A.accountId = @AccountId
			{0} and A.periodId=@StartPeriod
		GROUP BY SubjectCode,calDetail.Name,calDetail.Code,A.CalItem1,A.CalItem2,A.CalItem3,A.CalItem4,A.CalItem5
       order by calCode
", where1);

                        StartData = cnn.Query<ZB_KMBAL>(start_sql,
                        new
                        {
                            AccountId = request.AccountList == null ? user.AccountId : request.AccountList,//账套
                            StartPeriod = StartPeriod

                        }, ts).ToList();
                        EndData = cnn.Query<ZB_KMBAL>(start_sql,
                        new
                        {
                            AccountId = request.AccountList == null ? user.AccountId : request.AccountList,//账套
                            StartPeriod = EndPeriod

                        }, ts).ToList();
                        foreach (var item in data)
                        {
                            newData.Add(item);
                            List<ZB_KMBAL> newCal = new List<ZB_KMBAL>();
                            var lstCal = calHelper.Where(s => s.Code == item.Code).ToList();
                            if (lstCal.Count > 0)
                            {
                                foreach (var cal in lstCal)
                                {

                                    var lstStart = StartData.Where(s => s.calCode == cal.calCode && s.CalItem == cal.CalItem).ToList();
                                    if (lstStart.Count > 0)
                                    {
                                        cal.NumStart_J = lstStart[0].NumStart_J;
                                        cal.BWBStart_J = lstStart[0].BWBStart_J;
                                        cal.NumStart_D = lstStart[0].NumStart_D;
                                        cal.BWBStart_D = lstStart[0].BWBStart_D;

                                        if (cal.BWBStart_J > cal.BWBStart_D)
                                        {
                                            cal.BWBStart_J = cal.BWBStart_J - cal.BWBStart_D;
                                            cal.BWBStart_D = 0;
                                        }
                                        else
                                        {
                                            cal.BWBStart_D = cal.BWBStart_D - cal.BWBStart_J;
                                            cal.BWBStart_J = 0;
                                        }
                                    }
                                    else
                                    {
                                        cal.NumStart_J = 0;
                                        cal.BWBStart_J = 0;
                                        cal.NumStart_D = 0;
                                        cal.BWBStart_D = 0;
                                    }
                                    var lstEnd = EndData.Where(s => s.calCode == cal.calCode && s.CalItem == cal.CalItem).ToList();
                                    if (lstEnd.Count > 0)
                                    {
                                        cal.NumEnd_J = lstEnd[0].NumEnd_J;
                                        cal.BWBEnd_J = lstEnd[0].BWBEnd_J;
                                        cal.NumEnd_D = lstEnd[0].NumEnd_D;
                                        cal.BWBEnd_D = lstEnd[0].BWBEnd_D;

                                        if (cal.BWBEnd_J > cal.BWBEnd_D)
                                        {
                                            cal.BWBEnd_J = cal.BWBEnd_J - cal.BWBEnd_D;
                                            cal.BWBEnd_D = 0;
                                        }
                                        else
                                        {
                                            cal.BWBEnd_D = cal.BWBEnd_D - cal.BWBEnd_J;
                                            cal.BWBEnd_J = 0;
                                        }
                                    }
                                    else
                                    {
                                        cal.NumEnd_J = 0;
                                        cal.BWBEnd_J = 0;
                                        cal.NumEnd_D = 0;
                                        cal.BWBEnd_D = 0;
                                    }
                                    cal.Code = "  " + cal.Code + "_" + cal.calCode;
                                    newCal.Add(cal);

                                }
                                newData.AddRange(newCal);
                            }
                        }
                    }
                    List<ZB_KMBALHead> lsHead = new List<ZB_KMBALHead>();
                    ZB_KMBALComparer comparer = new BLL.ZB_KMBALComparer();
                    var lsData = data.Distinct(comparer);
                    //if (!string.IsNullOrEmpty(request.AccountList))
                    //{
                    //lsHead = (from item in lsData
                    //          select new ZB_KMBALHead
                    //          {
                    //              Year = item.Year,
                    //              Month = item.Month,
                    //              Data = data.Where(p => p.PeriodId == item.PeriodId).ToList()
                    //          }).Distinct().OrderBy(p => p.PeriodId).ToList();
                    //}

                    string countSql = A + @"select count(1) from " + table;

                    //int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    //{
                    //    AccountId = request.AccountList == null ? user.AccountId : request.AccountList//账套

                    //}, ts).ToString());

                    //sql = @"select * from tks_fas_accountSubject where accountId=@AccountId";
                    sql = @"select a.Code,ISNULL(b.Code,a.Code) as ParentCode,a.SLevel,a.Name,a.Category  from TKS_FAS_AccountSubject a
                            left join TKS_FAS_AccountSubject b
                            on a.ParentId=b.Id and a.AccountId=b.AccountId 
                          where a.accountid=@AccountId";
                    var subjectData = cnn.Query<Subject>(sql, new { AccountId = request.AccountList == null ? user.AccountId : request.AccountList }, ts).ToList();
                    Dictionary<string, Subject> dicSub = new Dictionary<string, Subject>();
                    foreach (var item in subjectData)
                    {
                        dicSub.Add(item.Code, item);
                    }
                    foreach (var item in newData)
                    {
                        item.IsChild = "Y";//是最小子科目，可以点击连接到【明显帐】
                        if (item.Code.Split('_').Length > 0)
                        {
                            item.ParentCode = dicSub[item.Code.Split('_')[0].Trim()].ParentCode;
                            item.SLevel = dicSub[item.Code.Split('_')[0].Trim()].SLevel;
                            item.Category = dicSub[item.Code.Split('_')[0].Trim()].Category;
                        }
                        else
                        {
                            item.ParentCode = dicSub[item.Code].ParentCode;
                            item.SLevel = dicSub[item.Code].SLevel;
                            item.Category = dicSub[item.Code].Category;
                        }

                    }
                    //科目汇总
                    GetGroupData(newData, dicSub);

                    ts.Commit();
                    int total = lstData.Count;
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    //response.Data = data;
                    //response.Data = newData.Where(p => (p.BWBStart_J + p.BWBStart_D + p.BWB_CJ + p.BWB_CD + p.BWBEnd_J + p.BWBEnd_D) > 0).ToList();
                    //response.Data = newData;
                    response.Data = lstData.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
                    response.PrintData = lstData;//打印数据
                    response.Total = total;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseKMBalListSearch;
                }
            }
        }
        List<ZB_KMBAL> lstData = new List<ZB_KMBAL>();
        List<ZB_KMBAL> lstGroup = new List<ZB_KMBAL>();
        public void GetGroupData(List<ZB_KMBAL> data, Dictionary<string, Subject> dicSub)
        {
            int flag = 0;
            List<ZB_KMBAL> lst = new List<ZB_KMBAL>();
            if (data.Count > 0 && dicSub.Count > 0)
            {
                foreach (var item in data)
                {
                    if (dicSub.ContainsKey(item.Code))
                    {
                        Subject sub = dicSub[item.Code];
                        if (item.Code == sub.ParentCode || item.ParentCode == "")
                        {
                            lst.Add(item);
                            //continue;
                        }
                        else
                        {
                            flag++;
                            ZB_KMBAL bal = new ZB_KMBAL();

                            bal.Code = sub.ParentCode;
                            bal.Name = dicSub[bal.Code].Name;
                            bal.IsCal = "N";
                            bal.Category = dicSub[bal.Code].Category;
                            bal.SLevel = dicSub[bal.Code].SLevel;
                            bal.ParentCode = dicSub[bal.Code].ParentCode == bal.Code ? "" : dicSub[bal.Code].ParentCode;
                            List<ZB_KMBAL> d_data = data.Where(s => s.ParentCode == sub.ParentCode && s.IsCal != "Y").ToList();
                            bal.NumStart_J = d_data.Sum(s => s.NumStart_J);
                            bal.BWBStart_J = d_data.Sum(s => s.BWBStart_J);
                            bal.NumStart_D = d_data.Sum(s => s.NumStart_D);
                            bal.BWBStart_D = d_data.Sum(s => s.BWBStart_D);
                            bal.Num_CJ = d_data.Sum(s => s.Num_CJ);
                            bal.BWB_CJ = d_data.Sum(s => s.BWB_CJ);
                            bal.NUM_CD = d_data.Sum(s => s.NUM_CD);
                            bal.BWB_CD = d_data.Sum(s => s.BWB_CD);
                            bal.Num_YJ = d_data.Sum(s => s.Num_YJ);
                            bal.BWB_YJ = d_data.Sum(s => s.BWB_YJ);
                            bal.Num_YD = d_data.Sum(s => s.Num_YD);
                            bal.BWB_YD = d_data.Sum(s => s.BWB_YD);
                            bal.NumEnd_J = d_data.Sum(s => s.NumEnd_J);
                            bal.BWBEnd_J = d_data.Sum(s => s.BWBEnd_J);
                            bal.NumEnd_D = d_data.Sum(s => s.NumEnd_D);
                            bal.BWBEnd_D = d_data.Sum(s => s.BWBEnd_D);
                            if (bal.BWBStart_J > bal.BWBStart_D)
                            {
                                bal.BWBStart_J = bal.BWBStart_J - bal.BWBStart_D;
                                bal.BWBStart_D = 0;
                            }
                            else
                            {
                                bal.BWBStart_D = bal.BWBStart_D - bal.BWBStart_J;
                                bal.BWBStart_J = 0;
                            }
                            if (bal.BWBEnd_J > bal.BWBEnd_D)
                            {
                                bal.BWBEnd_J = bal.BWBEnd_J - bal.BWBEnd_D;
                                bal.BWBEnd_D = 0;
                            }
                            else
                            {
                                bal.BWBEnd_D = bal.BWBEnd_D - bal.BWBEnd_J;
                                bal.BWBEnd_J = 0;
                            }

                            lst.Add(bal);

                            foreach (var dd in d_data)
                            {
                                dd.ParentCode = "";
                            }


                            lst.Add(item);
                        }
                    }
                    else if (item.Code.Split('_').Length > 0 && dicSub.ContainsKey(item.Code.Split('_')[0].Trim()))
                    {
                        lst.Add(item);
                    }
                }

                if (flag > 0)
                {
                    //var d_lst = GetGroupData(lst, dicSub);
                    GetGroupData(lst, dicSub);
                }
                else
                {
                    lstGroup = lst.GroupBy(s => new { s.Code, s.Name, s.IsCal, s.SLevel, s.Category, s.IsChild }).Select(s => new ZB_KMBAL
                    {
                        Code = s.First().Code,
                        Name = s.First().Name,
                        IsCal = s.First().IsCal,
                        SLevel = s.First().SLevel,
                        Category = s.First().Category,
                        IsChild = s.First().IsChild,
                        NumStart_J = s.Sum(x => x.NumStart_J),
                        BWBStart_J = s.Sum(x => x.BWBStart_J),
                        NumStart_D = s.Sum(x => x.NumStart_D),
                        BWBStart_D = s.Sum(x => x.BWBStart_D),
                        Num_CJ = s.Sum(x => x.Num_CJ),
                        BWB_CJ = s.Sum(x => x.BWB_CJ),
                        NUM_CD = s.Sum(x => x.NUM_CD),
                        BWB_CD = s.Sum(x => x.BWB_CD),
                        Num_YJ = s.Sum(x => x.Num_YJ),
                        BWB_YJ = s.Sum(x => x.BWB_YJ),
                        Num_YD = s.Sum(x => x.Num_YD),
                        BWB_YD = s.Sum(x => x.BWB_YD),
                        NumEnd_J = s.Sum(x => x.NumEnd_J),
                        BWBEnd_J = s.Sum(x => x.BWBEnd_J),
                        NumEnd_D = s.Sum(x => x.NumEnd_D),
                        BWBEnd_D = s.Sum(x => x.BWBEnd_D)
                    }).ToList();
                    foreach (var bal in lstGroup)
                    {
                        if (bal.BWBStart_J > bal.BWBStart_D)
                        {
                            bal.BWBStart_J = bal.BWBStart_J - bal.BWBStart_D;
                            bal.BWBStart_D = 0;
                        }
                        else
                        {
                            bal.BWBStart_D = bal.BWBStart_D - bal.BWBStart_J;
                            bal.BWBStart_J = 0;
                        }
                        if (bal.BWBEnd_J > bal.BWBEnd_D)
                        {
                            bal.BWBEnd_J = bal.BWBEnd_J - bal.BWBEnd_D;
                            bal.BWBEnd_D = 0;
                        }
                        else
                        {
                            bal.BWBEnd_D = bal.BWBEnd_D - bal.BWBEnd_J;
                            bal.BWBEnd_J = 0;
                        }

                    }
                    var Category1 = lstGroup.Where(s => s.Category == "1").ToList();
                    var Category2 = lstGroup.Where(s => s.Category == "2").ToList();
                    var Category3 = lstGroup.Where(s => s.Category == "3").ToList();
                    var Category4 = lstGroup.Where(s => s.Category == "4").ToList();
                    var Category5 = lstGroup.Where(s => s.Category == "5").ToList();

                    var Category_1 = lstGroup.Where(s => s.Category == "1" && s.SLevel == 1).ToList();
                    var Category_2 = lstGroup.Where(s => s.Category == "2" && s.SLevel == 1).ToList();
                    var Category_3 = lstGroup.Where(s => s.Category == "3" && s.SLevel == 1).ToList();
                    var Category_4 = lstGroup.Where(s => s.Category == "4" && s.SLevel == 1).ToList();
                    var Category_5 = lstGroup.Where(s => s.Category == "5" && s.SLevel == 1).ToList();
                    int c1 = 0, c2 = 0, c3 = 0, c4 = 0, c5 = 0;
                    foreach (var item in lstGroup)
                    {
                        lstData.Add(item);
                        if (item.Category == "1")
                        {

                            if (c1 == Category1.Count - 1)
                            {
                                ZB_KMBAL bal1 = GetSumSubject("资产小计", Category_1);
                                lstData.Add(bal1);
                            }
                            c1++;
                        }
                        if (item.Category == "2")
                        {

                            if (c2 == Category2.Count - 1)
                            {
                                ZB_KMBAL bal2 = GetSumSubject("负债小计", Category_2);
                                lstData.Add(bal2);

                            }
                            c2++;
                        }
                        if (item.Category == "3")
                        {

                            if (c3 == Category3.Count - 1)
                            {
                                ZB_KMBAL bal3 = GetSumSubject("权益小计", Category_3);
                                lstData.Add(bal3);
                            }
                            c3++;
                        }
                        if (item.Category == "4")
                        {

                            if (c4 == Category4.Count - 1)
                            {
                                ZB_KMBAL bal4 = GetSumSubject("成本小计", Category_4);
                                lstData.Add(bal4);
                            }
                            c4++;
                        }
                        if (item.Category == "5")
                        {

                            if (c5 == Category5.Count - 1)
                            {
                                ZB_KMBAL bal5 = GetSumSubject("损益小计", Category_5);
                                lstData.Add(bal5);
                            }
                            c5++;
                        }
                    }
                    var Category_sum = lstData.Where(s => s.Category == "" && s.SLevel == 0).ToList();
                    ZB_KMBAL bal_sum = GetSumSubject("合计", Category_sum);
                    lstData.Add(bal_sum);
                    //lstData = lstData.Distinct().ToList();
                }
            }
            //return lst;
        }

        public ZB_KMBAL GetSumSubject(string Desc, List<ZB_KMBAL> d_data)
        {
            ZB_KMBAL bal = new ZB_KMBAL();
            bal.Code = Desc;
            bal.Name = "";
            bal.IsCal = "N";
            bal.Category = "";
            bal.SLevel = 0;
            bal.ParentCode = "";
            bal.NumStart_J = d_data.Sum(s => s.NumStart_J);
            bal.BWBStart_J = d_data.Sum(s => s.BWBStart_J);
            bal.NumStart_D = d_data.Sum(s => s.NumStart_D);
            bal.BWBStart_D = d_data.Sum(s => s.BWBStart_D);
            bal.Num_CJ = d_data.Sum(s => s.Num_CJ);
            bal.BWB_CJ = d_data.Sum(s => s.BWB_CJ);
            bal.NUM_CD = d_data.Sum(s => s.NUM_CD);
            bal.BWB_CD = d_data.Sum(s => s.BWB_CD);
            bal.Num_YJ = d_data.Sum(s => s.Num_YJ);
            bal.BWB_YJ = d_data.Sum(s => s.BWB_YJ);
            bal.Num_YD = d_data.Sum(s => s.Num_YD);
            bal.BWB_YD = d_data.Sum(s => s.BWB_YD);
            bal.NumEnd_J = d_data.Sum(s => s.NumEnd_J);
            bal.BWBEnd_J = d_data.Sum(s => s.BWBEnd_J);
            bal.NumEnd_D = d_data.Sum(s => s.NumEnd_D);
            bal.BWBEnd_D = d_data.Sum(s => s.BWBEnd_D);
            return bal;
        }
        string GetNameByCode(string code, string accountId, IDbTransaction ts)
        {
            string sql = @"select * from tks_fas_accountSubject where accountId=@AccountId";

            var subjectData = cnn.Query<TKS_FAS_AccountSubject>(sql, new { AccountId = accountId }, ts).ToList();
            var d = subjectData.Where(p => p.Code == code).SingleOrDefault();
            if (d == null)
            {
                return string.Empty;
            }
            else
            {
                return d.ShowTitle;
            }
        }

        /// <summary>
        /// 03 科目余额表 全部
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseKMBalListSearch BALAccountGet(RequestKMBalListSearch request)
        {

            ResponseKMBalListSearch response = new ResponseKMBalListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    List<CodeValue> subjectCode = GetDocSubjectCode(request.PeriodId, ts, user, request.allPZ);
                    List<CodeValue> qcCode = GetQCCode(user, ts);
                    subjectCode.AddRange(qcCode);
                    List<CodeValue> fzCode = GetFZCode(user, ts);
                    subjectCode.AddRange(fzCode);

                    string where = string.Empty;

                    for (var i = 0; i < subjectCode.Count; i++)
                    {
                        if (i != 0)
                        {
                            where += "or A.SubjectCode='" + subjectCode[i].Code + "' ";
                        }
                        else
                        {
                            where += " A.SubjectCode='" + subjectCode[i].Code + "' ";
                        }
                    }
                    //科目条件没有获取到则过滤所有
                    where = "(" + (string.IsNullOrEmpty(where) ? "1=2" : where) + ")";

                    string table = @"(  
 select 
 SubjectCode as code,
  max( (case when RootName=1 then name end)) name,
 sum( (case when SCredit_Debit = 0 then NUMStartBAL else 0 end)) as NumStart_J,   --期初借方数量
  sum( (case when SCredit_Debit = 0 then  A.BWBStartBAL else 0 end)) as BWBStart_J, --期初本位币

   sum(   (case when SCredit_Debit = 1 then NUMStartBAL else 0 end)) as NumStart_D,   --期初借方数量
    sum( (case when SCredit_Debit = 1 then  A.BWBStartBAL else 0 end)) as BWBStart_D, --期初本位币
       

  sum( A.NUMDebitTotal)as Num_CJ ,--本期借方发生额数量


  sum( A.BWBDebitTotal) as BWB_CJ,  --本期借方发生额本位币

  sum( A.NUMCreditTotal) as NUM_CD,
   
   sum(A.BWBCreditTotal) as BWB_CD,--本期贷方发生额本位币

   sum(A.NUMDebitTotal_Y) as Num_YJ,--本年累计发生额数量


   sum(A.BWBDebitTotal_Y) as BWB_YJ,--本年累计发生额本位币

   sum(A.NUMCreditTotal_Y) Num_YD,--本年贷方累计发生额数量


   sum(A.BWBCreditTotal_Y) as BWB_YD,--本年贷方累计发生额本位币

   sum((case when ECredit_Debit = 0 then A.NUMEndBAL else 0 end)) as NumEnd_J,   --期末借方数量
   sum((case when ECredit_Debit = 0 then  A.BWBEndBAL else 0 end)) as BWBEnd_J, --期末本位币

     sum( (case when ECredit_Debit = 1 then A.NUMEndBAL else 0 end)) as NumEnd_D,   --期末借方数量
    sum( (case when ECredit_Debit = 1 then  A.BWBEndBAL else 0 end)) as BWBEnd_D --期末本位币

from (
  select a.AccountId,A.subjectId,a.Category,a.PeriodId,a.SubjectCode,a.Name,a.CurrencyCode,a.SCredit_Debit,
a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
a.isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,a.isLeaf, 1 as Type
,a.NUMEndBAL,a.YBEndBAL,a.BWBEndBAL,a.ECredit_Debit ,1 as RootName
  from TKS_FAS_GLBalance A
  union

  select a.AccountId,'' as subjectId,a.Category,a.PeriodId,a.SubjectCode,a.Name,a.CurrencyCode,a.SCredit_Debit,
a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
0 as  isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,1 as isLeaf, 2 as Type
,a.NUMEndBAL,a.YBEndBAL,a.BWBEndBAL,a.ECredit_Debit , 0 as RootName
  from TKS_FAS_FGLBalance A 
 ) A   where A.accountId=@AccountId 
                          and A.PeriodId = @PeriodId and " + where + " group by SubjectCode  ) M";

                    string sql = this.GetPageSql(" M.* ",
                            table, @" M.Code ",
                            request.PageSize, request.PageIndex);


                    List<ZB_KMBAL> data = cnn.Query<ZB_KMBAL>(sql,
                        new
                        {
                            AccountId = user.AccountId,//账套

                            PeriodId = request.PeriodId//期间
                        }, ts).ToList();

                    string countSql = @"select count(1) from " + table;

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = user.AccountId,//账套
                        PeriodId = request.PeriodId,

                    }, ts).ToString());


                    sql = @"select 
  '' as code,
  '合计' name,
 sum( (case when SCredit_Debit = 0 then NUMStartBAL else 0 end)) as NumStart_J,   --期初借方数量
  sum( (case when SCredit_Debit = 0 then  A.BWBStartBAL else 0 end)) as BWBStart_J, --期初本位币

   sum(   (case when SCredit_Debit = 1 then NUMStartBAL else 0 end)) as NumStart_D,   --期初借方数量
    sum( (case when SCredit_Debit = 1 then  A.BWBStartBAL else 0 end)) as BWBStart_D, --期初本位币
       

  sum( A.NUMDebitTotal)as Num_CJ ,--本期借方发生额数量


  sum( A.BWBDebitTotal) as BWB_CJ,  --本期借方发生额本位币

  sum( A.NUMCreditTotal) as NUM_CD,
   
   sum(A.BWBCreditTotal) as BWB_CD,--本期贷方发生额本位币

   sum(A.NUMDebitTotal_Y) as Num_YJ,--本年累计发生额数量


   sum(A.BWBDebitTotal_Y) as BWB_YJ,--本年累计发生额本位币

   sum(A.NUMCreditTotal_Y) Num_YD,--本年贷方累计发生额数量


   sum(A.BWBCreditTotal_Y) as BWB_YD,--本年贷方累计发生额本位币

   sum((case when ECredit_Debit = 0 then A.NUMEndBAL else 0 end)) as NumEnd_J,   --期末借方数量
   sum((case when ECredit_Debit = 0 then  A.BWBEndBAL else 0 end)) as BWBEnd_J, --期末本位币

     sum( (case when ECredit_Debit = 1 then A.NUMEndBAL else 0 end)) as NumEnd_D,   --期末借方数量
    sum( (case when ECredit_Debit = 1 then  A.BWBEndBAL else 0 end)) as BWBEnd_D --期末本位币
from (
  select a.AccountId,A.subjectId,a.Category,a.PeriodId,a.SubjectCode,a.Name,a.CurrencyCode,a.SCredit_Debit,
a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
a.isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,a.isLeaf, 1 as Type
,a.NUMEndBAL,a.YBEndBAL,a.BWBEndBAL,a.ECredit_Debit ,1 as RootName
  from TKS_FAS_GLBalance A
  union

  select a.AccountId,'' as subjectId,a.Category,a.PeriodId,a.SubjectCode,a.Name,a.CurrencyCode,a.SCredit_Debit,
a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
0 as  isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,1 as isLeaf, 2 as Type
,a.NUMEndBAL,a.YBEndBAL,a.BWBEndBAL,a.ECredit_Debit , 0 as RootName
  from TKS_FAS_FGLBalance A 
 ) A   where A.accountId=@AccountId 
                          and A.PeriodId = @PeriodId and " + where;


                    List<ZB_KMBAL> dataTotal = cnn.Query<ZB_KMBAL>(sql,
                        new
                        {
                            AccountId = user.AccountId,//账套

                            PeriodId = request.PeriodId//期间
                        }, ts).ToList();

                    data.AddRange(dataTotal);
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    response.Data = data;

                    response.Total = total;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseKMBalListSearch;
                }
            }
        }




        /// <summary>
        /// 获取辅助核算的科目
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private List<CodeValue> GetFZCode(MM_UserInfo user, IDbTransaction ts)
        {
            AccountSetBLL acc = new AccountSetBLL(cnn);
            var period = acc.GetFirstPeriod(user.AccountId, ts);
            string sql = @"select subjectCode as code ,name as value  from TKS_FAS_FGLBalance where   accountId=@AccountId
                        and periodId=@PeriodId";

            var data = cnn.Query<CodeValue>(sql, new
            {
                AccountId = user.AccountId,
                PeriodId = period.Id
            }, ts).ToList();

            return data;

        }

        /// <summary>
        /// 获取期初录入数值的科目
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private List<CodeValue> GetQCCode(MM_UserInfo user, IDbTransaction ts)
        {
            AccountSetBLL acc = new AccountSetBLL(cnn);
            var period = acc.GetFirstPeriod(user.AccountId, ts);
            string sql = @"select subjectCode as code ,name as value  from TKS_FAS_GLBalance where BWBStartBAL <> 0 and accountId=@AccountId
                        and periodId=@PeriodId
                        union
                        select subjectCode as code ,name as value  from TKS_FAS_FGLBalance where BWBStartBAL <> 0 and accountId=@AccountId
                        and periodId=@PeriodId";

            var data = cnn.Query<CodeValue>(sql, new
            {
                AccountId = user.AccountId,
                PeriodId = period.Id
            }, ts).ToList();

            return data;

        }

        /// <summary>
        /// 获取期间区间内的科目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseCodeGetInPeriod CodeGetInPeriod(RequestCodeGetInPeriod request)
        {

            ResponseCodeGetInPeriod res = new ResponseCodeGetInPeriod();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    //开始期间
                    var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                   @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                   new { Id = request.PeriodS }, ts);
                    //结束期间
                    var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                     @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                     new { Id = request.PeriodE }, ts);
                    if (periodS == null)
                    {
                        return new ResponseCodeGetInPeriod { IsSuccess = false, Message = "会计期间没有选择" };
                    }
                    //获取期间集合
                    List<TKS_FAS_MonthPeriodInfo> lsPeriod = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                        @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE ",
                        new
                        {
                            //StartDateS = ((DateTime)periodS.StartDate).ToString(),
                            //StartDateE = ((DateTime)periodE.StartDate).ToString(),
                            StartDateS = periodS.StartDate,
                            StartDateE = periodE.StartDate,
                            AccountId = user.AccountId
                        }, ts).ToList();
                    List<CodeValue> data = new List<CodeValue>();
                    foreach (var period in lsPeriod)
                    {
                        var _data = GetDocSubjectCode(period.Id, ts, user, "");
                        data.AddRange(_data);

                    }
                    CodeComparer cp = new CodeComparer();
                    data = data.Distinct(cp).ToList();

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "加载完成";
                    res.Data = data;
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseCodeGetInPeriod;
                }
            }
        }

        /// <summary>
        /// 获取科目，账簿查询项
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseDocSubjectCodeGet DocSubjectCodeGet(RequestDocSubjectCodeGet request)
        {
            ResponseDocSubjectCodeGet res = new ResponseDocSubjectCodeGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    List<CodeValue> data = GetDocSubjectCode(request.PeriodId, ts, user, "");

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "加载完成";
                    res.Data = data;
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseDocSubjectCodeGet;
                }
            }
        }

        /// <summary>
        /// 获取科目代码，去除有辅助核算的
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="ts"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private List<CodeValue> GetDocSubjectCode(string periodId, IDbTransaction ts, MM_UserInfo user, string allPZ)
        {
            //结转凭证科目过滤
            string sql = @" SELECT  B.showTitle as value ,A.SubjectCode as code
                            FROM TKS_FAS_DocDetail A left join TKS_FAS_AccountSubject B on
                            A.AccountId = B.AccountId and A.SubjectCode = B.Code
                            where A.source<>'JZ' and  A.accountId=@AccountId and A.PeriodId=@PeriodId and A.IsCalHelper=0
                            group by B.name,A.SubjectCode,B.showTitle";
            //if (!string.IsNullOrEmpty(allPZ))
            //{
            //    sql = @" SELECT  B.showTitle as value ,A.SubjectCode as code
            //                FROM TKS_FAS_DocDetail A left join TKS_FAS_AccountSubject B on
            //                A.AccountId = B.AccountId and A.SubjectCode = B.Code
            //                where A.accountId=@AccountId and A.PeriodId=@PeriodId 
            //                group by B.name,A.SubjectCode,B.showTitle";
            //}
            var data = cnn.Query<CodeValue>(sql,
                new { AccountId = user.AccountId, PeriodId = periodId }, ts).ToList();
            return data;
        }

        /// <summary>
        /// 04 科目汇总表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseSummaryListSearch SummaryListSearch(RequestSummaryListSearch request)
        {
            ResponseSummaryListSearch res = new ResponseSummaryListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    #region table
                    string table = @"
                        (
                        select 
                              M.RootCode as Code
	                          ,M.Name
                              ,sum(Money_Debit) as Money1
                             , sum(M.Money_Credit) as Money2
	                         ,sum(case when M.Credit_Debit=0 then  M.Quantity else 0 end) as NUM1
	                         ,sum(case when M.Credit_Debit=1 then M.Quantity else 0 end) as NUM2
	                           from (

                        select A.SubjectCode,A.Money_Credit,A.Money_Debit,A.Quantity,A.Credit_Debit,B.RootCode ,C.showTitle as name

                        from TKS_FAS_DocDetail A left join TKS_FAS_AccountSubject B 

                        on A.SubjectCode=B.Code and A.AccountId=B.AccountId
                        left join TKS_FAS_AccountSubject C on B.RootCode=C.Code and A.AccountId=C.AccountId
                        where {0} and A.source<>'JZ'
                         ) M group by  m.RootCode,m.Name
                         union 

                         select 
                              '9' 
	                          ,'合计'
                              ,sum(Money_Debit) as Money1
                             , sum(M.Money_Credit) as Money2
	                         ,sum(case when M.Credit_Debit=0 then  M.Quantity else 0 end) as NUM1
	                         ,sum(case when M.Credit_Debit=1 then M.Quantity else 0 end) as NUM2
	                           from (

                        select A.SubjectCode,A.Money_Credit,A.Money_Debit,A.Quantity,A.Credit_Debit,B.RootCode ,C.showTitle as name

                        from TKS_FAS_DocDetail A left join TKS_FAS_AccountSubject B 

                        on A.SubjectCode=B.Code and A.AccountId=B.AccountId
                        left join TKS_FAS_AccountSubject C on B.RootCode=C.Code and A.AccountId=C.AccountId
                        where {0} and A.source<>'JZ'

                         ) M  
                        ) N

                        ";
                    //if (!string.IsNullOrEmpty(request.allPZ))
                    //{
                    //    table = @"
                    //    (
                    //    select 
                    //          M.RootCode as Code
                    //       ,M.Name
                    //          ,sum(Money_Debit) as Money1
                    //         , sum(M.Money_Credit) as Money2
                    //      ,sum(case when M.Credit_Debit=0 then  M.Quantity else 0 end) as NUM1
                    //      ,sum(case when M.Credit_Debit=1 then M.Quantity else 0 end) as NUM2
                    //        from (

                    //    select A.SubjectCode,A.Money_Credit,A.Money_Debit,A.Quantity,A.Credit_Debit,B.RootCode ,C.showTitle as name

                    //    from TKS_FAS_DocDetail A left join TKS_FAS_AccountSubject B 

                    //    on A.SubjectCode=B.Code and A.AccountId=B.AccountId
                    //    left join TKS_FAS_AccountSubject C on B.RootCode=C.Code and A.AccountId=C.AccountId
                    //    where {0} 
                    //     ) M group by  m.RootCode,m.Name
                    //     union 

                    //     select 
                    //          '9' 
                    //       ,'合计'
                    //          ,sum(Money_Debit) as Money1
                    //         , sum(M.Money_Credit) as Money2
                    //      ,sum(case when M.Credit_Debit=0 then  M.Quantity else 0 end) as NUM1
                    //      ,sum(case when M.Credit_Debit=1 then M.Quantity else 0 end) as NUM2
                    //        from (

                    //    select A.SubjectCode,A.Money_Credit,A.Money_Debit,A.Quantity,A.Credit_Debit,B.RootCode ,C.showTitle as name

                    //    from TKS_FAS_DocDetail A left join TKS_FAS_AccountSubject B 

                    //    on A.SubjectCode=B.Code and A.AccountId=B.AccountId
                    //    left join TKS_FAS_AccountSubject C on B.RootCode=C.Code and A.AccountId=C.AccountId
                    //    where {0}

                    //     ) M  
                    //    ) N

                    //    ";
                    //}
                    #endregion

                    if (request.Type == "normal")
                    {
                        table = string.Format(table, "A.AccountId='" + user.AccountId + "' and A.PeriodId='" + request.PeriodId + "'");
                    }
                    else
                    {

                        #region 更多查询
                        //开始期间
                        var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                       @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                       new { Id = request.More.Period_S }, ts);
                        //结束期间
                        var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                         new { Id = request.More.Period_E }, ts);
                        if (periodS == null)
                        {
                            return new ResponseSummaryListSearch { IsSuccess = false, Message = "会计期间没有选择" };
                        }

                        if (periodS.EndDate > periodE.EndDate)
                        {
                            throw new NormalException("开始期间不能大于结束期间");
                        }
                        //获取期间集合
                        List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                            @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE ",
                            new
                            {
                                StartDateS = periodS.StartDate.ToString(),
                                StartDateE = periodE.StartDate.ToString(),
                                AccountId = user.AccountId
                            }, ts).ToList();

                        string periodWhere = string.Empty;//期间条件


                        int j = 0;
                        foreach (var period in periods)
                        {
                            if (j > 0)
                            {
                                periodWhere += " or ";
                            }

                            periodWhere += "  A.periodId='" + period.Id + "'";
                            j++;
                        }

                        string where = " A.AccountId='" + user.AccountId + "' and (" + periodWhere + ")";


                        if (!string.IsNullOrEmpty(request.More.Code_S))
                        {
                            where += " and A.SubjectCode>='" + request.More.Code_S + "'";
                        }

                        if (!string.IsNullOrEmpty(request.More.Code_E))
                        {
                            where += " and A.SubjectCode<='" + request.More.Code_E + "'";
                        }
                        table = string.Format(table, where);

                        #endregion
                    }

                    string sql = this.GetPageSql("*", table, "N.code",
                        request.PageSize,
                        request.PageIndex);

                    var data = cnn.Query<ZB_Summary>(sql,
                       null,
                        ts).ToList();
                    string countSql = @"select count(1)  from " + table;
                    int total = int.Parse(cnn.ExecuteScalar(countSql, null, ts).ToString());

                    ts.Commit();
                    res.PageIndex = request.PageIndex;
                    res.Data = data;
                    res.Total = total;
                    res.IsSuccess = true;
                    res.Message = "查询成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseSummaryListSearch;
                }
            }
        }

        /// <summary>
        /// 序时账
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseDocDetailAccountListSearch DocDetailListSearch(RequestSummaryListSearch request)
        {
            ResponseDocDetailAccountListSearch res = new ResponseDocDetailAccountListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);


                    #region 查询条件
                    //开始期间
                    var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                   @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                   new { Id = request.More.Period_S }, ts);
                    //结束期间
                    var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                     @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                     new { Id = request.More.Period_E }, ts);
                    if (periodS == null)
                    {
                        return new ResponseDocDetailAccountListSearch { IsSuccess = false, Message = "会计期间没有选择" };
                    }

                    if (periodS.EndDate > periodE.EndDate)
                    {
                        throw new NormalException("开始期间不能大于结束期间");
                    }
                    //获取期间集合
                    List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                        @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE ",
                        new
                        {
                            StartDateS = periodS.StartDate.ToString(),
                            StartDateE = periodE.StartDate.ToString(),
                            AccountId = user.AccountId
                        }, ts).ToList();

                    string periodWhere = string.Empty;//期间条件


                    int j = 0;
                    foreach (var period in periods)
                    {
                        if (j > 0)
                        {
                            periodWhere += " or ";
                        }

                        periodWhere += "  det.periodId='" + period.Id + "'";
                        j++;
                    }

                    string where = "  and (" + periodWhere + ")";


                    if (!string.IsNullOrEmpty(request.More.Code_S))
                    {
                        where += " and det.SubjectCode>='" + request.More.Code_S + "'";
                    }

                    if (!string.IsNullOrEmpty(request.More.Code_E))
                    {
                        where += " and det.SubjectCode<='" + request.More.Code_E + "'";
                    }


                    #endregion

                    string sql = string.Format(@"
        SELECT doc.PZDate, B.PZZ, doc.PZZNO
	        , CASE 
		        WHEN calDetail.Code IS NOT NULL THEN det.SubjectCode + '_' + calDetail.Code
		        ELSE det.SubjectCode
	        END AS SubjectCode
	        , CASE 
		        WHEN calDetail.Code IS NOT NULL THEN sub.Name + '_' + calDetail.Name
		        ELSE sub.Name
	        END AS Name, det.Summary
	        , CASE 
		        WHEN det.Credit_Debit = 0 THEN '借'
		        ELSE '贷'
	        END AS Credit_Debit
	        , CASE 
		        WHEN det.Credit_Debit = 0 THEN det.Money_Debit
		        ELSE det.Money_Credit
	        END AS amt
        FROM TKS_FAS_DocDetail det
	        LEFT JOIN TKS_FAS_Doc doc ON det.ParentId = doc.Id
	        LEFT JOIN TKS_FAS_CertificateWord B ON doc.PZZ = B.ID
	        LEFT JOIN TKS_FAS_AccountSubject sub
	        ON sub.AccountId = det.AccountId
		        AND sub.Code = det.SubjectCode
	        LEFT JOIN TKS_FAS_CaculateHelperDetail calDetail
	        ON calDetail.AccountId = det.AccountId
		        AND ((calDetail.ParentId = det.CalItem1
				        AND calDetail.Code = det.CalValue1)
			        OR (calDetail.ParentId = det.CalItem2
				        AND calDetail.Code = det.CalValue2)
			        OR (calDetail.ParentId = det.CalItem3
				        AND calDetail.Code = det.CalValue3)
			        OR (calDetail.ParentId = det.CalItem4
				        AND calDetail.Code = det.CalValue4)
			        OR (calDetail.ParentId = det.CalItem5
				        AND calDetail.Code = det.CalValue5))
        WHERE det.AccountId = @AccountId {0}
        ORDER BY doc.PZDate, B.PZZ, doc.PZZNO
", where);

                    List<DocDetailAccount> data = cnn.Query<DocDetailAccount>(sql,
                        new
                        {
                            user.AccountId

                        }, ts).ToList();

                    int total = data.Count;

                    ts.Commit();
                    res.PageIndex = request.PageIndex;
                    res.Data = data.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList(); ;
                    res.Total = total;
                    res.IsSuccess = true;
                    res.Message = "查询成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseDocDetailAccountListSearch;
                }
            }
        }
        /// <summary>
        /// 05 核算项目明细帐
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseCalcuAccountDetailListSearch CalcuAccountDetailListSearch(RequestCalcuAccountDetailListSearch request)
        {
            ResponseCalcuAccountDetailListSearch res = new ResponseCalcuAccountDetailListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    #region table
                    string table = @"(
 
select max(B.DetailDate) as DetailDate,'' as PZZ,B.summary,sum(B.num1) as num1,max(B.Unit_Price1) as Unit_Price1,
 sum(B.Money1) as Money1,sum(B.num2) as num2,max(B.Unit_Price2) as Unit_Price2,sum(b.Money2) as Money2,
 max(B.show_credit_debit) as show_credit_debit,
 sum(B.show_quantity) as show_quantity,sum(B.show_money) as show_money from 
 (
select B.StartDate as DetailDate,'' as PZZ,A.subjectCode +A.name as name,  '期初余额' as summary,

0 as num1,0 as Unit_Price1 ,0 as Money1,
0 as num2,0 as Unit_Price2 ,0 as Money2,

 A.SCredit_Debit as show_credit_debit,A.NUMStartBAL as Show_Quantity,A.BWBStartBAL as show_money
 
from 
TKS_FAS_FGLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id 

where {0}
and ( 
 
 (A.CalItem1=@CalItemId and A.CalValue1=@CalValue) or
 (A.CalItem2=@CalItemId and A.CalValue2=@CalValue) or
 (A.CalItem3=@CalItemId and A.CalValue3=@CalValue) or
 (A.CalItem4=@CalItemId and A.CalValue4=@CalValue) or
 (A.CalItem5=@CalItemId and A.CalValue5=@CalValue) 

 )
) B group by  B.summary
union


select max(A.DetailDate) as DetailDate,A.pzz,A.summary,sum(A.num1) as num1,max(A.Unit_Price1) as Unit_Price1,
 sum(A.Money1) as Money1,sum(A.num2) as num2,max(A.Unit_Price2) as Unit_Price2,sum(A.Money2) as Money2,
 max(A.show_credit_debit) as show_credit_debit,
 sum(A.show_quantity) as show_quantity,sum(A.show_money) as show_money from 
 (
 select  B.PZDate as DetailDate ,C.PZZ+cast(B.PZZNO as varchar) as pzz,A.SubjectDescription as name,
 A.Summary,
 
 A.Quantity as num1,0 as Unit_Price1 ,A.Money_Debit as Money1,
 A.Quantity as num2,0 as Unit_Price2 ,A.Money_Credit as Money2,
 0 as show_credit_debit,0as show_quantity,0 as show_money

 from 
 TKS_FAS_Doc B left join  TKS_FAS_DocDetail A on B.Id=A.ParentId
 left join TKS_FAS_CertificateWord C on B.PZZ=C.Id
 where   {0} and A.source<>'JZ'
 and ( 
 
 (A.CalItem1=@CalItemId and A.CalValue1=@CalValue) or
 (A.CalItem2=@CalItemId and A.CalValue2=@CalValue) or
 (A.CalItem3=@CalItemId and A.CalValue3=@CalValue) or
 (A.CalItem4=@CalItemId and A.CalValue4=@CalValue) or
 (A.CalItem5=@CalItemId and A.CalValue5=@CalValue) 

 )
   ) A group by  A.summary,A.pzz

  union

select max(B.DetailDate) as DetailDate,'' as pzz, B.summary,sum(B.num1) as num1,max(B.Unit_Price1) as Unit_Price1,
 sum(B.Money1) as Money1,sum(B.num2) as num2,max(B.Unit_Price2) as Unit_Price2,sum(b.Money2) as Money2,
 max(B.show_credit_debit) as show_credit_debit,
 sum(B.show_quantity) as show_quantity,sum(B.show_money) as show_money from 
 (
select B.EndDate as DetailDate,''as PZZ,A.subjectCode +A.name as name,'本期合计' as summary ,

A.NUMDebitTotal as num1,0 as Unit_Price1 ,A.BWBDebitTotal as Money1,
A.NUMCreditTotal as num2,0 as Unit_Price2 ,A.BWBCreditTotal as Money2,

A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money

 
from 
TKS_FAS_FGLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
where {0}
 and ( 
 
 (A.CalItem1=@CalItemId and A.CalValue1=@CalValue) or
 (A.CalItem2=@CalItemId and A.CalValue2=@CalValue) or
 (A.CalItem3=@CalItemId and A.CalValue3=@CalValue) or
 (A.CalItem4=@CalItemId and A.CalValue4=@CalValue) or
 (A.CalItem5=@CalItemId and A.CalValue5=@CalValue) 

 )

) B group by  B.summary
union 
select max(B.DetailDate) as DetailDate,''as pzz, B.summary,sum(B.num1) as num1,max(B.Unit_Price1) as Unit_Price1,
 sum(B.Money1) as Money1,sum(B.num2) as num2,max(B.Unit_Price2) as Unit_Price2,sum(b.Money2) as Money2,
 max(B.show_credit_debit) as show_credit_debit,
 sum(B.show_quantity) as show_quantity,sum(B.show_money) as show_money from 
(
select B.EndDate as DetailDate,'本年累计' as summary ,
 A.NUMDebitTotal_Y as num1,0 as Unit_Price1 ,A.BWBDebitTotal_Y as Money1,
A.NUMCreditTotal_Y as num2,0 as Unit_Price2 ,A.BWBCreditTotal_Y as Money2,

A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money
 
from 
TKS_FAS_FGLBalance A 
left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
 where {0}  and ( 
 
 (A.CalItem1=@CalItemId and A.CalValue1=@CalValue) or
 (A.CalItem2=@CalItemId and A.CalValue2=@CalValue) or
 (A.CalItem3=@CalItemId and A.CalValue3=@CalValue) or
 (A.CalItem4=@CalItemId and A.CalValue4=@CalValue) or
 (A.CalItem5=@CalItemId and A.CalValue5=@CalValue) 

 )
 )B group by  B.summary
 
 
) A";
                    //                    if (!string.IsNullOrEmpty(request.allPZ))
                    //                    {
                    //                        table = @"(

                    //select max(B.DetailDate) as DetailDate,'' as PZZ,B.summary,sum(B.num1) as num1,max(B.Unit_Price1) as Unit_Price1,
                    // sum(B.Money1) as Money1,sum(B.num2) as num2,max(B.Unit_Price2) as Unit_Price2,sum(b.Money2) as Money2,
                    // max(B.show_credit_debit) as show_credit_debit,
                    // sum(B.show_quantity) as show_quantity,sum(B.show_money) as show_money from 
                    // (
                    //select B.StartDate as DetailDate,'' as PZZ,A.subjectCode +A.name as name,  '期初余额' as summary,

                    //0 as num1,0 as Unit_Price1 ,0 as Money1,
                    //0 as num2,0 as Unit_Price2 ,0 as Money2,

                    // A.SCredit_Debit as show_credit_debit,A.NUMStartBAL as Show_Quantity,A.BWBStartBAL as show_money

                    //from 
                    //TKS_FAS_FGLBalance A 
                    //left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id 

                    //where {0}
                    //and ( 

                    // (A.CalItem1=@CalItemId and A.CalValue1=@CalValue) or
                    // (A.CalItem2=@CalItemId and A.CalValue2=@CalValue) or
                    // (A.CalItem3=@CalItemId and A.CalValue3=@CalValue) or
                    // (A.CalItem4=@CalItemId and A.CalValue4=@CalValue) or
                    // (A.CalItem5=@CalItemId and A.CalValue5=@CalValue) 

                    // )
                    //) B group by  B.summary
                    //union


                    //select max(A.DetailDate) as DetailDate,A.pzz,A.summary,sum(A.num1) as num1,max(A.Unit_Price1) as Unit_Price1,
                    // sum(A.Money1) as Money1,sum(A.num2) as num2,max(A.Unit_Price2) as Unit_Price2,sum(A.Money2) as Money2,
                    // max(A.show_credit_debit) as show_credit_debit,
                    // sum(A.show_quantity) as show_quantity,sum(A.show_money) as show_money from 
                    // (
                    // select  B.PZDate as DetailDate ,C.PZZ+cast(B.PZZNO as varchar) as pzz,A.SubjectDescription as name,
                    // A.Summary,

                    // A.Quantity as num1,0 as Unit_Price1 ,A.Money_Debit as Money1,
                    // A.Quantity as num2,0 as Unit_Price2 ,A.Money_Credit as Money2,
                    // 0 as show_credit_debit,0as show_quantity,0 as show_money

                    // from 
                    // TKS_FAS_Doc B left join  TKS_FAS_DocDetail A on B.Id=A.ParentId
                    // left join TKS_FAS_CertificateWord C on B.PZZ=C.Id
                    // where   {0}
                    // and ( 

                    // (A.CalItem1=@CalItemId and A.CalValue1=@CalValue) or
                    // (A.CalItem2=@CalItemId and A.CalValue2=@CalValue) or
                    // (A.CalItem3=@CalItemId and A.CalValue3=@CalValue) or
                    // (A.CalItem4=@CalItemId and A.CalValue4=@CalValue) or
                    // (A.CalItem5=@CalItemId and A.CalValue5=@CalValue) 

                    // )
                    //   ) A group by  A.summary,A.pzz

                    //  union

                    //select max(B.DetailDate) as DetailDate,'' as pzz, B.summary,sum(B.num1) as num1,max(B.Unit_Price1) as Unit_Price1,
                    // sum(B.Money1) as Money1,sum(B.num2) as num2,max(B.Unit_Price2) as Unit_Price2,sum(b.Money2) as Money2,
                    // max(B.show_credit_debit) as show_credit_debit,
                    // sum(B.show_quantity) as show_quantity,sum(B.show_money) as show_money from 
                    // (
                    //select B.EndDate as DetailDate,''as PZZ,A.subjectCode +A.name as name,'本期合计' as summary ,

                    //A.NUMDebitTotal as num1,0 as Unit_Price1 ,A.BWBDebitTotal as Money1,
                    //A.NUMCreditTotal as num2,0 as Unit_Price2 ,A.BWBCreditTotal as Money2,

                    //A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money


                    //from 
                    //TKS_FAS_FGLBalance A 
                    //left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
                    //where {0}
                    // and ( 

                    // (A.CalItem1=@CalItemId and A.CalValue1=@CalValue) or
                    // (A.CalItem2=@CalItemId and A.CalValue2=@CalValue) or
                    // (A.CalItem3=@CalItemId and A.CalValue3=@CalValue) or
                    // (A.CalItem4=@CalItemId and A.CalValue4=@CalValue) or
                    // (A.CalItem5=@CalItemId and A.CalValue5=@CalValue) 

                    // )

                    //) B group by  B.summary
                    //union 
                    //select max(B.DetailDate) as DetailDate,''as pzz, B.summary,sum(B.num1) as num1,max(B.Unit_Price1) as Unit_Price1,
                    // sum(B.Money1) as Money1,sum(B.num2) as num2,max(B.Unit_Price2) as Unit_Price2,sum(b.Money2) as Money2,
                    // max(B.show_credit_debit) as show_credit_debit,
                    // sum(B.show_quantity) as show_quantity,sum(B.show_money) as show_money from 
                    //(
                    //select B.EndDate as DetailDate,'本年累计' as summary ,
                    // A.NUMDebitTotal_Y as num1,0 as Unit_Price1 ,A.BWBDebitTotal_Y as Money1,
                    //A.NUMCreditTotal_Y as num2,0 as Unit_Price2 ,A.BWBCreditTotal_Y as Money2,

                    //A.ECredit_Debit as show_credit_debit,A.NUMEndBAL as show_quantity, A.BWBEndBAL as show_money

                    //from 
                    //TKS_FAS_FGLBalance A 
                    //left join TKS_FAS_MonthPeriodInfo B on A.PeriodId=B.id
                    // where {0}  and ( 

                    // (A.CalItem1=@CalItemId and A.CalValue1=@CalValue) or
                    // (A.CalItem2=@CalItemId and A.CalValue2=@CalValue) or
                    // (A.CalItem3=@CalItemId and A.CalValue3=@CalValue) or
                    // (A.CalItem4=@CalItemId and A.CalValue4=@CalValue) or
                    // (A.CalItem5=@CalItemId and A.CalValue5=@CalValue) 

                    // )
                    // )B group by  B.summary


                    //) A";
                    //                    }
                    #endregion
                    List<ZB_CalcuAccountDetail> data = new List<ZB_CalcuAccountDetail>();
                    int total = 0;
                    if (request.Type == "normal")
                    {
                        table = string.Format(table, "A.PeriodId='" + request.PeriodId + "'");
                        string sql = this.GetPageSql("A.*", table,
                   "   A.DetailDate,A.summary  desc",
                   request.PageSize, request.PageIndex);

                        data = cnn.Query<ZB_CalcuAccountDetail>(sql, new
                        {

                            CalItemId = request.CalculateItem,
                            CalValue = request.CalculateValue

                        }, ts).ToList();

                        string countSql = @"select count(1) from " + table;

                        total = int.Parse(cnn.ExecuteScalar(countSql, new
                        {


                            CalItemId = request.CalculateItem,
                            CalValue = request.CalculateValue

                        }, ts).ToString());
                    }
                    else
                    {
                        #region 更多查询
                        //开始期间
                        var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                                        @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                        new { Id = request.More.Period_S }, ts);
                        //结束期间
                        var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                         new { Id = request.More.Period_E }, ts);
                        if (periodS == null)
                        {
                            return new ResponseCalcuAccountDetailListSearch
                            { IsSuccess = false, Message = "会计期间没有选择" };
                        }
                        if (periodS.EndDate > periodE.EndDate)
                        {
                            throw new NormalException("开始期间不能大于结束期间");
                        }
                        //获取期间集合
                        List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                            @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE order by StartDate",
                            new
                            {
                                StartDateS = periodS.StartDate.ToString(),
                                StartDateE = periodE.StartDate.ToString(),
                                AccountId = user.AccountId
                            }, ts).ToList();


                        foreach (var period in periods)
                        {
                            var where = "  A.periodId='" + period.Id + "'";
                            var _sql = string.Format("select A.* from " + table + " order by A.DetailDate,A.summary  desc", where);

                            //string sql = this.GetPageSql("A.*", _sql,
                            //            "   A.DetailDate,A.summary  desc",
                            //                    request.PageSize, request.PageIndex);

                            var _data = cnn.Query<ZB_CalcuAccountDetail>(_sql, new
                            {

                                CalItemId = request.CalculateItem,
                                CalValue = request.CalculateValue

                            }, ts).ToList();

                            data.AddRange(_data);
                        }

                        total = 1;//随意赋值，全部查出来

                        #endregion

                    }



                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "加载完毕";
                    res.PageIndex = request.PageIndex;
                    res.Data = data;

                    res.Total = total;
                    return res;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseCalcuAccountDetailListSearch;
                }
            }
        }

        /// <summary>
        /// 获取辅助核算项类别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseCalculateItemGet CalculateItemGet(RequestCalculateItemGet request)
        {
            ResponseCalculateItemGet res = new ResponseCalculateItemGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);

                    string sql = "select id as code ,title as value from TKS_FAS_CaculateHelperItem where accountId=@AccountId";

                    var data = cnn.Query<CodeValue>(sql,
                        new
                        {
                            AccountId = user.AccountId
                        }
                        , ts).ToList();

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Data = data;
                    res.Message = "加载成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseCalculateItemGet;
                }
            }
        }

        /// <summary>
        /// 获取已经使用的辅助核算项值
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseCalculateValuesGet CalculateValuesGet(RequestCalculateValuesGet request)
        {
            ResponseCalculateValuesGet res = new ResponseCalculateValuesGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = @"select code ,value from (
                               select B.code as code ,B.code+' '+B.name as value 
                                from 
                                tks_fas_docDetail A left join TKS_FAS_CaculateHelperDetail B
                                on (A.calItem1=B.ParentId and A.calValue1=B.code) or 
                                    (A.calItem2=B.ParentId and A.calValue2=B.code) or
                                     (A.calItem3=B.ParentId and A.calValue3=B.code)  or
                                     (A.calItem4=B.ParentId and A.calValue4=B.code)  or
                                     (A.calItem5=B.ParentId and A.calValue5=B.code) 
								where A.PeriodId=@PeriodId and B.ParentId=@CalItemId
                             
								) M
								where code is not null
								group by code,value";


                    var data = cnn.Query<CodeValue>(sql,
                        new
                        {
                            PeriodId = request.PeriodId,
                            CalItemId = request.CalculateItemId
                        }, ts).ToList();

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Data = data;
                    res.Message = "";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseCalculateValuesGet;
                }
            }
        }

        /// <summary>
        /// 获取已使用的辅助核算项内容（期间区间内）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseCalculateValuesGetInPeriod CalculateValuesGetInPeriod(RequestCalculateValuesGetInPeriod request)
        {
            ResponseCalculateValuesGetInPeriod res = new ResponseCalculateValuesGetInPeriod();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = @"select code ,value from (
                               select B.code as code ,B.code+' '+B.name as value 
                                from 
                                tks_fas_docDetail A left join TKS_FAS_CaculateHelperDetail B
                                on (A.calItem1=B.ParentId and A.calValue1=B.code) or 
                                    (A.calItem2=B.ParentId and A.calValue2=B.code) or
                                     (A.calItem3=B.ParentId and A.calValue3=B.code)  or
                                     (A.calItem4=B.ParentId and A.calValue4=B.code)  or
                                     (A.calItem5=B.ParentId and A.calValue5=B.code) 
								where   B.ParentId=@CalItemId and {0}
                             
								) M
								where code is not null
								group by code,value";

                    //开始期间
                    var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                   @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                   new { Id = request.Period_S }, ts);
                    //结束期间
                    var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                     @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                     new { Id = request.Period_E }, ts);
                    if (periodS == null)
                    {
                        return new ResponseCalculateValuesGetInPeriod
                        { IsSuccess = false, Message = "会计期间没有选择" };
                    }
                    if (periodS.EndDate > periodE.EndDate)
                    {
                        throw new NormalException("开始期间不能大于结束期间");
                    }
                    //获取期间集合
                    List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                        @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE ",
                        new
                        {
                            StartDateS = periodS.StartDate.ToString(),
                            StartDateE = periodE.StartDate.ToString(),
                            AccountId = user.AccountId
                        }, ts).ToList();

                    string periodWhere = string.Empty;//期间条件


                    int j = 0;
                    foreach (var period in periods)
                    {
                        if (j > 0)
                        {
                            periodWhere += " or ";
                        }

                        periodWhere += "  A.periodId='" + period.Id + "'";
                        j++;
                    }
                    periodWhere = " (" + periodWhere + ") ";

                    sql = string.Format(sql, periodWhere);

                    var data = cnn.Query<CodeValue>(sql,
                        new
                        {
                            CalItemId = request.CalculateItemId
                        }, ts).Distinct().ToList();

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Data = data;
                    res.Message = "";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseCalculateValuesGetInPeriod;
                }
            }
        }

        /// <summary>
        /// 06 核算项目余额表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseCalBalListSearch CalBalListSearch(RequestCalBalListSearch request)
        {
            ResponseCalBalListSearch res = new ResponseCalBalListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    #region 核算项目余额表
                    string table = @" (
 select
 1 as no,
 M.code,
 (select name from TKS_FAS_CaculateHelperDetail where ParentId =@CalItemId and Code = M.Code) as name
 ,
 sum(M.NumStart_J) as NumStart_J,
 (case when sum(M.NumStart_J) = 0 then 0 else  sum(M.BWBStart_J) / sum(M.NumStart_J) end) as PriceStart_J,
 (case when  abs(sum(M.BWBStart_J))-abs(sum(M.BWBStart_D))>0 then sum(M.BWBStart_J)-sum(M.BWBStart_D) else 0 end) as BWBStart_J,

 
 sum(M.NumStart_D) as NumStart_D,
 (case when  abs(sum(M.BWBStart_J))-abs(sum(M.BWBStart_D))<0 then sum(M.BWBStart_D)-sum(M.BWBStart_J) else 0 end) as BWBStart_D,
 (case when sum(M.NumStart_D) = 0 then 0 else  sum(M.BWBStart_D) / sum(M.NumStart_D) end) as PriceStart_D,

  sum(Num_CJ) as Num_CJ,
(case when sum(Num_CJ) = 0 then 0 else  sum(BWB_CJ) / sum(Num_CJ) end) as Price_CJ,
 sum(BWB_CJ) as BWB_CJ,

 sum(M.NUM_CD) as NUM_CD,
 (case when sum(Num_CJ) = 0 then 0 else  sum(BWB_CJ) / sum(Num_CJ) end) as Price_CD,
 sum(M.BWB_CD) as BWB_CD,

 sum(Num_YJ) as Num_YJ,
  (case when sum(Num_YJ) = 0 then 0 else  sum(BWB_YJ) / sum(Num_YJ) end) as Price_YJ,
 sum(M.BWB_YJ) as BWB_YJ,

 sum(M.Num_YD) as Num_YD,
  (case when sum(Num_YD) = 0 then 0 else  sum(BWB_YD) / sum(Num_YD) end) as Price_YD,
 sum(M.BWB_YD) as BWB_YD,

 ( case when (sum(M.NumEnd_J)-sum(M.NumEnd_D))>0 then sum(M.NumEnd_J)-sum(M.NumEnd_D) else 0 end) as NumEnd_J,
   0 as PriceEnd_J,
(case when  abs(sum(M.BWBEnd_J))-abs(sum(M.BWBEnd_D))>0 then  sum(M.BWBEnd_J) - sum(M.BWBEnd_D)  else 0 end  ) as BWBEnd_J,

 ( case when (sum(M.NumEnd_J)-sum(M.NumEnd_D))<0 then sum(M.NumEnd_D)-sum(M.NumEnd_J) else 0 end) as NumEnd_D,
   0 as PriceEnd_D,
 (case when  abs(sum(M.BWBEnd_J))-abs(sum(M.BWBEnd_D))<0 then  sum(M.BWBEnd_D)- sum(M.BWBEnd_J) else 0 end  ) as BWBEnd_D

 from
 (
 select
 ( case when A.CalItem1 = @CalItemId then A.CalValue1
 when  A.CalItem2 = @CalItemId then A.CalValue2
 when A.CalItem3 = @CalItemId then A.CalValue3
  when A.CalItem4 = @CalItemId then A.CalValue4
  when  A.CalItem5 = @CalItemId then A.CalValue5 end) code,

 (case when SCredit_Debit = 0 then NUMStartBAL else 0 end) as NumStart_J,   --期初借方数量
   (case when SCredit_Debit = 0 then  A.BWBStartBAL else 0 end) as BWBStart_J, --期初本位币

      (case when SCredit_Debit = 1 then NUMStartBAL else 0 end) as NumStart_D,   --期初借方数量
     (case when SCredit_Debit = 1 then  A.BWBStartBAL else 0 end) as BWBStart_D, --期初本位币
       (case when SCredit_Debit = 1 then A.Unit else 0 end) as UnitStart_D, --单价

   A.NUMDebitTotal as Num_CJ ,--本期借方发生额数量


   A.BWBDebitTotal as BWB_CJ,  --本期借方发生额本位币
   A.NUMCreditTotal as NUM_CD,
   
   A.BWBCreditTotal as BWB_CD,--本期贷方发生额本位币

   A.NUMDebitTotal_Y as Num_YJ,--本年累计发生额数量


   A.BWBDebitTotal_Y as BWB_YJ,--本年累计发生额本位币

   A.NUMCreditTotal_Y Num_YD,--本年贷方累计发生额数量


   A.BWBCreditTotal_Y as BWB_YD,--本年贷方累计发生额本位币

   (case when ECredit_Debit = 0 then A.NUMEndBAL else 0 end) as NumEnd_J,   --期末借方数量
   (case when ECredit_Debit = 0 then  A.BWBEndBAL else 0 end) as BWBEnd_J, --期末本位币

      (case when ECredit_Debit = 1 then A.NUMEndBAL else 0 end) as NumEnd_D,   --期末借方数量
     (case when ECredit_Debit = 1 then  A.BWBEndBAL else 0 end) as BWBEnd_D --期末本位币


 from
 TKS_FAS_FGLBalance A
 where {0}
 and(

A.CalItem1 = @CalItemId  or
 A.CalItem2 = @CalItemId or
 A.CalItem3 = @CalItemId  or
 A.CalItem4 = @CalItemId  or
 A.CalItem5 = @CalItemId

 )
   ) M
    group by M.code


    union



 select
 2 as no,
  '',
 '合计' as name
 ,
 sum(M.NumStart_J) as NumStart_J,
 (case when sum(M.NumStart_J) = 0 then 0 else  sum(M.BWBStart_J) / sum(M.NumStart_J) end) as PriceStart_J,
 (case when  abs(sum(M.BWBStart_J))-abs(sum(M.BWBStart_D))>0 then sum(M.BWBStart_J)-sum(M.BWBStart_D) else 0 end) as BWBStart_J,

 sum(M.NumStart_D) as NumStart_D,
 (case when  abs(sum(M.BWBStart_J))-abs(sum(M.BWBStart_D))<0 then sum(M.BWBStart_D)-sum(M.BWBStart_J) else 0 end) as BWBStart_D,
 (case when sum(M.NumStart_D) = 0 then 0 else  sum(M.BWBStart_D) / sum(M.NumStart_D) end) as PriceStart_D,

  sum(Num_CJ) as Num_CJ,
(case when sum(Num_CJ) = 0 then 0 else  sum(BWB_CJ) / sum(Num_CJ) end) as Price_CJ,
 sum(BWB_CJ) as BWB_CJ,

 sum(M.NUM_CD) as NUM_CD,
 (case when sum(Num_CJ) = 0 then 0 else  sum(BWB_CJ) / sum(Num_CJ) end) as Price_CD,
 sum(M.BWB_CD) as BWB_CD,

 sum(Num_YJ) as Num_YJ,
  (case when sum(Num_YJ) = 0 then 0 else  sum(BWB_YJ) / sum(Num_YJ) end) as Price_YJ,
 sum(M.BWB_YJ) as BWB_YJ,

 sum(M.Num_YD) as Num_YD,
  (case when sum(Num_YD) = 0 then 0 else  sum(BWB_YD) / sum(Num_YD) end) as Price_YD,
 sum(M.BWB_YD) as BWB_YD,

  ( case when (sum(M.NumEnd_J)-sum(M.NumEnd_D))>0 then sum(M.NumEnd_J)-sum(M.NumEnd_D) else 0 end) as NumEnd_J,
   0 as PriceEnd_J,
(case when  abs(sum(M.BWBEnd_J))-abs(sum(M.BWBEnd_D))>0 then sum(M.BWBEnd_J)- sum(M.BWBEnd_D) else 0 end  ) as BWBEnd_J,

 ( case when  sum(M.NumEnd_J) - sum(M.NumEnd_D) <0 then sum(M.NumEnd_D)-sum(M.NumEnd_J) else 0 end) as NumEnd_D,
   0 as PriceEnd_D,
 (case when  abs(sum(M.BWBEnd_J))-abs(sum(M.BWBEnd_D))<0 then  sum(M.BWBEnd_D) -sum(M.BWBEnd_J) else 0 end  ) as BWBEnd_D


 from
 (
 select
 ( case when A.CalItem1 = @CalItemId then A.CalValue1
 when  A.CalItem2 = @CalItemId then A.CalValue2
 when A.CalItem3 = @CalItemId then A.CalValue3
  when A.CalItem4 = @CalItemId then A.CalValue4
  when  A.CalItem5 = @CalItemId then A.CalValue5 end) code,

 (case when SCredit_Debit = 0 then NUMStartBAL else 0 end) as NumStart_J,   --期初借方数量
   (case when SCredit_Debit = 0 then  A.BWBStartBAL else 0 end) as BWBStart_J, --期初本位币

      (case when SCredit_Debit = 1 then NUMStartBAL else 0 end) as NumStart_D,   --期初借方数量
     (case when SCredit_Debit = 1 then  A.BWBStartBAL else 0 end) as BWBStart_D, --期初本位币
       (case when SCredit_Debit = 1 then A.Unit else 0 end) as UnitStart_D, --单价

   A.NUMDebitTotal as Num_CJ ,--本期借方发生额数量


   A.BWBDebitTotal as BWB_CJ,  --本期借方发生额本位币
   A.NUMCreditTotal as NUM_CD,
   
   A.BWBCreditTotal as BWB_CD,--本期贷方发生额本位币

   A.NUMDebitTotal_Y as Num_YJ,--本年累计发生额数量


   A.BWBDebitTotal_Y as BWB_YJ,--本年累计发生额本位币

   A.NUMCreditTotal_Y Num_YD,--本年贷方累计发生额数量


   A.BWBCreditTotal_Y as BWB_YD,--本年贷方累计发生额本位币

   (case when ECredit_Debit = 0 then A.NUMEndBAL else 0 end) as NumEnd_J,   --期末借方数量
   (case when ECredit_Debit = 0 then  A.BWBEndBAL else 0 end) as BWBEnd_J, --期末本位币

      (case when ECredit_Debit = 1 then A.NUMEndBAL else 0 end) as NumEnd_D,   --期末借方数量
     (case when ECredit_Debit = 1 then  A.BWBEndBAL else 0 end) as BWBEnd_D --期末本位币


 from
 TKS_FAS_FGLBalance A
 where {0} 
 and(

A.CalItem1 = @CalItemId  or
 A.CalItem2 = @CalItemId or
 A.CalItem3 = @CalItemId  or
 A.CalItem4 = @CalItemId  or
 A.CalItem5 = @CalItemId

 )
   ) M
) K

";
                    #endregion
                    var table1 = string.Empty;
                    var table2 = string.Empty;
                    if (request.Type == "normal")
                    {
                        table1 = string.Format(table, "A.PeriodId ='" + request.PeriodId + "'");
                    }
                    else
                    {
                        //开始期间
                        var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                       @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                       new { Id = request.More.Period_S }, ts);
                        //结束期间
                        var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                         new { Id = request.More.Period_E }, ts);
                        if (periodS == null)
                        {
                            return new ResponseCalBalListSearch { IsSuccess = false, Message = "会计期间没有选择" };
                        }
                        if (periodS.EndDate > periodE.EndDate)
                        {
                            throw new NormalException("开始期间不能大于结束期间");
                        }
                        //获取期间集合
                        List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                            @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE ",
                            new
                            {
                                StartDateS = periodS.StartDate.ToString(),
                                StartDateE = periodE.StartDate.ToString(),
                                AccountId = user.AccountId
                            }, ts).ToList();

                        string periodWhere = string.Empty;//期间条件


                        int j = 0;
                        foreach (var period in periods)
                        {
                            if (j > 0)
                            {
                                periodWhere += " or ";
                            }


                            periodWhere += "  A.periodId='" + period.Id + "'";
                            j++;
                        }
                        table2 = string.Format(table, " (" + periodWhere + ") and IsFirstPeriodAdd=0 ");
                        periodWhere = " (" + periodWhere + ") ";
                        table1 = string.Format(table, periodWhere);

                    }


                    string sql = this.GetPageSql("*",
                                 table1, "K.no ",
                                request.PageSize, request.PageIndex);



                    var data = cnn.Query<ZB_CalBAL>(sql,
                        new { CalItemId = request.CalculateItem }, ts).ToList();

                    sql = @"select *  from " + table2;
                    var data1 = cnn.Query<ZB_CalBAL>(sql,
                      new { CalItemId = request.CalculateItem }, ts).ToList();
                    foreach (var item in data)
                    {
                        decimal cj = 0L;
                        decimal cd = 0L;
                        decimal yj = 0;
                        decimal yd = 0;

                        foreach (var first in data1)
                        {
                            if (item.Code == first.Code)
                            {
                                cj = first.BWB_CJ;
                                cd = first.BWB_CD;
                                yj = first.BWB_YJ;
                                yd = first.BWB_YD;
                                break;
                            }
                        }

                        item.BWB_CD = cd;
                        item.BWB_CJ = cj;
                        item.BWB_YD = yd;
                        item.BWB_YJ = yj;
                    }

                    string table3 = string.Empty;
                    if (request.Type == "normal")
                    {
                        table3 = string.Format(table, "A.PeriodId ='" + request.PeriodId + "'  and IsFirstPeriodAdd=1   ");
                    }
                    else
                    {
                        table3 = string.Format(table, "A.PeriodId ='" + request.More.Period_S + "' and A.CalValue1 in(" + string.Join(",", data.Select(p => "'" + p.Code + "'")) + ")  and IsFirstPeriodAdd=1 ");
                    }
                    sql = @"select *  from " + table3;
                    var data2 = cnn.Query<ZB_CalBAL>(sql,
                      new { CalItemId = request.CalculateItem }, ts).ToList();
                    foreach (var item in data)
                    {
                        decimal startJ = 0L;
                        decimal startD = 0L;
                        if (item.Code == "007")
                        {

                        }
                        foreach (var first in data2)
                        {
                            if (item.Code == first.Code)
                            {
                                startJ = first.BWBEnd_J;
                                startD = first.BWBEnd_D;
                                break;
                            }
                        }
                        item.BWBStart_D = startD;
                        item.BWBStart_J = startJ;
                        if (item.BWBStart_D - item.BWBStart_J > 0)
                        {
                            item.BWBStart_D = item.BWBStart_D - item.BWBStart_J;
                            item.BWBStart_J = 0;
                        }
                        else
                        {
                            item.BWBStart_J = item.BWBStart_J - item.BWBStart_D;
                            item.BWBStart_D = 0;
                        }

                        item.BWBEnd_D = item.BWBStart_D + item.BWB_CD;
                        item.BWBEnd_J = item.BWBStart_J + item.BWB_CJ;
                        if (item.BWBEnd_D - item.BWBEnd_J > 0)
                        {
                            item.BWBEnd_D = item.BWBEnd_D - item.BWBEnd_J;
                            item.BWBEnd_J = 0;
                        }
                        else
                        {
                            item.BWBEnd_J = item.BWBEnd_J - item.BWBEnd_D;
                            item.BWBEnd_D = 0;
                        }
                    }

                    string countSql = @"select count(1)  from " + table1;
                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        CalItemId = request.CalculateItem

                    }, ts).ToString());

                    res.PageIndex = request.PageIndex;
                    res.Data = data;
                    res.Total = total;
                    res.IsSuccess = true;
                    res.Message = "查询成功";
                    return res;


                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseCalBalListSearch;
                }

            }
        }
    }
}