<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminAlert.aspx.cs" Inherits="XONT.Ventura.AppConsole.Web.AdminAlert" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
        <style type="text/css">
	
.Captionstyle{
	font-size: 11px;
	font-family: Tahoma;
	font-weight: bold;
	color: #636363;
	height: 24px;
}

.Commenstyle {
	font-family: Tahoma;
	font-size: 11px;
}
.mailsendbtn
{
width:77px;
height:24px;
background-color:Transparent;
background-image: url("images/mailsendbtn.png");
background-repeat:no-repeat;
cursor:pointer;
}

body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	padding: none;
}

.Maintextstyle{
font-family:Verdana;
font-size:15px;	color: #ffffff; 

} 

    </style>
</head>
<body>
    <form id="form2" runat="server">
    <div>
    
        <table style="border-style: none; padding: 0px; margin: 0px; width: 430px;" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" valign="bottom" style="background-image: url('images/Admin_Alert_Top.png'); background-repeat: no-repeat; width:430px; height:34px">
                    <asp:Label ID="lblAdminMessage" runat="server" Text="Admin Message" CssClass="Maintextstyle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" valign="bottom" style="background-image: url('images/Admin_Alert_Middle.png'); background-repeat: repeat-y; width:430px;" >
                    <br /><div style="width:380px; text-align:left">
                    <asp:Label ID="lblDescription" runat="server" CssClass="Maintextstyle"></asp:Label>
                    </div>
                    <br />
                </td>
            </tr>
            <tr>
                <td align="center" valign="middle" style="background-image: url('images/Admin_Alert_Bottom.png'); background-repeat: no-repeat; width:430px; height:63px">
                    <asp:Button ID="btnOk" runat="server" onclick="btnOk_Click" Text="Ok" CssClass="mailsendbtn" 
                        BorderStyle="None" BorderWidth="0px" ForeColor="White" />
                </td>
            </tr>
        </table>
    
    </div>
    </form>
    
</body>
</html>