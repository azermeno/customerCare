<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TablaTickets.aspx.cs" Inherits="CustomerCare.TablaTickets" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<style>
.rtbIcon
{
    background:url(<%=ImagesFolder%>led.png);
    background-position: 12px 0px;
}
.RadToolBarDropDown .rtbWrap .rtbIcon
{
    background:url(<%=ImagesFolder%>checkbox.png);
    background-position: 13px 0px;
}
</style>
    <form id="form1" runat="server" style="height:100%;">
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
            <Services>
                <asp:ServiceReference InlineScript="True" Path="/Servicio.asmx" />
            </Services>
        </telerik:RadScriptManager>
    <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cbxAbiertas">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cbxCerradas">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cbxPretickets">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cbxCanceladas">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbResponsable">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbResponsable" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbEscalador">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbEscalador" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbSolicitante">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbResponsable" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbLevantador">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbLevantador" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" ZIndex="1">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadToolBar ID="rtbTipificar" runat="server" 
            onclientbuttonclicked="GuardarTipi">
        <Items>
            <telerik:RadToolBarButton runat="server" Enabled="False" PostBack="False" 
                Text="Tipificación:">
            </telerik:RadToolBarButton>
            <telerik:RadToolBarButton runat="server" Text="tipificacion">
                <ItemTemplate>
                    <telerik:RadTextBox ID="RadTextBox1" Runat="server" EmptyMessage="Click aquí." 
                        Width="300px">
                    </telerik:RadTextBox>
                </ItemTemplate>
            </telerik:RadToolBarButton>
            <telerik:RadToolBarButton runat="server" PostBack="False" Text="Guardar">
            </telerik:RadToolBarButton>
        </Items>
    </telerik:RadToolBar>
                <asp:HiddenField ID="hddFiltroDefault" runat="server" />
                <asp:HiddenField ID="hddInterno" runat="server" />
                <asp:HiddenField id="hddCodigo" runat="server" />
        <telerik:RadDock ID="rdcAcciones" Runat="server" Closed="True" 
                DefaultCommands="Close" DockMode="Floating" 
                EnableDrag="False" Left="250px" Title="" 
                Top="100px" Width="520px" DockHandle="TitleBar">
                <ContentTemplate>
                </ContentTemplate>
            </telerik:RadDock>
        <telerik:RadToolBar ID="RadToolBar1" runat="server" Width="100%" OnClientButtonClicked="clicktoolbar" OnClientDropDownClosing="cerrando" OnClientMouseOver="onMouseOver" OnClientMouseOut="onMouseOut" OnClientDropDownClosed="cerrado">
            <Items>
                <telerik:RadToolBarDropDown runat="server" Text="Usuario" 
                  Width="165px" ImageUrl="/Estilos/img_trans.gif">
                    <Buttons>
                        <telerik:RadToolBarButton runat="server" Text="Usuario">
                            <ItemTemplate>
                                <telerik:RadComboBox ID="rcbUsuario" Runat="server" AllowCustomText="True" 
                                    DataSourceID="sdsUsuario" DataTextField="nombre" DataValueField="codigo" 
                                    DropDownWidth="200px" Filter="Contains" 
                                    Width="141px" OnClientDropDownOpened="noCierresNada" OnClientDropDownClosed="puedesCerrar" OnClientSelectedIndexChanged="function(s,a){cambioFiltro(0);}">
                                </telerik:RadComboBox>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Responsable" Group="Responsable" Checked="true" ImageUrl="/Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Levantó" Group="Levantó" ImageUrl="/Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Escaló" Group="Escaló" ImageUrl="/Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Solicitó validación" Group="Solicitó" ImageUrl="/Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Validó" Group="Validó" ImageUrl="/Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Rechazó" Group="Rechazó" ImageUrl="/Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                    </Buttons>
                </telerik:RadToolBarDropDown>
                <telerik:RadToolBarDropDown runat="server" Text="Solicitante" 
                  Width="165px" ImageUrl="/Estilos/img_trans.gif">
                    <Buttons>
                <telerik:RadToolBarButton runat="server" Text="Solicitante">
                            <ItemTemplate>
                            <telerik:RadTextBox ID="rtbSolicitante" runat="server">
                            <ClientEvents OnBlur="paraFiltroSolicitante" OnFocus="iniciaFiltroSolicitante" OnKeyPress="checkSolicitante" />
                            </telerik:RadTextBox>
                                <telerik:RadTreeView ID="rtvSolicitante" runat="server" DataSourceID="sdsSoli" DataFieldID="Codigo" 
                                OnClientNodeChecked="solCheck" DataFieldParentID="Padre" DataTextField="Nombre" 
                                DataValueField="Codigo" CheckBoxes="true" CheckChildNodes="true" MultipleSelect="true">
                                </telerik:RadTreeView>
                              <asp:SqlDataSource ID="sdsSoli" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
            SelectCommand="SELECT * FROM (SELECT '00' as Codigo, 'Unidades' as Nombre, null as Padre
                          union
                                    SELECT '01' as Codigo, 'Clientes' as Nombre, null as Padre
                          union
                          SELECT '3' + cast(z.codigo as varchar(50)) as Codigo, z.Nombre + ' - ' + ISNULL(us2.Nombre,''), '00' as Padre
                          FROM Zonas z left join 
                          dbo.Internos AS i ON z.Supervisor = i.Codigo INNER JOIN
						  dbo.Usuarios AS us2 ON i.Usuario = us2.Codigo where activa=1
                          union
                          SELECT '6' + cast(s.codigo as varchar(50)) as Codigo,RTRIM( s.Nombre) + ' - ' + ISNULL(us2.Nombre,''), '3' + cast(zona as varchar(50)) as Padre
                          FROM Sitios s left join 
                          dbo.Internos AS i ON s.Responsable = i.Codigo INNER JOIN
						  dbo.Usuarios AS us2 ON i.Usuario = us2.Codigo
                          where s.activo=1
                          union
                          SELECT '1' + cast(codigo as varchar(50)) as Codigo, Nombre, '6' + cast(sitio as varchar(50)) as Padre
                          FROM Unidades where activa=1
                          union
                          SELECT '4' + cast(codigo as varchar(50)) as Codigo, Nombre, '01' as Padre
                          FROM Usuarios where codigo in (select usuario from clientes) and activo = 1) t ORDER BY Nombre "></asp:SqlDataSource>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                    </Buttons>
                </telerik:RadToolBarDropDown>
                <telerik:RadToolBarDropDown runat="server" Text="Estados"
                  Width="137px" ImageUrl="/Estilos/img_trans.gif">
                    <Buttons>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Inactivos" ImageUrl="/Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Activos" ImageUrl="/Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Validacion" ImageUrl="/Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Cerrados" ImageUrl="/Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Cancelados" ImageUrl="/Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                    </Buttons>
                </telerik:RadToolBarDropDown>
                
              

                <telerik:RadToolBarDropDown runat="server" Text="Fecha"
                  Width="175px" ImageUrl="/Estilos/img_trans.gif">
                    <Buttons>
                        <telerik:RadToolBarButton runat="server" Text = "Desde:"/>
                        <telerik:RadToolBarButton runat="server" Text="Desde">
                            <ItemTemplate>
                                <telerik:RadDateTimePicker ID="rdtpDesde" runat="server" Culture="es-MX" ClientEvents-OnDateSelected="function(s,a){cambioFiltro(3);}" ClientEvents-OnPopupOpening="noCierresNada" ClientEvents-OnPopupClosing="puedesCerrar"/>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Text = "Hasta:"/>
                        <telerik:RadToolBarButton runat="server" Text="Hasta">
                            <ItemTemplate>
                                <telerik:RadDateTimePicker ID="rdtpHasta" runat="server"  Culture="es-MX" ClientEvents-OnDateSelected="function(s,a){cambioFiltro(3);}" ClientEvents-OnPopupOpening="noCierresNada" ClientEvents-OnPopupClosing="puedesCerrar"/>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Text="Apertura" CheckOnClick="true" Group="donde" Checked="true" ImageUrl="/Estilos/img_trans.gif" Value="checkbox3"/>
                        <telerik:RadToolBarButton runat="server" Text="Clausura" CheckOnClick="true" Group="donde" ImageUrl="/Estilos/img_trans.gif" Value="checkbox3"/>
                        <telerik:RadToolBarButton runat="server" Text="Límite" CheckOnClick="true" Group="donde" ImageUrl="/Estilos/img_trans.gif" Value="checkbox3"/>
                    </Buttons>
                </telerik:RadToolBarDropDown>
                <telerik:RadToolBarDropDown runat="server" Text="Tipificación" Width="150px" ImageUrl="/Estilos/img_trans.gif">
                    <Buttons>
                        <telerik:RadToolBarButton runat="server" Text="Tipificación">
                            <ItemTemplate>
                                <telerik:RadTreeView ID="RadTreeView1" runat="server" DataSourceID="sdsTipi" DataFieldID="Codigo" OnClientNodeChecked="function(s,a){cambioFiltro(1);}" DataFieldParentID="Padre" DataTextField="Descripcion" DataValueField="Codigo" CheckBoxes="true" CheckChildNodes="true">
                                </telerik:RadTreeView>
                              <asp:SqlDataSource ID="sdsTipi" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
            SelectCommand="SELECT [Codigo], [Padre], [Descripcion] FROM [Tipificaciones] where activo=1"></asp:SqlDataSource>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                    </Buttons>
                </telerik:RadToolBarDropDown>

                <%--Aquí es donde ponemos la prioridad--%>
                <telerik:RadToolBarDropDown runat="server" Text="Prioridad" Width="137px" ImageUrl="/Estilos/img_trans.gif">
                    <Buttons>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Crítico" Group="Crítico" ImageUrl="/Estilos/img_trans.gif" Value="checkbox5">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Alta" Group="Alta" ImageUrl="/Estilos/img_trans.gif" Value="checkbox5">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Media" Group="Media" ImageUrl="/Estilos/img_trans.gif" Value="checkbox5">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Baja" Group="Baja" ImageUrl="/Estilos/img_trans.gif" Value="checkbox5">
                        </telerik:RadToolBarButton>
                    </Buttons>
                </telerik:RadToolBarDropDown>
                <%--Aquí TERMINA donde ponemos la prioridad--%>

                <telerik:RadToolBarButton runat="server" Value="guardarFiltro" ToolTip="Guardar como mi filtro default"/>
                <telerik:RadToolBarButton runat="server" Text="Buscar" Width="100%" 
                  Value="buscar">
                    <ItemTemplate>
                        <telerik:RadTextBox ID="rtbBuscar" runat="server" 
                            ClientEvents-OnKeyPress="buscar" EmptyMessage="Buscar..."
                            Width="180px">
                        </telerik:RadTextBox>
                    </ItemTemplate>
                </telerik:RadToolBarButton>

                <%--Agregado por Andrés LAra Maldonado para Exportar a EXcel toda la información de las incidencias en pantalla--%>
                <telerik:RadToolBarButton runat="server" Value="Exportar"  ImageUrl="/Estilos/Office2007/Images/30x30/Imprimir.png" ToolTip="Exporta la información a Excel" style="width: 18px; height: 18px; background: none;" />
                

            </Items>
        </telerik:RadToolBar>
        <asp:SqlDataSource ID="sdsUsuario" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="SELECT [Codigo], [Nombre] FROM [Usuarios] WHERE ([Activo] = 1) ORDER BY [Nombre]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsSolicitante" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="select * from (
