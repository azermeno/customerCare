<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Incidencias.aspx.cs" Inherits="CustomerCare.Incidencias" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/Incidencias.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/Incidencias.js" type="text/javascript"></script>
</head>
<body onload="load">
    <form id="form1" runat="server">
    <div style="width: 100%">
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
            <Services>
                <asp:ServiceReference InlineScript="True" Path="Servicio.asmx" />
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
            <telerik:AjaxSetting AjaxControlID="cbxPreincidencias">
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
                <asp:HiddenField ID="hddUsuario" runat="server" />
        <input id="hddCodigo" type="hidden" />
        <div style="text-align: right; height: 27px;">
            <telerik:RadDock ID="rdcSeguimiento" Runat="server" Closed="True" 
                DefaultCommands="Close" DockMode="Floating" 
                EnableDrag="False" Left="250px" Skin="Forest" Title="Escalar Incidencia" 
                Top="100px" Width="520px" DockHandle="TitleBar">
                <ContentTemplate>
                <TABLE height="13" cellspacing="1" cellpadding="1" width="500" align="center">
    <tr>
                      <td style="HEIGHT: 23px">Observaciones:</td>
                      <td style="HEIGHT: 23px" rowspan="3">
                        <div align="center">
                          <ASP:TextBox id="txtObs" runat="server" width="424px" height="150px" textmode="MultiLine"></ASP:TextBox>
						<div align="left">  <ASP:RequiredFieldValidator id="txtObsSegui" runat="server" controltovalidate="txtObs" errormessage="Las observaciones son necesarias"></ASP:RequiredFieldValidator></div>
