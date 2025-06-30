using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UserCreation.Common;
using XONT.Common.Message;

namespace XONT.Ventura.AppConsole.Web
{
    public partial class ucChengePass : System.Web.UI.UserControl
    {
        private User user;
        private MessageSet _message;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMas.Text = "";
        }

        protected void btnChangePass_Click(object sender, EventArgs e)
        {
            var validator = new Validator();
            string oldPass = txtoldPassword.Text.Trim();
            string newPass = txtnewPassword.Text.Trim();
            string errorMsg = string.Empty;
            bool isPassExpire = true;//History Expiire or User Chande password
            if (Session["Main_LoginUser"] != null)
            {
                isPassExpire = false;
                user = (User)Session["Main_LoginUser"];
            }
            else
            {
                user = (User) Session["Main_ChangePassCurrentUser"];
            }
            validator.ValidateUserPassword(user, oldPass, newPass, '1',
                                           ref errorMsg, ref _message);
            if (_message != null)
            {
                ErrorMessageDisplay();
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    lblMas.Text = errorMsg;
                    if (isPassExpire)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Main_ChangePass",
                                                            "javascript:ShowPopup();",
                                                            true);
                    }else
                    {
                       
                    }
                }
                else
                {
                    if (Session["Main_ChangePassCurrentUser"] != null)
                    {
                        Session.Remove("Main_ChangePassCurrentUser");
                    }
                    if (isPassExpire)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Main_ChangePass",
                                                            "javascript:HidePopup();",
                                                            true);
                    }else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Main_ChangePass",
                                                            "javascript:window.parent.closeFade() ;",
                                                            true); 
                    }
                    //   valFailureText.Text = "Successfully Change Password";
                }
            }
        }

        private void ErrorMessageDisplay()
        {
            Session["Error"] = _message;
            MessageDisplay.Dispaly(this);
        }
    }
}