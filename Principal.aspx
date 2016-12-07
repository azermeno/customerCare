<%@ Page Title="Página principal" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Principal.aspx.cs" Inherits="CustomerCare._Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Charting" tagprefix="telerik" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="Styles/Principal.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table width="100%">
	    <tr>
            <td align="center">
                <table cellspacing="1" cellpadding="1" width="100%" border="0">
                    <tr>
                        <td align="left" colspan="2"></td>
                        <td align="right">
    						<table cellspacing="1" cellpadding="1" border="0">
							    <tr>
    							    <td valign="middle">
								        <asp:Label id="lblISC" runat="server" font-names="Verdana" font-size="Large">ISC:</asp:Label>
                                    </td>
							        <td valign="middle">
    								    <asp:Label id="lblNSC" runat="server" font-names="Verdana" font-size="Large" font-bold="True" backcolor="White" borderstyle="Inset" borderwidth="3px" align="right" bordercolor="Gray" forecolor="Black">100</asp:Label>                                        
                                    </td>
							    </tr>
						    </table>
                        </td>
                    </tr>
                    <tr style="height:30px;">
                        <td width="330" align="left" rowspan="2">
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
                                ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" 
                                SelectCommand="HistoriaGeneral" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:Parameter DefaultValue="201005" Name="PrimerMes" Type="String" />
                                    <asp:Parameter DefaultValue="%" Name="Producto" Type="String" />
                                    <asp:Parameter DefaultValue="%" Name="Zona" Type="String" />
                                    <asp:Parameter DefaultValue="%" Name="Tecnico" Type="String" />
                                    <asp:Parameter DefaultValue="%" Name="Cliente" Type="String" />
                                    <asp:Parameter DefaultValue="1" Name="SoloEnc" Type="Int32" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <telerik:RadChart ID="RadChart1" runat="server" AutoLayout="True" 
                                DataSourceID="SqlDataSource1" DefaultType="Line" Skin="Marble">
                                <Appearance>
                                    <FillStyle FillType="Image">
                                        <FillSettings BackgroundImage="{chart}" ImageDrawMode="Flip">
                                        </FillSettings>
                                    </FillStyle>
                                    <Border Color="181, 166, 132" />
                                </Appearance>
                                <Series>
<telerik:ChartSeries Name="mesencuesta" DataYColumn="mesencuesta" Type="Line">
    <Appearance>
        <FillStyle FillType="ComplexGradient" MainColor="222, 202, 155">
            <FillSettings>
                <ComplexGradient>
                    <telerik:GradientElement Color="222, 202, 152" />
                    <telerik:GradientElement Color="211, 185, 123" Position="0.5" />
                    <telerik:GradientElement Color="183, 154, 84" Position="1" />
                </ComplexGradient>
            </FillSettings>
        </FillStyle>
        <TextAppearance TextProperties-Color="159, 159, 159">
        </TextAppearance>
        <Border Color="187, 149, 58" />
    </Appearance>
                                    </telerik:ChartSeries>
