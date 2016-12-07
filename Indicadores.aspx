<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Indicadores.aspx.cs" Inherits="CustomerCare.Indicadores" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" 
    namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Charting" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
<head id="Head1" runat="server">
    <title></title>
<link rel="stylesheet" type="text/css" href="<%=ConfigurationManager.AppSettings["Telerik.Web.UI.StyleSheetFolders"] %>/base.css" />
<link href="Styles/Indicadores.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <script src="Scripts/Indicadores.js?<%=DateTime.Now.Millisecond.ToString()%>" type="text/javascript"></script>
    
                    <telerik:RadScriptManager ID="rsmIndicadores" Runat="server">
                    </telerik:RadScriptManager>
                        <telerik:RadAjaxManager ID="ramIndicadores" runat="server" 
                        DefaultLoadingPanelID="alpPrincipal">
                            <AjaxSettings>
                                <telerik:AjaxSetting AjaxControlID="rtsIndicadores">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="rtsIndicadores" LoadingPanelID="none" />
                                        <telerik:AjaxUpdatedControl ControlID="rmpIndicadores" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="rcbMes">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="rgUnidades" />
                                        <telerik:AjaxUpdatedControl ControlID="RadChart2" />
                                        <telerik:AjaxUpdatedControl ControlID="lblNSC" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="rcbProducto">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="rgUnidades" />
                                        <telerik:AjaxUpdatedControl ControlID="RadChart2" />
                                        <telerik:AjaxUpdatedControl ControlID="lblNSC" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="rcbZona">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="rgUnidades" />
                                        <telerik:AjaxUpdatedControl ControlID="RadChart2" />
                                        <telerik:AjaxUpdatedControl ControlID="lblNSC" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="rcbTecnico">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="rgUnidades" />
                                        <telerik:AjaxUpdatedControl ControlID="RadChart2" />
                                        <telerik:AjaxUpdatedControl ControlID="lblNSC" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="rcbCliente">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="rgUnidades" />
                                        <telerik:AjaxUpdatedControl ControlID="RadChart2" />
                                        <telerik:AjaxUpdatedControl ControlID="lblNSC" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                                <telerik:AjaxSetting AjaxControlID="rgUnidades">
                                    <UpdatedControls>
                                        <telerik:AjaxUpdatedControl ControlID="rgUnidades" />
                                    </UpdatedControls>
                                </telerik:AjaxSetting>
                            </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="alpPrincipal" Runat="server">
                    </telerik:RadAjaxLoadingPanel>
                    <telerik:RadAjaxLoadingPanel ID="none" Runat="server" IsSticky="True">
                    </telerik:RadAjaxLoadingPanel>
                    <telerik:RadTabStrip ID="rtsIndicadores" runat="server" SelectedIndex="0" 
                        Align="Right" MultiPageID="rmpIndicadores"  
                        OnClientTabSelecting="onTabSelecting" AutoPostBack="True">
                        <Tabs>
                            <telerik:RadTab runat="server" Text="Unidades" Selected="True">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Técnicos">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Zonas">
                            </telerik:RadTab>
                            <telerik:RadTab runat="server" Text="Tipos">
                            </telerik:RadTab>
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage ID="rmpIndicadores" Runat="server" Width="100%" 
                        Height="500px" SelectedIndex="0" RenderSelectedPageOnly="True">
                        <telerik:RadPageView ID="RadPageView1" runat="server" Height="500px" 
                            Width="100%">
                            <div 
                                style="height: 42px; vertical-align:middle; background-image: url('Images/sprite.gif'); background-position:0 -7550; background-repeat: repeat-x;">
                               <span style="margin:10px 1px 0px 5px; position:relative;"> Mes:</span>
                                <telerik:RadComboBox ID="rcbMes" Runat="server" AutoPostBack="True" 
                                    CssClass="comboFiltro" DataSourceID="sdsMes" DataTextField="Column1" 
                                    DataValueField="Mes" LoadingMessage="Cargando..." Width="100px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="sdsMes" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                    ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="select distinct STUFF(convert(nvarchar,convert(datetime, Mes + '01', 112),107), 5, 4, ''), Mes
