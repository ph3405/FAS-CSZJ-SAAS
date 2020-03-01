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


namespace TKS.FAS.BLL.FAS.Set
{
    public class SupplierBLL : CommonBase
    {
        public ResponseSupplierListSearch SupplierListSearch(RequestSupplierListSearch request)
        {

            ResponseSupplierListSearch response = new ResponseSupplierListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    string sql = this.GetPageSql("*",
                        @"  TKS_FAS_Supplier where orgId=@OrgId",
                        " createDate  ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Supplier> data = cnn.Query<TKS_FAS_Supplier>(sql,
                        new
                        {
                            OrgId = user.Node.CreditCode
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_Supplier where orgId=@OrgId ";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        OrgId = user.Node.CreditCode
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

                    return this.DealException(response, ex) as ResponseSupplierListSearch;
                }
            }
        }

        public ResponseSupplierGet SupplierGet(RequestSupplierGet request)
        {
            ResponseSupplierGet response = new ResponseSupplierGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var data = cnn.QuerySingle<TKS_FAS_Supplier>("select * from tks_fas_Supplier where id=@Id", new
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
                    return this.DealException(response, ex) as ResponseSupplierGet;
                }
            }
        }

        public ResponseSupplierAdd SupplierAdd(RequestSupplierAdd request)
        {
            ResponseSupplierAdd response = new ResponseSupplierAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    var d = cnn.Query(@"select * from tks_fas_Supplier where 
                           supplierId=@SupplierId and orgId=@OrgId",
                        new { SupplierId=request.Data.SupplierId,OrgId=user.Node.CreditCode}, ts);

                    if (d.Count() > 0)
                    {
                        throw new NormalException("兼职会计已经存在");
                    }
                    

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.OrgId = user.Node.CreditCode;
                    request.Data.OrgName = user.Node.Name;
                    request.Data.CreateDate = DateTime.Now;
                    request.Data.CreateUser = user.User.UserName;
                    cnn.Insert<TKS_FAS_Supplier>(request.Data, ts);

                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSupplierAdd;
                }
            }
        }

        public ResponseSupplierUpdate SupplierUpdate(RequestSupplierUpdate request)
        {
            ResponseSupplierUpdate response = new ResponseSupplierUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    var d = cnn.Query(@"select * from tks_fas_Supplier where 
                           supplierId=@SupplierId and orgId=@OrgId",
                       new { SupplierId = request.Data.SupplierId, OrgId = user.Node.CreditCode }, ts);

                    if (d.Count() > 0)
                    {
                        throw new NormalException("兼职会计已经存在");
                    }
                   


                    string sql = @"update tks_fas_Supplier set 
                        supplierId=@SupplierId,
                        supplierName=@SupplierName
                        
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

                        throw new AppException(user.User.UserName, "SupplierUpdate", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSupplierUpdate;
                }

            }
        }

        public ResponseSupplierDelete SupplierDelete(RequestSupplierDelete request)
        {
            ResponseSupplierDelete response = new ResponseSupplierDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("delete from tks_fas_Supplier where id=@Id", new { Id=request.Id}, ts);



                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSupplierDelete;
                }
            }
        }
    }
}
