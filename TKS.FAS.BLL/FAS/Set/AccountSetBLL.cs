using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Common;
using System.Web;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Web.UI;
using System.Diagnostics;
using System.ComponentModel;
using NPOI.SS.Formula.Functions;
using System.Data.SqlClient;

namespace TKS.FAS.BLL.FAS
{
    public partial class AccountSetBLL : CommonBase
    {

        public string[] tableName = { "TKS_FAS_AccountSubject", "TKS_FAS_Currency", "TKS_FAS_CertificateWord", "TKS_FAS_GLBalance", "TKS_FAS_FGLBalance", "TKS_FAS_CaculateHelperItem", "TKS_FAS_CaculateHelperDetail", "TKS_FAS_FixedAssets", "TKS_FAS_FixedAssetsChange", "TKS_FAS_Doc", "TKS_FAS_DocDetail", "TKS_FAS_MonthPeriodInfo", "TKS_FAS_YearPeriodInfo", "TKS_FAS_PeriodTrack", "TKS_FAS_SubjectAss", "TKS_FAS_FixedAssetsLog", "TKS_FAS_DocTPL", "TKS_FAS_TPL2PZ" };
        public AccountSetBLL() { }
        public AccountSetBLL(IDbConnection _cnn)
        {
            cnn = _cnn;
        }
        public ResponseAccountListSearch AccountListSearch(RequestAccountListSearch request)
        {

            ResponseAccountListSearch response = new ResponseAccountListSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    var Permission = this.GetPermission(request.Token, ts);
                    string sql = this.GetPageSql(@"*,(case when DZ_CreditCode='" + user.Node.CreditCode + @"' then '1' else '0' end ) as createByMe",
                        @"(select  A.*,C.TrueName,(case when B.userid is null then 0 else 1 end) as Active from 
                    TKS_FAS_AccountInfo A left join 
                    (select * from TKS_FAS_UserCurrentAccount where UserId=@UserId) B
                    on A.id=B.accountId left join TKS_FAS_User C on A.AccountantId=C.id
                    where A.qy_name like @QYName 
                    and ( A.DZ_CreditCode=@DZCreditCode
                    or A.dl_creditCode=@DLCreditCode or wb_creditCode=@WB_CreditCode)) MM
                    ",
                        "active desc,createDate desc,updateDate desc",
                        request.PageSize,
                        request.PageIndex);
                    if (Permission.PLevel >= 20)
                    {
                        sql = this.GetPageSql(@"*,(case when DZ_CreditCode='" + user.Node.CreditCode + @"' then '1' else '0' end ) as createByMe",
                        @"(select  A.*,C.TrueName,(case when B.userid is null then 0 else 1 end) as Active from 
                    TKS_FAS_AccountInfo A left join 
                    (select * from TKS_FAS_UserCurrentAccount where UserId=@UserId) B
                    on A.id=B.accountId left join TKS_FAS_User C on A.AccountantId=C.id
                    where A.qy_name like @QYName 
                    and ( A.DZ_CreditCode=@DZCreditCode
                    or A.dl_creditCode=@DLCreditCode or wb_creditCode=@WB_CreditCode) and (A.CreateUser=@CreateUser or A.AccountantId=@AccountantId)) MM
                    ",
                        "active desc,createDate desc,updateDate desc",
                        request.PageSize,
                        request.PageIndex);
                    }
                    if (Permission.PLevel == 1)
                    {
                        //平台管理员，看到所有账套
                        sql = this.GetPageSql(@"*,(case when DZ_CreditCode='" + user.Node.CreditCode + @"' then '1' else '0' end ) as createByMe",
                        @"(select  A.*,C.TrueName,2 as Active from 
                    TKS_FAS_AccountInfo A left join TKS_FAS_User C on A.AccountantId=C.id
                    where A.qy_name like @QYName) MM
                    ",
                        "createDate desc,updateDate desc",
                        request.PageSize,
                        request.PageIndex);
                    }
                    List<TKS_FAS_AccountInfoExt> data = cnn.Query<TKS_FAS_AccountInfoExt>(sql,
                        new
                        {
                            QYName = "%" + request.Data.QY_Name + "%",
                            DZCreditCode = user.Node.CreditCode,
                            DLCreditCode = user.Node.CreditCode,
                            UserId = user.User.Id,
                            WB_CreditCode = user.Node.CreditCode,
                            CreateUser = user.User.TrueName,
                            AccountantId = user.User.Id
                        }, ts).ToList();

                    string countSql = @"select count(1) from tks_fas_AccountInfo where qy_name like @QYName 
                    and ( DZ_CreditCode=@DZCreditCode
                    or dl_creditCode=@DLCreditCode  or wb_creditCode=@WB_CreditCode)";
                    if (Permission.PLevel >= 20)
                    {
                        countSql = @"select count(1) from tks_fas_AccountInfo where qy_name like @QYName 
                    and ( DZ_CreditCode=@DZCreditCode
                    or dl_creditCode=@DLCreditCode  or wb_creditCode=@WB_CreditCode) and CreateUser=@CreateUser";
                    }
                    if (Permission.PLevel == 1)
                    {
                        countSql = @"select count(1) from tks_fas_AccountInfo where qy_name like @QYName ";
                    }
                    int total = int.Parse(cnn.ExecuteScalar(countSql, new
                    {
                        QYName = "%" + request.Data.QY_Name + "%",
                        DZCreditCode = user.Node.CreditCode,
                        DLCreditCode = user.Node.CreditCode,
                        WB_CreditCode = user.Node.CreditCode,
                        CreateUser = user.User.TrueName
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

                    return this.DealException(response, ex) as ResponseAccountListSearch;
                }
            }
        }
        public ResponseBackUpSearch BackUpSearch(RequestBackUpSearch request)
        {

            ResponseBackUpSearch response = new ResponseBackUpSearch();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    string sql = this.GetPageSql("*",
                        @"  TKS_FAS_BackUpInfo where AccountId=@AccountId",
                        " CreateDate desc ",
                        request.PageSize,
                        request.PageIndex);
                    if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
                    {
                        sql = this.GetPageSql("*",
                        @"  TKS_FAS_BackUpInfo where AccountId=@AccountId and CreateDate>=@StartDate and CreateDate<=@EndDate",
                        " CreateDate desc ",
                        request.PageSize,
                        request.PageIndex);
                    }
                    List<TKS_FAS_BackUpInfo> data = cnn.Query<TKS_FAS_BackUpInfo>(sql,
                        new
                        {
                            AccountId = user.AccountId,
                            StartDate = request.StartDate,
                            EndDate = request.EndDate,
                            CreateUser = user.User.Id
                        }, ts).ToList();


                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                    response.PageIndex = request.PageIndex;
                    response.Data = data;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();

                    return this.DealException(response, ex) as ResponseBackUpSearch;
                }
            }
        }

        public ResponseAccountAdd BackUp(RequestAccountAdd request)
        {

            ResponseAccountAdd response = new ResponseAccountAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                    var data_Account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(
                @"select * from TKS_FAS_AccountInfo where Id=@Id",
                new { Id = user.AccountId }, ts);
                    foreach (Process p in Process.GetProcessesByName("Excel"))
                    {
                        if (string.IsNullOrEmpty(p.MainWindowTitle))
                        {
                            p.Kill();
                        }
                    }
                    string sql_p = "select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId order by startDate";
                    var data_p = cnn.Query<TKS_FAS_MonthPeriodInfo>(sql_p, new { AccountId = user.AccountId }, ts).ToList();
                    string periodName = data_p[0].Year.ToString() + "." + data_p[0].Month.ToString() + "-" + data_p[data_p.Count() - 1].Year.ToString() + "." + data_p[data_p.Count() - 1].Month.ToString();
                    string uploadPath = HttpContext.Current.Server.MapPath("/upload/");
                    //string uploadPath = Path.Combine(HttpRuntime.AppDomainAppPath, "upload") + "/";
                    string fileName = user.User.Mobile + "_" + data_Account.QY_Name + "(" + periodName + ")" + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xls";// 文件名称 
                    string xdpath = "/upload/" + "备份/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                    uploadPath += "备份\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;



                    //判断路径是否存在
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    string filePath = uploadPath + "/" + fileName;
                    FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                    IWorkbook workbook = new HSSFWorkbook();

                    for (int i = 0; i < tableName.Length; i++)
                    {

                        string sql = string.Format(@"select * from {0} where accountId='{1}'", tableName[i], user.AccountId);


                        switch (tableName[i])
                        {
                            case "TKS_FAS_AccountSubject":

                                List<TKS_FAS_AccountSubject> data = cnn.Query<TKS_FAS_AccountSubject>(sql, null, ts).ToList();
                                if (data.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_Currency":
                                List<TKS_FAS_Currency> data_Currency = cnn.Query<TKS_FAS_Currency>(sql, null, ts).ToList();
                                if (data_Currency.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_Currency);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_CertificateWord":
                                List<TKS_FAS_CertificateWord> data_CertificateWord = cnn.Query<TKS_FAS_CertificateWord>(sql, null, ts).ToList();
                                if (data_CertificateWord.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_CertificateWord);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_GLBalance":
                                List<TKS_FAS_GLBalance> data_GLBalance = cnn.Query<TKS_FAS_GLBalance>(sql, null, ts).ToList();
                                if (data_GLBalance.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_GLBalance);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                            if (c == 35)
                                            {
                                                row.CreateCell(c).SetCellValue(DateTime.Now.ToShortDateString());
                                            }
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_FGLBalance":
                                List<TKS_FAS_FGLBalance> data_FGLBalance = cnn.Query<TKS_FAS_FGLBalance>(sql, null, ts).ToList();
                                if (data_FGLBalance.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_FGLBalance);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                            if (c == 44)
                                            {
                                                row.CreateCell(c).SetCellValue(DateTime.Now.ToShortDateString());
                                            }
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_CaculateHelperItem":
                                List<TKS_FAS_CaculateHelperItem> data_CaculateHelperItem = cnn.Query<TKS_FAS_CaculateHelperItem>(sql, null, ts).ToList();
                                if (data_CaculateHelperItem.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_CaculateHelperItem);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_CaculateHelperDetail":
                                List<TKS_FAS_CaculateHelperDetail> data_CaculateHelperDetail = cnn.Query<TKS_FAS_CaculateHelperDetail>(sql, null, ts).ToList();
                                if (data_CaculateHelperDetail.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_CaculateHelperDetail);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_FixedAssets":
                                List<TKS_FAS_FixedAssets> data_FixedAssets = cnn.Query<TKS_FAS_FixedAssets>(sql, null, ts).ToList();
                                if (data_FixedAssets.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_FixedAssets);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_Doc":
                                List<TKS_FAS_Doc> data_Doc = cnn.Query<TKS_FAS_Doc>(sql, null, ts).ToList();
                                if (data_Doc.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_Doc);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_DocDetail":
                                List<TKS_FAS_DocDetail> data_DocDetail = cnn.Query<TKS_FAS_DocDetail>(sql, null, ts).ToList();
                                if (data_DocDetail.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_DocDetail);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_MonthPeriodInfo":
                                List<TKS_FAS_MonthPeriodInfo> data_MonthPeriodInfo = cnn.Query<TKS_FAS_MonthPeriodInfo>(sql, null, ts).ToList();
                                if (data_MonthPeriodInfo.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_MonthPeriodInfo);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_YearPeriodInfo":
                                List<TKS_FAS_YearPeriodInfo> data_YearPeriodInfo = cnn.Query<TKS_FAS_YearPeriodInfo>(sql, null, ts).ToList();
                                if (data_YearPeriodInfo.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_YearPeriodInfo);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_PeriodTrack":
                                List<TKS_FAS_PeriodTrack> data_PeriodTrack = cnn.Query<TKS_FAS_PeriodTrack>(sql, null, ts).ToList();
                                if (data_PeriodTrack.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_PeriodTrack);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_FixedAssetsChange":
                                List<TKS_FAS_FixedAssetsChange> data_FixedAssetsChange = cnn.Query<TKS_FAS_FixedAssetsChange>(sql, null, ts).ToList();
                                if (data_FixedAssetsChange.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_FixedAssetsChange);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_SubjectAss":
                                List<TKS_FAS_SubjectAss> data_SubjectAss = cnn.Query<TKS_FAS_SubjectAss>(sql, null, ts).ToList();
                                if (data_SubjectAss.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_SubjectAss);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_FixedAssetsLog":
                                List<TKS_FAS_FixedAssetsLog> data_FixedAssetsLog = cnn.Query<TKS_FAS_FixedAssetsLog>(sql, null, ts).ToList();
                                if (data_FixedAssetsLog.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_FixedAssetsLog);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_DocTPL":
                                List<TKS_FAS_DocTPL> data_DocTPL = cnn.Query<TKS_FAS_DocTPL>(sql, null, ts).ToList();
                                if (data_DocTPL.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_DocTPL);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                            case "TKS_FAS_TPL2PZ":
                                List<TKS_FAS_TPL2PZ> data_TPL2PZ = cnn.Query<TKS_FAS_TPL2PZ>(sql, null, ts).ToList();
                                if (data_TPL2PZ.Count > 0)
                                {
                                    ISheet sheet = workbook.CreateSheet(tableName[i]);
                                    DataTable dt = ExcelHelper.ToDataTable(data_TPL2PZ);
                                    IRow rowHead = sheet.CreateRow(0);
                                    for (int j = 0; j < dt.Columns.Count; j++)
                                    {
                                        rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                    }
                                    for (int j = 0; j < dt.Rows.Count; j++)
                                    {
                                        IRow row = sheet.CreateRow(j + 1);
                                        for (int c = 0; c < dt.Columns.Count; c++)
                                        {
                                            row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        }

                                    }

                                }
                                break;
                        }
                    }
                    workbook.Write(file);

                    file.Close();
                    FileInfo fileInfo = new FileInfo(filePath);
                    TKS_FAS_BackUpInfo info = new TKS_FAS_BackUpInfo();
                    info.Id = Guid.NewGuid().ToString("N");
                    info.AccountId = user.AccountId;
                    info.Name = fileName;
                    //info.Path = xdpath + "/" + fileName;
                    info.Path = filePath;
                    info.Size = GetMB(fileInfo.Length);
                    info.CreateUser = user.User.Id;
                    info.CreateDate = DateTime.Now;
                    cnn.Insert<TKS_FAS_BackUpInfo>(info, ts);
                    //CreateExport(user.AccountId);
                    ts.Commit();


                    response.IsSuccess = true;
                    response.Message = "备份成功";
                    return response;
                }
                catch (Exception ex)
                {
                    response.Message = ex.ToString();
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseAccountAdd;
                }
                finally
                {
                    foreach (Process p in Process.GetProcessesByName("Excel"))
                    {
                        if (string.IsNullOrEmpty(p.MainWindowTitle))
                        {
                            p.Kill();
                        }
                    }
                    GC.Collect();
                }
            }
        }
        /// <summary>
        /// 将B转换为MB
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string GetMB(long b)
        {
            for (int i = 0; i < 2; i++)
            {
                b /= 1024;
            }
            return b + "MB";
        }
        public void ImportBack( string token,string fileName,string filePath,string Length="0")
        {
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(token, ts);
                    var data_Account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(
                @"select * from TKS_FAS_AccountInfo where Id=@Id",
                new { Id = user.AccountId }, ts);

                    TKS_FAS_BackUpInfo info = new TKS_FAS_BackUpInfo();
                    info.Id = Guid.NewGuid().ToString("N");
                    info.AccountId = user.AccountId;
                    info.Name = fileName;
                    //info.Path = xdpath + "/" + fileName;
                    info.Path = filePath;
                    info.Size = GetMB(long.Parse(Length));
                    info.CreateUser = user.User.Id;
                    info.CreateDate = DateTime.Now;
                    cnn.Insert<TKS_FAS_BackUpInfo>(info, ts);
                    //CreateExport(user.AccountId);
                    ts.Commit();


                   
                }
                catch (Exception ex)
                {
                   
                    ts.Rollback();
                    
                }
                finally
                {
                    foreach (Process p in Process.GetProcessesByName("Excel"))
                    {
                        if (string.IsNullOrEmpty(p.MainWindowTitle))
                        {
                            p.Kill();
                        }
                    }
                    GC.Collect();
                }
            }
        }

        public void CreateExport(MM_UserInfo user, TKS_FAS_AccountInfo data_Account, IDbTransaction ts)
        {


            try
            {
                //var user = this.UserInfoGet(request.Token, ts);
                //    var data_Account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(
                //@"select * from TKS_FAS_AccountInfo where Id=@Id",
                //new { Id = user.AccountId }, null);
                foreach (Process p in Process.GetProcessesByName("Excel"))
                {
                    if (string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        p.Kill();
                    }
                }
                string uploadPath = HttpContext.Current.Server.MapPath("/upload/");
                //string uploadPath = Path.Combine(HttpRuntime.AppDomainAppPath, "upload") + "/";
                string fileName = user.User.Mobile + "_" + data_Account.QY_Name + "_" + DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Second + ".xls";// 文件名称 
                string xdpath = "/upload/" + "备份Bak/" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                uploadPath += "备份Bak\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;



                //判断路径是否存在
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                string filePath = uploadPath + "/" + fileName;
                FileStream file = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                IWorkbook workbook = new HSSFWorkbook();

                for (int i = 0; i < tableName.Length; i++)
                {

                    string sql = string.Format(@"select * from {0} where accountId='{1}'", tableName[i], user.AccountId);


                    switch (tableName[i])
                    {
                        case "TKS_FAS_AccountSubject":

                            List<TKS_FAS_AccountSubject> data = cnn.Query<TKS_FAS_AccountSubject>(sql, null, ts).ToList();
                            if (data.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_Currency":
                            List<TKS_FAS_Currency> data_Currency = cnn.Query<TKS_FAS_Currency>(sql, null, ts).ToList();
                            if (data_Currency.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_Currency);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_CertificateWord":
                            List<TKS_FAS_CertificateWord> data_CertificateWord = cnn.Query<TKS_FAS_CertificateWord>(sql, null, ts).ToList();
                            if (data_CertificateWord.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_CertificateWord);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_GLBalance":
                            List<TKS_FAS_GLBalance> data_GLBalance = cnn.Query<TKS_FAS_GLBalance>(sql, null, ts).ToList();
                            if (data_GLBalance.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_GLBalance);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                        if (c == 35)
                                        {
                                            row.CreateCell(c).SetCellValue(DateTime.Now.ToShortDateString());
                                        }
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_FGLBalance":
                            List<TKS_FAS_FGLBalance> data_FGLBalance = cnn.Query<TKS_FAS_FGLBalance>(sql, null, ts).ToList();
                            if (data_FGLBalance.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_FGLBalance);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_CaculateHelperItem":
                            List<TKS_FAS_CaculateHelperItem> data_CaculateHelperItem = cnn.Query<TKS_FAS_CaculateHelperItem>(sql, null, ts).ToList();
                            if (data_CaculateHelperItem.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_CaculateHelperItem);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;

                        case "TKS_FAS_CaculateHelperDetail":
                            List<TKS_FAS_CaculateHelperDetail> data_CaculateHelperDetail = cnn.Query<TKS_FAS_CaculateHelperDetail>(sql, null, ts).ToList();
                            if (data_CaculateHelperDetail.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_CaculateHelperDetail);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_FixedAssets":
                            List<TKS_FAS_FixedAssets> data_FixedAssets = cnn.Query<TKS_FAS_FixedAssets>(sql, null, ts).ToList();
                            if (data_FixedAssets.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_FixedAssets);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_Doc":
                            List<TKS_FAS_Doc> data_Doc = cnn.Query<TKS_FAS_Doc>(sql, null, ts).ToList();
                            if (data_Doc.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_Doc);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_DocDetail":
                            List<TKS_FAS_DocDetail> data_DocDetail = cnn.Query<TKS_FAS_DocDetail>(sql, null, ts).ToList();
                            if (data_DocDetail.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_DocDetail);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_MonthPeriodInfo":
                            List<TKS_FAS_MonthPeriodInfo> data_MonthPeriodInfo = cnn.Query<TKS_FAS_MonthPeriodInfo>(sql, null, ts).ToList();
                            if (data_MonthPeriodInfo.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_MonthPeriodInfo);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_YearPeriodInfo":
                            List<TKS_FAS_YearPeriodInfo> data_YearPeriodInfo = cnn.Query<TKS_FAS_YearPeriodInfo>(sql, null, ts).ToList();
                            if (data_YearPeriodInfo.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_YearPeriodInfo);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_PeriodTrack":
                            List<TKS_FAS_PeriodTrack> data_PeriodTrack = cnn.Query<TKS_FAS_PeriodTrack>(sql, null, ts).ToList();
                            if (data_PeriodTrack.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_PeriodTrack);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_FixedAssetsChange":
                            List<TKS_FAS_FixedAssetsChange> data_FixedAssetsChange = cnn.Query<TKS_FAS_FixedAssetsChange>(sql, null, ts).ToList();
                            if (data_FixedAssetsChange.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_FixedAssetsChange);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_SubjectAss":
                            List<TKS_FAS_SubjectAss> data_SubjectAss = cnn.Query<TKS_FAS_SubjectAss>(sql, null, ts).ToList();
                            if (data_SubjectAss.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_SubjectAss);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_FixedAssetsLog":
                            List<TKS_FAS_FixedAssetsLog> data_FixedAssetsLog = cnn.Query<TKS_FAS_FixedAssetsLog>(sql, null, ts).ToList();
                            if (data_FixedAssetsLog.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_FixedAssetsLog);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_DocTPL":
                            List<TKS_FAS_DocTPL> data_DocTPL = cnn.Query<TKS_FAS_DocTPL>(sql, null, ts).ToList();
                            if (data_DocTPL.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_DocTPL);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                        case "TKS_FAS_TPL2PZ":
                            List<TKS_FAS_TPL2PZ> data_TPL2PZ = cnn.Query<TKS_FAS_TPL2PZ>(sql, null, ts).ToList();
                            if (data_TPL2PZ.Count > 0)
                            {
                                ISheet sheet = workbook.CreateSheet(tableName[i]);
                                DataTable dt = ExcelHelper.ToDataTable(data_TPL2PZ);
                                IRow rowHead = sheet.CreateRow(0);
                                for (int j = 0; j < dt.Columns.Count; j++)
                                {
                                    rowHead.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                                }
                                for (int j = 0; j < dt.Rows.Count; j++)
                                {
                                    IRow row = sheet.CreateRow(j + 1);
                                    for (int c = 0; c < dt.Columns.Count; c++)
                                    {
                                        row.CreateCell(c).SetCellValue(dt.Rows[j][c].ToString());
                                    }

                                }

                            }
                            break;
                    }
                }
                workbook.Write(file);

                file.Close();


            }
            catch (Exception ex)
            {
                ts.Rollback();
            }
            finally
            {
                foreach (Process p in Process.GetProcessesByName("Excel"))
                {
                    if (string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        p.Kill();
                    }
                }
                GC.Collect();
            }

        }




        public ResponseAccountGet AccountGet(RequestAccountGet request)
        {
            ResponseAccountGet response = new ResponseAccountGet();
            using (cnn = GetConnection())
            {
                try
                {
                    var data = cnn.QuerySingle<TKS_FAS_AccountInfoExt>(
                        @"select A.*,B.TrueName,c.name as SupplierName from tks_fas_AccountInfo A  
                        left join TKS_FAS_User B on A.AccountantId=B.id 
                        left join tks_fas_node C on A.WB_CreditCode=C.Id
                            where A.id=@Id ", new
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
                    return this.DealException(response, ex) as ResponseAccountGet;
                }
            }
        }

        public ResponseAccountAdd AccountAdd(RequestAccountAdd request)
        {
            ResponseAccountAdd response = new ResponseAccountAdd();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    var d = cnn.Query("select * from tks_fas_AccountInfo where qy_name=@QYName",
                        new { QYName = request.Data.QY_Name }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("账套名已经存在");
                    }
                    d = cnn.Query("select * from tks_fas_AccountInfo where qy_creditCode=@QYCreditCode",
                        new { QYCreditCode = request.Data.QY_CreditCode }, ts);

                    if (d.Count() > 0)
                    {
                        throw new NormalException("社会信用代码已经存在");
                    }

                    request.Data.Id = Guid.NewGuid().ToString("N");
                    request.Data.DZ_CreditCode = user.Node.CreditCode;
                    request.Data.WB_Status = 1;//自有
                    request.Data.IsInvitation = 0;
                    string AccountId = request.Data.Id;
                    request.Data.CreateUser = user.User.TrueName;
                    request.Data.CreateDate = DateTime.Now;
                    cnn.Insert<TKS_FAS_AccountInfo>(request.Data, ts);

                    var month = SetPeriod(request.Data, ts);

                    InitSubject(request.Data, ts);
                    InitCurrency(request.Data, ts);
                    InitPZZ(request.Data, ts);
                    InitBAL(request.Data, month, ts);
                    InitReport(request.Data, ts);
                    InitCalData(request.Data, ts);
                    InitFP(request.Data, ts);
                    //激活账套
                    string sql = @"delete from TKS_FAS_UserCurrentAccount where userId=@UserId";

                    cnn.Execute(sql, new { UserId = user.User.Id }, ts);


                    TKS_FAS_UserCurrentAccount cur = new TKS_FAS_UserCurrentAccount();
                    cur.Id = Guid.NewGuid().ToString("N");
                    cur.UserId = user.User.Id;
                    //cur.AccountId = request.Data.Id;
                    cur.AccountId = AccountId;
                    cnn.Insert<TKS_FAS_UserCurrentAccount>(cur, ts);

                    ts.Commit();
                    response.Id = request.Data.Id;

                    response.IsSuccess = true;
                    response.Message = "新增成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseAccountAdd;
                }
            }
        }


        public TKS_FAS_AccountInfo AccountInfoGet(string id, IDbTransaction ts)
        {
            string sql = @"select * from TKS_FAS_AccountInfo where id=@Id";

            var a = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(sql, new { Id = id }, ts);

            return a;
        }

        /// <summary>
        /// 初始化报表
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ts"></param>
        private void InitReport(TKS_FAS_AccountInfo data, IDbTransaction ts)
        {
            var reportHead = cnn.Query<TKS_FAS_ReportTPL>("select * from TKS_FAS_ReportTPL where accountId=@AccountId",
                new { AccountId = "-" }, ts).ToList();
            var reportBody = cnn.Query<TKS_FAS_ReportDetailTPL>("select * from TKS_FAS_ReportDetailTPL where accountId=@AccountId",
               new { AccountId = "-" }, ts).ToList();
            var formula = cnn.Query<TKS_FAS_Formula>("select * from TKS_FAS_Formula where accountId=@AccountId",
               new { AccountId = "-" }, ts).ToList();

            List<TKS_FAS_ReportTPL> newHeads = new List<TKS_FAS_ReportTPL>();
            Dictionary<string, string> dicHead = new Dictionary<string, string>();
            foreach (var head in reportHead)
            {
                var newHead = new TKS_FAS_ReportTPL();
                newHead = head.Clone() as TKS_FAS_ReportTPL;
                newHead.Id = Guid.NewGuid().ToString("N");
                newHead.AccountId = data.Id;
                newHeads.Add(newHead);
                if (!dicHead.ContainsKey(head.Id))
                {
                    dicHead.Add(head.Id, newHead.Id);
                }
            }

            List<TKS_FAS_ReportDetailTPL> newBodys = new List<TKS_FAS_ReportDetailTPL>();
            Dictionary<string, string> dicBody = new Dictionary<string, string>();
            foreach (var body in reportBody)
            {
                var newBody = body.Clone() as TKS_FAS_ReportDetailTPL;
                newBody.Id = Guid.NewGuid().ToString();
                if (dicHead.ContainsKey(body.ParentId))
                {
                    newBody.ParentId = dicHead[body.ParentId];
                }
                else
                {
                    throw new NormalException("报表中不存在Head：" + body.ParentId);
                }
                newBody.AccountId = data.Id;
                newBodys.Add(newBody);
                if (!dicBody.ContainsKey(body.Id))
                {
                    dicBody.Add(body.Id, newBody.Id);
                }
            }

            List<TKS_FAS_Formula> newFormulas = new List<TKS_FAS_Formula>();
            foreach (var f in formula)
            {
                var newFormula = f.Clone() as TKS_FAS_Formula;
                newFormula.Id = Guid.NewGuid().ToString("N");
                if (dicHead.ContainsKey(f.ReportTPLId))
                {


                    newFormula.ReportTPLId = dicHead[f.ReportTPLId];
                }
                else
                {
                    throw new NormalException("报表中不存在Head：" + f.ReportTPLId);
                }
                if (dicBody.ContainsKey(f.ReportDetailTPLId))
                {
                    newFormula.ReportDetailTPLId = dicBody[f.ReportDetailTPLId];
                }
                else
                {
                    throw new NormalException("报表中不存在明细ID：" + f.ReportDetailTPLId);
                }
                newFormula.AccountId = data.Id;
                newFormulas.Add(newFormula);
            }

            foreach (var item in newHeads)
            {
                cnn.Insert<TKS_FAS_ReportTPL>(item, ts);
            }

            foreach (var item in newBodys)
            {
                cnn.Insert<TKS_FAS_ReportDetailTPL>(item, ts);
            }

            foreach (var item in newFormulas)
            {
                cnn.Insert<TKS_FAS_Formula>(item, ts);
            }

        }

        private void InitPZZ(TKS_FAS_AccountInfo account, IDbTransaction ts)
        {
            string sql = @"insert into TKS_FAS_CertificateWord(id,accountid,pzz,showtitle,isDefault)
                        select newid(),@AccountId,pzz,showTitle,isDefault from TKS_FAS_CertificateWordBak";
            cnn.Execute(sql, new { AccountId = account.Id }, ts);
        }

        private void InitCurrency(TKS_FAS_AccountInfo account, IDbTransaction ts)
        {
            TKS_FAS_Currency currency = new TKS_FAS_Currency();
            currency.Id = Guid.NewGuid().ToString("N");
            currency.AccountId = account.Id;
            currency.Name = "人名币";
            currency.Code = "RMB";
            currency.ExchangeRate = 1;
            currency.IsBaseCurrency = 1;
            cnn.Insert<TKS_FAS_Currency>(currency, ts);
        }

        private void InitSubject(TKS_FAS_AccountInfo account, IDbTransaction ts)
        {

            string sql = @"  INSERT INTO  TKS_FAS_AccountSubject 
                           (Id
                           ,AccountId
                           ,Code
                           ,Name
                           ,showTitle
                           ,ParentId
                           ,rootCode
                           ,Category
                           ,Credit_Debit
                           ,IsUse
                           ,IsValid
                           ,IsCalHelperValid
                           ,IsQuantityValid
                           ,IsCurrencyValid
                           ,IsLeaf
                           ,SLevel
                           ,CalHelperValue
                           ,QuantityValue
                           ,CurrencyValue
                           ,IsCustom)
 
                           select
		                      newid()
                           ,@AccountId
                           ,Code
                           ,Name
                           ,showTitle
                           ,ParentId
                           ,rootCode
                           ,Category
                           ,Credit_Debit
                           ,IsUse
                           ,IsValid
                           ,IsCalHelperValid
                           ,IsQuantityValid
                           ,IsCurrencyValid
                           ,IsLeaf
                           ,SLevel
                           ,CalHelperValue
                           ,QuantityValue
                           ,CurrencyValue
                           ,IsCustom
		                   from  
		                   TKS_FAS_AccountSubjectBak";

            cnn.Execute(sql, new { AccountId = account.Id }, ts);
            //add by Hero.Zhang 更新科目的ParentId
            #region
            var Subject_bak = cnn.Query<TKS_FAS_AccountSubject>(
                   @"select * from TKS_FAS_AccountSubjectBak", null, ts).ToList();
            if (Subject_bak.Count > 0)
            {
                List<AccountSubject> lstSubject = new List<AccountSubject>();
                foreach (var bak in Subject_bak)
                {
                    if (bak.SLevel > 1)
                    {
                        AccountSubject _subject = new AccountSubject();
                        _subject.Code = bak.Code;
                        _subject.Category = bak.Category;
                        var Subject = cnn.Query<TKS_FAS_AccountSubject>(
                    @"select * from TKS_FAS_AccountSubjectBak where id=@id and category=@category",
                    new { id = bak.ParentId, category = bak.Category }, ts).FirstOrDefault();
                        _subject.ParentCode = "";
                        if (Subject != null)
                        {
                            _subject.ParentCode = Subject.Code;
                        }
                        lstSubject.Add(_subject);
                    }
                }
                if (lstSubject.Count > 0)
                {
                    foreach (var item in lstSubject)
                    {
                        cnn.Execute("update TKS_FAS_AccountSubject set ParentId=(select id from TKS_FAS_AccountSubject where Category=@Category and accountId=@AccountId and Code=@ParentCode) where Category=@Category and accountId=@AccountId and Code=@Code",
                      new { Category = item.Category, AccountId = account.Id, Code = item.Code, ParentCode = item.ParentCode }, ts);
                    }
                }
            }
            #endregion
        }
        public class AccountSubject
        {

            public string Code { get; set; }

            public string ParentCode { get; set; }
            public string Category { get; set; }

        }
        /// <summary>
        /// 初始化余额表
        /// </summary>
        /// <param name="account"></param>
        /// <param name="month"></param>
        /// <param name="ts"></param>
        private void InitBAL(TKS_FAS_AccountInfo account, TKS_FAS_MonthPeriodInfo month, IDbTransaction ts)
        {
            string sql = @"insert into TKS_FAS_GLBalance 
                        (id,
                        AccountId,
                        SubjectId,
                        SubjectCode,
                        ParentId,
                        category,
                        Name,
                        PeriodId,
                        Year,
                        CurrencyCode
                        ,SCredit_Debit,
                        isCalHelperValid,
                        isQuantityValid,
                        isDefaultCurrency,
                        isLeaf,
                        bwbStartBal
                        )
                        select 
                        newid(),
                        @AccountId,
                        id,
                        Code,
                        ParentId,
                        category,
                        Name,
                        @PeriodId,
                        @Year,
                        'RMB',
                        credit_debit,
                        iscalHelperValid,
                        isQuantityValid,
                        1,
                        isLeaf,
                        0
                        
                        from TKS_FAS_AccountSubject where accountId=@AccountId ";
            cnn.Execute(sql, new { AccountId = account.Id, PeriodId = month.Id, Year = month.Year }, ts);

            string sqlQC = @"insert into TKS_FAS_GLBalance 
                        (id,
                        AccountId,
                        SubjectId,
                        SubjectCode,
                        ParentId,
                        category,
                        Name,
                        PeriodId,
                        Year,
                        CurrencyCode
                        ,SCredit_Debit,
                        isCalHelperValid,
                        isQuantityValid,
                        isDefaultCurrency,
                        isLeaf,
                        bwbStartBal
                        )
                        select 
                        newid(),
                        @AccountId,
                        id,
                        Code,
                        ParentId,
                        category,
                        Name,
                        '',
                        @Year,
                        'RMB',
                        credit_debit,
                        iscalHelperValid,
                        isQuantityValid,
                        1,
                        isLeaf,
                        0
                        
                        from TKS_FAS_AccountSubject where accountId=@AccountId ";
            cnn.Execute(sqlQC, new { AccountId = account.Id, Year = month.Year }, ts);
        }

        /// <summary>
        /// 辅助核算项初始化 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="ts"></param>
        private void InitCalData(TKS_FAS_AccountInfo account, IDbTransaction ts)
        {
            TKS_FAS_CaculateHelperItem data = new TKS_FAS_CaculateHelperItem();
            data.Id = Guid.NewGuid().ToString("N");
            data.AccountId = account.Id;
            data.IsCustom = 0;
            data.Title = "客户";
            cnn.Insert<TKS_FAS_CaculateHelperItem>(data, ts);

            data = new TKS_FAS_CaculateHelperItem();
            data.Id = Guid.NewGuid().ToString("N");
            data.AccountId = account.Id;
            data.IsCustom = 0;
            data.Title = "供应商";
            cnn.Insert<TKS_FAS_CaculateHelperItem>(data, ts);

            data = new TKS_FAS_CaculateHelperItem();
            data.Id = Guid.NewGuid().ToString("N");
            data.AccountId = account.Id;
            data.IsCustom = 0;
            data.Title = "部门";
            cnn.Insert<TKS_FAS_CaculateHelperItem>(data, ts);
        }

        /// <summary>
        /// 初始化发票
        /// </summary>
        /// <param name="account"></param>
        /// <param name="ts"></param>
        private void InitFP(TKS_FAS_AccountInfo account, IDbTransaction ts)
        {
            string sql = @"select * from   TKS_FAS_Invoice2TPL where accountId='-' ";

            var lsFP = cnn.Query<TKS_FAS_Invoice2TPL>(sql, null, ts).ToList();



            foreach (var item in lsFP)
            {

                sql = "select * from tks_fas_docTPL where id=@Id";
                var doc = cnn.QuerySingleOrDefault<TKS_FAS_DocTPL>(sql, new { Id = item.TPLID }, ts);
                sql = "select * from TKS_FAS_DocDetailTPL where parentId=@ParentId";
                var lsDetail = cnn.Query<TKS_FAS_DocDetailTPL>(sql, new { ParentId = item.TPLID }, ts).ToList();

                doc.Id = Guid.NewGuid().ToString("N");
                doc.AccountId = account.Id;
                cnn.Insert<TKS_FAS_DocTPL>(doc, ts);

                item.Id = Guid.NewGuid().ToString("N");
                item.TPLID = doc.Id;
                item.AccountId = account.Id;
                item.CreateUser = account.CreateUser;
                item.CreateDate = account.CreateDate;
                cnn.Insert<TKS_FAS_Invoice2TPL>(item, ts);

                foreach (var detail in lsDetail)
                {
                    detail.Id = Guid.NewGuid().ToString("N");
                    detail.AccountId = account.Id;
                    detail.ParentId = doc.Id;
                    cnn.Insert<TKS_FAS_DocDetailTPL>(detail, ts);

                }

            }



        }

        private TKS_FAS_MonthPeriodInfo SetPeriod(TKS_FAS_AccountInfo account, IDbTransaction ts)
        {
            var date = account.StartYearMonth.Value;
            TKS_FAS_YearPeriodInfo year = new TKS_FAS_YearPeriodInfo();
            year.Id = Guid.NewGuid().ToString("N");
            year.AccountId = account.Id;
            year.StartDate = account.StartYearMonth;
            year.EndDate = date.AddMonths(13 - date.Month).AddSeconds(-1);
            year.Year = date.Year;
            year.IsActive = 1;
            year.PeriodNUM = 13 - date.Month;
            cnn.Insert<TKS_FAS_YearPeriodInfo>(year, ts);

            TKS_FAS_MonthPeriodInfo month = new TKS_FAS_MonthPeriodInfo();
            month.Id = Guid.NewGuid().ToString("N");
            month.IsActive = 1;
            month.AccountId = account.Id;
            month.IsPay = 0;
            month.Month = date.Month;
            month.Year = date.Year;
            month.StartDate = date;
            month.EndDate = date.AddMonths(1).AddSeconds(-1);
            month.ParentId = year.Id;
            cnn.Insert<TKS_FAS_MonthPeriodInfo>(month, ts);

            return month;
        }

        public ResponseAccountUpdate AccountUpdate(RequestAccountUpdate request)
        {
            ResponseAccountUpdate response = new ResponseAccountUpdate();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();

                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    var d = cnn.Query<TKS_FAS_AccountInfo>(@"select * from tks_fas_AccountInfo where
                    QY_name=@QYName and id<>@Id and dz_creditCode=@DZ_CreditCode",
                        new
                        {
                            QYName = request.Data.QY_Name
                            ,
                            Id = request.Data.Id,
                            DZ_CreditCode = user.Node.CreditCode
                        }, ts);
                    if (d.Count() > 0)
                    {
                        throw new NormalException("单位名已经存在");//当前代帐企业下
                    }
                    string sql = @"update tks_fas_accountInfo set 
                        qy_name=@QY_Name,
                        qy_address=@QY_Address,
                        QY_CreditCode=@QY_CreditCode,
                        qy_type=@QY_Type,
                        Dl_CreditCode=@DL_CreditCode,
                        AccountantId=@AccountantId,
                        StartYearMonth=@StartYearMonth,
                  
                        AccountantRule=@AccountantRule,
                        AddedValueTaxType=@AddedValueTaxType,
                        IsNeedReviewed=@IsNeedReviewed,
                        LandTax =@LandTax,
                        TaxRate = @TaxRate,
                        updateUser=@UpdateUser,
                        updateDate=@UpdateDate 
                        where id=@Id";
                    request.Data.UpdateDate = DateTime.Now;
                    request.Data.UpdateUser = user.User.TrueName;
                    var r = cnn.Execute(sql, request.Data, ts);
                    if (r == 1)
                    {
                        ts.Commit();
                        response.IsSuccess = true;
                        response.Message = "更新成功";
                        return response;
                    }
                    else
                    {

                        throw new AppException(user.User.UserName, "AccountUpdate", "数据更新异常", r + "行数据被更新");

                    }
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseAccountUpdate;
                }

            }
        }

        public ResponseAccountDelete AccountDelete(RequestAccountDelete request)
        {
            ResponseAccountDelete response = new ResponseAccountDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("delete from tks_fas_AccountInfo where id=@Id", request.Data, ts);

                    cnn.Execute("delete from TKS_FAS_YearPeriodInfo where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_MonthPeriodInfo where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute(" delete from TKS_FAS_UserCurrentAccount where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_CaculateHelperItem where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_AccountSubject where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_CaculateHelperDetail where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_CustomColDes where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_SubjectAss where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_Currency where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_CertificateWord where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    //凭证
                    cnn.Execute("delete from TKS_FAS_Doc where accountid=@AccountId",
                        new { AccountId = request.Data.Id }, ts);
                    cnn.Execute("delete from tks_fas_docdeTail where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);


                    //余额表
                    cnn.Execute("delete from TKS_FAS_GLBalance where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);
                    cnn.Execute("delete from TKS_FAS_FGLBalance where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_GLBalanceBak where accountId=@AccountId",
                    new { AccountId = request.Data.Id }, ts);
                    cnn.Execute("delete from TKS_FAS_FGLBalanceBak where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_TPL2PZ where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);
                    //模板
                    cnn.Execute("delete from TKS_FAS_DocTPL where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);
                    cnn.Execute("delete from TKS_FAS_DocDetailTPL where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    //发票
                    cnn.Execute("delete from TKS_FAS_Invoice where accountId=@AccountId",
                     new { AccountId = request.Data.Id }, ts);
                    cnn.Execute("delete from TKS_FAS_InvoiceAttach where accountId=@AccountId",
                   new { AccountId = request.Data.Id }, ts);

                    //报表
                    cnn.Execute("delete from TKS_FAS_ReportTPL where accountId=@AccountId",
                   new { AccountId = request.Data.Id }, ts);
                    cnn.Execute("delete from TKS_FAS_ReportDetailTPL where accountId=@AccountId",
                   new { AccountId = request.Data.Id }, ts);
                    cnn.Execute("delete from TKS_FAS_Formula where accountId=@AccountId",
                   new { AccountId = request.Data.Id }, ts);
                    //固定资产
                    cnn.Execute("delete from TKS_FAS_FixedAssets where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);
                    cnn.Execute("delete from TKS_FAS_FixedAssetsChange where accountId=@AccountId",
                       new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from tks_fas_periodTrack where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    cnn.Execute("delete from TKS_FAS_Invoice2TPL where accountId=@AccountId",
                        new { AccountId = request.Data.Id }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseAccountDelete;
                }
            }
        }

        public ResponseAccountDelete BackUpDelete(RequestAccountDelete request)
        {
            ResponseAccountDelete response = new ResponseAccountDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    cnn.Execute("delete from TKS_FAS_BackUpInfo where Id=@Id",
                       new { Id = request.Data.Id }, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseAccountDelete;
                }
            }
        }

        public ResponseAccountDelete BackUpDownload(RequestAccountDelete request)
        {
            ResponseAccountDelete response = new ResponseAccountDelete();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {

                    var data = cnn.QueryFirstOrDefault<TKS_FAS_BackUpInfo>(
                @"select * from TKS_FAS_BackUpInfo where Id=@Id",
                new { Id = request.Data.Id }, ts);
                    if (File.Exists(data.Path))
                    {
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "文件不存在";
                        response.IsSuccess = false;
                    }
                    ts.Commit();

                    //response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseAccountDelete;
                }
            }
        }

        public ResponseAccountDelete BackUpHuiFu(RequestAccountDelete request)
        {
            string AccountId = "";
            string userName = "";
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);
                     AccountId = user.AccountId;
                    userName = user.User.TrueName;
                    var data_Account = cnn.QueryFirstOrDefault<TKS_FAS_AccountInfo>(
                @"select * from TKS_FAS_AccountInfo where Id=@Id",
                new { Id = user.AccountId }, ts);
                    CreateExport(user, data_Account, ts);
                    ts.Commit();
                }
                catch (Exception)
                {
                    ts.Rollback();

                }

            }
            string n = "";
            ResponseAccountDelete response = new ResponseAccountDelete();
            using (SqlConnection cnn = new SqlConnection(CnnString))
            {
                cnn.Open();
                var ts = cnn.BeginTransaction();
                try
                {

                    var data = cnn.QueryFirstOrDefault<TKS_FAS_BackUpInfo>(
                @"select * from TKS_FAS_BackUpInfo where Id=@Id",
                new { Id = request.Data.Id }, ts);
                    if (File.Exists(data.Path))
                    {
                        List<DataTable> lstData = ExcelHelper.ReadExcelToList(data.Path, true);
                        if (lstData.Count > 0&&!string.IsNullOrEmpty(AccountId))
                        {
                            List<DataTable> newData = new List<DataTable>();
                            Dictionary<string, string> myDic = new Dictionary<string, string>();
                            DataTable dt_YearPeriodInfo = new DataTable();
                            dt_YearPeriodInfo.TableName = "TKS_FAS_YearPeriodInfo";
                            DataTable dt_MonthPeriodInfo = new DataTable();
                            dt_MonthPeriodInfo.TableName= "TKS_FAS_MonthPeriodInfo";
                            DataTable dt_Doc = new DataTable();
                            dt_Doc.TableName = "TKS_FAS_Doc";
                            DataTable dt_DocDetail = new DataTable();
                            dt_DocDetail.TableName = "TKS_FAS_DocDetail";
                            DataTable dt_FixedAssets = new DataTable();
                            dt_FixedAssets.TableName = "TKS_FAS_FixedAssets";
                            DataTable dt_CaculateHelperItem = new DataTable();
                            dt_CaculateHelperItem.TableName = "TKS_FAS_CaculateHelperItem";
                            DataTable dt_CaculateHelperDetail = new DataTable();
                            dt_CaculateHelperDetail.TableName = "TKS_FAS_CaculateHelperDetail";
                            DataTable dt_FGLBalance = new DataTable();
                            dt_FGLBalance.TableName = "TKS_FAS_FGLBalance";
                            DataTable dt_GLBalance = new DataTable();
                            dt_GLBalance.TableName = "TKS_FAS_GLBalance";
                            DataTable dt_CertificateWord = new DataTable();
                            dt_CertificateWord.TableName = "TKS_FAS_CertificateWord";
                            DataTable dt_Currency = new DataTable();
                            dt_Currency.TableName = "TKS_FAS_Currency";
                            DataTable dt_AccountSubject = new DataTable();
                            dt_AccountSubject.TableName = "TKS_FAS_AccountSubject";
                            DataTable dt_PeriodTrack = new DataTable();
                            dt_PeriodTrack.TableName = "TKS_FAS_PeriodTrack";
                            DataTable dt_FixedAssetsChange = new DataTable();
                            dt_FixedAssetsChange.TableName = "TKS_FAS_FixedAssetsChange";
                            DataTable dt_SubjectAss = new DataTable();
                            dt_SubjectAss.TableName = "TKS_FAS_SubjectAss";
                            DataTable dt_FixedAssetsLog = new DataTable();
                            dt_FixedAssetsLog.TableName = "TKS_FAS_FixedAssetsLog";
                            DataTable dt_DocTPL = new DataTable();
                            dt_DocTPL.TableName = "TKS_FAS_DocTPL";
                            DataTable dt_TPL2PZ = new DataTable();
                            dt_TPL2PZ.TableName = "TKS_FAS_TPL2PZ";
                            foreach (var lst in lstData)
                            {
                                string TableName = lst.TableName;
                                
                                switch(TableName)
                                {
                                    case "TKS_FAS_YearPeriodInfo":
                                        dt_YearPeriodInfo = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_MonthPeriodInfo":
                                        dt_MonthPeriodInfo = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_DocDetail":
                                        dt_DocDetail = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_Doc":
                                        dt_Doc = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_FixedAssets":
                                        dt_FixedAssets = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_CaculateHelperItem":
                                        dt_CaculateHelperItem = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_CaculateHelperDetail":
                                        dt_CaculateHelperDetail = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_FGLBalance":
                                        dt_FGLBalance = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_GLBalance":
                                        dt_GLBalance = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_CertificateWord":
                                        dt_CertificateWord = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_Currency":
                                        dt_Currency = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_AccountSubject":
                                        dt_AccountSubject = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_PeriodTrack":
                                        dt_PeriodTrack = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_FixedAssetsChange":
                                        dt_PeriodTrack = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_SubjectAss":
                                        dt_SubjectAss = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_FixedAssetsLog":
                                        dt_FixedAssetsLog = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_DocTPL":
                                        dt_DocTPL = lst.DataSet.Tables[0];
                                        break;
                                    case "TKS_FAS_TPL2PZ":
                                        dt_TPL2PZ = lst.DataSet.Tables[0];
                                        break;
                                }

                            }
                            #region 生成替换字典
                            foreach (DataRow dr in dt_FixedAssets.Rows)
                            {
                                myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                            }
                            foreach (DataRow dr in dt_YearPeriodInfo.Rows)
                            {
                                myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                            }
                            foreach (DataRow dr in dt_MonthPeriodInfo.Rows)
                            {
                                myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                            }
                            foreach (DataRow dr in dt_Currency.Rows)
                            {
                                myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                            }
                            foreach (DataRow dr in dt_CertificateWord.Rows)
                            {
                                myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                            }
                            foreach (DataRow dr in dt_AccountSubject.Rows)
                            {
                                myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                            }
                            foreach (DataRow dr in dt_CaculateHelperItem.Rows)
                            {
                                myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                            }
                            foreach (DataRow dr in dt_GLBalance.Rows)
                            {
                                myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                            }
                            if (dt_Doc.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt_Doc.Rows)
                                {
                                    myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                                }
                            }
                            if (dt_DocTPL.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt_DocTPL.Rows)
                                {
                                    myDic.Add(dr["Id"].ToString(), Guid.NewGuid().ToString("N"));
                                }
                            }
                            #endregion

                            #region Excel数据重新赋值
                            if (dt_SubjectAss.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt_SubjectAss.Rows)
                                {
                                    string Key_ParentId = dr["ParentId"].ToString();
                                    string Key_Code = dr["Code"].ToString();

                                    if (myDic.ContainsKey(Key_ParentId) && myDic.ContainsKey(Key_Code))
                                    {
                                        dr["Id"] = Guid.NewGuid().ToString("N");
                                        dr["ParentId"] = myDic[Key_ParentId];
                                        dr["Code"] = myDic[Key_Code];
                                        dr["AccountId"] = AccountId;

                                    }
                                }
                            }
                            if (dt_PeriodTrack.Rows.Count>0)
                            {
                                foreach (DataRow dr in dt_PeriodTrack.Rows)
                                {
                                    string Key_FromId = dr["FromId"].ToString();
                                    string Key_ToId = dr["ToId"].ToString();
                     
                                    if (myDic.ContainsKey(Key_FromId) && myDic.ContainsKey(Key_ToId))
                                    {
                                        dr["Id"] = Guid.NewGuid().ToString("N");
                                        dr["FromId"] = myDic[Key_FromId];
                                        dr["ToId"] = myDic[Key_ToId];
                                        dr["AccountId"] = AccountId;
                                   
                                    }
                                }
                            }
                            foreach (DataRow dr in dt_YearPeriodInfo.Rows)
                            {
                                string key = dr["Id"].ToString();
                                if (myDic.ContainsKey(key))
                                {
                                    dr["Id"] = myDic[key];
                                    dr["AccountId"] = AccountId;
                                }                              
                            }
                            foreach (DataRow dr in dt_MonthPeriodInfo.Rows)
                            {
                                string Key_Id= dr["Id"].ToString();
                                string Key_ParentId = dr["ParentId"].ToString();
                                if (myDic.ContainsKey(Key_Id)&& myDic.ContainsKey(Key_ParentId))
                                {
                                    dr["Id"] = myDic[Key_Id];
                                    dr["ParentId"] = myDic[Key_ParentId];
                                    dr["AccountId"] = AccountId;
                                }
                            }
                            if (dt_Doc.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt_Doc.Rows)
                                {
                                    string Key_Id = dr["Id"].ToString();
                                    string Key_PZZ = dr["PZZ"].ToString();
                                    string Key_PeriodId = dr["PeriodId"].ToString();
                                    if (myDic.ContainsKey(Key_Id) && myDic.ContainsKey(Key_PZZ) && myDic.ContainsKey(Key_PeriodId))
                                    {
                                        dr["Id"] = myDic[Key_Id];
                                        dr["PZZ"] = myDic[Key_PZZ];
                                        dr["PeriodId"] = myDic[Key_PeriodId];
                                        dr["AccountId"] = AccountId;
                                        dr["UpdateUser"] = userName;
                                        dr["UpdateDate"] = DateTime.Now;
                                    }
                                }
                                foreach (DataRow dr in dt_DocDetail.Rows)
                                {
                                    string Key_ParentId = dr["ParentId"].ToString(); 
                                    string Key_PeriodId = dr["PeriodId"].ToString();
                                    string Key_CalItem1 = dr["CalItem1"].ToString();
                                    if (myDic.ContainsKey(Key_ParentId) && myDic.ContainsKey(Key_PeriodId))
                                    {
                                        dr["Id"] = Guid.NewGuid().ToString("N");
                                        dr["ParentId"] = myDic[Key_ParentId];
                                        dr["PeriodId"] = myDic[Key_PeriodId];
                                        dr["AccountId"] = AccountId;
                                        if (myDic.ContainsKey(Key_CalItem1))
                                        {
                                            dr["CalItem1"] = myDic[Key_CalItem1];
                                        }
                                    }
                                }
                            }
                            foreach (DataRow dr in dt_CaculateHelperItem.Rows)
                            {
                                string Key_Id = dr["Id"].ToString();
                                if (myDic.ContainsKey(Key_Id))
                                {
                                    dr["Id"] = myDic[Key_Id];    
                                    dr["AccountId"] = AccountId;
                                }
                            }
                            foreach (DataRow dr in dt_CaculateHelperDetail.Rows)
                            {
                                string Key_ParentId = dr["ParentId"].ToString();
                                if (myDic.ContainsKey(Key_ParentId))
                                {
                                    dr["Id"] = Guid.NewGuid().ToString("N");
                                    dr["ParentId"] = myDic[Key_ParentId];
                                    dr["AccountId"] = AccountId;
                                    if (string.IsNullOrEmpty(dr["FoundDate"].ToString()))
                                    {
                                        dr["FoundDate"] = null;
                                    }
                                    if (string.IsNullOrEmpty(dr["CancelDate"].ToString()))
                                    {
                                        dr["CancelDate"] = null;
                                    }
                                }
                            }
                            foreach (DataRow dr in dt_GLBalance.Rows)
                            {
                                string Key_Id = dr["Id"].ToString();
                                string Key_SubjectId = dr["SubjectId"].ToString();
                                string Key_ParentId = dr["ParentId"].ToString();
                                string Key_PeriodId = dr["PeriodId"].ToString();
                                if (myDic.ContainsKey(Key_Id)&&myDic.ContainsKey(Key_SubjectId))
                                {
                                    dr["Id"] = myDic[Key_Id];
                                    dr["AccountId"] = AccountId;
                                    dr["SubjectId"] = myDic[Key_SubjectId];
                                    if (myDic.ContainsKey(Key_ParentId))
                                    {
                                        dr["ParentId"] = myDic[Key_ParentId];
                                    }
                                    if (myDic.ContainsKey(Key_PeriodId))
                                    {
                                        dr["PeriodId"] = myDic[Key_PeriodId];
                                    }
                                    
                                }
                         
                            }

                            if (dt_FGLBalance.Rows.Count>0)
                            {
                                foreach (DataRow dr in dt_FGLBalance.Rows)
                                {
                                    string Key_CalItem1 = dr["CalItem1"].ToString();
                                    string Key_ParentId = dr["ParentId"].ToString();
                                    string Key_PeriodId = dr["PeriodId"].ToString();

                                    dr["Id"] = Guid.NewGuid().ToString("N");
                                    dr["AccountId"] = AccountId;
                                    if (myDic.ContainsKey(Key_CalItem1))
                                    {
                                        dr["CalItem1"] = myDic[Key_CalItem1];
                                    }
                                    if (myDic.ContainsKey(Key_PeriodId))
                                    {
                                        dr["PeriodId"] = myDic[Key_PeriodId];
                                    }
                                    if (myDic.ContainsKey(Key_ParentId))
                                    {
                                        dr["ParentId"] = myDic[Key_ParentId];

                                    }

                                }
                            }

                            foreach (DataRow dr in dt_CertificateWord.Rows)
                            {
                                string key = dr["Id"].ToString();
                                if (myDic.ContainsKey(key))
                                {
                                    dr["Id"] = myDic[key];
                                    dr["AccountId"] = AccountId;
                                }
                            }
                            foreach (DataRow dr in dt_Currency.Rows)
                            {
                                string key = dr["Id"].ToString();
                                if (myDic.ContainsKey(key))
                                {
                                    dr["Id"] = myDic[key];
                                    dr["AccountId"] = AccountId;
                                }
                            }
                            foreach (DataRow dr in dt_AccountSubject.Rows)
                            {
                                string Key_Id = dr["Id"].ToString();
     
                                string Key_ParentId = dr["ParentId"].ToString();

                                if (myDic.ContainsKey(Key_Id))
                                {
                                    dr["Id"] = myDic[Key_Id];
                                    dr["AccountId"] = AccountId;
                                    if (myDic.ContainsKey(Key_ParentId))
                                    {
                                        dr["ParentId"] = myDic[Key_ParentId];
                                    }
                                   
                                  
                                }

                            }

                            foreach (DataRow dr in dt_FixedAssets.Rows)
                            {
                                string Key_Id = dr["Id"].ToString();
                                string Key_StartPeriod = dr["StartPeriod"].ToString();
                                if (myDic.ContainsKey(Key_Id) && myDic.ContainsKey(Key_StartPeriod))
                                {
                                    dr["Id"] = myDic[Key_Id];
                                    dr["AccountId"] = AccountId;
                                    dr["StartPeriod"] = myDic[Key_StartPeriod];


                                }

                            }
                            foreach (DataRow dr in dt_FixedAssetsChange.Rows)
                            {
                                string Key_ParentId = dr["ParentId"].ToString();
                                string Key_PeriodId = dr["PeriodId"].ToString();
                                if (myDic.ContainsKey(Key_ParentId) && myDic.ContainsKey(Key_PeriodId))
                                {
                                    dr["Id"] = Guid.NewGuid().ToString("N");
                                    dr["AccountId"] = AccountId;
                                    dr["ParentId"] = myDic[Key_ParentId];
                                    dr["PeriodId"] = myDic[Key_PeriodId];


                                }

                            }

                            if (dt_FixedAssetsLog.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt_FixedAssetsLog.Rows)
                                {
                                    string Key_FixedId = dr["FixedId"].ToString();
                                    string Key_PeriodId = dr["PeriodId"].ToString();
                                    dr["Id"] = Guid.NewGuid().ToString("N");
                                    dr["AccountId"] = AccountId;
                                    if (myDic.ContainsKey(Key_FixedId) && myDic.ContainsKey(Key_PeriodId))
                                    {
                                        
                                        dr["FixedId"] = myDic[Key_FixedId];
                                        dr["PeriodId"] = myDic[Key_PeriodId];
                                       
                                    }
                                }
                            }
                            if (dt_DocTPL.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt_DocTPL.Rows)
                                {
                                    string Key_Id = dr["Id"].ToString();
         
                                    if (myDic.ContainsKey(Key_Id))
                                    {
                                        dr["Id"] = myDic[Key_Id];
                                        dr["AccountId"] = AccountId;
                                        dr["UpdateUser"] = userName;
                                        dr["UpdateDate"] = DateTime.Now;

                                    }
                                }
                            }
                            if (dt_TPL2PZ.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt_TPL2PZ.Rows)
                                {
                                    string Key_PZId = dr["PZId"].ToString();
                                    //string Key_TPLId = dr["TPLId"].ToString();
                                    string Key_PeriodId = dr["PeriodId"].ToString();
                                    dr["Id"] = Guid.NewGuid().ToString("N");
                                    dr["AccountId"] = AccountId;
                                    if (myDic.ContainsKey(Key_PeriodId) && myDic.ContainsKey(Key_PZId))
                                    {
                                        dr["PZId"] = myDic[Key_PZId];
                                        //["TPLId"] = myDic[Key_TPLId];
                                        dr["PeriodId"] = myDic[Key_PeriodId];

                                    }
                                }
                            }
                            #endregion



                            newData.Add(dt_YearPeriodInfo);
                            newData.Add(dt_MonthPeriodInfo);
                            newData.Add(dt_Doc);
                            newData.Add(dt_DocDetail);
                            newData.Add(dt_FixedAssets);
                            newData.Add(dt_CaculateHelperItem);
                            newData.Add(dt_CaculateHelperDetail);
                            newData.Add(dt_FGLBalance);
                            newData.Add(dt_GLBalance);
                            newData.Add(dt_CertificateWord);
                            newData.Add(dt_Currency);
                            newData.Add(dt_AccountSubject);
                            newData.Add(dt_PeriodTrack);
                            newData.Add(dt_FixedAssetsChange);
                            newData.Add(dt_SubjectAss);
                            newData.Add(dt_FixedAssetsLog);
                            newData.Add(dt_DocTPL);
                            newData.Add(dt_TPL2PZ);
                            foreach (var lst in newData)
                            {

                                DataSet ds = lst.DataSet;
                                string TableName = lst.TableName;
                                n = TableName;
                                //string AccountId = ds.Tables[0].Rows[0]["AccountId"].ToString();
                                string del_sql = string.Format(@"delete from {0} where AccountId='{1}'", TableName, AccountId);
                                cnn.Execute(del_sql, null, ts);

                                if (ds != null && ds.Tables[0].Rows.Count != 0)
                                {
                                    SqlBulkCopy bulkCopy = new SqlBulkCopy(cnn, SqlBulkCopyOptions.Default,
                               ts);
                                    //SqlBulkCopy bulkCopy = new SqlBulkCopy(cnn.ConnectionString);
                                    bulkCopy.DestinationTableName = TableName;
                                    bulkCopy.BatchSize = ds.Tables[0].Rows.Count;

                                    bulkCopy.WriteToServer(ds.Tables[0]);

                                }
                            }
                        }
                        response.Message = "恢复成功";
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.Message = "文件不存在";
                        response.IsSuccess = false;
                    }
                    ts.Commit();

                    //response.Message = "删除成功";
                    return response;
                }
                catch (Exception ex)
                {
                    response.Message = n;
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseAccountDelete;
                }
            }
        }
        public TKS_FAS_BackUpInfo GetBackUpById(string id)
        {

            using (this.cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var data = cnn.QueryFirstOrDefault<TKS_FAS_BackUpInfo>(
               @"select * from TKS_FAS_BackUpInfo where Id=@Id",
               new { Id = id }, ts);

                    return data;


                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    throw ex;
                }
            }

        }
        /// <summary>
        /// 激活为当前账套
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseAccountActive AccountActive(RequestAccountActive request)
        {
            ResponseAccountActive response = new ResponseAccountActive();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);

                    string sql = @"delete from TKS_FAS_UserCurrentAccount where userId=@UserId";

                    cnn.Execute(sql, new { UserId = user.User.Id }, ts);


                    TKS_FAS_UserCurrentAccount cur = new TKS_FAS_UserCurrentAccount();
                    cur.Id = Guid.NewGuid().ToString("N");
                    cur.UserId = user.User.Id;
                    //cur.AccountId = request.Data.Id;
                    cur.AccountId = request.Data == null ? request.AccountId : request.Data.Id;
                    cnn.Insert<TKS_FAS_UserCurrentAccount>(cur, ts);

                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "激活成功";
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseAccountActive;
                }
            }
        }

        /// <summary>
        /// 获取可操作的账套列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ResponseOPAccountListGet OPAccountListGet(RequestOPAccountListGet request)
        {
            ResponseOPAccountListGet response = new ResponseOPAccountListGet();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGetButAccount(request.Token, ts);
                    var Permission = this.GetPermission(request.Token, ts);
                    List<TKS_FAS_AccountInfoExt> data = new List<TKS_FAS_AccountInfoExt>();
                    string sql = string.Empty;
                    //判断是否是制单员
                    var flag = IsZDY(user);
                    if (flag)
                    {
                        //制单员仅获取账套会计是自己的账套
                        sql = @"select * from TKS_FAS_AccountInfo where AccountantId=@AccountantId";
                        if (!string.IsNullOrEmpty(request.Name))
                        {
                            sql = @"select * from TKS_FAS_AccountInfo where AccountantId=@AccountantId and QY_Name like '%" + request.Name + "%'";
                        }
                        data = cnn.Query<TKS_FAS_AccountInfoExt>(sql, new { AccountantId = user.User.Id }, ts).ToList();

                    }
                    else
                    {   //其他，根据社会信用代码获取
                        if (user.Node.Type == 1)//代帐企业
                        {
                            //自有的并且未被外包  or 已经外包的
                            //sql = @"select * from TKS_FAS_AccountInfo 
                            //    where (DZ_CreditCode=@DZ_CreditCode and isOutSource=0)
                            //    or DL_CreditCode=@DL_CreditCode
                            //    or (wb_status=4 and wb_creditCode=@WB_CreditCode)";
                            //if (!string.IsNullOrEmpty(request.Name))
                            //{
                            //    sql = string.Format(@"select * from TKS_FAS_AccountInfo 
                            //    where (DZ_CreditCode=@DZ_CreditCode and isOutSource=0 and QY_Name like '%{0}%')
                            //    or DL_CreditCode=@DL_CreditCode
                            //    or (wb_status=4 and wb_creditCode=@WB_CreditCode)", request.Name);
                            //}
                            sql = @"select * from TKS_FAS_AccountInfo 
                                where (DZ_CreditCode=@DZ_CreditCode)
                                or DL_CreditCode=@DL_CreditCode
                                or (wb_status=4 and wb_creditCode=@WB_CreditCode)";
                            if (!string.IsNullOrEmpty(request.Name))
                            {
                                sql = string.Format(@"select * from TKS_FAS_AccountInfo 
                                where (DZ_CreditCode=@DZ_CreditCode and QY_Name like '%{0}%')
                                or DL_CreditCode=@DL_CreditCode
                                or (wb_status=4 and wb_creditCode=@WB_CreditCode)", request.Name);
                            }
                            if (Permission.PLevel >= 20)
                            {
                                //普通会计职能看到自己创建的，接收外包的，自己是是主办会计的
                                sql = @"select * from TKS_FAS_AccountInfo 
                                where (DZ_CreditCode=@DZ_CreditCode
                                or DL_CreditCode=@DL_CreditCode
                                or (wb_status=4 and wb_creditCode=@WB_CreditCode))
and (CreateUser=@CreateUser or AccountantId=@AccountantId)
";
                                if (!string.IsNullOrEmpty(request.Name))
                                {
                                    sql = string.Format(@"select * from TKS_FAS_AccountInfo 
                                where ((DZ_CreditCode=@DZ_CreditCode and QY_Name like '%{0}%')
                                or DL_CreditCode=@DL_CreditCode
                                or (wb_status=4 and wb_creditCode=@WB_CreditCode)) and (CreateUser=@CreateUser or AccountantId=@AccountantId)", request.Name);
                                }
                            }
                            data = cnn.Query<TKS_FAS_AccountInfoExt>(sql,
                                new
                                {
                                    DZ_CreditCode = user.Node.CreditCode,
                                    DL_CreditCode = user.Node.CreditCode,
                                    WB_CreditCode = user.Node.CreditCode,
                                    CreateUser = user.User.TrueName,
                                    AccountantId = user.User.Id
                                }, ts).ToList();
                        }
                        else
                        {
                            //雇主企业通过我的账套获取
                            sql = @"select  B.* from TKS_FAS_MyAccount  A,
                                     TKS_FAS_AccountInfo B   
                                where A.AccountId=B.ID and A.NodeId=@NodeId  ";
                            if (!string.IsNullOrEmpty(request.Name))
                            {
                                sql = @"select  B.* from TKS_FAS_MyAccount  A,
                                     TKS_FAS_AccountInfo B   
                                where A.AccountId=B.ID and A.NodeId=@NodeId and B.QY_Name like '%" + request.Name + "%'";
                            }
                            data = cnn.Query<TKS_FAS_AccountInfoExt>(sql, new { NodeId = user.Node.CreditCode }, ts).ToList();
                        }

                    }

                    sql = @"select * from TKS_FAS_UserCurrentAccount where userid=@UserId";
                    var curAccount = cnn.Query<TKS_FAS_UserCurrentAccount>(sql, new { UserId = user.User.Id }, ts).ToList();
                    if (curAccount.Count() == 0 && data.Count() > 0)
                    {
                        data[0].Active = 0;
                        TKS_FAS_MonthPeriodInfo period = GetActivePeriod(data[0].Id, ts);
                        response.IsSelected = false;
                        response.Year = period.Year;
                        response.Month = period.Month;
                        response.PeriodId = period.Id;
                    }
                    else if (curAccount.Count() > 0)
                    {
                        for (int i = 0; i < data.Count(); i++)
                        {
                            if (data[i].Id == curAccount[0].AccountId)
                            {

                                data[i].Active = 1;
                                var period = GetActivePeriod(data[i].Id, ts);
                                response.IsSelected = true;
                                response.Year = period.Year;
                                response.Month = period.Month;
                                response.PeriodId = period.Id;
                                break;
                            }
                        }
                    }

                    ts.Commit();

                    response.IsSuccess = true;
                    response.Data = data;
                    response.SelectAccount = data.Where(x => x.Active == 1).FirstOrDefault();
                    response.UserCreditCode = user.Node.CreditCode;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseOPAccountListGet;
                }
            }
        }

        /// <summary>
        /// 当前激活的账套的会计期间
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public TKS_FAS_MonthPeriodInfo GetActivePeriod(string accountId, IDbTransaction ts)
        {
            var period = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                @"select * from TKS_FAS_MonthPeriodInfo where accountId=@AccountId and isActive=1",
                new { AccountId = accountId }, ts);

            return period;
        }



        public TKS_FAS_MonthPeriodInfo GetFirstPeriod(string accountId, IDbTransaction ts)
        {


            //按时间排序 取第一个
            var monthPeriod = cnn.QueryFirst<TKS_FAS_MonthPeriodInfo>(
                @"select * from TKS_FAS_MonthPeriodInfo where 
                accountId=@AccountId order by  year ,month",
                new { AccountId = accountId }, ts);

            return monthPeriod;

        }


        /// <summary>
        /// 当前账套，第一个会计期间是否结转
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public bool IsFirstPeriodPay(string accountId, IDbTransaction ts)
        {

            var monthPeriod = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                @"select * from TKS_FAS_MonthPeriodInfo where 
                accountId=@AccountId   and ispay=1",
                new { AccountId = accountId }, ts).ToList();

            return monthPeriod.Count() > 0;

        }

        /// <summary>
        /// 获取账套
        /// </summary>
        /// <param name="accountid"></param>
        /// <param name="ts"></param>
        /// <returns></returns>
        public TKS_FAS_AccountInfo GetAccountSetByAccountId(string accountid, IDbTransaction ts)
        {
            string sql = @"select * from TKS_FAS_AccountInfo where id=@Id";
            var data = cnn.Query<TKS_FAS_AccountInfo>(sql, new { Id = accountid }, ts).ToList();
            return data[0];
        }

        /// <summary>
        /// 是否是制单员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool IsZDY(MM_UserInfo user)
        {

            string zdy = ConfigHelper.Read("ZDYRole");//制单员角色ID，可读配置
            bool flag = false;
            if (user.Roles.Count() > 1)
            {
                // throw new AppException(user.User.UserName, "IsZDY", "用户信息异常，拥有多个企业角色", "拥有多个企业角色");
                var role = user.Roles.Where(p => { return p.Type == 1 || p.Type == 2; }).FirstOrDefault();
                if (role == null)
                {
                    return false;
                }
                else if (role.Id == zdy)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (user.Roles[0].Id == zdy)
            {
                flag = true;
            }


            return flag;
        }


        #region 邀请码
        public ResponseAccountInvitationCodeGet InvitationCodeGet(RequestAccountInvitationCodeGet request)
        {
            var res = new ResponseAccountInvitationCodeGet();
            using (cnn = this.GetConnection())
            {
                var ts = cnn.BeginTransaction();


                try
                {
                    //已经邀请了并绑定了企业主的账套，无法再次邀请
                    string sql = @"select * from TKS_FAS_AccountInfo where Id=@Id and isInvitation=1";
                    var account = cnn.QueryFirstOrDefault(sql, new { Id = request.Id }, ts);
                    if (account != null)
                    {
                        throw new NormalException("已经绑定了企业主，无法再次邀请");
                    }
                    sql = "update TKS_FAS_AccountInfo set InvitationCode=@InvitationCode where id=@Id";

                    string code = GenerateRandomNumber(5);
                    cnn.Execute(sql, new { InvitationCode = code, Id = request.Id }, ts);

                    ts.Commit();
                    res.InvitationCode = code;
                    res.IsSuccess = true;
                    res.Message = "";
                    return res;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(res, ex) as ResponseAccountInvitationCodeGet;
                }

            }
        }

        private char[] constant =
      {
        '0','1','2','3','4','5','6','7','8','9',
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
      };
        string GenerateRandomNumber(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }




        #endregion

    }
}

