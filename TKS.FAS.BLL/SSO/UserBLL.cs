using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TKS.FAS.Common;
using TKS.FAS.Entity;
using TKS.FAS.Entity.SSO;
using Dapper;
using DapperExtensions;

namespace TKS.FAS.BLL
{
    public partial class UserBLL : CommonBase
    {
        public ResponseUserListSearch UserListSearch(RequestUserListSearch request)
        {

            ResponseUserListSearch response = new ResponseUserListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    string sql = this.GetPageSql("A.*,C.Name as NodeName,C.Type as NodeType",
                        @"TKS_FAS_User A left join TKS_FAS_User2Node B on A.Id=B.userId
                        left join tks_fas_node C on B.nodeId=C.id
                            where A.status=1 and 
                    (A.userName like @UserName or A.trueName like @TrueName)",
                        "A.createDate desc",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_UserExt> data = cnn.Query<TKS_FAS_UserExt>(sql,
                        new
                        {
                            UserName = "%" + request.UserName + "%",
                            TrueName = "%" + request.TrueName + "%"
                        }, ts).ToList();

                    string countSql = @"select count(1) from tks_fas_User where status=1 and
                            (username like @UserName or trueName like @TrueName)";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        UserName = "%" + request.UserName + "%",
                        TrueName = "%" + request.TrueName + "%"
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

                    return this.DealException(response, ex) as ResponseUserListSearch;
                }
            }
        }