<telerik:ChartSeries Name="encuestados" DataYColumn="encuestados" Type="Line">
    <Appearance>
        <FillStyle FillType="ComplexGradient" MainColor="172, 208, 217">
            <FillSettings>
                <ComplexGradient>
                    <telerik:GradientElement Color="172, 208, 217" />
                    <telerik:GradientElement Color="149, 193, 204" Position="0.5" />
                    <telerik:GradientElement Color="114, 162, 175" Position="1" />
                </ComplexGradient>
            </FillSettings>
        </FillStyle>
        <TextAppearance TextProperties-Color="159, 159, 159">
        </TextAppearance>
        <Border Color="129, 180, 193" />
    </Appearance>
                                    </telerik:ChartSeries>
                                    <telerik:ChartSeries DataYColumn="respuestas" Name="respuestas" Type="Line">
                                        <Appearance>
                                            <FillStyle FillType="ComplexGradient" MainColor="185, 208, 152">
                                                <FillSettings>
                                                    <ComplexGradient>
                                                        <telerik:GradientElement Color="185, 208, 152" />
                                                        <telerik:GradientElement Color="164, 194, 122" Position="0.5" />
                                                        <telerik:GradientElement Color="131, 166, 80" Position="1" />
                                                    </ComplexGradient>
                                                </FillSettings>
                                            </FillStyle>
                                            <TextAppearance TextProperties-Color="159, 159, 159">
                                            </TextAppearance>
                                            <Border Color="123, 154, 69" />
                                        </Appearance>
                                    </telerik:ChartSeries>
                                    <telerik:ChartSeries DataYColumn="indicador" Name="indicador" Type="Line">
                                        <Appearance>
                                            <FillStyle FillType="ComplexGradient" MainColor="187, 174, 165">
                                                <FillSettings>
                                                    <ComplexGradient>
                                                        <telerik:GradientElement Color="187, 174, 165" />
                                                        <telerik:GradientElement Color="163, 146, 135" Position="0.5" />
                                                        <telerik:GradientElement Color="134, 115, 103" Position="1" />
                                                    </ComplexGradient>
                                                </FillSettings>
                                            </FillStyle>
                                            <TextAppearance TextProperties-Color="159, 159, 159">
                                            </TextAppearance>
                                            <Border Color="136, 119, 102" />
                                        </Appearance>
                                    </telerik:ChartSeries>
                                    <telerik:ChartSeries DataYColumn="incidencias" Name="incidencias" Type="Line">
                                        <Appearance>
                                            <FillStyle FillType="ComplexGradient" MainColor="192, 161, 188">
                                                <FillSettings>
                                                    <ComplexGradient>
                                                        <telerik:GradientElement Color="192, 161, 188" />
                                                        <telerik:GradientElement Color="174, 136, 169" Position="0.5" />
                                                        <telerik:GradientElement Color="154, 108, 149" Position="1" />
                                                    </ComplexGradient>
                                                </FillSettings>
                                            </FillStyle>
                                            <TextAppearance TextProperties-Color="159, 159, 159">
                                            </TextAppearance>
                                            <Border Color="123, 80, 125" />
                                        </Appearance>
                                    </telerik:ChartSeries>
                                    <telerik:ChartSeries DataYColumn="dentrosla" Name="dentrosla" Type="Line">
                                        <Appearance>
                                            <FillStyle FillType="ComplexGradient" MainColor="149, 179, 179">
                                                <FillSettings>
                                                    <ComplexGradient>
                                                        <telerik:GradientElement Color="149, 179, 179" />
                                                        <telerik:GradientElement Color="117, 155, 155" Position="0.5" />
                                                        <telerik:GradientElement Color="96, 134, 134" Position="1" />
                                                    </ComplexGradient>
                                                </FillSettings>
                                            </FillStyle>
                                            <TextAppearance TextProperties-Color="159, 159, 159">
                                            </TextAppearance>
                                            <Border Color="96, 134, 134" />
                                        </Appearance>
                                    </telerik:ChartSeries>
