<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketDetalle.aspx.cs" Inherits="CustomerCare.TicketDetalle" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>


<body onload="TicketDetallePageLoad()" onclick="Principal.$find('rcmTipificacion').hide()">
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfCodigo" runat="server" />
    <asp:HiddenField ID="hfArchivo" runat="server" />
    <asp:HiddenField ID="hfVersion" runat="server" />
    <%--<asp:Button ID="btnGuardar" runat="server" Text="" Width="20px" 
        CssClass="invisible" onclick="btnGuardar_Click"/>--%>

   

    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" Runat="server">
    </telerik:RadSkinManager>
    <telerik:RadAjaxLoadingPanel ID="loading" runat="server" IsSticky="false" />
    <div>
        <!--BEGIN 29/09/2014 Agregado por Andrés LAra para que se muestre el modal -->
        <telerik:RadWindow ID="modalPopup" runat="server" Width="50%" Height="50%" Modal="true">
               <ContentTemplate>
                    <p style="text-align: center; color:black;">
                        <div id="modalText" style="text-align: left; color:black;">
                         </div>
                         
                    </p>
               </ContentTemplate>
          </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
          <script type="text/javascript">
              function togglePopupModality() {
                  var wnd = $find("<%=modalPopup.ClientID %>");
                   wnd.set_modal(!wnd.get_modal());
                   if (!wnd.get_modal()) document.documentElement.focus();
               }
              function showDialogInitially() {
                  var value = 50;
                  var browserWidth = $telerik.$(window).width();
                  var browserHeight = $telerik.$(window).height();
                  var divText = document.getElementById('modalText');
                  var text = document.getElementById('rtbDetalle').value;
                  //console.log(text);
                  var eachLine = text.split('\n');
                  var textHtml = "";
                  for (var i = 0, l = eachLine.length; i < l; i++) {
                      textHtml = textHtml+ eachLine[i] + "<br/>";
                  }
                  //console.log(textHtml);
                  divText.innerHTML = textHtml;
                  var wnd = $find("<%=modalPopup.ClientID %>");
                  wnd.setSize(Math.ceil(browserWidth * value / 100), Math.ceil(browserHeight * value / 100));
                  wnd.show();
                  Sys.Application.remove_load(showDialogInitially);
               }
               //Sys.Application.add_load(showDialogInitially);
          </script>
     </telerik:RadCodeBlock>
        <!--END 29/09/2014 Agregado por Andrés LAra para que se muestre el modal -->
    
        <telerik:RadToolBar ID="RadToolBar1" Runat="server" Width="100%" OnClientButtonClicked="tbClick">
            <Items>
                <telerik:RadToolBarButton runat="server" Text="Editar" Value="editar">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Guardar" Value="guardar" Enabled="false">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Button 9">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Validacion" Value="validacion" Enabled="false">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Validar" Value="validar" Enabled="false">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Rechazar" Value="rechazar" Enabled="false">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Button 9">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Seguimiento" Value="seguimiento" Enabled="false">
                </telerik:RadToolBarButton>
                <%--
                <telerik:RadToolBarSplitButton runat="server"  Enabled="false">
                    <Buttons>
                        <telerik:RadToolBarButton runat="server" Text="Nota Privada" Value="privado" ImageUrl="<%=ImagesFolder %>30x30/seguimiento.png">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Text="Seguimiento Interno" Value="interno" ImageUrl="<%=ImagesFolder %>30x30/seguimiento.png">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Text="Seguimiento Público" Value="publico" ImageUrl="<%=ImagesFolder %>30x30/seguimiento.png">
                        </telerik:RadToolBarButton>
                    </Buttons>
                </telerik:RadToolBarSplitButton>--%>
                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Button 9">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Nuevo derivado" Value="derivado" Enabled="false">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Button 9">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Adjuntar" Value="adjuntar">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Button 9">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Imprimir" Value="imprimir">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Button 9">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Cerrar" Value="cerrar" Enabled="false">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Descarga Todos" Value="desTodos">
                </telerik:RadToolBarButton>
            </Items>
        </telerik:RadToolBar>
    
    </div>
        
                    
                <telerik:RadToolBar ID="rtbTitulo" runat="server" Width="100%">
                    <Items>
                    <telerik:RadToolBarButton runat="server" Text="Button 0">
                            <ItemTemplate>
                            <asp:Label ID="lblTitulo" runat="server" Font-Bold="true" Font-Size="Large" ForeColor="Black">
                            </asp:Label>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
    <telerik:RadSplitter ID="rsPrincipal" Runat="server" Width="100%" 
        Height="100%" HeightOffset="32" Orientation="Horizontal">
        <telerik:RadPane ID="rpSuperior" Runat="server" Scrolling="None">
            <telerik:RadSplitter ID="rsSuperior" runat="server" Orientation="Vertical">
                <telerik:RadPane ID="rpAsunto" runat="server">
                    <telerik:RadToolBar ID="rtbAsunto" runat="server" Width="100%">
                        <Items>
                        <telerik:RadToolBarButton runat="server" Text="Button 0">
                                <ItemTemplate>
                                <asp:Label ID="lblAsunto" Text="Asunto:" runat="server" Font-Bold="true">
                                </asp:Label>
                                </ItemTemplate>
                            </telerik:RadToolBarButton>
                        </Items>
                    </telerik:RadToolBar>
                                <asp:TextBox Width="99.5%" Height="15px" ID="tbxAsunto" runat="server" Enabled="false"></asp:TextBox>
                <telerik:RadToolBar ID="RadToolBar3" runat="server" Width="100%" OnClientButtonClicked="showDialogInitially">
                    <Items>
                    <telerik:RadToolBarButton runat="server" Text="Button 0" >
                            <ItemTemplate>
                            <asp:Label ID="lblAsunto" Text="Detalle:" runat="server" Font-Bold="true"/>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Value="ShowDetalle">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>

                
                  <%--  <telerik:RadToolBar ID="RadToolBar2" runat="server" Width="100%" OnClientButtonClicked="rtbTipi_Click">
                    <Items>
                        <telerik:RadToolBarButton runat="server" Text="Button 0">
                            <ItemTemplate>
                                <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Tipificación:" Width="70px"></asp:Label>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Text="Button 1">
                            <ItemTemplate>
                                <telerik:RadTextBox ID="rtbTipi" Runat="server" Enabled="False" Width="400px">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Value="Tipificar">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>--%>


                        <telerik:RadTextBox Width="100%" ID="rtbDetalle" Runat="server" Rows="5" 
                            TextMode="MultiLine" Height="50%" Enabled="false">
                        </telerik:RadTextBox>
                                <%--<asp:TextBox Width="99.5%" Height="75px" Rows="5" ID="tbxDetalle" runat="server"></asp:TextBox>--%>
                <telerik:RadToolBar ID="RadToolBar4" runat="server" Width="100%">
                    <Items>
                    <telerik:RadToolBarButton runat="server" Text="Button 0">
                            <ItemTemplate>
                            <asp:Label ID="lblAsunto" Text="Solución:" runat="server" Font-Bold="true"/>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
                        <telerik:RadTextBox Width="100%" ID="rtbSolucion" Runat="server" Rows="5" 
                            TextMode="MultiLine" Height="75px" Enabled="false">
                            </telerik:RadTextBox>
                 <%-- <table cellspacing="2" cellpadding="0" width="100%" align="center" border="1" style="border-color:khaki;">
                    <tbody><tr>
                      <td align="center">
                          <table cellspacing="0" cellpadding="1" width="100%" align="center" border="1" bgcolor="#fffacd">  
                              <tbody><tr>
                                  <td rowspan="2" style="HEIGHT: 25px">
                                      <div align="center"><span runat="server" id="lblNoIncid" style="font-family:Arial;font-size:Larger;font-weight:bold;"></span></div>
                                  </td>
                                  <td rowspan="2"><font face="Arial" size="1"><strong id="lblSolicitante" runat="server"></strong></font></td>
                                  <td rowspan="2"><span id="txtSolicitante" style="font-family:Arial;font-size:Smaller;font-weight:bold;" runat="server"></span></td>
                                  <td style="HEIGHT: 25px"><font face="Arial" size="1"><strong id="lblInicio" runat="server"></strong></font></td>
                                  <td style="HEIGHT: 25px"><span id="txtInicio" style="font-family:Arial;font-size:Smaller;"></span></td>
                              </tr>
                              <tr>
                                  <td><font face="Arial" size="1"><strong id="lblFin" runat="server"></strong></font></td>
                                  <td><span id="txtFin" style="font-family:Arial;font-size:Smaller;" runat="server"></span></td>
                              </tr>
                              </tbody></table>
                      </td>
                    </tr>
                    <tr>
                      <td align="center" style="HEIGHT: 105px">
                        <table cellspacing="0" cellpadding="1" width="100%" align="center" bgcolor="#fffacd" border="1">
                            <tbody><tr>
                                <td>
                                  <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                      <tbody><tr><td><font face="Arial" size="1"><strong id="lblAsunto" runat="server"></strong></font><input type="text" id="inAsunto" runat="server" style="color:Black; font-family:Arial; width:90%" readonly="readonly"></td></tr>
                                      <tr>
                                          <td>
                                              <textarea id="inDetalle" cols="1" rows="3" style="color:MidnightBlue;font-family:Arial;font-size:Smaller;width:90%;" readonly="readonly">SOLICITO ALTA DE NOMBRE EN HOJA DE ENTREGA DE EQUIPO. LAP TOP </textarea>
                                          </td>
                                      </tr>
                                  </tbody></table>
                                </td>
                                <td width="60"><font face="arial" size="1"><strong>&nbsp;&nbsp; solucion:</strong></font></td>
                                <td>
                                  <span id="lblSolucion" style="color:MidnightBlue;font-family:Arial;font-size:Smaller;"></span>
                                </td>
                            </tr>
                        </tbody></table>
                      </td>
                    </tr>
                    </tbody></table>--%>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitBar2" runat="server">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="rpEventos" runat="server" MinHeight="520" Width="520">
                <telerik:RadToolBar ID="rtbLimite" runat="server" Width="100%" OnClientButtonClicked="repactar">
                    <Items>
                        <telerik:RadToolBarButton runat="server" Text="Button 0">
                            <ItemTemplate>
                                <asp:Label ID="lblIni" Font-Bold="true" runat="server" Text="Inicio:"></asp:Label>
                                <asp:Label ID="rtbIni" Runat="server" 
                                    Width="110px">
                                </asp:Label>
                                <asp:Label ID="lblCla" Font-Bold="true" runat="server" Text="Clausura:"></asp:Label>
                                <asp:Label ID="rtbCl" Runat="server" 
                                    Width="110px">
                                </asp:Label>
                                <asp:Label ID="lblLim" Font-Bold="true" runat="server" Text="Límite:"></asp:Label>
                                <asp:Label ID="rtbLimi" Runat="server" 
                                    >
                                </asp:Label>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Value="Repactar">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
                <telerik:RadToolBar ID="rtbTipificacion" runat="server" Width="100%" OnClientButtonClicked="rtbTipi_Click">
                    <Items>
                        <telerik:RadToolBarButton runat="server" Text="Button 0">
                            <ItemTemplate>
                                <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Tipificación:" Width="70px"></asp:Label>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Text="Button 1">
                            <ItemTemplate>
                                <telerik:RadTextBox ID="rtbTipi" Runat="server" Enabled="False" Width="400px">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Value="Tipificar">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
                    
                <telerik:RadToolBar ID="rtbResponsable" runat="server" Width="100%">
                    <Items>
                        <telerik:RadToolBarButton runat="server" Text="Button 0">
                            <ItemTemplate>
                                <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Responsable:" Width="70px"></asp:Label>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Text="Button 1">
                            <ItemTemplate>
                                <telerik:RadTextBox ID="rtbResp" Runat="server" Enabled="False">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Value="Escalar"  Enabled="false">
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Text="Button 0">
                            <ItemTemplate>
                                <asp:Label ID="lblSolicitante" Font-Bold="true" runat="server" Text="Solicitante:" Width="70px"></asp:Label>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Text="Button 1" Value="rtbSoli">
                            <ItemTemplate>
                                <telerik:RadTextBox ID="rtbSoli" Runat="server" Enabled="False" 
                                    Width="125px">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" Value="Cambiar" PostBack="False">
                        </telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
                
                <%--<telerik:RadToolBar ID="rtbSolicitante" runat="server" Width="100%" OnClientButtonClicking="rtbSolicitanteClicking">
                    <Items>
                    </Items>
                </telerik:RadToolBar>--%>
                <telerik:RadGrid ID="rgrFormatos" runat="server" AutoGenerateColumns="False" 
                    CellSpacing="0" Culture="es-ES" GridLines="None" Height="100px" Width="500px">
                    <ClientSettings>
                        <Scrolling AllowScroll="True" />
                        <ClientEvents OnCommand="function(){}" />
                        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
                    </ClientSettings>
                    <MasterTableView NoMasterRecordsText="Sin formatos.">
                        <CommandItemSettings ExportToPdfText="Export to Pdf" />
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px" />
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="formato" HeaderText="Formatos" 
                                UniqueName="column" HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" 
                        EnableImageSprites="True">
                    </HeaderContextMenu>
                </telerik:RadGrid>
                    <div id="divAdjuntos">
                    <a id="aDescargaTodos" href="a" >Descargar todos</a>
                <telerik:RadGrid ID="rgrAdjuntos" runat="server" AutoGenerateColumns="False" 
                    GridLines="None" Height="100px" CellSpacing="0" Culture="es-ES">
                    <ClientSettings>
                        <Scrolling AllowScroll="True" />
       <ClientEvents  OnCommand="function(){}" />
                <Scrolling AllowScroll="True" 
                    UseStaticHeaders="True" />
                    </ClientSettings>
                    <MasterTableView NoMasterRecordsText="No hay adjuntos.">
                        <CommandItemSettings ExportToPdfText="Export to Pdf" />
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px" />
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="archivo" HeaderText="Adjuntos" 
                                UniqueName="column" HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" 
                        EnableImageSprites="True">
                    </HeaderContextMenu>
                </telerik:RadGrid></div>
                <telerik:RadContextMenu ID="rcmSolicitante" 
        Runat="server" DataFieldID="Codigo" 
                            DataFieldParentID="Padre" 
        DataSourceID="sdsSolicitante" DataTextField="Descripcion" 
                            Skin="Ayuntamiento" DataValueField="Codigo" AutoScrollMinimumHeight="350" 
                            AutoScrollMinimumWidth="350" EnableAutoScroll="True" 
                            EnableRootItemScroll="True" OnClientItemClicking="cambiaSol">
                            <Targets>
                                <telerik:ContextMenuControlTarget ControlID="rtbSolicitante" />
                            </Targets>
                        </telerik:RadContextMenu>
                        <asp:SqlDataSource ID="sdsSolicitante" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                            
        
        SelectCommand="Select 'Z'+cast(codigo as varchar) as Codigo, nombre as Descripcion, null as Padre from zonas where activa=1
