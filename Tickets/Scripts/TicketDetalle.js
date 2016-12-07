var Principal = window.parent;
var tic;
var transf;
var texto;

function TicketDetallePageLoad() {
    document.getElementById("rgrEventos_GridData").style.height = "auto";
    //            alert("sera doble?");
    tic = document.getElementById("hfCodigo").value;
    //            var ralp = $find("ralpInvisible");
    //            if (!ralp.show())
    //                ralp.show("rpAsunto");
    evebind();
    $("#aDescargaTodos").attr("href", "/Acciones/Adjunto.aspx?tar=" + tic);
}

function tbClick(s, a) {
    switch (a.get_item().get_value()) {
        case "editar":
            $find("RadToolBar1").findItemByValue("guardar").enable();
            $find("RadToolBar1").findItemByValue("editar").disable();
            $("#tbxAsunto").removeAttr("disabled");
            $("#rtbDetalle").removeAttr("disabled");
            $("#rtbSolucion").removeAttr("disabled");
            break;
        case "guardar":
            //Principal.estampaInputs(document);
            $("#tbxAsunto").attr("disabled", "disabled");
            $("#rtbDetalle").attr("disabled", "disabled");
            $("#rtbSolucion").attr("disabled", "disabled");
            //                    alert("Edicion(" + tic + ", " + frames[0].getAsunto() + ", " + frames[0].getDetalle() + ", " + frames[0].getDatos() + ", " + Principal.datosusuario.co + ")");
            Principal.CustomerCare.Servicio.Edicion(tic, $("#tbxAsunto").val(), $("#rtbDetalle").val(), "{}", Principal.datosusuario.co, editada, error);
            break;
        case "adjuntar":
            var vAdj = Principal.radopen("Acciones/Adjuntar.aspx?inc=" + tic, "Adjuntar");
            vAdj.setSize(470, 400);
            break;
        case "validacion":
            var win = Principal.radopen("Acciones/Accion.aspx?acc=solval&cod=" + tic + "&sen=" + tic, Principal.serverVars.tituloVentanaSolicitarValidacion);
            win.setSize(520, 300);
            break;
        case "validar":
            var win = Principal.radopen("Acciones/Accion.aspx?acc=val&cod=" + tic + "&sen=" + tic, Principal.serverVars.tituloVentanaValidar);
            win.setSize(520, 300);
            break;
        case "rechazar":
            var win = Principal.radopen("Acciones/Accion.aspx?acc=rec&cod=" + tic + "&sen=" + tic, "Rechazar " + Principal.serverVars.ticket + " no. " + tic);
            win.setSize(520, 300);
            break;
        case "cerrar":
            var win = Principal.radopen("Acciones/Accion.aspx?acc=cer&cod=" + tic + "&sen=" + tic, "Cerrar " + Principal.serverVars.ticket + " no. " + tic);
            win.setSize(520, 300);
            break;
        case "imprimir":
            Clickheretoprint();
            //print();
            break;
    }
}

