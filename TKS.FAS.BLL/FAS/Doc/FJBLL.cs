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
    public class FJBLL:CommonBase
    {
        public ResponseFJListSearch FJListSearch(RequestFJListSearch request)
        {

            ResponseFJListSearch response = new ResponseFJListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
            
                  

                    string sql = this.GetPageSql("*",
                        @" TKS_FAS_Attachment  where accountId=@AccountId and 
                        createUser=@CreateUser and docId=@DocId",
                        " createDate  desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Attachment> data = cnn.Query<TKS_FAS_Attachment>(sql,
                        new
                        {
                            AccountId = user.AccountId,
                            CreateUser=user.User.UserName,
                            DocId=request.DocId
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_Attachment  where 
                    accountId=@AccountId and createUser=@CreateUser and docId=@DocId";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = user.AccountId,
                        CreateUser = user.User.UserName,
                        DocId = request.DocId
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

                    return this.DealException(response, ex) as ResponseFJListSearch;
                }
            }
        }



        public ResponseFJGet FJGet(RequestFJGet request)
        {
            ResponseFJGet response = new ResponseFJGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    var data = cnn.QuerySingle<TKS_FAS_Attachment>("select * from TKS_FAS_Attachment where id=@Id", new
                    {
                        Id = request.Data.Id
                    }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data  = data ;
                
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFJGet;
                }
            }
        }

        public ResponseFJAdd FJAdd(RequestFJAdd request)
        {
            ResponseFJAdd response = new ResponseFJAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.AccountId = user.AccountId;
                    request.Data.CreateUser = user.User.UserName;
                    request.Data.CreateDate = DateTime.Now;
                    cnn.Insert<TKS_FAS_Attachment>(request.Data, ts);

                    ts.Commit();
                    response.Id = request.Data .Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFJAdd;
                }
            }
        }

      
        public ResponseFJUpdate FJUpdate(RequestFJUpdate request)
        {
            ResponseFJUpdate response = new ResponseFJUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
 

                
                    ts.Commit();

                    response.IsSuccess = true;
                 
                    response.Message = "更新成功";



                    return response;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFJUpdate;
                }

            }
        }

        public ResponseFJDelete FJDelete(RequestFJDelete request)
        {
            ResponseFJDelete response = new ResponseFJDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    string sql = @" delete from TKS_FAS_Attachment where id=@Id";

                    cnn.Execute(sql, request.Data, ts);
 

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFJDelete;
                }
            }
        }
    }
}
