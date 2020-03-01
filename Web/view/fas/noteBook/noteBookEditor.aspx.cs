using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TKS.FAS.BLL.FAS;
using TKS.FAS.Entity;

public partial class view_fas_noteBookEditor : BasePage
{

    public string Id
    {
        get { return this.hidId.Value; }
        set { this.hidId.Value = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (!IsPostBack)
        {
            Id = Request.QueryString["id"];

            if (!string.IsNullOrEmpty(Id))
            {
                LoadData(Id);
            }

        }
    }

    private void LoadData(string id)
    {
        NewsBLL bll = new NewsBLL();
        var data = bll.Get(id);
        this.txtTitle.Value = data.title;

        this.txtBody.Value = data.content;
        this.txtSort.Value = data.Sort.ToString();
        //this.selStatus.Value = data.status.ToString();
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {


            NewsBLL bll = new NewsBLL();
            TKS_FAS_News data = new TKS_FAS_News();
            data.id = this.Id;
            data.title = this.txtTitle.Value;
            data.status = 0;
            data.content = this.txtBody.Value;
            data.type = "book";
            data.Sort = int.Parse(this.txtSort.Value);
            var res = bll.Save(data, Token);
            this.Id = res.id;
            this.lblError.Text = "保存成功";
        }
        catch (Exception ex)
        {
            lblError.Text = ex.Message;
        }
    }
}