from unidadesmensual
order by Mes desc"></asp:SqlDataSource>
                                <telerik:RadComboBox ID="rcbProducto" Runat="server" AllowCustomText="True" 
                                    AutoPostBack="True" DataSourceID="sdsProducto" DataTextField="Nombre" 
                                    DataValueField="Codigo" CssClass="comboFiltro" 
                                    EmptyMessage="Producto..." Filter="Contains" MarkFirstMatch="True" 
                                    onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible()) sender.showDropDown(); })" 
                                    OnItemDataBound="rcbProducto_ItemDataBound" Width="180px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="sdsProducto" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                    ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                                    SelectCommand="SELECT [Nombre], [Codigo] FROM [Productos] ORDER BY [Nombre]">
                                </asp:SqlDataSource>
                                <telerik:RadComboBox ID="rcbZona" Runat="server" AllowCustomText="True" 
                                    AutoPostBack="True" CssClass="comboFiltro" DataSourceID="sdsZona" 
                                    DataTextField="Nombre" DataValueField="Codigo" DropDownCssClass="dropdown" 
                                    EmptyMessage="Zona..." Filter="Contains" MarkFirstMatch="True" 
                                    onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible()) sender.showDropDown(); })" 
                                    Width="180px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="sdsZona" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                    ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                                    SelectCommand="SELECT [Nombre], [Codigo] FROM [Zonas] ORDER BY [Nombre]">
                                </asp:SqlDataSource>
                                <telerik:RadComboBox ID="rcbTecnico" Runat="server" AllowCustomText="True" 
                                    AutoPostBack="True" CssClass="comboFiltro" DataSourceID="sdsTecnico" 
                                    DataTextField="Nombre" DataValueField="Codigo" EmptyMessage="Técnico..." 
                                    Filter="Contains" MarkFirstMatch="True" 
                                    onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible()) sender.showDropDown(); })" 
                                    Width="180px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="sdsTecnico" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                    ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="SELECT distinct us.Nombre, i.Codigo
from unidades u join sitios s on u.sitio=s.codigo join internos i on s.responsable=i.codigo join usuarios us on i.usuario=us.codigo
order by us.nombre"></asp:SqlDataSource>
                                <telerik:RadComboBox ID="rcbCliente" Runat="server" AllowCustomText="True" 
                                    AutoPostBack="True" CssClass="comboFiltro" DataSourceID="sdsClientes" 
                                    DataTextField="nombre" DataValueField="codigo" EmptyMessage="Cliente..." 
                                    Filter="Contains" MarkFirstMatch="True" 
                                    onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible()) sender.showDropDown(); })" 
                                    Width="180px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="sdsClientes" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                    ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="
select u.Nombre, c.Codigo
from usuarios u join clientes c on u.codigo=c.usuario
order by u.nombre"></asp:SqlDataSource>
                            </div>
                            <telerik:RadSplitter ID="rsUnidades" Runat="server" Height="500px" 
                                LiveResize="True" Width="100%">
                                <telerik:RadPane ID="RadPane2" Runat="server" Scrolling="None">
                                    <div style="float: left; width: 100%; height: 500px; position: relative;">
                                        <asp:SqlDataSource ID="sdsResultados" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="DECLARE @Dia datetime, @Mes char(6), @Tecnico int, @Zona int, @Producto int, @Cliente int ;
DECLARE @cambiosdia TABLE (Unico int, Columna int, Anterior varchar(256), Fecha datetime);
SET @Mes=?
SET @Dia = CAST(@Mes + '16' as datetime);
SET @Producto=?
SET @Zona=?
SET @Tecnico=?
SET @Cliente=?
SELECT us.Alias as Unidad, ISC, Tendencia, Respuesta, Iniciales, Recibidas, Cerradas, Faltantes, DentroSLA
FROM UnidadesMensual um INNER JOIN
       		(
			SELECT t.Codigo, t.Alias, ISNULL(c.Anterior, t.Sitio) AS Sitio
        		FROM Unidades t LEFT OUTER JOIN @cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 2) = 2)
		) us ON um.Unidad = us.Codigo INNER JOIN
		(
			SELECT t.Codigo, ISNULL(c.Anterior, t.Producto) AS Producto
        		FROM Unidades t LEFT OUTER JOIN
        			@cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 5) = 5)
		) up ON um.Unidad = up.Codigo INNER JOIN
        	(
			SELECT t.Codigo, ISNULL(c.Anterior, t.Cliente) AS Cliente
	                FROM Unidades t LEFT OUTER JOIN
        	        	@cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 6) = 6)
		) uc ON um.Unidad = uc.Codigo INNER JOIN
	       	(
			SELECT t.Codigo, ISNULL(c.Anterior, t.Zona) AS Zona
	                FROM Sitios t LEFT OUTER JOIN
	                  	@cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 2) = 2)
		) sz ON us.Sitio = sz.Codigo INNER JOIN
	       	(
			SELECT t.Codigo, ISNULL(c.Anterior, t.Responsable) AS Responsable
	                FROM Sitios t LEFT OUTER JOIN
	                  	@cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 3) = 3)
		) sr ON us.Sitio = sr.Codigo
	WHERE	um.Mes = @Mes
		AND (@Producto = 0 OR up.Producto = @Producto) 
		AND (@Cliente = 0 OR uc.Cliente = @Cliente) 
		AND (@Zona = 0 OR sz.Zona = @Zona) 
		AND (@Tecnico = 0 OR sr.Responsable = @Tecnico) 
