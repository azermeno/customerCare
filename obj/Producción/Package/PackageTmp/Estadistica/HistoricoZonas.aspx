<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HistoricoZonas.aspx.cs" Inherits="CustomerCare.Scripts.HistoricoZonas" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Charting" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    .RadChart
    {
        margin: auto;
    }
    </style>
</head>
<body style="margin:0px;">
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
    </telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
    </telerik:RadSkinManager>
        <telerik:RadToolBar ID="RadToolBar1" Runat="server" Width="100%" 
        BorderStyle="None" BorderWidth="0px">
            <Items>
                <telerik:RadToolBarButton runat="server" Text="Button 0">
                    <ItemTemplate>
                        <asp:Label ID="lblZona" runat="server" Text=""></asp:Label>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Button 1">
                    <ItemTemplate>
                        <telerik:RadComboBox ID="rcbZona" runat="server" AllowCustomText="True" 
                            AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="nombre" 
                            DataValueField="codigo" Filter="Contains">
                        </telerik:RadComboBox>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Button 2">
                    <ItemTemplate>
                        <asp:Label ID="lblDesde" runat="server" Text="Primer mes:"></asp:Label>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Button 3">
                    <ItemTemplate>
                        <telerik:RadMonthYearPicker ID="RadMonthYearPicker1" runat="server" 
                            AutoPostBack="True">
                            <DateInput AutoPostBack="True" DateFormat="MMMM' de 'yyyy" 
                                DisplayDateFormat="MMMM' de 'yyyy">
                            </DateInput>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                        </telerik:RadMonthYearPicker>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
            </Items>
        </telerik:RadToolBar>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
            SelectCommand="select codigo, nombre from zonas where activa = 1">
        </asp:SqlDataSource>
    <div>
        <telerik:RadChart ID="RadChart1" runat="server" DefaultType="Line" 
            Height="500px" Width="1000px" Skin="BlueStripes">
            <Appearance>
                <FillStyle FillType="Hatch" MainColor="225, 235, 238" 
                    SecondColor="207, 223, 229">
                </FillStyle>
                <Border Color="131, 171, 184" />
            </Appearance>
            <Series>
                <telerik:ChartSeries DataLabelsColumn="Mes" DataYColumn="Tickets" 
                    Name="Series 1" Type="Line">
                    <Appearance>
                        <FillStyle MainColor="222, 202, 152" FillType="ComplexGradient">
                            <FillSettings>
                                <ComplexGradient>
                                    <telerik:GradientElement Color="222, 202, 152" />
                                    <telerik:GradientElement Color="211, 185, 123" Position="0.5" />
                                    <telerik:GradientElement Color="183, 154, 84" Position="1" />
                                </ComplexGradient>
                            </FillSettings>
                        </FillStyle>
                        <TextAppearance TextProperties-Color="62, 117, 154">
                        </TextAppearance>
                        <Border Color="187, 149, 58" />
                    </Appearance>
                </telerik:ChartSeries>
            </Series>
            <Legend Visible="False">
                <Appearance Dimensions-Margins="1px, 1%, 1%, 1px" Visible="False">
                    <ItemTextAppearance TextProperties-Color="81, 103, 114">
                    </ItemTextAppearance>
                    <FillStyle MainColor="241, 253, 255">
                    </FillStyle>
                    <Border Color="193, 214, 221" />
                </Appearance>
            </Legend>
            <PlotArea>
                <XAxis>
                    <Appearance Color="193, 214, 221" MajorTick-Color="154, 153, 129" 
                        MajorTick-Visible="False">
                        <MajorGridLines Color="221, 227, 221" Width="0" />
                        <LabelAppearance Visible="False">
                        </LabelAppearance>
                        <TextAppearance TextProperties-Color="102, 103, 86">
                        </TextAppearance>
                    </Appearance>
                    <AxisLabel>
                        <TextBlock>
                            <Appearance TextProperties-Color="102, 103, 86">
                            </Appearance>
                        </TextBlock>
                    </AxisLabel>
                </XAxis>
                <YAxis>
                    <Appearance Color="193, 214, 221" MajorTick-Color="154, 153, 129" 
                        MinorTick-Color="193, 214, 221">
                        <MajorGridLines Color="221, 227, 221" />
                        <MinorGridLines Color="221, 227, 221" />
                        <TextAppearance TextProperties-Color="102, 103, 86">
                        </TextAppearance>
                    </Appearance>
                    <AxisLabel>
                        <TextBlock>
                            <Appearance TextProperties-Color="102, 103, 86">
                            </Appearance>
                        </TextBlock>
                    </AxisLabel>
                </YAxis>
                <Appearance Dimensions-Margins="18%, 8%, 12%, 8%">
                    <FillStyle MainColor="241, 253, 255" SecondColor="Transparent">
                    </FillStyle>
                    <Border Color="193, 214, 221" />
                </Appearance>
            </PlotArea>
            <ChartTitle>
                <Appearance Dimensions-Margins="3%, 10px, 14px, 5%">
                    <FillStyle MainColor="">
                    </FillStyle>
                </Appearance>
                <TextBlock>
                    <Appearance TextProperties-Color="81, 103, 114" 
                        TextProperties-Font="Verdana, 18pt">
                    </Appearance>
                </TextBlock>
            </ChartTitle>
        </telerik:RadChart>
        <asp:SqlDataSource ID="sdsHistoricoZonas" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="use cibernetica
select count(*), DATEPART(month,Apertura), DATEPART(year,apertura) from Incidencias where Estado&gt;1 and Estado&lt;5
group by DATEPART (month, apertura), DATEPART (year, apertura)
order by DATEPART (year, apertura), DATEPART(month, apertura)"></asp:SqlDataSource>
    
    </div>
    <telerik:RadAjaxManagerProxy runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadToolBar1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadChart1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManagerProxy>
    </form>
</body>
</html>
