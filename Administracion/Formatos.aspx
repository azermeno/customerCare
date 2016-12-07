<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Formatos.aspx.cs" Inherits="CustomerCare.Scripts.Formatos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
<head runat="server">
    <title></title>
<link rel="stylesheet" type="text/css" href="<%=ConfigurationManager.AppSettings["Telerik.Web.UI.StyleSheetFolders"] %>/base.css" />
    <style type="text/css">
    body   
{
    margin: 0px 0px 0px 0px;
    padding: 0px;
    height: 100%;
    width: 100%;
}
    #RadSplitter1
    {
        display:block;
        width:100%;
        height:100%;
    }
</style>
<script type="text/javascript">
    function RadToolBar1_ButtonClicked(s, a) {
        switch (a.get_item().get_text()) {
            case "Nuevo":
                window.parent.addtab("NuevoFormato.aspx", "Nuevo formato", "nFormato").select();
                break;
        }
    }
    function RowSelected(s, a) {
        $find("RadPane2").set_content(a.get_gridDataItem().get_cell("Texto").innerHTML);
    }
    function rblClick(radio) {
        if (radio.value == "Formato")
            document.getElementById("trURL").style.display = "none";
        else 
            document.getElementById("trURL").style.display = "table-row";
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
                <%--<telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
                    <StyleSheets>
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Button.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Calendar.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/ComboBox.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Dock.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Editor.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Grid.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Input.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/ListBox.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Menu.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Slider.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Splitter.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/TabStrip.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/ToolBar.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/ToolTip.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/TreeList.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/TreeView.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Upload.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Window.Ayuntamiento.css" />
                    </StyleSheets>
                </telerik:RadStyleSheetManager>--%>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager><%--
    <telerik:RadSkinManager ID="RadSkinManager1" Runat="server" Skin="Black">
    </telerik:RadSkinManager>--%>
    <table style="width: 100%; height: 100%;" cellspacing="0">
    <tr><td>
    <telerik:RadSplitter ID="RadSplitter1" Runat="server" 
            Width="100%" Height="100%">
        <telerik:RadPane ID="RadPane1" Runat="server" Scrolling="None">
            <telerik:RadToolBar ID="RadToolBar1" Runat="server" 
                onclientbuttonclicked="RadToolBar1_ButtonClicked" Width="100%">
                <Items>
                    <telerik:RadToolBarButton runat="server" Enabled="False" 
                        Text="FORMATOS                 ">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" IsSeparator="True">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarSplitButton runat="server" Text="Nuevo">
                        <Buttons>
                            <telerik:RadToolBarButton runat="server" Text="Nuevo Formato">
                            </telerik:RadToolBarButton>
                            <telerik:RadToolBarButton runat="server" Text="Nuevo Enlace">
                            </telerik:RadToolBarButton>
                        </Buttons>
                    </telerik:RadToolBarSplitButton>
                    <telerik:RadToolBarButton runat="server" Text="Editar">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" Text="Eliminar">
                    </telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowSorting="True" 
                CellSpacing="0" DataSourceID="sdsFormatos" GridLines="None" Skin="Default" 
                Height="100%" AutoGenerateColumns="False">
                <ClientSettings AllowKeyboardNavigation="true">
                    <Selecting AllowRowSelect="True" />
                    <ClientEvents OnActiveRowChanged="RowSelected" OnRowSelected="RowSelected" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                </ClientSettings>
                <MasterTableView DataSourceID="sdsFormatos">
                    <CommandItemSettings ExportToPdfText="Export to PDF" 
                        ExportToCsvImageUrl="mvwres://Telerik.Web.UI, Version=2011.1.519.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.ExportToCsv.gif" 
                        ExportToExcelImageUrl="mvwres://Telerik.Web.UI, Version=2011.1.519.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.ExportToExcel.gif" 
                        ExportToPdfImageUrl="mvwres://Telerik.Web.UI, Version=2011.1.519.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.ExportToPdf.gif" 
                        ExportToWordImageUrl="mvwres://Telerik.Web.UI, Version=2011.1.519.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Grid.ExportToWord.gif" />
                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column">
                        <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column">
                        <HeaderStyle Width="20px" />
                    </ExpandCollapseColumn>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Nombre" 
                            FilterControlAltText="Filter Nombre column" HeaderText="Nombre" 
                            SortExpression="Nombre" UniqueName="Nombre">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Tipificacion" DataType="System.Int32" 
                            FilterControlAltText="Filter Tipificacion column" HeaderText="Tipificacion" 
                            SortExpression="Tipificacion" UniqueName="Tipificacion">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Texto" Display="False" 
                            FilterControlAltText="Filter Texto column" HeaderText="Texto" 
                            SortExpression="Texto" UniqueName="Texto">
                        </telerik:GridBoundColumn>
                    </Columns>
                    <EditFormSettings>
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                    </EditFormSettings>
                </MasterTableView>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
            </telerik:RadGrid>
            <asp:SqlDataSource ID="sdsFormatos" runat="server" 
                ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                SelectCommand="SELECT [Nombre], [Tipificacion], [Texto] FROM [Formatos]">
            </asp:SqlDataSource>
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitBar1" Runat="server">
            </telerik:RadSplitBar>
        <telerik:RadPane ID="RadPane2" Runat="server" ContentUrl="NuevoFormato.aspx">
        </telerik:RadPane>
    </telerik:RadSplitter>
    </td></tr></table>
    <telerik:RadDock ID="RadDock1" Runat="server" CloseText="Cerrar" 
        DefaultCommands="Close" DockMode="Floating" Height="150px" Title="Nuevo" 
        Width="350px" EnableViewState="False" Left="250px" Top="100px">
        <ContentTemplate>
        Formato<input type="radio" value="Formato" name="rbl" checked="checked" onclick="rblClick(this)"/> 
        Enlace<input type="radio" value="Enlace" name="rbl" onclick="rblClick(this)"/>
        <table>
            <tr>
                <td>Nombre:</td>
                <td><telerik:RadTextBox ID="rtbNombre" Runat="server" 
            EmptyMessage="Nombre..." Height="22px" Width="241px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td>Tipificación:</td>
                <td><telerik:RadTextBox ID="rtbTipificacion" Runat="server" 
            EmptyMessage="Click aquí." Height="22px" Width="241px"></telerik:RadTextBox></td>
            </tr>
            <tr id="trURL" style="display:none">
                <td>URL:</td>
                <td><telerik:RadTextBox ID="rtbURL" Runat="server" 
            EmptyMessage="URL a la que se enlazará" Height="22px" Width="241px"></telerik:RadTextBox></td>
            </tr>
        </table>
        </ContentTemplate>
    </telerik:RadDock>
    </form>
</body>
</html>
