<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevoTicket.aspx.cs" Inherits="NuevoTicket" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
    <script type="text/javascript">
        function inicial() {
            if (document.getElementById("chkIna").checked == true) {
                document.getElementById("LInicio").style.display = "block";
                //document.getElementById("LLimite").style.display = "block";
                document.getElementById("IniDate").style.display = "block";
                document.getElementById("IniTime").style.display = "block";
                //document.getElementById("rdtpInicio_ClientState").style.display = "block";
                //document.getElementById("rdtpInicio").style.display = "block";
                //document.getElementById("rdtpInicio_wrapper").style.display = "block";
            }
            else {
                document.getElementById("LInicio").style.display = "none";
                document.getElementById("IniDate").style.display = "none";
                document.getElementById("IniTime").style.display = "none";
            }
            if (document.getElementById("chkVal").checked == true) {
            document.getElementById("lblValidador").style.display = "block";
            document.getElementById("rcbValidador").style.display = "block";
            document.getElementById("Validador").style.display = "block";
            document.getElementById("ValueVali").style.display = "block";
            }
            else {
                document.getElementById("lblValidador").style.display = "none";
                document.getElementById("rcbValidador").style.display = "none";
                document.getElementById("Validador").style.display = "none";
                document.getElementById("ValueVali").style.display = "none";
            }
        }

        function inicialResuelta() {
            if (document.getElementById("chkRes").checked == true) {
                document.getElementById("rtbConclusion").style.display = "block";
                document.getElementById("lblConclusion").style.display = "block";
                document.getElementById("lblSolucion").style.display = "block";
                document.getElementById("rtbSolucion").style.display = "block";
                document.getElementById("divConclusion").style.display = "block";
                document.getElementById("divSolucion").style.display = "block";
            }
            else {
                document.getElementById("rtbConclusion").style.display = "none";
                document.getElementById("lblConclusion").style.display = "none";
                document.getElementById("lblSolucion").style.display = "none";
                document.getElementById("rtbSolucion").style.display = "none";
                document.getElementById("divConclusion").style.display = "none";
                document.getElementById("divSolucion").style.display = "none";
            }
        }

        function closeMsg(msg) {
            if (msg !== "Existen campos vacios, intente de nuevo.")
                window.close();
            else
                document.getElementById("dialogo").style.display = "none";
        }
    </script>
    
</head>
<body style="font-family: 'segoe ui', arial, sans-serif;
font-size: 12px;">
    <form id="form1" runat="server">
	<telerik:radscriptmanager ID="RadScriptManager1" runat="server">
		<Scripts>
			<%--Needed for JavaScript IntelliSense in VS2010--%>
			<%--For VS2008 replace RadScriptManager with ScriptManager--%>
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
			<asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
		</Scripts>
	    <Services>
            <asp:ServiceReference Path="/Servicio.asmx" />
        </Services>
        
    

	</telerik:radscriptmanager>
	<%--<telerik:radajaxmanager ID="RadAjaxManager1" runat="server">
	</telerik:radajaxmanager>--%>
 
	<telerik:radskinmanager ID="RadSkinManager1" Runat="server">
	</telerik:radskinmanager>
    <%--<telerik:radajaxloadingpanel ID="RadAjaxLoadingPanel1" Runat="server" 
        Transparency="40">
    </telerik:radajaxloadingpanel>--%>
     <%--<script src="<% = ResolveUrl("~/Scripts/jquery.timepicker.js") %>" type="text/javascript"></script>--%>
        <%--<script src="Scripts/jquery.timepicker.js"  type="text/javascript"></script>--%>
        <%-- <script src="<% = ResolveUrl("~/Scripts/jquery-1.9.1.js") %>" type="text/javascript"></script>
    <script src="<% = ResolveUrl("~/Scripts/jquery-ui.js") %>" type="text/javascript"></script>

    <script type="text/javascript">
               
        $(function () {
            $.datepicker.setDefaults($.datepicker.regional["es-mx"]);
            $("#fecha").datepicker();
            $("#fecha").datepicker("option", "showAnim", "slide");            
            $("#fecha2").datepicker();
            $("#fecha2").datepicker("option", "showAnim", "slide");
            $('#hora').timepicker();
        });
  </script>  
        <script type="text/javascript" src="~/Scripts/jquery.datepair.js"></script>
