using System;
using System.Data;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using XONT.Common.Data;
using System.Globalization;
using XONT.Common.Data.Resource;
using XONT.Common.Message;
using XONT.Ventura.Common.ConvertDateTime;
using XONT.VENTURA.V2MNT21;
using Button = System.Web.UI.WebControls.Button;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Control = System.Web.UI.Control;
using Label = System.Web.UI.WebControls.Label;
using System.Drawing.Text;
using UserCreation.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Timers;
using System.Xml.Linq;
using XONT.Ventura.Common;
using System.Net.Mail;
using System.Net;

namespace XONT.Ventura.AppConsole
{
    public partial class Main : System.Web.UI.Page
    {
        /*Previous App Console*/
        #region Page Variables
        public string _navigation;
        public string _priorityNavigation;
        private MessageSet _message;
        private User _user;
        private IUserDAO _userDao;
        public string _favorite;
        private MessageSet _msg;
        List<UserTask> systemTask = new List<UserTask>();
        List<UserTask> systemTaskDaily = new List<UserTask>();
        public UserRole defaultUserRole; //V2017
        public UserRole priorityUserRole; //V2017
        //2.0.2.7
        // var user = new User();

        #endregion

        #region Main Page Methods
        protected override void OnPreInit(EventArgs e)
        {
            if (Session["Theme"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Page.Theme = Session["Theme"].ToString();
                base.OnPreInit(e);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            CheckNewNotification();
            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _user = (User)Session["Main_LoginUser"];

            try
            {

                lblPasswordError.Text = "";
                userId.Value = _user.UserName.ToString().Trim();
                spUserName.InnerHtml = _user.UserName.ToString().Trim();
                txtBusinessUnit.Value = _user.BusinessUnit.ToString().Trim();

                _userDao = new UserManager();
                string businessUnitName = _userDao.GetBusinessUnitName(txtBusinessUnit.Value.ToString().Trim(), ref _message);
                currentBU.InnerHtml = businessUnitName.Trim(); //V2005
                ScriptManager.RegisterStartupScript(this, GetType(), "test", "testbu();", true);
                //setCurrentRole();
                if (_user == null)
                {
                    Response.Redirect("~/Login.aspx");
                }


                if (!IsPostBack)
                {
                    //for date selector
                    calSelectDate.Format = ConvertDateTime.AssignDateTimeFormat();
                    calSelectDate.Format = ConvertDateTime.AssignDateTimeFormat();
                    ConvertDateTime.GetCurrentCulture();

                    #region Reminders

                    showExpiredReminders();
                    loadReminders();
                    dispatchNearestReminder();

                    #endregion



                    #region Multimedia Loading

                    //Get Perticuler Multimeadia Data in Data Base
                    IUserDAO userDao = new UserManager();
                    List<MultimediaDetails> multimedias = userDao.GetMultimediaList(_user.UserName, _user.BusinessUnit, ref _message);

                    if (_message != null)
                    {
                        Session["Error"] = _message;
                        ErrorMessageDisplay();
                        return;
                    }


                    Session["Main_Multimedia"] = multimedias;
                    if (multimedias.Count > 0)
                    {
                        //Get List for Mandotory Multimeadia(Thease are compulsory view )
                        Session["Main_MultimediaMandotory"] = multimedias.Where(p => (p.Mandatory == '1') && (p.Type == 'I')).ToList();
                        //Call MultiMedia.aspx Page to view meadia
                        ScriptManager.RegisterStartupScript(this, GetType(), "Main_Multimeadia", "javascript:PopMultimediaURL('MultiMedia.aspx');", true);
                    }


                    #endregion



                    #region For Admin Alert
                    Session["AdminPrevAlert"] = " ";
                    Session["AdminAlertTime"] = " ";
                    fontName.Value = _user.FontName.Trim();
                    fontSize.Value = _user.FontSize.ToString().Trim();
                    fontColor.Value = _user.FontColor.Trim();
                    languageSelect.Value = _user.Language.Trim();
                    #endregion

                    #region Display Expiry Alert 
                    //VR015
                    #region V2044Removed
                    //if (Session["AppCon_LicenseType"] == "T" && Session["AppCon_ExpiryAlert"] == "1")
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alert Message", "alert('Ventura temporary license will expire in " + Session["AppCon_NoOfExpiry"].ToString() + " day(s).')", true); 
                    #endregion

                    //V2044Adding start
                    if (Session["AppCon_LicenseType"] == "T" && Session["AppCon_ExpiryAlert"] == "1")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alert Message", "alert('Ventura temporary license will expire in " + Session["AppCon_NoOfExpiry"].ToString() + " day(s).')", true);

                        DataTable dtLicenseInfo = _userDao.GetLisenceAltertInfo(_user.BusinessUnit, out _msg);

                        if (_msg != null)
                        {
                            Session["Error"] = _msg;
                            ErrorMessageDisplay();
                            return;
                        }


                        if (dtLicenseInfo.Rows.Count > 0 && dtLicenseInfo.Rows[0]["LicenseAlertMailIDs"].ToString().Trim() != "" && Application["IsMailBeingSent"] == null && (dtLicenseInfo.Rows[0]["LastLicenseAlertSentDate"] == DBNull.Value || ((DateTime)dtLicenseInfo.Rows[0]["LastLicenseAlertSentDate"]).Date != DateTime.Today))
                        {
                            Application["IsMailBeingSent"] = 1;//in order to prevernt case of duplicating mail when symaltanious access.

                            //sending the mail start


                            //Vasanthan asked to hardcode
                            //string fromMail = "support@xontworld.com";
                            string fromMail = "crmnotifications@xontworld.com";

                            string fromMailPw = "KeSp7tHetrAf";
                            string smtp = "mail.xontworld.com";
                            string smtpPort = "587";
                            string smtpEnableSsl = "false";

                            MailMessage Mail = new MailMessage();
                            Mail.From = new MailAddress(fromMail);

                            string[] mailIDs = dtLicenseInfo.Rows[0]["LicenseAlertMailIDs"].ToString().Split(',');

                            foreach (string toAddress in mailIDs)
                            {
                                Mail.To.Add(toAddress.ToString().Trim());
                            }

                            Mail.Subject = $"Ventra Lisence Expiry Alert";
                            Mail.IsBodyHtml = false;
                            Mail.Body = "Dear Customer, \n \nVentura Application Temporary License Will Expire In " + Session["AppCon_NoOfExpiry"].ToString() + $" Day(s). \n \nBusiness Unit Name : {businessUnitName}  \n \nThank you  \n \nTeam XONT.";
                            SmtpClient mailSender = new SmtpClient(smtp, int.Parse(smtpPort));
                            mailSender.UseDefaultCredentials = false;
                            mailSender.Credentials = new NetworkCredential(fromMail, fromMailPw);
                            mailSender.EnableSsl = bool.Parse(smtpEnableSsl);
                            mailSender.Send(Mail);
                            Mail.Dispose();

                            //sending the mail end


                            _userDao.UpdateLisenceAlertSentDate(_user.BusinessUnit, out _msg);
                            if (_msg != null)
                            {
                                Session["Error"] = _msg;
                                ErrorMessageDisplay();
                                return;
                            }

                            Application["IsMailBeingSent"] = null;
                        }
                    }
                    //V2044Adding start

                    #endregion


                    #region Change Language/Thems Settings
                    //Change Language
                    //var language = LanguageChange.Language.Sinhala;
                    ////var language = (LanguageChange.Language)Session["Main_Language"];
                    ////LanguageChange.ChangeLanguage(Page, language);

                    //Theme loading
                    string physicalApplicationPath = Request.PhysicalApplicationPath;
                    //Get Application Path to get User Change Css
                    Session["Main_PhysicalApplicationPath"] = physicalApplicationPath;
                    LoadThemes();

                    //load Fav
                    loadFavourites();

                    //load Propicture
                    LoadProPicture();




                    loadBU();

                    #endregion

                    #region Load Use Roles/Menus

                    //Bind User Roles in repUerRoles Repeater
                    _userDao = new UserManager();
                    List<UserRole> userroles = _user.UserRoles;
                    List<UserRole> roles = new List<UserRole>();
                    //V2017
                    List<UserRole> userRoleWOPriority = new List<UserRole>();
                    userRoleWOPriority = userroles.ToList();
                    userRoleWOPriority.RemoveAll(x => x.RoleCode.Contains("PRTROLE") == true);
                    repUerRoles.DataSource = userRoleWOPriority;
                    repUerRoles.DataBind();





                    //Get Menues for first Role Code
                    List<UserMenu> userMenus = new List<UserMenu>();
                    bool hasSystem = false;
                    string userRole = "";
                    string userRoleDesc = "";
                    string userDefaultRole = "";
                    string userDefaultRoleDesc = "";
                    if (userroles != null && userroles.Count > 0)
                    {
                        //V2017
                        foreach (UserRole u in userroles)
                        {
                            if (u.RoleCode.ToUpper().Trim() == "AUTOROLE" || u.RoleCode.ToUpper().Trim().Contains("PRTROLE") == true)
                            {
                                hasSystem = true;
                                userRole = u.RoleCode.Trim();
                                userRoleDesc = u.Description.Trim();
                                roles.Add(u);
                            }
                            else if (u.RoleCode.Trim() == _user.DefaultRoleCode.Trim())
                            {
                                defaultUserRole = u;
                                Session["DefaultRole"] = defaultUserRole;
                                userDefaultRole = u.RoleCode.Trim();
                                userDefaultRoleDesc = u.Description.Trim();
                                roles.Add(u);
                            }
                        }
                        if (!hasSystem)
                        {
                            // userRole = userroles[0].RoleCode.Trim();

                            userMenus = _userDao.GetUserManu(_user.UserName, userDefaultRole, ref _message);
                            if (_message != null)
                            {
                                Session["Error"] = _message;
                                return;
                            }
                            _user.UserMenus = userMenus;


                            string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
                            menu_load += loadMenus(_user, userDefaultRoleDesc, userDefaultRole);
                            _navigation = menu_load + "</ul>";
                        }
                        else
                        {

                            string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
                            string priorityMenu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
                            foreach (UserRole userrole in roles)
                            {

                                List<UserMenu> userMenus1 = _userDao.GetUserManu(_user.UserName, userrole.RoleCode.Trim(), ref _message);
                                if (_message != null)
                                {
                                    Session["Error"] = _message;
                                    MessageDisplay.Dispaly((Button)sender);
                                    return;
                                }


                                _user.UserMenus = userMenus1;

                                if (userrole.RoleCode.Trim().Contains("PRTROLE") == true)
                                {
                                    priorityUserRole = userrole;
                                    Session["PriorityRole"] = priorityUserRole;
                                    priorityMenu_load += loadMenus(_user, userrole.Description.Trim(), userrole.RoleCode.Trim());
                                }
                                else
                                {
                                    menu_load += loadMenus(_user, userrole.Description.Trim(), userrole.RoleCode.Trim());
                                }



                            }

                            _navigation = menu_load + "</ul>";
                            _priorityNavigation = priorityMenu_load + "</ul>";

                        }
                        string userDefRole = userDefaultRole;
                        ScriptManager.RegisterStartupScript(Page, GetType(), "BUnitScripting", "defaultBusinessUnit('" + userDefRole + "');", true);

                    }
                    //_user.UserMenus = userMenus;


                    //string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
                    //menu_load += loadMenus(_user, _user.UserRoles[0].Description);
                    //_navigation = menu_load + "</ul>";




                    Session["Main_LoginUser"] = _user; // add modified user to session MGD
                    #endregion
                    ChangeSettings();
                    LoadControlsValues();
                    #region Load User BusinessUnit Detail

                    BusinessUnit businessUnit = _userDao.GetBusinessUnit(_user.BusinessUnit.Trim(), _user.DistributorCode, ref _message);
                    Session["Main_BusinessUnitDetail"] = businessUnit;



                    #endregion


                    Session["helpActiveTaskCode"] = "";

                    LoadSystemTask();

                    //V2017

                    //System.Timers.Timer aTimer = new System.Timers.Timer();
                    //aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    //aTimer.Interval = 30 * 1000;
                    //aTimer.Enabled = true;

                }
                else
                {
                    ChangeSettings();
                    Session["Main_LoginUser"] = _user;
                    RemoveSession();
                }

            }
            catch (Exception ex)
            {
                _message = MessageCreate.CreateErrorMessage(0, ex, "Page_Load", "XONT.Ventura.AppConsole.WEB.dll");
                ErrorMessageDisplay();
            }
        }



        #region V2007
        //    public void SendNotifications()
        //    {
        //        string message = string.Empty;
        //        lbNames.Text = "";

        //        string conStr = ConfigurationManager.ConnectionStrings["SystemDB"].ConnectionString;
        //        SqlDependency.Stop(conStr);
        //SqlDependency.Start(conStr);
        //        using (SqlConnection connection = new SqlConnection(conStr))
        //        {
        //            string query = "SELECT [AlertMessage] FROM [dbo].[ZYAdminAlert]";

        //            using (SqlCommand command = new SqlCommand(query, connection))
        //            {
        //                command.Notification = null;
        //                connection.Open();
        //                SqlDependency dependency = new SqlDependency(command);
        //                dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

        //                SqlDataReader reader = command.ExecuteReader();

        //                if (reader.HasRows)
        //                {
        //                    reader.Read();
        //                    message = reader[0].ToString();
        //                    lbNames.Text = message;

        //                }
        //            }
        //        }
        //        //MyHub1 nHub = new MyHub1();
        //        //nHub.NotifyAllClients(message);
        //    }

        //    private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        //    {
        //        if (e.Type == SqlNotificationType.Change)
        //        {
        //            SendNotifications();
        //        }
        //        {
        //            //Do somthing here
        //            Console.WriteLine(e.Type);
        //        }
        //    }

        #endregion

        protected void Page_RenderComplete(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "TabsVisibility();", true);
            int temp = tmrReminder.Interval;
        }
        #endregion

