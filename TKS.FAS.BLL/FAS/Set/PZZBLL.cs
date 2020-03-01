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
    public class PZZBLL : CommonBase
    {
        public PZZBLL() { }

        public PZZBLL(IDbConnection cnn)
        {
            this.cnn = cnn;
        }
        public TKS_FAS_CertificateWord PZZGetById(string id, IDbTransaction ts)
        {
            var data = cnn.QuerySingle<TKS_FAS_CertificateWord>("select * from TKS_FAS_CertificateWord where id=@Id", new
            {
                Id = id
            }, ts);
            return data;
        }
        public ResponsePZZListSearch PZZListSearch(RequestPZZListSearch request)
        {

            ResponsePZZListSearch response = new ResponsePZZListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = this.GetPageSql("*",
                        @"  TKS_FAS_CertificateWord where accountId=@AccountId",
                        " isDefault desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_CertificateWord> data = cnn.Query<TKS_FAS_CertificateWord>(sql,
                        new
                        {
                            AccountId = user.AccountId
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_CertificateWord where accountId=@AccountId ";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = user.AccountId
                    }, ts).ToString());

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

                    return this.DealException(response, ex) as ResponsePZZListSearch;
                }
            }
        }

        public ResponsePZZTotalGet PZZTotalGet(RequestPZZTotalGet request)
        {

            ResponsePZZTotalGet response = new ResponsePZZTotalGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = "select * from TKS_FAS_CertificateWord where accountId=@AccountId order by isdefault desc";

                    List<TKS_FAS_CertificateWord> data = cnn.Query<TKS_FAS_CertificateWord>(sql,
                        new
                        {
                            AccountId = user.AccountId
                        }, ts).ToList();

                    AccountSetBLL account = new AccountSetBLL(cnn);
                    PeriodBLL periodBLL = new PeriodBLL(cnn);
                    var activePeriod = account.GetActivePeriod(user.AccountId, ts);
                    sql = @"select top 1 * from TKS_FAS_Doc 
                            where accountid=@AccountId and PZDate>=@StartDate order by CreateDate desc ";
                    var doc = cnn.QueryFirstOrDefault<TKS_FAS_Doc>(sql,
                        new
                        {
                            AccountId = user.AccountId,
                            StartDate = activePeriod.StartDate,
                        }, ts);
                    var period = new TKS_FAS_MonthPeriodInfo();
                    if (request.Type == "GD")
                    {
                        //add by Hero.Zhang 计提折旧日期默认为当前活动期间
                        period = activePeriod;
                        response.DefaultDate = DateTime.Parse(period.EndDate.ToString()).ToString("yyyy-MM-dd");
                    }
                    else
                    {


                        if (doc != null)
                        {
                            response.DefaultDate = (doc.PZDate ?? DateTime.Now).ToString("yyyy-MM-dd");
                            period = periodBLL.GetPeriod(doc.PeriodId, ts);
                        }
                        else
                        {
                            period = activePeriod;
                            if (period.Month == DateTime.Now.Month && period.Year == DateTime.Now.Year)
                            {
                                //当天日期属于本期
                                response.DefaultDate = DateTime.Parse(DateTime.Now.ToString()).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                response.DefaultDate = DateTime.Parse(period.EndDate.ToString()).ToString("yyyy-MM-dd");
                            }
                        }
                    }
                    sql = @"select (case when max(pzzno) is null then 0 else max(pzzno) end) from TKS_FAS_Doc 
                            where accountid=@AccountId and periodId=@PeriodId";
                    var max = cnn.ExecuteScalar(sql, new { AccountId = user.AccountId, PeriodId = period.Id }, ts);
                    response.No = (int.Parse(max.ToString()) + 1).ToString();





                    var currentAccount = account.GetAccountSetByAccountId(user.AccountId, ts);

                    ts.Commit();
                    response.AccountName = currentAccount.QY_Name;

                    response.IsSuccess = true;
                    response.Message = "加载完毕";

                    response.Data = data;

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponsePZZTotalGet;
                }
            }
        }

        public ResponsePZZGet PZZGet(RequestPZZGet request)
        {
            ResponsePZZGet response = new ResponsePZZGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var data = cnn.QuerySingle<TKS_FAS_CertificateWord>("select * from TKS_FAS_CertificateWord where id=@Id", new
                    {
                        Id = request.Data.Id
                    });

                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponsePZZGet;
                }
            }
        }

        public ResponsePZZAdd PZZAdd(RequestPZZAdd request)
        {
            ResponsePZZAdd response = new ResponsePZZAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var d = cnn.Query("select * from TKS_FAS_CertificateWord where pzz=@PZZ",
                        new { PZZ = request.Data.PZZ }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("凭证字已经存在");
                    }
                    d = cnn.Query("select * from TKS_FAS_CertificateWord where ShowTitle=@ShowTitle",
                        new { ShowTitle = request.Data.ShowTitle }, ts);

                    if (d.Count() > 0)
                    {
                        throw new NormalException("显示名称已经存在");
                    }
                    if (request.Data.IsDefault == 1)
                    {
                        cnn.Execute("update TKS_FAS_CertificateWord set isDefault=0 where accountId=@AccountId",
                                new { AccountId = user.AccountId }, ts);
                    }
                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.AccountId = user.AccountId;
                    cnn.Insert<TKS_FAS_CertificateWord>(request.Data, ts);

                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponsePZZAdd;
                }
            }
        }

        public ResponsePZZUpdate PZZUpdate(RequestPZZUpdate request)
        {
            ResponsePZZUpdate response = new ResponsePZZUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var d = cnn.Query("select * from TKS_FAS_CertificateWord where pzz=@PZZ and id<>@Id",
                    new { PZZ = request.Data.PZZ, Id = request.Data.Id }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("凭证字已经存在");
                    }
                    d = cnn.Query("select * from TKS_FAS_CertificateWord where ShowTitle=@ShowTitle and id<>@Id",
                        new { ShowTitle = request.Data.ShowTitle, Id = request.Data.Id }, ts);

                    if (d.Count() > 0)
                    {
                        throw new NormalException("显示名称已经存在");
                    }

                    if (request.Data.IsDefault == 1)
                    {
                        cnn.Execute("update TKS_FAS_CertificateWord set isDefault=0 where accountId=@AccountId",
                                new { AccountId = user.AccountId }, ts);
                    }

                    string sql = @"update TKS_FAS_CertificateWord set 
                        PZZ=@PZZ,
                        ShowTitle=@ShowTitle,
                        IsDefault=@IsDefault
                        where id=@Id";

                    var r = cnn.Execute(sql, request.Data, ts);


                    if (r == 1)
                    {
                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "更新成功";
                        return response;
                    }
                    else
                    {

                        throw new AppException(user.User.UserName, "PZZUpdate", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponsePZZUpdate;
                }

            }
        }

        public ResponsePZZDelete PZZDelete(RequestPZZDelete request)
        {
            ResponsePZZDelete response = new ResponsePZZDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("delete from TKS_FAS_CertificateWord where id=@Id", request.Data, ts);



                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponsePZZDelete;
                }
            }
        }


    }
}
