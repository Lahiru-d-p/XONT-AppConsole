<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZYMNT02edit.aspx.cs" Inherits="XONT.VENTURA.ZYMNT02.ZYMNT02edit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>:: ZYMNT02 ::</title>
    <link href="~/App_Themes/Blue/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../js/Disable_BackSpace.js" type="text/javascript"></script>
    <link href="../css/fontawesome-iconpicker.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="../icon-fonts/font-awesome-4.6.3/css/font-awesome.min.css" />
    <script src="../dist/js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../js/fontawesome-iconpicker.js" type="text/javascript"></script>
    <script type="text/javascript">



        $(function () {

            $(document).ready(function () {

                if ($.trim($('.currentTaskIcon').val()) == '') {
                    currentIcon = 'fa-cog';
                }
                else {
                    var currentIcon = $('.currentTaskIcon').val();

                }


                $('.icp-auto').iconpicker();

                $('.icp-dd').iconpicker({
                    selected: "" + currentIcon + "",
                    //defaultValue:'fa-apple'
                });

            }).trigger('click');



            $('.icp-dd').on('iconpickerSelected', function (e) {
                var taskClass = e.iconpickerInstance.options.fullClassFormatter(e.iconpickerValue);
                $('.currentTaskIcon').val(taskClass);
            });



            $('.icp').on('iconpickerSelected', function (e) {
                $('.lead .picker-target').get(0).className = 'picker-target fa-3x ' +
                        e.iconpickerInstance.options.iconBaseClass + ' ' +
                        e.iconpickerInstance.options.fullClassFormatter(e.iconpickerValue);


            });
        });
    </script>



