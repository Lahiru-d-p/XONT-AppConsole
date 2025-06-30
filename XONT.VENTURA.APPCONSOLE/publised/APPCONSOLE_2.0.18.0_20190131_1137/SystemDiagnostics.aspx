<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemDiagnostics.aspx.cs" EnableEventValidation="false" Inherits="XONT.Ventura.AppConsole.SystemDiagnostics" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="XONT.Common.CustomControls" Namespace="XONT.Common.CustomControls" TagPrefix="cc3" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script src="js/gridviewScroll.min.js" type="text/javascript"></script>
    <script src="js/jquery-ui.min.js" type="text/javascript"></script>
    <link href="css/fontawesome-iconpicker.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="icon-fonts/font-awesome-4.6.3/css/font-awesome.min.css" rel="stylesheet" />

    <script type="text/javascript">

        function pageLoad() {
            var gridWidth = $('.Gridtablestyle1').width() + 21;
            $('.Gridtablestyle1').gridviewScroll({
                width: gridWidth,
                height: 385
            });

            var gridWidth2 = $('.Gridtablestyle2').width() + 21;
            $('.Gridtablestyle2').gridviewScroll({
                width: gridWidth2,
                height: 385
            });

            var gridWidth3 = $('.Gridtablestyle3').width() + 21;
            $('.Gridtablestyle3').gridviewScroll({
                width: gridWidth3,
                height: 385
            });
            var gridWidth4 = $('.Gridtablestyle4').width() + 21;
            $('.Gridtablestyle4').gridviewScroll({
                width: gridWidth4,
                height: 385
            });
            var gridWidth5 = $('.Gridtablestyle5').width() + 21;
            $('.Gridtablestyle5').gridviewScroll({
                width: gridWidth5,
                height: 385
            });
            var gridWidth6 = $('.Gridtablestyle6').width() + 21;
            $('.Gridtablestyle6').gridviewScroll({
                width: gridWidth6,
                height: 385
            });
            LightSearch();
        }


    </script>
    <link href="App_Themes/Blue/StyleSheet.css" rel="stylesheet" />
    <style>
        .leftContent {
            padding-bottom: 40px;
        }

        .rightContent {
            display: inline-block;
            position: absolute;
            right: 1px;
            top: -4px;
            padding-bottom: 40px;
        }

        .gridWrapper {
            padding-bottom: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdateProgress ID="uprProgress" runat="server" DisplayAfter="120">
            <ProgressTemplate>
                <div id="divProgressBack" runat="server">
                    <div id="dvProgress" class="loading"></div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="leftContent">
                    <asp:Button runat="server" Style="display: none;" ID="btnForMessageDisplay" />
                    <cc3:GridExports ID="GridExports5" runat="server"
                        GrvHeader="XONT Libraries" AssociateGridID="grvXontLibs" />

                    <div class="gridWrapper">
                        <asp:GridView ID="grvXontLibs" runat="server" AutoGenerateColumns="False" BackColor="White"
                            BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" AllowPaging="false"
                            CellPadding="2" CssClass="Gridtablestyle5" OnSelectedIndexChanged="grvXontLibs_SelectedIndexChanged"
                            EmptyDataText="No data found for the given criteria" EmptyDataRowStyle-CssClass="Labelstyle"
                            AllowSorting="True" OnSorting="grvXontLibs_Sorting">
                            <RowStyle ForeColor="#000066" />
                            <Columns>
                                <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="name"
                                    HeaderText="Name" SortExpression="name">
                                    <HeaderStyle CssClass="GridPadding" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="version"
                                    HeaderText="Version" SortExpression="version">
                                    <HeaderStyle CssClass="GridPadding" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="BuildDate" DataFormatString="{0:d}"
                                    HeaderText="Build Date" SortExpression="BuildDate">
                                    <HeaderStyle CssClass="GridPadding" />
                                </asp:BoundField>
                                <asp:CommandField ButtonType="Link" HeaderText="Select" ItemStyle-VerticalAlign="Middle" SelectText=""
                                    ShowSelectButton="true" ControlStyle-CssClass="fa fa-hand-o-right" />
                            </Columns>
                            <HeaderStyle CssClass="GridviewScrollHeader" />
                            <RowStyle CssClass="GridviewScrollItem" />
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <SelectedRowStyle CssClass="GridviewScrollSelected" />
                        </asp:GridView>
                    </div>

                    <cc3:GridExports ID="GridExports4" runat="server"
                        GrvHeader="Client Side Libraries" AssociateGridID="grvClientLibraries" />
                    <div class="gridWrapper">
                        <asp:GridView ID="grvClientLibraries" runat="server" AutoGenerateColumns="False" BackColor="White"
                            BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" AllowPaging="false"
                            CellPadding="2" CssClass="Gridtablestyle4" EmptyDataRowStyle-CssClass="Labelstyle"
                            AllowSorting="True" OnSorting="grvClientLibraries_Sorting">
                            <RowStyle ForeColor="#000066" />
                            <Columns>
                                <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="name"
                                    HeaderText="Name" SortExpression="name">
                                    <HeaderStyle CssClass="GridPadding" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="version"
                                    HeaderText="Version" SortExpression="version">
                                    <HeaderStyle CssClass="GridPadding" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="GridviewScrollHeader" />
                            <RowStyle CssClass="GridviewScrollItem" />
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <SelectedRowStyle CssClass="GridviewScrollSelected" />
                        </asp:GridView>
                    </div>

                    <cc3:GridExports ID="GridExports3" runat="server"
                        GrvHeader="Third Party Libraries" AssociateGridID="grvOtherLiberies" />

                    <div class="gridWrapper">
                        <asp:GridView ID="grvOtherLiberies" runat="server" AutoGenerateColumns="False" BackColor="White"
                            BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" AllowPaging="false"
                            CellPadding="2" CssClass="Gridtablestyle2"
                            EmptyDataText="No data found for the given criteria" EmptyDataRowStyle-CssClass="Labelstyle"
                            AllowSorting="True" OnSorting="grvOtherLiberies_Sorting">
                            <RowStyle ForeColor="#000066" />
                            <Columns>
                                <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="name"
                                    HeaderText="Name" SortExpression="name">
                                    <HeaderStyle CssClass="GridPadding" />
                                </asp:BoundField>
                                <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="version"
                                    HeaderText="Version" SortExpression="version">
                                    <HeaderStyle CssClass="GridPadding" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="GridviewScrollHeader" />
                            <RowStyle CssClass="GridviewScrollItem" />
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <SelectedRowStyle CssClass="GridviewScrollSelected" />
                        </asp:GridView>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="uplRightPanel" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div runat="server" id="div2ndLayerLibs" class="rightContent">
                            <div runat="server" id="divComponentLibs" visible="false">
                                <cc3:GridExports ID="GridExports2" runat="server"
                                    GrvHeader="Component Libraries" AssociateGridID="grvComponentLibraries" />
                                <div class="gridWrapper">
                                    <asp:GridView ID="grvComponentLibraries" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" AllowPaging="false"
                                        CellPadding="2" CssClass="Gridtablestyle1"
                                        EmptyDataText="No data found for the given criteria" EmptyDataRowStyle-CssClass="Labelstyle"
                                        AllowSorting="True" OnSorting="grvComponentLibraries_Sorting">
                                        <RowStyle ForeColor="#000066" />
                                        <Columns>
                                            <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="name"
                                                HeaderText="Name" SortExpression="name">
                                                <HeaderStyle CssClass="GridPadding" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="version"
                                                HeaderText="Version" SortExpression="version">
                                                <HeaderStyle CssClass="GridPadding" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="BuildDate" DataFormatString="{0:d}"
                                                HeaderText="Build Date" SortExpression="BuildDate">
                                                <HeaderStyle CssClass="GridPadding" />
                                            </asp:BoundField>


                                        </Columns>
                                        <HeaderStyle CssClass="GridviewScrollHeader" />
                                        <RowStyle CssClass="GridviewScrollItem" />
                                        <PagerStyle CssClass="GridviewScrollPager" />
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <SelectedRowStyle CssClass="GridviewScrollSelected" />
                                    </asp:GridView>
                                </div>
                            </div>
                            <div runat="server" id="divLevel1RefLibs" visible="false">
                                <cc3:GridExports ID="GridExports1" runat="server"
                                    GrvHeader="Related Libraries - Level 1" AssociateGridID="grvRelatedLibraries" />

                                <div class="gridWrapper">
                                    <asp:GridView ID="grvRelatedLibraries" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" AllowPaging="false"
                                        CellPadding="2" CssClass="Gridtablestyle3"
                                        EmptyDataText="No data found for the given criteria" EmptyDataRowStyle-CssClass="Labelstyle"
                                        AllowSorting="True" OnSorting="grvRelatedLibraries_Sorting">
                                        <RowStyle ForeColor="#000066" />
                                        <Columns>
                                            <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="name"
                                                HeaderText="Name" SortExpression="name">
                                                <HeaderStyle CssClass="GridPadding" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="version"
                                                HeaderText="Version" SortExpression="version">
                                                <HeaderStyle CssClass="GridPadding" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="BuildDate" DataFormatString="{0:d}"
                                                HeaderText="Build Date" SortExpression="BuildDate">
                                                <HeaderStyle CssClass="GridPadding" />
                                            </asp:BoundField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridviewScrollHeader" />
                                        <RowStyle CssClass="GridviewScrollItem" />
                                        <PagerStyle CssClass="GridviewScrollPager" />
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <SelectedRowStyle CssClass="GridviewScrollSelected" />
                                    </asp:GridView>
                                </div>
                            </div>

                            <div runat="server" id="divLevel2RefLibs" visible="false">

                                <cc3:GridExports ID="GridExports6" runat="server"
                                    GrvHeader="Related Libraries - Level 2  (Excluding Level 1)" AssociateGridID="grv2ndLayerLibs" />
                                <div class="gridWrapper">
                                    <asp:GridView ID="grv2ndLayerLibs" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" AllowPaging="false"
                                        CellPadding="2" CssClass="Gridtablestyle6" EmptyDataRowStyle-CssClass="Labelstyle"
                                        AllowSorting="True" OnSorting="grv2ndLayerLibs_Sorting">
                                        <RowStyle ForeColor="#000066" />
                                        <Columns>
                                            <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="name"
                                                HeaderText="Name" SortExpression="name">
                                                <HeaderStyle CssClass="GridPadding" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="version"
                                                HeaderText="Version" SortExpression="version">
                                                <HeaderStyle CssClass="GridPadding" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderStyle-CssClass="GridPadding" DataField="BuildDate" DataFormatString="{0:d}"
                                                HeaderText="Build Date" SortExpression="BuildDate">
                                                <HeaderStyle CssClass="GridPadding" />
                                            </asp:BoundField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridviewScrollHeader" />
                                        <RowStyle CssClass="GridviewScrollItem" />
                                        <PagerStyle CssClass="GridviewScrollPager" />
                                        <FooterStyle BackColor="White" ForeColor="#000066" />
                                        <SelectedRowStyle CssClass="GridviewScrollSelected" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