//Agregado por Andrés Lara Maldonado al 16/010/2014 para impresiones
function Clickheretoprint() {
    var disp_setting = "toolbar=yes,location=no,directories=yes,menubar=yes,";
    disp_setting += "scrollbars=yes,width=650, height=600, left=100, top=25";
 //  console.log(document.getElementById("RAD_SPLITTER_PANE_CONTENT_RadPane2").innerHTML);


   // <div id="RAD_SPLITTER_PANE_CONTENT_RadPane2" style="width: 785px; height: 272px; overflow: auto;">
         
    var strDetalle = "";
    var eachLine = $("#rtbDetalle").val().split('\n');
    for (var i = 0, l = eachLine.length; i < l; i++) {
        strDetalle = strDetalle + eachLine[i] + "<br/>";
    }
    var strSolucion = "";
    eachLine = $("#rtbSolucion").val().split('\n');
    for (var i = 0, l = eachLine.length; i < l; i++) {
        strSolucion = strSolucion + eachLine[i] + "<br/>";
    }
    

    var content_vlue = //'<div class="principal">' + document.getElementById("rpAsunto").innerHTML +
    '<div class="container">  ' +
	' <div><div class="inner"><div class="tripol"><div class="image image-full"><img src="images/ccare.png"/></div></div><div class="ticket">' + document.getElementById("rtbTitulo_i0_lblTitulo").innerText + '</div><div class="tripor"><div class="image imaged-full"><img src="images/cibhor.png"/></div></div></div>  ' +
	' <div>  ' +
	'   <div class="inner"> ' +
	'      <div class="principal"> ' +
	'        <span id="idAsunto" style="font-weight:bold;">Asunto:</span> ' +
	'        <span id="idAsuntoDesc" style="">' + $("#tbxAsunto").val() + '</span> <br/> ' +
	'        <span id="idDetalle" style="font-weight:bold;">Detalle:</span> ' +
	'        <span id="idDetalleDesc" style="">' + strDetalle + '<br/></span> <br/> ' +
	'        <span id="idSolucion" style="font-weight:bold;">Soluci&oacute;n:</span> ' +
	'        <span id="idSolucionDesc" style="">' + strSolucion + '</span></div> ' +
	'         ' +
	'      <div class="fechas"> ' +
	'        <span id="idInicio" style="font-weight:bold;">Inicio:</span> ' +
	'        <span id="idInicioDesc" style="">' + document.getElementById("rtbLimite_i0_rtbIni").innerText + '</span> <br/> ' +
	'        <span id="idClausura" style="font-weight:bold;">Clausura:</span> ' +
	'        <span id="idClausuraDesc" style="">' + document.getElementById("rtbLimite_i0_rtbCl").innerText + '</span> <br/> ' +
	'        <span id="idLimie" style="font-weight:bold;">L&iacute;mite:</span> ' +
	'        <span id="idLimieDesc" style="">' + document.getElementById("rtbLimite_i0_rtbLimi").innerText + '</span> <br/> ' +
	'        <span id="idTipificacion" style="font-weight:bold;">Tipificiaci&oacute;n:</span> ' +
	'        <span id="idTipificacionDesc" style="">' + document.getElementById("rtbTipificacion_i1_rtbTipi").value + '</span> <br/> ' +
	'        <span id="idResponsable" style="font-weight:bold;">Responsable:</span> ' +
	'        <span id="idResponsableDesc" style="">' + document.getElementById("rtbResponsable_i1_rtbResp").value + '</span> <br/> ' +
	'        <span id="idSolicitante" style="font-weight:bold;">Solicitante:</span> ' +
	'        <span id="idSolicitanteDesc" style="">' + document.getElementById("rtbResponsable_i4_rtbSoli").value + '</span></div></div> ' +
	'     </div> ' +
	'   <div class="inner"> ' +
	'      <div class="eventos"> ' + document.getElementById("RAD_SPLITTER_PANE_CONTENT_RadPane1").innerHTML +
	'      </div> ' +
	'       ' +

	'      <div class="traza">' + document.getElementById("RAD_SPLITTER_PANE_CONTENT_RadPane2").innerHTML +
	'   </div> ' +
	'   </div> ' +
	'   <div class="inner"><div class="footer">Cibern&eacute;tica de M&eacute;xico - Inform&aacute;tica para la salud - 1992-2014<br> ' +
	'   <script languaje="JavaScript"> ' +
	'var mydate=new Date() ' +
	'var year=mydate.getYear() ' +
	'if (year < 1000) ' +
	'year+=1900 ' +
	'var day=mydate.getDay() ' +
	'var month=mydate.getMonth() ' +
	'var daym=mydate.getDate() ' +
	'if (daym<10) ' +
	'daym="0"+daym ' +
	'var dayarray=new Array ' +
	'("Domingo","Lunes","Martes","Miercoles","Jueves","Viernes","Sabado") ' +
	'var montharray=new Array("Enero","Febrero","Marzo","Abril","Mayo","Junio","Julio","Agosto", ' +
	'"Septiembre","Octubre","Noviembre","Diciembre") ' +
	'document.write("<font size=\'0.7em\'>"+dayarray[day]+" "+daym+" de "+montharray[month] ' +
	'+" de "+year+"</font>") ' +
	'</script></div></div> ' +
	' </div> ' +
	' <div> ' +
	' </div> ';

    var docprint = window.open("", "", disp_setting);
    docprint.document.open();
    docprint.document.write('<html><head>');
    docprint.document.write('<meta charset="UFT-8">');
    docprint.document.write('<title> &middot; Impresi&oacute;n de Incidencia &middot; </title>');
    docprint.document.write('<link rel="stylesheet" href="Estilos/style.css" type="text/css" media="screen">');
    docprint.document.write('<link rel="stylesheet" href="Estilos/style.css" type="text/css" media="print" />');
    docprint.document.write('<style class="cssdeck">');
    docprint.document.write('body {    background-color: #ffffff;    padding-top: 25px;}');
    docprint.document.write('</style>');
    docprint.document.write('</head><body onLoad="self.print()"><center>');
    docprint.document.write(content_vlue);
    docprint.document.write('</center></body></html>');
    docprint.document.close();
    docprint.focus();
}

