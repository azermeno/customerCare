<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerCare.aspx.cs" Inherits="CustomerCare.CustomerCare" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="application-name" content="CustomerCare" />
    <meta name="description" content="Sistema integral de tickets y atención al cliente" />
    <title>CustomerCare</title>
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="favicon.ico" rel="icon" type="image/x-icon" />

    <%--<script type="text/javascript">
        function tam_fram() {
            if (window.innerHeight) {
                //navegadores basados en mozilla 
                espacio_iframe = window.innerHeight - 110
            } else {
                if (document.body.clientHeight) {
                    //Navegadores basados en IExplorer, es que no tengo innerheight 
                    espacio_iframe = document.body.clientHeight - 110
                } else {
                    //otros navegadores 
                    espacio_iframe = 478
                }
            }
            var ifram = document.getElementsByTagName("iframe");//.height = espacio_iframe;
            for (var i = 0 ; i < ifram.length; i++) {
                ifram[i].height = espacio_iframe;
            }
            //document.write('<iframe frameborder="0" src="recursos/PDFs/AntecedentesRevolcucion.pdf" class="pdf" height="' + espacio_iframe + '">')

        }

        onresize = tam_fram();
        onload = tam_fram();
    </script>--%>
</head>
<body onresize="tam_fram();" onclick="tam_fram();" onchange="tam_fram();">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hddUsuario" runat="server" />
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <%--
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />--%>
            </Scripts>
            <Services>
                <asp:ServiceReference InlineScript="True" Path="Servicio.asmx" />
            </Services>
        </telerik:RadScriptManager>
        <telerik:RadWindowManager ID="rwmPrincipal" runat="server" Animation="Fade"
            Behaviors="Resize, Minimize, Close, Move, Reload"
            EnableAriaSupport="True" EnableShadow="True" EnableViewState="False"
            ShowContentDuringLoad="False" OnClientDragStart="WindowDragStart"
            KeepInScreenBounds="True" OnClientResizeStart="WindowDragStart"
            VisibleStatusbar="False">
        </telerik:RadWindowManager>
        <div class="title" id="logoprincipal" runat="server">
            <asp:Image ID="Image1" runat="server" />
        </div>
        <div class="loginDisplay" id="logindisplay" runat="server">
            <img id="semaforo" alt="semaforo" src="Estilos/img_trans.gif" style="width: 15px; height: 32px; background-image: url(<%=ImagesFolder%>semaforo.png); background-position: 15px 0px;" />
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <asp:HyperLink ID="HyperLink1" runat="server"
                NavigateUrl="javascript:Salir()">[Salir]</asp:HyperLink>
        </div>
        <asp:HiddenField ID="hddAccion" Visible="false" runat="server" />
        <table align="center">
            <tr style="height: 30px;" id="encabezado" runat="server">
                <td>
                    <telerik:RadMenu ID="RadMenu1" runat="server" OnClientItemClicked="RadMenu1_ItemClicked">
                        <Items>
                            <telerik:RadMenuItem runat="server" EnableViewState="False" PostBack="False"
                                Text="Nuevo Ticket" ToolTip="Nuevo ticket" Value="nvotick">
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem runat="server" EnableViewState="False" PostBack="False"
                                Text="Estadísticas">
                                <Items>
                                    <telerik:RadMenuItem runat="server" EnableViewState="False" Text="Gráficas" Value="graficas">
                                        <Items>
                                            <telerik:RadMenuItem runat="server" EnableViewState="False" Text="" Value="zonas">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" EnableViewState="false" Text="Comparativo" Value="comparativozonas"></telerik:RadMenuItem>
                                                    <telerik:RadMenuItem runat="server" EnableViewState="false" Text="Histórico" Value="historicozonas"></telerik:RadMenuItem>
                                                </Items>
                                            </telerik:RadMenuItem>
                                            <telerik:RadMenuItem runat="server" Value="Sitios">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Text="Comparativo"
                                                        Value="comparativositios">
                                                    </telerik:RadMenuItem>
                                                </Items>
                                            </telerik:RadMenuItem>
                                            <telerik:RadMenuItem runat="server" Value="Estados" Text="Estados">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Text="Comparativo"
                                                        Value="comparativoestados">
                                                    </telerik:RadMenuItem>
                                                </Items>
                                            </telerik:RadMenuItem>
                                            <telerik:RadMenuItem runat="server" Value="Tipificaciones" Text="Tipificación">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Text="Comparativo"
                                                        Value="comparativotipificaciones">
                                                    </telerik:RadMenuItem>
                                                </Items>
                                            </telerik:RadMenuItem>
                                            <telerik:RadMenuItem runat="server" Value="Conclusiones" Text="Conclusión">
                                                <Items>
                                                    <telerik:RadMenuItem runat="server" Text="Comparativo"
                                                        Value="comparativoconclusiones">
                                                    </telerik:RadMenuItem>
                                                </Items>
                                            </telerik:RadMenuItem>
                                        </Items>
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem runat="server" Value="reportes" Text="Reportes" />
                                    <telerik:RadMenuItem runat="server" Value="encuestas" Text="Encuestas" />
                                    <telerik:RadMenuItem runat="server" Value="mapa" Text="Mapa" />
                                    <telerik:RadMenuItem runat="server" Value="checador" Text="Checador" />
                                    <telerik:RadMenuItem runat="server" Value="checadorI" Text="Registros del Usuario" />
                                </Items>
                            </telerik:RadMenuItem>
                            <telerik:RadMenuItem runat="server" EnableViewState="False" PostBack="False"
                                Text="Administración" Enabled="false">
                                <Items>
                                    <telerik:RadMenuItem runat="server" EnableViewState="False" Text="Formatos" Value="formatos">
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem runat="server" EnableViewState="False" Text="Programadas" Value="programadas">
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem runat="server" EnableViewState="False" Text="Usuarios" Value="usuarios">
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem runat="server" EnableViewState="False" Text="Solicitantes" Value="solicitantes">
                                        <Items>
                                            <telerik:RadMenuItem runat="server" EnableViewState="False" Text="Unidades" Value="unidades">
                                            </telerik:RadMenuItem>
                                        </Items>
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem runat="server" EnableViewState="False" Text="Tipificaciones" Value="tipificaciones">
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem runat="server" EnableViewState="False" Text="Datos Extra" Value="datosextra">
                                    </telerik:RadMenuItem>
                                    <telerik:RadMenuItem runat="server" EnableViewState="False" Text="Perfiles" Value="perfiles">
                                    </telerik:RadMenuItem>
                                </Items>
                            </telerik:RadMenuItem>
                        </Items>
                    </telerik:RadMenu>
                </td>
            </tr>
        </table>
        <telerik:RadAjaxLoadingPanel ID="ralpRelojito" runat="server"
            EnableTheming="True" Transparency="60">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxLoadingPanel ID="ralpGris" runat="server" BackColor="LightGray" EnableTheming="False" Transparency="60">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"
            DefaultLoadingPanelID="ralpRelojito">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rtsPrincipal">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rtsPrincipal" />
                        <telerik:AjaxUpdatedControl ControlID="rmpPrincipal" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadTabStrip ID="rtsPrincipal" runat="server"
            MultiPageID="rmpPrincipal" SelectedIndex="0"
            CssClass="pestañas" OnClientLoad="OnClientLoad">
            <%--<Tabs>
            <telerik:RadTab runat="server" Text="" Selected="True" Value="TablaTickets">
            </telerik:RadTab>
        </Tabs>--%>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage ID="rmpPrincipal" runat="server" SelectedIndex="0"
            Width="100%" Height="100%" RenderSelectedPageOnly="True">
            <%--<telerik:RadPageView ID="rpvTickets" runat="server" 
                    ContentUrl="Tickets/Tickets.aspx">
                </telerik:RadPageView>--%>
        </telerik:RadMultiPage>
        <telerik:RadContextMenu ID="rcmTipificacion" runat="server" DataFieldID="Codigo"
            DataFieldParentID="Padre" DataSourceID="sdsTipificacion" DataTextField="Descripcion"
            OnClientItemClicked="rcmTipificacion_itemClicked" OnItemDataBound="rcmTipificacion_ItemDataBound"
            DataValueField="Codigo" AutoScrollMinimumHeight="350"
            AutoScrollMinimumWidth="350" EnableAutoScroll="True"
            EnableRootItemScroll="True">
            <Targets>
                <telerik:ContextMenuControlTarget ControlID="rtbTipificacion" />
            </Targets>
        </telerik:RadContextMenu>
        <asp:SqlDataSource ID="sdsTipificacion" runat="server"
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>"
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>"
            SelectCommand="SELECT CASE WHEN Hoja=1 THEN 'h' ELSE '' END + CAST(Codigo AS varchar) AS Codigo, Descripcion, CAST(Padre AS varchar) AS Padre, t.Activo FROM Tipificaciones t WHERE t.Activo=1 ORDER BY Descripcion"></asp:SqlDataSource>

        <telerik:RadContextMenu ID="rcmConclusion" runat="server" DataFieldID="Codigo"
            DataFieldParentID="Padre" DataSourceID="sdsConclusion" DataTextField="Descripcion"
            OnClientItemClicked="rcmConclusion_itemClicked" OnItemDataBound="rcmConclusion_ItemDataBound"
            DataValueField="Codigo" AutoScrollMinimumHeight="350"
            AutoScrollMinimumWidth="350" EnableAutoScroll="True"
            EnableRootItemScroll="True">
            <Targets>
                <telerik:ContextMenuControlTarget ControlID="rtbConclusion" />
            </Targets>
        </telerik:RadContextMenu>
        <asp:SqlDataSource ID="sdsConclusion" runat="server"
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>"
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>"
            SelectCommand="SELECT CASE WHEN Hoja=1 THEN 'h' ELSE '' END + CAST(Codigo AS varchar) AS Codigo, Descripcion, CAST(Padre AS varchar) AS Padre FROM Conclusiones ORDER BY Descripcion"></asp:SqlDataSource>


    </form>
</body>
</html>
