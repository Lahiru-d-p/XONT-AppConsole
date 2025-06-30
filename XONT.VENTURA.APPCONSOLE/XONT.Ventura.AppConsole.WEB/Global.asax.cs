using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;
using System.Web.UI;
using XONT.Common.Message;


namespace XONT.Ventura.AppConsole
{
    public class Global : HttpApplication
    {
        private StreamWriter _StreamWriter;
        System.Timers.Timer timScheduledTask;

        protected void Application_Start(object sender, EventArgs e)
        {
            Application["Main_UserCount"] = 0;
            var users = new List<User>();
            Application["Main_User"] = users;
            timScheduledTask = new System.Timers.Timer();

            // Timer interval is set in miliseconds,
            timScheduledTask.Interval = 30 * 1000;

            timScheduledTask.Enabled = true;
            timScheduledTask.Elapsed +=
            new System.Timers.ElapsedEventHandler(timScheduledTask_Elapsed);

            GlobalConfiguration.Configure(WebApiConfig.Register);//V2021
        }



        void timScheduledTask_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Main instance = new Main();
                instance.SendNotifications(1);



            }
            catch (Exception ex)
            {
                return;
            }

        }


        protected void Session_Start(object sender, EventArgs e)
        {
            Session["Main_LogOutResion"] = "Session TimeOut";
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("x-frame-options", "SAMEORIGIN");//VR029
            //SAMEORIGIN - allows page to be loaded in an iframe only if container page and iframe page are loaded from the same domain
            //DENY - disallows page to be loaded in an iframe (regardless of the domain)
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                // Get the error details 
                HttpException lastErrorWrapper = Server.GetLastError() as HttpException;

                Exception lastError = lastErrorWrapper;
                if (lastErrorWrapper.InnerException != null)
                    lastError = lastErrorWrapper.InnerException;

                string message = "";
                message += "\r\n\r\n ---------- Ventura Error Log Created On " + DateTime.Now.ToString() + "---------------------------------";

                message += "\r\n\r\nError Type Name : " + lastError.GetType().ToString();
                message += "\r\n\r\nMessage         : " + lastError.Message;
                message += "\r\n\r\nError Source    : " + lastError.Source;
                message += "\r\n\r\nTarget Site     :" + lastError.TargetSite;
                if (lastError.InnerException != null)
                {
                    message += "\r\n\r\nInner Exception Source :" + lastError.InnerException.Source;
                    message += "\r\n\r\nInner Exception Message :" + lastError.InnerException.Message;
                }
                message += "\r\n\r\nStack Trace  :" + lastError.StackTrace;
                message += "\r\nLog Completed\r\n----------------------------------------------------------------------------------------------------------";

                try
                {
                    //EventLog Log = new EventLog();
                    //Log.Source = "Ventura";
                    //Log.WriteEntry(message, EventLogEntryType.Information);
                    WriteLogFile(message);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public void OnUnhandledException(object o, UnhandledExceptionEventArgs e)
        {
            // Let this occur one time for each AppDomain.

            StringBuilder message =
                new StringBuilder(
                    "\r\n\r\nUnhandledException logged by Ventura UnhandledExceptionModule:\r\n\r\nappId=");

            string appId = (string)AppDomain.CurrentDomain.GetData(".appId");
            if (appId != null)
            {
                message.Append(appId);
            }

            Exception currentException = null;
            for (currentException = (Exception)e.ExceptionObject;
                 currentException != null;
                 currentException = currentException.InnerException)
            {
                message.AppendFormat("\r\n\r\ntype={0}\r\n\r\nmessage={1}\r\n\r\nstack=\r\n{2}\r\n\r\n",
                                     currentException.GetType().FullName,
                                     currentException.Message,
                                     currentException.StackTrace);

            }

            EventLog Log = new EventLog();
            Log.Source = "Ventura";
            Log.WriteEntry(message.ToString(), EventLogEntryType.Error);

        }



        protected void Session_End(object sender, EventArgs e)
        {
            //  Main login=new Main();
            UserLogOut(Session.SessionID);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            timScheduledTask.Enabled = false;
            ClosLogFile();
        }

        public void UserLogOut(string sessionID)
        {
            Application.Lock();
            var users = new List<User>();
            users = (List<User>)Application["Main_User"];
            var userOb = new User();
            if (!users.Equals(null))
            {
                foreach (User list in users)
                {
                    if (list.SessionId.Equals(sessionID.Trim()))
                    {
                        MessageSet message = null;
                        IUserDAO userDao = new UserManager();

                        //V2049 S
                        ActiveUserTask activeUserTask = new ActiveUserTask();
                        message = null;
                        activeUserTask.SessionID = list.SessionId;
                        activeUserTask.TaskCode = "";
                        activeUserTask.BusinessUnit = list.BusinessUnit;
                        userDao.UpdateActiveTask(activeUserTask, "2", ref message);
                        //V2049 E

                        int userCount = Convert.ToInt32(Application["Main_UserCount"]);
                        Application["Main_UserCount"] = userCount - 1;
                        userOb = list;
                        users.Remove(list);
                        userOb.Reson = Session["Main_LogOutResion"].ToString();
                        message = null;
                        userDao.SaveTimeOut(userOb, ref message);
                        if (message != null)
                        {
                            Session["Error"] = message;
                            MessageDisplay.Dispaly(new Control());
                            return;
                        }
                        break;
                    }
                }
            }

            Application["Main_User"] = users;

            Application.UnLock();
            //Response.Redirect("Login.aspx?Main_LogOut=Session Time Out"); //commented due to unexpected error Event code:3005
        }

        //VR008 Begin
        private void WriteLogFile(string message)
        {
            try
            {
                //OpenLogFile();
                //_StreamWriter.WriteLine(message);


                string fileName = GetLogFileName();
                FileStream logFile = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter writer = new StreamWriter(logFile);
                writer.WriteLine(message);
                writer.Close();
                logFile.Close();

            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void OpenLogFile()
        {
            if (_StreamWriter != null) return;

            string logpath = GetLogFileName();
            try
            {
                _StreamWriter = new StreamWriter(logpath, true);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void ClosLogFile()
        {
            try
            {
                if (_StreamWriter != null)
                {
                    _StreamWriter.Close();
                    _StreamWriter.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string GetLogFileName()
        {
            string logpath = @"C:\VenturaLog\";
            string fileName = "";
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogPath"]))
                {
                    logpath = ConfigurationManager.AppSettings["LogPath"].ToString().Trim();
                }

                if (!Directory.Exists(logpath))
                {
                    Directory.CreateDirectory(logpath);
                }

                fileName = logpath + @"\" + DateTime.Now.Year + "" + DateTime.Now.Month + "" + DateTime.Now.Day + ".txt";

            }
            catch (Exception ex)
            {

            }
            return fileName;
        }
        //VR008 End

        //V2021
        protected void Application_PostAuthorizeRequest()
        {

            if (IsWebApiRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            }
        }

        private bool IsWebApiRequest()
        {
            return HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(WebApiConfig.UrlPrefixRelative);
        }

        //V2021
    }
}