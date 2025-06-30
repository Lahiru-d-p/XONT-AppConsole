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

namespace AppConsole
{
    public partial class HelpContents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //VR016 start
            string taskCode;
            //if (!string.IsNullOrEmpty(Session["helpActiveTaskCode"].ToString()))
            if (Session["helpActiveTaskCode"] != null)
            {
                if (!string.IsNullOrEmpty(Session["helpActiveTaskCode"].ToString()))
                {
                    taskCode = Session["helpActiveTaskCode"].ToString().Trim();
                    if (taskCode != "undefined")
                        fraPage.Attributes["src"] = taskCode + "/" + taskCode + ".htm";
                    //help /
                    else
                        fraPage.Attributes["src"] = "";
                }
                else
                    fraPage.Attributes["src"] = "";
            }
            else
                fraPage.Attributes["src"] = "";
            //VR016 end
        }

        protected void trvTOC_SelectedNodeChanged(object sender, EventArgs e)
        {
            fraPage.Attributes["src"] = trvTOC.SelectedNode.Value;
        }
    }
}
