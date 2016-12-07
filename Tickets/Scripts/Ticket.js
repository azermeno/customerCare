$(document).ready(function () { setTimeout(load, 0) });
var xmlhttp = false;
/*@cc_on@*/
/*@if (@_jscript_version >= 5)
// JScript gives us Conditional compilation, we can cope with old IE versions.
// and security blocked creation of the objects.
try {
    xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
} catch (e) {
    try {
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    } catch (E) {
        xmlhttp = false;
    }
}
@end@*/
if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
    try {
        xmlhttp = new XMLHttpRequest();
    } catch (e) {
        xmlhttp = false;
    }
}
if (!xmlhttp && window.createRequest) {
    try {
        xmlhttp = window.createRequest();
    } catch (e) {
        xmlhttp = false;
    }
}

var Principal = window.parent.parent;
var frame = window.parent.frames[0];
var vioeve = 1;
var viodat = 1;

function datosobtenidos(res, args) {
    var pv = $find("RadMultiPage1").findPageViewByID("RadPageView3").get_element();
    pv.style.backgroundColor = "white";
    pv.appendChild(Principal.jsonToHtml('{"Datos del Ciudadano":' + res + '}'));
    viodat = 1;
}

function evebind() {
    //    alert("ObtieneEventos(" + frame.tics + ")");
    if (frame.$find("RadGrid1").get_selectedItems().length == 1)
        frame.CustomerCare.Servicio.ObtieneEventos(frame.tics, Principal.datosusuario.co, eventosobtenidos, frame.error);
}

function tabSelected(s, e) {
    switch (e.get_tab().get_index()) {
        case 1:
            if (vioeve == 0)
                evebind();
            break;
        case 2:
            if (viodat == 0 && frame.$find("RadGrid1").get_selectedItems().length == 1)
                frame.CustomerCare.Servicio.DatosCodigo(frame.tics, Principal.datosusuario.co, datosobtenidos, frame.error);
            break;
    }
}

function eventosobtenidos(res, args) {
    var table = $find("rgrEventos").get_masterTableView();
    try {
        var event = parent.parent.JSON.parse(res)
    }
    catch (e) {
        Principal.error("Error de sintaxis JSON en Servicio.ObtieneEventos cadena: " + res);
    }
    var imFolder = Principal.serverVars.imagesFolder + "20x20/";
    for (i = 0; i < event.length; i++) {
        var icono;
        switch (1*event[i].te) {
            case 1:
                icono = '<img src="' + imFolder + 'escala.png" style="float:left;">';
                break;
            case 2:
                icono = '<img src="' + imFolder + 'seguimiento.png" style="float:left;">';
                break;
            case 3:
                icono = '<img src="' + imFolder + 'solval.png" style="float:left;">';
                break;
            case 4:
                icono = '<img src="' + imFolder + 'valida.png" style="float:left;">';
                break;
            case 5:
                icono = '<img src="' + imFolder + 'rechaza.png" style="float:left;">';
                break;
            case 6:
                icono = '<img src="' + imFolder + 'seguimiento.png" style="float:left;">';
                break;
            case 7:
                icono = '<img src="' + imFolder + 'seguimiento.png" style="float:left;">';
                break;
            case 8:
                icono = '<img src="' + imFolder + 'edita.png" style="float:left;">';
                break;
            case 9:
                icono = '<img src="' + imFolder + 'cambia.png" style="float:left;">';
                break;
            case 10:
                icono = '<img src="' + imFolder + 'repacta.png" style="float:left;">';
                break;
            case 11:
                icono = '<img src="' + imFolder + 'cancela.png" style="float:left;">';
                break;
            case 12:
                icono = '<img src="' + imFolder + 'Priorizar.png" style="float:left;">';
                break;
        }
        event[i].on = icono + event[i].on;
    }
    table.set_dataSource(event);
    table.dataBind();
    vioeve = 1;
}

function cancelar() {
    var rdc = frame.$find("rdcAcciones");
    var rdccontent = frame.document.getElementById("rdcAcciones_C")
    rdc.set_title("Cancelar " + Principal.serverVars.ticket);
    rdc.set_width(520);
    frame.document.getElementById("hddCodigo").value = frame.tics;
    xmlhttp.open("GET", "/Acciones/cerrar.aspx?can=si", true);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            rdccontent.innerHTML = xmlhttp.responseText;
            rdc.set_closed(false);
        }
    }
    xmlhttp.send(null);
}

