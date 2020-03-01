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
    public class InvoiceBLL : CommonBase
    {
        public InvoiceBLL() { }
        public InvoiceBLL(IDbConnection cnn)
        {
            this.cnn = cnn;
        }

        /// <summary>
        /// 雇主企业发票查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseInvoiceListSearch GZInvoiceListSearch(RequestInvoiceListSearch request)
        {

            ResponseInvoiceListSearch response = new ResponseInvoiceListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    if (string.IsNullOrEmpty(request.AccountId))
                    {
                        
                        string sql = this.GetPageSql("A.*, C.pzz + '-'+convert(varchar,B.PZZNO)  as pzz",
                            @"   TKS_FAS_Invoice A left join tks_fas_doc B 
   on A.PZId=B.Id
   left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
where A.accountId=@AccountId",
                            " A.CreateDate desc ",
                            request.PageSize,
                            request.PageIndex);

                        List<TKS_FAS_InvoiceExt> data = cnn.Query<TKS_FAS_InvoiceExt>(sql,
                            new
                            {
                                AccountId = user.AccountId
                            }, ts).ToList();

                        string countSql = @"select count(1) from TKS_FAS_Invoice where accountId=@AccountId ";

                        int total = int.Parse(cnn.ExecuteScalar(countSql, new
                        {
                            AccountId = user.AccountId
                        }, ts).ToString());

                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "加载完毕";
                        response.PageIndex = request.PageIndex;
                        response.Data = data;
                        response.Total = total;
                        return response;
                    }
                    else
                    {
                        string sql = this.GetPageSql("A.*, C.pzz + '-'+convert(varchar,B.PZZNO)  as pzz",
                            @"   TKS_FAS_Invoice A left join tks_fas_doc B 
   on A.PZId=B.Id
   left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
where A.accountId=@AccountId  and A.Status=@Status ",
                            " A.CreateDate desc ",
                            request.PageSize,
                            request.PageIndex);

                        List<TKS_FAS_InvoiceExt> data = cnn.Query<TKS_FAS_InvoiceExt>(sql,
                            new
                            {
                                AccountId = request.AccountId,
                                Status=request.Status
                            }, ts).ToList();

                        string countSql = @"select count(1) from TKS_FAS_Invoice where accountId=@AccountId and Status=@Status ";

                        int total = int.Parse(cnn.ExecuteScalar(countSql, new
                        {
                            AccountId = request.AccountId,
                            Status = request.Status
                        }, ts).ToString());

                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "加载完毕";
                        response.PageIndex = request.PageIndex;
                        response.Data = data;
                        response.Total = total;
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseInvoiceListSearch;
                }
            }
        }


        /// <summary>
        /// 应收应付发票查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseSFInvoiceListSearch SFInvoiceListSearch(RequestInvoiceListSearch request)
        {

            ResponseSFInvoiceListSearch response = new ResponseSFInvoiceListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string where = "";
                    string whereDetail = "";
                    if (!string.IsNullOrEmpty(request.InvoiceNo))
                    {
                        where += string.Format("and A.InvoiceNo like '%{0}%'", request.InvoiceNo);
                    }
                    if (!string.IsNullOrEmpty(request.BasicDataName))
                    {
                        where += string.Format("and TKS_FAS_BasicData.Name like '%{0}%'", request.BasicDataName);
                    }
                    if (!string.IsNullOrEmpty(request.StartSFDate))
                    {
                        whereDetail += string.Format("and SFDate >='{0}'", request.StartSFDate);
                    }
                    if (!string.IsNullOrEmpty(request.EndSFDate))
                    {
                        whereDetail += string.Format("and SFDate <='{0}'", request.EndSFDate);
                    }
                    if (request.IsAll=="1")
                    {
                        where = "";
                        whereDetail = "";
                    }
                    string sql = this.GetPageSql("A.*, C.pzz + '-'+convert(varchar,B.PZZNO)  as pzz,TKS_FAS_BasicData.Name,TKS_FAS_BasicData.DataType",
                            @"   TKS_FAS_Invoice A left join tks_fas_doc B 
   on A.PZId=B.Id
   left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
    left join TKS_FAS_BasicData on A.BasicDataId=TKS_FAS_BasicData.Id
where A.accountId=@AccountId AND A.IsUse='1' "+ where,
                            " A.SFType desc ",
                            request.PageSize,
                            request.PageIndex);

                    List<TKS_FAS_InvoiceExt> data = cnn.Query<TKS_FAS_InvoiceExt>(sql,
                        new
                        {
                            AccountId = request.AccountId
                        }, ts).OrderByDescending(p => p.CreateDate).ToList();

                    string countSql = @"select count(1) from TKS_FAS_Invoice where accountId=@AccountId  AND IsUse='1' ";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = user.AccountId
                    }, ts).ToString());

                    List<string> InvoiceIds = data.Select(p => p.Id).ToList();

                    string ids = "('" + string.Join("','", InvoiceIds) + "')";

                    sql = @"select * from TKS_FAS_InvoiceSFDetail where InvoiceId in " + ids+ whereDetail+ " order by Convert(datetime, SFDate)";

                    var detail = cnn.Query<TKS_FAS_InvoiceSFDetail>(sql, null, ts).ToList();
                    //List<string> orderby = detail.Select(p => p.InvoiceId).Distinct().ToList();
                    List<TKS_FAS_InvoiceExt> newData = new List<TKS_FAS_InvoiceExt>();
                    foreach (var det in detail)
                    {
                        foreach (var item in data)
                        {
                            if (det.InvoiceId == item.Id)
                            {
                                newData.Add(item);
                            }
                        }

                    }
                    //for (int i = 0; i < orderby.Count; i++)
                    //{
                    //    foreach (var item in data)
                    //    {
                    //        if (item.Id== orderby[i])
                    //        {
                    //            newData.Add(item);
                    //        }
                    //    }
                    //}
                    var result = (from item in newData.OrderByDescending(a=>a.SFType).Distinct()
                                  select new InvoiceList
                                  {
                                      Head = item,
                                      Detail = detail.Where(p => p.InvoiceId == item.Id).OrderBy(p => p.SFDate).ToList()
                                  }).ToList();
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    response.Data = result;
                    response.Total = total;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseSFInvoiceListSearch;
                }
            }
        }

        /// <summary>
        /// 应收应付提醒查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseSFInvoiceListSearch SFWarnListSearch(RequestInvoiceListSearch request)
        {

            ResponseSFInvoiceListSearch response = new ResponseSFInvoiceListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = @"select * from TKS_FAS_InvoiceSFDetail where Status='OP' AND SFDate>=CONVERT(varchar(50), GETDATE(), 23)
                    and SFDate<=Convert(varchar(50), DATEADD(DAY, 7, getdate()), 23)";

                    var detail = cnn.Query<TKS_FAS_InvoiceSFDetail>(sql, null, ts).OrderBy(p => p.SFDate).ToList();
                    List<string> InvoiceIds = detail.Select(p => p.InvoiceId).Distinct().ToList();

                    string ids = "('" + string.Join("','", InvoiceIds) + "')";
                    if (InvoiceIds.Count > 0)
                    {


                        sql = this.GetPageSql("A.*, C.pzz + '-'+convert(varchar,B.PZZNO)  as pzz,TKS_FAS_BasicData.Name,TKS_FAS_BasicData.DataType",
                                @"   TKS_FAS_Invoice A left join tks_fas_doc B 
   on A.PZId=B.Id
   left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
    left join TKS_FAS_BasicData on A.BasicDataId=TKS_FAS_BasicData.Id
where A.accountId=@AccountId AND A.IsUse='1' AND A.Id in " + ids,
                                " A.SFType desc ",
                                request.PageSize,
                                request.PageIndex);
                    }
                    else
                    {
                        sql = this.GetPageSql("A.*, C.pzz + '-'+convert(varchar,B.PZZNO)  as pzz,TKS_FAS_BasicData.Name,TKS_FAS_BasicData.DataType",
                                @"   TKS_FAS_Invoice A left join tks_fas_doc B 
   on A.PZId=B.Id
   left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
    left join TKS_FAS_BasicData on A.BasicDataId=TKS_FAS_BasicData.Id
where A.accountId=@AccountId AND A.IsUse='1' AND 1=2",
                                " A.SFType desc ",
                                request.PageSize,
                                request.PageIndex);
                    }
                    List<TKS_FAS_InvoiceExt> data = cnn.Query<TKS_FAS_InvoiceExt>(sql,
                        new
                        {
                            AccountId = request.AccountId
                        }, ts).OrderByDescending(p => p.CreateDate).ToList();

                    string countSql = @"select count(1) from TKS_FAS_Invoice where accountId=@AccountId AND IsUse='1' AND Id in " + ids;
                    if (InvoiceIds.Count <= 0)
                    {
                        countSql = @"select count(1) from TKS_FAS_Invoice where accountId=@AccountId AND IsUse='1' AND 1=2";
                    }
                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = request.AccountId
                    }, ts).ToString());

                    List<string> orderby = detail.Select(p => p.InvoiceId).Distinct().ToList();
                    List<TKS_FAS_InvoiceExt> newData = new List<TKS_FAS_InvoiceExt>();
                    for (int i = 0; i < orderby.Count; i++)
                    {
                        foreach (var item in data)
                        {
                            if (item.Id == orderby[i])
                            {
                                newData.Add(item);
                            }
                        }
                    }
                    var result = (from item in newData.OrderByDescending(a => a.SFType)
                                  select new InvoiceList
                                  {
                                      Head = item,
                                      Detail = detail.Where(p => p.InvoiceId == item.Id).OrderBy(p => p.SFDate).ToList()
                                  }).ToList();
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    response.Data = result;
                    response.Total = total;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseSFInvoiceListSearch;
                }
            }
        }

        /// <summary>
        /// 实时未收付查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseSFInvoiceListSearch SFNoTimeListSearch(RequestInvoiceListSearch request)
        {

            ResponseSFInvoiceListSearch response = new ResponseSFInvoiceListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = @"select * from TKS_FAS_InvoiceSFDetail where SFStatus in('未收','未付')";

                    var detail = cnn.Query<TKS_FAS_InvoiceSFDetail>(sql, null, ts).OrderBy(p => p.SFDate).ToList();
                    List<string> InvoiceIds = detail.Select(p => p.InvoiceId).Distinct().ToList();

                    string ids = "('" + string.Join("','", InvoiceIds) + "')";
                    if (InvoiceIds.Count > 0)
                    {


                        sql = this.GetPageSql("A.*, C.pzz + '-'+convert(varchar,B.PZZNO)  as pzz,TKS_FAS_BasicData.Name,TKS_FAS_BasicData.DataType",
                                @"   TKS_FAS_Invoice A left join tks_fas_doc B 
   on A.PZId=B.Id
   left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
    left join TKS_FAS_BasicData on A.BasicDataId=TKS_FAS_BasicData.Id
where A.accountId=@AccountId AND A.IsUse='1' AND A.Id in " + ids,
                                " A.SFType desc ",
                                request.PageSize,
                                request.PageIndex);
                    }
                    else
                    {
                        sql = this.GetPageSql("A.*, C.pzz + '-'+convert(varchar,B.PZZNO)  as pzz,TKS_FAS_BasicData.Name,TKS_FAS_BasicData.DataType",
                                @"   TKS_FAS_Invoice A left join tks_fas_doc B 
   on A.PZId=B.Id
   left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
    left join TKS_FAS_BasicData on A.BasicDataId=TKS_FAS_BasicData.Id
where A.accountId=@AccountId AND A.IsUse='1' AND 1=2",
                                " A.SFType desc ",
                                request.PageSize,
                                request.PageIndex);
                    }
                    List<TKS_FAS_InvoiceExt> data = cnn.Query<TKS_FAS_InvoiceExt>(sql,
                        new
                        {
                            AccountId = request.AccountId
                        }, ts).OrderByDescending(p => p.InvoiceDate).ToList();

                    string countSql = @"select count(1) from TKS_FAS_Invoice where accountId=@AccountId AND IsUse='1' AND Id in " + ids;
                    if (InvoiceIds.Count <= 0)
                    {
                        countSql = @"select count(1) from TKS_FAS_Invoice where accountId=@AccountId AND IsUse='1' AND 1=2";
                    }
                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = request.AccountId
                    }, ts).ToString());

                    List<string> orderby = detail.Select(p => p.InvoiceId).Distinct().ToList();
                    List<TKS_FAS_InvoiceExt> newData = new List<TKS_FAS_InvoiceExt>();
                    for (int i = 0; i < orderby.Count; i++)
                    {
                        foreach (var item in data)
                        {
                            if (item.Id == orderby[i])
                            {
                                newData.Add(item);
                            }
                        }
                    }
                    var result = (from item in newData.OrderByDescending(a => a.SFType)
                                  select new InvoiceList
                                  {
                                      Head = item,
                                      Detail = detail.Where(p => p.InvoiceId == item.Id).OrderBy(p => p.SFDate).ToList()
                                  }).ToList();
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    response.Data = result;
                    response.Total = total;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseSFInvoiceListSearch;
                }
            }
        }
        /// <summary>
        /// 代帐企业发票查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseInvoiceListSearch DZInvoiceListSearch(RequestInvoiceListSearch request)
        {

            ResponseInvoiceListSearch response = new ResponseInvoiceListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = this.GetPageSql("A.*, C.pzz + '-'+convert(varchar,B.PZZNO)  as pzz",
                        @" TKS_FAS_Invoice A left join tks_fas_doc B 
   on A.PZId=B.Id
   left join TKS_FAS_CertificateWord C on B.PZZ=C.Id 
where A.accountId=@AccountId and A.status<>0",
                        " A.createDate desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_InvoiceExt> data = cnn.Query<TKS_FAS_InvoiceExt>(sql,
                        new
                        {
                            AccountId = user.AccountId
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_Invoice where accountId=@AccountId and status<>0";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = user.AccountId
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

                    return this.DealException(response, ex) as ResponseInvoiceListSearch;
                }
            }
        }


        public ResponseInvoiceGet InvoiceGet(RequestInvoiceGet request)
        {
            ResponseInvoiceGet response = new ResponseInvoiceGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var data = new TKS_FAS_InvoiceExt();
                    var lstSFDetail = new List<TKS_FAS_InvoiceSFDetail>();
                    if (!string.IsNullOrEmpty(request.Id))
                    {
                        data = cnn.QuerySingle<TKS_FAS_InvoiceExt>("select *,TKS_FAS_BasicData.Name as BasicDataName,TKS_FAS_BasicData.DataType from TKS_FAS_Invoice left join TKS_FAS_BasicData on TKS_FAS_Invoice.BasicDataId=TKS_FAS_BasicData.Id where TKS_FAS_Invoice.id=@Id", new
                        {
                            Id = request.Id
                        });
                        lstSFDetail = cnn.Query<TKS_FAS_InvoiceSFDetail>(@"select * from TKS_FAS_InvoiceSFDetail  where  InvoiceId=@InvoiceId",
                       new
                       {
                           InvoiceId = request.Id
                       }).ToList();

                    }
                    else
                    {
                        data = cnn.QuerySingle<TKS_FAS_InvoiceExt>("select *,TKS_FAS_BasicData.Name as BasicDataName,TKS_FAS_BasicData.DataType from TKS_FAS_Invoice left join TKS_FAS_BasicData on TKS_FAS_Invoice.BasicDataId=TKS_FAS_BasicData.Id where TKS_FAS_Invoice.id=@Id", new
                        {
                            Id = request.Data.Id
                        });
                        lstSFDetail = cnn.Query<TKS_FAS_InvoiceSFDetail>(@"select * from TKS_FAS_InvoiceSFDetail  where  InvoiceId=@InvoiceId",
                       new
                       {
                           InvoiceId = request.Data.Id
                       }).ToList();
                    }


                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;
                    response.lstSFDetail = lstSFDetail;
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponseInvoiceGet;
                }
            }
        }

        public ResponseInvoiceGet WX_InvoiceGet(RequestInvoiceGet request)
        {
            ResponseInvoiceGet response = new ResponseInvoiceGet();
            using (cnn = GetConnection())
            {
                try
                {

                    var data = cnn.QuerySingle<TKS_FAS_InvoiceExt>("select *,TKS_FAS_BasicData.Name,TKS_FAS_BasicData.DataType from TKS_FAS_Invoice left join TKS_FAS_BasicData on TKS_FAS_Invoice.BasicDataId=TKS_FAS_BasicData.Id where TKS_FAS_Invoice.id=@Id", new
                    {
                        Id = request.Id
                    });
                    List<TKS_FAS_InvoiceAttach> lst = cnn.Query<TKS_FAS_InvoiceAttach>(@"select * from TKS_FAS_InvoiceAttach  where  invoiceId=@InvoiceId",
                       new
                       {
                           InvoiceId = request.Id
                       }).ToList();
                    List<TKS_FAS_InvoiceSFDetail> lstSFDetail = cnn.Query<TKS_FAS_InvoiceSFDetail>(@"select * from TKS_FAS_InvoiceSFDetail  where  InvoiceId=@InvoiceId",
                       new
                       {
                           InvoiceId = request.Id
                       }).ToList();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;
                    response.lstAttach = lst;
                    response.lstSFDetail = lstSFDetail;
                    return response;
                }
                catch (Exception ex)
                {
                    return this.DealException(response, ex) as ResponseInvoiceGet;
                }
            }
        }
        public ResponseInvoiceAdd InvoiceAdd(RequestInvoiceAdd request)
        {
            ResponseInvoiceAdd response = new ResponseInvoiceAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.AccountId = user.AccountId;
                    request.Data.CreateUser = user.User.TrueName;
                    request.Data.CreateDate = DateTime.Now;
                    cnn.Insert<TKS_FAS_Invoice>(request.Data, ts);
                    JavaScriptSerializer Serializer = new JavaScriptSerializer();
                    List<SFDetail> lstSFDetail = Serializer.Deserialize<List<SFDetail>>(request.SFDetail);
                    //添加收付明细
                    if (request.Data.IsUse == "1")
                    {
                        if (lstSFDetail.Count > 0)
                        {
                            foreach (var item in lstSFDetail)
                            {
                                TKS_FAS_InvoiceSFDetail SF = new TKS_FAS_InvoiceSFDetail();
                                SF.Id = Guid.NewGuid().ToString("N");
                                SF.InvoiceId = request.Data.Id;
                                SF.SFDate = item.SFDate;
                                SF.SFMoney = Convert.ToDecimal(item.SFMoney);
                                SF.SFRemark = item.SFRemark;
                                SF.Seq = int.Parse(item.Seq);
                                SF.SFStatus = item.SFStatus;
                                SF.Status = "OP";
                                SF.CreateUser = user.User.TrueName;
                                SF.CreateDate = DateTime.Now;
                                cnn.Insert<TKS_FAS_InvoiceSFDetail>(SF, ts);
                            }
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
                    return this.DealException(response, ex) as ResponseInvoiceAdd;
                }
            }
        }

        /// <summary>
        /// 移动端上传发票
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="lsPic"></param>
        public void MInvoiceAdd(string token, TKS_FAS_Invoice invoice, List<TKS_FAS_InvoiceAttach> lsPic)
        {
            using (cnn = this.GetConnection())
            {

                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(token, ts);
                    invoice.Id = Guid.NewGuid().ToString("N");
                    invoice.AccountId = user.AccountId;
                    invoice.CreateUser = user.User.TrueName;
                    invoice.CreateDate = DateTime.Now;
                    invoice.Status = 0;
                    cnn.Insert<TKS_FAS_Invoice>(invoice, ts);

                    foreach (var item in lsPic)
                    {
                        item.Id = Guid.NewGuid().ToString("N");
                        item.InvoiceId = invoice.Id;
                        item.AccountId = user.AccountId;
                        item.CreateDate = invoice.CreateDate;
                        item.CreateUser = user.User.TrueName;
                        cnn.Insert<TKS_FAS_InvoiceAttach>(item, ts);
                    }

                    ts.Commit();
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw new AppException("", "MInvoiceAdd", "上传失败，请重试", ex.Message);
                }
            }
        }

        public ResponseInvoiceUpdate InvoiceUpdate(RequestInvoiceUpdate request)
        {
            ResponseInvoiceUpdate response = new ResponseInvoiceUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);


                    string sql = @" 
                            UPDATE  TKS_FAS_Invoice
                               SET
                                  InvoiceNo =@InvoiceNo
                                    ,InvoiceDate =@InvoiceDate
                                  ,Type =@Type
                                  ,IsVAT = @IsVAT
                                  ,RPStatus = @RPStatus
                                  ,PayMode =@PayMode
     
                                  ,Money =@Money
                                  ,TaxMoney =@TaxMoney
                               
     
                                  ,UpdateUser =@UpdateUser
                                  ,UpdateDate =@UpdateDate
                                  ,Memo = @Memo
                                    ,IsUse=@IsUse
                                    ,IsTaxYZ=@IsTaxYZ
                                    ,SFType=@SFType
                                    ,BasicDataId=@BasicDataId
                                    ,BadMoney=@BadMoney
                             where id=@Id

                            ";

                    var r = cnn.Execute(sql, request.Data, ts);
                    JavaScriptSerializer Serializer = new JavaScriptSerializer();
                    List<SFDetail> lstSFDetail = Serializer.Deserialize<List<SFDetail>>(request.SFDetail);
                    if (request.Data.IsUse == "1")
                    {
                        cnn.Execute(@"delete from TKS_FAS_InvoiceSFDetail where InvoiceId=@InvoiceId", new { InvoiceId = request.Data.Id }, ts);
                        if (lstSFDetail.Count > 0)
                        {
                            foreach (var item in lstSFDetail)
                            {
                                TKS_FAS_InvoiceSFDetail SF = new TKS_FAS_InvoiceSFDetail();
                                SF.Id = Guid.NewGuid().ToString("N");
                                SF.InvoiceId = request.Data.Id;
                                SF.SFDate = item.SFDate;
                                SF.SFMoney = Convert.ToDecimal(item.SFMoney);
                                SF.SFRemark = item.SFRemark;
                                SF.SFStatus = item.SFStatus;
                                SF.Seq = int.Parse(item.Seq);
                                SF.Status = "OP";
                                SF.CreateUser = user.User.TrueName;
                                SF.CreateDate = DateTime.Now;
                                cnn.Insert<TKS_FAS_InvoiceSFDetail>(SF, ts);
                            }
                        }
                    }

                    if (r == 1)
                    {
                        ts.Commit();
                        response.Id = request.Data.Id;
                        response.IsSuccess = true;
                        response.Message = "更新成功";
                        return response;
                    }
                    else
                    {

                        throw new AppException(user.User.UserName, "InvoiceUpdate", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceUpdate;
                }

            }
        }

        public ResponseInvoiceDelete InvoiceDelete(RequestInvoiceDelete request)
        {
            ResponseInvoiceDelete response = new ResponseInvoiceDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("delete from TKS_FAS_Invoice where id=@Id", request.Data, ts);

                    cnn.Execute(@"delete from TKS_FAS_InvoiceSFDetail where InvoiceId=@InvoiceId", new { InvoiceId = request.Data.Id }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
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
        public ResponseInvoiceDJ InvoiceDJ(RequestInvoiceDJ request)
        {
            ResponseInvoiceDJ response = new ResponseInvoiceDJ();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);


                    string sql = @" 
                            UPDATE  TKS_FAS_Invoice
                               SET
                                  
                                  Status = @Status
     
                                  ,UpdateUser =@UpdateUser
                                  ,UpdateDate =@UpdateDate
                                 
     
                             where id=@Id

                            ";
                    request.Data.Status = 1;
                    request.Data.UpdateUser = user.User.UserName;
                    request.Data.UpdateDate = DateTime.Now;
                    var r = cnn.Execute(sql, request.Data, ts);


                    if (r == 1)
                    {
                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "递交成功";
                        return response;
                    }
                    else
                    {

                        throw new AppException(user.User.UserName, "InvoiceUpdate", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceDJ;
                }

            }
        }

        /// <summary>
        /// 撤销发票递交
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseInvoiceCX InvoiceCX(RequestInvoiceCX request)
        {
            ResponseInvoiceCX response = new ResponseInvoiceCX();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    var data = cnn.QueryFirstOrDefault<TKS_FAS_Invoice>(
                        @"select * from TKS_FAS_Invoice where id=@Id ", new { Id = request.Data.Id }, ts);
                    if (data.Status == 2)
                    {
                        throw new Exception("财务已经立账");
                    }
                    string sql = @" 
                            UPDATE  TKS_FAS_Invoice
                               SET
                                  
                                  Status = @Status
     
                                  ,UpdateUser =@UpdateUser
                                  ,UpdateDate =@UpdateDate
                                 
     
                             where id=@Id

                            ";
                    request.Data.Status = 0;
                    request.Data.UpdateUser = user.User.UserName;
                    request.Data.UpdateDate = DateTime.Now;
                    var r = cnn.Execute(sql, request.Data, ts);


                    if (r == 1)
                    {
                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "撤销成功";
                        return response;
                    }
                    else
                    {

                        throw new AppException(user.User.UserName, "InvoiceCX", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceCX;
                }

            }
        }

        /// <summary>
        /// 发票立账，即生成凭证（获取对应的凭证模板ID）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseInvoiceLZ InvoiceLZ(RequestInvoiceLZ request)
        {
            ResponseInvoiceLZ response = new ResponseInvoiceLZ();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);


                    string sql = @"select * from TKS_FAS_Invoice where Id=@Id";

                    var invoice = cnn.QueryFirst<TKS_FAS_Invoice>(sql, new { Id = request.Data.Id }, ts);


                    sql = @"select * from TKS_FAS_Invoice2TPL where Type=@Type and IsVat=@IsVAT 
                        and RPStatus=@RPStatus and payMode=@PayMode and AccountId=@AccountId";

                    var tpl = cnn.Query<TKS_FAS_Invoice2TPL>(sql, invoice, ts).ToList();


                    if (tpl.Count() == 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "很抱歉，没有匹配的凭证模板";
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.TPLId = tpl[0].TPLID;
                        response.Money = invoice.Money;
                        response.Tax = invoice.TaxMoney;
                        response.Message = "获取成功";
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceLZ;
                }

            }
        }

        /// <summary>
        /// 立账
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ts"></param>
        public void LZ(string id, string pzId, MM_UserInfo user, IDbTransaction ts)
        {

            string sql = @" UPDATE  TKS_FAS_Invoice
                               SET
                                  Status = @Status
                                  ,UpdateUser =@UpdateUser
                                  ,UpdateDate =@UpdateDate
                                   ,pzid=@PZId
                             where id=@Id
                            ";
            var r = cnn.Execute(sql, new
            {
                Status = 2,
                UpdateUser = user.User.UserName,
                UpdateDate = DateTime.Now,
                Id = id,
                PZId = pzId
            }, ts);


            sql = @"INSERT INTO  TKS_FAS_Attachment
           (Id
           , Title
           , Path
           , AccountId
           , DocId
           , CreateUser
           , CreateDate)
            select newid(),
                   title,
                    path,
                    accountId,
                    @DocId,
                    @CreateUser,
                    @CreateDate
            from 
                TKS_FAS_InvoiceAttach
            where 
                invoiceId=@InvoiceId
";
            cnn.Execute(sql, new { DocId = pzId, CreateUser = user.User.UserName, CreateDate = DateTime.Now, InvoiceId = id }, ts);
        }


        public ResponseInvoiceDataGet DataGet(RequestInvoiceDataGet request)
        {
            var res = new ResponseInvoiceDataGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var data = new List<TKS_FAS_Invoice2TPL>();
                    if (!string.IsNullOrEmpty(request.AccountId))
                    {
                        data = cnn.Query<TKS_FAS_Invoice2TPL>(
                        @"select * from TKS_FAS_Invoice2TPL where accountID=@AccountId",
                        new { AccountId = request.AccountId }, ts
                        ).ToList();
                    }
                    else
                    {
                        var user = UserInfoGet(request.Token, ts);
                        data = cnn.Query<TKS_FAS_Invoice2TPL>(
                           @"select * from TKS_FAS_Invoice2TPL where accountID=@AccountId",
                           new { AccountId = user.AccountId }, ts
                           ).ToList();
                    }
                    

                    var fpData = cnn.Query<TKS_FAS_DATA>(
                        @"select * from TKS_FAS_DATA where parentId='001'", null, ts).ToList();

                    var ynData = cnn.Query<TKS_FAS_DATA>(
                        @"select * from TKS_FAS_DATA where parentId='200'", null, ts).ToList();

                    var zfData = cnn.Query<TKS_FAS_DATA>(
                        @"select * from TKS_FAS_DATA where parentId='100'", null, ts).ToList();
                    var rpData = cnn.Query<TKS_FAS_DATA>(
                        @"select * from TKS_FAS_DATA where parentId='300'", null, ts).ToList();
                    List<TKS_FAS_DATA> result = new List<TKS_FAS_DATA>();
                    if (request.Type == "1")//一级联动
                    {
                        #region 发票类型
                        var d = data;
                        var dis = (from item in d
                                   select new
                                   {
                                       Code = item.Type.ToString(),
                                       Name = fpData.Where(p => p.Code == item.Type.ToString()).SingleOrDefault().Name
                                   }).Distinct().ToList();
                        result = (from item in dis
                                  select new TKS_FAS_DATA
                                  {
                                      Code = item.Code,
                                      Name = item.Name
                                  }).ToList();
                        #endregion
                    }
                    else if (request.Type == "2")//二级联动
                    {
                        #region 是否增票
                        var d = data.Where(p =>
                        {
                            return p.Type == request.FPType;
                        }).ToList();
                        var dis = (from item in d
                                   select new
                                   {
                                       Code = item.IsVAT.ToString(),
                                       Name = ynData.Where(p => p.Code == item.IsVAT.ToString()).SingleOrDefault().Name
                                   }).Distinct().ToList();

                        result = (from item in dis
                                  select new TKS_FAS_DATA
                                  {
                                      Code = item.Code,
                                      Name = item.Name
                                  }).ToList();
                        #endregion
                    }
                    else if (request.Type == "3")
                    {
                        #region 收付状态
                        var d = data.Where(p =>
                        {
                            return p.Type == request.FPType && p.IsVAT == request.IsVAT;
                        }).ToList();
                        var dis = (from item in d
                                   select new
                                   {
                                       Code = item.RPStatus.ToString(),
                                       Name = rpData.Where(p => p.Code == item.RPStatus.ToString()).SingleOrDefault().Name
                                   }).Distinct().ToList();

                        result = (from item in dis
                                  select new TKS_FAS_DATA
                                  {
                                      Code = item.Code,
                                      Name = item.Name
                                  }).ToList();
                        #endregion
                    }
                    else if (request.Type == "4")
                    {
                        #region 支付方式
                        var d = data.Where(p =>
                        {
                            return p.Type == request.FPType && p.IsVAT == request.IsVAT
                            && p.RPStatus == request.RPStatus;
                        }).ToList();
                        var dis = (from item in d
                                   select new
                                   {
                                       Code = item.PayMode.ToString(),
                                       Name = zfData.Where(p => p.Code == item.PayMode.ToString()).SingleOrDefault().Name
                                   }).Distinct().ToList();

                        result = (from item in dis
                                  select new TKS_FAS_DATA
                                  {
                                      Code = item.Code,
                                      Name = item.Name
                                  }).ToList();
                        #endregion
                    }
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Data = result;
                    res.Message = "ok";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseInvoiceDataGet;
                }
            }
        }

        public ResponseInvoiceCX UploadFile()
        {
            ResponseInvoiceCX response = new ResponseInvoiceCX();
            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files["Filedata"]; //上传文件
                //HttpFileCollection file = System.Web.HttpContext.Current.Request.Files;
                Guid fileguid = Guid.NewGuid(); //生成新的文件名 不包含.和后缀名
                string newfilename = Guid.NewGuid().ToString("N");
                string uploadPath = HttpContext.Current.Server.MapPath("/uploadPic/");
                if (file != null)
                {
     
                    string extension = Path.GetExtension(file.FileName);
                    newfilename += extension;

                    string xdpath = "/uploadPic/" + "发票/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "/";
                    uploadPath += "发票\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "\\";

                    //判断路径是否存在
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    uploadPath += newfilename;


                    file.SaveAs(uploadPath);
                    xdpath += newfilename;


                    response.IsSuccess = true;
                    response.Data = xdpath;
                    response.Title = file.FileName;
                    string invoiceId = HttpContext.Current.Request.Form["invoiceId"];
                    string token = HttpContext.Current.Request.Form["token"];
                    if (!string.IsNullOrEmpty(invoiceId))
                    {
                        FPFJBLL bll = new FPFJBLL();
                        var res = bll.FPFJAdd(new TKS.FAS.Entity.FAS.RequestFPFJAdd
                        {
                            Token = token,
                            Data = new TKS.FAS.Entity.TKS_FAS_InvoiceAttach
                            {
                                Title = file.FileName,
                                InvoiceId = invoiceId,
                                Path = xdpath
                            }
                        });
                    }
                }
                else
                {
                    response.IsSuccess = false;
                }
                return response;
            }
            catch (Exception ex)
            {
                return this.DealException(response, ex) as ResponseInvoiceCX;
            }
            finally
            {
                GC.Collect();
            }


        }

        public ResponseInvoiceAdd WX_InvoiceAdd(RequestInvoiceAdd request)
        {
            JObject json_object = (JObject)JsonConvert.DeserializeObject(request.InvoiceInfo);
            string InvoiceNo = Convert.ToString(json_object.GetValue("InvoiceNo"));
            string InvoiceDate = Convert.ToString(json_object.GetValue("InvoiceDate"));
            int Type = Convert.ToInt32(Convert.ToString(json_object.GetValue("Type")) == "" ? "-1" : Convert.ToString(json_object.GetValue("Type")));
            int IsVAT = Convert.ToInt32(Convert.ToString(json_object.GetValue("IsVAT")) == "" ? "-1" : Convert.ToString(json_object.GetValue("IsVAT")));
            int RPStatus = Convert.ToInt32(Convert.ToString(json_object.GetValue("RPStatus")) == "" ? "-1" : Convert.ToString(json_object.GetValue("RPStatus")));

            int PayMode = Convert.ToInt32(Convert.ToString(json_object.GetValue("PayMode")) == "" ? "-1" : Convert.ToString(json_object.GetValue("PayMode")));
            int InvoiceNum = Convert.ToInt32(json_object.GetValue("InvoiceNum"));
            decimal Money = Convert.ToDecimal(json_object.GetValue("Money"));
            decimal TaxMoney = Convert.ToDecimal(json_object.GetValue("TaxMoney"));
            string Remark = Convert.ToString(json_object.GetValue("Remark"));

            string IsUse = Convert.ToString(json_object.GetValue("IsUse"));
            string IsTaxYZ = Convert.ToString(json_object.GetValue("IsTaxYZ"));
            string SFType = Convert.ToString(json_object.GetValue("SFType"));
            string BasicDataId = Convert.ToString(json_object.GetValue("BasicDataId"));
            decimal BadMoney = Convert.ToDecimal(json_object.GetValue("BadMoney"));
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<InvoiceAttach> lstAttach = Serializer.Deserialize<List<InvoiceAttach>>(request.Detail);

            List<SFDetail> lstSFDetail = Serializer.Deserialize<List<SFDetail>>(request.SFDetail);
            ResponseInvoiceAdd response = new ResponseInvoiceAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    //string sql = @"select * from TKS_FAS_User where Id=@Id";
                    //var user = cnn.Query<TKS_FAS_User>(sql, new { Id = request.UserId }, ts).FirstOrDefault();
                    TKS_FAS_Invoice Invoice = new TKS_FAS_Invoice();
                    Invoice.Id = Guid.NewGuid().ToString("N");
                    Invoice.AccountId = request.AccountId;
                    Invoice.CreateUser = user.User.TrueName;
                    Invoice.CreateDate = DateTime.Now;
                    Invoice.InvoiceNo = InvoiceNo;
                    Invoice.InvoiceDate = InvoiceDate;
                    Invoice.Type = Type;
                    Invoice.IsVAT = IsVAT;
                    Invoice.RPStatus = RPStatus;
                    Invoice.PayMode = PayMode;
                    Invoice.InvoiceNum = InvoiceNum;
                    Invoice.TaxMoney = TaxMoney;
                    Invoice.Money = Money;
                    Invoice.Status = 0;
                    Invoice.Memo = Remark;

                    Invoice.IsUse = IsUse;
                    Invoice.IsTaxYZ = IsTaxYZ;
                    Invoice.SFType = SFType;
                    Invoice.BasicDataId = BasicDataId;
                    Invoice.BadMoney = BadMoney;
                    cnn.Insert<TKS_FAS_Invoice>(Invoice, ts);
                    //添加发票附件
                    if (lstAttach.Count > 0)
                    {
                        foreach (var item in lstAttach)
                        {
                            TKS_FAS_InvoiceAttach Attach = new TKS_FAS_InvoiceAttach();
                            Attach.Id = Guid.NewGuid().ToString("N");
                            Attach.AccountId = request.AccountId;
                            Attach.CreateUser = user.User.TrueName;
                            Attach.CreateDate = DateTime.Now;
                            Attach.Title = item.Title;
                            Attach.Path = item.Path;
                            Attach.InvoiceId = Invoice.Id;
                            cnn.Insert<TKS_FAS_InvoiceAttach>(Attach, ts);
                        }
                    }

                    //添加收付明细
                    if (IsUse=="1")
                    {
                        if (lstSFDetail.Count > 0)
                        {
                            foreach (var item in lstSFDetail)
                            {
                                TKS_FAS_InvoiceSFDetail SF = new TKS_FAS_InvoiceSFDetail();
                                SF.Id = Guid.NewGuid().ToString("N");
                                SF.InvoiceId = Invoice.Id;                              
                                SF.SFDate = item.SFDate;
                                SF.SFMoney = Convert.ToDecimal(item.SFMoney);
                                SF.SFRemark = item.SFRemark;
                                SF.Seq = int.Parse(item.Seq);
                                SF.SFStatus = item.SFStatus;
                                SF.Status = "OP";
                                SF.CreateUser = user.User.TrueName;
                                SF.CreateDate = DateTime.Now;
                                cnn.Insert<TKS_FAS_InvoiceSFDetail>(SF, ts);
                            }
                        }
                    }
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

        public ResponseInvoiceUpdate WX_InvoiceUpdate(RequestInvoiceUpdate request)
        {
            JObject json_object = (JObject)JsonConvert.DeserializeObject(request.InvoiceInfo);
            string InvoiceNo = Convert.ToString(json_object.GetValue("InvoiceNo"));
            string InvoiceDate = Convert.ToString(json_object.GetValue("InvoiceDate"));
            int Type = Convert.ToInt32(Convert.ToString(json_object.GetValue("Type")) == "" ? "-1" : Convert.ToString(json_object.GetValue("Type")));
            int IsVAT = Convert.ToInt32(Convert.ToString(json_object.GetValue("IsVAT")) == "" ? "-1" : Convert.ToString(json_object.GetValue("IsVAT")));
            int RPStatus = Convert.ToInt32(Convert.ToString(json_object.GetValue("RPStatus")) == "" ? "-1" : Convert.ToString(json_object.GetValue("RPStatus")));

            int PayMode = Convert.ToInt32(Convert.ToString(json_object.GetValue("PayMode")) == "" ? "-1" : Convert.ToString(json_object.GetValue("PayMode")));
            int InvoiceNum = Convert.ToInt32(json_object.GetValue("InvoiceNum"));
            decimal Money = Convert.ToDecimal(json_object.GetValue("Money"));
            decimal TaxMoney = Convert.ToDecimal(json_object.GetValue("TaxMoney"));
            string Remark = Convert.ToString(json_object.GetValue("Remark"));

            string IsUse = Convert.ToString(json_object.GetValue("IsUse"));
            string IsTaxYZ = Convert.ToString(json_object.GetValue("IsTaxYZ"));
            string SFType = Convert.ToString(json_object.GetValue("SFType"));
            string BasicDataId = Convert.ToString(json_object.GetValue("BasicDataId"));
            decimal BadMoney = Convert.ToDecimal(json_object.GetValue("BadMoney"));
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<InvoiceAttach> lstAttach = Serializer.Deserialize<List<InvoiceAttach>>(request.Detail);

            List<SFDetail> lstSFDetail = Serializer.Deserialize<List<SFDetail>>(request.SFDetail);
            ResponseInvoiceUpdate response = new ResponseInvoiceUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    //var user = cnn.Query<TKS_FAS_User>(@"select * from TKS_FAS_User where Id=@Id", new { Id = request.UserId }, ts).FirstOrDefault();
                    string update_Invoice = @" 
                            UPDATE  TKS_FAS_Invoice
                               SET
                                  InvoiceNo =@InvoiceNo
                                    ,InvoiceDate =@InvoiceDate
                                  ,Type =@Type
                                  ,IsVAT = @IsVAT
                                  ,RPStatus = @RPStatus
                                  ,PayMode =@PayMode    
                                  ,Money =@Money
                                  ,TaxMoney =@TaxMoney    
                                  ,UpdateUser =@UpdateUser
                                  ,UpdateDate =@UpdateDate
                                  ,Memo = @Memo
                                  ,InvoiceNum=@InvoiceNum
                                    ,IsUse=@IsUse
                                    ,IsTaxYZ=@IsTaxYZ
                                    ,SFType=@SFType
                                    ,BasicDataId=@BasicDataId
                                    ,BadMoney=@BadMoney
                             where id=@Id";
                    cnn.Execute(update_Invoice, new { Id = request.Id, Memo = Remark, UpdateDate = DateTime.Now, UpdateUser = user.User.TrueName, TaxMoney = TaxMoney, Money = Money, PayMode = PayMode, RPStatus = RPStatus, IsVAT = IsVAT, Type = Type, InvoiceNo = InvoiceNo, InvoiceNum= InvoiceNum, IsUse= IsUse, SFType= SFType, BasicDataId= BasicDataId, BadMoney= BadMoney, IsTaxYZ= IsTaxYZ, InvoiceDate= InvoiceDate }, ts);

                    cnn.Execute(@"delete from TKS_FAS_InvoiceAttach where InvoiceId=@InvoiceId", new { InvoiceId = request.Id }, ts);
                    if (lstAttach.Count > 0)
                    {
                        foreach (var item in lstAttach)
                        {
                            TKS_FAS_InvoiceAttach Attach = new TKS_FAS_InvoiceAttach();
                            Attach.Id = Guid.NewGuid().ToString("N");
                            Attach.AccountId = request.AccountId;
                            Attach.CreateUser = user.User.TrueName;
                            Attach.CreateDate = DateTime.Now;
                            Attach.Title = item.Title;
                            Attach.Path = item.Path;
                            Attach.InvoiceId = request.Id;
                            cnn.Insert<TKS_FAS_InvoiceAttach>(Attach, ts);
                        }
                    }
                    if (IsUse == "1")
                    {
                        cnn.Execute(@"delete from TKS_FAS_InvoiceSFDetail where InvoiceId=@InvoiceId", new { InvoiceId = request.Id }, ts);
                        if (lstSFDetail.Count > 0)
                        {
                            foreach (var item in lstSFDetail)
                            {
                                TKS_FAS_InvoiceSFDetail SF = new TKS_FAS_InvoiceSFDetail();
                                SF.Id = Guid.NewGuid().ToString("N");
                                SF.InvoiceId = request.Id;
                                SF.SFDate = item.SFDate;
                                SF.SFMoney = Convert.ToDecimal(item.SFMoney);
                                SF.SFRemark = item.SFRemark;
                                SF.SFStatus = item.SFStatus;
                                SF.Seq = int.Parse(item.Seq);
                                SF.Status = "OP";
                                SF.CreateUser = user.User.TrueName;
                                SF.CreateDate = DateTime.Now;
                                cnn.Insert<TKS_FAS_InvoiceSFDetail>(SF, ts);
                            }
                        }
                    }

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

        public ResponseInvoiceDelete WX_InvoiceDelete(RequestInvoiceDelete request)
        {
            ResponseInvoiceDelete response = new ResponseInvoiceDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("delete from TKS_FAS_Invoice where id=@Id", new { Id = request.Id }, ts);
                    cnn.Execute(@"delete from TKS_FAS_InvoiceAttach where InvoiceId=@InvoiceId", new { InvoiceId = request.Id }, ts);


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceDelete;
                }
            }
        }

        public ResponseInvoiceDelete WX_SFInvoiceDelete(RequestInvoiceDelete request)
        {
            ResponseInvoiceDelete response = new ResponseInvoiceDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("update TKS_FAS_Invoice set IsUse='0' where id=@Id", new { Id = request.Id }, ts);
                    cnn.Execute(@"delete from TKS_FAS_InvoiceSFDetail where InvoiceId=@InvoiceId", new { InvoiceId = request.Id }, ts);


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceDelete;
                }
            }
        }

        public ResponseInvoiceAdd WX_SFWarnDelete(RequestInvoiceAdd request)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            List<string> lstSFDetail = Serializer.Deserialize<List<string>>(request.Ids);
            ResponseInvoiceAdd response = new ResponseInvoiceAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string ids = "('" + string.Join("','", lstSFDetail) + "')";
                    string update_Invoice = @" 
                            UPDATE  TKS_FAS_InvoiceSFDetail
                               SET
                                  Status ='DE'
                                 
                             where id in"+ ids;
                    cnn.Execute(update_Invoice, null, ts);
                    ts.Commit();

                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseInvoiceAdd;
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
