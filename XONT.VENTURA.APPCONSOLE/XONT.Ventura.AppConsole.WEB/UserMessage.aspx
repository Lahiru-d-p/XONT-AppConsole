<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMessage.aspx.cs" Inherits="XONT.Ventura.AppConsole.Web.UserMessage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>


    <style type="text/css">
        .Captionstyle {
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

        .mailsendbtn {
            width: 77px;
            height: 24px;
            background-color: Transparent;
            background-image: url("images/mailsendbtn.png");
            background-repeat: no-repeat;
            cursor: pointer;
        }

        body {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
            padding: none;
        }

        .Maintextstyle {
            font-family: Verdana;
            font-size: 11px;
            color: #ffffff;
        }
    </style>

</head>
<body>
    <form id="form2" runat="server">
        <div>

            <table style="border-style: none; padding: 0px; margin: 0px; background-image: url('images/user-error.png'); background-repeat: no-repeat; height: 216px; width: 430px;">
                <tr>
                    <td rowspan="5" width="20"></td>
                    <td height="50" colspan="2"></td>
                </tr>
                <tr>
                    <td height="20" valign="top">

                        <asp:Label ID="Label1" runat="server" Text="Message Number :"
                            CssClass="Maintextstyle"></asp:Label>
                        &nbsp;<asp:Label ID="lblMessageNumber" runat="server" Text="Label"
                            CssClass="Maintextstyle"></asp:Label>

                    </td>
                    <td width="70">&nbsp;</td>
                </tr>
                <tr>
                    <td height="80" valign="top">

                        <asp:Label ID="lblDisc" runat="server" CssClass="Maintextstyle"></asp:Label>

                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="Ok" CssClass="mailsendbtn"
                            BorderStyle="None" BorderWidth="0px" ForeColor="White" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
            </table>

        </div>
    </form>

</body>
</html>
