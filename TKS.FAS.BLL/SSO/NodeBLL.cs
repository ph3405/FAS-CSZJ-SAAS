using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.SSO;
using TKS.FAS.Common;

namespace TKS.FAS.BLL
{
    public class NodeBLL : CommonBase
    {
        public ResponseNodeListSearch NodeListSearch(RequestNodeListSearch request)
        {

            ResponseNodeListSearch response = new ResponseNodeListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = this.GetPageSql("A.*",
                        "TKS_FAS_Node A where A.name like @Name",
                        "createDate desc",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Node> data = cnn.Query<TKS_FAS_Node>(sql, new { Name = "%" + request.Data.Name + "%" }, ts).ToList();

                    string countSql = "select count(1) from tks_fas_Node where name like @Name";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new { Name = "%" + request.Data.Name + "%" }, ts).ToString());

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

                    return this.DealException(response, ex) as ResponseNodeListSearch;
                }
            }
        }

        public ResponseNodeListSearch SupplierSearch(RequestNodeListSearch request)
        {

            ResponseNodeListSearch response = new ResponseNodeListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = this.GetPageSql("A.*",
                        "TKS_FAS_Node A where A.name like @Name and type=1 and  isOutSource=1",
                        "createDate desc",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Node> data = cnn.Query<TKS_FAS_Node>(sql, new { Name = "%" + request.Data.Name + "%" }, ts).ToList();

                    string countSql = "select count(1) from tks_fas_Node where name like @Name and type=1 and isOutSource=1";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new { Name = "%" + request.Data.Name + "%" }, ts).ToString());

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

                    return this.DealException(response, ex) as ResponseNodeListSearch;
                }
            }
        }

        public ResponseNodeGet NodeGet(RequestNodeGet request)
        {
            ResponseNodeGet response = new ResponseNodeGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var user = cnn.QuerySingle<TKS_FAS_Node>("select * from tks_fas_Node where id=@Id", new
                    {
                        Id = request.Data.Id
                    });

                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = user;
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponseNodeGet;
                }
            }
        }

        public ResponseNodeAdd NodeAdd(RequestNodeAdd request)
        {
            ResponseNodeAdd response = new ResponseNodeAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    var data = cnn.Query("select * from tks_fas_Node where name=@Name",
                        new { Name = request.Data.Name }, ts);
                    if (data.Count() > 0)
                    {
                        throw new NormalException("机构名已经存在");
                    }

                    data = cnn.Query("select * from tks_fas_node where CreditCode=@CreditCode",
                        new { CreditCode=request.Data.CreditCode },ts);

                    if(data.Count() > 0)
                    {
                        throw new NormalException("信用代码已经存在");
                    }

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.CreateDate = DateTime.Now;
                    request.Data.CreateUser = user.User.UserName;

                    cnn.Insert<TKS_FAS_Node>(request.Data, ts);

                    ts.Commit();
                    response.Id = request.Data.Id;
                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseNodeAdd;
                }
            }
        }

        public ResponseNodeUpdate NodeUpdate(RequestNodeUpdate request)
        {
            ResponseNodeUpdate response = new ResponseNodeUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);

                    var Node = cnn.Query<TKS_FAS_Node>("select * from tks_fas_Node where name=@Name and id<>@Id",
                        new
                        {
                            Name = request.Data.Name
                            ,
                            Id = request.Data.Id
                        }, ts);
                    if (Node.Count() > 0)
                    {
                        throw new NormalException("机构名已经存在");
                    }
                    Node = cnn.Query<TKS_FAS_Node>("select * from tks_fas_Node where CreditCode=@CreditCode and id<>@Id",
                      new
                      {
                          CreditCode = request.Data.CreditCode
                          ,
                          Id = request.Data.Id
                      }, ts);
                    if (Node.Count() > 0)
                    {
                        throw new NormalException("信用代码已经存在");
                    }

                    request.Data.UpdateDate = DateTime.Now;
                    request.Data.UpdateUser = user.User.UserName;

                    string sql = @"update tks_fas_Node set 
                        name=@Name,
                        memo=@Memo,
                        CreditCode=@CreditCode,
                        type=@Type,
                        updateUser=@UpdateUser,
                        updateDate=@UpdateDate 
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
                        ts.Rollback();
                        return this.DealException(response, new AppException("user", "NodeUpdate", "数据更新异常", r + "行数据被更新")) as ResponseNodeUpdate;
                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseNodeUpdate;
                }

            }
        }

        public ResponseNodeDelete NodeDelete(RequestNodeDelete request)
        {
            ResponseNodeDelete response = new ResponseNodeDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);

                    var users = cnn.Query("select 1 from tks_fas_user2Node where nodeId=@NodeId",
                        new { NodeId = request.Data.Id }, ts);

                    if (users.Count() > 0)
                    {
                        throw new NormalException("机构下存在用户，禁止删除");
                    }
                    cnn.Execute("delete from tks_fas_Node where id=@Id", request.Data, ts);

                    //cnn.Execute("delete from tks_fas_user2Node where Nodeid=@NodeId",
                    //    new { NodeId = request.Data.Id }, ts);
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseNodeDelete;
                }
            }
        }
    }
}
