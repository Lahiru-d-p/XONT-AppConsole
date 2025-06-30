<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HelpContents.aspx.cs" Inherits="AppConsole.HelpContents" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Help</title>
    
       <link rel="Stylesheet" type="text/css" href="compomain.css" />
       
    <style type="text/css">
        .style1
        {
            width: 273px;
        }
        .Maintextstyle
        {}
    </style>
       
</head>
<script type ="text/jscript">   
    function changeScreenSize(w,h){window.resizeTo( w,h )}
</script>

<body>
    <form id="form2" runat="server" style="height: 500px">
     <div>
    <table border="0" cellpadding="0" cellspacing="0" 
             style="width: 915px; height: 519px;">
    
  <tr>
    <td width="" height="396" valign="top"><table width="" border="0" cellpadding="0" cellspacing="0">
      <tr>
        <td width="8" height="8" valign="top"><img src="images/border01.jpg" width="8" height="8" alt=""></td>
          <td width="100%" valign="top" background="images/borderline1.jpg"><img src="images/borderline1.jpg" width="8" height="8"></td>
          <td width="8" valign="top"><img src="images/border02.jpg" width="8" height="8"></td>
        </tr>
      <tr>
        <td valign="top" background="images/borderline4.jpg" class="style4"><img src="images/borderline4.jpg" width="8" height="8"></td>
          <td valign="top" class="style4">

<table cellpadding="0" cellspacing="0" style="width: 500px; height: 500px">
			<tr>
				<td valign="top" class="style1">
				    <asp:TreeView ID="trvTOC" runat="server" DataSourceID="xmlHelpContent" 
                        onselectednodechanged="trvTOC_SelectedNodeChanged" 
                        CssClass="Maintextstyle" Width="190px">
                        <DataBindings>
                            <asp:TreeNodeBinding DataMember="HelpPage" TextField="description" 
                                ValueField="ID" />
                        </DataBindings>
				    
                    </asp:TreeView>
                    <asp:XmlDataSource ID="xmlHelpContent" runat="server" DataFile="~/help.xml">
                    </asp:XmlDataSource>
                </td>
				<td valign="middle" align="center" style="width: 433px; height: 402px">				
				<iframe id="fraPage" runat="server" style="height: 489px; width:679px" 
                        class="Textboxstyle" >
				</iframe>
				</td>
			</tr>
			</table>



</td>
          <td valign="top" background="images/borderline2.jpg" class="style4"><img src="images/borderline2.jpg" width="8" height="8"></td>
        </tr>
      <tr>
        <td height="8" valign="top"><img src="images/border04.jpg" width="8" height="8"></td>
          <td valign="top" background="images/borderline3.jpg"><img src="images/borderline3.jpg" width="8" height="8"></td>
          <td valign="top"><img src="images/border03.jpg" width="8" height="8"></td>
        </tr>
    </table></td>
  </tr>
  
  </table>

    </div>
    </form>
</body>
</html>
