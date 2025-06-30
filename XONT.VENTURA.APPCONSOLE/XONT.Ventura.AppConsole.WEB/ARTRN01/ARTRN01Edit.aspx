<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ARTRN01Edit.aspx.cs" Inherits="XONT.Ventura.ARTRN01.Web.ARTRN01.ARTRN01Edit" %>

<%@ Register Assembly="XONT.Ventura.Common.Prompt.Web" Namespace="XONT.Ventura.Common.Prompt"
    TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::: ARTRN01Edit :::</title>
    <style type="text/css">
        .addico
        {
            width: 22px;
            height: 22px;
            border-style: none;
            padding: none;
            cursor: pointer;
            background: url( "../images/addico.png" ) no-repeat;
        }
        .removeico
        {
            width: 22px;
            height: 22px;
            border-style: none;
            padding: none;
            cursor: pointer;
            background: url( "../images/removeico.png" ) no-repeat;
        }
        .deleteico
        {
            width: 22px;
            height: 22px;
            border-style: none;
            padding: none;
            cursor: pointer;
            background: url( "../images/deleteico.png" ) no-repeat;
        }
        .checkico
        {
            width: 22px;
            height: 22px;
            border-style: none;
            padding: none;
            cursor: pointer;
            background: url( "../images/checkico.png" ) no-repeat;
        }
    </style>

    <script language="javascript" type="text/javascript">
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

        function disablebuttons() {
            document.getElementById('btnDelete').disabled = true;
            document.getElementById('btnSave').disabled = true;
            document.getElementById('btnSavePost').disabled = true;
        }
        
        
       

    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="disablebuttons();">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <table class="Commenstyle">
                    <tr>
                        <td width="120">
                            <asp:Label ID="lblTransactionCode" runat="server" Text="Transaction Code" CssClass="Captionstyle"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionCode" runat="server" AutoPostBack="true" OnTextChanged="txtTransactionCode_TextChanged"
                                CssClass="Textboxstyle" Width="120px" MaxLength="5"></asp:TextBox>
                            <cc1:AutoCompleteExtender ServiceMethod="SearchTransactionCode" MinimumPrefixLength="2"
                                CompletionInterval="100" EnableCaching="false" CompletionSetCount="100" TargetControlID="txtTransactionCode"
                                ID="AutoCompleteExtender2" runat="server" FirstRowSelected="false">
                            </cc1:AutoCompleteExtender>
                            <%-- <cc1:AutoCompleteExtender ID="aceTransactionCode" runat="server" MinimumPrefixLength="1" ServiceMethod="GetTransactionCodes" 
                                ServicePath="~/WebService1.asmx" TargetControlID="txtTransactionCode" EnableCaching="true" CompletionSetCount="12" > 
                            </cc1:AutoCompleteExtender>--%>
                        </td>
                        <cc1:TextBoxWatermarkExtender ID="TransactionCodeWatermarkExtender" runat="server"
                            TargetControlID="txtTransactionCode" WatermarkText="&lt;Transaction Code &gt;"
                            Enabled="True" />
                        <td>
                            <asp:Button ID="btnFindTransactionCode" runat="server" BorderStyle="None" CausesValidation="False"
                                CssClass="FindButton" Height="16px" OnClick="btnFindTransactionCode_Click" Width="20px" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionCodeDesc" runat="server" ReadOnly="True" CssClass="InactivTxtboxstyle"
                                Width="220px" TabIndex="-1"></asp:TextBox>
                            <asp:Label ID="lblTransactionCodeError" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvTransactionCode" runat="server" ErrorMessage="*"
                                ControlToValidate="txtTransactionCode" CssClass="Errormessagetextstyle" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                        <td width="18">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblCustomerRef" runat="server" Text="Customer Ref" CssClass="Captionstyle"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerRef" runat="server" MaxLength="20" CssClass="Textboxstyle"
                                Width="100px"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblReference" runat="server" CssClass="Captionstyle" Text="Reference"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReference" runat="server" CssClass="Textboxstyle" MaxLength="20"
                                Width="110px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblDocNumber" runat="server" Text="Document Number" CssClass="Captionstyle"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDocNumber" runat="server" MaxLength="20" CssClass="Textboxstyle"
                                Width="120px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvDocNumber" runat="server" ErrorMessage="*" ControlToValidate="txtDocNumber"
                                CssClass="Errormessagetextstyle" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkAuto" runat="server" AutoPostBack="true" Checked="True" CssClass="Checkboxstyle"
                                OnCheckedChanged="chkAuto_CheckedChanged" Text="Auto" />
                            &nbsp;
                            <asp:Button ID="btnCheck" runat="server" CausesValidation="False" CssClass="checkico"
                                OnClick="btnCheck_Click" Height="22px" Width="22px" ToolTip="Check" />
                        </td>
                        <td>
                        </td>
                        <td rowspan="7" valign="top">
                            <fieldset id="fsDescription" runat="server" title="Description">
                                <legend id="lDescription" runat="server" class="GridCaptionstyle" style="height: 25px">
                                    Description</legend>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="Textboxstyle" Height="100px"
                                    TextMode="MultiLine" Width="375px"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtDescription_TextBoxWatermarkExtender" runat="server"
                                    Enabled="True" TargetControlID="txtDescription" WatermarkText="&lt;Description &gt;">
                                </cc1:TextBoxWatermarkExtender>
                                <br />
                                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription"
                                    ErrorMessage="*" CssClass="Errormessagetextstyle" Display="Dynamic"></asp:RequiredFieldValidator>
                            </fieldset>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            <asp:Label ID="lblCustomer" runat="server" Text="Customer" CssClass="Captionstyle"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="Textboxstyle" Width="120px"
                                AutoPostBack="True" MaxLength="15" OnTextChanged="txtCustomer_TextChanged"></asp:TextBox>
                            <cc1:AutoCompleteExtender ServiceMethod="SearchCustomer" MinimumPrefixLength="2"
                                CompletionInterval="100" EnableCaching="false" CompletionSetCount="100" TargetControlID="txtCustomer"
                                ID="AutoCompleteExtender3" runat="server" FirstRowSelected="false">
                            </cc1:AutoCompleteExtender>
                            <%--  <cc1:AutoCompleteExtender ID="aceCustomer" runat="server" 
                                CompletionSetCount="12" EnableCaching="true" MinimumPrefixLength="1" 
                                ServiceMethod="GetCustomerCodes" ServicePath="~/WebService1.asmx" 
                                TargetControlID="txtCustomer">
                            </cc1:AutoCompleteExtender>--%>
                        </td>
                        <cc1:TextBoxWatermarkExtender ID="RetailerCodeWatermarkExtender" runat="server" TargetControlID="txtCustomer"
                            WatermarkText="&lt;Customer Code &gt;" Enabled="True" />
                        <td>
                            <asp:Button ID="btnFindCustomer" runat="server" BorderStyle="None" CausesValidation="False"
                                CssClass="FindButton" Height="16px" OnClick="btnFindCustomer_Click" Width="20px" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerDesc" runat="server" CssClass="InactivTxtboxstyle" ReadOnly="True"
                                Width="220px" TabIndex="-1"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="txtCustomer"
                                CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblCustomerError" runat="server" CssClass="Errormessagetextstyle" />
                        </td>
                        <td rowspan="6" width="15">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblTerritory" runat="server" CssClass="Captionstyle" Text="Territory"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTerritory" runat="server" CssClass="InactivTxtboxstyle" Width="120px"
                                AutoPostBack="True" MaxLength="4" OnTextChanged="txtTerritory_TextChanged" ReadOnly="True"></asp:TextBox>
                            <%-- <cc1:AutoCompleteExtender ID="aceterritory" runat="server" 
                                CompletionSetCount="12" EnableCaching="true" MinimumPrefixLength="1" 
                                ServiceMethod="GetTerritoyCodes" ServicePath="~/WebService1.asmx" 
                                TargetControlID="txtTerritory">
                            </cc1:AutoCompleteExtender>--%>
                        </td>
                        <td>
                            <asp:Button ID="btnFindTerritory" runat="server" BorderStyle="None" CausesValidation="False"
                                CssClass="FindButton" Enabled="False" Height="16px" OnClick="btnFindTerritory_Click"
                                Visible="False" Width="20px" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTerritoryDesc" runat="server" CssClass="InactivTxtboxstyle" ReadOnly="True"
                                Width="220px" TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDocDate" runat="server" CssClass="Captionstyle" Text="Doc Date"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDocDate" runat="server" AutoPostBack="True" CssClass="Textboxstyle"
                                OnTextChanged="txtDocDate_TextChanged" Width="120px"></asp:TextBox>
                            <asp:Button ID="btnDocDate" runat="server" BorderColor="Transparent" BorderStyle="None"
                                BorderWidth="0px" CssClass="calender" Height="17px" OnClick="btnDocDate_Click"
                                Text="..." ValidationGroup="ShowCalender" Width="20px" />
                            <div style="position: absolute">
                                <asp:Calendar ID="CalendarExtDocDate" runat="server" BackColor="White" BorderColor="#999999"
                                    CellPadding="3" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt"
                                    ForeColor="Black" Height="101px" OnDayRender="CalendarExtDocDate_DayRender" OnSelectionChanged="CalendarExtDocDate_SelectionChanged"
                                    Width="118px">
                                    <SelectedDayStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                    <SelectorStyle BackColor="#669999" />
                                    <WeekendDayStyle BackColor="#FFFFCC" />
                                    <TodayDayStyle BackColor="#CCCCCC" BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px"
                                        ForeColor="Black" />
                                    <OtherMonthDayStyle ForeColor="Gray" HorizontalAlign="Left" VerticalAlign="Top" Wrap="True" />
                                    <NextPrevStyle ForeColor="White" VerticalAlign="Bottom" />
                                    <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
                                    <TitleStyle BackColor="#006699" BorderColor="White" Font-Bold="True" ForeColor="White"
                                        Height="12px" />
                                </asp:Calendar>
                            </div>
                            <%-- <cc1:CalendarExtender ID="calDocDate" runat="server" 
                                TargetControlID="txtDocDate" PopupButtonID="btnDocDate" PopupPosition="Right"/>
                            <asp:Button ID="btnDocDate" runat="server" BorderColor="Transparent" BorderWidth="0px"
                            CssClass="calender" Height="17px" Text="" Width="20px" 
                            CausesValidation="False"/>--%>
                            <asp:RequiredFieldValidator ID="rfvDocDate" runat="server" ControlToValidate="txtDocDate"
                                CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </td>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtDocDate"
                            WatermarkText="&lt;Doc Date &gt;" Enabled="True" />
                        <td>
                            <asp:TextBox ID="txtPostingYearPeriod" runat="server" CssClass="InactivTxtboxstyle"
                                ReadOnly="True" Width="220px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPostingYearPeriod" runat="server" ControlToValidate="txtPostingYearPeriod"
                                CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="No period end dates defined"></asp:RequiredFieldValidator>
                            <%--<asp:RegularExpressionValidator ID="revDate" runat="server" 
                                ControlToValidate="txtDocDate" CssClass="Errormessagetextstyle" 
                                Display="Dynamic" ErrorMessage="Enter valid Date" 
                                ValidationExpression="^\d{1,2}\/\d{1,2}\/\d{4}$"></asp:RegularExpressionValidator>--%>
                        </td>
                        <td>
                            <asp:Label ID="lblDocDateError" runat="server" CssClass="Errormessagetextstyle" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSettlementTerm" runat="server" CssClass="Captionstyle" Text="Settlement Term"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSettlementTerm" runat="server" CssClass="Textboxstyle" ReadOnly="True"
                                Width="120px" OnTextChanged="txtSettlementTerm_TextChanged"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnFindSettlementTerm" runat="server" BorderStyle="None" CausesValidation="False"
                                CssClass="FindButton" Height="16px" OnClick="btnFindSettlementTerm_Click" Width="20px" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtSettlementTermDesc" runat="server" CssClass="InactivTxtboxstyle"
                                ReadOnly="True" Width="220px" TabIndex="-1"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDueDate" runat="server" CssClass="Captionstyle" Text="Due Date"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDueDate" runat="server" CssClass="Textboxstyle" Width="120px"></asp:TextBox>
                            <cc1:CalendarExtender ID="calDueDate" runat="server" TargetControlID="txtDueDate"
                                PopupButtonID="btnDueDate" PopupPosition="Right" />
                            <asp:Button ID="btnDueDate" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                CssClass="calender" Height="17px" Text="" Width="20px" CausesValidation="False" />
                            <asp:RequiredFieldValidator ID="rfvDueDate" runat="server" ControlToValidate="txtDueDate"
                                ErrorMessage="*" CssClass="Errormessagetextstyle" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                        <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" TargetControlID="txtDueDate"
                            WatermarkText="&lt;Due Date &gt;" Enabled="True" />
                        <td colspan="2">
                            <asp:Label ID="lblDueDateError" runat="server" CssClass="Errormessagetextstyle" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblCurrencyCode" runat="server" CssClass="Captionstyle" Text="Currency Code"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtCurrencyCode" runat="server" CssClass="InactivTxtboxstyle" MaxLength="3"
                                ReadOnly="True" Width="120px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td class="style1">
                            <fieldset id="fsExecutive0" runat="server" title="Executive">
                                <legend id="lgntExecutive0" runat="server" class="GridCaptionstyle">Executive</legend>
                                <table style="padding-bottom: 5px; margin-bottom: 5px;">
                                    <tr>
                                        <td style="margin-top: 10px; margin-bottom: 10px; padding-top: 10px; padding-bottom: 10px;">
                                            <asp:Label ID="lblDocument" runat="server" CssClass="Captionstyle" Text="Document"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDocument" runat="server" CssClass="Textboxstyle" MaxLength="10"
                                                Width="120px" AutoPostBack="True" OnTextChanged="txtDocument_TextChanged"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ServiceMethod="SearchDocumentExe" MinimumPrefixLength="2"
                                                CompletionInterval="100" EnableCaching="false" CompletionSetCount="100" TargetControlID="txtDocument"
                                                ID="AutoCompleteExtender4" runat="server" FirstRowSelected="false">
                                            </cc1:AutoCompleteExtender>
                                            <%-- <cc1:AutoCompleteExtender ID="txtDocument_AutoCompleteExtender" runat="server" 
                                                CompletionSetCount="12" EnableCaching="true" MinimumPrefixLength="1" 
                                                ServiceMethod="GetDocExecutiveCodes" ServicePath="~/WebService1.asmx" 
                                                TargetControlID="txtDocument">
                                            </cc1:AutoCompleteExtender>--%><asp:Button ID="btnFindDocument" runat="server" BorderStyle="None"
                                                CausesValidation="False" CssClass="FindButton" Height="16px" OnClick="btnFindDocument_Click"
                                                Width="20px" />
                                            <asp:TextBox ID="txtDocumentDesc" runat="server" CssClass="InactivTxtboxstyle" MaxLength="40"
                                                ReadOnly="True" Width="180px" TabIndex="-1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDocError" runat="server" CssClass="Errormessagetextstyle" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDocExeUserProfile" runat="server" CssClass="Captionstyle" Text="User Profile"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDocExeUserProCode" runat="server" CssClass="InactivTxtboxstyle"
                                                ReadOnly="true" Width="120px" TabIndex="-1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDocExeUserProfile" runat="server" CssClass="InactivTxtboxstyle"
                                                ReadOnly="true" Width="200px" TabIndex="-1"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblCollection" runat="server" CssClass="Captionstyle" Text="Collection"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCollection" runat="server" CssClass="Textboxstyle" MaxLength="10"
                                                Width="120px" AutoPostBack="True" OnTextChanged="txtCollection_TextChanged"></asp:TextBox>
                                            <cc1:AutoCompleteExtender ServiceMethod="SearchCollectionExe" MinimumPrefixLength="2"
                                                CompletionInterval="100" EnableCaching="false" CompletionSetCount="100" TargetControlID="txtCollection"
                                                ID="AutoCompleteExtender5" runat="server" FirstRowSelected="false">
                                            </cc1:AutoCompleteExtender>
                                            <%-- <cc1:AutoCompleteExtender ID="txtCollection_AutoCompleteExtender" 
                                                runat="server" CompletionSetCount="12" EnableCaching="true" 
                                                MinimumPrefixLength="1" ServiceMethod="GetColExecutiveCodes" 
                                                ServicePath="~/WebService1.asmx" TargetControlID="txtCollection">
                                            </cc1:AutoCompleteExtender>--%><asp:Button ID="btnFindCollection" runat="server"
                                                BorderStyle="None" CausesValidation="False" CssClass="FindButton" Height="16px"
                                                OnClick="btnFindCollection_Click" Width="20px" />
                                            <asp:TextBox ID="txtCollectionDesc" runat="server" CssClass="InactivTxtboxstyle"
                                                MaxLength="40" ReadOnly="True" Width="180px" TabIndex="-1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblColError" runat="server" CssClass="Errormessagetextstyle" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Label ID="lblColExeUserProfile" runat="server" CssClass="Captionstyle" Text="User Profile"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtColExeUserProCode" runat="server" CssClass="InactivTxtboxstyle"
                                                ReadOnly="true" Width="120px" TabIndex="-1"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtColExeUserProfile" runat="server" CssClass="InactivTxtboxstyle"
                                                ReadOnly="true" Width="200px" TabIndex="-1"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlExchangeInfo" runat="server">
                    <table style="padding-bottom: 3px; margin-bottom: 3px;">
                        <tr>
                            <td width="40" height="30">
                                <asp:Label ID="lblRate" runat="server" Text="Rate" CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="txtRate" runat="server" MaxLength="10" AutoPostBack="True" OnTextChanged="txtRate_TextChanged"
                                    CssClass="Textboxstyle" Width="120px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvRate" runat="server" ErrorMessage="*" ControlToValidate="txtRate"
                                    CssClass="Errormessagetextstyle" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td class="style2">
                                <asp:Label ID="lblDate" runat="server" CssClass="Captionstyle" Text="Date"></asp:Label>
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="txtDate" runat="server" CssClass="InactivTxtboxstyle" Width="120px"
                                    ReadOnly="True"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate"
                                    PopupButtonID="btnDate" PopupPosition="Right" />
                                <asp:Button ID="btnDate" runat="server" BorderColor="Transparent" BorderWidth="0px"
                                    CssClass="calender" Height="17px" Text="" Width="20px" CausesValidation="False"
                                    Enabled="False" />
                                <asp:Label ID="lblDateError" runat="server" CssClass="Errormessagetextstyle" />
                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                    CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lblRateError" runat="server" CssClass="Errormessagetextstyle" />
                                <asp:RegularExpressionValidator ID="revRate" runat="server" ControlToValidate="txtRate"
                                    CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="Only Positive Values"
                                    ValidationExpression="^[0-9]*(\.)?[0-9]+$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <fieldset id="fsGridArea" runat="server" title="Details" style="width:1250px" >
                    <legend id="Legend1" runat="server" class="GridCaptionstyle"  >Details</legend>
                    <table style="padding-bottom: 4px; margin-bottom: 4px;">
                        <tr>
                            <td>
                                <asp:Label ID="lblLineDescription" runat="server" Text="Line Description" CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNetAmount" runat="server" Text="Net Amt" CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVatCode" runat="server" CssClass="Captionstyle" Text="VAT Code"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                            </td>
                            <td>
                                <asp:Label ID="lblVATRate" runat="server" Text="VAT Rate " CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVATAmount" runat="server" Text="VAT Amt" CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblGrossAmount" runat="server" Text="Gross Amt" CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblGlCode" runat="server" Text="GL A/C Code" CssClass="Captionstyle"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                               <%-- <asp:Label ID="lblGlName" runat="server" Text="GL A/C Name" CssClass="Captionstyle"></asp:Label>--%>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtLineDescription" MaxLength="40" runat="server" Width="190px"
                                    CssClass="Textboxstyle"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNetAmount" runat="server" MaxLength="13" Width="75px" OnTextChanged="txtNetAmount_TextChanged1"
                                    CssClass="Textboxstyle"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNetAmount" runat="server" ControlToValidate="txtNetAmount"
                                    ErrorMessage="*" ValidationGroup="Detail" CssClass="Errormessagetextstyle" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                            <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" TargetControlID="txtNetAmount"
                                WatermarkText="&lt;Net Amt &gt;" Enabled="True" />
                            <td>
                                <asp:TextBox ID="txtVatCode2" runat="server" MaxLength="2" Width="75px" AutoPostBack="True"
                                    CssClass="Textboxstyle" OnTextChanged="txtVatCode_TextChanged"></asp:TextBox>
                                <cc1:AutoCompleteExtender ServiceMethod="SearchVAT" MinimumPrefixLength="2" CompletionInterval="100"
                                    EnableCaching="false" CompletionSetCount="100" TargetControlID="txtVatCode2"
                                    ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false">
                                </cc1:AutoCompleteExtender>
                                <%--<cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" MinimumPrefixLength="1" ServiceMethod="GetVatCodes" 
                            ServicePath="~/WebService1.asmx" TargetControlID="txtVatCode" EnableCaching="true" CompletionSetCount="12" > 
                            </cc1:AutoCompleteExtender> --%>
                            </td>
                            <td>
                                <asp:Button ID="btnFindVatCode" runat="server" BorderStyle="None" CausesValidation="False"
                                    CssClass="FindButton" Height="16px" Width="20px" OnClick="btnFindVatCode_Click" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtVatCodeDesc" MaxLength="50" runat="server" Width="200px" ReadOnly="True"
                                    CssClass="InactivTxtboxstyle"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVATRate" MaxLength="13" runat="server" ReadOnly="True" CssClass="NumberInactivTxtbox"
                                    Width="75px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVATAmount" MaxLength="13" runat="server" Width="75px" AutoPostBack="True"
                                    OnTextChanged="txtVATAmount_TextChanged" CssClass="Textboxstyle" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGrossAmt" runat="server" MaxLength="13" Width="75px" ReadOnly="True"
                                    CssClass="NumberInactivTxtbox"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGlCode" runat="server" Width="75px" CssClass="InactivTxtboxstyle"
                                    ReadOnly="true"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" TargetControlID="txtGlCode"
                                    WatermarkText="&lt;GL Code &gt;" Enabled="True" />
                            </td>
                            <td>
                                <asp:Button ID="btnFindGlCodes" runat="server" BorderStyle="None" CausesValidation="False"
                                    CssClass="FindButton" Height="16px" OnClick="btnFindGlCodes_Click" Width="20px" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtGlName" MaxLength="50" runat="server" Width="200px" ReadOnly="True"
                                    CssClass="InactivTxtboxstyle"></asp:TextBox>
                            </td>
                            <td>
                                  <asp:RequiredFieldValidator ID="RfvGlCode" runat="server" ControlToValidate="txtGlCode"
                                    CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="*" ValidationGroup="Detail"></asp:RequiredFieldValidator></td>
                            <td>
                                <asp:Button ID="btnAddGrid" runat="server" CssClass="addico" Height="22px" 
                                    OnClick="btnAddGrid_Click" ToolTip="Add" ValidationGroup="Detail" 
                                    Width="22px" />
                            </td>
                            <td>
                                <asp:Button ID="btnDeleteGrid" runat="server" CausesValidation="False" CssClass="deleteico"
                                    Height="22px" OnClick="btnDeleteGrid_Click" Width="22px" ToolTip="Delete" />
                            </td>
                            <td>
                                <asp:Button ID="btnClear" runat="server" CausesValidation="False" CssClass="removeico"
                                    Height="22px" OnClick="btnClear_Click" Width="22px" ToolTip="Clear" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:RegularExpressionValidator ID="revNetAmt" runat="server" ControlToValidate="txtNetAmount"
                                    CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="Only Positive Values"
                                    ValidationExpression="^[0-9]*(\.)?[0-9]+$" ValidationGroup="Detail"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblVatError" runat="server" CssClass="Errormessagetextstyle" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:RegularExpressionValidator ID="revVatAmt" runat="server" ControlToValidate="txtVATAmount"
                                    CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="Only Positive Values"
                                    ValidationExpression="^[0-9]*(\.)?[0-9]+$" ValidationGroup="Detail"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    
                    <asp:Panel ID="pnlViewTotalFields" runat="server">
                        <table>
                            <tr><td style="padding-top: 10px">
                                    <%--<asp:Panel ID="pnlDetailsScroll" runat="server">--%>
                                    <div id="GridScroll" class="DivScroll DivScroll2" style="width: auto; overflow: auto;">
                                        <asp:GridView ID="grvDetails" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="2" CssClass="Gridtablestyle"
                                            meta:resourcekey="GridView1Resource1" AutoGenerateSelectButton="True" OnSelectedIndexChanged="grvDetails_SelectedIndexChanged" >
                                            <RowStyle ForeColor="#000066" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk" runat="server" />
                                                        <asp:HiddenField ID="hdnDocumentNumberSystem" runat="server" Value='<%# Eval("DocumentNumberSystem") %>' />
                                                        <asp:HiddenField ID="hdnSequence" runat="server" Value='<%# Eval("Sequence") %>' />
                                                        <asp:HiddenField ID="hdnVATAmountSystem" runat="server" Value='<%# Eval("VATAmountSystem") %>' />
                                                        <asp:HiddenField ID="hdnNetAmountCurrency" runat="server" Value='<%# Eval("NetAmountCurrency") %>' />
                                                        <asp:HiddenField ID="hdnVATAmountCurrency" runat="server" Value='<%# Eval("VATAmountCurrency") %>' />
                                                        <asp:HiddenField ID="hdnGrossAmountCurrency" runat="server" Value='<%# Eval("GrossAmountCurrency") %>' />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="40px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Line Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLineDescription" runat="server" CssClass="Captionstyle" Text='<%# Bind("LineDescription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="220px" />
                                                    <HeaderStyle CssClass="GridPadding" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Net Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNetAmount" runat="server" CssClass="Captionstyle" Text='<%# Bind("NetAmountCurrency") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="108px" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="GridPadding" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVATCode" runat="server" CssClass="Captionstyle" Text='<%# Bind("VATCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="108px" />
                                                    <HeaderStyle CssClass="GridPadding" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT Rate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVATRate" runat="server" CssClass="Captionstyle" Text='<%# Bind("VATRate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="108px" />
                                                    <HeaderStyle CssClass="GridPadding" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="VAT Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVATAmount" runat="server" CssClass="Captionstyle" Text='<%# Bind("VATAmountCurrency") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="108px" />
                                                    <HeaderStyle CssClass="GridPadding" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Gross Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGrossAmount" runat="server" CssClass="Captionstyle" Text='<%# Bind("GrossAmountCurrency") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ControlStyle Width="108px" />
                                                    <HeaderStyle CssClass="GridPadding" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="GL A/C Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGlCode" runat="server" CssClass="Captionstyle" Text='<%# Bind("GlAccountCode") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdnGlAccountName" runat="server" Value='<%# Eval("GlAccountName") %>' />
                                                    </ItemTemplate>
                                                    <ControlStyle Width="108px" />
                                                    <HeaderStyle CssClass="GridPadding" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#006699" CssClass="GridPadding" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                    </div>
                                    <%-- </asp:Panel>--%>
                            
                                </tr>
                        </table>
                        <table>
                            <tr>
                                <td width="230">
                                    <asp:Label ID="lblTotal" runat="server" Text="Total" CssClass="Captionstyle"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td width="335">
                                    <asp:TextBox ID="txtTotalNetAmount" runat="server" ReadOnly="True" CssClass="NumberInactivTxtbox"
                                        Width="108px"></asp:TextBox>
                                </td>
                                <td width="108">
                                    <asp:TextBox ID="txtTotalVatAmount" runat="server" ReadOnly="True" CssClass="NumberInactivTxtbox"
                                        Width="108px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalGrossAmt" runat="server" ReadOnly="True" CssClass="NumberInactivTxtbox"
                                        Width="108px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTotalInBase" runat="server" Text="Total in Base" CssClass="Captionstyle"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalinBaseNetAmount" runat="server" ReadOnly="True" CssClass="NumberInactivTxtbox"
                                        Width="108px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalInBaseVatAmount" runat="server" ReadOnly="True" CssClass="NumberInactivTxtbox"
                                        Width="108px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTotalInBaseGrossAmt" runat="server" ReadOnly="True" CssClass="NumberInactivTxtbox"
                                        Width="108px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
                <table>
                    <tr>
                        <td height="40">
                            <asp:Button ID="btnDelete" runat="server" CssClass="MainButtonStyle" OnClick="btnDelete_Click"
                                Text="Delete" />
                            <asp:Button ID="btnSave" runat="server" CssClass="MainButtonStyle" OnClick="btnSave_Click"
                                Text="Save" />
                            <asp:Button ID="btnSavePost" runat="server" CssClass="MainButtonStyle" OnClick="btnSavePost_Click"
                                Text="Save &amp; Post" />
                            <asp:Button ID="btnCancel" runat="server" CausesValidation="false" CssClass="MainButtonStyle"
                                OnClick="btnCancel_Click" Text="Cancel" />
                            <asp:Label ID="lblError" runat="server" CssClass="Errormessagetextstyle" />
                            <asp:Label ID="lblMessage" runat="server" CssClass="Errormessagetextstyle" />
                            <cc2:ModalPrompt ID="mptTransactionCode" runat="server" ButtonCssClass="PromptButtonStyle"
                                EmptyDataText="No Data found to display" GridViewCssClass="PromptGridtablestyle"
                                HeadingCssClass="PromptCaptionstyle" HideFirstColumn="False" OnCancelled="mptTransactionCode_Cancelled"
                                OnItemSelected="mptTransactionCode_ItemSelected" PageSize="10" PromptTitle=""
                                ReturnAllFields="False" TableCssClass="PromptBasestyle" />
                            <cc2:ModalPrompt ID="mpCustomer" runat="server" ButtonCssClass="PromptButtonStyle"
                                EmptyDataText="No Data found to display" GridViewCssClass="PromptGridtablestyle"
                                HeadingCssClass="PromptCaptionstyle" HideFirstColumn="False" OnCancelled="mpCustomer_Cancelled"
                                OnItemSelected="mpCustomer_ItemSelected" PageSize="10" PromptTitle="Customers"
                                ReturnAllFields="False" TableCssClass="PromptBasestyle" />
                            <cc2:ModalPrompt ID="mpTerritory" runat="server" ButtonCssClass="PromptButtonStyle"
                                EmptyDataText="No Data found to display" GridViewCssClass="PromptGridtablestyle"
                                HeadingCssClass="PromptCaptionstyle" HideFirstColumn="False" OnCancelled="mpTerritory_Cancelled"
                                OnItemSelected="mpTerritory_ItemSelected" PageSize="10" PromptTitle="" ReturnAllFields="False"
                                TableCssClass="PromptBasestyle" />
                            <cc2:ModalPrompt ID="mpSettlementTerm" runat="server" ButtonCssClass="PromptButtonStyle"
                                EmptyDataText="No Data found to display" GridViewCssClass="PromptGridtablestyle"
                                HeadingCssClass="PromptCaptionstyle" HideFirstColumn="False" OnCancelled="mpSettlementTerm_Cancelled"
                                OnItemSelected="mpSettlementTerm_ItemSelected" PageSize="10" PromptTitle="" ReturnAllFields="False"
                                TableCssClass="PromptBasestyle" />
                            <cc2:ModalPrompt ID="mpDocument" runat="server" ButtonCssClass="PromptButtonStyle"
                                EmptyDataText="No Data found to display" GridViewCssClass="PromptGridtablestyle"
                                HeadingCssClass="PromptCaptionstyle" HideFirstColumn="False" OnCancelled="mpDocument_Cancelled"
                                OnItemSelected="mpDocument_ItemSelected" PageSize="10" PromptTitle="" ReturnAllFields="False"
                                TableCssClass="PromptBasestyle" />
                            <cc2:ModalPrompt ID="mpCollection" runat="server" ButtonCssClass="PromptButtonStyle"
                                EmptyDataText="No Data found to display" GridViewCssClass="PromptGridtablestyle"
                                HeadingCssClass="PromptCaptionstyle" HideFirstColumn="False" OnCancelled="mpCollection_Cancelled"
                                OnItemSelected="mpCollection_ItemSelected" PageSize="10" PromptTitle="" ReturnAllFields="False"
                                TableCssClass="PromptBasestyle" />
                            <cc2:ModalPrompt ID="mpVAT" runat="server" ButtonCssClass="PromptButtonStyle" EmptyDataText="No Data found to display"
                                GridViewCssClass="PromptGridtablestyle" HeadingCssClass="PromptCaptionstyle"
                                HideFirstColumn="False" OnCancelled="mpVAT_Cancelled" OnItemSelected="mpVAT_ItemSelected"
                                PageSize="10" PromptTitle="" ReturnAllFields="False" TableCssClass="PromptBasestyle" />
                            <cc2:ModalPrompt ID="mpGlCodes" runat="server" ButtonCssClass="PromptButtonStyle"
                                EmptyDataText="No Data found to display" GridViewCssClass="PromptGridtablestyle"
                                HeadingCssClass="PromptCaptionstyle" HideFirstColumn="False" OnCancelled="mpGlCodes_Cancelled"
                                OnItemSelected="mpGlCodes_ItemSelected" PageSize="10" PromptTitle="" ReturnAllFields="False"
                                TableCssClass="PromptBasestyle" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
    <p>
        &nbsp;</p>
</body>
</html>
