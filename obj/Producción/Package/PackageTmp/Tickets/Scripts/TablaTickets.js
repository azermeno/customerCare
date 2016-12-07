$(document).ready(function () { setTimeout(load, 0) });
var frame = window.parent.frames[1];
var Datos;
var tableView;
var rdc;
var verderivados = false;
var tics = "0";
var tickets = "";
var Principal = window.parent.parent;
var ralp;
var cancelRadGridCommandBind = 0;
var filtroActivo = new Array(7);
var img = new Array(7);
for (var j = 0; j < 7; j++) {
    filtroActivo[j] = false;
}
var tipi;
//var ledFecha = true;
var cerrar = false;
var Cerrar = true;
var Abierto = false;
var hideDropDown = 0;
var filtrando = false;
var rtvSolicitante;
var nodes;
function permiso(n) { return Principal.contains(Principal.datosusuario.ps, "" + n ); }
function cerrado(s, a) { if (filtrando) { tablabind(); filtrando = false; } }
function cambioFiltro(n) { if (filtroActivo[n] && !filtrando) { filtrando = true; } }
function puedesCerrar(s, a) { Abierto = false; setTimeout(function () { if (!Abierto) Cerrar = true; }, 1000); }
function noCierresNada(s, a) { Cerrar = false; Abierto = true; }
function tipiscroll() {
    hideDropDown++;
}
function cerrando(sender, args) {
    if (!cerrar || !Cerrar)
        args.set_cancel(true);
}

function hideDropDowns(items, n) {
    if (hideDropDown == n) {
        cerrar = true;
        var it;
        for (i = 0; i < items.get_count(); i++) {
            it = items.getItem(i);
            if (it._isDropDownItem())
                it.hideDropDown();
        }
        cerrar = false;
        hideDropDown = 0;
    }
}


function onMouseOver(sender, args) {
    var item = args.get_item();
    if (item.get_value != "buscar")
        hideDropDown++;
}


function onMouseOut(sender, args) {
    var item = args.get_item();
    var q = hideDropDown;
    if (item._isDropDownItem() || item._isDropDownChild()) {
        setTimeout(function () { hideDropDowns(sender.get_items(), q); }, 1000);
    }
}

//function ledfecha() {
//    var led = document.getElementById("ledfecha");
//    if (ledFecha) {
//        led.style.backgroundPosition = "9px 0px";
//        ledFecha = false;
//    }
//    else {
//        led.style.backgroundPosition = "0px 0px";
//        ledFecha = true;
//    }
//}



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
    var textbox = $find("rtbTipificar").findItemByText("tipificacion").findControl("RadTextBox1");
    textbox.set_value(texto);
    tipi = codigo;
}

function GuardarTipi(sender, args) {
    var cod = document.getElementById("hddCodigo").value;
    CustomerCare.Servicio.TipificarTicket(cod, tipi, Tipificada, error);
}

function Tipificada(a, b) {
    $find("rtbTipificar").get_element().style.display = "none";
    frame.document.getElementById("lblTipificacion").innerHTML = $find("rtbTipificar").findItemByText("tipificacion").findControl("RadTextBox1").get_value();
    $find("rtbTipificar").findItemByText("tipificacion").findControl("RadTextBox1").set_value("");
    tablabindsp();
}

