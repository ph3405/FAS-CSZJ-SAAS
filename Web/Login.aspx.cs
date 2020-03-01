using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TKS.FAS.BLL;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        UserBLL bll = new UserBLL();
        var res = bll.UserLogin(new TKS.FAS.Entity.SSO.RequestUserLogin
        {
            UserName = txtUserName.Value.Trim(),
            Password = txtPass.Value.Trim(),
            Mobile = txtUserName.Value.Trim()
        });

        if (res.IsSuccess == true)
        {
            UserSession u = new UserSession();
            u.Id = res.Id;
            u.Token = res.Token;
            u.TrueName = res.TrueName;
            u.UserName = res.UserName;
            u.Sex = res.Sex;
            u.NodeName = res.NodeName;
            SessionHelper.SetUserInfo(u);
            Response.Redirect("main.aspx");
           // Response.Redirect("MainBusiness.aspx");
        }
        else
        {
            lbError.Text = res.Message;
        }
    }
}