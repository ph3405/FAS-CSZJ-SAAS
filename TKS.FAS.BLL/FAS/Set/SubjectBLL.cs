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
    public class SubjectBLL : CommonBase
    {
        public SubjectBLL() { }

        public SubjectBLL(IDbConnection cnn)
        {
            this.cnn = cnn;
        }

        public ResponseSubjectListSearch SubjectListSearch(RequestSubjectListSearch request)
        {

            ResponseSubjectListSearch response = new ResponseSubjectListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = this.GetPageSql("*",
                        @"TKS_FAS_AccountSubject where accountId=@AccountId and category=@Category ",
                        " code",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_AccountSubjectExt> data = cnn.Query<TKS_FAS_AccountSubjectExt>(sql,
                        new
                        {
                            AccountId = user.AccountId,
                            Category = request.Data.Category
                        }, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_AccountSubject where accountId=@AccountId and category=@Category";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = user.AccountId,
                        Category = request.Data.Category
                    }, ts).ToString());
                    List<TKS_FAS_AccountSubjectExt> new_lst = new List<TKS_FAS_AccountSubjectExt>();
                    foreach (var item in data)
                    {
                        int isTree = data.FindAll(p => p.ParentId == item.Id).Count;
                        if (isTree>0)
                        {
                            item.IsTree = "Y";
                        }
                        new_lst.Add(item);
                    }
                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    response.lst = new_lst;
                    response.Total = total;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseSubjectListSearch;
                }
            }
        }

        public ResponseSubjectListSearch CheckSubjectIsReadonly(RequestSubjectListSearch request)
        {

            ResponseSubjectListSearch response = new ResponseSubjectListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    string sql = @"SELECT * FROM TKS_FAS_AccountSubject where accountId=@AccountId and category=@Category and Code=@Code";
                    response.CheckReadonly = "";
                    List<TKS_FAS_AccountSubject> data = cnn.Query<TKS_FAS_AccountSubject>(sql,
                        new
                        {
                            AccountId = user.AccountId,
                            Category = request.Data.Category,
                            Code = request.Data.Code
                        }, ts).ToList();
                    if (data.Count() > 0)
                    {
                        if (data[0].Code == data[0].RootCode)
                        {
                            List<TKS_FAS_AccountSubject> data1 = cnn.Query<TKS_FAS_AccountSubject>("SELECT * FROM TKS_FAS_AccountSubject where accountId=@AccountId and category=@Category and RootCode=@RootCode",
                       new
                       {
                           AccountId = user.AccountId,
                           Category = request.Data.Category,
                           RootCode = request.Data.Code
                       }, ts).ToList();
                            if (data1.Count() > 1)
                            {
                                response.CheckReadonly = "readonly";
                            }

                        }
                        else
                        {
                            List<TKS_FAS_AccountSubject> dataSubject = cnn.Query<TKS_FAS_AccountSubject>("SELECT * FROM TKS_FAS_AccountSubject where accountId=@AccountId and ParentId=@ParentId",
                       new
                       {
                           AccountId = user.AccountId,
                           ParentId = data[0].Id
                       }, ts).ToList();
                            if (dataSubject.Count() > 0)
                            {
                                response.CheckReadonly = "readonly";
                            }
                        }
                    }

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseSubjectListSearch;
                }
            }
        }

        /// <summary>
        /// 凭证的科目选择，只能是叶子科目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseSubjectTotalGet SubjectTotalGet(RequestSubjectTotalGet request)
        {
            ResponseSubjectTotalGet response = new ResponseSubjectTotalGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);

                    //科目必须是叶子节点科目，才可以选择 and isLeaf=1
                    string sql = @"select * from TKS_FAS_AccountSubject where isvalid=1 and isLeaf=1
                       and accountId=@AccountId ";
                    if (!string.IsNullOrEmpty(request.Category))
                    {
                        sql += " and Category =  '" + request.Category + "'";
                    }
                    if (!string.IsNullOrEmpty(request.ShowTitle))
                    {
                        sql += " and ShowTitle like  '%" + request.ShowTitle + "%'";
                    }
                    if (!string.IsNullOrEmpty(request.Code))
                    {
                        sql += " and Code like  '%" + request.Code + "%'";
                    }
                    sql += " order by code";
                    var data = cnn.Query<TKS_FAS_AccountSubject>(
                        sql,
                        new { AccountId = user.AccountId }, ts).ToList();
                    List<SubjectAutoData> autoData = new List<SubjectAutoData>();
                    foreach (var item in data)
                    {
                        SubjectAutoData newItem = new SubjectAutoData();
                        newItem.Id = item.Code;//存编码  其它使用 联合组键  （账套，编码）
                        newItem.label = item.Code + " " + item.ShowTitle;
                        newItem.value = item.Code + " " + item.ShowTitle;
                        newItem.IsCalHelperValid = item.IsCalHelperValid;
                        newItem.IsCurrencyValid = item.IsCurrencyValid;
                        newItem.IsQuantityValid = item.IsQuantityValid;
                        autoData.Add(newItem);
                    }
                    response.IsSuccess = true;
                    response.Data = autoData;
                    response.Message = "加载完成";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectTotalGet;
                }
            }
        }

        public ResponseSubjectTotalGet GetSubject(RequestSubjectTotalGet request)
        {
            ResponseSubjectTotalGet response = new ResponseSubjectTotalGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);

                    //科目必须是叶子节点科目，才可以选择 and isLeaf=1
                    var data = cnn.Query<TKS_FAS_AccountSubject>(
                        @"select * from TKS_FAS_AccountSubject where isvalid=1 and isLeaf=1
                       and accountId=@AccountId order by code",
                        new { AccountId = user.AccountId }, ts).ToList();
                    List<SubjectAutoData> autoData = new List<SubjectAutoData>();
                    foreach (var item in data)
                    {
                        SubjectAutoData newItem = new SubjectAutoData();
                        newItem.Id = item.Code;//存编码  其它使用 联合组键  （账套，编码）
                        newItem.label = item.Code + " " + item.ShowTitle;
                        newItem.value = item.Code;
                        newItem.IsCalHelperValid = item.IsCalHelperValid;
                        newItem.IsCurrencyValid = item.IsCurrencyValid;
                        newItem.IsQuantityValid = item.IsQuantityValid;
                        autoData.Add(newItem);
                    }
                    response.IsSuccess = true;
                    response.Data = autoData;
                    response.Message = "加载完成";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectTotalGet;
                }
            }
        }

        public TKS_FAS_AccountSubject GetSubject(string subjectCode, string accountId, IDbTransaction ts)
        {
            string sql = @"select * from TKS_FAS_AccountSubject
                    where Code=@Code and accountId=@AccountId";

            var subject = cnn.QuerySingleOrDefault<TKS_FAS_AccountSubject>(sql,
                new { Code = subjectCode, AccountId = accountId }, ts);


            return subject;

        }
        /// <summary>
        /// 获取下级科目
        /// </summary>
        /// <param name="subjectCode"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public List<TKS_FAS_AccountSubject> GetChildrenSubjects(string subjectId, string accountId, IDbTransaction ts)
        {
            string sql = @"select * from TKS_FAS_AccountSubject
                    where ParentId=@Id and accountId=@AccountId";
            return cnn.Query<TKS_FAS_AccountSubject>(sql, ts).ToList();
        }
        public ResponseSubjectTotalGet TPLSubjectTotalGet(RequestSubjectTotalGet request)
        {
            ResponseSubjectTotalGet response = new ResponseSubjectTotalGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);

                    //科目必须是叶子节点科目，才可以选择 and isLeaf=1
                    var data = cnn.Query<TKS_FAS_AccountSubject>(
                        @"select * from TKS_FAS_AccountSubjectBak where isvalid=1 and isLeaf=1
                       and accountId='-' order by code",
                        new { AccountId = user.AccountId }, ts).ToList();

                    List<SubjectAutoData> autoData = new List<SubjectAutoData>();
                    foreach (var item in data)
                    {
                        SubjectAutoData newItem = new SubjectAutoData();
                        newItem.Id = item.Code;//存编码  其它使用 联合组键  （账套，编码）
                        newItem.label = item.Code + " " + item.ShowTitle;
                        newItem.value = item.Code + " " + item.ShowTitle;
                        newItem.IsCalHelperValid = item.IsCalHelperValid;
                        newItem.IsCurrencyValid = item.IsCurrencyValid;
                        newItem.IsQuantityValid = item.IsQuantityValid;
                        autoData.Add(newItem);
                    }
                    response.IsSuccess = true;
                    response.Data = autoData;
                    response.Message = "加载完成";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectTotalGet;
                }
            }
        }

        /// <summary>
        /// 科目选择，供报表公式选择，一级科目以及叶子科目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseSubjectTotalGet SubjectFormulaGet(RequestSubjectTotalGet request)
        {
            ResponseSubjectTotalGet response = new ResponseSubjectTotalGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);

                    //科目 
                    var data = cnn.Query<TKS_FAS_AccountSubject>(
                        @"select * from TKS_FAS_AccountSubject where isvalid=1 and (isLeaf=1 or slevel=1)
                       and accountId=@AccountId order by code",
                        new { AccountId = user.AccountId }, ts).ToList();

                    List<SubjectAutoData> autoData = new List<SubjectAutoData>();
                    foreach (var item in data)
                    {
                        SubjectAutoData newItem = new SubjectAutoData();
                        newItem.Id = item.Code;//存编码  其它使用 联合组键  （账套，编码）
                        newItem.label = item.Code + " " + item.Name;
                        newItem.value = item.Code + " " + item.Name;
                        newItem.IsCalHelperValid = item.IsCalHelperValid;
                        newItem.IsCurrencyValid = item.IsCurrencyValid;
                        newItem.IsQuantityValid = item.IsQuantityValid;
                        autoData.Add(newItem);
                    }
                    response.IsSuccess = true;
                    response.Data = autoData;
                    response.Message = "加载完成";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectTotalGet;
                }
            }
        }

        public ResponseSubjectGet SubjectGet(RequestSubjectGet request)
        {
            ResponseSubjectGet response = new ResponseSubjectGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    var data = cnn.QuerySingle<TKS_FAS_AccountSubject>(
                        "select * from TKS_FAS_AccountSubject where id=@Id",
                        new
                        {
                            Id = request.Data.Id
                        }, ts);

                    var currency = cnn.Query<TKS_FAS_CurrencyExt>(
                       @"select * from TKS_FAS_Currency where accountId=@AccountId",
                       new { AccountId = user.AccountId }, ts).ToList();

                    var calItem = cnn.Query<TKS_FAS_CaculateHelperItemExt>(
                        @"select * from TKS_FAS_CaculateHelperItem where accountId=@AccountId",
                        new { AccountId = user.AccountId }, ts).ToList();

                    var checkedVal = cnn.Query<TKS_FAS_SubjectAss>(
                        @"select * from TKS_FAS_SubjectAss where accountId=@AccountId and parentId=@ParentId",
                        new { AccountId = user.AccountId, ParentId = request.Data.Id }, ts);

                    foreach (var item in checkedVal)
                    {
                        for (int i = 0; i < currency.Count(); i++)
                        {
                            if (currency[i].Id == item.Code)
                            {
                                currency[i].IsChecked = 1;
                            }
                        }

                        for (int j = 0; j < calItem.Count(); j++)
                        {
                            if (calItem[j].Id == item.Code)
                            {
                                calItem[j].IsChecked = 1;
                            }
                        }
                    }

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;
                    response.CalItem = calItem;
                    response.Currency = currency;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectGet;
                }
            }
        }

        public ResponseSubjectAdd SubjectAdd(RequestSubjectAdd request)
        {
            ResponseSubjectAdd response = new ResponseSubjectAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var parent = GetParent(request.Data.ParentId, ts);//1.get parent obj
                                                                      //取消科目名称重复验证 update by Hero.Zhang 20180319
                    var d = cnn.Query(@"select * from TKS_FAS_AccountSubject 
                                                                          where name=@Name and accountId=@AccountId and rootCode=@rootCode",
                        new { Name = request.Data.Name, AccountId = user.AccountId, rootCode = parent.RootCode }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("科目名已经存在");
                    }




                    var maxCode = cnn.ExecuteScalar(@"select max(code) as code from TKS_FAS_AccountSubject
                            where rootCode=@rootCode and slevel=@Slevel and accountid=@AccountId ",
                      new
                      {
                          rootCode = parent.RootCode,
                          AccountId = user.AccountId,
                          Slevel = parent.SLevel + 1
                      }, ts);


                    if (maxCode != null)
                    {
                        //修改转换方式，防止int溢出， updaye by Hero.Zhang 20180321
                        //request.Data.Code = (int.Parse(maxCode.ToString()) + 1).ToString();
                        request.Data.Code = (Convert.ToInt64(maxCode.ToString()) + 1).ToString();
                    }
                    else
                    {
                        request.Data.Code = parent.Code + IdByLevel(parent.SLevel);
                    }

                    SetParentNotLeaf(request.Data.ParentId, ts);

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    if (request.Data.IsCurrencyValid == 1)
                    {
                        request.Data.CurrencyValue = SetCurrency(request.Currency, request.Data.Id, ts);
                    }

                    if (request.Data.IsCalHelperValid == 1)
                    {

                        request.Data.CalHelperValue = SetCalItem(request.CalItem, request.Data.Id, ts);
                    }

                    request.Data.RootCode = parent.RootCode;
                    request.Data.AccountId = user.AccountId;
                    request.Data.IsCustom = 1;
                    request.Data.IsLeaf = 1;
                    request.Data.SLevel = parent.SLevel + 1;
                    request.Data.ShowTitle = parent.ShowTitle + "-" + request.Data.Name.Trim();//科目名称去除空格 update by Hero.Zhang 20180319
                    var r = cnn.Insert<TKS_FAS_AccountSubject>(request.Data, ts);

                    #region 设置余额表的状态
                    SetBAL(request.Data, ts);
                    #endregion

                    #region add by Hero.Zhang 上级科目数据更新到当前科目，同时上级科目数据清空
                    if (request.checkSub=="1")
                    {
                        

                        //替换凭证表
                        cnn.Execute("update TKS_FAS_DocDetail set SubjectCode=@SubjectCode,SubjectDescription=@SubjectDescription where SubjectCode=@ParentCode and AccountId=@AccountId",
                      new { ParentCode = parent.Code, SubjectCode = request.Data.Code, AccountId = user.AccountId,  SubjectDescription = request.Data.Code + " " + request.Data.ShowTitle }, ts);

                        var GLBalance = cnn.Query<TKS_FAS_GLBalance>(
                                            @"select * from TKS_FAS_GLBalance where SubjectId=@ParentId and AccountId=@AccountId",
                                            new { ParentId = parent.Id, AccountId = user.AccountId }, ts).ToList();
                        string updateNextGLBalance = "";
                        foreach (var item in GLBalance)
                        {
                            if (string.IsNullOrEmpty(item.PeriodId))
                            {
                                //更新期初子科目数据，将上级科目值移到子科目
                                 updateNextGLBalance = @"update TKS_FAS_GLBalance set  [NUMStartBAL]=@NUMStartBAL
                           ,[BWBStartBAL]=@BWBStartBAL
                           ,[YBStartBAL]=@YBStartBAL
                           ,[NUMDebitTotal]=@NUMDebitTotal
                           ,[NUMDebitTotal_Y]=@NUMDebitTotal_Y
                           ,[BWBDebitTotal]=@BWBDebitTotal
                           ,[BWBDebitTotal_Y]=@BWBDebitTotal_Y
                           ,[YBDebitTotal]=@YBDebitTotal
                           ,[YBDebitTotal_Y]=@YBDebitTotal_Y
                           ,[NUMCreditTotal]=@NUMCreditTotal
                           ,[NUMCreditTotal_Y]=@NUMCreditTotal_Y
                           ,[BWBCreditTotal]=@BWBCreditTotal
                           ,[BWBCreditTotal_Y]=@BWBCreditTotal_Y
                           ,[YBCreditTotal]=@YBCreditTotal
                           ,[YBCreditTotal_Y]=@YBCreditTotal_Y
                           ,[ECredit_Debit]=@ECredit_Debit
                           ,[NUMEndBAL]=@NUMEndBAL
                           ,[BWBEndBAL]=@BWBEndBAL
                           ,[YBEndBAL]=@YBEndBAL
                           ,[YearStartNumBAL]=@YearStartNumBAL
                           ,[YearStartYBBAL]=@YearStartYBBAL
                           ,[YearStartBWBBAL]=@YearStartBWBBAL
                            where SubjectId=@SubjectId and AccountId=@AccountId and PeriodId=''";
                                cnn.Execute(updateNextGLBalance,
                              new { SubjectId = request.Data.Id, AccountId = user.AccountId, NUMStartBAL=item.NUMStartBAL, BWBStartBAL=item.BWBStartBAL, YBStartBAL=item.YBStartBAL, NUMDebitTotal=item.NUMDebitTotal, NUMDebitTotal_Y=item.NUMDebitTotal_Y, BWBDebitTotal=item.BWBDebitTotal, BWBDebitTotal_Y=item.BWBDebitTotal_Y, YBDebitTotal=item.YBDebitTotal, YBDebitTotal_Y=item.YBDebitTotal_Y, NUMCreditTotal=item.NUMCreditTotal, NUMCreditTotal_Y=item.NUMCreditTotal_Y, BWBCreditTotal=item.BWBCreditTotal, BWBCreditTotal_Y=item.BWBCreditTotal_Y, YBCreditTotal=item.YBCreditTotal, YBCreditTotal_Y=item.YBCreditTotal_Y, ECredit_Debit=item.ECredit_Debit, NUMEndBAL=item.NUMEndBAL, BWBEndBAL=item.BWBEndBAL, YBEndBAL=item.YBEndBAL, YearStartNumBAL=item.YearStartNumBAL, YearStartYBBAL=item.YearStartYBBAL, YearStartBWBBAL=item.YearStartBWBBAL }, ts);
                            }
                            else
                            {
                                updateNextGLBalance = @"update TKS_FAS_GLBalance set  [NUMStartBAL]=@NUMStartBAL
                           ,[BWBStartBAL]=@BWBStartBAL
                           ,[YBStartBAL]=@YBStartBAL
                           ,[NUMDebitTotal]=@NUMDebitTotal
                           ,[NUMDebitTotal_Y]=@NUMDebitTotal_Y
                           ,[BWBDebitTotal]=@BWBDebitTotal
                           ,[BWBDebitTotal_Y]=@BWBDebitTotal_Y
                           ,[YBDebitTotal]=@YBDebitTotal
                           ,[YBDebitTotal_Y]=@YBDebitTotal_Y
                           ,[NUMCreditTotal]=@NUMCreditTotal
                           ,[NUMCreditTotal_Y]=@NUMCreditTotal_Y
                           ,[BWBCreditTotal]=@BWBCreditTotal
                           ,[BWBCreditTotal_Y]=@BWBCreditTotal_Y
                           ,[YBCreditTotal]=@YBCreditTotal
                           ,[YBCreditTotal_Y]=@YBCreditTotal_Y
                           ,[ECredit_Debit]=@ECredit_Debit
                           ,[NUMEndBAL]=@NUMEndBAL
                           ,[BWBEndBAL]=@BWBEndBAL
                           ,[YBEndBAL]=@YBEndBAL
                           ,[YearStartNumBAL]=@YearStartNumBAL
                           ,[YearStartYBBAL]=@YearStartYBBAL
                           ,[YearStartBWBBAL]=@YearStartBWBBAL
                            where SubjectId=@SubjectId and AccountId=@AccountId and PeriodId!=''";
                                cnn.Execute(updateNextGLBalance,
                              new { SubjectId = request.Data.Id, AccountId = user.AccountId, NUMStartBAL = item.NUMStartBAL, BWBStartBAL = item.BWBStartBAL, YBStartBAL = item.YBStartBAL, NUMDebitTotal = item.NUMDebitTotal, NUMDebitTotal_Y = item.NUMDebitTotal_Y, BWBDebitTotal = item.BWBDebitTotal, BWBDebitTotal_Y = item.BWBDebitTotal_Y, YBDebitTotal = item.YBDebitTotal, YBDebitTotal_Y = item.YBDebitTotal_Y, NUMCreditTotal = item.NUMCreditTotal, NUMCreditTotal_Y = item.NUMCreditTotal_Y, BWBCreditTotal = item.BWBCreditTotal, BWBCreditTotal_Y = item.BWBCreditTotal_Y, YBCreditTotal = item.YBCreditTotal, YBCreditTotal_Y = item.YBCreditTotal_Y, ECredit_Debit = item.ECredit_Debit, NUMEndBAL = item.NUMEndBAL, BWBEndBAL = item.BWBEndBAL, YBEndBAL = item.YBEndBAL, YearStartNumBAL = item.YearStartNumBAL, YearStartYBBAL = item.YearStartYBBAL, YearStartBWBBAL = item.YearStartBWBBAL }, ts);
                            }
                        }
                        

                        //string sql = @"insert into TKS_FAS_GLBalance 
                        //   ([Id]
                        //   ,[AccountId]
                        //   ,[SubjectId]
                        //   ,[SubjectCode]
                        //   ,[ParentId]
                        //   ,[Name]
                        //   ,[PeriodId]
                        //   ,[Year]
                        //   ,[CurrencyCode]
                        //   ,[SCredit_Debit]
                        //   ,[NUMStartBAL]
                        //   ,[BWBStartBAL]
                        //   ,[YBStartBAL]
                        //   ,[NUMDebitTotal]
                        //   ,[NUMDebitTotal_Y]
                        //   ,[BWBDebitTotal]
                        //   ,[BWBDebitTotal_Y]
                        //   ,[YBDebitTotal]
                        //   ,[YBDebitTotal_Y]
                        //   ,[NUMCreditTotal]
                        //   ,[NUMCreditTotal_Y]
                        //   ,[BWBCreditTotal]
                        //   ,[BWBCreditTotal_Y]
                        //   ,[YBCreditTotal]
                        //   ,[YBCreditTotal_Y]
                        //   ,[ECredit_Debit]
                        //   ,[NUMEndBAL]
                        //   ,[BWBEndBAL]
                        //   ,[YBEndBAL]
                        //   ,[Category]
                        //   ,[Unit]
                        //   ,[YearStartNumBAL]
                        //   ,[YearStartYBBAL]
                        //   ,[YearStartBWBBAL]
                        //   ,[CreateUser]
                        //   ,[CreateDate]
                        //   ,[IsCalHelperValid]
                        //   ,[IsQuantityValid]
                        //   ,[IsDefaultCurrency]
                        //   ,[IsLeaf])
                        //select 
                        //newid()
                        //,[AccountId]
                        //   ,@SubjectId
                        //   ,@SubjectCode
                        //   ,@parentId
                        //   ,@Name
                        //   ,[PeriodId]
                        //   ,[Year]
                        //   ,[CurrencyCode]
                        //   ,[SCredit_Debit]
                        //   ,[NUMStartBAL]
                        //   ,[BWBStartBAL]
                        //   ,[YBStartBAL]
                        //   ,[NUMDebitTotal]
                        //   ,[NUMDebitTotal_Y]
                        //   ,[BWBDebitTotal]
                        //   ,[BWBDebitTotal_Y]
                        //   ,[YBDebitTotal]
                        //   ,[YBDebitTotal_Y]
                        //   ,[NUMCreditTotal]
                        //   ,[NUMCreditTotal_Y]
                        //   ,[BWBCreditTotal]
                        //   ,[BWBCreditTotal_Y]
                        //   ,[YBCreditTotal]
                        //   ,[YBCreditTotal_Y]
                        //   ,[ECredit_Debit]
                        //   ,[NUMEndBAL]
                        //   ,[BWBEndBAL]
                        //   ,[YBEndBAL]
                        //   ,[Category]
                        //   ,[Unit]
                        //   ,[YearStartNumBAL]
                        //   ,[YearStartYBBAL]
                        //   ,[YearStartBWBBAL]
                        //   ,[CreateUser]
                        //   ,[CreateDate]
                        //   ,[IsCalHelperValid]
                        //   ,[IsQuantityValid]
                        //   ,[IsDefaultCurrency]
                        //   ,[IsLeaf]
                        
                        //from TKS_FAS_GLBalance where accountId=@AccountId and subjectId=@parentId";
                        //cnn.Execute(sql, new { parentId = parent.Id, AccountId = user.AccountId, SubjectId= request.Data.Id, SubjectCode= request.Data.Code, Name= request.Data.Name }, ts);
                        string updateGLBalance = @"update TKS_FAS_GLBalance set  [NUMStartBAL]=0
                           ,[BWBStartBAL]=0
                           ,[YBStartBAL]=0
                           ,[NUMDebitTotal]=0
                           ,[NUMDebitTotal_Y]=0
                           ,[BWBDebitTotal]=0
                           ,[BWBDebitTotal_Y]=0
                           ,[YBDebitTotal]=0
                           ,[YBDebitTotal_Y]=0
                           ,[NUMCreditTotal]=0
                           ,[NUMCreditTotal_Y]=0
                           ,[BWBCreditTotal]=0
                           ,[BWBCreditTotal_Y]=0
                           ,[YBCreditTotal]=0
                           ,[YBCreditTotal_Y]=0
                           ,[ECredit_Debit]=0
                           ,[NUMEndBAL]=0
                           ,[BWBEndBAL]=0
                           ,[YBEndBAL]=0
                           ,[YearStartNumBAL]=0
                           ,[YearStartYBBAL]=0
                           ,[YearStartBWBBAL]=0
                            where SubjectId=@ParentId and AccountId=@AccountId";
                        cnn.Execute(updateGLBalance,
                      new { ParentId = parent.Id, AccountId = user.AccountId }, ts);
                    }
                    #endregion
                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectAdd;
                }
            }
        }
        public ResponseSubjectAdd CheckSubject(RequestSubjectAdd request)
        {
            ResponseSubjectAdd response = new ResponseSubjectAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var parent = GetParent(request.Data.ParentId, ts);
                    var SubjectAss = cnn.Query(@"select * from TKS_FAS_SubjectAss where AccountId=@AccountId and ParentId=@ParentId", new { ParentId = parent.Id, AccountId = user.AccountId }, ts);
                    if (SubjectAss.Count() > 0)
                    {
                        throw new NormalException("上级科目存在辅助核算，请先删除上级科目的辅助核算");
                    }
                    var FGLBalance = cnn.Query(@"select * from TKS_FAS_FGLBalance where AccountId=@AccountId and ParentId=@ParentId", new { ParentId = parent.Id, AccountId = user.AccountId }, ts);
                    if (FGLBalance.Count() > 0)
                    {
                        throw new NormalException("上级科目的辅助核算已被使用，无法添加下级科目");
                    }
                    var DocDetail = cnn.Query(@"select * from TKS_FAS_DocDetail where AccountId=@AccountId and SubjectCode=@SubjectCode", new { SubjectCode = parent.Code, AccountId = user.AccountId }, ts);
                    response.Message = "";
                    if (DocDetail.Count() > 0)
                    {
                        response.Message = "上级科目中已有凭证，保存后新增科目将替代凭证中的上级科目，您要继续吗？";
                    }
                    var GLBalance = cnn.Query(@"select * from TKS_FAS_GLBalance where AccountId=@AccountId and SubjectId=@SubjectId and BWBStartBAL <> 0", new { SubjectId = parent.Id, AccountId = user.AccountId }, ts);
                    if (GLBalance.Count() > 0)
                    {
                        response.Message = "上级科目在期初中已有值，保存后新增科目将替代期初中的上级科目，您要继续吗？";
                    }
                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectAdd;
                }
            }
        }
        private void SetBAL(TKS_FAS_AccountSubject data, IDbTransaction ts)
        {
            AccountSetBLL accountBll = new FAS.AccountSetBLL(cnn);
            var flag = accountBll.IsFirstPeriodPay(data.AccountId, ts);

            if (flag)
            {
                //第一次结转后，无法更改余额表的状态
                return;
            }

            BALBLL bll = new FAS.BALBLL(cnn);
            if (data.IsCalHelperValid == 1)
            {
                bll.SetCalHelper(data, ts);
            }
            else
            {
                bll.UnSetCalHelper(data, ts);
            }
            if (data.IsQuantityValid == 1)
            {
                bll.SetQuantity(data, ts);
            }
            else
            {
                bll.UnSetQuantity(data, ts);
            }

            if (data.IsCurrencyValid == 1)
            {
                //获取科目的币别
                var currency = cnn.Query<TKS_FAS_SubjectAss>(
                    @"select A.id,a.parentId,a.accountId,a.value, b.code from TKS_FAS_SubjectAss A left join 
                TKS_FAS_Currency B on A.code=b.id  where type=1 
                and  a.parentId=@ParentId and a.accountId=@AccountId",
                    new { ParentId = data.Id, AccountId = data.AccountId }, ts).Select(p => p.Code).ToList();
                bll.SetCurrency(data, currency, ts);
            }
            else
            {
                bll.UnSetCurrency(data, ts);
            }
            //add by Hero.Zhang 将该科目的父级科目状态改变
            var GLBalance = cnn.Query<TKS_FAS_GLBalance>(
                    @"select * from TKS_FAS_GLBalance where accountId=@AccountId and subjectId=@subjectId and PeriodId = ''",
                    new { subjectId = data.ParentId, AccountId = data.AccountId }, ts).ToList();
            if (GLBalance != null && GLBalance.Count > 0)
            {
                foreach (var item in GLBalance)
                {
                    cnn.Execute("update TKS_FAS_GLBalance set isLeaf=0 where PeriodId='' and accountId=@AccountId and subjectId=@subjectId",
                       new { subjectId = data.ParentId, AccountId = data.AccountId }, ts);
                }
            }
        }

        /// <summary>
        /// 设置币别
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private string SetCurrency(List<string> currency, string subjectId, IDbTransaction ts)
        {
            string name = string.Empty;
            cnn.Execute("delete from TKS_FAS_SubjectAss where parentid=@ParentId and type=1",
            new { ParentId = subjectId }, ts);

            var inId = "'" + string.Join("','", currency.ToArray()) + "'";
            string sql = "select * from TKS_FAS_Currency where id in ({0})";
            sql = string.Format(sql, inId);
            var list = cnn.Query<TKS_FAS_Currency>(sql, null, ts).ToList();
            foreach (var item in list)
            {
                TKS_FAS_SubjectAss newData = new TKS_FAS_SubjectAss();
                newData.ParentId = subjectId;
                newData.Id = Guid.NewGuid().ToString();
                newData.AccountId = item.AccountId;
                newData.Code = item.Id;
                newData.Value = item.Name;
                newData.Type = 1;
                name += item.Name + "/";
                cnn.Insert<TKS_FAS_SubjectAss>(newData, ts);
            }

            return name;

        }

        /// <summary>
        /// 设置辅助核算
        /// </summary>
        /// <param name="calItem"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private string SetCalItem(List<string> calItem, string subjectId, IDbTransaction ts)
        {
            string name = string.Empty;

            cnn.Execute("delete from TKS_FAS_SubjectAss where parentid=@ParentId and type=2",
                new { ParentId = subjectId }, ts);

            var inId = "'" + string.Join("','", calItem.ToArray()) + "'";
            string sql = "select * from TKS_FAS_CaculateHelperItem where id in ({0})";
            sql = string.Format(sql, inId);
            var list = cnn.Query<TKS_FAS_CaculateHelperItem>(sql, null, ts).ToList();
            foreach (var item in list)
            {
                TKS_FAS_SubjectAss newData = new TKS_FAS_SubjectAss();
                newData.ParentId = subjectId;
                newData.Id = Guid.NewGuid().ToString();
                newData.AccountId = item.AccountId;
                newData.Code = item.Id;
                newData.Value = item.Title;
                newData.Type = 2;
                name += item.Title + "/";
                cnn.Insert<TKS_FAS_SubjectAss>(newData, ts);
            }

            return name;
        }

        private void SetParentNotLeaf(string parentId, IDbTransaction ts)
        {
            string sql = @"update TKS_FAS_AccountSubject set isLeaf=0 where id=@Id";
            cnn.Execute(sql, new { Id = parentId }, ts);
        }

        private void SetParentLeaf(string parentId, IDbTransaction ts)
        {
            string sql = @"select * from TKS_FAS_AccountSubject where parentid=@ParentId";

            var child = cnn.Query<TKS_FAS_AccountSubject>(sql, new { ParentId = parentId }, ts);
            if (child.Count() == 0)
            {
                cnn.Execute("update TKS_FAS_AccountSubject set IsLeaf=1 where id=@Id", new { Id = parentId }, ts);
            }
        }

        private string IdByLevel(int level)
        {
            var code = "";
            if (level == 1)
            {
                code = "001";
            }
            else
            {
                code = "01";
            }
            return code;
        }

        private TKS_FAS_AccountSubject GetParent(string parentId, IDbTransaction ts)
        {
            string sql = @"select * from TKS_FAS_AccountSubject where id=@Id";
            var items = cnn.Query<TKS_FAS_AccountSubject>(sql, new { Id = parentId }, ts).ToList();
            return items[0];
        }

        public ResponseSubjectUpdate SubjectUpdate(RequestSubjectUpdate request)
        {
            ResponseSubjectUpdate response = new ResponseSubjectUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    var flag = IsSubjectUse(request.Data.Id, user, ts);
                    if (flag)
                    {
                        throw new NormalException("科目已经使用，无法更改");
                    }
                    //request.Data.ShowTitle = parent.ShowTitle + "-" + request.Data.Name.Trim();
                    var oldsubject = cnn.QueryFirst<TKS_FAS_AccountSubject>("select * from TKS_FAS_AccountSubject where id=@Id",
                new { Id = request.Data.Id }, ts);
                    string ShowTitle = oldsubject.ShowTitle;
                    string[] strShowTitle = oldsubject.ShowTitle.Split('-');
                    if (strShowTitle.Length>1)
                    {
                        ShowTitle = strShowTitle[0] + "-" + request.Data.Name;
                    }
                    request.Data.ShowTitle = ShowTitle;
                    string sql = @"UPDATE TKS_FAS_AccountSubject
                           SET  
                              
                               IsValid = @IsValid
                              ,IsCalHelperValid = @IsCalHelperValid
                              ,IsQuantityValid = @IsQuantityValid
                              ,IsCurrencyValid =@IsCurrencyValid
                           
                              ,CalHelperValue = @CalHelperValue
                              ,QuantityValue = @QuantityValue
                              ,CurrencyValue = @CurrencyValue
                              ,Name=@Name
                              ,ShowTitle=@ShowTitle
                         WHERE
                          id=@Id";

                    if (request.Data.IsCurrencyValid == 1)
                    {
                        request.Data.CurrencyValue = SetCurrency(request.Currency, request.Data.Id, ts);
                    }
                    else
                    {
                        cnn.Execute("delete from TKS_FAS_SubjectAss where parentid=@ParentId and type=1",
                             new { ParentId = request.Data.Id }, ts);
                        request.Data.CurrencyValue = "";
                    }

                    if (request.Data.IsCalHelperValid == 1)
                    {
                        request.Data.CalHelperValue = SetCalItem(request.CalItem, request.Data.Id, ts);
                    }
                    else
                    {
                        cnn.Execute("delete from TKS_FAS_SubjectAss where parentid=@ParentId and type=2",
                           new { ParentId = request.Data.Id }, ts);
                        request.Data.CalHelperValue = "";
                    }


                    var r = cnn.Execute(sql, request.Data, ts);

                    #region 设置余额表的状态

                    var subject = cnn.QuerySingle<TKS_FAS_AccountSubject>("select * from TKS_FAS_AccountSubject where id=@Id",
                        new { Id = request.Data.Id }, ts);
                    SetBAL(subject, ts);
                    #endregion

                    if (r == 1)
                    {
                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "更新成功";
                        return response;
                    }
                    else
                    {

                        throw new AppException(user.User.UserName, "SubjectUpdate", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectUpdate;
                }

            }
        }

        /// <summary>
        /// 科目是否已经使用
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        private bool IsSubjectUse(string subjectId, MM_UserInfo user, IDbTransaction ts)
        {
            var subject = cnn.QueryFirst<TKS_FAS_AccountSubject>("select * from TKS_FAS_AccountSubject where id=@Id",
                new { Id = subjectId }, ts);

            var docs = cnn.Query("select * from TKS_FAS_DocDetail where accountId=@AccountId and subjectCode=@SubjectCode",
                new { AccountId = user.AccountId, SubjectCode = subject.Code }, ts);

            if (docs.Count() > 0)
                return true;
            else

                return false;
        }

        public ResponseSubjectDelete SubjectDelete(RequestSubjectDelete request)
        {
            ResponseSubjectDelete response = new ResponseSubjectDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    var flag = IsSubjectUse(request.Data.Id, user, ts);
                    if (flag)
                    {
                        throw new NormalException("科目已经使用，无法删除");
                    }
                    var GLBalance = cnn.Query<TKS_FAS_GLBalance>("select * from TKS_FAS_GLBalance where accountId=@AccountId and subjectId=@subjectId",
               new { AccountId = user.AccountId, subjectId = request.Data.Id }, ts).FirstOrDefault();

                    if (GLBalance != null)
                    {
                        if (GLBalance.BWBStartBAL != 0 || GLBalance.BWBDebitTotal_Y != 0 || GLBalance.BWBCreditTotal_Y != 0)
                        {
                            throw new NormalException("科目已经在期初中使用，无法删除");
                        }
                    }

                    var curSubject = cnn.QuerySingle<TKS_FAS_AccountSubject>(
                    "select * from TKS_FAS_AccountSubject where id=@Id", new { Id = request.Data.Id }, ts);

                    AccountSetBLL bll = new AccountSetBLL(cnn);
                    var period = bll.GetFirstPeriod(user.AccountId, ts);
                    //期初里有
                    if (curSubject.Code == curSubject.RootCode)
                    {
                        //删除的当前科目是最高父节点
                        var data1 = cnn.Query(@"delete  from TKS_FAS_GLBalance 
                                where accountId=@AccountId and subjectCode=@SubjectCode and periodId=''",

                                       new
                                       {
                                           AccountId = user.AccountId,
                                           SubjectCode = curSubject.Code
                                       }, ts);
                    }
                    else
                    {
                        //add by Hero.Zhang 删除无期间的余额表，同时更新该余额行的父级节点状态
                        var lstSubject = cnn.Query<TKS_FAS_GLBalance>(@"select * from TKS_FAS_GLBalance where accountId=@AccountId and subjectId=@subjectId and PeriodId = ''",
               new { AccountId = user.AccountId, subjectId = curSubject.ParentId }, ts).ToList();
                        if (lstSubject != null && lstSubject.Count > 0)
                        {
                            //删除的当前科目行存在父级科目，判断该父级科目下有多少子科目，若删除该行后，还存在子科目，状态维持不变，否则变更状态为1（可编辑）
                            var lstChild = cnn.Query<TKS_FAS_GLBalance>(@"select * from TKS_FAS_GLBalance where accountId=@AccountId and ParentId=@ParentId and PeriodId = ''",
               new { AccountId = user.AccountId, ParentId = curSubject.ParentId }, ts).ToList();
                            if (lstChild != null && lstChild.Count == 1)
                            {
                                //删除的这行是最后一个子科目，改变父科目余额表状态
                                cnn.Execute("update TKS_FAS_GLBalance set isLeaf=1 where PeriodId='' and accountId=@AccountId and subjectId=@subjectId",
                                  new { subjectId = curSubject.ParentId, AccountId = user.AccountId }, ts);
                            }

                        }


                        var data1 = cnn.Query(@"delete  from TKS_FAS_GLBalance 
                                where accountId=@AccountId and subjectCode=@SubjectCode and periodId=''",

                                       new
                                       {
                                           AccountId = user.AccountId,
                                           SubjectCode = curSubject.Code
                                       }, ts);
                    }

                    var data = cnn.Query(@"delete  from TKS_FAS_GLBalance 
                                where accountId=@AccountId and subjectCode=@SubjectCode and periodId=@PeriodId",

                                        new
                                        {
                                            AccountId = user.AccountId,
                                            SubjectCode = curSubject.Code,
                                            PeriodId = period.Id
                                        }, ts);




                    cnn.Execute("delete from tks_fas_AccountSubject where id=@Id", request.Data, ts);

                    cnn.Execute("delete from TKS_FAS_SubjectAss where parentId=@ParentId",
                        new { ParentId = request.Data.Id }, ts);



                    SetParentLeaf(curSubject.ParentId, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectDelete;
                }
            }
        }

        public ResponseSubjectUIInit SubjectUIInit(RequestSubjectUIInit request)
        {
            ResponseSubjectUIInit response = new ResponseSubjectUIInit();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    if (!string.IsNullOrEmpty(request.ParentId))
                    {
                        var data = cnn.QuerySingle<TKS_FAS_AccountSubject>(
                        "select * from TKS_FAS_AccountSubject where Id=@Id", new { Id = request.ParentId }, ts);
                        response.ParentSubject = data;
                    }


                    var currency = cnn.Query<TKS_FAS_CurrencyExt>(
                        @"select * from TKS_FAS_Currency where accountId=@AccountId",
                        new { AccountId = user.AccountId }, ts).ToList();

                    var calItem = cnn.Query<TKS_FAS_CaculateHelperItemExt>(
                        @"select * from TKS_FAS_CaculateHelperItem where accountId=@AccountId",
                        new { AccountId = user.AccountId }, ts).ToList();

                    ts.Commit();

                    response.IsSuccess = true;
                    response.Message = "初始化完成";
                    response.CalItem = calItem;
                    response.Currency = currency;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectUIInit;
                }
            }
        }


        /// <summary>
        /// 获取科目的辅助核算信息以及对应的数据源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseSubjectAssGet SubjectAssGet(RequestSubjectAssGet request)
        {
            ResponseSubjectAssGet response = new ResponseSubjectAssGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var head = cnn.Query<TKS_FAS_AccountSubject>(
                        @"select * from TKS_FAS_AccountSubject where code=@Code and AccountId=@AccountId  order by code",
                        new { Code = request.Id, AccountId = user.AccountId }, ts).ToList();
                    if (head.Count == 0)
                    {
                        throw new AppException(user.User.UserName, "ResponseSubjectAssGet", "科目信息不存在",
                            "科目代码：" + request.Id + ",在账套ID:" + user.AccountId + "中不存在");
                    }

                    var subject = head[0];

                    var subjectAss = cnn.Query<TKS_FAS_SubjectAss>(
                        @"select * from TKS_FAS_SubjectAss where parentId=@ParentId and accountId=@AccountId order by code",
                        new { ParentId = subject.Id, AccountId = user.AccountId }, ts).ToList();

                    List<SubjectAssSource> ls1 = new List<SubjectAssSource>();
                    List<SubjectAssSource> ls2 = new List<SubjectAssSource>();

                    foreach (var item in subjectAss)
                    {

                        if (item.Type == 1)
                        {
                            SubjectAssSource newItem = new SubjectAssSource();
                            var currency = cnn.QuerySingle<TKS_FAS_Currency>(
                                @"select * from TKS_FAS_Currency where id=@Id and accountId=@AccountId",
                                new { Id = item.Code, AccountId = user.AccountId }, ts);
                            newItem.Item.Code = currency.Code;
                            newItem.Item.Value = currency.Name;
                            newItem.Rate = currency.ExchangeRate;
                            ls1.Add(newItem);
                        }
                        else
                        {
                            var calHelper = cnn.Query<TKS_FAS_CaculateHelperItem>(
                                @"select * from TKS_FAS_CaculateHelperItem where id=@Id and accountId=@AccountId",
                                new { Id = item.Code, AccountId = user.AccountId }, ts).ToList();

                            foreach (var cal in calHelper)
                            {
                                SubjectAssSource newItem = new SubjectAssSource();
                                newItem.Item.Code = cal.Id;
                                newItem.Item.Value = cal.Title;

                                var data = cnn.Query<TKS_FAS_CaculateHelperDetail>(
                                    @"select * from TKS_FAS_CaculateHelperDetail where 
                                parentId=@ParentId and accountId=@AccountId  order by code",
                                    new { ParentId = cal.Id, AccountId = user.AccountId }, ts).ToList();
                                newItem.Source = GetSource(data);
                                ls2.Add(newItem);
                            }

                        }

                    }
                    if (ls1.Count > 0)
                    {
                        ls1[0].Item.Select = 1;
                    }

                    response.Code = subject.Code;
                    response.Name = subject.ShowTitle;
                    response.IsCalHelperValid = subject.IsCalHelperValid == 1 ? true : false;
                    response.IsCurrencyValid = subject.IsCurrencyValid == 1 ? true : false;
                    response.IsQuantityValid = subject.IsQuantityValid == 1 ? true : false;
                    response.QuantityValue = subject.QuantityValue;
                    response.CalHelper = ls2;
                    response.Currency = ls1;
                    response.IsSuccess = true;
                    response.Message = "加载完成";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSubjectAssGet;
                }
            }
        }

        private List<CodeValue> GetSource(List<TKS_FAS_CaculateHelperDetail> data)
        {
            List<CodeValue> result = new List<CodeValue>();
            foreach (var item in data)
            {
                result.Add(new CodeValue
                {
                    Code = item.Code,
                    Value = item.Name
                });
            }
            return result;
        }


    }
}
