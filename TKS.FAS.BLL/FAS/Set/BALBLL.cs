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
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HPSF;

namespace TKS.FAS.BLL.FAS
{
    public class BALBLL : CommonBase
    {
        public BALBLL() { }
        public BALBLL(IDbConnection _cnn)
        {
            cnn = _cnn;
        }

        /// <summary>
        /// 期初数据加载
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBALListSearch BALListSearch(RequestBALListSearch request)
        {

            ResponseBALListSearch response = new ResponseBALListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    AccountSetBLL bll = new AccountSetBLL(cnn);
                    var period = bll.GetFirstPeriod(user.AccountId, ts);
                    var flag = bll.IsFirstPeriodPay(user.AccountId, ts);
                    string sqlQC = @"SELECT TT.SLevel,TT.RootCode,TT.ParentCode, T.*
                                    FROM (
	                                    SELECT M.*
	                                    FROM (
		                                    SELECT A.id, a.AccountId, A.subjectId, a.Category, a.PeriodId,a.ParentId
			                                    , a.SubjectCode, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal
			                                    , a.NUMDebitTotal_Y, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y
			                                    , a.NUMCreditTotal, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal
			                                    , a.YBCreditTotal_Y, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL
			                                    , a.YearStartBWBBAL, a.YearStartYBBAL, a.isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid
			                                    , a.isLeaf, 1 AS Type,'' AS CalValue1,'' AS CalItem1
		                                    FROM TKS_FAS_GLBalance  A
		                                    UNION
		                                    SELECT A.id, a.AccountId, '' AS subjectId, a.Category, a.PeriodId,a.ParentId
			                                    , a.SubjectCode, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal
			                                    , a.NUMDebitTotal_Y, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y
			                                    , a.NUMCreditTotal, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal
			                                    , a.YBCreditTotal_Y, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL
			                                    , a.YearStartBWBBAL, a.YearStartYBBAL, 0 AS isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid
			                                    , 1 AS isLeaf, 2 AS Type,a.CalValue1,a.CalItem1
		                                    FROM TKS_FAS_FGLBalance A
		                                    WHERE IsFirstPeriodAdd = 1
	                                    ) M
	                                    WHERE M.accountId = @AccountId
		                                    AND m.category = @Category
		                                    AND m.CurrencyCode = @CurrencyCode
		                                    AND m.PeriodId = ''
                                    ) T
	                                    LEFT JOIN (
		                                    SELECT c.SLevel,c.Code, c.RootCode,p.Code ParentCode
		                                    FROM TKS_FAS_AccountSubject c left outer join TKS_FAS_AccountSubject p on c.ParentId = p.id
		                                    WHERE c.category = @Category  AND c.accountId = @AccountId
	                                    ) TT
	                                    ON TT.Code = T.SubjectCode

	                                    ORDER BY TT.RootCode,T.SubjectCode,T.Name";
                    //string sqlQC = @"SELECT TT.SLevel,TT.RootCode, T.id, T.AccountId, T.subjectId, T.Category
                    //         , T.PeriodId, T.SubjectCode, T.Name, T.CurrencyCode, T.SCredit_Debit
                    //         , ISNULL(TTT.NUMDebitTotal, T.NUMDebitTotal) AS NUMDebitTotal
                    //         , ISNULL(TTT.NUMDebitTotal_Y, T.NUMDebitTotal_Y) AS NUMDebitTotal_Y
                    //         , ISNULL(TTT.BWBDebitTotal, T.BWBDebitTotal) AS BWBDebitTotal
                    //         , ISNULL(TTT.BWBDebitTotal_Y, T.BWBDebitTotal_Y) AS BWBDebitTotal_Y
                    //         , ISNULL(TTT.YBDebitTotal, T.YBDebitTotal) AS YBDebitTotal
                    //         , ISNULL(TTT.YBDebitTotal_Y, T.YBDebitTotal_Y) AS YBDebitTotal_Y
                    //         , ISNULL(TTT.NUMCreditTotal, T.NUMCreditTotal) AS NUMCreditTotal
                    //         , ISNULL(TTT.NUMCreditTotal_Y, T.NUMCreditTotal_Y) AS NUMCreditTotal_Y
                    //         , ISNULL(TTT.BWBCreditTotal, T.BWBCreditTotal) AS BWBCreditTotal
                    //         , ISNULL(TTT.BWBCreditTotal_Y, T.BWBCreditTotal_Y) AS BWBCreditTotal_Y
                    //         , ISNULL(TTT.YBCreditTotal, T.YBCreditTotal) AS YBCreditTotal
                    //         , ISNULL(TTT.YBCreditTotal_Y, T.YBCreditTotal_Y) AS YBCreditTotal_Y
                    //         , ISNULL(TTT.NUMStartBAL, T.NUMStartBAL) AS NUMStartBAL
                    //         , ISNULL(TTT.BWBStartBAL, T.BWBStartBAL) AS BWBStartBAL
                    //         , ISNULL(TTT.YBStartBAL, T.YBStartBAL) AS YBStartBAL
                    //         , ISNULL(TTT.YearStartNumBAL, T.YearStartNumBAL) AS YearStartNumBAL
                    //         , ISNULL(TTT.YearStartBWBBAL, T.YearStartBWBBAL) AS YearStartBWBBAL
                    //         , ISNULL(TTT.YearStartYBBAL, T.YearStartYBBAL) AS YearStartYBBAL, T.isCalHelperValid
                    //         , T.isDefaultCurrency, T.isQuantityValid, T.isLeaf, T.Type
                    //        FROM (
                    //         SELECT M.*
                    //         FROM (
                    //          SELECT A.id, a.AccountId, A.subjectId, a.Category, a.PeriodId
                    //           , a.SubjectCode, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal
                    //           , a.NUMDebitTotal_Y, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y
                    //           , a.NUMCreditTotal, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal
                    //           , a.YBCreditTotal_Y, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL
                    //           , a.YearStartBWBBAL, a.YearStartYBBAL, a.isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid
                    //           , a.isLeaf, 1 AS Type
                    //          FROM TKS_FAS_GLBalance A
                    //          UNION
                    //          SELECT A.id, a.AccountId, '' AS subjectId, a.Category, a.PeriodId
                    //           , a.SubjectCode, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal
                    //           , a.NUMDebitTotal_Y, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y
                    //           , a.NUMCreditTotal, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal
                    //           , a.YBCreditTotal_Y, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL
                    //           , a.YearStartBWBBAL, a.YearStartYBBAL, 0 AS isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid
                    //           , 1 AS isLeaf, 2 AS Type
                    //          FROM TKS_FAS_FGLBalance A
                    //          WHERE IsFirstPeriodAdd = 1
                    //         ) M
                    //         WHERE M.accountId = @AccountId
                    //          AND m.category = @Category
                    //          AND m.CurrencyCode = @CurrencyCode
                    //          AND m.PeriodId = ''
                    //        ) T
                    //         LEFT JOIN (
                    //          SELECT SLevel,Code, RootCode
                    //          FROM TKS_FAS_AccountSubject
                    //          WHERE category = @Category
                    //           AND accountId = @AccountId
                    //         ) TT
                    //         ON TT.Code = T.SubjectCode
                    //         LEFT JOIN (
                    //          SELECT TT.RootCode, T.CurrencyCode, T.SCredit_Debit, SUM(T.NUMDebitTotal) AS NUMDebitTotal
                    //           , SUM(T.NUMDebitTotal_Y) AS NUMDebitTotal_Y, SUM(T.BWBDebitTotal) AS BWBDebitTotal
                    //           , SUM(T.BWBDebitTotal_Y) AS BWBDebitTotal_Y, SUM(T.YBDebitTotal) AS YBDebitTotal
                    //           , SUM(T.YBDebitTotal_Y) AS YBDebitTotal_Y, SUM(T.NUMCreditTotal) AS NUMCreditTotal
                    //           , SUM(T.NUMCreditTotal_Y) AS NUMCreditTotal_Y, SUM(T.BWBCreditTotal) AS BWBCreditTotal
                    //           , SUM(T.BWBCreditTotal_Y) AS BWBCreditTotal_Y, SUM(T.YBCreditTotal) AS YBCreditTotal
                    //           , SUM(T.YBCreditTotal_Y) AS YBCreditTotal_Y, SUM(T.NUMStartBAL) AS NUMStartBAL
                    //           , SUM(T.BWBStartBAL) AS BWBStartBAL, SUM(T.YBStartBAL) AS YBStartBAL
                    //           , SUM(T.YearStartNumBAL) AS YearStartNumBAL, SUM(T.YearStartBWBBAL) AS YearStartBWBBAL
                    //           , SUM(T.YearStartYBBAL) AS YearStartYBBAL
                    //          FROM (
                    //           SELECT M.*
                    //           FROM (
                    //            SELECT A.id, a.AccountId, A.subjectId, a.Category, a.PeriodId
                    //             , a.SubjectCode, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal
                    //             , a.NUMDebitTotal_Y, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y
                    //             , a.NUMCreditTotal, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal
                    //             , a.YBCreditTotal_Y, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL
                    //             , a.YearStartBWBBAL, a.YearStartYBBAL, a.isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid
                    //             , a.isLeaf, 1 AS Type
                    //            FROM TKS_FAS_GLBalance A
                    //            UNION
                    //            SELECT A.id, a.AccountId, '' AS subjectId, a.Category, a.PeriodId
                    //             , a.SubjectCode, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal
                    //             , a.NUMDebitTotal_Y, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y
                    //             , a.NUMCreditTotal, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal
                    //             , a.YBCreditTotal_Y, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL
                    //             , a.YearStartBWBBAL, a.YearStartYBBAL, 0 AS isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid
                    //             , 1 AS isLeaf, 2 AS Type
                    //            FROM TKS_FAS_FGLBalance A
                    //            WHERE IsFirstPeriodAdd = 1
                    //           ) M
                    //           WHERE M.accountId = @AccountId
                    //            AND m.category = @Category
                    //            AND m.CurrencyCode = @CurrencyCode
                    //            AND m.PeriodId = ''
                    //          ) T
                    //           LEFT JOIN (
                    //            SELECT Code, RootCode
                    //            FROM TKS_FAS_AccountSubject
                    //            WHERE category = @Category
                    //             AND accountId = @AccountId
                    //           ) TT
                    //           ON TT.Code = T.SubjectCode
                    //          WHERE T.SubjectCode != TT.RootCode
                    //          GROUP BY TT.RootCode, T.CurrencyCode, T.SCredit_Debit
                    //         ) TTT
                    //         ON T.SubjectCode = TTT.RootCode
                    //        ORDER BY T.SubjectCode, T.Name";
                    List<TKS_FAS_GLBalanceExt> data = cnn.Query<TKS_FAS_GLBalanceExt>(sqlQC,
                       new
                       {
                           AccountId = user.AccountId,//账套
                           Category = request.Category,//科目类别
                           CurrencyCode = request.CurrencyCode//币别
                       }, ts).OrderByDescending(p => p.SLevel).ToList();
                    //data.Sum(p=>p.ECredit_Debit=1)
                    List<TKS_FAS_GLBalanceExt> lst = new List<TKS_FAS_GLBalanceExt>();
                    foreach (var item in data)
                    {
#if DEBUG
                        if (item.SubjectCode == "1131")
                        {

                        }
#endif
                        if (item.Type == 1)
                        {
                            if (data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Count() == 0)
                            {
                                //非辅助核算科目
                                item.NUMDebitTotal += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMDebitTotal : -1 * p.NUMDebitTotal);
                                item.NUMDebitTotal_Y += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMDebitTotal_Y : -1 * p.NUMDebitTotal_Y);
                                item.BWBDebitTotal += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBDebitTotal : -1 * p.BWBDebitTotal);
                                item.BWBDebitTotal_Y += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBDebitTotal_Y : -1 * p.BWBDebitTotal_Y);
                                item.YBDebitTotal += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBDebitTotal : -1 * p.YBDebitTotal);
                                item.YBDebitTotal_Y += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBDebitTotal_Y : -1 * p.YBDebitTotal_Y);
                                item.NUMCreditTotal += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMCreditTotal : -1 * p.NUMCreditTotal);
                                item.NUMCreditTotal_Y += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMCreditTotal_Y : -1 * p.NUMCreditTotal_Y);
                                item.BWBCreditTotal += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBCreditTotal : -1 * p.BWBCreditTotal);
                                item.BWBCreditTotal_Y += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBCreditTotal_Y : -1 * p.BWBCreditTotal_Y);
                                item.YBCreditTotal += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBCreditTotal : -1 * p.YBCreditTotal);
                                item.YBCreditTotal_Y += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBCreditTotal_Y : -1 * p.YBCreditTotal_Y);
                                item.NUMStartBAL += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMStartBAL : -1 * p.NUMStartBAL);
                                item.BWBStartBAL += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBStartBAL : -1 * p.BWBStartBAL);
                                item.YBStartBAL += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBStartBAL : -1 * p.YBStartBAL);
                                item.YearStartNumBAL += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YearStartNumBAL : -1 * p.YearStartNumBAL);
                                item.YearStartBWBBAL += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YearStartBWBBAL : -1 * p.YearStartBWBBAL);
                                item.YearStartYBBAL += data.Where(p => p.ParentCode == item.SubjectCode && p.Type != 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YearStartYBBAL : -1 * p.YearStartYBBAL);
                            }
                            else
                            {
                                item.NUMDebitTotal = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMDebitTotal : -1 * p.NUMDebitTotal);
                                item.NUMDebitTotal_Y = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMDebitTotal_Y : -1 * p.NUMDebitTotal_Y);
                                item.BWBDebitTotal = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBDebitTotal : -1 * p.BWBDebitTotal);
                                item.BWBDebitTotal_Y = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBDebitTotal_Y : -1 * p.BWBDebitTotal_Y);
                                item.YBDebitTotal = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBDebitTotal : -1 * p.YBDebitTotal);
                                item.YBDebitTotal_Y = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBDebitTotal_Y : -1 * p.YBDebitTotal_Y);
                                item.NUMCreditTotal = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMCreditTotal : -1 * p.NUMCreditTotal);
                                item.NUMCreditTotal_Y = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMCreditTotal_Y : -1 * p.NUMCreditTotal_Y);
                                item.BWBCreditTotal = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBCreditTotal : -1 * p.BWBCreditTotal);
                                item.BWBCreditTotal_Y = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBCreditTotal_Y : -1 * p.BWBCreditTotal_Y);
                                item.YBCreditTotal = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBCreditTotal : -1 * p.YBCreditTotal);
                                item.YBCreditTotal_Y = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBCreditTotal_Y : -1 * p.YBCreditTotal_Y);
                                item.NUMStartBAL = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.NUMStartBAL : -1 * p.NUMStartBAL);
                                item.BWBStartBAL = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBStartBAL : -1 * p.BWBStartBAL);
                                item.YBStartBAL = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YBStartBAL : -1 * p.YBStartBAL);
                                item.YearStartNumBAL = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YearStartNumBAL : -1 * p.YearStartNumBAL);
                                item.YearStartBWBBAL = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YearStartBWBBAL : -1 * p.YearStartBWBBAL);
                                item.YearStartYBBAL = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.YearStartYBBAL : -1 * p.YearStartYBBAL);

                                item.BWBStartBAL = data.Where(p => p.SubjectCode == item.SubjectCode && p.Type == 2).Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBStartBAL : -1 * p.BWBStartBAL);
                            }
                        }
                        data = data.OrderBy(p => p.RootCode).ThenBy(p => p.SubjectCode).ThenBy(p => p.Name).ToList();
                        //TKS_FAS_GLBalanceExt ext = new TKS_FAS_GLBalanceExt();
                        //ext = item;
                        //ext.NUMDebitTotal = SumValue(data, "NUMDebitTotal", item);
                        //ext.NUMDebitTotal_Y = SumValue(data, "NUMDebitTotal_Y", item);
                        //ext.BWBDebitTotal = SumValue(data, "BWBDebitTotal", item);
                        //ext.BWBDebitTotal_Y = SumValue(data, "BWBDebitTotal_Y", item);
                        //ext.YBDebitTotal = SumValue(data, "YBDebitTotal", item);
                        //ext.YBDebitTotal_Y = SumValue(data, "YBDebitTotal_Y", item);
                        //ext.NUMCreditTotal = SumValue(data, "NUMCreditTotal", item);
                        //ext.NUMCreditTotal_Y = SumValue(data, "NUMCreditTotal_Y", item);
                        //ext.BWBCreditTotal = SumValue(data, "BWBCreditTotal", item);
                        //ext.BWBCreditTotal_Y = SumValue(data, "BWBCreditTotal_Y", item);
                        //ext.YBCreditTotal = SumValue(data, "YBCreditTotal", item);
                        //ext.YBCreditTotal_Y = SumValue(data, "YBCreditTotal_Y", item);
                        //ext.NUMStartBAL = SumValue(data, "NUMStartBAL", item);
                        //ext.BWBStartBAL = SumValue(data, "BWBStartBAL", item);
                        //Info f = SumValue(data, "BWBStartBAL", item);
                        //if (f.SCredit_Debit != -1)
                        //{
                        //    ext.SCredit_Debit = f.SCredit_Debit;
                        //}
                        //ext.BWBStartBAL = f.Value;
                        //ext.YBStartBAL = SumValue(data, "YBStartBAL", item);
                        //ext.YearStartNumBAL = SumValue(data, "YearStartNumBAL", item);
                        //ext.YearStartBWBBAL = SumValue(data, "YearStartBWBBAL", item);
                        //ext.YearStartYBBAL = SumValue(data, "YearStartYBBAL", item);
                        //lst.Add(ext);
                    }
                    var resPH = CalculatePH(new RequestBALCalculate { Token = request.Token }, ts);

                    response.startBWB1 = resPH.startBWB1;
                    response.startBWB2 = resPH.startBWB2;
                    response.startBWB3 = resPH.startBWB3;
                    response.startBWB4 = resPH.startBWB4;
                    response.startBWB5 = resPH.startBWB5;

                    response.deLJBWB1 = resPH.deLJBWB1;
                    response.deLJBWB2 = resPH.deLJBWB2;
                    response.deLJBWB3 = resPH.deLJBWB3;
                    response.deLJBWB4 = resPH.deLJBWB4;
                    response.deLJBWB5 = resPH.deLJBWB5;

                    response.crLJBWB1 = resPH.crLJBWB1;
                    response.crLJBWB2 = resPH.crLJBWB2;
                    response.crLJBWB3 = resPH.crLJBWB3;
                    response.crLJBWB4 = resPH.crLJBWB4;
                    response.crLJBWB5 = resPH.crLJBWB5;


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    response.Data = data;
                    response.IsFirstPeriodPaid = flag;
                    response.Total = 9999;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseBALListSearch;
                }
            }
        }
        //public decimal SumValue(List<TKS_FAS_GLBalanceExt> data, string column, TKS_FAS_GLBalanceExt item)
        //{
        //    decimal val = 0;
        //    //Info f = new Info();
        //    //f.SCredit_Debit = -1;
        //    lstExt.Clear();
        //    List<TKS_FAS_GLBalanceExt> lst = new List<TKS_FAS_GLBalanceExt>();

        //    //if (item.IsCalHelperValid == 0)
        //    if (item.Type == 1 && item.IsCalHelperValid == 0)
        //    {
        //        //非辅助核算
        //        if (item.RootCode == item.SubjectCode)
        //        {
        //            //最上级科目节点，获取科目下最小节点集合
        //            //List<TKS_FAS_GLBalanceExt> tmp = data.Where(p => p.RootCode == item.RootCode && p.RootCode != p.SubjectCode && p.IsCalHelperValid == 0).ToList();
        //            List<TKS_FAS_GLBalanceExt> tmp = GetBalance(data.Where(p => p.IsCalHelperValid == 0).ToList(), item, item.SubjectId);
        //            if (tmp.Count == 0)
        //            {
        //                //没有下级科目
        //                lst.Add(item);
        //            }
        //            else
        //            {

        //                //下级科目中有辅助核算
        //                foreach (var t in tmp)
        //                {
        //                    //辅助核算
        //                    List<TKS_FAS_GLBalanceExt> cal_tmp = GetFBalance(data.Where(p => p.IsCalHelperValid == 0).ToList(), t, t.Id);
        //                    if (cal_tmp.Count == 0)
        //                    {

        //                        lst.Add(t);
        //                    }
        //                    else
        //                    {

        //                        lst.AddRange(cal_tmp);
        //                    }
        //                }
        //                //lst = tmp;
        //            }

        //        }
        //        else
        //        {
        //            //非最上级科目，判断该科目下是否有子科目行
        //            List<TKS_FAS_GLBalanceExt> tmp = GetBalance(data.Where(p => p.IsCalHelperValid == 0).ToList(), item, item.SubjectId);
        //            if (tmp.Count == 0)
        //            {
        //                //没有下级科目
        //                lst.Add(item);
        //            }
        //            else
        //            {

        //                //下级科目中有辅助核算
        //                foreach (var t in tmp)
        //                {
        //                    //辅助核算
        //                    List<TKS_FAS_GLBalanceExt> cal_tmp = GetFBalance(data.Where(p => p.IsCalHelperValid == 0).ToList(), t, t.Id);
        //                    if (cal_tmp.Count == 0)
        //                    {

        //                        lst.Add(t);
        //                    }
        //                    else
        //                    {

        //                        lst.AddRange(cal_tmp);
        //                    }
        //                }
        //                //lst = tmp;
        //            }
        //        }

        //    }
        //    else
        //    {
        //        //辅助核算
        //        List<TKS_FAS_GLBalanceExt> tmp = GetFBalance(data.Where(p => p.IsCalHelperValid == 0).ToList(), item, item.Id);
        //        if (tmp.Count == 0)
        //        {

        //            lst.Add(item);
        //        }
        //        else
        //        {
        //            lst = tmp;
        //        }
        //        //lst.Add(item);
        //    }
        //    if (lst != null && lst.Count > 0)
        //    {


        //        switch (column)
        //        {
        //            case "NUMDebitTotal":
        //                val = lst.Sum(p => p.NUMDebitTotal);
        //                break;
        //            case "NUMDebitTotal_Y":
        //                val = lst.Sum(p => p.NUMDebitTotal_Y);
        //                break;
        //            case "BWBDebitTotal":
        //                val = lst.Sum(p => p.BWBDebitTotal);
        //                break;
        //            case "BWBDebitTotal_Y":
        //                val = lst.Sum(p => p.BWBDebitTotal_Y);
        //                break;
        //            case "YBDebitTotal":
        //                val = lst.Sum(p => p.YBDebitTotal);
        //                break;
        //            case "YBDebitTotal_Y":
        //                val = lst.Sum(p => p.YBDebitTotal_Y);
        //                break;
        //            case "NUMCreditTotal":
        //                val = lst.Sum(p => p.NUMCreditTotal);
        //                break;
        //            case "NUMCreditTotal_Y":
        //                val = lst.Sum(p => p.NUMCreditTotal_Y);
        //                break;
        //            case "BWBCreditTotal":
        //                val = lst.Sum(p => p.BWBCreditTotal);
        //                break;
        //            case "BWBCreditTotal_Y":
        //                val = lst.Sum(p => p.BWBCreditTotal_Y);
        //                break;
        //            case "YBCreditTotal":
        //                val = lst.Sum(p => p.YBCreditTotal);
        //                break;
        //            case "YBCreditTotal_Y":
        //                val = lst.Sum(p => p.YBCreditTotal_Y);
        //                break;
        //            case "NUMStartBAL":
        //                val = lst.Sum(p => p.NUMStartBAL);
        //                break;
        //            case "BWBStartBAL":

        //                //decimal jie_Money = lst.Where(s => s.SCredit_Debit == 0).Sum(p => p.BWBStartBAL);
        //                //decimal dai_Money = lst.Where(s => s.SCredit_Debit == 1).Sum(p => p.BWBStartBAL);
        //                //if (jie_Money > dai_Money)
        //                //{
        //                //    f.SCredit_Debit = 0;
        //                //    val = jie_Money - dai_Money;
        //                //}
        //                //else if (jie_Money < dai_Money)
        //                //{
        //                //    f.SCredit_Debit = 1;
        //                //    val = dai_Money - jie_Money;
        //                //}
        //                val = lst.Sum(p => p.SCredit_Debit == item.SCredit_Debit ? p.BWBStartBAL : -1 * p.BWBStartBAL);
        //                break;
        //            case "YBStartBAL":
        //                val = lst.Sum(p => p.YBStartBAL);
        //                break;
        //            case "YearStartNumBAL":
        //                val = lst.Sum(p => p.YearStartNumBAL);
        //                break;
        //            case "YearStartBWBBAL":
        //                val = lst.Sum(p => p.YearStartBWBBAL);
        //                break;
        //            case "YearStartYBBAL":
        //                val = lst.Sum(p => p.YearStartYBBAL);
        //                break;
        //        }
        //    }
        //    //f.Value = val;
        //    //return f;
        //    return val;
        //}
        List<TKS_FAS_GLBalanceExt> lstExt = new List<TKS_FAS_GLBalanceExt>();
        public List<TKS_FAS_GLBalanceExt> GetBalance(List<TKS_FAS_GLBalanceExt> data, TKS_FAS_GLBalanceExt ext, string SubjectId)
        {


            List<TKS_FAS_GLBalanceExt> lst = data.Where(p => p.ParentId == SubjectId).ToList();
            if (lst.Count > 0)
            {
                foreach (var item in lst)
                {
                    GetBalance(data, ext, item.SubjectId);
                }
            }
            else
            {
                lstExt.AddRange(data.Where(p => p.SubjectId == SubjectId).ToList());
            }

            return lstExt;
        }

        public List<TKS_FAS_GLBalanceExt> GetFBalance(List<TKS_FAS_GLBalanceExt> data, TKS_FAS_GLBalanceExt ext, string id)
        {


            List<TKS_FAS_GLBalanceExt> lst = data.Where(p => p.ParentId == id).ToList();
            if (lst.Count > 0)
            {
                //foreach (var item in lst)
                //{
                //    GetBalance(data, ext, item.SubjectId);
                //}
                lstExt.AddRange(data.Where(p => p.ParentId == id).ToList());
            }
            //else
            //{
            //    lstExt.AddRange(data.Where(p => p.SubjectId == SubjectId).ToList());
            //}

            return lstExt;
        }
        public List<TKS_FAS_GLBalanceExt> LsBALGet(string token)
        {

            using (this.cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(token, ts);
                    AccountSetBLL bll = new AccountSetBLL(cnn);
                    var firstPeriod = bll.GetFirstPeriod(user.AccountId, ts);

                    //                    string sql = @"select * from ( select A.id,a.AccountId,A.subjectId,a.Category,a.PeriodId,
                    //a.SubjectCode,a.Name,a.CurrencyCode,(case when a.SCredit_Debit=0 then '借' else '贷' end) cd,
                    //a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
                    //,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
                    //a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
                    //a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
                    //a.isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,a.isLeaf, 1 as Type

                    //  from TKS_FAS_GLBalance A
                    //  union

                    //  select A.id,a.AccountId,'' as subjectId,a.Category,a.PeriodId,a.SubjectCode,a.Name,a.CurrencyCode,
                    //(case when a.SCredit_Debit=0 then '借' else '贷' end) cd,
                    //a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
                    //,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
                    //a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
                    //a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
                    //0 as  isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,1 as isLeaf, 2 as Type

                    //  from TKS_FAS_FGLBalance A  where IsFirstPeriodAdd=1 )M  where M.accountId=@AccountId 

                    //                            and m.CurrencyCode='RMB' and m.PeriodId=@PeriodId
                    //                        order by M.SubjectCode ";
                    string sql = @"select * from ( select A.id,a.AccountId,A.subjectId,a.Category,a.PeriodId,
a.SubjectCode,a.Name,a.CurrencyCode,(case when a.SCredit_Debit=0 then '借' else '贷' end) cd,
a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
a.isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,a.isLeaf, 1 as Type, '否' as IsHelper
,'' as calCode,'' as calName
  from TKS_FAS_GLBalance A
  union

  select A.id,a.AccountId,'' as subjectId,a.Category,a.PeriodId,a.SubjectCode,a.Name,a.CurrencyCode,
(case when a.SCredit_Debit=0 then '借' else '贷' end) cd,
a.NUMDebitTotal,a.NUMDebitTotal_Y,a.BWBDebitTotal,a.BWBDebitTotal_Y,a.YBDebitTotal,a.YBDebitTotal_Y
,a.NUMCreditTotal,a.NUMCreditTotal_Y,a.BWBCreditTotal,a.BWBCreditTotal_Y,a.YBCreditTotal,a.YBCreditTotal_Y,
a.NUMStartBAL,a.BWBStartBAL,a.YBStartBAL,
a.YearStartNumBAL,a.YearStartBWBBAL,a.YearStartYBBAL,
0 as  isCalHelperValid,a.isDefaultCurrency,a.isQuantityValid,1 as isLeaf, 2 AS Type, '是' AS IsHelper
		,calDetail.Code as calCode,calDetail.Name as calName

  from TKS_FAS_FGLBalance A 	
  left join TKS_FAS_CaculateHelperDetail calDetail
	on calDetail.ParentId in(A.CalItem1,A.CalItem2,A.CalItem3,A.CalItem4,A.CalItem5)
	and calDetail.Code in(A.CalValue1,A.CalValue2,A.CalValue3,A.CalValue4,A.CalValue5)  
 where IsFirstPeriodAdd=1 )M  where M.accountId=@AccountId 
 
                            and m.CurrencyCode='RMB' and m.PeriodId=''
                        order by M.SubjectCode,M.Name ";
                    var data = cnn.Query<TKS_FAS_GLBalanceExt>(sql,
                        new
                        {
                            AccountId = user.AccountId,
                            PeriodId = firstPeriod.Id
                        }, ts).ToList();

                    return data;


                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw ex;
                }
            }

        }

        //public class Info
        //{
        //    public int SCredit_Debit;
        //    public decimal Value;
        //}
        public ResponseBALAdd BALAdd(RequestBALAdd request)
        {
            ResponseBALAdd response = new ResponseBALAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    var baseBAL = cnn.QueryFirst<TKS_FAS_GLBalance>(
                        @"select * from TKS_FAS_GLBalance where id=@Id and accountId=@AccountId",
                        new { Id = request.Data.BALId, AccountId = user.AccountId }, ts);
                    //update by Hero.Zhang 批量添加辅助核项目 20180903
                    foreach (var item in request.lst)
                    {
                        TKS_FAS_FGLBalance newData = new TKS_FAS_FGLBalance();
                        newData.Id = Guid.NewGuid().ToString("N");
                        newData.Category = baseBAL.Category;
                        newData.AccountId = user.AccountId;
                        newData.CurrencyCode = baseBAL.CurrencyCode;
                        newData.IsDefaultCurrency = baseBAL.IsDefaultCurrency;
                        newData.IsQuantityValid = baseBAL.IsQuantityValid;
                        newData.ParentId = baseBAL.Id;
                        newData.Unit = baseBAL.Unit;
                        newData.Year = baseBAL.Year;
                        newData.SCredit_Debit = baseBAL.SCredit_Debit;
                        newData.PeriodId = baseBAL.PeriodId;
                        newData.IsFirstPeriodAdd = 1;
                        newData.CreateUser = user.User.UserName;
                        newData.CreateDate = DateTime.Now;
                        string _name = baseBAL.Name;
                        string _code = baseBAL.SubjectCode;
                        #region 辅助核算赋值
                        //string[] cals = request.Data.CalValue1.Split(new char[] { '#' });
                        string[] cals = item.CalValue1.Split(new char[] { '#' });
                        for (int i = 0; i < cals.Length; i++)
                        {
                            if (string.IsNullOrEmpty(cals[i])) continue;
                            string[] ss = cals[i].Split(new char[] { ',' });
                            if (i == 0)
                            {
                                newData.CalItem1 = ss[0];
                                newData.CalValue1 = ss[1];
                                _code += "_" + ss[1];
                                _name += "_" + ss[2];

                            }
                            else if (i == 1)
                            {
                                newData.CalItem2 = ss[0];
                                newData.CalValue2 = ss[1];
                                _code += "_" + ss[1];
                                _name += "_" + ss[2];
                            }
                            else if (i == 2)
                            {
                                newData.CalItem3 = ss[0];
                                newData.CalValue3 = ss[1];
                                _code += "_" + ss[1];
                                _name += "_" + ss[2];
                            }
                            else if (i == 3)
                            {
                                newData.CalItem4 = ss[0];
                                newData.CalValue4 = ss[1];
                                _code += "_" + ss[1];
                                _name += "_" + ss[2];
                            }
                            else if (i == 4)
                            {
                                newData.CalItem5 = ss[0];
                                newData.CalValue5 = ss[1];
                                _code += "_" + ss[1];
                                _name += "_" + ss[2];
                            }

                        }
                        #endregion
                        //newData.SubjectCode = _code;
                        newData.SubjectCode = baseBAL.SubjectCode;//科目代码保持基础代码
                        newData.Name = _name;


                        var d = cnn.Query(@"select * from TKS_FAS_FGLBalance
                        where accountId=@AccountId  and subjectCode=@SubjectCode and currencyCode=@CurrencyCode",
                            new { AccountId = user.AccountId, SubjectCode = _code, CurrencyCode = baseBAL.CurrencyCode }, ts);

                        if (d.Count() > 0)
                        {
                            throw new NormalException("科目已经存在");
                        }
                        if (string.IsNullOrEmpty(newData.CalValue1))
                        {
                            throw new NormalException("辅助核算数据异常");
                        }
                        var r = cnn.Insert<TKS_FAS_FGLBalance>(newData, ts);

                        //添加一组没有期间ID的数据 Add By Hero.Zhang 20180512
                        //TKS_FAS_FGLBalance tData = newData;
                        //tData.Id = Guid.NewGuid().ToString("N");
                        //tData.PeriodId = "";
                        //var row = cnn.Insert<TKS_FAS_FGLBalance>(tData, ts);
                    }
                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBALAdd;
                }
            }
        }


        public ResponseBALCalculate BALCalculate(RequestBALCalculate request)
        {
            var res = new ResponseBALCalculate();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    res = CalculatePH(request, ts);

                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(res, ex) as ResponseBALCalculate;
                }
            }
        }

        public ResponseBALCalculate CalculatePH(RequestBALCalculate request, IDbTransaction ts)
        {
            ResponseBALCalculate res = new ResponseBALCalculate();
            var user = this.UserInfoGet(request.Token, ts);
            AccountSetBLL accountSet = new AccountSetBLL(cnn);
            var period = accountSet.GetActivePeriod(user.AccountId, ts);
            var firstPeriod = accountSet.GetFirstPeriod(user.AccountId, ts);
            #region modify by andy 期初值存放到无期间中
            //var data = cnn.Query<TKS_FAS_GLBalance>(
            //    @"select * from TKS_FAS_GLBalance where accountId=@AccountId and periodId=@PeriodId"
            //    , new { AccountId = user.AccountId, PeriodId = firstPeriod.Id }, ts).ToList();
            var data = cnn.Query<TKS_FAS_GLBalance>(
                @"select * from TKS_FAS_GLBalance where accountId=@AccountId and periodId=@PeriodId"
                , new { AccountId = user.AccountId, PeriodId = "" }, ts).ToList();
            #endregion

            #region 变量

            decimal startBWB1 = 0;//期初借方本位币合计 资产
            decimal startBWB2 = 0;//期初贷方本位币合计 负债
            decimal startBWB3 = 0;//期初贷方本位币合计 权益
            decimal startBWB4 = 0;//期初借方本位币合计 成本
            decimal startBWB5 = 0;//期初借方本位币合计 损益

            //第一期的累计是年度的累计

            decimal deLJBWB1 = 0;//借方累计合计 资产 
            decimal deLJBWB2 = 0;//借方累计合计 负债 
            decimal deLJBWB3 = 0;//借方累计合计 权益 
            decimal deLJBWB4 = 0;//借方累计合计 成本 
            decimal deLJBWB5 = 0;//借方累计合计 损益 

            decimal crLJBWB1 = 0;//贷方累计合计 资产 
            decimal crLJBWB2 = 0;//贷方累计合计 负债 
            decimal crLJBWB3 = 0;//贷方累计合计 权益 
            decimal crLJBWB4 = 0;//贷方累计合计 成本 
            decimal crLJBWB5 = 0;//贷方累计合计 损益 

            #endregion

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];
#if DEBUG
                if (item.SubjectCode == "1001")
                {

                }