</Series>
                                <Legend>
                                    <Appearance Dimensions-Margins="17.6%, 3%, 1px, 1px" 
                                        Position-AlignedPosition="TopRight">
                                        <ItemTextAppearance TextProperties-Color="101, 91, 72" 
                                            TextProperties-Font="Georgia, 9pt">
                                        </ItemTextAppearance>
                                        <FillStyle MainColor="Transparent">
                                        </FillStyle>
                                        <Border Color="Transparent" />
                                    </Appearance>
                                </Legend>
                                <PlotArea>
                                    <XAxis AutoScale="False" DataLabelsColumn="mesencuesta" MaxValue="201011" 
                                        MinValue="201006" Step="1">
                                        <Appearance Color="Transparent" MajorTick-Color="173, 164, 142">
                                            <MajorGridLines Color="94, 93, 87" PenStyle="Solid" />
                                            <TextAppearance TextProperties-Color="101, 91, 72" 
                                                TextProperties-Font="Georgia, 9pt">
                                            </TextAppearance>
                                        </Appearance>
                                        <AxisLabel>
                                            <TextBlock>
                                                <Appearance TextProperties-Color="101, 91, 72" 
                                                    TextProperties-Font="Georgia, 9pt">
                                                </Appearance>
                                            </TextBlock>
                                        </AxisLabel>
                                        <Items>
                                            <telerik:ChartAxisItem Value="201006">
                                                <TextBlock>
                                                    <Appearance TextProperties-Font="Georgia, 9pt">
                                                    </Appearance>
                                                </TextBlock>
                                            </telerik:ChartAxisItem>
                                            <telerik:ChartAxisItem Value="201007">
                                                <TextBlock>
                                                    <Appearance TextProperties-Font="Georgia, 9pt">
                                                    </Appearance>
                                                </TextBlock>
                                            </telerik:ChartAxisItem>
                                            <telerik:ChartAxisItem Value="201008">
                                                <TextBlock>
                                                    <Appearance TextProperties-Font="Georgia, 9pt">
                                                    </Appearance>
                                                </TextBlock>
                                            </telerik:ChartAxisItem>
                                            <telerik:ChartAxisItem Value="201009">
                                                <TextBlock>
                                                    <Appearance TextProperties-Font="Georgia, 9pt">
                                                    </Appearance>
                                                </TextBlock>
                                            </telerik:ChartAxisItem>
                                            <telerik:ChartAxisItem Value="201010">
                                                <TextBlock>
                                                    <Appearance TextProperties-Font="Georgia, 9pt">
                                                    </Appearance>
                                                </TextBlock>
                                            </telerik:ChartAxisItem>
                                            <telerik:ChartAxisItem Value="201011">
                                                <TextBlock>
                                                    <Appearance TextProperties-Font="Georgia, 9pt">
                                                    </Appearance>
                                                </TextBlock>
                                            </telerik:ChartAxisItem>
                                        </Items>
                                    </XAxis>
                                    <YAxis>
                                        <Appearance Color="Transparent" MajorTick-Color="173, 164, 142" 
                                            MinorTick-Color="173, 164, 142">
                                            <MajorGridLines Color="94, 93, 87" PenStyle="Solid" />
                                            <MinorGridLines Color="94, 93, 87" PenStyle="Solid" />
                                            <TextAppearance TextProperties-Color="101, 91, 72" 
                                                TextProperties-Font="Georgia, 9pt">
                                            </TextAppearance>
                                        </Appearance>
                                        <AxisLabel>
                                            <TextBlock>
                                                <Appearance TextProperties-Color="101, 91, 72" 
                                                    TextProperties-Font="Georgia, 9pt">
                                                </Appearance>
                                            </TextBlock>
                                        </AxisLabel>
                                    </YAxis>
                                    <Appearance Dimensions-Margins="22%, 24%, 12%, 10%">
                                        <FillStyle FillType="Image">
                                            <FillSettings BackgroundImage="{plotarea}" ImageAlign="Top" 
                                                ImageDrawMode="Flip" ImageFlip="FlipY">
                                            </FillSettings>
                                        </FillStyle>
                                        <Border Color="144, 136, 118" Width="8" />
                                    </Appearance>
                                </PlotArea>
                                <ChartTitle>
                                    <Appearance>
                                        <FillStyle MainColor="Transparent">
                                        </FillStyle>
                                    </Appearance>
                                    <TextBlock Text="Histórico">
                                        <Appearance TextProperties-Color="89, 79, 52" TextProperties-Font="Arial, 22pt">
                                        </Appearance>
                                    </TextBlock>
                                </ChartTitle>
                            </telerik:RadChart>
                        </td>
                        <td align="center" valign="middle"> 
		                    <asp:Panel id="panMesMostrado" style="POSITION: relative; top: 15px;" runat="server" width="278px">
    		                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
			                        <tr>
    				                    <td align="left" style="HEIGHT: 24px">
				                            <asp:Label id="labMesMostrado" runat="server" font-names="Verdana" font-size="Smaller">Mes:</asp:Label>                                            
                                        </td>
				                        <td align="right" style="HEIGHT: 24px">
                                            <asp:DropDownList id="cmbMesMostrado" runat="server" width="200px" autopostback="True">
                                            </asp:DropDownList>
                                        </td>
			                        </tr>
		                        </table>
                            </asp:Panel>
                        </td>
                        <td width="330" align="right" rowspan="2"></td>
                    </tr>
                    <tr>
                        <td align="center" valign="bottom"> 
		                    <img align="middle" src="Images/line.png" width="338" height="1" alt=""/>
		                    <asp:Panel id="panProducto" style="POSITION: relative" runat="server" width="278px">
                                <table cellspacing="1" cellpadding="1" width="100%" border="0">
			                        <tr>
                                        <td align="left">
				                            <asp:Label id="labProducto" runat="server" font-names="Verdana" font-size="Smaller">Producto:</asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:DropDownList id="cmbProducto" runat="server" width="200px" autopostback="True"></asp:DropDownList>
                                        </td>
			                        </tr>
		                        </table>
                            </asp:Panel>
		                    <asp:Panel id="panZona" style="POSITION: relative" runat="server" width="278px"  visible="True">
                                <table cellspacing="1" cellpadding="1" width="100%" border="0">
			                        <tr>
    			                        <td align="left" style="HEIGHT: 24px">
				                            <asp:Label id="labZona" runat="server" font-names="Verdana" font-size="Smaller">Zona:</asp:Label>
                                        </td>
                                        <td align="right" style="HEIGHT: 24px">
                                            <asp:DropDownList id="cmbZona" runat="server" width="200px" autopostback="True"></asp:DropDownList>
                                        </td>
			                         </tr>
		                         </table>
                            </asp:Panel>
                            <asp:Panel id="panTecnico" style="POSITION: relative" runat="server" width="278px">
    		                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                                    <tr>
                                        <td align="left" style="HEIGHT: 24px">
				                            <asp:Label id="labTecnico" runat="server" font-names="Verdana" font-size="Smaller">Técnico:</asp:Label>
                                        </td>
				                        <td align="right" style="HEIGHT: 24px">
                                            <asp:DropDownList id="cmbTecnico" runat="server" width="200px" autopostback="True"></asp:DropDownList>
                                        </td>
	                                 </tr>
		                         </table>
                            </asp:Panel>
	                        <asp:Panel id="panCliente" style="POSITION: relative" runat="server" width="278px">
    		                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
			                        <tr>
    		                            <td align="left" style="HEIGHT: 24px">
	                                        <asp:Label id="labCliente" runat="server" font-names="Verdana" font-size="Smaller">Cliente:</asp:Label>
                                        </td>
				                        <td align="right" style="HEIGHT: 24px">
                                            <asp:DropDownList id="cmbCliente" runat="server" width="200px" autopostback="True"></asp:DropDownList>
                                        </td>
			                        </tr>
		                        </table>
                            </asp:Panel>
                            <table cellspacing="1" cellpadding="1" width="278" border="0">
                                <tr>
                                    <td style="HEIGHT: 24px" align="center">
    		                            <asp:CheckBox id="chkSoloEnc" runat="server" text="Sólo encuestados" tooltip="Mostrar sólo los solicitantes que responden la encuesta" textalign="Left" autopostback="True"></asp:CheckBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>   
                </table>
            </td>
        </tr>
        <tr>
            <td>
		        <asp:Button id="btnLogin" runat="server" Font-Name="Verdana" text="Comentarios" width="100%" font-names="Verdana" height="19px"></asp:Button>
            </td>
        </tr>
        <tr>
            <td height="100%">
                <div id="scroller2" align="left">
		            <asp:Table id="tblComents" runat="server" width="100%" forecolor="Black" visible="False"></asp:Table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>







