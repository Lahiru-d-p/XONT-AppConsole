<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactCard.aspx.cs" Inherits="XONT.Ventura.HandTools.ContactCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <title></title>
    <link href="./assets_hand/css/icons.css" rel="stylesheet">
    <link href="./assets_hand/css/bootstrap.css" rel="stylesheet">
    <link href="./assets_hand/css/plugins.css" rel="stylesheet">
    <link href="./assets_hand/css/main.css" rel="stylesheet">

    <style>

          .col-md-55 {
    width: 49%;
  }
          .col-md-122 {
    width: 100%;
  }

          .panel .panel-controls a:hover i {
  color: #95A5A6;
}
          input, textarea
          {
              line-height:normal !important;
          }
    </style>
</head>
<body style="background-color:#fff;">
    <form id="form1" runat="server">
    <asp:MultiView ID="mvwScreens" runat="server" ActiveViewIndex="0">
    
                <asp:View ID="viwLoadContacts" runat="server">
                
                 <div id="content" class="collapsed-sidebar" style="border-style:none;">
                <div class="content-wrapper" style="border-style:none;">
                    <div class="content-inner" style="border-style:none;">
                        <!-- Start .row -->
                        <div class="row">
                            <!-- col-lg-8 end here -->
                            <div class="col-lg-4 col-md-4 sortable-layout ui-sortable" style="width:340px; border-style:none;">
                                <!-- col-lg-4 start here -->
                                <!-- End .shortcut buttons -->
                                <!-- End .panel -->
                                <div class="panel panel-teal  plain toggle panelMove panelClose panelRefresh" id="jst_0" style="width:308px; border-style:none;">
                                    <!-- Start .panel -->
                                    <div class="panel-heading" style="border-style:none;">
                                        <h4 class="panel-title">
                                            <i class="fa fa-th-list"></i>Contact Card</h4>
                                        <div class="panel-controls" style="margin-top: 8px;">
                                        
                                        
                                        
                <asp:Panel ID="Panel4" runat="server">
                   
<asp:LinkButton ID="btnSelect" runat="server" ToolTip="Multiple Select" Text="Select" onclick="btnSelect_Click" ><i class="fa fa-pencil-square-o fa-lg"></i></asp:LinkButton>
<asp:LinkButton ID="btnNewContact" runat="server" ToolTip="Create a New Contatct" Text="New" onclick="btnNewContact_Click"><i class="fa fa-plus-square-o fa-lg"></i></asp:LinkButton>
<asp:LinkButton ID="btnDelete" runat="server" ToolTip="Delete Selected" Text="Delete" onclick="btnDelete_Click" Visible="False"><i class="fa fa-trash-o fa-lg" style="margin-top: -1px;"></i></asp:LinkButton>



                    </asp:Panel>
                    
                    </div>
                                    </div>
                                    <div class="panel-body" style="display: block; border-bottom-color:#fff;">
                                        <div class="todo-widget">
                                        
                     <div class="todo-task-text">
                                        
                    <div id="GridScroll" class="DivScroll DivScroll2">
                        <asp:GridView ID="grvContacts" runat="server" AutoGenerateColumns="False" BackColor="White"
                            BorderColor="#CCCCCC" Width="100%" BorderWidth="0px" DataKeyNames="RecID"
                            CellPadding="0" CssClass="Gridtablestyle"
                            
                            GridLines="Vertical" ShowHeader="False">
                            <RowStyle ForeColor="#000066" />
                            <Columns>
                                <asp:TemplateField  Visible ="false" ShowHeader="False" HeaderStyle-Wrap = "true"><HeaderStyle Width="10px" /><ItemStyle Width="20px" BorderColor="White" /><ItemTemplate><asp:CheckBox ID="cboxSelectContact" runat="server" /></ItemTemplate></asp:TemplateField>
                                <asp:TemplateField HeaderText="title" SortExpression="name"><ItemTemplate><ul class="todo-list" id="today"><li class="todo-task-item"><asp:LinkButton ID="btnContact" Text='<%#Bind("ContactTitle")%>' AutoPostBack="True"  onclick="btnContactSelected_Click" runat="server"></asp:LinkButton></li></ul></ItemTemplate><ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" /></asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                        </asp:GridView>
                    </div></div>
                    <%--</asp:Panel>--%>

                        <asp:Label ID="lblNoContactsMsg" runat="server" CssClass="Errormessagetextstyle" ForeColor="Red"></asp:Label></asp:View>
                
                
                
                
           
                <asp:View ID="viwContactDisplay" runat="server">
                
                <div id="Div1" class="collapsed-sidebar">
                <div class="content-wrapper">
                    <div class="content-inner">
                        <!-- Start .row -->
                        <div class="row">
                            <!-- col-lg-8 end here -->
                            <div class="col-lg-4 col-md-4 sortable-layout ui-sortable" style="width:340px;">
                                <!-- col-lg-4 start here -->
                                <!-- End .shortcut buttons -->
                                <!-- End .panel -->
                                <div class="panel panel-teal  plain toggle panelMove panelClose panelRefresh" id="Div2">
                                    <!-- Start .panel -->
                                    <div class="panel-heading" style="border-style:none;">
                                        <h4 class="panel-title">
                                            <i class="fa fa-th-list"></i>Contact Card</h4>
                                        <div class="panel-controls" style="margin-top: 8px;">
                                        
                                                        <asp:LinkButton ID="btnSave" ToolTip="Save Contact" runat="server" CssClass="" OnClick="btnSave_Click"><i class="fa fa-floppy-o fa-lg"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="btnEditSave" ToolTip="Save Changes"  runat="server" CssClass="" Text="edit" OnClick="btnEditSave_Click" visible = "false"><i class="fa fa-floppy-o fa-lg"></i></asp:LinkButton>
                                                        
                                                        <asp:LinkButton ID="btnAmend" ToolTip="Edit Contact" runat="server" CssClass="" OnClick="btnAmend_Click" CausesValidation="False"><i class="fa fa-pencil-square-o fa-lg"></i></asp:LinkButton>   
                                                        <asp:LinkButton ID="btnCancel" ToolTip="Close/Cancel" CssClass="" OnClick="btnCancel_Click" runat="server" CausesValidation="False" ><i class="fa fa-times fa-lg"></i></asp:LinkButton>
                                        
                                        </div>
                                    </div>
                                                                
                                                                
                                            <div class="panel-body" style="display: block; border-bottom-color:#fff;">
                                                <div class="todo-widget">
                                                    <ul class="todo-list" id="Ul1">
                                                        <li class="todo-task-item">
                                                            <div class="todo-task-text">
                
                    <asp:Panel ID="Panel1" runat="server">

                                    <asp:Panel ID="pnlContact" runat="server">
                                        
                                        <div>
                                                    <asp:Label ID="lblFirstName" runat="server" CssClass="Captionstyle" 
                                                        Text=" Full Name : " style="display: block; margin:5px 0;"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                        ControlToValidate="txtFirstName" ErrorMessage="*" ForeColor="Red"> </asp:RequiredFieldValidator>                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLastName" ErrorMessage="*" ForeColor="Red"> </asp:RequiredFieldValidator>