<div align="right"> <ASP:CheckBox id="chkEnviarMail" runat="server" textalign="Left" text="Enviar Mail (SMS)"></ASP:CheckBox></div>
                        </div></td>
	</tr> 
	<tr>
	  <td>
                        <div align="center">
                            <input id="btnGuardar" type="button" value="Guardar" onclick="guardarSeguimiento()"/>
                        </div>
		</td>
	</tr>
	<tr>
		<td>
                        <div align="center">
                         </div>
		</td>
	</tr>
                </TABLE>
                </ContentTemplate>
                </telerik:RadDock>
            <telerik:RadDock ID="rdcCerrar" Runat="server" Closed="True" 
                DefaultCommands="Close" DockMode="Floating" 
                EnableDrag="False" Left="250px" Skin="Forest" Title="Cerrar Incidencia" 
                Top="100px" Width="520px" DockHandle="TitleBar">
                <ContentTemplate>
                <TABLE height="13" cellspacing="1" cellpadding="1" width="500" align="center">
    <tr>
                      <td style="HEIGHT: 23px">Solución:</td>
                      <td style="HEIGHT: 23px">
                        <div align="center">
                          <ASP:TextBox id="TextBox2" runat="server" width="424px"></ASP:TextBox>
                          <ASP:RequiredFieldValidator id="valResumen" runat="server" controltovalidate="TextBox2" errormessage="Hace falta el resúmen"></ASP:RequiredFieldValidator>
                        </div></td>
	</tr>
	<tr>
                      <td style="HEIGHT: 23px">Detalle:</td>
                      <td style="HEIGHT: 23px" rowspan="3">
                        <div align="center">
                          <ASP:TextBox id="TextBox3" runat="server" width="424px" height="150px" textmode="MultiLine"></ASP:TextBox>
                        </div></td>
	</tr> 
	<tr>
	  <td>
                        <div align="center">
                        <input id="ButCerrar" type="button" value="Cerrar" onclick="salirCerrar()" />
                        </div>
          
		</td>
	</tr>
	<tr>
		<td>
                        <div align="center">
                        </div>
		</td>
	</tr>
                </TABLE>
                </ContentTemplate>
            </telerik:RadDock>
        <asp:CheckBox ID="cbxAbiertas" runat="server" Text="Abiertas" 
            AutoPostBack="True" Checked="True" 
                oncheckedchanged="cbxAbiertas_CheckedChanged" />
        <asp:CheckBox ID="cbxCerradas" runat="server" Text="Cerradas" 
            AutoPostBack="True" oncheckedchanged="cbxCerradas_CheckedChanged" />
        <asp:CheckBox ID="cbxPreincidencias" runat="server" Text="Preincidencias" 
            AutoPostBack="True" oncheckedchanged="cbxPreincidencias_CheckedChanged" />
        <asp:CheckBox ID="cbxCanceladas" runat="server" Text="Canceladas" 
            AutoPostBack="True" oncheckedchanged="cbxCanceladas_CheckedChanged" />
            </div>
        <telerik:RadDock ID="RadDock1" Runat="server" Width="500px" CloseText="Cerrar" 
            CollapseText="Colapsar" DockMode="Floating" ExpandText="Expandir" 
            Height="450px" Left="520px" Title="Más filtros" Top="29px" 
            BackColor="#66CCFF" Collapsed="True" DefaultCommands="ExpandCollapse" 
            EnableDrag="False">
            <ContentTemplate>
                <telerik:RadDatePicker ID="rdpAperturaDesde" Runat="server" 
                    style="margin-bottom: 0px" Culture="es-MX" Skin="WebBlue">
                    <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" 
                        ViewSelectorText="x" Skin="WebBlue">
                    </Calendar>
                    <DateInput DateFormat="dd/MM/yyyy" 
                        DisplayDateFormat="dd/MM/yyyy" EmptyMessage="Desde...">
                    </DateInput>
                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                </telerik:RadDatePicker>
                <telerik:RadDatePicker ID="rdpAperturaHasta" runat="server" Culture="es-MX" 
                    Skin="WebBlue">
                    <Calendar Skin="WebBlue" UseColumnHeadersAsSelectors="False" 
                        UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" 
                        EmptyMessage="Hasta...">
                    </DateInput>
                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                </telerik:RadDatePicker>
                <telerik:RadDatePicker ID="rdpLimiteDesde" Runat="server" Culture="es-MX" 
                    Skin="WebBlue" style="margin-bottom: 0px">
                    <Calendar Skin="WebBlue" UseColumnHeadersAsSelectors="False" 
                        UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" 
                        EmptyMessage="Desde...">
                    </DateInput>
                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                </telerik:RadDatePicker>
                <telerik:RadDatePicker ID="rdpLimiteHasta" runat="server" Culture="es-MX" 
                    Skin="WebBlue">
                    <Calendar Skin="WebBlue" UseColumnHeadersAsSelectors="False" 
                        UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" 
                        EmptyMessage="Hasta...">
                    </DateInput>
                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                </telerik:RadDatePicker>
                <telerik:RadDatePicker ID="rdpClausuraDesde" Runat="server" Culture="es-MX" 
                    Skin="WebBlue" style="margin-bottom: 0px">
                    <Calendar Skin="WebBlue" UseColumnHeadersAsSelectors="False" 
                        UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" 
                        EmptyMessage="Desde...">
                    </DateInput>
                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                </telerik:RadDatePicker>
                <telerik:RadDatePicker ID="rdpClausuraHasta" runat="server" Culture="es-MX" 
                    Skin="WebBlue">
                    <Calendar Skin="WebBlue" UseColumnHeadersAsSelectors="False" 
                        UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" 
                        EmptyMessage="Hasta...">
                    </DateInput>
                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                </telerik:RadDatePicker>
                <telerik:RadDatePicker ID="rdpCancelaDesde" Runat="server" Culture="es-MX" 
                    Skin="WebBlue" style="margin-bottom: 0px">
                    <Calendar Skin="WebBlue" UseColumnHeadersAsSelectors="False" 
                        UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" 
                        EmptyMessage="Desde...">
                    </DateInput>
                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                </telerik:RadDatePicker>
                <telerik:RadDatePicker ID="rdpCancelaHasta" runat="server" Culture="es-MX" 
                    Skin="WebBlue">
                    <Calendar Skin="WebBlue" UseColumnHeadersAsSelectors="False" 
                        UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                    </Calendar>
                    <DateInput DateFormat="dd/MM/yyyy" DisplayDateFormat="dd/MM/yyyy" 
                        EmptyMessage="Hasta...">
                    </DateInput>
                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                </telerik:RadDatePicker>
                <telerik:RadTreeView ID="RadTreeView1" Runat="server" CheckBoxes="True" 
                    CheckChildNodes="True" DataFieldID="Codigo" DataFieldParentID="Padre" 
                    DataSourceID="sdsTipificacion" DataTextField="Descripcion" 
                    DataValueField="Codigo" ForeColor="Black" Skin="WebBlue">
                </telerik:RadTreeView>
                <telerik:RadButton ID="RadButton1" runat="server" Skin="WebBlue" Text="Aplicar" 
                    onclick="RadButton1_Click">
                </telerik:RadButton>
        </ContentTemplate>
        </telerik:RadDock>
        <asp:SqlDataSource ID="sdsTipificacion" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" 
            SelectCommand="SELECT [Codigo], [Descripcion], [Padre] FROM [Tipificaciones]">
        </asp:SqlDataSource>
        <div style="height: 27px; padding-top: 4px; background-image: url('Images/fondobarra.png'); background-repeat: repeat-x; padding-left: 4px;">
        <asp:SqlDataSource ID="sdsResponsable" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="SELECT [Codigo], [Nombre] FROM [Usuarios] WHERE ([Activo] = 1) ORDER BY [Nombre]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsEscalador" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="SELECT [Codigo], [Nombre] FROM [Usuarios] WHERE ([Activo] = 1) ORDER BY [Nombre]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsSolicitante" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="select u.nombre, 'I' + cast(i.codigo as varchar) from usuarios u join internos i on u.codigo=i.usuario where activo=1
