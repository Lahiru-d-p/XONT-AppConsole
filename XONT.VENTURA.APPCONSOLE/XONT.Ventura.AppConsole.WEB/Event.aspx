<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Event.aspx.cs" Inherits="XONT.Ventura.AppConsole.Web.Event" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>::Ventura::</title>
    <style type="text/css">
        body
        {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
	background-color:Transparent;
	background-image: url('images/event.png');
	background-repeat: no-repeat;
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
				
		.MainButtonStyle:hover 
		{
			background-image: url("images/butntry1.jpg");
			color: #003366;
		}
		.Errormessagetextstyle {
	color: #ffffff;
	font-family: Tahoma;
	font-size: 11px;
	font-style:normal;
}
.Captionstyle{
	font-size: 11px;
	font-family: Tahoma;
	font-weight: bold;
	color: #ffffff;
}
    </style>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8" /></head>
<body>
    <form id="form1" runat="server">
          <table style="margin-top: 42px; margin-left: 13px; height: 338px; width: 405px;">
                  <tr>
                    <td width="106">
                        <asp:Label ID="Label1" runat="server" Text="Error Log Number" 
                            CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle" width="8">:</td>
                    <td>
                        <asp:Label ID="lblELNumber" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Error Time" CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblETime" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="WorkStation ID" 
                            CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblWId" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                      </td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="User Name" CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblUName" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="IPAddress" CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblIPAddress" runat="server" BorderStyle="Solid" 
                            BorderWidth="0px" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label6" runat="server" Text="Message Number" 
                            CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblMNumber" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td valign="top">
                        <asp:Label ID="Label7" runat="server" Text="Error Discription" 
                            CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle" valign="top">:</td>
                    <td valign="top" rowspan="2">
                        <asp:Label ID="lblDisc" runat="server" BorderWidth="0px" EnableTheming="True" 
                            Height="16px" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td valign="top">&nbsp;
                        </td>
                    <td class="Captionstyle" valign="top">&nbsp;</td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label8" runat="server" Text="Error Source" 
                            CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblESource" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label9" runat="server" Text="DLL Name" CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblDLLName" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label10" runat="server" Text="Version" CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblEVersion" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label11" runat="server" Text="Routine" CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblRoutine" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                        </td>
                  </tr>
                  <tr>
                    <td>
                        <asp:Label ID="Label12" runat="server" Text="Error Line Number" 
                            CssClass="Captionstyle"></asp:Label>
                      </td>
                    <td class="Captionstyle">:</td>
                    <td>
                        <asp:Label ID="lblELineNumber" runat="server" CssClass="Errormessagetextstyle"></asp:Label>
                      </td>
                  </tr>
                  <tr>
                    <td>
                        &nbsp;</td>
                    <td class="Captionstyle">&nbsp;</td>
                    <td>
                        &nbsp;</td>
                  </tr>
                  <tr>
                    <td></td>
                    <td></td>
                    <td style="text-align: right"><span>
                        <asp:Button ID="btnMail" runat="server" onclick="btnMail_Click" Text="Mail Log" 
                    Width="77px" CssClass="mailsendbtn" Height="24px" BorderStyle="None" 
                            ForeColor="White" />
              <asp:Button ID="btnOK" runat="server" onclick="btnOK_Click" Text="Ok" 
                Width="77px" CssClass="mailsendbtn" Height="24px" BorderStyle="None" 
                            ForeColor="White" /></span></td>
                  </tr>
                  </table>

          </form>
</body>
</html>