union
Select 'S'+cast(codigo as varchar) as Codigo,nombre as Descripcion,'Z'+cast(Zona as varchar) as Padre from sitios where activo=1
union
select 'U'+cast(codigo as varchar) as Codigo,nombre as Descripcion,'S'+cast(Sitio as varchar) as Padre from unidades where activa=1
union
select 'C'+cast(codigo as varchar) as Codigo,nombre as Descripcion, null as Padre from usuarios where activo=1 and codigo in (select usuario from clientes)"></asp:SqlDataSource>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitBar1" Runat="server">
            </telerik:RadSplitBar>
        <telerik:RadPane ID="rpDerecho" runat="server" Scrolling="None" Width="20%" MinWidth="250">
        
            <telerik:RadSplitter ID="RadSplitter1" runat="server" Orientation="Vertical">
                <telerik:RadPane ID="RadPane1" runat="server">
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
                            <telerik:GridBoundColumn DataField="on" HeaderText="Eventos" 
                                UniqueName="column" HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="fa" HeaderText="Fecha" UniqueName="column1">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" 
                        EnableImageSprites="True">
                    </HeaderContextMenu>
                </telerik:RadGrid>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitBar3" runat="server">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="RadPane2" runat="server" MinHeight="150">
            <telerik:RadGrid ID="rgrTraza" runat="server" AutoGenerateColumns="False" 
                    GridLines="None" DataSourceID="sdsTraza" Culture="es-MX"  CellSpacing="0">
                    <ClientSettings AllowColumnHide="True" EnableAlternatingItems="False">
       <Selecting AllowRowSelect="True" EnableDragToSelectRows="False" />
       <ClientEvents  OnCommand="function(){}" />
                        <Scrolling AllowScroll="True" 
                    UseStaticHeaders="True" />
                <Resizing AllowColumnResize="True" AllowResizeToFit="True" 
                    ClipCellContentOnResize="False" />
       <ClientEvents  OnCommand="function(){}" />
                    </ClientSettings>
            <AlternatingItemStyle BackColor="White" BorderColor="White" BorderStyle="Solid" 
                BorderWidth="1px" Wrap="False" />
                    <MasterTableView NoMasterRecordsText="Vacío">
                        <CommandItemSettings ExportToPdfText="Export to Pdf" />
                        <RowIndicatorColumn>
                            <HeaderStyle Width="20px" />
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn>
                            <HeaderStyle Width="20px" />
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="uo" HeaderText="Traza" 
                                UniqueName="column" HeaderStyle-Font-Bold="true">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="fa" HeaderText="Fecha" UniqueName="column1">
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default" 
                        EnableImageSprites="True">
                    </HeaderContextMenu>
                </telerik:RadGrid>
                <asp:SqlDataSource ID="sdsTraza" runat="server"
                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" ></asp:SqlDataSource>
            </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>

    </telerik:RadSplitter>
    </form>
</body>
</html>
