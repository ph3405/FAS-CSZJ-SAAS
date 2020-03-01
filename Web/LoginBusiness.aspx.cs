using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TKS.FAS.BLL;

public partial class LoginBusiness : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        UserBLL bll = new UserBLL();
        var res = bll.WXLogin(new TKS.FAS.Entity.SSO.RequestUserLogin
        {
            Mobile = txtMobile.Value.Trim(),
            VerCode = txtVerCode.Value.Trim()
        });

        if (res.IsSuccess == true)
        {
            UserSession u = new UserSession();
            u.Id = res.user.Id;
            u.Token = res.Token;
            u.TrueName = res.user.TrueName;
            u.UserName = res.user.UserName;
            u.Sex = res.user.Sex;
            //u.NodeName = res.user.NodeName;
            SessionHelper.SetUserInfo(u);
            //Response.Redirect("main.aspx");
            Response.Redirect("MainBusiness.aspx");
        }
        else
        {
            lbError.Text = res.Message;
        }
    }
}