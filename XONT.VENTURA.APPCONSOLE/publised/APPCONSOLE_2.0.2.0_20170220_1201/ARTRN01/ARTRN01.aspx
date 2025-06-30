<%@ Page Language="C#" AutoEventWireup="True"  CodeBehind="ARTRN01.aspx.cs"  Inherits="XONT.Ventura.ARTRN01.ARTRN01" %>
<%--<%@ Page Language="C#" AutoEventWireup="True" runat= "server"  %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="XONT.Ventura.Common.Prompt.Web" Namespace="XONT.Ventura.Common.Prompt"
    TagPrefix="cc2" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::: ARTRN01 :::</title>

    <script type="text/javascript">
        function SelectAllCheckboxesSpecific(spanChk, gridName) {

            var IsChecked = spanChk.checked;

            var Chk = spanChk;

            Parent = document.getElementById(gridName);

            var items = Parent.getElementsByTagName('input');

            for (i = 0; i < items.length; i++) {

                if (items[i].id != Chk && items[i].type == "checkbox") {

                    if (items[i].checked != IsChecked) {

                        items[i].click();

                    }

                }

            }
        }        
 
    </script>

    <style type="text/css">
        .style1
        {
            height: 53px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdateProgress ID="uprProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
            DisplayAfter="120">
            <ProgressTemplate>
                <div id="divProgressBack" runat="server" style="position: absolute; width: 100%;
                    z-index: 1000; height: 100%; top: 0; left: 0; background-color: White;
                   " />
                <img class="loading" alt="" align="center" style="position: absolute; vertical-align: middle;
                    left: 35%; top: 25%;" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlInvoiceCol" runat="server" CssClass="collapsePanelHeader" Height="30px">
                    <asp:Image ID="imgCollapseClassific" runat="server" />
                    &nbsp;<asp:Label ID="lblInvoice" runat="server" CssClass="Linkboldtext">Searching Criteria</asp:Label>
                </asp:Panel>
                <cc1:CollapsiblePanelExtender ID="cpeInvoice" runat="server" TargetControlID="pnlInvoice"
                    ExpandControlID="pnlInvoiceCol" CollapseControlID="pnlInvoiceCol" TextLabelID="lblInvoice"
                    CollapsedText="Searching Criteria" ExpandedText="Hide" ImageControlID="imgCollapseClassific"
                    Collapsed="True" ExpandedImage="~/images/imgdown.png" CollapsedImage="~/images/imgup.png"
                    SuppressPostBack="true">
                </cc1:CollapsiblePanelExtender>
                <asp:Panel ID="pnlInvoice" runat="server">
                    <table class="Commenstyle">
                        <tr>
                            <td>
                                <asp:Label ID="lblTransactionCode" runat="server" Text="Transaction Code" CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionCode" runat="server" CssClass="Textboxstyle" Width="120px"
                                    OnTextChanged="txtTransactionCode_TextChanged" MaxLength="5"></asp:TextBox>
                                <cc1:AutoCompleteExtender ServiceMethod="SearchTransactionCode" MinimumPrefixLength="2"
                                    CompletionInterval="100" EnableCaching="false" CompletionSetCount="100" TargetControlID="txtTransactionCode"
                                    ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false">
                                </cc1:AutoCompleteExtender>
                                <%--<cc1:AutoCompleteExtender ID="aceTransactionCode" runat="server" MinimumPrefixLength="1"
                                    ServiceMethod="GetTransactionCodes" ServicePath="~/WebService1.asmx" TargetControlID="txtTransactionCode"
                                    EnableCaching="true" CompletionSetCount="12">
                                </cc1:AutoCompleteExtender>--%>
                                <asp:Button ID="btnFindTransactionCode" runat="server" BorderStyle="None" OnClick="btnFindTransactionCode_Click"
                                    CausesValidation="False" CssClass="FindButton" Height="16px" Width="20px" />
                                <asp:TextBox ID="txtTransactionCodeDesc" runat="server" CssClass="InactivTxtboxstyle"
                                    ReadOnly="True" Width="250px" 
                                    ontextchanged="txtTransactionCodeDesc_TextChanged" TabIndex="-1"></asp:TextBox>
                                <asp:Label ID="lblTransactionCodeError" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblDocDateFrom" runat="server" Text="Doc Date From" CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocDateFrom" runat="server" CssClass="Textboxstyle" Width="120px"></asp:TextBox>
                                <cc1:CalendarExtender ID="calDocDateFrom" runat="server" TargetControlID="txtDocDateFrom"
                                    PopupButtonID="btnDocDateFrom" PopupPosition="TopRight" />
                            </td>
                            <td>
                                <asp:Button ID="btnDocDateFrom" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    CausesValidation="False" CssClass="calender" Height="17px" Text="" Width="20px" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblTo" runat="server" CssClass="Captionstyle" Text="To"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocDateTo" runat="server" CssClass="Textboxstyle" Width="120px"></asp:TextBox>
                                <cc1:CalendarExtender ID="calDocDateTo" runat="server" TargetControlID="txtDocDateTo"
                                    PopupButtonID="btnDocDateTo" PopupPosition="TopLeft" />
                            </td>
                            <td>
                                <asp:Button ID="btnDocDateTo" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    CausesValidation="False" CssClass="calender" Height="17px" Text="" Width="20px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDocumentNo" runat="server" Text="Document No." CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDocumentNo" runat="server" MaxLength="20" Width="120px" CssClass="Textboxstyle"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="4">
                                <asp:Label ID="lblDocDateFromError" runat="server" CssClass="Errormessagetextstyle" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblDocDateToError" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCustomer" runat="server" CssClass="Captionstyle" Text="Customer"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="Textboxstyle" Width="120px"
                                    MaxLength="15"></asp:TextBox>
                                <cc1:AutoCompleteExtender ServiceMethod="SearchCustomer" MinimumPrefixLength="2"
                                    CompletionInterval="100" EnableCaching="false" CompletionSetCount="100" TargetControlID="txtCustomer"
                                    ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false">
                                </cc1:AutoCompleteExtender>
                                <%-- <cc1:AutoCompleteExtender ID="aceCustomer" runat="server" CompletionSetCount="12"
                                    EnableCaching="true" MinimumPrefixLength="1" ServiceMethod="GetCustomerCodes"
                                    ServicePath="~/WebService1.asmx" TargetControlID="txtCustomer">
                                </cc1:AutoCompleteExtender>--%>
                                <asp:Button ID="btnFindCustomer" runat="server" BorderStyle="None" CausesValidation="False"
                                    CssClass="FindButton" Height="16px" OnClick="btnFindCustomer_Click" Width="20px" />
                                <asp:TextBox ID="txtCustomerDesc" runat="server" CssClass="InactivTxtboxstyle" ReadOnly="True"
                                    Width="250px" Enabled="False" TabIndex="-1"></asp:TextBox>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblReference" runat="server" CssClass="Captionstyle" Text="Reference"></asp:Label>
                            </td>
                            <td colspan="8">
                                <asp:TextBox ID="txtReference" runat="server" CssClass="Textboxstyle" MaxLength="20"
                                    Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="style1">
                            </td>
                            <td class="style1">
                                <fieldset id="fsStatus" runat="server" class="Commenstyle" title="Status">
                                    <legend id="lStatus" runat="server">Status </legend>
                                    <asp:CheckBoxList ID="chkStatusList" runat="server" CssClass="Captionstyle" RepeatDirection="Horizontal">
                                        <asp:ListItem Enabled="true" Selected="True" Text="Open" Value="Open"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Text="Posted" Value="Posted"></asp:ListItem>
                                        <asp:ListItem Enabled="true" Text="Cancelled" Value="Cancelled"></asp:ListItem>
                                    </asp:CheckBoxList>
                                </fieldset>
                            </td>
                            <td class="style1">
                                &nbsp;
                            </td>
                            <td colspan="9" class="style1">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <asp:Button ID="btnList" runat="server" CssClass="MainButtonStyle" OnClick="btnList_Click"
                                    Text="List" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="9" height="40">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table>
                    <tr>
                        <td>
                            <%--<asp:Panel ID="pnlInvoicesScroll" runat="server">--%>
                            <div id="GridScroll" class="DivScroll DivScroll2" style="width: 1000px; overflow: auto;">
                                <asp:GridView ID="grvInvoices" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" CssClass="Gridtablestyle"
                                    meta:resourcekey="GridView1Resource1" EmptyDataRowStyle-CssClass="Labelstyle"
                                    EmptyDataText="No Data found matching the given criteria" OnPageIndexChanging="grvInvoices_PageIndexChanging"
                                    OnRowDataBound="grvInvoices_RowDataBound" AllowSorting="True" OnSorting="grvInvoices_Sorting"
                                    Width="1000px">
                                    <RowStyle ForeColor="#000066" />
                                    <RowStyle ForeColor="#000066" />
                                    <EmptyDataRowStyle CssClass="Labelstyle" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Select">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chk" />
                                                <asp:HiddenField ID="hfID" runat="server" Value='<%# Eval("hdnDocNuSys") %>' />
                                                <asp:HiddenField ID="hfCurrencyAmount" runat="server" Value='<%# Eval("hdnCurrencyAmount") %>' />
                                                <asp:HiddenField ID="hfCustomerCode" runat="server" Value='<%# Eval("CustomerCode") %>' />
                                            </ItemTemplate>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxesSpecific(this,'grvInvoices');"
                                                    runat="server" />
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Status" HeaderStyle-CssClass="GridPadding" SortExpression="Status"
                                            HeaderText="Status">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Txn Code" HeaderStyle-CssClass="GridPadding" SortExpression="Txn Code"
                                            HeaderText="Txn Code">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Doc Number" HeaderStyle-CssClass="GridPadding" SortExpression="Doc Number"
                                            HeaderText="Doc Number">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                        <%--<asp:BoundField DataField="Doc Date" DataFormatString="{0:d}" SortExpression="Doc Date"
                                        HeaderStyle-CssClass="GridPadding" HeaderText="Doc Date">
                                        <HeaderStyle CssClass="GridPadding" />
                                    </asp:BoundField>--%>
                                        <asp:BoundField DataField="DisplayDate" SortExpression="Doc Date" HeaderStyle-CssClass="GridPadding"
                                            HeaderText="Doc Date">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Customer" HeaderStyle-CssClass="GridPadding" SortExpression="Customer"
                                            HeaderText="Customer">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Reference" HeaderStyle-CssClass="GridPadding" SortExpression="Reference"
                                            HeaderText="Reference">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Currency" HeaderStyle-CssClass="GridPadding" SortExpression="Currency"
                                            HeaderText="Currency">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Amount" HeaderStyle-CssClass="GridPadding" SortExpression="Amount"
                                            HeaderText="Amount">
                                            <HeaderStyle CssClass="GridPadding" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Created By" HeaderStyle-CssClass="GridPadding" SortExpression="Created By"
                                            HeaderText="Created By">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="hdnCustomerCode" HeaderStyle-CssClass="GridPadding" Visible="false"
                                            HeaderText="hdnCustomerCode">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="hdnCurrencyCode" HeaderStyle-CssClass="GridPadding" Visible="false"
                                            HeaderText="hdnCurrencyCode">
                                            <HeaderStyle CssClass="GridPadding" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" CssClass="GridPadding" />
                                </asp:GridView>
                            </div>
                            <%--</asp:Panel>--%>
                        </td>
                    </tr>
                    <tr>
                        <td height="40">
                            <asp:Button ID="btnNew" runat="server" CausesValidation="False" CssClass="MainButtonStyle"
                                OnClick="btnNew_Click" Text="New" />
                            <asp:Button ID="btnEdit" runat="server" CssClass="MainButtonStyle" OnClick="btnEdit_Click"
                                Text="Edit" />
                            <asp:Button ID="btnPrintDocument" runat="server" CssClass="MainButtonStyle" Text="Print Document"
                                Visible="false" />
                            <asp:Button ID="btnReport" runat="server" CssClass="MainButtonStyle" Text="Report"
                                Visible="false" />
                            <%--<asp:Button ID="btnDeleted" runat="server" CssClass="MainButtonStyle" OnClick="btnDeleted_Click"
                                Text="Delete" />--%>
                            <input id="btnDeleted" onclick="this.disabled=true;" type="button" value="Delete"
                                name="btnDeleted" runat="server" onserverclick="btnDeleted_Click" class="MainButtonStyle">
                            <%--<asp:Button ID="btnPost" runat="server" CssClass="MainButtonStyle" OnClick="btnPost_Click"
                                Text="Post" />--%>
                            <input id="btnPost" onclick="this.disabled=true;" type="button" value="Post" name="btnPost"
                                runat="server" onserverclick="btnPost_Click" class="MainButtonStyle">
                            <asp:Button ID="btnExit" runat="server" CausesValidation="false" CssClass="MainButtonStyle"
                                OnClientClick="javascript:window.parent.document.getElementById('btnClose').onclick();"  Text="Exit" />
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="Errormessagetextstyle" />
                            <cc2:ModalPrompt ID="mptTransactionCode" runat="server" ButtonCssClass="PromptButtonStyle"
                                EmptyDataText="No Data found to display" GridViewCssClass="PromptGridtablestyle"
                                HeadingCssClass="PromptCaptionstyle" HideFirstColumn="False" OnCancelled="mptTransactionCode_Cancelled"
                                OnItemSelected="mptTransactionCode_ItemSelected" PageSize="10" PromptTitle=""
                                ReturnAllFields="False" TableCssClass="PromptBasestyle" />
                            <cc2:ModalPrompt ID="mpCustomer" runat="server" ButtonCssClass="PromptButtonStyle" PromptTitle="Customers"
                                EmptyDataText="No Data found to display" GridViewCssClass="PromptGridtablestyle"
                                HeadingCssClass="PromptCaptionstyle" HideFirstColumn="False" OnCancelled="mpCustomer_Cancelled"
                                OnItemSelected="mpCustomer_ItemSelected" PageSize="10" ReturnAllFields="False"
                                TableCssClass="PromptBasestyle" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
