<%@ Page language="c#" Debug="true" Codebehind="consola.pas" AutoEventWireup="false" Inherits="consola.Tconsola" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
  <head><title>Consola Principal</title>
  <script type="text/javascript"> 
	  function querySt(ji)
	  {
		hu = window.location.search.substring(1);
		gy = hu.split("&");
		for (i=0;i<gy.length;i++)
		{
		  ft = gy[i].split("=");
		  if (ft[0] == ji)
		  {
			return ft[1];
		  }
		}
	  };
	  function muestraEncReq(x)
	  {
		window.open("resencreq.aspx?encuestado=" + x, "_blank", "toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=750, height=500");
	  };
	  function ticketsReq(x)
	  {
	      incWin = window.open("Tickets/tickets.aspx?rq=" + x, "_blank", "toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=750, height=500");
		incWin.moveTo(0,0)
		incWin.resizeTo(screen.width, screen.height)
	  };  
	  function MisTickets()
	  {
		window.open("tickets.aspx", "tickets", "toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=" + screen.availWidth + ", height=" + screen.availHeight + ", top=0, left=0");
	  };
	  function CompTecnicos()
	  {
		window.open("tecnico.html", "tecnicos","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, width=1000, height=750");
	  };
	  function CompZonas()
	  {
		window.open("zona.html", "zonas","toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=auto, resizable=yes, width=1000, height=750");
	  };
	  function Capturador()
	  {
		window.open("capturador.aspx", "tickets", "toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, copyhistory=no, width=" + screen.availWidth + ", height=" + screen.availHeight + ", top=0, left=0");
	  };
  </script>
	<style type="text/css">BODY {
	OVERFLOW: hidden
}
#background {
	Z-INDEX: 1; WIDTH: 100%; POSITION: absolute; HEIGHT: 100%
}
#scroller {
	Z-INDEX: 2; LEFT: 0px; OVERFLOW: auto; WIDTH: 100%; POSITION: absolute; TOP: 0px; HEIGHT: 100%
}
#scroller2 {
	Z-INDEX: 3; LEFT: 0px; OVERFLOW: auto; WIDTH: 100%; POSITION: relative; TOP: 0px; HEIGHT: 100%
}
</style>
  </head>

  <body style="MARGIN: 0px" text="#ffffff" bgcolor="blue">
	 <form method="post" runat="server"><img id="background"
		   alt="" src="<%= ConfigurationManager.AppSettings["background"] %>">
		   <div id="scroller" align="center">
		   <table width="100%" height="100%">
		   <tr><td align="center">
  <table cellspacing="1" cellpadding="1" width="95%" border="0">
  <tr><td align="left" colspan="2">
		<img align="middle" src="logo2.png"></td><td align="right">
						<table cellspacing="1" cellpadding="1" border="0">
							<tr>
							  <td valign="middle">
								<ASP:Label id="lblISC" runat="server" font-names="Verdana" font-size="Large">ISC:</ASP:Label></td>
							  <td valign="middle">

								  <ASP:Label id="lblNSC" runat="server" font-names="Verdana" font-size="Large" font-bold="True" backcolor="White" borderstyle="Inset" borderwidth="3px" align="right" bordercolor="Gray" forecolor="Black">100</ASP:Label></td>
							</tr>
						</table></td></tr>

  <tr>
					  <td colspan="3">
		<img align="middle" src="line.png" width="100%" height="1">
</td></tr>
  <tr><td width="330" align="left" rowspan="3"><ASP:PlaceHolder id="holderGraficaGral" runat="server"></ASP:PlaceHolder></td>
