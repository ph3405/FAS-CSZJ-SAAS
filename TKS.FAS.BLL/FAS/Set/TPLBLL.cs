using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Common;
using TKS.FAS.BLL.FAS;

namespace TKS.FAS.BLL.FAS
{
    /// <summary>
    /// 用户设置快速凭证
    /// </summary>
    public class TPLBLL : CommonBase
    {
        public ResponseTPLListSearch TPLListSearch(RequestTPLListSearch request)
        {

            ResponseTPLListSearch response = new ResponseTPLListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string where = string.Empty;

                    string sql = this.GetPageSql("A.* ",
                        @"  TKS_FAS_DocTPL A
                        where A.accountId=@AccountId  and type=@Type and tplTarget=0 and isCustom=1",//用户自定义
                        " A.createDate  desc,A.updateDate desc ",
                        request.PageSize,
                        request.PageIndex);
                    if (request.Type == "-1")
                    {
                        sql = this.GetPageSql("A.* ",
                        @"  TKS_FAS_DocTPL A
                        where A.accountId=@AccountId and tplTarget=0 and isCustom=1",//用户自定义
                        " A.createDate  desc,A.updateDate desc ",
                        request.PageSize,
                        request.PageIndex);
                    }
                    List<TKS_FAS_DocTPL> data = cnn.Query<TKS_FAS_DocTPL>(sql,
                        new
                        {
                            AccountId = user.AccountId,
                            Type = request.Type
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_DocTPL 
                    where accountId=@AccountId  and type=@Type and tplTarget=0 and isCustom=1";
                    if (request.Type == "-1")
                    {
                        countSql = @"select count(1) from TKS_FAS_DocTPL 
                    where accountId=@AccountId and tplTarget=0 and isCustom=1";
                    }
                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = user.AccountId,
                        Type = request.Type
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
                    return this.DealException(response, ex) as ResponseTPLListSearch;
                }
            }
        }

