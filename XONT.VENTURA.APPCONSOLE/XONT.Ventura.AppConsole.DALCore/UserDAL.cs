using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Transactions;
using XONT.Common.Data;
using XONT.Common.Message;
using XONT.Ventura.AppConsole.DALCore;
using XONT.Ventura.AppConsole.DomainCore;
using XONT.VENTURA.V2MNT21;

namespace XONT.Ventura.AppConsole.DALCore
{
    public class UserDAL
    {
        private readonly DBHelper _dbHelper;
        private readonly User user;
        private readonly string _userDbConnectionString;
        private readonly string _systemDbConnectionString;
        private readonly IConfiguration _configuration;

        #region Public Methods

        #region Common Calling Functions

        public UserDAL(DBHelper dbHelper,IConfiguration configuration)
        {
            _configuration = configuration;
            _userDbConnectionString = _configuration.GetConnectionString("UserDB");
            _systemDbConnectionString = _configuration.GetConnectionString("SystemDB");
            _dbHelper = dbHelper;
            user = new User();
        }

        //V2005

        //V2044Adding start       

        public DataTable GetLisenceAltertInfo(string businessUnit, out MessageSet msg)
        {
            DataTable dtBU = new DataTable();
            msg = null;

            try
            {
                string commandText = @"
            SELECT LicenseAlertMailIDs, LastLicenseAlertSentDate 
            FROM dbo.ZYBusinessUnit
            WHERE BusinessUnit = @BusinessUnit";

                SqlParameter param = new SqlParameter("@BusinessUnit", SqlDbType.NVarChar)
                {
                    Value = businessUnit.Trim()
                };

                dtBU = _dbHelper.ExecuteQuery(_systemDbConnectionString, commandText, new[] { param });
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetLisenceAltertInfo", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }

            return dtBU;
        }

        public bool UpdateLisenceAlertSentDate(string businessUnit, out MessageSet msg)
        {
            bool isOK = false;
            msg = null;

            try
            {
                string commandText = @"
            UPDATE dbo.ZYBusinessUnit
            SET LastLicenseAlertSentDate = @TodayDate
            WHERE BusinessUnit = @BusinessUnit";

                var parameters = new[]
                {
            new SqlParameter("@TodayDate", SqlDbType.Char) { Value = DateTime.Today.ToString("yyyyMMdd") },
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar) { Value = businessUnit.Trim() }
        };

                int rowsAffected = _dbHelper.ExecuteNonQuery(_systemDbConnectionString, commandText, parameters);
                isOK = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "UpdateLisenceAlertSentDate", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }

            return isOK;
        }