        #region Implementation for Reminders MGD
        protected void dispatchNearestReminder()
        {

            _userDao = new UserManager();
            DateTime now = DateTime.Now;
            DataTable nearestReminder = _userDao.getMostRecentReminder(_user.BusinessUnit, _user.UserName, ref _message);


            if (nearestReminder.Rows.Count > 0 && _message == null)
            {
                DateTime reminderTime = (DateTime)nearestReminder.Rows[0]["TriggerTime"];
                TimeSpan timeSpan = reminderTime - now;

                if ((int)timeSpan.TotalSeconds > 0)
                {
                    ViewState["nearestReminder"] = (string)nearestReminder.Rows[0]["ReminderID"];
                    tmrReminder.Interval = (int)timeSpan.TotalMilliseconds;
                    tmrReminder.Enabled = true;
                }
            }

        }
        //protected void setCurrentRole()
        //{
        //    ScriptManager.RegisterStartupScript(Page, GetType(), "setCureentRoleScript", "setActiveRole();", true);
        //}

        //V2007



        protected void showExpiredReminders()
        {

            _userDao = new UserManager();
            DataTable tempTbl = _userDao.GetExpiredReminders(_user.BusinessUnit, _user.UserName, ref _message);
            if (_message == null && tempTbl.Rows.Count > 0)
            {
                rptExpiredReminders.DataSource = tempTbl;
                rptExpiredReminders.DataBind();

                List<string> toDelete = new List<string>();
                foreach (DataRow row in tempTbl.Rows)
                {
                    toDelete.Add((string)row["ReminderID"]);
                    _userDao.DeleteReminders(_user.UserName, toDelete, ref _message); //-- do uncomment when neccessary
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "showExpiredRemindersScript", "showExpiredReminders();", true);// -- do uncomment when neccessary
                if (_message != null)
                {
                    ErrorMessageDisplay();
                    _message = null;
                }
            }
        }

