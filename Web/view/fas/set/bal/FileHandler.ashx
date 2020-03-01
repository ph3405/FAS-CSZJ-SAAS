<%@ WebHandler Language="C#" Class="FileHandler" %>

using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Web.SessionState;
using System.Drawing;
using System.IO;
using TKS.FAS.BLL.FAS;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;


public class FileHandler : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {

        try
        {

            HttpPostedFile file = context.Request.Files["Filedata"]; //上传文件


            if (file != null)
            {
                var user = SessionHelper.GetUserInfo();

                var dt = ExcelToDataTable("期初0", true, file);

                BALBLL bll = new TKS.FAS.BLL.FAS.BALBLL();
                bll.BALImport(dt, user.Token);

                //下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                context.Response.Write("1");



            }
        }
        catch (Exception ex)
        {
            context.Response.Write("0");
        }
        finally
        {
            GC.Collect();
        }
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    /// <summary>
    /// 将excel中的数据导入到DataTable中
    /// </summary>
    /// <param name="sheetName">excel工作薄sheet的名称</param>
    /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
    /// <returns>返回的DataTable</returns>
    public DataTable ExcelToDataTable(string sheetName, bool isFirstRowColumn,HttpPostedFile fs)
    {
        ISheet sheet = null;
        DataTable data = new DataTable();
        int startRow = 0;
        try
        {
            IWorkbook workbook = null;
            if (fs.FileName.IndexOf(".xlsx") > 0) // 2007版本
                workbook = new XSSFWorkbook(fs.InputStream);
            else if (fs.FileName.IndexOf(".xls") > 0) // 2003版本
                workbook = new HSSFWorkbook(fs.InputStream);

            if (sheetName != null)
            {
                sheet = workbook.GetSheet(sheetName);
                if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                {
                    sheet = workbook.GetSheetAt(0);
                }
            }
            else
            {
                sheet = workbook.GetSheetAt(0);
            }
            if (sheet != null)
            {
                IRow firstRow = sheet.GetRow(0);
                int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                if (isFirstRowColumn)
                {
                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        ICell cell = firstRow.GetCell(i);
                        if (cell != null)
                        {
                            string cellValue = cell.StringCellValue;
                            if (cellValue != null)
                            {
                                DataColumn column = new DataColumn(cellValue);
                                data.Columns.Add(column);
                            }
                        }
                    }
                    startRow = sheet.FirstRowNum + 1;
                }
                else
                {
                    startRow = sheet.FirstRowNum;
                }

                //最后一列的标号
                int rowCount = sheet.LastRowNum;
                for (int i = startRow; i <= rowCount; ++i)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; //没有数据的行默认是null　　　　　　　

                    DataRow dataRow = data.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                    {
                        if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                            dataRow[j] = row.GetCell(j).ToString();
                    }
                    data.Rows.Add(dataRow);
                }
            }

            return data;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
            return null;
        }
    }




}