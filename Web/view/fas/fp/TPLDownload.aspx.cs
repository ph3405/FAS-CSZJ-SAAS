using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TKS.FAS.BLL;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text;
using NPOI.HPSF;

public partial class view_fas_set_bal_TPLDownload : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string columnName = "";
        string columnTitle = "";
        BALBLL bll = new BALBLL();
        string sql = @"
SELECT A.InvoiceDate AS 发票日期, A.InvoiceNo AS 发票号码, A.Memo AS 备注
	        , CASE 
		        WHEN A.IsTaxYZ = '1' THEN fp.Name
		        ELSE 'NUN'
	        END AS 业务类型
	        , CASE 
		        WHEN A.IsTaxYZ = '1' THEN yn.Name
		        ELSE 'NUN'
	        END AS 增值税专用发票
	        , CASE 
		        WHEN A.IsTaxYZ = '1' and rp.Name!='请选择' THEN rp.Name
		        ELSE 'NUN'
	        END AS 收付状态
	        , CASE 
		        WHEN A.IsTaxYZ = '1' and zf.Name!='请选择' THEN zf.Name
		        ELSE 'NUN'
	        END AS 支付方式
	        , CASE 
		        WHEN A.IsTaxYZ = '1' THEN CONVERT(varchar(50),A.Money)
		        ELSE 'NUN'
	        END AS 含税金额
	        , CASE 
		        WHEN A.IsTaxYZ = '1' and A.IsVAT=1 THEN CONVERT(varchar(50),A.TaxMoney)
		        ELSE 'NUN'
	        END AS 税金金额
	        , CASE 
		        WHEN A.IsUse = '1' THEN CONVERT(varchar(50),A.Money)
		        ELSE 'NUN'
	        END AS 含税金额
	        , CASE 
		        WHEN A.IsUse = '1' THEN A.SFType
		        ELSE 'NUN'
	        END AS 收付类型
	        , CASE 
		        WHEN A.IsUse = '1' THEN B.Name
		        ELSE 'NUN'
	        END AS 供应商客户
	        , CASE 
		        WHEN A.IsUse = '1' THEN det.SFDate
		        ELSE 'NUN'
	        END AS 收付日期
	        , CASE 
		        WHEN A.IsUse = '1' THEN CONVERT(varchar(50),det.SFMoney)
		        ELSE 'NUN'
	        END AS 收付金额
	        , CASE 
		        WHEN A.IsUse = '1' THEN det.SFStatus
		        ELSE 'NUN'
	        END AS 细项收付状态
	        , CASE 
		        WHEN A.IsUse = '1' THEN det.SFRemark
		        ELSE ''
	        END AS 细项备注
	        , CASE 
		        WHEN  A.IsUse = '1' and A.Money - s_det.SFMoney!=0 THEN CONVERT(varchar(50),A.Money - s_det.SFMoney)
		        ELSE '0'
	        END AS [坏账金额(指整张发票)]
        FROM TKS_FAS_Invoice A
	        LEFT JOIN TKS_FAS_DATA fp
	        ON fp.code = A.Type
		        AND fp.ParentId = '001'
	        LEFT JOIN TKS_FAS_DATA yn
	        ON yn.code =CONVERT(varchar(50), A.IsVAT)
		        AND yn.ParentId = '200'
	        LEFT JOIN TKS_FAS_DATA zf
	        ON zf.code = CONVERT(varchar(50),A.PayMode)
		        AND zf.ParentId = '100'
	        LEFT JOIN TKS_FAS_DATA rp
	        ON rp.code = CONVERT(varchar(50),A.RPStatus)
		        AND rp.ParentId = '300'
	        LEFT JOIN TKS_FAS_BasicData B ON B.Id = A.BasicDataId
	        LEFT JOIN TKS_FAS_InvoiceSFDetail det ON det.InvoiceId = A.Id
	        LEFT JOIN (
		        SELECT InvoiceId, SUM(SFMoney) AS SFMoney
		        FROM TKS_FAS_InvoiceSFDetail
		        GROUP BY InvoiceId
	        ) s_det
	        ON s_det.InvoiceId = A.Id
        WHERE A.AccountId = '{0}'
        order by CONVERT(datetime, A.InvoiceDate), A.InvoiceNo,CONVERT(datetime, det.SFDate)";
        DataTable table = bll.getDT(sql,this.Token);
        foreach (DataColumn dc in table.Columns)
        {
            columnName = columnName + dc.Caption + ",";

            //columnTitle = columnTitle + getLabel(dc.Caption) + ",";
            columnTitle = columnTitle + dc.Caption + ",";
        }
        if (columnName.EndsWith(","))
        {
            columnName = columnName.Substring(0, columnName.Length - 1);
        }

        if (columnTitle.EndsWith(","))
        {
            columnTitle = columnTitle.Substring(0, columnTitle.Length - 1);
        }
        string filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel");
        filepath += "\\发票导出" + DateTime.Now.ToString("yyyy-MM-dd") + Guid.NewGuid().ToString() + ".xls";
        //string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Temp\\";
        //filepath = filepath + getLabel(name) + "-" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + ".xls";
        WriteToFile(table, "发票导出", "发票导出", columnName.Split(','), columnTitle.Split(','), filepath);

    }
    public void WriteToFile(DataTable table, string headerText, string sheetName, string[] columnName, string[] columnTitle, string filepath)
    {
        //Write the stream data of workbook to the root directory
        string strFields = "";


        foreach (DataColumn dc in table.Columns)
        {
            strFields = strFields + dc.Caption + ",";
        }
        if (strFields.EndsWith(","))
        {
            strFields = strFields.Substring(0, strFields.Length - 1);
        }
        XSSFWorkbook hssfworkbook = GenerateData(table, headerText, sheetName, columnName, columnTitle);
        FileInfo fi = new FileInfo(filepath);
        if (!fi.Directory.Exists)
        {
            fi.Directory.Create();
        }
       
        FileStream file = fi.OpenWrite();
        hssfworkbook.Write(file);
        var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        byte[] buffer = new byte[fs.Length];
        BinaryReader r = new BinaryReader(fs);

        r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开

        buffer = r.ReadBytes((int)r.BaseStream.Length);
        Context.Response.Clear();
        Context.Response.Buffer = true;
        Context.Response.AddHeader("Pragma", "No-cache");
        Context.Response.AddHeader("Cache-Control", "no-cache");

        Context.Response.ContentEncoding = Encoding.Default;
        Context.Response.ContentType = "application/vnd.ms-excel";
        Context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "发票" + ".xls"));
        Context.Response.BinaryWrite(buffer);
        Context.Response.Flush();
        Context.Response.Close();
        if (fi.Exists)
        {
            // fi.Delete();
        }
        file.Close();

        //return filepath;
    }
    public static XSSFWorkbook GenerateData(DataTable table, string headerText, string sheetName, string[] columnName, string[] columnTitle)
    {
        XSSFWorkbook hssfworkbook = new XSSFWorkbook();
        ISheet sheet = hssfworkbook.CreateSheet(sheetName);

        #region 设置文件属性信息

        //创建一个文档摘要信息实体。
        DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
        dsi.Company = ""; //公司名称 

        //创建一个摘要信息实体。
        SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
        si.Subject = "";
        si.Author = " ";
        si.Title = headerText;
        si.Subject = headerText;
        si.CreateDateTime = DateTime.Now;

        #endregion

        ICellStyle dateStyle = hssfworkbook.CreateCellStyle();
        IDataFormat format = hssfworkbook.CreateDataFormat();
        dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

        IFont fontdate = hssfworkbook.CreateFont();
        fontdate.FontHeightInPoints = 10;
        fontdate.FontName = "Arial";
        dateStyle.SetFont(fontdate);
        #region 取得列宽

        int[] colWidth = new int[columnName.Length];
        for (int i = 0; i < columnName.Length; i++)
        {
            colWidth[i] = Encoding.GetEncoding(936).GetBytes(columnTitle[i]).Length;
        }
        for (int i = 0; i < table.Rows.Count; i++)
        {
            for (int j = 0; j < columnName.Length; j++)
            {
                int intTemp = Encoding.GetEncoding(936).GetBytes(table.Rows[i][columnName[j]].ToString()).Length;
                if (intTemp > colWidth[j])
                {
                    colWidth[j] = intTemp;
                }
            }
        }

        #endregion

        int rowIndex = 0;
        foreach (DataRow row in table.Rows)
        {
            #region 新建表，填充表头，填充列头，样式
            if (rowIndex == 65535 || rowIndex == 0)
            {
                if (rowIndex != 0)
                {
                    sheet = hssfworkbook.CreateSheet(sheetName + ((int)rowIndex / 65535).ToString());
                }

                #region 表头及样式
                //if (!string.IsNullOrEmpty(headerText))
                {
                    IRow headerRow = sheet.CreateRow(0);
                    headerRow.HeightInPoints = 25;
                    headerRow.CreateCell(0).SetCellValue(headerText);

                    ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                    headStyle.Alignment = HorizontalAlignment.Center;
                    IFont font = hssfworkbook.CreateFont();
                    font.FontHeightInPoints = 20;
                    font.Boldweight = 700;
                    font.FontName = "Arial";
                    headStyle.SetFont(font);

                    headerRow.GetCell(0).CellStyle = headStyle;
                    //sheet.AddMergedRegion(new Region(0, 0, 0, dtSource.Columns.Count - 1)); 
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
                }
                #endregion
                #region 列头及样式(发票主信息，税金预知，应收应付)
                {
                    //HSSFRow headerRow = sheet.CreateRow(1); 
                    IRow headerRow;

                    headerRow = sheet.CreateRow(1);
                
                    ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                    headStyle.Alignment = HorizontalAlignment.Center;
                    IFont font = hssfworkbook.CreateFont();
                    font.FontHeightInPoints = 10;
                    font.Boldweight = 700;
                    font.FontName = "Arial";
                    headStyle.SetFont(font);

                    headerRow.CreateCell(0).SetCellValue("发票主信息");
                    headerRow.GetCell(0).CellStyle = headStyle;
                    headerRow.CreateCell(3).SetCellValue("税金预知");
                    headerRow.GetCell(3).CellStyle = headStyle;
                    headerRow.CreateCell(9).SetCellValue("应收应付");
                    headerRow.GetCell(9).CellStyle = headStyle;
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, 2));
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 3, 8));
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 9, table.Columns.Count - 1));
                }
                #endregion
                #region 列头及样式
                {
                    //HSSFRow headerRow = sheet.CreateRow(1); 
                    IRow headerRow;
                    //if (!string.IsNullOrEmpty(headerText))
                    //{
                    //    headerRow = sheet.CreateRow(0);
                    //}
                    //else
                    //{
                    //headerRow = sheet.CreateRow(1);
                    headerRow = sheet.CreateRow(2);
                    //}
                    ICellStyle headStyle = hssfworkbook.CreateCellStyle();
                    headStyle.Alignment = HorizontalAlignment.Center;
                    IFont font = hssfworkbook.CreateFont();
                    font.FontHeightInPoints = 10;
                    font.Boldweight = 700;
                    font.FontName = "Arial";
                    headStyle.SetFont(font);

                    for (int i = 0; i < columnName.Length; i++)
                    {
                        headerRow.CreateCell(i).SetCellValue(columnTitle[i]== "含税金额1" ? "含税金额" : columnTitle[i]);
                        headerRow.GetCell(i).CellStyle = headStyle;
                        //设置列宽 
                        if ((colWidth[i] + 1) * 256 > 30000)
                        {
                            sheet.SetColumnWidth(i, 10000);
                        }
                        else
                        {
                            sheet.SetColumnWidth(i, (colWidth[i] + 1) * 256);
                        }

                    }
                    /* 
                    foreach (DataColumn column in dtSource.Columns) 
                    { 
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName); 
                        headerRow.GetCell(column.Ordinal).CellStyle = headStyle; 

                        //设置列宽    
                        sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256); 
                    } 
                     * */
                }
                #endregion
                //if (!string.IsNullOrEmpty(headerText))
                //{
                //    rowIndex = 1;
                //}
                //else
                //{
                rowIndex = 2+1;
                //}

            }
            #endregion

            #region 填充数据
            ICellStyle dataStyle = hssfworkbook.CreateCellStyle();

            IFont fontdata = hssfworkbook.CreateFont();
            fontdata.FontHeightInPoints = 10;
            fontdata.FontName = "Arial";
            dataStyle.SetFont(fontdata);
            IRow dataRow = sheet.CreateRow(rowIndex);
            for (int i = 0; i < columnName.Length; i++)
            {
                ICell newCell = dataRow.CreateCell(i);
                newCell.CellStyle = dataStyle;
                string drValue = row[columnName[i]].ToString();

                switch (table.Columns[columnName[i]].DataType.ToString())
                {
                    case "System.String"://字符串类型   
                        if (drValue.ToUpper() == "TRUE")
                            newCell.SetCellValue("是");
                        else if (drValue.ToUpper() == "FALSE")
                            newCell.SetCellValue("否");
                        newCell.SetCellValue(drValue);
                        break;
                    case "System.DateTime"://日期类型    
                        DateTime dateV;
                        DateTime.TryParse(drValue, out dateV);
                        newCell.SetCellValue(dateV);

                        newCell.CellStyle = dateStyle;//格式化显示    
                        break;
                    case "System.Boolean"://布尔型    
                        bool boolV = false;
                        bool.TryParse(drValue, out boolV);
                        if (boolV)
                            newCell.SetCellValue("是");
                        else
                            newCell.SetCellValue("否");
                        break;
                    case "System.Int16"://整型    
                    case "System.Int32":
                    case "System.Int64":
                    case "System.Byte":
                        int intV = 0;
                        int.TryParse(drValue, out intV);
                        newCell.SetCellValue(intV);
                        break;
                    case "System.Decimal"://浮点型    
                    case "System.Double":
                        double doubV = 0;
                        double.TryParse(drValue, out doubV);
                        newCell.SetCellValue(doubV);
                        break;
                    case "System.DBNull"://空值处理    
                        newCell.SetCellValue("");
                        break;
                    default:
                        newCell.SetCellValue("");
                        break;
                }

            }

            #endregion

            rowIndex++;
        }

        return hssfworkbook;
    }
    public void DataTableToExcel(string fileName, DataTable data, string sheetName, int pageSize, bool isColumnWritten)
    {
        int i = 0;
        int j = 0;
        int count = 0;

        IWorkbook workbook = null;
        
        var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        if (fileName.IndexOf(".xlsx") > 0) // 2007版本
            workbook = new XSSFWorkbook();
        else if (fileName.IndexOf(".xls") > 0) // 2003版本
            workbook = new HSSFWorkbook();
        if (workbook == null) return;
        int total = data.Rows.Count;
        ICellStyle style = workbook.CreateCellStyle();
        style.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
        style.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
        int loop = total % pageSize > 0 ? (total / pageSize + 1) : total / pageSize;
        
        for (int l = 0; l < loop; l++)
        {
            ISheet sheet = workbook.CreateSheet(sheetName + l.ToString());
            
            if (isColumnWritten == true) //写入DataTable的列名
            {
                IRow row = sheet.CreateRow(0);
                
                for (j = 0; j < data.Columns.Count; ++j)
                {
                    var cellHeard = row.CreateCell(j);
                    cellHeard.CellStyle = style;
                    //row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
                    cellHeard.SetCellValue(data.Columns[j].ColumnName);
                }
                count = 1;
            }
            else
            {
                count = 0;
            }
            int dataNum = (l + 1) * pageSize <= total ? (l + 1) * pageSize : total;
            for (i = l * pageSize; i < dataNum; i++)
            {
                IRow row = sheet.CreateRow(count);
                for (j = 0; j < data.Columns.Count; ++j)
                {
                    if (j == 0)
                    {
                        //add by Hero.Zhang 隐藏第一列
                        sheet.SetColumnHidden(j, true);
                    }

                    string val = "";
                    if (data.Rows[i][j].ToString() != "0.00")
                    {
                        val = data.Rows[i][j].ToString();
                    }
                    var cellNum = row.CreateCell(j);
                    cellNum.CellStyle = style;
                    //row.CreateCell(j).SetCellValue(val);
                    cellNum.SetCellValue(val);
                }
            ++count;
            }
            //列宽自适应，只对英文和数字有效
            for (int h = 0; h<= data.Rows.Count; h++)
            {
                sheet.AutoSizeColumn(h);
            }
            //获取当前列的宽度，然后对比本列的长度，取最大值
            for (int columnNum = 0; columnNum <= data.Columns.Count; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
            }
        }
       
        workbook.Write(fs); //写入到excel
        

        byte[] buffer = new byte[fs.Length];
        BinaryReader r = new BinaryReader(fs);

        r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开

        buffer = r.ReadBytes((int)r.BaseStream.Length);
        Context.Response.Clear();
        Context.Response.Buffer = true;
        Context.Response.AddHeader("Pragma", "No-cache");
        Context.Response.AddHeader("Cache-Control", "no-cache");

        Context.Response.ContentEncoding = Encoding.Default;
        Context.Response.ContentType = "application/vnd.ms-excel";
        Context.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}",  "期初导入模板" + ".xls"));
        Context.Response.BinaryWrite(buffer);
        Context.Response.Flush();
        Context.Response.Close();

    }

    /// <summary>    
    /// 将泛型集合类转换成DataTable    

    /// </summary>    
    /// <typeparam name="T">集合项类型</typeparam>    

    /// <param name="list">集合</param>    
    /// <returns>数据集(表)</returns>    
    public static DataTable ToDataTable<T>(IList<T> list)
    {
        return ToDataTable<T>(list, null);

    }


    /**/

    /// <summary>    
    /// 将泛型集合类转换成DataTable    
    /// </summary>    
    /// <typeparam name="T">集合项类型</typeparam>    
    /// <param name="list">集合</param>    
    /// <param name="propertyName">需要返回的列的列名</param>    
    /// <returns>数据集(表)</returns>    
    public static DataTable ToDataTable<T>(IList<T> list, params string[] propertyName)
    {
        List<string> propertyNameList = new List<string>();
        if (propertyName != null)
            propertyNameList.AddRange(propertyName);
        DataTable result = new DataTable();
        if (list.Count > 0)
        {
            PropertyInfo[] propertys = list[0].GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                if (propertyNameList.Count == 0)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
                else
                {
                    if (propertyNameList.Contains(pi.Name))
                        result.Columns.Add(pi.Name, pi.PropertyType);
                }
            }

            for (int i = 0; i < list.Count; i++)
            {
                ArrayList tempList = new ArrayList();
                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                    }
                }
                object[] array = tempList.ToArray();
                result.LoadDataRow(array, true);
            }
        }
        return result;

    }
}