<td align="center" valign="top"><ASP:Literal id="litTickets" runat="server" text='<input onclick="MisTickets()" type="button" value="Tickets">'></ASP:Literal><ASP:Literal id="litZonas" runat="server" text='<input onclick="CompZonas()" type="button" value="Zonas">'></ASP:Literal><ASP:Literal id="litTecnicos" runat="server" text='<input onclick="CompTecnicos()" type="button" value="Técnicos">'></ASP:Literal><ASP:Literal id="litCapturador" runat="server" text='<input onclick="Capturador()" type="button" value="Capturador">'></ASP:Literal>
</td><td width="330" align="right" rowspan="3">	<web:ChartControl id="grfPreguntas" runat="server" width="328px" borderwidth="1px" borderstyle="Outset" padding="4" toppadding="2" ycustomend="100" yvaluesinterval="10" ycustomstart="50" chartpadding="15" leftchartpadding="7" height="182px"
								  bordercolor="Silver" rightchartpadding="-10" topchartpadding="-10" haschartlegend="False" tooltip="Gráfico por pregunta">
                          <Charts>
                            <web:ColumnChart name="Preguntas" maxcolumnwidth="30" showlinemarkers="False">
                              <Fill color="SpringGreen"></Fill>
                              <DataLabels visible="True" font="Verdana, 6pt">
                                <border color="Transparent"></border>
                                <background color="Transparent"></background>
                              </DataLabels>
                              <Shadow offsety="1" offsetx="2" visible="True"></Shadow>
                            </web:ColumnChart>
                          </Charts>
                          <XTitle stringformat="Center,Near,Character,LineLimit"></XTitle>
                          <YAxisFont stringformat="Far,Near,Character,LineLimit" font="Verdana, 8pt"></YAxisFont>
                          <ChartTitle stringformat="Center,Near,Character,LineLimit" font="Verdana, 8pt"></ChartTitle>
                          <XAxisFont stringformat="Center,Near,Character,LineLimit" font="Verdana, 8pt"></XAxisFont>
                          <Legend width="80" font="Verdana, 8pt">
                            <border color="Silver"></border>
                            <background color="Silver"></background>
                          </Legend>
                          <Background centercolor="Transparent" color="Silver"></Background>
                          <YTitle stringformat="Center,Near,Character,LineLimit"></YTitle>
                          <Border endcap="Round" startcap="Round" color="Silver" width="1"></Border>
                          <PlotBackground centercolor="Transparent"></PlotBackground>
					</web:ChartControl></td></tr>
  <tr>
<td align="center" valign="middle"> 
		<img align="middle" src="line.png" width="338" height="1">
		<ASP:Panel id="panMesMostrado" style="POSITION: relative" runat="server" width="278px">

		  <TABLE cellspacing="1" cellpadding="1" width="100%" border="0">
			  <tr>
				<td align="left" style="HEIGHT: 24px">
				  <ASP:Label id="labMesMostrado" runat="server" font-names="Verdana" font-size="Smaller">Mes:</ASP:Label></td>
				<td align="right" style="HEIGHT: 24px"><ASP:DropDownList id="cmbMesMostrado" runat="server" width="200px" autopostback="True"></ASP:DropDownList></td>
			  </tr>
		  </TABLE></ASP:Panel></td></tr>
  <tr>
