using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Security;
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

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

        //protected void Application_AuthenticateRequest(object sender, EventArgs e) { } // V2053 remove

        // V2053 START
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext currentContext = HttpContext.Current;

            string requestedPath = currentContext.Request.AppRelativeCurrentExecutionFilePath;

            if (requestedPath.Equals("~/Main.aspx", StringComparison.OrdinalIgnoreCase) ||
                requestedPath.Equals("Main.aspx", StringComparison.OrdinalIgnoreCase)   ||
                requestedPath.Equals("/Main.aspx", StringComparison.OrdinalIgnoreCase)  )
            {
                return;
            }

            string rawFilePath = currentContext.Request.FilePath;

            // Allow access to non-ASPX static files (CSS, JS, images, etc.)
            string fileExtension = Path.GetExtension(rawFilePath);
            if (!string.IsNullOrEmpty(fileExtension) && !fileExtension.Equals(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            HttpCookie authCookie = currentContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                try
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (authTicket != null && !authTicket.Expired)
                    {
                        string userName = authTicket.Name;
                        string cacheKey = authTicket.UserData;

                        HashSet<string> allowedUrls = null;
                        if (!string.IsNullOrEmpty(cacheKey))
                        {
                            allowedUrls = currentContext.Cache[cacheKey] as HashSet<string>; // Cast to HashSet<string>
                        }

                        // If the list is not found in cache (e.g., cache eviction, bad key), or if the UserData was empty/invalid
                        if (allowedUrls == null)
                        {
                            // If the list isn't in cache, the user cannot be properly authorized. Force re-login.
                            System.Diagnostics.Trace.TraceWarning($"Global.asax AuthenticateRequest: User's allowed URLs not found in cache for key '{cacheKey}'. Forcing re-login for user: {userName ?? "N/A"}.");
                            FormsAuthentication.SignOut();
                            currentContext.Response.Redirect(FormsAuthentication.LoginUrl, true);
                            HttpContext.Current.ApplicationInstance.CompleteRequest();
                            return;
                        }

                        // Store the list in HttpContext.Items for access in AuthorizeRequest
                        currentContext.Items["AllowedComponentUrls"] = allowedUrls;
                        currentContext.Items["AuthenticatedUserName"] = userName;

                        FormsIdentity id = new FormsIdentity(authTicket);
                        currentContext.User = new System.Security.Principal.GenericPrincipal(id, null);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError($"Global.asax AuthenticateRequest Error: {ex.Message}\r\nStack Trace: {ex.StackTrace}");
                    FormsAuthentication.SignOut();
                    currentContext.Response.Redirect(FormsAuthentication.LoginUrl, true);
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // End request after redirect
                }
            }
        }

        protected void Application_AuthorizeRequest(object sender, EventArgs e)
        {
            HttpContext currentContext = HttpContext.Current;

            string requestedPath = currentContext.Request.AppRelativeCurrentExecutionFilePath;

            if (requestedPath.Equals("~/Main.aspx", StringComparison.OrdinalIgnoreCase) ||
                requestedPath.Equals("Main.aspx", StringComparison.OrdinalIgnoreCase) ||
                requestedPath.Equals("/Main.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string rawFilePath = currentContext.Request.FilePath;

            // Allow access to non-ASPX static files (CSS, JS, images, etc.)
            string fileExtension = Path.GetExtension(rawFilePath);
            if (!string.IsNullOrEmpty(fileExtension) && !fileExtension.Equals(".aspx", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string normalizedPath = NormalizeUrl(requestedPath);
            string normalizedLoginUrl = NormalizeUrl(FormsAuthentication.LoginUrl);

            // Allow access to Login and CustomError pages without authentication
            if (normalizedPath.Equals(normalizedLoginUrl, StringComparison.OrdinalIgnoreCase) ||
                normalizedPath.Equals("~/CustomError.aspx", StringComparison.OrdinalIgnoreCase) ||
                normalizedPath.Equals("~/Main.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Ensure the user is authenticated
            if (!currentContext.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            // Get username from HttpContext.Items (populated in AuthenticateRequest)
            string userName = currentContext.Items["AuthenticatedUserName"] as string;
            if (string.IsNullOrEmpty(userName))
            {
                // This state indicates a problem (user is authenticated but username lost or not set)
                System.Diagnostics.Trace.TraceWarning("Global.asax AuthorizeRequest: Authenticated user has no username in HttpContext.Items. Forcing re-login.");
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            // Retrieve HashSet<string> from HttpContext.Items (populated in AuthenticateRequest)
            HashSet<string> allowedComponentUrls = currentContext.Items["AllowedComponentUrls"] as HashSet<string>; // Cast to HashSet<string>

            // This check is still vital: if the list is null, access cannot be granted.
            if (allowedComponentUrls == null)
            {
                System.Diagnostics.Trace.TraceError("Global.asax AuthorizeRequest: AllowedComponentUrls not found in HttpContext.Items. This indicates a severe issue during Forms Auth ticket processing or cache lookup.");
                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            // Check if the authenticated user's requested URL is in the allowed list
            if (allowedComponentUrls.Contains(normalizedPath))
            {
                return; // Allowed by the authorized task list
            }

            // Deny Access if none of the above allow rules are met
            System.Diagnostics.Trace.TraceWarning($"Access Denied for user '{userName}' to URL: '{requestedPath}'. Not in allowed list.");
            currentContext.Response.Redirect("~/CustomError.aspx");
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private string NormalizeUrl(string url)
        {
            // Remove query string if present
            int queryStringIndex = url.IndexOf('?');
            if (queryStringIndex > -1)
            {
                url = url.Substring(0, queryStringIndex);
            }

            // Ensure it starts with a tilde (~/) for application-relative paths
            if (!url.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
            {
                if (url.StartsWith("/"))
                {
                    url = "~" + url;
                }
                else
                {
                    url = "~/" + url;
                }
            }

            // Critical: Handle trailing slashes consistently.
            string fileExtension = Path.GetExtension(url);

            if (!string.IsNullOrEmpty(fileExtension))
            {
                // If it's a file with an extension, remove any trailing slash
                url = url.TrimEnd('/');
            }
            else // It's a path without an extension (likely a directory or an Angular route)
            {
                // Ensure it has a trailing slash for consistency, unless it's just "~/" (root).
                if (!url.EndsWith("/", StringComparison.OrdinalIgnoreCase) && !url.Equals("~/", StringComparison.OrdinalIgnoreCase))
                {
                    url += "/";
                }
            }
            return url;
        }
        // V2053 END

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