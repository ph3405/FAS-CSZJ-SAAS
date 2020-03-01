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
    public class QMBLL : CommonBase
    {
        /// <summary>
        /// 第一步的时候的期末检查
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseQMValidate QMValidate(RequestQMValidate request)
        {
            var res = new ResponseQMValidate();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    AccountSetBLL account = new FAS.AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);
                    ValidateJTZJ(user, period, request, ts);

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
                        List<TKS_FAS_GLBalanceExt> data = cnn.Query<TKS_FAS_GLBalanceExt>(sqlQC,
                      new
                      {
                          AccountId = user.AccountId,//账套
                          Category = 1,//科目类别
                          CurrencyCode = "RMB"//币别
                      }, ts).ToList();
                        decimal val = data.Sum(p => p.BWBStartBAL);
                        if (val != FixedAssets.InitialAssetValue)
                        {
                            throw new NormalException("固定资产原值与期初固定资产不匹配</br>固定资产原值：" + FixedAssets.InitialAssetValue.ToString() + "</br>期初固定资产：" + val.ToString());
                        }
                    }
                    #endregion
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "ok";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseQMValidate;
                }
            }
        }

        /// <summary>
        /// 计提折旧校验
        /// </summary>
        /// <param name="user"></param>
        /// <param name="period"></param>
        /// <param name="request"></param>
        /// <param name="ts"></param>
        private void ValidateJTZJ(MM_UserInfo user, TKS_FAS_MonthPeriodInfo period, RequestQMValidate request, IDbTransaction ts)
        {
            //如果是第一期，期初平衡校验
            AccountSetBLL accountSet = new AccountSetBLL(cnn);

            var firstPeriod = accountSet.GetFirstPeriod(user.AccountId, ts);

            if (firstPeriod.Id == period.Id)
            {
                BALBLL balBLL = new FAS.BALBLL(cnn);
                var res = balBLL.CalculatePH(new Entity.FAS.RequestBALCalculate { Token = request.Token }, ts);
                if (res.IsSuccess == false)
                {
                    throw new NormalException(res.Message);
                }
            }

            //是否有待审核的凭证未审核
            string sql = @"select * from tks_fas_doc where checkStatus=1 and periodId=@PeriodId";
            //逻辑修改：由于可以录入非当前期间的凭证，结转时，需要获取符合条件的所有凭证明细
            var data = cnn.Query(sql, new { PeriodId = period.Id }, ts);
            if (data.Count() > 0)
            {
                throw new NormalException("当前期间还有未审核的凭证，请先审核后再做结转");
            }

            //固定资产新增，以及变更的凭证是否都已经生成
            sql = @"select * from TKS_FAS_FixedAssetsChange where 
                   ( docId is null or docId='') and AccountId=@AccountId  and PeriodId=@PeriodId";

            data = cnn.Query(sql, new { AccountId = user.AccountId, PeriodId = period.Id }, ts);
            if (data.Count() > 0)
            {
                throw new NormalException("固定资产新增以及变更还有凭证未生成");
            }
            //期末结转时，固定资产的计提折旧凭证是否都已经生成


            FixedAssetsBLL fbll = new FixedAssetsBLL(cnn);
            bool flag = true;
            //获取固定资产
            List<TKS_FAS_FixedAssets> fixedAssets = fbll.FixedAssetsGetByAcccount(user.AccountId, period, ts);

            foreach (var item in fixedAssets)
            {
                if (item.IsStartPeriodDepreciation == 0 && item.StartPeriod == period.Id) { continue; }
                flag = false;
            }

            sql = @"SELECT  A.*,b.TPLTarget
                FROM  TKS_FAS_TPL2PZ A left join TKS_FAS_DocTPL B on A.TPLId =B.Id
                    where A.accountId=@AccountId and A.PeriodId=@PeriodId and B.tplTarget=3";
            data = cnn.Query(sql, new { AccountId = user.AccountId, PeriodId = period.Id }, ts);

            sql = @"select * from  TKS_FAS_FixedAssets left outer join TKS_FAS_MonthPeriodInfo period on StartPeriod=period.Id where TKS_FAS_FixedAssets.accountId=@AccountId and ( ( DepreciationMethod='1'
                    and IsStartPeriodDepreciation=1    ) or ( DepreciationMethod='1' and  ((period.Month <@Month and period.Year=@Year) or period.Year<@Year ) )) ";
            var fdata = cnn.Query(sql, new
            {
                AccountId = user.AccountId,
                Month = period.Month,
                Year = period.Year,
                StartPeriod = period.Id
            }, ts);

            if (data.Count() == 0 && fdata.Count() > 0 && !flag)
            {
                throw new NormalException("计提折旧与结账凭证未生成，请生成后再进行期末结转");
            }
        }

        /// <summary>
        /// 获取期末检查的模板(指定账套，会计期间)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseQMCheckTPLsGet QMCheckTPLsGet(RequestQMCheckTPLsGet request)
        {
            var res = new ResponseQMCheckTPLsGet();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    var user = UserInfoGet(request.Token, ts);
                    AccountSetBLL accountBll = new AccountSetBLL(cnn);
                    var period = accountBll.GetActivePeriod(user.AccountId, ts);
                    string where = "";
                    if (period.Month == 12)
                    {
                        where = " or A.tplTarget=5 or A.tplTarget=6";
                    }


                    //获取期末检查模板  是否已经生成凭证
                    string sql = @"select A.*,(case when  B.id is null then 0 else 1 end) as IsGenPZ,
                            C.AMT_DBT as Money,B.pzid
                            from TKS_FAS_DocTPL A 
                            left join  (select * from  tks_fas_tpl2pz  where accountId=@AccountId 
                            and periodId=@PeriodId )B   on A.Id=B.tplid     left join tks_fas_doc C on B.PZId=c.Id                         
                            where (A.tplTarget=4 or A.tplTarget=3 " + where + ") and A.isCustom=0      ";

                    var data = cnn.Query<TKS_FAS_DocTPLExt>(sql, new
                    {
                        AccountId = user.AccountId,//账套
                        PeriodId = period.Id//当前  期间
                    }, ts).ToList();



                    res.IsSuccess = true;
                    res.Data = data;
                    res.Message = "";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseQMCheckTPLsGet;
                }
            }
        }

        /// <summary>
        /// 期末结转保存
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseQMCarryOver QMCarryOver(RequestQMCarryOver request)
        {
            var res = new ResponseQMCarryOver();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);

                    AccountSetBLL account = new FAS.AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);
                    Validate(user, period, request, ts);

                    CarryOver(period, ts);
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "结转成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseQMCarryOver;
                }
            }
        }

        private void Validate(MM_UserInfo user, TKS_FAS_MonthPeriodInfo period, RequestQMCarryOver request, IDbTransaction ts)
        {
            //如果是第一期，期初平衡校验
            AccountSetBLL accountSet = new AccountSetBLL(cnn);

            var firstPeriod = accountSet.GetFirstPeriod(user.AccountId, ts);

            if (firstPeriod.Id == period.Id)
            {
                BALBLL balBLL = new FAS.BALBLL(cnn);
                var res = balBLL.CalculatePH(new Entity.FAS.RequestBALCalculate { Token = request.Token }, ts);
                if (res.IsSuccess == false)
                {
                    throw new NormalException(res.Message);
                }
            }

            //是否有待审核的凭证未审核
            string sql = @"select * from tks_fas_doc where checkStatus=1 and periodId=@PeriodId";
            var data = cnn.Query(sql, new { PeriodId = period.Id }, ts);
            if (data.Count() > 0)
            {
                throw new NormalException("当前期间还有未审核的凭证，请先审核后再做结转");
            }

            #region

            //固定资产新增，以及变更的凭证是否都已经生成
            sql = @"select * from TKS_FAS_FixedAssetsChange where 
                   ( docId is null or docId='') and AccountId=@AccountId  and PeriodId=@PeriodId";

            data = cnn.Query(sql, new { AccountId = user.AccountId, PeriodId = period.Id }, ts);
            if (data.Count() > 0)
            {
                throw new NormalException("固定资产新增以及变更还有凭证未生成");

            }
            #endregion

            #region  固定资产 当期没有可以计提折旧的，允许结转
            //期末结转时，固定资产的计提折旧凭证是否都已经生成

            FixedAssetsBLL fbll = new FixedAssetsBLL(cnn);
            bool flag = true;
            //获取固定资产
            List<TKS_FAS_FixedAssets> fixedAssets = fbll.FixedAssetsGetByAcccount(user.AccountId, period, ts);

            foreach (var item in fixedAssets)
            {
                if (item.IsStartPeriodDepreciation == 0 && item.StartPeriod == period.Id) { continue; }
                flag = false;
            }

            //模板是否生成凭证
            sql = @"SELECT  A.*,b.TPLTarget
                FROM  TKS_FAS_TPL2PZ A left join TKS_FAS_DocTPL B on A.TPLId =B.Id
                    where A.accountId=@AccountId and A.PeriodId=@PeriodId and B.tplTarget=3";
            data = cnn.Query(sql, new { AccountId = user.AccountId, PeriodId = period.Id }, ts);

            //固定资产是否需要折旧
            //sql = @"select * from  TKS_FAS_FixedAssets where accountId=@AccountId and DepreciationMethod='1'";
            sql = @"select * from  TKS_FAS_FixedAssets left outer join TKS_FAS_MonthPeriodInfo period on StartPeriod=period.Id where TKS_FAS_FixedAssets.accountId=@AccountId and ( ( DepreciationMethod='1'
                    and IsStartPeriodDepreciation=1    ) or ( DepreciationMethod='1' and  ((period.Month <@Month and period.Year=@Year) or period.Year<@Year ) )) ";
            var fdata = cnn.Query(sql, new { AccountId = user.AccountId, Year = period.Year, Month = period.Month }, ts);

            if (data.Count() == 0 && fdata.Count() > 0 && !flag)
            {
                throw new NormalException("计提折旧与结账凭证未生成，请生成后再进行期末结转");
            }

            #endregion

            if (period.Month == 12)
            {
                sql = @"select * from tks_fas_doc A left join TKS_FAS_DocDetail B on 
                    A.id=B.parentId where A.periodId=@PeriodId
                             and A.source='SY' and B.subjectCode='3131' ";
                data = cnn.Query(sql, new { PeriodId = period.Id }, ts);
                sql = @"SELECT   *   FROM  TKS_FAS_DocDetail 
                    where SubjectCode like '5%' and periodId=@PeriodId";
                var docs = cnn.Query<TKS_FAS_DocDetail>(sql, new { PeriodId = period.Id }, ts).ToList();


                if (data.Count() == 0 && docs.Count() > 0)
                {
                    throw new NormalException("亲，您还未做结转损益");
                }
            }
        }

        /// <summary>
        /// 期末结转
        /// </summary>
        /// <param name="period"></param>
        /// <param name="ts"></param>
        private void CarryOver(TKS_FAS_MonthPeriodInfo period, IDbTransaction ts)
        {
            BalanceBLL balBLL = new BalanceBLL(cnn);
            //标准科目余额表，去除辅助核算
            //科目余额表，报表取数据分别从标准科目余额表和 辅助核算余额表取

            //获取账套期间下的凭证,按科目代码，借贷方向进行分组
            string sql = @"  select accountid,PeriodId, SubjectCode,Credit_Debit, sum(Money_Debit) Money_Debit,sum(Money_Credit) Money_Credit,
            sum(Quantity) Quantity,max(Price) Price,max(Unit) Unit,max(CurrencyCode) CurrencyCode
            from TKS_FAS_DocDetail
            where IsCalHelper=0 and source<>'SY'  and accountid=@AccountId and PeriodId=@PeriodId 
            group by SubjectCode,Credit_Debit,PeriodId,accountid";

            var data1 = cnn.Query<TKS_FAS_DocDetail>(sql, new { AccountId = period.AccountId, PeriodId = period.Id, StartDate = period.StartDate, EndDate = period.EndDate }, ts).ToList();

            //存在辅助核算项的凭证
            //更新辅助核算项余额表，以便账簿统计的时候计算

            sql = @" select Credit_Debit,SubjectDescription,PeriodId,accountid, SubjectCode, sum(Money_Debit) Money_Debit,sum(Money_Credit) Money_Credit,
              sum(Quantity) Quantity,max(Price) Price,max(Unit) Unit,max(CurrencyCode) CurrencyCode,
            calItem1,calValue1,calItem2,calValue2,calItem3,calValue3,calItem4,calValue4,calItem5,calValue5
              from TKS_FAS_DocDetail
              where accountid=@AccountId and PeriodId=@PeriodId and IsCalHelper=1 and source<>'SY'
              group by SubjectCode ,PeriodId,accountid,SubjectDescription,Credit_Debit,
             calItem1,calValue1,calItem2,calValue2,calItem3,calValue3,calItem4,calValue4,calItem5,calValue5";
            var data2 = cnn.Query<TKS_FAS_DocDetail>(sql, new { AccountId = period.AccountId, PeriodId = period.Id, StartDate = period.StartDate, EndDate = period.EndDate }, ts).ToList();




            //获得当前期间下的凭证数据后，对当前账套期间下的余额表进行更新

            #region 更新普通项余额表

            sql = @"select * from TKS_FAS_GLBalance where accountId=@AccountId and periodId=@PeriodId";

            var blanceData = cnn.Query<TKS_FAS_GLBalance>(sql, new { AccountId = period.AccountId, PeriodId = period.Id }, ts).ToList();




            //期末结转前，对当前的余额进行备份
            sql = @"delete from TKS_FAS_GLBalanceBak where  AccountId=@AccountId and  periodId=@PeriodId";
            cnn.Execute(sql, new { AccountId = period.AccountId, PeriodId = period.Id }, ts);

            sql = @"insert into TKS_FAS_GLBalanceBak select * from TKS_FAS_GLBalance 
                    where AccountId=@AccountId and PeriodId=@PeriodId";

            cnn.Execute(sql, new { AccountId = period.AccountId, PeriodId = period.Id }, ts);

            //key 科目代码+币别
            Dictionary<string, TKS_FAS_GLBalance> dicBlance = new Dictionary<string, TKS_FAS_GLBalance>();


            foreach (var item in blanceData)
            {
                dicBlance.Add(item.SubjectCode + item.CurrencyCode, item);
            }

            foreach (var item in data1)
            {
                if (dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
                {
                    //余额表存在,则更新
                    //modify by andy   新增凭证时更新余额表
                    //balBLL.GLBalanceUpdate(dicBlance[item.SubjectCode + item.CurrencyCode], item, ts);

                }
                else
                {
                    //不存在，新增一条
                    //modify by andy   新增凭证时更新余额表
                    //var b = balBLL.GLBalanceAdd(item, ts);
                    //var g = balBLL.GLBalanceUpdate(b, item, ts);
                    //if (!dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
                    //    dicBlance.Add(item.SubjectCode + item.CurrencyCode, g);
                }
            }


            //本年利润处理
            #region modify by andy 转移至 产生结转损益凭证时  同步至余额表
            //sql = "select * from TKS_FAS_DocDetail where source='SY' and subjectCode='3131' and periodId=@PeriodId ";
            //var bnlr = cnn.Query<TKS_FAS_DocDetail>(sql, new { PeriodId = period.Id }, ts).ToList();
            //if (bnlr != null)
            //{
            //    foreach (var item in bnlr)
            //    {
            //        if (dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
            //        {
            //            //余额表存在,则更新
            //            balBLL.GLBalanceUpdate(dicBlance[item.SubjectCode + item.CurrencyCode], item, ts);
            //        }
            //    }

            //}

            //if (period.Month == 12)
            //{
            //    sql = "select * from TKS_FAS_DocDetail where source='SY' and subjectCode='3141105' and periodId=@PeriodId ";
            //    var wfplr = cnn.Query<TKS_FAS_DocDetail>(sql, new { PeriodId = period.Id }, ts).ToList();
            //    foreach (var item in wfplr)
            //    {
            //        if (dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
            //        {
            //            //余额表存在,则更新
            //            balBLL.GLBalanceUpdate(dicBlance[item.SubjectCode + item.CurrencyCode], item, ts);
            //        }
            //    }
            //}
            #endregion

            #endregion

            //期末结转前，对当前的辅助余额进行备份
            sql = @"delete from TKS_FAS_FGLBalanceBak where  AccountId=@AccountId and  periodId=@PeriodId";
            cnn.Execute(sql, new { AccountId = period.AccountId, PeriodId = period.Id }, ts);

            sql = @"insert into TKS_FAS_FGLBalanceBak select * from TKS_FAS_FGLBalance 
                    where AccountId=@AccountId and PeriodId=@PeriodId";

            cnn.Execute(sql, new { AccountId = period.AccountId, PeriodId = period.Id }, ts);


            #region 标准余额表，如果录入了期初，但是没有发生额，也要处理
            DealOnlyQC(period, ts);

            #endregion
            #region 检测后期数据 并更新
            BalanceBLL balanceBLL = new BalanceBLL(cnn);

            foreach (var item in blanceData)
            {
#if DEBUG
                if (item.SubjectCode == "4105001")
                {

                }
#endif
                if (item.YearStartYBBAL != 0 || item.YearStartBWBBAL != 0
                    || item.BWBStartBAL != 0 || item.YBStartBAL != 0
                    || item.BWBEndBAL != 0 || item.YBEndBAL != 0
                    || item.BWBCreditTotal != 0 || item.BWBDebitTotal != 0)
                {
                    try
                    {
                        balanceBLL.GLBanceCurrentPeriodUpdate(item, ts);
                        balanceBLL.GLBanceEffectPeriodsUpdate(item, ts);
                    }
                    catch (Exception ex)
                    {

                    }
                   
                }

            }
            #endregion


            #region 更新辅助核算项余额表



            //待入库的辅助核算项余额，由于凭证明细存在多条，条件为：科目、核算项一致的数据，只是借贷方向不一致
            // 数据准备放在缓存中操作
            List<TKS_FAS_FGLBalance> saveData = new List<TKS_FAS_FGLBalance>();


            foreach (var item in data2)
            {
                //逻辑修改：在新增凭证时，对余额表进行操作，结转时取消该操作
                //update by Hero.Zhang 20180515
                // FGLBalanceAdd(item, saveData, ts);//辅助核算项科目全部当新纪录插入核算余额表，状态为非期初录入

            }


            foreach (var item in saveData)
            {
                //逻辑修改：在新增凭证时，对余额表进行操作，结转时取消该操作
                //update by Hero.Zhang 20180515
                //cnn.Insert<TKS_FAS_FGLBalance>(item, ts);
            }
            #endregion

            #region 更新当前期间 为已结转 ，激活状态为  未激活
            sql = "update TKS_FAS_MonthPeriodInfo set isPay=1 ,isActive=0 where id=@Id and isPay=0 and isActive=1 ";
            cnn.Execute(sql, new { Id = period.Id }, ts);
            #endregion

            #region modify by andy 生成下一期
            PeriodBLL periodBLL = new PeriodBLL(cnn);
            var nextPeriod = periodBLL.CreateNextPeriod(period, ts);
            sql = "update TKS_FAS_MonthPeriodInfo set isPay=0 ,isActive=1 where id=@Id";
            cnn.Execute(sql, new { Id = nextPeriod.Id }, ts);
            //sql = @"select * from tks_fas_periodTrack where fromId=@FromId";
            //var trackData = cnn.QueryFirstOrDefault<TKS_FAS_PeriodTrack>(sql, new { FromId = period.Id }, ts);

            //TKS_FAS_MonthPeriodInfo newPeriod = new TKS_FAS_MonthPeriodInfo();
            //if (trackData == null)
            //{
            //    newPeriod.Id = Guid.NewGuid().ToString("N");
            //}
            //else
            //{
            //    newPeriod.Id = trackData.ToId;
            //}

            //newPeriod.AccountId = period.AccountId;
            //newPeriod.IsActive = 1;
            //newPeriod.IsPay = 0;

            //if (period.Month == 12)
            //{
            //    newPeriod.Month = 1;
            //    newPeriod.Year = period.Year + 1;
            //    //如果是12月份，则把年度期间也更新为激活，插入新的年度期间
            //    sql = @"update TKS_FAS_YearPeriodInfo set isClose=1,isActive=0 where id=@Id and isClose=0 and isActive=1";
            //    cnn.Execute(sql, new { Id = period.ParentId }, ts);

            //    TKS_FAS_YearPeriodInfo newYearPeriod = new TKS_FAS_YearPeriodInfo();
            //    newYearPeriod.Id = Guid.NewGuid().ToString("N");
            //    newYearPeriod.IsActive = 1;
            //    newYearPeriod.IsClose = 0;
            //    newYearPeriod.AccountId = period.AccountId;
            //    newYearPeriod.StartDate = DateTime.Parse(newPeriod.Year + "-01-01");
            //    newYearPeriod.EndDate = DateTime.Parse(newPeriod.Year + "-12-31 23:59:59");
            //    newYearPeriod.PeriodNUM = 12;
            //    newYearPeriod.Year = newPeriod.Year;
            //    cnn.Insert<TKS_FAS_YearPeriodInfo>(newYearPeriod, ts);

            //    newPeriod.ParentId = newYearPeriod.Id;
            //}
            //else
            //{
            //    newPeriod.ParentId = period.ParentId;
            //    newPeriod.Month = period.Month + 1;
            //    newPeriod.Year = period.Year;


            //}
            //newPeriod.StartDate = DateTime.Parse(newPeriod.Year + "-" + newPeriod.Month + "-01");
            //newPeriod.EndDate = newPeriod.StartDate.Value.AddMonths(1).AddSeconds(-1);
            //cnn.Insert<TKS_FAS_MonthPeriodInfo>(newPeriod, ts);

            //if (trackData == null)
            //{
            //    //期间历史
            //    TKS_FAS_PeriodTrack track = new TKS_FAS_PeriodTrack();
            //    track.Id = Guid.NewGuid().ToString("N");
            //    track.FromId = period.Id;
            //    track.ToId = newPeriod.Id;
            //    track.FromYear = period.Year.ToString();
            //    track.ToYear = newPeriod.Year.ToString();
            //    track.FromMonth = period.Month.ToString();
            //    track.ToMonth = newPeriod.Month.ToString();
            //    track.AccountId = period.AccountId;
            //    cnn.Insert<TKS_FAS_PeriodTrack>(track, ts);

            //}
            #endregion

            #region 插入新的余额表
            //BalanceAddup(newPeriod, period, ts);

            #endregion
            balanceBLL.UpdateEffectDocDetailsFGL(period, ts);
            var effectPeriods = periodBLL.GetNextPeriods(period.AccountId, period?.Year ?? 0, period?.Month ?? 0, ts);
            foreach (var nextPeriod1 in effectPeriods)
            {
                balanceBLL.FGLBanceNextPeriodUpdate(nextPeriod1, period, ts);
                period = nextPeriod1;
            }
        }

        /// <summary>
        ///  是否需要生成 生产成本 对冲凭证
        /// </summary>
        /// <param name="period"></param>
        /// <param name="ts"></param>
        private void GenSCCB(MM_UserInfo user, TKS_FAS_MonthPeriodInfo period, string code, IDbTransaction ts)
        {
            //先删除已经生成的生产成本的结转凭证
            BalanceBLL balanceBLL = new BalanceBLL(cnn);
            balanceBLL.SCBalanceUpdate(period, ts, user, true);

            string sql = "delete from tks_fas_doc where periodId=@PeriodId and source='SC'";
            cnn.Execute(sql, new { PeriodId = period.Id, StartDate = period.StartDate, EndDate = period.EndDate, AccountId = user.AccountId }, ts);

            sql = "delete from TKS_FAS_DocDetail where periodId=@PeriodId and source='SC' ";
            cnn.Execute(sql, new { PeriodId = period.Id, StartDate = period.StartDate, EndDate = period.EndDate, AccountId = user.AccountId }, ts);



            sql = @"select sum(money_debit) as money_debit ,sum(money_credit)  as money_credit
                        from TKS_FAS_DocDetail 
                        where periodId=@PeriodId and subjectCode=@SubjectCode 
                       group by money_debit,money_credit";

            var data1 = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
                new { PeriodId = period.Id, SubjectCode = "4101", StartDate = period.StartDate, EndDate = period.EndDate, AccountId = user.AccountId }, ts);

            var data2 = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
                new { PeriodId = period.Id, SubjectCode = "4105", StartDate = period.StartDate, EndDate = period.EndDate, AccountId = user.AccountId }, ts);

            var data3 = cnn.QueryFirstOrDefault<TKS_FAS_DocDetail>(sql,
                new { PeriodId = period.Id, SubjectCode = "4107", StartDate = period.StartDate, EndDate = period.EndDate, AccountId = user.AccountId }, ts);

            #region 4101 
            int debitCredit1 = 0;
            decimal money1 = 0;
            if (data1 != null)
            {

                if (data1.Money_Credit > 0 || data1.Money_Debit > 0)
                {
                    //生产成本科目存在金额，汇总后，取大的一方的反方向对冲

                    if (data1.Money_Debit - data1.Money_Credit > 0)
                    {
                        debitCredit1 = 1;//取贷方
                        money1 = data1.Money_Debit - data1.Money_Credit;
                    }
                    else
                    {
                        debitCredit1 = 0;//取借方
                        money1 = data1.Money_Credit - data1.Money_Debit;
                    }
                }
            }

            #endregion

            #region 4105 
            int debitCredit2 = 0;
            decimal money2 = 0;
            if (data2 != null)
            {

                if (data2.Money_Credit > 0 || data2.Money_Debit > 0)
                {

                    if (data2.Money_Debit - data2.Money_Credit > 0)
                    {
                        debitCredit2 = 1;//取贷方
                        money2 = data2.Money_Debit - data2.Money_Credit;
                    }
                    else
                    {
                        debitCredit2 = 0;//取借方
                        money2 = data2.Money_Credit - data2.Money_Debit;
                    }
                }
            }

            #endregion


            #region 4107 
            int debitCredit3 = 0;
            decimal money3 = 0;
            if (data3 != null)
            {

                if (data3.Money_Credit > 0 || data3.Money_Debit > 0)
                {

                    if (data3.Money_Debit - data3.Money_Credit > 0)
                    {
                        debitCredit3 = 1;//取贷方
                        money3 = data3.Money_Debit - data3.Money_Credit;
                    }
                    else
                    {
                        debitCredit3 = 0;//取借方
                        money3 = data3.Money_Credit - data3.Money_Debit;
                    }
                }
            }

            #endregion

            if (data1 == null && data2 == null && data3 == null)
                return;

            sql = @"select * from TKS_FAS_CertificateWord where accountId=@AccountId and pzz='转'";

            var pzz = cnn.QuerySingleOrDefault<TKS_FAS_CertificateWord>(sql,
                new { AccountId = period.AccountId }, ts);

            //sql = "select * from tks_fas_doc where periodId=@PeriodId and pzz=@PZZ";
            sql = "select * from tks_fas_doc where  PZDate>=@StartDate and PZDate<=@EndDate and pzz=@PZZ and AccountId=@AccountId";
            var d = cnn.Query(sql, new { PeriodId = period.Id, PZZ = pzz.Id, StartDate = period.StartDate, EndDate = period.EndDate, AccountId = period.AccountId }, ts);
            string date = DateTime.Parse(period.EndDate.ToString()).ToString("yyyy-MM-dd");

            //生产成本余额不为0，则产生结转凭证，对冲
            TKS_FAS_Doc doc = new TKS_FAS_Doc();
            doc.Id = Guid.NewGuid().ToString("N");
            doc.PZDate = DateTime.Parse(date);
            doc.PZZ = pzz.Id;
            doc.Source = "SC";
            doc.PZZNO = d.Count() + 1;
            doc.PeriodId = period.Id;
            doc.Year = period.Year;

            doc.AccountId = period.AccountId;
            doc.CreateUser = user.User.TrueName;
            doc.CreateDate = DateTime.Now;


            //4101 生产成本  4105 制造费用  4107  劳务成本 
            TKS_FAS_DocDetail detail1 = new TKS_FAS_DocDetail();
            TKS_FAS_DocDetail detail2 = new TKS_FAS_DocDetail();
            TKS_FAS_DocDetail detail3 = new TKS_FAS_DocDetail();
            int Seq = 1;
            #region 4101 生产成本
            if (money1 > 0)
            {

                detail1.Id = Guid.NewGuid().ToString("N");
                detail1.AccountId = user.AccountId;
                detail1.PeriodId = period.Id;

                detail1.Credit_Debit = debitCredit1;
                if (detail1.Credit_Debit == 0)
                {
                    detail1.Money_Debit = money1;
                }
                else
                {
                    detail1.Money_Credit = money1;
                }

                detail1.ParentId = doc.Id;
                detail1.CurrencyCode = "RMB";
                detail1.SubjectCode = "4101";
                detail1.SubjectDescription = "4101 生产成本";
                detail1.Summary = "结转成本";
                detail1.Year = period.Year;
                detail1.Source = "SC";
                detail1.Seq = Seq;
                cnn.Insert<TKS_FAS_DocDetail>(detail1, ts);
                Seq++;
            }

            #endregion

            #region 4105 制造费用
            if (money2 > 0)
            {

                detail2.Id = Guid.NewGuid().ToString("N");
                detail2.AccountId = user.AccountId;
                detail2.PeriodId = period.Id;

                detail2.Credit_Debit = debitCredit2;
                if (detail2.Credit_Debit == 0)
                {
                    detail2.Money_Debit = money2;
                }
                else
                {
                    detail2.Money_Credit = money2;
                }
                detail2.ParentId = doc.Id;
                detail2.CurrencyCode = "RMB";
                detail2.SubjectCode = "4105";
                detail2.SubjectDescription = "4105 制造费用";
                detail2.Summary = "结转成本";
                detail2.Year = period.Year;
                detail2.Source = "SC";
                detail2.Seq = Seq;
                cnn.Insert<TKS_FAS_DocDetail>(detail2, ts);
                Seq++;
            }

            #endregion

            #region 4107  劳务成本 
            if (money3 > 0)
            {
                detail3.Id = Guid.NewGuid().ToString("N");
                detail3.AccountId = user.AccountId;
                detail3.PeriodId = period.Id;

                detail3.Credit_Debit = debitCredit3;
                if (detail3.Credit_Debit == 0)
                {
                    detail3.Money_Debit = money3;
                }
                else
                {
                    detail3.Money_Credit = money3;
                }
                detail3.ParentId = doc.Id;
                detail3.CurrencyCode = "RMB";
                detail3.SubjectCode = "4107";
                detail3.SubjectDescription = "4107  劳务成本 ";
                detail3.Summary = "结转成本";
                detail3.Year = period.Year;
                detail3.Source = "SC";
                detail3.Seq = Seq;
                cnn.Insert<TKS_FAS_DocDetail>(detail3, ts);
                Seq++;
            }

            #endregion


            decimal left = detail1.Money_Debit + detail2.Money_Debit + detail3.Money_Debit;
            decimal right = detail1.Money_Credit + detail2.Money_Credit + detail3.Money_Credit;



            TKS_FAS_DocDetail detail22 = new TKS_FAS_DocDetail();
            detail22.Id = Guid.NewGuid().ToString("N");
            detail22.AccountId = user.AccountId;
            detail22.PeriodId = period.Id;
            if (left > right)
            {
                detail22.Credit_Debit = 1;
                detail22.Money_Credit = left - right;
                doc.AMT_DBT = left;
            }
            else
            {
                detail22.Credit_Debit = 0;
                detail22.Money_Debit = right - left;
                doc.AMT_DBT = right;
            }


            detail22.ParentId = doc.Id;
            detail22.CurrencyCode = "RMB";
            if (code == "1")
            {
                detail22.SubjectCode = "5401";
                detail22.SubjectDescription = "5401 主营业务成本";
            }
            else
            {
                detail22.SubjectCode = "1243";
                detail22.SubjectDescription = "1243 库存商品";
            }

            detail22.Summary = "结转成本";
            detail22.Year = period.Year;
            detail22.Source = "SC";
            detail22.Seq = Seq;
            cnn.Insert<TKS_FAS_DocDetail>(detail22, ts);

            cnn.Insert<TKS_FAS_Doc>(doc, ts);
            balanceBLL.SCBalanceUpdate(period, ts, user);
        }

        /// <summary>
        /// 期初的时候处理
        /// </summary>
        /// <param name="period"></param>
        /// <param name="ts"></param>
        private void DealOnlyQC(TKS_FAS_MonthPeriodInfo period, IDbTransaction ts)
        {
            string sql = @"update TKS_FAS_GLBalance  set ECredit_Debit=SCredit_Debit ,
                        NUMEndBAL=NUMStartBAL,BWBEndBAL=BWBStartBAL,YBEndBAL=YBStartBAL where 
                        PeriodId=@PeriodId and NUMDebitTotal=0 and BWBDebitTotal=0 and YBDebitTotal=0
                        and NUMCreditTotal=0 and BWBCreditTotal=0 and YBCreditTotal=0 ";

            cnn.Execute(sql, new { PeriodId = period.Id }, ts);

            sql = @"update TKS_FAS_FGLBalance  set ECredit_Debit=SCredit_Debit ,
                        NUMEndBAL=NUMStartBAL,BWBEndBAL=BWBStartBAL,YBEndBAL=YBStartBAL where 
                        PeriodId=@PeriodId and NUMDebitTotal=0 and BWBDebitTotal=0 and YBDebitTotal=0
                        and NUMCreditTotal=0 and BWBCreditTotal=0 and YBCreditTotal=0 ";

            cnn.Execute(sql, new { PeriodId = period.Id }, ts);
        }







        /// <summary>
        /// 更新辅助核算余额表(废弃)
        /// </summary>
        /// <param name="currentBalance"></param>
        /// <param name="newDoc"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private TKS_FAS_FGLBalance FGLBalanceUpdate(TKS_FAS_FGLBalance currentBalance, TKS_FAS_DocDetail newDoc, IDbTransaction ts)
        {
            //年度是累加的，年初是确定后就不变的，本月的是当前月，期末是公式计算

            if (newDoc.Credit_Debit == 0)//借方
            {
                currentBalance.NUMDebitTotal += newDoc.Quantity;//本月借方累计数量
                currentBalance.NUMDebitTotal_Y += newDoc.Quantity;//年度借方累计数量
                currentBalance.YBDebitTotal += newDoc.YB;
                currentBalance.YBDebitTotal_Y += newDoc.YB;//年度借方累计原币
                currentBalance.BWBDebitTotal += newDoc.Money_Debit;//本位币
                currentBalance.BWBDebitTotal_Y += newDoc.Money_Debit;//年度借方累计 本位币

            }
            else
            {
                //贷方
                currentBalance.NUMCreditTotal += newDoc.Quantity;
                currentBalance.NUMCreditTotal_Y += newDoc.Quantity;//数量
                currentBalance.YBCreditTotal += newDoc.YB;
                currentBalance.YBCreditTotal_Y += newDoc.YB;
                currentBalance.BWBCreditTotal += newDoc.Money_Credit;
                currentBalance.BWBCreditTotal_Y += newDoc.Money_Credit;

            }

            if (currentBalance.SCredit_Debit == 0)//期初余额方向 借
            {
                //期初+本期借方-本期贷方 >0 借 <0 贷 =0 平
                var val = currentBalance.BWBStartBAL + currentBalance.BWBDebitTotal - currentBalance.BWBCreditTotal;
                int cb = 0;
                if (val > 0)
                    cb = 0;
                else if (val == 0)
                    cb = 0;
                else
                    cb = 1;

                currentBalance.ECredit_Debit = cb;
                currentBalance.BWBEndBAL = Math.Abs(val);
                currentBalance.NUMEndBAL = currentBalance.NUMStartBAL + currentBalance.NUMDebitTotal - currentBalance.NUMCreditTotal;
                currentBalance.YBEndBAL = currentBalance.YBStartBAL + currentBalance.YBDebitTotal - currentBalance.YBCreditTotal;
            }
            else
            {
                //期初+本期贷方-本期借方 >0 贷 <0 借 =0 平
                var val = currentBalance.BWBStartBAL + currentBalance.BWBCreditTotal - currentBalance.BWBDebitTotal;
                int cb = 0;
                if (val > 0)
                    cb = 1;
                else if (val == 0)
                    cb = 1;
                else
                    cb = 0;

                currentBalance.ECredit_Debit = cb;
                currentBalance.BWBEndBAL = Math.Abs(val);
                currentBalance.NUMEndBAL = currentBalance.NUMStartBAL + currentBalance.NUMCreditTotal - currentBalance.NUMDebitTotal;
                currentBalance.YBEndBAL = currentBalance.YBStartBAL + currentBalance.YBCreditTotal - currentBalance.YBDebitTotal;

            }

            string sql = @"UPDATE  TKS_FAS_FGLBalance
                       SET  
                           CurrencyCode = @CurrencyCode
                          ,NUMDebitTotal = @NUMDebitTotal
                          ,NUMDebitTotal_Y = @NUMDebitTotal_Y
                          ,BWBDebitTotal = @BWBDebitTotal
                          ,BWBDebitTotal_Y =@BWBDebitTotal_Y
                          ,YBDebitTotal = @YBDebitTotal
                          ,YBDebitTotal_Y =@YBDebitTotal_Y
                          ,NUMCreditTotal = @NUMCreditTotal
                          ,NUMCreditTotal_Y = @NUMCreditTotal_Y
                          ,BWBCreditTotal =@BWBCreditTotal
                          ,BWBCreditTotal_Y =@BWBCreditTotal_Y
                          ,YBCreditTotal = @YBCreditTotal
                          ,YBCreditTotal_Y =@YBCreditTotal_Y
                          ,ECredit_Debit = @ECredit_Debit
                          ,NUMEndBAL =@NUMEndBAL
                          ,BWBEndBAL = @BWBEndBAL
                          ,YBEndBAL = @YBEndBAL
                          ,Unit = @Unit
                     WHERE id=@Id";
            cnn.Execute(sql, currentBalance, ts);

            return currentBalance;
        }





        /// <summary>
        /// 获取当前激活年度的所有会计期间
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseQMListSearch QMListSearch(RequestQMListSearch request)
        {
            var res = new ResponseQMListSearch();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    AccountSetBLL ac = new AccountSetBLL(cnn);
                    var period = ac.GetActivePeriod(user.AccountId, ts);

                    string sql = @"select * from tks_fas_periodTrack where toId=@ToId";

                    var track = cnn.QueryFirstOrDefault<TKS_FAS_PeriodTrack>(sql, new { ToId = period.Id }, ts);
                    string field = string.Empty;
                    if (track != null)
                    {
                        //可反结转
                        field = " , ( case when id='" + track.FromId + "' then 1 else 0 end ) as canUnCarryOver";
                    }

                    sql = this.GetPageSql("A.* " + field,
             @"  TKS_FAS_MonthPeriodInfo  A
                        where    accountId=@AccountId",
             "A.year desc, A.month desc",
             request.PageSize,
             request.PageIndex);

                    List<TKS_FAS_MonthPeriodInfoExt> data = cnn.Query<TKS_FAS_MonthPeriodInfoExt>(sql,
                        new
                        {
                            AccountId = user.AccountId
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_MonthPeriodInfo A
                    where   accountId=@AccountId";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {

                        AccountId = user.AccountId
                    }, ts).ToString());

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
                    return this.DealException(res, ex) as ResponseQMListSearch;
                }
            }


        }

        /// <summary>
        /// 反结转
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseQMUnCarryOver QMUnCarryOver(RequestQMUnCarryOver request)
        {
            var res = new ResponseQMUnCarryOver();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    AccountSetBLL account = new FAS.AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);

                    string sql = @"select * from tks_fas_periodTrack where accountId=@AccountId
                            and toId=@ToId";
                    List<TKS_FAS_PeriodTrack> lsTrack = cnn.Query<TKS_FAS_PeriodTrack>(sql,
                        new { AccountId = user.AccountId, ToId = period.Id }, ts).ToList();
                    if (lsTrack.Count == 0)
                    {
                        throw new NormalException("当前期间未找到上一期间,无法反结转");
                    }
                    //找到track后，处理当前的期间和前一期间
                    UnCarryOver(lsTrack[0], ts, user);

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "反结转成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseQMUnCarryOver;
                }
            }
        }

        //private void UnCarryOver(TKS_FAS_PeriodTrack track, IDbTransaction ts)
        //{
        //    //1.删除当前的期间
        //    #region 删除当前期间
        //    string sql = @"select * from tks_fas_MonthPeriodInfo where id=@Id";

        //    var data = cnn.QueryFirst<TKS_FAS_MonthPeriodInfo>(sql, new { Id = track.ToId }, ts);

        //    if (data.Month == 1)
        //    {
        //        sql = @"delete from TKS_FAS_MonthPeriodInfo where id=@Id";

        //        cnn.Execute(sql, new { Id = track.ToId }, ts);

        //        sql = @"delete from tks_fas_yearPeriodInfo where id=@Id";

        //        cnn.Execute(sql, new { Id = data.ParentId }, ts);
        //    }
        //    else
        //    {
        //        sql = @"delete from TKS_FAS_MonthPeriodInfo where id=@Id";

        //        cnn.Execute(sql, new { Id = track.ToId }, ts);
        //    }
        //    #endregion

        //    //2.删除当前期间的余额表
        //    #region 删除当前期间的余额表
        //    sql = @"delete from TKS_FAS_GLBalance where periodId=@PeriodId";
        //    cnn.Execute(sql, new { PeriodId = track.ToId }, ts);
        //    sql = @"delete from TKS_FAS_FGLBalance where periodId=@PeriodId";
        //    cnn.Execute(sql, new { PeriodId = track.ToId }, ts);
        //    #endregion


        //    //3.更新上一期间的状态
        //    #region 更新上一期间的状态
        //    sql = @"update TKS_FAS_MonthPeriodInfo set isPay=0 ,isActive=1 
        //    where id=@Id and isPay=1 and isActive=0";
        //    cnn.Execute(sql, new { Id = track.FromId }, ts);
        //    #endregion

        //    //4.删除上一期间的余额表
        //    sql = @"delete from TKS_FAS_GLBalance where periodId=@PeriodId";
        //    cnn.Execute(sql, new { PeriodId = track.FromId }, ts);
        //    sql = @"delete from TKS_FAS_FGLBalance where periodId=@PeriodId";
        //    cnn.Execute(sql, new { PeriodId = track.FromId }, ts);
        //    //5.还原上一期间的余额表

        //    sql = @"insert into TKS_FAS_GLBalance select * from 
        //        TKS_FAS_GLBalanceBak where periodId=@PeriodId";
        //    cnn.Execute(sql, new { PeriodId = track.FromId }, ts);

        //    sql = @"insert into TKS_FAS_FGLBalance select * from 
        //        TKS_FAS_FGLBalanceBak where periodId=@PeriodId";
        //    cnn.Execute(sql, new { PeriodId = track.FromId }, ts);


        //    //6.删除上一期间生成的结转损益凭证
        //    sql = @"delete from tks_fas_doc where periodId=@PeriodId and ( source='JZ' or source='SC' or source='SY' )";
        //    cnn.Execute(sql, new { PeriodId = track.FromId }, ts);

        //    sql = @"delete from tks_fas_docDetail where periodId=@PeriodId and ( source='JZ' or source='SC' or source='SY' )";
        //    cnn.Execute(sql, new { PeriodId = track.FromId }, ts);


        //}

        private void UnCarryOver(TKS_FAS_PeriodTrack track, IDbTransaction ts, MM_UserInfo user)
        {
            BalanceBLL balanceBLL = new BalanceBLL(cnn);
            PeriodBLL periodBLL = new PeriodBLL(cnn);
            #region 该期间active 下期间inactive
            string sql = @"update TKS_FAS_MonthPeriodInfo set isPay=0 ,isActive=1 
            where id=@Id and isPay=1 and isActive=0";
            cnn.Execute(sql, new { Id = track.FromId }, ts);
            sql = @"update TKS_FAS_MonthPeriodInfo set isActive=0 
            where id=@Id and isPay=0 and isActive=1";
            cnn.Execute(sql, new { Id = track.ToId }, ts);
            #endregion

            #region 反结转 结转损益
            var fromPeriod = periodBLL.GetPeriod(track.FromId, ts);
            balanceBLL.SYBalanceUpdate(fromPeriod, ts, true);
            balanceBLL.SCBalanceUpdate(fromPeriod, ts, user, true);

            sql = @"delete from tks_fas_doc where periodId=@PeriodId and ( source='JZ' or source='SC' or source='SY' )";
            cnn.Execute(sql, new { PeriodId = track.FromId }, ts);

            sql = @"delete from tks_fas_docDetail where periodId=@PeriodId and ( source='JZ' or source='SC' or source='SY' )";
            cnn.Execute(sql, new { PeriodId = track.FromId }, ts);

            #endregion
        }
        #region 生产成本
        public ResponseSCCBGen SCCBGen(RequestSCCBGen request)
        {

            var res = new ResponseSCCBGen();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    AccountSetBLL account = new FAS.AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);
                    GenSCCB(user, period, request.SCCBCode, ts);
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "结转成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseSCCBGen;
                }
            }
        }
        #endregion

        #region 结转损益

        public ResponseQMSYStatusGet QMSYStatusGet(RequestQMSYStatusGet request)
        {
            var res = new ResponseQMSYStatusGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    AccountSetBLL bll = new AccountSetBLL(cnn);
                    var period = bll.GetActivePeriod(user.AccountId, ts);

                    string sql = @"select * from tks_fas_doc where periodId=@PeriodId
                             and source='SY' ";
                    var data = cnn.Query(sql, new { PeriodId = period.Id }, ts);
                    ts.Commit();
                    if (data.Count() > 0)
                    {
                        res.IsSuccess = true;
                        res.Message = "亲，本期已经结转损益";
                    }
                    else
                    {

                        sql = @"SELECT   *   FROM  TKS_FAS_DocDetail 
                    where SubjectCode like '5%' and periodId=@PeriodId";
                        var DocDetail = cnn.Query<TKS_FAS_DocDetail>(sql, new { PeriodId = period.Id }, ts).ToList();

                        if (DocDetail.Count == 0)
                        {
                            res.IsSuccess = true;
                            res.Message = "亲，本期无需结转损益";

                        }
                        else
                        {
                            res.IsSuccess = false;
                            res.Message = "亲，您还未结转损益";
                        }


                    }
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseQMSYStatusGet;
                }
            }
        }

        public ResponseQMSYGen QMSYGen(RequestQMSYGen request)
        {
            var res = new ResponseQMSYGen();

            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                BalanceBLL balanceBLL = new BalanceBLL(cnn);
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    AccountSetBLL bll = new AccountSetBLL(cnn);
                    var period = bll.GetActivePeriod(user.AccountId, ts);
                    balanceBLL.SYBalanceUpdate(period, ts, true);
                    string sql = @"delete from tks_fas_doc where periodId=@PeriodId and source='SY' ";
                    cnn.Execute(sql, new { PeriodId = period.Id }, ts);

                    sql = @"delete from tks_fas_docDetail where periodId=@PeriodId and source='SY' ";
                    cnn.Execute(sql, new { PeriodId = period.Id }, ts);



                    sql = @"select   *   from  tks_fas_docdetail 
                    where subjectcode like '5%' and periodid=@periodid";

                    var data = cnn.Query<TKS_FAS_DocDetail>(sql, new { PeriodId = period.Id }, ts).ToList();

                    if (data.Count == 0)
                    {
                        return new ResponseQMSYGen { IsSuccess = true, Message = "亲，本期无需结转损益" };
                    }

                    sql = @"select * from TKS_FAS_CertificateWord where accountId=@AccountId and pzz='转'";

                    var pzz = cnn.QuerySingleOrDefault<TKS_FAS_CertificateWord>(sql,
                        new { AccountId = user.AccountId }, ts);

                    if (pzz == null)
                        throw new NormalException("凭证字基础数据异常");
                    string date = DateTime.Parse(period.EndDate.ToString()).ToString("yyyy-MM-dd");
                    TKS_FAS_Doc doc = new TKS_FAS_Doc();
                    doc.Id = Guid.NewGuid().ToString("N");
                    //doc.PZDate = DateTime.Now;
                    doc.PZDate = DateTime.Parse(date);
                    doc.PZZ = pzz.Id;
                    doc.Source = "SY";
                    doc.PeriodId = period.Id;

                    sql = "select * from tks_fas_doc where periodId=@PeriodId and pzz=@PZZ ";

                    var oldDocs = cnn.Query(sql, new { PeriodId = period.Id, PZZ = pzz.Id }, ts);

                    doc.PZZNO = oldDocs.Count() + 1;

                    doc.Year = period.Year;

                    doc.AccountId = user.AccountId;
                    doc.CreateUser = user.User.TrueName;
                    doc.CreateDate = DateTime.Now;

                    decimal credit = 0;
                    decimal debit = 0;
                    int Seq = 1;
                    foreach (var detail in data)
                    {
                        if (detail.Credit_Debit == 0)
                        {
                            detail.Credit_Debit = 1;
                            detail.Money_Credit = detail.Money_Debit;//取反
                            detail.Money_Debit = 0;
                            credit += detail.Money_Credit;
                        }
                        else
                        {
                            detail.Credit_Debit = 0;
                            detail.Money_Debit = detail.Money_Credit;
                            detail.Money_Credit = 0;
                            debit += detail.Money_Debit;
                        }
                        detail.Id = Guid.NewGuid().ToString("N");
                        detail.ParentId = doc.Id;
                        detail.Source = "SY";
                        detail.Seq = Seq;
                        cnn.Insert<TKS_FAS_DocDetail>(detail, ts);
                        Seq++;
                    }

                    TKS_FAS_DocDetail bnlr = new TKS_FAS_DocDetail();
                    bnlr.Id = Guid.NewGuid().ToString("N");
                    bnlr.AccountId = user.AccountId;
                    bnlr.PeriodId = period.Id;
                    if (debit > credit)
                    {
                        bnlr.Credit_Debit = 1;
                        bnlr.Money_Credit = debit - credit;
                        doc.AMT_DBT = debit;
                    }
                    else
                    {
                        bnlr.Credit_Debit = 0;
                        bnlr.Money_Debit = credit - debit;
                        doc.AMT_DBT = credit;
                    }

                    bnlr.ParentId = doc.Id;
                    bnlr.CurrencyCode = "RMB";
                    bnlr.SubjectCode = "3131";
                    bnlr.SubjectDescription = "3131 本年利润";
                    bnlr.Summary = "结转损益";
                    bnlr.Year = period.Year;
                    bnlr.Source = "SY";
                    bnlr.Seq = Seq;
                    cnn.Insert<TKS_FAS_DocDetail>(bnlr, ts);
                    cnn.Insert<TKS_FAS_Doc>(doc, ts);
                    if (period.Month == 12)
                    {
                        GenZ2(bnlr, pzz, oldDocs.Count(), period, user, ts);
                    }

                    balanceBLL.SYBalanceUpdate(period, ts);
                    ts.Commit();

                    res.IsSuccess = true;
                    res.Message = "亲，本期已经结转损益";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseQMSYGen;
                }
            }
        }

        private void GenZ2(TKS_FAS_DocDetail bnlr, TKS_FAS_CertificateWord pzz,
            int no, TKS_FAS_MonthPeriodInfo period, MM_UserInfo user, IDbTransaction ts)
        {
            TKS_FAS_Doc doc = new TKS_FAS_Doc();
            doc.Id = Guid.NewGuid().ToString("N");
            doc.PZDate = period.EndDate;
            doc.PZZ = pzz.Id;
            doc.Source = "SY";
            doc.PZZNO = no + 2;
            doc.PeriodId = period.Id;
            doc.Year = period.Year;

            doc.AccountId = user.AccountId;
            doc.CreateUser = user.User.TrueName;
            doc.CreateDate = DateTime.Now;



            TKS_FAS_DocDetail detail1 = new TKS_FAS_DocDetail();
            detail1.Id = Guid.NewGuid().ToString("N");
            detail1.AccountId = user.AccountId;
            detail1.PeriodId = period.Id;
            if (bnlr.Credit_Debit == 0)
            {
                detail1.Credit_Debit = 1;
                detail1.Money_Credit = bnlr.Money_Debit;
                doc.AMT_DBT = detail1.Money_Credit;
            }
            else
            {
                detail1.Credit_Debit = 0;
                detail1.Money_Debit = bnlr.Money_Credit;
                doc.AMT_DBT = detail1.Money_Debit;
            }

            detail1.ParentId = doc.Id;
            detail1.CurrencyCode = "RMB";
            detail1.SubjectCode = "3131";
            detail1.SubjectDescription = "3131 本年利润";
            detail1.Summary = "结转损益";
            detail1.Year = period.Year;
            detail1.Source = "SY";
            cnn.Insert<TKS_FAS_DocDetail>(detail1, ts);


            TKS_FAS_DocDetail detail2 = new TKS_FAS_DocDetail();
            detail2.Id = Guid.NewGuid().ToString("N");
            detail2.AccountId = user.AccountId;
            detail2.PeriodId = period.Id;
            if (bnlr.Credit_Debit == 0)
            {
                detail2.Credit_Debit = 0;
                detail2.Money_Debit = bnlr.Money_Debit;

            }
            else
            {
                detail2.Credit_Debit = 1;
                detail2.Money_Credit = bnlr.Money_Credit;

            }

            detail2.ParentId = doc.Id;
            detail2.CurrencyCode = "RMB";
            detail2.SubjectCode = "3141105";
            detail2.SubjectDescription = "3141105 利润分配-未分配利润";
            detail2.Summary = "结转损益";
            detail2.Year = period.Year;
            detail2.Source = "SY";
            cnn.Insert<TKS_FAS_DocDetail>(detail2, ts);

            cnn.Insert<TKS_FAS_Doc>(doc, ts);

        }
        #endregion
    }
}
