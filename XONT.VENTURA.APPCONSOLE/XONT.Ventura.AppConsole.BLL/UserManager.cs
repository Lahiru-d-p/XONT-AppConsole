using System.Collections.Generic;
using System.Data;
using XONT.Common.Message;
using XONT.Ventura.AppConsole.Domain;
using XONT.Ventura.Common;
using XONT.Ventura.Common.GenerateLicense;
using XONT.Ventura.Common.Prompt;
using XONT.VENTURA.V2MNT21;

namespace XONT.Ventura.AppConsole
{
    public class UserManager : IUserDAO
    {
        #region Implementation of IUserDAO

        private readonly UserDAL userDal;

        public UserManager()
        {
            userDal = new UserDAL();
        }

        #region VR007
        public DataTable GetAllUsers(ref MessageSet msg)
        {
            DataTable _dTable = userDal.GetAllUsers(ref msg);
            return _dTable;
        }
        #endregion

        //V2044Adding start
        public DataTable GetLisenceAltertInfo(string businessUnit, out MessageSet msg)
        {
            return userDal.GetLisenceAltertInfo(businessUnit, out msg);
        }

        public bool UpdateLisenceAlertSentDate(string businessUnit, out MessageSet msg)
        {
            return userDal.UpdateLisenceAlertSentDate(businessUnit, out msg);
        }
        //V2044adding end


        //V2023Added
        public void RestorePagingToggle(string BusinessUnit, string TaskCode, string AllowPaging, out MessageSet msg)
        {
            userDal.RestorePagingToggle(BusinessUnit, TaskCode, AllowPaging, out msg);
        }

        public DataTable getReminderDetails(string businessUnit, string userName, string reminderId, ref MessageSet msg)
        {
            return userDal.getReminderDetails(businessUnit, userName, reminderId, ref msg);
        }
        public DataTable getReminders(string businessUnit, string userName)
        {
            return userDal.getReminders(businessUnit, userName);
        }
        public void SaveReminders(string businessUnit, string userName, string reminderName, string message, string timeString, ref MessageSet msg)
        {
            userDal.SaveReminders(businessUnit, userName, reminderName, message, timeString, ref msg);
        }
        public DataTable getMostRecentReminder(string businessUnit, string userName, ref MessageSet msg)
        {
            return userDal.getMostRecentReminder(businessUnit, userName, ref msg);
        }

        public DataTable GetExpiredReminders(string businessUnit, string userName, ref MessageSet msg)
        {
            return userDal.GetExpiredReminders(businessUnit, userName, ref msg);
        }

        public void UpdateReminder(string businessUnit, string userName, string reminderName, string message, string timeString, string reminderNameOld, ref MessageSet msg)
        {
            userDal.UpdateReminder(businessUnit, userName, reminderName, message, timeString, reminderNameOld, ref msg);
        }

        public void DeleteReminders(string userName, List<string> reminders, ref MessageSet message)
        {
            userDal.DeleteReminders(userName, reminders, ref message);
        }

        public DataTable GetReminderJustExpired(string businessUnit, string userName, string reminder, ref MessageSet msg)
        {
            return userDal.GetReminderJustExpired(businessUnit, userName, reminder, ref msg);
        }


        public User GetUserInfo(string userName, string password, ref MessageSet message)
        {
            User user = userDal.GetUserInfo(userName, password, ref message);
            return user;
        }
        //VR011
        public bool CheckUserAvailable(string userName, ref MessageSet message)
        {
            bool available = userDal.CheckUserAvailable(userName, ref message);
            return available;
        }
        public bool CheckNoOfAttemptsExceeded(string userName, ref MessageSet message)
        {
            bool exceeded = userDal.CheckNoOfAttemptsExceeded(userName, ref message);
            return exceeded;
        }
        //VR011

        public List<UserMenu> GetUserManu(string userName, string roleCode, ref MessageSet message)
        {
            List<UserMenu> userMenus = userDal.GetUserManu(userName, roleCode, ref message);
            return userMenus;
        }