        public ResponseTPLGet TPLGet(RequestTPLGet request)
        {
            ResponseTPLGet response = new ResponseTPLGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    var head = cnn.QuerySingle<TKS_FAS_DocTPL>("select * from TKS_FAS_DocTPL where id=@Id", new
                    {
                        Id = request.Data.Id
                    }, ts);

                    var detail = cnn.Query<TKS_FAS_DocDetailTPL>(
                        @"select * from tks_fas_docDetailTPL where parentId=@ParentId",
                        new { ParentId = head.Id }, ts).ToList();

                    for (int i = 0; i < detail.Count; i++)
                    {
                        var item = detail[i];

                        if (!string.IsNullOrEmpty(item.CalItem1))
                        {
                            detail[i].CalValue1 += item.CalItem1 + "," + item.CalValue1 + "#";
                        }

                        if (!string.IsNullOrEmpty(item.CalItem2))
                        {
                            detail[i].CalValue1 += item.CalItem2 + ',' + item.CalValue2 + "#";
                        }

                        if (!string.IsNullOrEmpty(item.CalItem3))
                        {
                            detail[i].CalValue1 += item.CalItem3 + "," + item.CalValue3 + "#";
                        }
                        if (!string.IsNullOrEmpty(item.CalItem4))
                        {
                            detail[i].CalValue1 += item.CalItem4 + "," + item.CalValue4 + "#";
                        }
                        if (!string.IsNullOrEmpty(item.CalItem5))
                        {
                            detail[i].CalValue1 += item.CalItem5 + "," + item.CalValue5 + "#";
                        }

                    
                    }
                 
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Head = head;
                    response.Detail = detail;
                    #region add by Hero.Zhang 判断科目行是否有子科目
                    int flag = 0;
                    for (int i = 0; i < detail.Count(); i++)
                    {
                        var item = detail[i];
                        List<TKS_FAS_AccountSubject> sub = cnn.Query<TKS_FAS_AccountSubject>(@"select * from TKS_FAS_AccountSubject where AccountId=@AccountId and RootCode=@RootCode",
                        new { AccountId = user.AccountId, RootCode = item.SubjectCode }, ts).ToList();
                        if (sub.Count > 1)
                        {
                            flag++;
                            response.CheckParent = "1";
                            response.Message += "</br>第" + (i + 1).ToString() + "行有子科目，请重新选择";
                        }
                    }
                    //if (flag>0)
                    //{
                    //    throw new NormalException("固定资产原值与期初固定资产不匹配</br>固定资产原值：" + FixedAssets.InitialAssetValue.ToString() + "</br>期初固定资产：" + val.ToString());
                    //}
                   
                    #endregion
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseTPLGet;
                }
            }
        }

     

        public ResponseTPLAdd TPLAdd(RequestTPLAdd request)
        {
            ResponseTPLAdd response = new ResponseTPLAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    request.Head.Id = Guid.NewGuid().ToString("N");
                    Add(request, ts, user);

                    ts.Commit();
                    response.Id = request.Head.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseTPLAdd;
                }
            }
        }

        private string Add(RequestTPLAdd request, System.Data.IDbTransaction ts, MM_UserInfo user)
        {
            AccountSetBLL ac = new AccountSetBLL(cnn);
            var period = ac.GetActivePeriod(user.AccountId, ts);




            decimal total = 0;
            var flag = 0;
            //凭证上辅助核算项直接写死对应列
            foreach (var item in request.Detail)
            {

                if (string.IsNullOrEmpty(item.SubjectCode))
                    continue;

                item.Id = Guid.NewGuid().ToString("N");
                #region 辅助核算赋值
                string[] cals = item.CalValue1.Split(new char[] { '#' });
                for (int i = 0; i < cals.Length; i++)
                {
                    if (string.IsNullOrEmpty(cals[i])) continue;
                    string[] ss = cals[i].Split(new char[] { ',' });
                    if (i == 0)
                    {
                        item.CalItem1 = ss[0];
                        item.CalValue1 = ss[1];
                    }
                    else if (i == 1)
                    {
                        item.CalItem2 = ss[0];
                        item.CalValue2 = ss[1];
                    }
                    else if (i == 2)
                    {
                        item.CalItem3 = ss[0];
                        item.CalValue3 = ss[1];
                    }
                    else if (i == 3)
                    {
                        item.CalItem4 = ss[0];
                        item.CalValue4 = ss[1];
                    }
                    else if (i == 4)
                    {
                        item.CalItem5 = ss[0];
                        item.CalValue5 = ss[1];
                    }

                }
                #endregion

                total += item.Money_Debit;

                item.Year = period.Year;
                item.ParentId = request.Head.Id;
                item.AccountId = user.AccountId;
                cnn.Insert<TKS_FAS_DocDetailTPL>(item, ts);
                flag++;
            }
            if (flag == 0)
                throw new NormalException("亲，您还没有填写明细");



            request.Head.AccountId = user.AccountId;
            request.Head.AMT_DBT = total;
            request.Head.IsCarry = 0;
            request.Head.TPLTarget = 0;//用户选择
            request.Head.IsCustom = 1;//自定义
            request.Head.CreateUser = user.User.TrueName;
            request.Head.CreateDate = DateTime.Now;
            cnn.Insert<TKS_FAS_DocTPL>(request.Head, ts);

            return request.Head.Id;
        }

        public ResponseTPLUpdate TPLUpdate(RequestTPLUpdate request)
        {
            ResponseTPLUpdate response = new ResponseTPLUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    string sql = @" delete from TKS_FAS_DocTPL where id=@Id";

                    cnn.Execute(sql, request.Head, ts);

                    sql = "delete from TKS_FAS_DocDetailTPL where parentId=@ParentId";
                    cnn.Execute(sql, new { ParentId = request.Head.Id }, ts);


                    request.Head.Id = request.Head.Id;
                    var id = Add(new RequestTPLAdd
                    {
                        Token = request.Token,
                        Head = request.Head,
                        Detail = request.Detail
                    }, ts, user);
                    ts.Commit();

                    response.IsSuccess = true;
                    response.Id = id;
                    response.Message = "更新成功";
                    return response;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseTPLUpdate;
                }

            }
        }

        public ResponseTPLDelete TPLDelete(RequestTPLDelete request)
        {
            ResponseTPLDelete response = new ResponseTPLDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    string sql = @" delete from TKS_FAS_DocTPL where id=@Id";

                    cnn.Execute(sql, request.Data, ts);

                    sql = "delete from TKS_FAS_DocDetailTPL where parentId=@ParentId";
                    cnn.Execute(sql, new { ParentId = request.Data.Id }, ts);


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseTPLDelete;
                }
            }
        }

        /// <summary>
        /// 发票模板获取
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseFPtplListSearch FPtplListSearch(RequestFPtplListSearch request)
        {

            ResponseFPtplListSearch response = new ResponseFPtplListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = this.GetPageSql("A.*,B.title",
                        @"   TKS_FAS_Invoice2TPL A left join  TKS_FAS_DocTPL B on 
                        A.tplId=B.id where A.accountId=@AccountId",
                        " A.createDate desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Invoice2TPLExt> data = cnn.Query<TKS_FAS_Invoice2TPLExt>(sql,
                        new
                        {
                            AccountId=user.AccountId
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_Invoice2TPL  where AccountId=@AccountId";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId=user.AccountId
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

                    return this.DealException(response, ex) as ResponseFPtplListSearch;
                }
            }
        }
    }
}