</asp:Label>
                                                
                                                </div>
                                            <div>
                                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="col-md-55" EnableViewState="False" MaxLength="30" ToolTip="First Name"></asp:TextBox>
                                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="col-md-55" EnableViewState="False" MaxLength="30" ToolTip="Last Name"></asp:TextBox>
                                                </div>
                                            <div>
                                                    <asp:Label ID="lblCompany" runat="server" CssClass="Captionstyle" Text=" Company : " style="display: block; margin:5px 0;"><asp:RequiredFieldValidator id="RequiredFieldValidator3" runat="server" ControlToValidate="txtCompany" ErrorMessage="*" ForeColor="Red"> </asp:RequiredFieldValidator></asp:Label>
                                               
                                                    <asp:TextBox ID="txtCompany" runat="server" CssClass="col-md-122" MaxLength="50" EnableViewState="False"></asp:TextBox>
                                                </div>
                                            <div>
                                                    <asp:Label ID="lblTelephone" runat="server" CssClass="Captionstyle" Text="Phone No : " style="display: block; margin:5px 0;">                                                        <asp:RequiredFieldValidator id="RequiredFieldValidator5" runat="server" ControlToValidate="txtMobile" ErrorMessage="*" ForeColor="Red"> </asp:RequiredFieldValidator>
</asp:Label>
                                               
                                                    <asp:TextBox ID="txtTelephone" runat="server" CssClass="col-md-55" MaxLength="15" EnableViewState="False"></asp:TextBox>
                                                        <asp:TextBox ID="txtMobile" runat="server" CssClass="col-md-55" MaxLength="15" EnableViewState="False"></asp:TextBox>
                                                        
                                                </div>
                                            <div>
                                                    <asp:Label ID="lblEmail" runat="server" CssClass="Captionstyle" Text=" E-Mail : " style="display: block; margin:5px 0;">                                                        <asp:RequiredFieldValidator id="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" ForeColor="Red"> </asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="regEmailAddress" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email Not Valid!" ValidationExpression="^([\w\d\-\.]+)@{1}(([\w\d\-]{1,67})|([\w\d\-]+\.[\w\d\-]{1,67}))\.(([a-zA-Z\d]{1,20})(\.[a-zA-Z\d]{2})?)$" CssClass="Errormessagetextstyle"></asp:RegularExpressionValidator>