#endif
                #region 期初余额统计
                if (item.SCredit_Debit == 0)
                {
                    if (item.Category == "1")//资产 借
                    {
                        startBWB1 += item.BWBStartBAL;
                    }
                    else if (item.Category == "2")//负债 贷
                    {
                        startBWB2 -= item.BWBStartBAL;
                    }
                    else if (item.Category == "3")//权益 贷
                    {
                        startBWB3 -= item.BWBStartBAL;
                    }
                    else if (item.Category == "4")//成本 借
                    {
                        startBWB4 += item.BWBStartBAL;
                    }
                    else if (item.Category == "5")//损益 借
                    {
                        startBWB5 += item.BWBStartBAL;
                    }
                }
                else
                {
                    if (item.Category == "1")
                    {
                        startBWB1 -= item.BWBStartBAL;
                    }
                    else if (item.Category == "2")
                    {
                        startBWB2 += item.BWBStartBAL;
                    }
                    else if (item.Category == "3")
                    {
                        startBWB3 += item.BWBStartBAL;
                    }
                    else if (item.Category == "4")
                    {
                        startBWB4 -= item.BWBStartBAL;
                    }
                    else if (item.Category == "5")
                    {
                        startBWB5 -= item.BWBStartBAL;
                    }

                }
                #endregion

                #region 本期累计合计
                if (item.Category == "1")//资产 借
                {
                    deLJBWB1 += item.BWBDebitTotal_Y;
                    crLJBWB1 += item.BWBCreditTotal_Y;
                }
                else if (item.Category == "2")//负债 贷
                {
                    deLJBWB2 += item.BWBDebitTotal_Y;
                    crLJBWB2 += item.BWBCreditTotal_Y;
                }
                else if (item.Category == "3")//权益 贷
                {
                    deLJBWB3 += item.BWBDebitTotal_Y;
                    crLJBWB3 += item.BWBCreditTotal_Y;
                }
                else if (item.Category == "4")//成本 借
                {
                    deLJBWB4 += item.BWBDebitTotal_Y;
                    crLJBWB4 += item.BWBCreditTotal_Y;
                }
                else if (item.Category == "5")//损益 借
                {
                    deLJBWB5 += item.BWBDebitTotal_Y;
                    crLJBWB5 += item.BWBCreditTotal_Y;
                }
                #endregion
            }

            //var data2 = cnn.Query<TKS_FAS_FGLBalance>(
            // @"select * from TKS_FAS_FGLBalance where accountId=@AccountId and periodId=@PeriodId"
            // , new { AccountId = user.AccountId, PeriodId = firstPeriod.Id }, ts).ToList();
            var data2 = cnn.Query<TKS_FAS_FGLBalance>(
             @"select * from TKS_FAS_FGLBalance where accountId=@AccountId and periodId=@PeriodId"
             , new { AccountId = user.AccountId, PeriodId = "" }, ts).ToList();
            for (int i = 0; i < data2.Count; i++)
            {
                var item = data2[i];

                if (item.SCredit_Debit == 0)
                {
                    if (item.Category == "1")
                    {

                        startBWB1 += item.BWBStartBAL;
                    }
                    else if (item.Category == "2")
                    {

                        startBWB2 -= item.BWBStartBAL;
                    }
                    else if (item.Category == "3")
                    {

                        startBWB3 -= item.BWBStartBAL;
                    }
                    else if (item.Category == "4")
                    {

                        startBWB4 += item.BWBStartBAL;
                    }
                    else if (item.Category == "5")
                    {

                        startBWB5 += item.BWBStartBAL;
                    }
                }
                else
                {
                    if (item.Category == "1")
                    {

                        startBWB1 -= item.BWBStartBAL;
                    }
                    else if (item.Category == "2")
                    {

                        startBWB2 += item.BWBStartBAL;
                    }
                    else if (item.Category == "3")
                    {

                        startBWB3 += item.BWBStartBAL;
                    }
                    else if (item.Category == "4")
                    {

                        startBWB4 -= item.BWBStartBAL;
                    }
                    else if (item.Category == "5")
                    {

                        startBWB5 -= item.BWBStartBAL;
                    }
                }

                #region 本期累计合计
                if (item.Category == "1")//资产 借
                {
                    deLJBWB1 += item.BWBDebitTotal_Y;
                    crLJBWB1 += item.BWBCreditTotal_Y;
                }
                else if (item.Category == "2")//负债 贷
                {
                    deLJBWB2 += item.BWBDebitTotal_Y;
                    crLJBWB2 += item.BWBCreditTotal_Y;
                }
                else if (item.Category == "3")//权益 贷
                {
                    deLJBWB3 += item.BWBDebitTotal_Y;
                    crLJBWB3 += item.BWBCreditTotal_Y;
                }
                else if (item.Category == "4")//成本 借
                {
                    deLJBWB4 += item.BWBDebitTotal_Y;
                    crLJBWB4 += item.BWBCreditTotal_Y;
                }
                else if (item.Category == "5")//损益 借
                {
                    deLJBWB5 += item.BWBDebitTotal_Y;
                    crLJBWB5 += item.BWBCreditTotal_Y;
                }
                #endregion
            }
            string error = string.Empty;
            //1：资产
            //2：负债
            //3：权益
            //4：成本
            //5：损益

            if ((startBWB1 + startBWB4 + startBWB5) != (startBWB2 + startBWB3))
            {
                error += "【期初余额借贷不平衡】";
            }
            var d1 = deLJBWB1 + deLJBWB2 + deLJBWB3 + deLJBWB4 + deLJBWB5;
            var d2 = crLJBWB1 + crLJBWB2 + crLJBWB3 + crLJBWB4 + crLJBWB5;
            if (d1 != d2)
            {
                error += "【期初累计发生额不平衡】";
            }

            res.startBWB1 = startBWB1;
            res.startBWB2 = startBWB2;
            res.startBWB3 = startBWB3;
            res.startBWB4 = startBWB4;
            res.startBWB5 = startBWB5;
            res.deLJBWB1 = deLJBWB1;
            res.deLJBWB2 = deLJBWB2;
            res.deLJBWB3 = deLJBWB3;
            res.deLJBWB4 = deLJBWB4;
            res.deLJBWB5 = deLJBWB5;
            res.crLJBWB1 = crLJBWB1;
            res.crLJBWB2 = crLJBWB2;
            res.crLJBWB3 = crLJBWB3;
            res.crLJBWB4 = crLJBWB4;
            res.crLJBWB5 = crLJBWB5;

            if (!string.IsNullOrEmpty(error))
            {
                res.IsSuccess = false;
                res.Message = error;
            }
            else
            {
                res.IsSuccess = true;
                res.Message = "借贷平衡";
            }
            #region add by Hero.Zhang 期初结转时，匹配固定资产

            string sql = @"select * from tks_fas_periodTrack where accountId=@AccountId
                            and toId=@ToId";
            List<TKS_FAS_PeriodTrack> lsTrack = cnn.Query<TKS_FAS_PeriodTrack>(sql,
                new { AccountId = user.AccountId, ToId = period.Id }, ts).ToList();
            if (lsTrack.Count == 0)
            {
                //当前期间为该账套期初期间
                var FixedAssets = cnn.QueryFirst<FixedAssetsInitialAssetValue>(@"select ISNULL(Sum(InitialAssetValue),0) as InitialAssetValue from TKS_FAS_FixedAssets
 where AccountId=@AccountId and AddType='期初'", new { AccountId = user.AccountId }, ts);

                string sqlQC = @"SELECT TT.SLevel,TT.RootCode, T.*
                                    FROM (
	                                    SELECT M.*
	                                    FROM (
		                                    SELECT A.id, a.AccountId, A.subjectId, a.Category, a.PeriodId,a.ParentId
			                                    , a.SubjectCode, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal
			                                    , a.NUMDebitTotal_Y, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y
			                                    , a.NUMCreditTotal, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal
			                                    , a.YBCreditTotal_Y, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL
			                                    , a.YearStartBWBBAL, a.YearStartYBBAL, a.isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid
			                                    , a.isLeaf, 1 AS Type
		                                    FROM TKS_FAS_GLBalance A
		                                    UNION
		                                    SELECT A.id, a.AccountId, '' AS subjectId, a.Category, a.PeriodId,a.ParentId
			                                    , a.SubjectCode, a.Name, a.CurrencyCode, a.SCredit_Debit, a.NUMDebitTotal
			                                    , a.NUMDebitTotal_Y, a.BWBDebitTotal, a.BWBDebitTotal_Y, a.YBDebitTotal, a.YBDebitTotal_Y
			                                    , a.NUMCreditTotal, a.NUMCreditTotal_Y, a.BWBCreditTotal, a.BWBCreditTotal_Y, a.YBCreditTotal
			                                    , a.YBCreditTotal_Y, a.NUMStartBAL, a.BWBStartBAL, a.YBStartBAL, a.YearStartNumBAL
			                                    , a.YearStartBWBBAL, a.YearStartYBBAL, 0 AS isCalHelperValid, a.isDefaultCurrency, a.isQuantityValid
			                                    , 1 AS isLeaf, 2 AS Type
		                                    FROM TKS_FAS_FGLBalance A
		                                    WHERE IsFirstPeriodAdd = 1
	                                    ) M
	                                    WHERE M.accountId = @AccountId
		                                    AND m.category = @Category
		                                    AND m.CurrencyCode = @CurrencyCode
		                                    AND m.PeriodId = ''
                                    ) T
	                                    LEFT JOIN (
		                                    SELECT SLevel,Code, RootCode
		                                    FROM TKS_FAS_AccountSubject
		                                    WHERE category = @Category
			                                    AND accountId = @AccountId

	                                    ) TT
	                                    ON TT.Code = T.SubjectCode
                                        where TT.RootCode='1501'
	                                    ORDER BY TT.RootCode,T.SubjectCode,T.Name";
                List<TKS_FAS_GLBalanceExt> data_GLBalance = cnn.Query<TKS_FAS_GLBalanceExt>(sqlQC,
              new
              {
                  AccountId = user.AccountId,//账套
                  Category = 1,//科目类别
                  CurrencyCode = "RMB"//币别
              }, ts).ToList();
                decimal val = data_GLBalance.Sum(p => p.BWBStartBAL);
                if (val != FixedAssets.InitialAssetValue)
                {
                    res.IsSuccess = false;
                    res.Message = "固定资产原值与期初固定资产不匹配</br>固定资产原值：" + FixedAssets.InitialAssetValue.ToString() + "</br>期初固定资产：" + val.ToString();

                }
            }
            #endregion
            return res;
        }



        private string GenSubjectCode(RequestBALAdd request, TKS_FAS_GLBalance parent, IDbTransaction ts)
        {
            return parent.SubjectCode + request.Data.CalValue1;
        }

        TKS_FAS_GLBalance Calculate(string inputType, decimal money, string accountId, TKS_FAS_GLBalance bal, IDbTransaction ts)
        {

            if (bal.SCredit_Debit == 0)
            {
                #region 借 
                var currency = cnn.QueryFirst<TKS_FAS_Currency>(
                       @"select * from tks_fas_currency where accountId=@AccountId and code=@Code",
                       new { AccountId = accountId, Code = bal.CurrencyCode }, ts);
                //年初余额= 期初 + 贷方累计 - 借方累计
                switch (inputType)
                {
                    case "NUMStartBAL":
                        {
                            bal.NUMStartBAL = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL + bal.NUMCreditTotal_Y - bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBStartBAL":
                        {
                            bal.YBStartBAL = money;
                            bal.BWBStartBAL = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL + bal.YBCreditTotal_Y - bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBStartBAL":
                        {
                            bal.BWBStartBAL = money;
                            // bal.YBStartBAL = Math.Round(money / currency.ExchangeRate, 2);
                            bal.YearStartBWBBAL = bal.BWBStartBAL + bal.BWBCreditTotal_Y - bal.BWBDebitTotal_Y;
                            break;
                        }
                    case "NUMDebitTotal_Y":
                        {
                            bal.NUMDebitTotal_Y = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL + bal.NUMCreditTotal_Y - bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBDebitTotal_Y":
                        {
                            bal.YBDebitTotal_Y = money;
                            bal.BWBDebitTotal_Y = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL + bal.YBCreditTotal_Y - bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBDebitTotal_Y":
                        {
                            bal.BWBDebitTotal_Y = money;
                            bal.YearStartBWBBAL = bal.BWBStartBAL + bal.BWBCreditTotal_Y - bal.BWBDebitTotal_Y;
                            break;
                        }
                    case "NUMCreditTotal_Y":
                        {
                            bal.NUMCreditTotal_Y = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL + bal.NUMCreditTotal_Y - bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBCreditTotal_Y":
                        {
                            bal.YBCreditTotal_Y = money;
                            bal.BWBCreditTotal_Y = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL + bal.YBCreditTotal_Y - bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBCreditTotal_Y":
                        {
                            bal.BWBCreditTotal_Y = money;
                            bal.YearStartBWBBAL = bal.BWBStartBAL + bal.BWBCreditTotal_Y - bal.BWBDebitTotal_Y;
                            break;
                        }
                    default:
                        break;
                }
                #endregion
            }
            else
            {
                #region 贷 
                var currency = cnn.QueryFirst<TKS_FAS_Currency>(
                       @"select * from tks_fas_currency where accountId=@AccountId and code=@Code",
                       new { AccountId = accountId, Code = bal.CurrencyCode }, ts);
                //年初余额= 期初 + 借方累计 - 贷方累计 
                switch (inputType)
                {
                    case "NUMStartBAL":
                        {
                            bal.NUMStartBAL = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL - bal.NUMCreditTotal_Y + bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBStartBAL":
                        {
                            bal.YBStartBAL = money;
                            bal.BWBStartBAL = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL - bal.YBCreditTotal_Y + bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBStartBAL":
                        {
                            bal.BWBStartBAL = money;
                            // bal.YBStartBAL = Math.Round(money / currency.ExchangeRate, 2);
                            bal.YearStartBWBBAL = bal.BWBStartBAL - bal.BWBCreditTotal_Y + bal.BWBDebitTotal_Y;
                            break;
                        }
                    case "NUMDebitTotal_Y":
                        {
                            bal.NUMDebitTotal_Y = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL - bal.NUMCreditTotal_Y + bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBDebitTotal_Y":
                        {
                            bal.YBDebitTotal_Y = money;
                            bal.BWBDebitTotal_Y = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL - bal.YBCreditTotal_Y + bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBDebitTotal_Y":
                        {
                            bal.BWBDebitTotal_Y = money;
                            bal.YearStartBWBBAL = bal.BWBStartBAL - bal.BWBCreditTotal_Y + bal.BWBDebitTotal_Y;
                            break;
                        }
                    case "NUMCreditTotal_Y":
                        {
                            bal.NUMCreditTotal_Y = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL - bal.NUMCreditTotal_Y + bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBCreditTotal_Y":
                        {
                            bal.YBCreditTotal_Y = money;
                            bal.BWBCreditTotal_Y = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL - bal.YBCreditTotal_Y + bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBCreditTotal_Y":
                        {
                            bal.BWBCreditTotal_Y = money;
                            bal.YearStartBWBBAL = bal.BWBStartBAL - bal.BWBCreditTotal_Y + bal.BWBDebitTotal_Y;
                            break;
                        }
                    default:
                        break;
                }
                #endregion
            }

            return bal;
        }

        TKS_FAS_FGLBalance Calculate(string inputType, decimal money, string accountId, TKS_FAS_FGLBalance bal, IDbTransaction ts)
        {
            if (bal.SCredit_Debit == 0)
            {
                #region 借 
                var currency = cnn.QueryFirst<TKS_FAS_Currency>(
                       @"select * from tks_fas_currency where accountId=@AccountId and code=@Code",
                       new { AccountId = accountId, Code = bal.CurrencyCode }, ts);
                //年初余额= 期初 + 贷方累计 - 借方累计
                switch (inputType)
                {
                    case "NUMStartBAL":
                        {
                            bal.NUMStartBAL = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL + bal.NUMCreditTotal_Y - bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBStartBAL":
                        {
                            bal.YBStartBAL = money;
                            bal.BWBStartBAL = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL + bal.YBCreditTotal_Y - bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBStartBAL":
                        {
                            bal.BWBStartBAL = money;
                            // bal.YBStartBAL = Math.Round(money / currency.ExchangeRate, 2);
                            bal.YearStartBWBBAL = bal.BWBStartBAL + bal.BWBCreditTotal_Y - bal.BWBDebitTotal_Y;
                            break;
                        }
                    case "NUMDebitTotal_Y":
                        {
                            bal.NUMDebitTotal_Y = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL + bal.NUMCreditTotal_Y - bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBDebitTotal_Y":
                        {
                            bal.YBDebitTotal_Y = money;
                            bal.BWBDebitTotal_Y = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL + bal.YBCreditTotal_Y - bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBDebitTotal_Y":
                        {
                            bal.BWBDebitTotal_Y = money;
                            bal.YearStartBWBBAL = bal.BWBStartBAL + bal.BWBCreditTotal_Y - bal.BWBDebitTotal_Y;
                            break;
                        }
                    case "NUMCreditTotal_Y":
                        {
                            bal.NUMCreditTotal_Y = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL + bal.NUMCreditTotal_Y - bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBCreditTotal_Y":
                        {
                            bal.YBCreditTotal_Y = money;
                            bal.BWBCreditTotal_Y = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL + bal.YBCreditTotal_Y - bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBCreditTotal_Y":
                        {
                            bal.BWBCreditTotal_Y = money;
                            bal.YearStartBWBBAL = bal.BWBStartBAL + bal.BWBCreditTotal_Y - bal.BWBDebitTotal_Y;
                            break;
                        }
                    default:
                        break;
                }
                #endregion
            }
            else
            {
                #region 贷 
                var currency = cnn.QueryFirst<TKS_FAS_Currency>(
                       @"select * from tks_fas_currency where accountId=@AccountId and code=@Code",
                       new { AccountId = accountId, Code = bal.CurrencyCode }, ts);
                //年初余额= 期初 + 借方累计 - 贷方累计 
                switch (inputType)
                {
                    case "NUMStartBAL":
                        {
                            bal.NUMStartBAL = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL - bal.NUMCreditTotal_Y + bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBStartBAL":
                        {
                            bal.YBStartBAL = money;
                            bal.BWBStartBAL = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL - bal.YBCreditTotal_Y + bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBStartBAL":
                        {
                            bal.BWBStartBAL = money;
                            // bal.YBStartBAL = Math.Round(money / currency.ExchangeRate, 2);
                            bal.YearStartBWBBAL = bal.BWBStartBAL - bal.BWBCreditTotal_Y + bal.BWBDebitTotal_Y;
                            break;
                        }
                    case "NUMDebitTotal_Y":
                        {
                            bal.NUMDebitTotal_Y = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL - bal.NUMCreditTotal_Y + bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBDebitTotal_Y":
                        {
                            bal.YBDebitTotal_Y = money;
                            bal.BWBDebitTotal_Y = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL - bal.YBCreditTotal_Y + bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBDebitTotal_Y":
                        {
                            bal.BWBDebitTotal_Y = money;
                            bal.YearStartBWBBAL = bal.BWBStartBAL - bal.BWBCreditTotal_Y + bal.BWBDebitTotal_Y;
                            break;
                        }
                    case "NUMCreditTotal_Y":
                        {
                            bal.NUMCreditTotal_Y = money;
                            bal.YearStartNumBAL = bal.NUMStartBAL - bal.NUMCreditTotal_Y + bal.NUMDebitTotal_Y;
                            break;
                        }
                    case "YBCreditTotal_Y":
                        {
                            bal.YBCreditTotal_Y = money;
                            bal.BWBCreditTotal_Y = money * currency.ExchangeRate;
                            bal.YearStartYBBAL = bal.YBStartBAL - bal.YBCreditTotal_Y + bal.YBDebitTotal_Y;
                            break;
                        }
                    case "BWBCreditTotal_Y":
                        {
                            bal.BWBCreditTotal_Y = money;
                            bal.YearStartBWBBAL = bal.BWBStartBAL - bal.BWBCreditTotal_Y + bal.BWBDebitTotal_Y;
                            break;
                        }
                    default:
                        break;
                }
                #endregion
            }
            return bal;
        }
        public ResponseBALUpdate BALUpdate(RequestBALUpdate request)
        {
            ResponseBALUpdate response = new ResponseBALUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {

                    BalanceBLL balanceBLL = new BalanceBLL(cnn);
                    PeriodBLL periodBLL = new PeriodBLL(cnn);
                    var user = this.UserInfoGet(request.Token, ts);

                    string sql = string.Empty;
                    if (request.Type == 1)
                    {
                        var item = cnn.QueryFirst<TKS_FAS_GLBalance>(
                            @"select * from TKS_FAS_GLBalance where id=@Id", new { Id = request.Id }, ts);

                        sql = @"UPDATE TKS_FAS_GLBalance
                           SET  
                                NUMStartBAL=@NUMStartBAL,
                                BWBStartBAL=@BWBStartBAL,
                                YBStartBAL=@YBStartBAL,
                                NUMDebitTotal_Y=@NUMDebitTotal_Y,
                                BWBDebitTotal_Y=@BWBDebitTotal_Y,
                                YBDebitTotal_Y=@YBDebitTotal_Y,
                                NUMCreditTotal_Y=@NUMCreditTotal_Y,
                                BWBCreditTotal_Y=@BWBCreditTotal_Y,
                                YBCreditTotal_Y=@YBCreditTotal_Y,
                                YearStartNumBAL=@YearStartNumBAL,
                                YearStartYBBAL=@YearStartYBBAL,
                                YearStartBWBBAL=@YearStartBWBBAL
                           WHERE
                            id=@Id
                           ";
                        var newData = Calculate(request.InputType, request.Money, user.AccountId, item, ts);
                        cnn.Execute(sql, newData, ts);
                        #region 期初数据同步到后续期间中

                        //var startDte = firstPeriod.StartDate?.AddDays(-1);
                        //var nextPeriods = periodBLL.GetNextPeriods(user.AccountId, startDte?.Year ?? 0, startDte?.Month ?? 0, ts);
                        //TKS_FAS_GLBalance balance = new TKS_FAS_GLBalance();
                        //balance.AccountId = newData.AccountId;
                        //balance.SubjectCode = newData.SubjectCode;
                        //balance.SubjectId = newData.SubjectId;
                        //balance.SCredit_Debit = newData.SCredit_Debit;
                        //balance.Name = newData.Name;
                        //balance.IsLeaf = newData.IsLeaf;
                        //balance.IsDefaultCurrency = newData.IsDefaultCurrency;
                        //balance.IsCalHelperValid = newData.IsCalHelperValid;
                        //balance.IsQuantityValid = newData.IsQuantityValid;
                        //balance.Year = newData.Year;
                        //balance.Category = newData.Category;
                        //balance.CurrencyCode = newData.CurrencyCode;

                        //balance.NUMStartBAL = newData.NUMStartBAL;
                        //balance.BWBStartBAL = newData.BWBStartBAL;
                        //balance.YBStartBAL = newData.YBStartBAL;
                        //balance.NUMDebitTotal_Y = newData.NUMDebitTotal_Y;
                        //balance.BWBDebitTotal_Y = newData.BWBDebitTotal_Y;
                        //balance.YBDebitTotal_Y = newData.YBDebitTotal_Y;
                        //balance.NUMCreditTotal_Y = newData.NUMCreditTotal_Y;
                        //balance.BWBCreditTotal_Y = newData.BWBCreditTotal_Y;
                        //balance.YBCreditTotal_Y = newData.YBCreditTotal_Y;
                        //balance.YearStartNumBAL = newData.YearStartNumBAL;
                        //balance.YearStartYBBAL = newData.YearStartYBBAL;
                        //balance.YearStartBWBBAL = newData.YearStartBWBBAL;
                        //balance.ECredit_Debit = newData.SCredit_Debit;
                        //balance.NUMEndBAL = balance.NUMStartBAL;
                        //balance.BWBEndBAL = balance.BWBStartBAL;
                        //balance.YBEndBAL = balance.YBStartBAL;
                        ////年初  年度累计  自动清0
                        //if (firstPeriod.Month == 1)
                        //{
                        //    balance.NUMCreditTotal_Y = 0;
                        //    balance.NUMDebitTotal_Y = 0;
                        //    balance.YBCreditTotal_Y = 0;
                        //    balance.YBCreditTotal_Y = 0;
                        //    balance.BWBDebitTotal_Y = 0;
                        //    balance.BWBCreditTotal_Y = 0;
                        //    balance.YearStartBWBBAL = balance.BWBStartBAL;
                        //    balance.YearStartNumBAL = balance.NUMStartBAL;
                        //    balance.YearStartYBBAL = balance.NUMStartBAL;
                        //}

                        //foreach (var nextPeriod in nextPeriods)
                        //{
                        //    var thisBalance = cnn.QueryFirstOrDefault<TKS_FAS_GLBalance>("select * from TKS_FAS_GLBalance where AccountId=@AccountId and SubjectId=@SubjectId and PeriodId=@PeriodId", new { AccountId = balance.AccountId, SubjectId = balance.SubjectId, PeriodId = nextPeriod.Id }, ts);
                        //    if (thisBalance == null)
                        //    {
                        //        thisBalance = new TKS_FAS_GLBalance();
                        //        thisBalance.Id = Guid.NewGuid().ToString("N");
                        //        thisBalance.SCredit_Debit = balance.ECredit_Debit;
                        //        thisBalance.AccountId = balance.AccountId;
                        //        thisBalance.PeriodId = nextPeriod.Id;
                        //        thisBalance.SubjectCode = balance.SubjectCode;
                        //        thisBalance.Name = balance.Name;
                        //        thisBalance.IsLeaf = balance.IsLeaf;
                        //        thisBalance.IsDefaultCurrency = balance.IsDefaultCurrency;
                        //        thisBalance.IsCalHelperValid = balance.IsCalHelperValid;
                        //        thisBalance.IsQuantityValid = balance.IsQuantityValid;
                        //        thisBalance.Year = balance.Year;
                        //        thisBalance.Category = balance.Category;
                        //        thisBalance.CurrencyCode = balance.CurrencyCode;
                        //        cnn.Insert<TKS_FAS_GLBalance>(thisBalance, ts);
                        //    }
                        //    balance = balanceBLL.GLBanceNextPeriodUpdate(balance, thisBalance, ts);
                        //}


                        #endregion
                        UpdateToFirstPeriod(request.Type, newData, null, user, request.Money, request.InputType, ts);
                    }
                    else
                    {
                        var item = cnn.QueryFirst<TKS_FAS_FGLBalance>(
                           @"select * from TKS_FAS_FGLBalance where id=@Id", new { Id = request.Id }, ts);


                        sql = @"UPDATE TKS_FAS_FGLBalance
                           SET  
                                NUMStartBAL=@NUMStartBAL,
                                BWBStartBAL=@BWBStartBAL,
                                YBStartBAL=@YBStartBAL,
                                NUMDebitTotal_Y=@NUMDebitTotal_Y,
                                BWBDebitTotal_Y=@BWBDebitTotal_Y,
                                YBDebitTotal_Y=@YBDebitTotal_Y,
                                NUMCreditTotal_Y=@NUMCreditTotal_Y,
                                BWBCreditTotal_Y=@BWBCreditTotal_Y,
                                YBCreditTotal_Y=@YBCreditTotal_Y,
                                YearStartNumBAL=@YearStartNumBAL,
                                YearStartYBBAL=@YearStartYBBAL,
                                YearStartBWBBAL=@YearStartBWBBAL
                           WHERE
                            id=@Id
                           ";

                        var newData = Calculate(request.InputType, request.Money, user.AccountId, item, ts);
                        cnn.Execute(sql, newData, ts);
                        #region 期初数据同步到第一期
                        //var firstPeriodItem = cnn.QueryFirstOrDefault<TKS_FAS_FGLBalance>(
                        //   @"select * from TKS_FAS_FGLBalance where AccountId=@AccountId and PeriodId=@PeriodId and Name=@Name and IsFirstPeriodAdd=1 ", new { AccountId = firstPeriod.AccountId, PeriodId = firstPeriod.Id, Name = newData.Name }, ts);

                        //if (firstPeriodItem == null)
                        //{
                        //    firstPeriodItem = newData;
                        //    firstPeriodItem.PeriodId = firstPeriod.Id;
                        //    firstPeriodItem.Id = Guid.NewGuid().ToString("N");
                        //    cnn.Insert<TKS_FAS_FGLBalance>(firstPeriodItem, ts);
                        //}
                        //else
                        //{
                        //    string periodId = firstPeriodItem.Id;
                        //    firstPeriodItem = newData;
                        //    firstPeriodItem.Id = periodId;
                        //}
                        //if (firstPeriod.Month == 1)
                        //{
                        //    firstPeriodItem.NUMCreditTotal_Y = 0;
                        //    firstPeriodItem.NUMDebitTotal_Y = 0;
                        //    firstPeriodItem.YBCreditTotal_Y = 0;
                        //    firstPeriodItem.YBCreditTotal_Y = 0;
                        //    firstPeriodItem.BWBDebitTotal_Y = 0;
                        //    firstPeriodItem.BWBCreditTotal_Y = 0;
                        //    firstPeriodItem.YearStartBWBBAL = firstPeriodItem.BWBStartBAL;
                        //    firstPeriodItem.YearStartNumBAL = firstPeriodItem.NUMStartBAL;
                        //    firstPeriodItem.YearStartYBBAL = firstPeriodItem.NUMStartBAL;
                        //}
                        //else
                        //{
                        //    firstPeriodItem = Calculate(request.InputType, request.Money, user.AccountId, firstPeriodItem, ts);
                        //}
                        //firstPeriodItem.ECredit_Debit = firstPeriodItem.SCredit_Debit;
                        //firstPeriodItem.BWBEndBAL = firstPeriodItem.BWBStartBAL;
                        //firstPeriodItem.NUMEndBAL = firstPeriodItem.NUMStartBAL;
                        //firstPeriodItem.YBEndBAL = firstPeriodItem.YBStartBAL;
                        //sql = @"UPDATE TKS_FAS_FGLBalance
                        //   SET  
                        //        NUMStartBAL=@NUMStartBAL,
                        //        BWBStartBAL=@BWBStartBAL,
                        //        YBStartBAL=@YBStartBAL,
                        //        NUMDebitTotal_Y=@NUMDebitTotal_Y,
                        //        BWBDebitTotal_Y=@BWBDebitTotal_Y,
                        //        YBDebitTotal_Y=@YBDebitTotal_Y,
                        //        NUMCreditTotal_Y=@NUMCreditTotal_Y,
                        //        BWBCreditTotal_Y=@BWBCreditTotal_Y,
                        //        YBCreditTotal_Y=@YBCreditTotal_Y,
                        //        YearStartNumBAL=@YearStartNumBAL,
                        //        YearStartYBBAL=@YearStartYBBAL,
                        //        YearStartBWBBAL=@YearStartBWBBAL,
                        //        ECredit_Debit = @ECredit_Debit,
                        //        BWBEndBAL = @BWBEndBAL,
                        //        NUMEndBAL = @NUMEndBAL,
                        //        YBEndBAL = @YBEndBAL
                        //   WHERE
                        //    id=@Id
                        //   ";
                        //cnn.Execute(sql, firstPeriodItem, ts);





                        #endregion
                        UpdateToFirstPeriod(request.Type, null, newData, user, request.Money, request.InputType, ts);
                    }




                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "更新成功";
                    return response;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBALUpdate;
                }

            }
        }

        /// <summary>
        /// 期初数据更新到第一期 add by Hero.Zhang
        /// </summary>
        /// <param name="type"></param>
        /// <param name="GLBalance"></param>
        /// <param name="FGLBalance"></param>
        /// <param name="firstPeriod"></param>
        /// <param name="user"></param>
        /// <param name="Money"></param>
        /// <param name="InputType"></param>
        /// <param name="cnn"></param>
        /// <param name="ts"></param>
        public void UpdateToFirstPeriod(int type, TKS_FAS_GLBalance GLBalance, TKS_FAS_FGLBalance FGLBalance, MM_UserInfo user, decimal Money, string InputType, IDbTransaction ts)
        {
            //AccountSetBLL accountSetBLL = new AccountSetBLL(cnn);
            BalanceBLL balanceBLL = new BalanceBLL(cnn);
            PeriodBLL periodBLL = new PeriodBLL(cnn);
            AccountSetBLL accountSetBLL = new AccountSetBLL(cnn);
            var firstPeriod = accountSetBLL.GetFirstPeriod(user.AccountId, ts);
            var startDte = firstPeriod.StartDate?.AddDays(-1);
            var nextPeriods = periodBLL.GetNextPeriods(user.AccountId, startDte?.Year ?? 0, startDte?.Month ?? 0, ts);
            if (type == 1)
            {
                #region 期初数据同步到后续期间中


                TKS_FAS_GLBalance balance = new TKS_FAS_GLBalance();
                balance.AccountId = GLBalance.AccountId;
                balance.SubjectCode = GLBalance.SubjectCode;
                balance.SubjectId = GLBalance.SubjectId;
                balance.SCredit_Debit = GLBalance.SCredit_Debit;
                balance.Name = GLBalance.Name;
                balance.IsLeaf = GLBalance.IsLeaf;
                balance.IsDefaultCurrency = GLBalance.IsDefaultCurrency;
                balance.IsCalHelperValid = GLBalance.IsCalHelperValid;
                balance.IsQuantityValid = GLBalance.IsQuantityValid;
                balance.Year = GLBalance.Year;
                balance.Category = GLBalance.Category;
                balance.CurrencyCode = GLBalance.CurrencyCode;

                balance.NUMStartBAL = GLBalance.NUMStartBAL;
                balance.BWBStartBAL = GLBalance.BWBStartBAL;
                balance.YBStartBAL = GLBalance.YBStartBAL;
                balance.NUMDebitTotal_Y = GLBalance.NUMDebitTotal_Y;
                balance.BWBDebitTotal_Y = GLBalance.BWBDebitTotal_Y;
                balance.YBDebitTotal_Y = GLBalance.YBDebitTotal_Y;
                balance.NUMCreditTotal_Y = GLBalance.NUMCreditTotal_Y;
                balance.BWBCreditTotal_Y = GLBalance.BWBCreditTotal_Y;
                balance.YBCreditTotal_Y = GLBalance.YBCreditTotal_Y;
                balance.YearStartNumBAL = GLBalance.YearStartNumBAL;
                balance.YearStartYBBAL = GLBalance.YearStartYBBAL;
                balance.YearStartBWBBAL = GLBalance.YearStartBWBBAL;
                balance.ECredit_Debit = GLBalance.SCredit_Debit;
                balance.NUMEndBAL = balance.NUMStartBAL;
                balance.BWBEndBAL = balance.BWBStartBAL;
                balance.YBEndBAL = balance.YBStartBAL;
                //年初  年度累计  自动清0
                if (firstPeriod.Month == 1)
                {
                    balance.NUMCreditTotal_Y = 0;
                    balance.NUMDebitTotal_Y = 0;
                    balance.YBCreditTotal_Y = 0;
                    balance.YBCreditTotal_Y = 0;
                    balance.BWBDebitTotal_Y = 0;
                    balance.BWBCreditTotal_Y = 0;
                    balance.YearStartBWBBAL = balance.BWBStartBAL;
                    balance.YearStartNumBAL = balance.NUMStartBAL;
                    balance.YearStartYBBAL = balance.NUMStartBAL;
                }

                foreach (var nextPeriod in nextPeriods)
                {
                    var thisBalance = cnn.QueryFirstOrDefault<TKS_FAS_GLBalance>("select * from TKS_FAS_GLBalance where AccountId=@AccountId and SubjectId=@SubjectId and PeriodId=@PeriodId", new { AccountId = balance.AccountId, SubjectId = balance.SubjectId, PeriodId = nextPeriod.Id }, ts);
                    if (thisBalance == null)
                    {
                        thisBalance = new TKS_FAS_GLBalance();
                        thisBalance.Id = Guid.NewGuid().ToString("N");
                        thisBalance.SCredit_Debit = balance.ECredit_Debit;
                        thisBalance.AccountId = balance.AccountId;
                        thisBalance.PeriodId = nextPeriod.Id;
                        thisBalance.SubjectCode = balance.SubjectCode;
                        thisBalance.Name = balance.Name;
                        thisBalance.IsLeaf = balance.IsLeaf;
                        thisBalance.IsDefaultCurrency = balance.IsDefaultCurrency;
                        thisBalance.IsCalHelperValid = balance.IsCalHelperValid;
                        thisBalance.IsQuantityValid = balance.IsQuantityValid;
                        thisBalance.Year = balance.Year;
                        thisBalance.Category = balance.Category;
                        thisBalance.CurrencyCode = balance.CurrencyCode;
                        cnn.Insert<TKS_FAS_GLBalance>(thisBalance, ts);
                    }
                    balance = balanceBLL.GLBanceNextPeriodUpdate(balance, thisBalance, ts);
                }


                #endregion
            }
            else
            {
                #region 期初数据同步到第一期
                var firstPeriodItem = cnn.QueryFirstOrDefault<TKS_FAS_FGLBalance>(
                   @"select * from TKS_FAS_FGLBalance where AccountId=@AccountId and SubjectCode=@SubjectCode and PeriodId=@PeriodId and Name=@Name and IsFirstPeriodAdd=1 ", new { AccountId = firstPeriod.AccountId, PeriodId = firstPeriod.Id, Name = FGLBalance.Name, SubjectCode = FGLBalance.SubjectCode }, ts);

                if (firstPeriodItem == null)
                {
                    firstPeriodItem = FGLBalance;
                    firstPeriodItem.PeriodId = firstPeriod.Id;
                    firstPeriodItem.Id = Guid.NewGuid().ToString("N");
                    if (string.IsNullOrEmpty(firstPeriodItem.CalValue1))
                    {
                        throw new NormalException("辅助核算数据异常");
                    }
                    cnn.Insert<TKS_FAS_FGLBalance>(firstPeriodItem, ts);
                }
                else
                {
                    string periodId = firstPeriodItem.Id;
                    firstPeriodItem = FGLBalance;
                    firstPeriodItem.Id = periodId;
                }
                if (firstPeriod.Month == 1)
                {
                    firstPeriodItem.NUMCreditTotal_Y = 0;
                    firstPeriodItem.NUMDebitTotal_Y = 0;
                    firstPeriodItem.YBCreditTotal_Y = 0;
                    firstPeriodItem.YBCreditTotal_Y = 0;
                    firstPeriodItem.BWBDebitTotal_Y = 0;
                    firstPeriodItem.BWBCreditTotal_Y = 0;
                    firstPeriodItem.YearStartBWBBAL = firstPeriodItem.BWBStartBAL;
                    firstPeriodItem.YearStartNumBAL = firstPeriodItem.NUMStartBAL;
                    firstPeriodItem.YearStartYBBAL = firstPeriodItem.NUMStartBAL;
                }
                else
                {
                    firstPeriodItem = Calculate(InputType, Money, user.AccountId, firstPeriodItem, ts);
                }
                firstPeriodItem.ECredit_Debit = firstPeriodItem.SCredit_Debit;
                firstPeriodItem.BWBEndBAL = firstPeriodItem.BWBStartBAL;
                firstPeriodItem.NUMEndBAL = firstPeriodItem.NUMStartBAL;
                firstPeriodItem.YBEndBAL = firstPeriodItem.YBStartBAL;
                string sql = @"UPDATE TKS_FAS_FGLBalance
                           SET  
                                NUMStartBAL=@NUMStartBAL,
                                BWBStartBAL=@BWBStartBAL,
                                YBStartBAL=@YBStartBAL,
                                NUMDebitTotal_Y=@NUMDebitTotal_Y,
                                BWBDebitTotal_Y=@BWBDebitTotal_Y,
                                YBDebitTotal_Y=@YBDebitTotal_Y,
                                NUMCreditTotal_Y=@NUMCreditTotal_Y,
                                BWBCreditTotal_Y=@BWBCreditTotal_Y,
                                YBCreditTotal_Y=@YBCreditTotal_Y,
                                YearStartNumBAL=@YearStartNumBAL,
                                YearStartYBBAL=@YearStartYBBAL,
                                YearStartBWBBAL=@YearStartBWBBAL,
                                ECredit_Debit = @ECredit_Debit,
                                BWBEndBAL = @BWBEndBAL,
                                NUMEndBAL = @NUMEndBAL,
                                YBEndBAL = @YBEndBAL
                           WHERE
                            id=@Id
                           ";
                cnn.Execute(sql, firstPeriodItem, ts);
                var currentPeriod = firstPeriod;
                foreach (var nextPeriod in nextPeriods.Where(p => p.Id != firstPeriod.Id).ToList())
                {
                    balanceBLL.FGLBanceNextPeriodUpdate(nextPeriod, currentPeriod, ts);
                    currentPeriod = nextPeriod;
                }



                #endregion
            }
        }
        public void BALImport(DataTable dt, string token)
        {
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    //AccountSetBLL accountSetBLL = new AccountSetBLL(cnn);
                    BalanceBLL balanceBLL = new BalanceBLL(cnn);
                    PeriodBLL periodBLL = new PeriodBLL(cnn);
                    var user = this.UserInfoGet(token, ts);
                    //var firstPeriod = accountSetBLL.GetFirstPeriod(user.AccountId, ts);
                    string sql = string.Empty;

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var rowItem = dt.Rows[i];
                        var item = cnn.QueryFirstOrDefault<TKS_FAS_GLBalance>(
                           @"select * from TKS_FAS_GLBalance where id=@Id",
                           new { Id = rowItem["Id"].ToString() }, ts);

                        if (item != null)
                        {
                            if (item.IsLeaf == 0)
                            {
                                continue;
                            }
                            sql = @"UPDATE TKS_FAS_GLBalance
                           SET  
                                NUMStartBAL=@NUMStartBAL,
                                BWBStartBAL=@BWBStartBAL,
                                YBStartBAL=@YBStartBAL,
                                NUMDebitTotal_Y=@NUMDebitTotal_Y,
                                BWBDebitTotal_Y=@BWBDebitTotal_Y,
                                YBDebitTotal_Y=@YBDebitTotal_Y,
                                NUMCreditTotal_Y=@NUMCreditTotal_Y,
                                BWBCreditTotal_Y=@BWBCreditTotal_Y,
                                YBCreditTotal_Y=@YBCreditTotal_Y,
                                YearStartNumBAL=@YearStartNumBAL,
                                YearStartYBBAL=@YearStartYBBAL,
                                YearStartBWBBAL=@YearStartBWBBAL
                           WHERE
                            id=@Id
                           ";
                            var m1 = rowItem["期初余额"].ToString();
                            var m2 = rowItem["借方累计"].ToString();
                            var m3 = rowItem["贷方累计"].ToString();

                            if (!string.IsNullOrEmpty(m1))
                            {
                                var newData = Calculate("BWBStartBAL", decimal.Parse(m1), user.AccountId, item, ts);
                                cnn.Execute(sql, newData, ts);
                                UpdateToFirstPeriod(1, newData, null, user, decimal.Parse(m1), "BWBStartBAL", ts);
                            }
                            if (!string.IsNullOrEmpty(m2))
                            {
                                var newData = Calculate("BWBDebitTotal_Y", decimal.Parse(m2), user.AccountId, item, ts);
                                cnn.Execute(sql, newData, ts);
                                UpdateToFirstPeriod(1, newData, null, user, decimal.Parse(m2), "BWBDebitTotal_Y", ts);
                            }
                            if (!string.IsNullOrEmpty(m3))
                            {
                                var newData = Calculate("BWBCreditTotal_Y", decimal.Parse(m3), user.AccountId, item, ts);
                                cnn.Execute(sql, newData, ts);
                                UpdateToFirstPeriod(1, newData, null, user, decimal.Parse(m3), "BWBCreditTotal_Y", ts);
                            }



                        }
                        else
                        {
                            var item2 = cnn.QueryFirstOrDefault<TKS_FAS_FGLBalance>(
                           @"select * from TKS_FAS_FGLBalance where id=@Id",
                           new { Id = rowItem["Id"].ToString() }, ts);

                            if (item2 == null)
                            {
                                continue;
                            }
                            sql = @"UPDATE TKS_FAS_FGLBalance
                           SET  
                                NUMStartBAL=@NUMStartBAL,
                                BWBStartBAL=@BWBStartBAL,
                                YBStartBAL=@YBStartBAL,
                                NUMDebitTotal_Y=@NUMDebitTotal_Y,
                                BWBDebitTotal_Y=@BWBDebitTotal_Y,
                                YBDebitTotal_Y=@YBDebitTotal_Y,
                                NUMCreditTotal_Y=@NUMCreditTotal_Y,
                                BWBCreditTotal_Y=@BWBCreditTotal_Y,
                                YBCreditTotal_Y=@YBCreditTotal_Y,
                                YearStartNumBAL=@YearStartNumBAL,
                                YearStartYBBAL=@YearStartYBBAL,
                                YearStartBWBBAL=@YearStartBWBBAL
                           WHERE
                            id=@Id
                           ";
                            var m1 = rowItem["期初余额"].ToString();
                            var m2 = rowItem["借方累计"].ToString();
                            var m3 = rowItem["贷方累计"].ToString();
                            if (!string.IsNullOrEmpty(m1))
                            {
                                var newData = Calculate("BWBStartBAL", decimal.Parse(m1), user.AccountId, item2, ts);
                                cnn.Execute(sql, newData, ts);
                                UpdateToFirstPeriod(2, null, newData, user, decimal.Parse(m1), "BWBStartBAL", ts);
                            }
                            if (!string.IsNullOrEmpty(m2))
                            {
                                var newData = Calculate("BWBDebitTotal_Y", decimal.Parse(m2), user.AccountId, item2, ts);
                                cnn.Execute(sql, newData, ts);
                                UpdateToFirstPeriod(2, null, newData, user, decimal.Parse(m2), "BWBDebitTotal_Y", ts);
                            }
                            if (!string.IsNullOrEmpty(m3))
                            {
                                var newData = Calculate("BWBCreditTotal_Y", decimal.Parse(m3), user.AccountId, item2, ts);
                                cnn.Execute(sql, newData, ts);
                                UpdateToFirstPeriod(2, null, newData, user, decimal.Parse(m3), "BWBCreditTotal_Y", ts);
                            }

                        }
                    }
                    ts.Commit();
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw ex;
                }

            }
        }

        public void FixedAssetsImport(DataTable dt, string token)
        {
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(token, ts);
                    AccountSetBLL account = new AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);

                    BalanceBLL balanceBLL = new BalanceBLL(cnn);
                    PeriodBLL periodBLL = new PeriodBLL(cnn);

                    //var firstPeriod = accountSetBLL.GetFirstPeriod(user.AccountId, ts);
                    string sql = string.Empty;

                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var rowItem = dt.Rows[i];
                        #region 判断
                        //string NowPeriod = period.Year + "-" + period.Month;
                        DateTime StartUseDate = DateTime.Parse(rowItem["StartUseDate"].ToString());
                        if (StartUseDate.Year > period.Year)
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行开始使用日期不能大于当前期间");
                        }
                        else
                        {
                            if (StartUseDate.Year == period.Year && StartUseDate.Month > period.Month)
                            {
                                throw new NormalException("第" + (i + 1).ToString() + "行开始使用日期不能大于当前期间");
                            }
                        }
                        //DateTime NowPeriodDate = DateTime.Parse(NowPeriod);
                        //if (DateTime.Compare(StartUseDate, NowPeriodDate) < 0) //判断日期大小
                        //{
                        //    throw new NormalException("第" + (i + 1).ToString() + "行开始使用日期不能大于当前期间");
                        //}

                        if (string.IsNullOrEmpty(rowItem["DocNo"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行资产编号不能为空");

                        }
                        if (string.IsNullOrEmpty(rowItem["AssetsClass"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行资产类别不能为空");

                        }
                        if (string.IsNullOrEmpty(rowItem["Name"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行资产名称不能为空");

                        }
                        if (string.IsNullOrEmpty(rowItem["StartUseDate"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行开始使用日期不能为空");

                        }
                        try
                        {
                            DateTime t = DateTime.Parse(rowItem["StartUseDate"].ToString());
                        }
                        catch (Exception)
                        {

                            throw new NormalException("第" + (i + 1).ToString() + "行开始使用日期格式不正确");
                        }
                        if (string.IsNullOrEmpty(rowItem["AddType"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行增加方式不能为空");

                        }
                        if (string.IsNullOrEmpty(rowItem["DepreciationMethod"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行折旧方法不能为空");

                        }
                        if (string.IsNullOrEmpty(rowItem["IsStartPeriodDepreciation"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行录入当期是否折旧不能为空");

                        }

                        //if (string.IsNullOrEmpty(rowItem["AssetImpairmentSubjectCode"].ToString()))
                        //{
                        //    throw new NormalException("第" + (i + 1).ToString() + "行资产清理科目不能为空");

                        //}
                        //if (string.IsNullOrEmpty(rowItem["AssetImpairmentSubjectCode"].ToString()))
                        //{
                        //    throw new NormalException("第" + (i + 1).ToString() + "行资产清理科目编号不能为空");

                        //}
                        //if (string.IsNullOrEmpty(rowItem["AssetImpairmentSubjectName"].ToString()))
                        //{
                        //    throw new NormalException("第" + (i + 1).ToString() + "行资产清理科目不能为空");

                        //}
                        if (string.IsNullOrEmpty(rowItem["ADSubjectCode"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行累计折旧科目编号不能为空");

                        }
                        //if (string.IsNullOrEmpty(rowItem["ADSubjectName"].ToString()))
                        //{
                        //    throw new NormalException("第" + (i + 1).ToString() + "行累计折旧科目不能为空");

                        //}
                        if (string.IsNullOrEmpty(rowItem["DCostSubjectCode"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行折旧费用科目编号不能为空");

                        }
                        //if (string.IsNullOrEmpty(rowItem["DCostSubjectName"].ToString()))
                        //{
                        //    throw new NormalException("第" + (i + 1).ToString() + "行折旧费用科目不能为空");

                        //}

                        if (string.IsNullOrEmpty(rowItem["PreUseMonth"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行预计使用月份不能为空");

                        }
                        try
                        {
                            decimal PreUseMonth = decimal.Parse(rowItem["PreUseMonth"].ToString());
                        }
                        catch (Exception)
                        {

                            throw new NormalException("第" + (i + 1).ToString() + "行预计使用月份格式不正确");
                        }
                        if (string.IsNullOrEmpty(rowItem["GDCode"].ToString()))
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行固定资产科目编号不能为空");

                        }
                        //if (string.IsNullOrEmpty(rowItem["GDName"].ToString()))
                        //{
                        //    throw new NormalException("第" + (i + 1).ToString() + "行固定资产科目不能为空");

                        //}
                        #endregion

                        TKS_FAS_FixedAssets fix = new TKS_FAS_FixedAssets();
                        fix.Id = Guid.NewGuid().ToString("N");
                        fix.AccountId = user.AccountId;
                        fix.StartPeriod = period.Id;//当前期间为录入期间
                        fix.CreateUser = user.User.UserName;
                        fix.CreateDate = DateTime.Now;
                        fix.DocNo = rowItem["DocNo"].ToString();
                        fix.Name = rowItem["Name"].ToString();
                        fix.AddType = rowItem["AddType"].ToString();

                        fix.AssetsClass = rowItem["AssetsClass"].ToString();
                        fix.AssetsClass = fix.AssetsClass.Length > 3 ? fix.AssetsClass.Substring(0, 3) : "";
                        fix.SpecificationType = rowItem["SpecificationType"].ToString();

                        fix.StartUseDate = DateTime.Parse(rowItem["StartUseDate"].ToString());
                        fix.UseDeptName = rowItem["UseDeptName"].ToString();
                        fix.Supplier = rowItem["Supplier"].ToString();
                        fix.DepreciationMethod = rowItem["DepreciationMethod"].ToString() == "平均年限法" ? "1" : "2";
                        fix.IsStartPeriodDepreciation = rowItem["IsStartPeriodDepreciation"].ToString() == "否" ? 0 : 1;

                        fix.AssetImpairmentSubjectCode = "1606";
                        fix.AssetImpairmentSubjectName = "固定资产清理";


                        fix.ADSubjectCode = rowItem["ADSubjectCode"].ToString();
                        sql = @"select * from TKS_FAS_AccountSubject where accountId=@AccountId and code=@SubjectCode";
                        var data_AD = cnn.QueryFirstOrDefault<TKS_FAS_AccountSubject>(sql,
                            new { AccountId = user.AccountId, SubjectCode = fix.ADSubjectCode }, ts);
                        if (data_AD == null)
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行累计折旧科目编号不存在");
                        }
                        fix.ADSubjectName = data_AD.Code + " " + data_AD.ShowTitle;


                        fix.DCostSubjectCode = rowItem["DCostSubjectCode"].ToString();
                        sql = @"select * from TKS_FAS_AccountSubject where accountId=@AccountId and code=@SubjectCode";
                        var data = cnn.QueryFirstOrDefault<TKS_FAS_AccountSubject>(sql,
                            new { AccountId = user.AccountId, SubjectCode = fix.DCostSubjectCode }, ts);
                        if (data == null)
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行折旧费用科目编号不存在");
                        }

                        fix.DCostSubjectName = data.Code + " " + data.ShowTitle;

                        fix.InitialAssetValue = decimal.Parse(rowItem["InitialAssetValue"].ToString());
                        fix.ScrapValueRate = 0;
                        if (!string.IsNullOrEmpty(rowItem["ScrapValueRate"].ToString()))
                        {
                            try
                            {
                                var rate = rowItem["ScrapValueRate"].ToString();
                                rate = rate.Replace("%", "");
                                fix.ScrapValueRate = decimal.Parse(rate);
                            }
                            catch (Exception)
                            {

                                throw new NormalException("第" + (i + 1).ToString() + "行残值率格式不正确");
                            }
                        }
                        fix.ScrapValue = 0;//预计残值
                        if (fix.ScrapValueRate > 0)
                        {
                            fix.ScrapValue = fix.InitialAssetValue * fix.ScrapValueRate / 100;
                        }
                        fix.PreUseMonth = int.Parse(rowItem["PreUseMonth"].ToString());
                        fix.DpreMonth = 0;
                        if (!string.IsNullOrEmpty(rowItem["AccumulativeDpre"].ToString()))
                        {
                            try
                            {
                                fix.DpreMonth = int.Parse(rowItem["DpreMonth"].ToString());
                            }
                            catch (Exception)
                            {

                                throw new NormalException("第" + (i + 1).ToString() + "行已折旧月份格式不正确");
                            }
                        }

                        fix.RemainderUseMonth = fix.PreUseMonth - fix.DpreMonth;
                        fix.AccumulativeDpre = 0;
                        if (!string.IsNullOrEmpty(rowItem["AccumulativeDpre"].ToString()))
                        {
                            try
                            {
                                fix.AccumulativeDpre = decimal.Parse(rowItem["AccumulativeDpre"].ToString());
                            }
                            catch (Exception)
                            {

                                throw new NormalException("第" + (i + 1).ToString() + "行累计折旧格式不正确");
                            }
                        }
                        fix.AccumulativeDpre_Y = 0;
                        if (!string.IsNullOrEmpty(rowItem["AccumulativeDpre_Y"].ToString()))
                        {
                            try
                            {
                                fix.AccumulativeDpre_Y = decimal.Parse(rowItem["AccumulativeDpre_Y"].ToString());
                            }
                            catch (Exception)
                            {

                                throw new NormalException("第" + (i + 1).ToString() + "行本年累计折旧格式不正确");
                            }
                        }
                        fix.PreviousAccumulativeDpre = fix.AccumulativeDpre - fix.AccumulativeDpre_Y;
                        fix.DprePerMonth = 0;
                        if (fix.RemainderUseMonth != 0)
                        {
                            fix.DprePerMonth = (fix.InitialAssetValue - fix.ScrapValue - fix.AccumulativeDpre) / fix.RemainderUseMonth;
                        }
                        fix.IsGenPZ = 0;
                        fix.Memo = rowItem["Memo"].ToString();
                        fix.GDCode = rowItem["GDCode"].ToString();
                        sql = @"select * from TKS_FAS_AccountSubject where accountId=@AccountId and code=@SubjectCode";
                        data = cnn.QueryFirstOrDefault<TKS_FAS_AccountSubject>(sql,
                            new { AccountId = user.AccountId, SubjectCode = fix.GDCode }, ts);
                        if (data == null)
                        {
                            throw new NormalException("第" + (i + 1).ToString() + "行固定资产科目编号不存在");
                        }

                        fix.GDName = data.Code + " " + data.ShowTitle;
                        fix.InputVAT = 0;
                        if (!string.IsNullOrEmpty(rowItem["InputVAT"].ToString()))
                        {
                            try
                            {
                                fix.InputVAT = decimal.Parse(rowItem["InputVAT"].ToString());
                            }
                            catch (Exception)
                            {

                                throw new NormalException("第" + (i + 1).ToString() + "行进项税额格式不正确");
                            }
                        }
                        if (fix.AddType != "期初")
                        {
                            AddChange(fix, ts);
                        }
                        cnn.Insert<TKS_FAS_FixedAssets>(fix, ts);
                    }
                    ts.Commit();
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw ex;
                }

            }
        }

        public void FPImport(DataTable dt, string token)
        {
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(token, ts);
                    AccountSetBLL account = new AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);

                    BalanceBLL balanceBLL = new BalanceBLL(cnn);
                    PeriodBLL periodBLL = new PeriodBLL(cnn);

                    var fpData = cnn.Query<TKS_FAS_DATA>(
                        @"select * from TKS_FAS_DATA where parentId='001'", null, ts).ToList();

                    var ynData = cnn.Query<TKS_FAS_DATA>(
                        @"select * from TKS_FAS_DATA where parentId='200'", null, ts).ToList();

                    var zfData = cnn.Query<TKS_FAS_DATA>(
                        @"select * from TKS_FAS_DATA where parentId='100'", null, ts).ToList();
                    var rpData = cnn.Query<TKS_FAS_DATA>(
                        @"select * from TKS_FAS_DATA where parentId='300'", null, ts).ToList();
                    //var firstPeriod = accountSetBLL.GetFirstPeriod(user.AccountId, ts);
                    string sql = string.Empty;
                    if (dt.Rows.Count > 0)
                    {
                        DataTable dtAllInvoice = dt.DefaultView.ToTable(true, new string[] { "InvoiceNo" });
                        for (var i = 0; i < dtAllInvoice.Rows.Count; i++)
                        {

                            string InvoiceNo = dtAllInvoice.Rows[i][0].ToString();
                            DataRow rowInvoice = dt.Select("InvoiceNo='" + InvoiceNo + "'").Distinct().ToArray()[0];
                            string InvoiceDate = rowInvoice["InvoiceDate"].ToString();
                            string Memo = rowInvoice["Memo"].ToString();
                            string Type = rowInvoice["Type"].ToString();
                            string IsVAT = rowInvoice["IsVAT"].ToString();
                            string RPStatus = rowInvoice["RPStatus"].ToString();
                            string PayMode = rowInvoice["PayMode"].ToString();
                            string Money = rowInvoice["Money"].ToString();
                            string TaxMoney = rowInvoice["TaxMoney"].ToString();

                            string SF_Money = rowInvoice["SF_Money"].ToString();
                            string SFType = rowInvoice["SFType"].ToString();
                            string BasicDataName = rowInvoice["BasicDataName"].ToString();
                            string SFDate = rowInvoice["SFDate"].ToString();
                            string SFStatus = rowInvoice["SFStatus"].ToString();
                            string SFMoney = rowInvoice["SFMoney"].ToString();
                            string SFRemark = rowInvoice["SFRemark"].ToString();
                            TKS_FAS_Invoice invoice = new TKS_FAS_Invoice();
                            invoice.Id = Guid.NewGuid().ToString("N");
                            invoice.AccountId = user.AccountId;
                            invoice.CreateUser = user.User.TrueName;
                            invoice.CreateDate = DateTime.Now;
                            invoice.Memo = Memo;
                            invoice.InvoiceNo = InvoiceNo;
                            var InvoiceInfo = cnn.Query<TKS_FAS_Invoice>(
                 @"select * from TKS_FAS_Invoice where InvoiceNo='" + InvoiceNo + "' and AccountId='" + user.AccountId + "'", null, ts).ToList();
                            if (InvoiceInfo.Count > 0)
                            {
                                throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "已经存在，请核对");
                            }
                            if (string.IsNullOrEmpty(InvoiceNo))
                            {
                                throw new NormalException("第" + (i + 1).ToString() + "张发票发票号码不能为空");
                            }
                            try
                            {
                                DateTime t = DateTime.Parse(InvoiceDate);
                            }
                            catch (Exception)
                            {

                                throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "发票日期格式不正确");
                            }
                            invoice.InvoiceDate = InvoiceDate;
                            invoice.InvoiceNum = 0;
                            invoice.Type = -1;
                            invoice.IsVAT = -1;
                            invoice.RPStatus = -1;
                            invoice.PayMode = -1;
                            invoice.Money = 0;
                            invoice.TaxMoney = 0;
                            invoice.IsTaxYZ = "0";
                            invoice.IsUse = "0";
                            if (!string.IsNullOrEmpty(Type) || !string.IsNullOrEmpty(IsVAT) || !string.IsNullOrEmpty(RPStatus) || !string.IsNullOrEmpty(PayMode) || !string.IsNullOrEmpty(Money) || !string.IsNullOrEmpty(TaxMoney))
                            {
                                //启用税金预知
                                invoice.IsTaxYZ = "1";
                                if (string.IsNullOrEmpty(Money))
                                {
                                    throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "含税金额不能为空");
                                }
                                else
                                {
                                    try
                                    {
                                        invoice.Money = decimal.Parse(Money);
                                    }
                                    catch (Exception)
                                    {

                                        throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "含税金额格式不正确");
                                    }

                                }
                                if (string.IsNullOrEmpty(Type))
                                {
                                    throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "业务类型不能为空");
                                }
                                else
                                {
                                    var lstType = fpData.Where(p => p.Name == Type).ToList();
                                    if (lstType.Count != 1)
                                    {
                                        throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "业务类型不存在");
                                    }
                                    else
                                    {
                                        invoice.Type = int.Parse(lstType[0].Code);
                                    }
                                }
                                if (string.IsNullOrEmpty(IsVAT))
                                {
                                    throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "增值税专用发票不能为空");
                                }
                                else
                                {
                                    var lstIsVAT = ynData.Where(p => p.Name == IsVAT).ToList();
                                    if (lstIsVAT.Count != 1)
                                    {
                                        throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "增值税专用发票不存在");
                                    }
                                    else
                                    {
                                        invoice.IsVAT = int.Parse(lstIsVAT[0].Code);
                                        if (invoice.IsVAT == 1)
                                        {
                                            if (string.IsNullOrEmpty(TaxMoney))
                                            {
                                                throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "税金金额不能为空");
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    invoice.TaxMoney = decimal.Parse(TaxMoney);
                                                }
                                                catch (Exception)
                                                {

                                                    throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "税金金额格式不正确");
                                                }

                                            }
                                        }
                                    }
                                }
                                if (string.IsNullOrEmpty(RPStatus))
                                {
                                    throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "收付状态不能为空");
                                }
                                else
                                {
                                    var lstRPStatus = rpData.Where(p => p.Name == RPStatus).ToList();
                                    if (lstRPStatus.Count != 1)
                                    {
                                        throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "收付状态不存在");
                                    }
                                    else
                                    {
                                        invoice.RPStatus = int.Parse(lstRPStatus[0].Code);
                                        if (invoice.RPStatus == 1)
                                        {
                                            //支付方式
                                            if (string.IsNullOrEmpty(PayMode))
                                            {
                                                throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "支付方式不能为空");
                                            }
                                            else
                                            {
                                                var lstPayMode = zfData.Where(p => p.Name == PayMode).ToList();
                                                if (lstPayMode.Count != 1)
                                                {
                                                    throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "支付方式不存在");
                                                }
                                                else
                                                {
                                                    invoice.PayMode = int.Parse(lstPayMode[0].Code);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(SF_Money) || !string.IsNullOrEmpty(SFType) || !string.IsNullOrEmpty(BasicDataName) || !string.IsNullOrEmpty(SFDate) || !string.IsNullOrEmpty(SFStatus) || !string.IsNullOrEmpty(SFMoney) || !string.IsNullOrEmpty(SFRemark))
                            {
                                //启用应收应付与到期提醒
                                invoice.IsUse = "1";

                                if (string.IsNullOrEmpty(SF_Money))
                                {
                                    throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "含税金额不能为空");
                                }
                                else
                                {
                                    if (SF_Money != Money && !string.IsNullOrEmpty(Money))
                                    {
                                        throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "税金预知中的含税金额与应收应付中的含税金额不一致");
                                    }
                                    try
                                    {
                                        invoice.Money = decimal.Parse(SF_Money);
                                    }
                                    catch (Exception)
                                    {

                                        throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "含税金额格式不正确");
                                    }

                                }
                                if (string.IsNullOrEmpty(SFType))
                                {
                                    throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "收付类型不能为空");
                                }
                                else
                                {
                                    invoice.SFType = SFType;
                                    var BasicData = cnn.Query<TKS_FAS_BasicData>(
                  @"select * from TKS_FAS_BasicData where DataType='Vendor' and UserId='" + user.User.Id + "' and Name='" + BasicDataName + "'", null, ts).ToList();
                                    if (SFType == "应收")
                                    {
                                        //客户
                                        BasicData = cnn.Query<TKS_FAS_BasicData>(
                  @"select * from TKS_FAS_BasicData where DataType='Customer' and UserId='" + user.User.Id + "' and Name='" + BasicDataName + "'", null, ts).ToList();
                                        if (BasicData.Count <= 0)
                                        {
                                            throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "客户不存在");
                                        }
                                        else
                                        {
                                            invoice.BasicDataId = BasicData[0].Id;
                                        }
                                    }
                                    else if (SFType == "应付")
                                    {
                                        //供应商
                                        if (BasicData.Count <= 0)
                                        {
                                            throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "供应商不存在");
                                        }
                                        else
                                        {
                                            invoice.BasicDataId = BasicData[0].Id;
                                        }
                                    }

                                }
                            }

                            DataRow[] rowDetail = dt.Select("InvoiceNo='" + InvoiceNo + "'").Distinct().ToArray();
                            decimal sumSFMoney = 0;
                            for (var j = 0; j < rowDetail.Count(); j++)
                            {
                                DataRow dr = rowDetail[j];
                                string det_SF_Money = dr["SF_Money"].ToString();
                                //string SFType = dr["SFType"].ToString();
                                //string BasicDataName = dr["BasicDataName"].ToString();
                                string det_SFDate = dr["SFDate"].ToString();
                                string det_SFStatus = dr["SFStatus"].ToString();
                                string det_SFMoney = dr["SFMoney"].ToString();
                                string det_SFRemark = dr["SFRemark"].ToString();
                                if (invoice.IsUse == "1")
                                {
                                    //启用应收应付与到期提醒
                                    TKS_FAS_InvoiceSFDetail SF = new TKS_FAS_InvoiceSFDetail();
                                    SF.Id = Guid.NewGuid().ToString("N");
                                    SF.InvoiceId = invoice.Id;
                                    SF.SFRemark = det_SFRemark;
                                    SF.Seq = j + 1;
                                    SF.Status = "OP";
                                    SF.CreateUser = user.User.TrueName;
                                    SF.CreateDate = DateTime.Now;
                                    try
                                    {
                                        DateTime t = DateTime.Parse(det_SFDate);
                                        SF.SFDate = det_SFDate.Replace('/', '-');
                                    }
                                    catch (Exception)
                                    {
                                        throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "收付日期格式不正确");
                                    }
                                    if (string.IsNullOrEmpty(det_SFMoney))
                                    {
                                        throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "收付金额不能为空");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            SF.SFMoney = decimal.Parse(det_SFMoney);
                                            sumSFMoney += SF.SFMoney;
                                        }
                                        catch (Exception)
                                        {

                                            throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "收付金额格式不正确");
                                        }

                                    }
                                    if (string.IsNullOrEmpty(det_SFStatus))
                                    {
                                        throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "细项收付状态不能为空");
                                    }
                                    else
                                    {
                                        if (SFType == "应收")
                                        {
                                            if (det_SFStatus != "已收" && det_SFStatus != "未收")
                                            {
                                                throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "细项收付状态填写有误");
                                            }
                                        }
                                        else if (SFType == "应付")
                                        {
                                            if (det_SFStatus != "已付" && det_SFStatus != "未付")
                                            {
                                                throw new NormalException("第" + (i + 1).ToString() + "张发票" + InvoiceNo + "细项收付状态填写有误");
                                            }
                                        }
                                        SF.SFStatus = det_SFStatus;
                                    }


                                    cnn.Insert<TKS_FAS_InvoiceSFDetail>(SF, ts);
                                }
                            }
                            invoice.BadMoney = invoice.Money - sumSFMoney;
                            cnn.Insert<TKS_FAS_Invoice>(invoice, ts);
                        }
                    }

                    ts.Commit();
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw ex;
                }

            }
        }

        public FileInfo getExcelBySql(RequestExcel request)
        {

            string columnName = "";
            string columnTitle = "";
            string sql = @"SELECT A.InvoiceDate AS 发票日期, A.InvoiceNo AS 发票号码, A.Memo AS 备注
	        , CASE 
		        WHEN A.IsTaxYZ = '1' THEN fp.Name
		        ELSE ''
	        END AS 业务类型
	        , CASE 
		        WHEN A.IsTaxYZ = '1' THEN yn.Name
		        ELSE ''
	        END AS 增值税专用发票
	        , CASE 
		        WHEN A.IsTaxYZ = '1' THEN rp.Name
		        ELSE ''
	        END AS 收付状态
	        , CASE 
		        WHEN A.IsTaxYZ = '1' THEN zf.Name
		        ELSE ''
	        END AS 支付方式
	        , CASE 
		        WHEN A.IsTaxYZ = '1' THEN A.Money
		        ELSE 0
	        END AS 含税金额
	        , CASE 
		        WHEN A.IsTaxYZ = '1' THEN A.TaxMoney
		        ELSE 0
	        END AS 税金金额
	        , CASE 
		        WHEN A.IsUse = '1' THEN A.Money
		        ELSE 0
	        END AS 发票金额
	        , CASE 
		        WHEN A.IsUse = '1' THEN A.SFType
		        ELSE ''
	        END AS 收付类型
	        , CASE 
		        WHEN A.IsUse = '1' THEN B.Name
		        ELSE ''
	        END AS 供应商客户
	        , CASE 
		        WHEN A.IsUse = '1' THEN det.SFDate
		        ELSE ''
	        END AS 收付日期
	        , CASE 
		        WHEN A.IsUse = '1' THEN det.SFMoney
		        ELSE 0
	        END AS 收付金额
	        , CASE 
		        WHEN A.IsUse = '1' THEN det.SFStatus
		        ELSE ''
	        END AS 细项收付状态
	        , CASE 
		        WHEN A.IsUse = '1' THEN det.SFRemark
		        ELSE ''
	        END AS 细项备注
	        , CASE 
		        WHEN A.IsUse = '1' THEN A.Money - s_det.SFMoney
		        ELSE 0
	        END AS 坏账金额
        FROM TKS_FAS_Invoice A
	        LEFT JOIN TKS_FAS_DATA fp
	        ON fp.code = A.Type
		        AND fp.ParentId = '001'
	        LEFT JOIN TKS_FAS_DATA yn
	        ON yn.code = A.Type
		        AND yn.ParentId = '200'
	        LEFT JOIN TKS_FAS_DATA zf
	        ON zf.code = A.Type
		        AND zf.ParentId = '100'
	        LEFT JOIN TKS_FAS_DATA rp
	        ON rp.code = A.Type
		        AND rp.ParentId = '300'
	        LEFT JOIN TKS_FAS_BasicData B ON B.Id = A.BasicDataId
	        LEFT JOIN TKS_FAS_InvoiceSFDetail det ON det.InvoiceId = A.Id
	        LEFT JOIN (
		        SELECT InvoiceId, SUM(SFMoney) AS SFMoney
		        FROM TKS_FAS_InvoiceSFDetail
		        GROUP BY InvoiceId
	        ) s_det
	        ON s_det.InvoiceId = A.Id
        WHERE A.AccountId = '{0}'";
            DataTable table = getDT(sql, request.Token);
            foreach (DataColumn dc in table.Columns)
            {
                columnName = columnName + dc.Caption + ",";

                //columnTitle = columnTitle + getLabel(dc.Caption) + ",";
                columnTitle = columnTitle + dc.Caption + ",";
            }
            if (columnName.EndsWith(","))
            {
                columnName = columnName.Substring(0, columnName.Length - 1);
            }

            if (columnTitle.EndsWith(","))
            {
                columnTitle = columnTitle.Substring(0, columnTitle.Length - 1);
            }
            string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel");
            filepath += "\\发票导出" + DateTime.Now.ToString("yyyy-MM-dd") + Guid.NewGuid().ToString() + ".xls";
            //string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\";
            //filepath = filepath + getLabel(name) + "-" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".xls";
            WriteToFile(table, request.Name, request.Name, columnName.Split(','), columnTitle.Split(','), filepath);
            FileInfo d = new FileInfo(filepath);

            return d;
        }

        public DataTable getDT(string sql, string token)
        {
            DataTable table = new DataTable();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                var user = this.UserInfoGet(token, ts);
                var reader = cnn.ExecuteReader(string.Format(sql, user.AccountId), null, ts);
                table.Load(reader);
                ts.Commit();
            }

            return table;
        }

        public static string WriteToFile(DataTable table, string headerText, string sheetName, string[] columnName, string[] columnTitle, string filepath)
        {
            //Write the stream data of workbook to the root directory
            string strFields = "";


            foreach (DataColumn dc in table.Columns)
            {
                strFields = strFields + dc.Caption + ",";
            }
            if (strFields.EndsWith(","))
            {
                strFields = strFields.Substring(0, strFields.Length - 1);
            }
            XSSFWorkbook hssfworkbook = GenerateData(table, headerText, sheetName, columnName, columnTitle);
            FileInfo fi = new FileInfo(filepath);
            if (!fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            FileStream file = fi.OpenWrite();
            hssfworkbook.Write(file);
            if (fi.Exists)
            {
                // fi.Delete();
            }
            file.Close();

            return filepath;
        }
        public static XSSFWorkbook GenerateData(DataTable table, string headerText, string sheetName, string[] columnName, string[] columnTitle)
        {
            XSSFWorkbook hssfworkbook = new XSSFWorkbook();
            ISheet sheet = hssfworkbook.CreateSheet(sheetName);

            #region 设置文件属性信息

            //创建一个文档摘要信息实体。
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = ""; //公司名称 

            //创建一个摘要信息实体。
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "";
            si.Author = " ";
            si.Title = headerText;
            si.Subject = headerText;
            si.CreateDateTime = DateTime.Now;

            #endregion

            ICellStyle dateStyle = hssfworkbook.CreateCellStyle();
            IDataFormat format = hssfworkbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            IFont fontdate = hssfworkbook.CreateFont();
            fontdate.FontHeightInPoints = 10;
            fontdate.FontName = "Arial";
            dateStyle.SetFont(fontdate);
            #region 取得列宽

            int[] colWidth = new int[columnName.Length];
            for (int i = 0; i < columnName.Length; i++)
            {
                colWidth[i] = Encoding.GetEncoding(936).GetBytes(columnTitle[i]).Length;
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < columnName.Length; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(table.Rows[i][columnName[j]].ToString()).Length;
                    if (intTemp > colWidth[j])
                    {
                        colWidth[j] = intTemp;
                    }
                }
            }

            #endregion

            int rowIndex = 0;
            foreach (DataRow row in table.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = hssfworkbook.CreateSheet(sheetName + ((int)rowIndex / 65535).ToString());
                    }

                    #region 表头及样式
                    //if (!string.IsNullOrEmpty(headerText))
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(headerText);

                        ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = hssfworkbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        font.FontName = "Arial";
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1)); 
                        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
                    }
                    #endregion

                    #region 列头及样式
                    {
                        //HSSFRow headerRow = sheet.CreateRow(1); 
                        IRow headerRow;
                        //if (!string.IsNullOrEmpty(headerText))
                        //{
                        //    headerRow = sheet.CreateRow(0);
                        //}
                        //else
                        //{
                        headerRow = sheet.CreateRow(1);
                        //}
                        ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        IFont font = hssfworkbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        font.FontName = "Arial";
                        headStyle.SetFont(font);

                        for (int i = 0; i < columnName.Length; i++)
                        {
                            headerRow.CreateCell(i).SetCellValue(columnTitle[i]);
                            headerRow.GetCell(i).CellStyle = headStyle;
                            //设置列宽 
                            if ((colWidth[i] + 1) * 256 > 30000)
                            {
                                sheet.SetColumnWidth(i, 10000);
                            }
                            else
                            {
                                sheet.SetColumnWidth(i, (colWidth[i] + 1) * 256);
                            }
                        }
                        /* 
                        foreach (DataColumn column in dtSource.Columns) 
                        { 
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName); 
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle; 
   
                            //设置列宽    
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256); 
                        } 
                         * */
                    }
                    #endregion
                    //if (!string.IsNullOrEmpty(headerText))
                    //{
                    //    rowIndex = 1;
                    //}
                    //else
                    //{
                    rowIndex = 2;
                    //}

                }
                #endregion

                #region 填充数据
                ICellStyle dataStyle = hssfworkbook.CreateCellStyle();

                IFont fontdata = hssfworkbook.CreateFont();
                fontdata.FontHeightInPoints = 10;
                fontdata.FontName = "Arial";
                dataStyle.SetFont(fontdata);
                IRow dataRow = sheet.CreateRow(rowIndex);
                for (int i = 0; i < columnName.Length; i++)
                {
                    ICell newCell = dataRow.CreateCell(i);
                    newCell.CellStyle = dataStyle;
                    string drValue = row[columnName[i]].ToString();

                    switch (table.Columns[columnName[i]].DataType.ToString())
                    {
                        case "System.String"://字符串类型   
                            if (drValue.ToUpper() == "TRUE")
                                newCell.SetCellValue("是");
                            else if (drValue.ToUpper() == "FALSE")
                                newCell.SetCellValue("否");
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型    
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示    
                            break;
                        case "System.Boolean"://布尔型    
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            if (boolV)
                                newCell.SetCellValue("是");
                            else
                                newCell.SetCellValue("否");
                            break;
                        case "System.Int16"://整型    
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型    
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理    
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }

                #endregion

                rowIndex++;
            }

            return hssfworkbook;
        }
        private void AddChange(TKS_FAS_FixedAssets data, IDbTransaction ts)
        {
            TKS_FAS_FixedAssetsChange change = new TKS_FAS_FixedAssetsChange();
            change.Id = Guid.NewGuid().ToString("N");
            change.ParentId = data.Id;
            change.DocNo = data.DocNo;
            change.PeriodId = data.StartPeriod;
            change.AccountId = data.AccountId;
            change.AssetsName = data.Name;
            change.PreContent = data.InitialAssetValue.ToString();
            change.CurContent = data.InitialAssetValue.ToString();
            change.ChangeType = 1;
            change.CreateUser = data.CreateUser;
            change.CreateDate = data.CreateDate;
            cnn.Insert<TKS_FAS_FixedAssetsChange>(change, ts);
        }
        public ResponseBALDelete BALDelete(RequestBALDelete request)
        {
            ResponseBALDelete response = new ResponseBALDelete();

            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    cnn.Execute("delete from TKS_FAS_FGLBalance where id=@Id",
                        new { Id = request.Id }, ts);
                    #region 期初数据同步到第一期
                    AccountSetBLL accountSetBLL = new AccountSetBLL(cnn);
                    var firstPeriod = accountSetBLL.GetFirstPeriod(user.AccountId, ts);
                    cnn.Execute("delete from TKS_FAS_FGLBalance where PeriodId=@PeriodId and accountId = @AccountId and SubjectCode = @SubjectCode and Name = @Name and IsFirstPeriodAdd=1",
                        new { PeriodId = firstPeriod.Id, AccountId = user.AccountId, SubjectCode = request.SubjectCode, Name = request.SubjectName }, ts);


                    PeriodBLL periodBLL = new PeriodBLL(cnn);
                    BalanceBLL balanceBLL = new BalanceBLL(cnn);
                    var startDte = firstPeriod.StartDate;
                    var nextPeriods = periodBLL.GetNextPeriods(user.AccountId, startDte?.Year ?? 0, startDte?.Month ?? 0, ts);
                    var currentPeriod = firstPeriod;
                    foreach (var nextPeriod in nextPeriods)
                    {
                        balanceBLL.FGLBanceNextPeriodUpdate(nextPeriod, currentPeriod, ts);
                        currentPeriod = nextPeriod;
                    }

                    #endregion
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBALDelete;
                }
            }
        }



        /// <summary>
        /// 设置辅助核算
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="subjectCode"></param>
        /// <param name="ts"></param>
        public void SetCalHelper(TKS_FAS_AccountSubject subject, IDbTransaction ts)
        {
            string sql = @"update TKS_FAS_GLBalance set IsCalHelperValid=1 
                        where accountId=@AccountId and subjectCode=@SubjectCode";
            cnn.Execute(sql, new { AccountId = subject.AccountId, SubjectCode = subject.Code }, ts);

        }

        /// <summary>
        /// 重置辅助核算
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="subjectCode"></param>
        /// <param name="ts"></param>
        public void UnSetCalHelper(TKS_FAS_AccountSubject subject, IDbTransaction ts)
        {
            string sql = @"update TKS_FAS_GLBalance set IsCalHelperValid=0 
                        where accountId=@AccountId and subjectCode=@SubjectCode";
            cnn.Execute(sql, new { AccountId = subject.AccountId, SubjectCode = subject.Code }, ts);

        }

        /// <summary>
        /// 启用数量
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="subjectCode"></param>
        /// <param name="ts"></param>
        public void SetQuantity(TKS_FAS_AccountSubject subject, IDbTransaction ts)
        {
            string sql = @"update TKS_FAS_GLBalance set IsQuantityValid=1,unit=@Unit 
                        where accountId=@AccountId and subjectCode=@SubjectCode";
            cnn.Execute(sql, new { AccountId = subject.AccountId, SubjectCode = subject.Code, Unit = subject.QuantityValue }
            , ts);

        }

        /// <summary>
        /// 重置启用数量
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="subjectCode"></param>
        /// <param name="ts"></param>
        public void UnSetQuantity(TKS_FAS_AccountSubject subject, IDbTransaction ts)
        {
            string sql = @"update TKS_FAS_GLBalance set IsQuantityValid=0 
                        where accountId=@AccountId and subjectCode=@SubjectCode";
            cnn.Execute(sql, new { AccountId = subject.AccountId, SubjectCode = subject.Code }, ts);

        }

        /// <summary>
        /// 设置币别
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="subject"></param>
        /// <param name="currency"></param>
        /// <param name="ts"></param>
        public void SetCurrency(TKS_FAS_AccountSubject subject, List<string> currency, IDbTransaction ts)
        {

            //基于已经存在的科目代码进行操作
            //查询原先的科目，轮询已有的币别

            string sql = @"select * from TKS_FAS_GLBalance where accountId=@AccountId and subjectCode=@SubjectCode";

            var data = cnn.Query<TKS_FAS_GLBalance>(sql,
                new { AccountId = subject.AccountId, SubjectCode = subject.Code }, ts).ToList();

            var existCurrency = data.Select(p => p.CurrencyCode).ToList();
            AccountSetBLL bll = new AccountSetBLL(cnn);
            var period = bll.GetActivePeriod(subject.AccountId, ts);
            TKS_FAS_GLBalance newData;
            for (int i = 0; i < currency.Count; i++)
            {
                if (!existCurrency.Contains(currency[i]))
                {
                    //未包含的币别
                    #region 余额表新增此科目的其它币别
                    newData = new TKS_FAS_GLBalance();
                    newData.Id = Guid.NewGuid().ToString("N");
                    newData.SubjectId = subject.Id;
                    newData.SubjectCode = subject.Code;
                    newData.ParentId = subject.ParentId;
                    newData.PeriodId = period.Id;
                    newData.IsCalHelperValid = subject.IsCalHelperValid;
                    newData.IsQuantityValid = subject.IsQuantityValid;
                    newData.CurrencyCode = currency[i];
                    if (newData.CurrencyCode == "RMB")
                    {
                        newData.IsDefaultCurrency = 1;
                    }
                    else
                    {
                        newData.IsDefaultCurrency = 0;
                    }
                    newData.IsLeaf = subject.IsLeaf;
                    newData.Name = subject.Name;
                    newData.Category = subject.Category;
                    newData.SCredit_Debit = subject.Credit_Debit;
                    newData.Year = period.Year;
                    newData.Unit = subject.QuantityValue;
                    newData.AccountId = subject.AccountId;
                    cnn.Insert<TKS_FAS_GLBalance>(newData, ts);
                    TKS_FAS_GLBalance tData = new TKS_FAS_GLBalance();
                    tData = newData;
                    tData.Id = Guid.NewGuid().ToString("N");
                    tData.PeriodId = "";
                    cnn.Insert<TKS_FAS_GLBalance>(tData, ts);
                    #endregion
                }
            }

            //删除不在币别集合内的
            foreach (var item in data)
            {
                if (!currency.Contains(item.CurrencyCode))
                {
                    //删除
                    cnn.Execute(@"delete from TKS_FAS_GLBalance where id=@Id",
                        new { Id = item.Id }, ts);

                }
            }

        }


        /// <summary>
        /// 币别恢复成本位币
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="subject"></param>
        /// <param name="ts"></param>
        public void UnSetCurrency(TKS_FAS_AccountSubject subject, IDbTransaction ts)
        {
            //基于已经存在的科目代码进行操作
            AccountSetBLL bll = new AccountSetBLL(cnn);
            var period = bll.GetActivePeriod(subject.AccountId, ts);

            //账套-期间-科目代码》
            //string sql = @"delete from TKS_FAS_GLBalance where 
            //            accountId=@AccountId and subjectCode=@SubjectCode and periodId=@PeriodId";
            string sql = @"delete from TKS_FAS_GLBalance where 
                        accountId=@AccountId and subjectCode=@SubjectCode";
            cnn.Execute(sql, new { AccountId = subject.AccountId, SubjectCode = subject.Code, PeriodId = period.Id }, ts);


            TKS_FAS_GLBalance newData;


            #region 余额表默认RMB本位币科目
            newData = new TKS_FAS_GLBalance();
            newData.Id = Guid.NewGuid().ToString("N");
            newData.SubjectId = subject.Id;
            newData.AccountId = subject.AccountId;//账套ID
            newData.SubjectCode = subject.Code;//科目代码
            newData.PeriodId = period.Id;//期间代码
            newData.ParentId = subject.ParentId;

            newData.IsCalHelperValid = subject.IsCalHelperValid;
            newData.IsQuantityValid = subject.IsQuantityValid;
            newData.CurrencyCode = "RMB";

            newData.IsDefaultCurrency = 1;

            newData.IsLeaf = subject.IsLeaf;
            newData.Name = subject.Name;
            newData.Category = subject.Category;
            newData.SCredit_Debit = subject.Credit_Debit;
            newData.Year = period.Year;
            newData.Unit = subject.QuantityValue;

            cnn.Insert<TKS_FAS_GLBalance>(newData, ts);
            TKS_FAS_GLBalance tData = new TKS_FAS_GLBalance();
            tData = newData;
            tData.Id = Guid.NewGuid().ToString("N");
            tData.PeriodId = "";
            cnn.Insert<TKS_FAS_GLBalance>(tData, ts);
            #endregion


        }

    }
}
