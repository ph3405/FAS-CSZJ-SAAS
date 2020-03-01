<%@ WebHandler Language="C#" Class="appHandler" %>

using System;
using System.Web;
using System.IO;
using System.Collections.Generic;
using TKS.FAS.BLL;
using TKS.FAS.Entity;
using TKS.FAS.BLL.FAS;
public class appHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string token = context.Request.Form["token"];
            string accountId = context.Request.Form["accountId"];
            string invoiceNo = context.Request.Form["invoiceNo"];
            string isVAT = context.Request.Form["isVAT"];
            string payMode = context.Request.Form["payMode"];
            string rpStatus = context.Request.Form["rpStatus"];
            string money = context.Request.Form["money"];
            string taxmoney = context.Request.Form["taxmoney"];
            string type = context.Request.Form["type"];
            int count = context.Request.Files.Count;

            string xdpath = "/uploadPic/" + "发票/" + DateTime.Now.Year + "-"
                    + DateTime.Now.Month + "-" + DateTime.Now.Day + "/";

            string uploadPath = HttpContext.Current.Server.MapPath("/uploadPic/");
            uploadPath += "发票\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "\\";

            //判断路径是否存在
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            TKS_FAS_Invoice invoice = new TKS_FAS_Invoice();
            List<TKS_FAS_InvoiceAttach> lsPic = new List<TKS_FAS_InvoiceAttach>();

            invoice.AccountId = accountId;
            invoice.InvoiceNo = invoiceNo;
            invoice.Type = int.Parse(type);
            invoice.InvoiceNum = count;
            invoice.IsVAT = int.Parse(isVAT);
            invoice.PayMode = int.Parse(payMode);
            invoice.AccountId = accountId;
            invoice.RPStatus = int.Parse(rpStatus);
            invoice.Money = decimal.Parse(money);
            if (invoice.IsVAT == 1)
            {
                invoice.TaxMoney = decimal.Parse(taxmoney);
            }

            var allKeys = context.Request.Files.AllKeys;
            for (var i = 0; i < allKeys.Length; i++)
            {
                HttpPostedFile _f = context.Request.Files[allKeys[i]];
                string newfilename = Guid.NewGuid().ToString("N") + ".jpg";

                string savePath = uploadPath + newfilename;
                var path = xdpath + newfilename;
                _f.SaveAs(savePath);
                TKS_FAS_InvoiceAttach attach = new TKS_FAS_InvoiceAttach();
                attach.Title = "pic" + i;
                attach.Path = path;
                lsPic.Add(attach);
            }
            InvoiceBLL bll = new InvoiceBLL();
            bll.MInvoiceAdd(token, invoice, lsPic);
          
            context.Response.Write("");
        }
        catch (Exception ex)
        {
            context.Response.Write("提交异常：" + ex.Message);
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

}