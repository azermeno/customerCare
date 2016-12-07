<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nuevoticketScript.aspx.cs" Inherits="CustomerCare.Scripts.nuevoticketScript" %>

var x
x = $(document);
x.ready(inicio);


function colapsar() {
    spl = $find("RadSplitter2");
    pan = spl.getPaneByIndex(1);
    if (this.checked)
        pan.expand(1);
    else
        pan.collapse(1);
}

        //rtbTipificacion.Attributes.Add("onclick", "showMenu(event)");
var VPrincipal = window.parent;
function inicio() {
    //var x;
    //x = $("#rtbTipificacion");
    //x.click(showMenu);
    //x.keyup(showMenuK);
    chkRes = document.getElementById("chkRes");
    chkRes.onclick=colapsar;
}

function showMenu(e) {
    //alert("orale");
    var contextMenu = VPrincipal.$find("rcmTipificacion");
    var tempX = 0
    var tempY = 0
        if (VPrincipal.IE) { // grab the x-y pos.s if browser is IE
            tempX = event.clientX + document.body.scrollLeft
            tempY = event.clientY + document.body.scrollTop
        } else {  // grab the x-y pos.s if browser is NS
            tempX = e.pageX
            tempY = e.pageY
        }
        // catch possible negative values in NS4
        if (tempX < 0) { tempX = 0 }
        if (tempY < 0) { tempY = 0 }
        var el = parent.frames["rwnNvaInc"].frameElement.offsetParent;
        while (el) {
            tempX += el.offsetLeft;
            tempY += el.offsetTop;
            el = el.offsetParent;
        }
        VPrincipal.rcmTipificacion_opener = "nvainc";
    contextMenu.showAt(tempX, tempY);
}

function showMenuK(n) {
    var contextMenu = VPrincipal.$find("rcmTipificacion");
    VPrincipal.rcmTipificacion_opener = "nvainc";
    if (n == 9) {
        contextMenu.showAt(80, 255);
        contextMenu.get_items().getItem(0).focus();
    }
}

function escribirLabel(sender, args) {
    var texto = ' => ?';
    var item = args.get_item();
    var codigo = item.get_value();
    var n = item.get_level();
    if (codigo.substring(0,1)=='h') {
        texto = '';
        document.getElementById('chkRes').disabled = false;
        codigo = codigo.substring(1)
    }
    for (i = 0; i <= n; i++) {
        texto = ' => ' + item.get_text() + texto;
        item = item.get_parent();
    }
    texto = texto.substring(4);
    var textbox = $find("rtbTipificacion");
    textbox.set_value(texto);
    document.getElementById('hflTipificacion').value = codigo;
    var combo = $find("rtbAsunto");
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


function querySt(ji) {
    hu = window.location.search.substring(1);
    gy = hu.split("&");
    for (i = 0; i < gy.length; i++) {
        ft = gy[i].split("=");
        if (ft[0] == ji) {
            return ft[1].replace(/%20/g, " ").replace(/%22/g, "\"");
        }
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
    val1 = document.getElementById("RequiredFieldValidator1");
    val2 = document.getElementById("RequiredFieldValidator2");
    ValidatorEnable(val1, true);
    ValidatorEnable(val2, true);
    if (val1.isvalid && val2.isvalid) {
        asu = $find("rtbAsunto").get_value();
        dasu = $find("rtbDetalle").get_value();
        usu = document.getElementById("HiddenField1").value;
        res = $find("rcbResponsable").get_value();
        sol = $find("rcbSolicitante").get_value();
        tip = document.getElementById("hflTipificacion").value;
        if (document.getElementById("chkRes").checked) {
            rin = "si";
            solu = $find("rtbResumen").get_value();
            dsolu = $find("rtbSolucion").get_value();
        }
        else {
            rin = "";
            solu = "";
            dsolu = "";
        }
        $find("RadAjaxLoadingPanel1").show("RadSplitter2");
        CustomerCare.Servicio.NuevoTicket(asu, dasu, usu, res, sol, tip, rin, solu, dsolu, guardada, error);
    }
}

function guardada(res, e) {
    alert("<%= ConfigurationManager.AppSettings["ticket"] %> guardad<%= ConfigurationManager.AppSettings["mascfem"] %> con número " + res);
    ValidatorEnable(document.getElementById("RequiredFieldValidator1"), false);
    ValidatorEnable(document.getElementById("RequiredFieldValidator2"), false);
    $find("rcbResponsable").set_value(document.getElementById("HiddenField1").value);
    $find("rcbSolicitante").clearSelection();
    $find("rtbAsunto").set_value("");
    $find("rtbDetalle").set_value("");
    $find("rtbTipificacion").set_value("");
    $find("rtbSolucion").set_value("");
    $find("rtbResumen").set_value("");
    document.getElementById("chkRes").Checked = false;
    spl = $find("RadSplitter2");
    pan = spl.getPaneByIndex(1);
    pan.collapse(1);
    $find("RadAjaxLoadingPanel1").hide("RadSplitter2");
    GetRadWindow().close();
}

function error(e) {
    alert("Ups! hubo un error: " + e.get_message());
    $find("RadAjaxLoadingPanel1").hide("RadSplitter2");
}


