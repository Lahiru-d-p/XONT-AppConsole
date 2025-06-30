using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XONT.Ventura.AppConsole.Web
{
    public partial class AdminAlert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string AdminAlert = Request.QueryString["AdminAlert"];
                if (AdminAlert != null)
                {
                    lblAdminMessage.Text = AdminAlert.ToString();
                }
            }
        }
        protected void btnOk_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript((Page)this, this.GetType(), "script1", "window.parent.HideAdminAlert();", true);
        }
    }
}
