<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ciu.aspx.cs" Inherits="CustomerCare.ciu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="es" lang="es" dir="ltr">



<head> 
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

  <title>Denuncias a Servidores Públicos | Guadalajara - Gobierno Municipal</title>

  <meta http-equiv="Content-Type" content="text/html; charset=utf-8" /> 
<!--[if IE]>
<link type="text/css" rel="stylesheet" media="all" href="/sites/default/themes/ayuntamiento/css/ie.css?S" />
<![endif]--> 
<!--[if lte IE 6]>
<link type="text/css" rel="stylesheet" media="all" href="/sites/default/themes/ayuntamiento/css/ie6.css?S" />
<![endif]--> 
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
        texto = texto.substring(4);
        var textbox = $find("rtbDependencia");
        textbox.set_value(texto);
        document.getElementById('hflDependencia').value = codigo;
        var combo = $find("rtbHechos");
        combo.focus();
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

<body class="not-front not-logged-in node-type-webform page-denuncias-servidores-p-blicos section-denuncias-servidores-p-blicos one-sidebar sidebar-second" style="font-family: Verdana, Arial; font-size: 12pt">

        <div id="content-area">

          <div id="node-1745" class="node node-type-webform node-promoted clearfix"> 
  
  
    <div class="content"> 
    <div> 
<p><span>En este espacio podrás enviar tu denuncia o reconocimiento, misma que será atendida de forma directa por la Oficina de Combate a la Corrupción de la Presidencia Municipal de Guadalajara, la cual dará seguimiento y respuesta a tu solicitud, con base en las facultades establecidas en el&nbsp;<span>Artículo 29 del Reglamento de la Administración Pública Municipal de Guadalajara.</span>&nbsp;</span></p> 
</div> 
<p>&nbsp;</p> 
        <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
<div><fieldset class="webform-component-fieldset" id="webform-component-datos-personales-del-denunciante"><legend>Datos personales del denunciante</legend><div class="webform-component webform-component-textfield" id="webform-component-datos-personales-del-denunciante--nombre"><div class="form-item" id="edit-submitted-datos-personales-del-denunciante-nombre-wrapper"> 
 <label for="edit-submitted-datos-personales-del-denunciante-nombre">Nombre: <span class="form-required" title="Este campo es obligatorio.">*</span></label> 
 <telerik:RadTextBox ID="rtbNombre" runat="server" 
        EmptyMessage="Escriba aquí su nombre" MaxLength="128"
             CssClass="form-text required" Width="325px">
        </telerik:RadTextBox>
</div> 
</div><div class="webform-component webform-component-textfield" id="webform-component-datos-personales-del-denunciante--apellidos"><div class="form-item" id="edit-submitted-datos-personales-del-denunciante-apellidos-wrapper"> 
 <label for="edit-submitted-datos-personales-del-denunciante-apellidos">Apellidos: <span class="form-required" title="Este campo es obligatorio.">*</span></label> 
 <telerik:RadTextBox ID="rtbApellido" runat="server" EmptyMessage="Escriba aquí su apellido" MaxLength="128"
             CssClass="form-text required" Width="325px">
        </telerik:RadTextBox>
        </div> 
</div><div class="webform-component webform-component-textfield" id="webform-component-datos-personales-del-denunciante--calle-y-numero"><div class="form-item" id="edit-submitted-datos-personales-del-denunciante-calle-y-numero-wrapper"> 
 <label for="edit-submitted-datos-personales-del-denunciante-calle-y-numero">Calle y Número: <span class="form-required" title="Este campo es obligatorio.">*</span></label> 
 <telerik:RadTextBox ID="rtbDireccion" runat="server" EmptyMessage="Escriba su calle y número" MaxLength="128"
             CssClass="form-text required" Width="325px">
        </telerik:RadTextBox>
</div> 
</div><div class="webform-component webform-component-textfield" id="webform-component-datos-personales-del-denunciante--colonia"><div class="form-item" id="edit-submitted-datos-personales-del-denunciante-colonia-wrapper"> 
 <label for="edit-submitted-datos-personales-del-denunciante-colonia">Colonia: <span class="form-required" title="Este campo es obligatorio.">*</span></label> 
        <telerik:RadTextBox ID="rtbColonia" runat="server" EmptyMessage="Escriba aquí su colonia" MaxLength="128"
             CssClass="form-text required" Width="325px">
        </telerik:RadTextBox>
</div> 
</div><div class="webform-component webform-component-textfield" id="webform-component-datos-personales-del-denunciante--codigo-postal"><div class="form-item" id="edit-submitted-datos-personales-del-denunciante-codigo-postal-wrapper"> 
 <label for="edit-submitted-datos-personales-del-denunciante-codigo-postal">Código Postal: </label> 
<telerik:RadTextBox ID="rtbCP" runat="server" EmptyMessage="Escriba aquí su código postal" MaxLength="128"
             CssClass="form-text required" Width="325px">
        </telerik:RadTextBox>
        </div> 
</div><div class="webform-component webform-component-textfield" id="webform-component-datos-personales-del-denunciante--telefono"><div class="form-item" id="edit-submitted-datos-personales-del-denunciante-telefono-wrapper"> 
 <label for="edit-submitted-datos-personales-del-denunciante-telefono">Teléfono: <span class="form-required" title="Este campo es obligatorio.">*</span></label> 
<telerik:RadTextBox ID="rtbTelefono" runat="server" EmptyMessage="Escriba aquí su teléfono" MaxLength="128"
             CssClass="form-text required" Width="325px">
        </telerik:RadTextBox>
