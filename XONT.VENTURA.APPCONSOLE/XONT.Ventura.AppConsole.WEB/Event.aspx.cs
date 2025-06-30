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
using System.Text;
using System.Xml.Linq;
using System.Net.Mail;
using XONT.Common.Message;

namespace XONT.Ventura.AppConsole.Web
{
    public partial class Event : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageSet msg = (MessageSet)Session["Error"];
            btnOK.Attributes.Add("OnClick", "self.close()");
            if (msg != null)
            {
                string userName = "";
                try
                {
                    userName = Session["Main_UserName"].ToString().Trim();
                }
                catch
                { }


                string hostName = System.Net.Dns.GetHostName();
                string clientIPAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();

                Page.Title = msg.AppName;
                lblELNumber.Text = msg.ErrorLog.ToString();
                lblETime.Text = msg.ErrorTime.ToString();
                lblEVersion.Text = msg.Version;
                lblDisc.Text = msg.Desc;
                lblDLLName.Text = msg.DllName;
                lblELineNumber.Text = msg.LineNumber.ToString();
                lblESource.Text = msg.ErrorSource;

                lblUName.Text = userName;
                lblWId.Text = hostName;
                lblIPAddress.Text = clientIPAddress;
                lblMNumber.Text = msg.MsgNumber.ToString();
                lblRoutine.Text = msg.Routine;
            }
            Session.Remove("Error");
            //  else
            // {
            //Response.Redirect("", true);
            //Response.Redirect("User.aspx");
            //  }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            Session.Remove("Error");
            this.Dispose();
            ScriptManager.RegisterStartupScript(Page, GetType(), "scriptHidePopup", "javascript:window.parent.HideErrorPopup();", true);
       

        }

        protected void btnMail_Click(object sender, EventArgs e)
        {
            MailMessage msgMail = new MailMessage();

            string errorEmail = System.Configuration.ConfigurationManager.AppSettings["ErrorMail"].ToString();
            string errorEmailFrom = System.Configuration.ConfigurationManager.AppSettings["ErrorMailFrom"].ToString();

            if (errorEmail == "")
            {
                errorEmail = "support@xontworld.com";
            }

            msgMail.To.Add(errorEmail);

            MailAddress mailFrom = new MailAddress(errorEmailFrom);
            msgMail.From = mailFrom;

            msgMail.Subject = "X-ONT Error Notification";

            msgMail.IsBodyHtml = true;
            StringBuilder strBody = new StringBuilder("<html><body>");
            strBody.Append("<br>Error Number      : " + lblELNumber.Text + "</br>");
            strBody.Append("<br>Error Time        : " + lblETime.Text + "</br>");
            strBody.Append("<br>WorkStation ID    : " + lblWId.Text + "</br>");
            strBody.Append("<br>User Name         : " + lblUName.Text + "</br>");
            strBody.Append("<br>IPAddress         : " + lblIPAddress.Text + "</br>");
            strBody.Append("<br>Message Number    : " + lblMNumber.Text + "</br>");
            strBody.Append("<br>Error Discription : " + lblDisc.Text + "</br>");
            strBody.Append("<br>Error Source      : " + lblESource.Text + "</br>");
            strBody.Append("<br>DLL Name          : " + lblDLLName.Text + "</br>");
            strBody.Append("<br>Version           : " + lblEVersion.Text + "</br>");
            strBody.Append("<br>Routine           : " + lblRoutine.Text + "</br>");
            strBody.Append("<br>Error Line Number : " + lblELineNumber.Text + "</br>");
            strBody.Append("</body></html>");



            msgMail.Body = strBody.ToString();
            string smtp = System.Configuration.ConfigurationManager.AppSettings["SMTP"].ToString();
           
            SmtpClient client = new SmtpClient();
            client.Host = smtp;

            try {
                client.Send(msgMail);
            }
            catch(Exception ex)
            {

            }
            }
    }
}
