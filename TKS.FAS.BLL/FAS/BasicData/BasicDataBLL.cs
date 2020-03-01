using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Common;
using System.Data;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace TKS.FAS.BLL.FAS
{
    public class BasicDataBLL : CommonBase
    {
        public BasicDataBLL() { }
        
        public BasicDataBLL(IDbConnection cnn)
        {
            this.cnn = cnn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseBasicDataSearch BasicDataListSearch(RequestBasicDataSearch request)
        {

            ResponseBasicDataSearch response = new ResponseBasicDataSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string where = string.Empty;
                    if (!string.IsNullOrEmpty(request.Name))
                    {
                        where += " and  Name like '%"+ request.Name + "%'";
                    }
                    string sql = this.GetPageSql("*",
                            @"   TKS_FAS_BasicData  
                               where UserId=@UserId and DataType=@DataType" + where,
                            " createDate desc ",
                            request.PageSize,
                            request.PageIndex);

                    List<TKS_FAS_BasicData> data = cnn.Query<TKS_FAS_BasicData>(sql,
                        new
                        {
                            UserId = request.UserId,
                            DataType = request.DataType
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_BasicData where UserId=@UserId and DataType=@DataType" + where;

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        UserId = request.UserId,
                        DataType = request.DataType
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

                    return this.DealException(response, ex) as ResponseBasicDataSearch;
                }
            }
        }





        public ResponseBasicDataGet WX_BasicDataGet(RequestInvoiceGet request)
        {
            ResponseBasicDataGet response = new ResponseBasicDataGet();
            using (cnn = GetConnection())
            {
                try
                {

                    var data = cnn.QuerySingle<TKS_FAS_BasicData>("select * from TKS_FAS_BasicData where id=@Id", new
                    {
                        Id = request.Id
                    });

                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;
         
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponseBasicDataGet;
                }
            }
        }





        public ResponseInvoiceAdd WX_BasicDataAdd(RequestBasicDataSearch request)
        {

            ResponseInvoiceAdd response = new ResponseInvoiceAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    //string sql = @"select * from TKS_FAS_User where Id=@Id";
                    //var user = cnn.Query<TKS_FAS_User>(sql, new { Id = request.UserId }, ts).FirstOrDefault();
                    TKS_FAS_BasicData Invoice = new TKS_FAS_BasicData();
                    Invoice.Id = Guid.NewGuid().ToString("N");
                    Invoice.UserId = request.UserId;
                    Invoice.CreateUser = user.User.TrueName;
                    Invoice.CreateDate = DateTime.Now;
                    Invoice.Name = request.Name;
                    Invoice.DataType = request.DataType;
                    cnn.Insert<TKS_FAS_BasicData>(Invoice, ts);

               
                    ts.Commit();

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceAdd;
                }
            }
        }

        public ResponseInvoiceUpdate WX_BasicDataUpdate(RequestBasicDataSearch request)
        {
            ResponseInvoiceUpdate response = new ResponseInvoiceUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    //var user = cnn.Query<TKS_FAS_User>(@"select * from TKS_FAS_User where Id=@Id", new { Id = request.UserId }, ts).FirstOrDefault();
                    string update_Invoice = @" 
                            UPDATE  TKS_FAS_BasicData
                               SET
                                  Name =@Name
                                  
                             where id=@Id";
                    cnn.Execute(update_Invoice, new { Id = request.Id, Name = request .Name}, ts);
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "更新成功";
                    return response;


                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceUpdate;
                }

            }
        }

        public ResponseInvoiceDelete WX_BasicDataDelete(RequestInvoiceDelete request)
        {
            ResponseInvoiceDelete response = new ResponseInvoiceDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"select * from TKS_FAS_Invoice where BasicDataId=@BasicDataId";
                    var data = cnn.Query<TKS_FAS_Invoice>(sql, new
                    {
                        BasicDataId = request.Id

                    }, ts).ToList();

                    if (data.Count > 0)
                    {
                        ts.Commit();
                        response.IsSuccess = false;
                        response.Message = "已经被发票使用，无法删除";
                       
                    }
                    else
                    {
                        cnn.Execute("delete from TKS_FAS_BasicData where id=@Id", new { Id = request.Id }, ts);

                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "删除成功";
                    }
                    
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceDelete;
                }
            }
        }

        /// <summary>
        /// 发票递交
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseInvoiceDJ WX_InvoiceDJ(RequestInvoiceDJ request)
        {
            ResponseInvoiceDJ response = new ResponseInvoiceDJ();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    //var user = cnn.Query<TKS_FAS_User>(@"select * from TKS_FAS_User where Id=@Id", new { Id = request.UserId }, ts).FirstOrDefault();


                    string sql = @" 
                            UPDATE  TKS_FAS_Invoice
                               SET                                  
                                  Status = 1   
                                  ,UpdateUser =@UpdateUser
                                  ,UpdateDate =@UpdateDate
                                     
                             where id=@Id
                            ";
                    cnn.Execute(sql, new { Id = request.Id, UpdateDate = DateTime.Now, UpdateUser= user.User.TrueName}, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "递交成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceDJ;
                }

            }
        }

        public ResponseInvoiceDJ WX_InvoiceCX(RequestInvoiceDJ request)
        {
            ResponseInvoiceDJ response = new ResponseInvoiceDJ();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    //var user = cnn.Query<TKS_FAS_User>(@"select * from TKS_FAS_User where Id=@Id", new { Id = request.UserId }, ts).FirstOrDefault();


                    string sql = @" 
                            UPDATE  TKS_FAS_Invoice
                               SET                                  
                                  Status = 0   
                                  ,UpdateUser =@UpdateUser
                                  ,UpdateDate =@UpdateDate
                                     
                             where id=@Id
                            ";
                    cnn.Execute(sql, new { Id = request.Id, UpdateDate = DateTime.Now, UpdateUser = user.User.TrueName }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "撤销成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceDJ;
                }

            }
        }
    }
}