union
select nombre, '1' + cast(codigo as varchar) from unidades where activa=1
union
select nombre, '2' + cast(codigo as varchar) from grupossolucion where activo=1
union
select nombre, '3' + cast(codigo as varchar) from zonas where activa=1
union
select u.nombre, '4' + cast(c.codigo as varchar) from usuarios u join clientes c on u.codigo=c.usuario where activo=1
union
select u.nombre, '5' + cast(i.codigo as varchar) from usuarios u join internos i on u.codigo=i.usuario where activo=1">
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sdsLevantador" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" 
            SelectCommand="SELECT [Codigo], [Nombre] FROM [Usuarios] WHERE ([Activo] = 1) ORDER BY [Nombre]">
        </asp:SqlDataSource>
        <telerik:RadComboBox ID="rcbResponsable" Runat="server" AllowCustomText="True" 
            DataSourceID="sdsResponsable" DataTextField="nombre" DataValueField="codigo" 
            EmptyMessage="Responsable..." Filter="Contains" AutoPostBack="True" 
            DropDownWidth="200px" Width="120px" 
                onselectedindexchanged="rcbResponsable_SelectedIndexChanged">
        </telerik:RadComboBox>
        <telerik:RadComboBox ID="rcbEscalador" Runat="server" AllowCustomText="True" 
            DataSourceID="sdsEscalador" DataTextField="nombre" DataValueField="codigo" 
            EmptyMessage="Escalador..." Filter="Contains" AutoPostBack="True" 
            DropDownWidth="200px" Width="120px" 
                onselectedindexchanged="rcbEscalador_SelectedIndexChanged">
        </telerik:RadComboBox>
        <telerik:RadComboBox ID="rcbSolicitante" Runat="server" AllowCustomText="True" 
            DataSourceID="sdsSolicitante" DataTextField="nombre" DataValueField="Column1" 
            EmptyMessage="Solicitante..." Filter="Contains" AutoPostBack="True" 
            DropDownWidth="200px" Width="120px" 
                onselectedindexchanged="rcbSolicitante_SelectedIndexChanged">
        </telerik:RadComboBox>
        <telerik:RadComboBox ID="rcbLevantador" Runat="server" AllowCustomText="True" 
            DataSourceID="sdsLevantador" DataTextField="Nombre" DataValueField="Codigo" 
            EmptyMessage="Levantador..." Filter="Contains" AutoPostBack="True" 
            DropDownWidth="200px" Width="120px" 
                onselectedindexchanged="rcbLevantador_SelectedIndexChanged">
        </telerik:RadComboBox>
        </div>
        <telerik:GridHyperLinkColumn DataNavigateUrlFields="Codigo" 
            DataNavigateUrlFormatString="javascript:cerrar({0})" 
            Text="&lt;image src=&quot;images/seguimiento2.png&quot; alt=&quot;Cerrar&quot;&gt;" 
            UniqueName="column">
        </telerik:GridHyperLinkColumn>
        <asp:SqlDataSource ID="sdsIncidencias" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" 
            SelectCommand="SELECT top 200
	i.Codigo
    ,case estado
        when 2 then '&lt;a href=&quot;javascript:cerrar(' + Cast(i.Codigo as varchar) + ')&quot;&gt;&lt;image src=&quot;images/seguimiento2.png&quot; alt=&quot;Cerrar&quot;/&gt;&lt;/a&gt;' +
                    '&lt;a href=&quot;javascript:seguimiento(' + Cast(i.Codigo as varchar) + ')&quot;&gt;&lt;image src=&quot;images/seguimiento.png&quot; alt=&quot;Seguimiento&quot;/&gt;&lt;/a&gt;'
        else ''
        end
        as Controles
	,i.Responsable as res
	,i.Levanto as lev
	,dbo.Escaladores(i.Codigo) as esc
	--,case 
    		--WHEN sum (cast((e.TipoEvento=1) as smallint))&gt;1 THEN e.datos
    		--ELSE '0'
    		--END as esc
	, Cast(i.TipoSolicitante as varchar) + cast(i.Solicitante as varchar) as sol
	, i.Estado
	, i.Tipificacion
       	, i.Asunto 
     	--, t.Descripcion AS Tipo 
	, CONVERT(char(10),i.Apertura,103) + ' ' + CONVERT(char(5),i.Apertura,114) as Apertura 
	, CONVERT(char(10),i.Limite,103) + ' ' + CONVERT(char(5),i.Limite,114) as Límite 
	, CONVERT(char(10),i.Clausura,103) + ' ' + CONVERT(char(5),i.Clausura,114) as Clausura 
	, u.login as Responsable 
	, CASE i.TipoSolicitante 
		WHEN 1 THEN un.Nombre 
		WHEN 2 THEN gs.Nombre 
		WHEN 3 THEN zo.Nombre 
		WHEN 4 THEN u3.Nombre 
		WHEN 5 THEN u4.Nombre 
	END AS Solicitante
	--, u2.nombre as Levantó 
	--, Solucion as Solución 
