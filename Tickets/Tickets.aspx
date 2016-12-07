<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="CustomerCare.Tickets" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="margin:0px; height:100%;">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadSplitter ID="RadSplitter1" runat="server" 
        Height="100%" LiveResize="True" Orientation="Horizontal" SplitBarsSize="5px" 
        Width="100%" BorderSize="0">
        <telerik:RadPane ID="RadPane1" runat="server" 
            ContentUrl="TablaTickets.aspx" EnableViewState="False" MinHeight="150">
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitBar1" runat="server" 
            CollapseExpandPaneText="Colapsar/expandir el panel de abajo." 
            CollapseMode="Backward" BorderSize="10">
            <AdjacentPanesNames BottomPaneName="abajo" LeftPaneName="izquierda" 
                RightPaneName="derecha" TopPaneName="arriba" />
        </telerik:RadSplitBar>
        <telerik:RadPane ID="RadPane2" runat="server" ContentUrl="Ticket.aspx" 
            Height="150px">
        </telerik:RadPane>
    </telerik:RadSplitter>
    </form>
</body>
</html>
