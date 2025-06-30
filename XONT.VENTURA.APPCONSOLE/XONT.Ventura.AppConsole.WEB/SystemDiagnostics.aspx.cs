using System;
using System.Collections.Generic;
using System.Configuration;
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
    public partial class SystemDiagnostics : System.Web.UI.Page
    {
        MessageSet _msg = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    ConvertDateTime.GetCurrentCulture();

                    SystemVersionInfo cvi = VenturaSystemInfo.GetSystemVersionInfo(out _msg);
                    if (_msg != null)
                    {
                        DisplayError();
                        return;
                    }

                    grvXontLibs.DataSource = cvi.XONTLibs;
                    grvXontLibs.DataBind();
                    Session["System_XONTLibs"] = cvi.XONTLibs;

                    grvOtherLiberies.DataSource = cvi.ThirdPartyLibs;
                    grvOtherLiberies.DataBind();
                    Session["System_ThirdPartyLibs"] = cvi.ThirdPartyLibs;

                    List<LibInfo> nodeModules = VenturaSystemInfo.GetClientLibsVersionInfo(out _msg);
                    if (_msg != null)
                    {
                        DisplayError();
                        return;
                    }

                    grvClientLibraries.DataSource = nodeModules;
                    grvClientLibraries.DataBind();
                    Session["System_ClientLibs"] = nodeModules;

                    string dateFormat = ConfigurationManager.AppSettings["ClientDateFormat"];

                    lblClientDateValue.Text = dateFormat;
                }
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "Page_Load");
            }
        }

        protected void grvXontLibs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow selectedRow = grvXontLibs.SelectedRow;
                string libName = selectedRow.Cells[0].Text.ToString();
                if (libName.ToLower().Contains(".web") || libName.ToLower().Contains(".dal") || libName.ToLower().Contains(".domain") || libName.ToLower().Contains(".bll"))
                {
                    libName = libName.Replace($".{libName.Split('.').Last()}","");
                }

                ComponentVersionInfo cvi = VenturaSystemInfo.GetComponentVersionInfo(libName, false, out _msg);
                if (_msg != null)
                {
                    DisplayError();
                    return;
                }

                ConvertDateTime.GetCurrentCulture();

                grvRelatedLibraries.DataSource = cvi.Level1RefLibs;
                grvRelatedLibraries.DataBind();
                Session["System_Level1Refs"] = cvi.Level1RefLibs;

                grv2ndLayerLibs.DataSource = cvi.Level2RefLibs;
                grv2ndLayerLibs.DataBind();
                Session["System_Level2Refs"] = cvi.Level2RefLibs;

                divLevel1RefLibs.Visible = cvi.Level1RefLibs.Count > 0;
                divLevel2RefLibs.Visible = cvi.Level2RefLibs.Count > 0;

            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvXontLibs_SelectedIndexChanged");
            }
        }


        protected void grvXontLibs_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<LibInfo> xontAssembliesInfo = Session["System_XONTLibs"] as List<LibInfo>;
                e.SortDirection = DynamicQueryable.GridSortDirectionChange(sender, e.SortExpression, e.SortDirection);
                if (xontAssembliesInfo != null)
                {
                    if (e.SortDirection == SortDirection.Ascending)
                    {
                        xontAssembliesInfo = xontAssembliesInfo.AsQueryable().OrderBy(e.SortExpression).ToList();
                    }
                    else
                    {
                        xontAssembliesInfo = xontAssembliesInfo.AsQueryable().OrderByDescending(e.SortExpression).ToList();
                    }
                }

                ConvertDateTime.GetCurrentCulture();
                grvXontLibs.DataSource = xontAssembliesInfo;
                grvXontLibs.DataBind();
                Session["System_XONTLibs"] = xontAssembliesInfo;
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvXontLibs_Sorting");
            }
        }

        protected void grvOtherLiberies_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<LibInfo> thirdPartyAssemblies = Session["System_ThirdPartyLibs"] as List<LibInfo>;
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
                Session["System_ThirdPartyLibs"] = thirdPartyAssemblies;
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvOtherLiberies_Sorting");
            }
        }

        protected void grvClientLibraries_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<LibInfo> nodeModules = Session["System_ClientLibs"] as List<LibInfo>;
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
                Session["System_ClientLibs"] = nodeModules;
            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvClientLibraries_Sorting");
            }
        }

        protected void grvRelatedLibraries_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<RefLibInfo> relatedAssembliesInfo = Session["System_Level1Refs"] as List<RefLibInfo>;
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
                Session["System_Level1Refs"] = relatedAssembliesInfo;

            }
            catch (Exception ex)
            {
                displayExceptionError(ex, "grvRelatedLibraries_Sorting");
            }
        }

        protected void grv2ndLayerLibs_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<LibInfo> SecondLayerRefLibsInfo = Session["System_Level2Refs"] as List<LibInfo>;
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
                Session["System_Level2Refs"] = SecondLayerRefLibsInfo;
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