function repactar(s, a) {
    if (a.get_item().get_value() == "Repactar") {
        var win = Principal.radopen("Acciones/Repactar.aspx?cod=" + tic + "&sen=" + tic, "Repactar " + Principal.serverVars.ticket + " " + tic);
        win.setSize(520, 300);
    }
}

function editada(res) {
    $find("RadToolBar1").findItemByValue("guardar").disable();
    $find("RadToolBar1").findItemByValue("editar").enable();
    evebind();
}

function error(e) { }

function evebind() {
    //            alert("ObtieneEventos(" + tic + ")");
    if (tic.length > 0) {
        Principal.CustomerCare.Servicio.ObtieneEventos(tic, Principal.datosusuario.co, eventosobtenidos, Principal.error);
    }
}

function eventosobtenidos(res, args) {
    var table = $find("rgrEventos").get_masterTableView();
    var event = top.JSON.parse(res)
    table.set_dataSource(event);
    table.dataBind();
}


function cambiaSol(sen, args) {
    args.set_cancel(true);
    var item = args.get_item();
    transf = item.get_text();
    Principal.CustomerCare.Servicio.CambiaSol(tic, item.get_value(), Principal.datosusuario.co, solicitantecambiado, Principal.error);
}

function solicitantecambiado(res, args) {
    $find("rcmSolicitante").hide();
    $find("rtbSolicitante").findItemByValue("rtbSoli").findControl("rtbSoli").set_value(transf);
}

function rtbSolicitanteClicking(sen, args) {
    if (args.get_item().get_value() == "Cambiar") {
        $find("rcmSolicitante").showAt(500, 300);
    }
    args.set_cancel(true);
}

function showMenu(e) {
    //alert("orale");
    var contextMenu = Principal.$find("rcmTipificacion");
    var tempX = 0
    var tempY = 0
    if (Principal.BrowserDetect.browser == "Explorer") { // grab the x-y pos.s if browser is IE
        tempX = event.clientX + document.body.scrollLeft
        tempY = event.clientY + document.body.scrollTop
    } else {  // grab the x-y pos.s if browser is NS
        tempX = e.pageX
        tempY = e.pageY
    }
    // catch possible negative values in NS4
    if (tempX < 0) { tempX = 0 }
    if (tempY < 0) { tempY = 0 }
    var el = top.getSelectedTabWindow().frameElement.offsetParent;
    while (el) {
        tempX += el.offsetLeft;
        tempY += el.offsetTop;
        el = el.offsetParent;
    }
    Principal.rcmTipificacion_opener = "tipificardet";
    contextMenu.showAt(tempX, tempY);
    $telerik.cancelRawEvent(e);
}

function rtbTipi_Click(s, a) {
    if (a.get_item.get_value = "Tipificar") {
        if (!e) var e = window.event;
        showMenu(e);
        e.cancelBubble = true;
        if (e.stopPropagation) e.stopPropagation();
    }
}

function rtbShowDetalle_Click(s, a) {
    if (a.get_item.get_value = "ShowDetalle") {
        $(document).ready(function () {
            $("#modalDetalle").reveal();
        });
        console.log(document.getElementById("rtbDetalle").value);
    }
}

function escribirLabel(sender, args) {
    texto = ' --> ?';
    var item = args.get_item();
    var codigo = item.get_value();
    var n = item.get_level();
    if (codigo.substring(0, 1) == 'h') {
        texto = '';
        codigo = codigo.substring(1)
    }
    for (i = 0; i <= n; i++) {
        texto = ' --> ' + item.get_text() + texto;
        item = item.get_parent();
    }
    texto = texto.substring(4);
    var tipi = codigo;
    $find("loading").show("rsPrincipal");
    Principal.CustomerCare.Servicio.TipificarTicket(tic, tipi, Tipificada, error);
}

function Tipificada(a, b) {
    $find("loading").hide("rsPrincipal");
    var textbox = $find("rtbTipificacion").findItemByText("Button 1").findControl("rtbTipi");
    textbox.set_value(texto);
}