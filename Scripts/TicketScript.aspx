<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TicketScript.aspx.cs" Inherits="CustomerCare.Scripts.WebForm1" %>

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

var principal = window.parent.parent;
var frame = window.parent.frames[0];
var vioeve = 1;
var viodat = 1;

function jsonToHtml(json) {
    var html = document.createElement('div');
    var dinc = JSON.parse(json);
    for (var nom in dinc) {
        var fieldset = document.createElement('fieldset');
        var legend = document.createElement('legend');
        legend.appendChild(document.createTextNode(nom));
        fieldset.appendChild(legend);
        for (var prop in dinc[nom]) {
            if (typeof dinc[nom][prop] == 'string') {
                var label = document.createElement('label');
                label.setAttribute('for', prop);
                label.appendChild(document.createTextNode(prop + ': '));
                fieldset.appendChild(label);
                var span = document.createElement('span');
                span.setAttribute('id', prop);
                span.appendChild(document.createTextNode(dinc[nom][prop]));
                fieldset.appendChild(span);
            }
            else {
                for (var prop2 in dinc[nom][prop]) {
                    var label = document.createElement('label');
                    label.setAttribute('for', prop2);
                    label.appendChild(document.createTextNode(prop2 + ': '));
                    fieldset.appendChild(label);
                    var span = document.createElement('span');
                    span.setAttribute('id', prop2);
                    span.appendChild(document.createTextNode(dinc[nom][prop][prop2] + '   '));
                    fieldset.appendChild(span);
                }
            }
            fieldset.appendChild(document.createElement('br'));
        }
        html.appendChild(fieldset);
    }
    return html;
}

function datosobtenidos(res, args) {
    var pv = $find("RadMultiPage1").findPageViewByID("RadPageView3").get_element();
    pv.style.backgroundColor="white";
    pv.appendChild(jsonToHtml('{"Datos del Ciudadano":'+res+'}'));
    viodat = 1;
}

function evebind() {
    frame.CustomerCare.Servicio.ObtieneEventos(frame.tic, eventosobtenidos, frame.error);
}

function tabSelected(s, e) {
switch (e.get_tab().get_index()){
    case 1:
        if (vioeve==0)
            evebind();
    break;
    case 2:
        if (viodat==0)
            frame.CustomerCare.Servicio.DatosCodigo(frame.tic, datosobtenidos, frame.error);
    break;
    }
}

function eventosobtenidos(res, args) {
    var table = $find("rgrEventos").get_masterTableView();
    //    alert(res);
    var event = JSON.parse(res)
    table.set_dataSource(event);
    table.dataBind();
    vioeve = 1;
}

function cancelar() {
    var rdc = frame.$find("rdcAcciones");
    var rdccontent = frame.document.getElementById("rdcAcciones_C")
    rdc.set_title("Cancelar <%= ConfigurationManager.AppSettings["ticket"].ToLower() %>");
    rdc.set_width(520);
    frame.document.getElementById("hddCodigo").value = frame.tic;
    xmlhttp.open("GET", "cerrar.aspx?can=si", true);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            rdccontent.innerHTML = xmlhttp.responseText;
    rdc.set_closed(false);
        }
    }
    xmlhttp.send(null);
}

function cerrar() {
    var rdc = frame.$find("rdcAcciones");
    var rdccontent = frame.document.getElementById("rdcAcciones_C")
    rdc.set_title("Cerrar <%= ConfigurationManager.AppSettings["ticket"].ToLower() %>");
    rdc.set_width(520);
    frame.document.getElementById("hddCodigo").value = frame.tic;
    xmlhttp.open("GET", "cerrar.aspx", true);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            rdccontent.innerHTML = xmlhttp.responseText;
    rdc.set_closed(false);
        }
    }
    xmlhttp.send(null);
}

function rechazar() {
    var rdc = frame.$find("rdcAcciones");
    var rdccontent = frame.document.getElementById("rdcAcciones_C")
            rdc.set_title("Rechazar <%= ConfigurationManager.AppSettings["ticket"].ToLower() %> no. " + frame.tic);
    rdc.set_width(520);
    frame.document.getElementById("hddCodigo").value = frame.tic;
    rdccontent.innerHTML = '<iframe src="validar.aspx?cod=' + frame.tic + '&est=6&rec=si" width="100%" frameborder="0" height="220px"/>';
    rdc.set_closed(false);
}

function escalar() {
    var rdc = frame.$find("rdcAcciones");
    var rdccontent = frame.document.getElementById("rdcAcciones_C")
            rdc.set_title("Escalar");
    rdc.set_width(520);
    frame.document.getElementById("hddCodigo").value = frame.tic;
    rdccontent.innerHTML = '<iframe src="validar.aspx?cod=' + frame.tic + '&est=1&esc=si" width="100%" frameborder="0" height="220px"/>';
    rdc.set_closed(false);
}

function validar(est) {
    var rdc = frame.$find("rdcAcciones");
    var rdccontent = frame.document.getElementById("rdcAcciones_C")
    switch (est) {
        case 2:
            rdc.set_title("<%= ConfigurationManager.AppSettings["tituloVentanaSolicitarValidacion"] %>");
            break;
        case 3:
            rdc.set_title("<%= ConfigurationManager.AppSettings["tituloVentanaValidar"] %>");
            break;
    }
    rdc.set_width(520);
    frame.document.getElementById("hddCodigo").value = frame.tic;
    rdccontent.innerHTML = '<iframe src="validar.aspx?cod=' + frame.tic + '&est=' + est + '" width="100%" frameborder="0" height="220px"/>';
    rdc.set_closed(false);
}

function seguimiento() {
    var rdc = frame.$find("rdcAcciones");
    var rdccontent = frame.document.getElementById("rdcAcciones_C")
    rdc.set_title("Seguimiento <%= ConfigurationManager.AppSettings["ticket"].ToLower() %> " + frame.tic);
    rdc.set_width(540);
    frame.document.getElementById("hddCodigo").value = frame.tic;
    xmlhttp.open("GET", "seguimiento.aspx", true);
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4) {
            rdccontent.innerHTML = xmlhttp.responseText;
        }
    }
    xmlhttp.send(null)
    rdc.set_closed(false);
}

function nuevohijo() {
var vnt=principal.$find("rwnNvaInc");
vnt.setUrl("nuevoticket.aspx?dad="+frame.tic)
vnt.set_title("Nuevo ticket dependiente de " + frame.tic)
vnt.show();
}

function verhijos() {
principal.addtab("Tickets.aspx?hijas="+frame.tic,frame.tic+"-Hijos").select();
}

function tipificar() {
    parent.frames[0].$find("rtbTipificar").get_element().style.display="inline";
    frame.document.getElementById("hddCodigo").value = frame.tic;
}

