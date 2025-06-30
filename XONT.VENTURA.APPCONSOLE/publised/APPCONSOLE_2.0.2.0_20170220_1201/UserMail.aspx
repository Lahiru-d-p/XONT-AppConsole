<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMail.aspx.cs" Inherits="XONT.Ventura.AppConsole.Web.UserMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxtoolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>

        <script type="text/javascript" src="JS/colorPicker/picker.js"></script>

<style type="text/css">
        
.Textboxstyle

{
background-color:#FFF;
color: #01539D;
font-family: Trebuchet MS;
font-size: 13px;
}

.adduser

{
background-color:Transparent;
background-image: url("images1/adduser.png");
background-repeat:no-repeat;
cursor:pointer;
}

.mailsendbtn

{
width:77px;
height:24px;
background-color:Transparent;
background-image: url("images1/mailsendbtn.png");
background-repeat:no-repeat;
cursor:pointer;
}
		.Checkboxstyle {
		color: #01539D;
		font-family: Trebuchet MS;
		font-size: 13px;
		margin-left: -5px;
		padding-left:-5px;
		}

    </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <input id="txtTheme" type="hidden" runat="server" value="blue" />
    <div>
       
        <table style="border-style: none; padding: 0px; margin: 0px; background-image: url('images1/emailbdy.png'); background-repeat: no-repeat; height: 361px; width: 424px;">
            <tr>
                <td class="style2" height="35" width="17">
                    &nbsp;</td>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td colspan="2" height="24">
                    <asp:Label ID="txtFrom" runat="server" CssClass="Textboxstyle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td colspan="2" height="22">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td width="365">
                    <asp:Label ID="txtTo" runat="server" CssClass="Textboxstyle"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" BorderStyle="None" BorderWidth="0px" 
                        Height="11px" Width="11px" CssClass="adduser" />
                </td>
            </tr>
            <tr>
                <td class="style3">
                </td>
                <td valign="bottom" class="style1" colspan="2" height="26">
                    <asp:CheckBox ID="chkAttachment" runat="server" CssClass="Checkboxstyle" />
                    </td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td valign="middle" colspan="2" height="8">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style2">
                    &nbsp;</td>
                <td valign="top" colspan="2" height="118">
                    <textarea id="txtBody" name="S1" runat="server" 
                        
                        style="padding: 0px; margin: 0px; border-style: none; border-width: 0px; width: 364px; height: 115px" 
                        class="Textboxstyle"></textarea></td>
            </tr>
            <tr class="choosed_color_cell">
                <td height="40" class="style2">
                    &nbsp;</td>
                <td colspan="2">
                    <asp:Button ID="btnOk" runat="server" Text="Send" 
                        OnClientClick="javascript:window.parent.HideMail();" OnClick="Button1_Click" 
                        CssClass="mailsendbtn" BorderStyle="None" BorderWidth="0px" 
                        ForeColor="White" />
                    <asp:Button ID="btnOk0" runat="server" Text="Close" OnClick="btnOk0_Click" 
                        CssClass="mailsendbtn" BorderStyle="None" BorderWidth="0px" 
                        ForeColor="White" />
                </td>
            </tr>
            <tr class="choosed_color_cell">
                <td class="style2">
                    &nbsp;</td>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
