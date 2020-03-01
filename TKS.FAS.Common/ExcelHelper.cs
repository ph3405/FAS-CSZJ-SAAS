using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace TKS.FAS.Common
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 将List转换成DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                //dt.Columns.Add(property.Name, property.PropertyType);
                dt.Columns.Add(property.Name);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }

        public static List<DataTable> ReadExcelToList(string path, bool hasTitle = false)
        {
            string fileType = System.IO.Path.GetExtension(path);
            //连接字符串
            /*string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";*/ // Office 07及以上版本 不能出现多余的空格 而且分号注意
            string connstring = string.Format("Provider=Microsoft.Jet.OLEDB.{0}.0;" +
                            "Extended Properties=\"Excel {1}.0;HDR={2};IMEX=1;\";" +
                            "data source={3};",
                            (fileType == ".xls" ? 4 : 12), (fileType == ".xls" ? 8 : 12), (hasTitle ? "Yes" : "NO"), path);
            List<DataTable> list = new List<DataTable>();
            DataTable dtSheet = GetAllSheet(connstring);
            if (dtSheet.Rows.Count>0)
            {
                for (int i = 0; i < dtSheet.Rows.Count; i++)
                {
                    DataTable dt = ReadExcelToTable(connstring, dtSheet.Rows[i][2].ToString().Trim());
                    if (dt.Rows.Count > 0)
                    {
                        list.Add(dt);
                    }
                }
            }
            return list;


        }
        public static DataTable ReadExcelToTable(string connstring, string sheetsName)//excel存放的路径
        {
            try
            {
                DataTable dt = new DataTable();
                using (OleDbConnection conn = new OleDbConnection(connstring))
                {
                    conn.Open();
                    string sql = string.Format("SELECT * FROM [{0}]", sheetsName); //查询字符串

                    OleDbCommand oleDbCommand = new OleDbCommand(sql, conn);
                    OleDbDataAdapter ada = new OleDbDataAdapter(oleDbCommand);
                    DataSet set = new DataSet();
                    ada.Fill(set);
                    conn.Close();

                    conn.Dispose();
                    dt = set.Tables[0];
                    dt.TableName = sheetsName.Replace("$", "");
                    return dt;

                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static DataTable GetAllSheet(string connstring)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(connstring))
                {
                    conn.Open();
                    DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null); //得到所有sheet的名字
                    conn.Close();

                    conn.Dispose();
                    //return set.Tables[0];
                    return sheetsName;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}
