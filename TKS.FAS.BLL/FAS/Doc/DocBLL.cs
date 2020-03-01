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

namespace TKS.FAS.BLL.FAS
{
    public class DocBLL : CommonBase
    {
        public ResponseDocListSearch DocListSearch(RequestDocListSearch request)
        {

            ResponseDocListSearch response = new ResponseDocListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string where = string.Empty;
                    object predicate;
                    object accountList;
                    if (request.Type == "normal")
                    {
                        #region 一般查询
                        if (!string.IsNullOrEmpty(request.PZZ))
                        {
                            where = " and A.pzz=@PZZ";
                        }
                        if (!string.IsNullOrEmpty(request.PZZ_S))
                        {
                            where += " and pzzNo>=" + int.Parse(request.PZZ_S);
                        }

                        if (!string.IsNullOrEmpty(request.PZZ_E))
                        {
                            where += " and pzzNo<=" + int.Parse(request.PZZ_E);
                        }

                        if (!string.IsNullOrEmpty(request.Period) && request.Period != "##")
                        {
                            where += " and A.periodId='" + request.Period + "'";
                        }
                        predicate = new
                        {
                            AccountId = user.AccountId,
                            PZZ = request.PZZ
                        };
                        #endregion
                    }
                    else
                    {
                        #region 更多查询
                        if (!string.IsNullOrEmpty(request.More.PZZ))
                        {
                            where = " and A.pzz=@PZZ";
                        }
                        if (!string.IsNullOrEmpty(request.More.PZZ_S))
                        {
                            where += " and pzzNo>=" + int.Parse(request.More.PZZ_S);
                        }

                        if (!string.IsNullOrEmpty(request.More.PZZ_E))
                        {
                            where += " and pzzNo<=" + int.Parse(request.More.PZZ_E);
                        }

                        if (!string.IsNullOrEmpty(request.More.Period_S))
                        {
                            var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                         new { Id = request.More.Period_S }, ts);
                            where += " and A.pzdate>='" + DateTime.Parse(periodS.StartDate.ToString()).ToString("yyyy-MM-dd") + "'";
                        }

                        if (!string.IsNullOrEmpty(request.More.Period_E))
                        {
                            var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                         new { Id = request.More.Period_E }, ts);
                            where += " and A.pzdate<='" + DateTime.Parse(periodE.EndDate.ToString()).ToString("yyyy-MM-dd") + "'";
                        }
                        if (!string.IsNullOrEmpty(request.AccountList))
                        {
                            where += " and  A.accountId in" + request.AccountList;
                        }


                        predicate = new
                        {
                            AccountId = user.AccountId,
                            PZZ = request.More.PZZ
                        };

                        #endregion
                    }