ORDER BY case when Respuesta like '%intentos' then 1 else 0 end, ISC">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="rcbMes" DefaultValue="201008" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbProducto" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbZona" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbTecnico" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbCliente" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <telerik:RadGrid ID="rgUnidades" runat="server" AllowPaging="True" 
                                            AllowSorting="True" AutoGenerateColumns="False" Culture="es-MX" 
                                            DataSourceID="sdsResultados" GridLines="None" Height="500px" PageSize="18" 
                                            Width="100%">
                                            <SortingSettings SortToolTip="Click aquí para ordenar" />
                                            <ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                            </ClientSettings>
                                            <AlternatingItemStyle BackColor="#CCCCCC" />
                                            <MasterTableView DataSourceID="sdsResultados">
                                                <CommandItemSettings ExportToPdfText="Export to Pdf" />
                                                <RowIndicatorColumn>
                                                    <HeaderStyle Width="20px" />
                                                </RowIndicatorColumn>
                                                <ExpandCollapseColumn>
                                                    <HeaderStyle Width="20px" />
                                                </ExpandCollapseColumn>
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Unidad" HeaderText="Unidad" 
                                                        SortExpression="Unidad" UniqueName="Unidad">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ISC" DataType="System.Byte" 
                                                        HeaderText="ISC" SortExpression="ISC" UniqueName="ISC">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Tendencia" DataType="System.Int16" 
                                                        HeaderText="Tendencia" SortExpression="Tendencia" UniqueName="Tendencia">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Respuesta" HeaderText="Respuesta" 
                                                        SortExpression="Respuesta" UniqueName="Respuesta">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Iniciales" DataType="System.Int32" 
                                                        HeaderText="Iniciales" SortExpression="Iniciales" UniqueName="Iniciales">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Recibidas" DataType="System.Int32" 
                                                        HeaderText="Recibidas" SortExpression="Recibidas" UniqueName="Recibidas">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Cerradas" DataType="System.Int32" 
                                                        HeaderText="Cerradas" SortExpression="Cerradas" UniqueName="Cerradas">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Faltantes" DataType="System.Int32" 
                                                        HeaderText="Faltantes" ReadOnly="True" SortExpression="Faltantes" 
                                                        UniqueName="Faltantes">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="DentroSLA" DataType="System.Int32" 
                                                        HeaderText="DentroSLA" SortExpression="DentroSLA" UniqueName="DentroSLA">
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" 
                                                EnableImageSprites="True">
                                            </HeaderContextMenu>
                                        </telerik:RadGrid>
                                    </div>
                                </telerik:RadPane>
                                <telerik:RadSplitBar ID="RadSplitBar1" Runat="server" CollapseMode="Backward">
                                </telerik:RadSplitBar>
                                <telerik:RadPane ID="RadPane1" Runat="server" EnableTheming="True" 
                                    MaxWidth="400" MinWidth="200" Scrolling="None" Width="200px">
                                    <div style="height: 300px; width: 300px">
                                        <asp:SqlDataSource ID="sdsHistorico" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="SELECT MesLetra, cast(SLA as decimal(5,2)) as SLA, cast(ISC as decimal(5,2)) as ISC
