using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XONT.Common.Message;
using XONT.Ventura.Common.ConvertDateTime;

namespace XONT.Ventura.AppConsole
{
    public partial class ComponentDiagnositics : System.Web.UI.Page
    {
        //private string _task
        string _taskID = "";
        MessageSet _msg = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _taskID = Request.QueryString["TaskID"].ToString().ToUpper();
                string TaskType = Request.QueryString["TaskType"].ToString().ToUpper();
                if (!IsPostBack)
                {
                    ConvertDateTime.GetCurrentCulture();

                    ComponentVersionInfo cvi = VenturaSystemInfo.GetComponentVersionInfo(_taskID, true, out _msg);
                    if (_msg != null)
                    {
                        DisplayError();
                        return;
                    }

                    grvRelatedLibraries.DataSource = cvi.Level1RefLibs;
                    grvRelatedLibraries.DataBind();
                    Session[_taskID + "_RelatedLibs"] = cvi.Level1RefLibs;

                    grv2ndLayerLibs.DataSource = cvi.Level2RefLibs;
                    grv2ndLayerLibs.DataBind();
                    Session[_taskID + "_SecondLayerRelatedLibs"] = cvi.Level2RefLibs;

                    grvXontOtherLibs.DataSource = cvi.OtherXONTLibs;
                    grvXontOtherLibs.DataBind();
                    Session[_taskID + "_XONTOtherLibs"] = cvi.OtherXONTLibs;


                    grvComponentLibraries.DataSource = cvi.CoponentLibs;
                    grvComponentLibraries.DataBind();
                    Session[_taskID + "_ComponentLibs"] = cvi.CoponentLibs;

                    grvOtherLiberies.DataSource = cvi.ThirdPartyLibs;
                    grvOtherLiberies.DataBind();
                    Session[_taskID + "_ThirdPartyLibs"] = cvi.ThirdPartyLibs;

                    if (TaskType == "V3")
                    {
                        List<LibInfo> nodeModules = VenturaSystemInfo.GetClientLibsVersionInfo(out _msg);
                        if (_msg != null)
                        {
                            DisplayError();
                            return;
                        }

                        grvClientLibraries.DataSource = nodeModules;
                        grvClientLibraries.DataBind();
                        Session[_taskID + "_ClientLibs"] = nodeModules;
                        clientLibWrapper.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "Page_Load");
            }
        }

