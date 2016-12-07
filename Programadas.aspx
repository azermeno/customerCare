<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Programadas.aspx.cs" Inherits="CustomerCare.Programadas" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Programadas</title>
    <style type="text/css">
    body, form, html   
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
        var programada;
        function RadToolBar1_ButtonClicked(s, e) {
            switch (e.get_item().get_value()) {
                case "A":
                    $find("RadDock1").set_closed(false);
                    break;
                case "E":
                    if ($find("RadGrid1").get_selectedItems().length == 1) 
                        $find("RadDock2").set_closed(false);
                    break;
            }
        }
        function RowSelected(s, a) {
            programada = frames[0];
            $find("RadToolBar1").findItemByValue("E").enable();
            programada.$find("rtbAsunto").set_value(a.get_gridDataItem().get_cell("Asunto").innerHTML.replace(/^(\&nbsp\;)+|(\&nbsp\;)+$/g, ''));
            programada.$find("rtbDetalle").set_value(a.get_gridDataItem().get_cell("DetalleAsunto").innerHTML.replace(/^(\&nbsp\;)+|(\&nbsp\;)+$/g, ''));
            programada.document.getElementById("hflCodigo").value = a.get_gridDataItem().get_cell("Codigo").innerHTML.replace(/^(\&nbsp\;)+|(\&nbsp\;)+$/g, '');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" Runat="server" Skin="Black">
    </telerik:RadSkinManager>
    <table style="width: 100%; height: 100%;" cellspacing="0">
    <tr><td>
    <telerik:RadSplitter ID="RadSplitter1" Runat="server" 
            Width="100%" Height="100%">
        <telerik:RadPane ID="RadPane1" Runat="server" Scrolling="None" Width="260px">
            <telerik:RadToolBar ID="RadToolBar1" Runat="server" 
                onclientbuttonclicked="RadToolBar1_ButtonClicked" Width="100%">
                <Items>
                    <telerik:RadToolBarButton runat="server" Text="Añadir" Value="A">
                    </telerik:RadToolBarButton>
                    <telerik:RadToolBarButton runat="server" Text="Eliminar" Value="E" Enabled="false">
                    </telerik:RadToolBarButton>
                </Items>
            </telerik:RadToolBar>
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowSorting="True" 
                CellSpacing="0" DataSourceID="sdsProgramadas" GridLines="None" Skin="Default" 
                Height="100%" AutoGenerateColumns="False">
                <ClientSettings AllowKeyboardNavigation="true">
                    <Selecting AllowRowSelect="True" />
                    <ClientEvents OnActiveRowChanged="RowSelected" OnRowSelected="RowSelected" />
                    <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                </ClientSettings>
                <MasterTableView DataSourceID="sdsProgramadas">
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
                        <telerik:GridBoundColumn DataField="Codigo" DataType="System.Int32" 
                            FilterControlAltText="Filter Tipificacion column" HeaderText="Codigo" 
                            SortExpression="Tipificacion" UniqueName="Codigo" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Nombre" 
                            FilterControlAltText="Filter Nombre column" HeaderText="Nombre" 
                            SortExpression="Nombre" UniqueName="Nombre">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Asunto" Display="False" 
                            FilterControlAltText="Filter Texto column" HeaderText="Asunto" 
                            SortExpression="Texto" UniqueName="Asunto">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DetalleAsunto" Display="False" 
                            FilterControlAltText="Filter Texto column" HeaderText="DetalleAsunto" 
                            SortExpression="Texto" UniqueName="DetalleAsunto">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SolRes" Display="False" 
                            FilterControlAltText="Filter Texto column" HeaderText="SolRes" 
                            SortExpression="Texto" UniqueName="SolRes">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Tipificacion" DataType="System.Int32" 
                            FilterControlAltText="Filter Tipificacion column" HeaderText="Tipificacion" 
                            SortExpression="Tipificacion" UniqueName="Tipificacion" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Periodo" Display="False" 
                            FilterControlAltText="Filter Texto column" HeaderText="Periodo" 
                            SortExpression="Texto" UniqueName="Periodo">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Tiempo" DataType="System.Int32" 
                            FilterControlAltText="Filter Tipificacion column" HeaderText="Tipificacion" 
                            SortExpression="Tipificacion" UniqueName="Tiempo" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Caducidad" DataType="System.DateTime" 
                            FilterControlAltText="Filter Tipificacion column" HeaderText="Caducidad" 
                            SortExpression="Tipificacion" UniqueName="Caducidad" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Fecha" DataType="System.DateTime" 
                            FilterControlAltText="Filter Tipificacion column" HeaderText="Fecha" 
                            SortExpression="Tipificacion" UniqueName="Fecha" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Hora" DataType="System.DateTime" 
                            FilterControlAltText="Filter Tipificacion column" HeaderText="Hora" 
                            SortExpression="Tipificacion" UniqueName="Hora" Display="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Datos" 
                            FilterControlAltText="Filter Tipificacion column" HeaderText="Datos" 
                            SortExpression="Tipificacion" UniqueName="Datos" Display="false">
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
            <asp:SqlDataSource ID="sdsProgramadas" runat="server" 
                ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" 
                
                SelectCommand="SELECT [Codigo], [Nombre], [Asunto], [DetalleAsunto], [SolRes], [Tipificacion], [periodo] as Periodo, [tiempo_asignado] as Tiempo, [caducidad] as Caducidad, [fecha] as Fecha, [hora] as Hora, [datos] as Datos FROM [Programadas]">
            </asp:SqlDataSource>
        </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitBar1" Runat="server">
            </telerik:RadSplitBar>
        <telerik:RadPane ID="RadPane2" Runat="server" ContentUrl="Programada.aspx">
        </telerik:RadPane>
    </telerik:RadSplitter>
    </td></tr></table>
    <telerik:RadDock ID="RadDock1" Runat="server" CloseText="Cerrar" 
        DefaultCommands="Close" DockMode="Floating" Height="95px" Title="Añadir" 
        Width="316px" EnableViewState="False" Left="250px" Top="100px" 
        Closed="True">
        <ContentTemplate>
        <table>
            <tr>
                <td>Nombre:</td>
                <td><telerik:RadTextBox ID="rtbNombre" Runat="server" 
            EmptyMessage="Nombre..." Height="22px" Width="241px"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td></td>
                <td align="right">
                    <telerik:RadButton ID="rbtAceptar" runat="server" Text="Aceptar" Skin="Default" 
                        onclick="rbtAceptar_Click">
                    </telerik:RadButton></td>
            </tr>
        </table>
        </ContentTemplate>
    </telerik:RadDock>
    <telerik:RadDock ID="RadDock2" Runat="server" CloseText="Cerrar" 
        DefaultCommands="Close" DockMode="Floating" Height="81px" Title="Eliminar" 
        Width="250px" EnableViewState="False" Left="250px" Top="100px" 
        Closed="True">
        <ContentTemplate>
        <table width="100%">
            <tr>
                <td align="center">¿En verdad desea eliminarla?</td>
            </tr>
            <tr>
                <td align="center">
                    <telerik:RadButton ID="rbtEliminar" runat="server" Text="Aceptar" Skin="Default" 
                        onclick="rbtEliminar_Click">
                    </telerik:RadButton></td>
            </tr>
        </table>
        </ContentTemplate>
    </telerik:RadDock>
    </form>
</body>
</html>