        protected void lbtnRemindLater_Click(object sender, EventArgs e)
        {
            _userDao = new UserManager();
            string timeString = "";
            DateTime local = DateTime.Now.ToLocalTime();
            local = local.AddMinutes(10);
            timeString = local.Month.ToString() + "-" + local.Day.ToString() + "-" + local.Year.ToString() + " ";
            timeString += local.Hour.ToString() + ":" + local.Minute.ToString() + ":0 " + local.ToString("tt", CultureInfo.InvariantCulture);
            _userDao.UpdateReminder(_user.BusinessUnit, _user.UserName, lblReminderName.Text.ToString(), lblReminderTriggered.Text.ToString(), timeString, lblReminderName.Text.ToString(), ref _message);
            if (_message != null)
            {
                ErrorMessageDisplay();
                _message = null;
            }
            if (_message == null)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "scriptHideReminder", "HideReminderPopup();", true);
            }
            dispatchNearestReminder();
        }

        protected void lbtnReminderDispose_Click(object sender, EventArgs e)
        {
            _userDao = new UserManager();
            _userDao.DeleteReminders(_user.UserName, new List<string>() { lblReminderName.Text.ToString() }, ref _message);
            dispatchNearestReminder();
            loadReminders();
            if (_message != null)
            {
                ErrorMessageDisplay();
                _message = null;
            }
        }

        protected void reminderTriggered(object sender, EventArgs e)
        {
            _userDao = new UserManager();
            string reminder = ViewState["nearestReminder"].ToString();

            DataTable reminderDetails = _userDao.GetReminderJustExpired(_user.BusinessUnit, _user.UserName, reminder, ref _message);
            if (reminderDetails.Rows.Count > 0 && _message == null)
            {
                lblReminderName.Text = (string)reminderDetails.Rows[0]["ReminderID"];
                lblReminderTriggered.Text = (string)reminderDetails.Rows[0]["Message"];
                tmrReminder.Enabled = false;
                ScriptManager.RegisterStartupScript(Page, GetType(), "scriptShowReminder", "ShowReminderPopup();", true);

            }
            if (_message != null)
            {
                ErrorMessageDisplay();
                _message = null;
            }
        }

        protected void lbtnReminderCancel_Click(object sender, EventArgs e)
        {
            mlViewReminder.ActiveViewIndex = 0;
        }

        protected void saveReminder(bool isNew)
        {

            //     04-08-2016 1:56:09 PM
            DateTime reminderDate = new DateTime();
            if (txtReminderDate.Text.Trim() != "")
            {

                if (!DateTime.TryParse(txtReminderDate.Text.ToString(), out reminderDate))
                {
                    lblReminderError.Text = "Invalid Reminder Date";
                    lblReminderError.Visible = true;
                    lbtnReminderEditSave.Visible = true;
                    return;
                }
            }

            string time = reminderDate.ToShortDateString().Replace("/", "-");
            time += " ";
            time += tmeReminder.Hour.ToString();
            time += ":";
            time += tmeReminder.Minute.ToString();
            time += ":";
            time += tmeReminder.Second.ToString();
            time += " ";
            time += tmeReminder.AmPm.ToString();


            _userDao = new UserManager();
            if (isNew)
            {
                _userDao.SaveReminders(_user.BusinessUnit, _user.UserName.Trim(), txtReminderTitle.Text.ToString().TrimEnd(), txtReminderMsg.Text.ToString(), time, ref _message);

            }
            else
            {
                _userDao.UpdateReminder(_user.BusinessUnit, _user.UserName.Trim(), txtReminderTitle.Text.ToString().TrimEnd(), txtReminderMsg.Text.ToString(), time, hdnfEditReminder.Value.ToString(), ref _message);
            }

            if (_message != null)
            {
                lblReminderError.Text = "Reminder Already Exists!";
                lbtnReminderSave.Visible = true;
                return;
            }
            loadReminders();
            dispatchNearestReminder();
            mlViewReminder.ActiveViewIndex = 0;
        }
        protected void lbtnReminderSave_Click(object sender, EventArgs e)
        {
            saveReminder(true);
        }

        protected void lbtnReminderEditSave_Click(object sender, EventArgs e)
        {
            saveReminder(false);
        }
        protected void lbtnNew_click(object sender, EventArgs e)
        {
            lbtnReminderSave.Visible = true;
            btnSelectDate.Visible = true;
            lbtnEditSelectedReminder.Visible = false;
            lbtnReminderEditSave.Visible = false;
            calSelectDate.SelectedDate = DateTime.Today;
            if (rptReminders.Items.Count < 10)
            {
                mlViewReminder.ActiveViewIndex = 1;
                return;
            }
            lblRemindersCount.Text = "Upto 10 reminders only";

        }

        protected void showHideSelection(bool state)
        {
            foreach (RepeaterItem item in rptReminders.Items)
            {
                CheckBox tempCbx = (CheckBox)item.FindControl("cbxReminderSelect");
                tempCbx.Visible = state;
            }
            lbtnDelete.Visible = state;
        }
        protected void lbtnSelect_click(object sender, EventArgs e)
        {
            bool state = lbtnDelete.Visible;
            showHideSelection(!state);
        }
        protected void lbtnDelete_click(object sender, EventArgs e)
        {
            List<string> toDelete = new List<string>();
            foreach (RepeaterItem item in rptReminders.Items)
            {
                CheckBox tempCbx = (CheckBox)item.FindControl("cbxReminderSelect");

                if (tempCbx.Checked)
                {
                    LinkButton tempLbtn = (LinkButton)item.FindControl("lbtnReminder");
                    toDelete.Add(tempLbtn.Text.Trim().ToString());
                }
            }
            if (toDelete.Count > 0)
            {
                _userDao = new UserManager();
                _userDao.DeleteReminders(_user.UserName, toDelete, ref _message);
                if (_message != null)
                {
                    lblReminderError.Text = _message.Desc;
                    return;
                }
                loadReminders();
            }
            showHideSelection(false);
        }
        protected void loadReminders()
        {
            _userDao = new UserManager();
            DataTable reminders = _userDao.getReminders(_user.BusinessUnit, _user.UserName);
            rptReminders.DataSource = reminders;
            rptReminders.DataBind();
            mlViewReminder.ActiveViewIndex = 0;
        }


        protected void loadBU()//ss newly added
        {

            _userDao = new UserManager();
            DataTable bu = _userDao.getBUnit(_user.UserName.Trim(), ref _message);
            bUnit.DataSource = bu;
            bUnit.DataBind();


        }

        protected void ReminderEdit_click(object sender, EventArgs e)
        {
            hdnfEditReminder.Value = txtReminderTitle.Text;
            btnSelectDate.Visible = true;
            lbtnReminderCancel.Text = "Cancel";
            lbtnReminderEditSave.Visible = true;
        }


        protected void rptReminders_itemSelected(object sender, RepeaterCommandEventArgs e) //ReminderID, Message, TriggerTime
        {
            _userDao = new UserManager();
            string selectedReminder = e.CommandArgument.ToString();
            DataTable details = _userDao.getReminderDetails(_user.BusinessUnit, _user.UserName.ToString().Trim(), selectedReminder, ref _message);

            if (details.Rows.Count > 0 && _message == null)
            {
                lbtnEditSelectedReminder.Visible = true;
                txtReminderTitle.ReadOnly = true;
                btnSelectDate.Visible = false;
                tmeReminder.ReadOnly = true;
                calSelectDate.Enabled = false;
                txtReminderDate.ReadOnly = true;
                txtReminderMsg.ReadOnly = true;
                lbtnReminderCancel.Text = "OK";
                txtReminderDate.ReadOnly = true;

                txtReminderTitle.Text = (string)details.Rows[0]["ReminderID"];

                DateTime reminderTime = (DateTime)details.Rows[0]["TriggerTime"];

                calSelectDate.SelectedDate = reminderTime;
                txtReminderDate.Text = reminderTime.ToShortDateString();

                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                if (reminderTime.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                tmeReminder.SetTime(reminderTime.Hour, reminderTime.Minute, am_pm);

                txtReminderMsg.Text = (string)details.Rows[0]["Message"];

                mlViewReminder.ActiveViewIndex = 1;

            }
        }

        #endregion

        #region Create AppConsole elements/Handling events
        protected string createMenuTasks(string menuCode, string roleName, string roleCode) //mgd newly added
        {
            try
            {

                _userDao = new UserManager();
                List<UserTask> userTasks = _userDao.GetUserTask(menuCode, _user.UserName.Trim(), ref _message);
                if (_message != null)
                {
                    Session["Error"] = _message;
                    MessageDisplay.Dispaly(this);
                    return "";
                }

                string roleName1 = roleName.Replace(" ", "_");

                string Element_Tasks = "<ul class='sub' id='documents' style='padding-left:0;'>";
                foreach (UserTask task in userTasks)
                {
                    ////Automate the creation of  help files - V2010

                    string AppPath = Request.PhysicalApplicationPath;
                    string fileNew = AppPath + "HelpFiles\\" + task.TaskCode.Trim() + "";
                    string fileold = "E:\\XontHelpFiles\\" + task.TaskCode.Trim() + "";
                    string xmlfile = AppPath + "HelpFiles\\help.xml";
                    DirectoryCopy(fileold, fileNew, xmlfile, "parentDir", task.TaskCode.Trim(), task.Description.Trim(), true);

                    // // End


                    string icon = "";
                    string iconName = task.Icon.Trim();
                    string iconInitial = iconName.Substring(0, 3);
                    string fa = "fa-";
                    bool result = iconInitial.Equals(fa, StringComparison.Ordinal);
                    if (result == true)
                    {
                        icon = iconName;
                    }
                    else
                    {
                        icon = "fa-home";
                    }

                    //V2017
                    if (roleCode.Contains("PRTROLE") == true)
                    {
                        Element_Tasks += @"<li id='priority'  class='e drag1' style='display:inline-block';>";
                        Element_Tasks += @" <a name='divUserTask' id='" + task.TaskCode + "' style='text-decoration:none !important;'";
                        //Element_Tasks += " onclick=\"LoadTask('" + task.TaskCode.Trim() + "/" + task.TaskCode.Trim() + ".aspx',";//V2021
                        Element_Tasks += " onclick=\"LoadTask('" + task.ExecutionScript.Trim() + "',";////V2021
                        Element_Tasks += " '" + task.Description + "',";
                        Element_Tasks += " '" + task.TaskCode.Trim() + "',";
                        Element_Tasks += " '" + task.TaskType.Trim() + "',";
                        Element_Tasks += " '" + _user.UserName.ToString().Trim() + "',"; //V2049
                        Element_Tasks += " '" + task.ApplicationCode.ToString().Trim() + "',"; //V2049
                        Element_Tasks += " '" + task.ExclusivityMode.ToString().Trim() + "')\"/> "; //V2049
                        //Element_Tasks += " '" + _user.UserName.ToString().Trim() + "')\"/> "; //V2049
                        Element_Tasks += @" <i style='font-size:14px;margin-left:10px;' class='fa " + icon + "' title='" + task.Description + "'></i><span style='display:none;font-size:13px;' class='role_code'>" + task.TaskCode.Trim() + "</span>";
                        Element_Tasks += @" </a> </li>";
                    }
                    else
                    {
                        Element_Tasks += @"<li id='drag'  class='e drag1'>";
                        Element_Tasks += @" <a name='divUserTask' id='" + task.TaskCode + "'>";
                        Element_Tasks += @" <i style='font-size:14px;margin-left:10px;' class='fa " + icon + "'></i><span style='display:none;' class='role_code'>" + task.TaskCode.Trim() + "</span>";
                        Element_Tasks += @" <input id='" + roleName1 + "-" + task.TaskCode.Trim() + "_task' style='background:none;border:none;width:82%;overflow:hidden;text-overflow:ellipsis;text-align:left;padding:0;' ";
                        Element_Tasks += @" name='" + task.TaskCode + "'";
                        Element_Tasks += @" type='button' title='" + task.Description + "' value='" + task.Caption + "'";
                        Element_Tasks += @" class='AppCMenutask txt' ";
                        //Element_Tasks += " onclick=\"LoadTask('" + task.TaskCode.Trim() + "/" + task.TaskCode.Trim() + ".aspx',";//V2021
                        Element_Tasks += " onclick=\"LoadTask('" + task.ExecutionScript.Trim() + "',";////V2021
                        Element_Tasks += " '" + task.Description + "',";
                        Element_Tasks += " '" + task.TaskCode.Trim() + "',";
                        // Element_Tasks += " '1',";
                        Element_Tasks += " '" + task.TaskType.Trim() + "',";
                        Element_Tasks += " '" + _user.UserName.Trim() + "',";//V2049
                        Element_Tasks += " '" + task.ApplicationCode.ToString().Trim() + "',"; //V2049
                        Element_Tasks += " '" + task.ExclusivityMode.ToString().Trim() + "')\"/>"; //V2049
                        //Element_Tasks += " '" + _user.UserName.ToString().Trim() + "')\"/>"; //V2049


                        //url, des, taskid, tasktype, username
                        Element_Tasks += @" </a> </li>";
                    }

                    if (menuCode.ToUpper().Trim() == "AUTOMENU")
                    {
                        systemTask.Add(task);
                    }

                    //2.0.2.7
                    if (menuCode.ToUpper().Trim() == "AUTODAILY")
                    {
                        _userDao = new UserManager();
                        bool available = _userDao.CheckDailyMenu("AUTODAILY", ref _message);
                        if (_message != null)
                        {
                            Session["Error"] = _message;
                            MessageDisplay.Dispaly((btnSelectDate));
                        }
                        if (available)
                        {
                            systemTaskDaily.Add(task);
                        }
                    }
                }
                Element_Tasks += " </ul>";
                return Element_Tasks;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #region HelpFilesAutomation - V2010

        private static void DirectoryCopy(string sourceDirName, string destDirName, string xmlfile, string dirType, string taskcode, string desc, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (dir.Exists)
            {
                DirectoryInfo[] dirs = dir.GetDirectories();
                // If the destination directory does not exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                    if (dirType == "parentDir")
                    {
                        if (File.Exists(xmlfile))
                        {
                            XDocument xmlDoc = XDocument.Load(xmlfile);
                            XElement root = new XElement("HelpPage",
                                new XAttribute("ID", taskcode.Trim() + "/" + taskcode.Trim() + ".html"),
                                new XAttribute("description", desc.Trim()));
                            xmlDoc.Root.Add(root);
                            xmlDoc.Save(xmlfile);
                        }
                    }
                }
                else
                {
                    return;
                }


                // Get the file contents of the directory to copy.
                FileInfo[] files = dir.GetFiles();

                foreach (FileInfo file in files)
                {
                    // Create the path to the new copy of the file.
                    string temppath = Path.Combine(destDirName, file.Name);

                    // Copy the file.
                    file.CopyTo(temppath, false);
                }

                // If copySubDirs is true, copy the subdirectories.
                if (copySubDirs)
                {

                    foreach (DirectoryInfo subdir in dirs)
                    {
                        // Create the subdirectory.
                        string temppath = Path.Combine(destDirName, subdir.Name);

                        // Copy the subdirectories.
                        DirectoryCopy(subdir.FullName, temppath, "", "subDir", "", "", copySubDirs);
                    }
                }


            }
        }


        #endregion


        protected string createUserMenuElement(UserMenu userMenu, string roleName, string roleCode)//mgd newly added
        {
            //V2017
            string userMenuItem = "";
            if (roleCode.Contains("PRTROLE") == true)
            {
                userMenuItem = @"";
            }
            else
            {
                userMenuItem = @" <li class='d view'>
            <input name='mycheckboxes'  class='cb' type='checkbox' checked='checked'/> 
          <a href='#' id='expand' title='" + userMenu.MenuCode.Trim() + "-" + userMenu.Description + "'><i style='font-size:14px;' class='fa fa-cog'></i><span class='txt' style='overflow:hidden;white-space:nowrap;text-overflow:ellipsis;width:78%;position:absolute;'>" + userMenu.Description + "</span></a> ";
            }
            userMenuItem += createMenuTasks(userMenu.MenuCode, roleName, roleCode) + "</li>";
            return userMenuItem;
        }

        protected string loadMenus(User user, string roleName, string roleCode)//mgd newly added
        {

            //lblRoleName.Text = "role - " + roleName;
            //string MenuPanel = @" <ul id='sideNav' class='nav nav-pills nav-stacked'  style='height:0;'> ";
            //string MenuPanel = @" <ul id='sideNav' class='nav nav-pills nav-stacked'  style=''> ";
            //foreach (UserMenu menu in user.UserMenus)
            //{
            //    MenuPanel += createUserMenuElement(menu, roleName) ;
            //}
            //MenuPanel += "</ul>";
            //return MenuPanel;

            //V2017
            string MenuPanel = "";
            if (roleCode.Contains("PRTROLE") == true)
            {
                MenuPanel = @"";
            }
            else
            {
                MenuPanel = @"<li class='role_name'><a style='font-size:12px;text-transform:uppercase;background-color:transparent !important;'>Role - " + roleName + "</a></li>";
            }


            foreach (UserMenu menu in user.UserMenus)
            {
                MenuPanel += createUserMenuElement(menu, roleName, roleCode);
            }
            //MenuPanel += "</ul>";
            return MenuPanel;

        }


        //protected void repUerRoles_ItemCommand(object source, RepeaterCommandEventArgs e)//mgd newly added
        //{


        //    ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "updatePanelScript();", true);
        //    //loadMenus(_user, e.CommandArgument.ToString());
        //    loadMenus(_user, e.CommandArgument.ToString());
        //}


        protected void itemBound(object sender, RepeaterItemEventArgs e)//mgd newly added
        {
            Button role = (Button)e.Item.FindControl("BtnUserRole");
            if (role != null)
            {
                string kick = role.ToolTip.Trim();
                role.ToolTip = kick.Replace(" ", "_");
            }
        }

        //protected void btnUerRoles_Click(object sender, EventArgs e)//mgd newly added
        //{
        //    _user = (User)Session["Main_LoginUser"];
        //    _userDao = new UserManager();
        //    List<UserMenu> userMenus = _userDao.GetUserManu(_user.UserName, ((Button)sender).CommandName.Trim(), ref _message);
        //    if (_message != null)
        //    {
        //        Session["Error"] = _message;
        //        MessageDisplay.Dispaly((Button)sender);
        //        return;
        //    }

        //    _user.UserMenus = userMenus;
        //}

        protected void btnBUnit_Click(object sender, EventArgs e)//ss newly added
        {
            Button temp = (Button)sender;
            LoadThemes();
            _userDao = new UserManager();




            string RoleName = "";
            string RoleCode = "";
            string priority_menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
            priorityUserRole = (UserRole)Session["PriorityRole"];
            if (priorityUserRole != null)
            {
                RoleName = priorityUserRole.Description.Trim();
                RoleCode = priorityUserRole.RoleCode.Trim();

                _user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), RoleCode, ref _message);
                if (_message != null)
                {
                    Session["Error"] = _message;
                    return;
                }
            }


            priority_menu_load += loadMenus(_user, RoleName, RoleCode);
            _priorityNavigation = priority_menu_load + "</ul>";


            string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";


            defaultUserRole = (UserRole)Session["DefaultRole"];
            if (defaultUserRole != null)
            {
                RoleName = defaultUserRole.Description.Trim();
                RoleCode = defaultUserRole.RoleCode.Trim();
            }
            else
            {
                RoleName = _user.UserRoles[0].Description.Trim();
                RoleCode = _user.UserRoles[0].RoleCode.Trim();
            }
            _user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), RoleCode, ref _message);
            if (_message != null)
            {
                Session["Error"] = _message;
                return;
            }
            menu_load += loadMenus(_user, RoleName, RoleCode);
            _navigation = menu_load + "</ul>";

            //setting the default menu after postback to the server
            dispatchNearestReminder();
            loadFavourites();
            LoadControlsValues();
            BusinessUnit businessUnit = _userDao.GetBusinessUnit(temp.Text.Trim(), _user.DistributorCode, ref _message);
            Session["Main_BusinessUnitDetail"] = businessUnit;
            _user.BusinessUnit = temp.Text;

            //V2025Start
            var userStatic = (User)Session["Main_LoginUserStatic"];
            userStatic.BusinessUnit = _user.BusinessUnit;
            //V2025End

            this.Page_Load(null, null);
            // ScriptManager.RegisterStartupScript(Page, GetType(), "BUnitScript", "testbu1('" + test + "');", true);
        }


        protected void Check_Clicked(object sender, EventArgs e)
        {
            string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
            foreach (RepeaterItem i in repUerRoles.Items)
            {
                CheckBox cb = (CheckBox)i.FindControl("chk_box");
                if (cb.Checked == true)
                {
                    HiddenField btn_id = (HiddenField)i.FindControl("btn_id");
                    HiddenField btn_desc = (HiddenField)i.FindControl("btn_desc");
                    _user = (User)Session["Main_LoginUser"];
                    _userDao = new UserManager();
                    List<UserMenu> userMenus = _userDao.GetUserManu(_user.UserName, btn_id.Value.Trim(), ref _message);
                    if (_message != null)
                    {
                        Session["Error"] = _message;
                        MessageDisplay.Dispaly((Button)sender);
                        return;
                    }

                    _user.UserMenus = userMenus;
                    ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "updatePanelScript();", true);
                    menu_load += loadMenus(_user, btn_desc.Value.Trim(), btn_id.Value.Trim());


                }

            }
            _navigation = menu_load + "</ul>";
            ///* ss language added newly */
            //var language = (LanguageChange.Language)Session["Main_Language"];
            //var languageList = new List<LanguageList>();
            //languageList.AddRange(from b in _user.UserMenus
            //                      select new LanguageList { Key = b.MenuCode, Value = b.Description });
            //LanguageChange.ChangeLanguageInObject(ref languageList, language);
            //foreach (LanguageList list in languageList)
            //{
            //    int index = _user.UserMenus.FindIndex(p => p.MenuCode.Trim() == list.Key.Trim());
            //    if (!String.IsNullOrEmpty(list.Value)) _user.UserMenus[index].Description = list.Value;
            //}
            ///* end */
            uplMenuTasks.Update();
            //V2002
            ScriptManager.RegisterStartupScript(Page, GetType(), "graphical", "graphicalViewScript();", true);

        }


        private byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert, System.Drawing.Imaging.ImageFormat formatOfImage)//mgd newly added
        {
            byte[] imgBytes;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageToConvert.Save(ms, formatOfImage);
                    imgBytes = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return imgBytes;
        }

        private bool SaveUserChangeSettings()
        {
            bool iserror = false;
            IUserDAO userDao = new UserManager();
            userDao.UpdateUserSetting(_user, ref _msg);
            if (_msg != null)
            {
                Session["Error"] = _msg;
                MessageDisplay.Dispaly(ok);
                iserror = true;

            }
            return iserror;
        }

        protected void resetProfilePic(object sender, EventArgs e)
        {
            _userDao = new UserManager();
            _userDao.ResetProfilePicture(_user, ref _msg);
            if (_msg != null)
            {
                Session["Error"] = _msg;
                MessageDisplay.Dispaly(ok);

            }

        }

        protected void saveSettingDefault(object sender, EventArgs e) // ss newly added
        {
            if (Session["Main_LoginUser"] != null)
            {
                _user.Theme = "blue";
                _user.Language = "English";

                _user.FontName = "Trebuchet MS";
                _user.FontSize = 13;
                _user.FontColor = "#bbbbbb";
                fontName.Value = "Trebuchet MS";
                fontSize.Value = "13";
                fontColor.Value = "#bbbbbb";
                languageSelect.Value = "English";

                bool iserror = SaveUserChangeSettings();
                if (!iserror)
                {
                    Session["Theme"] = _user.Theme;

                    ReadWriteFileDefault();
                    SetLanguage();
                }
                else
                {

                    Session["Error"] = MessageCreate.CreateUserMessage(100002, "", "", "", "", "", "");
                    MessageDisplay.Dispaly(this);
                }
            }
            else
            {
                Session["Error"] = MessageCreate.CreateUserMessage(100002, "", "", "", "", "", "");
                MessageDisplay.Dispaly(this);

            }
            LoadThemes();
            _userDao = new UserManager();
            //_user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), _user.UserRoles[0].RoleCode, ref _message);
            //if (_message != null)
            //{
            //    Session["Error"] = _message;
            //    return;
            //}
            //string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
            //menu_load += loadMenus(_user, _user.UserRoles[0].Description, _user.UserRoles[0].RoleCode);//setting the default menu after postback to the server
            //_navigation = menu_load + "</ul>";
            string RoleName = "";
            string RoleCode = "";
            string priority_menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
            priorityUserRole = (UserRole)Session["PriorityRole"];
            if (priorityUserRole != null)
            {
                RoleName = priorityUserRole.Description.Trim();
                RoleCode = priorityUserRole.RoleCode.Trim();

                _user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), RoleCode, ref _message);
                if (_message != null)
                {
                    Session["Error"] = _message;
                    return;
                }
            }


            priority_menu_load += loadMenus(_user, RoleName, RoleCode);
            _priorityNavigation = priority_menu_load + "</ul>";


            string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";


            defaultUserRole = (UserRole)Session["DefaultRole"];
            if (defaultUserRole != null)
            {
                RoleName = defaultUserRole.Description.Trim();
                RoleCode = defaultUserRole.RoleCode.Trim();
            }
            else
            {
                RoleName = _user.UserRoles[0].Description.Trim();
                RoleCode = _user.UserRoles[0].RoleCode.Trim();
            }
            _user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), RoleCode, ref _message);
            if (_message != null)
            {
                Session["Error"] = _message;
                return;
            }
            menu_load += loadMenus(_user, RoleName, RoleCode);
            _navigation = menu_load + "</ul>";





            dispatchNearestReminder();
            loadFavourites();
            LoadControlsValues();

            if (imgProUpload.HasFile)
            {
                System.Drawing.Imaging.ImageFormat format;
                String fileExtension = System.IO.Path.GetExtension(imgProUpload.FileName).ToLower().Trim();

                switch (fileExtension)
                {
                    case ".jpg":
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case ".jpeg":
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case ".png":
                        format = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                    default:
                        return;
                }

                _userDao = new UserManager();

                System.Drawing.Image proImg = System.Drawing.Image.FromStream(imgProUpload.PostedFile.InputStream);
                byte[] toSave = ConvertImageToByteArray(proImg, format);
                profile_image.Src = "data:image/jpg;base64," + Convert.ToBase64String(toSave);
                _userDao.saveProfilePicture(_user.UserName.Trim(), toSave, ref _message);

                if (_message != null)
                {
                    Session["Error"] = _message;
                    MessageDisplay.Dispaly((Button)sender);
                    return;
                }
            }
        }



        protected void saveSetting(object sender, EventArgs e)//mgd newly added 
        {

            if (Session["Main_LoginUser"] != null)
            {

                _user.Theme = txtTheme.Value.Trim();
                _user.Language = languageSelect.Value.Trim();
                _user.FontName = fontName.Value.Trim();
                _user.FontSize = Convert.ToInt16(fontSize.Value.Trim());
                _user.FontColor = fontColor.Value.Trim();

                SetLanguage();

                bool iserror = SaveUserChangeSettings();
                if (!iserror)
                {
                    Session["Theme"] = _user.Theme;

                    ReadWriteFile();
                    if (defaultAvatar.Checked)
                    {
                        profile_image.Src = "assets/img/avatars/avatar.png";
                        resetProfilePic(sender, e);
                        imgProUpload.PostedFile.InputStream.Dispose();
                        //   ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "RemoveProfilePicture();", true);

                    }

                    // ReadWriteFileAppConsole();
                }
                else
                {

                    Session["Error"] = MessageCreate.CreateUserMessage(100002, "", "", "", "", "", "");
                    MessageDisplay.Dispaly(this);
                }
            }
            else
            {
                Session["Error"] = MessageCreate.CreateUserMessage(100002, "", "", "", "", "", "");
                MessageDisplay.Dispaly(this);

            }



            LoadThemes();
            _userDao = new UserManager();
            //_user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), _user.UserRoles[0].RoleCode, ref _message);
            //if (_message != null)
            //{
            //    Session["Error"] = _message;
            //    return;
            //}
            //string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
            //menu_load += loadMenus(_user, _user.UserRoles[0].Description, _user.UserRoles[0].RoleCode);//setting the default menu after postback to the server
            //_navigation = menu_load + "</ul>";
            string RoleName = "";
            string RoleCode = "";
            string priority_menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
            priorityUserRole = (UserRole)Session["PriorityRole"];
            if (priorityUserRole != null)
            {
                RoleName = priorityUserRole.Description.Trim();
                RoleCode = priorityUserRole.RoleCode.Trim();

                _user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), RoleCode, ref _message);
                if (_message != null)
                {
                    Session["Error"] = _message;
                    return;
                }
            }


            priority_menu_load += loadMenus(_user, RoleName, RoleCode);
            _priorityNavigation = priority_menu_load + "</ul>";


            string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";


            defaultUserRole = (UserRole)Session["DefaultRole"];
            if (defaultUserRole != null)
            {
                RoleName = defaultUserRole.Description.Trim();
                RoleCode = defaultUserRole.RoleCode.Trim();
            }
            else
            {
                RoleName = _user.UserRoles[0].Description.Trim();
                RoleCode = _user.UserRoles[0].RoleCode.Trim();
            }
            _user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), RoleCode, ref _message);
            if (_message != null)
            {
                Session["Error"] = _message;
                return;
            }
            menu_load += loadMenus(_user, RoleName, RoleCode);
            _navigation = menu_load + "</ul>";





            dispatchNearestReminder();
            loadFavourites();
            LoadControlsValues();

            if (imgProUpload.HasFile)
            {
                System.Drawing.Imaging.ImageFormat format;
                String fileExtension = System.IO.Path.GetExtension(imgProUpload.FileName).ToLower().Trim();

                switch (fileExtension)
                {
                    case ".jpg":
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case ".jpeg":
                        format = System.Drawing.Imaging.ImageFormat.Jpeg;
                        break;
                    case ".png":
                        format = System.Drawing.Imaging.ImageFormat.Png;
                        break;
                    default:
                        return;
                }

                _userDao = new UserManager();

                System.Drawing.Image proImg = System.Drawing.Image.FromStream(imgProUpload.PostedFile.InputStream);
                byte[] toSave = ConvertImageToByteArray(proImg, format);
                profile_image.Src = "data:image/jpg;base64," + Convert.ToBase64String(toSave);
                _userDao.saveProfilePicture(_user.UserName.Trim(), toSave, ref _message);

                if (_message != null)
                {
                    Session["Error"] = _message;
                    MessageDisplay.Dispaly((Button)sender);
                    return;
                }
            }


        }

        protected void btn_changePassword(object sender, EventArgs e)
        {

            var validator = new Validator();
            string oldPass = txtCurrentPassword.Text.ToString().Trim();
            string newPass = txtNewPassword.Text.ToString().Trim();
            string errorMsg = string.Empty;
            validator.ValidateUserPassword(_user, oldPass, newPass, '1',
                                         ref errorMsg, ref _message);

            //string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
            //menu_load += loadMenus(_user, _user.UserRoles[0].Description, _user.UserRoles[0].RoleCode);//setting the default menu after postback to the server
            //_navigation = menu_load + "</ul>";
            _userDao = new UserManager();
            string RoleName = "";
            string RoleCode = "";
            string priority_menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";
            priorityUserRole = (UserRole)Session["PriorityRole"];
            if (priorityUserRole != null)
            {
                RoleName = priorityUserRole.Description.Trim();
                RoleCode = priorityUserRole.RoleCode.Trim();

                _user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), RoleCode, ref _message);
                if (_message != null)
                {
                    Session["Error"] = _message;
                    return;
                }
            }


            priority_menu_load += loadMenus(_user, RoleName, RoleCode);
            _priorityNavigation = priority_menu_load + "</ul>";


            string menu_load = @"<ul id='sideNav' class='nav nav-pills nav-stacked'  style=''>";


            defaultUserRole = (UserRole)Session["DefaultRole"];
            if (defaultUserRole != null)
            {
                RoleName = defaultUserRole.Description.Trim();
                RoleCode = defaultUserRole.RoleCode.Trim();
            }
            else
            {
                RoleName = _user.UserRoles[0].Description.Trim();
                RoleCode = _user.UserRoles[0].RoleCode.Trim();
            }
            _user.UserMenus = _userDao.GetUserManu(_user.UserName.Trim(), RoleCode, ref _message);
            if (_message != null)
            {
                Session["Error"] = _message;
                return;
            }
            menu_load += loadMenus(_user, RoleName, RoleCode);
            _navigation = menu_load + "</ul>";





            dispatchNearestReminder();
            loadFavourites();
            LoadControlsValues();
            if (_message != null)
            {
                ErrorMessageDisplay();
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    lblPasswordError.Text = errorMsg;
                    return;
                }
                else
                {

                    //   valFailureText.Text = "Successfully Change Password";
                }
                ScriptManager.RegisterStartupScript(Page, GetType(), "changePassword", "closePasswordModal();", true);
            }
        }


        private void SetLanguage()
        {
            switch (_user.Language.Trim())
            {
                case ("English"):
                    Session["Main_Language"] = LanguageChange.Language.English;
                    break;
                case ("Sinhala"):
                    Session["Main_Language"] = LanguageChange.Language.Sinhala;
                    break;
                case ("Tamil"):
                    Session["Main_Language"] = LanguageChange.Language.Tamil;
                    break;
            }
        }
        private void SetLanguageIndex(string language)
        {
            switch (language.Trim())
            {
                case ("English"):
                    selectLanguage.SelectedIndex = 0;
                    break;
                case ("Sinhala"):
                    selectLanguage.SelectedIndex = 1;
                    break;
                case ("Tamil"):
                    selectLanguage.SelectedIndex = 2;
                    break;
            }
        }

        private void ChangeSettings()
        {
            var language = (LanguageChange.Language)Session["Main_Language"];
            _user = (User)Session["Main_LoginUser"];
            //Change Role Language
            var languageList = new List<LanguageList>();
            languageList.AddRange(from b in _user.UserRoles
                                  select new LanguageList { Key = b.RoleCode, Value = b.Description });
            LanguageChange.ChangeLanguageInObject(ref languageList, language);
            foreach (LanguageList list in languageList)
            {
                int index = _user.UserRoles.FindIndex(p => p.RoleCode.Trim() == list.Key.Trim());
                if (!String.IsNullOrEmpty(list.Value)) _user.UserRoles[index].Description = list.Value;
            }


            //Change Menu Language
            languageList = new List<LanguageList>();
            languageList.AddRange(from b in _user.UserMenus
                                  select new LanguageList { Key = b.MenuCode, Value = b.Description });
            LanguageChange.ChangeLanguageInObject(ref languageList, language);
            foreach (LanguageList list in languageList)
            {
                int index = _user.UserMenus.FindIndex(p => p.MenuCode.Trim() == list.Key.Trim());
                if (!String.IsNullOrEmpty(list.Value)) _user.UserMenus[index].Description = list.Value;
            }
            Session["Main_LoginUser"] = _user;
        }


        private void LoadControlsValues()
        {
            var fonts = new InstalledFontCollection();
            List<string> fontsNames = fonts.Families.Select(p => p.Name).ToList();
            ddlFont.DataSource = fontsNames;
            ddlFont.DataBind();
            if (ddlFontSize.Items.Count == 0)
            {
                for (int i = 0; i < 14; i++)
                {
                    ddlFontSize.Items.Add(i.ToString());
                }
                ddlFontSize.DataBind();

            }


            ////Assign User Values
            ddlFont.SelectedValue = _user.FontName.Trim();
            ddlFontSize.SelectedValue = Convert.ToString(_user.FontSize);
            colorBackgroundColor.Value = _user.FontColor.Trim();
            SetLanguageIndex(_user.Language.Trim());
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "UserSetting_Script1", "javascript:loadColor();", true);


        }

        private void LoadSystemTask()
        {
            if (systemTask.Count > 0)
            {
                foreach (UserTask task in systemTask)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "BUnitScriptingio'" + task.TaskCode + "'", "LoadTask2('" + task.TaskCode + "');", true);
                }
            }
            if (systemTaskDaily.Count > 0)
            {
                foreach (UserTask task in systemTaskDaily)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "BUnitScriptingiodaily'" + task.TaskCode + "'", "LoadTask2('" + task.TaskCode + "');", true);
                }
                _userDao = new UserManager();
                _userDao.UpdateDailyMenu("AUTODAILY", ref _message);

                if (_message != null)
                {
                    Session["Error"] = _message;
                    MessageDisplay.Dispaly((btnSelectDate));
                    return;
                }
            }

            //foreach (UserTask task in systemTask)
            //{

            //    ScriptManager.RegisterStartupScript(Page, GetType(), "BUnitScriptingio'" +task.TaskCode+"'" + task.TaskCode, "LoadTask('" + task.TaskCode.Trim() + "/" + task.TaskCode.Trim() + ".aspx','" + task.Description + "','1','" + task.TaskType.Trim() + "','" + _user.UserName.ToString().Trim() + "');", true);
            //    //System.Threading.Thread.Sleep(30000);

            //}


        }

        private void ReadWriteFile()
        {
            string AppPath = Request.PhysicalApplicationPath;
            string cssSheetName = _user.BusinessUnit.Trim() + "_" + _user.UserName.Trim();
            string fileNew = AppPath + "App_Themes\\UserThemes\\" + cssSheetName + ".css";
            string fileOri = AppPath + "App_Themes\\" + _user.Theme.Trim() + "\\StyleSheet.css";
            string txtLine = "";
            string emptyTxtLine = "";
            try
            {
                if (File.Exists(fileNew))
                {
                    File.Delete(fileNew);
                }
            }
            catch (Exception)
            {
            }

            StreamReader objReader;
            StreamWriter objWriter;

            objReader = new StreamReader(fileOri);
            objWriter = new StreamWriter(fileNew);
            do
            {
                emptyTxtLine = objReader.ReadLine();
                emptyTxtLine = emptyTxtLine.Replace("\t", "");
                if (emptyTxtLine.Trim() == "")
                {
                    continue;
                }
                txtLine = emptyTxtLine + "\r\n";
                string[] words = txtLine.Split(':');
                string firstText = "";
                string last = "";
                if (words.Length == 2)
                {
                    firstText = words[0].Trim();
                    last = words[1].Trim();


                    bool isChangeStyle = false;
                    switch (words[0].Trim())
                    {
                        case ("font-family"):
                            last = fontName.Value.Trim();
                            isChangeStyle = true;
                            break;
                        case ("color"):
                            last = fontColor.Value.Trim();
                            isChangeStyle = true;
                            break;
                        case ("font-size"):
                            last = fontSize.Value.Trim() + "px";
                            isChangeStyle = true;
                            break;
                    }


                    last = last.Replace("url(" + '"' + "images/",
                                        "url(" + '"' + "../" + _user.Theme + "/images/");

                    if (isChangeStyle)
                    {
                        txtLine = firstText + ":" + last + ";";
                    }
                    else
                    {
                        txtLine = firstText + ":" + last;
                    }
                }
                objWriter.WriteLine(txtLine);
            } while (objReader.Peek() != -1);
            objReader.Close();
            objWriter.Close();
        }

        //private void ReadWriteFileAppConsole()
        //{
        //    string AppPath = Request.PhysicalApplicationPath;
        //    string cssSheetName = _user.BusinessUnit.Trim() + "_" + _user.UserName.Trim();
        //    string fileNew = AppPath + "assets\\css\\UserThemes\\" + cssSheetName + ".css";
        //    string fileOri = AppPath + "assets\\css\\" + _user.Theme.Trim() + ".css";
        //    string txtLine = "";
        //    string emptyTxtLine = "";
        //    try
        //    {
        //        if (File.Exists(fileNew))
        //        {
        //            File.Delete(fileNew);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    StreamReader objReader;
        //    StreamWriter objWriter;

        //    objReader = new StreamReader(fileOri);
        //    objWriter = new StreamWriter(fileNew);
        //    do
        //    {
        //        emptyTxtLine = objReader.ReadLine();
        //        emptyTxtLine = emptyTxtLine.Replace("\t", "");
        //        if (emptyTxtLine.Trim() == "")
        //        {
        //            continue;
        //        }
        //        txtLine = emptyTxtLine + "\r\n";
        //        string[] words = txtLine.Split(':');
        //        string firstText = "";
        //        string last = "";
        //        if (words.Length == 2)
        //        {
        //            firstText = words[0].Trim();
        //            last = words[1].Trim();


        //            bool isChangeStyle = false;
        //            switch (words[0].Trim())
        //            {
        //                case ("font-family"):
        //                    last = ddlFont.SelectedValue.Trim();
        //                    isChangeStyle = true;
        //                    break;
        //                case ("color"):
        //                    last = colorBackgroundColor.Value.Trim();
        //                    isChangeStyle = true;
        //                    break;
        //                case ("font-size"):
        //                    last = ddlFontSize.SelectedValue.Trim() + "px";
        //                    isChangeStyle = true;
        //                    break;
        //            }


        //            last = last.Replace("url(" + '"' + "../images/",
        //                                "url(" + '"' + "../../images/");

        //            if (isChangeStyle)
        //            {
        //                txtLine = firstText + ":" + last + ";";
        //            }
        //            else
        //            {
        //                txtLine = firstText + ":" + last;
        //            }
        //        }
        //        objWriter.WriteLine(txtLine);
        //    } while (objReader.Peek() != -1);
        //    objReader.Close();
        //    objWriter.Close();

        //}

        private void ReadWriteFileDefault()
        {
            string AppPath = Request.PhysicalApplicationPath;
            string cssSheetName = _user.BusinessUnit.Trim() + "_" + _user.UserName.Trim();
            string fileNew = AppPath + "App_Themes\\UserThemes\\" + cssSheetName + ".css";
            string fileOri = AppPath + "App_Themes\\" + _user.Theme.Trim() + "\\StyleSheet.css";
            string txtLine = "";
            string emptyTxtLine = "";
            try
            {
                if (File.Exists(fileNew))
                {
                    File.Delete(fileNew);
                }
            }
            catch (Exception)
            {
            }
        }

        //private void ReadWriteFileDefaultAppConsole()
        //{
        //    string AppPath = Request.PhysicalApplicationPath;
        //    string cssSheetName = _user.BusinessUnit.Trim() + "_" + _user.UserName.Trim();
        //    string fileNew = AppPath + "assets\\css\\UserThemes\\" + cssSheetName + ".css";
        //    string fileOri = AppPath + "assets\\css\\" + _user.Theme.Trim() + "\\StyleSheet.css";

        //    string txtLine = "";
        //    string emptyTxtLine = "";
        //    try
        //    {
        //        if (File.Exists(fileNew))
        //        {
        //            File.Delete(fileNew);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}


        protected void LoadProPicture()//mgd newly added
        {

            if (_user.HasProPicture == '1')
            {
                _userDao = new UserManager();

                byte[] imageByte = _userDao.getImageData(_user.UserName.Trim(), ref _message);
                if (_message == null)
                {
                    profile_image.Src = "data:image/jpg;base64," + Convert.ToBase64String(imageByte);
                }

                if (_message != null)
                {
                    ErrorMessageDisplay();
                    _message = null;
                }
            }

        }

        protected void btnNoteClick(object sender, EventArgs e)//mgd newly added
        {
            Session["TaskCode"] = txtCurrentTaskCode.Value.ToString();
            string temp = Session["TaskCode"].ToString();
            ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "trial1();", true);
        }
        protected void btnSearchClose_Click(object sender, EventArgs e)//ss newly added
        {
            Check_Clicked(sender, e);
        }
        //protected void btnViewAlt_Click(object sender, EventArgs e)//ss newly added
        //{
        //    Check_Clicked(sender, e);
        //}
        //protected void btnView_Click(object sender, EventArgs e)//ss newly added
        //{
        //    Check_Clicked(sender, e);
        //}

        protected void btnNoteCloseClick(object sender, EventArgs e)//mgd newly added
        {

            ScriptManager.RegisterStartupScript(Page, GetType(), "script1", "trial2();", true);
        }

        public void ComponentUserThemes(Page page)
        {
            _user = (User)Session["Main_LoginUser"];
            string AppPath = Session["Main_PhysicalApplicationPath"].ToString().Trim();
            string cssSheetName = _user.BusinessUnit.Trim() + "_" + _user.UserName.Trim();
            string fileNew = AppPath + "App_Themes\\UserThemes\\" + cssSheetName + ".css";
            if (File.Exists(fileNew))
            {
                string css = @"<link href=""../App_Themes/UserThemes/" + cssSheetName + ".css" +
                             @""" type=""text/css"" rel=""stylesheet"" />";

                page.ClientScript.RegisterStartupScript(page.GetType(), page + "_UserSettingCSS", css, false);
            }
        }
        protected void LoadThemes()//mgd newly added
        {

            _user = (User)Session["Main_LoginUser"];
            string selectedTheme = _user.Theme.Trim();
            ScriptManager.RegisterStartupScript(Page, GetType(), "te", "ApplyColor('" + selectedTheme + "')", true);
            string AppPath = Session["Main_PhysicalApplicationPath"].ToString().Trim();
            string cssSheetName = _user.BusinessUnit.Trim() + "_" + _user.UserName.Trim();
            string fileNew = AppPath + "App_Themes\\UserThemes\\" + cssSheetName + ".css";
            if (File.Exists(fileNew))
            {

                string css = @"<link href=""" + ResolveUrl("App_Themes/UserThemes/" + cssSheetName + ".css") +
                            @""" type=""text/css"" rel=""stylesheet"" />";
                ScriptManager.RegisterStartupScript(Page, GetType(), "Main_ScriptTheme", css, false);
                Session["Theme"] = selectedTheme;
            }
            else
            {
                string css = @"<link href=""" + ResolveUrl("./assets/css/" + selectedTheme + ".css") +
                             @""" type=""text/css"" rel=""stylesheet"" />";
                ScriptManager.RegisterStartupScript(Page, GetType(), "Main_ScriptTheme", css, false);
                Session["Theme"] = selectedTheme;
            }



            //string selectedTheme = _user.Theme.Trim();
            //ScriptManager.RegisterStartupScript(Page, GetType(), "te", "ApplyColor('" + selectedTheme + "')", true);
            //try
            //{
            //    string css = @"<link href=""" + ResolveUrl("./assets/css/" + selectedTheme + ".css") +
            //                 @""" type=""text/css"" rel=""stylesheet"" />";
            //    ScriptManager.RegisterStartupScript(Page, GetType(), "Main_ScriptTheme", css, false);
            //        Session["Theme"] = selectedTheme;
            //    }
            //    catch (Exception e){

            //}
        }

        protected void loadFavourites()//mgd newly added
        {
            try
            {
                _userDao = new UserManager();
                DataTable fav = _userDao.getUserfavourites(_user.BusinessUnit.Trim(), _user.UserName.Trim(), ref _message);
                foreach (DataRow row in fav.Rows)
                {
                    string id = row["BookmarkID"].ToString();
                    string name = row["Bookmark"].ToString();
                    string value = row["TaskPath"].ToString();
                    int rowNumb = fav.Rows.IndexOf(row);

                    CheckBox rowCBX = new CheckBox();
                    rowCBX.ID = id;
                    rowCBX.CssClass = "cbx_css";
                    rowCBX.Style.Add("margin", "5px 4px 0px 4px");

                    HyperLink rowHype = new HyperLink();
                    rowHype.Text = name;
                    rowHype.ID = id + "_link";
                    rowHype.NavigateUrl = value;
                    rowHype.CssClass = "fav_css";

                    System.Web.UI.HtmlControls.HtmlGenericControl rowDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");

                    rowDiv.ID = id + "_div";

                    rowDiv.Controls.Add(rowCBX);
                    rowDiv.Controls.Add(rowHype);

                    plhFavourite.Controls.Add(rowDiv);

                }
            }
            catch (Exception e)
            { }

        }

        #endregion

        #region User Role/Menu/Task Loading Functions

        [WebMethod]
        public static void saveBookmarks(string bookmark_id, string taskPath, string bookmark_name) //mgd newly added
        {

            IUserDAO userBll = new UserManager();
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            MessageSet temp = new MessageSet();
            userBll.saveFavourites(user.BusinessUnit.Trim(), user.UserName.Trim(), bookmark_id.ToString().Trim(), bookmark_name.ToString().Trim(), taskPath.ToString().Trim(), ref temp);
        }

        [WebMethod]
        public static void deleteBookmarks(string[] arryOfB)//mgd newly added
        {
            //string[] toBeDeleted = arryOfB;
            IUserDAO userBLL = new UserManager();
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            MessageSet temp = new MessageSet();
            userBLL.DeleteBookmarks(user.UserName, arryOfB, ref temp);
        }

        [WebMethod]
        //This Method Use to Add entry to the Task Log file

        public void setCurrentTaskSession(string taskId)
        {

        }

        [WebMethod]
        public static void saveTheme(string theme) //mgd newly added
        {

            IUserDAO userBLL = new UserManager();
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            user.Theme = theme.ToString().Trim();
            MessageSet temp = new MessageSet();
            userBLL.UpdateUserSetting(user, ref temp);

        }

        [WebMethod]
        public static void AddTaskDetails(string taskId)//mgd modified from old app console
        {

            try
            {
                User user = (User)HttpContext.Current.Session["Main_LoginUser"];
                //HttpContext.Current.Session.Add("TaskCode",taskId); Wrong - get the active task not the opening task
                IUserDAO userBLL = new UserManager();
                ActiveUserTask activeUserTask = new ActiveUserTask();
                MessageSet msg = null;

                activeUserTask.SessionID = HttpContext.Current.Session.SessionID.ToString();
                activeUserTask.TaskCode = taskId;
                activeUserTask.UserName = user.UserName;
                activeUserTask.BusinessUnit = user.BusinessUnit;
                activeUserTask.Status = "1";
                activeUserTask.ExecutionType = "0";
                userBLL.LogTaskInfo(activeUserTask, ref msg);
                string temp = (string)HttpContext.Current.Session["TaskCode"];
            }

            catch (Exception e)
            {


            }

        }
        //VR003 End

        //V2049S
        [WebMethod]
        public static string LogActiveTaskInfo(string taskId, string applicationCode, string exclusivityMode)
        {
            string errorMsg = "";
            try
            {
                User user = (User)HttpContext.Current.Session["Main_LoginUser"];
                IUserDAO userBLL = new UserManager();
                ActiveUserTask activeUserTask = new ActiveUserTask();
                MessageSet msg = null;

                if (user != null)
                {
                    activeUserTask.BusinessUnit = user.BusinessUnit;
                    activeUserTask.ApplicationCode = applicationCode;
                    activeUserTask.TaskCode = taskId;
                    activeUserTask.ExclusivityMode = exclusivityMode;
                    activeUserTask.UserName = user.UserName;
                    activeUserTask.PowerUser = user.PowerUser;
                    activeUserTask.WorkstationID = user.WorkstationId;
                    activeUserTask.StatusFlag = "1";
                    activeUserTask.SessionID = HttpContext.Current.Session.SessionID.ToString();

                    errorMsg = userBLL.LogActiveTaskInfo(activeUserTask, ref msg);
                }
            }

            catch (Exception e)
            {


            }

            return errorMsg;
        }
        //V2049E
        #endregion

        #region Log Out and Task Close

        #region LogOut

        protected void btnLogout_Click(object sender, EventArgs e)//mgd newly added
        {
            HttpContext.Current.Session["Main_LogOutResion"] = "User LogOut";
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Redirect("Login.aspx?Main_LogOut=User LogOut");

        }

        #endregion

        #region Task Close

        //This Method Use to Remove Session Variabales
        //Each Component Sessions are Naming Like TaskCode_Name(VRMNT01_DataList)
        //When Task is Close Remove all Sessions with contain closed Task Code Name before '_' 
        public void RemoveSession() //mgd modified
        {
            //ToDo: Need to modify to add text box and update it
            string taskCodeList = txtTaskCode.Value;
            string[] taskList = taskCodeList.Split(',');

            foreach (string s in taskList)
            {
                if (Session.Count > 0)
                {
                    NameObjectCollectionBase.KeysCollection sessionList = Session.Keys;
                    var sessionListBack = new List<string>();
                    for (int i = 0; i < sessionList.Count; i++)
                    {
                        string[] sessionKey = sessionList[i].Split('_');

                        if (sessionKey[0].Trim() == s.Trim())
                        {
                            sessionListBack.Add(sessionList[i]);
                            //check whether this is needed if yes find out why and when on what purpose
                        }
                    }

                    foreach (string count in sessionListBack)
                    {
                        Session.Remove(count);
                    }
                }
            }
        }


        [WebMethod]
        public static void AbandonSession()
        {
            //V2023Start
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            IUserDAO userBLL = new UserManager();
            MessageSet msg = null;
            if (HttpContext.Current.Session["PagingToggledComponents"] != null)
            {
                List<ComponentMasterControl> ComponentMasterAlterations = (List<ComponentMasterControl>)HttpContext.Current.Session["PagingToggledComponents"];
                foreach (ComponentMasterControl task in ComponentMasterAlterations)
                {
                    userBLL.RestorePagingToggle(user.BusinessUnit, task.ComponentID.Trim(), task.AllowPaging.Trim(), out msg);
                }
                HttpContext.Current.Session["PagingToggledComponents"] = null;
            }

            //V2049 S
            if (user != null)
            {
                ActiveUserTask activeUserTask = new ActiveUserTask();
                msg = null;
                activeUserTask.SessionID = HttpContext.Current.Session.SessionID.ToString();
                activeUserTask.TaskCode = "";
                activeUserTask.BusinessUnit = user.BusinessUnit;
                activeUserTask.UserName = user.UserName;
                userBLL.UpdateActiveTask(activeUserTask, "2", ref msg);
            }
            //V2049 E

            //V2023End
            //HttpContext.Current.Session["Main_LogOutResion"] = "Browser Closed";
            //HttpContext.Current.Session.Abandon();
            //HttpContext.Current.Response.Redirect("Login.aspx?Main_LogOut=Browser Closed");
        }

        [WebMethod]
        //This Method Use to Remove Session Variabales
        //When Task is Close Remove all Sessions with contain closed  Task Code Name before '_' 
        public static void ClearSessionTask(string taskId)// mgd modifing according to new tab closing method, active tab setting won't be needed
        {

            if (HttpContext.Current.Session.Count > 0)
            {
                var keyCount = new List<int>();
                NameObjectCollectionBase.KeysCollection sessionList = HttpContext.Current.Session.Keys;
                var sessionListBack = new List<string>();
                for (int i = 0; i < sessionList.Count; i++)
                {
                    string[] sessionKey = sessionList[i].Split('_');

                    if (sessionKey[0].Trim() == taskId.Trim())
                    {
                        sessionListBack.Add(sessionList[i]);
                    }
                }

                foreach (string count in sessionListBack)
                {
                    HttpContext.Current.Session.Remove(count);
                }
            }
            //if (activeTab.Trim() == "3")
            //{
            //    //  ifm_Main_4.Attributes.Add("src","Blank.aspx");
            //}

            //VR003 Begin - Update Log table for closed Task
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            IUserDAO userBLL = new UserManager();
            ActiveUserTask activeUserTask = new ActiveUserTask();
            MessageSet msg = null;
            activeUserTask.SessionID = HttpContext.Current.Session.SessionID.ToString();
            activeUserTask.TaskCode = taskId;
            activeUserTask.ExecutionType = "1";
            userBLL.LogTaskInfo(activeUserTask, ref msg);
            //VR003 End

            //V2049 S

            if (user != null)
            {
                activeUserTask.BusinessUnit = user.BusinessUnit;

                userBLL.UpdateActiveTask(activeUserTask, "2", ref msg);
            }
            //V2049 E

            //V2023Start
            if (HttpContext.Current.Session["PagingToggledComponents"] != null)
            {
                List<ComponentMasterControl> ComponentMasterAlterations = (List<ComponentMasterControl>)HttpContext.Current.Session["PagingToggledComponents"];
                ComponentMasterControl closedTask = ComponentMasterAlterations.Find(c => c.ComponentID == taskId.Trim());
                if (closedTask != null)
                {
                    userBLL.RestorePagingToggle(user.BusinessUnit, closedTask.ComponentID.Trim(), closedTask.AllowPaging.Trim(), out msg);
                    ComponentMasterAlterations.Remove(closedTask);
                }
            }
            //V2023End

        }

        #endregion

        protected void ScriptManager1_Unload(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                //try this for the javascript not working part in the UpdatePanel content
            }
        }

        protected void txtImgCounter_Unload(object sender, EventArgs e)
        {
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
        }

        #endregion

        #region Common Functions

        private void ErrorMessageDisplay()
        {
            Session["Error"] = _message;
            MessageDisplay.Dispaly(this);
        }

        #endregion

        #region UserMail

        [WebMethod]
        public static void ImgMailClick()
        {

            User _user = (User)HttpContext.Current.Session["Main_LoginUser"];
            string AppPath = HttpContext.Current.Request.PhysicalApplicationPath;
            string imgName = _user.BusinessUnit.Trim() + "_" + _user.UserName.Trim() +
                             DateTime.Now.ToString("yyyy-mm-dd-HH-mm-ss-tt");

            string fileNew = @AppPath + "UserMail\\" + imgName + ".jpg";



            HttpContext.Current.Session["Main_MailImage"] = fileNew;
            GetIframelSceenCapture(fileNew);

            //ScriptManager.RegisterStartupScript(Page, GetType(), "script1",
            //                                    "javascript:alert('kkk');window.PopUpURL('UserMail.aspx');", true);
        }

        //To Get Screen Capture
        private static void GetIframelSceenCapture(string filePath)
        {
            int screenWidth = Screen.GetBounds(new Point(0, 0)).Width;
            int screenHeight = Screen.GetBounds(new Point(0, 0)).Height;
            var bmpScreenShot = new Bitmap(screenWidth - 255, screenHeight - 209);
            Graphics gfx = Graphics.FromImage(bmpScreenShot);
            gfx.CopyFromScreen(255, 205, 0, 0, new Size(screenWidth, screenHeight));
            bmpScreenShot.Save(filePath, ImageFormat.Jpeg);

            GC.Collect();

        }

        #endregion

        #region For Admin Alert
        [WebMethod]
        public static string CheckNotification()
        {
            if (HttpContext.Current.Cache["AdminNewAlert"] != null)
            {
                string message = HttpContext.Current.Cache["AdminNewAlert"].ToString();
                string oldMessage = HttpContext.Current.Session["AdminPrevAlert"].ToString();

                if (oldMessage != message)
                {
                    HttpContext.Current.Session["AdminPrevAlert"] = message;
                    return message;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }



        #endregion

        #region Admin Alert Timer Fumction
        public void SendNotifications(int i)
        {
            if (i == 1)
            {

                string message = string.Empty;
                _userDao = new UserManager();

                DataTable nearestAlert = _userDao.getMostRecentAlert(ref _message);

                if (_message != null)
                {
                    ErrorMessageDisplay();
                    _message = null;
                }
                if (nearestAlert.Rows.Count > 0 && _message == null)
                {
                    int Repeat = (int)nearestAlert.Rows[0]["RepeatTimes"];
                    if (Repeat > 0)
                    {
                        message = (string)nearestAlert.Rows[0]["AlertMessage"];
                        DateTime Alert = (DateTime)nearestAlert.Rows[0]["AlertTime"];
                        int TimeInterval = (int)nearestAlert.Rows[0]["TimeInterval"];
                        DateTime TimeSpanOriginal = Alert.AddSeconds(TimeInterval);
                        string TimeSpan = TimeSpanOriginal.ToString();
                        int AlertNumber = (int)nearestAlert.Rows[0]["AlertNumber"];
                        int RecID = (int)nearestAlert.Rows[0]["RecID"];

                        _userDao.UpdateAlertCurrent(AlertNumber, RecID, TimeSpan, ref _message);
                        if (_message != null)
                        {
                            ErrorMessageDisplay();
                            _message = null;
                        }

                    }
                    else
                    {
                        message = (string)nearestAlert.Rows[0]["AlertMessage"];
                        int AlertNumber = (int)nearestAlert.Rows[0]["AlertNumber"];
                        int RecID = (int)nearestAlert.Rows[0]["RecID"];

                        System.Timers.Timer _delayTimer = new System.Timers.Timer();
                        _delayTimer.Interval = 1000;
                        _delayTimer.Elapsed += (o, e) => _userDao.DeleteAlert(AlertNumber, RecID, ref _message);
                        _delayTimer.Start();
                        if (_message != null)
                        {
                            ErrorMessageDisplay();
                            _message = null;
                        }
                    }


                }

                NotificationHub nHub = new NotificationHub();
                nHub.NotifyAllClients(message);
            }
        }


        #endregion

        #region User Notification - V2017  


        public void CheckNewNotification()


        {

            _user = (User)Session["Main_LoginUser"];
            _userDao = new UserManager();
            UnreadMessages.Attributes.Add("style", "display:none");


            _userDao = new UserManager();
            DataTable Notifications = _userDao.getUserNotification(_user.UserName, _user.BusinessUnit, ref _message);
            if (_message != null)
            {
                Session["Error"] = _message;
                return;
            }

            rep_Notifications.DataSource = Notifications;
            rep_Notifications.DataBind();

            if (_message != null)
            {
                ErrorMessageDisplay();
                _message = null;
            }

            int unreadNotifications = _userDao.getNewNotification(_user.UserName, _user.BusinessUnit, 1, ref _message);

            if (_message != null)
            {
                ErrorMessageDisplay();
                _message = null;
            }
            if (unreadNotifications != 0)
            {
                UnreadMessages.InnerHtml = unreadNotifications.ToString();
                ScriptManager.RegisterStartupScript(this, GetType(), "notification", "showNotification();", true);
            }
        }
        [WebMethod]
        public static void OpenNotification()
        {
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            IUserDAO userBLL = new UserManager();
            MessageSet msg = null;
            userBLL.UpdateNotification(user.BusinessUnit, user.UserName, 0, 1, ref msg);
        }

        [WebMethod]
        public static void deleteNotifications(int[] arryOfB)
        {
            IUserDAO userBLL = new UserManager();
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            MessageSet msg = new MessageSet();
            userBLL.DeleteNotifications(user.UserName, arryOfB, ref msg);
        }

        [WebMethod]
        public static void executeNotification(int recId)
        {
            IUserDAO userBLL = new UserManager();
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            MessageSet msg = new MessageSet();
            userBLL.UpdateNotification(user.BusinessUnit, user.UserName, recId, 2, ref msg);
        }

        [WebMethod]
        public static Array getNotificationTask(string taskCode)
        {
            IUserDAO userBLL = new UserManager();
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            MessageSet msg = new MessageSet();
            UserTask userTasks = userBLL.UserNotificationTask(taskCode, user.UserName.Trim(), ref msg);
            List<string> parameter = new List<string>();
            parameter.Add(userTasks.TaskCode.Trim());
            parameter.Add(userTasks.Description.Trim());
            parameter.Add(userTasks.TaskType.Trim());
            parameter.Add(userTasks.UserName.ToString().Trim());
            parameter.Add(userTasks.url);
            //+ userTasks.Description + "','" + userTasks.TaskCode.Trim() + "','" + userTasks.TaskType.Trim() + "','" + userTasks.UserName.ToString().Trim() + "' ";
            return parameter.ToArray();
        }


        protected void tmrNotification_Tick(object sender, EventArgs e)
        {
            try
            {
                CheckNewNotification();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        #endregion

        #region UserNotification Task - V2018
        //V2036Adding start
        [WebMethod]
        public static void ClearSystemInfoSessions()
        {
            HttpContext.Current.Session["System_XONTLibs"] = null;
            HttpContext.Current.Session["System_ClientLibs"] = null;
            HttpContext.Current.Session["System_ThirdPartyLibs"] = null;
            HttpContext.Current.Session["System_ComponentLibs"] = null;
            HttpContext.Current.Session["System_Level1Refs"] = null;
            HttpContext.Current.Session["System_Level2Refs"] = null;
        }
        //V2036Adding end


        [WebMethod]
        public static Array checkUserNotificationTask(string taskCode)
        {
            IUserDAO userBLL = new UserManager();
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            MessageSet msg = new MessageSet();
            bool exists = userBLL.CheckUserNotificationTask(taskCode, user.BusinessUnit.Trim(), ref msg);
            string taskExists = "";
            if (exists)
            {
                taskExists = "true";
            }
            else
            {
                taskExists = "false";
            }
            List<string> parameter = new List<string>();
            parameter.Add(taskCode.Trim());
            parameter.Add(taskExists.Trim());
            //+ userTasks.Description + "','" + userTasks.TaskCode.Trim() + "','" + userTasks.TaskType.Trim() + "','" + userTasks.UserName.ToString().Trim() + "' ";
            return parameter.ToArray();
        }
        #endregion

        [WebMethod]
        public static void UpdateUserNotification(string taskCode)
        {
            IUserDAO userBLL = new UserManager();
            User user = (User)HttpContext.Current.Session["Main_LoginUser"];
            MessageSet msg = new MessageSet();
            userBLL.UpdateUserNotification(user.BusinessUnit.Trim(), user.UserName.Trim(), taskCode, ref msg);
        }


    }
}
