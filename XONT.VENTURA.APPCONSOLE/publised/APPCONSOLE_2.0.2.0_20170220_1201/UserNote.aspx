<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserNote.aspx.cs" Inherits="XONT.Ventura.HandTools._Default" %>

<!DOCTYPE html>
<html lang="en" class=" js no-touch">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <title></title>
    <link href="assets_hand/css/icons.css" rel="stylesheet">
    <link href="assets_hand/css/bootstrap.css" rel="stylesheet">
    <link href="assets_hand/css/plugins.css" rel="stylesheet">
    <link href="assets_hand/css/main.css" rel="stylesheet">

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
    </style>
</head>
<body style="background-color:#fff;">
    <form id="form1" runat="server">
    <asp:MultiView ID="mvwScreens" runat="server" ActiveViewIndex="0">
        <asp:View ID="viwLoadNotes" runat="server">
            <div id="content" class="collapsed-sidebar" style="border-style:none;">
                <div class="content-wrapper">
                    <div class="content-inner" style="border-style: solid;border-width: 1px; border-top:0px; border-color:#fff;">
                        <!-- Start .row -->
                        <div class="row">
                            <!-- col-lg-8 end here -->
                            <div class="col-lg-4 col-md-4 sortable-layout ui-sortable">
                                <!-- col-lg-4 start here -->
                                <!-- End .shortcut buttons -->
                                <!-- End .panel -->
                                <div class="panel panel-teal  plain toggle panelMove panelClose panelRefresh" id="jst_0" style="margin-bottom:0px;">
                                    <!-- Start .panel -->
                                    <div class="panel-heading" style="border-style:none; padding:0px 10px !important;">
                                        <%--<h4 class="panel-title">
                                            <i class="fa fa-th-list"></i>User Notes</h4>--%>
                                        <div class="panel-controls" style="z-index: 200000;">
                                            <asp:Panel ID="Panel2" runat="server">
                                                <asp:LinkButton ID="btnSelect" ToolTip="Select Multiple" runat="server" OnClick="btnSelect_Click"><i class="fa fa-pencil-square-o fa-lg"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnNewNote" ToolTip="New Note" runat="server" OnClick="btnNewNote_Click"><i class="fa fa-plus-square-o fa-lg"></i></asp:LinkButton>
                                                <asp:LinkButton ID="btnDelete" ToolTip="Delete Selected" runat="server" Visible="false" OnClick="btnDelete_Click"><i class="fa fa-trash-o fa-lg"></i></asp:LinkButton>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <div class="panel-body" style="display: block; border-style:none; padding:0px 10px !important;">
                                        <div class="todo-widget">
                                            <div style="float:left; margin-top:4px; margin-bottom:10px;">
                                                <h4 class="panel-title" style="display: inline; font-weight: 200; font-size: 14px;">
                                                    Sort by</h4>
                                                <span class="panel-title" style="display: inline-block;">
                                                    <asp:DropDownList ID="ddlSort" runat="server" AutoPostBack="true"
                                        onselectedindexchanged="ddlSort_SelectedIndexChanged">
                                        <asp:ListItem Text="Sort by"></asp:ListItem>
                                        <asp:ListItem Text="Date Modified" Value="UpdatedOn Desc" ></asp:ListItem>
                                        <asp:ListItem Text="Date Modified" Value="UpdatedOn" ></asp:ListItem>
                                        <asp:ListItem Text="Name" Value ="NoteTitle Desc"></asp:ListItem>
                                        <asp:ListItem Text="Name" Value="NoteTitle "></asp:ListItem>
                                    </asp:DropDownList>
                                                </span>
                                            </div>
                                            
                                                    <div class="todo-task-text">
                                                        <asp:GridView Style="background: none; border: none; width:100%;" ID="grvNotes" runat="server"
                                                            AutoGenerateColumns="False" DataKeyNames="NoteTitle" GridLines="Vertical" ShowHeader="False">
                                                            <Columns>
                                                                <asp:TemplateField Visible="false" ShowHeader="False" HeaderStyle-Wrap="true" ItemStyle-BorderStyle="None" ItemStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="cboxSelectNote" runat="server" BorderWidth="0px" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="title" SortExpression="name" ItemStyle-BorderStyle="None">
                                                                    <ItemTemplate><ul class="todo-list" id="today">
                                                <li class="todo-task-item">
                                                                        <asp:Button ID="btnNote" runat="server" Text='<%#Bind("NoteTitle")%>' AutoPostBack="True"
                                                                            OnClick="btnNoteSelected_Click" Style="background: none; border: none;" /></li>
                                            </ul>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                <div style="width:310px; position:fixed; bottom:0px; text-align:center;">
                                            <asp:Label ID="lblDiscountEditMsg" runat="server" CssClass="Errormessagetextstyle"
                                                ForeColor="Red"></asp:Label>
                                            <asp:Label ID="lblNoNotesMsg" runat="server" CssClass="Errormessagetextstyle" ForeColor="Red"></asp:Label></div>
        </asp:View>
        <div id="Div1" class="collapsed-sidebar">
            <div class="content-wrapper">
                <div class="content-inner"  style="border-style: solid;border-width: 1px; border-top:0px; border-color:#fff;">
                    <asp:View ID="viwNoteDisplay" runat="server">
                        <asp:Panel ID="Panel1" runat="server">
                            <asp:Panel ID="pnlNote" runat="server">
                                <!-- Start .row -->
                                <div class="row">
                                    <!-- col-lg-8 end here -->
                                    <div class="col-lg-4 col-md-4 sortable-layout ui-sortable">
                                        <!-- col-lg-4 start here -->
                                        <!-- End .shortcut buttons -->
                                        <!-- End .panel -->
                                        <div class="panel panel-teal  plain toggle panelMove panelClose panelRefresh" id="Div2" style="margin-bottom:0px;">
                                            <!-- Start .panel -->
                                            <div class="panel-heading" style="border-style:none; padding:0px 10px !important;">

                                        <h4 class="panel-title">
                                            <i class="fa fa-th-list"></i>User Notes</h4>
                                            <div class="panel-controls">
                                            <asp:Panel ID="Panel4" runat="server">
                                            <asp:LinkButton ID="btnSave" ToolTip="Save Changes" runat="server" OnClick="btnSave_Click"><i class="fa fa-floppy-o fa-lg"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btnAmend" ToolTip="Edit Note" runat="server" CssClass="MainButtonStyle" OnClick="btnAmend_Click" Visible="false" ><i class="fa fa-floppy-o fa-lg"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btnCancel" ToolTip="Close/Cancel" runat="server" OnClick="btnCancel_Click" CausesValidation="False" ><i class="fa fa-times fa-lg"></i></asp:LinkButton>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                                                
                                                                
                                            <div class="panel-body" style="display: block; border-style:none; padding:0px 10px !important;">
                                                <div class="todo-widget">
                                                    <ul class="todo-list" id="Ul1">
                                                        <li class="todo-task-item">
                                                            <div class="todo-task-text">
                                                                <%--<asp:Label ID="lblNoteTitle" runat="server" CssClass="Captionstyle" Text="Tittle : "></asp:Label>--%>
                                                                <asp:TextBox ID="txtNoteTitle" runat="server" CssClass="Textboxstyle" MaxLength="30"
                                                                    EnableViewState="False" style="width: 100%; margin-top:10px;"></asp:TextBox>
                                                                
                                                                <asp:TextBox ID="txtNoteBody" runat="server" CssClass="Textboxstyle" MaxLength="50"
                                                                    EnableTheming="True" Rows="4" TextMode="MultiLine" style="width: 100%; margin-top:10px;"></asp:TextBox>
                                                                
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNoteTitle"
                                                                    ErrorMessage="*" ForeColor="Red" Display="Dynamic">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:Label ID="lblNoteBodyMsg" runat="server" CssClass="Errormessagetextstyle" ForeColor="Red" EnableViewState="false"></asp:Label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                </div>
                </asp:Panel> </asp:Panel> </asp:View>
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
