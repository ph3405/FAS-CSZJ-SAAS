using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FindPass1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void btnNext_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtMobile.Value))
        {
            lblError.InnerText = "请输入手机号";
            return;
        }

        UserRegist context = Session["context"] as UserRegist;

        if (context == null)
        {
            lblError.InnerText = "请输入合理的验证码";
            return;
        }

        if (context.Mobile == txtMobile.Value && context.VerCode == txtVerCode.Value)
        {
            Response.Redirect("findPass2.aspx");
        }
        else
        {
            lblError.InnerText = "请输入合理的验证码";
        }
    }
}