<td align="center" valign="bottom"> 
		<img align="middle" src="line.png" width="338" height="1">
		 <ASP:Panel id="panProducto" style="POSITION: relative" runat="server" width="278px">

		  <TABLE cellspacing="1" cellpadding="1" width="100%" border="0">
			  <tr>
				<td align="left">
				  <ASP:Label id="labProducto" runat="server" font-names="Verdana" font-size="Smaller">Producto:</ASP:Label></td>
				<td align="right"><ASP:DropDownList id="cmbProducto" runat="server" width="200px" autopostback="True"></ASP:DropDownList></td>
			  </tr>
		  </TABLE></ASP:Panel>
		<ASP:Panel id="panZona" style="POSITION: relative" runat="server" width="278px"  visible="True">

		  <TABLE cellspacing="1" cellpadding="1" width="100%" border="0">
			  <tr>
				<td align="left" style="HEIGHT: 24px">
				  <ASP:Label id="labZona" runat="server" font-names="Verdana" font-size="Smaller">Zona:</ASP:Label></td>
				<td align="right" style="HEIGHT: 24px"><ASP:DropDownList id="cmbZona" runat="server" width="200px" autopostback="True"></ASP:DropDownList></td>
			  </tr>
		  </TABLE></ASP:Panel>
		<ASP:Panel id="panTecnico" style="POSITION: relative" runat="server" width="278px">

		  <TABLE cellspacing="1" cellpadding="1" width="100%" border="0">
			  <tr>
				<td align="left" style="HEIGHT: 24px">
				  <ASP:Label id="labTecnico" runat="server" font-names="Verdana" font-size="Smaller">Técnico:</ASP:Label></td>
				<td align="right" style="HEIGHT: 24px"><ASP:DropDownList id="cmbTecnico" runat="server" width="200px" autopostback="True"></ASP:DropDownList></td>
			  </tr>
		  </TABLE></ASP:Panel>
		<ASP:Panel id="panCliente" style="POSITION: relative" runat="server" width="278px">

		  <TABLE cellspacing="1" cellpadding="1" width="100%" border="0">
			  <tr>
				<td align="left" style="HEIGHT: 24px">
				  <ASP:Label id="labCliente" runat="server" font-names="Verdana" font-size="Smaller">Cliente:</ASP:Label></td>
				<td align="right" style="HEIGHT: 24px"><ASP:DropDownList id="cmbCliente" runat="server" width="200px" autopostback="True"></ASP:DropDownList></td>
			  </tr>
		  </TABLE></ASP:Panel>
                        <table cellspacing="1" cellpadding="1" width="278" border="0">
                            <tr>
                              <td style="HEIGHT: 24px" align="center">

		  <ASP:CheckBox id="chkSoloEnc" runat="server" text="Sólo encuestados" tooltip="Mostrar sólo los Dependencias que responden la encuesta" textalign="Left" autopostback="True"></ASP:CheckBox></td>
                            </tr>
                        </table></td></tr>   </table>
<tr><td height="100%">

