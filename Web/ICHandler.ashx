<%@ WebHandler Language="C#" Class="ICHandler" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using TKS.FAS.BLL;
using TKS.FAS.Entity;
using TKS.FAS.BLL.FAS;

public class ICHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string token = context.Request.Form["token"];
            string companyAddress = context.Request.Form["companyAddress"];
            string companyAddressDetail = context.Request.Form["companyAddressDetail"];
            string Registered_Capital = context.Request.Form["Registered_Capital"];
            string IsNeedICRegister = context.Request.Form["IsNeedICRegister"];
            string IsNeedHPBG = context.Request.Form["IsNeedHPBG"];
            string IsNeedKH = context.Request.Form["IsNeedKH"];
            string IsNeedYBNS = context.Request.Form["IsNeedYBNS"];
            string Company_Type = context.Request.Form["Company_Type"];
            string Bussiness_Range = context.Request.Form["Bussiness_Range"];
            string Legal_Person = context.Request.Form["Legal_Person"];
            string Supervise_Name = context.Request.Form["Supervise_Name"];
            string Contact_Name = context.Request.Form["Contact_Name"];
            string Contact_Phone = context.Request.Form["Contact_Phone"];
            string companyName = context.Request.Form["CompanyName"];
            int count = context.Request.Files.Count;

            string xdpath = "/uploadPic/" + "工商/" + DateTime.Now.Year + "-"
                    + DateTime.Now.Month + "-" + DateTime.Now.Day + "/";

            string uploadPath = HttpContext.Current.Server.MapPath("/uploadPic/");
            uploadPath += "工商\\" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "\\";

            //判断路径是否存在
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            #region 法人身份证
            string legalFrontPath = string.Empty;
            HttpPostedFile legalFrontPic = context.Request.Files["imgLegal1"];
            if (legalFrontPic != null)
            {
                string fileName = Guid.NewGuid().ToString("N") + ".jpg";
                legalFrontPath = xdpath + fileName;
                legalFrontPic.SaveAs(uploadPath + fileName);
            }

            string legalBackPath = string.Empty;
            HttpPostedFile legalBackPic = context.Request.Files["imgLegal2"];
            if (legalBackPic != null)
            {
                string fileName = Guid.NewGuid().ToString("N") + ".jpg";
                legalBackPath = xdpath + fileName;
                legalBackPic.SaveAs(uploadPath + fileName);
            }
            #endregion

            #region 监事身份证
            string Supervise_ID_FrontPath = string.Empty;
            HttpPostedFile Supervise_ID_FrontPic = context.Request.Files["imgSupervise1"];
            if (Supervise_ID_FrontPic != null)
            {
                string fileName = Guid.NewGuid().ToString("N") + ".jpg";
                Supervise_ID_FrontPath = xdpath + fileName;
                Supervise_ID_FrontPic.SaveAs(uploadPath + fileName);
            }
            string Supervise_ID_BackPath = string.Empty;
            HttpPostedFile Supervise_ID_BackPic = context.Request.Files["imgSupervise2"];
            if (Supervise_ID_BackPic != null)
            {
                string fileName = Guid.NewGuid().ToString("N") + ".jpg";
                Supervise_ID_BackPath = xdpath + fileName;
                Supervise_ID_BackPic.SaveAs(uploadPath + fileName);
            }
            #endregion

            #region 房产证
            string POC_Path = string.Empty;
            HttpPostedFile POC_Pic = context.Request.Files["imgFangChan"];
            if (POC_Pic != null)
            {
                string fileName = Guid.NewGuid().ToString("N") + ".jpg";
                POC_Path = xdpath + fileName;
                POC_Pic.SaveAs(uploadPath + fileName);
            }

            string Lease_Agreement_Path = string.Empty;
            HttpPostedFile Lease_Agreement_Pic = context.Request.Files["imgZuLin"];
            if (Lease_Agreement_Pic != null)
            {
                string fileName = Guid.NewGuid().ToString("N") + ".jpg";
                Lease_Agreement_Path = xdpath + fileName;
                Lease_Agreement_Pic.SaveAs(uploadPath + fileName);
            }
            #endregion


            RequestICRegisterAdd request = new RequestICRegisterAdd();

            request.Data = new TKS_FAS_ICRegisterOrder();

            string[] address = companyAddress.Split(new char[] { ' ' });
            request.Token = token;
            request.Data.Province = address[0];
            request.Data.City = address[1];
            request.Data.Town = address[2];
            request.Data.CompanyName = companyName;
            request.Data.CompanyAddress = companyAddressDetail;//详细地址
            request.Data.Registered_Capital = decimal.Parse(Registered_Capital);//注册资金
            request.Data.IsNeedHPBG = IsNeedHPBG;
            request.Data.IsNeedICRegister = IsNeedICRegister;
            request.Data.IsNeedKH = IsNeedKH;
            request.Data.IsNeedYBNS = IsNeedYBNS;
            request.Data.Company_Type = Company_Type;//企业性质
            request.Data.Bussiness_Range = Bussiness_Range;
            request.Data.Legal_Person = Legal_Person;//法人
            request.Data.Legal_ID_FrontPic = legalFrontPath;
            request.Data.Legal_ID_BackPic = legalBackPath;
            request.Data.Supervise_Name = Supervise_Name;//监事
            request.Data.Supervise_ID_FrontPic = Supervise_ID_FrontPath;
            request.Data.Supervise_ID_BackPic = Supervise_ID_BackPath;
            request.Data.Contact_Name = Contact_Name;//注册联系人
            request.Data.Contact_Phone = Contact_Phone;//注册联系人电话
            request.Data.POC_Pic = POC_Path;//房产证附件
            request.Data.Lease_Agreement_Pic = Lease_Agreement_Path;//租赁合同



            ICRegisterBLL bll = new ICRegisterBLL();
            var res = bll.ICRegisterAdd(request);
            if (res.IsSuccess)
            {
                context.Response.Write("");
            }
            else
            {
                context.Response.Write(res.Message);
            }

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