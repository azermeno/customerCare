<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComparativoTipificaciones.aspx.cs" Inherits="CustomerCare.WebForm6" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Charting" tagprefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
    .RadChart
    {
        margin: auto;
    }
    </style>
    <script type="text/javascript">
        var IE = document.all ? true : false;
        function showMenu(e) {
            //alert("orale");
            var contextMenu = $find("rcmTipificacion");
            var tempX = 0
            var tempY = 0
            if (IE) { // grab the x-y pos.s if browser is IE
                tempX = event.clientX + document.body.scrollLeft
                tempY = event.clientY + document.body.scrollTop
            } else {  // grab the x-y pos.s if browser is NS
                tempX = e.pageX
                tempY = e.pageY
            }
            // catch possible negative values in NS4
            if (tempX < 0) { tempX = 0 }
            if (tempY < 0) { tempY = 0 }
            contextMenu.showAt(tempX, tempY);
            $telerik.cancelRawEvent(event);
        }

        function showMenuK(n) {
            var contextMenu = $find("rcmTipificacion");
            if (n == 9) {
                contextMenu.showAt(400, 300);
                contextMenu.get_items().getItem(0).focus();
            }
        }

        function escribirLabel(sender, args) {
            var texto = '  ';
            var item = args.get_item();
            var codigo = item.get_value();
            var n = item.get_level();
            for (i = 0; i <= n; i++) {
                texto = '  ' + item.get_text() + texto;
                item = item.get_parent();
            }
            texto = texto.substring(2);
            var textbox = $find("RadToolBar1").findItemByText("comboZona").findControl("rtbDependencia");
            textbox.set_value(texto);
            document.getElementById('hflDependencia').value = codigo;
            var contextMenu = $find("rcmTipificacion");
            contextMenu.hide();
            document.forms[0].submit();
        }

        function cerrarMenu(n) {
            var contextMenu = $find("rcmTipificacion");
            var combo = $find("rtbAsunto");
            if (n == 9) {
                contextMenu.hide();
                combo.focus();
            }
        }

    </script> 
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
                <telerik:RadToolBarButton runat="server" Text="comboZona" PostBack="false">
                    <ItemTemplate>
                    <telerik:RadTextBox ID="rtbDependencia" Runat="server" 
        EmptyMessage="" Width="325px" ReadOnly="true"
             CssClass="form-text required">
    </telerik:RadTextBox>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
            </Items>
        </telerik:RadToolBar>
        <telerik:RadContextMenu ID="rcmTipificacion" 
        Runat="server" DataFieldID="Codigo" 
                            DataFieldParentID="Padre" 
        DataSourceID="sdsTipificacion" DataTextField="Descripcion" 
                            onclientitemclicked="escribirLabel" 
                            Skin="" DataValueField="Codigo" AutoScrollMinimumHeight="350" 
                            AutoScrollMinimumWidth="350" EnableAutoScroll="True" 
                            EnableRootItemScroll="True">
                            <Targets>
                                <telerik:ContextMenuControlTarget ControlID="rtbTipificacion" />
                            </Targets>
                        </telerik:RadContextMenu>
                        <asp:SqlDataSource ID="sdsTipificacion" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                            
        
        SelectCommand="Select 'Z'+cast(codigo as varchar) as Codigo, nombre as Descripcion, null as Padre from zonas where activa=1
        union
        Select 'S'+cast(codigo as varchar) as Codigo,nombre as Descripcion,'Z'+cast(Zona as varchar) as Padre from sitios where activo=1
        union
        select 'U'+cast(codigo as varchar) as Codigo,nombre as Descripcion,'S'+cast(Sitio as varchar) as Padre from unidades where activa=1"></asp:SqlDataSource>
    <asp:HiddenField ID="hflDependencia" runat="server" />
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