function priorizar() {
    var win = Principal.radopen("Acciones/Accion.aspx?acc=pri&cod=" + frame.tics + "&sen=Tickets", "Priorizar " + Principal.serverVars.ticket + " no. " + frame.tickets);
    win.setSize(520, 300);
}

function cerrar() {
    var win = Principal.radopen("Acciones/Accion.aspx?acc=cer&cod=" + frame.tics + "&sen=Tickets", "Cerrar " + Principal.serverVars.ticket + " no. " + frame.tickets);
    win.setSize(520, 300);
}

function rechazar() {
    var win = Principal.radopen("Acciones/Accion.aspx?acc=rec&cod=" + frame.tics + "&sen=Tickets", "Rechazar " + Principal.serverVars.ticket + " no. " + frame.tickets);
    win.setSize(520, 300);
}

function escalar() {
    var win = Principal.radopen("Acciones/Accion.aspx?acc=esc&cod=" + frame.tics + "&sen=Tickets", "Escalar " + Principal.serverVars.ticket + " no. " + frame.tickets);
    win.setSize(520, 300);
}

function validar() {
    var win = Principal.radopen("Acciones/Accion.aspx?acc=val&cod=" + frame.tics + "&sen=Tickets", Principal.serverVars.tituloVentanaValidar);
    win.setSize(520, 300);
}

function solVal() {
    var win = Principal.radopen("Acciones/Accion.aspx?acc=solval&cod=" + frame.tics + "&sen=Tickets", Principal.serverVars.tituloVentanaSolicitarValidacion);
    win.setSize(520, 300);
}

function reprogramar() {
    var win = Principal.radopen("Acciones/Reprogramar.aspx?cod=" + frame.tics + "&sen=" + frame.tics, "Reprogramar " + Principal.serverVars.ticket + " " + frame.tickets);
    win.setSize(520, 300);
}

function proyecto() {
    Principal.addtab(Principal.serverVars.urlproyecto + frame.tics, "Proyecto de la Incidencia " + frame.tics, "Proyecto").select();
}

function tareasProgramas() {
    Principal.addtab(Principal.serverVars.urlproyecto + frame.tics, "Proyecto de la Incidencia " + frame.tics, "Proyecto").select();
}

function seguimiento() {
    var rdc = frame.$find("rdcAcciones");
    var rdccontent = frame.document.getElementById("rdcAcciones_C")
    rdc.set_title("Seguimiento " + Principal.serverVars.ticket + " " + frame.tickets);
    rdc.set_width(540);
    frame.document.getElementById("hddCodigo").value = frame.tics;
    xmlhttp.open("GET", "/Acciones/seguimiento.aspx", true);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            rdccontent.innerHTML = xmlhttp.responseText;
        }
    }
    xmlhttp.send(null)
    rdc.set_closed(false);
}

function nuevoderivado() {
    var vnt = Principal.radopen("Tickets/NuevoTicket.aspx?dad=" + frame.tics, "nvotichij");
    vnt.set_title("Nuev" + Principal.serverVars.mascfem + " " + Principal.serverVars.ticket + " derivad" + Principal.serverVars.mascfem + " de " + frame.tickets)
    vnt.setSize(960, 405);
    vnt.add_beforeClose(Principal.cerrarMenu);
}

function verderivados() {
    Principal.addtab("Tickets/Tickets.aspx?hijas=" + frame.tics, frame.tickets + "-Derivadas", "D"+frame.tics).select();
}

function tipificar() {
    parent.frames[0].$find("rtbTipificar").get_element().style.display = "inline";
    frame.document.getElementById("hddCodigo").value = frame.tics;
}

function repactar() {
    var win = Principal.radopen("Acciones/Repactar.aspx?cod=" + frame.tics + "&sen=" + frame.tics, "Repactar " + Principal.serverVars.ticket + " " + frame.tickets);
    win.setSize(520, 300);
}


function load() {
    $("#rgrEventos_GridData").height("auto");
    $(".accion").hide();
    //    setInterval(tablabindsp, 60000);
}

function exportar() {
    console.log("acá es donde se va a exportar");
}