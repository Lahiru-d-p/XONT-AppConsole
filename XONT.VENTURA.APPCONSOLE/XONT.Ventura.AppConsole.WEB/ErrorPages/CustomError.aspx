<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomError.aspx.cs" Inherits="XONT.Ventura.AppConsole.ErrorPages.CustomError" %>

<!DOCTYPE html>

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
        <td><span class="style1"><img src="~/images/error.gif" width="75" height="75" /></span></td>
         <td align="center" class="style1">System error has occured. Please contact the Administrator !</td>
      </tr>
      </table>
    <h1 class="style1">&nbsp;</h1>
    </div>
    </form>
</body>
</html>