select nombre, '1' + cast(codigo as varchar) as codigo from unidades where activa=1
union
select nombre, '3' + cast(codigo as varchar) as codigo from zonas where activa=1
union
select nombre, '6' + cast(codigo as varchar) as codigo from sitios where activo=1
union
select nombre, '4' + cast(codigo as varchar) as codigo from usuarios where codigo in (select usuario from clientes) and activo = 1) sols
order by nombre">
        </asp:SqlDataSource>
        <telerik:RadGrid ID="RadGrid1" 
            runat="server" AllowPaging="True" 
            AllowSorting="True" Culture="es-MX"
            GridLines="None" AutoGenerateColumns="False" 
            PageSize="50"  CellSpacing="0" AllowMultiRowSelection="true">
            <SortingSettings SortToolTip="Click aquí para ordenar." />
            <ClientSettings AllowColumnHide="True" AllowKeyboardNavigation="True" 
                EnableAlternatingItems="False">
       <Selecting AllowRowSelect="True" EnableDragToSelectRows="False" />
       <ClientEvents  OnCommand="RadGrid1_Command" 
                    OnRowSelected="RowSelected" OnActiveRowChanged="RowSelected" 
                    OnRowDataBound="RadGrid1_RowDataBound" OnRowDblClick="RowDblclick"/>

                <Scrolling AllowScroll="True" 
                    UseStaticHeaders="True" />
                <Resizing AllowColumnResize="True" AllowResizeToFit="True" 
                    ClipCellContentOnResize="False" />
       </ClientSettings>
            <AlternatingItemStyle BackColor="White" BorderColor="White" BorderStyle="Solid" 
                BorderWidth="1px" Wrap="False" />