</asp:Label>
                                               
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="col-md-122" MaxLength="60" EnableViewState="False"></asp:TextBox>
                                                </div>
                                            <div>
                                                    <asp:Label ID="lblAddress" runat="server" CssClass="Captionstyle" Text=" Address : " style="display: block; margin:5px 0;"><asp:RequiredFieldValidator id="RequiredFieldValidator7" runat="server" ControlToValidate="txtAddress1" ErrorMessage="*" ForeColor="Red"> </asp:RequiredFieldValidator>
</asp:Label>
                                                
                                                    <asp:TextBox ID="txtAddress1" runat="server" CssClass="col-md-122" MaxLength="200" EnableViewState="False" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            <div>
                                                    <asp:Label ID="Label2" runat="server" CssClass="Captionstyle" 
                                                        Text=" Country : " style="display: block; margin:5px 0;">
<asp:RequiredFieldValidator id="RequiredFieldValidator9" runat="server" ControlToValidate="txtCountry" ErrorMessage="*" ForeColor="Red"> </asp:RequiredFieldValidator>
</asp:Label>
                                                
                                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="col-md-122" MaxLength="50" EnableViewState="False"></asp:TextBox>
                                                </div>

<asp:Label ID="lblContactBodyMsg" runat="server" CssClass="Errormessagetextstyle" ForeColor="Red" EnableViewState= "false" ></asp:Label>

                                                
                                                 </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                </div>
                                    </asp:Panel>
                                           </asp:Panel>
                </asp:View>
                
                </div>
        </div>
        </div>
        <!-- End .panel -->
        <!-- End .panel -->
        </div>
        <!-- col-lg-4 end here -->
        </div>
        <!-- End .row -->
        </div> </div> </div>



        <script src="http://code.jquery.com/jquery-2.1.1.min.js"></script>

        <script>
            window.jQuery || document.write('<script src="assets/js/libs/jquery-2.1.1.min.js">\x3C/script>')
        </script>

        <script src="http://code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

        <script>
            window.jQuery || document.write('<script src="assets/js/libs/jquery-ui-1.10.4.min.js">\x3C/script>')
        </script>

        <!--[if lt IE 9]>
      <script type="text/javascript" src="assets/js/libs/excanvas.min.js"></script>
      <script type="text/javascript" src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
      <script type="text/javascript" src="assets/js/libs/respond.min.js"></script>
    <![endif]-->
        <!-- Bootstrap plugins -->

        <script src="assets/js/bootstrap/bootstrap.js"></script>

        <!-- Core plugins ( not remove ) -->

        <script src="assets/js/libs/modernizr.custom.js"></script>

        <!-- Handle responsive view functions -->

        <script src="assets/js/jRespond.min.js"></script>

        <!-- Custom scroll for sidebars,tables and etc. -->

        <script src="assets/plugins/core/slimscroll/jquery.slimscroll.min.js"></script>

        <script src="assets/plugins/core/slimscroll/jquery.slimscroll.horizontal.min.js"></script>

        <!-- Highlight code blocks -->

        <script src="assets/plugins/misc/highlight/highlight.pack.js"></script>

        <!-- Handle template sounds -->

        <script src="assets/plugins/misc/ion-sound/ion.sound.js"></script>

        <!-- Proivde quick search for many widgets -->

        <script src="assets/plugins/core/quicksearch/jquery.quicksearch.js"></script>

        <!-- Prompt modal -->

        <script src="assets/plugins/ui/bootbox/bootbox.js"></script>

        <!-- Other plugins ( load only nessesary plugins for every page) -->

        <script src="assets/plugins/forms/icheck/jquery.icheck.js"></script>

        <script src="assets/plugins/charts/flot/jquery.flot.custom.js"></script>

        <script src="assets/plugins/charts/flot/jquery.flot.pie.js"></script>

        <script src="assets/plugins/charts/flot/jquery.flot.resize.js"></script>

        <script src="assets/plugins/charts/flot/jquery.flot.time.js"></script>

        <script src="assets/plugins/charts/flot/jquery.flot.growraf.js"></script>

        <script src="assets/plugins/charts/flot/jquery.flot.categories.js"></script>

        <script src="assets/plugins/charts/flot/jquery.flot.stack.js"></script>

        <script src="assets/plugins/charts/flot/jquery.flot.orderBars.js"></script>

        <script src="assets/plugins/charts/flot/jquery.flot.tooltip.min.js"></script>

        <script src="assets/plugins/charts/flot/date.js"></script>

        <script src="assets/plugins/charts/sparklines/jquery.sparkline.js"></script>

        <script src="assets/plugins/charts/pie-chart/jquery.easy-pie-chart.js"></script>

        <script src="assets/plugins/ui/weather/skyicons.js"></script>

        <script src="assets/plugins/ui/calendar/fullcalendar.js"></script>

        <script src="assets/js/jquery.appStart.js"></script>

        <script src="assets/js/app.js"></script>

        <script src="assets/js/pages/dashboard.js"></script>

    </asp:MultiView>
    </form>
</body>
</html>
