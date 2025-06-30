<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ucChengePass.ascx.cs" Inherits="XONT.Ventura.AppConsole.Web.ucChengePass" %>
 
          <link href="App_Themes/blue/StyleSheet.css" rel="stylesheet" type="text/css" />
 <style type="text/css">
     


  .modeldivstyle
        {
            position: absolute;
            display: none;
            z-index: 100002;
            background-color: White;
        }
        .graydiv
        {
            position: absolute;
            background-color: #ffffff;
            left: 0px;
            top: 0px;
            z-index: 10000;
            display: none;
            vertical-align: middle;
        /*    background-position: center;*/
        }
        /*Modal Popup*/.ModalBackground
        {
            background-color: #ffffff;
            filter: alpha(opacity=50);
            opacity: 0.5;
        /*    vertical-align: middle;
         /*   background-position: center;*/
        }
        
.captext{color:#0c526f;font-family:Tahoma;font-size:12px;font-weight:bold;text-align:left}
.Txt1{color:#FFF;font-family:Verdana;font-size:15px;font-weight:bold}
.Txt2{color:#FFF;font-family:Tahoma;font-size:9pt;text-align:justify}
.Txt3{color:#FFF;font-family:Verdana;font-size:12px;font-weight:bold}
.Txt4{color:#666;font-family:Tahoma;font-size:11px;text-align:right}
.bkimg{border-style:none; height:252px; background-image: url('images/chnagepassword.png'); background-color:Transparent; background-repeat: no-repeat;
         width: 444px;
     }

 </style>
 <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                        
                            <table cellpadding="0" allowtransparency="true" cellspacing="0"  class="bkimg" style="top:30%;left:0;right:0;bottom:0;position:absolute;margin:auto;">
                                <tr>
                                    <td style="align: center">
                            <table style="margin-top: 40px;">
                              
                                <tr>
                                    <td style="text-align: left" width="40">
                                        &nbsp;</td>
                                    <td style="text-align: left" width="140">
                                        <asp:Label ID="lbloldPassword" runat="server" Text="Old Password" 
                                            CssClass="Txt3"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtoldPassword" runat="server" TextMode="Password" 
                                            EnableViewState="true" CssClass="Textboxstyle" Width="200px"></asp:TextBox>
                                     </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        &nbsp;</td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblnewPassword" runat="server" Text="New Password" 
                                            CssClass="Txt3"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtnewPassword" runat="server" TextMode="Password" 
                                            CssClass="Textboxstyle" Width="200px"></asp:TextBox>
                                   </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        &nbsp;</td>
                                    <td style="text-align: left">
                                        <asp:Label ID="lblconfermPassword" runat="server" Text="Confirm Password" 
                                            CssClass="Txt3"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtconfirmPassword" runat="server" TextMode="Password" 
                                            CssClass="Textboxstyle" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td height="40">
                                        &nbsp;</td>
                                    <td height="40">
                                        &nbsp;</td>
                                    <td style="text-align: left">
                                        <asp:Button ID="btnChangePass" runat="server" Text="Apply"  
                                             CssClass="MainButtonStyle"  UseSubmitBehavior="false"

                                            ValidationGroup="LTVal" onclick="btnChangePass_Click"  />
                                                                              </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">
                                        &nbsp;</td>
                                    <td colspan="2" style="text-align: left">
                                        <asp:Label ID="lblMas" runat="server" Text="" CssClass="Errormessagetextstyle"></asp:Label>
                                        <asp:CompareValidator ID="covpassword" CssClass="Errormessagetextstyle" 
                                            runat="server" ValidationGroup="valUserCreation"
                                            ControlToCompare="txtnewPassword" ControlToValidate="txtconfirmPassword"
                                            ErrorMessage="Password Not Match....." Display="Dynamic"></asp:CompareValidator>
                                        
                                       
                                    </td>
                                </tr>
                            </table>
                                    </td>
                                </tr>
</table>

                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>
                    --%>