                    string sql = this.GetPageSql("A.*,B.pzz as PZZName," +
                        "AttachmentCount=(select count(att.Id) from TKS_FAS_Attachment att where att.DocId=A.Id ) ",
                        @"  TKS_FAS_Doc A left join TKS_FAS_CertificateWord B on A.pzz=B.ID
                        where A.accountId=@AccountId " + where,
                        " A.PZDate,A.PeriodId,B.PZZ,A.PZZNO",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_DocExt> data = cnn.Query<TKS_FAS_DocExt>(sql, predicate, ts).ToList();
                    if (!string.IsNullOrEmpty(request.AccountList))
                    {
                        accountList = new
                        {
                            AccountList = request.AccountList,
                            PZZ = request.More.PZZ
                        };
                        //打印工具专用
                        sql = this.GetPageSql(" row_number()  OVER  ( ORDER  BY  A.PZZNO  ASC )  AS  RowNum,A.*,B.pzz as PZZName," +
                        "AttachmentCount=(select count(att.Id) from TKS_FAS_Attachment att where att.DocId=A.Id ) ",
                        @"  TKS_FAS_Doc A left join TKS_FAS_CertificateWord B on A.pzz=B.ID
                        where 1=1 " + where,
                        " A.pzDate  desc,A.pzzNo  ",
                        request.PageSize,
                        request.PageIndex);
                        data = cnn.Query<TKS_FAS_DocExt>(sql, accountList, ts).ToList();
                    }
                    string countSql = @"select count(1) from TKS_FAS_Doc A 
                    where A.accountId=@AccountId  " + where;

                    int total = int.Parse(cnn.ExecuteScalar(countSql, predicate, ts).ToString());


                    List<string> docIds = data.Select(p => p.Id).ToList();

                    string ids = "('" + string.Join("','", docIds) + "')";

                    sql = @"select * from TKS_FAS_DocDetail where parentId in " + ids;

                    var detail = cnn.Query<TKS_FAS_DocDetail>(sql, null, ts).ToList();

                    var result = (from item in data
                                  select new DocList
                                  {
                                      Head = item,
                                      Detail = detail.Where(p => p.ParentId == item.Id).OrderBy(p => p.Seq).ToList()
                                  }).ToList();

                    AccountSetBLL account = new AccountSetBLL(cnn);
                    var currentAccount = account.GetAccountSetByAccountId(user.AccountId, ts);
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    response.Data = result;
                    response.Total = total;
                    response.AccountName = currentAccount.QY_Name;

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseDocListSearch;
                }
            }
        }

        public ResponseDocListSearch GetDocByAccount(RequestDocListSearch request)
        {

            ResponseDocListSearch response = new ResponseDocListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    List<TKS_FAS_Doc> data = cnn.Query<TKS_FAS_Doc>(@"select * from   TKS_FAS_Doc
        where AccountId=@AccountId
        order by PZDate,PeriodId,PZZ,PZZNO",
                       new
                       {
                           AccountId = user.AccountId
                       }, ts).ToList();
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
  
                    response.lstAll = data;


                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseDocListSearch;
                }
            }
        }

        public ResponseDocListAttachment DocAttachment(RequestDocListSearch request)
        {

            ResponseDocListAttachment response = new ResponseDocListAttachment();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string where = string.Empty;

                    if (!string.IsNullOrEmpty(request.PZZ_S))
                    {
                        where += " and pzzNo>=" + int.Parse(request.PZZ_S);
                    }

                    if (!string.IsNullOrEmpty(request.PZZ_E))
                    {
                        where += " and pzzNo<=" + int.Parse(request.PZZ_E);
                    }


                    var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                 @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                 new { Id = request.More.Period_S }, ts);
                    where += " and A.pzdate>='" + DateTime.Parse(periodS.StartDate.ToString()).ToString("yyyy-MM-dd") + "'";


                    var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                 @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                 new { Id = request.More.Period_E }, ts);
                    where += " and A.pzdate<='" + DateTime.Parse(periodE.EndDate.ToString()).ToString("yyyy-MM-dd") + "'";

                    if (periodS == null)
                    {
                        return new ResponseDocListAttachment { IsSuccess = false, Message = "会计期间没有选择" };
                    }
                    if (periodS.EndDate > periodE.EndDate)
                    {
                        throw new NormalException("开始期间不能大于结束期间");
                    }
                    //获取期间集合
                    List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE ",
                         new
                         {
                             StartDateS = periodS.StartDate.ToString(),
                             StartDateE = periodE.StartDate.ToString(),
                             AccountId = user.AccountId
                         }, ts).ToList();
                    string periodWhere = string.Empty;//期间条件
                    int j = 0;
                    foreach (var period in periods)
                    {
                        if (j > 0)
                        {
                            periodWhere += " or ";
                        }
                        periodWhere += "  periodId='" + period.Id + "'";
                        j++;
                    }
                    periodWhere = "  (" + periodWhere + ")";
                    string getdoc = string.Format(@"select * from TKS_FAS_Doc where AccountId=@AccountId and {0}", periodWhere);
                    List<TKS_FAS_Doc> doc = cnn.Query<TKS_FAS_Doc>(getdoc, new { AccountId = user.AccountId }, ts).ToList();
                    string docWhere = "";
                    int t = 0;
                    foreach (var d in doc)
                    {
                        if (t > 0)
                        {
                            docWhere += " or ";
                        }
                        docWhere += " DocId='" + d.Id + "'";
                        t++;
                    }
                    docWhere = "  (" + docWhere + ")";
                    string getAttachment = string.Format(@"select * from TKS_FAS_Attachment where AccountId='{0}' and {1}", user.AccountId, docWhere);
                    List<TKS_FAS_Attachment> lstAttachment = cnn.Query<TKS_FAS_Attachment>(getAttachment, null, ts).ToList();
                    List<string> lstUrl = new List<string>();
                    foreach (var item in lstAttachment)
                    {
                        lstUrl.Add(item.Path);
                    }
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.ImgUrl = lstUrl;


                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseDocListAttachment;
                }
            }
        }
   
        public ResponseDocGet DocGet(RequestDocGet request)
        {
            ResponseDocGet response = new ResponseDocGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    var head = cnn.QueryFirstOrDefault<TKS_FAS_Doc>("select * from TKS_FAS_Doc where id=@Id", new
                    {
                        Id = request.Data.Id
                    }, ts);
                    if (head == null)
                    {
                        throw new Exception("凭证已被删除");
                    }
                    var detail = cnn.Query<TKS_FAS_DocDetail>(
                        @"select * from tks_fas_docDetail where parentId=@ParentId order by Seq",
                        new { ParentId = head.Id }, ts).ToList();
                    //TKS_FAS_DocDetail item = new TKS_FAS_DocDetail();
                    for (int i = 0; i < detail.Count; i++)
                    {
                        var item = detail[i];
                        string CalValue1 = item.CalValue1;
                        detail[i].CalValue1 = "";
                        if (!string.IsNullOrEmpty(item.CalItem1))
                        {
                            //detail[i].CalValue1 += item.CalItem1 + "," + item.CalValue1 + "#";
                            detail[i].CalValue1 += item.CalItem1 + "," + CalValue1 + "#";
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
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseDocGet;
                }
            }
        }

        public ResponseDocAdd DocAdd(RequestDocAdd request)
        {
            ResponseDocAdd response = new ResponseDocAdd();
            using (cnn = GetConnection())
            {
                BalanceBLL balBLL = new BalanceBLL(cnn);
                var ts = cnn.BeginTransaction();
                try
                {

                    var user = this.UserInfoGet(request.Token, ts);

                    #region modify by andy 期间根据凭证日期计算
                    //AccountSetBLL account = new FAS.AccountSetBLL(cnn);
                    //var period = account.GetActivePeriod(user.AccountId, ts);
                    var pzDate = request.Head.PZDate;
                    PeriodBLL balanceBLL = new PeriodBLL(cnn);
                    var period = balanceBLL.GetPeriod(user.AccountId, pzDate?.Year ?? 0, pzDate?.Month ?? 0, ts);
                    #endregion

                    request.Head.Id = Guid.NewGuid().ToString("N");
                    request.Head.Source = "LR";
                    Add(request, ts, user);

                    DealByPZType(request, ts, user);

                    #region 新增凭证时，对余额表进行操作 Add by Hero.Zhang

                    balBLL.UpdateBalance(request, ts, user);

                    #endregion
                    ts.Commit();
                    response.Id = request.Head.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseDocAdd;
                }
            }
        }





        /// <summary>
        /// 根据保存的来源类型处理
        ///     例如：如果是从模板生成触发，期末检查模板要关联生成的凭证
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ts"></param>
        /// <param name="user"></param>
        private void DealByPZType(RequestDocAdd request, IDbTransaction ts, MM_UserInfo user)
        {
            if (request.Type == PZType.QM.ToString())//期末
            {
                AccountSetBLL account = new AccountSetBLL(cnn);
                var period = account.GetActivePeriod(user.AccountId, ts);
                TKS_FAS_TPL2PZ newData = new TKS_FAS_TPL2PZ();
                newData.Id = Guid.NewGuid().ToString();
                newData.TPLId = request.TPLId;
                newData.PZId = request.Head.Id;
                newData.AccountId = user.AccountId;
                newData.CreateUser = user.User.UserName;
                newData.CreateDate = DateTime.Now;
                newData.PeriodId = period.Id;
                cnn.Insert<TKS_FAS_TPL2PZ>(newData, ts);
            }
            else if (request.Type == PZType.FP.ToString())
            {
                InvoiceBLL bll = new FAS.InvoiceBLL(cnn);
                bll.LZ(request.Key, request.Head.Id, user, ts);
            }
            else if (request.Type == PZType.CHANGE.ToString())
            {
                PZZBLL pzz = new PZZBLL(cnn);
                var pzzData = pzz.PZZGetById(request.Head.PZZ, ts);
                FixedAssetsBLL fixedAssetsBLL = new FixedAssetsBLL(cnn);
                fixedAssetsBLL.ChangeSetPZ(request.Head.Id, pzzData.PZZ + request.Head.PZZNO, request.Key, ts);
                fixedAssetsBLL.SetIsGenPZ(request.Key, user, ts);
            }
            else if (request.Type == PZType.GD.ToString())
            {
                AccountSetBLL account = new AccountSetBLL(cnn);
                var period = account.GetActivePeriod(user.AccountId, ts);
                TKS_FAS_TPL2PZ newData = new TKS_FAS_TPL2PZ();
                newData.Id = Guid.NewGuid().ToString();
                newData.TPLId = request.TPLId;
                newData.PZId = request.Head.Id;
                newData.AccountId = user.AccountId;
                newData.CreateUser = user.User.UserName;
                newData.CreateDate = DateTime.Now;
                newData.PeriodId = period.Id;
                cnn.Insert<TKS_FAS_TPL2PZ>(newData, ts);
                //更新固定资产
                FixedAssetsBLL fixedAssetsBLL = new FixedAssetsBLL(cnn);
                fixedAssetsBLL.Depreciation(user.AccountId, ts, period);
            }
        }

        private string Add(RequestDocAdd request, System.Data.IDbTransaction ts, MM_UserInfo user)
        {
            AccountSetBLL ac = new AccountSetBLL(cnn);
            #region modify by andy 期间根据凭证日期计算
            //var period = ac.GetActivePeriod(user.AccountId, ts);
            var pzDate = request.Head.PZDate;
            PeriodBLL balanceBLL = new PeriodBLL(cnn);
            var period = balanceBLL.GetPeriod(user.AccountId, pzDate?.Year ?? 0, pzDate?.Month ?? 0, ts);
            #endregion

            

            var d = cnn.Query(@"select * from TKS_FAS_Doc where  accountId=@AccountId 
                                    and periodId=@PeriodId and pzz=@PZZ and pzzno=@PZZNO",
                new { AccountId = user.AccountId, PeriodId = period.Id, PZZ = request.Head.PZZ, PZZNO = request.Head.PZZNO }, ts);
            if (d.Count() > 0)
            {
                throw new NormalException("当前账套，会计期间下，凭证字号已经存在");
            }

            if (request.Head.PZDate.HasValue)
            {
                if (period.StartDate > request.Head.PZDate || period.EndDate < request.Head.PZDate)
                {
                    //可以录入当前期间之后期间的凭证 update by Hero.Zhang 20180507
                    //throw new NormalException("凭证日期必须在当前会计期间");
                }
            }
            else
            {
                throw new NormalException("凭证日期必须选择");
            }
            var accountSet = ac.GetAccountSetByAccountId(user.AccountId, ts);

            if (accountSet.IsNeedReviewed == 1)
            {
                request.Head.CheckStatus = 1;
            }
            else
            {
                request.Head.CheckStatus = 3;
            }
            decimal debit = 0;
            decimal credit = 0;

            decimal total = 0;
            var flag = 0;
            string strMessage = "";
            List<TKS_FAS_AccountSubject> sub = cnn.Query<TKS_FAS_AccountSubject>(@"select * from TKS_FAS_AccountSubject where AccountId=@AccountId",
                       new { AccountId = user.AccountId }, ts).ToList();
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
                //录入在借方，则凭证借贷方向是借，否则是贷
                if (item.Money_Debit != 0)
                    item.Credit_Debit = 0;
                else
                    item.Credit_Debit = 1;
                if (string.IsNullOrEmpty(item.CurrencyCode))
                {
                    item.CurrencyCode = "RMB";//默认RMB
                }
                item.SubjectDescription = item.SubjectDescription.Trim();
                item.PeriodId = period.Id;
                item.Year = period.Year;
                item.Source = request.Head.Source;
                item.ParentId = request.Head.Id;
                item.AccountId = user.AccountId;
                //add by Hero.Zhang 添加期初，期末值 2018-12-18
                //                #region add by Hero.Zhang 添加期初，期末值 2018-12-18
                //                item.FirstMoney = 0;
                //                item.EndBAL = 0;
                //                var det = cnn.Query<TKS_FAS_DocDetail>(@"
                //    select top 1 det.* from TKS_FAS_DocDetail det
                //    left join TKS_FAS_Doc doc on det.ParentId=doc.Id
                //    where det.AccountId=@AccountId
                //    and det.PeriodId=@PeriodId
                //    and det.SubjectCode=@SubjectCode
                //    order by doc.CreateDate  desc",
                //    new { AccountId = user.AccountId, PeriodId = period.Id, SubjectCode = item.SubjectCode}, ts).ToList();
                //                if (det.Count>0)
                //                {
                //                    //此科目在该期间上一次的期末值为这一次的期初值
                //                    item.FirstMoney = det[0].EndBAL;
                //                    if (item.Credit_Debit==det[0].Credit_Debit)
                //                    {
                //                        item.EndBAL = item.FirstMoney + item.Money_Debit + item.Money_Credit;
                //                    }
                //                    else
                //                    {
                //                        item.EndBAL = Math.Abs(item.FirstMoney - (item.Money_Debit + item.Money_Credit));
                //                    }
                //                }
                //                else
                //                {
                //                    //第一次存入，从期初余额中取值
                //                    var bal = cnn.Query<TKS_FAS_GLBalance>(@"
                //    select top 1 det.* from TKS_FAS_GLBalance det
                //    where det.AccountId=@AccountId
                //    and det.PeriodId=''
                //    and det.SubjectCode=@SubjectCode
                //    ",
                //new { AccountId = user.AccountId,  SubjectCode = item.SubjectCode }, ts).ToList();
                //                    if (bal.Count>0)
                //                    {
                //                        item.FirstMoney = bal[0].BWBStartBAL;
                //                        if (item.Credit_Debit == bal[0].SCredit_Debit)
                //                        {
                //                            item.EndBAL = item.FirstMoney + item.Money_Debit + item.Money_Credit;
                //                        }
                //                        else
                //                        {
                //                            item.EndBAL = Math.Abs(item.FirstMoney - (item.Money_Debit + item.Money_Credit));
                //                        }
                //                    }
                //                }
                //                #endregion
                var info = sub.Where(p => p.Code == item.SubjectCode).ToList();
                if (info.Count()==0)
                {
                    throw new NormalException(item.SubjectCode + "科目不存在");
                }
                if (info[0].IsCalHelperValid == 1 && string.IsNullOrEmpty(item.CalValue1))
                {
                    throw new NormalException(item.SubjectCode + "科目中有辅助核算，请选择辅助核算");
                }
                List<TKS_FAS_AccountSubject> checkSub = sub.FindAll(p => p.ParentId == info[0].Id).ToList();
                if (checkSub.Count() > 0)
                {
                   
                    strMessage += "</br>第" + (flag + 1).ToString() + "行有子科目，请重新选择";
                }
                cnn.Insert<TKS_FAS_DocDetail>(item, ts);
                flag++;
            }
            if (flag == 0)
                throw new NormalException("亲，您还没有填写明细");
            if (flag == 1)
            {
                throw new NormalException("亲，明细不能只存在一条");
            }
            if (strMessage!="")
            {
                throw new NormalException(strMessage);
            }
            request.Head.PeriodId = period.Id;
            request.Head.Year = period.Year;
            request.Head.AccountId = user.AccountId;
            request.Head.AMT_DBT = total;
            request.Head.CreateUser = user.User.TrueName;
            request.Head.CreateDate = DateTime.Now;
            //add by Hero.Zhang 存入来源 20180903
            request.Head.Source = request.Type;
            if (string.IsNullOrEmpty(request.Type))
            {
                request.Head.Source = "LR";
            }
            cnn.Insert<TKS_FAS_Doc>(request.Head, ts);

            return request.Head.Id;
        }





        /// <summary>
        /// 辅助核算项余额表新增
        /// </summary>
        /// <param name="currentBalance"></param>
        /// <param name="newDoc"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private void FGLBalanceAdd(TKS_FAS_DocDetail newDoc, List<TKS_FAS_FGLBalance> saveData, IDbTransaction ts)
        {
            TKS_FAS_FGLBalance currentBalance = new TKS_FAS_FGLBalance();


            TKS_FAS_AccountSubject subject = GetSubjectBySubjectCode(newDoc, ts);
            currentBalance.SCredit_Debit = subject.Credit_Debit;

            //年度是累加的，年初是确定后就不变的，本月的是当前月，期末是公式计算

            //按凭证的借贷方向进行操作
            if (newDoc.Credit_Debit == 0)//借方
            {
                currentBalance.NUMDebitTotal += newDoc.Quantity;//本月借方累计数量
                currentBalance.NUMDebitTotal_Y += newDoc.Quantity;//年度借方累计数量
                currentBalance.YBDebitTotal += newDoc.YB;
                currentBalance.YBDebitTotal_Y += newDoc.YB;//年度借方累计原币
                currentBalance.BWBDebitTotal += newDoc.Money_Debit;//本位币
                currentBalance.BWBDebitTotal_Y += newDoc.Money_Debit;//年度借方累计 本位币

            }
            else
            {
                //贷方
                currentBalance.NUMCreditTotal += newDoc.Quantity;
                currentBalance.NUMCreditTotal_Y += newDoc.Quantity;//数量
                currentBalance.YBCreditTotal += newDoc.YB;
                currentBalance.YBCreditTotal_Y += newDoc.YB;
                currentBalance.BWBCreditTotal += newDoc.Money_Credit;
                currentBalance.BWBCreditTotal_Y += newDoc.Money_Credit;

            }

            if (currentBalance.SCredit_Debit == 0)//期初余额方向 借
            {
                //期初+本期借方-本期贷方 >0 借 <0 贷 =0 平
                var val = currentBalance.BWBStartBAL + currentBalance.BWBDebitTotal - currentBalance.BWBCreditTotal;
                int cb = 0;
                if (val > 0)
                    cb = 0;
                else if (val == 0)
                    cb = 0;
                else
                    cb = 1;

                currentBalance.ECredit_Debit = cb;
                currentBalance.BWBEndBAL = Math.Abs(val);
                currentBalance.NUMEndBAL = currentBalance.NUMStartBAL + currentBalance.NUMDebitTotal - currentBalance.NUMCreditTotal;
                currentBalance.YBEndBAL = currentBalance.YBStartBAL + currentBalance.YBDebitTotal - currentBalance.YBCreditTotal;
            }
            else
            {
                //期初+本期贷方-本期借方 >0 贷 <0 借 =0 平
                var val = currentBalance.BWBStartBAL + currentBalance.BWBCreditTotal - currentBalance.BWBDebitTotal;
                int cb = 0;
                if (val > 0)
                    cb = 1;
                else if (val == 0)
                    cb = 1;
                else
                    cb = 0;

                currentBalance.ECredit_Debit = cb;
                currentBalance.BWBEndBAL = Math.Abs(val);
                currentBalance.NUMEndBAL = currentBalance.NUMStartBAL + currentBalance.NUMCreditTotal - currentBalance.NUMDebitTotal;
                currentBalance.YBEndBAL = currentBalance.YBStartBAL + currentBalance.YBCreditTotal - currentBalance.YBDebitTotal;

            }


            currentBalance.Id = Guid.NewGuid().ToString();
            currentBalance.CalItem1 = newDoc.CalItem1;
            currentBalance.CalItem2 = newDoc.CalItem2;
            currentBalance.CalItem3 = newDoc.CalItem3;
            currentBalance.CalItem4 = newDoc.CalItem4;
            currentBalance.CalItem5 = newDoc.CalItem5;
            currentBalance.CalValue1 = newDoc.CalValue1;
            currentBalance.CalValue2 = newDoc.CalValue2;
            currentBalance.CalValue3 = newDoc.CalValue3;
            currentBalance.CalValue4 = newDoc.CalValue4;
            currentBalance.CalValue5 = newDoc.CalValue5;
            currentBalance.SubjectCode = newDoc.SubjectCode;
            currentBalance.AccountId = newDoc.AccountId;
            currentBalance.PeriodId = newDoc.PeriodId;
            currentBalance.Year = newDoc.Year;
            currentBalance.Category = subject.Category;
            currentBalance.ParentId = subject.Id;
            currentBalance.CurrencyCode = newDoc.CurrencyCode;
            currentBalance.Name = newDoc.SubjectDescription.Trim();
            currentBalance.IsFirstPeriodAdd = 0;
            currentBalance.IsDefaultCurrency = newDoc.CurrencyCode == "RMB" ? 1 : 0;


            saveData.Add(currentBalance);


        }

        private TKS_FAS_AccountSubject GetSubjectBySubjectCode(TKS_FAS_DocDetail newDoc, IDbTransaction ts)
        {
            string sql = @"select * from tks_fas_AccountSubject where 
                    code=@Code and accountId=@AccountId ";

            var data = cnn.Query<TKS_FAS_AccountSubject>(sql,
                new
                {
                    Code = newDoc.SubjectCode,
                    AccountId = newDoc.AccountId,

                }, ts).ToList();

            if (data.Count > 0)
            {
                return data[0];
            }
            else
            {
                throw new AppException("", "GetBalBySubjectCode", "期末结转异常",
                    "期末结转：辅助核算项余额表插入异常，账套ID：" + newDoc.AccountId + "，凭证明细ID:" + newDoc.Id + @"
                    科目代码：" + newDoc.SubjectCode + "没有找到科目");
            }
        }
        public ResponseDocUpdate DocUpdate(RequestDocUpdate request)
        {
            ResponseDocUpdate response = new ResponseDocUpdate();

            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                BalanceBLL balBLL = new BalanceBLL(cnn);
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    var doc = cnn.QueryFirst<TKS_FAS_Doc>("select * from TKS_FAS_Doc where id=@Id",
                        new { Id = request.Head.Id }, ts);
                    PeriodBLL period = new PeriodBLL(cnn);
                    bool flag = period.IsPeriodPaid(doc.PeriodId, ts);
                    if (flag)
                    {
                        throw new NormalException("该凭证已经期末结转，无法修改");
                    }

                    flag = period.IsHasPeriod(doc.PeriodId, ts);

                    if (!flag)
                    {
                        throw new NormalException("该凭证期间非激活状态，无法修改");
                    }
                    List<TKS_FAS_DocDetail> Detail = cnn.Query<TKS_FAS_DocDetail>("select *  from TKS_FAS_DocDetail where parentId=@ParentId", new { ParentId = request.Head.Id }, ts).ToList();

                    #region 先回滚余额表 Hero.Zhang 
                    RequestDocAdd oldDoc = new RequestDocAdd();
                    oldDoc.Head = doc;
                    oldDoc.Detail = Detail;
                    foreach(var det in oldDoc.Detail)
                    {
                        det.Money_Debit = -1 * det.Money_Debit;
                        det.Money_Credit =-1* det.Money_Credit;
                        det.Quantity = -1 * det.Quantity;
                    }
                    balBLL.UpdateBalance(oldDoc, ts, user);
                    #endregion

                    string sql = @" delete from TKS_FAS_Doc where id=@Id";

                    cnn.Execute(sql, request.Head, ts);

                    sql = "delete from TKS_FAS_DocDetail where parentId=@ParentId";
                    cnn.Execute(sql, new { ParentId = request.Head.Id }, ts);


                    request.Head.Id = request.Head.Id;
                    request.Head.Source = "LR";
                    var id = Add(new RequestDocAdd
                    {
                        Token = request.Token,
                        Head = request.Head,
                        Detail = request.Detail
                    }, ts, user);

                    #region 更新余额表 Hero.Zhang 
                    RequestDocAdd docRequest = new RequestDocAdd();
                    docRequest.Head = request.Head;
                    docRequest.Detail = request.Detail;
                    docRequest.Type = request.Type;
                    docRequest.TPLId = request.TPLId;
                    docRequest.Key = request.Key;
                    balBLL.UpdateBalance(docRequest, ts, user);
                    #endregion
                    ts.Commit();

                    response.IsSuccess = true;
                    response.Id = id;
                    response.Message = "更新成功";



                    return response;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseDocUpdate;
                }

            }
        }

        public ResponseDocDelete DocDelete(RequestDocDelete request)
        {
            ResponseDocDelete response = new ResponseDocDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    BalanceBLL balanceBLL = new BalanceBLL(cnn);
                    var user = this.UserInfoGet(request.Token, ts);
                    FixedAssetsBLL fixBLL = new FixedAssetsBLL(cnn);
                    var flag = fixBLL.IsDocHasChangeRecord(request.Data.Id, ts);
                    if (flag)
                    {
                        //取消此判断 update by Hero.Zhang 20180404
                        //throw new NormalException("该凭证通过固定资产生成，无法删除");
                    }


                    string sql = @"select * from tks_fas_doc where id=@Id";
                    var DocDate = cnn.Query(sql, new { Id = request.Data.Id }, ts);
                    if (DocDate.Count() <= 0)
                    {
                        throw new NormalException("该凭证已经删除");
                    }
                    var data = cnn.QueryFirst<TKS_FAS_Doc>(sql, new { Id = request.Data.Id }, ts);
                    PeriodBLL period = new PeriodBLL(cnn);
                    flag = period.IsPeriodPaid(data.PeriodId, ts);
                    if (flag)
                    {
                        throw new NormalException("该凭证已经期末结转，无法删除");
                    }
                    #region 先回滚余额表 Hero.Zhang 
                    List<TKS_FAS_DocDetail> Detail = cnn.Query<TKS_FAS_DocDetail>("select *  from TKS_FAS_DocDetail where parentId=@ParentId", new { ParentId = request.Data.Id }, ts).ToList();
                    var doc = cnn.QueryFirst<TKS_FAS_Doc>("select * from TKS_FAS_Doc where id=@Id",
                        new { Id = request.Data.Id }, ts);
                    RequestDocAdd oldDoc = new RequestDocAdd();
                    oldDoc.Head = doc;
                    oldDoc.Detail = Detail;
                    foreach (var det in oldDoc.Detail)
                    {
                        det.Money_Debit = -1 * det.Money_Debit;
                        det.Money_Credit = -1 * det.Money_Credit;
                        det.Quantity = -1 * det.Quantity;
                    }
                    balanceBLL.UpdateBalance(oldDoc, ts, user);
                    #endregion

                    sql = @" delete from TKS_FAS_Doc where id=@Id";

                    cnn.Execute(sql, request.Data, ts);

                    sql = "delete from TKS_FAS_DocDetail where parentId=@ParentId";
                    cnn.Execute(sql, new { ParentId = request.Data.Id }, ts);

                    //发票状态置为 递交财务
                    sql = "update tks_fas_invoice set status=1 where pzId=@PZId and AccountId=@AccountId";
                    cnn.Execute(sql, new { PZId = data.Id, AccountId = data.AccountId }, ts);

                    sql = "delete from TKS_FAS_TPL2PZ where pzId=@PZID and AccountId=@AccountId ";
                    cnn.Execute(sql, new { PZID = data.Id, AccountId = data.AccountId }, ts);
                    // add by Hero.Zhang 20180404
                    /*判断次凭证号是否在变更记录及生成凭证表TKS_FAS_FixedAssetsChange中，如果存在，则
                    将这笔数据还原成新增并且没有生成凭证的状态；同时将固定资产表TKS_FAS_FixedAssets中这笔固定资产变更为‘未生成凭证状态’,同时将折旧相关数据回冲一个月*/
                    var check = "select * from TKS_FAS_FixedAssetsChange where docid=@docid and AccountId=@AccountId";
                    var datacheck = cnn.Query(check, new { docid = data.Id, AccountId = data.AccountId }, ts);
                    if (datacheck.Count() > 0)
                    {
                        var dataUpdate = cnn.QueryFirst<TKS_FAS_FixedAssetsChange>(check, new { docid = data.Id, AccountId = data.AccountId }, ts);
                        if (dataUpdate != null)
                        {
                            sql = "update TKS_FAS_FixedAssetsChange set ChangeType=1,DocId='',DocPZZ='' where id=@id  and AccountId=@AccountId";
                            //cnn.Execute(sql, new { id = request.TKS_FAS_FixedAssetsChange_Id, AccountId = data.AccountId }, ts);
                            cnn.Execute(sql, new { id = dataUpdate.Id, AccountId = data.AccountId }, ts);
                            sql = "update TKS_FAS_FixedAssets set IsGenPZ=0 where id=@id  and AccountId=@AccountId";
                            //cnn.Execute(sql, new { id = request.TKS_FAS_FixedAssets_Id, AccountId = data.AccountId }, ts);
                            cnn.Execute(sql, new { id = dataUpdate.ParentId, AccountId = data.AccountId }, ts);
                            
                        }
                    }
                    //add by Hero.Zhang 20180903 删除计提折旧凭证，回滚固定资产计提折旧数据
                    if (data.Source== PZType.GD.ToString())
                    {
                        //add by Hero.Zhang 删除折旧凭证判断，如果改凭证的日期之后有折旧凭证，则不允许删除
                        var checkPZ = @"select * from tks_fas_doc where source='GD' and PZDate>@PZDate AND AccountId=@AccountId";
                        var PZcheck = cnn.Query(checkPZ, new { PZDate = data.PZDate, AccountId = data.AccountId }, ts);
                        if (PZcheck.Count() > 0)
                        {
                            throw new NormalException("该计提折旧凭证无法删除，请先删除下一个期间的计提折旧凭证");
                        }

                        //回滚固定资产
                        fixBLL.RollbackFixedAssets(user.AccountId, data.PeriodId, ts);
                    }
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseDocDelete;
                }
            }
        }

        /// <summary>
        /// 凭证审核
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseDocAudit DocAudit(RequestDocAudit request)
        {
            ResponseDocAudit res = new ResponseDocAudit();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"update tks_fas_doc set checkStatus=2 where id=@Id and checkStatus=1";
                    cnn.Execute(sql, new { Id = request.Data.Id }, ts);
                    ts.Commit();

                    res.IsSuccess = true;
                    res.Message = "审核成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseDocAudit;
                }

            }
        }

        /// <summary>
        /// 凭证取消审核
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseDocUnAudit DocUnAudit(RequestDocUnAudit request)
        {
            ResponseDocUnAudit res = new ResponseDocUnAudit();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = @"update tks_fas_doc set checkStatus=1 where id=@Id and checkStatus=2";
                    cnn.Execute(sql, new { Id = request.Data.Id }, ts);
                    ts.Commit();

                    res.IsSuccess = true;
                    res.Message = "取消审核成功";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseDocUnAudit;
                }

            }
        }

        public ResponseDocNOGet GetNextDocNO(RequestDocNOGet request)
        {
            using (cnn = GetConnection())
            {
                ResponseDocNOGet res = new ResponseDocNOGet();
                DateTime docDate = DateTime.Parse(request.DocDate);
                PeriodBLL periodBLL = new PeriodBLL(cnn);
                var user = this.UserInfoGet(request.Token, null);
                AccountSetBLL accountSetBLL = new AccountSetBLL(cnn);
                var activePeriod = accountSetBLL.GetActivePeriod(user.AccountId, null);
                if ((activePeriod.Year == docDate.Year && activePeriod.Month > docDate.Month)
                    || activePeriod.Year > docDate.Year)
                {
                    res.IsSuccess = false;
                    res.Message = "该期别,不可新增凭证";
                    return res;
                }
                var period = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>("select * from TKS_FAS_MonthPeriodInfo where Year=@Year and AccountId=@AccountId and Month=@Month", new { Year = docDate.Year, Month = docDate.Month, AccountId = user.AccountId }, null);
                string no = string.Empty;
                if (period == null)
                {
                    no = "1";
                }
                else
                {
                    string sql = @"select (case when max(pzzno) is null then 0 else max(pzzno) end) from TKS_FAS_Doc where accountid=@AccountId and periodId=@PeriodId";
                    var max = cnn.ExecuteScalar(sql, new { AccountId = user.AccountId, PeriodId = period.Id }, null);
                    no = (int.Parse(max.ToString()) + 1).ToString();
                }
                res.NO = no;
                res.IsSuccess = true;
                return res;
            }
        }
    }
}
