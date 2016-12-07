<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComparativoEstados.aspx.cs" Inherits="CustomerCare.ComparativoEstados" %>

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
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
        <telerik:RadToolBar ID="RadToolBar1" Runat="server" Width="100%" 
        BorderStyle="None" BorderWidth="0px" AutoPostBack="True">
            <Items>
                <telerik:RadToolBarButton runat="server" Text="Mensual" Checked="True" 
                    CheckOnClick="True" Group="Tipo">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Anual" CheckOnClick="True" 
                    Group="Tipo">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Button 2" PostBack="False">
                    <ItemTemplate>
                        <asp:Label ID="lblMA" runat="server" Text="Mes:"></asp:Label>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Button 3" PostBack="False">
                    <ItemTemplate>
                        <telerik:RadMonthYearPicker ID="RadMonthYearPicker1" runat="server" 
                            AutoPostBack="True">
                            <DateInput AutoPostBack="True" DateFormat="MMMM' de 'yyyy" 
                                DisplayDateFormat="MMMM' de 'yyyy">
                            </DateInput>
                            <DatePopupButton HoverImageUrl="" ImageUrl="" />
                        </telerik:RadMonthYearPicker>
                        <telerik:RadMaskedTextBox ID="RadNumericTextBox1" Runat="server" 
                            AutoPostBack="True" DisplayMask="####" DisplayPromptChar=" " 
                            EmptyMessage="Año con cuatro dígitos." Mask="####" PromptChar=" ">
                        </telerik:RadMaskedTextBox>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
            </Items>
        </telerik:RadToolBar>
    <div>
        <telerik:RadChart ID="RadChart1" runat="server" 
            Height="500px" Width="1000px" Skin="BlueStripes" 
            SeriesOrientation="Horizontal" DefaultType="Pie" 
            IntelligentLabelsEnabled="True">
            <Appearance>
                <FillStyle FillType="Hatch" MainColor="225, 235, 238" 
                    SecondColor="207, 223, 229">
                </FillStyle>
                <Border Color="131, 171, 184" />
            </Appearance>
            <Series>
                <telerik:ChartSeries DataLabelsColumn="Zona" DataYColumn="Tickets" 
                    Name="Series 1" Type="Pie">
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
<Appearance RotationAngle="270"></Appearance>

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
<Appearance RotationAngle="0"></Appearance>

                        <TextBlock>
                            <Appearance TextProperties-Color="102, 103, 86">
                            </Appearance>
                        </TextBlock>
                    </AxisLabel>
                </YAxis>

<YAxis2>
<AxisLabel>
<Appearance RotationAngle="0"></Appearance>
</AxisLabel>
</YAxis2>

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
    
    </div>
    <telerik:RadAjaxManagerProxy ID="RadAjaxManagerProxy1" runat="server">
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
