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
    public class FPFJBLL : CommonBase
    {
        public ResponseFPFJListSearch FPFJListSearch(RequestFPFJListSearch request)
        {

            ResponseFPFJListSearch response = new ResponseFPFJListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    string sql = this.GetPageSql("*",
                        @" TKS_FAS_InvoiceAttach  where  invoiceId=@InvoiceId",
                        " createDate  desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_InvoiceAttach> data = cnn.Query<TKS_FAS_InvoiceAttach>(sql,
                        new
                        {
                            CreateUser = user.User.UserName,
                            InvoiceId = request.InvoiceId
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_InvoiceAttach  where 
                    invoiceId=@InvoiceId";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        CreateUser = user.User.UserName,
                        InvoiceId = request.InvoiceId
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

                    return this.DealException(response, ex) as ResponseFPFJListSearch;
                }
            }
        }



        public ResponseFPFJGet FPFJGet(RequestFPFJGet request)
        {
            ResponseFPFJGet response = new ResponseFPFJGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    var data = cnn.QuerySingle<TKS_FAS_InvoiceAttach>("select * from TKS_FAS_InvoiceAttach where id=@Id", new
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
                    return this.DealException(response, ex) as ResponseFPFJGet;
                }
            }
        }

        public ResponseFPFJAdd FPFJAdd(RequestFPFJAdd request)
        {
            ResponseFPFJAdd response = new ResponseFPFJAdd();
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
                    cnn.Insert<TKS_FAS_InvoiceAttach>(request.Data, ts);

                    SetInvoiceNum(request.Data.InvoiceId, ts, user);

                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFPFJAdd;
                }
            }
        }

        /// <summary>
        /// 更新发票附件的数量
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <param name="ts"></param>
        /// <param name="user"></param>
        private void SetInvoiceNum(string invoiceId, System.Data.IDbTransaction ts, MM_UserInfo user)
        {
            string sql = @"select count(1) from TKS_FAS_InvoiceAttach where 
                    invoiceId=@InvoiceId and AccountId=@AccountId";

            int total = int.Parse(cnn.ExecuteScalar(sql, new
            {
                AccountId = user.AccountId,
                InvoiceId =  invoiceId
            }, ts).ToString());

            sql = @"update TKS_FAS_Invoice set invoiceNum=@InvoiceNum where id=@Id";

            cnn.Execute(sql, new { Id = invoiceId, InvoiceNum = total }, ts);
        }

        public ResponseFPFJUpdate FPFJUpdate(RequestFPFJUpdate request)
        {
            ResponseFPFJUpdate response = new ResponseFPFJUpdate();
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
                    return this.DealException(response, ex) as ResponseFPFJUpdate;
                }

            }
        }

        public ResponseFPFJDelete FPFJDelete(RequestFPFJDelete request)
        {
            ResponseFPFJDelete response = new ResponseFPFJDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var data = cnn.QueryFirst<TKS_FAS_InvoiceAttach>("select * from TKS_FAS_InvoiceAttach where id=@Id", request.Data,ts);

                    string sql = @" delete from TKS_FAS_InvoiceAttach where id=@Id";

                    cnn.Execute(sql, request.Data, ts);

                    SetInvoiceNum(data.InvoiceId, ts, user);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFPFJDelete;
                }
            }
        }
    }
}
