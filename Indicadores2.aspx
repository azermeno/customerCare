<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Indicadores2.aspx.cs" Inherits="CustomerCare._Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" 
    namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Charting" tagprefix="telerik" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="Styles/Indicadores.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div style="width: 100%; left: 0px; top: 0px; height: 100%;">
                <div>
                <span class="labFiltro">
                    <asp:Label ID="labMesMostrado" runat="server">Mes:</asp:Label>
                </span>
        <telerik:RadComboBox ID="rcbMes" Runat="server" AutoPostBack="True" 
                    CssClass="comboFiltro" DataSourceID="sdsMes" DataTextField="Column1" 
                    DataValueField="Mes" Skin="Windows7" Width="200px">
                </telerik:RadComboBox>
                <asp:SqlDataSource ID="sdsMes" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                    ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="select distinct STUFF(convert(nvarchar,convert(datetime, Mes + '01', 112),107), 5, 4, ''), Mes
from unidadesmensual
order by Mes desc"></asp:SqlDataSource>
                <span class="labFiltro">
                    <asp:Label ID="labProducto" runat="server">Producto:</asp:Label>
                </span>
                <telerik:RadComboBox ID="rcbProducto" Runat="server" AutoPostBack="True" 
                    CssClass="comboFiltro" DataSourceID="sdsProducto" DataTextField="Nombre" 
                    DataValueField="Codigo" Skin="Windows7" Width="200px">
                </telerik:RadComboBox>
                <asp:SqlDataSource ID="sdsProducto" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                    ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="select Nombre, Codigo from (
select 'Todos' as Nombre, 0 as Codigo, 0 as es
union
SELECT [Nombre], [Codigo], 1 as es FROM [Productos])t 
ORDER BY t.es, t.[Nombre]"></asp:SqlDataSource>
                <span class="labFiltro">
                    <asp:Label ID="labZona" runat="server">Zona:</asp:Label>
                </span>
                <telerik:RadComboBox ID="rcbZona" Runat="server" AutoPostBack="True" 
                    CssClass="comboFiltro" DataSourceID="sdsZona" DataTextField="Nombre" 
                    DataValueField="Codigo" Width="200px">
                </telerik:RadComboBox>
                <asp:SqlDataSource ID="sdsZona" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                    ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="select Nombre, Codigo from (
select 'Todas' as Nombre, 0 as Codigo, 0 as es
union
SELECT [Nombre], [Codigo], 1 as es FROM [Zonas])t 
ORDER BY t.es, t.[Nombre]"></asp:SqlDataSource>
                <span class="labFiltro">
                    <asp:Label ID="labTecnico" runat="server">Técnico:</asp:Label>
                </span>
                <telerik:RadComboBox ID="rcbTecnico" Runat="server" AutoPostBack="True" 
                    CssClass="comboFiltro" DataSourceID="sdsTecnico" DataTextField="Nombre" 
                    DataValueField="Codigo" Width="200px">
                </telerik:RadComboBox>
                <asp:SqlDataSource ID="sdsTecnico" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                    ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="select Nombre, Codigo
from
(
select 'Todos' as Nombre, 0 as Codigo, 0 as es
union
SELECT distinct us.Nombre, i.Codigo, 1 as es
from unidades u join sitios s on u.sitio=s.codigo join internos i on s.responsable=i.codigo join usuarios us on i.usuario=us.codigo
)t
order by t.es, t.nombre"></asp:SqlDataSource>
                <span class="labFiltro">
                    <asp:Label ID="labCliente" runat="server">Cliente:</asp:Label>
                </span>
                <telerik:RadComboBox ID="rcbCliente" Runat="server" AutoPostBack="True" 
                    CssClass="comboFiltro" DataSourceID="sdsClientes" DataTextField="nombre" 
                    DataValueField="codigo" Width="200px">
                </telerik:RadComboBox>
                <asp:SqlDataSource ID="sdsClientes" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                    ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="select nombre, codigo
from
(
select 'Todos' as Nombre, 0 as Codigo, 0 as es
union
select u.Nombre, c.Codigo, 1 as es
from usuarios u join clientes c on u.codigo=c.usuario
)t
order by t.es, t.nombre"></asp:SqlDataSource>
                </div>
    <telerik:RadSplitter ID="RadSplitter1" Runat="server" Skin="WebBlue" 
        Width="100%" LiveResize="True" Height="530px">
        <telerik:RadPane ID="RadPane2" Runat="server" Scrolling="None">
            <div style="float: left; width: 100%; height: 500px; position: relative;">
                <asp:SqlDataSource ID="sdsResultados" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                    ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="DECLARE @Dia datetime, @Mes char(6), @Tecnico int, @Zona int, @Producto int, @Cliente int ;
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
            
                <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" 
                    AllowSorting="True" AutoGenerateColumns="False" Culture="es-MX" 
                    DataSourceID="sdsResultados" GridLines="None" Height="530px" PageSize="20" 
                    ShowFooter="True" Skin="Vista" Width="100%">
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
        <telerik:RadSplitBar ID="RadSplitBar1" Runat="server" 
            CollapseMode="Backward">
            </telerik:RadSplitBar>
        <telerik:RadPane ID="RadPane1" Runat="server" EnableTheming="True" 
            MaxWidth="400" MinWidth="200" Scrolling="None" Width="200px">
            <div style="height: 300px; width: 300px">
                <asp:SqlDataSource ID="sdsHistorico" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                    ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="SELECT MesLetra, cast(SLA as decimal(5,2)) as SLA, cast(ISC as decimal(5,2)) as ISC
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
                <telerik:RadChart ID="RadChart2" runat="server" AutoLayout="True" 
                    ClientIDMode="Static" DataSourceID="sdsHistorico" DefaultType="Line" 
                    Height="250px" onclick="RadChart1_Click" Skin="LightGreen" Width="200px">
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
    </div>
    <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbMes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadChart1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbProducto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadChart1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbZona">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadChart1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTecnico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadChart1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbCliente">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadChart1" />
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
    <div id="ISC">
        <asp:Label id="lblISC" runat="server">ISC:</asp:Label>
        <asp:Label id="lblNSC" runat="server" backcolor="White" borderstyle="Inset" 
            borderwidth="3px" align="right" bordercolor="Gray" forecolor="Black">
                100
        </asp:Label>                                        
    </div>
            
    </asp:Content>