        public string GetBusinessUnitName(string businessUnit, ref MessageSet msg)
        {
            string businessUnitName = string.Empty;
            DataTable bunit = new DataTable();

            try
            {
                string commandText = @"
            SELECT DISTINCT BusinessUnitName 
            FROM ZYBusinessUnit 
            INNER JOIN ZYUserBusUnit ON ZYBusinessUnit.BusinessUnit = ZYUserBusUnit.BusinessUnit 
            WHERE ZYUserBusUnit.BusinessUnit = @BusinessUnit";

                SqlParameter param = new SqlParameter("@BusinessUnit", SqlDbType.NVarChar)
                {
                    Value = businessUnit.Trim()
                };

                bunit = _dbHelper.ExecuteQuery(_systemDbConnectionString, commandText, new[] { param });

                if (bunit.Rows.Count > 0)
                {
                    businessUnitName = bunit.Rows[0]["BusinessUnitName"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetBusinessUnitName", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }

            return businessUnitName;
        }
        //End

        #region VR007
        public DataTable GetAllUsers(ref MessageSet msg)
        {
            DataTable dataTable = new DataTable("All_Users");
            msg = null;

            try
            {
                string commandText = @"
            SELECT *  
            FROM dbo.ZYUser  
            WHERE UserLevelGroup <> 'OWNER'";

                dataTable = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters: null 
                );
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetAllUsers",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return dataTable;
        }
        #endregion

        public User GetUserInfo(string userName, string password, ref MessageSet message)
        {
            GetUserMainData(userName, password, ref message);
            //VR003 Begin
            if (user == null)
            {
                return null;
            }
            //VR003 End

            if (user.AlreadyLogin != null)
            {
                if (user.isExists && (user.AlreadyLogin.Equals(string.Empty)))
                {
                    GetUserRoles(userName, ref message);
                }
            }
            else
            { return user; }

            return user;
        }

        //VR011 add start

        public bool CheckUserAvailable(string userName, ref MessageSet message)
        {
            bool available = false;

            try
            {
                string commandText = @"
            SELECT UserName 
            FROM ZYUser  
            WHERE UserName = @UserName
              AND UserLevelGroup <> 'OWNER'";

                var parameter = new SqlParameter("@UserName", SqlDbType.NVarChar)
                {
                    Value = userName.Trim()
                };

                DataTable resultTable = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                available = resultTable.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "CheckUserAvailable",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return available;
        }

        public bool CheckNoOfAttemptsExceeded(string userName, ref MessageSet message)
        {
            bool isExceeded = false;
            int exceedAttempts = -1;

            try
            {
                string spName = "usp_CheckNoOfAttemptsExceeded";

                var parameters = new[]
                {
            new SqlParameter("@UserName", SqlDbType.NVarChar)
            {
                Value = userName.Trim()
            }
        };

                DataTable dtResults = _dbHelper.ExecuteStoredProcedure(
                    _systemDbConnectionString,
                    spName,
                    parameters
                );

                if (dtResults != null && dtResults.Rows.Count > 0)
                {
                    exceedAttempts = dtResults.Rows[0].Field<int>("Attempt");
                    isExceeded = exceedAttempts >= 0;
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "CheckNoOfAttemptsExceeded",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return isExceeded;
        }//VR011 add end

        public string GetLicenseInfo(string userName, string businessUnit, ref MessageSet message)
        {
            string licenseKey = GetLicenseKey(userName, businessUnit, ref message);
            return licenseKey;
        }
        public string GetExpiryAlertData(string userName, string businessUnit, ref MessageSet message)//VR015
        {
            string startFrom = GetExpiryAlertStartFrom(userName, businessUnit, ref message);
            return startFrom;
        }

        public BusinessUnit GetBusinessUnit(string businessUnit, ref MessageSet message)
        {
            var objBusinessUnit = new BusinessUnit();
            GetUserBusinessUnit(businessUnit, ref objBusinessUnit, ref message);
            return objBusinessUnit;
        }

        //VR002 Begin (Replace BU Infor with Distributor name and address
        public BusinessUnit GetBusinessUnit(string businessUnit, string distributorCode, ref MessageSet message)
        {
            var objBusinessUnit = new BusinessUnit();
            GetUserBusinessUnit(businessUnit, ref objBusinessUnit, distributorCode, ref message);
            return objBusinessUnit;
        }//VR002 End

        public void SaveUserLoginData(User userOb, ref MessageSet message)
        {
            SaveLoginData(userOb, ref message);
        }

        public void SaveTimeOut(User userOb, ref MessageSet message)
        {
            UpdateLoginData(userOb, ref message);
        }

        public void UpdateLoginAttemptUser(User userOb, ref MessageSet message)
        {
            UpdateLoginAttempt(userOb, ref message)
                ;
        }

        public void UpdateActiveFlagUser(User userOb, ref MessageSet message)
        {
            UpdateActiveFlag(userOb, ref message)
                ;
        }

        public bool CheckUser(string userName, string passWord, ref MessageSet message)
        {
            bool isExists = CheckUserData(userName, passWord, ref message);

            return isExists;
        }

        public void GetPassword(string businessUnit, ref Password password, ref MessageSet message)
        {
            GetPasswordData(businessUnit, ref message)
                ;
            password = user.PasswordData;
        }

        public void GetPasswordHistoryList(string userName, string reusePeriod, ref List<string> passList,
                                           ref MessageSet message)
        {
            GetPasswordHistoryListData(userName, reusePeriod, ref passList, ref message)
                ;
        }

        public void SaveChangePassword(string userName, string password, string passwordChange, ref MessageSet message) //V2004
        {
            SaveChangePasswordData(userName, password, passwordChange, ref message)
                ;
        }

        public void UpdateUserSetting(User userOb, ref MessageSet message)
        {
            UpdateUserSettingData(userOb, ref message);
        }

        public void ResetProfilePicture(User userOb, ref MessageSet message)
        {
            ResetProfilePictureData(userOb, ref message);
        }

        //public void UpdateMultimediaDate(string businessUnit, string userName, List<int> multRecID,
        //                                 ref MessageSet message)
        //{

        //    UpdateMultimedia(businessUnit, userName, territoryCode, multRecID,targetCategory, ref message);
        //}

        public List<MultimediaDetails> GetMultimedia(string userName, string businessUnit, DataTable dtMasterGroupVal,
                                              ref MessageSet msg)
        {
            List<MultimediaDetails> multimedia = GetUserMultimedia(userName, businessUnit, dtMasterGroupVal, ref msg)
                ;
            return multimedia;
        }

        public void GetActiveUsers(ref List<User> activeUsers, ref MessageSet message)
        {
            GetActiveUsersData(ref activeUsers, ref message);
        }

        //VR025
        public List<string> GetObjectLockSessionsList(string businessUnit, ref MessageSet message)
        {
            List<string> sessionList = new List<string>();

            try
            {
                string commandText = @"
            SELECT ISNULL(SessionID, '') AS SessionID
            FROM ZYObjectLock
            WHERE BusinessUnit = @BusinessUnit AND StatusFlag = '1'";

                var parameter = new SqlParameter("@BusinessUnit", SqlDbType.NVarChar)
                {
                    Value = businessUnit.Trim()
                };

                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                foreach (DataRow row in dt.Rows)
                {
                    sessionList.Add(row["SessionID"].ToString());
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetObjectLockSessionsList",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return sessionList;
        }
        //VR025

        //V2023Start
        public void RestorePagingToggle(string businessUnit, string taskCode, string allowPaging, out MessageSet msg)
        {
            msg = null;

            try
            {
                string commandText = @"
            UPDATE RD.ComponentMasterControl
            SET AllowPaging = @AllowPaging
            WHERE BusinessUnit = @BusinessUnit AND ComponentID = @TaskCode";

                var parameters = new[]
                {
            new SqlParameter("@AllowPaging", SqlDbType.Char, 1) { Value = allowPaging },
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50) { Value = businessUnit.Trim() },
            new SqlParameter("@TaskCode", SqlDbType.NVarChar, 50) { Value = taskCode.Trim() }
        };

                _dbHelper.ExecuteNonQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "RestorePagingToggle",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        //V2023End
        #region Get User Menu/Task
        public List<UserMenu> GetUserMenu(string userName, string roleCode, ref MessageSet message)
        {
            var userMenus = new List<UserMenu>();

            try
            {
                string commandText = @"
            SELECT ZYRoleMenu.MenuCode, 
                   ZYMenuHeader.Description, 
                   ZYMenuHeader.Icon
            FROM ZYRoleMenu
            INNER JOIN ZYUserRole ON ZYRoleMenu.RoleCode = ZYUserRole.RoleCode
            INNER JOIN ZYMenuHeader ON ZYRoleMenu.MenuCode = ZYMenuHeader.MenuCode
            WHERE ZYUserRole.UserName = @UserName
              AND ZYRoleMenu.RoleCode = @RoleCode
            ORDER BY ZYRoleMenu.Sequence";

                var parameters = new[]
                {
            new SqlParameter("@UserName", SqlDbType.NVarChar) { Value = userName.Trim() },
            new SqlParameter("@RoleCode", SqlDbType.NVarChar) { Value = roleCode.Trim() }
        };

                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );

                foreach (DataRow row in dt.Rows)
                {
                    userMenus.Add(new UserMenu
                    {
                        MenuCode = row["MenuCode"]?.ToString() ?? "",
                        Description = row["Description"]?.ToString() ?? "",
                        Icon = row["Icon"]?.ToString()?.Trim() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserMenu",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return userMenus;
        }
        public List<UserTask> GetUserTask(string menuCode, ref MessageSet message)
        {
            var userTasks = new List<UserTask>();

            try
            {
                string commandText = @"
            SELECT ZYTask.TaskCode, 
                   ZYTask.Caption, 
                   ZYTask.Description, 
                   ZYTask.ExecutionScript, 
                   ZYTask.Icon
            FROM ZYTask
            INNER JOIN ZYMenuDetail ON ZYTask.TaskCode = ZYMenuDetail.TaskCode
            WHERE ZYMenuDetail.MenuCode = @MenuCode
            ORDER BY ZYMenuDetail.Sequence";

                var parameter = new SqlParameter("@MenuCode", SqlDbType.NVarChar)
                {
                    Value = menuCode.Trim()
                };

                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                var dllsInSiteBin = AppDomain.CurrentDomain.GetAssemblies().ToList();

                foreach (DataRow row in dt.Rows)
                {
                    string taskCode = row["TaskCode"]?.ToString().Trim() ?? "";
                    string executionScript = row["ExecutionScript"]?.ToString() ?? "";
                    bool isV2Component = executionScript.Contains(".aspx");
                    string version = "0.0.0.0";

                    if (!isV2Component)
                    {
                        var assemblyVersion = dllsInSiteBin.FindAll(a => a.FullName.Contains(taskCode));
                        if (assemblyVersion.Count > 0)
                        {
                            version = assemblyVersion[0].GetName().Version.ToString();
                            executionScript += "?v=" + version;
                        }
                    }

                    userTasks.Add(new UserTask
                    {
                        TaskCode = taskCode,
                        Caption = row["Caption"]?.ToString() ?? "",
                        Description = row["Description"]?.ToString() ?? "",
                        ExecutionScript = executionScript,
                        Icon = row["Icon"]?.ToString()?.Trim() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserTask",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return userTasks;
        }

        public List<UserTask> GetUserTask(string menuCode, string userName, ref MessageSet message)
        {
            var userTasks = new List<UserTask>();

            try
            {
                string commandText = @"
            SELECT 
                ZYTask.TaskCode,
                ZYTask.Caption,
                ZYTask.Description,
                ZYTask.ExecutionScript,
                ZYTask.Icon,
                ISNULL(ZYTask.TaskType, '') AS TaskType,
                ISNULL(ZYTask.ExclusivityMode, '') AS ExclusivityMode,
                ISNULL(ZYTask.ApplicationCode, '') AS ApplicationCode
            FROM ZYTask
            INNER JOIN ZYMenuDetail ON ZYTask.TaskCode = ZYMenuDetail.TaskCode
            WHERE ZYMenuDetail.MenuCode = @MenuCode
            ORDER BY ZYMenuDetail.Sequence";

                var parameter = new SqlParameter("@MenuCode", SqlDbType.NVarChar)
                {
                    Value = menuCode.Trim()
                };

                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                var dllsInSiteBin = AppDomain.CurrentDomain.GetAssemblies().ToList();

                foreach (DataRow row in dt.Rows)
                {
                    string taskCode = row["TaskCode"]?.ToString().Trim() ?? "";
                    string executionScript = row["ExecutionScript"]?.ToString() ?? "";
                    bool isV2Component = executionScript.Contains(".aspx");
                    string version = "0.0.0.0";

                    if (!isV2Component)
                    {
                        var assemblyVersion = dllsInSiteBin.FindAll(a => a.FullName.Contains(taskCode));
                        if (assemblyVersion.Count > 0)
                        {
                            version = assemblyVersion[0].GetName().Version.ToString();
                            executionScript += "?v=" + version;
                        }
                    }

                    userTasks.Add(new UserTask
                    {
                        TaskCode = taskCode,
                        Caption = row["Caption"]?.ToString() ?? "",
                        Description = row["Description"]?.ToString() ?? "",
                        ExecutionScript = executionScript,
                        Icon = row["Icon"]?.ToString()?.Trim() ?? "",
                        TaskType = row["TaskType"]?.ToString() ?? "",
                        UserName = userName.Trim(),
                        ExclusivityMode = row["ExclusivityMode"]?.ToString()?.Trim() ?? "",
                        ApplicationCode = row["ApplicationCode"]?.ToString()?.Trim() ?? ""
                    });
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserTask",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return userTasks;
        }
        #endregion


        //VR003 Begin
        
        public void LogTaskInfo(ActiveUserTask userActiveTask, ref MessageSet message)
        {
            try
            {
                string spName = "usp_UpdateCurrentUserTask";

                var parameters = new[]
                {
            new SqlParameter("@SessionID", SqlDbType.NVarChar, 50)
            {
                Value = (object)userActiveTask.SessionID ?? DBNull.Value
            },
            new SqlParameter("@TaskCode", SqlDbType.NVarChar, 50)
            {
                Value = (object)userActiveTask.TaskCode ?? DBNull.Value
            },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = (object)userActiveTask.UserName ?? DBNull.Value
            },
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
            {
                Value = (object)userActiveTask.BusinessUnit ?? DBNull.Value
            },
            new SqlParameter("@EndDateTime", SqlDbType.DateTime)
            {
                Value = userActiveTask.EndDateTime.HasValue
                    ? (object)userActiveTask.EndDateTime.Value
                    : DBNull.Value
            },
            new SqlParameter("@Status", SqlDbType.NVarChar, 50)
            {
                Value = (object)userActiveTask.Status ?? DBNull.Value
            },
            new SqlParameter("@ExecutionType", SqlDbType.NVarChar, 50)
            {
                Value = (object)userActiveTask.ExecutionType ?? DBNull.Value
            }
        };

                // Execute stored procedure
                _dbHelper.ExecuteNonQuery(
                    _systemDbConnectionString,
                    spName,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "LogTaskInfo",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        //VR003 End

        //V2049 S
        public int GetActiveTaskCount(
            string businessUnit,
            string taskCode,
            string territoryCode,
            string sessionID,
            ref MessageSet message)
        {
            int activeTaskCount = 0;

            try
            {
                string spName = "[dbo].[usp_GetUserData]";

                var parameters = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
                { Value = (object)businessUnit ?? DBNull.Value },
            new SqlParameter("@TaskCode", SqlDbType.NVarChar, 50)
                { Value = (object)taskCode ?? DBNull.Value },
            new SqlParameter("@TerritoryCode", SqlDbType.NVarChar, 50)
                { Value = (object)territoryCode ?? DBNull.Value },
            new SqlParameter("@SessionID", SqlDbType.NVarChar, 50)
                { Value = (object)sessionID ?? DBNull.Value },
            new SqlParameter("@ExecutionType", SqlDbType.NVarChar, 50)
                { Value = "5" }
        };

                DataTable dtResult = _dbHelper.ExecuteStoredProcedure(
                    _systemDbConnectionString,
                    spName,
                    parameters
                );

                if (dtResult != null && dtResult.Rows.Count > 0 && dtResult.Columns.Contains("ActiveTaskCount"))
                {
                    activeTaskCount = Convert.ToInt32(dtResult.Rows[0]["ActiveTaskCount"]);
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetActiveTaskCount",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return activeTaskCount;
        }
        public DataTable GetActiveTasks(
    string businessUnit,
    string userName,
    string taskCode,
    string territoryCode,
    string sessionID,
    ref MessageSet message)
        {
            try
            {
                string spName = "[dbo].[usp_GetUserData]";

                var parameters = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
                { Value = (object)businessUnit ?? DBNull.Value },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                { Value = (object)userName ?? DBNull.Value },
            new SqlParameter("@TaskCode", SqlDbType.NVarChar, 50)
                { Value = (object)taskCode ?? DBNull.Value },
            new SqlParameter("@TerritoryCode", SqlDbType.NVarChar, 50)
                { Value = (object)territoryCode ?? DBNull.Value },
            new SqlParameter("@SessionID", SqlDbType.NVarChar, 50)
                { Value = (object)sessionID ?? DBNull.Value },
            new SqlParameter("@ExecutionType", SqlDbType.NVarChar, 50)
                { Value = "6" }
        };

                return _dbHelper.ExecuteStoredProcedure(
                    _systemDbConnectionString,
                    spName,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetActiveTasks",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
                return new DataTable();
            }
        }
        public DataTable GetUserTerritory(
    string businessUnit,
    string userName,
    ref MessageSet message)
        {
            try
            {
                string spName = "[RD].[usp_AppConsoleGetData]";

                var parameters = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
                { Value = (object)businessUnit ?? DBNull.Value },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                { Value = (object)userName ?? DBNull.Value },
            new SqlParameter("@ExecutionType", SqlDbType.NVarChar, 50)
                { Value = "1" }
        };

                return _dbHelper.ExecuteStoredProcedure(
                    _userDbConnectionString,
                    spName,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserTerritory",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
                return new DataTable();
            }
        }
        public List<string> GetTaskLockSessionsList(string businessUnit, ref MessageSet message)
        {
            List<string> sessionList = new List<string>();

            try
            {
                string spName = "[dbo].[usp_GetUserData]";

                var parameters = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
            {
                Value = (object)businessUnit ?? DBNull.Value
            },
            new SqlParameter("@ExecutionType", SqlDbType.NVarChar, 50)
            {
                Value = "7"
            }
        };

                DataTable dtResult = _dbHelper.ExecuteStoredProcedure(
                    _systemDbConnectionString,
                    spName,
                    parameters
                );

                foreach (DataRow row in dtResult.Rows)
                {
                    sessionList.Add(row["SessionID"].ToString());
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetTaskLockSessionsList",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return sessionList;
        }
        public bool UpdateActiveTask(ActiveUserTask userActiveTask, string executionType, ref MessageSet message)
        {
            try
            {
                string spName = "[dbo].[usp_UpdateZYActiveTask]";

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
                { Value = (object)userActiveTask.BusinessUnit ?? DBNull.Value },
            new SqlParameter("@TaskCode", SqlDbType.NVarChar, 50)
                { Value = (object)userActiveTask.TaskCode ?? DBNull.Value },
            new SqlParameter("@SessionID", SqlDbType.NVarChar, 50)
                { Value = (object)userActiveTask.SessionID ?? DBNull.Value },
            new SqlParameter("@ExecutionType", SqlDbType.NVarChar, 50)
                { Value = executionType }
        };

                if (executionType == "1")
                {
                    parameters.AddRange(new[]
                    {
                new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                    { Value = (object)userActiveTask.UserName ?? DBNull.Value },
                new SqlParameter("@ApplicationCode", SqlDbType.NVarChar, 50)
                    { Value = (object)userActiveTask.ApplicationCode ?? DBNull.Value },
                new SqlParameter("@ExclusivityMode", SqlDbType.NVarChar, 50)
                    { Value = (object)userActiveTask.ExclusivityMode ?? DBNull.Value },
                new SqlParameter("@WorkstationID", SqlDbType.NVarChar, 50)
                    { Value = (object)userActiveTask.WorkstationID ?? DBNull.Value },
                new SqlParameter("@StatusFlag", SqlDbType.NVarChar, 50)
                    { Value = (object)userActiveTask.StatusFlag ?? DBNull.Value },
                new SqlParameter("@TerritoryCode", SqlDbType.NVarChar, 50)
                    { Value = (object)userActiveTask.TerritoryCode ?? DBNull.Value }
            });
                }

                int rowsAffected = _dbHelper.ExecuteNonQuery(
                    _systemDbConnectionString,
                    spName,
                    parameters.ToArray()
                );

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateActiveTask",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
                return false;
            }
        }

        //V2049 E

        #endregion

        #endregion

        #region Private Methods

        #region Select functions

        private void GetUserBusinessUnit(string businessUnit, ref BusinessUnit objBusinessUnit, ref MessageSet message)
        {
            try
            {
                string commandText = @"
            SELECT 
                BusinessUnit,
                BusinessUnitName,
                AddressLine1,
                AddressLine2,
                AddressLine3,
                AddressLine4,
                AddressLine5,
                PostCode,
                TelephoneNumber,
                FaxNumber,
                EmailAddress,
                WebAddress,
                VATRegistrationNumber,
                Logo
            FROM dbo.ZYBusinessUnit
            WHERE BusinessUnit = @BusinessUnit";

                var parameter = new SqlParameter("@BusinessUnit", SqlDbType.NVarChar)
                {
                    Value = businessUnit.Trim()
                };

                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                if (dt.Rows.Count == 0) return;

                DataRow row = dt.Rows[0];

                objBusinessUnit.BusinessUnitCode = row["BusinessUnit"]?.ToString() ?? "";
                objBusinessUnit.BusinessUnitName = row["BusinessUnitName"]?.ToString() ?? "";
                objBusinessUnit.AddressLine1 = row["AddressLine1"]?.ToString() ?? "";
                objBusinessUnit.AddressLine2 = row["AddressLine2"]?.ToString() ?? "";
                objBusinessUnit.AddressLine3 = row["AddressLine3"]?.ToString() ?? "";
                objBusinessUnit.AddressLine4 = row["AddressLine4"]?.ToString() ?? "";
                objBusinessUnit.AddressLine5 = row["AddressLine5"]?.ToString() ?? "";
                objBusinessUnit.PostCode = row["PostCode"]?.ToString() ?? "";
                objBusinessUnit.TelephoneNumber = row["TelephoneNumber"]?.ToString() ?? "";
                objBusinessUnit.FaxNumber = row["FaxNumber"]?.ToString() ?? "";
                objBusinessUnit.EmailAddress = row["EmailAddress"]?.ToString() ?? "";
                objBusinessUnit.VATRegistrationNumber = row["VATRegistrationNumber"]?.ToString() ?? "";
                objBusinessUnit.Logo = row["Logo"]?.ToString() ?? "";
                objBusinessUnit.WebAddress = row["WebAddress"]?.ToString()?.Trim() ?? "";
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserBusinessUnit",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        //VR002 Begin
        private void GetUserBusinessUnit(
            string businessUnit,
            ref BusinessUnit objBusinessUnit,
            string distributorCode,
            ref MessageSet message)
        {
            try
            {
                // Step 1: Get VAT & Logo from ZYBusinessUnit
                string businessUnitSQL = @"
            SELECT VATRegistrationNumber, Logo
            FROM dbo.ZYBusinessUnit
            WHERE BusinessUnit = @BusinessUnit";

                var buParam = new SqlParameter("@BusinessUnit", SqlDbType.NVarChar)
                {
                    Value = businessUnit.Trim()
                };

                DataTable dtBusinessUnit = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    businessUnitSQL,
                    new[] { buParam }
                );

                if (dtBusinessUnit.Rows.Count > 0)
                {
                    objBusinessUnit.Logo = dtBusinessUnit.Rows[0]["Logo"]?.ToString() ?? "";
                }

                // Step 2: Get Distributor Info
                string distInfoSQL = @"
            SELECT 
                BusinessUnit, DistributorCode, DistributorName,
                AddressLine1, AddressLine2, AddressLine3,
                AddressLine4, AddressLine5,
                PostCode, TelephoneNumber, FaxNumber,
                EMailAddress, WebAddress, VATRegistrationNo
            FROM RD.Distributor
            WHERE BusinessUnit = @BusinessUnit
              AND DistributorCode = @DistributorCode
              AND Status = '1'";

                var distParams = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar)
            {
                Value = businessUnit.Trim()
            },
            new SqlParameter("@DistributorCode", SqlDbType.NVarChar)
            {
                Value = distributorCode.Trim()
            }
        };

                DataTable dtDistributor = _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    distInfoSQL,
                    distParams
                );

                if (dtDistributor.Rows.Count > 0)
                {
                    DataRow dtRow = dtDistributor.Rows[0];

                    objBusinessUnit.BusinessUnitCode = dtRow["BusinessUnit"]?.ToString() ?? "";
                    objBusinessUnit.BusinessUnitName = dtRow["DistributorName"]?.ToString()?.Trim() ?? "";
                    objBusinessUnit.AddressLine1 = dtRow["AddressLine1"]?.ToString()?.Trim() ?? "";
                    objBusinessUnit.AddressLine2 = dtRow["AddressLine2"]?.ToString()?.Trim() ?? "";
                    objBusinessUnit.AddressLine3 = dtRow["AddressLine3"]?.ToString()?.Trim() ?? "";
                    objBusinessUnit.AddressLine4 = dtRow["AddressLine4"]?.ToString()?.Trim() ?? ""; // V2006
                    objBusinessUnit.AddressLine5 = dtRow["AddressLine5"]?.ToString()?.Trim() ?? ""; // V2006
                    objBusinessUnit.PostCode = dtRow["PostCode"]?.ToString() ?? "";
                    objBusinessUnit.TelephoneNumber = dtRow["TelephoneNumber"]?.ToString() ?? "";
                    objBusinessUnit.FaxNumber = dtRow["FaxNumber"]?.ToString() ?? "";
                    objBusinessUnit.EmailAddress = dtRow["EMailAddress"]?.ToString() ?? "";
                    objBusinessUnit.VATRegistrationNumber = dtRow["VATRegistrationNo"]?.ToString() ?? "";
                    objBusinessUnit.WebAddress = dtRow["WebAddress"]?.ToString()?.Trim() ?? "";
                }
                else
                {
                    // Step 3: Fallback to BusinessUnit info if no distributor found
                    string fallbackSQL = @"
                SELECT 
                    BusinessUnit, BusinessUnitName,
                    AddressLine1, AddressLine2, AddressLine3,
                    AddressLine4, AddressLine5,
                    PostCode, TelephoneNumber, FaxNumber,
                    EmailAddress, VATRegistrationNumber, Logo, WebAddress
                FROM dbo.ZYBusinessUnit
                WHERE BusinessUnit = @BusinessUnit";

                    DataTable dtFallback = _dbHelper.ExecuteQuery(
                        _systemDbConnectionString,
                        fallbackSQL,
                        new[] { buParam }
                    );

                    if (dtFallback.Rows.Count > 0)
                    {
                        DataRow dtRow = dtFallback.Rows[0];

                        objBusinessUnit.BusinessUnitCode = dtRow["BusinessUnit"]?.ToString() ?? "";
                        objBusinessUnit.BusinessUnitName = dtRow["BusinessUnitName"]?.ToString() ?? "";
                        objBusinessUnit.AddressLine1 = dtRow["AddressLine1"]?.ToString() ?? "";
                        objBusinessUnit.AddressLine2 = dtRow["AddressLine2"]?.ToString() ?? "";
                        objBusinessUnit.AddressLine3 = dtRow["AddressLine3"]?.ToString() ?? "";
                        objBusinessUnit.AddressLine4 = dtRow["AddressLine4"]?.ToString() ?? "";
                        objBusinessUnit.AddressLine5 = dtRow["AddressLine5"]?.ToString() ?? "";
                        objBusinessUnit.PostCode = dtRow["PostCode"]?.ToString() ?? "";
                        objBusinessUnit.TelephoneNumber = dtRow["TelephoneNumber"]?.ToString() ?? "";
                        objBusinessUnit.FaxNumber = dtRow["FaxNumber"]?.ToString() ?? "";
                        objBusinessUnit.EmailAddress = dtRow["EmailAddress"]?.ToString() ?? "";
                        objBusinessUnit.VATRegistrationNumber = dtRow["VATRegistrationNumber"]?.ToString() ?? "";
                        objBusinessUnit.WebAddress = dtRow["WebAddress"]?.ToString()?.Trim() ?? "";
                    }
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserBusinessUnit",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }//VR002 End

        private bool CheckUserData(string userName, string password, ref MessageSet message)
        {
            bool isExists = false;
            var stroEncript = new StroEncript();
            string encriptPass = stroEncript.Encript(password.Trim());

            try
            {
                string spName = "[dbo].[usp_GetUserData]";

                var parameters = new[]
                {
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userName.Trim()
            },
            new SqlParameter("@Password", SqlDbType.NVarChar, 100)
            {
                Value = encriptPass
            },
            new SqlParameter("@ExecutionType", SqlDbType.Char, 1)
            {
                Value = "1"
            },
            new SqlParameter("@DefaultBusinessUnit", SqlDbType.Char, 1)
            {
                Value = "1"
            }
        };

                DataTable dtResult = _dbHelper.ExecuteStoredProcedure(
                    _systemDbConnectionString,
                    spName,
                    parameters
                );

                if (dtResult.Rows.Count > 0)
                {
                    char passwordLocked = Convert.ToChar(dtResult.Rows[0]["PasswordLocked"]);
                    string activeFlag = dtResult.Rows[0]["ActiveFlag"].ToString();

                    if (passwordLocked == '0')
                    {
                        CheckActiveFlagUser(activeFlag, userName, ref message);

                        if (user.ActiveFlag)
                        {
                            isExists = true;
                        }
                        else
                        {
                            message = MessageCreate.CreateUserMessage(100003, "", "", "", "", "", "");
                        }
                    }
                    else
                    {
                        message = MessageCreate.CreateUserMessage(100004, "", "", "", "", "", "");
                    }
                }
                else
                {
                    message = MessageCreate.CreateUserMessage(100005, "", "", "", "", "", "");
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "CheckUserData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return isExists;
        }

        private void GetUserMainData(string userName, string password, ref MessageSet message)
        {
            string encriptPass = password; // VR029: assuming password is already encrypted
            try
            {
                string spName = "[dbo].[usp_GetUserData]";

                var parameters = new[]
                {
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userName.Trim()
            },
            new SqlParameter("@Password", SqlDbType.NVarChar, 100)
            {
                Value = encriptPass
            },
            new SqlParameter("@ExecutionType", SqlDbType.Char, 1)
            {
                Value = "1"
            },
            new SqlParameter("@DefaultBusinessUnit", SqlDbType.Char, 1)
            {
                Value = "1"
            }
        };

                DataTable dtResult = _dbHelper.ExecuteStoredProcedure(
                    _systemDbConnectionString,
                    spName,
                    parameters
                );

                // Check if user exists
                if (dtResult.Rows.Count > 0)
                {
                    string activeFlag = "";
                    foreach (DataRow row in dtResult.Rows)
                    {
                        user.isExists = true;
                        user.BusinessUnit = row["BusinessUnit"]?.ToString();
                        user.UserName = row["UserName"]?.ToString();
                        user.UserFullName = row["UserFullName"]?.ToString();
                        user.UserLevelGroup = row["UserLevelGroup"]?.ToString();
                        user.Password = row["Password"]?.ToString();
                        user.PasswordLocked = Convert.ToChar(row["PasswordLocked"]);
                        activeFlag = row["ActiveFlag"]?.ToString();
                        user.PowerUser = row["PowerUser"]?.ToString();
                        user.Theme = row["Theme"]?.ToString();
                        user.Language = row["Language"]?.ToString();
                        user.CaptionEditor = row["CaptionEditor"]?.ToString().Trim() == "1"; // V2033
                        user.FontColor = row["FontColor"]?.ToString();
                        user.FontSize = int.TryParse(row["FontSize"]?.ToString(), out int fontSize) ? fontSize : 0;
                        user.FontName = row["FontName"]?.ToString();
                        user.HasProPicture = row["ProPicAvailable"]?.ToString().Length > 0 ? row["ProPicAvailable"].ToString()[0] : '\0'; // New Ventura
                        user.DefaultRoleCode = row["RoleCode"]?.ToString(); // v2014

                        // Distributor Code - VR002
                        string distributorCode = row["DistributorCode"]?.ToString();
                        user.DistributorCode = string.IsNullOrEmpty(distributorCode) ? null : distributorCode.Trim();

                        // VR013
                        user.RestrictFOCInvoice = row["RestrictFOCInvoice"]?.ToString() ?? "0";

                        // VR024
                        user.SupplierCode = row["SupplierCode"]?.ToString()?.Trim() ?? "";
                        user.CustomerCode = row["CustomerCode"]?.ToString()?.Trim() ?? "";

                        // VR028
                        user.ExecutiveCode = row["ExecutiveCode"]?.ToString()?.Trim() ?? "";

                        // V2004
                        user.PasswordChange = row["PasswordChange"]?.ToString() ?? "";

                        // V2042
                        user.POReturnAuthorizationLevel = row["POReturnAuthorizationLevel"]?.ToString() ?? "";
                        user.POReturnAuthorizationUpTo = row["POReturnAuthorizationUpTo"]?.ToString() ?? "";

                        // V2048
                        user.PUSQQtyEdit = row["PUSQQtyEdit"]?.ToString() ?? "";
                    }

                    // Check user status
                    if (user.PasswordLocked == '0')
                    {
                        CheckActiveFlagUser(activeFlag, userName, ref message);
                        if (user.ActiveFlag)
                        {
                            GetPasswordData(user.BusinessUnit?.Trim(), ref message);
                            GetLastLoggingDAte(userName, ref message);

                            if (!user.IsUserExpire && !user.IsPassExpire)
                            {
                                CheckUserAlradyInSession(userName, ref message);

                                if (string.IsNullOrEmpty(user.AlreadyLogin))
                                {
                                    GetLastPasswordChangeDate(userName, ref message);
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    user.UserName = userName;
                    user.Password = password;
                    user.PasswordChange = ""; // V2004
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserMainData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private void CheckUserAlradyInSession(string userName, ref MessageSet message)
        {
            // VR008
            user.AlreadyLogin = ""; 
            return;
            // VR008

            string dateTime = "";

            try
            {
                string commandText = @"
            SELECT MAX(Date) AS Date, MAX(Time) AS Time
            FROM ZYPasswordLoginDetails 
            WHERE UserName = @UserName
              AND SuccessfulLogin = N'1'
              AND LogOutTime IS NULL
            GROUP BY UserName, SuccessfulLogin, LogOutTime";

                var parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                {
                    Value = userName.Trim()
                };

                DataTable dtResult = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                if (dtResult.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResult.Rows)
                    {
                        if (row["Date"] != DBNull.Value && row["Time"] != DBNull.Value)
                        {
                            DateTime date = Convert.ToDateTime(row["Date"]);
                            DateTime time = Convert.ToDateTime(row["Time"]);
                            dateTime = date.ToShortDateString() + " " + time.ToLongTimeString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "CheckUserAlradyInSession",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            user.AlreadyLogin = dateTime;
        }
        private void GetUserRoles(string userName, ref MessageSet message)
        {
            var userRoles = new List<UserRole>();
            try
            {
                if (userName.Trim() != "administrator") // VR006
                {
                    string commandText = @"
                SELECT ZYUserRole.RoleCode, ZYRole.Description, ZYRole.Icon
                FROM ZYUser
                INNER JOIN ZYUserRole ON ZYUser.UserName = ZYUserRole.UserName
                INNER JOIN ZYRole ON ZYUserRole.RoleCode = ZYRole.RoleCode
                WHERE ZYUser.UserName = @UserName
                ORDER BY ZYUserRole.Sequence";

                    var parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                    {
                        Value = userName.Trim()
                    };

                    DataTable dtResult = _dbHelper.ExecuteQuery(
                        _systemDbConnectionString,
                        commandText,
                        new[] { parameter }
                    );

                    foreach (DataRow row in dtResult.Rows)
                    {
                        userRoles.Add(new UserRole
                        {
                            RoleCode = row["RoleCode"]?.ToString() ?? "",
                            Description = row["Description"]?.ToString() ?? "",
                            Icon = row["Icon"]?.ToString()?.Trim() ?? ""
                        });
                    }
                }
                else
                {
                    // VR006: Special case for administrator
                    userRoles.Add(new UserRole
                    {
                        RoleCode = "05_ROLE",
                        Description = "SYSTEM",
                        Icon = "role1.png"
                    });
                }

                user.UserRoles = userRoles;
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserRoles",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        public List<UserRole> GetUserRole(string userName, ref MessageSet message)
        {
            var userRoles = new List<UserRole>();

            try
            {
                if (userName.Trim() != "administrator")
                {
                    string commandText = @"
                SELECT ZYUserRole.RoleCode, ZYRole.Description, ZYRole.Icon
                FROM ZYUser
                INNER JOIN ZYUserRole ON ZYUser.UserName = ZYUserRole.UserName
                INNER JOIN ZYRole ON ZYUserRole.RoleCode = ZYRole.RoleCode
                WHERE ZYUser.UserName = @UserName
                ORDER BY ZYUserRole.Sequence";

                    var parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                    {
                        Value = userName.Trim()
                    };

                    DataTable dtResult = _dbHelper.ExecuteQuery(
                        _systemDbConnectionString,
                        commandText,
                        new[] { parameter }
                    );

                    foreach (DataRow row in dtResult.Rows)
                    {
                        userRoles.Add(new UserRole
                        {
                            RoleCode = row["RoleCode"]?.ToString() ?? "",
                            Description = row["Description"]?.ToString() ?? "",
                            Icon = row["Icon"]?.ToString()?.Trim() ?? ""
                        });
                    }
                }
                else
                {
                    userRoles.Add(new UserRole
                    {
                        RoleCode = "05_ROLE",
                        Description = "SYSTEM",
                        Icon = "role1.png"
                    });
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserRole",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
            

            return userRoles;
        }

        public DataTable getUserfavourites(string businessUnit, string userName, ref MessageSet message)
        {
            DataTable dt = new DataTable("UserFavourites");

            try
            {
                string commandText = @"
            SELECT BookmarkID, Bookmark, TaskPath
            FROM RD.Bookmarks
            WHERE BusinessUnit = @BusinessUnit
              AND UserName = @UserName";

                var parameters = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
            {
                Value = businessUnit.Trim()
            },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userName.Trim()
            }
        };

                dt = _dbHelper.ExecuteQuery(
                   _userDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "getUserfavourites",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return dt;
        }

        public string GetLicenseKey(string userName, string businessUnit, ref MessageSet message)
        {
            string licenseKey = "";

            try
            {
                string commandText = @"
            SELECT ZYUserLicenseTable.LicenseNumbers
            FROM ZYUserBusUnit
            INNER JOIN ZYUser ON ZYUserBusUnit.UserName = ZYUser.UserName
            INNER JOIN ZYUserLicenseTable ON ZYUserBusUnit.BusinessUnit = ZYUserLicenseTable.BusinessUnit
            WHERE ZYUserBusUnit.BusinessUnit = @BusinessUnit";

                var parameter = new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
                {
                    Value = businessUnit.Trim()
                };

                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                if (dt.Rows.Count > 0 && dt.Rows[0]["LicenseNumbers"] != DBNull.Value)
                {
                    licenseKey = dt.Rows[0]["LicenseNumbers"].ToString();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetLicenseKey",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return licenseKey;
        }
        private string GetExpiryAlertStartFrom(string userName, string businessUnit, ref MessageSet message)
        {
            string startFrom = "";

            try
            {
                string commandText = @"
            SELECT ZYUserLicenseTable.ExpiryAlertStartFrom
            FROM ZYUserBusUnit
            INNER JOIN ZYUser ON ZYUserBusUnit.UserName = ZYUser.UserName
            INNER JOIN ZYUserLicenseTable ON ZYUserBusUnit.BusinessUnit = ZYUserLicenseTable.BusinessUnit
            WHERE ZYUserBusUnit.BusinessUnit = @BusinessUnit";

                var parameter = new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
                {
                    Value = businessUnit.Trim()
                };

                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                if (dt.Rows.Count > 0 && dt.Rows[0]["ExpiryAlertStartFrom"] != DBNull.Value)
                {
                    startFrom = dt.Rows[0]["ExpiryAlertStartFrom"].ToString();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetExpiryAlertStartFrom",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return startFrom;
        }
        private void GetPasswordData(string businessUnit, ref MessageSet message)
        {
            var password = new Password();
            try
            {
                string commandText = @"
            SELECT DISTINCT 
                ZYPasswordControl.MinLength, 
                ZYPasswordControl.MaxLength, 
                ZYPasswordControl.NoOfSpecialCharacters,
                ZYPasswordControl.ReusePeriod, 
                ZYPasswordControl.NoOfAttempts,
                ZYPasswordControl.ExpirePeriodInMonths,
                ZYPasswordControl.UserExpirePeriodInMonths
            FROM ZYPasswordControl
            INNER JOIN ZYUserBusUnit ON ZYUserBusUnit.BusinessUnit = ZYPasswordControl.BusinessUnit
            WHERE ZYUserBusUnit.BusinessUnit = @BusinessUnit";

                var parameter = new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
                {
                    Value = businessUnit.Trim()
                };

                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    password.MaxLength = row["MaxLength"]?.ToString() ?? "";
                    password.MinLength = row["MinLength"]?.ToString() ?? "";
                    password.NoOfSpecialCharacters = row["NoOfSpecialCharacters"]?.ToString() ?? "";
                    password.ReusePeriod = row["ReusePeriod"] != DBNull.Value
                        ? Convert.ToInt32(row["ReusePeriod"])
                        : 0;
                    password.NoOfAttempts = row["NoOfAttempts"] != DBNull.Value
                        ? Convert.ToInt32(row["NoOfAttempts"])
                        : 0;
                    password.ExpirePeriodInMonths = row["ExpirePeriodInMonths"] != DBNull.Value
                        ? Convert.ToInt32(row["ExpirePeriodInMonths"])
                        : 0;
                    password.UserExpirePeriodInMonths = row["UserExpirePeriodInMonths"] != DBNull.Value
                        ? Convert.ToInt32(row["UserExpirePeriodInMonths"])
                        : 0;
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetPasswordData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            user.PasswordData = password;
        }
        private void GetLastLoggingDAte(string userName, ref MessageSet message)
        {
            string date = string.Empty;
            string time = string.Empty;
            DateTime datetime = DateTime.Now;
            DateTime dateExpire = new DateTime(); // VR010
            long dif = -1;

            try
            {
                if (user.PasswordData?.UserExpirePeriodInMonths == 0) // VR011
                {
                    user.IsUserExpire = false;
                    return;
                }

                string commandText = @"
            SELECT ISNULL(LEFT(MAX(Date), 12), LEFT(CONVERT(VARCHAR(26), GETDATE(), 100), 12)) AS Date,
                   ISNULL(RIGHT(MAX([Time]), 7), RIGHT((CONVERT(VARCHAR(26), GETDATE(), 100)), 7)) AS [Time]
            FROM ZYPasswordLoginDetails
            WHERE UserName = @UserName AND SuccessfulLogin = '1'
              AND Date = (
                  SELECT MAX(Date)
                  FROM ZYPasswordLoginDetails
                  WHERE UserName = @UserName AND SuccessfulLogin = '1'
              )";

                var parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                {
                    Value = userName.Trim()
                };

                DataTable dtResult = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                foreach (DataRow row in dtResult.Rows)
                {
                    date = row["Date"]?.ToString();
                    time = row["Time"]?.ToString();
                }

                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time))
                {
                    datetime = Convert.ToDateTime(date + " " + time);
                    dateExpire = datetime.AddMonths(user.PasswordData.UserExpirePeriodInMonths); // VR010
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetLastLoggingDAte",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            if (DateTime.Now > dateExpire)
            {
                user.IsUserExpire = true;
            }
            else
            {
                user.IsUserExpire = false;
            }
        }
        private void GetLastPasswordChangeDate(string userName, ref MessageSet message)
        {
            string date = string.Empty;
            string time = string.Empty;
            DateTime datetime = DateTime.Now;
            long dif = -1;

            try
            {
                string commandText = @"
            SELECT ISNULL(LEFT(MAX(Date), 12), LEFT(CONVERT(VARCHAR(26), GETDATE(), 100), 12)) AS Date,
                   ISNULL(RIGHT(MAX([Time]), 7), RIGHT((CONVERT(VARCHAR(26), GETDATE(), 100)), 7)) AS [Time]
            FROM ZYPasswordHistory
            WHERE UserName = @UserName";

                var parameter = new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                {
                    Value = userName.Trim()
                };

                DataTable dtResult = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    new[] { parameter }
                );

                foreach (DataRow row in dtResult.Rows)
                {
                    date = row["Date"]?.ToString() ?? string.Empty;
                    time = row["Time"]?.ToString() ?? string.Empty;
                }

                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(time))
                {
                    datetime = Convert.ToDateTime(date + " " + time);
                    dif = (long)(DateTime.Now - datetime).TotalDays / 30; // Approximate months
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetLastPasswordChangeDate",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            if (dif > user.PasswordData?.ExpirePeriodInMonths)
            {
                user.IsPassExpire = true;
            }
            else
            {
                user.IsPassExpire = false;
            }
        }
        private List<MultimediaDetails> GetUserMultimedia(
            string userName,
            string businessUnit,
            DataTable dtMasterGroupVal,
            ref MessageSet msg)
        {
            var multimedia = new List<MultimediaDetails>();

            try
            {
                string territoryCode = GetUserTerritoryCode(userName, businessUnit, ref msg);

                string commandText = @"
            SELECT DISTINCT
                m.AlertID, 
                m.FileName,
                m.Extension,
                m.Text,
                m.Description, 
                m.Mandatory, 
                m.Type,
                t.Occurence,
                t.TargetCategory
            FROM RD.MultimediaHeader AS m
            INNER JOIN (
                SELECT DISTINCT * FROM RD.MultimediaDetails 
                WHERE TargetCategory = 4 AND Occurence > 0
                UNION ALL
                SELECT DISTINCT * FROM RD.MultimediaDetails 
                WHERE TargetCategory = 3 AND Occurence > 0 AND TargetValue = @UserName
                @TerritoryQuery
            ) AS t ON m.AlertID = t.AlertID AND m.BusinessUnit = t.BusinessUnit
            WHERE m.BusinessUnit = @BusinessUnit
              AND m.Status = '1'
              AND m.Category = 'B'
              AND ((m.StartDate <= @Today OR m.StartDate IS NULL))
              AND ((m.EndDate IS NULL) OR (m.EndDate >= @Today))";

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50) { Value = userName.Trim() },
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50) { Value = businessUnit.Trim() },
            new SqlParameter("@Today", SqlDbType.Date) { Value = DateTime.Today }
        };

                string territoryQuery = "";
                if (!string.IsNullOrEmpty(territoryCode))
                {
                    territoryQuery = @"
                UNION ALL
                SELECT DISTINCT * FROM RD.MultimediaDetails 
                WHERE TargetCategory = 1 AND Occurence > 0 AND TargetValue = @TerritoryCode";

                    parameters.Add(new SqlParameter("@TerritoryCode", SqlDbType.NVarChar, 50)
                    {
                        Value = territoryCode.Trim()
                    });
                }

                string finalCommand = commandText.Replace("@TerritoryQuery", territoryQuery);

                DataTable dtResult = _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    finalCommand,
                    parameters.ToArray()
                );

                foreach (DataRow row in dtResult.Rows)
                {
                    multimedia.Add(new MultimediaDetails
                    {
                        FileName = row["FileName"]?.ToString() ?? "",
                        Extension = row["Extension"]?.ToString() ?? "",
                        Text = row["Text"]?.ToString() ?? "",
                        Description = row["Description"]?.ToString() ?? "",
                        Mandatory = Convert.ToChar(row["Mandatory"]?.ToString()),
                        Type = Convert.ToChar(row["Type"]?.ToString())
                    });

                    int alertId = Convert.ToInt32(row["AlertID"]);
                    int targetCategory = Convert.ToInt32(row["TargetCategory"]);

                    UpdateMultimedia(businessUnit, userName, territoryCode, alertId, targetCategory, ref msg);
                }
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserMultimedia",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return multimedia;
        }
        //V2015

        private string GetUserTerritoryCode(string userName, string businessUnit, ref MessageSet message)
        {
            string territory = "";

            try
            {
                string commandText = @"
            SELECT TerritoryCode
            FROM XA.UserTerritory
            WHERE UserName = @UserName
              AND BusinessUnit = @BusinessUnit
              AND Status = '1'";

                var parameters = new[]
                {
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userName.Trim()
            },
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
            {
                Value = businessUnit.Trim()
            }
        };

                DataTable dtResult = _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );

                if (dtResult.Rows.Count > 0)
                {
                    territory = dtResult.Rows[0]["TerritoryCode"]?.ToString() ?? "";
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetUserTerritoryCode",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return territory;
        }
        private void GetActiveUsersData(ref List<User> activeUsers, ref MessageSet message)
        {
            try
            {
                string commandText = @"
            SELECT ZYUser.UserName,
                   ZYUserBusUnit.BusinessUnit,
                   ZYUser.Password,
                   ZYUser.ActiveFlag,
                   ZYUser.PasswordLocked,
                   ZYUser.PowerUser
            FROM ZYUser
            INNER JOIN ZYUserBusUnit ON ZYUser.UserName = ZYUserBusUnit.UserName
            WHERE ZYUserBusUnit.DefaultBusinessUnit = '1'
              AND ZYUser.UserName NOT IN ('admin', 'administrator', 'xontadmin')
              AND ZYUser.UserLevelGroup <> 'OWNER'";

                DataTable dtResult = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters: null
                );

                if (dtResult.Rows.Count > 0)
                {
                    User previousUser = null;

                    foreach (DataRow row in dtResult.Rows)
                    {
                        var userOb = new User
                        {
                            isExists = true,
                            BusinessUnit = row["BusinessUnit"]?.ToString(),
                            UserName = row["UserName"]?.ToString(),
                            Password = row["Password"]?.ToString(),
                            PasswordLocked = Convert.ToChar(row["PasswordLocked"]),
                            ActiveFlagDescription = row["ActiveFlag"]?.ToString(),
                            PowerUser = row["PowerUser"]?.ToString(),
                            ActiveFlag = false
                        };

                        if (previousUser == null ||
                            previousUser.UserName?.Trim() != userOb.UserName?.Trim())
                        {
                            CheckActiveFlagUser(ref userOb, ref message);
                            previousUser = userOb;
                        }

                        activeUsers.Add(userOb);
                    }
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetActiveUsersData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private void GetPasswordHistoryListData(
            string userName,
            string reusePeriod,
            ref List<string> passList,
            ref MessageSet message)
        {
            passList = new List<string>();

            try
            {
                string commandText = @"
            SELECT TOP (@ReusePeriod) Password
            FROM ZYPasswordHistory
            WHERE UserName = @UserName
            ORDER BY Date DESC, Time DESC";

                var parameters = new[]
                {
            new SqlParameter("@ReusePeriod", SqlDbType.Int)
            {
                Value = int.Parse(reusePeriod)  // Ensure reusePeriod is valid
            },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userName.Trim()
            }
        };

                DataTable dtResult = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );

                foreach (DataRow row in dtResult.Rows)
                {
                    string password = row["Password"]?.ToString();
                    if (!string.IsNullOrEmpty(password))
                    {
                        passList.Add(password);
                    }
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetPasswordHistoryListData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region Update functions

        public void SaveReminders(string businessUnit, string userName, string reminderName, string message, string timeString, ref MessageSet msg)
        {
            string command = @"
        INSERT INTO [RD].[Reminders] 
            ([BusinessUnit], [UserName], [ReminderID], [Message], [CreatedBy], [TriggerTime]) 
        VALUES 
            (@BusinessUnit, @UserName, @ReminderName, @Message, @CreatedBy, CONVERT(DATETIME, @TimeString, 101))";

            try
            {
                var parameters = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50) { Value = businessUnit.Trim() },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50) { Value = userName.Trim() },
            new SqlParameter("@ReminderName", SqlDbType.NVarChar, 100) { Value = reminderName.Trim() },
            new SqlParameter("@Message", SqlDbType.NVarChar, -1) { Value = message },
            new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50) { Value = userName.Trim() },
            new SqlParameter("@TimeString", SqlDbType.NVarChar, 50) { Value = timeString }
        };

                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _userDbConnectionString,
                        command,
                        parameters
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "SaveReminders",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        public void UpdateReminder(string businessUnit, string userName, string reminderName, string message, string timeString, string reminderNameOld, ref MessageSet msg)
        {
            string command = @"
        UPDATE [RD].[Reminders] 
        SET 
            ReminderID = @ReminderName, 
            Message = @Message, 
            CreatedBy = @CreatedBy, 
            TriggerTime = CONVERT(DATETIME, @TimeString, 101)
        WHERE 
            BusinessUnit = @BusinessUnit 
            AND UserName = @UserName 
            AND ReminderID = @ReminderNameOld";

            try
            {
                var parameters = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50) { Value = businessUnit.Trim() },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50) { Value = userName.Trim() },
            new SqlParameter("@ReminderName", SqlDbType.NVarChar, 100) { Value = reminderName.Trim() },
            new SqlParameter("@Message", SqlDbType.NVarChar, -1) { Value = message },
            new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50) { Value = userName.Trim() },
            new SqlParameter("@TimeString", SqlDbType.NVarChar, 50) { Value = timeString },
            new SqlParameter("@ReminderNameOld", SqlDbType.NVarChar, 100) { Value = reminderNameOld.Trim() }
        };

                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _userDbConnectionString,
                        command,
                        parameters
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateReminder",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        public void DeleteReminders(string userName, List<string> reminders, ref MessageSet message)
        {
            if (reminders == null || reminders.Count == 0)
                return;

            string command = "DELETE FROM RD.Reminders WHERE CreatedBy = @UserName AND ReminderID IN (";

            // Build parameter list dynamically
            var parameters = new List<SqlParameter>();
            for (int i = 0; i < reminders.Count; i++)
            {
                string paramName = "@ReminderID" + i;
                command += paramName + ",";
                parameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar, 100) { Value = reminders[i].Trim() });
            }

            command = command.TrimEnd(',') + ")";
            parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 50) { Value = userName.Trim() });

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _userDbConnectionString,
                        command,
                        parameters.ToArray()
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "DeleteReminders",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }

        public void DeleteBookmarks(string userName, string[] bookmarks, ref MessageSet message)
        {
            if (bookmarks == null || bookmarks.Length == 0)
                return;

            string command = "DELETE FROM RD.Bookmarks WHERE CreatedBy = @UserName AND BookmarkID IN (";

            var parameters = new List<SqlParameter>();
            for (int i = 0; i < bookmarks.Length; i++)
            {
                string paramName = "@BookmarkID" + i;
                command += paramName + ",";
                parameters.Add(new SqlParameter(paramName, SqlDbType.NVarChar, 100)
                {
                    Value = bookmarks[i].Trim()
                });
            }

            command = command.TrimEnd(',') + ")";
            parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userName.Trim()
            });

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _userDbConnectionString,
                        command,
                        parameters.ToArray()
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "DeleteBookmarks",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private void UpdateUserSettingData(User userOb, ref MessageSet message)
        {
            string commandText = @"
        UPDATE [dbo].[ZYUser]
        SET 
            Theme = @Theme,
            Language = @Language,
            FontName = @FontName,
            FontColor = @FontColor,
            FontSize = @FontSize
        WHERE UserName = @UserName";

            try
            {
                var parameters = new[]
                {
            new SqlParameter("@Theme", SqlDbType.NVarChar, 50)
            {
                Value = userOb.Theme?.Trim() ?? ""
            },
            new SqlParameter("@Language", SqlDbType.NVarChar, 50)
            {
                Value = userOb.Language?.Trim() ?? ""
            },
            new SqlParameter("@FontName", SqlDbType.NVarChar, 50)
            {
                Value = userOb.FontName?.Trim() ?? ""
            },
            new SqlParameter("@FontColor", SqlDbType.NVarChar, 50)
            {
                Value = userOb.FontColor?.Trim() ?? ""
            },
            new SqlParameter("@FontSize", SqlDbType.Int)
            {
                Value = userOb.FontSize > 0 ? (object)userOb.FontSize : DBNull.Value
            },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userOb.UserName?.Trim() ?? ""
            }
        };

                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _systemDbConnectionString,
                        commandText,
                        parameters
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateUserSettingData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private void ResetProfilePictureData(User userOb, ref MessageSet message)
        {
            string commandText = @"
        UPDATE [dbo].[ZYUser]
        SET ProPicAvailable = @ProPicAvailable
        WHERE [UserName] = @UserName";

            try
            {
                var parameters = new[]
                {
            new SqlParameter("@ProPicAvailable", SqlDbType.Char, 1)
            {
                Value = "0"
            },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userOb.UserName.Trim()
            }
        };

                _dbHelper.ExecuteNonQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "ResetProfilePictureData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }

        private void UpdateLoginData(User userOb, ref MessageSet message)
        {
            string commandText = string.Empty;

            try
            {
                string logoutTime = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                if (userOb.SuccessfulLogin.Equals("0"))
                {
                    commandText = @"
                UPDATE [ZYPasswordLoginDetails]
                SET [LogOutTime] = @LogOutTime,
                    [LogOutReson] = @LogOutReson
                WHERE [UserName] = @UserName
                  AND [WorkstationID] = @WorkstationId
                  AND [SessionID] = @SessionId
                  AND [SuccessfulLogin] = '0'
                  AND [LogOutReson] IS NULL";
                }
                else
                {
                    commandText = @"
                UPDATE [ZYPasswordLoginDetails]
                SET [LogOutTime] = @LogOutTime,
                    [LogOutReson] = @LogOutReson
                WHERE [UserName] = @UserName
                  AND [WorkstationID] = @WorkstationId
                  AND [SessionID] = @SessionId
                  AND [LogOutReson] IS NULL";
                }

                var parameters = new[]
                {
            new SqlParameter("@LogOutTime", SqlDbType.DateTime) { Value = logoutTime },
            new SqlParameter("@LogOutReson", SqlDbType.NVarChar, 255)
            {
                Value = string.IsNullOrEmpty(userOb.Reson) ? (object)DBNull.Value : userOb.Reson.Trim()
            },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userOb.UserName.Trim()
            },
            new SqlParameter("@WorkstationId", SqlDbType.NVarChar, 50)
            {
                Value = userOb.WorkstationId.Trim()
            },
            new SqlParameter("@SessionId", SqlDbType.NVarChar, 50)
            {
                Value = userOb.SessionId.Trim()
            }
        };

                _dbHelper.ExecuteNonQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateLoginData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private string SaveLoginData(User userOb, ref MessageSet message)
        {
            try
            {
                string commandText = @"
            INSERT INTO [ZYPasswordLoginDetails]
                ([UserName], [Date], [Time], [Password], [WorkstationID], [SuccessfulLogin], [SessionID], [Reson])
            VALUES
                (@UserName, @Date, @Time, @Password, @WorkstationID, @SuccessfulLogin, @SessionID, @Reson)";

                var parameters = new[]
                {
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userOb.UserName.Trim()
            },
            new SqlParameter("@Date", SqlDbType.Char, 8)
            {
                Value = DateTime.Now.ToString("yyyyMMdd")
            },
            new SqlParameter("@Time", SqlDbType.Char, 8)
            {
                Value = DateTime.Now.ToLongTimeString()
            },
            new SqlParameter("@Password", SqlDbType.NVarChar, 255)
            {
                Value = userOb.Password
            },
            new SqlParameter("@WorkstationID", SqlDbType.NVarChar, 50)
            {
                Value = userOb.WorkstationId.Trim()
            },
            new SqlParameter("@SuccessfulLogin", SqlDbType.Char, 1)
            {
                Value = userOb.SuccessfulLogin
            },
            new SqlParameter("@SessionID", SqlDbType.NVarChar, 50)
            {
                Value = userOb.SessionId.Trim()
            },
            new SqlParameter("@Reson", SqlDbType.NVarChar, 255)
            {
                Value = string.IsNullOrEmpty(userOb.Reson) ? (object)DBNull.Value : userOb.Reson.Trim()
            }
        };

                _dbHelper.ExecuteNonQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "SaveLoginData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return string.Empty; 
        }
        private void UpdateLoginAttempt(User userOb, ref MessageSet message)
        {
            try
            {
                string commandText = @"
            UPDATE [ZYUser]
            SET PasswordLocked = '1'
            WHERE UserName = @UserName";

                var parameters = new[]
                {
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userOb.UserName.Trim()
            }
        };

                _dbHelper.ExecuteNonQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateLoginAttempt",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private void SaveChangePasswordData(string userName, string password, string passwordChange, ref MessageSet message)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userName.Trim()
            },
            new SqlParameter("@Password", SqlDbType.NVarChar, 255)
            {
                Value = password
            },
            new SqlParameter("@PasswordChange", SqlDbType.NVarChar, 1)
            {
                Value = passwordChange
            }
        };

                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteStoredProcedure(
                        _systemDbConnectionString,
                        "dbo.usp_UpdateUserPasswordHistory",
                        parameters
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "SaveChangePasswordData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private void UpdateActiveFlag(User userOb, ref MessageSet message)
        {
            var stroEncript = new StroEncript();
            string encriptActiveFlag = stroEncript.Encript("0", userOb.UserName.Trim()); // VR009

            try
            {
                string commandText = @"
            UPDATE [ZYUser]  
            SET ActiveFlag = @ActiveFlag  
            WHERE [UserName] = @UserName";

                var parameters = new[]
                {
            new SqlParameter("@ActiveFlag", SqlDbType.NVarChar, 255)
            {
                Value = encriptActiveFlag
            },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userOb.UserName
            }
        };

                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _systemDbConnectionString,
                        commandText,
                        parameters
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateActiveFlag",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private void UpdateMultimedia(string businessUnit, string username, string territoryCode, int multRecID, int targetCategory, ref MessageSet message)
        {
            try
            {
                string commandText = @"
            UPDATE [RD].[MultimediaDetails]
            SET [Occurence] = [Occurence] - 1
            WHERE [BusinessUnit] = @BusinessUnit
              AND AlertID = @AlertID
              AND (Occurence > 0)";

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
            {
                Value = businessUnit.Trim()
            },
            new SqlParameter("@AlertID", SqlDbType.Int)
            {
                Value = multRecID
            }
        };

                if (targetCategory == 1)
                {
                    commandText += " AND TargetValue = @TargetValue";
                    parameters.Add(new SqlParameter("@TargetValue", SqlDbType.NVarChar, 50)
                    {
                        Value = territoryCode.Trim()
                    });
                }
                else if (targetCategory == 3)
                {
                    commandText += " AND TargetValue = @TargetValue";
                    parameters.Add(new SqlParameter("@TargetValue", SqlDbType.NVarChar, 50)
                    {
                        Value = username.Trim()
                    });
                }

                _dbHelper.ExecuteNonQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters.ToArray()
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateMultimedia",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        //VR025
        public void ReleaseObjectLocks(string businessUnit, List<string> sessionIdList, ref MessageSet message)
        {
            try
            {
                foreach (string sessionId in sessionIdList)
                {
                    var parameters = new[]
                    {
                new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
                {
                    Value = businessUnit.Trim()
                },
                new SqlParameter("@SessionID", SqlDbType.NVarChar, 50)
                {
                    Value = sessionId.Trim()
                }
            };

                    _dbHelper.ExecuteStoredProcedure(
                        _systemDbConnectionString,
                        "dbo.usp_ReleaseObjectLock",
                        parameters
                    );
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "ReleaseObjectLocks",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        #endregion

        //CheckActiveFlag
        private void CheckActiveFlagUser(string activeFlag, string userName, ref MessageSet message)
        {
            bool activeVal = false;
            try
            {
                var stroEncript = new StroEncript();
                string tt = stroEncript.Encript("1", userName);
                string activeData = stroEncript.Decript(activeFlag, userName);
                if (activeData.Equals("1"))
                {
                    activeVal = true;
                }
            }
            catch (Exception)
            {
                // throw;
            }
            // string decriptPsass = stroEncript.Encript(password);
            user.ActiveFlag = activeVal;
        }

        private void CheckActiveFlagUser(ref User userOb, ref MessageSet message)
        {
            bool activeVal = false;
            try
            {
                var stroEncript = new StroEncript();
                string tt = stroEncript.Encript("1", userOb.UserName);
                string activeData = stroEncript.Decript(userOb.ActiveFlagDescription.Trim(), userOb.UserName.Trim());
                if (activeData.Equals("1"))
                {
                    activeVal = true;
                    userOb.ActiveFlag = true;
                }
            }
            catch (Exception ex)
            {
                //VR011 Remove
                //message = MessageCreate.CreateErrorMessage(0, ex, "Check Active Flag User",
                //                                           "XONT.Ventura.AppConsole.DAL.dll"); 
            }

            // string decriptPsass = stroEncript.Encript(password);
            user.ActiveFlag = activeVal;
        }


        #endregion

        public DataTable getReminders(string businessUnit, string userName)
        {
            string commandText = @"
        SELECT ReminderID 
        FROM RD.Reminders 
        WHERE BusinessUnit = @BusinessUnit 
          AND UserName = @UserName";

            var parameters = new[]
            {
        new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
        {
            Value = businessUnit?.Trim() ?? ""
        },
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        }
    };

            try
            {
                return _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                return new DataTable();
            }
        }

        public DataTable GetExpiredReminders(string businessUnit, string userName, ref MessageSet msg)
        {
            string commandText = @"
        SELECT ReminderID, Message, TriggerTime 
        FROM RD.Reminders 
        WHERE UserName = @UserName 
          AND BusinessUnit = @BusinessUnit 
          AND TriggerTime < @CurrentTime
        ORDER BY TriggerTime";

            var parameters = new[]
            {
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        },
        new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
        {
            Value = businessUnit?.Trim() ?? ""
        },
        new SqlParameter("@CurrentTime", SqlDbType.DateTime)
        {
            Value = DateTime.Now
        }
    };

            try
            {
                return _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetExpiredReminders",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                return new DataTable();
            }
        }

        public DataTable getReminderDetails(string businessUnit, string userName, string reminderId, ref MessageSet msg)
        {
            string commandText = @"
        SELECT ReminderID, Message, TriggerTime 
        FROM RD.Reminders 
        WHERE BusinessUnit = @BusinessUnit 
          AND UserName = @UserName 
          AND ReminderID = @ReminderID";

            var parameters = new[]
            {
        new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
        {
            Value = businessUnit?.Trim() ?? ""
        },
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        },
        new SqlParameter("@ReminderID", SqlDbType.NVarChar, 100)
        {
            Value = reminderId?.Trim() ?? ""
        }
    };

            try
            {
                return _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "getReminderDetails",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );

                return new DataTable();
            }
        }
        
        public DataTable GetReminderJustExpired(string businessUnit, string userName, string reminder, ref MessageSet msg)
        {
            string commandText = @"
        SELECT TOP (1) ReminderID, Message 
        FROM RD.Reminders 
        WHERE BusinessUnit = @BusinessUnit 
          AND UserName = @UserName 
          AND ReminderID = @ReminderID";

            var parameters = new[]
            {
        new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
        {
            Value = businessUnit?.Trim() ?? ""
        },
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        },
        new SqlParameter("@ReminderID", SqlDbType.NVarChar, 100)
        {
            Value = reminder?.Trim() ?? ""
        }
    };

            try
            {
                return _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetReminderJustExpired",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
                return new DataTable();
            }
        }

        public DataTable getMostRecentReminder(string businessUnit, string userName, ref MessageSet msg)
        {
            string commandText = @"
        SELECT TOP (1) TriggerTime, ReminderID 
        FROM RD.Reminders 
        WHERE BusinessUnit = @BusinessUnit 
          AND UserName = @UserName 
          AND TriggerTime > @CurrentTime 
        ORDER BY TriggerTime";

            var parameters = new[]
            {
        new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
        {
            Value = businessUnit?.Trim() ?? ""
        },
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        },
        new SqlParameter("@CurrentTime", SqlDbType.DateTime)
        {
            Value = DateTime.Now
        }
    };

            try
            {
                return _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "getMostRecentReminder",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
                return new DataTable();
            }
        }
        public void saveFavourites(string businessUnit, string userName, string bookmarkId, string bookmarkName, string path, ref MessageSet message)
        {
            string commandText = @"
        INSERT INTO [RD].[Bookmarks] 
            ([BusinessUnit], [UserName], [BookmarkID], [Bookmark], [TaskPath], [CreatedBy])
        VALUES 
            (@BusinessUnit, @UserName, @BookmarkID, @Bookmark, @TaskPath, @CreatedBy)";

            var parameters = new[]
            {
        new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
        {
            Value = businessUnit?.Trim() ?? ""
        },
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        },
        new SqlParameter("@BookmarkID", SqlDbType.NVarChar, 100)
        {
            Value = bookmarkId?.Trim() ?? ""
        },
        new SqlParameter("@Bookmark", SqlDbType.NVarChar, 255)
        {
            Value = bookmarkName?.Trim() ?? ""
        },
        new SqlParameter("@TaskPath", SqlDbType.NVarChar, 255)
        {
            Value = path?.Trim() ?? ""
        },
        new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        }
    };

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _userDbConnectionString,
                        commandText,
                        parameters
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "saveFavourites",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        public void saveProfilePicture(string userName, byte[] image, ref MessageSet message)
        {
            string spName = "usp_AddProfilePicture";

            var parameters = new[]
            {
        new SqlParameter("@ProfilePicture", SqlDbType.VarBinary, -1)
        {
            Value = image ?? new byte[0]
        },
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        },
        new SqlParameter("@ProPicAvailable", SqlDbType.Char, 1)
        {
            Value = "1"
        }
    };

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteStoredProcedure(
                        _systemDbConnectionString,
                        spName,
                        parameters
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "saveProfilePicture",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }

        public byte[] getImageData(string userName, ref MessageSet message)
        {
            string commandText = @"
        SELECT ProfileImage 
        FROM dbo.ZYUser 
        WHERE UserName = @UserName";

            var parameters = new[]
            {
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        }
    };

            try
            {
                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );

                if (dt.Rows.Count > 0 && dt.Rows[0]["ProfileImage"] != DBNull.Value)
                {
                    return (byte[])dt.Rows[0]["ProfileImage"];
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "getImageData",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return new byte[0];
        }
        public DataTable getBUnit(string userName, ref MessageSet message)
        {
            string commandText = @"
        SELECT BusinessUnit 
        FROM ZYUserBusUnit 
        WHERE UserName = @UserName";

            var parameters = new[]
            {
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        }
    };

            try
            {
                return _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "getBUnit",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
                return new DataTable();
            }
        }

        public string GetDefMenuCode(string userName, ref MessageSet message)
        {
            string commandText = @"
        SELECT MenuCode 
        FROM dbo.ZYUser 
        WHERE UserName = @UserName";

            var parameters = new[]
            {
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        }
    };

            try
            {
                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["MenuCode"]?.ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "GetDefMenuCode",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return string.Empty;
        }
        #region Admin Alert V2007
        public DataTable getMostRecentAlert(ref MessageSet msg)
        {
            string commandText = @"
        SELECT TOP (1) * 
        FROM dbo.ZYAdminAlertCurrent 
        WHERE AlertTime < @CurrentTime 
        ORDER BY AlertTime ASC";

            var parameters = new[]
            {
        new SqlParameter("@CurrentTime", SqlDbType.DateTime)
        {
            Value = DateTime.Now
        }
    };

            try
            {
                return _dbHelper.ExecuteQuery(
                   _systemDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "getMostRecentAlert",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
                return new DataTable();
            }
        }
        public void DeleteAlert(int alertNumber, int recID, ref MessageSet message)
        {
            string command = @"
        DELETE FROM dbo.ZYAdminAlertCurrent
        WHERE AlertNumber = @AlertNumber
          AND RecID = @RecID";

            var parameters = new[]
            {
        new SqlParameter("@AlertNumber", SqlDbType.Int) { Value = alertNumber },
        new SqlParameter("@RecID", SqlDbType.Int) { Value = recID }
    };

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _systemDbConnectionString,
                        command,
                        parameters
                    );

                    UpdateAlert(alertNumber, recID);

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "DeleteAlert",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private void UpdateAlert(int alertNumber, int recID)
        {
            string command = @"
        UPDATE dbo.ZYAdminAlert
        SET Completed = '1'
        WHERE AlertNumber = @AlertNumber";

            var parameter = new SqlParameter("@AlertNumber", SqlDbType.Int)
            {
                Value = alertNumber
            };

            try
            {
                _dbHelper.ExecuteNonQuery(
                    _systemDbConnectionString,
                    command,
                    new[] { parameter }
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating alert completion status.", ex);
            }
        }
        public void UpdateAlertCurrent(int alertNumber, int recID, string timeSpan, ref MessageSet message)
        {
            string command = @"
        UPDATE dbo.ZYAdminAlertCurrent
        SET RepeatTimes = RepeatTimes - 1
        WHERE AlertNumber = @AlertNumber
          AND RecID = @RecID";

            var parameters = new[]
            {
        new SqlParameter("@AlertNumber", SqlDbType.Int) { Value = alertNumber },
        new SqlParameter("@RecID", SqlDbType.Int) { Value = recID }
    };

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _systemDbConnectionString,
                        command,
                        parameters
                    );

                    UpdateAlertTime(alertNumber, recID, timeSpan);

                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateAlertCurrent",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        private void UpdateAlertTime(int alertNumber, int recID, string timeSpan)
        {
            if (!DateTime.TryParse(timeSpan, out DateTime alertTime))
            {
                throw new ArgumentException("Invalid time span format.", nameof(timeSpan));
            }

            string command = @"
        UPDATE dbo.ZYAdminAlertCurrent
        SET AlertTime = @AlertTime
        WHERE AlertNumber = @AlertNumber
          AND RecID = @RecID";

            var parameters = new[]
            {
        new SqlParameter("@AlertTime", SqlDbType.DateTime) { Value = alertTime },
        new SqlParameter("@AlertNumber", SqlDbType.Int) { Value = alertNumber },
        new SqlParameter("@RecID", SqlDbType.Int) { Value = recID }
    };

            try
            {
                _dbHelper.ExecuteNonQuery(
                    _systemDbConnectionString,
                    command,
                    parameters
                );
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating alert time.", ex);
            }
        }
        #endregion

        #region DailyAlert V2016
        public void UpdateDailyMenu(string menuCode, ref MessageSet message)
        {
            string commandText = @"
        UPDATE dbo.ZYMenuHeader
        SET LastAutoexecutedOn = @CurrentDateTime
        WHERE MenuCode = @MenuCode";

            var parameters = new[]
            {
        new SqlParameter("@CurrentDateTime", SqlDbType.DateTime)
        {
            Value = DateTime.Now
        },
        new SqlParameter("@MenuCode", SqlDbType.NVarChar, 50)
        {
            Value = menuCode?.Trim() ?? ""
        }
    };

            try
            {
                _dbHelper.ExecuteNonQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateDailyMenu",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        public bool CheckDailyMenu(string menuCode, ref MessageSet message)
        {
            bool available = false;

            string commandText = @"
        SELECT MenuCode
        FROM ZYMenuHeader
        WHERE MenuCode = @MenuCode
          AND (LastAutoexecutedOn IS NULL OR CAST(LastAutoexecutedOn AS DATE) < CAST(@CurrentDateTime AS DATE))";

            var parameters = new[]
            {
        new SqlParameter("@CurrentDateTime", SqlDbType.DateTime)
        {
            Value = DateTime.Now
        },
        new SqlParameter("@MenuCode", SqlDbType.NVarChar, 50)
        {
            Value = menuCode?.Trim() ?? ""
        }
    };

            try
            {
                DataTable result = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );

                available = result.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "CheckDailyMenu",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return available;
        }
        #endregion


        #region V2017 - Notification(s)
        public int getNewNotification(string userName, string businessUnit, int executionType, ref MessageSet message)
        {
            string commandText = "";

            if (executionType == 1)
            {
                commandText = @"
            SELECT COUNT(*) AS Notification 
            FROM RD.UserNotifications 
            WHERE BusinessUnit = @BusinessUnit 
              AND UserName = @UserName 
              AND Status = '1'";
            }
            else if (executionType == 2)
            {
                commandText = @"
            SELECT COUNT(*) AS Notification 
            FROM RD.UserNotifications 
            WHERE BusinessUnit = @BusinessUnit 
              AND UserName = @UserName 
              AND Status = '2'";
            }

            var parameters = new[]
            {
        new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
        {
            Value = businessUnit?.Trim() ?? ""
        },
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        }
    };

            try
            {
                DataTable dt = _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );

                if (dt.Rows.Count > 0 && dt != null)
                {
                    return int.Parse(dt.Rows[0]["Notification"].ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "getNewNotification",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return 0;
        }
        public DataTable getUserNotification(string userName, string businessUnit, ref MessageSet message)
        {
            string commandText = @"
        SELECT RecID, Date, UserName, SenderID, Message, TaskCode, Description, Status, Type
        FROM RD.UserNotifications
        WHERE BusinessUnit = @BusinessUnit
          AND UserName = @UserName
          AND Status <> '4'";

            var parameters = new[]
            {
        new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
        {
            Value = businessUnit?.Trim() ?? ""
        },
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        }
    };

            try
            {
                return _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "getUserNotification",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
                return new DataTable();
            }
        }
        public void UpdateNotification(string businessUnit, string userName, int recId, int executionType, ref MessageSet message)
        {
            string commandText = "";
            SqlParameter[] parameters;

            if (executionType == 1)
            {
                commandText = @"
            UPDATE [RD].[UserNotifications] 
            SET Status = '2' 
            WHERE BusinessUnit = @BusinessUnit 
              AND UserName = @UserName 
              AND Status = '1'";

                parameters = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
            {
                Value = businessUnit?.Trim() ?? ""
            },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userName?.Trim() ?? ""
            }
        };
            }
            else if (executionType == 2)
            {
                commandText = @"
            UPDATE [RD].[UserNotifications] 
            SET Status = '3' 
            WHERE BusinessUnit = @BusinessUnit 
              AND UserName = @UserName 
              AND RecID = @RecID";

                parameters = new[]
                {
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
            {
                Value = businessUnit?.Trim() ?? ""
            },
            new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
            {
                Value = userName?.Trim() ?? ""
            },
            new SqlParameter("@RecID", SqlDbType.Int)
            {
                Value = recId
            }
        };
            }
            else
            {
                return;
            }

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _userDbConnectionString,
                        commandText,
                        parameters
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateNotification",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        public void DeleteNotifications(string userName, int[] notifications, ref MessageSet message)
        {
            if (notifications == null || notifications.Length == 0)
                return;

            string commandText = @"
        UPDATE [RD].[UserNotifications] 
        SET Status = '4' 
        WHERE UserName = @UserName 
          AND RecID IN ";

            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
        {
            Value = userName?.Trim() ?? ""
        }
    };

            // Build dynamic IN clause safely
            for (int i = 0; i < notifications.Length; i++)
            {
                string paramName = "@RecID" + i;
                commandText += paramName + ",";
                parameters.Add(new SqlParameter(paramName, SqlDbType.Int)
                {
                    Value = notifications[i]
                });
            }

            commandText = commandText.TrimEnd(',') + ")";

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbHelper.ExecuteNonQuery(
                        _userDbConnectionString,
                        commandText,
                        parameters.ToArray()
                    );
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "DeleteNotifications",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }
        public UserTask UserNotificationTask(string taskCode, string userName, ref MessageSet message)
        {
            var userTask = new UserTask();
            string commandText = @"
        SELECT TOP 1 ZYTask.TaskCode, ZYTask.Caption, ZYTask.Description, ZYTask.ExecutionScript, ZYTask.Icon,
               ISNULL(ZYTask.TaskType, '') AS TaskType,
               ExecutionScript AS url
        FROM ZYTask
        WHERE ZYTask.TaskCode = @TaskCode";

            var parameters = new[]
            {
        new SqlParameter("@TaskCode", SqlDbType.NVarChar, 50)
        {
            Value = taskCode?.Trim() ?? ""
        }
    };

            try
            {
                DataTable dt = _dbHelper.ExecuteQuery(
                    _systemDbConnectionString,
                    commandText,
                    parameters
                );

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    userTask = new UserTask
                    {
                        TaskCode = row["TaskCode"]?.ToString() ?? "",
                        Caption = row["Caption"]?.ToString() ?? "",
                        Description = row["Description"]?.ToString() ?? "",
                        ExecutionScript = row["ExecutionScript"]?.ToString() ?? "",
                        Icon = row["Icon"]?.ToString()?.Trim() ?? "",
                        TaskType = row["TaskType"]?.ToString() ?? "",
                        UserName = userName?.Trim() ?? "",
                        url = row["url"]?.ToString()?.Trim() ?? ""
                    };
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UserNotificationTask",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return userTask;
        }

        #endregion

        #region V2018
        public bool CheckUserNotificationTask(string taskCode, string businessUnit, ref MessageSet message)
        {
            bool available = false;

            string commandText = @"
        SELECT * 
        FROM RD.UserNotificationsTask  
        WHERE SourceTaskCode = @SourceTaskCode
          AND Status = '1' 
          AND BusinessUnit = @BusinessUnit";

            var parameters = new[]
            {
        new SqlParameter("@SourceTaskCode", SqlDbType.NVarChar, 50)
        {
            Value = taskCode?.Trim() ?? ""
        },
        new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
        {
            Value = businessUnit?.Trim() ?? ""
        }
    };

            try
            {
                DataTable result = _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );

                available = result.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "CheckUserNotificationTask",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }

            return available;
        }
        public void UpdateUserNotification(string businessUnit, string userName, string taskCode, ref MessageSet msg)
        {
            try
            {
                string commandText = @"
            SELECT UserName, TaskCode, Description 
            FROM RD.UserNotificationsTask  
            WHERE SourceTaskCode = @SourceTaskCode
              AND Status = '1' 
              AND BusinessUnit = @BusinessUnit";

                var parameters = new[]
                {
            new SqlParameter("@SourceTaskCode", SqlDbType.NVarChar, 50)
            {
                Value = taskCode?.Trim() ?? ""
            },
            new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
            {
                Value = businessUnit?.Trim() ?? ""
            }
        };

                DataTable userNotificationTask = _dbHelper.ExecuteQuery(
                    _userDbConnectionString,
                    commandText,
                    parameters
                );

                if (userNotificationTask.Rows.Count == 1)
                {
                    foreach (DataRow row in userNotificationTask.Rows)
                    {
                        string taskUserName = row["UserName"]?.ToString().Trim() ?? "";
                        string taskTaskCode = row["TaskCode"]?.ToString().Trim() ?? "";
                        string description = row["Description"]?.ToString().Trim() ?? "";

                        string insertCommand = @"
                    INSERT INTO [RD].[UserNotifications] 
                        ([BusinessUnit], [Date], [UserName], [SenderID], [Type], [TaskCode], [Description], [Status], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn])
                    VALUES 
                        (@BusinessUnit, @Date, @UserName, @SenderID, 'T', @TaskCode, @Description, '1', @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn)";

                        var insertParameters = new[]
                        {
                    new SqlParameter("@BusinessUnit", SqlDbType.NVarChar, 50)
                    {
                        Value = businessUnit?.Trim() ?? ""
                    },
                    new SqlParameter("@Date", SqlDbType.DateTime)
                    {
                        Value = DateTime.Now
                    },
                    new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                    {
                        Value = taskUserName
                    },
                    new SqlParameter("@SenderID", SqlDbType.NVarChar, 50)
                    {
                        Value = userName?.Trim() ?? ""
                    },
                    new SqlParameter("@TaskCode", SqlDbType.NVarChar, 50)
                    {
                        Value = taskTaskCode
                    },
                    new SqlParameter("@Description", SqlDbType.NVarChar, -1)
                    {
                        Value = description
                    },
                    new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50)
                    {
                        Value = userName?.Trim() ?? ""
                    },
                    new SqlParameter("@CreatedOn", SqlDbType.DateTime)
                    {
                        Value = DateTime.Now
                    },
                    new SqlParameter("@UpdatedBy", SqlDbType.NVarChar, 50)
                    {
                        Value = userName?.Trim() ?? ""
                    },
                    new SqlParameter("@UpdatedOn", SqlDbType.DateTime)
                    {
                        Value = DateTime.Now
                    }
                };

                        using (var ts = new TransactionScope(TransactionScopeOption.Required))
                        {
                            _dbHelper.ExecuteNonQuery(
                                _userDbConnectionString,
                                insertCommand,
                                insertParameters
                            );

                            ts.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(
                    0,
                    ex,
                    "UpdateUserNotification",
                    "XONT.Ventura.AppConsole.DAL.dll"
                );
                Console.WriteLine(ex);
            }
        }



        //V2053 Add Start
        public DataTable GetAuthorizedTaskURLs(string userName, ref MessageSet message)
        {
            try
            {
                string spName = "[dbo].[usp_GetAuthorizedTaskURLs]";

                var parameters = new[]
                {
                new SqlParameter("@UserName", SqlDbType.NVarChar, 50)
                { Value = (object)userName ?? DBNull.Value }
                };

                return _dbHelper.ExecuteStoredProcedure(
                    _systemDbConnectionString,
                    spName,
                    parameters
                );

            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetAuthorizedTaskURLs", "XONT.Ventura.AppConsole.DAL.dll");
                return new DataTable();
            }
        }
        //V2053 Add End


        #endregion

    }
}