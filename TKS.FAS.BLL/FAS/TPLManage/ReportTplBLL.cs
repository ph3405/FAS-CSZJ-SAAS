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
    public class ReportTplBLL : CommonBase
    {
        #region 表头
        public ResponseBBtplListSearch BBtplListSearch(RequestBBtplListSearch request)
        {

            ResponseBBtplListSearch response = new ResponseBBtplListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    string where = string.Empty;


                    string sql = this.GetPageSql("A.* ",
                        @"  TKS_FAS_ReportTPL A where A.accountId='-'",
                        " A.createDate  desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_ReportTPL> data = cnn.Query<TKS_FAS_ReportTPL>(sql,
                        new
                        {


                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_ReportTPL A where A.accountId='-'
                      ";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {

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
                    return this.DealException(response, ex) as ResponseBBtplListSearch;
                }
            }
        }

        public ResponseBBtplGet BBtplGet(RequestBBtplGet request)
        {
            ResponseBBtplGet response = new ResponseBBtplGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    var data = cnn.QuerySingle<TKS_FAS_ReportTPL>("select * from TKS_FAS_ReportTPL where id=@Id", new
                    {
                        Id = request.Data.Id
                    }, ts);


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBBtplGet;
                }
            }
        }

        public ResponseBBtplAdd BBtplAdd(RequestBBtplAdd request)
        {
            ResponseBBtplAdd response = new ResponseBBtplAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    var user = this.UserInfoGetButAccount(request.Token, ts);



                   
                    request.Data.CreateUser = user.User.UserName;
                    request.Data.CreateDate = DateTime.Now;
                    request.Data.AccountId = "-";
                    cnn.Insert<TKS_FAS_ReportTPL>(request.Data, ts);

                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBBtplAdd;
                }
            }
        }


        public ResponseBBtplUpdate BBtplUpdate(RequestBBtplUpdate request)
        {
            ResponseBBtplUpdate response = new ResponseBBtplUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);

                    string sql = @"update TKS_FAS_ReportTPL set 
                        title=@Title,updateUser=@UpdateUser,updateDate=@UpdateDate where id=@Id";

                    request.Data.UpdateUser = user.User.UserName;
                    request.Data.UpdateDate = DateTime.Now;
                    cnn.Execute(sql, request.Data, ts);

                    ts.Commit();

                    response.IsSuccess = true;
                    response.Id = request.Data.Id;
                    response.Message = "更新成功";
                    return response;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBBtplUpdate;
                }

            }
        }

        public ResponseBBtplDelete BBtplDelete(RequestBBtplDelete request)
        {
            ResponseBBtplDelete response = new ResponseBBtplDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    string sql = @" delete from TKS_FAS_ReportTPL where id=@Id";

                    cnn.Execute(sql, request.Data, ts);

                    sql = "delete from TKS_FAS_ReportDetailTPL where parentId=@ParentId";
                    cnn.Execute(sql, new { ParentId = request.Data.Id }, ts);

                    sql = "delete from TKS_FAS_Formula where ReportTPLId=@ReportTPLId";
                    cnn.Execute(sql, new { ReportTPLId = request.Data.Id }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBBtplDelete;
                }
            }
        }

        #endregion

        #region 明细
        public ResponseBBDetailtplListSearch BBDetailtplListSearch(RequestBBDetailtplListSearch request)
        {

            ResponseBBDetailtplListSearch response = new ResponseBBDetailtplListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    string where = string.Empty;


                    string sql = this.GetPageSql("A.* ",
                        @"  TKS_FAS_ReportDetailTPL A where parentId=@ParentId",
                        " A.seq  ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_ReportDetailTPL> data = cnn.Query<TKS_FAS_ReportDetailTPL>(sql,
                        new
                        {
                            ParentId = request.Id

                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_ReportDetailTPL A where parentId=@ParentId
                      ";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        ParentId = request.Id
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
                    return this.DealException(response, ex) as ResponseBBDetailtplListSearch;
                }
            }
        }

        public ResponseBBDetailtplGet BBDetailtplGet(RequestBBDetailtplGet request)
        {
            ResponseBBDetailtplGet response = new ResponseBBDetailtplGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    var data = cnn.QuerySingle<TKS_FAS_ReportDetailTPL>("select * from TKS_FAS_ReportDetailTPL where id=@Id", new
                    {
                        Id = request.Data.Id
                    }, ts);


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBBDetailtplGet;
                }
            }
        }

        public ResponseBBDetailtplAdd BBDetailtplAdd(RequestBBDetailtplAdd request)
        {
            ResponseBBDetailtplAdd response = new ResponseBBDetailtplAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    var user = this.UserInfoGetButAccount(request.Token, ts);

                    var data = cnn.Query("select * from TKS_FAS_ReportDetailTPL where id=@Id", new { Id = request.Data.Id }, ts);
                    if(data.Count() > 0)
                    {
                        throw new NormalException("ID已经存在");
                    }
                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.AccountId = "-";
                    cnn.Insert<TKS_FAS_ReportDetailTPL>(request.Data, ts);

                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBBDetailtplAdd;
                }
            }
        }


        public ResponseBBDetailtplUpdate BBDetailtplUpdate(RequestBBDetailtplUpdate request)
        {
            ResponseBBDetailtplUpdate response = new ResponseBBDetailtplUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);

                    string sql = @"update TKS_FAS_ReportDetailTPL set
                                columnName=@ColumnName,
                                SourceType=@SourceType,
                                SourceValue=@SourceValue,
                                Category=@Category,
                                seq=@Seq,
                                OperatorCharacter=@OperatorCharacter
                                where id=@Id";

                    cnn.Execute(sql, request.Data, ts);

                    ts.Commit();

                    response.IsSuccess = true;
                    response.Id = request.Data.Id;
                    response.Message = "更新成功";
                    return response;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBBDetailtplUpdate;
                }

            }
        }

        public ResponseBBDetailtplDelete BBDetailtplDelete(RequestBBDetailtplDelete request)
        {
            ResponseBBDetailtplDelete response = new ResponseBBDetailtplDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    string sql = "delete from TKS_FAS_ReportDetailTPL where id=@Id";
                    cnn.Execute(sql, new { Id = request.Data.Id }, ts);

                    sql = "delete from TKS_FAS_Formula where ReportDetailTPLId=@ReportDetailTPLId";
                    cnn.Execute(sql, new { ReportDetailTPLId = request.Data.Id }, ts);
                    
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseBBDetailtplDelete;
                }
            }
        }

        #endregion
    }
}