FROM dbo.Historico(?,4,?,?,?,?)
ORDER BY Mes">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="rcbMes" DefaultValue="201009" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbProducto" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbZona" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbTecnico" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbCliente" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <asp:SqlDataSource ID="sdsPreguntas" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                                            SelectCommand="Select Codigo as Pregunta, dbo.propre(Codigo,?,?,?,?,?) from Preguntas">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="rcbMes" DefaultValue="201009" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbTecnico" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbZona" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbProducto" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbCliente" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <telerik:RadChart ID="RadChart2" runat="server" AutoLayout="True" 
                                            ClientIDMode="Static" DataSourceID="sdsHistorico" DefaultType="Line"
                                            Height="250px" Width="200px">
                                            <Appearance>
                                                <Border Color="134, 134, 134" />
                                            </Appearance>
                                            <Series>
                                                <telerik:ChartSeries DataYColumn="SLA" Name="SLA" Type="Line">
                                                    <Appearance>
                                                        <FillStyle FillType="Solid" MainColor="69, 115, 167">
                                                        </FillStyle>
                                                        <TextAppearance TextProperties-Color="Black">
                                                        </TextAppearance>
                                                        <Border Color="69, 115, 167" />
                                                    </Appearance>
                                                </telerik:ChartSeries>
                                                <telerik:ChartSeries DataYColumn="ISC" Name="ISC" Type="Line">
                                                    <Appearance>
                                                        <FillStyle FillType="Solid" MainColor="107, 70, 68">
                                                        </FillStyle>
                                                        <TextAppearance TextProperties-Color="Black">
                                                        </TextAppearance>
                                                        <Border Color="107, 70, 68" />
                                                    </Appearance>
                                                </telerik:ChartSeries>
                                            </Series>
                                            <Legend>
                                                <Appearance Position-Auto="False" Position-X="130" 
                                                    Position-Y="2" Dimensions-Margins="15%, 2%, 1px, 1px" 
                                                    Dimensions-Paddings="2px, 8px, 6px, 3px">
                                                    <ItemTextAppearance TextProperties-Color="Black">
                                                    </ItemTextAppearance>
                                                    <ItemMarkerAppearance Figure="Square">
                                                    </ItemMarkerAppearance>
                                                </Appearance>
                                            </Legend>
                                            <PlotArea>
                                                <XAxis AutoScale="False" DataLabelsColumn="Col0 enlazada a datos" MaxValue="4" 
                                                    MinValue="1" Step="1">
                                                    <Appearance Color="134, 134, 134" MajorTick-Color="134, 134, 134">
                                                        <MajorGridLines Color="134, 134, 134" Width="0" />
                                                        <TextAppearance TextProperties-Color="Black">
                                                        </TextAppearance>
                                                    </Appearance>
                                                    <AxisLabel>
                                                        <TextBlock>
                                                            <Appearance TextProperties-Color="Black">
                                                            </Appearance>
                                                        </TextBlock>
                                                    </AxisLabel>
                                                    <Items>
                                                        <telerik:ChartAxisItem Value="1">
                                                            <TextBlock Text="Jun">
                                                            </TextBlock>
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="2">
                                                            <TextBlock Text="Jul">
                                                            </TextBlock>
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="3">
                                                            <TextBlock Text="Ago">
                                                            </TextBlock>
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="4">
                                                            <TextBlock Text="Sep">
                                                            </TextBlock>
                                                        </telerik:ChartAxisItem>
                                                    </Items>
                                                </XAxis>
                                                <YAxis IsZeroBased="False">
                                                    <Appearance Color="134, 134, 134" MajorTick-Color="134, 134, 134" 
                                                        MinorTick-Color="134, 134, 134">
                                                        <MajorGridLines Color="134, 134, 134" />
                                                        <MinorGridLines Color="134, 134, 134" />
                                                        <TextAppearance TextProperties-Color="Black">
                                                        </TextAppearance>
                                                    </Appearance>
                                                    <AxisLabel>
                                                        <TextBlock>
                                                            <Appearance TextProperties-Color="Black">
                                                            </Appearance>
                                                        </TextBlock>
                                                    </AxisLabel>
                                                </YAxis>
                                                <Appearance Dimensions-Margins="18%, 24%, 12%, 10%">
                                                    <FillStyle MainColor="" FillType="Solid">
                                                    </FillStyle>
                                                </Appearance>
                                            </PlotArea>
                                            <ChartTitle Appearance-Position-AlignedPosition="TopLeft">
                                                <Appearance Position-Auto="False" Position-X="5" Position-Y="2.62">
                                                    <FillStyle MainColor="">
                                                    </FillStyle>
                                                </Appearance>
                                                <TextBlock Text="Histórico">
                                                    <Appearance TextProperties-Color="Black" TextProperties-Font="Arial, 18px">
                                                    </Appearance>
                                                </TextBlock>
                                            </ChartTitle>
                                        </telerik:RadChart>
                                        <telerik:RadChart ID="RadChart4" runat="server" AutoLayout="True" 
                                            ClientIDMode="Static" DataSourceID="sdsPreguntas" Height="250px" 
                                            SeriesOrientation="Horizontal" Width="200px">
                                            <Appearance>
                                                <Border Color="134, 134, 134" />
                                            </Appearance>
                                            <Series>
                                                <telerik:ChartSeries DataYColumn="Column1" Name="Column1">
                                                    <Appearance>
                                                        <FillStyle FillType="Solid" MainColor="69, 115, 167">
                                                        </FillStyle>
                                                        <TextAppearance TextProperties-Color="Black">
                                                        </TextAppearance>
                                                        <Border Color="69, 115, 167" />
                                                    </Appearance>
                                                </telerik:ChartSeries>
                                            </Series>
                                            <Legend Visible="False">
                                                <Appearance Position-Auto="False" 
                                                    Position-X="130" Position-Y="2" Visible="False" 
                                                    Dimensions-Margins="15%, 2%, 1px, 1px" Dimensions-Paddings="2px, 8px, 6px, 3px">
                                                    <ItemTextAppearance TextProperties-Color="Black">
                                                    </ItemTextAppearance>
                                                    <ItemMarkerAppearance Figure="Square">
                                                    </ItemMarkerAppearance>
                                                </Appearance>
                                            </Legend>
                                            <PlotArea>
                                                <XAxis AutoScale="False" DataLabelsColumn="Pregunta" MaxValue="7" MinValue="1" 
                                                    Step="1">
                                                    <Appearance Color="134, 134, 134" MajorTick-Color="134, 134, 134">
                                                        <MajorGridLines Color="134, 134, 134" Width="0" />
                                                        <TextAppearance TextProperties-Color="Black">
                                                        </TextAppearance>
                                                    </Appearance>
                                                    <AxisLabel>
                                                        <Appearance RotationAngle="270">
                                                        </Appearance>
                                                        <TextBlock>
                                                            <Appearance TextProperties-Color="Black">
                                                            </Appearance>
                                                        </TextBlock>
                                                    </AxisLabel>
                                                    <Items>
                                                        <telerik:ChartAxisItem Value="1">
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="2">
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="3">
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="4">
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="5">
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="6">
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="7">
                                                        </telerik:ChartAxisItem>
                                                    </Items>
                                                </XAxis>
                                                <YAxis IsZeroBased="False">
                                                    <Appearance Color="134, 134, 134" MajorTick-Color="134, 134, 134" 
                                                        MinorTick-Color="134, 134, 134">
                                                        <MajorGridLines Color="134, 134, 134" />
                                                        <MinorGridLines Color="134, 134, 134" />
                                                        <TextAppearance TextProperties-Color="Black">
                                                        </TextAppearance>
                                                    </Appearance>
                                                    <AxisLabel>
                                                        <Appearance RotationAngle="0">
                                                        </Appearance>
                                                        <TextBlock>
                                                            <Appearance TextProperties-Color="Black">
                                                            </Appearance>
                                                        </TextBlock>
                                                    </AxisLabel>
                                                </YAxis>
                                                <YAxis2>
                                                    <AxisLabel>
                                                        <Appearance RotationAngle="0">
                                                        </Appearance>
                                                    </AxisLabel>
                                                </YAxis2>
                                                <Appearance>
                                                    <FillStyle MainColor="" FillType="Solid">
                                                    </FillStyle>
                                                </Appearance>
                                            </PlotArea>
                                            <ChartTitle Appearance-Position-AlignedPosition="TopLeft">
                                                <Appearance Position-Auto="False" Position-X="5" Position-Y="2.62">
                                                    <FillStyle MainColor="">
                                                    </FillStyle>
                                                </Appearance>
                                                <TextBlock Text="Preguntas">
                                                    <Appearance TextProperties-Color="Black" TextProperties-Font="Arial, 18px">
                                                    </Appearance>
                                                </TextBlock>
                                            </ChartTitle>
                                        </telerik:RadChart>
                                    </div>
                                </telerik:RadPane>
                            </telerik:RadSplitter>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="RadPageView2" runat="server">
                            <div style="height: 27px; padding-top: 4px; background-image: url('Images/fondobarra.png'); background-repeat: repeat-x;">
                                <span class="labFiltro">
                                <asp:Label ID="labMesMostrado0" runat="server">Mes:</asp:Label>
                                </span>
                                <telerik:RadComboBox ID="rcbMes0" Runat="server" AutoPostBack="True" 
                                    CssClass="comboFiltro" DataSourceID="sdsMes0" DataTextField="Column1" 
                                    DataValueField="Mes" LoadingMessage="Cargando..." Width="100px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="sdsMes0" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                    ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="select distinct STUFF(convert(nvarchar,convert(datetime, Mes + '01', 112),107), 5, 4, ''), Mes
