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
    public class RoleBLL : CommonBase
    {
        public ResponseRoleListSearch RoleListSearch(RequestRoleListSearch request)
        {

            ResponseRoleListSearch response = new ResponseRoleListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);

                    string sql = this.GetPageSql("A.*",
                        "TKS_FAS_Role A where A.name like @Name",
                        "createDate desc",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Role> data = cnn.Query<TKS_FAS_Role>(sql, new { Name = "%" + request.Data.Name + "%" }, ts).ToList();

                    string countSql = "select count(1) from tks_fas_role where name like @Name";

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

                    return this.DealException(response, ex) as ResponseRoleListSearch;
                }
            }
        }

        public ResponseRoleGet RoleGet(RequestRoleGet request)
        {
            ResponseRoleGet response = new ResponseRoleGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var user = cnn.QuerySingle<TKS_FAS_Role>("select * from tks_fas_role where id=@Id", new
                    {
                        Id = request.Data.Id
                    });
                    var Role2Permission = cnn.Query<TKS_FAS_Role2Permission>("select * from TKS_FAS_Role2Permission where RoleId=@RoleId", new
                    {
                        RoleId = request.Data.Id
                    }).ToList();

                    var Permission = cnn.Query<TKS_FAS_Role2PermissionExt>("select * from TKS_FAS_PermissionInfo", null).ToList();
                    List<TKS_FAS_Role2PermissionExt> ptRoles = new List<TKS_FAS_Role2PermissionExt>();
                    
                    foreach (var item in Permission)
                    {
                        if (Role2Permission.Any(p => p.Permission == item.Permission))
                        {
                            item.IsChecked = true;
                        }
                        else
                        {
                            item.IsChecked = false;
                        }
                        ptRoles.Add(item);
                    }
                    RoleInfo roleInfo = new RoleInfo();
                    roleInfo.Data = user;
                    roleInfo.lstRole2Permission = ptRoles;
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = roleInfo;
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponseRoleGet;
                }
            }
        }

        public ResponseRoleAdd RoleAdd(RequestRoleAdd request)
        {
            ResponseRoleAdd response = new ResponseRoleAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);

                    var data = cnn.Query("select * from tks_fas_role where name=@Name",
                        new { Name = request.Data.Name }, ts);
                    if (data.Count() > 0)
                    {
                        throw new NormalException("角色已经存在");
                    }

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.CreateDate = DateTime.Now;
                    request.Data.CreateUser = user.User.UserName;
                    cnn.Insert<TKS_FAS_Role>(request.Data, ts);
                    if (request.Permission.Length>0)
                    {
                        foreach (var item in request.Permission)
                        {
                            TKS_FAS_Role2Permission Role2Permission = new TKS_FAS_Role2Permission();
                            Role2Permission.Id = Guid.NewGuid().ToString("N");
                            Role2Permission.RoleId = request.Data.Id;
                            Role2Permission.Permission = item;
                            
                            if (item== "平台管理员")
                            {
                                Role2Permission.PLevel = 1;
                            }
                            else if (item == "组织机构管理员")
                            {
                                Role2Permission.PLevel = 10;
                            }
                            else if (item == "组织机构会计")
                            {
                                Role2Permission.PLevel = 20;
                            }
                            cnn.Insert<TKS_FAS_Role2Permission>(Role2Permission, ts);
                        }
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
                    return this.DealException(response, ex) as ResponseRoleAdd;
                }
            }
        }

        public ResponseRoleUpdate RoleUpdate(RequestRoleUpdate request)
        {
            ResponseRoleUpdate response = new ResponseRoleUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    var role = cnn.Query<TKS_FAS_Role>("select * from tks_fas_role where name=@Name and id<>@Id",
                        new
                        {
                            Name = request.Data.Name
                            ,
                            Id = request.Data.Id
                        }, ts);
                    if (role.Count() > 0)
                    {
                        throw new NormalException("角色名已经存在");
                    }

                    request.Data.UpdateUser = user.User.UserName;
                    request.Data.UpdateDate = DateTime.Now;

                    string sql = @"update tks_fas_role set 
                        name=@Name,
                        memo=@Memo,
                        type=@Type,
                        updateUser=@UpdateUser,
                        updateDate=@UpdateDate 
                        where id=@Id";
                    cnn.Execute(sql, request.Data, ts);
                    cnn.Execute("delete from TKS_FAS_Role2Permission where RoleId=@RoleId",
                        new { RoleId = request.Data.Id }, ts);
                    foreach (var item in request.Permission)
                    {
                        TKS_FAS_Role2Permission _row = new TKS_FAS_Role2Permission();
                        _row.Id = Guid.NewGuid().ToString("N");
                        _row.RoleId = request.Data.Id;
                        _row.Permission = item;
                        if (item== "平台管理员")
                        {
                            _row.PLevel = 1;
                        }
                        else if (item == "组织机构管理员")
                        {
                            _row.PLevel = 10;
                        }
                        else if (item == "组织机构会计")
                        {
                            _row.PLevel = 20;
                        }
                        cnn.Insert<TKS_FAS_Role2Permission>(_row, ts);
                    }
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "更新成功";
                    return response;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseRoleUpdate;
                }

            }
        }

        public ResponseRoleDelete RoleDelete(RequestRoleDelete request)
        {
            ResponseRoleDelete response = new ResponseRoleDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);

                    cnn.Execute("delete from tks_fas_role where id=@Id", request.Data, ts);

                    cnn.Execute("delete from tks_fas_user2Role where roleid=@RoleId",
                        new { RoleId = request.Data.Id }, ts);
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseRoleDelete;
                }
            }
        }


        #region operate users of role 
        public ResponseUser2RoleListSearch User2RoleListSearch(RequestUser2RoleListSearch request)
        {
            ResponseUser2RoleListSearch response = new ResponseUser2RoleListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    if (string.IsNullOrEmpty(request.RoleId))
                    {
                        throw new AppException("", "User2RoleListSearch", "当前角色未知", "RoleId为空");
                    }
                    string sql = this.GetPageSql("A.*,D.name as NodeName",
                        @"TKS_FAS_User A left join tks_fas_user2role B on A.id=b.userId
                    left join tks_fas_user2Node C on B.A.id=C.userid 
                    left join tks_fas_node D on C.nodeId=D.Id
                    where A.status=1 and 
                    (A.userName like @UserName or A.trueName like @TrueName)
                    and B.roleId=@RoleId",
                        "createDate desc",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_User2RoleExt> data = cnn.Query<TKS_FAS_User2RoleExt>(sql,
                        new
                        {
                            UserName = "%" + request.UserName + "%",
                            TrueName = "%" + request.TrueName + "%",
                            RoleId = request.RoleId
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_User A left join tks_fas_user2role B on A.id=b.userId
                    left join tks_fas_user2Node C on B.A.id=C.userid 
                    left join tks_fas_node D on C.nodeId=D.Id
                    where A.status=1 and 
                    (A.userName like @UserName or A.trueName like @TrueName)
                    and B.roleId=@RoleId";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        UserName = "%" + request.UserName + "%",
                        TrueName = "%" + request.TrueName + "%",
                        RoleId = request.RoleId
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

                    return this.DealException(response, ex) as ResponseUser2RoleListSearch;
                }
            }
        }

        public ResponseUser2RoleAdd User2RoleAdd(RequestUser2RoleAdd request)
        {
            ResponseUser2RoleAdd response = new ResponseUser2RoleAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = "";
                    var r = cnn.Execute(sql,null, ts);

                    ts.Commit();
                   
                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseUser2RoleAdd;
                }
            }
        }
        #endregion
    }
}
