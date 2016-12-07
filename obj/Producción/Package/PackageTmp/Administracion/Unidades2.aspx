<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Unidades2.aspx.cs" Inherits="CustomerCare.Unidades2" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Unidades</title>
</head>
<body class="BODY">
    <form id="mainForm" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function RowDblClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
    <telerik:RadGrid ID="RadGrid1" runat="server" CssClass="RadGrid" 
      GridLines="None" PageSize="20" AllowSorting="True" AutoGenerateColumns="False"
        ShowStatusBar="True" AllowAutomaticInserts="True"
        AllowAutomaticUpdates="True" DataSourceID="sdsZonas" CellSpacing="0" 
      Culture="es-MX" OnItemUpdated="RadGrid1_ItemUpdated" OnItemInserted="RadGrid1_ItemInserted" OnInsertCommand="RadGrid1_InsertCommand" OnItemCreated="RadGrid1_ItemCreated">
        <MasterTableView CommandItemDisplay="Top" DataSourceID="sdsZonas"
            DataKeyNames="Codigo" EditMode="InPlace" 
          NoDetailRecordsText="No hay registros." NoMasterRecordsText="No hay registros." Name="Zonas">
            <DetailTables>
              <telerik:GridTableView runat="server" 
                DataKeyNames="Codigo" DataSourceID="sdsSitios" CommandItemDisplay="Top" Name="Sitios">
                <ParentTableRelation>
                  <telerik:GridRelationFields MasterKeyField="Codigo" DetailKeyField="Zona" />
                </ParentTableRelation>
                <DetailTables>
                  <telerik:GridTableView runat="server" 
                    DataSourceID="sdsUnidades" CommandItemDisplay="Top" Name="Unidades" EditMode="InPlace">
                    <ParentTableRelation>
                      <telerik:GridRelationFields DetailKeyField="Sitio" MasterKeyField="Codigo" />
                    </ParentTableRelation>
                    <CommandItemSettings ExportToPdfText="Exportar a PDF" 
                      AddNewRecordText="Agregar" ExportToExcelText="Exportar a Excel" 
                      ShowExportToExcelButton="True" ShowExportToPdfButton="True" />
                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                      Visible="True">
                      <HeaderStyle Width="20px" />
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                      Visible="True">
                      <HeaderStyle Width="20px" />
                    </ExpandCollapseColumn>
                    <Columns>
                      <telerik:GridDragDropColumn FilterControlAltText="Filter DragDropColumn column">
                      </telerik:GridDragDropColumn>
                      <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" 
                        FilterControlAltText="Filter EditCommandColumn column" UpdateText="Guardar">
                      </telerik:GridEditCommandColumn>
                      <telerik:GridBoundColumn DataField="Codigo" 
                        FilterControlAltText="Filter Codigo column" HeaderText="Codigo" ReadOnly="True" 
                        UniqueName="Codigo">
                      </telerik:GridBoundColumn>
                      <telerik:GridCheckBoxColumn DataField="Activa" DataType="System.Boolean" 
                        FilterControlAltText="Filter Activa column" HeaderText="Activa" 
                        SortExpression="Activa" UniqueName="Activa">
                      </telerik:GridCheckBoxColumn>
                      <telerik:GridBoundColumn DataField="Alias" 
                        FilterControlAltText="Filter Alias column" HeaderText="Alias" 
                        SortExpression="Alias" UniqueName="Alias">
                      </telerik:GridBoundColumn>
                      <telerik:GridBoundColumn FilterControlAltText="Filter Nombre column" 
                        UniqueName="Nombre" DataField="Nombre" HeaderText="Nombre">
                      </telerik:GridBoundColumn>
                      <telerik:GridDropDownColumn DataField="Cliente" DataSourceID="sdsClientes" 
                        FilterControlAltText="Filter Cliente column" HeaderText="Cliente" 
                        ListTextField="Nombre" ListValueField="Codigo" SortExpression="Cliente" 
                        UniqueName="Cliente">
                      </telerik:GridDropDownColumn>
                      <telerik:GridDropDownColumn DataField="Producto" DataSourceID="sdsProductos" 
                        FilterControlAltText="Filter Producto column" HeaderText="Producto" 
                        ListTextField="Nombre" ListValueField="Codigo" SortExpression="Producto" 
                        UniqueName="Producto">
                      </telerik:GridDropDownColumn>
                    </Columns>
                    <EditFormSettings>
                      <EditColumn FilterControlAltText="Filter EditCommandColumn1 column" 
                        UniqueName="EditCommandColumn1">
                      </EditColumn>
                    </EditFormSettings>
                  </telerik:GridTableView>
                </DetailTables>
                <CommandItemSettings ExportToPdfText="Exportar a PDF" 
                  AddNewRecordText="Agregar" ExportToExcelText="Exportar a Excel" 
                  ShowExportToExcelButton="True" ShowExportToPdfButton="True" />
                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" 
                  Visible="True">
                  <HeaderStyle Width="20px" />
                </RowIndicatorColumn>
                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" 
                  Visible="True">
                  <HeaderStyle Width="20px" />
                </ExpandCollapseColumn>
                <Columns>
                  <telerik:GridDragDropColumn FilterControlAltText="Filter DragDropColumn column">
                  </telerik:GridDragDropColumn>
                  <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" 
                    FilterControlAltText="Filter EditCommandColumn column" UpdateText="Guardar">
                  </telerik:GridEditCommandColumn>
                  <telerik:GridCheckBoxColumn DataField="Activo" DataType="System.Boolean" 
                    FilterControlAltText="Filter Activo column" HeaderText="Activo" 
                    SortExpression="Activo" UniqueName="Activo">
                  </telerik:GridCheckBoxColumn>
                  <telerik:GridBoundColumn DataField="Nombre" 
                    FilterControlAltText="Filter Nombre column" HeaderText="Nombre" 
                    UniqueName="Nombre">
                  </telerik:GridBoundColumn>
                  <telerik:GridDropDownColumn DataField="Responsable" DataSourceID="sdsResponsables" 
                    FilterControlAltText="Filter Responsable column" HeaderText="Responsable" 
                    ListTextField="Nombre" ListValueField="Codigo" UniqueName="Responsable">
                  </telerik:GridDropDownColumn>
                </Columns>
                <EditFormSettings>
                  <EditColumn FilterControlAltText="Filter EditCommandColumn1 column" 
                    UniqueName="EditCommandColumn1">
                  </EditColumn>
                </EditFormSettings>
              </telerik:GridTableView>
            </DetailTables>