from unidadesmensual
order by Mes desc"></asp:SqlDataSource>
                                <telerik:RadComboBox ID="rcbProducto0" Runat="server" AllowCustomText="True" 
                                    AutoPostBack="True" DataSourceID="sdsProducto0" DataTextField="Nombre" 
                                    DataValueField="Codigo" DropDownCssClass="dropdowncristos" 
                                    EmptyMessage="Producto..." Filter="Contains" MarkFirstMatch="True" 
                                    onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible()) sender.showDropDown(); })" 
                                    OnItemDataBound="rcbProducto_ItemDataBound" Width="180px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="sdsProducto0" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                    ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                                    SelectCommand="SELECT [Nombre], [Codigo] FROM [Productos] ORDER BY [Nombre]">
                                </asp:SqlDataSource>
                                <telerik:RadComboBox ID="rcbZona0" Runat="server" AllowCustomText="True" 
                                    AutoPostBack="True" CssClass="comboFiltro" DataSourceID="sdsZona0" 
                                    DataTextField="Nombre" DataValueField="Codigo" DropDownCssClass="dropdown" 
                                    EmptyMessage="Zona..." Filter="Contains" MarkFirstMatch="True" 
                                    onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible()) sender.showDropDown(); })" 
                                    Width="180px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="sdsZona0" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                    ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                                    SelectCommand="SELECT [Nombre], [Codigo] FROM [Zonas] ORDER BY [Nombre]">
                                </asp:SqlDataSource>
                                <telerik:RadComboBox ID="rcbCliente0" Runat="server" AllowCustomText="True" 
                                    AutoPostBack="True" CssClass="comboFiltro" DataSourceID="sdsClientes0" 
                                    DataTextField="nombre" DataValueField="codigo" EmptyMessage="Cliente..." 
                                    Filter="Contains" MarkFirstMatch="True" 
                                    onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible()) sender.showDropDown(); })" 
                                    Width="180px">
                                </telerik:RadComboBox>
                                <asp:SqlDataSource ID="sdsClientes0" runat="server" 
                                    ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                    ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="