        public List<UserTask> GetUserTask(string menuCode, ref MessageSet message)
        {
            List<UserTask> userTasks = userDal.GetUserTask(menuCode, ref message);
            return userTasks;
        }
        public List<UserTask> GetUserTask(string menuCode, string userName, ref MessageSet message)//VR019
        {
            List<UserTask> userTasks = userDal.GetUserTask(menuCode, userName, ref message);
            return userTasks;
        }

        public LicenseKey GetLicenseData(string userName, string businessUnit, ref MessageSet message)
        {
            LicenseKey licenseKey = null;
            var generateLicenseBll = new GenerateLicenseBLL();
            string licenseInf = userDal.GetLicenseInfo(userName, businessUnit, ref message);
            licenseKey = generateLicenseBll.DecripLicenseKey(licenseInf.Trim());

            string startFrom = userDal.GetExpiryAlertData(userName, businessUnit, ref message);
            licenseKey.ExpiryAlertStartFrom = int.Parse(startFrom.Trim());

            return licenseKey;
        }

        public void SaveUserLoginData(User userOb, ref MessageSet message)
        {
            userDal.SaveUserLoginData(userOb, ref message);
        }
        public void SaveTimeOut(User userOb, ref MessageSet message)
        {
            userDal.SaveTimeOut(userOb, ref message);
        }
        public void UpdateLoginAttemptUser(User userOb, ref MessageSet message)
        {
            userDal.UpdateLoginAttemptUser(userOb, ref message);
        }
        public void UpdateActiveFlagUser(User userOb, ref MessageSet message)
        {
            userDal.UpdateActiveFlagUser(userOb, ref message);
        }

        //public void UpdateUser(User newUser)
        //{
        //    var setSPParameter = new Common();
        //    var spParametersList = new List<SPParameter>();

        //    setSPParameter.SetSPParameterList(spParametersList, "UserName", newUser.UserName, "");
        //    setSPParameter.SetSPParameterList(spParametersList, "UserFullName", newUser.UserFullName, "");
        //    setSPParameter.SetSPParameterList(spParametersList, "TimeStamp", newUser.TimeStamp, "");
        //    //setSPParameter.SetSPParameterList(spParametersList, "@FlagType", "UP", "");
        //    //setSPParameter.SetSPParameterList(spParametersList, "@FlagUpType", "UP", "");
        //    userDal.UpdateUser(spParametersList, newUser);
        //}


        public bool CheckUser(string userName, string passWord, ref MessageSet message)
        {
            bool isExists = userDal.CheckUser(userName, passWord, ref message);

            return isExists;
        }

        public void UpdateUserSetting(User userOb, ref MessageSet message)
        {
            userDal.UpdateUserSetting(userOb, ref message);

        }

        public void ResetProfilePicture(User userOb, ref MessageSet message)
        {
            userDal.ResetProfilePicture(userOb, ref message);
        }

        public BusinessUnit GetBusinessUnit(string businessUnit, ref MessageSet message)
        {
            var objBusinessUnit = new BusinessUnit();
            objBusinessUnit = userDal.GetBusinessUnit(businessUnit, ref message);
            return objBusinessUnit;
        }

        //VR002 Begin (If Dist Code exists then Replace BU Infor with Distributor name and address)
        public BusinessUnit GetBusinessUnit(string businessUnit, string distributorCode, ref MessageSet message)
        {
            var objBusinessUnit = new BusinessUnit();
            if (!string.IsNullOrEmpty(distributorCode))
            {
                objBusinessUnit = userDal.GetBusinessUnit(businessUnit, distributorCode, ref message);
            }
            else
            {
                objBusinessUnit = userDal.GetBusinessUnit(businessUnit, ref message);
            }
            return objBusinessUnit;
        }
        //VR002 End

