using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;
using System.Globalization;
using Microsoft.VisualBasic;
using XONT.Common.Data;
using XONT.Common.Message;
using XONT.Ventura.AppConsole.Domain;
using XONT.VENTURA.V2MNT21;
using System.Text;
using System.Linq;

namespace XONT.Ventura.AppConsole
{
    public class UserDAL
    {
        private readonly CommonDBService _dbServise;
        private readonly User user;
        private ParameterSet _common;
        private DataTable _dTable;

        #region Public Methods

        #region Common Calling Functions

        public UserDAL()
        {
            _dbServise = new CommonDBService();
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
                string commandText = "";
                commandText += "select LicenseAlertMailIDs,LastLicenseAlertSentDate from dbo.ZYBusinessUnit";
                commandText += " WHERE BusinessUnit = '" + businessUnit.Trim() + "' ";

                _dbServise.StartService();
                dtBU = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetLisenceAltertInfo", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }
            finally
            {
                _dbServise.CloseService();
            }
            return dtBU;
        }

        public bool UpdateLisenceAlertSentDate(string businessUnit, out MessageSet msg)
        {
            bool isOK = false;
            msg = null;

            try
            {
                string commandText = $"update dbo.ZYBusinessUnit set LastLicenseAlertSentDate = '{DateTime.Today.ToString("yyyyMMdd")}'";
                commandText += " WHERE BusinessUnit = '" + businessUnit.Trim() + "' ";

                _dbServise.StartService();
                isOK = _dbServise.ExcecuteWithReturn(CommonVar.DBConName.SystemDB, commandText) > 0;
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "UpdateLisenceAlertSentDate", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }
            finally
            {
                _dbServise.CloseService();
            }
            return isOK;
        }
        //V2044Adding end



        public string GetBusinessUnitName(string businessUnit, ref MessageSet msg)
        {
            string businessUnitName;
            DataTable bunit = new DataTable();
            try
            {
                string commandText = "";
                commandText += "SELECT DISTINCT BusinessUnitName ";
                commandText += "FROM ZYBusinessUnit INNER JOIN ZYUserBusUnit ";
                commandText += "ON ZYBusinessUnit.BusinessUnit = ZYUserBusUnit.BusinessUnit ";
                commandText += "WHERE ZYUserBusUnit.BusinessUnit = '" + businessUnit.Trim() + "' ";

                _dbServise.StartService();
                bunit = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "Get buint", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }
            finally
            {
                _dbServise.CloseService();
            }
            businessUnitName = bunit.Rows[0]["BusinessUnitName"].ToString().Trim();
            return businessUnitName;
        }

        //End

        #region VR007
        public DataTable GetAllUsers(ref MessageSet msg)
        {

            string commandText = "SELECT *  ";
            commandText = commandText + " FROM dbo.ZYUser ";
            commandText = commandText + " where UserLevelGroup <> 'OWNER' "; //V2047

            _dbServise.StartService();
            DataTable dataTable = new DataTable("All_Users");
            dataTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);
            _dbServise.CloseService();

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
                string commandText = "SELECT UserName FROM ZYUser  WHERE UserName ='" + userName.Trim() + "'";
                commandText += " and UserLevelGroup <> 'OWNER'"; //V2047
                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);
                _dbServise.CloseService();

                if (_dTable.Rows.Count > 0)
                {
                    available = true;
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "CheckUserAvailable",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            return available;
        }