select u.Nombre, c.Codigo
from usuarios u join clientes c on u.codigo=c.usuario
order by u.nombre"></asp:SqlDataSource>
                            </div>
                            <telerik:RadSplitter ID="rsTecnicos" Runat="server" Height="500px" 
                                LiveResize="True" Width="100%">
                                <telerik:RadPane ID="RadPane3" Runat="server" Scrolling="None">
                                    <div style="float: left; width: 100%; height: 500px; position: relative;">
                                        <asp:SqlDataSource ID="sdsResultados0" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="DECLARE @Dia datetime, @Mes char(6), @Tecnico int, @Zona int, @Producto int, @Cliente int ;
DECLARE @cambiosdia TABLE (Unico int, Columna int, Anterior varchar(256), Fecha datetime);
SET @Mes=?
SET @Dia = CAST(@Mes + '16' as datetime);
SET @Producto=?
SET @Zona=?
SET @Tecnico=?
SET @Cliente=?
SELECT us.Alias as Unidad, ISC, Tendencia, Respuesta, Iniciales, Recibidas, Cerradas, Faltantes, DentroSLA
FROM UnidadesMensual um INNER JOIN
       		(
			SELECT t.Codigo, t.Alias, ISNULL(c.Anterior, t.Sitio) AS Sitio
        		FROM Unidades t LEFT OUTER JOIN @cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 2) = 2)
		) us ON um.Unidad = us.Codigo INNER JOIN
		(
			SELECT t.Codigo, ISNULL(c.Anterior, t.Producto) AS Producto
        		FROM Unidades t LEFT OUTER JOIN
        			@cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 5) = 5)
		) up ON um.Unidad = up.Codigo INNER JOIN
        	(
			SELECT t.Codigo, ISNULL(c.Anterior, t.Cliente) AS Cliente
	                FROM Unidades t LEFT OUTER JOIN
        	        	@cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 6) = 6)
		) uc ON um.Unidad = uc.Codigo INNER JOIN
        	(
			SELECT t.Codigo, ISNULL(c.Anterior, t.Encuestada) AS Encuestada
	                FROM Unidades t LEFT OUTER JOIN
        	        	@cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 3) = 3)
		) ue ON um.Unidad = ue.Codigo INNER JOIN
	       	(
			SELECT t.Codigo, ISNULL(c.Anterior, t.Zona) AS Zona
	                FROM Sitios t LEFT OUTER JOIN
	                  	@cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 2) = 2)
		) sz ON us.Sitio = sz.Codigo INNER JOIN
	       	(
			SELECT t.Codigo, ISNULL(c.Anterior, t.Responsable) AS Responsable
	                FROM Sitios t LEFT OUTER JOIN
	                  	@cambiosdia c ON c.Unico = t.Unico AND (ISNULL(c.Columna, 3) = 3)
		) sr ON us.Sitio = sr.Codigo
	WHERE	um.Mes = @Mes
		AND (@Producto = 0 OR up.Producto = @Producto) 
		AND (@Cliente = 0 OR uc.Cliente = @Cliente) 
		AND (@Zona = 0 OR sz.Zona = @Zona) 
		AND (@Tecnico = 0 OR sr.Responsable = @Tecnico) 