        public ResponseUserGet UserGet(RequestUserGet request)
        {
            ResponseUserGet response = new ResponseUserGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    string sql = @"  select A.*,B.NodeId ,C.Name as NodeName,C.Type as NodeType from tks_fas_user 
                        A left join TKS_FAS_User2Node B on A.id=b.UserId
                        left join TKS_FAS_Node C on B.NodeId=c.id where A.id=@Id";
                    var data = cnn.QuerySingle<TKS_FAS_UserExt>(sql, new
                    {
                        Id = request.Data.Id
                    },ts);
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserGet;
                }
            }
        }
        public ResponseUserAdd UserAdd(RequestUserAdd request)
        {
            ResponseUserAdd response = new ResponseUserAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    string pass = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(request.Data.Password, "MD5");

                    request.Data.Password = pass;

                    var data = cnn.Query("select * from tks_fas_User where userName=@UserName",
                        new { UserName = request.Data.UserName }, ts);
                    if (data.Count() > 0)
                    {
                        throw new NormalException("用户名已经存在");
                    }
                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.CreateUser = user.User.UserName;
                    request.Data.CreateDate = DateTime.Now;
                    cnn.Insert<TKS_FAS_User>(request.Data, ts);

                    //org
                    cnn.Execute("delete from tks_fas_user2Node where userid=@UserId",
                        new { UserId = request.Data.Id }, ts);
                    cnn.Insert<TKS_FAS_User2Node>(new TKS_FAS_User2Node
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        NodeId = request.Data.NodeId,
                        UserId = request.Data.Id
                    }, ts);
                    ts.Commit();
                    response.Id = request.Data.Id;
                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserAdd;
                }
            }
        }

        public ResponseUserUpdate UserUpdate(RequestUserUpdate request)
        {
            ResponseUserUpdate response = new ResponseUserUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);

                    var User = cnn.Query<TKS_FAS_User>(@"select * from tks_fas_User where
                            userName=@UserName and id<>@Id",
                        new
                        {
                            UserName = request.Data.UserName
                            ,
                            Id = request.Data.Id
                        }, ts);
                    if (User.Count() > 0)
                    {
                        throw new NormalException("用户名已经存在");
                    }
                  
                    request.Data.UpdateDate = DateTime.Now;
                    request.Data.UpdateUser = user.User.UserName;
                    string sql = @"update tks_fas_User set 
                            trueName=@TrueName,
                            status=@Status,
                            sex=@Sex,
                            mobile=@Mobile,
                            memo=@Memo,
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
                        return this.DealException(response, new AppException("user", "UserUpdate", "数据更新异常", r + "行数据被更新")) as ResponseUserUpdate;
                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserUpdate;
                }

            }
        }

        public ResponseUserUpdate UserUpdate2(RequestUserUpdate request)
        {
            ResponseUserUpdate response = new ResponseUserUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);

                    var User = cnn.Query<TKS_FAS_User>(@"select * from tks_fas_User where
                            userName=@UserName and id<>@Id",
                        new
                        {
                            UserName = request.Data.UserName
                            ,
                            Id = request.Data.Id
                        }, ts);
                    if (User.Count() > 0)
                    {
                        throw new NormalException("用户名已经存在");
                    }

                    request.Data.UpdateDate = DateTime.Now;
                    request.Data.UpdateUser = user.User.UserName;
                    string sql = @"update tks_fas_User set 
                            trueName=@TrueName,
  
                            sex=@Sex,
                  
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
                        return this.DealException(response, new AppException("user", "UserUpdate", "数据更新异常", r + "行数据被更新")) as ResponseUserUpdate;
                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserUpdate;
                }

            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseUserDelete UserDelete(RequestUserDelete request)
        {
            ResponseUserDelete response = new ResponseUserDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    if (user.User.Id == request.Data.Id)
                    {
                        throw new NormalException("用户为当前登录用户，不允许删除");
                    }
                    cnn.Execute("delete from tks_fas_User where id=@Id", request.Data, ts);

                    cnn.Execute("delete from tks_fas_user2Role where userid=@UserId",
                        new { UserId = request.Data.Id }, ts);


                    cnn.Execute("delete from tks_fas_user2Node where userid=@UserId",
                        new { UserId = request.Data.Id }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserDelete;
                }
            }
        }

        /// <summary>
        /// 获取用户设置的角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseUserRoleCheckedGet UserRoleCheckedGet(RequestUserRoleCheckedGet request)
        {
            ResponseUserRoleCheckedGet response = new ResponseUserRoleCheckedGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                  
                 
                    string sql = @"select * from tks_fas_role ";
                    var roles = cnn.Query<TKS_FAS_RoleExt>(sql,null, ts).ToList();

                    sql = @"select * from  TKS_FAS_User2Role where userid=@UserId";

                    var checkedRoles = cnn.Query<TKS_FAS_User2Role>(sql, new { UserId = request.UserId }, ts).ToList();
                    ts.Commit();
                    List<TKS_FAS_RoleExt> ptRoles = new List<TKS_FAS_RoleExt>();
                    List<TKS_FAS_RoleExt> qyRoles = new List<TKS_FAS_RoleExt>();
                    foreach (var item in roles)
                    {
                        if (checkedRoles.Any(p => p.RoleId == item.Id))
                        {
                            item.IsChecked = true;
                        }
                        else
                        {
                            item.IsChecked = false;
                        }
                        if (item.Type == 1)//PT
                        {
                            ptRoles.Add(item);
                        }


                        if (item.Type == 2&&user.Node.Type==1)//QY
                        {
                            qyRoles.Add(item);
                        }
                        else if (item.Type == 3 && user.Node.Type == 0)
                        {
                            qyRoles.Add(item);
                        }
                        else if (user.Node.Type == 2)
                        {
                            qyRoles.Add(item);
                        }

                    }
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PTRoles = ptRoles;
                    response.QYRoles = qyRoles;
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponseUserRoleCheckedGet;
                }
            }
        }

        public ResponseUserRoleCheckedUpdate UserRoleCheckedUpdate(RequestUserRoleCheckedUpdate request)
        {
            ResponseUserRoleCheckedUpdate response = new ResponseUserRoleCheckedUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    string sql = @"delete from tks_fas_user2role where userid=@UserId";
                    cnn.Execute(sql, new {UserId=request.UserId }, ts);

                    foreach (var item  in request.RoleIds)
                    {
                        TKS_FAS_User2Role _row = new TKS_FAS_User2Role();
                        _row.Id = Guid.NewGuid().ToString("N");
                        _row.RoleId = item;
                        _row.UserId = request.UserId;
                        cnn.Insert<TKS_FAS_User2Role>(_row, ts);
                    }

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "设置成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUserRoleCheckedUpdate;
                }
            }
        }
    }
}
