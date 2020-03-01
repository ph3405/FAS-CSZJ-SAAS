using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TKS.FAS.Common;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;

namespace TKS.FAS.BLL
{
    public class MyAccountBLL : Common.CommonBase
    {

        public ResponseMyAccountListSearch MyAccountListSearch(RequestMyAccountListSearch request)
        {

            ResponseMyAccountListSearch response = new ResponseMyAccountListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    string sql = this.GetPageSql("*",
                        @"  TKS_FAS_MyAccount where nodeId=@NodeId",
                        " createDate desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_MyAccount> data = cnn.Query<TKS_FAS_MyAccount>(sql,
                        new
                        {
                            NodeId = user.Node.CreditCode
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_MyAccount where nodeId=@NodeId ";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        NodeId = user.Node.CreditCode
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

                    return this.DealException(response, ex) as ResponseMyAccountListSearch;
                }
            }
        }

        public ResponseMyAccountGet MyAccountGet(RequestMyAccountGet request)
        {
            ResponseMyAccountGet response = new ResponseMyAccountGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var data = cnn.QuerySingle<TKS_FAS_MyAccount>("select * from TKS_FAS_MyAccount where id=@Id", new
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
                    return this.DealException(response, ex) as ResponseMyAccountGet;
                }
            }
        }

        public ResponseMyAccountAdd MyAccountAdd(RequestMyAccountAdd request)
        {
            ResponseMyAccountAdd response = new ResponseMyAccountAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    //根据邀请码获取账套信息
                    string sql = @"select * from TKS_FAS_AccountInfo where InvitationCode=@Code and IsInvitation=0";

                    var account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(sql, new { Code = request.InvitationCode }, ts);
                    if (account == null)
                    {
                        throw new NormalException("邀请码已经失效");
                    }


                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    var d = cnn.Query("select * from TKS_FAS_MyAccount where AccountId=@AccountId and NodeId=@NodeId",
                        new { AccountId = account.Id, NodeId = user.Node.CreditCode }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("已关联账套");
                    }

                    TKS_FAS_MyAccount myAccount = new TKS_FAS_MyAccount();
                    myAccount.Id = Guid.NewGuid().ToString("N");
                    myAccount.AccountId = account.Id;
                    myAccount.AccountName = account.QY_Name;
                    myAccount.NodeId = user.Node.CreditCode;
                    myAccount.CreateUser = user.User.TrueName;
                    myAccount.CreateDate = DateTime.Now;
                    cnn.Insert<TKS_FAS_MyAccount>(myAccount, ts);

                    cnn.Execute(@"update TKS_FAS_AccountInfo set IsInvitation=1 ,
                                    InvitationQYCode=@InvitationQYCode,
                                    InvitationQYName=@InvitationQYName
                                where id=@Id",
                                new
                                {
                                    InvitationQYCode = user.Node.CreditCode,
                                    InvitationQYName = user.Node.Name,
                                    Id = account.Id
                                }, ts);

                    ts.Commit();

                    response.IsSuccess = true;
                    response.Message = "关联成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseMyAccountAdd;
                }
            }
        }

        public ResponseMyAccountDelete MyAccountDelete(RequestMyAccountDelete request)
        {
            ResponseMyAccountDelete response = new ResponseMyAccountDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = "select * from TKS_FAS_MyAccount where id=@Id";
                    var myAccount = cnn.QueryFirstOrDefault<TKS_FAS_MyAccount>(sql, new { Id = request.Data.Id }, ts);



                    sql = @"select * from TKS_FAS_AccountInfo where id=@AccountId";

                    var account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(sql, new { AccountId = myAccount.AccountId }, ts);
                    if (account == null)
                    {
                        throw new NormalException("账套不存在");
                    }


                    cnn.Execute("delete from TKS_FAS_MyAccount where id=@Id", request.Data, ts);

                    cnn.Execute(@"update TKS_FAS_AccountInfo set IsInvitation=0 ,
                                   InvitationQYCode='',
                                    InvitationQYName=''
                                where id=@Id",
                             new
                             {
                                 Id = account.Id
                             }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseMyAccountDelete;
                }
            }
        }

        public ResponseMyAccountDelete DelMyAccount(RequestMyAccountDelete request)
        {
            ResponseMyAccountDelete response = new ResponseMyAccountDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    //string sql = "select * from TKS_FAS_MyAccount where id=@Id";
                    //var myAccount = cnn.QueryFirstOrDefault<TKS_FAS_MyAccount>(sql, new { Id = request.Data.Id }, ts);



                    string sql = @"select * from TKS_FAS_AccountInfo where id=@AccountId";

                    var account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(sql, new { AccountId = request.Data.Id }, ts);
                    if (account == null)
                    {
                        throw new NormalException("账套不存在");
                    }


                    cnn.Execute("delete from TKS_FAS_MyAccount where AccountId=@AccountId and NodeId=@NodeId", new
                    {
                        AccountId = account.Id,
                        NodeId= user.Node.CreditCode
                    }, ts);

                    cnn.Execute(@"update TKS_FAS_AccountInfo set IsInvitation=0 ,
                                   InvitationQYCode='',
                                    InvitationQYName=''
                                where id=@Id",
                             new
                             {
                                 Id = account.Id
                             }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "取消成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseMyAccountDelete;
                }
            }
        }
    }
}
