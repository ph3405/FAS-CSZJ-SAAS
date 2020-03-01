using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TKS.FAS.Entity.FAS;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Common;
using System.Data;

namespace TKS.FAS.BLL.FAS 
{
    public class KSPZBLL:CommonBase
    {
        /// <summary>
        /// 获取快速凭证模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseKSPZTPLsGet KSPZTPLsGet(RequestKSPZTPLsGet request)
        {
            var res = new ResponseKSPZTPLsGet();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    AccountSetBLL accountBll = new AccountSetBLL(cnn);
                    var period = accountBll.GetActivePeriod(user.AccountId, ts);

                    //获取期末检查模板  是否已经生成凭证
                    //update by Hero.Zhang 按照原始模板查询
                    string sql = @"select A.*,0 as IsGenPZ
                          
                            from TKS_FAS_DocTPL A 
                                               
                            where A.tplTarget=1 and A.isCustom=0   and a.AccountId='-'   ";

                    var data = cnn.Query<TKS_FAS_DocTPLExt>(sql, new
                    {
                        AccountId = user.AccountId,//账套
                        PeriodId = period.Id//当前  期间
                    }, ts).ToList();

                    res.IsSuccess = true;
                    res.Data = data;
                    res.Message = "";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseKSPZTPLsGet;
                }
            }
        }
    }
}
