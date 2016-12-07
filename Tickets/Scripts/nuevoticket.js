var Principal = window.parent;
$(document).ready(inicio);

function rtbClick(s,a){
switch (a.get_item().get_text())
{
case "Guardar":
guardarTicket();
break;
}
}

//function muestrarai() {
//    $(".rai").toggle();
//    var rp = $find("RadPane3");
//    rp.getVarSize() == 160 ? rp.setVarSize(300) : rp.setVarSize(160);
//}

function muestraini() {
    $(".rtbUL>:eq(0)").toggle();
    $(".rtbUL>:eq(1)").toggle();
    $(".rtbUL>:eq(2)").toggle();
    var lblt = $("#lblTipificacion");
    lblt.text() != "Tipificación*: " ? lblt.text("Tipificación*: ") : lblt.text("Tipificación: ");
    }

        //rtbTipificacion.Attributes.Add("onclick", "showMenu(event)");
    function inicio() {
//    $("#chkRes").click(muestrarai);
    $("#chkIna").click(muestraini);
    if (!($("#chkRes:visible")[0])) {
        $(".rtbUL>:eq(3)").toggle();
        $(".rtbUL>:eq(4)").toggle();
        $(".rtbUL>:eq(5)").toggle();
    }
    muestraini();
    $(".rai").toggle();
    setTimeout('$find("RadToolBar1").findItemByText("limite").findControl("rdtpLimite").set_selectedDate(new Date(parent.horaServidor()+1000*60*60*4));', 100)
}

function showMenu(e, menu) {
    //alert("orale");
    var contextMenu = Principal.$find(menu);
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
        var el = top.$find("rwmPrincipal").getActiveWindow().get_contentFrame().offsetParent;
        while (el) {
            tempX += el.offsetLeft;
            tempY += el.offsetTop;
            el = el.offsetParent;
        }
        eval('Principal.' + menu + '_opener = "nvainc"');
    contextMenu.showAt(tempX, tempY);
}

function showMenuK(n, menu) {
    var contextMenu = Principal.$find(menu);
    eval('Principal.'+ menu + '_opener = "nvainc"');
    if (n == 9) {
        contextMenu.showAt(80, 255);
        contextMenu.get_items().getItem(0).focus();
    }
}

function escribirLabel(menu, args) {
    var texto = ' => ?';
    var item = args.get_item();
    var codigo = item.get_value();
    var n = item.get_level();
    var combo;
    switch (menu) {
        case "Conclusion":
            if (codigo.substring(0, 1) == 'h') {
                codigo = codigo.substring(1);
                texto = '';
            }
            combo = $find("rtbSolucion");
            break;
        case "Tipificacion":
            if (codigo.substring(0, 1) == 'h') {
                codigo = codigo.substring(1);
                $("#chkRes").removeAttr("disabled");
                texto = '';
            }
            combo = $find("rtbAsunto");
            break;
    }
    for (i = 0; i <= n; i++) {
        texto = ' => ' + item.get_text() + texto;
        item = item.get_parent();
    }
    texto = texto.substring(4);
    var textbox = $find("rtb" + menu);
    textbox.set_value(texto);
    document.getElementById('hfl' + menu).value = codigo;
    combo.focus();
}

function cerrarMenu(n, menu) {
    var contextMenu = $find(menu);
    var combo = $find("rtbAsunto");
    if (n == 9) {
        contextMenu.hide();
        combo.focus();
    }
}


function querySt(ji) {
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1].replace(/%20/g, " ").replace(/%22/g, "\"");
        }
        return undefined;
    }
}

function selecciona_value(objInput) {

    var valor_input = objInput.value;
    var longitud = valor_input.length;

    if (objInput.setSelectionRange) {
        objInput.focus();
        objInput.setSelectionRange(0, longitud);
    }
    else if (objInput.createTextRange) {
        var range = objInput.createTextRange();
        range.collapse(true);
        range.moveEnd('character', longitud);
        range.moveStart('character', 0);
        range.select();
    }
}

function GetRadWindow()
{
    var oWindow = null;
    if (window.radWindow)
        oWindow = window.radWindow;
    else if (window.frameElement.radWindow)
        oWindow = window.frameElement.radWindow;
    return oWindow;
}

