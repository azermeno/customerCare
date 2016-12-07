<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisorTemporal.aspx.cs" Inherits="CustomerCare.VisorTemporal" %>

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
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
      <Scripts>
        <asp:ScriptReference Assembly="Telerik.Web.UI" 
          Name="Telerik.Web.UI.Common.Core.js">
        </asp:ScriptReference>
        <asp:ScriptReference Assembly="Telerik.Web.UI" 
          Name="Telerik.Web.UI.Common.jQuery.js">
        </asp:ScriptReference>
        <asp:ScriptReference Assembly="Telerik.Web.UI" 
          Name="Telerik.Web.UI.Common.jQueryInclude.js">
        </asp:ScriptReference>
      </Scripts>
    </telerik:RadScriptManager>
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
                  Width="165px" ImageUrl="Estilos/img_trans.gif">
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
                            CheckOnClick="True" Text="Responsable" Group="Responsable" Checked="true" ImageUrl="Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Levantó" Group="Levantó" ImageUrl="Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Escaló" Group="Escaló" ImageUrl="Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Solicitó validación" Group="Solicitó" ImageUrl="Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Validó" Group="Validó" ImageUrl="Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="Rechazó" Group="Rechazó" ImageUrl="Estilos/img_trans.gif" Value="checkbox0">
                        </telerik:RadToolBarButton>
                    </Buttons>
                </telerik:RadToolBarDropDown>
                <telerik:RadToolBarDropDown runat="server" Text="Solicitante" 
                  Width="165px" ImageUrl="Estilos/img_trans.gif">
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
  SELECT '3' + cast(codigo as varchar(50)) as Codigo, Nombre, '00' as Padre
  FROM Zonas where activa=1
  union
  SELECT '6' + cast(codigo as varchar(50)) as Codigo, Nombre, '3' + cast(zona as varchar(50)) as Padre
  FROM Sitios where activo=1
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
                  Width="137px" ImageUrl="Estilos/img_trans.gif">
                    <Buttons>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Inactivos" ImageUrl="Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Activos" ImageUrl="Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Validacion" ImageUrl="Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Cerrados" ImageUrl="Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" AllowSelfUnCheck="True" 
                            CheckOnClick="True" Text="" Group="Cancelados" ImageUrl="Estilos/img_trans.gif" Value="checkbox2">
                        </telerik:RadToolBarButton>
                    </Buttons>
                </telerik:RadToolBarDropDown>
                <telerik:RadToolBarDropDown runat="server" Text="Fecha"
                  Width="175px" ImageUrl="Estilos/img_trans.gif">
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
                        <telerik:RadToolBarButton runat="server" Text="Apertura" CheckOnClick="true" Group="donde" Checked="true" ImageUrl="Estilos/img_trans.gif" Value="checkbox3"/>
                        <telerik:RadToolBarButton runat="server" Text="Clausura" CheckOnClick="true" Group="donde" ImageUrl="Estilos/img_trans.gif" Value="checkbox3"/>
                        <telerik:RadToolBarButton runat="server" Text="Límite" CheckOnClick="true" Group="donde" ImageUrl="Estilos/img_trans.gif" Value="checkbox3"/>
                    </Buttons>
                </telerik:RadToolBarDropDown>
                <telerik:RadToolBarDropDown runat="server" Text="Tipificación" Width="150px" ImageUrl="Estilos/img_trans.gif">
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
    <telerik:RadScheduler ID="RadScheduler1" runat="server" 
      DataDescriptionField="DetalleAsunto" DataEndField="Limite" 
      DataKeyField="Codigo" DataSourceID="sdsLinea" DataStartField="Apertura" 
      DataSubjectField="Asunto" EnableDescriptionField="True" 
      SelectedView="TimelineView" ShowViewTabs="False">
    </telerik:RadScheduler>
    <asp:SqlDataSource ID="sdsLinea" runat="server" 
      ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
      ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
      SelectCommand="SELECT [Codigo], [Asunto], [Apertura], [Limite], [DetalleAsunto] FROM [Incidencias] WHERE  [Limite] is not NULL and [Apertura] is not NULL and [Apertura]<[Limite]">
    </asp:SqlDataSource>
    </form>
</body>
</html>
