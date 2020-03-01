using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TKS.FAS.Common;
public partial class MainBusiness : BasePage
{
    public string PhotoPath
    {
        get;set;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Sex == "0")
            {
                PhotoPath = "images/woman.png";
            }
            else
            {
                PhotoPath = "images/man.png";
            }
        }
    }

    protected void logout_Click(object sender, EventArgs e)
    {
        SessionHelper.Clear();
        Response.Redirect("~/LoginBusiness.aspx");
    }
}