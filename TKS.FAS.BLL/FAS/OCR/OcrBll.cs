using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TKS.FAS.Common;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using Dapper;
using DapperExtensions;
using System.Text.RegularExpressions;

using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace TKS.FAS.BLL.FAS
{
    public class OcrBll : CommonBase
    {
        public ResponseOcrInfo GetResponseOcrInfo(RequestOcrInfo request)
        {
            string querys = "";
            //String bodys = "{//图像数据：base64编码，要求base64编码后大小不超过4M，最短边至少15px，最长边最大4096px，支持jpg/png/bmp格式，和url参数只能同时存在一个\"img\":\"\",//图像url地址：图片完整URL，URL长度不超过1024字节，URL对应的图片base64编码后大小不超过4M，最短边至少15px，最长边最大4096px，支持jpg/png/bmp格式，和img参数只能同时存在一个\"url\":\"\"}";
            string bodys = "{\"img\":\"" + request.baseimgData + "\"}";
            string host = ConfigurationManager.AppSettings["ocr_host"];
            string path = ConfigurationManager.AppSettings["ocr_path"];
            string appcode = ConfigurationManager.AppSettings["ocr_appcode"];

            string url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;
            ResponseOcrInfo response = new ResponseOcrInfo();
            try
            {
                if (0 < querys.Length)
                {
                    url = url + "?" + querys;
                }

                if (host.Contains("https://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                }
                else
                {
                    httpRequest = (HttpWebRequest)WebRequest.Create(url);
                }
                httpRequest.Method = "POST";
                httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
                //根据API的要求，定义相对应的Content-Type
                httpRequest.ContentType = "application/json; charset=UTF-8";
                //httpRequest.ContentType = "application/x-www-form-urlencoded;multipart/form-data";
                //application / x - www - form - urlencoded 和 multipart/ form - data
                if (0 < bodys.Length)
                {
                    byte[] data = Encoding.UTF8.GetBytes(bodys);
                    using (Stream stream = httpRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                try
                {
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    httpResponse = (HttpWebResponse)ex.Response;
                }

                string content = string.Empty;
                //Response.Write(httpResponse.StatusCode);
                //Response.Write(httpResponse.Method);
                //Response.Write(httpResponse.Headers);
                Stream st = httpResponse.GetResponseStream();
                using (StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8")))
                {
                    content = reader.ReadToEnd();
                }

                //Response.Write(reader.ReadToEnd());
                //Response.Write("\n");

                //Response.Redirect("OCR_ResultDetail.aspx?baseData=" + content);
                //string decodeContent = System.Web.HttpUtility.UrlEncode(content);
                
                if (!string.IsNullOrWhiteSpace(content))
                {
                    //context.Response.Write("{\"statu\":\"ok\",\"msg\":\"" + decodeContent + "\"}");
                    response.data = "{\"statu\":\"ok\",\"msg\":\"" + content + "\"}";
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                }
                else
                {
                    //context.Response.Write("{\"statu\":\"no\",\"msg\":\"" + decodeContent + "\"}");
                    response.data = "{\"statu\":\"no\",\"msg\":\"" + content + "\"}";
                    response.IsSuccess = true;
                    response.Message = "加载完毕";
                }

                

                return response;
            }
            catch (Exception ex)
            {
                return this.DealException(response, ex) as ResponseOcrInfo;
            }
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

    }
}
