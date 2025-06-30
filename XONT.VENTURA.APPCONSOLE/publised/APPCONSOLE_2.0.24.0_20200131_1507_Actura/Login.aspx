<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Login.aspx.cs" Inherits="XONT.Ventura.AppConsole.Login" EnableEventValidation="false" %>

<%@ Register Src="ucChengePass.ascx" TagName="ucChengePass" TagPrefix="uc1" %>

<!DOCTYPE html>
<html lang="en" class="no-js">

<head id="Head1" runat="server">

    <!-- /.website title -->
    <title>Ventura Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta charset="utf-8">
    <!-- CSS Files -->
    <link href="css/bootstrap.css" rel="stylesheet" media="screen">
    <!-- Colors -->
    <link href="css/css-index.css" rel="stylesheet" media="screen">
     <script src="assets/cdn/jquery.js"></script>
        <script type="text/javascript" src="//code.jquery.com/jquery-migrate-1.2.1.min.js"></script>
    <!-- Google Fonts -->
    <link rel="stylesheet" href="http://fonts.googleapis.com/css?family=Lato:100,300,400,700,900,100italic,300italic,400italic,700italic,900italic" />
    

    <%--V2041Adding start--%>
    <script type="text/javascript" src="js/aes.js"></script>
    <script type="text/javascript" src="js/aes_xont.js"></script>
    <%--V2041Adding end--%>

    <script type="text/JavaScript" language="JavaScript">
        //Show Hide Passwordword Expire User Control

        function HidePopup() {
            document.getElementById("divChangePass").style.display = "none";
            objDiv = document.getElementById("divChangePassCon"); objDiv.style.display = "none"; return false
        }
        function ShowPopup() {
            try {
                document.getElementById("divChangePass").style.display = "block";
                objDiv = document.getElementById("divChangePassCon"); objDiv.style.display = "block";
                objDiv.style.width = document.body.scrollWidth; objDiv.style.height = document.body.scrollHeight
            }
            catch (e) { alert(e) } return false
        }
    </script>
    <script>
    function validatePage()
    {
        $("#btnLoginButton").val('Loading...');
       // $('#btnLoginButton').prop('readonly', true);
      
       return true;
    }
  </script>
    <style>
        #txtUserName[disabled] {
            background-color:white !important;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <!-- /.preloader -->
        <div id="preloader"></div>
        <div id="top"></div>

        <!-- /.parallax full screen background image -->
        <div class="landing" style="background-image: url('images/actura_bk.jpg'); background-position: 50% 1.192px !important;" data-img-width="2000" data-img-height="1500" data-diff="100">

            <div class="overlay">
                <%--sd--%>
                <div id="divChangePassCon" class="ModalBackground graydiv">
                </div>
                <div id="divChangePass" class="modeldivstyle" style="background-color: rgba(40, 46, 55, 0.43); overflow: hidden; top: 0; left: 0; right: 0; bottom: 0; margin: auto;">
                    <div style="width: 100%; height: 100%;">
                        <uc1:ucChengePass ID="ucChengePass1" runat="server" />
                    </div>
                    <img alt="" src="images/close_pop.png" style="top: 1%; left: 89%; position: absolute;"
                        onclick="HidePopup()" />
                </div>
                <%--sad--%>
                <div class="container">
                    <div class="row">
                        <div class="col-md-7" style="padding-top: 225px;">

                            <!-- /.main title -->
                            <h1 class="wow fadeInLeft">Welcome to Actura ERP
                            </h1>

                            <!-- /.header paragraph -->
                            <div class="landing-text wow fadeInUp">
                                <p style="font-style:italic;font-variant:unset;font-size:17px !important;font-weight:initial;text-align:justify;">
                                    A highly productive work force has an enormous impact on your company's top and bottom line. Employees today are face with enormous challenges than ever before due to the fast paced digital economy, fierce competition, dynamic deals and demanding customers and there is only one thing that makes them standout; that’s accurate and timely BUSINESS DATA & INFORMATION. That's why a cutting edge business solution such as X-ONT ACTURA is critical to your companies success.
                                </p>
                            </div>



                        </div>

                        <!-- /.signup form -->
                        <div class="col-md-5" style="margin-top: 4%">

                            <div class="signup-header wow fadeInUp">
                                <h3 class="form-title text-center">USER LOGIN</h3>
                                <form class="form-header" action="" role="form" method="POST" id="#">
                                    <div class="form-group">
                                        <%--<asp:TextBox ID="txtUserName" name="MERGE1" runat="server" CssClass="form-control input-lg" TabIndex="1"></asp:TextBox>--%>
                                        <input type="text" ID="txtUserName" required name="" class="form-control input-lg" TabIndex="1"/>
                                    </div>
                                    <div class="form-group">
                                        <input type="password" id="txtPassword" required name="" onchange="javascript:SetFocus('btnLoginButton')" class="form-control input-lg" tabindex="2" />
                                    </div>

                                    <div class="form-group">
                                        <select id="selectLanguage" name="D1" runat="server" class="form-control input-lg" tabindex="3">
                                            <option>Change Language</option>
                                            <option>English</option>
                                            <option>Sinhala</option>
                                            <option>Tamil</option>
                                        </select>

                                    </div>

                                    <div class="form-group last">
                                        <asp:HiddenField ID="hdnEncryptUN" runat="server" />
                                        <asp:HiddenField ID="hdnEncryptPW" runat="server" />
                                        <asp:Button ID="btnLoginButton" runat="server" CommandName="Login" ValidationGroup="Login1" Text="LOGIN" OnClick="btnLoginButton_Click" OnClientClick="btnLogin_ClientClick()" CssClass="btn btn-block btn-lg" TabIndex="4"  />
                                         <%--OnClientClick="validatePage();"--%>
                                        <input id="btnChangePassCon" runat="server" type="button" class="passwordchange btn-default" style="margin-left: 25%; margin-top: 10px;" value="Change Password" tabindex="5" onclick="ShowPopup()" />

                                    </div>
                                    <p class="privacy text-center" style="text-decoration: none;">Please Read our <a href="#">privacy policy</a>.</p>

                                    <div id="divErrormessage" style="position: absolute;">
                                        <span class="Errormessagetextstyle" style="color: red;">
                                            <asp:Literal ID="valFailureText" runat="server" EnableViewState="False"></asp:Literal>
                                            <span class="Errormessagetextstyle" style="color: red;">
                                                <asp:Literal ID="valUserNamePassword" runat="server"
                                                    EnableViewState="False"></asp:Literal><%--VR011 add--%>
                                                <%--VR011 remove--%>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        ControlToValidate="txtUserName" ErrorMessage="User name required !" 
                                        SetFocusOnError="True" ValidationGroup="Login1" Display="Dynamic" 
                                        ForeColor="#E50505"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ControlToValidate="txtPassword" ErrorMessage="Password required !" 
                                        SetFocusOnError="True" ValidationGroup="Login1" Display="Dynamic" 
                                        ForeColor="#E50505"></asp:RequiredFieldValidator>--%> 
                                        
                                            </span>
                                            <%--v2041Removed<asp:RegularExpressionValidator ID="revUserName" runat="server" ControlToValidate="txtUserName" CssClass="Errormessagetextstyle" Display="Dynamic" ValidationExpression="^[^(')]*$"></asp:RegularExpressionValidator>--%>
                                    </div>

                                    <asp:Label ID="lblLicence" runat="server" Text="License to"></asp:Label>
                                    <asp:Image ID="imgLogo" src="images/logo.jpg" alt="" runat="server" />

                                </form>
                            </div>

                        </div>
                    </div>


                    <p class="privacy text-center" style="text-decoration: none; margin-bottom: 2%;">Copyright &copy; 2016 X-ONT SOFTWARE PVT LTD. All Rights Reserved.</p>

                </div>
            </div>
        </div>



        <%--</div>--%>



        <!-- /.javascript files -->
        <script src="jslog/jquery.js"></script>
        <script src="jslog/bootstrap.min.js"></script>
        <script src="jslog/custom.js"></script>
        <script src="jslog/jquery.sticky.js"></script>

        <script src="jslog/owl.carousel.min.js"></script>

        <script>
            new WOW().init();
        </script>
    </form>
</body>



</html>
