using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Common;

namespace TKS.FAS.BLL.FAS
{
    public class CurrencyBLL : CommonBase
    {
        public ResponseCurrencyListSearch CurrencyListSearch(RequestCurrencyListSearch request)
        {

            ResponseCurrencyListSearch response = new ResponseCurrencyListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = this.GetPageSql("*",
                        @"  TKS_FAS_Currency where accountId=@AccountId",
                        " isBaseCurrency desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Currency> data = cnn.Query<TKS_FAS_Currency>(sql,
                        new
                        {
                            AccountId=user.AccountId
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_Currency where accountId=@AccountId ";

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

                    return this.DealException(response, ex) as ResponseCurrencyListSearch;
                }
            }
        }

        public ResponseCurrencyGet CurrencyGet(RequestCurrencyGet request)
        {
            ResponseCurrencyGet response = new ResponseCurrencyGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var data = cnn.QuerySingle<TKS_FAS_Currency>("select * from tks_fas_Currency where id=@Id", new
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
                    return this.DealException(response, ex) as ResponseCurrencyGet;
                }
            }
        }

        public ResponseCurrencyAdd CurrencyAdd(RequestCurrencyAdd request)
        {
            ResponseCurrencyAdd response = new ResponseCurrencyAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var d = cnn.Query(@"select * from tks_fas_Currency where 
                        code=@Code and AccountId=@AccountId",
                        new { Code = request.Data.Code,AccountId=user.AccountId }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("编码已经存在");
                    }
                    d = cnn.Query("select * from tks_fas_Currency where name=@Name and AccountId=@AccountId",
                        new { Name = request.Data.Name, AccountId = user.AccountId }, ts);

                    if (d.Count() > 0)
                    {
                        throw new NormalException("名称已经存在");
                    }

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.AccountId = user.AccountId;
                    cnn.Insert<TKS_FAS_Currency>(request.Data, ts);

                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCurrencyAdd;
                }
            }
        }

        public ResponseCurrencyUpdate CurrencyUpdate(RequestCurrencyUpdate request)
        {
            ResponseCurrencyUpdate response = new ResponseCurrencyUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var d = cnn.Query("select * from tks_fas_Currency where code=@Code and id<>@Id",
                    new { Code = request.Data.Code,Id=request.Data.Id }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("编码已经存在");
                    }
                    d = cnn.Query("select * from tks_fas_Currency where name=@Name and id<>@Id",
                        new { Name = request.Data.Name,Id=request.Data.Id }, ts);

                    if (d.Count() > 0)
                    {
                        throw new NormalException("名称已经存在");
                    }


                    string sql = @"update tks_fas_Currency set 
                        code=@Code,
                        name=@Name,
                        exchangeRate=@ExchangeRate
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

                        throw new AppException(user.User.UserName, "CurrencyUpdate", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCurrencyUpdate;
                }

            }
        }

        public ResponseCurrencyDelete CurrencyDelete(RequestCurrencyDelete request)
        {
            ResponseCurrencyDelete response = new ResponseCurrencyDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("delete from tks_fas_Currency where id=@Id", request.Data, ts);

            

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCurrencyDelete;
                }
            }
        }

    }
}
