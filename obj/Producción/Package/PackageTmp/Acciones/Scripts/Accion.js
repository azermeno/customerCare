$(document).ready(inicio);

var Principal = window.top;
var sender;
var frame;
function inicio() {
    $("#rtbConclusion_wrapper").click(showMenu);
    sender = serverVars.sen;
    switch (sender) {
        case "Tickets":
            frame = Principal.$find("rtsPrincipal").findTabByValue(sender + "").get_pageView().get_element().getElementsByTagName("iframe")[0].contentWindow.frames[0];
            break;
        default:
            frame = Principal.$find("rtsPrincipal").findTabByValue(sender + "").get_pageView().get_element().getElementsByTagName("iframe")[0].contentWindow;
            break;
    }
}

var concl;

function escribirLabel(sender, args) {
    var texto = ' --> ?';
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
    var textbox = $find("rtbConclusion");
    textbox.set_value(texto);
    $("#rtbConclusion_display").text("");
    concl = codigo;
}

function salir() {
    $find("ralp").show("Panel1");
    var res;
    if ($find("rcbResponsable")) {
        res = $find("rcbResponsable").get_value() * 1;
    }
    var pri;
    if ($find("rcbPrioridad")) {
        pri = $find("rcbPrioridad").get_value() * 1;
    }
    if (res == 0) {
        alert("No ha seleccionado un responsable válido");
        return;
    }
    if (pri == 0) {
        alert("No ha seleccionado prioridad");
        return;
    }
        var men = document.getElementById("rtbObs").value;
        var cod = serverVars.cod;
        var usu = Principal.datosusuario.co;
        var est;
        if (serverVars.est)
            est = serverVars.est * 1;
        else
            est = -1;
        switch (serverVars.acc) {
            case "rec":
                Principal.CustomerCare.Servicio.Rechazar(cod, men, usu, realizada, Principal.error);
                break;
            case "esc":
                Principal.CustomerCare.Servicio.Escalar(cod, res, men, usu, realizada, Principal.error);
                break;
            case "cer":
                Principal.CustomerCare.Servicio.Cerrar(cod, concl, men, realizada, Principal.error);
                break;
            case "val":
                Principal.CustomerCare.Servicio.Validar(cod, men, usu, realizada, Principal.error);
                break;
            case "solval":
                Principal.CustomerCare.Servicio.solVal(cod, res, men, usu, realizada, Principal.error);
                break;
            case "pri":
                Principal.CustomerCare.Servicio.Priorizar(cod, pri, men, usu, realizada, Principal.error);
                break;
        }
}

function GetRadWindow() {
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    return oWindow;
}

function realizada(res, e) {
    GetRadWindow().close();
    alert(res);
    $find("ralp").hide("Panel1");
    switch (sender) {
        case "Tickets":
            frame.document.getElementById("hddCodigo").value = "";
            frame.tablabind();
            break;
        default:
            frame.location.reload();
    }
}

function showMenu(e) {
    //alert("orale");
    var contextMenu = Principal.$find("rcmConclusion");
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
    var el = GetRadWindow().get_contentFrame().offsetParent;
    while (el) {
        tempX += el.offsetLeft;
        tempY += el.offsetTop;
        el = el.offsetParent;
    }
    Principal.rcmConclusion_opener = "accion";
    contextMenu.showAt(tempX, tempY);
}

function showMenuK(n) {
    var contextMenu = Principal.$find("rcmConclusion");
    Principal.rcmTipificacion_opener = "nvainc";
    if (n == 9) {
        contextMenu.showAt(80, 255);
        contextMenu.get_items().getItem(0).focus();
    }
}