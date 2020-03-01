using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Common;


namespace TKS.FAS.BLL
{
    public class ICRegisterBLL : CommonBase
    {
        public ResponseICRegisterListSearch  Search(RequestICRegisterListSearch request)
        {

            ResponseICRegisterListSearch response = new ResponseICRegisterListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    string sql = this.GetPageSql("*",
                        @" TKS_FAS_ICRegisterOrder  where  VendorCode=@VendorCode",
                        " create_Date  desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_ICRegisterOrder> data = cnn.Query<TKS_FAS_ICRegisterOrder>(sql,
                        new
                        {
                            VendorCode = user.Node.CreditCode
                         
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_ICRegisterOrder  where 
                    VendorCode=@VendorCode";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        VendorCode = user.Node.CreditCode
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

                    return this.DealException(response, ex) as ResponseICRegisterListSearch;
                }
            }
        }

        public ResponseICRegisterGetByUser GetByUser(RequestICRegisterGetByUser request)
        {
            var res = new ResponseICRegisterGetByUser();
            using (this.cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = @"select * from TKS_FAS_ICRegisterOrder 
                                where create_user=@create_user order by create_date desc";

                    var data = cnn.Query<TKS_FAS_ICRegisterOrder>(sql, 
                        new { create_user=user.User.UserName }, ts).ToList();

                    ts.Commit();
                    res.Data = data;
                    res.IsSuccess = true;
                    res.Message = "";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseICRegisterGetByUser;
                }
            }
        }

        public ResponseICRegisterAdd ICRegisterAdd(RequestICRegisterAdd request)
        {
            var res = new ResponseICRegisterAdd();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    //string sql = @"select * from TKS_FAS_ICRegisterOrder";
                    var user = this.UserInfoGet(request.Token, ts);
                    request.Data.OrderNo = "IC" + user.User.UserName + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    request.Data.Status = "0";
                    request.Data.Create_User = user.User.UserName;
                    request.Data.Create_Date = DateTime.Now;

                    cnn.Insert<TKS_FAS_ICRegisterOrder>(request.Data, ts);

                    ts.Commit();

                    res.IsSuccess = true;
                    res.Message = "新增成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseICRegisterAdd;
                }
            }
        }

        public ResponseICRegisterPrePay ICRegisterPrePay(RequestICRegisterPrePay request)
        {
            var res = new ResponseICRegisterPrePay();

            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"update TKS_FAS_ICRegisterOrder set status='2' , Pre_Pay_Money=@Pre_Pay_Money 
                                where orderNo=@OrderNo and status='1' ";
                    cnn.Execute(sql, new
                    {
                        Pre_Pay_Money = request.Money,
                        OrderNo = request.OrderNo
                    }, ts);
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "预支付成功";
                    return res;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseICRegisterPrePay;
                }
            }
        }


        public ResponseICRegisterRegFinish ICRegisterRegFinish(RequestICRegisterRegFinish request)
        {
            var res = new ResponseICRegisterRegFinish();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"update TKS_FAS_ICRegisterOrder set Status='4' where orderNo=@orderNo and status='3' ";
                    cnn.Execute(sql, new { orderNo =request.OrderNo },ts);
                    res.IsSuccess = true;
                    res.Message = "工商注册完成";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseICRegisterRegFinish;
                }
            }
        }

        public ResponseICRegisterAccept ICRegisterAccept(RequestICRegisterAccept request)
        {
            var res = new ResponseICRegisterAccept();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"update   TKS_FAS_ICRegisterOrder set status='3' where
                                    orderNo=@orderNo and status='2' ";
                    cnn.Execute(sql, new {orderNo=request.OrderNo }, ts);
                    res.IsSuccess = true;
                    res.Message = "已受理";
                    return res;
                }
                catch(Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseICRegisterAccept;
                }
            }
        }


        public ResponseICRegisterPayAll ICRegisterPayAll(RequestICRegisterPayAll request)
        {
            var res = new ResponseICRegisterPayAll();
            using(cnn=this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"update TKS_FAS_ICRegisterOrder set Pay_Money_Date=@Pay_Money_Date,
                                status='5',Pay_Money=@Pay_Money where orderNo=@orderNo and status='4'  ";

                    cnn.Execute(sql, new { Pay_Money_Date=DateTime.Now,
                        Pay_Money=request.Money,
                        orderNo=request.OrderNo
                    }, ts);
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "付款完成";
                    return res;
                }
                catch(Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseICRegisterPayAll;
                }
            }
        }


        public ResponseICRegisterEnd ICRegisterEnd(RequestICRegisterEnd request)
        {
            var res = new ResponseICRegisterEnd();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"update TKS_FAS_ICRegisterOrder set status='6'  
                        where status='5' and orderNo=@orderNo ";
                    cnn.Execute(sql,new { orderNo=request.OrderNo },ts);

                    ts.Commit();

                    res.IsSuccess = true;
                    res.Message = "已结案";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseICRegisterEnd;
                }
            }
        }
    }
}