<script type="text/javascript">
    // initialize input widgets first
    $('#datepairExample .time').timepicker({
        'showDuration': true,
        'timeFormat': 'g:ia'
    });

    $('#datepairExample .date').datepicker({
        'format': 'yyyy-m-d',
        'autoclose': true
    });

    // initialize datepair
    $('#datepairExample').datepair();
</script>  
        <link href="../Estilos/jquery.timepicker.css" rel="stylesheet" />
        <link href="../Estilos/jquery-ui.css" rel="stylesheet" />--%>
        <%--<telerik:radsplitter ID="RadSplitter2" Runat="server" Orientation="Horizontal" 
        Width="100%" LiveResize="True" Height="100%">
        <telerik:RadPane ID="RadPane3" Runat="server" Height="160px" 
                Scrolling="None">--%>
        
            <telerik:RadSplitter ID="RadSplitter1" Runat="server" 
                Width="100%">
                <telerik:RadPane ID="RadPane1" Runat="server" MinWidth="315" 
                    Width="315px" Scrolling="None">
                    <div style="width: 570px" class="yolopuse">
                        <asp:Label ID="lblSolicitante" runat="server" CssClass="label" 
                            Font-Bold="False" Text="Dependencia: "></asp:Label>
                        <br />
                        <telerik:RadComboBox ID="rcbSolicitante" Runat="server" 
                            DataSourceID="sdsSolicitante" DataTextField="Nombre" DataValueField="Codigo" 
                            Filter="Contains" EmptyMessage="Solicitante..."
                            onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible() &amp;&amp; e.get_domEvent().keyCode != 9) sender.showDropDown(); })" 
                            Width="300px" TabIndex="1" MarkFirstMatch="True">
                        </telerik:RadComboBox>
                        <asp:Label ID="Solicitante" runat="server" Text="Label"></asp:Label>
                        <telerik:RadTextBox ID="ValueSol" Runat="server" Text="">
                        </telerik:RadTextBox>
                        <br />
                        <asp:SqlDataSource ID="sdsSolicitante" runat="server" 
                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                            SelectCommand="select Nombre, '1' + cast(codigo as varchar) as Codigo from unidades where activa=1 union select u.Nombre, '4' + cast(u.codigo as varchar) as Codigo from clientes c join usuarios u on c.usuario=u.codigo where u.activo=1 order by nombre">
                        </asp:SqlDataSource>
                    </div>
                    <div class="yolopuse">
                        <asp:Label ID="lblResponsable" runat="server" CssClass="label" 
                            Text="Responsable: "></asp:Label>
                        <br />
                        <telerik:RadComboBox ID="rcbResponsable" Runat="server" 
                            Filter="Contains" EmptyMessage="Responsable..."
                            
                            Width="300px" TabIndex="2" MarkFirstMatch="True" DataTextField="Nombre" DataValueField="Codigo">
                        </telerik:RadComboBox>
                        <%--<telerik:RadComboBox ID="rcbResponsable" Runat="server" 
                            Filter="Contains" EmptyMessage="Responsable..."
                            onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible() &amp;&amp; e.get_domEvent().keyCode != 9) sender.showDropDown(); })" 
                            Width="300px" TabIndex="2" MarkFirstMatch="True">
                        </telerik:RadComboBox>--%>
                        <asp:Label ID="Responsable" runat="server" Text="Label"></asp:Label>
                        <telerik:RadTextBox ID="ValueResp" Runat="server" Text="">
                        </telerik:RadTextBox>
                        <br />
                        <%--<asp:SqlDataSource ID="sdsResponsable" runat="server"  
                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                            SelectCommand="SELECT u.Nombre, u.Codigo FROM Usuarios u join Internos i on u.codigo=i.usuario WHERE (u.Activo = 1) ORDER BY u.Nombre">
                        </asp:SqlDataSource>--%>
                    </div>
                    <div class="yolopuse" id="divTipificacion" runat="server">
                        <asp:Label ID="lblTipificacion" runat="server" CssClass="label" 
                            Text="Tipificación: "></asp:Label>
                        <%--<asp:RequiredFieldValidator ID="rfvTipificar" runat="server" 
                            ControlToValidate="rtbTipificacion" ErrorMessage="RequiredFieldValidator" 
                            ForeColor="#FF3300">Favor de tipificar</asp:RequiredFieldValidator>--%>
                        <br />
                        <telerik:RadTextBox ID="rtbTipificacion" Runat="server" EmptyMessage="Seleccione..." 
                            ReadOnly="True" Width="300px" TabIndex="3" >
                        </telerik:RadTextBox>
                        <%--<telerik:RadContextMenu ID="rcmTipificacion" Runat="server" DataFieldID="Codigo" 
                            DataFieldParentID="Padre" DataSourceID="sdsTipificacion" DataTextField="Descripcion" 
                            onclientitemclicked="escribirLabel" OnItemDataBound="rcmTipificacion_ItemDataBound" 
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
                            
                            
                            
                            SelectCommand="SELECT CASE WHEN Hoja=1 THEN 'h' ELSE '' END + CAST(Codigo AS varchar) AS Codigo, Descripcion, CAST(Padre AS varchar) AS Padre FROM Tipificaciones"></asp:SqlDataSource>--%>
                        
                    </div>
                    <asp:HiddenField ID="hflTipificacion" runat="server" />
                        <div class="yolopuse">
                            <asp:Label ID="lblResueltaIngresar" runat="server" CssClass="label" 
                                Text="Resuelta al ingresar: "></asp:Label>
                            <asp:CheckBox ID="chkRes" runat="server" Enabled="true" OnClick="inicialResuelta()" TabIndex="4"/>&nbsp;
                            <asp:Label ID="lblInactiva" runat="server" CssClass="label" 
                                Text="Inactiva: "></asp:Label>
                            <asp:CheckBox ID="chkIna" runat="server" OnClick="inicial();" TabIndex="7"/>&nbsp;
                            <asp:Label ID="lblValidada" runat="server" CssClass="label" 
                                Text="Validada: "></asp:Label>
                            <asp:CheckBox ID="chkVal" runat="server" OnClick="inicial();" TabIndex="8"/>
                        </div>
                        <div class="yolopuse">
                        <asp:Label ID="lblPrioridad" runat="server" CssClass="label" 
                            Text="Prioridad: "></asp:Label>
                        <br />
                        <telerik:RadComboBox ID="rcbPrioridad" Runat="server" 
                            DataSourceID="sdsPrioridad" DataTextField="Descripcion" DataValueField="Codigo" 
                            Filter="Contains" EmptyMessage="Prioridad..."
                            onclientkeypressing="(function(sender, e){ if (!sender.get_dropDownVisible() &amp;&amp; e.get_domEvent().keyCode != 9) sender.showDropDown(); })" 
                            Width="300px" TabIndex="12" MarkFirstMatch="True">
                        </telerik:RadComboBox>
                        <br />
                        <asp:SqlDataSource ID="sdsPrioridad" runat="server"  
                            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
                            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
                            SelectCommand="SELECT Codigo, Descripcion FROM Severidades ORDER BY Codigo">
                        </asp:SqlDataSource>
                    </div>


                    <div class="yolopuse">
                        <asp:Label runat="server" ID="LInicio"
                            Text="Inicio:" CssClass="invisible">
                        </asp:Label>
                    <asp:TextBox type="date" ID="IniDate" CssClass="elemento invisible dato" runat="server" TabIndex="10" ClientIDMode="Static"></asp:TextBox>
                    <asp:TextBox type="time" ID="IniTime" CssClass="elemento invisible dato" min="00:00:00" max="23:59" runat="server" TabIndex="11" ClientIDMode="Static"></asp:TextBox>
                        </div>
                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="RegularExpressionValidator" ControlToValidate="IniTime" ValidationExpression="/^\d{4}-\d{2}-\d{2} ([0-1]\d|2[0-3]):([0-5]\d):([0-5]\d) |a|p|. m.$/;"></asp:RegularExpressionValidator>--%>
                    <%--<div>--%>
                    <%--<telerik:RadDateTimePicker ID="rdtpInicio" CssClass="elemento invisible" TimeView-Culture="es-MX" Calendar-CultureInfo="es-MX" ClientIDMode="Static" runat="server"></telerik:RadDateTimePicker>--%>
                                
                            <%--<asp:TextBox ID ="fecha" type="fecha" runat="server" placeholder="dd/mm/aaaa"   Font-Size="Medium" ClientIDMode="Static"/>     
                                  <asp:Label ID="asteriscordtpInicio" runat="server" Text="*" Style="color:red" Visible ="false"/>--%>
                <%--<asp:TextBox ID ="fecha2" type="fecha" runat="server" placeholder="dd/mm/aaaa"   Font-Size="Medium" ClientIDMode="Static"/>     
                                  <asp:Label ID="Label1" runat="server" Text="*" Style="color:red" Visible ="false"/>--%>
                    <div class="yolopuse">    
                    <asp:Label runat="server" ID="LLimite"
                            Text="Límite:" style="width:100%; float: left;">
                        </asp:Label>
                    <asp:TextBox type="date" ID="LimDate" CssClass="elemento invisible dato" runat="server" TabIndex="8" ClientIDMode="Static"></asp:TextBox>
                    <asp:TextBox type="time" ID="LimTime" CssClass="elemento invisible dato" min="00:00:00" max="23:59" runat="server" TabIndex="9" ClientIDMode="Static"></asp:TextBox>
                                <%--<telerik:RadDateTimePicker ID="rdtpLimite" Runat="server" CssClass="elemento  invisible" TimeView-Culture="es-MX" Calendar-CultureInfo="es-MX" ClientIDMode="Static">
                                </telerik:RadDateTimePicker>--%>
                        <%--</div>--%>
                        </div>
                    <div class="yolopuse">
                        <asp:Label ID="lblValidador" runat="server" CssClass="invisible" 
                            Text="Validador: "></asp:Label>
                        <br />
                        <telerik:RadComboBox ID="rcbValidador" Runat="server" CssClass="elemento invisible dato" 
                            Filter="Contains" EmptyMessage="Validador..." Width="300px" TabIndex="2" MarkFirstMatch="True" DataTextField="Nombre" DataValueField="Codigo">
                        </telerik:RadComboBox>
                        <asp:Label ID="Validador" runat="server" Text="Label" CssClass="invisible"></asp:Label>
                        <telerik:RadTextBox ID="ValueVali" CssClass="elemento invisible dato" Runat="server" Text="">
                        </telerik:RadTextBox>
                        <br />
                    </div>

                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitBar1" Runat="server">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="RadPane2" Runat="server" Scrolling="None" MinWidth="610">
                    <div class="yolopuse">
                        <asp:Label ID="lblAsunto" runat="server" CssClass="label" Text="Asunto*: "></asp:Label>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="rtbAsunto" ErrorMessage="RequiredFieldValidator" 
                            ForeColor="#FF3300">El asunto es obligatorio</asp:RequiredFieldValidator>--%><br />
                        <telerik:RadTextBox ID="rtbAsunto" Runat="server" Width="600px" 
                            TabIndex="13">
                        </telerik:RadTextBox>
                    </div>
                    <div class="yolopuse">
                        <asp:Label ID="lblDetalle" runat="server" CssClass="label" Text="Detalle: "></asp:Label>
                        <br />
                        <telerik:RadTextBox ID="rtbDetalle" Runat="server" Rows="5" 
                            TextMode="MultiLine" Width="600px" TabIndex="14">
                        </telerik:RadTextBox>
                    </div>
                    <div class="yolopuse rai" id="divConclusion" runat="server">
                        <asp:Label ID="lblConclusion" runat="server" CssClass="label" 
                            Text="Conclusión: "></asp:Label>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="rtbConclusion" ErrorMessage="RequiredFieldValidator" 
                            ForeColor="#FF3300">Conclusión necesaria</asp:RequiredFieldValidator>--%>
                        <br />
                        <telerik:RadTextBox ID="rtbConclusion" Runat="server" EmptyMessage="Seleccione..." 
                            ReadOnly="True" Width="600px" TabIndex="5">
                        </telerik:RadTextBox>
                    </div>
                    <asp:HiddenField ID="hflConclusion" runat="server" />
                    <%--<div class="yolopuse rai">
                                <asp:Label ID="lblResumen" runat="server" CssClass="label" 
                                    Text="Resumen: "></asp:Label>
                                <br />
                                <telerik:RadTextBox ID="rtbResumen" Runat="server" TabIndex="9" 
                                    Width="700px">
                                </telerik:RadTextBox>
                            </div>--%>
                            <div class="yolopuse rai" id="divSolucion">
                                <asp:Label ID="lblSolucion" runat="server" Text="Solución:"></asp:Label>
                            <br />
                                <telerik:RadTextBox ID="rtbSolucion" Runat="server" Width="600px" 
                                    TabIndex="6" Rows="5" TextMode="MultiLine">
                                </telerik:RadTextBox>
                            </div>
                </telerik:RadPane>
            </telerik:RadSplitter>
        
        <%--</telerik:RadPane><telerik:RadSplitBar ID="RadSplitBar2" Runat="server" 
                EnableResize="False"></telerik:RadSplitBar><telerik:RadPane ID="RadPane4" Runat="server" 
                    Scrolling="None" BorderColor="Black" BorderWidth="1px" 
                CssClass="btnGuardar">
                        </telerik:RadPane>
                            <telerik:RadSplitBar ID="RadSplitBar3" Runat="server" 
                CollapseMode="Forward" EnableResize="False" Height="20px" TabIndex="6">
                            </telerik:RadSplitBar>
        <telerik:RadPane ID="RadPane5" Runat="server" MinHeight="0" 
                Scrolling="None" BorderColor="Black" BorderWidth="1px" 
                CssClass="btnGuardar">--%>


            <div class="bottom">
                <div class="elemento">
                    <asp:Button ID="Guardar" runat="server" TabIndex="15" Text="Guardar" CssClass="boton" OnClick="Guardar_Click" OnClientClick='document.getElementById("dialogo").style.display = "inline";' />
                </div>
            </div>
        
        <div runat="server" id="dialogo" class="dialbase">
           <div id="mnsg" class="dialmsg">
                <asp:Label ID="msg" runat="server" Text="Procesando su solicitud..." />
               <div id="areaboton" runat="server" class="areaboton">
                <input type="button" id="Aceptar" value="Aceptar" class="boton aceptar" style="display:inline;"  onclick='closeMsg(document.getElementById("msg").textContent);' />
                   </div>
            </div> 
        </div>

        
        
        <%--
        </telerik:RadPane>
    </telerik:radsplitter>--%>
	<asp:HiddenField ID="HiddenField1" runat="server" />
	<asp:SqlDataSource ID="SqlDataSource1" runat="server"></asp:SqlDataSource>
	<asp:SqlDataSource ID="SqlDataSource2" runat="server"></asp:SqlDataSource>
	</form>
</body>
</html>
