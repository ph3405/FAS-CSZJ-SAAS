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

public partial class view_fas_set_bal_TPLDownload : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BALBLL bll = new BALBLL();
        var lsData = bll.LsBALGet(this.Token);

        var dt = ToDataTable(lsData, new string[] {"Id","SubjectCode","Name", "CD","IsHelper","calCode","calName",
            "BWBStartBAL", "BWBDebitTotal_Y","BWBCreditTotal_Y" });
        dt.Columns["Id"].SetOrdinal(0);
        dt.Columns["SubjectCode"].SetOrdinal(1);
        dt.Columns["Name"].SetOrdinal(2);
        dt.Columns["CD"].SetOrdinal(3);
        dt.Columns["IsHelper"].SetOrdinal(4);
        dt.Columns["calCode"].SetOrdinal(5);
        dt.Columns["calName"].SetOrdinal(6);
        dt.Columns["BWBStartBAL"].SetOrdinal(7);
        dt.Columns["BWBDebitTotal_Y"].SetOrdinal(8);
        dt.Columns["BWBCreditTotal_Y"].SetOrdinal(9);
        dt.Columns["Id"].ColumnName = "Id";
        dt.Columns["SubjectCode"].ColumnName = "代码";
        dt.Columns["Name"].ColumnName = "名称";
        dt.Columns["CD"].ColumnName = "方向";

        dt.Columns["IsHelper"].ColumnName = "辅助核算项";
        dt.Columns["calCode"].ColumnName = "辅助核算编码";
        dt.Columns["calName"].ColumnName = "辅助核算名称";

        dt.Columns["BWBStartBAL"].ColumnName = "期初余额";
        dt.Columns["BWBDebitTotal_Y"].ColumnName = "借方累计";
        dt.Columns["BWBCreditTotal_Y"].ColumnName = "贷方累计";

        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel");
        path += "\\期初导入" + DateTime.Now.ToString("yyyy-MM-dd") + Guid.NewGuid().ToString() + ".xls";
        DataTableToExcel(path, dt, "期初", 1000, true);
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