        public List<MultimediaDetails> GetMultimediaList(string userName, string businessUnit, ref MessageSet message)
        {
            IMasterGroupManager masterGroupManager = new MasterGroupManager();
            DataTable dtUserMasterValues = masterGroupManager.GetGroupTypeValues(businessUnit, userName, "00", out message);
            List<MultimediaDetails> multimedia = userDal.GetMultimedia(userName, businessUnit, dtUserMasterValues, ref message);
            return multimedia;
        }

        //public void UpdateMultimediaDate(string businessUnit, string userName, List<int> multRecID, ref MessageSet message)
        //{
        //    userDal.UpdateMultimediaDate(businessUnit, userName, multRecID, ref message);
        //}
        public void GetActiveUsers(ref List<User> activeUsers, ref MessageSet message)
        {
            userDal.GetActiveUsers(ref activeUsers, ref message);
        }

        public void GetPassword(string businessUnit, ref Password password, ref MessageSet message)
        {
            userDal.GetPassword(businessUnit, ref password, ref message)
            ;
        }
        public void GetPasswordHistoryList(string userName, string reusePeriod, ref List<string> passList, ref MessageSet message)
        {
            userDal.GetPasswordHistoryList(userName, reusePeriod, ref passList, ref message)
            ;
        }
        public void SaveChangePassword(string userName, string password, string passwordChange, ref MessageSet message) //V2004
        {
            userDal.SaveChangePassword(userName, password, passwordChange, ref message);

        }

        //VR003 Begin
        public void LogTaskInfo(ActiveUserTask userActiveTask, ref MessageSet message)
        {
            userDal.LogTaskInfo(userActiveTask, ref message);
        }
        //VR003 End

        #endregion

        //V2049 S
      
