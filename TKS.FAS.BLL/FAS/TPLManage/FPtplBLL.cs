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
   public class FPtplBLL:CommonBase
    {
        public ResponseFPtplListSearch FPtplListSearch(RequestFPtplListSearch request)
        {

            ResponseFPtplListSearch response = new ResponseFPtplListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    string sql = this.GetPageSql("A.*,B.title",
                        @"   TKS_FAS_Invoice2TPL A left join  TKS_FAS_DocTPL B on A.tplId=B.id where A.accountId='-'",
                        " A.createDate desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Invoice2TPLExt> data = cnn.Query<TKS_FAS_Invoice2TPLExt>(sql,
                        new
                        {
                             
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_Invoice2TPL where accountId='-'  ";

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

                    return this.DealException(response, ex) as ResponseFPtplListSearch;
                }
            }
        }


        public ResponseFPtplGet FPtplGet(RequestFPtplGet request)
        {
            ResponseFPtplGet response = new ResponseFPtplGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var data = cnn.QuerySingle<TKS_FAS_Invoice2TPLExt>(
                        @"select A.*,B.title from TKS_FAS_Invoice2TPL A left join  TKS_FAS_DocTPL B on A.tplId=B.id where A.id=@Id", new
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
                    return this.DealException(response, ex) as ResponseFPtplGet;
                }
            }
        }

        public ResponseFPtplAdd FPtplAdd(RequestFPtplAdd request)
        {
            ResponseFPtplAdd response = new ResponseFPtplAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.AccountId = "-";
                    request.Data.CreateDate = DateTime.Now;
                    request.Data.CreateUser = user.User.UserName;
                    cnn.Insert<TKS_FAS_Invoice2TPL>(request.Data, ts);

                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFPtplAdd;
                }
            }
        }

        public ResponseFPtplUpdate FPtplUpdate(RequestFPtplUpdate request)
        {
            ResponseFPtplUpdate response = new ResponseFPtplUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);


                    string sql = @" 
                            UPDATE  TKS_FAS_Invoice2TPL
                               SET
                                  
                                  Type =@Type
                                  ,IsVAT = @IsVAT
                                  ,RPStatus = @RPStatus
                                  ,PayMode =@PayMode
                                  ,TPLID=@TPLID
                                  
                                  ,UpdateUser =@UpdateUser
                                  ,UpdateDate =@UpdateDate
                                  
                             where id=@Id

                            ";
                    request.Data.UpdateDate = DateTime.Now;
                    request.Data.UpdateUser = user.User.UserName;
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

                        throw new AppException(user.User.UserName, "FPtplUpdate", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFPtplUpdate;
                }

            }
        }

        public ResponseFPtplDelete FPtplDelete(RequestFPtplDelete request)
        {
            ResponseFPtplDelete response = new ResponseFPtplDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("delete from TKS_FAS_Invoice2TPL where id=@Id", request.Data, ts);



                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFPtplDelete;
                }
            }
        }
    }
}