function guardarTicket() {
    var ina = "";
    if ($("#chkIna")[0].checked) 
        ina = "si";
    val1 = document.getElementById("RequiredFieldValidator1");
    ValidatorEnable(val1, true);
    if (Principal.contains(Principal.datosusuario.ps, "16")) {
        val2 = document.getElementById("RequiredFieldValidator2");
        var solicitante = $find("ValueSol").get_value();
        if (solicitante == "")
            ValidatorEnable(val2, true);
    }
    var valido;
    if (solicitante!="") 
        valido = true;
    else {
        if (val1.isvalid && (ina == "si" || !(Principal.contains(Principal.datosusuario.ps, "16")) || val2.isvalid)) 
            valido = true;
        else
            valido = false;
    }
    var lim = $find("RadToolBar1").findItemByText("limite").findControl("rdtpLimite").get_selectedDate();
    var ini = $find("RadToolBar1").findItemByText("inicio").findControl("rdtpInicio").get_selectedDate();
    ini == null ? ini = new Date(0) : 0;
    lim == null ? lim = new Date(0) : 0;
    valido = ina == "si" && ini > lim ? false : valido;
    lim < ini ? alert("El límite debe ser posterior al inicio.") : 0;
    if (valido) {
        var asu = $find("rtbAsunto").get_value();
        var dasu = $find("rtbDetalle").get_value();
        var usu = document.getElementById("HiddenField1").value;
        var res = $find("rcbResponsable").get_value();
        var sol = $find("rcbSolicitante").get_value();
        if(solicitante=="")
            var tip = document.getElementById("hflTipificacion").value;
         else {  
            sol = $find("ValueSol").get_value();
            res = $find("ValueResp").get_value();
        }
        if ($("#chkRes")[0].checked) {
            rin = "si";
            var con = document.getElementById("hflConclusion").value;
            solu = $find("rtbSolucion").get_value();
        }
        else {
            rin = "";
            con = "0";
            solu = "";
        }
        $find("RadAjaxLoadingPanel1").show("RadSplitter2");
        var dad = querySt("dad");
        var pri = 3;
        if ($find("rcbPrioridad"))
            pri = $find("rcbPrioridad").get_value();
        if (dad == undefined) {
            if (solicitante!="") 
                CustomerCare.Servicio.NuevoTicket(asu, dasu, usu, res, sol, "0", 0, con, solu, lim, ina, ini, pri, guardada, error);
            else
                CustomerCare.Servicio.NuevoTicket(asu, dasu, usu, res, sol, tip, rin, con, solu, lim, ina, ini, pri, guardada, error);
        }
        else {
            if (solicitante!="") 
                CustomerCare.Servicio.NuevoTicketDerivado(asu, dasu, usu, res, sol, "0", 0, con, solu, dad, lim, ina, ini, pri, guardada, error);
            else
                CustomerCare.Servicio.NuevoTicketDerivado(asu, dasu, usu, res, sol, tip, rin, con, solu, dad, lim, ina, ini, pri, guardada, error);
        }
    }
}

function guardada(res, e) {
    alert(res);
    ValidatorEnable(document.getElementById("RequiredFieldValidator1"), false);
    var solicitante = $find("ValueSol").get_value();
    if (solicitante=="")
        ValidatorEnable(document.getElementById("RequiredFieldValidator2"), false);
    $find("rcbResponsable").set_value(document.getElementById("HiddenField1").value);
    $find("rcbSolicitante").clearSelection();
    $find("rtbAsunto").set_value("");
    $find("rtbDetalle").set_value("");
    if (solicitante=="") {
        $find("rtbTipificacion").set_value("");
        $find("rtbSolucion").set_value("");
        $find("rtbConclusion").set_value("");
        document.getElementById("chkRes").Checked = false;
        document.getElementById("chkIna").Checked = false;
    }
//    spl = $find("RadSplitter2");
//    pan = spl.getPaneByIndex(1);
//    pan.collapse(1);
    $find("RadAjaxLoadingPanel1").hide("RadSplitter2");
    window.parent.frames[0].frames[0].tablabindsp();
    top.$find("rwmPrincipal").getActiveWindow().close();
}

function error(e) { Principal.error(e); }