ORDER BY ue.Encuestada DESC, case when Respuesta like '%intentos' then 1 else 0 end, ISC">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="rcbMes" DefaultValue="201008" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbProducto" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbZona" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbTecnico" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbCliente" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <telerik:RadGrid ID="rgTecnicos" runat="server" AllowPaging="True" 
                                            AllowSorting="True" AutoGenerateColumns="False" Culture="es-MX" 
                                            DataSourceID="sdsResultados0" GridLines="None" Height="500px" PageSize="18" 
                                            ShowFooter="True" Width="100%">
                                            <SortingSettings SortToolTip="Click aquí para ordenar" />
                                            <ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                                                <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                                            </ClientSettings>
                                            <AlternatingItemStyle BackColor="#CCCCCC" />
                                            <MasterTableView DataSourceID="sdsResultados0">
                                                <CommandItemSettings ExportToPdfText="Export to Pdf" />
                                                <RowIndicatorColumn>
                                                    <HeaderStyle Width="20px" />
                                                </RowIndicatorColumn>
                                                <ExpandCollapseColumn>
                                                    <HeaderStyle Width="20px" />
                                                </ExpandCollapseColumn>
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="Unidad" HeaderText="Unidad" 
                                                        SortExpression="Unidad" UniqueName="Unidad">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ISC" DataType="System.Byte" 
                                                        HeaderText="ISC" SortExpression="ISC" UniqueName="ISC">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Tendencia" DataType="System.Int16" 
                                                        HeaderText="Tendencia" SortExpression="Tendencia" UniqueName="Tendencia">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Respuesta" HeaderText="Respuesta" 
                                                        SortExpression="Respuesta" UniqueName="Respuesta">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Iniciales" DataType="System.Int32" 
                                                        HeaderText="Iniciales" SortExpression="Iniciales" UniqueName="Iniciales">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Recibidas" DataType="System.Int32" 
                                                        HeaderText="Recibidas" SortExpression="Recibidas" UniqueName="Recibidas">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Cerradas" DataType="System.Int32" 
                                                        HeaderText="Cerradas" SortExpression="Cerradas" UniqueName="Cerradas">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Faltantes" DataType="System.Int32" 
                                                        HeaderText="Faltantes" ReadOnly="True" SortExpression="Faltantes" 
                                                        UniqueName="Faltantes">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="DentroSLA" DataType="System.Int32" 
                                                        HeaderText="DentroSLA" SortExpression="DentroSLA" UniqueName="DentroSLA">
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                            <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" 
                                                EnableImageSprites="True">
                                            </HeaderContextMenu>
                                        </telerik:RadGrid>
                                    </div>
                                </telerik:RadPane>
                                <telerik:RadSplitBar ID="RadSplitBar2" Runat="server" CollapseMode="Backward">
                                </telerik:RadSplitBar>
                                <telerik:RadPane ID="RadPane4" Runat="server" EnableTheming="True" 
                                    MaxWidth="400" MinWidth="200" Scrolling="None" Width="200px">
                                    <div style="height: 300px; width: 300px">
                                        <asp:SqlDataSource ID="sdsHistorico0" runat="server" 
                                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="SELECT MesLetra, cast(SLA as decimal(5,2)) as SLA, cast(ISC as decimal(5,2)) as ISC