FROM 	Incidencias i 
	INNER JOIN Usuarios u on i.responsable=u.codigo 
	--INNER JOIN Usuarios u2 on i.levanto=u2.codigo 
	--INNER JOIN Tipificaciones t on t.codigo=i.Tipificacion 
	LEFT JOIN Unidades un on un.codigo=i.solicitante 
	LEFT JOIN Internos it on it.codigo=i.solicitante 
	LEFT JOIN GruposSolucion gs on gs.codigo=i.solicitante 
	LEFT JOIN Zonas zo on zo.codigo=i.solicitante 
	LEFT JOIN Clientes cl on cl.codigo=i.solicitante 
	LEFT JOIN Usuarios u3 on cl.usuario=u3.codigo 
	LEFT JOIN Usuarios u4 on it.usuario=u4.codigo 
	--LEFT JOIN Eventos e on e.incidencia=i.codigo 
ORDER BY i.Codigo desc">
        </asp:SqlDataSource>
    
        <telerik:RadGrid ID="RadGrid1" 
            OnPreRender="RadGrid1_PreRender" runat="server" AllowPaging="True" 
            AllowSorting="True" Culture="es-MX" DataSourceID="sdsIncidencias" 
            GridLines="None" AutoGenerateColumns="False" OnItemCreated="RadGrid1_ItemCreated"
            OnItemCommand="RadGrid1_ItemCommand" AllowFilteringByColumn="True" 
            EnableLinqExpressions="False">
            <SortingSettings SortToolTip="Click aquí para ordenar." />
            <%--<ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                <Selecting AllowRowSelect="True" />
            </ClientSettings>--%>
            <ClientSettings AllowKeyboardNavigation="True">
                <DataBinding Location="Servicio.asmx" SelectCountMethod="Contar" 
                    SelectMethod="Incidencias">
                </DataBinding>
            </ClientSettings>
<MasterTableView DataSourceID="sdsIncidencias" DataKeyNames="Codigo" 
                NoMasterRecordsText="No hay tickets con estos filtros." 
                IsFilterItemExpanded="False">
    <NestedViewTemplate>
        <asp:Panel runat="server" ID="InnerContainer" CssClass="viewWrap" Visible="false">
        <asp:Label ID="Label1" Font-Bold="true" Font-Italic="true" Text='<%# Eval("Codigo") %>'
                                    Visible="false" runat="server" />
        <asp:SqlDataSource
            ID="sdsLocal" runat="server"
            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" 
            SelectCommand="
            SELECT i.Asunto, i.DetalleAsunto, i.Solucion, i.DetalleSolucion, dbo.Tipificacion(i.Tipificacion) as Tipo, u.Nombre FROM Incidencias i join Usuarios u ON i.Levanto=u.Codigo WHERE i.Codigo=?
            ">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="Label1" PropertyName="Text" Type="String" Name="Codigo" />
                                    </SelectParameters></asp:SqlDataSource>
            <asp:FormView ID="FormView1" runat="server" DataSourceID="sdsLocal">
            <ItemTemplate>
            <table cellspacing="0" cellpadding="1" width="100%" align="center" bgcolor="#fffacd" border="1">
	<tr>
                    <td valign="top"><font face="Arial" size="1"><strong>&nbsp;&nbsp; Asunto:</strong></font></td>
                    <td valign="top">
                      <table cellspacing="0" cellpadding="0" width="100%" border="0">
			  <tr>
				<td>
                              <ASP:Label id="lblAsunto" runat="server" forecolor="Blue" font-names="Arial"><%# Eval("Asunto") %></ASP:Label></td>
			  </tr>
			  <tr>
				<td>
                              <ASP:Label id="lblDetalle" runat="server" forecolor="MidnightBlue" font-names="Arial" font-size="Smaller"><%# Eval("DetalleAsunto") %></ASP:Label></td>
			  </tr>
                      </table></td>
                    <td valign="top" width="60"><font face="arial" size="1"><strong>&nbsp;&nbsp; Solucion:</strong></font></td>
                    <td valign="top">
                      <table cellspacing="0" cellpadding="0" width="100%" border="0">
			  <tr>
				<td>
                              <ASP:Label id="lblSolucion" runat="server" forecolor="Blue" font-names="Arial"><%# Eval("Solucion") %></ASP:Label></td>
              </tr>
              <tr>
                <td>
                              <ASP:Label id="lblSolDet" runat="server" forecolor="MidnightBlue" font-names="Arial" font-size="Smaller"><%# Eval("DetalleSolucion") %></ASP:Label></td>
              </tr>
                      </table></td>
	</tr>
	<tr>
                    <td width="60"><font face="Arial" size="1"><strong>&nbsp;&nbsp; Tipificación:</strong></font></td>
					<td>
					  <ASP:Label id="lblTipificacion" runat="server" font-names="Arial" font-size="Smaller"><%# Eval("Tipo") %></ASP:Label></td>
					<td>
                      <p align="center"><font face="Arial"><strong><font size="1">Levantó:</font>
                          </strong>
                        </font>
                      </p></td>
					<td>
					  <ASP:Label id="lblLevanto" runat="server" font-names="Arial" font-size="Smaller"><%# Eval("Nombre") %></ASP:Label></td>
	</tr>
              </table>
            </ItemTemplate>
            </asp:FormView> 
        </asp:Panel>
        
    </NestedViewTemplate>