function buscar(sender, args) {
    if (args.get_keyCode() == 13) {
        tablabind();
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

//var nbind=0;

function clicktoolbar(s, a) {
    var item = a.get_item();
    var items = s.get_items();
    if (item._isDropDownItem()) {
        for (var i = 0; i < items.get_count(); i++) {
            try { items.getItem(i).showDropDown(); }
            catch (e) { }
        }
    }
    switch (item.get_value()) {
        case "checkbox0": cambioFiltro(0);
            break;
        case "checkbox1": cambioFiltro(1);
            break;
        case "checkbox2": cambioFiltro(2);
            break;
        case "checkbox3": cambioFiltro(3);
            break;
        case "checkbox4": cambioFiltro(4);
            break;
        case "guardarFiltro":
            var usu = Principal.datosusuario.co;
            var filtr = filtro();
            filtr = filtr.substr(0, filtr.lastIndexOf('!'));
            ralp.show("RadToolBar1");
            ralp.show("RadGrid1");
            CustomerCare.Servicio.GuardarFiltro(filtr, usu, function (res, arg) {
                alert("Filtro guardado");
                ralp.hide("RadToolBar1");
                ralp.hide("RadGrid1");
            }, error);
            break;

        case "checkbox5": cambioFiltro(6);
            break;
        case "Exportar": Exportar();
            break;
    }
    //    nbind++;
    //    tablabindr(nbind);
    //  }
}

var finalVal = "";
function datosobtenidosL(res, args) {
    try {
       // console.log("El resultado es: "+res);
        DatosL = Principal.JSON.parse(res);
       // setDatos(DatosL);
        //console.log(DatosL);
    }
    catch (e) {
        alert("JSON cadena no valida: " + res);
        return;
    }

    //console.log("estoy en el : " + Cur);
    j = i = 0;
    while (i < DatosL.d.length) {

        var limcla;
        switch (DatosL.d[i].eo) {
            case 1: limcla = DatosL.d[i].le.Key == "" ? "Sin límite" : DatosL.d[i].le.Key; break;
            case 2: limcla = DatosL.d[i].le.Value; break;
            case 3: limcla = DatosL.d[i].le.Value; break;
            default: limcla = DatosL.d[i].ca;
        }

        result = DatosL.d[i].co.replace(/"/g, '""');
        if (result.search(/("|,|\n)/g) >= 0)
            result = '"' + result + '"';
        result = result.replace(',', '');
        finalVal += result + ',';

        result = DatosL.d[i].ao.toUpperCase() + " " + DatosL.d[i].de.toLowerCase().replace(/"/g, '""');
        if (result.search(/("|,|\n)/g) >= 0)
            result = '"' + result + '"';
        result = result.replace(',', '');
        finalVal += result + ',';

        result = DatosL.d[i].aa.replace(/"/g, '""');
        if (result.search(/("|,|\n)/g) >= 0)
            result = '"' + result + '"';
        result = result.replace(',', '');
        finalVal += result + ',';

        result = limcla.replace(/"/g, '""');
        if (result.search(/("|,|\n)/g) >= 0)
            result = '"' + result + '"';
        result = result.replace(',', '');
        finalVal += result + ',';

        result = DatosL.d[i].re.n.replace(/"/g, '""');
        if (result.search(/("|,|\n)/g) >= 0)
            result = '"' + result + '"';
        result = result.replace(',', '');
        finalVal += result + ',';

        result = DatosL.d[i].se.n.replace(/"/g, '""');
        if (result.search(/("|,|\n)/g) >= 0)
            result = '"' + result + '"';
        result = result.replace(',', '');
        finalVal += result + ',';

        result = Principal.estado(DatosL.d[i].eo).replace(/"/g, '""');
        if (result.search(/("|,|\n)/g) >= 0)
            result = '"' + result + '"';
        result = result.replace(',', '');
        finalVal += result;
        //finalVal = finalVal.replace('\n','_');
        finalVal += '\n';
        i++;
    }

    if (Cur + 1 == TotalP) {
        var IniVal = 'Incidencia,Asunto/Detalle,Apertura,Límite/Clausura,Responsable,Solicitane,Estado \n';
        var pom = document.createElement('a');
        var csvContent = IniVal + finalVal; //here we load our csv data 
        var blob = new Blob([csvContent], { type: 'text/csv;charset=ISO-8859-1;' });
        var url = URL.createObjectURL(blob);
        pom.href = url;
        pom.setAttribute('download', 'incidencias.csv');
        pom.click();
    }
}
var Cur;
var TotalP;
function Exportar() {
    
    var result;

    var j = 0;
    var i = 0;

    var totalPages = tableView.get_pageCount();
    var pageSize = tableView.get_pageSize();
    var sort = tableView.get_sortExpressions().toString();
    var currentPageIndex = tableView.get_currentPageIndex();
    var usu = Principal.datosusuario.co;
    finalVal = "";
    TotalP = totalPages;
    for(var k = 0; k<totalPages;k++)
    {
        Cur = k;
        CustomerCare.Servicio.GetDataAndCount(k * pageSize, pageSize, filtro(), usu, datosobtenidosL, error);
    }
     
   
 
}

//function clickingToolBar(s, a) {
////  if (a.get_item()._isDropDownItem()) {
////    a.set_cancel(true);
////  }
//}

//function selecteditemchanged(s, e) {
//    document.getElementById("hddBuscando").value = "false";
//    $find("RadToolBar1").findItemByText("Buscar").findControl("rtbBuscar").set_value("");
//    tablabind();
//}

function RadGrid1_RowDataBound(sender, args) {
    if (!Datos)
        return;
    var i = args.get_dataItem()["index"];
    var codigo = args.get_dataItem()["Codigo"];
    var celdaCodigo = args.get_item().get_cell("codigo");
    var celdaIconos = args.get_item().get_cell("Iconos");
    var celdaAsunto = args.get_item().get_cell("asunto");
    switch (Datos.d[i].pd) {
        case 1:
            celdaAsunto.className = "tdCritico"
            break;
        case 2:
            celdaAsunto.className = "tdAlto"
            break;
        case 3:
            celdaAsunto.className = "tdMedio"
            break;
        case 4:
            celdaAsunto.className = "tdBajo"
            break;
    }
    var strt = Principal.valorColumnaCodigo(Datos.d[i].co);
    var imFolder = Principal.serverVars.imagesFolder;
    celdaCodigo.innerHTML = strt;
    if (celdaIconos) {
        if (Datos.d[i].vs.length > 0) {
                                    var j = 0;
                                    var strt = '';
                                    var imag
                                    while (j < Datos.d[i].vs.length) {
                                        if (Datos.d[i].vs[j].ar == "a")
                                            imag =  imFolder + "20x20/valida.png";
                                        else
                                            imag = imFolder + "20x20/rechaza.png";
                                        strt += '<!----><img src="' + imag + '" style="float:right; margin-right:2px;" title="' + Datos.d[i].vs[j].vr + '"/>';
                                        j++;
                                    }
                                    if (celdaIconos.innerHTML.indexOf('<!---->') == -1) {
                                        celdaIconos.innerHTML += strt;
                                    }
//                                }, error);
            //                                args.get_item().get_cell("codigo").innerHTML += "Hay";
        }
        if (Datos.d[i].hs > 0)
            celdaIconos.innerHTML += '<img src="' + imFolder + '20x20/derivados.png" style="float:right; margin-right:2px;"/>';
    }
    switch (Datos.d[i].eo) {
        case 1: args.get_item().get_element().style.background = "LightBlue";
            break;
        case 2: args.get_item().get_element().style.background = "#fffacd";
            break;
        case 3: args.get_item().get_element().style.background = "#eea0a0";
            break;
        case 4: args.get_item().get_element().style.background = "LightGray";
            break;
        default: args.get_item().get_element().style.background = "Gray";
    }
}

function showMenu(e) {
    var contextMenu = Principal.$find("rcmTipificacion");
    if ((!e.relatedTarget) || (!$telerik.isDescendantOrSelf(contextMenu.get_element(), e.relatedTarget))) {
        contextMenu.showAt(280, 250);
    }
    $telerik.cancelRawEvent(e);
    Principal.rcmTipificacion_opener = "tipificar";
}

function RowDblclick(sender, eventArgs) {
    var i = eventArgs.get_itemIndexHierarchical();
    if (permiso(22))
        Principal.addtab("Tickets/TicketDetalle.aspx?tic=" + Datos.d[i].co, Principal.serverVars.ticket + " " + Principal.valorColumnaCodigo(Datos.d[i].co), Datos.d[i].co).select();
}

function RowSelected(sender, eventArgs) {
    var fram = frame.document;
    fram.getElementById("icValidar").title = Principal.serverVars.tituloVentanaValidar;
    frame.vioeve = 0;
    frame.viodat = 0;
    tickets=""
    tics = "";
    var items = sender.get_selectedItems();
    var levs = "";
    var tips = "";
    var levantadores = new Array();
    var tipificaciones = new Array();
    $(".accion", fram).show();
    for (var i = 0; i < items.length; i++) {
        var index = items[i].get_itemIndexHierarchical();
        var mia = (Datos.d[index].re.n.charAt(Datos.d[index].re.n.length - 1) == '*');
        var est = Datos.d[index].eo;
        $("#icDerivados", fram).hide();
        if (!permiso(16) || est > 3) $("#icTipificar", fram).hide();
        if (!permiso(17) || est > 3) $("#icSeguimiento", fram).hide();
        if (!mia || est != 2) $("#icCerrar", fram).hide();
        if (!mia || est != 3) $("#icValidar", fram).hide();
        if (!mia || est != 3) $("#icRechazar", fram).hide();
        if (!mia || est != 2) $("#icSolVal", fram).hide();
        if ((est > 4 || !permiso(1)) && (!mia || est != 1)) $("#icCancelar", fram).hide();
        if (est > 3 || !permiso(3)) $("#icDerivado", fram).hide();
        if (!permiso(25) || est > 3) $("#icRepactar", fram).hide();
        if (est > 3 || (!permiso(2) && !mia)) $("#icEscalar", fram).hide();
        if (!permiso(33) || est > 3) $("#icPriorizar", fram).hide();
        if (est > 3 || !mia || !permiso(39)) $("#icReprogramar", fram).hide();
        if (!permiso(40)) $("#icProyecto", fram).hide();
        tics += Datos.d[index].co + ",";
        tickets += Principal.valorColumnaCodigo(Datos.d[index].co) + ", ";
        if ($.inArray(Datos.d[index].lr.c, levantadores) < 0) {
            levantadores.push(Datos.d[index].lr.c);
            levs += Datos.d[index].lr.n + ", ";
        }
        if ($.inArray(Datos.d[index].tn.Key, tipificaciones) < 0) {
            tipificaciones.push(Datos.d[index].tn.Key);
            tips += Datos.d[index].tn.Value + ", ";
        }
    }
    levs = levs.substring(0, levs.length - 2);
    tips = tips.substring(0, tips.length - 2);
    tickets = tickets.substring(0, tickets.length - 2);
    tics = tics.substring(0, tics.length - 1);
    $("#tdCodigo", fram).html(tickets);
    $("#lblLevanto", fram).html(levs);
    $("#lblTipificacion", fram).html(tips);
    var rts = frame.$find("RadTabStrip1");
    if (items.length == 1) {
        var ind = eventArgs.get_itemIndexHierarchical();
//        tic = Datos.d[ind].co;
        if (Datos.d[ind].hs == 0) $("#icDerivados", fram).hide();
        $("#lblAsunto", fram).html(Datos.d[ind].ao);
        $("#lblDetalle", fram).html(Datos.d[ind].de);
        $("#lblSolucion", fram).html(Datos.d[ind].sn);
        if (rts.get_selectedTab().get_index() == 1)
            frame.evebind();
        if (rts.get_selectedTab().get_index() == 2) {
            rts.get_selectedTab().get_pageView().get_element().innerHTML = "";
            CustomerCare.Servicio.DatosCodigo(tics, Principal.datosusuario.co, frame.datosobtenidos, error);
        }
    }
    else {
        $("#icDerivado", fram).hide();
        $("#lblAsunto", fram).html("Selección múltiple");
        $("#lblDetalle", fram).html("");
        $("#lblSolucion", fram).html("");
        if (rts.get_selectedTab().get_index() > 0)
            rts.set_selectedIndex(0);
    }
    if (verderivados)
        frame.verderivados();
    verderivados = false;
}

function usuario() {
    var botonesUsu = $find("RadToolBar1").get_items().getItem(0).get_buttons();
    var rls = 0;
    if (botonesUsu.getButton(1).get_checked()) rls = rls + 1;
    if (botonesUsu.getButton(2).get_checked()) rls = rls + 2;
    if (botonesUsu.getButton(3).get_checked()) rls = rls + 4;
    if (botonesUsu.getButton(4).get_checked()) rls = rls + 8;
    if (botonesUsu.getButton(5).get_checked()) rls = rls + 16;
    if (botonesUsu.getButton(6).get_checked()) rls = rls + 32;
    return botonesUsu.getButton(0).findControl("rcbUsuario").get_value() + "." + rls;
}


function solicitante() {
    //    var botonesSol = $find("RadToolBar1").get_items().getItem(1).get_buttons();
    //    return botonesSol.getButton(0).findControl("rcbSolicitante").get_value();
    var tree = $find("RadToolBar1")._findItemByHierarchicalIndex("1").get_buttons().getButton(0).findControl("rtvSolicitante");
    var result = "";
    var selected = tree.get_checkedNodes();
    for (var i = 0; i < selected.length; i++)
        result += selected[i].get_value() + ",";
    return result.substr(0, result.length - 1);
}


function estados() {
    var botonesEst = $find("RadToolBar1").get_items().getItem(2).get_buttons();
    var estds = 0;
    if (botonesEst.getButton(0).get_checked()) estds = estds + 1;
    if (botonesEst.getButton(1).get_checked()) estds = estds + 2;
    if (botonesEst.getButton(2).get_checked()) estds = estds + 4;
    if (botonesEst.getButton(3).get_checked()) estds = estds + 8;
    if (botonesEst.getButton(4).get_checked()) estds = estds + 16;
    return estds;
}

function prioridad() {
    var botonesUsu = $find("RadToolBar1").get_items().getItem(5).get_buttons();
    var rls = 0;
    if (botonesUsu.getButton(0).get_checked()) rls = rls + 1;
    if (botonesUsu.getButton(1).get_checked()) rls = rls + 2;
    if (botonesUsu.getButton(2).get_checked()) rls = rls + 4;
    if (botonesUsu.getButton(3).get_checked()) rls = rls + 8;
    //console.log("acá es en priorirdad");
    //console.log(rls);
    return  rls;
}

function fecstr(fecha) {
    return (fecha.getDate() < 10 ? "0" + fecha.getDate() : fecha.getDate()) + "/" + (fecha.getMonth() + 1 < 10 ? "0" + (fecha.getMonth() + 1) : fecha.getMonth() + 1) + "/" + fecha.getFullYear() + " " + (fecha.getHours() < 10 ? "0" + fecha.getHours() : fecha.getHours()) + ":" + (fecha.getMinutes() < 10 ? "0" + fecha.getMinutes() : fecha.getMinutes());
}

function fecha() {
    var botonesFec = $find("RadToolBar1")._findItemByHierarchicalIndex("3").get_buttons();
    var fechaDesde = botonesFec.getButton(1).findControl("rdtpDesde").get_selectedDate();
    if (fechaDesde == null) fechaDesde = new Date(0);
    var fechaHasta = botonesFec.getButton(3).findControl("rdtpHasta").get_selectedDate();
    if (fechaHasta == null) fechaHasta = new Date(3155760000000);
    var checks = document.getElementsByName("RadToolBar1$i3$i2$rblDonde");
    var i;
    for (i = 0; i < 3; i++) {
        if (botonesFec.getButton(4 + i).get_checked())
            break;
    }
    return fecstr(fechaDesde) + "." + fecstr(fechaHasta) + "." + i;
}

function tipificacion() {
    var tree = $find("RadToolBar1")._findItemByHierarchicalIndex("4").get_buttons().getButton(0).findControl("RadTreeView1");
    var result = "";
    var selected = tree.get_checkedNodes();
    for (var i = 0; i < selected.length; i++)
        result += selected[i].get_value() + ",";
    return result.substr(0, result.length - 1);
}

function busqueda() {
    //console.log("acá en la búsqueda");
    //console.log(Principal.valorBuscar($find("RadToolBar1")._findItemByHierarchicalIndex("7").findControl("rtbBuscar").get_value()));
    //console.log($find("RadToolBar1")._findItemByHierarchicalIndex("7").findControl("rtbBuscar").get_value());
    return Principal.valorBuscar($find("RadToolBar1")._findItemByHierarchicalIndex("7").findControl("rtbBuscar").get_value());
}

function filtro() {
    var str = "";
    if (filtroActivo[0])
        str = str + usuario() + "!";
    else
        str = str + "!";
    if (filtroActivo[1])
        str = str + solicitante() + "!";
    else
        str = str + "!";
    if (filtroActivo[2])
        str = str + estados() + "!";
    else
        str = str + "!";
    if (filtroActivo[3])
        str = str + fecha() + "!";
    else
        str = str + "!";
    if (filtroActivo[4])
        str = str + tipificacion() + "!";
    else
        str = str + "!";
    if (filtroActivo[5])
        str = str + prioridad() + "!";
    else
        str = str + "!";
    return str + busqueda();
}

function tablabindsp() {
    var pageSize = tableView.get_pageSize();
    var sort = tableView.get_sortExpressions().toString();
    var currentPageIndex = tableView.get_currentPageIndex();
    var usu = Principal.datosusuario.co;
    /*
    console.log("los datos para pinrtas");
    console.log(currentPageIndex );
    console.log(pageSize);
    console.log(filtro());
    console.log(usu);
    console.log(sort);
    console.log(datosobtenidos);
    console.log(error);
    */
    if (sort != "")
        CustomerCare.Servicio.GetDataAndCountSort(currentPageIndex * pageSize, pageSize, filtro(), usu, sort, datosobtenidos, error);
    else
        CustomerCare.Servicio.GetDataAndCount(currentPageIndex * pageSize, pageSize, filtro(), usu, datosobtenidos, error);
    tableView.clearSelectedItems();
    try {
        var fram = frame.document;
        fram.getElementById("tdCodigo").innerHTML = "";
        fram.getElementById("lblAsunto").innerHTML = "";
        fram.getElementById("lblDetalle").innerHTML = "";
        fram.getElementById("lblSolucion").innerHTML = "";
        fram.getElementById("lblLevanto").innerHTML = "";
        fram.getElementById("lblTipificacion").innerHTML = "";
        $(".accion", fram).hide();
        var rts = frame.$find("RadTabStrip1");
        if (rts.get_selectedTab().get_index() != 0)
            rts.findTabByText("Datos").select();
    }
    catch (e) { };
    nbind = 0;
    return;
}

function tablabind() {
    //if (!filtrando)
    ralp.show("RadGrid1");
    tablabindsp();
}

var DatosGlobal = Array();
function datosobtenidos(res, args) {
    try {
        Datos = Principal.JSON.parse(res);
    }
    catch (e) {
        alert("JSON cadena no valida: " + res);
        return;
    }
    DatosGlobal = new Array();
    cont2 = new Array();
    var i = 0;
    
    while (i < Datos.d.length) {
        var limcla;
        switch (Datos.d[i].eo) {
            case 1: limcla = Datos.d[i].le.Key == "" ? "Sin límite" : Datos.d[i].le.Key; break;
            case 2: limcla = Datos.d[i].le.Value; break;
            case 3: limcla = Datos.d[i].le.Value; break;
            default: limcla = Datos.d[i].ca;
        }
        var a = {
            index: i,
            Codigo: Datos.d[i].co,
            Iconos: "",
            Asunto: Datos.d[i].ao.toUpperCase() + " " + Datos.d[i].de.toLowerCase(),
            Apertura: Datos.d[i].aa,
            Limcla: limcla,
            Clausura: Datos.d[i].ca,
            Responsable: Datos.d[i].re.n,
            Solicitante: Datos.d[i].se.n,
            Estado: Principal.estado(Datos.d[i].eo),
            Prioridad: Principal.prioridad( Datos.d[i].pd)
        };
        DatosGlobal.push(a);
        i++;
    }
   // console.log("acá en datosobtenidos");
   // console.log(DatosGlobal);
    tableView.set_dataSource(DatosGlobal);
    tableView.set_virtualItemCount(Datos.c);
    tableView.dataBind();
    pinta();
    ralp.hide("RadGrid1");
}

//function valobtenidas(res, args) {
//    alert("hola");
//  }

function activaFiltro(x) {
    //console.log("paso a activar filtro: " + x);
    if (filtroActivo[x]) {
        filtroActivo[x] = false;
        img[x].style.backgroundPosition = "12px 0px";
    }
    else {
        filtroActivo[x] = true;
        img[x].style.backgroundPosition = "0px 0px";
    }
    if (!filtrando) {
        //ralp.show("RadGrid1");
        filtrando = true;
    }
}

function strfec(fecha) {
    var dh = fecha.split(' ');
    var dma = dh[0].split("/");
    var hm = dh[1].split(":");
    return new Date(1 * dma[2], 1 * dma[1], 1 * dma[0], 1 * hm[0], 1 * hm[1]);
}

function load() {
    if (!permiso(10)) {
        $(".rtbUL>:eq(3)").toggle();
        $(".rtbUL>:eq(4)").toggle();
        $(".rtbUL>:eq(7)").toggle();
    }
    tableView = $find("RadGrid1").get_masterTableView();
    ralp = $find("RadAjaxLoadingPanel1");
    rdc = $find("rdcAcciones");
    var toolbar = $find("RadToolBar1")
    document.getElementById("RadGrid1_GridData").style.height = "auto";
    toolbar.findItemByText("Tipificación").get_dropDownElement().onscroll = tipiscroll;
    toolbar.findItemByText("Solicitante").get_dropDownElement().onscroll = tipiscroll;
    for (var i = 0; i < 7; i++) {
        img[i] = toolbar._findItemByHierarchicalIndex(i + "").get_imageElement();
    }
    //console.log(img);
    img[0].onclick = function () { activaFiltro(0); }
    img[1].onclick = function () { activaFiltro(1); }
    img[2].onclick = function () { activaFiltro(2); }
    img[3].onclick = function () { activaFiltro(3); }
    img[4].onclick = function () { activaFiltro(4); }
    img[5].onclick = function () { activaFiltro(5); }
    var dropTipi = toolbar._findItemByHierarchicalIndex("4").get_dropDownElement();
    dropTipi.className += " dropTree";
    var dropSol = toolbar._findItemByHierarchicalIndex("1").get_dropDownElement();
    dropSol.className += " dropTree";
    img[6].style.width = "18px";
    img[6].style.height = "18px";
    img[6].style.background = "none";
    img[6].parentNode.className += " guardaBotonIn";
    img[6].parentNode.parentNode.parentNode.parentNode.parentNode.className += " guardaBoton";
    switch (Principal.BrowserDetect.browser) {
        case "Chrome":
            dropTipi.style.paddingRight = "17px";
            dropSol.style.paddingRight = "17px";
            break;
        case "Firefox":
            dropTipi.style.paddingRight = "23px";
            dropSol.style.paddingRight = "23px";
            for (i = 0; i < 7; i++) {
                img[i].style.width = "8px";
                img[i].style.height = "8px";
            }
            break;
        case "Explorer":
            switch (Principal.BrowserDetect.version) {
                case 7:
                    for (i = 0; i < 5; i++) {
                        img[i].style.width = "8px";
                        img[i].style.height = "8px";
                    }
                    break;
            }
            break;
    }
    var downs = $(".rtbWrap.rtbExpandDown");
    for (i = 0; i < downs.length; i++) { downs[i].setAttribute("href", "javascript:") };
    var filtroDefault = document.getElementById("hddFiltroDefault").value.split('!');
    if (filtroDefault[0] != "") {
        var fil = filtroDefault[0].split('.');
        var botonesUsu = $find("RadToolBar1").get_items().getItem(0).get_buttons();
        botonesUsu.getButton(0).findControl("rcbUsuario").findItemByValue(fil[0]).select();
        var rls = fil[1] * 1;
        if (rls > 31) { botonesUsu.getButton(6).check(); rls -= 32; }
        if (rls > 15) { botonesUsu.getButton(5).check(); rls -= 16; }
        if (rls > 7) { botonesUsu.getButton(4).check(); rls -= 8; }
        if (rls > 3) { botonesUsu.getButton(3).check(); rls -= 4; }
        if (rls > 1) { botonesUsu.getButton(2).check(); rls -= 2; }
        if (rls > 0) { botonesUsu.getButton(1).check(); rls -= 1; }
        activaFiltro(0);
    }
    if (filtroDefault[1] != "") {
        var botonesSol = $find("RadToolBar1").get_items().getItem(1).get_buttons();
        botonesSol.getButton(0).findControl("rcbSolicitante").findItemByValue(filtroDefault[1]).select();
        activaFiltro(1);
    }
    if (filtroDefault[2] != "") {
        var botonesEst = $find("RadToolBar1").get_items().getItem(2).get_buttons();
        var estds = filtroDefault[2] * 1;
        if (estds > 15) { botonesEst.getButton(4).check(); estds -= 16; }
        if (estds > 7) { botonesEst.getButton(3).check(); estds -= 8; }
        if (estds > 3) { botonesEst.getButton(2).check(); estds -= 4; }
        if (estds > 1) { botonesEst.getButton(1).check(); estds -= 2; }
        if (estds > 0) { botonesEst.getButton(0).check(); estds -= 1; }
        activaFiltro(2);
    }
    if (filtroDefault[3] != "") {
        var botonesFec = $find("RadToolBar1")._findItemByHierarchicalIndex("3").get_buttons();
        var fil = filtroDefault[3].split('.');
        var fechaDesde = strfec(fil[0])
        var rdtpDesde = botonesFec.getButton(1).findControl("rdtpDesde");
        if (fechaDesde < new Date(60000)) rdtpDesde.clear();
        else rdtpDesde.set_selectedDate(fechaDesde);
        var rdtpHasta = botonesFec.getButton(3).findControl("rdtpHasta");
        var fechaHasta = strfec(fil[1])
        if (fechaHasta > new Date(3155759940000)) rdtpHasta.clear();
        rdtpHasta.set_selectedDate(fechaHasta);
        var checks = document.getElementsByName("RadToolBar1$i3$i2$rblDonde");
        var i = fil[2] * 1;
        botonesFec.getButton(4 + i).check();
        activaFiltro(3);
    }
    if (filtroDefault[4] != "") {
        var tree = $find("RadToolBar1")._findItemByHierarchicalIndex("4").get_buttons().getButton(0).findControl("RadTreeView1");
        var tipis = filtroDefault[4].split(',');
        for (var i = 0; i < tipis.length; i++) {
            tree.findNodeByValue(tipis[i]).check();
        }
        activaFiltro(4);
    }
    filtrando = false;
    rtvSolicitante = $find("RadToolBar1").get_items().getItem(1).get_buttons().getButton(0).findControl("rtvSolicitante");
    nodes = rtvSolicitante.get_allNodes();
    tablabind();
    setInterval(tick, 60000);
}

function solCheck(s, a) {
    cambioFiltro(1);
}

var filtroSolicitanteID = 0;

function paraFiltroSolicitante(s, e) {
    clearInterval(filtroSolicitanteID);
}

function iniciaFiltroSolicitante(s, e) {
    filtroSolicitanteID = setInterval(function () { filtroSolicitante(s); }, 500);
}

function filtroSolicitante(sender) {
    var t = sender.get_value();
    if (t.length > 1) {
        var r = new RegExp(t, "i");
        for (var i = 0; i < nodes.length; i++) {
            var padre = nodes[i].get_parent();
            if (r.test(nodes[i].get_text())) {
                while (padre.expand) {
                    padre.expand();
                    padre = padre.get_parent();
                }
                nodes[i].select();
            }
            else {
                nodes[i].unselect();
                if (padre.collapse && padre.get_allNodes().every(function (x) { return !r.test(x.get_text()); })) {
                    padre.collapse();
                }
            }
        }
    }
    else if (t.length == 0)
        rtvSolicitante.get_nodes().toArray().forEach(function (x) { x.collapse(); x.unselect(); })
}


function checkSolicitante(sender, eventArgs) {
    if (eventArgs.get_keyCode() == 13) {
        rtvSolicitante.uncheckAllNodes();
        rtvSolicitante.checkNodes(rtvSolicitante.get_selectedNodes());
        hideDropDowns($find("RadToolBar1").get_items(), hideDropDown);
        tablabind();
    }
}

function pinta() {
    var horaserv = Principal.horaServidor();
    if ((typeof horaserv) != "string") {
        var dataitems = tableView.get_dataItems();
        var limite;
        var apertura;
        var clausura;
        if (!Datos)
            return;
        for (i = 0; i < Datos.d.length; i++) {
            if (permiso(14) && (Datos.d[i].eo == 2 || Datos.d[i].eo == 3 || Datos.d[i].eo == 4)) {
                limite = Datos.d[i].le.Key;
                if (limite.length > 0) {
                    limite = (new Date('20' + limite.substr(5, 2), mesnum(limite.substr(2, 3)), limite.substr(0, 2), limite.substr(8, 2), limite.substr(11, 2), 0)).getTime();
                    apertura = Datos.d[i].aa;
                    apertura = (new Date('20' + apertura.substr(5, 2), mesnum(apertura.substr(2, 3)), apertura.substr(0, 2), apertura.substr(8, 2), apertura.substr(11, 2), 0)).getTime();
                    if (Datos.d[i].eo != 4) {
                        dataitems[i].get_cell("codigo").style.background = color(Math.floor(32 * (horaserv - apertura) / (limite - apertura)));
                    }
                    else {
                        clausura = Datos.d[i].ca;
                        clausura = (new Date('20' + clausura.substr(5, 2), mesnum(clausura.substr(2, 3)), clausura.substr(0, 2), clausura.substr(8, 2), clausura.substr(11, 2), 0)).getTime();
                        dataitems[i].get_cell("codigo").style.background = color(Math.floor(32 * (clausura - apertura) / (limite - apertura)));
                    }
                }
                else
                    dataitems[i].get_cell("codigo").style.background = color(-1);
            }
        }
    }
    else
        setTimeout(pinta, 50);
}

function tick() {
    pinta();
    if (!filtrando) {
        var pageSize = tableView.get_pageSize();
        var sort = tableView.get_sortExpressions().toString();
        var currentPageIndex = tableView.get_currentPageIndex();
        var usu = Principal.datosusuario.co;
        try {
            if (sort == "")
                CustomerCare.Servicio.Actualiza(currentPageIndex * pageSize, filtro(), usu, llegoactualiza, error);
            else
                CustomerCare.Servicio.ActualizaSort(currentPageIndex * pageSize, filtro(), usu, sort, llegoactualiza, error);
        }
        catch (e) {
            alert("Actualiza(" + currentPageIndex * pageSize + ", " + filtro() + ", " + usu + ")");
        }
    }
}

function gama(i, f, n) {
    var ri = parseInt(i.substr(1, 2), 16);
    var rf = parseInt(f.substr(1, 2), 16);
    var gi = parseInt(i.substr(3, 2), 16);
    var gf = parseInt(f.substr(3, 2), 16);
    var bi = parseInt(i.substr(5, 2), 16);
    var bf = parseInt(f.substr(5, 2), 16);
    if (n < 0)
        return i;
    else if (n > 1)
        return f;
    else {
        var r = Math.round(ri + n * (rf - ri)).toString(16);
        var g = Math.round(gi + n * (gf - gi)).toString(16);
        var b = Math.round(bi + n * (bf - bi)).toString(16);
        r = r.length == 1 ? '0' + r : r;
        g = g.length == 1 ? '0' + g : g;
        b = b.length == 1 ? '0' + b : b;
        return ("#" + r + g + b);
    }
}

function color(n) {
    if (n < 0)
        return "#008000";
    else if (n < 32)
        return gama("#008000", "#F04000", n / 32);
    else
        return "#F00000";
}

function mesnum(mes) {
    switch (mes) {
        case "ene":
            return "00";
            break;
        case "feb":
            return "01";
            break;
        case "mar":
            return "02";
            break;
        case "abr":
            return "03";
            break;
        case "may":
            return "04";
            break;
        case "jun":
            return "05";
            break;
        case "jul":
            return "06";
            break;
        case "ago":
            return "07";
            break;
        case "sep":
            return "08";
            break;
        case "oct":
            return "09";
            break;
        case "nov":
            return "10";
            break;
        case "dic":
            return "11";
            break;
    }
}
function llegoactualiza(res, args) {
    Principal.semaforoVerde();
    if (res != "-1" && Datos) {
        if (Datos.d[0].co != res)
            tablabindsp();
    }
}

function RadGrid1_Command(sender, args) {
    //alert(args.get_commandName());
    //    alert(args.get_commandArgument());
    //    alert(args.get_domEvent());
    args.set_cancel(true);
    //    var pageSize = sender.get_masterTableView().get_pageSize();
    //    var sortExpressions = sender.get_masterTableView().get_sortExpressions();
    //    var sortExpressionsAsSQL = sortExpressions.toString();
    var command = args.get_commandName()
    if ((command == "Page" || command == "PageSize" || command == "Sort") && cancelRadGridCommandBind == 0)
        tablabind();
    else
        cancelRadGridCommandBind = 0;
}

function guardarSeguimiento() {
    var cod = $("#hddCodigo").val();
    var obs = $("#rtbObs").val();
    var usu = Principal.datosusuario.co;
    var x = $("#chkEnviarMail")[0];
    var rbl = $("[name='rblTipoEvento']");
    var eve = "2";
    for (var i = 0; i < rbl.length; i++) {
        if (rbl[i].checked) {
            eve = rbl[i].value;
        }
    }
    if (x.checked)
        env = "si";
    else
        env = "";

    CustomerCare.Servicio.Seguimiento(cod, obs, usu, env, eve, segGuardado, error);
}

function segGuardado(res, e) {
    rdc.set_closed(true);
    $("#hddCodigo").val("");
    frame.evebind();
}

function salirCancelar() {
    var usu = Principal.datosusuario.co;
    var cod = $("#hddCodigo").val();
    var sol = $("#RadTextBox1").val();
    CustomerCare.Servicio.CancelarTicket(cod, sol, usu, incCancelada, error);
}

function incCancelada(res, e) {
    if (res == "Cancelacion") {
        tablabind();
        alert("Ticket cancelado");
    }
    else {
        alert("Hubo un error, no se hizo ningun cambio");
    }
    rdc.set_closed(true);
    document.getElementById("hddCodigo").value = "";
}

function error(e) {
    if (e.get_message() == "The server method 'Actualiza' failed." || e.get_message() == "The server method 'ActualizaDerivados' failed." || e.get_message() == "Error del método de servidor 'ActualizaDerivados'." || e.get_message() == "Error del método de servidor 'Actualiza'.")
        Principal.semaforoRojo();
    else
        Principal.error(e);
    ralp.hide("RadGrid1");
}





