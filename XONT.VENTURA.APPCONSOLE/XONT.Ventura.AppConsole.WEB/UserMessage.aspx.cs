using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using XONT.Common.Message;

namespace XONT.Ventura.AppConsole.Web
{
    public partial class UserMessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageSet msg = (MessageSet)Session["Error"];
            btnOk.Attributes.Add("OnClick", "self.close()");
            if (msg != null)
            {
                lblMessageNumber.Text = msg.MsgNumber.ToString();
                lblDisc.Text = msg.Desc;
            }
            Session.Remove("Error");
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            Session.Remove("Error");
            this.Dispose();
           // ScriptManager.RegisterStartupScript(this, this.GetType(), "script1", "window.parent.HidePopup();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "script1", "javascript:window.parent.HideUserMessage();", true);
        }
    }
}
