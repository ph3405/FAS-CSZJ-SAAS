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
    public class CustomDataBLL : CommonBase
    {
        public ResponseSearchUIInit SearchUIInit(RequestSearchUIInit request)
        {
            ResponseSearchUIInit response = new ResponseSearchUIInit();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    string sql = @"select * from TKS_FAS_CustomColDes where accountId=@AccountId and parentId=@ParentId order by ColumnCode ";

                    var data = cnn.Query<TKS_FAS_CustomColDes>(sql, new
                    {
                        AccountId = user.AccountId,
                        ParentId = request.Data.Id
                    }, ts).ToList();

                    response.IsSuccess = true;
                    response.Message = "初始化完成";
                    response.Data = data;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseSearchUIInit;
                }
            }
        }

        public ResponseAddUIInit AddUIInit(RequestAddUIInit request)
        {
            ResponseAddUIInit response = new ResponseAddUIInit();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = @"select * from TKS_FAS_CustomColDes
                    where parentId=@ParentId and accountId=@AccountId";

                    var data = cnn.Query<TKS_FAS_CustomColDes>(sql, new
                    {
                        ParentId = request.Head.Id,
                        AccountId = user.AccountId
                    }, ts).ToList();
                    ts.Commit();

                    response.Data = data;
                    response.IsSuccess = true;
                    response.Message = "初始化完成";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseAddUIInit;
                }
            }
        }
        public ResponseCustomDataListSearch CustomDataListSearch(RequestCustomDataListSearch request)
        {

            ResponseCustomDataListSearch response = new ResponseCustomDataListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = this.GetPageSql("*",
                        @" TKS_FAS_CaculateHelperDetail where 
                    accountId=@AccountId and parentId=@ParentId and 
                    (code like @Code or name like @Name)",
                        "Code",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_CaculateHelperDetail> data = cnn.Query<TKS_FAS_CaculateHelperDetail>(sql,
                        new
                        {
                            AccountId = user.AccountId,
                            ParentId = request.Head.Id,
                            Code = "%" + request.Data.Code + "%",
                            Name = "%" + request.Data.Name + "%"
                        }, ts).ToList();

                    string countSql = @"select count(1) from  TKS_FAS_CaculateHelperDetail where 
                        accountId=@AccountId and parentId=@ParentId and 
                            (code like @Code or name like @Name)";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        AccountId = user.AccountId,
                        ParentId = request.Head.Id,
                        Code = "%" + request.Data.Code + "%",
                        Name = "%" + request.Data.Name + "%"
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

                    return this.DealException(response, ex) as ResponseCustomDataListSearch;
                }
            }
        }

        public ResponseCustomDataGet CustomDataGet(RequestCustomDataGet request)
        {
            ResponseCustomDataGet response = new ResponseCustomDataGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    //detail data id-->parentId-->meta data
                    var data = cnn.QuerySingle<TKS_FAS_CaculateHelperDetail>(@"select * from 
                            TKS_FAS_CaculateHelperDetail where id=@Id", new
                    {
                        Id = request.Data.Id
                    }, ts);

                    var head = cnn.QuerySingle<TKS_FAS_CaculateHelperItem>(@"select * from
                    TKS_FAS_CaculateHelperItem where id=@Id", new
                    {
                        Id = data.ParentId
                    }, ts);

                    var customColDes = cnn.Query<TKS_FAS_CustomColDes>(@"select * from
                    TKS_FAS_CustomColDes where parentId=@ParentId", new
                    {
                        ParentId = data.ParentId
                    }, ts).ToList();

                    var customCodeValue = CodeBindValue(customColDes, data);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;
                    response.Head = head;
                    response.CustomDes = customCodeValue;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCustomDataGet;
                }
            }
        }

        private List<TKS_FAS_CustomColDesExt> CodeBindValue(List<TKS_FAS_CustomColDes> customColDes,
            TKS_FAS_CaculateHelperDetail data)
        {
            #region foolish bind
            List<TKS_FAS_CustomColDesExt> result = new List<TKS_FAS_CustomColDesExt>();
            foreach (var item in customColDes)
            {
                TKS_FAS_CustomColDesExt newData = new TKS_FAS_CustomColDesExt();
                newData.Id = item.Id;
                newData.AccountId = item.AccountId;
                newData.ColumnCode = item.ColumnCode;
                newData.ColumnName = item.ColumnName;
                newData.ParentId = item.ParentId;

                if (item.ColumnCode == "1")
                {
                    newData.Value = data.Custom1;
                }
                else if (item.ColumnCode == "2")
                {
                    newData.Value = data.Custom2;
                }
                else if (item.ColumnCode == "3")
                {
                    newData.Value = data.Custom3;
                }
                else if (item.ColumnCode == "4")
                {
                    newData.Value = data.Custom4;
                }
                else if (item.ColumnCode == "5")
                {
                    newData.Value = data.Custom5;
                }
                else if (item.ColumnCode == "6")
                {
                    newData.Value = data.Custom6;
                }
                else if (item.ColumnCode == "7")
                {
                    newData.Value = data.Custom7;
                }
                else if (item.ColumnCode == "8")
                {
                    newData.Value = data.Custom8;
                }

                result.Add(newData);
            }

            return result;
            #endregion
        }

        public ResponseCustomDataAdd CustomDataAdd(RequestCustomDataAdd request)
        {
            ResponseCustomDataAdd response = new ResponseCustomDataAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    Dictionary<string, string> saveData = List2Dic(request.Data);

                    var user = this.UserInfoGet(request.Token, ts);
                    var item = cnn.QueryFirst<Max_CaculateHelperDetailCode>(
                             @"select isnull(max(code),0) Code from TKS_FAS_CaculateHelperDetail
        where accountId=@AccountId and parentId=@ParentId", new
                             {
                                 AccountId = user.AccountId,
                                 ParentId = request.Head.Id
                             }, ts);
                    saveData["Code"] = (item.Code + 1).ToString().PadLeft(3, '0');
                    #region code ,name verification
                    //var d = cnn.Query(@"select * from TKS_FAS_CaculateHelperDetail 
                    //        where code=@Code and accountId=@AccountId and parentId=@ParentId",
                    //    new
                    //    {
                    //        Code = saveData["Code"],
                    //        AccountId = user.AccountId,
                    //        ParentId = request.Head.Id//item id
                    //    }, ts);
                    //if (d.Count() > 0)
                    //{
                    //    throw new NormalException("编码已经存在");
                    //}

                    var d = cnn.Query(@"select * from TKS_FAS_CaculateHelperDetail 
                            where name=@Name and accountId=@AccountId and parentId=@ParentId",
                    new
                    {
                        Name = saveData["Name"],
                        AccountId = user.AccountId,
                        ParentId = request.Head.Id//item id
                    }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("名称已经存在");
                    }
                    #endregion


                    TKS_FAS_CaculateHelperDetail detail = new TKS_FAS_CaculateHelperDetail();

                    detail = LoadByDic(saveData);
                    detail.ParentId = request.Head.Id;
                    detail.AccountId = user.AccountId;
                    cnn.Insert<TKS_FAS_CaculateHelperDetail>(detail, ts);

                    ts.Commit();
                    response.Id = request.Head.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCustomDataAdd;
                }
            }
        }

        public void CustomDataImport(DataTable dt, string token, string itemId)
        {
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(token, ts);
                    AccountSetBLL account = new AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);

                    BalanceBLL balanceBLL = new BalanceBLL(cnn);
                    PeriodBLL periodBLL = new PeriodBLL(cnn);

                    string sql = string.Empty;
                    sql = @"delete from TKS_FAS_CaculateHelperDetail where AccountId=@AccountId and ParentId=@ParentId";
                    cnn.Execute(sql, new { AccountId = user.AccountId, ParentId = itemId }, ts);
                    if (dt.DefaultView.ToTable(true, "Name").Rows.Count < dt.Rows.Count)
                    {
                        throw new NormalException("名称不能重复");
                    }
                    for (var i = 0; i < dt.Rows.Count; i++)
                    {
                        var rowItem = dt.Rows[i];
                        TKS_FAS_CaculateHelperDetail fix = new TKS_FAS_CaculateHelperDetail();
                        fix.Id = Guid.NewGuid().ToString("N");
                        fix.AccountId = user.AccountId;
                        fix.ParentId = itemId;//当前期间为录入期间
                        fix.IsValid = 1;
                        fix.Code = (i + 1).ToString().PadLeft(3, '0');
                        fix.Name = rowItem["Name"].ToString();
                        fix.Memo = rowItem["Memo"].ToString();
                        fix.Custom1 = "#nodata#";
                        fix.Custom2 = "#nodata#";
                        fix.Custom3 = "#nodata#";
                        fix.Custom4 = "#nodata#";
                        fix.Custom5 = "#nodata#";
                        fix.Custom6 = "#nodata#";
                        fix.Custom7 = "#nodata#";
                        fix.Custom8 = "#nodata#";
                        cnn.Insert<TKS_FAS_CaculateHelperDetail>(fix, ts);
                    }
                    ts.Commit();
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw ex;
                }

            }
        }
        private TKS_FAS_CaculateHelperDetail LoadByDic(Dictionary<string, string> saveData)
        {
            TKS_FAS_CaculateHelperDetail data = new TKS_FAS_CaculateHelperDetail();
            data.Id = Guid.NewGuid().ToString();

            data.Code = saveData["Code"];
            data.Name = saveData["Name"];
            data.Memo = saveData["Memo"];
            data.Custom1 = saveData.ContainsKey("Custom1") ? saveData["Custom1"] : "#nodata#";
            data.Custom2 = saveData.ContainsKey("Custom2") ? saveData["Custom2"] : "#nodata#";
            data.Custom3 = saveData.ContainsKey("Custom3") ? saveData["Custom3"] : "#nodata#";
            data.Custom4 = saveData.ContainsKey("Custom4") ? saveData["Custom4"] : "#nodata#";
            data.Custom5 = saveData.ContainsKey("Custom5") ? saveData["Custom5"] : "#nodata#";
            data.Custom6 = saveData.ContainsKey("Custom6") ? saveData["Custom6"] : "#nodata#";
            data.Custom7 = saveData.ContainsKey("Custom7") ? saveData["Custom7"] : "#nodata#";
            data.Custom8 = saveData.ContainsKey("Custom8") ? saveData["Custom8"] : "#nodata#";
            data.IsValid = saveData.ContainsKey("IsValid") ? 1 : 0;
            return data;

        }

        private Dictionary<string, string> List2Dic(List<CodeValue> data)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in data)
            {
                dic.Add(item.Code, item.Value);
            }
            return dic;
        }

        public ResponseCustomDataUpdate CustomDataUpdate(RequestCustomDataUpdate request)
        {
            ResponseCustomDataUpdate response = new ResponseCustomDataUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    Dictionary<string, string> saveData = List2Dic(request.Data);
                    #region code ,name verification
                    var d = cnn.Query(@"select * from TKS_FAS_CaculateHelperDetail 
                            where code=@Code and accountId=@AccountId and parentId=@ParentId",
                        new
                        {
                            Code = saveData["Code"],
                            AccountId = user.AccountId,
                            ParentId = request.Head.Id//item id
                        }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("编码已经存在");
                    }

                    d = cnn.Query(@"select * from TKS_FAS_CaculateHelperDetail 
                            where name=@Name and accountId=@AccountId and parentId=@ParentId",
                    new
                    {
                        Name = saveData["Name"],
                        AccountId = user.AccountId,
                        ParentId = request.Head.Id//item id
                    }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("名称已经存在");
                    }
                    #endregion


                    var item = cnn.QueryFirstOrDefault<TKS_FAS_CaculateHelperDetail>(@"select * from TKS_FAS_CaculateHelperDetail 
                            where id=@id",
                        new
                        {
                            id = request.Id
                        }, ts);
                    string orginalName = item.Name;
                    string upSql = @"update TKS_FAS_FGLBalance set 
                            Name = REPLACE(Name,'@OrignalName','@Name')
                            where CalItem1=@CalItem1 and AccountId=@AccountId AND
                            CalValue1=@CalValue1";
                    cnn.Execute(upSql, new
                    {
                        OrignalName = orginalName,
                        Name = saveData["Name"],
                        CalItem1 = item.ParentId,
                        CalValue1 = item.Code,
                        AccountId = item.AccountId
                    }, ts);


                    string upSql1 = @"update TKS_FAS_DocDetail set 
                            SubjectDescription = REPLACE(SubjectDescription,'" + orginalName + @"','" + saveData["Name"] + @"')
                            where CalItem1='" + item.ParentId + "' and AccountId='" + item.AccountId + @"' AND
                            CalValue1= '" + item.Code + "'";
                    cnn.Execute(upSql1, null, ts);
                    //string upSql1 = @"update TKS_FAS_DocDetail set 
                    //        SubjectDescription = REPLACE(SubjectDescription,'@OrignalName','@Name')
                    //        where CalItem1=@CalItem1 and AccountId=@AccountId AND
                    //        CalValue1=@CalValue1";
                    //var res = cnn.Execute(upSql1, new
                    //{
                    //    OrignalName = orginalName,
                    //    Name = saveData["Name"],
                    //    CalItem1 = item.ParentId,
                    //    CalValue1 = item.Code,
                    //    AccountId = item.AccountId
                    //}, ts);

                    string sql = @"update TKS_FAS_CaculateHelperDetail set 
                            code=@Code,
                            name=@Name,
                            memo=@Memo,
                            custom1=@Custom1,
                            custom2=@Custom2,
                            custom3=@Custom3,
                            custom4=@Custom4,                            
                            custom5=@Custom5,
                            custom6=@Custom6,
                            custom7=@Custom7,
                            custom8=@Custom8,
                            isValid=@IsValid
                        where id=@Id
                            ";

                    var data = LoadByDic(saveData);
                    data.Id = request.Id;
                    int r = cnn.Execute(sql, data, ts);

                    if (r == 1)
                    {
                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "更新成功";
                        return response;
                    }
                    else
                    {

                        throw new AppException(user.User.UserName, "CustomDataUpdate", "数据更新异常", r + "行数据被更新");

                    }



                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCustomDataUpdate;
                }

            }
        }

        public ResponseCustomDataDelete CustomDataDelete(RequestCustomDataDelete request)
        {
            ResponseCustomDataDelete response = new ResponseCustomDataDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var detail = cnn.Query<TKS_FAS_CaculateHelperDetail>(@"select * from TKS_FAS_CaculateHelperDetail where id=@Id", request.Data, ts).FirstOrDefault();
                    if (detail != null)
                    {
                        var doc = cnn.Query(@"select * from TKS_FAS_DocDetail where CalItem1=@CalItem1 and CalValue1=@CalValue1", new { CalItem1 = detail.ParentId, CalValue1 = detail.Code }, ts);
                        if (doc.Count() > 0)
                        {
                            throw new NormalException("辅助核算项已使用，不可删除");
                        }
                    }

                    cnn.Execute("delete from TKS_FAS_CaculateHelperDetail where id=@Id", request.Data, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseCustomDataDelete;
                }
            }
        }
    }
}