        public bool CheckNoOfAttemptsExceeded(string userName, ref MessageSet message)
        {
            int exceedAttempts = -1;
            bool isExceeded = false;
            try
            {

                ParameterSet common = new ParameterSet();
                List<SPParameter> spParametersList = new List<SPParameter>();

                common.SetSPParameterList(spParametersList, "UserName", userName.Trim(), "");

                _dbServise.StartService();
                DataTable dtResults = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, "usp_CheckNoOfAttemptsExceeded", spParametersList);
                _dbServise.CloseService();

                if (dtResults != null)
                {
                    if (dtResults.Rows.Count > 0)
                        exceedAttempts = dtResults.Rows[0].Field<int>("Attempt");
                    if (exceedAttempts >= 0) isExceeded = true;
                }

            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "CheckNoOfAttemptsExceeded",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                // _dbServise.CloseService();
            }
            return isExceeded;
        }
        //VR011 add end

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
                string commandText = "SELECT     ISNULL(ZYObjectLock.SessionID,'') as SessionID " +
                                     " FROM         ZYObjectLock " +
                                     " WHERE (ZYObjectLock.BusinessUnit = '" + businessUnit + "' ) AND ZYObjectLock.StatusFlag='1' ";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                foreach (DataRow dtRow in _dTable.Rows)
                {
                    sessionList.Add(dtRow["SessionID"].ToString());
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetObjectLockSessionsList", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            return sessionList;
        }
        //VR025

        //V2023Start
        public void RestorePagingToggle(string BusinessUnit, string TaskCode, string AllowPaging, out MessageSet msg)
        {
            msg = null;
            try
            {
                string updateCommand = $"update RD.ComponentMasterControl set AllowPaging = '{ AllowPaging}'";
                updateCommand += $" where BusinessUnit ='{BusinessUnit}' and ComponentID = '{TaskCode}'";

                _dbServise.StartService();
                _dbServise.Excecute(CommonVar.DBConName.UserDB, updateCommand);
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "RestorePagingToggle", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

        }

        //V2023End
        #region Get User Menu/Task

        public List<UserMenu> GetUserManu(string userName, string roleCode, ref MessageSet message)
        {
            var userMenus = new List<UserMenu>();
            try
            {
                string commandText = "SELECT ZYRoleMenu.MenuCode , ZYMenuHeader.Description ,ZYMenuHeader.Icon " +
                                     " FROM ZYRoleMenu INNER JOIN " +
                                     "   ZYUserRole ON ZYRoleMenu.RoleCode = ZYUserRole.RoleCode INNER JOIN " +
                                     "  ZYMenuHeader ON ZYRoleMenu.MenuCode = ZYMenuHeader.MenuCode " +
                                     " where ZYUserRole.UserName='" + userName + "' and ZYRoleMenu.RoleCode ='" +
                                     roleCode + "'  ORDER BY ZYRoleMenu.Sequence";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);
                _dbServise.CloseService();

                foreach (DataRow dtRow in _dTable.Rows)
                {
                    userMenus.Add(new UserMenu
                    {
                        MenuCode =
                                              !string.IsNullOrEmpty(dtRow["MenuCode"].ToString())
                                                  ? dtRow["MenuCode"].ToString()
                                                  : "",
                        Description = dtRow["Description"].ToString(),
                        Icon = dtRow["Icon"].ToString().Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get User Menu Data",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }

            return userMenus;
        }

        public List<UserTask> GetUserTask(string menuCode, ref MessageSet message)
        {
            var userTask = new List<UserTask>();
            try
            {
                string commandText =
                    "SELECT ZYTask.TaskCode , ZYTask.Caption, ZYTask.Description,ZYTask.ExecutionScript,ZYTask.Icon " +
                    " FROM ZYTask INNER JOIN " +
                    "   ZYMenuDetail ON ZYTask.TaskCode = ZYMenuDetail.TaskCode " +
                    " where ZYMenuDetail.MenuCode ='" + menuCode + "' ORDER BY ZYMenuDetail.Sequence";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);

                var dllsInSiteBin = AppDomain.CurrentDomain.GetAssemblies().ToList();//V2038Added

                foreach (DataRow dtRow in _dTable.Rows)
                {
                    //V2038 Adding start
                    string version = "0.0.0.0";
                    string curTskCode = !string.IsNullOrEmpty(dtRow["TaskCode"].ToString().Trim()) ? dtRow["TaskCode"].ToString().Trim() : "";
                    string executionScript = dtRow["ExecutionScript"].ToString();
                    bool isV2Component = executionScript.Contains(".aspx");
                    if (!isV2Component)
                    {
                        var assemplyVersion = dllsInSiteBin.FindAll(a => a.FullName.Contains(curTskCode));
                        if (assemplyVersion.Count > 0)
                        {
                            version = assemplyVersion[0].GetName().Version.ToString();
                            executionScript = executionScript + "?v=" + version;
                        }
                    }
                    //V2038 Adding end

                    userTask.Add(new UserTask
                    {
                        TaskCode =
                                             !string.IsNullOrEmpty(dtRow["TaskCode"].ToString())
                                                 ? dtRow["TaskCode"].ToString()
                                                 : "",
                        Description = dtRow["Description"].ToString(),
                        Caption = dtRow["Caption"].ToString(),
                        //ExecutionScript = dtRow["ExecutionScript"].ToString(),  // V2038Removed
                        ExecutionScript = executionScript,  // V2038 Added

                        Icon = dtRow["Icon"].ToString().Trim()

                    });
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get User Task Data",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

            return userTask;
        }

        public List<UserTask> GetUserTask(string menuCode, string userName, ref MessageSet message)//VR019
        {
            var userTask = new List<UserTask>();
            try
            {
                string commandText =
                    "SELECT ZYTask.TaskCode ,ZYTask.Caption ,  ZYTask.Description,ZYTask.ExecutionScript,ZYTask.Icon " +
                    ",ISNULL(ZYTask.TaskType ,'') as TaskType " +  //VR019
                    ",ISNULL(ZYTask.ExclusivityMode ,'') as ExclusivityMode " +  //V2049
                    ",ISNULL(ZYTask.ApplicationCode ,'') as ApplicationCode " +  //V2049
                    " FROM ZYTask INNER JOIN " +
                    "   ZYMenuDetail ON ZYTask.TaskCode = ZYMenuDetail.TaskCode " +
                    " where ZYMenuDetail.MenuCode ='" + menuCode + "' ORDER BY ZYMenuDetail.Sequence";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);

                var dllsInSiteBin = AppDomain.CurrentDomain.GetAssemblies().ToList();//V2038Added

                foreach (DataRow dtRow in _dTable.Rows)
                {
                    //V2038 Adding start
                    string version = "0.0.0.0";
                    string curTskCode = !string.IsNullOrEmpty(dtRow["TaskCode"].ToString().Trim()) ? dtRow["TaskCode"].ToString().Trim() : "";
                    string executionScript = dtRow["ExecutionScript"].ToString();
                    bool isV2Component = executionScript.Contains(".aspx");
                    if (!isV2Component)
                    {
                        var assemplyVersion = dllsInSiteBin.FindAll(a => a.FullName.Contains(curTskCode));
                        if (assemplyVersion.Count > 0)
                        {
                            version = assemplyVersion[0].GetName().Version.ToString();
                            executionScript = executionScript + "?v=" + version;
                        }
                    }
                    //V2038 Adding end

                    userTask.Add(new UserTask
                    {
                        TaskCode =
                            !string.IsNullOrEmpty(dtRow["TaskCode"].ToString())
                                ? dtRow["TaskCode"].ToString()
                                : "",
                        Description = dtRow["Description"].ToString(),
                        Caption = dtRow["Caption"].ToString(),
                        //ExecutionScript = dtRow["ExecutionScript"].ToString(),  // V2038Removed
                        ExecutionScript = executionScript,  // V2038 Added
                        Icon = dtRow["Icon"].ToString().Trim()
                        ,
                        TaskType = !string.IsNullOrEmpty(dtRow["TaskType"].ToString()) ? dtRow["TaskType"].ToString() : ""//VR019
                        ,
                        UserName = userName.Trim(),
                        ExclusivityMode = dtRow["ExclusivityMode"].ToString().Trim(),//V2049
                        ApplicationCode = dtRow["ApplicationCode"].ToString().Trim() //V2049
                    });
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get User Task Data",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

            return userTask;
        }

        #endregion


        //VR003 Begin
        public void LogTaskInfo(ActiveUserTask userActiveTask, ref MessageSet message)
        {
            string commandText = string.Empty;
            try
            {

                ParameterSet common = new ParameterSet();
                List<SPParameter> spParametersList = new List<SPParameter>();


                common.SetSPParameterList(spParametersList, "SessionID", userActiveTask.SessionID, "");
                common.SetSPParameterList(spParametersList, "TaskCode", userActiveTask.TaskCode, "");

                common.SetSPParameterList(spParametersList, "UserName", userActiveTask.UserName, "");
                common.SetSPParameterList(spParametersList, "BusinessUnit", userActiveTask.BusinessUnit, "");
                common.SetSPParameterList(spParametersList, "EndDateTime", userActiveTask.EndDateTime, "");
                common.SetSPParameterList(spParametersList, "Status", userActiveTask.Status, "");
                common.SetSPParameterList(spParametersList, "ExecutionType", userActiveTask.ExecutionType, "");

                _dbServise.StartService();
                _dbServise.Excecute(CommonVar.DBConName.SystemDB, "usp_UpdateCurrentUserTask", spParametersList);
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "LogTaskInfo",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        //VR003 End

        //V2049 S
        public int GetActiveTaskCount(string businessUnit, string taskCode, string territoryCode, string sessionID, ref MessageSet message)
        {
            DataTable dtResult = new DataTable();
            int activeTaskCount = 0;
            try
            {
                ParameterSet common = new ParameterSet();
                List<SPParameter> spParametersList = new List<SPParameter>();

                common.SetSPParameterList(spParametersList, "BusinessUnit", businessUnit, "");
                common.SetSPParameterList(spParametersList, "TaskCode", taskCode, "");
                common.SetSPParameterList(spParametersList, "TerritoryCode", territoryCode, "");
                common.SetSPParameterList(spParametersList, "SessionID", sessionID, "");
                common.SetSPParameterList(spParametersList, "ExecutionType", "5", "");

                _dbServise.StartService();
                dtResult = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, "[dbo].[usp_GetUserData]", spParametersList);

                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    activeTaskCount = Convert.ToInt32(dtResult.Rows[0]["ActiveTaskCount"].ToString());
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetActiveTaskCount", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

            return activeTaskCount;
        }

        public DataTable GetActiveTasks(string businessUnit, string userName, string taskCode, string territoryCode, string sessionID, ref MessageSet message)
        {
            DataTable dtResult = new DataTable();
            try
            {
                ParameterSet common = new ParameterSet();
                List<SPParameter> spParametersList = new List<SPParameter>();

                common.SetSPParameterList(spParametersList, "BusinessUnit", businessUnit, "");
                common.SetSPParameterList(spParametersList, "UserName", userName, "");
                common.SetSPParameterList(spParametersList, "TaskCode", taskCode, "");
                common.SetSPParameterList(spParametersList, "TerritoryCode", territoryCode, "");
                common.SetSPParameterList(spParametersList, "SessionID", sessionID, "");
                common.SetSPParameterList(spParametersList, "ExecutionType", "6", "");

                _dbServise.StartService();
                dtResult = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, "[dbo].[usp_GetUserData]", spParametersList);


            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetActiveTasks", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

            return dtResult;
        }

        public DataTable GetUserTerritory(string businessUnit, string userName, ref MessageSet message)
        {
            DataTable dtResult = new DataTable();
            try
            {
                ParameterSet common = new ParameterSet();
                List<SPParameter> spParametersList = new List<SPParameter>();

                common.SetSPParameterList(spParametersList, "BusinessUnit", businessUnit, "");
                common.SetSPParameterList(spParametersList, "UserName", userName, "");
                common.SetSPParameterList(spParametersList, "ExecutionType", "1", "");

                _dbServise.StartService();
                dtResult = _dbServise.FillDataTable(CommonVar.DBConName.UserDB, "[RD].[usp_AppConsoleGetData]", spParametersList);
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetUserTerritory", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

            return dtResult;
        }

        public List<string> GetTaskLockSessionsList(string businessUnit, ref MessageSet message)
        {
            List<string> sessionList = new List<string>();
            DataTable dtResult = new DataTable();
            try
            {
                ParameterSet common = new ParameterSet();
                List<SPParameter> spParametersList = new List<SPParameter>();

                common.SetSPParameterList(spParametersList, "BusinessUnit", businessUnit, "");
                common.SetSPParameterList(spParametersList, "ExecutionType", "7", "");

                _dbServise.StartService();
                dtResult = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, "[dbo].[usp_GetUserData]", spParametersList);

                foreach (DataRow dtRow in dtResult.Rows)
                {
                    sessionList.Add(dtRow["SessionID"].ToString());
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetTaskLockSessionsList", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            return sessionList;
        }

        public bool UpdateActiveTask(ActiveUserTask userActiveTask, string executionType, ref MessageSet message)
        {
            try
            {
                ParameterSet common = new ParameterSet();
                List<SPParameter> spParametersList = new List<SPParameter>();

                common.SetSPParameterList(spParametersList, "BusinessUnit", userActiveTask.BusinessUnit, "");
                common.SetSPParameterList(spParametersList, "TaskCode", userActiveTask.TaskCode, "");
                common.SetSPParameterList(spParametersList, "SessionID", userActiveTask.SessionID, "");
                common.SetSPParameterList(spParametersList, "ExecutionType", executionType, "");

                if (executionType == "1")
                {
                    common.SetSPParameterList(spParametersList, "UserName", userActiveTask.UserName, "");
                    common.SetSPParameterList(spParametersList, "ApplicationCode", userActiveTask.ApplicationCode, "");
                    common.SetSPParameterList(spParametersList, "ExclusivityMode", userActiveTask.ExclusivityMode, "");
                    common.SetSPParameterList(spParametersList, "WorkstationID", userActiveTask.WorkstationID, "");
                    common.SetSPParameterList(spParametersList, "StatusFlag", userActiveTask.StatusFlag, "");
                    common.SetSPParameterList(spParametersList, "TerritoryCode", userActiveTask.TerritoryCode, "");
                }

                _dbServise.StartService();
                if (_dbServise.ExcecuteWithReturn(CommonVar.DBConName.SystemDB, "[dbo].[usp_UpdateZYActiveTask]", spParametersList) <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "UpdateActiveTask", "XONT.Ventura.AppConsole.DAL.dll");
                return false;
            }
            finally
            {
                _dbServise.CloseService();
            }

            return true;
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
                string commandText =
                    "SELECT [BusinessUnit],[BusinessUnitName],[AddressLine1],[AddressLine2],[AddressLine3] " +
                    " ,[AddressLine4] ,[AddressLine5],[PostCode],[TelephoneNumber],[FaxNumber] " +
                    "   ,[EmailAddress],[WebAddress],[VATRegistrationNumber],[Logo], [WebAddress] " +//V2032Added [WebAddress]
                    "  FROM [dbo].[ZYBusinessUnit]  WHERE BusinessUnit ='" + businessUnit.Trim() + "'";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);
                _dbServise.CloseService();

                foreach (DataRow dtRow in _dTable.Rows)
                {
                    objBusinessUnit.BusinessUnitCode = dtRow["BusinessUnit"].ToString();
                    objBusinessUnit.BusinessUnitName = !string.IsNullOrEmpty(dtRow["BusinessUnitName"].ToString())
                                                           ? dtRow["BusinessUnitName"].ToString()
                                                           : "";
                    objBusinessUnit.AddressLine1 = !string.IsNullOrEmpty(dtRow["AddressLine1"].ToString())
                                                       ? dtRow["AddressLine1"].ToString()
                                                       : "";
                    objBusinessUnit.AddressLine2 = !string.IsNullOrEmpty(dtRow["AddressLine2"].ToString())
                                                       ? dtRow["AddressLine2"].ToString()
                                                       : "";
                    objBusinessUnit.AddressLine3 = !string.IsNullOrEmpty(dtRow["AddressLine3"].ToString())
                                                       ? dtRow["AddressLine3"].ToString()
                                                       : "";
                    objBusinessUnit.AddressLine4 = !string.IsNullOrEmpty(dtRow["AddressLine4"].ToString())
                                                       ? dtRow["AddressLine4"].ToString()
                                                       : "";
                    objBusinessUnit.AddressLine5 = !string.IsNullOrEmpty(dtRow["AddressLine5"].ToString())
                                                       ? dtRow["AddressLine5"].ToString()
                                                       : "";
                    objBusinessUnit.PostCode = !string.IsNullOrEmpty(dtRow["PostCode"].ToString())
                                                   ? dtRow["PostCode"].ToString()
                                                   : "";
                    objBusinessUnit.TelephoneNumber = !string.IsNullOrEmpty(dtRow["TelephoneNumber"].ToString())
                                                          ? dtRow["TelephoneNumber"].ToString()
                                                          : "";
                    objBusinessUnit.FaxNumber = !string.IsNullOrEmpty(dtRow["FaxNumber"].ToString())
                                                    ? dtRow["FaxNumber"].ToString()
                                                    : "";
                    objBusinessUnit.EmailAddress = !string.IsNullOrEmpty(dtRow["EmailAddress"].ToString())
                                                       ? dtRow["EmailAddress"].ToString()
                                                       : "";
                    objBusinessUnit.VATRegistrationNumber =
                        !string.IsNullOrEmpty(dtRow["VATRegistrationNumber"].ToString())
                            ? dtRow["VATRegistrationNumber"].ToString()
                            : "";
                    objBusinessUnit.Logo = !string.IsNullOrEmpty(dtRow["Logo"].ToString())
                                               ? dtRow["Logo"].ToString()
                                               : "";
                    objBusinessUnit.WebAddress = dtRow["WebAddress"].ToString().Trim();//V2032Added
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get User Menu Data",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
        }

        //VR002 Begin
        private void GetUserBusinessUnit(string businessUnit, ref BusinessUnit objBusinessUnit, string distributorCode, ref MessageSet message)
        {
            try
            {
                string businessUnitSQL = "SELECT [VATRegistrationNumber],[Logo] " +
                    "  FROM [dbo].[ZYBusinessUnit]  WHERE BusinessUnit ='" + businessUnit.Trim() + "'";

                _dbServise.StartService();

                DataTable dtBusinessUnit = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, businessUnitSQL);
                //Set the Logo
                if (dtBusinessUnit.Rows.Count > 0)
                {
                    objBusinessUnit.Logo = (dtBusinessUnit.Rows[0]["Logo"] != DBNull.Value) ? dtBusinessUnit.Rows[0]["Logo"].ToString() : "";
                }

                //Get Dist Info
                string distInfoSQL = "SELECT RD.Distributor.BusinessUnit, RD.Distributor.DistributorCode, RD.Distributor.DistributorName, ";
                distInfoSQL += " RD.Distributor.AddressLine1, RD.Distributor.AddressLine2, RD.Distributor.AddressLine3, RD.Distributor.AddressLine4, RD.Distributor.AddressLine5, ";
                distInfoSQL += " RD.Distributor.PostCode, RD.Distributor.TelephoneNumber, RD.Distributor.FaxNumber, RD.Distributor.EMailAddress, RD.Distributor.WebAddress, ";
                distInfoSQL += " VATRegistrationNo";

                distInfoSQL += " FROM RD.Distributor ";

                distInfoSQL += " WHERE (RD.Distributor.BusinessUnit = '" + businessUnit + "')  ";
                distInfoSQL += " AND (RD.Distributor.DistributorCode = '" + distributorCode + "')";
                distInfoSQL += " AND (RD.Distributor.Status = '1')  ";

                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.UserDB, distInfoSQL);
                _dbServise.CloseService();

                if (_dTable.Rows.Count > 0)
                {
                    DataRow dtRow = _dTable.Rows[0];
                    objBusinessUnit.BusinessUnitCode = dtRow["BusinessUnit"].ToString();
                    objBusinessUnit.BusinessUnitName = (dtRow["DistributorName"] != DBNull.Value) ? dtRow["DistributorName"].ToString().Trim() : "";
                    objBusinessUnit.AddressLine1 = (dtRow["AddressLine1"] != DBNull.Value) ? dtRow["AddressLine1"].ToString().Trim() : "";
                    objBusinessUnit.AddressLine2 = (dtRow["AddressLine2"] != DBNull.Value) ? dtRow["AddressLine2"].ToString().Trim() : "";
                    objBusinessUnit.AddressLine3 = (dtRow["AddressLine3"] != DBNull.Value) ? dtRow["AddressLine3"].ToString().Trim() : "";

                    #region V2006
                    //objBusinessUnit.AddressLine3 = (dtRow["AddressLine3"] != DBNull.Value) ? dtRow["AddressLine4"].ToString().Trim() : "";
                    //objBusinessUnit.AddressLine3 = (dtRow["AddressLine3"] != DBNull.Value) ? dtRow["AddressLine5"].ToString().Trim() : "";
                    #endregion

                    objBusinessUnit.AddressLine4 = (dtRow["AddressLine4"] != DBNull.Value) ? dtRow["AddressLine4"].ToString().Trim() : ""; //V2006
                    objBusinessUnit.AddressLine5 = (dtRow["AddressLine5"] != DBNull.Value) ? dtRow["AddressLine5"].ToString().Trim() : ""; //V2006

                    objBusinessUnit.PostCode = (dtRow["PostCode"] != DBNull.Value) ? dtRow["PostCode"].ToString() : "";
                    objBusinessUnit.TelephoneNumber = (dtRow["TelephoneNumber"] != DBNull.Value) ? dtRow["TelephoneNumber"].ToString() : "";
                    objBusinessUnit.FaxNumber = (dtRow["FaxNumber"] != DBNull.Value) ? dtRow["FaxNumber"].ToString() : "";
                    objBusinessUnit.EmailAddress = (dtRow["EMailAddress"] != DBNull.Value) ? dtRow["EMailAddress"].ToString() : "";
                    objBusinessUnit.EmailAddress = (dtRow["EMailAddress"] != DBNull.Value) ? dtRow["EMailAddress"].ToString() : "";
                    objBusinessUnit.VATRegistrationNumber = (dtRow["VATRegistrationNo"] != DBNull.Value) ? dtRow["VATRegistrationNo"].ToString() : "";
                    objBusinessUnit.WebAddress = dtRow["WebAddress"].ToString().Trim();//V2032Added
                }
                else
                {
                    //Dist Infor Not found. Replace with Busines Unit info.
                    DataRow dtRow = _dTable.Rows[0];
                    objBusinessUnit.BusinessUnitCode = dtRow["BusinessUnit"].ToString();
                    objBusinessUnit.BusinessUnitName = !string.IsNullOrEmpty(dtRow["BusinessUnitName"].ToString())
                                                           ? dtRow["BusinessUnitName"].ToString()
                                                           : "";
                    objBusinessUnit.AddressLine1 = !string.IsNullOrEmpty(dtRow["AddressLine1"].ToString())
                                                       ? dtRow["AddressLine1"].ToString()
                                                       : "";
                    objBusinessUnit.AddressLine2 = !string.IsNullOrEmpty(dtRow["AddressLine2"].ToString())
                                                       ? dtRow["AddressLine2"].ToString()
                                                       : "";
                    objBusinessUnit.AddressLine3 = !string.IsNullOrEmpty(dtRow["AddressLine3"].ToString())
                                                       ? dtRow["AddressLine3"].ToString()
                                                       : "";
                    objBusinessUnit.AddressLine4 = !string.IsNullOrEmpty(dtRow["AddressLine4"].ToString())
                                                       ? dtRow["AddressLine4"].ToString()
                                                       : "";
                    objBusinessUnit.AddressLine5 = !string.IsNullOrEmpty(dtRow["AddressLine5"].ToString())
                                                       ? dtRow["AddressLine5"].ToString()
                                                       : "";
                    objBusinessUnit.PostCode = !string.IsNullOrEmpty(dtRow["PostCode"].ToString())
                                                   ? dtRow["PostCode"].ToString()
                                                   : "";
                    objBusinessUnit.TelephoneNumber = !string.IsNullOrEmpty(dtRow["TelephoneNumber"].ToString())
                                                          ? dtRow["TelephoneNumber"].ToString()
                                                          : "";
                    objBusinessUnit.FaxNumber = !string.IsNullOrEmpty(dtRow["FaxNumber"].ToString())
                                                    ? dtRow["FaxNumber"].ToString()
                                                    : "";
                    objBusinessUnit.EmailAddress = !string.IsNullOrEmpty(dtRow["EmailAddress"].ToString())
                                                       ? dtRow["EmailAddress"].ToString()
                                                       : "";
                    objBusinessUnit.VATRegistrationNumber =
                        !string.IsNullOrEmpty(dtRow["VATRegistrationNumber"].ToString())
                            ? dtRow["VATRegistrationNumber"].ToString() : "";

                    objBusinessUnit.WebAddress = dtRow["WebAddress"].ToString().Trim();//V2032Added
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetUserBusinessUnit", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }
        //VR002 End

        private bool CheckUserData(string userName, string password, ref MessageSet message)
        {
            bool isExists = false;
            var stroEncript = new StroEncript();
            string encriptPsass = stroEncript.Encript(password.Trim());
            try
            {
                //VR001 START
                //string commandText =
                //    "SELECT ZYUser.UserName,ZYUser.UserFullName,ZYUserBusUnit.BusinessUnit,ZYUser.TimeStamp,ZYUser.UserLevelGroup,ZYUser.Password,ZYUser.ActiveFlag " +
                //    ",ZYUser.PasswordLocked,ZYUser.PowerUser FROM ZYUser " +
                //    "INNER JOIN ZYUserBusUnit ON ZYUser.UserName = ZYUserBusUnit.UserName " +
                //    " where CAST(RTRIM(ZYUser.UserName) as VARBINARY(30))= CAST('" + userName +
                //    "'AS VARBINARY(30)) and ZYUser.Password='" + encriptPsass +
                //    "'";

                string commandText = string.Empty;
                var paramSet = new ParameterSet();
                var spParametersList = new List<SPParameter>();

                paramSet.SetSPParameterList(spParametersList, "UserName", userName, "");
                paramSet.SetSPParameterList(spParametersList, "Password", encriptPsass, "");
                paramSet.SetSPParameterList(spParametersList, "ExecutionType", '1', "");
                paramSet.SetSPParameterList(spParametersList, "DefaultBusinessUnit", '1', "");

                commandText = "[dbo].[usp_GetUserData]";
                //VR001 END
                _dbServise.StartService();
                //_dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);//VR001 DEL
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText, spParametersList); //VR001 ADD

                if (_dTable.Rows.Count > 0)
                {
                    //VR001 START
                    //string activeData = stroEncript.Decript("1", userName.Trim());
                    //if (activeData.Equals("1"))
                    //{
                    //    isExists = true;
                    //}
                    string activeFlag = "";
                    char passwordLocked = Convert.ToChar(_dTable.Rows[0]["PasswordLocked"]);
                    activeFlag = _dTable.Rows[0]["ActiveFlag"].ToString();
                    if (passwordLocked.Equals('0'))
                    {
                        CheckActiveFlagUser(activeFlag, userName, ref message); //Chack User is an Active User
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
                //VR001 END
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Check User Data", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            return isExists;
        }


        private void GetUserMainData(string userName, string password, ref MessageSet message)
        {
            //var stroEncript = new StroEncript();
            //string encriptPsass = stroEncript.Encript(password);
            var encriptPsass = password;////VR029
            try
            {
                string commandText = string.Empty;
                var paramSet = new ParameterSet();
                var spParametersList = new List<SPParameter>();
                string defaultbu = "1";
                paramSet.SetSPParameterList(spParametersList, "UserName", userName, "");
                paramSet.SetSPParameterList(spParametersList, "Password", encriptPsass, "");
                paramSet.SetSPParameterList(spParametersList, "ExecutionType", '1', "");
                paramSet.SetSPParameterList(spParametersList, "DefaultBusinessUnit", defaultbu, "");

                commandText = "[dbo].[usp_GetUserData]";

                //string commandText =
                //    "SELECT ZYUser.UserName,ZYUser.UserFullName,ZYUserBusUnit.BusinessUnit,ZYUser.TimeStamp,ZYUser.UserLevelGroup,ZYUser.Password,ZYUser.ActiveFlag " +
                //    ",ZYUser.PasswordLocked,ZYUser.PowerUser,ZYUser.Theme,ZYUser.Language,ZYUser.FontColor,ZYUser.FontName,ZYUser.FontSize FROM ZYUser " +
                //    "INNER JOIN ZYUserBusUnit ON ZYUser.UserName = ZYUserBusUnit.UserName " +
                //    " where CAST(RTRIM(ZYUser.UserName) as VARBINARY(30))= CAST('" + userName +
                //    "'AS VARBINARY(30)) and ZYUser.Password='" + encriptPsass +
                //    "'";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText, spParametersList);

                //Check User Exists In Table
                if (_dTable.Rows.Count > 0)
                {
                    string activeFlag = "";
                    foreach (DataRow dtRow in _dTable.Rows)
                    {
                        user.isExists = true;
                        user.BusinessUnit = dtRow["BusinessUnit"].ToString();
                        user.UserName = dtRow["UserName"].ToString();
                        //  user.TimeStamp = dtRow["TimeStamp"] as byte[];
                        user.UserFullName = dtRow["UserFullName"].ToString();
                        user.UserLevelGroup = dtRow["UserLevelGroup"].ToString();
                        user.Password = dtRow["Password"].ToString();
                        user.PasswordLocked = Convert.ToChar(dtRow["PasswordLocked"]);
                        activeFlag = dtRow["ActiveFlag"].ToString();
                        user.PowerUser = dtRow["PowerUser"].ToString();
                        user.Theme = dtRow["Theme"].ToString();
                        user.Language = dtRow["Language"].ToString();
                        user.CaptionEditor = dtRow["CaptionEditor"].ToString().Trim() == "1";//V2033Added
                        user.FontColor = dtRow["FontColor"].ToString();
                        user.FontSize = !string.IsNullOrEmpty(dtRow["FontSize"].ToString())
                                            ? Convert.ToInt32(dtRow["FontSize"].ToString())
                                            : 0;
                        user.FontName = dtRow["FontName"].ToString();
                        user.HasProPicture = dtRow["ProPicAvailable"].ToString()[0];//New Ventura
                        //VR002 Begin (Set the Dist code for the user
                        string distributorCode = dtRow["DistributorCode"].ToString();
                        user.DefaultRoleCode = dtRow["RoleCode"].ToString(); //v2014
                        if (!string.IsNullOrEmpty(distributorCode))
                        {
                            user.DistributorCode = distributorCode.Trim();
                        }
                        else
                        {
                            user.DistributorCode = null;
                        }
                        //VR002 End

                        //VR013
                        user.RestrictFOCInvoice = (dtRow["RestrictFOCInvoice"] != null)
                                                      ? dtRow["RestrictFOCInvoice"].ToString()
                                                      : "0";
                        //VR013
                        //VR024
                        string supplierCode = dtRow["SupplierCode"].ToString();
                        if (!string.IsNullOrEmpty(supplierCode))
                        {
                            user.SupplierCode = supplierCode.Trim();
                        }
                        else
                        {
                            user.SupplierCode = "";
                        }
                        string customerCode = dtRow["CustomerCode"].ToString();
                        if (!string.IsNullOrEmpty(customerCode))
                        {
                            user.CustomerCode = customerCode.Trim();
                        }
                        else
                        {
                            user.CustomerCode = "";
                        }
                        //VR024

                        //VR028 Start
                        string executiveCode = dtRow["ExecutiveCode"].ToString();
                        if (!string.IsNullOrEmpty(executiveCode))
                        {
                            user.ExecutiveCode = executiveCode.Trim();
                        }
                        else
                        {
                            user.ExecutiveCode = "";
                        }
                        //VR028 End 
                        user.PasswordChange = dtRow["PasswordChange"].ToString(); //V2004

                        user.POReturnAuthorizationLevel = dtRow["POReturnAuthorizationLevel"].ToString(); //V2042
                        user.POReturnAuthorizationUpTo = dtRow["POReturnAuthorizationUpTo"].ToString();  //V2042
                        user.PUSQQtyEdit = dtRow["PUSQQtyEdit"].ToString(); //V2048
                    }
                    // activeFlag=""
                    if (user.PasswordLocked.Equals('0'))
                    {
                        CheckActiveFlagUser(activeFlag, userName, ref message); //Chack User is an Active User
                        if (user.ActiveFlag)
                        {
                            GetPasswordData(user.BusinessUnit.Trim(), ref message); //Get Password Control Data
                            GetLastLoggingDAte(userName, ref message); //Check User IS Expired
                            if (!user.IsUserExpire && !user.IsPassExpire)
                            {
                                CheckUserAlradyInSession(userName, ref message); //Checkk user Already Login

                                if (user.AlreadyLogin.Equals(string.Empty))
                                {
                                    GetLastPasswordChangeDate(userName, ref message); //Check Password Exired
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
                    //V2004
                    user.PasswordChange = "";
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get User Data", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        private void CheckUserAlradyInSession(string userName, ref MessageSet message)
        {
            //VR008 Begin
            user.AlreadyLogin = "";
            return;
            //VR008 End


            string dateTime = "";
            try
            {
                string commandText =
                    "SELECT     MAX(Date) AS Date, MAX(Time) AS Time, UserName, SuccessfulLogin, LogOutTime" +
                    " FROM         ZYPasswordLoginDetails " +
                    " WHERE     (UserName = '" + userName + "') " +
                    " GROUP BY UserName, SuccessfulLogin, LogOutTime HAVING      (SuccessfulLogin = N'1') AND (LogOutTime IS  NULL)";



                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                if (_dTable.Rows.Count > 0)
                {
                    foreach (DataRow dtRow in _dTable.Rows)
                    {
                        DateTime date = Convert.ToDateTime(dtRow["Date"]);
                        DateTime time = Convert.ToDateTime(dtRow["Time"]);
                        dateTime = (date.ToShortDateString() + " " + time.ToLongTimeString());
                    }
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Check User Alredy In session",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            user.AlreadyLogin = dateTime;

        }

        private void GetUserRoles(string userName, ref MessageSet message)
        {
            var userRoles = new List<UserRole>();
            try
            {
                if (userName.Trim() != "administrator") //VR006
                {
                    string commandText = "SELECT ZYUserRole.RoleCode, ZYRole.Description,ZYRole.Icon " +
                                 " FROM ZYUser " +
                                 "  INNER JOIN ZYUserRole ON ZYUser.UserName = ZYUserRole.UserName " +
                                 "  INNER JOIN  ZYRole ON ZYUserRole.RoleCode = ZYRole.RoleCode " +
                                 "where ZYUser.UserName='" + userName + "' ORDER BY ZYUserRole.Sequence";


                    _dbServise.StartService();
                    _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                    foreach (DataRow dtRow in _dTable.Rows)
                    {
                        userRoles.Add(new UserRole
                        {
                            RoleCode =
                                                  !string.IsNullOrEmpty(dtRow["RoleCode"].ToString())
                                                      ? dtRow["RoleCode"].ToString()
                                                      : "",
                            Description = dtRow["Description"].ToString(),
                            Icon = dtRow["Icon"].ToString().Trim(),
                        });
                    }
                }
                #region VR006
                else
                {
                    userRoles.Add(new UserRole
                    {
                        RoleCode = "05_ROLE",
                        Description = "SYSTEM",
                        Icon = "role1.png",
                    });
                }

                #endregion

                user.UserRoles = userRoles;
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get User Roles", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        public List<UserRole> GetUserRole(string userName, ref MessageSet message)//VR016
        {
            var userRoles = new List<UserRole>();
            try
            {
                if (userName.Trim() != "administrator")
                {
                    string commandText = "SELECT ZYUserRole.RoleCode, ZYRole.Description,ZYRole.Icon " +
                                 " FROM ZYUser " +
                                 "  INNER JOIN ZYUserRole ON ZYUser.UserName = ZYUserRole.UserName " +
                                 "  INNER JOIN  ZYRole ON ZYUserRole.RoleCode = ZYRole.RoleCode " +
                                 "where ZYUser.UserName='" + userName + "' ORDER BY ZYUserRole.Sequence";


                    _dbServise.StartService();
                    _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                    foreach (DataRow dtRow in _dTable.Rows)
                    {
                        userRoles.Add(new UserRole
                        {
                            RoleCode =
                                !string.IsNullOrEmpty(dtRow["RoleCode"].ToString())
                                    ? dtRow["RoleCode"].ToString()
                                    : "",
                            Description = dtRow["Description"].ToString(),
                            Icon = dtRow["Icon"].ToString().Trim(),
                        });
                    }
                }
                #region VR006
                else
                {
                    userRoles.Add(new UserRole
                    {
                        RoleCode = "05_ROLE",
                        Description = "SYSTEM",
                        Icon = "role1.png",
                    });
                }

                #endregion


            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get User Roles", "XONT.Ventura.AppConsole.DAL.dll");
            }

            finally
            {

                _dbServise.CloseService();
            }

            return userRoles;
        }


        public DataTable getUserfavourites(string businessUnit, string userName, ref MessageSet message)//mgd
        {


            DataTable dt = new DataTable();
            string query = "";

            try
            {

                query += "SELECT BookmarkID,Bookmark, TaskPath ";
                query += "FROM RD.Bookmarks ";
                query += "WHERE BusinessUnit = '" + businessUnit + "' AND UserName = '" + userName + "' ";

                _dbServise.StartService();
                dt = _dbServise.FillDataTable(CommonVar.DBConName.UserDB, query);

            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "getUserfavourites", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);

            }
            finally
            {
                _dbServise.CloseService();

            }
            return dt;
        }


        private string GetLicenseKey(string userName, string businessUnit, ref MessageSet message)
        {
            string licenseKey = "";
            try
            {
                string commandText = "SELECT     ZYUserLicenseTable.LicenseNumbers " +
                                     " FROM         ZYUserBusUnit INNER JOIN " +
                                     "  ZYUser ON ZYUserBusUnit.UserName = ZYUser.UserName INNER JOIN " +
                                     "   ZYUserLicenseTable ON ZYUserBusUnit.BusinessUnit = ZYUserLicenseTable.BusinessUnit " +
                                     // "     (ZYUser.UserName = '" + userName +"' ) AND"+
                                     " WHERE (ZYUserBusUnit.BusinessUnit = '" + businessUnit + "' )";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                foreach (DataRow dtRow in _dTable.Rows)
                {
                    licenseKey = dtRow["LicenseNumbers"].ToString();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get License Data", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            return licenseKey;
        }

        private string GetExpiryAlertStartFrom(string userName, string businessUnit, ref MessageSet message)//VR015
        {
            string StartFrom = "";
            try
            {
                string commandText = "SELECT     ZYUserLicenseTable.ExpiryAlertStartFrom " +
                                     " FROM         ZYUserBusUnit INNER JOIN " +
                                     "  ZYUser ON ZYUserBusUnit.UserName = ZYUser.UserName INNER JOIN " +
                                     "   ZYUserLicenseTable ON ZYUserBusUnit.BusinessUnit = ZYUserLicenseTable.BusinessUnit " +
                                     // "     (ZYUser.UserName = '" + userName +"' ) AND"+
                                     " WHERE (ZYUserBusUnit.BusinessUnit = '" + businessUnit + "' )";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                foreach (DataRow dtRow in _dTable.Rows)
                {
                    StartFrom = dtRow["ExpiryAlertStartFrom"].ToString();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetExpiryAlertStartFrom", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            return StartFrom;
        }

        private void GetPasswordData(string businessUnit, ref MessageSet message)
        {
            var password = new Password();
            try
            {
                string commandText = "SELECT   DISTINCT  ZYPasswordControl.MinLength, ZYPasswordControl.MaxLength, " +
                                     " ZYPasswordControl.NoOfSpecialCharacters,ZYPasswordControl.ReusePeriod, " +
                                     "  ZYPasswordControl.NoOfAttempts,ZYPasswordControl.ExpirePeriodInMonths,ZYPasswordControl.UserExpirePeriodInMonths " +
                                     "   FROM         ZYPasswordControl Inner Join ZYUserBusUnit ON " +
                                     "ZYUserBusUnit.BusinessUnit = ZYPasswordControl.BusinessUnit" +
                                     " WHERE     (ZYUserBusUnit.BusinessUnit = '" + businessUnit + "')";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                foreach (DataRow dtRow in _dTable.Rows)
                {
                    password.MaxLength = dtRow["MaxLength"].ToString();
                    password.MinLength = dtRow["MinLength"].ToString();
                    password.NoOfSpecialCharacters = dtRow["NoOfSpecialCharacters"].ToString();
                    password.ReusePeriod = Convert.ToInt32(dtRow["ReusePeriod"]);
                    password.NoOfAttempts = Convert.ToInt32(dtRow["NoOfAttempts"]);
                    password.ExpirePeriodInMonths = Convert.ToInt32(dtRow["ExpirePeriodInMonths"]);
                    password.UserExpirePeriodInMonths = Convert.ToInt32(dtRow["UserExpirePeriodInMonths"]);
                }
            }
            catch (SqlException ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get Password Data", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }
            finally
            {
                _dbServise.CloseService();
            }

            user.PasswordData = password;
        }

        private void GetLastLoggingDAte(string userName, ref MessageSet message)
        {
            string date = string.Empty;
            string time = string.Empty;
            DateTime datetime = DateTime.Now;
            DateTime dateExpire = new DateTime();//VR010

            long dif = -1;


            try
            {
                if (user.PasswordData.UserExpirePeriodInMonths == 0) //VR011 add
                {
                    user.IsUserExpire = false;
                    return;
                }

                string commandText =
                    "SELECT  isnull(left(MAX(Date),12),LEFT(CONVERT(VARCHAR(26), GETDATE(), 100),12)) AS Date,isnull(right(MAX([Time] ),7),right((CONVERT(VARCHAR(26),GETDATE(), 100)),7))AS [Time]  " +
                    " From ZYPasswordLoginDetails " +
                    "   WHERE UserName='" + userName +
                    "' and   SuccessfulLogin='1' and Date=(select Max(Date) from ZYPasswordLoginDetails  WHERE UserName='" +
                    userName + "' and SuccessfulLogin='1' )";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);

                foreach (DataRow dtRow in _dTable.Rows)
                {
                    date = dtRow["Date"].ToString();
                    time = dtRow["Time"].ToString();
                }

                date = date + time;
                if (!date.Equals(string.Empty))
                {
                    datetime = Convert.ToDateTime(date);

                    //dif = DateAndTime.DateDiff(DateInterval.Month, datetime, DateTime.Now, FirstDayOfWeek.System,
                    //                           FirstWeekOfYear.System);

                    dateExpire = DateAndTime.DateAdd(DateInterval.Month, user.PasswordData.UserExpirePeriodInMonths * -1, DateTime.Now);//VR010
                }
            }
            catch (SqlException ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Last Logging date", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }
            finally
            {
                _dbServise.CloseService();
            }


            //if (dif > user.PasswordData.UserExpirePeriodInMonths)
            if (dateExpire.Date > datetime.Date)//VR010
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
                string commandText =
                    "SELECT  isnull(left(MAX(Date),12),LEFT(CONVERT(VARCHAR(26), GETDATE(), 100),12)) AS Date,isnull(right(MAX([Time] ),7),right((CONVERT(VARCHAR(26),GETDATE(), 100)),7))AS [Time]  " +
                    " From ZYPasswordHistory " +
                    "   WHERE UserName='" + userName + "' ";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);

                foreach (DataRow dtRow in _dTable.Rows)
                {
                    date = dtRow["Date"].ToString();
                    time = dtRow["Time"].ToString();
                }

                date = date + time;
                if (!date.Equals(string.Empty))
                {
                    datetime = Convert.ToDateTime(date);

                    dif = DateAndTime.DateDiff(DateInterval.Month, datetime, DateTime.Now, FirstDayOfWeek.System,
                                               FirstWeekOfYear.System);
                }
            }
            catch (SqlException ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Last Logging date", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }
            finally
            {
                _dbServise.CloseService();
            }


            if (dif > user.PasswordData.ExpirePeriodInMonths)
            {
                user.IsPassExpire = true;
            }
            else
            {
                user.IsPassExpire = false;
            }
        }

        private List<MultimediaDetails> GetUserMultimedia(string userName, string businessUnit, DataTable dtMasterGroupVal,
                                                   ref MessageSet msg)
        {
            var multimedia = new List<MultimediaDetails>();
            try
            {
                ////string commandText = "";
                ////commandText = commandText + " SELECT DISTINCT   ";
                ////commandText = commandText +
                ////              "   RD.Multimedia.FileName, RD.Multimedia.Extension, RD.MultimediaAttachment.ActivateDate, RD.MultimediaAttachment.DisplayMode,   ";
                ////commandText = commandText +
                ////              "    RD.MultimediaAttachment.MandotoryStatus, RD.MultimediaAttachment.DisplayType,RD.MultimediaAttachment.MultimediaRecID ";
                ////commandText = commandText + "  FROM         RD.MultimediaAttachment INNER JOIN ";
                ////commandText = commandText +
                ////              "   RD.Multimedia ON RD.MultimediaAttachment.MultimediaRecID = RD.Multimedia.RecID AND RD.MultimediaAttachment.BusinessUnit = RD.Multimedia.BusinessUnit ";
                ////commandText = commandText + "WHERE     ((RD.MultimediaAttachment.UserName='" + userName.Trim() +
                ////              "') AND (RD.MultimediaAttachment.BusinessUnit='" + businessUnit.Trim() + "')  ";
                ////commandText = commandText +
                ////              " AND ((RD.MultimediaAttachment.DisplayMode='0') OR (RD.MultimediaAttachment.DisplayMode='2')) ";
                //////Display Mode Auto(First Time Load Auto Play-Not Attach to Select List) Or Both (First Tme Load And Attach To Select List)
                ////commandText = commandText + " AND ((RD.MultimediaAttachment.ActivateDate <='" + DateTime.Today.ToString("yyyyMMdd") + //V2008 (before only DateTime.Today including the one right below)
                ////              "') OR (RD.MultimediaAttachment.ActivateDate IS NULL) ) ";
                ////commandText = commandText + " AND ( (RD.MultimediaAttachment.DisplayDate IS NULL) OR (RD.MultimediaAttachment.DisplayDate >='" + DateTime.Today.ToString("yyyyMMdd")  + "' )) ";//VR0022
                //////commandText = commandText + " AND ( (RD.MultimediaAttachment.DisplayDate IS NULL) ) ";
                //////foreach (DataRow dtRow in dtMasterGroupVal.Rows)
                //////{

                //////        commandText = commandText + " OR  (RD.MultimediaAttachment.MasterGroup ='" +
                //////                      dtRow["MasterGroupValue"].ToString().Trim() + "')";

                //////}
                ////commandText = commandText + ")";
                string territoryCode = GetUserTerritoryCode(userName, businessUnit, ref msg);

                string commandText = "";
                commandText = commandText + " SELECT DISTINCT ";
                commandText = commandText + " m.AlertID, m.FileName,m.Extension,m.Text,m.Description, m.Mandatory, m.Type , t.Occurence,t.TargetCategory FROM  RD.MultimediaHeader as m INNER JOIN  ";
                commandText = commandText + " (Select DISTINCT * FROM RD.MultimediaDetails WHERE TargetCategory = 4 AND Occurence > 0 ";
                commandText = commandText + " UNION ALL Select DISTINCT * FROM RD.MultimediaDetails WHERE TargetCategory = 3 AND Occurence > 0 and [TargetValue ] = '" + userName.Trim() + "' ";
                if (territoryCode != "")
                {
                    commandText = commandText + " UNION ALL Select DISTINCT * FROM RD.MultimediaDetails WHERE TargetCategory = 1 AND Occurence > 0 and [TargetValue ] = '" + territoryCode.Trim() + "' ";
                }
                commandText = commandText + " ) as t";
                commandText = commandText + " ON m.AlertID = t.AlertID AND m.BusinessUnit = t.BusinessUnit ";
                commandText = commandText + " WHERE m.BusinessUnit = '" + businessUnit.Trim() + "' AND m.Status = '1' AND m.Category = 'B' ";
                //Display Mode Auto(First Time Load Auto Play-Not Attach to Select List) Or Both (First Tme Load And Attach To Select List)
                commandText = commandText + " AND ((m.StartDate <='" + DateTime.Today.ToString("yyyy-MM-dd") + "') OR (m.StartDate IS NULL) ) ";
                commandText = commandText + " AND ( (m.EndDate IS NULL) OR (m.EndDate >='" + DateTime.Today.ToString("yyyy-MM-dd") + "' )) ";



                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.UserDB, commandText);


                foreach (DataRow dtRow in _dTable.Rows)
                {
                    multimedia.Add(new MultimediaDetails
                    {
                        FileName =
                            !string.IsNullOrEmpty(dtRow["FileName"].ToString())
                                ? dtRow["FileName"].ToString()
                                : "",
                        Extension = dtRow["Extension"].ToString(),
                        Text = dtRow["Text"].ToString(),
                        Description = dtRow["Description"].ToString(),
                        Mandatory = Convert.ToChar(dtRow["Mandatory"].ToString()),
                        Type = Convert.ToChar(dtRow["Type"].ToString())

                        //ActivateDate = Convert.ToDateTime(dtRow["ActivateDate"]),
                        //DisplayMode = dtRow["DisplayMode"].ToString(),
                        //MandotoryStatus = dtRow["MandotoryStatus"].ToString(),
                        //DisplayType = dtRow["DisplayType"].ToString(),
                        //MultimediaRecID = Convert.ToInt16(dtRow["MultimediaRecID"])
                    });

                    UpdateMultimedia(businessUnit, userName, territoryCode, Convert.ToInt32(dtRow["AlertID"].ToString()), Convert.ToInt32(dtRow["TargetCategory"].ToString()), ref msg);
                }


            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetUserMultimedia", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            return multimedia;
        }

        //V2015
        private string GetUserTerritoryCode(string userName, string businessUnit, ref MessageSet message)
        {
            string territory = "";
            try
            {
                string commandText = "SELECT  TerritoryCode" +
                                     " FROM  XA.UserTerritory " +
                                     "WHERE     (UserName = '" + userName + "' ) AND " +
                                     " (BusinessUnit = '" + businessUnit + "' ) AND (Status = '1') ";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.UserDB, commandText);


                foreach (DataRow dtRow in _dTable.Rows)
                {
                    territory = dtRow["TerritoryCode"].ToString();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "GetUserTerritoryCode", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            return territory;
        }
        private void GetActiveUsersData(ref List<User> activeUsers, ref MessageSet message)
        {
            try
            {
                string commandText =
                    "SELECT ZYUser.UserName,ZYUserBusUnit.BusinessUnit,ZYUser.Password,ZYUser.ActiveFlag " +
                    ",ZYUser.PasswordLocked,ZYUser.PowerUser FROM ZYUser " +
                    "INNER JOIN ZYUserBusUnit ON ZYUser.UserName = ZYUserBusUnit.UserName " +
                    "WHERE ZYUserBusUnit.DefaultBusinessUnit = '1' " + //V2004
                     //"AND ZYUser.UserName NOT IN( 'administrator','xontadmin') " + //V2012
                    "AND ZYUser.UserName NOT IN('admin', 'administrator','xontadmin') " + //V2050
                    "AND ZYUser.UserLevelGroup <> 'OWNER' " + //V2047
                    "";//"WHERE ZYUser.UserName NOT IN( 'administrator','xontadmin') ";//VR019 //VR030 comment
                
                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                if (_dTable.Rows.Count > 0)
                {
                    string activeFlag = "";
                    var userNameS = new User();
                    foreach (DataRow dtRow in _dTable.Rows)
                    {
                        var userOb = new User();
                        userOb.isExists = true;
                        userOb.BusinessUnit = dtRow["BusinessUnit"].ToString();
                        userOb.UserName = dtRow["UserName"].ToString();
                        //  userOb.TimeStamp = dtRow["TimeStamp"] as byte[];
                        //userOb.UserFullName = dtRow["UserFullName"].ToString();
                        //userOb.UserLevelGroup = dtRow["UserLevelGroup"].ToString();
                        userOb.Password = dtRow["Password"].ToString();
                        userOb.PasswordLocked = Convert.ToChar(dtRow["PasswordLocked"]);
                        userOb.ActiveFlagDescription = dtRow["ActiveFlag"].ToString();
                        userOb.PowerUser = dtRow["PowerUser"].ToString();
                        //userOb.Theme = dtRow["Theme"].ToString();
                        //userOb.Language = dtRow["Language"].ToString();
                        //userOb.FontColor = dtRow["FontColor"].ToString();
                        //userOb.FontSize = !string.IsNullOrEmpty(dtRow["FontSize"].ToString()) ? Convert.ToInt32(dtRow["FontSize"].ToString()) : 0;
                        //userOb.FontName = dtRow["FontName"].ToString();
                        // activeFlag=""
                        userOb.ActiveFlag = false;
                        if (string.IsNullOrEmpty(userNameS.UserName) ||
                            userNameS.UserName.Trim() != userOb.UserName.Trim())
                        {
                            CheckActiveFlagUser(ref userOb, ref message);
                            userNameS = userOb;
                        }
                        //VR019 comment
                        //if (userNameS.ActiveFlag)
                        //{
                        activeUsers.Add(userOb);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get ActiveS User Data",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        private void GetPasswordHistoryListData(string userName, string reusePeriod, ref List<string> passList, ref MessageSet message)
        {
            passList = new List<string>();
            try
            {
                string commandText = "SELECT     TOP (" + reusePeriod + ") Password"
                                     + " FROM         ZYPasswordHistory"
                                     + " WHERE UserName='" + userName + "'"
                                     + " ORDER BY Date DESC, Time DESC";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                foreach (DataRow dtRow in _dTable.Rows)
                {
                    passList.Add(dtRow["Password"].ToString());
                }
            }
            catch (SqlException ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get Password Data", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);
            }
            finally
            {
                _dbServise.CloseService();
            }
        }


        #endregion

        #region Update functions


        public void SaveReminders(string businessUnit, string userName, string reminderName, string message, string timeString, ref MessageSet msg)
        {

            string command = "INSERT INTO [RD].[Reminders] ([BusinessUnit], [UserName], [ReminderID], [Message], [CreatedBy],[TriggerTime]) ";
            command += " VALUES( '" + businessUnit + "','" + userName + "','" + reminderName + "','" + message + "','" + userName + "', CONVERT(DATETIME,'" + timeString + "' ,101))";

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.UserDB, command);
                    ts.Complete();
                }
            }

            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "SaveReminders",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

        }



        public void UpdateReminder(string businessUnit, string userName, string reminderName, string message, string timeString, string reminderNameOld, ref MessageSet msg)
        {
            string command = "UPDATE [RD].[Reminders] SET ReminderID = '" + reminderName + "', Message = '" + message + "', CreatedBy = '" + userName + "', TriggerTime = CONVERT(DATETIME,'" + timeString + "' ,101)";
            command += " WHERE BusinessUnit = '" + businessUnit + "' AND UserName ='" + userName + "' AND ReminderID = '" + reminderNameOld + "'";

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.UserDB, command);
                    ts.Complete();
                }
            }

            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "UpdateReminder", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

        }
        public void DeleteReminders(string userName, List<string> reminders, ref MessageSet message)
        {

            string toDelete = "'";
            foreach (string reminder in reminders)
            {
                toDelete += reminder + "','";
            }
            toDelete = toDelete.TrimEnd('\'');
            toDelete = toDelete.TrimEnd(',');
            string command = "DELETE from RD.Reminders ";
            command += " WHERE CreatedBy = '" + userName.Trim() + "' AND ";
            command += " ReminderID in (" + toDelete + ")";

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.UserDB, command);
                    ts.Complete();
                }
            }

            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "DeleteReminders",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

        }


        public void DeleteBookmarks(string userName, string[] bookmarks, ref MessageSet message)
        {
            string toDelete = "'";
            foreach (string bookmark in bookmarks)
            {
                toDelete += bookmark + "','";
            }
            toDelete = toDelete.TrimEnd('\'');
            toDelete = toDelete.TrimEnd(',');
            string command = "DELETE from RD.Bookmarks ";
            command += " WHERE CreatedBy = '" + userName.Trim() + "' AND ";
            command += " bookmarkID in (" + toDelete + ")";

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.UserDB, command);
                    ts.Complete();
                }
            }

            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "DeleteBookmarks",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }


        private void UpdateUserSettingData(User userOb, ref MessageSet message)
        {
            string licenseKey = string.Empty;
            string commandText = string.Empty;
            try
            {
                commandText = "UPDATE [ZYUser]  " +
                              " Set [Theme]='" + userOb.Theme.Trim() + "',[Language]='" + userOb.Language.Trim() + "'" +
                              ",[FontName]='" + userOb.FontName.Trim() + "'" + ",[FontColor]='" +
                              userOb.FontColor.Trim() + "'" + ",[FontSize]='" + userOb.FontSize + "'" +
                              "Where [UserName]='" + userOb.UserName.Trim() + "'";

                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.SystemDB, commandText);
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Update Theme Data",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        private void ResetProfilePictureData(User userOb, ref MessageSet message)
        {

            string commandText = string.Empty;
            try
            {
                commandText = "UPDATE [ZYUser] " +
                              " SET ProPicAvailable = '0' " +
                              " WHERE [UserName] = '" + userOb.UserName.Trim() + "' ";

                _dbServise.StartService();
                _dbServise.Excecute(CommonVar.DBConName.SystemDB, commandText);
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Reset Profile Picture", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }


        private void UpdateLoginData(User userOb, ref MessageSet message)
        {
            string licenseKey = string.Empty;
            string commandText = string.Empty;
            try
            {
                if (userOb.SuccessfulLogin.Equals("0"))
                {
                    commandText = "UPDATE [ZYPasswordLoginDetails]  " +
                                  //" Set [LogOutTime]='" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "',[LogOutReson] ='" + userOb.Reson + "'" +//VR029
                                  " Set [LogOutTime]='" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "',[LogOutReson] ='" + userOb.Reson + "'" +//VR029
                                  "Where [UserName]='" + userOb.UserName + "'  and [WorkstationID]='" +
                                  userOb.WorkstationId + "' and [SessionID]='" + userOb.SessionId +
                                  "' and [SuccessfulLogin]= '0' and LogOutReson IS NULL ";
                }
                else
                {
                    commandText = "UPDATE [ZYPasswordLoginDetails]  " +
                                   //" Set [LogOutTime]='" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "',[LogOutReson] ='" + userOb.Reson + "'" +//VR029
                                   " Set [LogOutTime]='" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss") + "',[LogOutReson] ='" + userOb.Reson + "'" +//VR029
                                  "Where [UserName]='" + userOb.UserName + "'  and [WorkstationID]='" +
                                  userOb.WorkstationId + "' and [SessionID]='" + userOb.SessionId +
                                  "' and LogOutReson IS NULL ";
                }
                //using (var ts = new TransactionScope(TransactionScopeOption.Required))
                //{
                _dbServise.StartService();
                _dbServise.Excecute(CommonVar.DBConName.SystemDB, commandText);
                //ts.Complete();
                //}
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Update Logging Data",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        private string SaveLoginData(User userOb, ref MessageSet message)
        {
            string licenseKey = "";
            try
            {
                string commandText = "INSERT INTO [ZYPasswordLoginDetails] " +
                                     " ([UserName],[Date],[Time],[Password],[WorkstationID] " +
                                     "  ,[SuccessfulLogin],[SessionID],[Reson])  VALUES( " +
                                     "  '" + userOb.UserName + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" +
                                     DateTime.Now.ToLongTimeString() + "'," +
                                     "'" + userOb.Password + "','" + userOb.WorkstationId + "','" +
                                     userOb.SuccessfulLogin + "','" + userOb.SessionId + "','" + userOb.Reson + "' )";

                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.SystemDB, commandText);
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Save Login Details",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            return licenseKey;
        }

        private void UpdateLoginAttempt(User userOb, ref MessageSet message)
        {
            string licenseKey = "";
            try
            {
                string commandText = "UPDATE [ZYUser]  " +
                                     " SET PasswordLocked = '1' " +
                                     "Where [UserName]='" + userOb.UserName + "' ";

                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.SystemDB, commandText);
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Update No Of Logging Attemots",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        private void SaveChangePasswordData(string userName, string password, string passwordChange, ref MessageSet message) //V2004
        {
            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    var paramSet = new ParameterSet();
                    var spParametersList = new List<SPParameter>();

                    paramSet.SetSPParameterList(spParametersList, "UserName", userName, "");
                    paramSet.SetSPParameterList(spParametersList, "Password", password, "");
                    paramSet.SetSPParameterList(spParametersList, "PasswordChange", passwordChange, "");

                    string commandText = "dbo.usp_UpdateUserPasswordHistory";
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.SystemDB, commandText, spParametersList);

                    ts.Complete();
                }
            }
            catch
                (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "SaveChangePasswordData",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        private void UpdateActiveFlag(User userOb, ref MessageSet message)
        {
            var stroEncript = new StroEncript();
            //string encriptActiveFlag = stroEncript.Encript("0", userOb.UserName);
            string encriptActiveFlag = stroEncript.Encript("0", userOb.UserName.Trim());//VR009
            try
            {
                string commandText = "UPDATE [ZYUser]  " +
                                     " SET ActiveFlag = '" + encriptActiveFlag + "' " +
                                     "Where [UserName]='" + userOb.UserName + "' ";

                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.SystemDB, commandText);
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Update No Of Logging Attemots",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        private void UpdateMultimedia(string businessUnit, string username, string territoryCode, int multRecID, int targetCategory, ref MessageSet message)
        {
            string commandText = "";
            try
            {
                commandText = "UPDATE [RD].[MultimediaDetails]  " +
                              " Set  [Occurence] = [Occurence] - 1 " +
                              " Where  [BusinessUnit] ='" + businessUnit.Trim() + "' " +
                              " AND  AlertID ='" + multRecID + "' " +
                              " AND  (Occurence > 0) ";
                if (targetCategory == 1)
                {
                    commandText = commandText + "AND TargetValue = '" + territoryCode.Trim() + "' ";
                }
                else if (targetCategory == 3)
                {
                    commandText = commandText + "AND TargetValue = '" + username.Trim() + "' ";
                }

                _dbServise.StartService();
                _dbServise.Excecute(CommonVar.DBConName.UserDB, commandText);


            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "UpdateMultimedia",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        //VR025
        public void ReleaseObjectLocks(string businessUnit, List<string> sessionIdList, ref MessageSet message)
        {
            try
            {
                foreach (string sessionId in sessionIdList)
                {
                    var paramSet = new ParameterSet();
                    var spParametersList = new List<SPParameter>();

                    paramSet.SetSPParameterList(spParametersList, "BusinessUnit", businessUnit, "");
                    paramSet.SetSPParameterList(spParametersList, "SessionID", sessionId, "");

                    string commandText = "dbo.usp_ReleaseObjectLock";
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.SystemDB, commandText, spParametersList);
                    _dbServise.CloseService();
                }

            }
            catch
                (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "ReleaseObjectLocks", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                //_dbServise.CloseService();
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
            string tempCommand = @"SELECT ReminderID FROM RD.Reminders WHERE BusinessUnit ='" + businessUnit + "' AND UserName = '" + userName + " '";
            DataTable dt = new DataTable();
            try
            {
                CommonDBService tempDb = new CommonDBService();
                tempDb.StartService();
                dt = tempDb.FillDataTable(tempCommand);
                tempDb.CloseService();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }


        public DataTable GetExpiredReminders(string businessUnit, string userName, ref MessageSet msg)
        {
            string timeString = "";
            DateTime local = DateTime.Now.ToLocalTime();
            timeString = local.Month.ToString() + "-" + local.Day.ToString() + "-" + local.Year.ToString() + " ";
            timeString += local.Hour.ToString() + ":" + local.Minute.ToString() + ":0 " + local.ToString("tt", CultureInfo.InvariantCulture);

            string command = @"SELECT ReminderID, Message,TriggerTime FROM RD.Reminders WHERE UserName ='" + userName + "' AND BusinessUnit ='" + businessUnit + "' AND TriggerTime < CONVERT(DATETIME, '" + timeString + "', 101)  ORDER BY TriggerTime ";
            DataTable dt = new DataTable();
            try
            {
                CommonDBService tempDb = new CommonDBService();
                tempDb.StartService();
                dt = tempDb.FillDataTable(command);
                tempDb.CloseService();
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetExpiredReminders",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            return dt;

        }


        public DataTable getReminderDetails(string businessUnit, string userName, string reminderId, ref MessageSet msg)
        {

            string tempCommand = @"SELECT ReminderID, Message, TriggerTime FROM RD.Reminders WHERE BusinessUnit ='" + businessUnit + "' AND  UserName = '" + userName.Trim() + "' AND ReminderID = '" + reminderId + "'";
            DataTable dt = new DataTable();
            try
            {
                CommonDBService tempDb = new CommonDBService();
                tempDb.StartService();
                dt = tempDb.FillDataTable(tempCommand);
                tempDb.CloseService();

            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "getReminderDetails", "XONT.Ventura.AppConsole.DAL.dll");
            }
            return dt;

        }

        public DataTable GetReminderJustExpired(string businessUnit, string userName, string reminder, ref MessageSet msg)
        {

            string command = @"SELECT TOP (1) ReminderID, Message FROM RD.Reminders WHERE BusinessUnit ='" + businessUnit + "' AND UserName ='" + userName + "' AND ReminderID = '" + reminder + "'";
            DataTable dt = new DataTable();
            try
            {
                CommonDBService tempDb = new CommonDBService();
                tempDb.StartService();
                dt = tempDb.FillDataTable(command);
                tempDb.CloseService();


            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetReminderJustExpired", "XONT.Ventura.AppConsole.DAL.dll");
            }
            return dt;
        }
        public DataTable getMostRecentReminder(string businessUnit, string userName, ref MessageSet msg)
        {
            string timeString = "";
            DateTime local = DateTime.Now.ToLocalTime();
            timeString = local.Month.ToString() + "-" + local.Day.ToString() + "-" + local.Year.ToString() + " ";
            timeString += local.Hour.ToString() + ":" + local.Minute.ToString() + ":0 " + local.ToString("tt", CultureInfo.InvariantCulture);
            string tempCommand = "SELECT TOP (1) TriggerTime, ReminderID FROM RD.Reminders WHERE BusinessUnit ='" + businessUnit + "' AND UserName ='" + userName + "' AND TriggerTime > CONVERT(DATETIME, '" + timeString + "', 101) ORDER BY TriggerTime ";
            DataTable dt = new DataTable();
            try
            {
                CommonDBService tempDb = new CommonDBService();
                tempDb.StartService();
                dt = tempDb.FillDataTable(tempCommand);
                tempDb.CloseService();

            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "getMostRecentReminder", "XONT.Ventura.AppConsole.DAL.dll");
            }
            return dt;
        }

        public void saveFavourites(string businessUnit, string userName, string bookmarkId, string bookmarkName, string path, ref MessageSet message)//mgd
        {
            bool action;
            try
            {

                string commandText = "INSERT INTO [RD].[Bookmarks] " +
                                         " ([BusinessUnit],[UserName],[BookmarkID],[Bookmark],[TaskPath],[CreatedBy])  " +
                                         "VALUES( '" + businessUnit + "','" + userName + "','" + bookmarkId + "','" + bookmarkName + "','" + path + "','" + userName + "')";


                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    action = _dbServise.ExcecuteWithReturn(CommonVar.DBConName.UserDB, commandText) > 0;
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "saveFavourites",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

        }


        public void saveProfilePicture(string userName, byte[] image, ref MessageSet message)
        {

            bool action;
            try
            {

                ParameterSet ParaSet = new ParameterSet();
                var spParametersList = new List<SPParameter>();


                ParaSet.SetSPParameterList(spParametersList, "ProfilePicture", image, "");
                ParaSet.SetSPParameterList(spParametersList, "UserName", userName, "");
                ParaSet.SetSPParameterList(spParametersList, "ProPicAvailable", '1', "");


                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    action = _dbServise.ExcecuteWithReturn(CommonVar.DBConName.SystemDB, "usp_AddProfilePicture", spParametersList) > 0;
                    ts.Complete();
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "saveProfilePicture",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }


        public byte[] getImageData(string userName, ref MessageSet message)
        {
            byte[] imageData = new byte[0];
            DataTable dt = new DataTable();
            string command = "SELECT ProfileImage FROM dbo.ZYUser WHERE UserName = '" + userName.Trim() + "'";
            try
            {

                _dbServise.StartService();
                dt = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, command);
                imageData = (byte[])dt.Rows[0]["ProfileImage"];

            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "get Image Data", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
            return imageData;
        }

        public DataTable getBUnit(string userName, ref MessageSet message)//ss
        {
            DataTable bu = new DataTable();
            string query = "";

            try
            {

                query += "SELECT BusinessUnit ";
                query += "FROM ZYUserBusUnit ";
                query += "WHERE UserName = '" + userName + "' ";

                _dbServise.StartService();
                bu = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, query);

            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "getBUnit", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);

            }
            finally
            {
                _dbServise.CloseService();

            }
            return bu;
        }


        public string GetDefMenuCode(string userName, ref MessageSet message)
        {

            string menuCode;
            DataTable dt = new DataTable();
            string query = "";

            try
            {
                //query = query + "SELECT UserName, RoleCode, MenuCode ";
                query = query + "SELECT MenuCode ";
                query = query + "FROM dbo.ZYUser ";
                query = query + "WHERE  dbo.ZYUser.UserName = '" + userName + "' ";

                _dbServise.StartService();
                dt = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, query);

                //menuCode = dt.Rows[1].Cells[3].Text.ToString();

            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "Get menu code", "XONT.Ventura.AppConsole.DAL.dll");
                Console.WriteLine(ex);

            }
            finally
            {
                _dbServise.CloseService();

            }
            menuCode = dt.Rows[0]["MenuCode"].ToString().Trim();
            return menuCode;

        }

        #region Admin Alert V2007

        public DataTable getMostRecentAlert(ref MessageSet msg)
        {
            string timeString = "";
            DateTime local = DateTime.Now.ToLocalTime();
            timeString = local.Month.ToString() + "-" + local.Day.ToString() + "-" + local.Year.ToString() + " ";
            timeString += local.Hour.ToString() + ":" + local.Minute.ToString() + ":0 " + local.ToString("tt", CultureInfo.InvariantCulture);

            DataTable dt = new DataTable();
            try
            {
                string tempCommand = "SELECT TOP (1) * FROM dbo.ZYAdminAlertCurrent WHERE AlertTime < CONVERT(DATETIME, '" + timeString + "', 101) ORDER BY AlertTime ASC ";
                _dbServise.StartService();
                dt = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, tempCommand);

            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "getMostRecentAlert", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();

            }
            return dt;
        }

        public void DeleteAlert(int AlertNumber, int RecID, ref MessageSet message)
        {


            string command = "DELETE from dbo.ZYAdminAlertCurrent ";
            command += " WHERE AlertNumber = '" + AlertNumber + "' AND ";
            command += " RecID = (" + RecID + ")";

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.SystemDB, command);
                    UpdateAlert(AlertNumber, RecID);

                    ts.Complete();
                }
            }

            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "DeleteAlert",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

        }

        public void UpdateAlert(int AlertNumber, int RecID)
        {


            string command = "Update dbo.ZYAdminAlert ";
            command += " SET Completed = '1' ";
            command += " WHERE AlertNumber = '" + AlertNumber + "' ";

            try
            {
                _dbServise.StartService();
                _dbServise.Excecute(CommonVar.DBConName.SystemDB, command);

            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _dbServise.CloseService();
            }

        }

        public void UpdateAlertCurrent(int AlertNumber, int RecID, string TimeSpan, ref MessageSet message)
        {


            string command = "Update dbo.ZYAdminAlertCurrent ";
            command += " SET RepeatTimes = RepeatTimes - 1 ";
            command += " WHERE AlertNumber = '" + AlertNumber + "' AND ";
            command += " RecID = (" + RecID + ")";

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.SystemDB, command);
                    UpdateAlertTime(AlertNumber, RecID, TimeSpan);

                    ts.Complete();
                }
            }

            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "DeleteAlert",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

        }

        public void UpdateAlertTime(int AlertNumber, int RecID, string TimeSpan)
        {
            DateTime timeSpan = DateTime.Parse(TimeSpan);

            string command = "Update dbo.ZYAdminAlertCurrent ";
            command += " SET AlertTime = '" + timeSpan + "' ";
            command += " WHERE AlertNumber = '" + AlertNumber + "' AND ";
            command += " RecID = (" + RecID + ")";

            try
            {
                _dbServise.StartService();
                _dbServise.Excecute(CommonVar.DBConName.SystemDB, command);

            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _dbServise.CloseService();
            }

        }
        #endregion

        #region DailyAlert V2016
        public void UpdateDailyMenu(string MenuCode, ref MessageSet message)
        {

            string command = "Update dbo.ZYMenuHeader ";
            //V2022
            command += " SET LastAutoexecutedOn = '" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "' ";
            command += " WHERE MenuCode = '" + MenuCode.Trim() + "' ";

            try
            {
                _dbServise.StartService();
                _dbServise.Excecute(CommonVar.DBConName.SystemDB, command);

            }

            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _dbServise.CloseService();
            }

        }

        public bool CheckDailyMenu(string MenuCode, ref MessageSet message)
        {
            // DateTime Now = DateTime.Now;
            String Now = DateTime.Now.ToString("yyyyMMdd hh:mm:ss");
            bool available = false;
            try
            {
                string commandText = "SELECT MenuCode FROM ZYMenuHeader  WHERE MenuCode ='" + MenuCode.Trim() + "'";
                commandText += " AND ( LastAutoexecutedOn IS NULL  OR  CAST(LastAutoexecutedOn AS DATE) < CAST('" + Now + "' AS DATE) ) ";

                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);
                _dbServise.CloseService();

                if (_dTable.Rows.Count > 0)
                {
                    available = true;
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "CheckUserAvailable",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            return available;
        }

        #endregion


        #region V2017 - Notification(s)
        public int getNewNotification(string UserName, string BusinessUnit, int ExecutionType, ref MessageSet _message)
        {
            int unreadNotification = 0;
            string command = "";
            if (ExecutionType == 1)
            {
                command = @"SELECT COUNT(*) AS Notification  FROM RD.UserNotifications WHERE BusinessUnit ='" + BusinessUnit.Trim() + "' AND UserName ='" + UserName.Trim() + "' AND Status = '1' ";
            }
            else if (ExecutionType == 2)
            {
                command = @"SELECT COUNT(*) AS Notification  FROM RD.UserNotifications WHERE BusinessUnit ='" + BusinessUnit.Trim() + "' AND UserName ='" + UserName.Trim() + "' AND Status = '2' ";
            }
            DataTable dt = new DataTable();
            try
            {
                CommonDBService tempDb = new CommonDBService();
                tempDb.StartService();
                dt = tempDb.FillDataTable(command);
                tempDb.CloseService();

                if (dt.Rows.Count > 0 && dt != null)
                {
                    unreadNotification = int.Parse(dt.Rows[0]["Notification"].ToString().Trim());
                }

            }
            catch (Exception ex)
            {
                _message = MessageCreate.CreateErrorMessage(0, ex, "getNewNotification", "XONT.Ventura.AppConsole.DAL.dll");
            }
            return unreadNotification;
        }

        public DataTable getUserNotification(string UserName, string BusinessUnit, ref MessageSet _message)
        {
            string command = @"SELECT RecID,Date,UserName,SenderID,Message,TaskCode,Description,Status,Type";
            command += " FROM RD.UserNotifications WHERE BusinessUnit ='" + BusinessUnit.Trim() + "' AND UserName ='" + UserName.Trim() + "' AND Status != '4' ";
            DataTable dt = new DataTable();
            try
            {
                _dbServise.StartService();
                dt = _dbServise.FillDataTable(CommonVar.DBConName.UserDB, command);
                _dbServise.CloseService();
            }
            catch (Exception ex)
            {
                _message = MessageCreate.CreateErrorMessage(0, ex, "getUserNotification", "XONT.Ventura.AppConsole.DAL.dll");
            }
            return dt;
        }

        public void UpdateNotification(string businessUnit, string userName, int recId, int executionType, ref MessageSet msg)
        {
            string command = "";
            if (executionType == 1)
            {
                command = "UPDATE [RD].[UserNotifications] SET Status = '2' ";
                command += " WHERE BusinessUnit = '" + businessUnit + "' AND UserName ='" + userName + "' AND Status = '1' ";
            }
            else if (executionType == 2)
            {
                command = "UPDATE [RD].[UserNotifications] SET Status = '3' ";
                command += " WHERE BusinessUnit = '" + businessUnit + "' AND UserName ='" + userName + "' AND RecID = '" + recId + "' ";
            }

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.UserDB, command);
                    ts.Complete();
                }
            }

            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "UpdateNotification", "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

        }

        public void DeleteNotifications(string userName, int[] notifications, ref MessageSet message)
        {
            string toDelete = "'";
            foreach (int notification in notifications)
            {
                toDelete += notification + "','";
            }
            toDelete = toDelete.TrimEnd('\'');
            toDelete = toDelete.TrimEnd(',');
            string command = "UPDATE [RD].[UserNotifications] SET Status = '4' ";
            command += " WHERE UserName = '" + userName.Trim() + "' AND ";
            command += " RecID in (" + toDelete + ")";

            try
            {
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    _dbServise.StartService();
                    _dbServise.Excecute(CommonVar.DBConName.UserDB, command);
                    ts.Complete();
                }
            }

            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "DeleteNotifications",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }

        public UserTask UserNotificationTask(string taskCode, string userName, ref MessageSet message)//VR019
        {
            var userTask = new UserTask();
            try
            {
                string commandText =
                    "SELECT TOP 1 ZYTask.TaskCode ,ZYTask.Caption ,  ZYTask.Description,ZYTask.ExecutionScript,ZYTask.Icon " +
                    ",ISNULL(ZYTask.TaskType ,'') as TaskType, ExecutionScript as url " +
                    " FROM ZYTask " +
                    " where ZYTask.TaskCode ='" + taskCode + "' ";


                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.SystemDB, commandText);


                foreach (DataRow dtRow in _dTable.Rows)
                {
                    userTask = (new UserTask
                    {
                        TaskCode =
                            !string.IsNullOrEmpty(dtRow["TaskCode"].ToString())
                                ? dtRow["TaskCode"].ToString()
                                : "",
                        Description = dtRow["Description"].ToString(),
                        Caption = dtRow["Caption"].ToString(),
                        ExecutionScript = dtRow["ExecutionScript"].ToString(),
                        Icon = dtRow["Icon"].ToString().Trim()
                        ,
                        TaskType = !string.IsNullOrEmpty(dtRow["TaskType"].ToString()) ? dtRow["TaskType"].ToString() : ""//VR019
                        ,
                        UserName = userName.Trim()
                        ,
                        url = dtRow["url"].ToString().Trim()
                    });
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "UserNotificationTask",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }

            return userTask;
        }


        #endregion

        #region V2018
        public bool CheckUserNotificationTask(string taskCode, string businessUnit, ref MessageSet message)
        {
            bool available = false;
            try
            {
                string commandText = "SELECT * FROM RD.UserNotificationsTask  WHERE SourceTaskCode ='" + taskCode.Trim() + "'";
                commandText += " AND Status = '1' AND BusinessUnit ='" + businessUnit.Trim() + "'";

                _dbServise.StartService();
                _dTable = _dbServise.FillDataTable(CommonVar.DBConName.UserDB, commandText);
                _dbServise.CloseService();

                if (_dTable.Rows.Count > 0)
                {
                    available = true;
                }
            }
            catch (Exception ex)
            {
                message = MessageCreate.CreateErrorMessage(0, ex, "CheckUserNotificationTask",
                                                           "XONT.Ventura.AppConsole.DAL.dll");
            }
            return available;
        }

        public void UpdateUserNotification(string businessUnit, string userName, string taskCode, ref MessageSet msg)
        {
            try
            {

                String _commandText = "";

                _dbServise.StartService();
                using (var ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DataTable userNotificationTask = new DataTable();
                    _commandText = "SELECT UserName, TaskCode,Description FROM RD.UserNotificationsTask  WHERE SourceTaskCode ='" + taskCode.Trim() + "'";
                    _commandText += " AND Status = '1' AND BusinessUnit ='" + businessUnit.Trim() + "'";

                    userNotificationTask = _dbServise.FillDataTable(CommonVar.DBConName.UserDB, _commandText);

                    if (userNotificationTask.Rows.Count == 1)
                    {
                        string taskusername = "";
                        string tasktaskcode = "";
                        string description = "";
                        foreach (DataRow dt in userNotificationTask.Rows)
                        {
                            taskusername = dt["UserName"].ToString().Trim();
                            tasktaskcode = dt["TaskCode"].ToString().Trim();
                            description = dt["Description"].ToString().Trim();

                            _commandText = "INSERT INTO [RD].[UserNotifications] ([BusinessUnit], [Date], [UserName], [SenderID],[Type],[TaskCode],[Description],[Status],[CreatedBy],[CreatedOn],[UpdatedBy],[UpdatedOn]) ";
                            _commandText += " VALUES( '" + businessUnit.Trim() + "','" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "','" + taskusername.Trim() + "','" + userName.Trim() + "','T','" + tasktaskcode.Trim() + "' , ";
                            _commandText += " '" + description.Trim() + "','1','" + userName.Trim() + "','" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "','" + userName.Trim() + "','" + DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + "' ) ";
                            _dbServise.Excecute(CommonVar.DBConName.UserDB, _commandText);
                        }


                    }
                    ts.Complete();
                }
            }

            catch (Exception ex)
            {

                msg = MessageCreate.CreateErrorMessage(0, ex, "UpdateUserNotification", "XONT.VENTURA.V2MNT21.DAL.dll");
            }
            finally
            {
                _dbServise.CloseService();
            }
        }



        #endregion
    }
}