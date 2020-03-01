using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.SSO;
using TKS.FAS.Common;

namespace TKS.FAS.BLL 
{
   public class NodeAccountAccountBLL:CommonBase
    {
        /// <summary>
        /// 流水明细
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseNodeAccountDetailSearch NodeAccountListSearch(RequestNodeAccountDetailSearch request)
        {

            ResponseNodeAccountDetailSearch response = new ResponseNodeAccountDetailSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    string sql = this.GetPageSql("A.*",
                        "TKS_FAS_Node_AccountDetailList A where A.nodeId like @NodeId",
                        "createDate desc",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Node_AccountDetailList> data = cnn.Query<TKS_FAS_Node_AccountDetailList>(sql, 
                        new { NodeId=user.Node.CreditCode }, ts).ToList();

                    string countSql = "select count(1) from TKS_FAS_Node_AccountDetailList where A.nodeId like @NodeId";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new { NodeId = user.Node.CreditCode }, ts).ToString());

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

                    return this.DealException(response, ex) as ResponseNodeAccountDetailSearch;
                }
            }
        }
 
        /// <summary>
        /// 账户信息获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseNodeAccountGet NodeAccountGet(RequestNodeAccountGet request)
        {
            ResponseNodeAccountGet response = new ResponseNodeAccountGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    var data = cnn.QuerySingle<TKS_FAS_Node_Account>("select * from TKS_FAS_Node_Account where NodeId=@NodeId", new
                    {
                         NodeId=user.Node.CreditCode
                    });
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseNodeAccountGet;
                }
            }
        }

        public ResponseNodeAccountAdd NodeAccountAdd(RequestNodeAccountAdd request)
        {
            ResponseNodeAccountAdd response = new ResponseNodeAccountAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                  

                    var  data = cnn.Query("select * from tks_fas_NodeAccount where CreditCode=@CreditCode",
                        new { CreditCode =user.Node.CreditCode }, ts);

                    if (data.Count() > 0)
                    {
                        throw new NormalException("信用代码已经存在");
                    }
 
                    ts.Commit();
                
                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseNodeAccountAdd;
                }
            }
        }
 
    }
}