<CommandItemSettings ExportToPdfText="Exportar a PDF" AddNewRecordText="Agregar" 
              ExportToCsvText="Exportar a CSV" ExportToExcelText="Exportar a Excel" 
              ExportToWordText="Exportar a Word" RefreshText="Actualizar" 
              ShowExportToExcelButton="True" ShowExportToPdfButton="True" 
              ShowRefreshButton="False"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>
            <Columns>
                <telerik:GridBoundColumn UniqueName="Nombre" HeaderText="Nombre" 
                  DataField="Nombre" FilterControlAltText="Filter Nombre column" 
                  SortExpression="Nombre">
                <%--<telerik:GridCheckBoxColumn DataField="Activa" DataType="System.Boolean" 
                  FilterControlAltText="Filter Activa column" HeaderText="Activa" 
                  SortExpression="Activa" UniqueName="Activa">
                </telerik:GridCheckBoxColumn>--%>
                </telerik:GridBoundColumn>
                <telerik:GridDropDownColumn DataField="Supervisor" DataSourceID="sdsInternos" 
                  FilterControlAltText="Filter Supervisor column" HeaderText="Supervisor" 
                  ListTextField="Nombre" ListValueField="Codigo" UniqueName="Supervisor">
                </telerik:GridDropDownColumn>
                <telerik:GridEditCommandColumn CancelText="Cancelar" EditText="Editar" 
                  FilterControlAltText="Filter EditCommandColumn column" UpdateText="Guardar">
                </telerik:GridEditCommandColumn>
            </Columns>

<EditFormSettings>
<EditColumn UniqueName="EditCommandColumn1" FilterControlAltText="Filter EditCommandColumn1 column"></EditColumn>
</EditFormSettings>
        </MasterTableView>
        <ClientSettings AllowRowsDragDrop="True">
<Selecting CellSelectionMode="None" AllowRowSelect="True"></Selecting>

            <ClientEvents OnRowDblClick="RowDblClick" />
        </ClientSettings>

<FilterMenu EnableImageSprites="False"></FilterMenu>
    </telerik:RadGrid>

<asp:SqlDataSource ID="sdsUnidades" runat="server" ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
      SelectCommand="SELECT [Codigo], [Sitio], [Alias], [Producto], [Cliente], [Nombre], [Activa] FROM [Unidades] WHERE [Sitio]=@Sitio" 
      InsertCommand="INSERT INTO [Unidades] ([Sitio], [Alias], [Producto], [Cliente], [Nombre], [Activa]) VALUES (@Sitio,  @Alias, @Producto, @Cliente, @Nombre, @Activa)" 
      UpdateCommand="UPDATE [Unidades] SET [Alias] = @Alias, [Producto] = @Producto, [Cliente] = @Cliente, [Nombre] = @Nombre, [Activa] = @Activa WHERE [Codigo] = @Codigo" >
      <SelectParameters>
          <asp:Parameter Name="Sitio" Type="Int32" />
      </SelectParameters>
      <InsertParameters>
          <asp:Parameter Name="Sitio" Type="Int32" />
          <asp:Parameter Name="Alias" Type="String" />
          <asp:Parameter Name="Producto" Type="Int32" />
          <asp:Parameter Name="Cliente" Type="Int32" />
          <asp:Parameter Name="Nombre" Type="String" />
          <asp:Parameter Name="Activa" Type="Boolean" />
      </InsertParameters>
      <UpdateParameters>
          <asp:Parameter Name="Alias" Type="String" />
          <asp:Parameter Name="Producto" Type="Int32" />
          <asp:Parameter Name="Cliente" Type="Int32" />
          <asp:Parameter Name="Nombre" Type="String" />
          <asp:Parameter Name="Activa" Type="Boolean" />
          <asp:Parameter Name="Codigo" Type="Int32" />
      </UpdateParameters>
    </asp:SqlDataSource>
