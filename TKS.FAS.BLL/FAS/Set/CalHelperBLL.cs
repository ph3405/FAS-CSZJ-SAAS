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
    public class CalHelperBLL : CommonBase
    {
        public ResponseCalHelperListSearch CalHelperListSearch(RequestCalHelperListSearch request)
        {

            ResponseCalHelperListSearch response = new ResponseCalHelperListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = this.GetPageSql("*",
                        @" TKS_FAS_CaculateHelperItem where accountId=@AccountId",
                        " iscustom desc,title ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_CaculateHelperItem> data = cnn.Query<TKS_FAS_CaculateHelperItem>(sql,
                        new
                        {
                            AccountId = user.AccountId
                        }, ts).ToList();

                    string countSql = @"select count(1) from  TKS_FAS_CaculateHelperItem where accountId=@AccountId";

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

                    return this.DealException(response, ex) as ResponseCalHelperListSearch;
                }
            }
        }

        public ResponseCalHelperGet CalHelperGet(RequestCalHelperGet request)
        {
            ResponseCalHelperGet response = new ResponseCalHelperGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    //辅助核算类别是否被使用
                    var ass = cnn.Query("select * from TKS_FAS_SubjectAss where code=@Code", 
                        new {Code=request.Data.Id }, ts).ToList();
                    if(ass.Count() > 0)
                    {
                        response.IsUse = 1;
                    }
                    else
                    {
                        response.IsUse = 0;
                    }
                    var head = cnn.QuerySingle<TKS_FAS_CaculateHelperItem>("select * from TKS_FAS_CaculateHelperItem where id=@Id", new
                    {
                        Id = request.Data.Id
                    }, ts);

                    var customDes = cnn.Query<TKS_FAS_CustomColDes>("select * from TKS_FAS_CustomColDes where parentId=@ParentId", new
                    {
                        ParentId = request.Data.Id
                    }, ts).ToList();
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Head = head;
                    response.CustomDes = customDes;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCalHelperGet;
                }
            }
        }

        public ResponseCalHelperAdd CalHelperAdd(RequestCalHelperAdd request)
        {
            ResponseCalHelperAdd response = new ResponseCalHelperAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var d = cnn.Query(@"select * from TKS_FAS_CaculateHelperItem 
                            where title=@Title and accountId=@AccountId",
                        new { Title = request.Head.Title,AccountId=user.AccountId }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("类别名已经存在");
                    }
 
                    request.Head.Id = Guid.NewGuid().ToString("N");
                    request.Head.AccountId = user.AccountId;
                    request.Head.IsCustom = 1;
                    cnn.Insert<TKS_FAS_CaculateHelperItem>(request.Head, ts);
 
                    foreach (var item in request.CustomDes)
                    {
                        item.Id = Guid.NewGuid().ToString("N");
                        item.AccountId = user.AccountId;
                        item.ParentId = request.Head.Id;
                        cnn.Insert<TKS_FAS_CustomColDes>(item, ts);
                    }

                    ts.Commit();
                    response.Id = request.Head.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCalHelperAdd;
                }
            }
        }

        public ResponseCalHelperUpdate CalHelperUpdate(RequestCalHelperUpdate request)
        {
            ResponseCalHelperUpdate response = new ResponseCalHelperUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var d = cnn.Query(@"select * from TKS_FAS_CaculateHelperItem 
                            where title=@Title and accountId=@AccountId and id<>@Id",
                       new { Title = request.Head.Title ,AccountId=user.AccountId,Id=request.Head.Id}, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("类别名已经存在");
                    }
                 
                    string sql = @"update TKS_FAS_CaculateHelperItem set 
                        title=@Title
                        where id=@Id";
             
                    var r = cnn.Execute(sql, request.Head, ts);

                    cnn.Execute("delete from TKS_FAS_CustomColDes where parentId=@ParentId",
                        new {ParentId=request.Head.Id },ts);

                    foreach (var item in request.CustomDes)
                    {
                        item.Id = Guid.NewGuid().ToString("N");
                        item.AccountId = user.AccountId;
                        item.ParentId = request.Head.Id;
                        cnn.Insert<TKS_FAS_CustomColDes>(item, ts);
                    }

                    if (r == 1)
                    {
                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "更新成功";
                        return response;
                    }
                    else
                    {

                        throw new AppException(user.User.UserName, "CalHelperUpdate", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCalHelperUpdate;
                }

            }
        }

        public ResponseCalHelperDelete CalHelperDelete(RequestCalHelperDelete request)
        {
            ResponseCalHelperDelete response = new ResponseCalHelperDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("delete from TKS_FAS_CaculateHelperItem where id=@Id", request.Data, ts);

                    cnn.Execute("delete from TKS_FAS_CustomColDes where ParentId=@ParentId",
                        new { ParentId = request.Data.Id }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCalHelperDelete;
                }
            }
        }
    }
}
