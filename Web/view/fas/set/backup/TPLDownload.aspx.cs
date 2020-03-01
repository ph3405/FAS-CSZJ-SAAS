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
using System.Diagnostics;

public partial class view_fas_set_backup_TPLDownload : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        foreach (Process p in Process.GetProcessesByName("Excel"))
        {
            if (string.IsNullOrEmpty(p.MainWindowTitle))
            {
                p.Kill();
            }
        }
        AccountSetBLL bll = new AccountSetBLL();
        var data = bll.GetBackUpById(Request.QueryString["id"]);
        if (data != null)
        {
            //string uploadPath = HttpContext.Current.Server.MapPath(data.Path);
            if (File.Exists(data.Path))
            {
                //以字符流的形式下载文件 
                FileStream fs = new FileStream(data.Path, FileMode.Open);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.ContentType = "application/vnd.ms-excel";
                //通知浏览器下载文件而不是打开 
                Response.AddHeader("Content-Disposition", "attachment; filename=" + data.Name);
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }

           
        }

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

        int loop = total % pageSize > 0 ? (total / pageSize + 1) : total / pageSize;

        for (int l = 0; l < loop; l++)
        {
            ISheet sheet = workbook.CreateSheet(sheetName + l.ToString());

            if (isColumnWritten == true) //写入DataTable的列名
            {
                IRow row = sheet.CreateRow(0);
                for (j = 0; j < data.Columns.Count; ++j)
                {
                    row.CreateCell(j).SetCellValue(data.Columns[j].ColumnName);
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
                    string val ="";
                    if(data.Rows[i][j].ToString()!="0.00")
                    {
                        val = data.Rows[i][j].ToString();
                    }
                    row.CreateCell(j).SetCellValue(val);
                }
                ++count;
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