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
    public class FunctionBLL : CommonBase
    {

        public ResponseFuncTreeGet FuncTreeGet(RequestFuncTreeGet request)
        {
            ResponseFuncTreeGet response = new ResponseFuncTreeGet();
            using (cnn = GetConnection())
            {
                try
                {
                    string sql = @"WITH CTE(id,
                         name,
                        img,
                        PARENTID,seq) AS
                         (SELECT id,name,img, PARENTID,seq
                            FROM tks_fas_function
                           WHERE id = @Id
  
                          UNION ALL
                          SELECT B.id,B.name,B.img,B.PARENTID,B.Seq
                            FROM tks_fas_function B
                           INNER JOIN CTE
                              ON  B.PARENTID = CTE.id)
      
                     SELECT * FROM CTE order by seq";
                    var funcs = cnn.Query<TKS_FAS_Function>(sql, new { Id = "-" });

                    TKS_FAS_TreeItem root = new TKS_FAS_TreeItem();
                    var first = funcs.Where(p => p.Id == "-").FirstOrDefault();

                    root = Trans2TreeItem(first);

                    CreateTree(funcs.ToList(), ref root);
                    response.Data = new List<TKS_FAS_TreeItem>();
                    response.Data.Add(root);
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponseFuncTreeGet;
                }
            }
        }

        void CreateTree(List<TKS_FAS_Function> data, ref TKS_FAS_TreeItem root)
        {
            for (int i = 0; i < data.Count; i++)
            {
                var cur = data[i];
                if (cur.ParentId == root.id)
                {
                    var child = Trans2TreeItem(cur);
                    root.children.Add(child);
                    CreateTree(data, ref child);
                }
            }
        }

        void CreateTree(List<TKS_FAS_Function> data, ref TKS_FAS_TreeItem root, List<TKS_FAS_Entity2Function> checkedFuncs)
        {
            for (int i = 0; i < data.Count; i++)
            {
                var cur = data[i];
                if (cur.ParentId == root.id)
                {
                    var child = Trans2TreeItem(cur);
                    if (checkedFuncs.Any(p => { return p.FunctionId == child.id&&p.CheckState=="checked"; }))
                    {
                        child.@checked = true;
                    }
                    root.children.Add(child);
                    CreateTree(data, ref child, checkedFuncs);
                }
            }
        }

        TKS_FAS_TreeItem Trans2TreeItem(TKS_FAS_Function func)
        {
            TKS_FAS_TreeItem item = new TKS_FAS_TreeItem();
            item.id = func.Id;
            item.text = func.Name;
            item.state = "open";
            item.@checked = false;
            item.children = new List<TKS_FAS_TreeItem>();
            return item;
        }

        public ResponseFuncGet FuncGet(RequestFuncGet request)
        {
            ResponseFuncGet response = new ResponseFuncGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var user = cnn.QuerySingle<TKS_FAS_Function>("select * from tks_fas_function where id=@Id", new
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
                    return this.DealException(response, ex) as ResponseFuncGet;
                }
            }
        }

        public ResponseFuncAdd FuncAdd(RequestFuncAdd request)
        {
            ResponseFuncAdd response = new ResponseFuncAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    //var user = cnn.Query("select * from tks_fas_function where name=@Name",
                    //    new { Name = request.Data.Name }, ts);
                    //if (user.Count() > 0)
                    //{
                    //    throw new NormalException("功能点已经存在");
                    //}
                    //ignore func duplicate name
                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.CreateDate = DateTime.Now;
                    request.Data.CreateUser = user.User.UserName;
                    var r = cnn.Insert<TKS_FAS_Function>(request.Data, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFuncAdd;
                }
            }
        }


        public ResponseFuncUpdate FuncUpdate(RequestFuncUpdate request)
        {
            ResponseFuncUpdate response = new ResponseFuncUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    request.Data.UpdateDate = DateTime.Now;
                    request.Data.UpdateUser = user.User.UserName;
                    string sql = @"update tks_fas_function set 
                        name=@Name,
                        url=@Url,
                        img=@Img,
                        seq=@Seq,
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
                        return this.DealException(response, new AppException("user", "FuncUpdate", "数据更新异常", r + "行数据被更新")) as ResponseFuncUpdate;
                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFuncUpdate;
                }

            }
        }

        public ResponseFuncDelete FuncDelete(RequestFuncDelete request)
        {
            ResponseFuncDelete response = new ResponseFuncDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    string sql = @"WITH CTE(id,
                         name,
                        img,
                        PARENTID) AS
                         (SELECT id,name,img, PARENTID
                            FROM tks_fas_function
                           WHERE id = @Id
  
                          UNION ALL
                          SELECT B.id,B.name,B.img,B.PARENTID
                            FROM tks_fas_function B
                           INNER JOIN CTE
                              ON  B.PARENTID = CTE.id)
      
                     SELECT * FROM CTE ";
                    var funcs = cnn.Query<TKS_FAS_Function>(sql, new { Id = request.Data.Id }, ts).Select(p => p.Id).ToList();

                    var inId = "'" + string.Join("','", funcs.ToArray()) + "'";
                    string delSql = string.Format("delete from tks_fas_function where id in ({0})", inId);
                    var r = cnn.Execute(delSql, null, ts);
                    delSql = string.Format("delete from tks_fas_entity2Function where functionId in ({0})", inId);
                    cnn.Execute(delSql, null, ts);
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFuncDelete;
                }
            }
        }

        public ResponseFuncTreeCheckedGet FuncTreeCheckedGet(RequestFuncTreeCheckedGet request)
        {
            ResponseFuncTreeCheckedGet response = new ResponseFuncTreeCheckedGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"WITH CTE(id,
                         name,
                        img,
                        PARENTID,seq) AS
                         (SELECT id,name,img, PARENTID,seq
                            FROM tks_fas_function
                           WHERE id = @Id
  
                          UNION ALL
                          SELECT B.id,B.name,B.img,B.PARENTID,B.Seq
                            FROM tks_fas_function B
                           INNER JOIN CTE
                              ON  B.PARENTID = CTE.id)
      
                     SELECT * FROM CTE order by seq";
                    var funcs = cnn.Query<TKS_FAS_Function>(sql, new { Id = "-" }, ts).ToList();

                    sql = "select * from TKS_FAS_Entity2Function where type=@Type and entityId=@EntityId";

                    var checkedFuncs = cnn.Query<TKS_FAS_Entity2Function>(sql, new
                    {
                        Type = "R",
                        EntityId = request.EntityId
                    }, ts).ToList();
                    ts.Commit();




                    TKS_FAS_TreeItem root = new TKS_FAS_TreeItem();
                    var first = funcs.Where(p => p.Id == "-").FirstOrDefault();

                    root = Trans2TreeItem(first);
                    if (checkedFuncs.Any(p => { return p.FunctionId == root.id && p.CheckState == "checked"; }))
                    {
                        root.@checked = true;
                    }
                    CreateTree(funcs, ref root, checkedFuncs);
                    response.Data = new List<TKS_FAS_TreeItem>();
                    response.Data.Add(root);
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponseFuncTreeCheckedGet;
                }
            }
        }

        public ResponseFuncTreeCheckedUpdate FuncTreeCheckedUpdate(RequestFuncTreeCheckedUpdate request)
        {
            ResponseFuncTreeCheckedUpdate response = new ResponseFuncTreeCheckedUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    string sql = "delete from tks_fas_entity2Function where type=@Type and entityId=@EntityId";
                    cnn.Execute(sql, new { Type = "R", EntityId = request.RoleId }, ts);
                    foreach (var item in request.Data)
                    {
                        cnn.Insert<TKS_FAS_Entity2Function>(new TKS_FAS_Entity2Function
                        {
                            Id = Guid.NewGuid().ToString("N"),
                            EntityId = request.RoleId,
                            Type = "R",
                            FunctionId = item.id,
                            CheckState=item.checkState
                            
                        }, ts);
                    }

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "保存成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFuncTreeCheckedUpdate;
                }
            }
        }
    }
}
