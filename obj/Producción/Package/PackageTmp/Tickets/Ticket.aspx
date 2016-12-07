<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ticket.aspx.cs" Inherits="CustomerCare.TicketPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type='text/javascript' src='../Scripts/jquery-1.7.2.min.js'></script>
	<script type='text/javascript' src='https://www.google.com/jsapi'></script>
    <script src="../Scripts/jquery.reveal.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
                <%--<telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
                    <StyleSheets>
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/TabStrip.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Grid.Ayuntamiento.css" />
                    </StyleSheets>
                </telerik:RadStyleSheetManager>--%>
    <%--<script src="Scripts/Ticket.js?<%=DateTime.Now.Millisecond.ToString()%>" type="text/javascript"></script>--%>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
        <telerik:RadTabStrip ID="RadTabStrip1" runat="server" 
            MultiPageID="RadMultiPage1" SelectedIndex="0" 
        onclienttabselected="tabSelected" CssClass="pestañas">
            <Tabs>
                <telerik:RadTab runat="server" Selected="True" Text="Datos">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Eventos">
                </telerik:RadTab>
                <telerik:RadTab runat="server" Text="Datos Ciudadano" Value="datosExtras" Visible="false">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip> 
        <div id="divImagenes">
    <img src="<%=ImagesFolder%>20x20/cancela.png" alt="Cancelar" title="Cancelar" onclick="cancelar()" id="icCancelar" class="accion"/>
    <img src="<%=ImagesFolder%>20x20/rechaza.png" alt="Rechazar" title="Rechazar" onclick="rechazar()" id="icRechazar" class="accion"/>
    <img src="<%=ImagesFolder%>20x20/valida.png" alt="Validar" title="Validar" onclick="validar()" id="icValidar" class="accion"/>
    <img src="<%=ImagesFolder%>20x20/tipifica.png" alt="Tipificar" title="Tipificar" onclick="tipificar()" id="icTipificar" class="accion"/>
    <img src="<%=ImagesFolder%>20x20/seguimiento.png" alt="Seguimiento" title="Dar seguimiento" onclick="seguimiento()" id="icSeguimiento" class="accion"/>
    <img src="<%=ImagesFolder%>20x20/derivado.png" alt="Nuev<%=ConfigurationManager.AppSettings["mascfem"]%> <%=ConfigurationManager.AppSettings["ticket"].ToLower()%> derivad<%=ConfigurationManager.AppSettings["mascfem"]%>" title="Nuev<%=ConfigurationManager.AppSettings["mascfem"]%> <%=ConfigurationManager.AppSettings["ticket"]%> derivad<%=ConfigurationManager.AppSettings["mascfem"]%>" onclick="nuevoderivado()" id="icDerivado"  class="accion"/>
    <img src="<%=ImagesFolder%>20x20/derivados.png" alt="Ver <%=ConfigurationManager.AppSettings["ticket"].ToLower()%>s derivad<%=ConfigurationManager.AppSettings["mascfem"]%>s" title="Ver <%=ConfigurationManager.AppSettings["ticket"]%>s derivad<%=ConfigurationManager.AppSettings["mascfem"]%>s" onclick="verderivados()" id="icDerivados"  class="accion"/>
    <img src="Images/imprimir2.png" alt="Imprimir" title="Imprimir" onclick="exportar()" id="icImprimir"  class="accion"/>
    <%--    <img src="Images/attach.png" alt="Adjuntar" onclick="adjuntar()" id="icAdjuntar"  class="accion"/>--%>
    <img src="<%=ImagesFolder%>20x20/repacta.png" alt="Repactar" title="Repactar" onclick="repactar()" id="icRepactar"  class="accion"/>
    <img src="<%=ImagesFolder%>20x20/escala.png" alt="Escalar" title="Escalar" onclick="escalar()" id="icEscalar"  class="accion"/>
    <img src="<%=ImagesFolder%>20x20/solval.png" alt="Solicitar Validacion" title="<%=ConfigurationManager.AppSettings["tituloVentanaSolicitarValidacion"] %>" onclick="solVal()" id="icSolVal"  class="accion"/>
    <img src="<%=ImagesFolder%>20x20/cerrar.png" alt="Cerrar" title="Cerrar" onclick="cerrar()" id="icCerrar" class="accion"/>
    <img src="<%=ImagesFolder%>20x20/priorizar.png" alt="Priorizar" title="Priorizar" onclick="priorizar()" id="icPriorizar" class="accion"/>
    <img src="<%=ImagesFolder%>20x20/proyecto.png" alt="Proyecto" title="Ir al proyecto" onclick="proyecto()" id="icProyecto" class="accion"/>
    <img src="<%=ImagesFolder%>20x20/reprogramar.png" alt="Reprogramar" title="Reprogramar Incidencia" onclick="reprogramar()" id="icReprogramar" class="accion"/>
    </div>
        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" Width="100%" 
            SelectedIndex="0">
            <telerik:RadPageView ID="RadPageView1" runat="server">
            <table cellspacing="0" cellpadding="1" width="100%" align="center" 
                    bgcolor="#fffacd" border="1">
	<tr>
    <td rowspan="2" id="tdCodigo" style="width:70px; text-align:center; font-family:Arial; font-size:3; font-weight:bolder;"></td>
                    <td valign="top"><font face="Arial" size="1"><strong>&nbsp;&nbsp; Asunto:</strong></font></td>
                    <td valign="top">
                      <table cellspacing="0" cellpadding="0" width="100%" border="0">
			  <tr>
				<td>
                              <ASP:Label id="lblAsunto" runat="server" forecolor="Blue" font-names="Arial"></ASP:Label></td>
			  </tr>
			  <tr>
				<td>
                              <ASP:Label id="lblDetalle" runat="server" forecolor="MidnightBlue" 
                                  font-names="Arial" font-size="Smaller"></ASP:Label></td>
			  </tr>
                      </table></td>
                    <td valign="top" width="60"><font face="arial" size="1"><strong>&nbsp;&nbsp; Solución:</strong></font></td>
                    <td valign="top">
                      <table cellspacing="0" cellpadding="0" width="100%" border="0">
			  <tr>
				<td>
                              <ASP:Label id="lblSolucion" runat="server" forecolor="Blue" font-names="Arial"></ASP:Label></td>
              
              </tr>
                      </table></td>
	</tr>
	<tr>
                    <td width="60"><font face="Arial" size="1"><strong>&nbsp;&nbsp; Tipificación:</strong></font></td>
					<td id="tdTipificacion">
					  <ASP:Label id="lblTipificacion" runat="server" font-names="Arial" 
                            font-size="Smaller"></ASP:Label></td>
					<td>
                      <p align="center"><font face="Arial">
                          <strong><font size="1">Levantó:</font>
                          </strong>
                        </font>
                      </p></td>
					<td>
					  <ASP:Label id="lblLevanto" runat="server" font-names="Arial" font-size="Smaller"></ASP:Label></td>
	</tr>
              </table>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView2" runat="server">
                <telerik:RadGrid ID="rgrEventos" runat="server" AutoGenerateColumns="False" 
                    GridLines="None" Culture="es-MX"  CellSpacing="0">
                    <ClientSettings AllowColumnHide="True" EnableAlternatingItems="False">
       <Selecting AllowRowSelect="True" EnableDragToSelectRows="False" />
       <ClientEvents  OnCommand="function(){}" />
                        <Scrolling AllowScroll="True" 
                    UseStaticHeaders="True" />
                <Resizing AllowColumnResize="True" AllowResizeToFit="True" 
                    ClipCellContentOnResize="False" />
       <ClientEvents  OnCommand="function(){}" />
                    </ClientSettings>
                    <MasterTableView NoMasterRecordsText="No hay eventos.">
                        <CommandItemSettings ExportToPdfText="Export to Pdf" />
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px" />
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="on" HeaderText="Observación" 
                                UniqueName="column">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="fa" HeaderText="Fecha" UniqueName="column1">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" 
                        EnableImageSprites="True">
                    </HeaderContextMenu>
                </telerik:RadGrid>
            </telerik:RadPageView>
            <telerik:RadPageView ID="RadPageView3" runat="server">
            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </form>
</body>
</html>
