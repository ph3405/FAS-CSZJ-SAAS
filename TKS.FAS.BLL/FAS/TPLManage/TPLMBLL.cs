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
using System.Data;

namespace TKS.FAS.BLL.FAS
{
    /// <summary>
    /// 综合模板设置
    /// </summary>
    public class TPLMBLL : CommonBase
    {
        public ResponseTPLMListSearch TPLListSearch(RequestTPLMListSearch request)
        {

            ResponseTPLMListSearch response = new ResponseTPLMListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    string where = string.Empty;
                    if (request.TPLTarget != 999)
                    {
                        where += " and A.tplTarget= " + request.TPLTarget;
                    }

                  

                    if (!string.IsNullOrEmpty(request.Title))
                    {
                        where += string.Format(" and A.title like '%{0}%'", request.Title);
                    }
                    //update by Hero.Zhang 只显示初始模板数据accountId='-'
                    string sql = this.GetPageSql("A.* ",
                        @"  TKS_FAS_DocTPL A
                        where  A.isCustom=0 and A.accountId='-'" + where,//非自定义
                        " A.createDate  desc,A.updateDate desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_DocTPL> data = cnn.Query<TKS_FAS_DocTPL>(sql,
                        new
                        {


                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_DocTPL A
                    where  A.isCustom=0  and A.accountId='-'" + where;

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {

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
                    return this.DealException(response, ex) as ResponseTPLMListSearch;
                }
            }
        }

        /// <summary>
        /// 模板加载
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseTPLMGet TPLGet(RequestTPLMGet request)
        {
            ResponseTPLMGet response = new ResponseTPLMGet();
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
                    AccountSetBLL account = new AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);

                    //模板体明细
                    var detail = cnn.Query<TKS_FAS_DocDetailTPL>(
                        @"select * from tks_fas_docDetailTPL where parentId=@ParentId",
                        new { ParentId = head.Id }, ts).ToList();
                    //如果是固定资产，则需要捞取当前账套的所有固定资产的每月折旧额，生成计提折旧凭证
                    //需要对detail进行再包装
                    if (head.TPLTarget == 3)
                    {
                        #region 固定资产，计提折旧
                        FixedAssetsBLL fbll = new FixedAssetsBLL(cnn);
                      
                        //获取固定资产
                        List<TKS_FAS_FixedAssets> fixedAssets = fbll.FixedAssetsGetByAcccount(user.AccountId,period, ts);
                        //凭证明细
                        List<TKS_FAS_DocDetailTPL> newDetail = new List<TKS_FAS_DocDetailTPL>();
                        head.AMT_DBT = 0;
                        foreach (var item in fixedAssets)
                        {
                            if (item.DepreciationMethod == "2") continue;
                            if (item.IsStartPeriodDepreciation == 0 && item.StartPeriod == period.Id)
                                continue;
                            if (item.DpreMonth < item.PreUseMonth)
                            {
                                //固定资产累计折旧科目，产生到凭证
                                //用固定资产表单上的科目覆盖模板的科目
                                for (var i = 0; i < detail.Count; i++)
                                {
                                    //update by Hero.Zhang 每次循环前必须将借贷金额初始化
                                    detail[i].Money_Credit = 0;
                                    detail[i].Money_Debit = 0;
                                    if (detail[i].Credit_Debit == 1)
                                    {
                                        detail[i].SubjectCode = item.ADSubjectCode;
                                        //detail[i].SubjectDescription = item.ADSubjectCode + " " + item.ADSubjectName;
                                        detail[i].SubjectDescription = item.ADSubjectName;
                                    }
                                    else if (detail[i].Credit_Debit == 0)
                                    {

                                        detail[i].SubjectCode = item.DCostSubjectCode;
                                        detail[i].SubjectDescription = item.DCostSubjectName;
                                    }
                                }
                                //填充金额
                                var d = FillDetail(item.DprePerMonth, detail);
                                newDetail.AddRange(d);
                                head.AMT_DBT += item.DprePerMonth;
                                //if (item.ADSubjectCode == "1502001")
                                //{
                                //    test.Add(item);
                                //}
                            }
                            
                        }
                        //分组合并
                        var lst = newDetail.GroupBy(x => new { x.Id, x.ParentId, x.LineNo, x.AccountId, x.Year, x.SubjectCode, x.SubjectDescription, x.Summary, x.Credit_Debit, x.PartnerCode, x.Unit, x.CurrencyCode, x.CalItem1, x.CalValue1, x.CalItem2, x.CalValue2, x.CalItem3, x.CalValue3, x.CalItem4, x.CalValue4, x.CalItem5, x.CalValue5, x.Rate, x.Quantity, x.Price, x.IsCurrency, x.IsQuantity, x.IsCalHelper, x.Balance, x.YB, x.IPercent, x.SourceType }).Select(x => new TKS_FAS_DocDetailTPL
                        {
                            Id = x.First().Id,
                            ParentId = x.First().ParentId,
                            LineNo = x.First().LineNo,
                            AccountId = x.First().AccountId,
                            Year = x.First().Year,
                            SubjectCode = x.First().SubjectCode,
                            SubjectDescription = x.First().SubjectDescription,
                            Summary = x.First().Summary,
                            Credit_Debit = x.First().Credit_Debit,
                            PartnerCode = x.First().PartnerCode,
                            Unit = x.First().Unit,
                            CurrencyCode = x.First().CurrencyCode,
                            CalItem1 = x.First().CalItem1,
                            CalValue1 = x.First().CalValue1,
                            CalItem2 = x.First().CalItem2,
                            CalValue2 = x.First().CalValue2,
                            CalItem3 = x.First().CalItem3,
                            CalValue3 = x.First().CalValue3,
                            CalItem4 = x.First().CalItem4,
                            CalValue4 = x.First().CalValue4,
                            CalItem5 = x.First().CalItem5,
                            CalValue5 = x.First().CalValue5,
                            Rate = x.First().Rate,
                            Quantity = x.First().Quantity,
                            Price = x.First().Price,
                            IsCurrency = x.First().IsCurrency,
                            IsQuantity = x.First().IsQuantity,
                            IsCalHelper = x.First().IsCalHelper,
                            Balance = x.First().Balance,
                            YB = x.First().YB,
                            IPercent = x.First().IPercent,
                            SourceType = x.First().SourceType,
                            Money_Credit = x.Sum(s => s.Money_Credit),
                            Money_Debit = x.Sum(s => s.Money_Debit)
                        }).ToList();
                        //detail = newDetail; 
                        detail = lst;
                        #endregion
                    }
                    else if (head.TPLTarget == 7)
                    {
                        #region 固定资产变动单类型
                        var changeTypeToTpl = cnn.QuerySingleOrDefault<TKS_FAS_FixedAssetsChange>(
                            "select * from TKS_FAS_FixedAssetsChange where Id=@Id ",
                            new { Id = request.Key }, ts);
                        if (changeTypeToTpl != null)
                        {

                            //固定资产变动类型新增的情况
                            string fixsql = @"select B.* from TKS_FAS_FixedAssetsChange A 
                        left join TKS_FAS_FixedAssets B on A.parentId=B.id where A.id=@ID";
                            TKS_FAS_FixedAssets fixedAssets = cnn.QuerySingleOrDefault<TKS_FAS_FixedAssets>(fixsql, new { ID = request.Key }, ts);
                            if (fixedAssets == null)
                            {
                                throw new Exception("固定资产不存在");
                            }
                            FillDetail(request.Money, detail);

                            if (changeTypeToTpl.ChangeType == 1)
                            {
                                #region 新增
                                for (var i = 0; i < detail.Count; i++)
                                {

                                    if (detail[i].SubjectCode == "1501")
                                    {
                                        detail[i].SubjectCode = fixedAssets.GDCode;
                                        detail[i].SubjectDescription = fixedAssets.GDName;
                                    }
                                    else if (detail[i].SubjectCode == "217100101")
                                    {
                                        if (detail[i].Credit_Debit == 0)
                                        {
                                            detail[i].Money_Debit = fixedAssets.InputVAT;
                                        }
                                        else
                                            detail[i].Money_Credit = fixedAssets.InputVAT;
                                    }
                                    else if (detail[i].SubjectCode == "1002")
                                    {
                                        if (detail[i].Credit_Debit == 0)
                                        {
                                            detail[i].Money_Debit = request.Money + fixedAssets.InputVAT;
                                        }
                                        else
                                        {
                                            detail[i].Money_Credit = request.Money + fixedAssets.InputVAT;
                                        }

                                    }
                                }

                                request.Money += fixedAssets.InputVAT;
                                #endregion
                            }
                            else if (changeTypeToTpl.ChangeType == 8)
                            {
                                #region 报废
                                for (var i = 0; i < detail.Count; i++)
                                {

                                    if (detail[i].SubjectCode == "1501")
                                    {
                                        if (detail[i].Credit_Debit == 0)
                                        {
                                            detail[i].Money_Debit = fixedAssets.InitialAssetValue;
                                        }
                                        else
                                            detail[i].Money_Credit = fixedAssets.InitialAssetValue;
                                    }
                                    else if (detail[i].SubjectCode == "1502")
                                    {
                                        if (detail[i].Credit_Debit == 0)
                                        {
                                            detail[i].Money_Debit = fixedAssets.AccumulativeDpre+fixedAssets.DprePerMonth;//累计折旧+每月折旧额
                                        }
                                        else
                                            detail[i].Money_Credit = fixedAssets.AccumulativeDpre+ fixedAssets.DprePerMonth;
                                    }
                                    else if (detail[i].SubjectCode == "1701")
                                    {
                                        if (detail[i].Credit_Debit == 0)
                                        {
                                            detail[i].Money_Debit = fixedAssets.InitialAssetValue- fixedAssets.AccumulativeDpre- fixedAssets.DprePerMonth;//减去累计折旧和每月折旧额
                                        }
                                        else
                                        {
                                            detail[i].Money_Credit = fixedAssets.InitialAssetValue - fixedAssets.AccumulativeDpre- fixedAssets.DprePerMonth;
                                        }

                                    }
                                }

                                request.Money = fixedAssets.InitialAssetValue;
                                #endregion
                            }
 
                        }
                        else
                        {
                            FillDetail(request.Money, detail);
                        }
                        #endregion
                    }
                    else if (head.TPLTarget == 2)
                    {
                        #region 发票上传
                        //var fp = cnn.Query<TKS_FAS_Invoice>(
                        //    "select * from TKS_FAS_Invoice where id=@Id", new { Id =request.Data.Id}, ts);
                        //if (fp == null)
                        //{
                        //    throw new NormalException("未找到对应发票");
                        //}
                        FillDetail(request.Money, detail);
                        decimal total = 0;
                        for (var i = 0; i < detail.Count; i++)
                        {
                            if (detail[i].SourceType == 0)
                            {
                                //发生额比例，不做处理
                            }
                            else if (detail[i].SourceType==1) {
                                //含税金额
                                if (detail[i].Credit_Debit == 0)
                                {
                                    detail[i].Money_Debit = request.Money;
                                }
                                else
                                {
                                    detail[i].Money_Credit = request.Money;
                                }
                            }
                            else if (detail[i].SourceType == 2)
                            {
                                //税金
                                if (detail[i].Credit_Debit == 0)
                                {
                                    detail[i].Money_Debit = request.Tax;
                                }
                                else
                                {
                                    detail[i].Money_Credit = request.Tax;
                                }
                            }
                            else if (detail[i].SourceType == 3)
                            {
                                //未税金额
                                if (detail[i].Credit_Debit == 0)
                                {
                                    detail[i].Money_Debit =request.Money- request.Tax;
                                }
                                else
                                {
                                    detail[i].Money_Credit =request.Money- request.Tax;
                                }
                            }

                            total += detail[i].Money_Debit;
                        }
                        request.Money = total;
                        #endregion
                    }
                    else
                    {
                        FillDetail(request.Money, detail);
                    }

                    if (request.Money > 0)
                    {
                        head.AMT_DBT = request.Money;
                    }
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Head = head;
                    response.Detail = detail;
                    #region add by Hero.Zhang 判断科目行是否有子科目
                    for (int i = 0; i < detail.Count(); i++)
                    {
                        var item = detail[i];
                        List<TKS_FAS_AccountSubject> sub = cnn.Query<TKS_FAS_AccountSubject>(@"select * from TKS_FAS_AccountSubject where AccountId=@AccountId and RootCode=@RootCode",
                        new { AccountId = user.AccountId, RootCode = item.SubjectCode }, ts).ToList();
                        if (sub.Count > 1)
                        {
                            response.CheckParent = "1";
                            response.Message += "</br>第" + (i + 1).ToString() + "行有子科目，请重新选择";
                        }
                    }
                    #endregion
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseTPLMGet;
                }
            }
        }


        /// <summary>
        /// 综合模板管理（无需选择账套）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseTPLMGet TPLMGet(RequestTPLMGet request)
        {
            ResponseTPLMGet response = new ResponseTPLMGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    var head = cnn.QuerySingle<TKS_FAS_DocTPL>("select * from TKS_FAS_DocTPL where id=@Id", new
                    {
                        Id = request.Data.Id
                    }, ts);


                    //模板体明细
                    var detail = cnn.Query<TKS_FAS_DocDetailTPL>(
                        @"select * from tks_fas_docDetailTPL where parentId=@ParentId",
                        new { ParentId = head.Id }, ts).ToList();
                    //如果是固定资产，则需要捞取当前账套的所有固定资产的每月折旧额，生成计提折旧凭证
                    //需要对detail进行再包装
                    if (head.TPLTarget == 3)
                    {
                        #region 固定资产，计提折旧
                        FillDetail(request.Money, detail);
                        #endregion
                    }
                    else if (head.TPLTarget == 7)
                    {
                        #region 固定资产变动单类型
                        var changeTypeToTpl = cnn.QuerySingleOrDefault<TKS_FAS_FixedAssetsChange>(
                            "select * from TKS_FAS_FixedAssetsChange where Id=@Id and changeType=1",
                            new { Id = request.Key }, ts);
                        if (changeTypeToTpl != null)
                        {
                            //固定资产变动类型新增的情况
                            string fixsql = @"select B.* from TKS_FAS_FixedAssetsChange A 
                        left join TKS_FAS_FixedAssets B on A.parentId=B.id where A.id=@ID";
                            TKS_FAS_FixedAssets fixedAssets = cnn.QuerySingleOrDefault<TKS_FAS_FixedAssets>(fixsql, new { ID = request.Key }, ts);
                            if (fixedAssets == null)
                            {
                                throw new Exception("固定资产不存在");
                            }
                            FillDetail(request.Money, detail);
                            for (var i = 0; i < detail.Count; i++)
                            {

                                if (detail[i].SubjectCode == "1501")
                                {
                                    detail[i].SubjectCode = fixedAssets.GDCode;
                                    detail[i].SubjectDescription = fixedAssets.GDName;
                                }
                                else if (detail[i].SubjectCode == "217100101")
                                {
                                    if (detail[i].Credit_Debit == 0)
                                    {
                                        detail[i].Money_Debit = fixedAssets.InputVAT;
                                    }
                                    else
                                        detail[i].Money_Credit = fixedAssets.InputVAT;
                                }
                                else if (detail[i].SubjectCode == "1002")
                                {
                                    if (detail[i].Credit_Debit == 0)
                                    {
                                        detail[i].Money_Debit = request.Money + fixedAssets.InputVAT;
                                    }
                                    else
                                    {
                                        detail[i].Money_Credit = request.Money + fixedAssets.InputVAT;
                                    }

                                }
                            }

                            request.Money += fixedAssets.InputVAT;

                        }
                        else
                        {
                            FillDetail(request.Money, detail);
                        }
                        #endregion
                    }
                    else if (head.TPLTarget == 2)
                    {
                        #region 发票上传
                        //var fp = cnn.Query<TKS_FAS_Invoice>(
                        //    "select * from TKS_FAS_Invoice where id=@Id", new { Id =request.Data.Id}, ts);
                        //if (fp == null)
                        //{
                        //    throw new NormalException("未找到对应发票");
                        //}
                        FillDetail(request.Money, detail);
                        decimal total = 0;
                        for (var i = 0; i < detail.Count; i++)
                        {
                            if (detail[i].SourceType == 0)
                            {
                                //发生额比例，不做处理
                            }
                            else if (detail[i].SourceType == 1)
                            {
                                //含税金额
                                if (detail[i].Credit_Debit == 0)
                                {
                                    detail[i].Money_Debit = request.Money;
                                }
                                else
                                {
                                    detail[i].Money_Credit = request.Money;
                                }
                            }
                            else if (detail[i].SourceType == 2)
                            {
                                //税金
                                if (detail[i].Credit_Debit == 0)
                                {
                                    detail[i].Money_Debit = request.Tax;
                                }
                                else
                                {
                                    detail[i].Money_Credit = request.Tax;
                                }
                            }
                            else if (detail[i].SourceType == 3)
                            {
                                //未税金额
                                if (detail[i].Credit_Debit == 0)
                                {
                                    detail[i].Money_Debit = request.Money - request.Tax;
                                }
                                else
                                {
                                    detail[i].Money_Credit = request.Money - request.Tax;
                                }
                            }

                            total += detail[i].Money_Debit;
                        }
                        request.Money = total;
                        #endregion
                    }
                    else
                    {
                        FillDetail(request.Money, detail);
                    }

                    if (request.Money > 0)
                    {
                        head.AMT_DBT = request.Money;
                    }
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Head = head;
                    response.Detail = detail;

                    #region add by Hero.Zhang 判断科目行是否有子科目
                    for (int i = 0; i < detail.Count(); i++)
                    {
                        var item = detail[i];
                        List<TKS_FAS_AccountSubject> sub = cnn.Query<TKS_FAS_AccountSubject>(@"select * from TKS_FAS_AccountSubject where AccountId=@AccountId and RootCode=@RootCode",
                       new { AccountId = user.AccountId, RootCode = item.SubjectCode }, ts).ToList();
                        if (sub.Count > 1)
                        {
                            response.CheckParent = "1";
                            response.Message += "</br>第" + (i + 1).ToString() + "行有子科目，请重新选择";
                        }
                    }
                    #endregion

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseTPLMGet;
                }
            }
        }

        private List<TKS_FAS_DocDetailTPL> FillDetail(decimal money, List<TKS_FAS_DocDetailTPL> detail)
        {
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
                detail[i] = FillMoney(money, detail[i]);//如果有金额，则填充
            }
            List<TKS_FAS_DocDetailTPL> newDetail = new List<TKS_FAS_DocDetailTPL>();
            foreach (var item in detail)
            {
                newDetail.Add(new TKS_FAS_DocDetailTPL
                {
                    AccountId = item.AccountId,
                    CalItem1 = item.CalItem1,
                    CalItem2 = item.CalItem2,
                    CalItem3 = item.CalItem3,
                    CalItem4 = item.CalItem4,
                    CalItem5 = item.CalItem5,
                    CalValue1 = item.CalValue1,
                    CalValue2 = item.CalValue2,
                    CalValue3 = item.CalValue3,
                    CalValue4 = item.CalValue4,
                    CalValue5 = item.CalValue5,
                    IsCalHelper = item.IsCalHelper,
                    IsCurrency = item.IsCurrency,
                    IsQuantity = item.IsQuantity,
                    Credit_Debit = item.Credit_Debit,
                    IPercent = item.IPercent,
                    CurrencyCode = item.CurrencyCode,
                    Money_Credit = item.Money_Credit,
                    Money_Debit = item.Money_Debit,
                    Summary = item.Summary,
                    SubjectCode = item.SubjectCode,
                    SubjectDescription = item.SubjectDescription,
                    Price = item.Price,
                    ParentId = item.ParentId


                });
            }


            return newDetail;
        }

        /// <summary>
        /// 填充金额
        /// </summary>
        /// <param name="money"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private TKS_FAS_DocDetailTPL FillMoney(decimal money, TKS_FAS_DocDetailTPL row)
        {
            if (money > 0)
            {
                if (string.IsNullOrEmpty(row.SubjectCode))
                {
                    return row;
                }
                else
                {
                    //根据模板的借贷方向，填充金额*模板设定的发生额比例，默认为1
                    if (row.Credit_Debit == 0)
                        row.Money_Debit = (money * row.IPercent);
                    else
                        row.Money_Credit = (money * row.IPercent);
                    return row;
                }
            }
            else
            {
                return row;
            }
        }
        public ResponseTPLMAdd TPLAdd(RequestTPLMAdd request)
        {
            ResponseTPLMAdd response = new ResponseTPLMAdd();
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
                    return this.DealException(response, ex) as ResponseTPLMAdd;
                }
            }
        }

        /// <summary>
        /// 综合模板新增（无需选择账套）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseTPLMAdd TPLMAdd(RequestTPLMAdd request)
        {
            ResponseTPLMAdd response = new ResponseTPLMAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    var user = this.UserInfoGetButAccount(request.Token, ts);
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
                    return this.DealException(response, ex) as ResponseTPLMAdd;
                }
            }
        }

        private string Add(RequestTPLMAdd request, System.Data.IDbTransaction ts, MM_UserInfo user)
        {
            string accountId = "-";

            if (request.Type == "custom")
            {
                accountId = user.AccountId;
            }

            if (string.IsNullOrEmpty(request.Head.Title))
            {
                throw new NormalException("请填写模板名称");
            }
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
                item.Summary = item.Summary.Trim();
                item.SubjectDescription = item.SubjectDescription.Trim();

                item.Year = period==null?2099:period.Year;
                item.ParentId = request.Head.Id;
                item.AccountId = accountId;
                cnn.Insert<TKS_FAS_DocDetailTPL>(item, ts);
                flag++;
            }
            if (flag == 0)
                throw new NormalException("亲，您还没有填写明细");



            request.Head.AccountId = accountId;//非特定账套下
            request.Head.AMT_DBT = total;

            request.Head.IsCustom = 0;//非自定义
            request.Head.CreateUser = user.User.TrueName;
            request.Head.CreateDate = DateTime.Now;
            cnn.Insert<TKS_FAS_DocTPL>(request.Head, ts);

            return request.Head.Id;
        }

        public ResponseTPLMUpdate TPLUpdate(RequestTPLMUpdate request)
        {
            ResponseTPLMUpdate response = new ResponseTPLMUpdate();
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
                    var id = Add(new RequestTPLMAdd
                    {
                        Type = request.Type,
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
                    return this.DealException(response, ex) as ResponseTPLMUpdate;
                }

            }
        }

        /// <summary>
        /// 综合模板更新（无需选择账套）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseTPLMUpdate TPLMUpdate(RequestTPLMUpdate request)
        {
            ResponseTPLMUpdate response = new ResponseTPLMUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);

                    string sql = @" delete from TKS_FAS_DocTPL where id=@Id";

                    cnn.Execute(sql, request.Head, ts);

                    sql = "delete from TKS_FAS_DocDetailTPL where parentId=@ParentId";
                    cnn.Execute(sql, new { ParentId = request.Head.Id }, ts);


                    request.Head.Id = request.Head.Id;
                    var id = Add(new RequestTPLMAdd
                    {
                        Type = request.Type,
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
                    return this.DealException(response, ex) as ResponseTPLMUpdate;
                }

            }
        }
        public ResponseTPLMDelete TPLDelete(RequestTPLMDelete request)
        {
            ResponseTPLMDelete response = new ResponseTPLMDelete();
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
                    return this.DealException(response, ex) as ResponseTPLMDelete;
                }
            }
        }
    }
}