FROM dbo.Historico(?,4,?,?,?,?)
ORDER BY Mes">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="rcbMes" DefaultValue="201009" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbProducto" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbZona" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbTecnico" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                                <asp:ControlParameter ControlID="rcbCliente" DefaultValue="0" Name="?" 
                                                    PropertyName="SelectedValue" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                        <telerik:RadChart ID="RadChart3" runat="server" AutoLayout="True" 
                                            ClientIDMode="Static" DataSourceID="sdsHistorico0" DefaultType="Line" 
                                            Height="250px" Width="200px">
                                            <Appearance>
                                                <FillStyle MainColor="235, 249, 213">
                                                </FillStyle>
                                                <Border Color="203, 225, 169" />
                                            </Appearance>
                                            <Series>
                                                <telerik:ChartSeries DataYColumn="SLA" Name="SLA" Type="Line">
                                                    <Appearance>
                                                        <FillStyle FillType="ComplexGradient" MainColor="243, 206, 119">
                                                            <FillSettings>
                                                                <ComplexGradient>
                                                                    <telerik:GradientElement Color="243, 206, 119" />
                                                                    <telerik:GradientElement Color="236, 190, 82" Position="0.5" />
                                                                    <telerik:GradientElement Color="210, 157, 44" Position="1" />
                                                                </ComplexGradient>
                                                            </FillSettings>
                                                        </FillStyle>
                                                        <TextAppearance TextProperties-Color="112, 93, 56">
                                                        </TextAppearance>
                                                        <Border Color="223, 170, 40" />
                                                    </Appearance>
                                                </telerik:ChartSeries>
                                                <telerik:ChartSeries DataYColumn="ISC" Name="ISC" Type="Line">
                                                    <Appearance>
                                                        <FillStyle FillType="ComplexGradient" MainColor="154, 220, 230">
                                                            <FillSettings>
                                                                <ComplexGradient>
                                                                    <telerik:GradientElement Color="154, 220, 230" />
                                                                    <telerik:GradientElement Color="121, 207, 220" Position="0.5" />
                                                                    <telerik:GradientElement Color="89, 185, 204" Position="1" />
                                                                </ComplexGradient>
                                                            </FillSettings>
                                                        </FillStyle>
                                                        <TextAppearance TextProperties-Color="112, 93, 56">
                                                        </TextAppearance>
                                                        <Border Color="117, 177, 192" />
                                                    </Appearance>
                                                </telerik:ChartSeries>
                                            </Series>
                                            <Legend>
                                                <Appearance Corners="Round, Round, Round, Round, 6" 
                                                    Position-AlignedPosition="None" Position-Auto="False" Position-X="130" 
                                                    Position-Y="2">
                                                    <ItemTextAppearance TextProperties-Color="113, 94, 57">
                                                    </ItemTextAppearance>
                                                    <Border Color="225, 217, 201" />
                                                </Appearance>
                                            </Legend>
                                            <PlotArea>
                                                <XAxis AutoScale="False" DataLabelsColumn="Col0 enlazada a datos" MaxValue="4" 
                                                    MinValue="1" Step="1">
                                                    <Appearance Color="226, 218, 202" MajorTick-Color="216, 207, 190">
                                                        <MajorGridLines Color="226, 218, 202" />
                                                        <TextAppearance TextProperties-Color="112, 93, 56">
                                                        </TextAppearance>
                                                    </Appearance>
                                                    <AxisLabel>
                                                        <TextBlock>
                                                            <Appearance TextProperties-Color="112, 93, 56">
                                                            </Appearance>
                                                        </TextBlock>
                                                    </AxisLabel>
                                                    <Items>
                                                        <telerik:ChartAxisItem Value="1">
                                                            <TextBlock Text="Jun">
                                                            </TextBlock>
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="2">
                                                            <TextBlock Text="Jul">
                                                            </TextBlock>
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="3">
                                                            <TextBlock Text="Ago">
                                                            </TextBlock>
                                                        </telerik:ChartAxisItem>
                                                        <telerik:ChartAxisItem Value="4">
                                                            <TextBlock Text="Sep">
                                                            </TextBlock>
                                                        </telerik:ChartAxisItem>
                                                    </Items>
                                                </XAxis>
                                                <YAxis IsZeroBased="False">
                                                    <Appearance Color="226, 218, 202" MajorTick-Color="226, 218, 202" 
                                                        MinorTick-Color="226, 218, 202">
                                                        <MajorGridLines Color="231, 225, 212" />
                                                        <MinorGridLines Color="231, 225, 212" />
                                                        <TextAppearance TextProperties-Color="112, 93, 56">
                                                        </TextAppearance>
                                                    </Appearance>
                                                    <AxisLabel>
                                                        <TextBlock>
                                                            <Appearance TextProperties-Color="112, 93, 56">
                                                            </Appearance>
                                                        </TextBlock>
                                                    </AxisLabel>
                                                </YAxis>
                                                <Appearance Dimensions-Margins="18%, 23%, 12%, 10%">
                                                    <FillStyle MainColor="254, 255, 228" SecondColor="Transparent">
                                                    </FillStyle>
                                                    <Border Color="226, 218, 202" />
                                                </Appearance>
                                            </PlotArea>
                                            <ChartTitle Appearance-Position-AlignedPosition="TopLeft">
                                                <Appearance Position-Auto="False" Position-X="5" Position-Y="2.62">
                                                    <FillStyle MainColor="">
                                                    </FillStyle>
                                                </Appearance>
                                                <TextBlock Text="Histórico">
                                                    <Appearance TextProperties-Color="77, 153, 4">
                                                    </Appearance>
                                                </TextBlock>
                                            </ChartTitle>
                                        </telerik:RadChart>
                                    </div>
                                </telerik:RadPane>
                            </telerik:RadSplitter>
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="RadPageView3" runat="server">
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
    <div id="ISC">
    <table>
        <tr><td><asp:Label id="lblISC" runat="server">ISC:</asp:Label></td><td>
        <asp:Label id="lblNSC" runat="server" backcolor="White" borderstyle="Inset" 
            borderwidth="2px" align="right" bordercolor="Gray" forecolor="Black">
                100
        </asp:Label>  
        </td></tr>                                      
    </table>
    </div>
</form>
</body>
</html>