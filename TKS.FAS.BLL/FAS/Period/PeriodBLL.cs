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
    public class PeriodBLL : CommonBase
    {
        public PeriodBLL()
        {

        }

        public PeriodBLL(IDbConnection cnn)
        {
            this.cnn = cnn;
        }

        /// <summary>
        /// 获取账套下的已结转的期间
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponsePeriodPaidGet PeriodPaidGet(RequestPeriodPaidGet request)
        {
            ResponsePeriodPaidGet res = new ResponsePeriodPaidGet();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);

                    //string sql = "select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId and isPay=1 order by startDate";
                    //modify by andy 未结转也可查看数据
                    string sql = "select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId order by startDate";
                    var data = cnn.Query<TKS_FAS_MonthPeriodInfo>(sql, new { AccountId = user.AccountId }, ts).ToList();

                    sql = @"select * from tks_fas_accountInfo where id=@Id";

                    var account = cnn.QueryFirst<TKS_FAS_AccountInfo>(sql, new { Id = user.AccountId }, ts);

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "加载成功";
                    res.Account = account;
                    res.Data = data;
                    return res;
                }
                catch (Exception ex)
                {
                    return this.DealException(res, ex) as ResponsePeriodPaidGet;
                }
            }
        }

        public ResponsePeriodPaidGet WX_PeriodPaidGet(RequestPeriodPaidGet request)
        {
            ResponsePeriodPaidGet res = new ResponsePeriodPaidGet();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {


                    //string sql = "select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId and isPay=1 order by startDate";
                    //modify by andy 未结转也可查看数据
                    string sql = "select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId order by startDate";
                    var data = cnn.Query<TKS_FAS_MonthPeriodInfo>(sql, new { AccountId = request.AccountId }, ts).ToList();

                    sql = @"select * from tks_fas_accountInfo where id=@Id";

                    var account = cnn.QueryFirst<TKS_FAS_AccountInfo>(sql, new { Id = request.AccountId }, ts);

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "加载成功";
                    res.Account = account;
                    res.Data = data;
                    return res;
                }
                catch (Exception ex)
                {
                    return this.DealException(res, ex) as ResponsePeriodPaidGet;
                }
            }
        }

        /// <summary>
        /// 获取账套下的所有的期间
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponsePeriodPaidGet PeriodGet(RequestPeriodPaidGet request)
        {
            ResponsePeriodPaidGet res = new ResponsePeriodPaidGet();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);

                    string sql = "select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId order by startDate ";

                    var data = cnn.Query<TKS_FAS_MonthPeriodInfo>(sql, new { AccountId = user.AccountId }, ts).ToList();

                    sql = @"select * from tks_fas_accountInfo where id=@Id";

                    var account = cnn.QueryFirst<TKS_FAS_AccountInfo>(sql, new { Id = user.AccountId }, ts);

                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "加载成功";
                    res.Account = account;
                    res.Data = data;
                    return res;
                }
                catch (Exception ex)
                {
                    return this.DealException(res, ex) as ResponsePeriodPaidGet;
                }
            }
        }

        /// <summary>
        /// 期间是否结转
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool IsPeriodPaid(string periodId, IDbTransaction ts)
        {
            string sql = "select * from TKS_FAS_MonthPeriodInfo where id=@Id and isPay=1";

            var data = cnn.Query(sql, new { Id = periodId }, ts);

            if (data.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 期间是否存在
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool IsHasPeriod(string periodId, IDbTransaction ts)
        {
            string sql = @"select * from TKS_FAS_MonthPeriodInfo where id=@Id";
            var data = cnn.Query(sql, new { Id = periodId }, ts);
            if (data.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取当前期间的上一个期间
        /// </summary>
        /// <param name="curPeriod"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public TKS_FAS_MonthPeriodInfo GetPrePeriod(TKS_FAS_MonthPeriodInfo curPeriod, IDbTransaction ts)
        {
            int month = 1;
            int year = 1997;
            if (curPeriod.Month > 1)
            {
                month = curPeriod.Month - 1;
                year = curPeriod.Year;
            }
            else
            {
                month = 12;
                year = curPeriod.Year - 1;
            }

            string sql = @"select * from TKS_FAS_MonthPeriodInfo 
                            where accountId=@AccountId and year=@Year and month=@Month";

            var data = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>
                (sql, new { AccountId = curPeriod.AccountId, Year = year, Month = month }, ts);

            return data;
        }
        public TKS_FAS_MonthPeriodInfo CetNextPeriod(TKS_FAS_MonthPeriodInfo curPeriod, IDbTransaction ts)
        {
            int month = 1;
            int year = 1997;
            if (curPeriod.Month == 12)
            {
                month = 1;
                year = curPeriod.Year + 1;
            }
            else
            {
                month = curPeriod.Month + 1;
                year = curPeriod.Year;
            }

            string sql = @"select * from TKS_FAS_MonthPeriodInfo 
                            where accountId=@AccountId and year=@Year and month=@Month";

            var data = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>
                (sql, new { AccountId = curPeriod.AccountId, Year = year, Month = month }, ts);
            if (data != null)
            {
                return data;
            }
            return null;
        }
        public TKS_FAS_MonthPeriodInfo CreateNextPeriod(TKS_FAS_MonthPeriodInfo curPeriod, IDbTransaction ts)
        {
            BalanceBLL balanceBLL = new BalanceBLL(cnn);
            int month = 1;
            int year = 1997;
            if (curPeriod.Month == 12)
            {
                month = 1;
                year = curPeriod.Year + 1;
            }
            else
            {
                month = curPeriod.Month + 1;
                year = curPeriod.Year;
            }

            string sql = @"select * from TKS_FAS_MonthPeriodInfo 
                            where accountId=@AccountId and year=@Year and month=@Month";

            var data = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>
                (sql, new { AccountId = curPeriod.AccountId, Year = year, Month = month }, ts);
            if (data != null)
            {
                //balanceBLL.BalanceAddup(data, curPeriod, ts);
                return data;
            }
            else
            {
                #region 插入新的期间

                sql = @"select * from tks_fas_periodTrack where fromId=@FromId";
                var trackData = cnn.QueryFirstOrDefault<TKS_FAS_PeriodTrack>(sql, new { FromId = curPeriod.Id }, ts);

                TKS_FAS_MonthPeriodInfo newPeriod = new TKS_FAS_MonthPeriodInfo();
                if (trackData == null)
                {
                    newPeriod.Id = Guid.NewGuid().ToString("N");
                }
                else
                {
                    newPeriod.Id = trackData.ToId;
                }

                newPeriod.AccountId = curPeriod.AccountId;
                newPeriod.IsActive = 0;
                newPeriod.IsPay = 0;

                if (curPeriod.Month == 12)
                {
                    newPeriod.Month = 1;
                    newPeriod.Year = curPeriod.Year + 1;
                    //如果是12月份，则把年度期间也更新为激活，插入新的年度期间
                    sql = @"update TKS_FAS_YearPeriodInfo set isClose=1,isActive=0 where id=@Id and isClose=0 and isActive=1";
                    cnn.Execute(sql, new { Id = curPeriod.ParentId }, ts);

                    TKS_FAS_YearPeriodInfo newYearPeriod = new TKS_FAS_YearPeriodInfo();
                    newYearPeriod.Id = Guid.NewGuid().ToString("N");
                    newYearPeriod.IsActive = 0;
                    newYearPeriod.IsClose = 0;
                    newYearPeriod.AccountId = curPeriod.AccountId;
                    newYearPeriod.StartDate = DateTime.Parse(newPeriod.Year + "-01-01");
                    newYearPeriod.EndDate = DateTime.Parse(newPeriod.Year + "-12-31 23:59:59");
                    newYearPeriod.PeriodNUM = 12;
                    newYearPeriod.Year = newPeriod.Year;
                    cnn.Insert<TKS_FAS_YearPeriodInfo>(newYearPeriod, ts);

                    newPeriod.ParentId = newYearPeriod.Id;
                }
                else
                {
                    newPeriod.ParentId = curPeriod.ParentId;
                    newPeriod.Month = curPeriod.Month + 1;
                    newPeriod.Year = curPeriod.Year;


                }
                newPeriod.StartDate = DateTime.Parse(newPeriod.Year + "-" + newPeriod.Month + "-01");
                newPeriod.EndDate = newPeriod.StartDate.Value.AddMonths(1).AddSeconds(-1);
                cnn.Insert<TKS_FAS_MonthPeriodInfo>(newPeriod, ts);
                if (trackData == null)
                {
                    //期间历史
                    TKS_FAS_PeriodTrack track = new TKS_FAS_PeriodTrack();
                    track.Id = Guid.NewGuid().ToString("N");
                    track.FromId = curPeriod.Id;
                    track.ToId = newPeriod.Id;
                    track.FromYear = curPeriod.Year.ToString();
                    track.ToYear = newPeriod.Year.ToString();
                    track.FromMonth = curPeriod.Month.ToString();
                    track.ToMonth = newPeriod.Month.ToString();
                    track.AccountId = curPeriod.AccountId;
                    cnn.Insert<TKS_FAS_PeriodTrack>(track, ts);

                }

                #endregion

                balanceBLL.BalanceAddup(newPeriod, curPeriod, ts);
                balanceBLL.FBalanceAddup(newPeriod, curPeriod, ts);
                return newPeriod;
            }

        }

        public TKS_FAS_MonthPeriodInfo GetPeriod(string periodId, IDbTransaction ts)
        {
            string sql = @"select * from TKS_FAS_MonthPeriodInfo 
                            where id=@PeriodId";

            var data = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>
                (sql, new { PeriodId = periodId }, ts);
            if (data != null)
            {
                return data;
            }
            return null;
        }

        public TKS_FAS_MonthPeriodInfo GetPeriod(string accountId, int year, int month, IDbTransaction ts)
        {
            var period = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>("select * from TKS_FAS_MonthPeriodInfo where Year=@Year and AccountId=@AccountId and Month=@Month", new { Year = year, Month = month, AccountId = accountId }, ts);
            if (period != null)
            {
                return period;
            }
            AccountSetBLL account = new FAS.AccountSetBLL(cnn);
            var activePeriod = account.GetActivePeriod(accountId, ts);
            var nextPeriod = activePeriod;
            do
            {
                nextPeriod = CreateNextPeriod(nextPeriod, ts);
                if (nextPeriod.Month == month && nextPeriod.Year == year)
                {
                    period = nextPeriod;
                    break;
                }
            } while (true);
            return period;
        }
        public List<TKS_FAS_MonthPeriodInfo> GetPrePeriods(string accountId, int year, int month, IDbTransaction ts)
        {
            var periods = cnn.Query<TKS_FAS_MonthPeriodInfo>("select * from TKS_FAS_MonthPeriodInfo where ((Year=@Year and Month<@Month) or (Year<@Year) ) and AccountId=@AccountId order by year ,month", new { Year = year, Month = month, AccountId = accountId }, ts).ToList();
            return periods;
        }
        public List<TKS_FAS_MonthPeriodInfo> GetNextPeriods(string accountId, int year, int month, IDbTransaction ts)
        {
            //var nextDate = new DateTime(year, month, 1).AddMonths(1);
            var periods = cnn.Query<TKS_FAS_MonthPeriodInfo>("select * from TKS_FAS_MonthPeriodInfo where ((Year=@Year and Month>@Month) or (Year>@Year) ) and AccountId=@AccountId order by year ,month", new { Year = year, Month = month, AccountId = accountId }, ts).ToList();
            return periods;
        }
    }
}
