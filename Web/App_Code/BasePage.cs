using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TKS.FAS.Common;

/// <summary>
/// BasePage 的摘要说明
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public string Token { get; set; }

    public string FuncId { get; set; }

    public string TrueName { get; set; }

    public string UserName { get; set; }

    public string Sex { get; set; }

    public string NodeName { get; set; }

    public string Id { get; set; }

    public BasePage()
    {
       
    }

    protected override void OnInit(EventArgs e)
    {

        var user = SessionHelper.GetUserInfo();
        if (user == null)
        {
            SessionHelper.Clear();
            Response.Redirect("~/Login.aspx");
        }
        else
        {
            TrueName = user.TrueName;
            Token = user.Token;
            UserName = user.UserName;
            Sex = user.Sex;
            NodeName = user.NodeName;
            Id = user.Id;
            FuncId = ConfigHelper.Read("funcId");
        }
        base.OnInit(e);
    }
}