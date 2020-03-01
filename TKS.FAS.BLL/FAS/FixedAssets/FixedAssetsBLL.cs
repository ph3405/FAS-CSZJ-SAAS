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
    public class FixedAssetsBLL : CommonBase
    {
        public FixedAssetsBLL()
        {
        }
        public FixedAssetsBLL(IDbConnection cnn)
        {
            this.cnn = cnn;
        }

        /// <summary>
        /// 只能查询当前期间下的信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseFixedAssetsListSearch FixedAssetsListSearch(RequestFixedAssetsListSearch request)
        {

            ResponseFixedAssetsListSearch response = new ResponseFixedAssetsListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    AccountSetBLL account = new AccountSetBLL(cnn);
                    var user = this.UserInfoGet(request.Token, ts);
                    var period = account.GetActivePeriod(user.AccountId, ts);
                    var PeriodDate = request.PeriodDate.Split('-');
                    var condition = new
                    {
                        AccountId = user.AccountId,
                        PeriodId= request.PeriodId,
                        PeriodDate= PeriodDate[0]+ PeriodDate[1].PadLeft(2,'0'),
                        Year = PeriodDate[0],
                        Month= PeriodDate[1],
                        Name = "%" + request.Name + "%"

                    };

                    string sql = this.GetPageSql("a.*,a.InitialAssetValue-a.AccumulativeDpre as ZCJZ,log.Amount",
                        @" TKS_FAS_FixedAssets a left join(select distinct FixedId,Amount from TKS_FAS_FixedAssetsLog where Amount>0 and  accountId=@AccountId and PeriodId=@PeriodId) log on a.id=log.FixedId  where a.accountId=@AccountId and 
                         a.name like @Name and convert(nvarchar(6),a.StartUseDate,112)<=@PeriodDate",
                        " a.DocNo",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_FixedAssetsExt> data = cnn.Query<TKS_FAS_FixedAssetsExt>(sql,
                       condition, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_FixedAssets  where 
                    accountId=@AccountId and   name like @Name and convert(nvarchar(6),StartUseDate,112)<=@PeriodDate";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, condition, ts).ToString());

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

                    return this.DealException(response, ex) as ResponseFixedAssetsListSearch;
                }
            }
        }

        public ResponseFixedAssetsChangeListSearch FixedAssetsChangeListSearch(RequestFixedAssetsChangeListSearch request)
        {

            ResponseFixedAssetsChangeListSearch response = new ResponseFixedAssetsChangeListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    AccountSetBLL account = new AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);

                    var condition = new
                    {
                        AccountId = user.AccountId,

                        Name = "%" + request.Name + "%"

                    };

                    string sql = this.GetPageSql(@" A.*,  convert(varchar(20),B.Year)+'年'+ convert(varchar(20),B.Month)+'期'  as period 
                        ,C.tplId,ISCHUZHI=(SELECT 'Y' from TKS_FAS_FixedAssetsChange where ChangeType=8 and A.ParentId=ParentId)",
                        @" TKS_FAS_FixedAssetsChange A left join tks_fas_monthPeriodInfo B on
                            A.periodId=B.id left join tks_fas_changeType2TPL C on A.changeType=C.changeType
                       
                            where A.accountId=@AccountId   
                         and A.assetsname like @Name ",
                        " createDate  desc ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_FixedAssetsChangeExt> data = cnn.Query<TKS_FAS_FixedAssetsChangeExt>(sql,
                       condition, ts).ToList();

                    string countSql = @"select count(1) from TKS_FAS_FixedAssetsChange  where 
                    accountId=@AccountId and     assetsname like @Name";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, condition, ts).ToString());

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

                    return this.DealException(response, ex) as ResponseFixedAssetsChangeListSearch;
                }
            }
        }


        public ResponseFixedAssetsGet FixedAssetsGet(RequestFixedAssetsGet request)
        {
            ResponseFixedAssetsGet response = new ResponseFixedAssetsGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGet(request.Token, ts);
                    AccountSetBLL account = new AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);
                    var data = cnn.QuerySingle<TKS_FAS_FixedAssets>("select * from TKS_FAS_FixedAssets where id=@Id", new
                    {
                        Id = request.Id
                    }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;
                    response.NowPeriod = period.Id;

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFixedAssetsGet;
                }
            }
        }

        /// <summary>
        /// 新增固定资产，都是在当前的期间下操作
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseFixedAssetsAdd FixedAssetsAdd(RequestFixedAssetsAdd request)
        {
            ResponseFixedAssetsAdd response = new ResponseFixedAssetsAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    AccountSetBLL account = new AccountSetBLL(cnn);
                    var period = account.GetActivePeriod(user.AccountId, ts);
                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.AccountId = user.AccountId;
                    request.Data.StartPeriod = period.Id;//当前期间为录入期间
                    request.Data.CreateUser = user.User.UserName;
                    request.Data.CreateDate = DateTime.Now;
                    cnn.Insert<TKS_FAS_FixedAssets>(request.Data, ts);
                    if (request.Data.AddType != "期初")
                    {
                        AddChange(request.Data, ts);
                    }


                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFixedAssetsAdd;
                }
            }
        }

        private void AddChange(TKS_FAS_FixedAssets data, IDbTransaction ts)
        {
            TKS_FAS_FixedAssetsChange change = new TKS_FAS_FixedAssetsChange();
            change.Id = Guid.NewGuid().ToString("N");
            change.ParentId = data.Id;
            change.DocNo = data.DocNo;
            change.PeriodId = data.StartPeriod;
            change.AccountId = data.AccountId;
            change.AssetsName = data.Name;
            change.PreContent = data.InitialAssetValue.ToString();
            change.CurContent = data.InitialAssetValue.ToString();
            change.ChangeType = 1;
            change.CreateUser = data.CreateUser;
            change.CreateDate = data.CreateDate;
            cnn.Insert<TKS_FAS_FixedAssetsChange>(change, ts);
        }
        public ResponseFixedAssetsIsGenPZ IsGenPZ(RequestFixedAssetsIsGenPZ request)
        {
            var res = new ResponseFixedAssetsIsGenPZ();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);

                    var data = cnn.Query<TKS_FAS_FixedAssets>(
                        "select * from TKS_FAS_FixedAssets where AccountId=@AccountId and isGenPZ=0 and AddType<>'期初'", new { AccountId = user.AccountId }, ts);
                    if (data.Count() > 0)
                    {
                        res.IsSuccess = false;
                        res.Message = "存在未生成凭证的固定资产";
                    }
                    else
                    {
                        res.IsSuccess = true;
                        res.Message = "ok";
                    }
                    return res;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseFixedAssetsIsGenPZ;
                }
            }

        }
        public ResponseFixedAssetsUpdate FixedAssetsUpdate(RequestFixedAssetsUpdate request)
        {
            ResponseFixedAssetsUpdate response = new ResponseFixedAssetsUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    //更新固定资产前，先检查是否存在变更记录，若存在，先更新变更记录
                    var check = "select * from TKS_FAS_FixedAssetsChange where ParentId=@id";
                    var dataUpdate = cnn.Query(check, new { id = request.Data.Id}, ts);
                    if (dataUpdate.Count()>0)
                    {
                        //add by Hero.Zhang如果购入类型是期初，则删除变更记录
                        if (request.Data.AddType == "期初")
                        {
                            cnn.Execute(@"delete from TKS_FAS_FixedAssetsChange where ParentId=@id", new { id = request.Data.Id }, ts);
                        }
                        else
                        {
                            //获取更新前固定资产
                            var data = cnn.QueryFirst<TKS_FAS_FixedAssets>("select * from TKS_FAS_FixedAssets where id=@Id", new { Id = request.Data.Id }, ts);
                            if (data != null)
                            {
                                string updateSql = @"update TKS_FAS_FixedAssetsChange 
                                             set DocNo = @DocNo,
                                                 AssetsName = @AssetsName,
                                                 PreContent = @PreContent,
                                                 CurContent = @CurContent where ParentId=@Id";
                                cnn.Execute(updateSql, new { DocNo = data.DocNo, AssetsName = data.Name, PreContent = request.Data.InitialAssetValue, CurContent = request.Data.InitialAssetValue, Id = request.Data.Id }, ts);
                            }
                        }
                    }
                    else
                    {
                        //add by Hero.Zhang
                        //如果不存在变更记录，判断是否是期初购入类型，不是期初则新增变更记录
                        if (request.Data.AddType != "期初")
                        {
                            AccountSetBLL account = new AccountSetBLL(cnn);
                            var period = account.GetActivePeriod(user.AccountId, ts);
                            request.Data.AccountId = user.AccountId;
                            request.Data.StartPeriod = period.Id;//当前期间为录入期间
                            request.Data.CreateUser = user.User.UserName;
                            request.Data.CreateDate = DateTime.Now;
                            AddChange(request.Data, ts);
                        }
                    }
                    string sql = @"UPDATE  TKS_FAS_FixedAssets
   SET  
       DocNo = @DocNo
      ,Name = @Name
      ,AddType = @AddType
      ,AssetsClass = @AssetsClass
      ,SpecificationType = @SpecificationType
      ,StartUseDate = @StartUseDate
      ,UseDeptId = @UseDeptId
      ,UseDeptName = @UseDeptName
      ,Supplier = @Supplier
      ,DepreciationMethod = @DepreciationMethod
      ,IsStartPeriodDepreciation = @IsStartPeriodDepreciation
      ,ADSubjectCode = @ADSubjectCode
      ,ADSubjectName = @ADSubjectName
      ,DCostSubjectCode = @DCostSubjectCode
      ,DCostSubjectName = @DCostSubjectName
      ,AssetImpairmentSubjectCode = @AssetImpairmentSubjectCode
      ,AssetImpairmentSubjectName = @AssetImpairmentSubjectName
      ,InitialAssetValue = @InitialAssetValue
      ,ScrapValueRate = @ScrapValueRate
      ,ScrapValue = @ScrapValue
      ,PreUseMonth = @PreUseMonth
      ,DpreMonth = @DpreMonth
      ,RemainderUseMonth = @RemainderUseMonth
      ,AccumulativeDpre = @AccumulativeDpre
      ,AccumulativeDpre_Y = @AccumulativeDpre_Y
      ,PreviousAccumulativeDpre = @PreviousAccumulativeDpre
      ,DprePerMonth = @DprePerMonth
      ,GDCode=@GDCode
      ,GDName=@GDName
      ,InputVAT=@InputVAT
      ,Memo = @Memo 
      
 WHERE 
       Id=@Id";
                    cnn.Execute(sql, request.Data, ts);
                    ts.Commit();

                    response.IsSuccess = true;

                    response.Message = "更新成功";



                    return response;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFixedAssetsUpdate;
                }

            }
        }

        public ResponseFixedAssetsDelete FixedAssetsDelete(RequestFixedAssetsDelete request)
        {
            ResponseFixedAssetsDelete response = new ResponseFixedAssetsDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    string sql = "select * from TKS_FAS_FixedAssetsChange where parentId=@ParentId";
                    var data = cnn.Query<TKS_FAS_FixedAssetsChange>(sql, new { ParentId = request.Data.Id }, ts).ToList();
                    bool flag = false;
                    if (data.Count() > 0)
                    {

                        PeriodBLL bll = new PeriodBLL(cnn);
                        foreach (var item in data)
                        {
                            flag = bll.IsPeriodPaid(item.PeriodId, ts);
                            if (flag == true)
                                break;
                        }

                    }

                    if (flag)//已经结转 不允许删除凭证
                    {
                        throw new NormalException("固定资产生成的凭证已经期末结转，不允许删除");
                    }
                    else
                    {
                        sql = @" delete from TKS_FAS_FixedAssets where id=@Id";

                        cnn.Execute(sql, request.Data, ts);

                        sql = "delete from TKS_FAS_FixedAssetsChange where parentId=@ParentId";
                        cnn.Execute(sql, new { ParentId = request.Data.Id }, ts);

                        foreach (var item in data)
                        {
                            sql = "delete from TKS_FAS_Doc where id=@Id";
                            cnn.Execute(sql, new { Id = item.DocId }, ts);

                            sql = "delete from TKS_FAS_DocDetail where parentId=@ParentId";
                            cnn.Execute(sql, new { ParentId = item.DocId }, ts);

                        }
                        //add by Hero.Zhang删除对应的log 
                        sql = @" delete from TKS_FAS_FixedAssetsLog where FixedId=@FixedId";

                        cnn.Execute(sql, new { FixedId = request.Data.Id }, ts);

                    }


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFixedAssetsDelete;
                }
            }
        }

        /// <summary>
        /// 处置资产
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseFixedAssetsDeal FixedAssetsDeal(RequestFixedAssetsDeal request)
        {
            ResponseFixedAssetsDeal response = new ResponseFixedAssetsDeal();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = UserInfoGet(request.Token, ts);

                    AccountSetBLL bll = new AccountSetBLL(cnn);
                    var period = bll.GetActivePeriod(user.AccountId, ts);

                    string sql = "select * from TKS_FAS_FixedAssets where isGenPz=1 and id=@Id";
                    var data = cnn.Query<TKS_FAS_FixedAssets>(sql, new { Id = request.Data.Id }, ts).FirstOrDefault();

                    if (data == null)
                    {
                        throw new NormalException("未生成凭证，无法报废");
                    }
                    TKS_FAS_FixedAssetsChange change = new TKS_FAS_FixedAssetsChange();
                    change.Id = Guid.NewGuid().ToString("N");
                    change.ParentId = data.Id;
                    change.DocNo = data.DocNo;
                    change.PeriodId = period.Id;
                    change.AccountId = data.AccountId;
                    change.AssetsName = data.Name;
                    change.PreContent = (data.InitialAssetValue - data.AccumulativeDpre).ToString();
                    change.CurContent = (data.InitialAssetValue - data.AccumulativeDpre).ToString();
                    change.ChangeType = 8;
                    change.CreateUser = data.CreateUser;
                    change.CreateDate = data.CreateDate;
                    cnn.Insert<TKS_FAS_FixedAssetsChange>(change, ts);
                    sql = "update TKS_FAS_FixedAssets set status=1 where Id=@Id";
                    cnn.Execute(sql, new { Id = data.Id }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "处置成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFixedAssetsDeal;
                }
            }
        }


        /// <summary>
        /// 变动记录关联上凭证
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="docPZZ"></param>
        /// <param name="id"></param>
        /// <param name="ts"></param>
        public void ChangeSetPZ(string docId, string docPZZ, string id, IDbTransaction ts)
        {
            string sql = @"update TKS_FAS_FixedAssetsChange set docId=@DocId
                        ,docPZZ=@DocPZZ where id=@Id";

            cnn.Execute(sql, new { DocId = docId, DocPZZ = docPZZ, Id = id }, ts);
        }

        /// <summary>
        /// 凭证是否已经关联上了固定资产
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool IsDocHasChangeRecord(string docId, IDbTransaction ts)
        {
            string sql = @"select * from TKS_FAS_FixedAssetsChange where docId=@DocId";

            var data = cnn.Query(sql, new { DocId = docId }, ts);

            if (data.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 固定资产已经生成凭证，改变凭证状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ts"></param>
        public void SetIsGenPZ(string id, MM_UserInfo user, IDbTransaction ts)
        {
            string sql = @"select * from  TKS_FAS_FixedAssetsChange   where id=@Id";

            var data = cnn.QueryFirst<TKS_FAS_FixedAssetsChange>(sql, new { Id = id }, ts);
            if (data.ChangeType == 1)
            {
                sql = "update TKS_FAS_FixedAssets set isGenPz=1 where Id=@Id";
                cnn.Execute(sql, new { Id = data.ParentId }, ts);
            }
            else if (data.ChangeType == 8)
            {
                //AccountSetBLL account = new AccountSetBLL(cnn);
                //var period = account.GetActivePeriod(user.AccountId, ts);
                //sql = @"SELECT  A.*,b.TPLTarget
                //FROM  TKS_FAS_TPL2PZ A left join TKS_FAS_DocTPL B on A.TPLId =B.Id
                //    where A.accountId=@AccountId and A.PeriodId=@PeriodId and B.tplTarget=3";
                //var _data = cnn.Query(sql, new { AccountId = user.AccountId, PeriodId = period.Id }, ts);

                //sql = @"select * from  TKS_FAS_FixedAssets where accountId=@AccountId and DepreciationMethod='1'";
                //var fdata = cnn.Query(sql, new { AccountId = user.AccountId }, ts);

                //if (_data.Count() == 0 && fdata.Count() > 0)
                //{
                //    throw new NormalException("计提折旧与结账凭证未生成，请生成后再进行报废");
                //}

                //sql = "update TKS_FAS_FixedAssets set status=1 where Id=@Id";
                //cnn.Execute(sql, new { Id = data.ParentId }, ts);
            }
        }

        /// <summary>
        /// 获取计提折旧的固定资产
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="period"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public List<TKS_FAS_FixedAssets> FixedAssetsGetByAcccount(string accountId, TKS_FAS_MonthPeriodInfo period, IDbTransaction ts)
        {

            string sql = @"select parentId from TKS_FAS_FixedAssetsChange 
                        where changeType=8 and accountId=@AccountId and periodId=@PeriodId";
            //已经处置，但是处置期间是当期的固定资产，还需要计提折旧
            var lsChange = cnn.Query<TKS_FAS_FixedAssetsChange>(sql, new {AccountId=accountId,PeriodId=period.Id }, ts).ToList ();
            string where = string.Empty;
            if (lsChange.Count > 0)
            {
                var con = lsChange.Select(p => p.ParentId).ToList();
                where="or ( id in ('"+  string.Join("','", con.ToArray()) + "') )";
            }

            sql = @"select * from tks_fas_fixedAssets where 
                        (DepreciationMethod=1 and status=0 and accountId=@AccountId) "
                +where;
            var data = cnn.Query<TKS_FAS_FixedAssets>(sql, new { AccountId = accountId }, ts).ToList();
            return data;
        }

        /// <summary>
        /// 折旧
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ts"></param>
        public void Depreciation(string accountId, IDbTransaction ts, TKS_FAS_MonthPeriodInfo period)
        {
 
            //update by Hero.Zhang 只计提折旧当前期间和当期之前的固定资产
            var lstPeriod = cnn.Query<TKS_FAS_MonthPeriodInfo>(@"select * from TKS_FAS_MonthPeriodInfo where 
 AccountId=@AccountId and StartDate<=@StartDate", new { AccountId = accountId, StartDate = period.StartDate }, ts).ToList();
            string sql_where = "(";
            foreach (var item in lstPeriod)
            {
                if (sql_where == "(")
                {
                    sql_where += "'" + item.Id + "'";
                }
                else
                {
                    sql_where += ",'" + item.Id + "'";
                }
            }
            sql_where += ")";
            string sql =string.Format(@"select * from tks_fas_fixedAssets where accountId=@AccountId and StartPeriod in {0}", sql_where);
            var data = cnn.Query<TKS_FAS_FixedAssets>(sql, new { AccountId = accountId }, ts).ToList();

            foreach (var item in data)
            {
                if (item.DepreciationMethod == "2") continue;
                if (item.IsStartPeriodDepreciation == 0 && item.StartPeriod == period.Id)
                    continue;
                if (item.DpreMonth < item.PreUseMonth)
                {
                    item.DpreMonth += 1;
                    item.RemainderUseMonth = item.PreUseMonth - item.DpreMonth;
                    item.AccumulativeDpre_Y += item.DprePerMonth;
                    item.AccumulativeDpre += item.DprePerMonth;
                    item.PreviousAccumulativeDpre += item.AccumulativeDpre - item.AccumulativeDpre_Y;

                    sql = @"update TKS_FAS_FixedAssets set DpreMonth=@DpreMonth,
                      RemainderUseMonth=@RemainderUseMonth,
                      AccumulativeDpre_Y=@AccumulativeDpre_Y,
                    AccumulativeDpre=@AccumulativeDpre,
                    PreviousAccumulativeDpre=@PreviousAccumulativeDpre
                    where 
                        id=@Id";

                    cnn.Execute(sql, item, ts);

                    #region log
                    TKS_FAS_FixedAssetsLog log = new TKS_FAS_FixedAssetsLog();
                    log.Id = Guid.NewGuid().ToString();
                    log.AccountId = accountId;
                    log.PeriodId = period.Id;
                    log.FixedId = item.Id;
                    log.Amount = item.DprePerMonth;
                    log.CreateDate = DateTime.Now; 
                    cnn.Insert<TKS_FAS_FixedAssetsLog>(log, ts);
                    #endregion
                }
            }
        }

        /// <summary>
        /// 折旧数据回滚
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ts"></param>
        public void RollbackFixedAssets(string accountId,string PeriodId, IDbTransaction ts)
        {
            AccountSetBLL account = new AccountSetBLL(cnn);
            var period = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                @"select * from TKS_FAS_MonthPeriodInfo where Id=@Id",
                new { Id = PeriodId }, ts);
            //var period = account.GetActivePeriod(accountId, ts);
            var lstPeriod = cnn.Query<TKS_FAS_MonthPeriodInfo>(@"select * from TKS_FAS_MonthPeriodInfo where 
 AccountId=@AccountId and StartDate<=@StartDate", new { AccountId = accountId, StartDate = period.StartDate }, ts).ToList();
            string sql_where = "(";
            foreach (var item in lstPeriod)
            {
                if (sql_where == "(")
                {
                    sql_where += "'" + item.Id + "'";
                }
                else
                {
                    sql_where += ",'" + item.Id + "'";
                }
            }
            sql_where += ")";
            string sql = string.Format(@"select * from tks_fas_fixedAssets where accountId=@AccountId and StartPeriod in {0}", sql_where);
            var data = cnn.Query<TKS_FAS_FixedAssets>(sql, new { AccountId = accountId}, ts).ToList();

            foreach (var item in data)
            {
                if (item.DepreciationMethod == "2") continue;
                if (item.IsStartPeriodDepreciation == 0 && item.StartPeriod == period.Id)
                    continue;
                if (item.DpreMonth <= item.PreUseMonth)
                {
                    //item.DpreMonth += 1;
                    //item.RemainderUseMonth = item.PreUseMonth - item.DpreMonth;
                    //item.AccumulativeDpre_Y += item.DprePerMonth;
                    //item.AccumulativeDpre += item.DprePerMonth;
                    //item.PreviousAccumulativeDpre += item.AccumulativeDpre - item.AccumulativeDpre_Y;
                    item.DpreMonth--;
                    item.PreviousAccumulativeDpre -= item.AccumulativeDpre - item.AccumulativeDpre_Y;
                    item.RemainderUseMonth = item.PreUseMonth - item.DpreMonth;
                    item.AccumulativeDpre_Y -= item.DprePerMonth;
                    item.AccumulativeDpre -= item.DprePerMonth;
                    

                    sql = @"update TKS_FAS_FixedAssets set DpreMonth=@DpreMonth,
                      RemainderUseMonth=@RemainderUseMonth,
                      AccumulativeDpre_Y=@AccumulativeDpre_Y,
                    AccumulativeDpre=@AccumulativeDpre,
                    PreviousAccumulativeDpre=@PreviousAccumulativeDpre
                    where 
                        id=@Id";

                    cnn.Execute(sql, item, ts);
                    #region 更新log
                    sql = @"update TKS_FAS_FixedAssetsLog set Amount=0
                    where 
                        AccountId=@AccountId and FixedId=@FixedId and PeriodId=@PeriodId";

                    cnn.Execute(sql, new { AccountId = accountId, FixedId= item.Id, PeriodId= period.Id }, ts);
                    #endregion
                }
            }
        }
    }
}
