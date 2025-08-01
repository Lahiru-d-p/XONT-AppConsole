using System.Collections.Generic;
using XONT.Common.Message;
using XONT.Ventura.AppConsole.DomainCore;
using XONT.Ventura.Common;
using XONT.VENTURA.V2MNT21;
using System.Data; //VR007

namespace XONT.Ventura.AppConsole.BLLCore
{
    public interface IUserDAO
    {
        //V2044adding start
        DataTable GetLisenceAltertInfo(string businessUnit, out MessageSet msg);
        bool UpdateLisenceAlertSentDate(string businessUnit, out MessageSet msg);
        //V2044adding end

        void RestorePagingToggle(string BusinessUnit, string TaskCode, string AllowPaging, out MessageSet msg);//V2023Added
        DataTable getReminderDetails(string businessUnit, string userName, string reminderId, ref MessageSet msg);
        DataTable getReminders(string businessUnit, string userName);
        DataTable getBUnit(string v, ref MessageSet _message);
        void SaveReminders(string businessUnit, string userName, string reminderName, string message, string timeString, ref MessageSet msg);
        DataTable getMostRecentReminder(string businessUnit, string userName, ref MessageSet msg);
        DataTable GetExpiredReminders(string businessUnit, string userName, ref MessageSet msg);
        void UpdateReminder(string businessUnit, string userName, string reminderName, string message, string timeString, string reminderNameOld, ref MessageSet msg);
        void DeleteReminders(string userName, List<string> reminders, ref MessageSet message);
        DataTable GetReminderJustExpired(string businessUnit, string userName, string reminder, ref MessageSet msg);
        byte[] getImageData(string userName, ref MessageSet message);//mgd
        void saveProfilePicture(string userName, byte[] image, ref MessageSet message);//mgd
        void DeleteBookmarks(string userName, string[] bookmarks, ref MessageSet message);//mgd
        DataTable getUserfavourites(string businessUnit, string userName, ref MessageSet message);//mgd
        User GetUserInfo(string userName, string password, ref MessageSet message);
        List<UserMenu> GetUserManu(string userName, string roleCode, ref MessageSet message);
        List<UserTask> GetUserTask(string menuCode, ref MessageSet message);
        List<UserTask> GetUserTask(string menuCode, string userName, ref MessageSet message);//VR019
        LicenseKey GetLicenseData(string userName, string businessUnit, ref MessageSet message);
        void saveFavourites(string businessUnit, string userName, string bookmarkId, string bookmarkName, string path, ref MessageSet message);//mgd
        void SaveUserLoginData(User userOb, ref MessageSet message);
        void SaveTimeOut(User userOb, ref MessageSet message);
        void UpdateLoginAttemptUser(User userOb, ref MessageSet message);
        void UpdateActiveFlagUser(User userOb, ref MessageSet message);
        bool CheckUser(string userName, string passWord, ref MessageSet message);
        void UpdateUserSetting(User userOb, ref MessageSet message);
        void ResetProfilePicture(User userob, ref MessageSet message);
        BusinessUnit GetBusinessUnit(string businessUnit, ref MessageSet message);
        List<MultimediaDetails> GetMultimediaList(string userName, string businessUnit, ref MessageSet message);
        //void UpdateMultimediaDate(string businessUnit, string userName, List<int> multRecID, ref MessageSet message);
        void GetActiveUsers(ref List<User> activeUsers, ref MessageSet message);
        void GetPassword(string businessUnit, ref Password password, ref MessageSet message);
        void GetPasswordHistoryList(string userName, string reusePeriod, ref List<string> passList, ref MessageSet message);
        void SaveChangePassword(string userName, string password, string passwordChange, ref MessageSet message); //V2004
        BusinessUnit GetBusinessUnit(string businessUnit, string distributorCode, ref MessageSet message);//VR002 
        void LogTaskInfo(ActiveUserTask userActiveTask, ref MessageSet message);//VR003
        DataTable GetAllUsers(ref MessageSet msg); //VR007
        bool CheckUserAvailable(string userName, ref MessageSet message);//VR011
        bool CheckNoOfAttemptsExceeded(string userName, ref MessageSet message);//VR011
        List<UserRole> GetUserRoles(string userName, ref MessageSet message);//VR016
        List<string> GetObjectLockSessionsList(string businessUnit, ref MessageSet message);//VR025
        void ReleaseObjectLocks(string businessUnit, List<string> sessionIdList, ref MessageSet message);//VR025
        string GetDefMenuCode(string userName, ref MessageSet message);// jan08

        string GetBusinessUnitName(string businessUnit, ref MessageSet message); //V2005

        //V2007
        DataTable getMostRecentAlert(ref MessageSet msg);
        void DeleteAlert(int AlertNumber,int RecID, ref MessageSet _message);

        void UpdateAlertCurrent(int AlertNumber, int RecID, string TimeSpan, ref MessageSet _message);

        //V2016
        void UpdateDailyMenu(string MenuCode, ref MessageSet message);
        bool CheckDailyMenu(string MenuCode, ref MessageSet message);

        //V2017
        int getNewNotification(string UserName, string BusinessUnit,int ExecutionType, ref MessageSet _message);

        DataTable getUserNotification(string UserName, string BusinessUnit, ref MessageSet _message);

        void UpdateNotification(string BusinessUnit, string UserName, int recId, int executionType, ref MessageSet msg);
        void DeleteNotifications(string userName, int[] notifications, ref MessageSet message);
        UserTask UserNotificationTask(string taskcode, string userName, ref MessageSet message);

        //V2018
        bool CheckUserNotificationTask(string taskCode, string businessUnit, ref MessageSet message);
        void UpdateUserNotification(string businessUnit, string userName, string taskCode, ref MessageSet msg);

        string LogActiveTaskInfo(ActiveUserTask userActiveTask, ref MessageSet msg);//V2049
        List<string> GetTaskLockSessionsList(string businessUnit, ref MessageSet message);//V2049
        bool UpdateActiveTask(ActiveUserTask userActiveTask, string executionType, ref MessageSet message);//V2049
        bool UpdateActiveTask(string businessUnit, List<string> lstSessionID, ref MessageSet message);//V2049
        List<UserTask> GetAuthorizedTaskURLs(string userName, ref MessageSet message); //V2053 Add
    }
}