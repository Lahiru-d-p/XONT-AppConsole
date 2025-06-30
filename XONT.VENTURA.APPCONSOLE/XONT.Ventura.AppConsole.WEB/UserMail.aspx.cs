using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.UI;
using System.Windows.Forms;
using XONT.Common.Message;
using System.Net;

namespace XONT.Ventura.AppConsole.Web
{
    public partial class UserMail : Page
    {
        private MessageSet _msg;
        private User user;

        protected void Page_Load(object sender, EventArgs e)
        {
            txtFrom.Text = "crmnotifications@xontworld.com";
            txtTo.Text = "support@xontworld.com";
            user = (User) Session["Main_LoginUser"];
            if(!IsPostBack)
            chkAttachment.Checked = true;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            AsAttachment(Session["Main_MailImage"].ToString());
        }

        private void GetFullSceenCapture(string filePath)
        {
            int screenWidth = Screen.GetBounds(new Point(0, 0)).Width;
            int screenHeight = Screen.GetBounds(new Point(0, 0)).Height;
            var bmpScreenShot = new Bitmap(screenWidth, screenHeight);
            Graphics gfx = Graphics.FromImage(bmpScreenShot);
            gfx.CopyFromScreen(0, 0, 0, 0, new Size(screenWidth, screenHeight));
            bmpScreenShot.Save(filePath, ImageFormat.Jpeg);

            GC.Collect();
        }

        private void GetIframelSceenCapture(string filePath)
        {
            int screenWidth = Screen.GetBounds(new Point(0, 0)).Width;
            int screenHeight = Screen.GetBounds(new Point(0, 0)).Height;
            var bmpScreenShot = new Bitmap(screenWidth - 255, screenHeight - 209);
            Graphics gfx = Graphics.FromImage(bmpScreenShot);
            gfx.CopyFromScreen(255, 205, 0, 0, new Size(screenWidth, screenHeight));
            bmpScreenShot.Save(filePath, ImageFormat.Jpeg);

            GC.Collect();
                       
        }

        //private void GetSceenCapture(string filePath)
        //{
        //       ScreenCapture sc = new ScreenCapture();
        //    Image img = sc.CaptureScreen();
        //     display image in a Picture control named imageDisplay
        //    img.Save(filePath, ImageFormat.Jpeg);
        //    GC.Collect();

        //}

        private void AsAttachment(string filePath)
        {
            string fromAddress = "vcrmnotifications@xontworld.com";
            string toAddress = "surija.s@xontworld.com";
            string path = filePath;
            var mailMessage = new MailMessage(fromAddress, toAddress);
            mailMessage.Subject = "Ventura";


            if (chkAttachment.Checked)
            {
                var logo = new Attachment(path);
                mailMessage.Attachments.Add(logo);
            }
            mailMessage.Body = txtBody.Value;
            // mailMessage.IsBodyHtml = true;
            var mailSender = new SmtpClient(ConfigurationManager.AppSettings["SMTP"]);
            try {
                mailSender.Send(mailMessage);
            }
            catch(Exception ex)
            {


            }

        }

        private void AsEmailBody(string filePath)
        {
            var email = new MailMessage("crmnotifications@xontworld.com", "support@xontworld.com");

            email.Subject = "Ventura";
            email.IsBodyHtml = true;
            email.Body =
                txtBody.Value +
                "<br/><div style=\"font-family:Arial\">Ventura Screen:<br /><br /><img src=\"@@IMAGE@@\" alt=\"\"><br /><br /></div>";


            string contentID = Path.GetFileName(filePath).Replace(".", "") + "@zofm";
            string attachmentPath = filePath;
            var inline = new Attachment(attachmentPath);
            inline.ContentDisposition.Inline = true;
            inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            inline.ContentId = contentID;
            inline.ContentType.MediaType = "image/jpeg";
            inline.ContentType.Name = Path.GetFileName(attachmentPath);
            email.Attachments.Add(inline);

            email.Body = email.Body.Replace("@@IMAGE@@", "cid:" + contentID);
            email.IsBodyHtml = true;
            var mailSender = new SmtpClient(ConfigurationManager.AppSettings["SMTP"]);
            mailSender.Send(email);
        }

        protected void btnOk0_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "javascript:window.parent.HideMail();", true);
        }
    }
}