        public string LogActiveTaskInfo(ActiveUserTask userActiveTask, ref MessageSet msg)
        {
            string error = "";

            int activeTaskCount = 0;
            int currentSessionCount = 0;
            int currentUserCount = 0;

            DataTable dtActiveTasks = new DataTable();

            if (userActiveTask.ExclusivityMode == "1")
            {
                dtActiveTasks = userDal.GetActiveTasks(userActiveTask.BusinessUnit, userActiveTask.UserName, userActiveTask.TaskCode, "", userActiveTask.SessionID, ref msg);
                if (msg != null)
                    return error;

                if (dtActiveTasks != null && dtActiveTasks.Rows.Count > 0)
                {
                    activeTaskCount = int.Parse(dtActiveTasks.Rows[0]["ActiveTaskCount"].ToString());
                    currentSessionCount = int.Parse(dtActiveTasks.Rows[0]["CurrentSessionCount"].ToString());

                    if (currentSessionCount == 0)
                    {
                        if (activeTaskCount > 0)
                        {
                            error = "Task is already opened by another user.";
                            return error;
                        }
                        else
                        {
                            if (!userDal.UpdateActiveTask(userActiveTask, "1", ref msg))
                            {
                                return error;
                            }
                        }
                    }
                }
            }
            else if (userActiveTask.ExclusivityMode == "2")
            {
                DataTable dtUserTetty = new DataTable();

                if (userActiveTask.PowerUser == "0")
                {
                    dtUserTetty = userDal.GetUserTerritory(userActiveTask.BusinessUnit, userActiveTask.UserName, ref msg);
                    if (msg != null)
                        return error;
                }

                if (userActiveTask.PowerUser == "1" ||
                    (userActiveTask.PowerUser == "0" && (dtUserTetty != null && dtUserTetty.Rows.Count > 1)))
                {
                    dtActiveTasks = userDal.GetActiveTasks(userActiveTask.BusinessUnit, userActiveTask.UserName, userActiveTask.TaskCode, "", userActiveTask.SessionID, ref msg);
                    if (msg != null)
                        return error;

                    if (dtActiveTasks != null && dtActiveTasks.Rows.Count > 0)
                    {
                        activeTaskCount = int.Parse(dtActiveTasks.Rows[0]["ActiveTaskCount"].ToString());
                        currentSessionCount = int.Parse(dtActiveTasks.Rows[0]["CurrentSessionCount"].ToString());

                        if (currentSessionCount == 0)
                        {
                            if (activeTaskCount > 0)
                            {
                                error = "Task is already opened by another user.";
                                return error;
                            }
                            else
                            {
                                if (!userDal.UpdateActiveTask(userActiveTask, "1", ref msg))
                                {
                                    return error;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (dtUserTetty != null && dtUserTetty.Rows.Count > 0)
                    {
                        string userTetty = dtUserTetty.Rows[0]["TerritoryCode"].ToString().Trim();

                        dtActiveTasks = userDal.GetActiveTasks(userActiveTask.BusinessUnit, userActiveTask.UserName, userActiveTask.TaskCode, userTetty, userActiveTask.SessionID, ref msg);
                        if (msg != null)
                            return error;

                        if (dtActiveTasks != null && dtActiveTasks.Rows.Count > 0)
                        {
                            activeTaskCount = int.Parse(dtActiveTasks.Rows[0]["ActiveTaskCount"].ToString());
                            currentSessionCount = int.Parse(dtActiveTasks.Rows[0]["CurrentSessionCount"].ToString());

                            if (currentSessionCount == 0)
                            {
                                if (activeTaskCount > 0)
                                {
                                    error = "Task is already opened by another user.";
                                    return error;
                                }
                                else
                                {
                                    userActiveTask.TerritoryCode = userTetty;
                                    if (!userDal.UpdateActiveTask(userActiveTask, "1", ref msg))
                                    {
                                        return error;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            return error;
        }

        public List<string> GetTaskLockSessionsList(string businessUnit, ref MessageSet message)
        {
            return userDal.GetTaskLockSessionsList(businessUnit, ref message);
        }

        public bool UpdateActiveTask(ActiveUserTask userActiveTask, string executionType, ref MessageSet message)
        {
            return userDal.UpdateActiveTask(userActiveTask, executionType, ref message);
        }

        public bool UpdateActiveTask(string businessUnit, List<string> lstSessionID, ref MessageSet message)
        {
            ActiveUserTask activeUserTask;
            foreach (var sessionID in lstSessionID)
            {
                message = null;

                activeUserTask = new ActiveUserTask();
                message = null;
                activeUserTask.SessionID = sessionID;
                activeUserTask.TaskCode = "";
                activeUserTask.BusinessUnit = businessUnit;

                userDal.UpdateActiveTask(activeUserTask, "2", ref message);
                if (message != null)
                {
                    return false;
                }
            }
            return true;
        }
      
        //V2049 E

        public byte[] getImageData(string userName, ref MessageSet message)
        {
            return userDal.getImageData(userName, ref message);
        }
        public void saveProfilePicture(string userName, byte[] image, ref MessageSet message)
        {
            userDal.saveProfilePicture(userName, image, ref message);
        }
        public void DeleteBookmarks(string userName, string[] bookmarks, ref MessageSet message)
        {
            userDal.DeleteBookmarks(userName, bookmarks, ref message);
        }
        public void saveFavourites(string businessUnit, string userName, string bookmarkId, string bookmarkName, string path, ref MessageSet message)
        {

            userDal.saveFavourites(businessUnit, userName, bookmarkId, bookmarkName, path, ref message);
        }

        public List<UserRole> GetUserRoles(string userName, ref MessageSet message)//VR016
        {
            return userDal.GetUserRole(userName, ref message);
        }

        //VR025
        public List<string> GetObjectLockSessionsList(string businessUnit, ref MessageSet message)
        {
            return userDal.GetObjectLockSessionsList(businessUnit, ref message);
        }

        public void ReleaseObjectLocks(string businessUnit, List<string> sessionIdList, ref MessageSet message)
        {
            userDal.ReleaseObjectLocks(businessUnit, sessionIdList, ref message);
        }
        //VR025

        public string GetDefMenuCode(string userName, ref MessageSet message)  //jan08
        {
            string menuCode;
            menuCode = userDal.GetDefMenuCode(userName, ref message);
            return menuCode;
        }



        public DataTable getUserfavourites(string businessUnit, string userName, ref MessageSet message)
        {
            return userDal.getUserfavourites(businessUnit, userName, ref message);
        }

        public DataTable getBUnit(string userName, ref MessageSet message)
        {
            return userDal.getBUnit(userName, ref message);
        }

        public string GetBusinessUnitName(string businessUnit, ref MessageSet message) //V2005
        {
            string businessUnitName;
            businessUnitName = userDal.GetBusinessUnitName(businessUnit, ref message);
            return businessUnitName;
        }

        //V2007
        public DataTable getMostRecentAlert(ref MessageSet msg)
        {
            return userDal.getMostRecentAlert(ref msg);
        }
        public void DeleteAlert(int AlertNumber, int RecID, ref MessageSet _message)
        {
            userDal.DeleteAlert(AlertNumber, RecID, ref _message);
        }

        public void UpdateAlertCurrent(int AlertNumber, int RecID, string TimeSpan, ref MessageSet _message)
        {
            userDal.UpdateAlertCurrent(AlertNumber, RecID, TimeSpan, ref _message);
        }

        //V2016
        public void UpdateDailyMenu(string MenuCode, ref MessageSet message)
        {
            userDal.UpdateDailyMenu(MenuCode, ref message);
        }

        public bool CheckDailyMenu(string MenuCode, ref MessageSet message)
        {
            bool isExists = userDal.CheckDailyMenu(MenuCode, ref message);

            return isExists;
        }

        //V2017
        public int getNewNotification(string UserName, string BusinessUnit, int ExecutionType, ref MessageSet _message)
        {
            int unreadNotification = 0;
            unreadNotification = userDal.getNewNotification(UserName, BusinessUnit, ExecutionType, ref _message);
            return unreadNotification;
        }

        public DataTable getUserNotification(string UserName, string BusinessUnit, ref MessageSet _message)
        {
            return userDal.getUserNotification(UserName, BusinessUnit, ref _message);

        }

        public void UpdateNotification(string BusinessUnit, string UserName, int recId, int executionType, ref MessageSet msg)
        {
            userDal.UpdateNotification(BusinessUnit, UserName, recId, executionType, ref msg);
        }

        public void DeleteNotifications(string userName, int[] notifications, ref MessageSet message)
        {
            userDal.DeleteNotifications(userName, notifications, ref message);
        }

        public UserTask UserNotificationTask(string taskCode, string userName, ref MessageSet message)
        {
            UserTask userTasks = userDal.UserNotificationTask(taskCode, userName, ref message);
            return userTasks;
        }

        //V2018

        public bool CheckUserNotificationTask(string taskCode, string businessUnit, ref MessageSet message)
        {
            bool isExists = userDal.CheckUserNotificationTask(taskCode, businessUnit, ref message);

            return isExists;
        }

        public void UpdateUserNotification(string businessUnit, string userName, string taskCode, ref MessageSet msg)
        {
            userDal.UpdateUserNotification(businessUnit, userName, taskCode, ref msg);
        }

        //V2053 Add Start
        public List<UserTask> GetAuthorizedTaskURLs(string userName, ref MessageSet message)
        {
            var dt = new DataTable();
            
            dt = userDal.GetAuthorizedTaskURLs(userName, ref message);

            var taskList = new List<UserTask>();

            foreach (DataRow dtRow in dt.Rows)
            {
                var task = new UserTask();
                task = (new UserTask
                {
                    TaskCode = !string.IsNullOrEmpty(dtRow["TaskCode"].ToString()) ? dtRow["TaskCode"].ToString() : "",
                    UserName = userName.Trim(),
                    url = dtRow["url"].ToString().Trim()
                });
                taskList.Add(task);
            }

            return taskList;
        }
        //V2053 Add End
    }
}