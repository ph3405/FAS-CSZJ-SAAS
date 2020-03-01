using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using TKS.FAS.BLL;
using TKS.FAS.Entity.SSO;
public partial class Register2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserRegist context = Session["context"] as UserRegist;

        if (context == null)
        {
            Response.Redirect("Register1.aspx");

        }
    }


    bool PassStrongCheck(string pass)
    {
        var regex = new Regex(@"
(?=.*[0-9])                     #必须包含数字
(?=.*[a-zA-Z])                  #必须包含小写或大写字母

.{6,16}                         #至少6个字符，最多16个字符
", RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

        return regex.IsMatch(pass);


    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        int nodeType = int.Parse(selNodeType.SelectedValue);

        if (nodeType == 0)
        {
            if (string.IsNullOrEmpty(txtNodeName.Value))
            {
                lblError.InnerText = "请填写企业名称";
                return;
            }
            if (string.IsNullOrEmpty(txtUCode.Value))
            {
                lblError.InnerText = "请填写社会统一化代码";
                return;
            }
        }

        if (string.IsNullOrEmpty(txtUserName.Value))
        {
            lblError.InnerText = "请填写用户名";
            return;
        }

        if (string.IsNullOrEmpty(txtPassword1.Value) || string.IsNullOrEmpty(txtPassword2.Value))
        {
            lblError.InnerText = "请填写密码";
            return;
        }
        if (txtPassword1.Value.Length < 6 || txtPassword1.Value.Length > 16)
        {
            lblError.InnerText = "密码必须6到16个字符";
            return;
        }
        if (txtPassword1.Value != txtPassword2.Value)
        {
            lblError.InnerText = "密码不一致";
            return;
        }


        if (!PassStrongCheck(txtPassword1.Value))
        {

            lblError.InnerText = "密码必须包含数字、小写或大写字母";
            return;
        }
        UserRegist context = Session["context"] as UserRegist;
        RequestUserRegister request = new RequestUserRegister();

        request.User = new TKS.FAS.Entity.TKS_FAS_User();
        request.User.UserName = txtUserName.Value.Trim();
        request.User.Password = txtPassword1.Value;
        request.User.Sex = selSex.SelectedValue;
        request.User.Mobile = context.Mobile;
        request.User.ZCService = chkZC.Checked ? 1 : 0;
        request.User.Province = txtProvince.Value;
        request.User.City = txtCity.Value;
        request.User.Town = txtTown.Value;
        request.Node = new TKS.FAS.Entity.TKS_FAS_Node();
        request.Node.Name = txtNodeName.Value.Trim();
        request.Node.Type = nodeType;
        request.Node.IsOutSource = int.Parse(selOUT.SelectedValue);
        request.Node.UCode = txtUCode.Value;

        UserBLL bll = new UserBLL();
        var res = bll.UserRegister(request);
        if (res.IsSuccess == true)
        {
            Session.Clear();
            Response.Redirect("Register3.aspx");
        }
        else
        {
            lblError.InnerText = res.Message;
            return;
        }


    }
}