<%--<CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>

<RowIndicatorColumn>
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>--%>

<CommandItemSettings ExportToPdfText="Export to Pdf"></CommandItemSettings>

<ExpandCollapseColumn Visible="True"></ExpandCollapseColumn>

    <Columns>
        <telerik:GridBoundColumn DataField="Codigo" DataType="System.Int32" 
            HeaderText="Codigo" ReadOnly="True" SortExpression="Codigo" UniqueName="Codigo">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn ReadOnly="true" DataField="Controles" UniqueName="Controles">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="res" HeaderText="res" 
            SortExpression="res" UniqueName="res" DataType="System.Int32" 
            Visible="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="lev" HeaderText="lev" 
            SortExpression="lev" UniqueName="lev" DataType="System.Int32" 
            Visible="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="esc" HeaderText="esc" 
            ReadOnly="True" SortExpression="esc" UniqueName="esc" Visible="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="sol" HeaderText="sol" ReadOnly="True" 
            SortExpression="sol" UniqueName="sol" Visible="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Estado" HeaderText="Estado" 
            SortExpression="Estado" UniqueName="Estado" DataType="System.Int32" 
            Visible="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Tipificacion" HeaderText="Tipificacion" 
            SortExpression="Tipificacion" UniqueName="Tipificacion" 
            DataType="System.Int32" Visible="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Asunto" HeaderText="Asunto" 
            SortExpression="Asunto" UniqueName="Asunto">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Apertura" HeaderText="Apertura" 
            SortExpression="Apertura" UniqueName="Apertura" ReadOnly="True">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Límite" HeaderText="Límite" ReadOnly="True" 
            SortExpression="Límite" UniqueName="Límite">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Clausura" HeaderText="Clausura" 
            ReadOnly="True" SortExpression="Clausura" UniqueName="Clausura" Visible="False">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Responsable" HeaderText="Responsable" 
            SortExpression="Responsable" UniqueName="Responsable">
        </telerik:GridBoundColumn>
        <telerik:GridBoundColumn DataField="Solicitante" HeaderText="Solicitante" 
            ReadOnly="True" SortExpression="Solicitante" UniqueName="Solicitante">
        </telerik:GridBoundColumn>
    </Columns>
</MasterTableView>

            <PagerStyle FirstPageToolTip="Primera página" LastPageToolTip="Última página" 
                NextPagesToolTip="Páginas siguientes" NextPageToolTip="Página siguiente" 
                PageSizeLabelText="Tamaño de página:" PrevPagesToolTip="Páginas anteriores" 
                PrevPageToolTip="Página anterior" />

<%--<HeaderContextMenu EnableImageSprites="True" CssClass="GridContextMenu GridContextMenu_Default"></HeaderContextMenu>--%>

<HeaderContextMenu EnableImageSprites="True" CssClass="GridContextMenu GridContextMenu_Default"></HeaderContextMenu>
        </telerik:RadGrid>
    
    </div>
    </form>
</body>
</html>