<asp:SqlDataSource ID="sdsZonas" runat="server" 
ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
      SelectCommand="SELECT [Codigo], [Nombre], [Supervisor], [Activa] FROM [Zonas]"
      InsertCommand="INSERT INTO [Zonas] ([Nombre], [Supervisor], [Activa]) VALUES ( @Nombre, @Supervisor, @Activa)" 
      UpdateCommand="UPDATE [Zonas] SET [Nombre] = @Nombre, [Supervisor] = @Supervisor, [Activa] = @Activa WHERE [Codigo] = @Codigo" >
      <InsertParameters>
          <asp:Parameter Name="Nombre" Type="String" />
          <asp:Parameter Name="Supervisor" Type="Int32" />
          <asp:Parameter Name="Activa" Type="Boolean" />
      </InsertParameters>
      <UpdateParameters>
          <asp:Parameter Name="Nombre" Type="String" />
          <asp:Parameter Name="Supervisor" Type="Int32" />
          <asp:Parameter Name="Activa" Type="Boolean" />
          <asp:Parameter Name="Codigo" Type="Int32" />
      </UpdateParameters>
    </asp:SqlDataSource>
<asp:SqlDataSource ID="sdsSitios" runat="server" 
ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
      SelectCommand="SELECT [Codigo], [Zona], [Responsable], [Nombre], [Activo] FROM [Sitios] WHERE [Zona]=@Zona" 
      InsertCommand="INSERT INTO [Sitios] ([Zona], [Nombre], [Responsable], [Activo]) VALUES (@Zona, @Nombre, @Responsable, @Activo)" 
      UpdateCommand="UPDATE [Sitios] SET [Nombre] = @Nombre, [Responsable] = @Responsable, [Activo] = @Activo WHERE [Codigo] = @Codigo" >
      <SelectParameters>
          <asp:Parameter Name="Zona" Type="Int32" />
      </SelectParameters>
      <InsertParameters>
          <asp:Parameter Name="Zona" Type="Int32" />
          <asp:Parameter Name="Nombre" Type="String" />
          <asp:Parameter Name="Responsable" Type="Int32" />
          <asp:Parameter Name="Activo" Type="Boolean" />
      </InsertParameters>
      <UpdateParameters>
          <asp:Parameter Name="Nombre" Type="String" />
          <asp:Parameter Name="Responsable" Type="Int32" />
          <asp:Parameter Name="Activo" Type="Boolean" />
          <asp:Parameter Name="Codigo" Type="Int32" />
      </UpdateParameters>
    </asp:SqlDataSource>
<asp:SqlDataSource ID="sdsProductos" runat="server" 
ConnectionString="<%$ ConnectionStrings:CustomerCare %>"      
      SelectCommand="SELECT [Codigo], [Nombre] FROM [Productos] WHERE ([Activo] = 1)" >
    </asp:SqlDataSource>
<asp:SqlDataSource ID="sdsClientes" runat="server" 
ConnectionString="<%$ ConnectionStrings:CustomerCare %>"         
      SelectCommand="SELECT c.[Codigo], u.[Nombre] from Clientes c join Usuarios u on c.Usuario=u.Codigo Where u.Activo=1" >
    </asp:SqlDataSource>
<asp:SqlDataSource ID="sdsInternos" runat="server" 
ConnectionString="<%$ ConnectionStrings:CustomerCare %>"         
      SelectCommand="SELECT c.[Codigo], u.[Nombre] from Internos c join Usuarios u on c.Usuario=u.Codigo Where u.Activo=1" >
    </asp:SqlDataSource>

<asp:SqlDataSource ID="sdsUnidades0" runat="server" ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
      SelectCommand="SELECT * FROM [Aplicaciones]" 
      ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" >
    </asp:SqlDataSource>
<asp:SqlDataSource ID="sdsResponsables" runat="server" 
ConnectionString="<%$ ConnectionStrings:CustomerCare %>"         
      SelectCommand="SELECT c.[Codigo], u.[Nombre] from Internos c join Usuarios u on c.Usuario=u.Codigo Where u.Activo=1" >
    </asp:SqlDataSource>
    <br />
    <!-- content end -->
    </form>
</body>
</html>