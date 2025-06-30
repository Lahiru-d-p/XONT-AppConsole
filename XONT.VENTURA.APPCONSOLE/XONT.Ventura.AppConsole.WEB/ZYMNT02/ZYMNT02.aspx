<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZYMNT02.aspx.cs" Inherits="XONT.VENTURA.ZYMNT02.ZYMNT02" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="XONT.Common.CustomControls" Namespace="XONT.Common.CustomControls" TagPrefix="cc3" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: ZYMNT02 ::</title>
    <script src="../js/gridcheckbox.js" type="text/javascript"></script>
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/gridviewScroll.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function pageLoad() {
            var gridWidth = $('.Gridtablestyle').width() + 21;
            $('.Gridtablestyle').gridviewScroll({
                width: gridWidth,
                height: 385
            });
        }

    </script>
    <script src="../js/Disable_BackSpace.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">


        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdateProgress ID="uprProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="120">
            <ProgressTemplate>
                <div id="divProgressBack" runat="server">
                    <div id="dvProgress" class="loading"></div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlLoadSelections" runat="server" CssClass="collapsePanelHeader" Height="20px">
                    <asp:Image ID="imgCollapseClassific" runat="server" />
                    &nbsp;<asp:Label ID="lblLoadSelections" runat="server" CssClass="Linkboldtext">Selection Criteria</asp:Label>
                </asp:Panel>
                <cc1:CollapsiblePanelExtender ID="cpeSelectionCriteria" runat="server" TargetControlID="pnlSelection"
                    ExpandControlID="pnlLoadSelections" CollapseControlID="pnlLoadSelections" TextLabelID="lblLoadSelections"
                    CollapsedText="Selection Criteria" ExpandedText="Hide" ImageControlID="imgCollapseClassific"
                    Collapsed="true" ExpandedImage="~/images/imgdown.png" CollapsedImage="~/images/imgup.png"
                    SuppressPostBack="True" Enabled="True"></cc1:CollapsiblePanelExtender>

                <asp:Panel ID="pnlSelection" runat="server" CssClass="Commenstyle">
                    <table>
                        <tr>
                            <td class="Captionstyle">
                                <label id="lblTaskCode">
                                    Task Code
                                </label>
                            </td>
                            <td style="padding-left: 20px;">
                                <asp:TextBox ID="txtTaskCode" runat="server" CssClass="Textboxstyle" MaxLength="9" Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Captionstyle">
                                <label id="lblDescription">
                                    Description
                                </label>
                            </td>
                            <td style="padding-left: 20px;">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="Textboxstyle" MaxLength="50"
                                    Width="340px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="Captionstyle">
                                <label id="lblModuleCode">
                                    Module Code
                                </label>
                            </td>
                            <td style="padding-left: 20px;">
                                <asp:TextBox ID="txtModuleCode" MaxLength="2" runat="server" CssClass="Textboxstyle"
                                    Width="120px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table style="padding-left: 100px;">
                        <tr>


                            <td>

                                <asp:Panel ID="Panel1" runat="server" GroupingText="Search type" CssClass="GridCaptionstyle">
                                    <asp:RadioButtonList ID="rblSelection" runat="server" CssClass="Captionstyle"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Value="0" Selected="True">Start with</asp:ListItem>
                                        <asp:ListItem Value="1">Anywhere in text</asp:ListItem>
                                    </asp:RadioButtonList>
                                </asp:Panel>

                            </td>


                        </tr>

                        <tr>

                            <td class="style1" style="padding-left: 5px;">
                                <asp:CheckBox ID="chkActiveOnly" runat="server" Checked="true"
                                    CssClass="Checkboxstyle" Text="Active Only" />
                            </td>
                        </tr>
                    </table>
                    <table style="padding-left: 100px;">
                        <tr>

                            <td height="40">
                                <asp:Button ID="btnList" runat="server" Text="List"
                                    CssClass="MainButtonStyle" OnClick="btnList_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>


                <table>
                    <tr>
                        <td>
                            <cc3:GridExports ID="GridExports1" runat="server" ComponentID="ZYMNT02" OnPagingToggleClick="GridExports1_PagingToggleClick" GrvHeader="Tasks" AssociateGridID="grvTasks" />
                            <asp:GridView ID="grvTasks" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
                                CellPadding="2" CssClass="Gridtablestyle"
                                OnPageIndexChanging="grvTask_PageIndexChanging" OnPageIndexChanged="grvTask_PageIndexChanged"
                                EmptyDataText="No data found for the given criteria" EmptyDataRowStyle-CssClass="Labelstyle"
                                AllowSorting="True" OnSorting="grvtask_Sorting" OnRowDataBound="grvTasks_RowDataBound">
                                <RowStyle ForeColor="#000066" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="chk" runat="server" onclick="javascript:SelectOneCheckBox('grvTasks','chkSelectValGrid');" />
                                            <asp:HiddenField ID="Status" runat="server" Value='<%# Eval("Enabled") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="TaskCode"
                                        HeaderText="Task Code" SortExpression="TaskCode">
                                        <HeaderStyle CssClass="GridPadding" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="Description"
                                        HeaderText="Description" SortExpression="Description">
                                        <HeaderStyle CssClass="GridPadding" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="Caption"
                                        HeaderText="Caption" SortExpression="Caption">
                                        <HeaderStyle CssClass="GridPadding" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="ApplicationCode"
                                        HeaderText="Module Code" SortExpression="ApplicationCode">
                                        <HeaderStyle CssClass="GridPadding" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="Version"
                                        HeaderText="Version" SortExpression="Version">
                                        <HeaderStyle CssClass="GridPadding" />
                                    </asp:BoundField>
                                </Columns>
                                <HeaderStyle CssClass="GridviewScrollHeader" />
                                <RowStyle CssClass="GridviewScrollItem" />
                                <PagerStyle CssClass="GridviewScrollPager" />
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <SelectedRowStyle CssClass="GridviewScrollSelected" />
                            </asp:GridView>
                            <input type="hidden" id="chkSelectValGrid" runat="server" />
                        </td>
                    </tr>

                    <td class="pagingAreaStyle">
                        <table style="width: 230px">
                            <tr>
                                <td>
                                    <asp:Button ID="btnFisrt" runat="server" BorderStyle="None" BorderWidth="0px"
                                        CssClass="skipfwrdleft" Height="22px" Width="24px"
                                        OnClick="btnFisrt_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPrev" runat="server" BorderStyle="None" BorderWidth="0px"
                                        CssClass="fwrdleft" Height="22px" Width="24px" OnClick="btnPrev_Click" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCurrentPage" Text="1" TabIndex="-1" runat="server" CssClass="Textboxstyle"
                                        ReadOnly="True" Width="40px"></asp:TextBox>
                                </td>
                                <td align="center" width="25">OF</td>
                                <td>
                                    <asp:TextBox ID="txtLastPage" runat="server" TabIndex="-1" CssClass="Textboxstyle"
                                        ReadOnly="True" Width="40px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btnNext" runat="server" BorderStyle="None" BorderWidth="0px"
                                        CssClass="fwrdright" Height="22px" Width="24px" OnClick="btnNext_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnLast" runat="server" BorderStyle="None" BorderWidth="0px"
                                        CssClass="skipfwrdright" Height="22px" Width="24px"
                                        OnClick="btnLast_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    </tr>

                </table>

                <asp:UpdatePanel ID="uplButtonArea" runat="server">
                    <ContentTemplate>
                        <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblSelectOneError" runat="server" CssClass="Errormessagetextstyle" Text="Please select a record first." Visible="false" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <tr>
                            <td height="40">
                                <asp:Button ID="btnNew" runat="server" CssClass="MainButtonStyle"
                                    meta:resourcekey="btnNewResource1" Text="New" OnClick="btnNew_Click" />
                                <asp:Button ID="btnNewBasedOn" runat="server" CssClass="MainButtonStyle"
                                    meta:resourcekey="btnNewResource1" Text="NewBasedOn"
                                    OnClick="btnNewBasedOn_Click" />
                                <asp:Button ID="btnEdit" runat="server" CssClass="MainButtonStyle"
                                    meta:resourcekey="btnEditResource1" Text="Edit" OnClick="btnEdit_Click" />
                                <asp:Button ID="btnOK" runat="server" CssClass="MainButtonStyle"
                                    meta:resourcekey="btnEditResource1" Text="Exit" OnClick="btnExit_Click" />
                            </td>
                        </tr>
                </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
