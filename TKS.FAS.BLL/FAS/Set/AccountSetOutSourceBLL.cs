using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Common;


namespace TKS.FAS.BLL.FAS
{
    public partial class AccountSetBLL
    {
        public ResponseAccountOutSource AccountOutSource(RequestAccountOutSource request)
        {
            ResponseAccountOutSource res = new ResponseAccountOutSource();
            using (cnn = GetConnection())
            {
                try
                {
                    string sql = @"update tks_fas_accountInfo set 
                        AccountantId='',
                        wb_status=2, 
                        isOutSource=1 ,
                        WB_CreditCode=@WB_CreditCode
                        where id=@Id and isOutSource=0";

                    cnn.Execute(sql, request.Data);

                    res.IsSuccess = true;
                    res.Message = "发起外包成功";
                    return res;
                }
                catch (Exception ex)
                {
                    return this.DealException(res, ex) as ResponseAccountOutSource;
                }

            }

        }


        public ResponseAccountUnOut AccountUnOut(RequestAccountUnOut request)
        {
            ResponseAccountUnOut res = new ResponseAccountUnOut();
            using (cnn = GetConnection())
            {
                try
                {
                    string sql = @"update tks_fas_accountInfo set 
                             AccountantId='',
                            wb_status=5, 
                            isOutSource=0 ,
                            WB_CreditCode=''
                        where id=@Id and isOutSource=1";
                    cnn.Execute(sql, request.Data);

                    res.IsSuccess = true;
                    res.Message = "撤销成功";
                    return res;
                }
                catch (Exception ex)
                {
                    return this.DealException(res, ex) as ResponseAccountUnOut;
                }
            }
        }

        public ResponseAccountAcceptOut AccountAcceptOut(RequestAccountAcceptOut request)
        {
            ResponseAccountAcceptOut res = new ResponseAccountAcceptOut();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    //接受外包，外包机构设为接收者的结构
                    string sql = @"update tks_fas_accountInfo set wb_status=4 , isOutSource=1  where
                        id=@Id and wb_status=2";

                    cnn.Execute(sql, new {   Id = request.Data.Id }, ts);
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "接收成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseAccountAcceptOut;
                }
            }
        }

        public ResponseAccountRejectOut AccountRejectOut(RequestAccountRejectOut request)
        {
            ResponseAccountRejectOut res = new ResponseAccountRejectOut();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"update tks_fas_accountinfo set wb_status=3, isOutSource=0  where id=@Id and wb_status=2";
                    cnn.Execute(sql, new { Id = request.Data.Id }, ts);
                    ts.Commit();
                    res.IsSuccess = true;
                    res.Message = "拒绝成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseAccountRejectOut;
                }
            }
        }
    }
}
