<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorInActiveUser.aspx.cs" Inherits="XONT.Ventura.AppConsole.Web.ErrorInActiveUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Error Page</title>
    <style type="text/css">
        .style1
        {
            text-align: center;
			font-family:Verdana, Geneva, sans-serif;
			font-size:20px;
			font-weight:bold;
			color:#C00;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <br />
    <br />
        <br />
      <table width="600" border="0" align="center">
      <tr>
        <td><span class="style1"><img src="images/error.gif" width="75" height="75" /></span></td>
         <td align="center" class="style1">Not an Active User !</td>
      </tr>
      </table>
    <h1 class="style1">&nbsp;</h1>
    </div>
    </form>
</body>
</html>