</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdateProgress ID="uprProgress" runat="server" DisplayAfter="120">
            <ProgressTemplate>
                <div id="divProgressBack" runat="server">
                    <div id="dvProgress" class="loading"></div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
        <%--  <input type="hidden" id="iconClass" runat="server" class="taskIcon" />--%>
        <input type="hidden" id="currentIcon" runat="server" class="currentTaskIcon" />
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblModuleCode" runat="server" Text="Module Code" CssClass="Captionstyle"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="DropDownList1" runat="server"
                        Width="60px" CssClass="Dropdownboxstyle" TabIndex="1">
                    </asp:DropDownList>
                    &nbsp;
                <asp:Label ID="lblTaskCode" runat="server" Text="Task Code" CssClass="Captionstyle"></asp:Label>
                    &nbsp;<asp:TextBox ID="TextBoxTaskCode" runat="server" CssClass="Textboxstyle" AutoPostBack="true" OnTextChanged="TextBoxTaskCode_TextChanged"
                        Width="172px" MaxLength="10" TabIndex="2"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldTaskCode" runat="server" ControlToValidate="TextBoxTaskCode" SetFocusOnError="true"
                        CssClass="Errormessagetextstyle" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ValidationExpression="^[A-Za-z0-9_-]+$" CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="Invalid entry" ControlToValidate="TextBoxTaskCode"></asp:RegularExpressionValidator>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCaption" runat="server" Text="Caption" CssClass="Captionstyle"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="TextBoxCaption" runat="server" CssClass="Textboxstyle"
                        Width="430px" MaxLength="40" TabIndex="3"></asp:TextBox>

                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldCaption" runat="server" ControlToValidate="TextBoxCaption" SetFocusOnError="true"
                        CssClass="Errormessagetextstyle" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                        ValidationExpression="^[^&amp;#&quot;;']*$" CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="Invalid entry" ControlToValidate="TextBoxCaption"></asp:RegularExpressionValidator>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblDescription" runat="server" Text="Description" CssClass="Captionstyle"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="TextBoxDescrip" runat="server" CssClass="Textboxstyle"
                        Width="430px" MaxLength="60" TabIndex="4"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldDescript" runat="server" ControlToValidate="TextBoxDescrip" SetFocusOnError="true"
                        CssClass="Errormessagetextstyle" ErrorMessage="*"
                        Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                        ValidationExpression="^[^&amp;#&quot;;']*$" CssClass="Errormessagetextstyle" Display="Dynamic" ErrorMessage="Invalid entry" ControlToValidate="TextBoxDescrip"></asp:RegularExpressionValidator>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblExecutionScript" runat="server" Text="Execution Script" CssClass="Captionstyle"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="TextBoxExectScr" runat="server" CssClass="Textboxstyle" Enabled="false"
                        Width="314px" MaxLength="50" TabIndex="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblExclusivityMode" runat="server" Text="Exclusivity Mode" CssClass="Captionstyle"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="TextBoxExclusiMode" runat="server" CssClass="Textboxstyle"
                        Width="120px" TabIndex="6" MaxLength="5"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regValNumbersOnly" runat="server" SetFocusOnError="true"
                        ControlToValidate="TextBoxExclusiMode" Display="Dynamic"
                        ErrorMessage="Please enter only numbers" ValidationExpression="^\d+$"
                        CssClass="Errormessagetextstyle"></asp:RegularExpressionValidator>
                    &nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblVersion" runat="server" Text="Version" CssClass="Captionstyle"></asp:Label>
                    &nbsp;<asp:TextBox ID="TextBoxVersion" runat="server" CssClass="Textboxstyle"
                        Width="123px" MaxLength="12" TabIndex="7"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regValNumbersOnly1" runat="server" SetFocusOnError="true"
                        ControlToValidate="TextBoxVersion" Display="Dynamic"
                        ErrorMessage="Please enter only numbers" ValidationExpression="^(\d|-)?(\d|,)*\.?\d*$"
                        CssClass="Errormessagetextstyle"></asp:RegularExpressionValidator></td>

            </tr>

            <tr>
                <td>
                    <asp:Label ID="lblIcon" runat="server" Text="Icon" CssClass="Captionstyle"></asp:Label>
                </td>
                <td valign="top">
                    <%--  <asp:Button ID="substract" runat="server" Width="22px" BorderStyle="None" BorderWidth="0px"
                                CssClass="calendermoveleft" Height="17px"
                                CausesValidation="false" OnClick="substract_Click" TabIndex="8" />
                            &nbsp;<asp:Image ID="ImageIcon" runat="server" />

                            &nbsp;<asp:Button ID="add" runat="server" Width="25px" BorderStyle="None"
                                CssClass="calendermoveright" Height="17px"
                                CausesValidation="false" OnClick="add_Click" TabIndex="9" />--%>

                    <%--VR007--%>

                    <div class="btn-group">
                        <button type="button" class="btn btn-primary iconpicker-component"><i class="fa fa-cog" id="test" runat="server"></i></button>
                        <button type="button" class="icp icp-dd btn btn-primary dropdown-toggle" data-selected="fa-car" data-toggle="dropdown">
                            <span class="caret"></span>
                            <span class="sr-only">Toggle Dropdown</span>
                        </button>
                        <div class="dropdown-menu"></div>
                    </div>

                </td>
                <td>
                    &nbsp;<asp:Label ID="Label1" runat="server" Text="Task Group" CssClass="Captionstyle"></asp:Label>
                    &nbsp;<asp:DropDownList ID="ddlTaskGroup" runat="server" CssClass="Textboxstyle"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlTaskGroup_SelectedIndexChanged">
                    </asp:DropDownList>

                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblActive" Text="Active" runat="server" CssClass="Captionstyle"></asp:Label>
                </td>

                <td colspan="2">
                    <asp:CheckBox ID="chkActive" runat="server" CssClass="Checkboxstyle" Style="margin-left: 0 !important;"
                        Text="Active" TabIndex="15" />
                </td>
            </tr>

            <tr>
                <td></td>
                <td style="height:40px" colspan="2">
                    <asp:Button ID="btnOK" runat="server" Text="OK" CssClass="MainButtonStyle"
                        OnClick="btnOK_Click" TabIndex="11" />
                    <asp:Button ID="btnCancel" runat="server"
                        Text="Cancel" CausesValidation="false" CssClass="MainButtonStyle"
                        OnClick="btnCancel_Click" TabIndex="12" />
                    <asp:Label ID="LabelMessage2" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <%--</ContentTemplate>--%>
        <%--        </asp:UpdatePanel>--%>
    </form>
</body>
</html>