<MasterTableView NoMasterRecordsText="No hay tickets con estos filtros." PageSize="20">
<CommandItemSettings ExportToPdfText="Exportar a Pdf"></CommandItemSettings>

<RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

<ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="Codigo" DataType="System.Int32" 
            HeaderText="Codigo" SortExpression="CodigoCompuesto" ReadOnly="True" UniqueName="codigo" ItemStyle-Width="30">
            <HeaderStyle Width="85px" />

<ItemStyle Width="30px"></ItemStyle>
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Iconos" DataType="System.String" HeaderText="" ReadOnly="true" UniqueName="Iconos">
            <HeaderStyle Width="40px" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Asunto" HeaderText="Asunto" 
            SortExpression="Asunto" UniqueName="Asunto">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Apertura" HeaderText="Apertura" 
            SortExpression="Apertura" UniqueName="Apertura" ReadOnly="True" ItemStyle-Wrap="False" ItemStyle-Width="92px">
            <HeaderStyle Width="92px" />
<ItemStyle Wrap="False" Width="92px"></ItemStyle>
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Limcla" HeaderText="Límite/Clausura" ReadOnly="True" 
            SortExpression="Limcla" UniqueName="Limcla">
            <HeaderStyle Width="92px" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Clausura" HeaderText="Clausura" 
            ReadOnly="True" SortExpression="Clausura" UniqueName="Clausura" Visible="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Responsable" HeaderText="Responsable" 
            SortExpression="ResNombre" UniqueName="Responsable">
            <HeaderStyle Width="92px" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Solicitante" HeaderText="Dependencia" 
            ReadOnly="True" SortExpression="SolNombre" UniqueName="Solicitante">
            <HeaderStyle Width="200px" />
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Estado" HeaderText="Estado" 
            ReadOnly="True" SortExpression="Estado" UniqueName="Estado" Visible="false">
            <HeaderStyle Width="92px"/>
        </telerik:GridBoundColumn>

        <telerik:GridBoundColumn DataField="Prioridad" HeaderText="Prioridad" 
            ReadOnly="True" SortExpression="rSeveridad" UniqueName="Prioridad" >
            <HeaderStyle Width="92px"/>
        </telerik:GridBoundColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
    <PagerStyle AlwaysVisible="True" FirstPageToolTip="Primera página." 
                LastPageToolTip="Última página." NextPagesToolTip="Páginas siguientes." 
                NextPageToolTip="Página siguiente." PageSizeLabelText="Tamaño de página:" 
                PrevPagesToolTip="Páginas anteriores." 
        PrevPageToolTip="Página anterior." 
        PagerTextFormat="Cambiar página: {4} &amp;nbsp;Página &lt;strong&gt;{0}&lt;/strong&gt; de &lt;strong&gt;{1}&lt;/strong&gt;, &lt;strong&gt;{5}&lt;/strong&gt; tickets en total." />
</MasterTableView>

            <ActiveItemStyle BorderStyle="Outset" BorderWidth="2px" Font-Bold="True" 
                Wrap="False" />
            <ItemStyle BorderWidth="1px" 
                Wrap="False" />
            <SelectedItemStyle BorderStyle="Outset" BorderWidth="2px" Font-Bold="True" 
                ForeColor="Black" />

<FilterMenu EnableImageSprites="False"></FilterMenu>

<HeaderContextMenu EnableImageSprites="True" CssClass="GridContextMenu GridContextMenu_Default"></HeaderContextMenu>
        </telerik:RadGrid>
    </form>
</body>
</html>
