using System;
using System.Collections.Generic;
using System.Web.UI;
using XONT.Common.Data;
using XONT.Common.Message;
using XONT.Ventura.Common;
using System.Data; //VR007
using XONT.Ventura.Common.GenerateLicense; //VR007
using System.Text.RegularExpressions;//VR012
using System.IO;
using System.Web; //VR027
using System.Web.Security;

namespace XONT.Ventura.AppConsole
{
    public partial class Login : Page
    {
        private MessageSet _message;

        private User user;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Session.SessionID == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                //Page.Theme = txtTheme.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["LoginCount"] = "1";
            }
            if (Request.QueryString["Main_LogOut"] != null)
            {
                valFailureText.Text = Request.QueryString["Main_LogOut"]; //Parameter Error Massage
            }
            //switch (selectLanguage.SelectedIndex)
            //{
            //    case (1):
            //        Session["Main_Language"] = LanguageChange.Language.English;
            //        break;
            //    case (2):
            //        Session["Main_Language"] = LanguageChange.Language.Sinhala;
            //        break;
            //    case (3):
            //        Session["Main_Language"] = LanguageChange.Language.Tamil;
            //        break;

            //}


            //Change Password Byyon Visible False
            btnChangePassCon.Visible = false;
            SetLicenseText();//VR027
        }


        private void saveLanguage(User userUpdated)
        {

            IUserDAO userDao = new UserManager();
            MessageSet temp = new MessageSet();
            userDao.UpdateUserSetting(userUpdated, ref temp);

        }

        private void SetLanguage()//mgd
        {
            switch (selectLanguage.Value.ToString().Trim())
            {
                case ("English"):
                    Session["Main_Language"] = LanguageChange.Language.English;
                    user.Language = "English";
                    saveLanguage(user);
                    break;
                case ("Sinhala"):
                    Session["Main_Language"] = LanguageChange.Language.Sinhala;
                    user.Language = "Sinhala";
                    saveLanguage(user);
                    break;
                case ("Tamil"):
                    Session["Main_Language"] = LanguageChange.Language.Tamil;
                    user.Language = "Tamil";
                    saveLanguage(user);
                    break;
                default:
                    SetDefaultLanguage();
                    break;
            }
        }

        private void SetDefaultLanguage()
        {
            try
            {
                switch (user.Language.ToString().Trim())
                {
                    case ("English"):
                        Session["Main_Language"] = LanguageChange.Language.English;
                        user.Language = "English";
                        break;
                    case ("Sinhala"):
                        Session["Main_Language"] = LanguageChange.Language.Sinhala;
                        user.Language = "Sinhala";
                        break;
                    case ("Tamil"):
                        Session["Main_Language"] = LanguageChange.Language.Tamil;
                        user.Language = "Tamil";
                        break;
                }
            }
            catch (Exception e)
            {
                Session["Main_Language"] = LanguageChange.Language.English;
                user.Language = "English";
            }

        }

        protected void btnLoginButton_Click(object sender, EventArgs e)
        {
            valFailureText.Text = "";

            //V2041Adding start
            string UserNamePlain = AESEncrytDecry.DecryptStringAES(hdnEncryptUN.Value);
            string pwPlain = AESEncrytDecry.DecryptStringAES(hdnEncryptPW.Value);
            //V2041Adding end

            // validate single quote
            //if (txtUserName.Text.ToString().Trim() != "")
            if (UserNamePlain.Trim() != "")
            {
                #region V2041Removed
                //if (!System.Text.RegularExpressions.Regex.IsMatch(txtUserName.Text.ToString().Trim(), revUserName.ValidationExpression.ToString().Trim())) 
                //if (!System.Text.RegularExpressions.Regex.IsMatch(UserNamePlain.Trim(), revUserName.ValidationExpression.ToString().Trim()))
                #endregion
                if (!System.Text.RegularExpressions.Regex.IsMatch(UserNamePlain.Trim(), "^[^(')]*$"))//V2041Added
                {
                    valUserNamePassword.Text = "Single quote not allowed";
                    return;
                }
            }

            #region V2041Removed
            //if (txtUserName.Text.ToString().Trim() == "" && txtPassword.Value.Trim() == "") 
            #endregion
            if (UserNamePlain.Trim() == "" && pwPlain.Trim() == "")//V2041
            {
                valUserNamePassword.Text = "User name & password required !";
                return;
            }
            #region V2041Removed
            //else if (txtUserName.Text.ToString().Trim() == "") 
            #endregion
            else if (UserNamePlain.Trim() == "")
            {
                valUserNamePassword.Text = "User name required !";
                return;
            }
            #region V2041Removed
            //else if (txtPassword.Value.Trim() == "") 
            #endregion
            else if (pwPlain.Trim() == "")
            {
                valUserNamePassword.Text = "Password required !";
                return;
            }
            //VR011 add end

            IUserDAO userDao = new UserManager();

            user = LoginProcess();
            if (_message == (null))
            {
                int loginCount = int.Parse(ViewState["LoginCount"].ToString());
                if ((user.IsError))
                {
                    if (user.UserRoles.Count > 0)
                    {
                        int userCount = Convert.ToInt32(Application["Main_UserCount"]);
                        //if (user.UserName.Trim() != "administrator" && user.UserName.Trim() != "xontadmin")//VR019 //V2050R
                        if (user.UserName.Trim() != "admin" && user.UserName.Trim() != "administrator" && user.UserName.Trim() != "xontadmin")//V2050
                            Application["Main_UserCount"] = userCount + 1;
                        var users = (List<User>)Application["Main_User"];
                        bool isExistsUser = false;

                        if (!users.Equals(null))
                        {
                            foreach (User list in users)
                            {
                                if (list.SessionId.Equals(user.SessionId))
                                {
                                    isExistsUser = true;
                                    break;
                                }
                            }
                        }
                        if (!isExistsUser)
                        {
                            Application.Lock();
                            users.Add(user);
                            Application["Main_User"] = users;
                            Application.UnLock();

                            user.SuccessfulLogin = "1";
                            userDao.SaveUserLoginData(user, ref _message);
                            if (_message != null)
                            {
                                ErrorMessageDisplay();
                                return;
                            }

                            user.CreatedByAppConsole = true;//V2025
                            SetLanguage();//mgd
                            Session["Main_LoginUser"] = user;
                            Session["Theme"] = user.Theme.Trim();
                            Session["Main_UserName"] = user.UserName;
                            Session["Main_BusinessUnit"] = user.BusinessUnit;
                            Session["Main_LoginUserStatic"] = user;
                            //Add User To Cache
                            if (Cache["Main_LoginUserList"] != null)
                            {
                                ((List<User>)Cache["Main_LoginUserList"]).Add(user);
                            }
                            else
                            {
                                var userList = new List<User>();
                                userList.Add(user);
                                Cache["Main_LoginUserList"] = userList;
                            }

                            #region Removed V2033(assuming unneccesary)
                            //string script = "<Script language='javascript'>";
                            //script = script + " window.open('Main.aspx','newwin','statusbar=no,status=no ,personalbar=no,status=no,directories=no,locationbar=no,location=no,fullscreen=no,scrollbars=no,maximize=yes,menubar=no,toolbar=no,addressbar=no,top=0,left=0,resizable=yes,');";

                            //script = script + "if(window.opener==null){  if (navigator.appName == 'Netscape') {window.opener = self;window.close();}else{window.opener = 'X';window.open('', '_parent', ''); self.close();}}else{};";
                            //script = script + "</script>";
                            //Response.Write(script); 
                            #endregion

                            Response.Redirect("Main.aspx");
                        }
                        else
                        {
                            user.SuccessfulLogin = "0";
                            ViewState["LoginCount"] = loginCount + 1;
                            user.Reson = "Another User Login Same Browser";
                            valFailureText.Text = "Another User Login Same Browser";
                            userDao.SaveUserLoginData(user, ref _message);
                            if (_message != null)
                            {
                                ErrorMessageDisplay();
                                return;
                            }
                            userDao.SaveTimeOut(user, ref _message);
                            if (_message != null)
                            {
                                ErrorMessageDisplay();
                                return;
                            }


                            //if (user.UserName.Trim() != "administrator" && user.UserName.Trim() != "xontadmin")//VR050R
                            if (user.UserName.Trim() != "admin" && user.UserName.Trim() != "administrator" && user.UserName.Trim() != "xontadmin")//VR050
                            {
                                Application.Lock();
                                userCount = Convert.ToInt32(Application["Main_UserCount"]);
                                Application["Main_UserCount"] = userCount - 1;
                                Application.UnLock();
                            }
                        }
                    }
                    else
                    {
                        ViewState["LoginCount"] = loginCount + 1;
                        valFailureText.Text = "Please Attached User roles";
                        user.Reson = "User roles Not Attached";
                        user.SuccessfulLogin = "0";
                        userDao.SaveUserLoginData(user, ref _message);
                        if (_message != null)
                        {
                            ErrorMessageDisplay();
                            return;
                        }

                        userDao.SaveTimeOut(user, ref _message);
                        if (_message != null)
                        {
                            ErrorMessageDisplay();
                            return;
                        } //Set User Error
                    }
                }
                else
                {
                    userDao.SaveUserLoginData(user, ref _message);
                    if (_message != null)
                    {
                        ErrorMessageDisplay();
                        return;
                    }
                    ViewState["LoginCount"] = loginCount + 1;
                    userDao.SaveTimeOut(user, ref _message);
                    if (_message != null)
                    {
                        ErrorMessageDisplay();
                        return;
                    }
                    //Set User Error
                    //  valFailureText.Text = "Invalid User";
                    if (user.PasswordChange != "")
                    {
                        if (user.PasswordChange.Equals("1"))
                        {
                            btnChangePassCon.Visible = true;
                            Session["Main_ChangePassCurrentUser"] = user;
                        }
                    }

                    if (user.IsPassExpire)
                    {
                        btnChangePassCon.Visible = true;
                        Session["Main_ChangePassCurrentUser"] = user;
                    }
                }
            }
            else
            {
                valFailureText.Text = _message.Desc.Trim(); //VR011 add
                user.Reson = _message.Desc.Trim(); //VR011 add

                user.SuccessfulLogin = "0";
                userDao.SaveUserLoginData(user, ref _message);
                if (_message != null)
                {
                    ErrorMessageDisplay();
                    return;
                }
                userDao.SaveTimeOut(user, ref _message);
                if (_message != null)
                {
                    ErrorMessageDisplay();
                    return;
                } //Set User Error
                //  valFailureText.Text = "Invalid User";
            }
        }

        private User LoginProcess()
        {
            IUserDAO userDao = new UserManager();

            //V2041Adding start
            string plainUN = AESEncrytDecry.DecryptStringAES(hdnEncryptUN.Value);
            string plainPW = AESEncrytDecry.DecryptStringAES(hdnEncryptPW.Value);

            var stroEncript = new StroEncript();

            string encriptPsass = stroEncript.Encript(plainPW.Trim());
            User user = userDao.GetUserInfo(plainUN.Trim(), encriptPsass, ref _message);
            //V2041Adding end

            #region V2041Removed
            //var stroEncript = new StroEncript();//VR029
            //string encriptPsass = stroEncript.Encript(txtPassword.Value.Trim());//VR029
            //User user = userDao.GetUserInfo(txtUserName.Text.Trim(), encriptPsass, ref _message);//VR029 
            #endregion
            if (_message != null)
            {
                ErrorMessageDisplay();
                return user;
            }

            //VR003 Begin
            if (user == null)
            {
                user = new User();
                user.SuccessfulLogin = "0";
                user.Reson = "Invalid User";
                user.IsError = false;
                //valFailureText.Text = "Invalid User ...."; //V2050R
                valFailureText.Text = "Invalid User Name or Password"; //V2050

            }
            //VR003 Begin

            //VR025
            List<string> objectLockSessionList = new List<string>();
            if (user.BusinessUnit != null)//VR026
            {
                objectLockSessionList = userDao.GetObjectLockSessionsList(user.BusinessUnit.Trim(), ref _message);
                if (_message != null)
                {
                    ErrorMessageDisplay();
                    return user;
                }
            }
            if (objectLockSessionList.Count > 0)
                ReleaseObjectLocks(user.BusinessUnit.Trim(), objectLockSessionList);

            //VR025

            //V2049S
            List<string> taskLockSessionList = new List<string>();
            if (user.BusinessUnit != null)
            {
                taskLockSessionList = userDao.GetTaskLockSessionsList(user.BusinessUnit.Trim(), ref _message);
                if (_message != null)
                {
                    ErrorMessageDisplay();
                    return user;
                }
            }
            if (taskLockSessionList.Count > 0)
                ReleaseTaskLocks(user.BusinessUnit.Trim(), taskLockSessionList);

            //V2049E

            user.WorkstationId = GetWorkStationId(); //GET LOGING IP
            user.SessionId = Session.SessionID; //GET SESSION ID
            int loginCount = int.Parse(ViewState["LoginCount"].ToString()); //check with No of Attemts

            //First Check UserName and Password Exists in Data BAse
            #region V2041Removed
            //if (user.isExists && user.UserName.Trim().Equals(txtUserName.Text.Trim())) 
            #endregion
            if (user.isExists && user.UserName.Trim().Equals(plainUN.Trim()))//V2041Added
            {
                //V2004
                if (user.PasswordChange.Equals("0"))
                {
                    //check User Password is Locked
                    if (user.PasswordLocked.Equals('0'))
                    {
                        //Check User Is Active User
                        if (user.ActiveFlag) //Is Active User
                        {
                            //Check User Expire
                            if (!user.IsUserExpire)
                            {
                                //Check Password Expire
                                if (!user.IsPassExpire)
                                {
                                    //Check No Of Attemts
                                    //if (loginCount < user.PasswordData.NoOfAttempts)//VR011 remove 
                                    //{
                                    //Check User Already Logging
                                    if (user.AlreadyLogin.Equals(string.Empty))
                                    {
                                        //SuccessFull Login
                                        // user.SuccessfulLogin = "0";
                                        LicenseKey licenseKey = userDao.GetLicenseData(user.UserName, user.BusinessUnit,
                                                                                       ref _message);

                                        if (_message != null)
                                        {
                                            ErrorMessageDisplay();
                                            return user;
                                        }
                                        if (user.BusinessUnit.Trim() == licenseKey.BusinessUnit)
                                        {
                                            //Check With Expire date IF Type Temporary
                                            if (licenseKey.LicenseType.Equals("P"))
                                            {
                                                Session["AppCon_LicenseType"] = "P";//VR015
                                                SimultaniousHandle(user, licenseKey);
                                                if (user.IsError)
                                                {
                                                }
                                            }
                                            else if (licenseKey.LicenseType.Equals("T"))
                                            {
                                                Session["AppCon_LicenseType"] = "T";//VR015
                                                {
                                                    if (licenseKey.ExpireDate >= (DateTime.Now))
                                                    {

                                                        SimultaniousHandle(user, licenseKey);

                                                        //VR015 
                                                        if (licenseKey.ExpiryAlertStartFrom > 0 && licenseKey.ExpiryAlertStartFrom >= (licenseKey.ExpireDate - DateTime.Today).Days)
                                                        {
                                                            Session["AppCon_NoOfExpiry"] = (licenseKey.ExpireDate - DateTime.Today).Days;
                                                            Session["AppCon_ExpiryAlert"] = "1";
                                                        }
                                                        else
                                                        {
                                                            Session["AppCon_ExpiryAlert"] = "0";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        user.SuccessfulLogin = "0";
                                                        user.IsError = false;
                                                        user.Reson = "License Key Expired";
                                                        valFailureText.Text = "License Key Expired...";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                user.SuccessfulLogin = "0";
                                                user.Reson = "Invalid License Key";
                                                user.IsError = false;
                                                valFailureText.Text = "Invalid License Key....";
                                            }
                                        }
                                        else
                                        {
                                            user.SuccessfulLogin = "0";
                                            user.Reson = "Invalid License Key";
                                            user.IsError = false;
                                            valFailureText.Text = "Invalid License Key....";
                                        }
                                    } //check user alreay Login
                                    else
                                    {
                                        user.IsError = false;
                                        user.SuccessfulLogin = "0";
                                        user.Reson = "Already Login";
                                        valFailureText.Text = "Already Login at " + user.AlreadyLogin;
                                    }

                                } //Check Password Expire //VR011 remove end
                                else
                                {
                                    user.IsError = false;
                                    user.SuccessfulLogin = "0";
                                    user.Reson = "Password Expire";
                                    valFailureText.Text = "Password Expire";
                                }
                            } //Check User Expire
                            else
                            {
                                user.IsError = false;
                                user.SuccessfulLogin = "0";
                                user.Reson = "Your account has been Expired";
                                valFailureText.Text = "Your account has been Expired";
                                userDao.UpdateActiveFlagUser(user, ref _message);
                                if (_message != null)
                                {
                                    ErrorMessageDisplay();
                                    return user;
                                }
                            }
                        } //Is Active User
                        else
                        {
                            user.IsError = false;
                            user.SuccessfulLogin = "0";
                            user.Reson = "Not an Active User";
                            valFailureText.Text = "Not an Active User";
                        } //Check User exist
                    } //password Lock
                    else
                    {
                        user.IsError = false;
                        user.SuccessfulLogin = "0";
                        user.Reson = "Password Locked";
                        valFailureText.Text = "Password Locked";
                    }
                }
                else
                {
                    user.IsError = false;
                    user.SuccessfulLogin = "0";
                    user.Reson = "Password changed";
                    valFailureText.Text = "Password changed by admin";
                }
                //}VR014
            }
            else
            {
                //VR011
                #region V2041Removed
                //bool available = userDao.CheckUserAvailable(txtUserName.Text.Trim(), ref _message);
                #endregion
                bool available = userDao.CheckUserAvailable(plainUN.Trim(), ref _message);//V2041Added
                if (_message != null)
                {
                    ErrorMessageDisplay();
                    return user;
                }
                if (available)
                {
                    //Check attempts
                    #region V2041Removed
                    //bool exceeded = userDao.CheckNoOfAttemptsExceeded(txtUserName.Text.Trim(), ref _message); 
                    #endregion
                    bool exceeded = userDao.CheckNoOfAttemptsExceeded(plainUN.Trim(), ref _message);//V2041Added
                    if (_message != null)
                    {
                        ErrorMessageDisplay();
                        return user;
                    }
                    if (exceeded)
                    {
                        user.IsError = false;
                        user.SuccessfulLogin = "0";
                        user.Reson = "More Attempts";
                        userDao.UpdateLoginAttemptUser(user, ref _message);
                        valFailureText.Text = "More Attempts";
                        if (_message != null)
                        {
                            ErrorMessageDisplay();
                            return user;
                        }
                    }
                    else
                    {
                        user.SuccessfulLogin = "0";
                        user.Reson = "Invalid Password";
                        user.IsError = false;
                        //valFailureText.Text = "Invalid Password"; //V2050 R
                        valFailureText.Text = "Invalid User Name or Password"; //V2050

                    }


                }
                else
                {
                    //VR011

                    user.IsError = false;
                    user.SuccessfulLogin = "0";
                    user.Reson = "Invalid User";
                    //valFailureText.Text = "Invalid User";//V2050R
                    valFailureText.Text = "Invalid User Name or Password";//V2050

                }
            }


            return user;
        }

        private string GetWorkStationId()
        {
            // Look for a proxy address first
            String _ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            // If there is no proxy, get the standard remote address
            if (string.IsNullOrEmpty(_ip) || _ip.ToLower() == "unknown")
            {
                _ip = Request.ServerVariables["REMOTE_ADDR"];
            }

            //   string ip = Request.ServerVariables["remote_addr"];

            //string strHostName = Dns.GetHostName();
            //string clientIpAddress = Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            return _ip;
        }

        private void SimultaniousHandle(User userOb, LicenseKey licenseKey)
        {
            bool reload = false;
            if (licenseKey.LicenseMode.Equals("N"))
            {
                if (Application["Main_ActiveUserCount"] == null)
                {
                    IUserDAO userDao = new UserManager();
                    var activeUsers = new List<User>();
                    userDao.GetActiveUsers(ref activeUsers, ref _message);
                    //Get Active Users
                    if (_message != null)
                    {
                        userOb.IsError = false;
                        ErrorMessageDisplay();
                        return;
                    }
                    else
                    {
                        reload = true;
                        Application["Main_ActiveUserCount"] = activeUsers;
                    }
                }
                List<User> activeUsersBu =
                    ((List<User>)Application["Main_ActiveUserCount"]).FindAll(
                        userO => userO.BusinessUnit.Trim() == userOb.BusinessUnit.Trim());
                ;

                int userS = Convert.ToInt32(activeUsersBu.Count);
                if (userS > Convert.ToInt32(licenseKey.NoOfUsers)) //Check with total active uses
                {
                    userOb.IsError = false;
                    userOb.SuccessfulLogin = "0";
                    userOb.Reson = "Maximun No Of Active Users Exceed In This System";
                    valFailureText.Text = "Maximun No Of Active Users Exceed In This System";
                }
                else if (userS <= 0)
                {
                    #region IF Newly Add BU

                    if (reload)
                    {
                        userOb.IsError = false;
                        userOb.SuccessfulLogin = "0";
                        userOb.Reson = "Active Users not available In This System";
                        valFailureText.Text = "Active Users not available In This System";
                    }
                    else
                    {
                        IUserDAO userDao = new UserManager();
                        var activeUsers = new List<User>();
                        userDao.GetActiveUsers(ref activeUsers, ref _message);
                        //Get Active Users
                        if (_message != null)
                        {
                            userOb.IsError = false;
                            ErrorMessageDisplay();
                            return;
                        }
                        else
                        {
                            Application["Main_ActiveUserCount"] = activeUsers;
                        }
                        activeUsersBu =
                            ((List<User>)Application["Main_ActiveUserCount"]).FindAll(
                                userO => userO.BusinessUnit.Trim() == userOb.BusinessUnit.Trim());
                        ;

                        userS = Convert.ToInt32(activeUsersBu.Count);
                        if (userS > Convert.ToInt32(licenseKey.NoOfUsers)) //Check with total active uses
                        {
                            userOb.IsError = false;
                            userOb.SuccessfulLogin = "0";
                            userOb.Reson = "Maximun No Of Active Users Exceed In This System";
                            valFailureText.Text = "Maximun No Of Active Users Exceed In This System";
                        }
                        else if (userS <= 0)
                        {
                            userOb.IsError = false;
                            userOb.SuccessfulLogin = "0";
                            userOb.Reson = " Active Users not available In This System";
                            valFailureText.Text = "Active Users not available In This System";
                        }
                        else
                        {
                            userOb.IsError = true;
                            userOb.Reson = "SuccessFull login";
                        }
                    }

                    #endregion
                }
                else
                {
                    userOb.IsError = true;
                    userOb.Reson = "SuccessFull login";
                }
            }
            else if (licenseKey.LicenseMode.Equals("S"))
            {
                int userS = Convert.ToInt32(Application["Main_UserCount"]);
                //if (userS >= Convert.ToInt32(licenseKey.NoOfUsers))
                if (userS > Convert.ToInt32(licenseKey.NoOfUsers))//VR018
                {
                    userOb.IsError = false;
                    userOb.SuccessfulLogin = "0";
                    userOb.Reson = "Maximun No Of Users Already Use In This System";
                    valFailureText.Text = "Maximun No Of Users Already Use In This System";
                }
                else
                {
                    userOb.IsError = true;
                    userOb.Reson = "SuccessFull login";
                }
            }
            {
            }
        }

        private void ErrorMessageDisplay()
        {

            valFailureText.Text = _message.Desc.Trim();//VR011 add

        }

        #region VR007
        private void FilterActiveUsers(ref int intActiveUserCount)
        {
            IUserDAO userManager = new UserManager();
            var allUsers = userManager.GetAllUsers(ref _message);

            //var generateLicenseKey = new GenerateLicenseBLL();

            if (allUsers != null && allUsers.Rows.Count > 0)
            {
                foreach (DataRow row in allUsers.Rows)
                {
                    string userName = row["UserName"].ToString().Trim();
                    //string activeFlag = row["ActiveFlag"].ToString().Trim();
                    //string genACtiveFlag = generateLicenseKey.ReturaActiveFlagUser("1", userName);

                    //if (string.Equals(activeFlag, genACtiveFlag) && userName != "administrator" && userName != "xontadmin")
                    //if (userName != "administrator" && userName != "xontadmin") //V2050R
                    if (userName != "admin" && userName != "administrator" && userName != "xontadmin") //V2050
                    {
                        intActiveUserCount++;
                    }
                }
            }
        }
        #endregion

        //VR025
        private void ReleaseObjectLocks(string businessUnit, List<string> objectLockSessions)
        {
            var users = new List<User>();
            users = (List<User>)Application["Main_User"];
            List<string> expiredSessionList = new List<string>();

            if (!users.Equals(null))
            {
                foreach (string session in objectLockSessions)
                {
                    var listFiltered = users.FindAll(item => item.SessionId == session.Trim());

                    if (listFiltered.Count == 0)
                    {
                        expiredSessionList.Add(session.Trim());
                    }
                }
            }
            if (expiredSessionList.Count > 0)
            {
                IUserDAO userDao = new UserManager();
                MessageSet message = null;
                userDao.ReleaseObjectLocks(businessUnit, expiredSessionList, ref message);
                if (message != null)
                {
                    Session["Error"] = message;
                    MessageDisplay.Dispaly(btnLoginButton);
                    return;
                }
            }


        }

        //V2049S
        private void ReleaseTaskLocks(string businessUnit, List<string> taskLockSessions)
        {
            var users = new List<User>();
            users = (List<User>)Application["Main_User"];
            List<string> expiredSessionList = new List<string>();

            if (!users.Equals(null))
            {
                foreach (string session in taskLockSessions)
                {
                    var listFiltered = users.FindAll(item => item.SessionId == session.Trim());

                    if (listFiltered.Count == 0)
                    {
                        expiredSessionList.Add(session.Trim());
                    }
                }
            }
            if (expiredSessionList.Count > 0)
            {
                IUserDAO userDao = new UserManager();
                MessageSet message = null;
                userDao.UpdateActiveTask(businessUnit, expiredSessionList, ref message);
                if (message != null)
                {
                    Session["Error"] = message;
                    MessageDisplay.Dispaly(btnLoginButton);
                    return;
                }
            }
        }
        //V2049E

        //VR027 Begin
        private void SetLicenseText()
        {
            lblLicence.Text = "";
            string licenseText = System.Web.Configuration.WebConfigurationManager.AppSettings["LicenseText"];

            //Set the License Text
            if (!string.IsNullOrEmpty(licenseText))
            {
                lblLicence.Text = licenseText;
            }


            //Hide the Logo Image if not exists
            try
            {
                string imageLogo = "";
                imageLogo = System.Web.Configuration.WebConfigurationManager.AppSettings["logoimage"];


                //imageLogo = MapPath(HttpContext.Current.Request.ApplicationPath) + "images\\logo.jpg";
                if (!string.IsNullOrEmpty(imageLogo) && File.Exists(imageLogo))
                {
                    imgLogo.Visible = true;
                    imgLogo.ImageUrl = imageLogo;
                }
                else
                {
                    imgLogo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                imgLogo.Visible = false;
            }
        }

        //VR032

        //V2008...
        private void RemoveRedirCookie()
        {
            Response.Cookies.Add(new HttpCookie("__LOGINCOOKIE__", ""));
        }

        private void AddRedirCookie()
        {

            FormsAuthenticationTicket ticket =
            new FormsAuthenticationTicket(1, "Test", DateTime.Now, DateTime.Now.AddSeconds(5), false, "");
            string encryptedText = FormsAuthentication.Encrypt(ticket);
            Response.Cookies.Add(new HttpCookie("__LOGINCOOKIE__", encryptedText));
        }
        //...V2008
    }
}