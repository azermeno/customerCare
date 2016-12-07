<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nuevaincidencia.aspx.cs" Inherits="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Nueva Incidencia</title>
	<telerik:RadStyleSheetManager id="RadStyleSheetManager1" runat="server" />
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/nuevaincidencia.js"></script>
    <link href="Styles/nuevaincidencia.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
	<telerik:RadScriptManager ID="RadScriptManager1" runat="server">
		<Scripts>
			<%--Needed for JavaScript IntelliSense in VS2010--%>
			<%--For VS2008 replace RadScriptManager with ScriptManager--%>
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
		</Scripts>
	    <Services>
            <asp:ServiceReference Path="Servicio.asmx" />
        </Services>
	</telerik:RadScriptManager>
	<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
	</telerik:RadAjaxManager>
 
	<telerik:RadSkinManager ID="RadSkinManager1" Runat="server" Skin="Forest">
	</telerik:RadSkinManager>
	<div>
        <telerik:RadSplitter ID="RadSplitter2" Runat="server" Orientation="Horizontal" 
        Skin="Forest" Width="100%" LiveResize="True" Height="340px">
        <telerik:RadPane ID="RadPane3" Runat="server" Height="155px" 
                Scrolling="None">
            <telerik:RadSplitter ID="RadSplitter1" Runat="server" Skin="Forest" 
                Width="100%">
                <telerik:RadPane ID="RadPane1" Runat="server" MinWidth="315" 
                    Width="315px" Scrolling="None">
                    <div style="width: 570px" class="yolopuse">
                        <asp:Label ID="lblSolicitante" runat="server" CssClass="label" 
                            Font-Bold="False" Text="Solicitante: "></asp:Label>
                        <br />
                        <telerik:RadComboBox ID="rcbSolicitante" Runat="server" 
                            DataSourceID="sdsSolicitante" DataTextField="Nombre" DataValueField="Codigo" 
                            Filter="Contains" 
                            onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible() &amp;&amp; e.get_domEvent().keyCode != 9) sender.showDropDown(); })" 
                            Skin="Forest" Width="300px" TabIndex="1" MarkFirstMatch="True">
                        </telerik:RadComboBox>
                        <asp:SqlDataSource ID="sdsSolicitante" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" 
                            SelectCommand="SELECT  Nombre, Codigo FROM Unidades ORDER BY Nombre">
                        </asp:SqlDataSource>
                    </div>
                    <div class="yolopuse">
                        <asp:Label ID="lblResponsable" runat="server" CssClass="label" 
                            Text="Responsable: "></asp:Label>
                        <br />
                        <telerik:RadComboBox ID="rcbResponsable" Runat="server" 
                            DataSourceID="sdsResponsable" DataTextField="Nombre" DataValueField="Codigo" 
                            Filter="Contains" 
                            onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible() &amp;&amp; e.get_domEvent().keyCode != 9) sender.showDropDown(); })" 
                            Skin="Forest" Width="300px" TabIndex="2" MarkFirstMatch="True">
                        </telerik:RadComboBox>
                        <asp:SqlDataSource ID="sdsResponsable" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" 
                            
                            
                            
                            
                            SelectCommand="SELECT Nombre, Codigo FROM Usuarios WHERE (Activo = 1) ORDER BY Nombre">
                        </asp:SqlDataSource>
                    </div>
                    <div class="yolopuse">
                        <asp:Label ID="lblTipificacion" runat="server" CssClass="label" 
                            Text="Tipificación*: "></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="rtbTipificacion" ErrorMessage="RequiredFieldValidator" 
                            ForeColor="#FF3300">Favor de dar al menos el Grupo</asp:RequiredFieldValidator>
                        <br />
                        <telerik:RadTextBox ID="rtbTipificacion" Runat="server" EmptyMessage="Seleccione..." 
                            ReadOnly="True" Skin="Forest" Width="300px" TabIndex="3">
                        </telerik:RadTextBox>
                        <telerik:RadContextMenu ID="rcmTipificacion" Runat="server" DataFieldID="Codigo" 
                            DataFieldParentID="Padre" DataSourceID="sdsTipificacion" DataTextField="Descripcion" 
                            onclientitemclicked="escribirLabel" OnItemDataBound="rcmTipificacion_ItemDataBound" 
                            Skin="Forest" DataValueField="Codigo" AutoScrollMinimumHeight="350" 
                            AutoScrollMinimumWidth="350" EnableAutoScroll="True" 
                            EnableRootItemScroll="True">
                            <Targets>
                                <telerik:ContextMenuControlTarget ControlID="rtbTipificacion" />
                            </Targets>
                        </telerik:RadContextMenu>
                        <asp:SqlDataSource ID="sdsTipificacion" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
                            ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" 
                            
                            
                            
                            SelectCommand="SELECT CASE WHEN Hoja=1 THEN 'h' ELSE '' END + CAST(Codigo AS varchar) AS Codigo, Descripcion, CAST(Padre AS varchar) AS Padre FROM Tipificaciones"></asp:SqlDataSource>
                        <asp:HiddenField ID="hflTipificacion" runat="server" />
                    </div>
                        <div class="yolopuse">
                            <asp:Label ID="Label1" runat="server" CssClass="label" 
                                Text="Resuelta al ingresar: "></asp:Label>
                            <asp:CheckBox ID="chkRes" runat="server" Enabled="False" TabIndex="8"/>
                        </div>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitBar1" Runat="server">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="RadPane2" Runat="server" Scrolling="None" MinWidth="610">
                    <div class="yolopuse">
                        <asp:Label ID="lblAsunto" runat="server" CssClass="label" Text="Asunto*: "></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="rtbAsunto" ErrorMessage="RequiredFieldValidator" 
                            ForeColor="#FF3300">El asunto es obligatorio</asp:RequiredFieldValidator>
                        <br />
                        <telerik:RadTextBox ID="rtbAsunto" Runat="server" Skin="Forest" Width="600px" 
                            TabIndex="4">
                        </telerik:RadTextBox>
                    </div>
                    <div class="yolopuse">
                        <asp:Label ID="lblDetalle" runat="server" CssClass="label" Text="Detalle: "></asp:Label>
                        <br />
                        <telerik:RadTextBox ID="rtbDetalle" Runat="server" Rows="5" Skin="Forest" 
                            TextMode="MultiLine" Width="600px" TabIndex="5">
                        </telerik:RadTextBox>
                    </div>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </telerik:RadPane>
                        <telerik:RadSplitBar ID="RadSplitBar2" Runat="server" 
                EnableResize="False">
                        </telerik:RadSplitBar>
                        <telerik:RadPane ID="RadPane4" Runat="server" 
                    Scrolling="None" Collapsed="True" Height="140px">
                            <div class="yolopuse">
                                <asp:Label ID="lblResumen" runat="server" CssClass="label" 
                                    Text="Resumen: "></asp:Label>
                                <br />
                                <telerik:RadTextBox ID="rtbResumen" Runat="server" Skin="Forest" TabIndex="9" 
                                    Width="700px">
                                </telerik:RadTextBox>
                            </div>
                            <div class="yolopuse">
                                <asp:Label ID="lblSolucion" runat="server" Text="Solución:"></asp:Label>
                            <br />
                                <telerik:RadTextBox ID="rtbSolucion" Runat="server" Skin="Forest" Width="700px" 
                                    TabIndex="10" Rows="4" TextMode="MultiLine">
                                </telerik:RadTextBox>
                            </div>
                        </telerik:RadPane>
                            <telerik:RadSplitBar ID="RadSplitBar3" Runat="server" 
                CollapseMode="Forward" EnableResize="False" Height="20px" TabIndex="6">
                            </telerik:RadSplitBar>
        <telerik:RadPane ID="RadPane5" Runat="server" MinHeight="0" 
                Scrolling="None" BorderColor="Black" BorderWidth="1px" 
                CssClass="btnGuardar">
            <div class="yolopuse bottom">
                <input id="btnGuardar" type="button" value="Guardar" onclick="guardarIncidencia()"/>
            </div>
        </telerik:RadPane>
    </telerik:RadSplitter>
    </div>
	<asp:HiddenField ID="HiddenField1" runat="server" />
	</form>
</body>
</html>
