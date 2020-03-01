using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var user = SessionHelper.GetUserInfo();
        if (user == null)
        {
            SessionHelper.Clear();
            app.Visible = false;
            login.Visible = true;

        }
        else
        {
            login.Visible = false;
            app.Visible = true;
        }
    }
}
