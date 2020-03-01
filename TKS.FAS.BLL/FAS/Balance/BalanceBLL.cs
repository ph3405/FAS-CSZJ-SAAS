using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using TKS.FAS.Common;
using TKS.FAS.Entity.FAS;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;

namespace TKS.FAS.BLL.FAS
{
    /// <summary>
    /// 期间
    /// </summary>
    public class BalanceBLL : CommonBase
    {
        public BalanceBLL()
        {

        }

        public BalanceBLL(IDbConnection cnn)
        {
            this.cnn = cnn;
        }
        /// <summary>
        /// 新期间余额初始数据加载
        /// </summary>
        /// <param name="newPeriod"></param>
        /// <param name="oldPeriod"></param>
        /// <param name="ts"></param>
        public void BalanceAddup(TKS_FAS_MonthPeriodInfo newPeriod, TKS_FAS_MonthPeriodInfo oldPeriod, IDbTransaction ts)
        {
            #region 非12月
            string sql = @" insert into  TKS_FAS_GLBalance (Id
      ,AccountId
      ,SubjectId
      ,SubjectCode
      ,ParentId
      ,Name
      ,PeriodId
      ,Year
      ,CurrencyCode
      ,SCredit_Debit
      ,NUMStartBAL
      ,BWBStartBAL
      ,YBStartBAL
      ,NUMDebitTotal
      ,NUMDebitTotal_Y
      ,BWBDebitTotal
      ,BWBDebitTotal_Y
      ,YBDebitTotal
      ,YBDebitTotal_Y
      ,NUMCreditTotal
      ,NUMCreditTotal_Y
      ,BWBCreditTotal
      ,BWBCreditTotal_Y
      ,YBCreditTotal
      ,YBCreditTotal_Y
      ,ECredit_Debit
      ,NUMEndBAL
      ,BWBEndBAL
      ,YBEndBAL
      ,Category
      ,Unit
      ,YearStartNumBAL
      ,YearStartYBBAL
      ,YearStartBWBBAL
      ,CreateUser
      ,CreateDate
      ,IsCalHelperValid
      ,IsQuantityValid
      ,IsDefaultCurrency
      ,IsLeaf)
 
 select 
 newid()
      ,AccountId
      ,SubjectId
      ,SubjectCode
      ,ParentId
      ,Name
      ,@newPeriodId
      ,@Year
      ,CurrencyCode
      ,ECredit_Debit as SCredit_Debit
      ,NUMEndBAL as  NUMStartBAL
      ,BWBEndBAL as BWBStartBAL
      ,YBEndBAL as YBStartBAL
      ,0 as NUMDebitTotal
      ,NUMDebitTotal_Y
      ,0 as BWBDebitTotal
      ,BWBDebitTotal_Y
      ,0 as YBDebitTotal
      ,YBDebitTotal_Y
      ,0 as NUMCreditTotal
      ,NUMCreditTotal_Y
      ,0 as BWBCreditTotal
      ,BWBCreditTotal_Y
      ,0 as YBCreditTotal
      ,YBCreditTotal_Y
      ,0 as  ECredit_Debit
      ,0 as NUMEndBAL
      ,0 as BWBEndBAL
      ,0 as YBEndBAL
      ,Category
      ,Unit
      ,YearStartNumBAL
      ,YearStartYBBAL
      ,YearStartBWBBAL
      ,CreateUser
      ,CreateDate
      ,IsCalHelperValid
      ,IsQuantityValid
      ,IsDefaultCurrency
      ,IsLeaf
  FROM  TKS_FAS_GLBalance 
  where accountId=@AccountId and PeriodId=@PeriodId";


            #endregion

            if (oldPeriod.Month == 12)
            {
                #region 12月
                sql = @" insert into  TKS_FAS_GLBalance (Id
      ,AccountId
      ,SubjectId
      ,SubjectCode
      ,ParentId
      ,Name
      ,PeriodId
      ,Year
      ,CurrencyCode
      ,SCredit_Debit
      ,NUMStartBAL
      ,BWBStartBAL
      ,YBStartBAL
      ,NUMDebitTotal
      ,NUMDebitTotal_Y
      ,BWBDebitTotal
      ,BWBDebitTotal_Y
      ,YBDebitTotal
      ,YBDebitTotal_Y
      ,NUMCreditTotal
      ,NUMCreditTotal_Y
      ,BWBCreditTotal
      ,BWBCreditTotal_Y
      ,YBCreditTotal
      ,YBCreditTotal_Y
      ,ECredit_Debit
      ,NUMEndBAL
      ,BWBEndBAL
      ,YBEndBAL
      ,Category
      ,Unit
      ,YearStartNumBAL
      ,YearStartYBBAL
      ,YearStartBWBBAL
      ,CreateUser
      ,CreateDate
      ,IsCalHelperValid
      ,IsQuantityValid
      ,IsDefaultCurrency
      ,IsLeaf)
 
 select 
 newid()
      ,AccountId
      ,SubjectId
      ,SubjectCode
      ,ParentId
      ,Name
      ,@newPeriodId
      ,@Year
      ,CurrencyCode
      ,ECredit_Debit as SCredit_Debit
      ,NUMEndBAL as  NUMStartBAL
      ,BWBEndBAL as BWBStartBAL
      ,YBEndBAL as YBStartBAL
      ,0 as NUMDebitTotal
      ,0 as NUMDebitTotal_Y
      ,0 as BWBDebitTotal
      ,0 as BWBDebitTotal_Y
      ,0 as YBDebitTotal
      ,0 as YBDebitTotal_Y
      ,0 as NUMCreditTotal
      ,0 as NUMCreditTotal_Y
      ,0 as BWBCreditTotal
      ,0 as BWBCreditTotal_Y
      ,0 as YBCreditTotal
      ,0 as YBCreditTotal_Y
      ,0 as  ECredit_Debit
      ,0 as NUMEndBAL
      ,0 as BWBEndBAL
      ,0 as YBEndBAL
      ,Category
      ,Unit
      ,NUMEndBAL as  YearStartNumBAL
      ,YBEndBAL as YearStartYBBAL
      ,BWBEndBAL as YearStartBWBBAL
      ,CreateUser
      ,CreateDate
      ,IsCalHelperValid
      ,IsQuantityValid
      ,IsDefaultCurrency
      ,IsLeaf
  FROM  TKS_FAS_GLBalance 
  where accountId=@AccountId and PeriodId=@PeriodId";