</div> 
</div><div class="webform-component webform-component-email" id="webform-component-datos-personales-del-denunciante--correo-electronico"><div class="form-item" id="edit-submitted-datos-personales-del-denunciante-correo-electronico-wrapper"> 
 <label for="edit-submitted-datos-personales-del-denunciante-correo-electronico">Correo Electrónico: </label> 
<telerik:RadTextBox ID="rtbCorreo" runat="server" EmptyMessage="Escriba aquí su correo electrónico" MaxLength="128"
             CssClass="form-text required" Width="325px">
        </telerik:RadTextBox>
            </div> 
</div></fieldset> 
<fieldset class="webform-component-fieldset" id="webform-component-datos-del-servidor-publico-denunciado"><legend>Datos del servidor público denunciado</legend><div class="webform-component webform-component-textfield" id="webform-component-datos-del-servidor-publico-denunciado--dependencia-2"><div class="form-item" id="edit-submitted-datos-del-servidor-publico-denunciado-dependencia-2-wrapper"> 
 <label for="edit-submitted-datos-del-servidor-publico-denunciado-dependencia-2">Dependencia:* </label> 
 <telerik:RadTextBox ID="rtbDependencia" Runat="server" 
        EmptyMessage="Haga click aquí." Width="325px" ReadOnly="true"
             CssClass="form-text required">
    </telerik:RadTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
         ErrorMessage="Este campo es obligatorio." ControlToValidate="rtbDependencia" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator><telerik:RadContextMenu ID="rcmTipificacion" 
        Runat="server" DataFieldID="Codigo" 
                            DataFieldParentID="Padre" 
        DataSourceID="sdsTipificacion" DataTextField="Descripcion" 
                            onclientitemclicked="escribirLabel" 
                            Skin="Ayuntamiento" DataValueField="Codigo" AutoScrollMinimumHeight="350" 
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
    </div> 
</div><div class="webform-component webform-component-textfield" id="webform-component-datos-del-servidor-publico-denunciado--nombre-del-servidor-publico"><div class="form-item" id="edit-submitted-datos-del-servidor-publico-denunciado-nombre-del-servidor-publico-wrapper"> 
 <label for="edit-submitted-datos-del-servidor-publico-denunciado-nombre-del-servidor-publico">Nombre del servidor publico: </label> 
 <telerik:RadTextBox ID="rtbFuncionario" runat="server" EmptyMessage="Nombre del funcionario." MaxLength="128"
             CssClass="form-text required" Width="325px">
        </telerik:RadTextBox>
            </div> 
</div><div class="webform-component webform-component-date" id="webform-component-datos-del-servidor-publico-denunciado--fecha-de-la-anomalia"><div class="form-item" id="edit-submitted-datos-del-servidor-publico-denunciado-fecha-de-la-anomalia-wrapper"> 
 <label for="edit-submitted-datos-del-servidor-publico-denunciado-fecha-de-la-anomalia">Fecha de la anomalía: </label> 
 <telerik:RadDatePicker ID="rdpFecha" Runat="server">
    </telerik:RadDatePicker>
</div> 
</div><div class="webform-component webform-component-textarea" id="webform-component-datos-del-servidor-publico-denunciado--narre-los-hechos-en-el-siguiente-espacio"><div class="form-item" id="edit-submitted-datos-del-servidor-publico-denunciado-narre-los-hechos-en-el-siguiente-espacio-wrapper"> 
 <label for="edit-submitted-datos-del-servidor-publico-denunciado-narre-los-hechos-en-el-siguiente-espacio"> Hechos: </label> 
<telerik:RadTextBox ID="rtbHechos" Runat="server" 
        EmptyMessage="Escriba aquí un resumen de los hechos." Width="475px" CssClass="form-textarea">
    </telerik:RadTextBox></div> 
</div>
    <div class="webform-component webform-component-textarea" id="Div1"><div class="form-item" id="Div2"> 
 <label for="edit-submitted-datos-del-servidor-publico-denunciado-detalles"> Detalle: </label> 
 <telerik:RadTextBox ID="rtbDetalle" Runat="server" 
        EmptyMessage="Describa aquí los hechos a detalle." Width="475px"  CssClass="form-textarea"
        Height="200px" TextMode="MultiLine">
    </telerik:RadTextBox></div> 
</div></fieldset> 
<input type="hidden" name="details[sid]" id="edit-details-sid" value=""  /> 
<input type="hidden" name="details[page_num]" id="edit-details-page-num" value="1"  /> 
<input type="hidden" name="details[page_count]" id="edit-details-page-count" value="1"  /> 
<input type="hidden" name="details[finished]" id="edit-details-finished" value="0"  /> 
<input type="hidden" name="form_build_id" id="form-f9eb12ad30ef1d4a44a3fea100fd4156" value="form-f9eb12ad30ef1d4a44a3fea100fd4156"  /> 
<input type="hidden" name="form_id" id="edit-webform-client-form-1745" value="webform_client_form_1745"  /> 
</div>
<telerik:RadButton ID="RadButton1" runat="server" Text="Enviar" OnClick="Guardar" Skin="Ayuntamiento">
    </telerik:RadButton>
        </form>
<div class="field field-type-text field-field-mensaje-inferior"> 
    <div class="field-items"> 
            <div class="field-item odd"> 
                    <p class="MsoNormal"><strong><span>NOTA: Para dar trámite a tu queja, denuncia o reconocimiento, es indispensable que captures correctamente tus datos. En ningún caso el Ayuntamiento de Guadalajara ni la Oficina de Combate a la Corrupción darán a conocer a terceros la información enviada por el usuario.</span></strong></p> 
        </div> 
        </div> 
</div> 
  </div> 
 
  </div> <!-- /.node --> 
        </div>


</body>

</html>