        protected void grvComponentLibraries_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<LibInfo> componentAssembliesInfo = Session[_taskID + "_ComponentLibs"] as List<LibInfo>;
                e.SortDirection = DynamicQueryable.GridSortDirectionChange(sender, e.SortExpression, e.SortDirection);
                if (componentAssembliesInfo != null)
                {
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        componentAssembliesInfo = componentAssembliesInfo.AsQueryable().OrderBy(e.SortExpression).ToList();
                    }
                    else
                    {
                        componentAssembliesInfo = componentAssembliesInfo.AsQueryable().OrderByDescending(e.SortExpression).ToList();
                    }
                }

                ConvertDateTime.GetCurrentCulture();
                grvComponentLibraries.DataSource = componentAssembliesInfo;
                grvComponentLibraries.DataBind();
                Session[_taskID + "_ComponentLibs"] = componentAssembliesInfo;
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvComponentLibraries_Sorting");
            }
        }

        protected void grvXontOtherLibs_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<LibInfo> xontOtherAssembliesInfo = Session[_taskID + "_XONTOtherLibs"] as List<LibInfo>;
                e.SortDirection = DynamicQueryable.GridSortDirectionChange(sender, e.SortExpression, e.SortDirection);
                if (xontOtherAssembliesInfo != null)
                {
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        xontOtherAssembliesInfo = xontOtherAssembliesInfo.AsQueryable().OrderBy(e.SortExpression).ToList();
                    }
                    else
                    {
                        xontOtherAssembliesInfo = xontOtherAssembliesInfo.AsQueryable().OrderByDescending(e.SortExpression).ToList();
                    }
                }

                ConvertDateTime.GetCurrentCulture();
                grvXontOtherLibs.DataSource = xontOtherAssembliesInfo;
                grvXontOtherLibs.DataBind();
                Session[_taskID + "_XONTOtherLibs"] = xontOtherAssembliesInfo;
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvXontOtherLibs_Sorting");
            }
        }

        protected void grvOtherLiberies_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<LibInfo> thirdPartyAssemblies = Session[_taskID + "_ThirdPartyLibs"] as List<LibInfo>;
                e.SortDirection = DynamicQueryable.GridSortDirectionChange(sender, e.SortExpression, e.SortDirection);
                if (thirdPartyAssemblies != null)
                {
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        thirdPartyAssemblies = thirdPartyAssemblies.AsQueryable().OrderBy(e.SortExpression).ToList();
                    }
                    else
                    {
                        thirdPartyAssemblies = thirdPartyAssemblies.AsQueryable().OrderByDescending(e.SortExpression).ToList();
                    }
                }

                ConvertDateTime.GetCurrentCulture();
                grvOtherLiberies.DataSource = thirdPartyAssemblies;
                grvOtherLiberies.DataBind();
                Session[_taskID + "_ThirdPartyLibs"] = thirdPartyAssemblies;
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvOtherLiberies_Sorting");
            }
        }

        protected void grvRelatedLibraries_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<RefLibInfo> relatedAssembliesInfo = Session[_taskID + "_RelatedLibs"] as List<RefLibInfo>;
                e.SortDirection = DynamicQueryable.GridSortDirectionChange(sender, e.SortExpression, e.SortDirection);
                if (relatedAssembliesInfo != null)
                {
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        relatedAssembliesInfo = relatedAssembliesInfo.AsQueryable().OrderBy(e.SortExpression).ToList();
                    }
                    else
                    {
                        relatedAssembliesInfo = relatedAssembliesInfo.AsQueryable().OrderByDescending(e.SortExpression).ToList();
                    }
                }

                ConvertDateTime.GetCurrentCulture();
                grvRelatedLibraries.DataSource = relatedAssembliesInfo;
                grvRelatedLibraries.DataBind();
                Session[_taskID + "_RelatedLibs"] = relatedAssembliesInfo;

            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvRelatedLibraries_Sorting");
            }
        }

        protected void grvClientLibraries_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<LibInfo> nodeModules = Session[_taskID + "_ClientLibs"] as List<LibInfo>;
                e.SortDirection = DynamicQueryable.GridSortDirectionChange(sender, e.SortExpression, e.SortDirection);
                if (nodeModules != null)
                {
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        nodeModules = nodeModules.AsQueryable().OrderBy(e.SortExpression).ToList();
                    }
                    else
                    {
                        nodeModules = nodeModules.AsQueryable().OrderByDescending(e.SortExpression).ToList();
                    }
                }

                ConvertDateTime.GetCurrentCulture();
                grvClientLibraries.DataSource = nodeModules;
                grvClientLibraries.DataBind();
                Session[_taskID + "_ClientLibs"] = nodeModules;
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvClientLibraries_Sorting");
            }
        }

        protected void grv2ndLayerLibs_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<LibInfo> SecondLayerRefLibsInfo = Session[_taskID + "_SecondLayerRelatedLibs"] as List<LibInfo>;
                e.SortDirection = DynamicQueryable.GridSortDirectionChange(sender, e.SortExpression, e.SortDirection);
                if (SecondLayerRefLibsInfo != null)
                {
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        SecondLayerRefLibsInfo = SecondLayerRefLibsInfo.AsQueryable().OrderBy(e.SortExpression).ToList();
                    }
                    else
                    {
                        SecondLayerRefLibsInfo = SecondLayerRefLibsInfo.AsQueryable().OrderByDescending(e.SortExpression).ToList();
                    }
                }

                ConvertDateTime.GetCurrentCulture();
                grv2ndLayerLibs.DataSource = SecondLayerRefLibsInfo;
                grv2ndLayerLibs.DataBind();
                Session[_taskID + "_SecondLayerRelatedLibs"] = SecondLayerRefLibsInfo;
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grv2ndLayerLibs_Sorting");
            }
        }

        private void displayExceptionError(Exception exception, string methodName)
        {
            MessageSet msg = MessageCreate.CreateErrorMessage(0, exception, methodName, "XONT.Ventura.AppConsole.WEB.dll");
            Session["Error"] = msg;
            MessageDisplay.Dispaly(btnForMessageDisplay);
        }

        private void DisplayError()
        {
            Session["Error"] = _msg;
            MessageDisplay.Dispaly(btnForMessageDisplay);
            _msg = null;
        }

        protected void grvRelatedLibraries_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    RefLibInfo bounding = (RefLibInfo)e.Row.DataItem;

                    if (bounding.version == "NA")
                    {
                        e.Row.CssClass = "LibNotFoundStyles GridviewScrollItem";
                    }
                    else if (bounding.version != bounding.builtWithVersion)
                    {
                        e.Row.CssClass = "InactiveGridRowStyle GridviewScrollItem";
                    }
                }
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvRelatedLibraries_RowDataBound");
            }
        }
    }


}