                #endregion
            }

            cnn.Execute(sql, new
            {
                newPeriodId = newPeriod.Id,
                Year = newPeriod.Year,
                AccountId = newPeriod.AccountId
             ,
                PeriodId = oldPeriod.Id
            }, ts);

        }
        /// <summary>
        /// 新期间辅助核算余额初始数据加载
        /// </summary>
        /// <param name="newPeriod"></param>
        /// <param name="oldPeriod"></param>
        /// <param name="ts"></param>
        public void FBalanceAddup(TKS_FAS_MonthPeriodInfo newPeriod, TKS_FAS_MonthPeriodInfo oldPeriod, IDbTransaction ts)
        {
            #region 非12月
            string sql = @" 
INSERT INTO  TKS_FAS_FGLBalance
           (Id
           ,ParentId
           ,AccountId
           ,SubjectCode
           ,Name
           ,PeriodId
           ,Year
           ,CalItem1
           ,CalValue1
           ,CalItem2
           ,CalValue2
           ,CalItem3
           ,CalValue3
           ,CalItem4
           ,CalValue4
           ,CalItem5
           ,CalValue5
           ,CurrencyCode
           ,SCredit_Debit
           ,NUMStartBAL
           ,BWBStartBAL
           ,YBStartBAL
           ,NUMDebitTotal
           ,NUMDebitTotal_Y
           ,BWBDebitTotal
           ,BWBDebitTotal_Y
           ,YBDebitTotal
           ,YBDebitTotal_Y
           ,NUMCreditTotal
           ,NUMCreditTotal_Y
           ,BWBCreditTotal
           ,BWBCreditTotal_Y
           ,YBCreditTotal
           ,YBCreditTotal_Y
           ,ECredit_Debit
           ,NUMEndBAL
           ,BWBEndBAL
           ,YBEndBAL
           ,Category
           ,Unit
           ,YearStartNumBAL
           ,YearStartYBBAL
           ,YearStartBWBBAL
           ,CreateUser
           ,CreateDate
           ,IsDefaultCurrency
           ,IsQuantityValid,IsFirstPeriodAdd)
     select 
			newid()
           ,ParentId
           ,AccountId
           ,SubjectCode
           ,Name
           ,@newPeriodId
		   ,@Year
           ,CalItem1
           ,CalValue1
           ,CalItem2
           ,CalValue2
           ,CalItem3
           ,CalValue3
           ,CalItem4
           ,CalValue4
           ,CalItem5
           ,CalValue5
           ,CurrencyCode
          ,ECredit_Debit as SCredit_Debit
      ,NUMEndBAL as  NUMStartBAL
      ,BWBEndBAL as BWBStartBAL
      ,YBEndBAL as YBStartBAL
            ,0 as NUMDebitTotal
      ,NUMDebitTotal_Y
      ,0 as BWBDebitTotal
      ,BWBDebitTotal_Y
      ,0 as YBDebitTotal
      ,YBDebitTotal_Y
      ,0 as NUMCreditTotal
      ,NUMCreditTotal_Y
      ,0 as BWBCreditTotal
      ,BWBCreditTotal_Y
      ,0 as YBCreditTotal
      ,YBCreditTotal_Y
      ,0 as  ECredit_Debit
      ,0 as NUMEndBAL
      ,0 as BWBEndBAL
      ,0 as YBEndBAL
           ,Category
           ,Unit
           ,YearStartNumBAL
           ,YearStartYBBAL
           ,YearStartBWBBAL
           ,CreateUser
           ,CreateDate
           ,IsDefaultCurrency
           ,IsQuantityValid,
            1 as IsFirstPeriodAdd
 FROM  TKS_FAS_FGLBalance 
  where accountId=@AccountId and PeriodId=@PeriodId 
 
";
            #endregion

            if (oldPeriod.Month == 12)
            {
                #region 12月
                sql = @" 
INSERT INTO  TKS_FAS_FGLBalance
           (Id
           ,ParentId
           ,AccountId
           ,SubjectCode
           ,Name
           ,PeriodId
           ,Year
           ,CalItem1
           ,CalValue1
           ,CalItem2
           ,CalValue2
           ,CalItem3
           ,CalValue3
           ,CalItem4
           ,CalValue4
           ,CalItem5
           ,CalValue5
           ,CurrencyCode
           ,SCredit_Debit
           ,NUMStartBAL
           ,BWBStartBAL
           ,YBStartBAL
           ,NUMDebitTotal
           ,NUMDebitTotal_Y
           ,BWBDebitTotal
           ,BWBDebitTotal_Y
           ,YBDebitTotal
           ,YBDebitTotal_Y
           ,NUMCreditTotal
           ,NUMCreditTotal_Y
           ,BWBCreditTotal
           ,BWBCreditTotal_Y
           ,YBCreditTotal
           ,YBCreditTotal_Y
           ,ECredit_Debit
           ,NUMEndBAL
           ,BWBEndBAL
           ,YBEndBAL
           ,Category
           ,Unit
           ,YearStartNumBAL
           ,YearStartYBBAL
           ,YearStartBWBBAL
           ,CreateUser
           ,CreateDate
           ,IsDefaultCurrency
           ,IsQuantityValid,IsFirstPeriodAdd)
     select 
			newid()
           ,ParentId
           ,AccountId
           ,SubjectCode
           ,Name
           ,@newPeriodId
		   ,@Year
           ,CalItem1
           ,CalValue1
           ,CalItem2
           ,CalValue2
           ,CalItem3
           ,CalValue3
           ,CalItem4
           ,CalValue4
           ,CalItem5
           ,CalValue5
           ,CurrencyCode
          ,ECredit_Debit as SCredit_Debit
      ,NUMEndBAL as  NUMStartBAL
      ,BWBEndBAL as BWBStartBAL
      ,YBEndBAL as YBStartBAL
            ,0 as NUMDebitTotal
      ,0 as NUMDebitTotal_Y
      ,0 as BWBDebitTotal
      ,0 as BWBDebitTotal_Y
      ,0 as YBDebitTotal
      ,0 as YBDebitTotal_Y
      ,0 as NUMCreditTotal
      ,0 as NUMCreditTotal_Y
      ,0 as BWBCreditTotal
      ,0 as BWBCreditTotal_Y
      ,0 as YBCreditTotal
      ,0 as YBCreditTotal_Y
      ,0 as  ECredit_Debit
      ,0 as NUMEndBAL
      ,0 as BWBEndBAL
      ,0 as YBEndBAL
           ,Category
           ,Unit
           ,NUMEndBAL as YearStartNumBAL
           ,YBEndBAL as YearStartYBBAL
           ,BWBEndBAL as YearStartBWBBAL
           ,CreateUser
           ,CreateDate
           ,IsDefaultCurrency
           ,IsQuantityValid
           ,1 as IsFirstPeriodAdd
 FROM  TKS_FAS_FGLBalance 
  where accountId=@AccountId and PeriodId=@PeriodId 
 
";
                #endregion
            }

            cnn.Execute(sql, new
            {
                newPeriodId = newPeriod.Id,
                Year = newPeriod.Year,
                AccountId = newPeriod.AccountId,
                PeriodId = oldPeriod.Id
            }, ts);

        }

        /// <summary>
        /// 更新余额表 Add By Hero.Zhang 20180514
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ts"></param>
        /// <param name="user"></param>
        public void UpdateBalance(RequestDocAdd request, IDbTransaction ts, MM_UserInfo user)
        {
            #region modify by andy 期间根据凭证日期计算
            //AccountSetBLL account = new FAS.AccountSetBLL(cnn);
            //var period = account.GetActivePeriod(user.AccountId, ts);
            var pzDate = request.Head.PZDate;
            PeriodBLL periodBLL = new PeriodBLL(cnn);
            var period = periodBLL.GetPeriod(user.AccountId, pzDate?.Year ?? 0, pzDate?.Month ?? 0, ts);
            #endregion

            #region 后期余额受影响的期间
            //var nextPeriods = periodBLL.GetNextPeriods(user.AccountId, pzDate?.Year ?? 0, pzDate?.Month ?? 0, ts);
            #endregion
            string sql = @"select * from TKS_FAS_GLBalance where accountId=@AccountId and periodId=@PeriodId";

            var blanceData = cnn.Query<TKS_FAS_GLBalance>(sql, new { AccountId = user.AccountId, PeriodId = request.Head.PeriodId }, ts).ToList();
            //key 科目代码+币别
            Dictionary<string, TKS_FAS_GLBalance> dicBlance = new Dictionary<string, TKS_FAS_GLBalance>();

            foreach (var item in blanceData)
            {
                dicBlance.Add(item.SubjectCode + item.CurrencyCode, item);
            }
            List<TKS_FAS_DocDetail> docDetail = request.Detail;
            List<TKS_FAS_FGLBalance> saveData = new List<TKS_FAS_FGLBalance>();
            foreach (var item in docDetail)
            {
                if (string.IsNullOrEmpty(item.SubjectCode))
                    continue;
                //标准科目余额表，去除辅助核算
                if (item.IsCalHelper == 0)
                {
                    if (dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
                    {
                        //余额表存在,则更新
                        GLBalanceUpdate(dicBlance[item.SubjectCode + item.CurrencyCode], item, ts);
                    }
                    else
                    {
                        //不存在，新增一条
                        var b = GLBalanceAdd(item, ts);
                        var g = GLBalanceUpdate(b, item, ts);
                        if (!dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
                            dicBlance.Add(item.SubjectCode + item.CurrencyCode, g);

                    }
                }
                else if (item.IsCalHelper == 1)
                {
                    //更新辅助核算项余额表
                    FGLBalanceAdd(item, saveData, ts);//辅助核算项科目全部当新纪录插入核算余额表，状态为非期初录入
                }
            }
            foreach (var item in saveData)
            {
                if (string.IsNullOrEmpty(item.CalValue1))
                {
                    throw new NormalException("辅助核算数据异常");
                }
                cnn.Insert<TKS_FAS_FGLBalance>(item, ts);
            }
            if (saveData.Count > 0)
            {
                UpdateEffectDocDetailsFGL(period, ts);
                var effectPeriods = periodBLL.GetNextPeriods(period.AccountId, period?.Year ?? 0, period?.Month ?? 0, ts);
                foreach (var nextPeriod in effectPeriods)
                {
                    FGLBanceNextPeriodUpdate(nextPeriod, period, ts);
                    period = nextPeriod;
                }
            }

        }

        /// <summary>
        /// 更新余额表
        /// </summary>
        /// <param name="currentBalance"></param>
        /// <param name="newDoc"></param>
        /// <param name="ts"></param>
        /// <param name="effectPeriods">受影响的期间</param>
        public TKS_FAS_GLBalance GLBalanceUpdate(TKS_FAS_GLBalance currentBalance, TKS_FAS_DocDetail newDoc, IDbTransaction ts)
        {

            //#region 针对科目余额表数据 记下的数据与科目方向相反 对于科目为负的  跳转数据
            //if (currentBalance.SCredit_Debit == 0 && currentBalance.BWBCreditTotal < 0)
            //{
            //    currentBalance.BWBDebitTotal += -1 * currentBalance.BWBCreditTotal;
            //    currentBalance.BWBCreditTotal = 0;
            //}
            //if (currentBalance.SCredit_Debit == 1 && currentBalance.BWBDebitTotal < 0)
            //{
            //    currentBalance.BWBCreditTotal += -1 * currentBalance.BWBDebitTotal;
            //    currentBalance.BWBDebitTotal = 0;
            //}
            //#endregion


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

            string sql = @"UPDATE  TKS_FAS_GLBalance
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

            if (currentBalance.SubjectCode == "3131")
            {
                //sql = "update TKS_FAS_GLBalance set scredit_debit=1 where id=@Id";
                //cnn.Execute(sql, currentBalance, ts);
            }

            GLBanceEffectPeriodsUpdate(currentBalance, ts);
            return currentBalance;
        }
        public void GLBanceEffectPeriodsUpdate(TKS_FAS_GLBalance currentBalance, IDbTransaction ts)
        {
            if (string.IsNullOrEmpty(currentBalance.SubjectId) && !string.IsNullOrEmpty(currentBalance.SubjectCode))
            {
                var subject = cnn.QueryFirstOrDefault<TKS_FAS_AccountSubject>("select * from TKS_FAS_AccountSubject where AccountId=@AccountId  and Code=@Code", new { AccountId = currentBalance.AccountId, Code = currentBalance.SubjectCode }, ts);
                if (subject != null)
                {
                    currentBalance.SubjectId = subject.Id;
                    currentBalance.ParentId = subject.ParentId;
                    currentBalance.Name = subject.Name;
                    string sql = @"UPDATE  TKS_FAS_GLBalance
                       SET  SubjectId = @SubjectId,Name=@Name
                     WHERE id=@Id";
                    cnn.Execute(sql, currentBalance, ts);
                }
            }
#if DEBUG
            if (currentBalance.SubjectCode.Contains("5602014"))
            {

            }
#endif

            UpdateEffectDocDetails(currentBalance, ts);


            PeriodBLL periodBLL = new PeriodBLL(cnn);
            var period = periodBLL.GetPeriod(currentBalance.PeriodId, ts);
            var effectPeriods = periodBLL.GetNextPeriods(period.AccountId, period?.Year ?? 0, period?.Month ?? 0, ts);

            var balance = currentBalance;
            if (effectPeriods != null)
            {
                foreach (var nextPeriod in effectPeriods)
                {
                    var thisBalance = cnn.QueryFirstOrDefault<TKS_FAS_GLBalance>("select * from TKS_FAS_GLBalance where AccountId=@AccountId and SubjectId=@SubjectId and PeriodId=@PeriodId", new { AccountId = balance.AccountId, SubjectId = balance.SubjectId, PeriodId = nextPeriod.Id }, ts);
                    if (thisBalance == null)
                    {
                        thisBalance = new TKS_FAS_GLBalance();
                        thisBalance.Id = Guid.NewGuid().ToString("N");
                        thisBalance.SCredit_Debit = balance.SCredit_Debit;
                        thisBalance.AccountId = balance.AccountId;
                        thisBalance.PeriodId = nextPeriod.Id;
                        thisBalance.SubjectId = balance.SubjectId;
                        thisBalance.ParentId = balance.ParentId;
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
                    balance = GLBanceNextPeriodUpdate(balance, thisBalance, ts);
                }

            }
        }
        public TKS_FAS_GLBalance GLBanceNextPeriodUpdate(TKS_FAS_GLBalance lastBalance,
          TKS_FAS_GLBalance currentBalance, IDbTransaction ts)
        {
            var subject = cnn.QueryFirstOrDefault<TKS_FAS_AccountSubject>("select * from TKS_FAS_AccountSubject where AccountId=@AccountId  and Code=@Code", new { AccountId = currentBalance.AccountId, Code = currentBalance.SubjectCode }, ts);
            #region 期初=上期期末
            currentBalance.SCredit_Debit = lastBalance.ECredit_Debit;
            currentBalance.NUMStartBAL = lastBalance.NUMEndBAL;
            currentBalance.BWBStartBAL = lastBalance.BWBEndBAL;
            currentBalance.YBStartBAL = lastBalance.YBEndBAL;
            #endregion
            #region 借方
            //currentBalance.NUMDebitTotal = currentBalance.NUMDebitTotal;//不变
            currentBalance.NUMDebitTotal_Y = lastBalance.NUMDebitTotal_Y + currentBalance.NUMDebitTotal; //上期年度累计+当期发生
            //currentBalance.BWBDebitTotal = currentBalance.BWBDebitTotal;//不变
            currentBalance.BWBDebitTotal_Y = lastBalance.BWBDebitTotal_Y + currentBalance.BWBDebitTotal;
            //currentBalance.YBDebitTotal = currentBalance.YBDebitTotal;//不变
            currentBalance.YBDebitTotal_Y = lastBalance.YBDebitTotal_Y + currentBalance.YBDebitTotal;
            #endregion
            #region 贷方
            //currentBalance.NUMCreditTotal = currentBalance.NUMCreditTotal;//不变
            currentBalance.NUMCreditTotal_Y = lastBalance.NUMCreditTotal_Y + currentBalance.NUMCreditTotal; //上期年度累计+当期发生
            //currentBalance.BWBCreditTotal = currentBalance.BWBCreditTotal;//不变
            currentBalance.BWBCreditTotal_Y = lastBalance.BWBCreditTotal_Y + currentBalance.BWBCreditTotal;
            //currentBalance.YBCreditTotal = currentBalance.YBCreditTotal;//不变
            currentBalance.YBCreditTotal_Y = lastBalance.YBCreditTotal_Y + currentBalance.YBCreditTotal;
            #endregion
            #region 期末
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

            #endregion
            //if (lastBalance.SCredit_Debit != currentBalance.SCredit_Debit)
            //{
            //    lastBalance.YearStartNumBAL = -1 * lastBalance.YearStartNumBAL;
            //    lastBalance.YearStartYBBAL = -1 * lastBalance.YearStartYBBAL;
            //    lastBalance.YearStartBWBBAL = -1 * lastBalance.YearStartBWBBAL;
            //}
            #region 年初余额
            if (currentBalance.Year > lastBalance.Year && lastBalance.Year > 0)
            {
                if (subject.Credit_Debit == currentBalance.SCredit_Debit)
                {
                    currentBalance.YearStartNumBAL = lastBalance.NUMEndBAL;
                    currentBalance.YearStartYBBAL = lastBalance.YBEndBAL;
                    currentBalance.YearStartBWBBAL = lastBalance.BWBEndBAL;
                }
                else
                {
                    currentBalance.YearStartNumBAL = lastBalance.NUMEndBAL;
                    currentBalance.YearStartYBBAL = -1 * lastBalance.YBEndBAL;
                    currentBalance.YearStartBWBBAL = -1 * lastBalance.BWBEndBAL;
                }
#if DEBUG 
                if (currentBalance.SubjectCode.Contains("5602014"))
                {

                }
#endif
                currentBalance.BWBCreditTotal_Y = currentBalance.BWBCreditTotal;
                currentBalance.BWBDebitTotal_Y = currentBalance.BWBDebitTotal;
                currentBalance.YBCreditTotal_Y = currentBalance.YBCreditTotal;
                currentBalance.YBDebitTotal_Y = currentBalance.YBDebitTotal;
                currentBalance.NUMCreditTotal_Y = currentBalance.NUMCreditTotal;
                currentBalance.NUMDebitTotal_Y = currentBalance.NUMDebitTotal;
            }
            else
            {
                currentBalance.YearStartNumBAL = lastBalance.YearStartNumBAL;
                currentBalance.YearStartYBBAL = lastBalance.YearStartYBBAL;
                currentBalance.YearStartBWBBAL = lastBalance.YearStartBWBBAL;
            }

            #endregion
            string sql = @"UPDATE  TKS_FAS_GLBalance
                       SET SCredit_Debit =@SCredit_Debit
                          ,ECredit_Debit =@ECredit_Debit
                          ,NUMStartBAL = @NUMStartBAL
                          ,BWBStartBAL = @BWBStartBAL
                          ,YBStartBAL = @YBStartBAL

                          ,NUMDebitTotal_Y =@NUMDebitTotal_Y
                          ,BWBDebitTotal_Y = @BWBDebitTotal_Y
                          ,YBDebitTotal_Y =@YBDebitTotal_Y

                          ,NUMCreditTotal_Y = @NUMCreditTotal_Y
                          ,BWBCreditTotal_Y = @BWBCreditTotal_Y
                          ,YBCreditTotal_Y =@YBCreditTotal_Y
                          
                          ,NUMEndBAL =@NUMEndBAL
                          ,BWBEndBAL = @BWBEndBAL
                          ,YBEndBAL = @YBEndBAL

                          ,YearStartNumBAL = @YearStartNumBAL
                          ,YearStartYBBAL =@YearStartYBBAL
                          ,YearStartBWBBAL =@YearStartBWBBAL
                     WHERE id=@Id";
            cnn.Execute(sql, currentBalance, ts);

            if (currentBalance.SubjectCode == "3131")
            {
                //sql = "update TKS_FAS_GLBalance set scredit_debit=1 where id=@Id";
                //cnn.Execute(sql, currentBalance, ts);
            }

            UpdateEffectDocDetails(currentBalance, ts);
            return currentBalance;
        }
        public TKS_FAS_GLBalance GLBanceCurrentPeriodUpdate(TKS_FAS_GLBalance currentBalance, IDbTransaction ts)
        {
            TKS_FAS_GLBalance lastBalance = null;
            var periodBLL = new PeriodBLL(cnn);
            var currentPeriod = periodBLL.GetPeriod(currentBalance.PeriodId, ts);
            var prePeriod = periodBLL.GetPrePeriod(currentPeriod, ts);


            if (prePeriod == null)
            {
                var qichuBalance = cnn.QueryFirstOrDefault<TKS_FAS_GLBalance>("select * from TKS_FAS_GLBalance where SubjectId=@SubjectId and PeriodId=@PeriodId  and AccountId=@AccountId", new { SubjectId = currentBalance.SubjectId, AccountId = currentBalance.AccountId, PeriodId = "" }, ts);
                if(qichuBalance == null)
                {
                    return null;
                }
                lastBalance = new TKS_FAS_GLBalance();
                lastBalance.AccountId = qichuBalance.AccountId;
                lastBalance.SubjectCode = qichuBalance.SubjectCode;
                lastBalance.SubjectId = qichuBalance.SubjectId;
                lastBalance.SCredit_Debit = qichuBalance.SCredit_Debit;
                lastBalance.Name = qichuBalance.Name;
                lastBalance.IsLeaf = qichuBalance.IsLeaf;
                lastBalance.IsDefaultCurrency = qichuBalance.IsDefaultCurrency;
                lastBalance.IsCalHelperValid = qichuBalance.IsCalHelperValid;
                lastBalance.IsQuantityValid = qichuBalance.IsQuantityValid;
                lastBalance.Year = qichuBalance.Year;
                lastBalance.Category = qichuBalance.Category;
                lastBalance.CurrencyCode = qichuBalance.CurrencyCode;

                lastBalance.NUMStartBAL = qichuBalance.NUMStartBAL;
                lastBalance.BWBStartBAL = qichuBalance.BWBStartBAL;
                lastBalance.YBStartBAL = qichuBalance.YBStartBAL;
                lastBalance.NUMDebitTotal_Y = qichuBalance.NUMDebitTotal_Y;
                lastBalance.BWBDebitTotal_Y = qichuBalance.BWBDebitTotal_Y;
                lastBalance.YBDebitTotal_Y = qichuBalance.YBDebitTotal_Y;
                lastBalance.NUMCreditTotal_Y = qichuBalance.NUMCreditTotal_Y;
                lastBalance.BWBCreditTotal_Y = qichuBalance.BWBCreditTotal_Y;
                lastBalance.YBCreditTotal_Y = qichuBalance.YBCreditTotal_Y;
                lastBalance.YearStartNumBAL = qichuBalance.YearStartNumBAL;
                lastBalance.YearStartYBBAL = qichuBalance.YearStartYBBAL;
                lastBalance.YearStartBWBBAL = qichuBalance.YearStartBWBBAL;
                lastBalance.ECredit_Debit = qichuBalance.SCredit_Debit;
                lastBalance.NUMEndBAL = lastBalance.NUMStartBAL;
                lastBalance.BWBEndBAL = lastBalance.BWBStartBAL;
                lastBalance.YBEndBAL = lastBalance.YBStartBAL;
                //年初  年度累计  自动清0
                if (currentPeriod.Month == 1)
                {
                    lastBalance.NUMCreditTotal_Y = 0;
                    lastBalance.NUMDebitTotal_Y = 0;
                    lastBalance.YBCreditTotal_Y = 0;
                    lastBalance.YBCreditTotal_Y = 0;
                    lastBalance.BWBDebitTotal_Y = 0;
                    lastBalance.BWBCreditTotal_Y = 0;
                    lastBalance.YearStartBWBBAL = lastBalance.BWBStartBAL;
                    lastBalance.YearStartNumBAL = lastBalance.NUMStartBAL;
                    lastBalance.YearStartYBBAL = lastBalance.NUMStartBAL;
                }
            }
            else
            {
                lastBalance = cnn.QueryFirstOrDefault<TKS_FAS_GLBalance>("select * from TKS_FAS_GLBalance where SubjectId=@SubjectId and PeriodId=@PeriodId  and AccountId=@AccountId", new { SubjectId = currentBalance.SubjectId, AccountId = currentBalance.AccountId, PeriodId = (prePeriod == null ? "" : prePeriod.Id) }, ts);
            }
            //add by Hero.Zhang 上期没有直接取消后续操作
            if (lastBalance == null)
            {
                return null;
            }
            return GLBanceNextPeriodUpdate(lastBalance, currentBalance, ts);
        }
        /// <summary>
        /// 凭证明细科目增加 期初和期末
        /// </summary>
        /// <param name="currentBalance"></param>
        /// <param name="ts"></param>
        public void UpdateEffectDocDetails(TKS_FAS_GLBalance currentBalance, IDbTransaction ts)
        {
#if DEBUG
            if (currentBalance.SubjectCode == "4105001")
            {

            }
#endif
            cnn = ts.Connection;
            var docDetailList = cnn.Query<TKS_FAS_DocDetail>(@"
    select det.* from TKS_FAS_DocDetail det
    left join TKS_FAS_Doc doc on det.ParentId=doc.Id
    where det.AccountId=@AccountId
    and det.PeriodId=@PeriodId
    and det.SubjectCode=@SubjectCode
    order by doc.CreateDate,det.Seq ", new { AccountId = currentBalance.AccountId, PeriodId = currentBalance.PeriodId, SubjectCode = currentBalance.SubjectCode }, ts).ToList();
            var subject = cnn.QueryFirstOrDefault<TKS_FAS_AccountSubject>("select * from TKS_FAS_AccountSubject where AccountId=@AccountId  and Code=@Code", new { AccountId = currentBalance.AccountId, Code = currentBalance.SubjectCode }, ts);
            if (subject == null) return;
            var starBAL = subject.Credit_Debit == currentBalance.SCredit_Debit ? currentBalance.BWBStartBAL : -1 * currentBalance.BWBStartBAL;
            foreach (var doc in docDetailList)
            {
                doc.StartBAL = starBAL;
                var currentBAL = (doc.Credit_Debit == 1 ? 1 : -1) * (doc.Money_Credit - doc.Money_Debit);
                doc.EndBAL = starBAL + (subject.Credit_Debit == doc.Credit_Debit ? currentBAL : -1 * currentBAL);
                starBAL = doc.EndBAL;

                string sql = @"UPDATE  TKS_FAS_DocDetail
                       SET  
                          StartBAL =@StartBAL
                          ,EndBAL = @EndBAL
                     WHERE id=@Id";
                cnn.Execute(sql, doc, ts);
            }
        }

        /// <summary>
        /// 凭证明细科目增加 期初和期末
        /// </summary>
        /// <param name="currentBalance"></param>
        /// <param name="ts"></param>
        public void UpdateEffectDocDetailsFGL(TKS_FAS_MonthPeriodInfo period, IDbTransaction ts)
        {
            cnn = ts.Connection;

            var subjects = cnn.Query<dynamic>(@"select distinct SubjectCode from TKS_FAS_FGLBalance
                          where AccountId=@AccountId and PeriodId=@PeriodId ", new { AccountId = period.AccountId, PeriodId = period.Id }, ts);
            foreach (var sub in subjects)
            {
                try
                {
                    var subject = cnn.QueryFirstOrDefault<TKS_FAS_AccountSubject>("select * from TKS_FAS_AccountSubject where AccountId=@AccountId  and Code=@Code", new { AccountId = period.AccountId, Code = sub.SubjectCode }, ts);
                    if (subject == null) return;
                    var starBAL = cnn.QueryFirstOrDefault<decimal?>(@"select sum( case when ECredit_Debit = @Credit_Debit then BWBEndBAL else -1*BWBEndBAL end)    
                                        from TKS_FAS_FGLBalance
                                        where AccountId=@AccountId and PeriodId=@PeriodId and IsFirstPeriodAdd =1 and SubjectCode=@SubjectCode ", new { Credit_Debit = subject.Credit_Debit, AccountId = period.AccountId, PeriodId = period.Id, SubjectCode = sub.SubjectCode }, ts) ?? 0;


                    var docDetailList = cnn.Query<TKS_FAS_DocDetail>(@"
    select det.* from TKS_FAS_DocDetail det
    left join TKS_FAS_Doc doc on det.ParentId=doc.Id
    where det.AccountId=@AccountId
    and det.PeriodId=@PeriodId
    and det.SubjectCode=@SubjectCode and det.IsCalHelper=1
    order by doc.CreateDate,det.Seq ", new { AccountId = period.AccountId, PeriodId = period.Id, SubjectCode = sub.SubjectCode }, ts).ToList();
                    foreach (var doc in docDetailList)
                    {
                        doc.StartBAL = starBAL;
                        var currentBAL = (doc.Credit_Debit == 1 ? 1 : -1) * (doc.Money_Credit - doc.Money_Debit);
                        doc.EndBAL = starBAL + (subject.Credit_Debit == doc.Credit_Debit ? currentBAL : -1 * currentBAL);
                        starBAL = doc.EndBAL;

                        string sql = @"UPDATE  TKS_FAS_DocDetail
                       SET  
                          StartBAL =@StartBAL
                          ,EndBAL = @EndBAL
                     WHERE id=@Id";
                        cnn.Execute(sql, doc, ts);
                    }
                }
                catch (Exception ex)
                {

                }


            }



        }
        /// <summary>
        /// 新增余额
        /// </summary>
        /// <param name="newDoc"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public TKS_FAS_GLBalance GLBalanceAdd(TKS_FAS_DocDetail newDoc, IDbTransaction ts)
        {
            TKS_FAS_GLBalance newAdd = new TKS_FAS_GLBalance();
            newAdd.Id = Guid.NewGuid().ToString("N");

            TKS_FAS_AccountSubject subject = GetSubjectBySubjectCode(newDoc, ts);
            newAdd.SCredit_Debit = subject.Credit_Debit;
            newAdd.AccountId = newDoc.AccountId;
            newAdd.PeriodId = newDoc.PeriodId;
            newAdd.SubjectCode = newDoc.SubjectCode;
            newAdd.Name = subject.Name;
            newAdd.IsLeaf = subject.IsLeaf;
            newAdd.IsDefaultCurrency = newDoc.CurrencyCode == "RMB" ? 1 : 0;
            newAdd.IsCalHelperValid = newDoc.IsCalHelper;
            newAdd.IsQuantityValid = newDoc.IsQuantity;
            newAdd.Year = newDoc.Year;
            newAdd.Category = subject.Category;
            newAdd.CurrencyCode = newDoc.CurrencyCode;
            cnn.Insert<TKS_FAS_GLBalance>(newAdd, ts);
            return newAdd;
        }

        /// <summary>
        /// 辅助核算项余额表新增
        /// </summary>
        /// <param name="currentBalance"></param>
        /// <param name="newDoc"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private void FGLBalanceAdd(TKS_FAS_DocDetail newDoc, List<TKS_FAS_FGLBalance> saveData, IDbTransaction ts)
        {
            TKS_FAS_FGLBalance currentBalance = new TKS_FAS_FGLBalance();


            TKS_FAS_AccountSubject subject = GetSubjectBySubjectCode(newDoc, ts);
            currentBalance.SCredit_Debit = subject.Credit_Debit;

            //年度是累加的，年初是确定后就不变的，本月的是当前月，期末是公式计算

            //Add By Hero.Zhang 
            //if (newDoc.Credit_Debit != currentBalance.SCredit_Debit)
            //{
            //    newDoc.Quantity = -1 * newDoc.Quantity;
            //    newDoc.YB = -1 * newDoc.YB;
            //    newDoc.Money_Debit = -1 * newDoc.Money_Debit;
            //}
            //按凭证的借贷方向进行操作
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


            currentBalance.Id = Guid.NewGuid().ToString();
            currentBalance.CalItem1 = newDoc.CalItem1;
            currentBalance.CalItem2 = newDoc.CalItem2;
            currentBalance.CalItem3 = newDoc.CalItem3;
            currentBalance.CalItem4 = newDoc.CalItem4;
            currentBalance.CalItem5 = newDoc.CalItem5;
            currentBalance.CalValue1 = newDoc.CalValue1;
            currentBalance.CalValue2 = newDoc.CalValue2;
            currentBalance.CalValue3 = newDoc.CalValue3;
            currentBalance.CalValue4 = newDoc.CalValue4;
            currentBalance.CalValue5 = newDoc.CalValue5;
            currentBalance.SubjectCode = newDoc.SubjectCode;
            currentBalance.AccountId = newDoc.AccountId;
            currentBalance.PeriodId = newDoc.PeriodId;
            currentBalance.Year = newDoc.Year;
            currentBalance.Category = subject.Category;
            currentBalance.ParentId = subject.Id;
            currentBalance.CurrencyCode = newDoc.CurrencyCode;
            currentBalance.Name = newDoc.SubjectDescription.Trim();
            currentBalance.IsFirstPeriodAdd = 0;
            currentBalance.IsDefaultCurrency = newDoc.CurrencyCode == "RMB" ? 1 : 0;

            saveData.Add(currentBalance);


        }

        public void FGLBanceNextPeriodUpdate(TKS_FAS_MonthPeriodInfo nextPeriod, TKS_FAS_MonthPeriodInfo currentPeriod, IDbTransaction ts)
        {
            cnn.Execute("delete from TKS_FAS_FGLBalance where  PeriodId=@PeriodId and IsFirstPeriodAdd=1",
                        new { PeriodId = nextPeriod.Id }, ts);
            FBalanceAddup(nextPeriod, currentPeriod, ts);

            UpdateEffectDocDetailsFGL(nextPeriod, ts);
        }
        /// <summary>
        /// 损益结转 更新至余额表
        /// </summary>
        /// <param name="periodInfo"></param>
        /// <param name="ts"></param>
        public void SYBalanceUpdate(TKS_FAS_MonthPeriodInfo period, IDbTransaction ts, bool del = false)
        {
            string sql = @"select * from TKS_FAS_GLBalance where accountId=@AccountId and periodId=@PeriodId";

            var blanceData = cnn.Query<TKS_FAS_GLBalance>(sql, new { AccountId = period.AccountId, PeriodId = period.Id }, ts).ToList();
            Dictionary<string, TKS_FAS_GLBalance> dicBlance = new Dictionary<string, TKS_FAS_GLBalance>();
            foreach (var item in blanceData)
            {
                dicBlance.Add(item.SubjectCode + item.CurrencyCode, item);
            }


            sql = @"select * from TKS_FAS_DocDetail where periodId=@PeriodId
                             and source='SY' ";
            var data = cnn.Query<TKS_FAS_DocDetail>(sql, new { PeriodId = period.Id }, ts).ToList();
            sql = @"select * from TKS_FAS_Doc where periodId=@PeriodId
                             and source='SY' ";
            var doc = cnn.Query<TKS_FAS_Doc>(sql, new { PeriodId = period.Id }, ts).FirstOrDefault();
            #region 后期余额受影响的期间

            #endregion
            foreach (var item in data)
            {
                if (del)
                {
                    item.Money_Debit = -1 * item.Money_Debit;
                    item.Money_Credit = -1 * item.Money_Credit;
                    item.Quantity = -1 * item.Quantity;
                }

                if (period.Month == 12 && item.SubjectCode == "3141105")
                {
                    if (dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
                    {
                        GLBalanceUpdate(dicBlance[item.SubjectCode + item.CurrencyCode], item, ts);
                    }
                    else
                    {
                        var b = GLBalanceAdd(item, ts);
                        var g = GLBalanceUpdate(b, item, ts);
                        if (!dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
                            dicBlance.Add(item.SubjectCode + item.CurrencyCode, g);
                    }
                }
                else
                {
                    //if (item.SubjectCode == "3131")
                    {
                        if (dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
                        {
                            //余额表存在,则更新
                            GLBalanceUpdate(dicBlance[item.SubjectCode + item.CurrencyCode], item, ts);
                        }
                        else
                        {
                            var b = GLBalanceAdd(item, ts);
                            var g = GLBalanceUpdate(b, item, ts);
                            if (!dicBlance.ContainsKey(item.SubjectCode + item.CurrencyCode))
                                dicBlance.Add(item.SubjectCode + item.CurrencyCode, g);
                        }

                    }
                }
            }
        }
        /// <summary>
        /// 成本 更新至余额表
        /// </summary>
        /// <param name="periodInfo"></param>
        /// <param name="ts"></param>
        public void SCBalanceUpdate(TKS_FAS_MonthPeriodInfo period, IDbTransaction ts, MM_UserInfo user, bool del = false)
        {
            string sql = @"select * from TKS_FAS_DocDetail where periodId=@PeriodId
                             and source='SC' ";
            var data = cnn.Query<TKS_FAS_DocDetail>(sql, new { PeriodId = period.Id }, ts).ToList();
            sql = @"select * from TKS_FAS_Doc where periodId=@PeriodId
                             and source='SC' ";
            var headData = cnn.Query<TKS_FAS_Doc>(sql, new { PeriodId = period.Id }, ts).ToList();
            foreach (var head in headData)
            {
                RequestDocAdd docRequest = new RequestDocAdd();
                docRequest.Head = head;
                docRequest.Detail = data.Where(p => p.ParentId == head.Id).ToList();
                if (del)
                {

                    foreach (var det in docRequest.Detail)
                    {
                        det.Money_Debit = -1 * det.Money_Debit;
                        det.Money_Credit = -1 * det.Money_Credit;
                        det.Quantity = -1 * det.Quantity;
                    }
                }

                UpdateBalance(docRequest, ts, user);
            }

        }
        private TKS_FAS_AccountSubject GetSubjectBySubjectCode(TKS_FAS_DocDetail newDoc, IDbTransaction ts)
        {
            string sql = @"select * from tks_fas_AccountSubject where 
                    code=@Code and accountId=@AccountId ";

            var data = cnn.Query<TKS_FAS_AccountSubject>(sql,
                new
                {
                    Code = newDoc.SubjectCode,
                    AccountId = newDoc.AccountId,

                }, ts).ToList();

            if (data.Count > 0)
            {
                return data[0];
            }
            else
            {
                throw new AppException("", "GetBalBySubjectCode", "期末结转异常",
                    "期末结转：辅助核算项余额表插入异常，账套ID：" + newDoc.AccountId + "，凭证明细ID:" + newDoc.Id + @"
                    科目代码：" + newDoc.SubjectCode + "没有找到科目");
            }
        }
        /// <summary>
        /// 获取科目的本期发生额
        /// </summary>
        /// <param name="period"></param>
        /// <param name="IDbTransaction"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public decimal GetSubjectCurrentOccurrenceAmount(TKS_FAS_MonthPeriodInfo period, string subjectCode, IDbTransaction ts)
        {
            decimal amount = 0;
            SubjectBLL subjectBLL = new SubjectBLL(ts.Connection);
            var subject = subjectBLL.GetSubject(subjectCode, period.AccountId, ts);
            var childres = subjectBLL.GetChildrenSubjects(subject.Id, period.AccountId, ts);
            if (childres.Count > 0)
            {

            }
            else
            {

            }
            return amount;
        }
    }
}
