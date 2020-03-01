using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Common;

namespace TKS.FAS.BLL.FAS
{
    public class FormulaBLL:CommonBase
    {
        public ResponseFormulaListSearch FormulaListSearch(RequestFormulaListSearch request)
        {

            ResponseFormulaListSearch response = new ResponseFormulaListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    string where = string.Empty;


                    string sql = this.GetPageSql("A.* ",
                        @"  TKS_FAS_Formula A where ReportDetailTPLId=@ReportDetailTPLId",
                        " A.CreateDate desc  ",
                        request.PageSize,
                        request.PageIndex);

                    List<TKS_FAS_Formula> data = cnn.Query<TKS_FAS_Formula>(sql,
                        new
                        {
                            ReportDetailTPLId = request.ReportDetailTPLId

                        }, ts).ToList();

                    string countSql = @"select count(1) from  TKS_FAS_Formula A where ReportDetailTPLId=@ReportDetailTPLId
                      ";

                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        ReportDetailTPLId = request.ReportDetailTPLId
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
                    return this.DealException(response, ex) as ResponseFormulaListSearch;
                }
            }
        }

        public ResponseFormulaGet FormulaGet(RequestFormulaGet request)
        {
            ResponseFormulaGet response = new ResponseFormulaGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = UserInfoGetButAccount(request.Token, ts);
                    var data = cnn.QuerySingle<TKS_FAS_Formula>("select * from TKS_FAS_Formula where id=@Id", new
                    {
                        Id = request.Data.Id
                    }, ts);


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.Data = data;

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFormulaGet;
                }
            }
        }

        public ResponseFormulaAdd FormulaAdd(RequestFormulaAdd request)
        {
            ResponseFormulaAdd response = new ResponseFormulaAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    var user = this.UserInfoGetButAccount(request.Token, ts);

                   var head= cnn.QueryFirst<TKS_FAS_ReportDetailTPL>("select * from TKS_FAS_ReportDetailTPL where id=@Id",
                        new { Id=request.Data.ReportDetailTPLId},ts);

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.ReportTPLId = head.ParentId;
                    request.Data.AccountId = head.AccountId;
                    request.Data.CreateUser = user.User.UserName;
                    request.Data.CreateDate = DateTime.Now;
                    cnn.Insert<TKS_FAS_Formula>(request.Data, ts);

                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFormulaAdd;
                }
            }
        }


        public ResponseFormulaUpdate FormulaUpdate(RequestFormulaUpdate request)
        {
            ResponseFormulaUpdate response = new ResponseFormulaUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);

                    string sql = @"update TKS_FAS_Formula set
                                subjectCode=@SubjectCode,
                                subjectName=@SubjectName,
                                OperatorCharacter=@OperatorCharacter,
                                ValueRule=@ValueRule
                                where id=@Id";

                    cnn.Execute(sql, request.Data, ts);

                    ts.Commit();

                    response.IsSuccess = true;
                    response.Id = request.Data.Id;
                    response.Message = "更新成功";
                    return response;

                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFormulaUpdate;
                }

            }
        }

        public ResponseFormulaDelete FormulaDelete(RequestFormulaDelete request)
        {
            ResponseFormulaDelete response = new ResponseFormulaDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    string sql = "delete from TKS_FAS_Formula where id=@Id";
                    cnn.Execute(sql, new { Id = request.Data.Id }, ts);


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseFormulaDelete;
                }
            }
        }
    }
}
