<%@ WebService Language="C#" Class="FASService" %>

using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.BLL;
using Aliyun.Acs.Sms;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Sms.Transform;
using Aliyun.Acs.Sms.Model.V20160927;
using Aliyun.Acs.Core.Utils;
using Aliyun.Acs.Core.Http;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
[System.Web.Script.Services.ScriptService]
public class FASService : System.Web.Services.WebService
{

    /// <summary>
    /// 短信发送
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public ResponseSendMessage FindPassMessage(RequestSendMessage request)
    {
        ResponseSendMessage response = new ResponseSendMessage();
        try
        {
            if (string.IsNullOrEmpty(request.MobilePhone))
            {
                return new ResponseSendMessage { IsSuccess = false, Message = "手机号不能为空" };
            }
            UserBLL bll = new UserBLL();
            if (!bll.IsMobileExist(request.MobilePhone))
            {
                return new ResponseSendMessage { IsSuccess = false, Message = "手机号不存在" };
            }

            string vcode = GenerateRandomNumber(4);
            IClientProfile profile = DefaultProfile.GetProfile("", "", "");
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendSmsRequest req = new SingleSendSmsRequest();
            req.SignName = "章小算";//"管理控制台中配置的短信签名（状态必须是验证通过）"
            req.TemplateCode = "SMS_115165077";//管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）"
            req.RecNum = request.MobilePhone;//"接收号码，多个号码可以逗号分隔"
            req.ParamString = "{\"code\":\"" + vcode + "\"}";//切记前面的code字段，只能是英文，不能是中文！！短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。"
            SingleSendSmsResponse httpResponse = client.GetAcsResponse(req);

            UserRegist context = new UserRegist();
            context.Mobile = request.MobilePhone;
            context.VerCode = vcode;

            Session["context"] = context;
            response.IsSuccess = true;

        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message; ;
        }

        return response;
    }

    /// <summary>
    /// 短信发送
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public ResponseSendMessage SendMessage(RequestSendMessage request)
    {
        ResponseSendMessage response = new ResponseSendMessage();
        try
        {
            if (string.IsNullOrEmpty(request.MobilePhone))
            {
                return new ResponseSendMessage { IsSuccess = false, Message = "手机号不能为空" };
            }
            UserBLL bll = new UserBLL();
            if (bll.IsMobileExist(request.MobilePhone))
            {
                return new ResponseSendMessage { IsSuccess = false, Message = "手机号已经存在" };
            }

            string vcode = GenerateRandomNumber(4);
            IClientProfile profile = DefaultProfile.GetProfile("", "", "");
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendSmsRequest req = new SingleSendSmsRequest();
            req.SignName = "章小算";//"管理控制台中配置的短信签名（状态必须是验证通过）"
            req.TemplateCode = "SMS_115165077";//管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）"
            req.RecNum = request.MobilePhone;//"接收号码，多个号码可以逗号分隔"
            req.ParamString = "{\"code\":\"" + vcode + "\"}";//切记前面的code字段，只能是英文，不能是中文！！短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。"
            SingleSendSmsResponse httpResponse = client.GetAcsResponse(req);

            UserRegist context = new UserRegist();
            context.Mobile = request.MobilePhone;
            context.VerCode = vcode;

            Session["context"] = context;
            response.IsSuccess = true;

        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message; ;
        }

        return response;
    }
    private char[] constant =
 {
        '0','1','2','3','4','5','6','7','8','9'
        };
    public string GenerateRandomNumber(int Length)
    {
        System.Text.StringBuilder newRandom = new System.Text.StringBuilder(10);
        Random rd = new Random();
        for (int i = 0; i < Length; i++)
        {
            newRandom.Append(constant[rd.Next(10)]);
        }
        return newRandom.ToString();
    }

    public class RequestSendMessage
    {
        public string MobilePhone { get; set; }
    }
    public class ResponseSendMessage : ResponseBase
    {

    }

    /// <summary>
    /// 发送邀请码
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [WebMethod]
    public ResponseAccountInvitationSend InvitationSend(RequestAccountInvitationSend request)
    {
        var res = new ResponseAccountInvitationSend();

        try
        {
            if (string.IsNullOrEmpty(request.MobilePhone))
            {
                throw new Exception ( "手机号不能为空" );
            }


            string vcode = request.Code;
            IClientProfile profile = DefaultProfile.GetProfile("", "", "");
            IAcsClient client = new DefaultAcsClient(profile);
            SingleSendSmsRequest req = new SingleSendSmsRequest();
            req.SignName = "章小算";//"管理控制台中配置的短信签名（状态必须是验证通过）"
            req.TemplateCode = "SMS_115165077";//管理控制台中配置的审核通过的短信模板的模板CODE（状态必须是验证通过）"
            req.RecNum = request.MobilePhone;//"接收号码，多个号码可以逗号分隔"
            req.ParamString = "{\"code\":\"" + vcode + "\"}";//切记前面的code字段，只能是英文，不能是中文！！短信模板中的变量；数字需要转换为字符串；个人用户每个变量长度必须小于15个字符。"
            SingleSendSmsResponse httpResponse = client.GetAcsResponse(req);




            res.IsSuccess = true;
            res.Message = "";

        }
        catch (Exception ex)
        {
            res.IsSuccess = false;
            res.Message = ex.Message;
        }

        return res;
    }


}