<div id="scroller2" align="left">
						<borland:DBWebGrid id="grdContesta" runat="server" width="100%" font-size="XX-Small" allowpaging="False" borderstyle="Outset" borderwidth="3px" backcolor="LightGray" bordercolor="Silver" cellpadding="1" dbdatasource="<%# dsWeb %>" readonly="True" selectedindex="0" autogeneratecolumns="False" datasource="<%# ds %>" tablename="contesta" allowsorting="True" font-names="Verdana" horizontalalign="Left" handlecolumnevents="False" handlepagingevents="False">
                        <PagerStyle horizontalalign="Right" forecolor="Black" mode="NumericPages"></PagerStyle>
                        <AlternatingItemStyle backcolor="Silver"></AlternatingItemStyle>
                        <SelectedItemStyle backcolor="Silver"></SelectedItemStyle>
                        <ItemStyle forecolor="White" backcolor="Silver"></ItemStyle>
                        <HeaderStyle font-size="XX-Small" font-names="Verdana" font-bold="True" horizontalalign="Center" forecolor="Black" verticalalign="Middle" backcolor="Gray"></HeaderStyle>
                        <Columns>
                          <ASP:HyperLinkColumn datanavigateurlfield="req" datanavigateurlformatstring="javascript:ticketsReq('{0}')" datatextfield="txtDescrip" sortexpression="txtDescrip" headertext="Requiriente"></ASP:HyperLinkColumn>
                          <ASP:BoundColumn datafield="cli_descri" sortexpression="cli_descri" headertext="Cliente" dataformatstring="&lt;a href=&quot;javascript:RecargaFiltroCliente('{0}')&quot;&gt;{0}&lt;/a&gt;">
                            <ItemStyle font-size="XX-Small"></ItemStyle>
                          </ASP:BoundColumn>
                          <ASP:BoundColumn datafield="usu_nombre" sortexpression="usu_nombre" readonly="True" headertext="T&#233;cnico" dataformatstring="&lt;a href=&quot;javascript:RecargaFiltroTecnico('{0}')&quot;&gt;{0}&lt;/a&gt;">
                            <HeaderStyle horizontalalign="Center" verticalalign="Middle"></HeaderStyle>
                            <ItemStyle font-size="XX-Small" horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                          </ASP:BoundColumn>
                          <ASP:BoundColumn datafield="NombreZona" sortexpression="NombreZona" readonly="True" headertext="Zona" dataformatstring="&lt;a href=&quot;javascript:RecargaFiltroZona('{0}')&quot;&gt;{0}&lt;/a&gt;">
                            <HeaderStyle horizontalalign="Center" verticalalign="Middle"></HeaderStyle>
                            <ItemStyle font-size="XX-Small" horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                          </ASP:BoundColumn>
                          <ASP:BoundColumn datafield="NombreProducto" sortexpression="NombreProducto" readonly="True" headertext="Producto" dataformatstring="&lt;a href=&quot;javascript:RecargaFiltroProducto('{0}')&quot;&gt;{0}&lt;/a&gt;">
                            <HeaderStyle horizontalalign="Center" verticalalign="Middle"></HeaderStyle>
                            <ItemStyle font-size="XX-Small" horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                          </ASP:BoundColumn>
                          <ASP:HyperLinkColumn datanavigateurlfield="req" datanavigateurlformatstring="javascript:muestraEncReq('{0}')" datatextfield="Indicador" sortexpression="indinum" headertext="S" datatextformatstring="{0}%"></ASP:HyperLinkColumn>
                          <ASP:TemplateColumn>
                            <ItemStyle horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                            <ItemTemplate>
								<asp:HyperLink borderwidth="0px" borderstyle="None" cssclass="sinborde" imageurl='<%# DataBinder.Eval(Container, "DataItem.tendencia") %>' runat="server" navigateurl='<%# DataBinder.Eval(Container, "DataItem.reqenc") %>'></asp:HyperLink>
                            </ItemTemplate>
                          </ASP:TemplateColumn>
                          <ASP:BoundColumn datafield="fecRespuesta" sortexpression="fecRespuesta" readonly="True" headertext="Fec" dataformatstring="{0:dd/MMM HH:mm}">
                            <ItemStyle font-size="XX-Small" horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                          </ASP:BoundColumn>
                          <ASP:BoundColumn datafield="InicioMes" sortexpression="InicioMes" headertext="Ini">
                            <ItemStyle horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                            <FooterStyle font-bold="True" horizontalalign="Center"></FooterStyle>
                          </ASP:BoundColumn>
                          <ASP:BoundColumn datafield="Recibidas" sortexpression="Recibidas" headertext="Rec">
                            <ItemStyle horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                            <FooterStyle font-bold="True" horizontalalign="Center"></FooterStyle>
                          </ASP:BoundColumn>
                          <ASP:BoundColumn datafield="Solucionadas" sortexpression="Solucionadas" headertext="Sol">
                            <ItemStyle horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                            <FooterStyle font-bold="True" horizontalalign="Center"></FooterStyle>
                          </ASP:BoundColumn>
                          <ASP:BoundColumn datafield="Pendientes" sortexpression="Pendientes" headertext="Fin">
                            <ItemStyle horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                            <FooterStyle font-bold="True" horizontalalign="Center"></FooterStyle>
                          </ASP:BoundColumn>
                          <ASP:BoundColumn datafield="SolSLA" sortexpression="SolSLA" headertext="SLA">
                            <ItemStyle horizontalalign="Center" verticalalign="Middle"></ItemStyle>
                            <FooterStyle font-bold="True" horizontalalign="Center"></FooterStyle>
                          </ASP:BoundColumn>
                        </Columns></borland:DBWebGrid>
</div></td></tr>
                    </form></tbody></table></td></tr></tbody></table></div>
  </body>
</html>
