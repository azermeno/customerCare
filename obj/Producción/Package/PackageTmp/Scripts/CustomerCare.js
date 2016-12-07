var tabStrip1;
var ralpRelojito;
var datosusuario;
var IE;
var usuG = 0;
$(function () {
    date = new Date();
    IE = document.all ? true : false;
    $(window).load(function () {
        var usu = document.getElementById("hddUsuario").value;
        usuG = usu;
        CustomerCare.Servicio.Instante(iniciareloj, error);
        CustomerCare.Servicio.DatosUsuario(usu, usuarioObtenido, error);
        var height = $(window).height();
        var multiPage = $find("rmpPrincipal");
        var totalHeight = height - 60;
        multiPage.get_element().style.height = totalHeight + "px";
        ralpRelojito = $find("ralpRelojito");
        addtab("Tickets/Tickets.aspx", serverVars.ticket + "s", "Tickets").select();
        if ($("#hddAccion").length) {
            var str = $("#hddAccion").val();
            var accion = str.substring(0, str.indexOf(":"));
            var accionDatos = str.substring(str.indexOf(":") + 1);
            switch (accion) {
                case "ticket":
                    addtab("Tickets/TicketDetalle.aspx?tic=" + accionDatos, serverVars.ticket + " " + valorColumnaCodigo(accionDatos), accionDatos).select();
                    break;
            }
        }
        document.getElementsByTagName("iframe")[0].style.height = totalHeight + "px";
    });
});



///////////////////////////////////////////CONEXIÓN AL SERVIDOR//////////////////////////////////////////

var semaforo = true;
function semaforoRojo() {
    if (semaforo) {
        $("#semaforo").css("background-position", "0px 0px")
        //        document.getElementById("semaforo").setAttribute("style", "width:15px; height:32px; background-image:url(" + serverVars.imagesFolder + "semaforo.png); ;");
        semaforo = false;
    }
}

function semaforoVerde() {
    if (!semaforo) {
        $("#semaforo").css("background-position", "15px 0px")
        //        document.getElementById("semaforo").setAttribute("style", "width:15px; height:32px; background-image:url(" + serverVars.imagesFolder + "semaforo.png); background-position:15px 0px;");
        semaforo = true;
    }
}







//--------------------------------------------------------------------Pestañas---------------------------------------------////////////////////////////////

function OnClientLoad() {
    tabStrip1 = $find(serverVars.rtsPrincipalId);

    for (var i = 0; i < tabStrip1.get_tabs().get_count(); i++) {
        AttachCloseImage(tabStrip1.get_tabs().getItem(i), serverVars.imagesFolder + "delete.png");
    }
}
function addtab(url, title, value) {
    var tab = new Telerik.Web.UI.RadTab();
    tab.set_text(title);
    tab.set_value(value);
    var mpg = $find(serverVars.rmpPrincipalId);
    mpg.trackChanges();
    var pageView = new Telerik.Web.UI.RadPageView();
    mpg.get_pageViews().add(pageView);
    pageView.set_contentUrl(url);
    pageView.get_element().style.height = "100%";
    mpg.commitChanges();
    tabStrip1.get_tabs().add(tab);
    AttachCloseImage(tab, serverVars.imagesFolder + "delete.png");
    return (tab);
}
function CreateCloseImage(closeImageUrl) {
    var closeImage = document.createElement("img");
    closeImage.src = closeImageUrl;
    closeImage.alt = "Cerrar esta pestaña";
    return closeImage;
}
function AttachCloseImage(tab, closeImageUrl) {
    var closeImage = CreateCloseImage(closeImageUrl);
    closeImage.AssociatedTab = tab;
    closeImage.onclick = function (e) {
        if (!e) e = event;
        if (!e.target) e = e.srcElement;

        deleteTab(tab);

        e.cancelBubble = true;
        if (e.stopPropagation) {
            e.stopPropagation();
        }

        return false;
    }
    tab.get_innerWrapElement().appendChild(closeImage);
}
function deleteTab(tab) {
    var tabStrip = $find(serverVars.rtsPrincipalId);

    var tabToSelect = tab.get_nextTab();
    if (!tabToSelect)
        tabToSelect = tab.get_previousTab();

    var pageview = tab.get_pageView();
    pageview.get_multiPage().get_pageViews().remove(pageview);

    tabStrip.get_tabs().remove(tab);

    if (tabToSelect)
        tabToSelect.set_selected(true);
}
function cerrarPesActiva() {
    var tab = $find(serverVars.rtsPrincipalId).get_selectedTab();
    window.parent.parent.deleteTab(tab);
} 
function getSelectedTabWindow() {
    var win = $find("rmpPrincipal").get_selectedPageView().get_element().getElementsByTagName("iframe")[0].contentWindow;
    return win;
} 
function ventanaPestanaValor(valor) {
    var tab = tabStrip1.findTabByValue(valor);
    var win = tab.get_pageView().get_element().getElementsByTagName("iframe")[0].contentWindow;
    return win;
}



//////////----------------------------------------------menú principal------------//////////////////////
var Principal = window.parent.parent;
function RadMenu1_ItemClicked(sender, args) {
    var value = args.get_item().get_value();
    if (value && value.substring(0, 13) == "clasificacion") {
        var clasificacion = value.substring(13);
        addtab("Administracion/Clases.aspx?clasificacion=" + clasificacion, clasificacion, clasificacion.toLowerCase()).select();
    }
    switch (value) {
        case "nvotick":
            var nvotick = radopen("Tickets/NuevoTicket.aspx", "nvotick");
            nvotick.setSize(960, 410);
            nvotick.add_beforeClose(cerrarMenu);
            break;
        case "nvoticklargo":
            addtab("Ayuntamiento/nueva.aspx", "Nueva Queja", "nueva").select();
            break;
        case "formatos":
            addtab("Administracion/Formatos.aspx", "Formatos", "formatos").select();
            break;
        case "programadas":
            addtab("Administracion/Programadas.aspx", "Programadas", "programadas").select();
            break;
        case "usuarios":
            addtab("Administracion/Usuarios.aspx", "Usuarios", "usuarios").select();
            break;
        //        case "solicitantes": 
        //            addtab("Unidades.aspx", "Solicitantes", "solicitantes").select(); 
        //            break; 
        case "comparativozonas":
            addtab("Estadistica/ComparativoZonas.aspx", "Comparativo " + serverVars.zona + "s", "cZonas").select();
            break;
        case "historicozonas":
            addtab("Estadistica/HistoricoZonas.aspx", "Historico " + serverVars.zona + "s", "hZonas").select();
            break;
        case "comparativositios":
            addtab("Estadistica/ComparativoSitios.aspx", "Comparativo " + serverVars.sitio + "s", "cSitios").select();
            break;
        case "comparativoestados":
            addtab("Estadistica/ComparativoEstados.aspx", "Comparativo Estados", "cEstados").select();
            break;
        case "comparativotipificaciones":
            addtab("Estadistica/ComparativoTipificaciones.aspx", "Comparativo por Tipificación", "cTipificacion").select();
            break;
        case "comparativoconclusiones":
            addtab("Estadistica/ComparativoConclusiones.aspx", "Comparativo por Conclusión", "cConclusion").select();
            break;
        case "reportes":
            addtab("Estadistica/Reportes.aspx", "Reportes", "reportes").select();
            break;
        case "tipificaciones":
            addtab("Administracion/Tipificaciones.aspx", "Tipificaciones", "tipificaciones").select();
            break;
        case "datosextra":
            addtab("Administracion/DatosExtra.aspx", "Datos Extra", "datosextra").select();
            break;
        case "perfiles":
            addtab("Administracion/Perfiles.aspx", "Perfiles", "perfiles").select();
            break;
        case "unidades":
            addtab("Administracion/Unidades.aspx", "Unidades", "unidades").select();
            break;
        case "encuestas":
            addtab(Principal.serverVars.urlencuestas, "CiberEncuestas ", "encuestas").select();
            break;
        case "mapa":
            addtab(Principal.serverVars.urlmapa, "Mapa ", "mapa").select();
            break;
        case "checador":
            addtab(Principal.serverVars.checador, "Checador ", "checador").select();
            break;
        case "checadorI":
            addtab(Principal.serverVars.checadorI + codifica(usuG), "Registros del Usuario ", "checadorI").select();
            break;
    }
}

function codifica(cod) {
    console.log(cod);
    var res = 0;
    var pow = 1;
    for (var i = cod.length-1;i>-1;i--)
    {
        var c0 =( parseInt(cod.substr(i, 1)) + 5)%10;
        res += c0*pow;
        pow*=10;
    }
    console.log(res);
    return res;
}

function Salir() {
    if (window.opener) {
        window.opener.location = "Salir.aspx";
        window.close();
    }
    else
        window.location = "Salir.aspx";
}


function usuarioObtenido(res, args) {
    datosusuario = JSON.parse(res);
}


//----------------------------------menús-------------------------------------//
var rcmTipificacion_opener = false;
function rcmTipificacion_itemClicked(sender, args) {
    if (rcmTipificacion_opener) {
        switch (rcmTipificacion_opener) {
            case "nvainc":
                $find("rwmPrincipal").getActiveWindow().get_contentFrame().contentWindow.escribirLabel("Tipificacion", args);
                break;
            case "tipificar":
                getSelectedTabWindow().frames[0].escribirLabel(sender, args);
                break;
            case "tipificardet":
                getSelectedTabWindow().escribirLabel(sender, args);
                break;
            case "programada":
                getSelectedTabWindow().frames[0].escribirLabel(sender, args);
                break;
        }
    }
}

var rcmConclusion_opener = false;
function rcmConclusion_itemClicked(sender, args) {
    if (rcmConclusion_opener) {
        switch (rcmConclusion_opener) {
            case "accion":
                $find("rwmPrincipal").getActiveWindow().get_contentFrame().contentWindow.escribirLabel(sender, args);
                break;
            case "nvainc":
                $find("rwmPrincipal").getActiveWindow().get_contentFrame().contentWindow.escribirLabel("Conclusion", args);
                break;
        }
    }
}

function cerrarMenu(s, e) {
    $find("rcmTipificacion").hide();
}







//------------------------------------------------------------------Utilidades----------------------------------------------------------//
function valorColumnaCodigo(codigo) {
    var index = codigo.indexOf('.');
    var co = (index==-1?codigo:codigo.substring(0, codigo.indexOf('.')));
    var result = eval(serverVars.valorColumnaCodigo) + "";
    return result + (index == -1 ? '' : codigo.substring(codigo.indexOf('.')));
}
function valorBuscar(texto) {
    return texto;
}
function estado(n) {
    switch (n) {
        case 1: return serverVars.preticket; break;
        case 2: return serverVars.activo; break;
        case 3: return serverVars.enValidacion; break;
        case 4: return serverVars.cerrado; break;
        case 5: return serverVars.cancelado; break;
    }
}
function prioridad(n) {
    switch (n) {
        case 1: return "Crítica"; break;
        case 2: return "Alta"; break;
        case 3: return "Media"; break;
        case 4: return "Baja"; break;

    }
}

function estampaInputs(documento) {
    var inputs = documento.getElementsByTagName("input");
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].setAttribute("value", inputs[i].value);
        if (inputs[i].checked)
            inputs[i].setAttribute("checked", "true");
    }
    var textareas = documento.getElementsByTagName("textarea");
    for (var i = 0; i < textareas.length; i++) {
        textareas[i].innerHTML = textareas[i].value;
    }
    var selects = documento.getElementsByTagName("select");
    for (var i = 0; i < selects.length; i++) {
        for (var j = 0; j < selects[i].options.length; j++) {
            if (selects[i].options[j].selected)
                selects[i].options[j].setAttribute("selected", "selected");
            else
                selects[i].options[j].removeAttribute("selected");
        }
    }
}
String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}
String.prototype.trimnbsp = function () {
    var _ret = this.replace(/^\s+|\s+$/g, '');
    return _ret.replace(/^(\&nbsp\;)+|(\&nbsp\;)+$/g, '');
}
function contains(a, obj) {
    for (var i = 0; i < a.length; i++) {
        if (a[i] === obj) {
            return true;
        }
    }
    return false;
}
function fireEvent(element, event) {
    if (document.createEvent) {
        // dispatch for firefox + others
        var evt = document.createEvent("HTMLEvents");
        evt.initEvent(event, true, true); // event type,bubbling,cancelable
        return element.dispatchEvent(evt);
    } else {
        // dispatch for IE
        var evtIE = document.createEventObject();
        return element.fireEvent('on' + event, evtIE)
    }
}

function objectToFieldset(object, nombre, leyenda) {
    var fieldset = document.createElement('fieldset');
    fieldset.setAttribute('id',nombre);
    var legend = document.createElement('legend');
    legend.appendChild(document.createTextNode(leyenda));
    fieldset.appendChild(legend);
    return objectToHtml(object, fieldset);
}

function objectToDiv(object, nombre){
    var div = document.createElement('div');
    div.setAttribute('id', nombre);
    return objectToHtml(object, div);
}

function objectToHtml(object, container) {
    for (var member in object) {
        if (typeof object[member] == 'string') {
            var div = document.createElement('div');
            var label = document.createElement('label');
            label.setAttribute('for', member);
            label.appendChild(document.createTextNode(member + ': '));
            div.appendChild(label);
            var span = document.createElement('span');
            span.setAttribute('class', 'riSingle RadInput RadInput_' + serverVars.Skin);
            var input = document.createElement('input');
            input.setAttribute('type', 'text');
            input.setAttribute('id', member);
            input.setAttribute('value', object[member]);
            span.appendChild(input);
            div.appendChild(span);
            container.appendChild(div);
        }
        else container.appendChild(objectToFieldset(object[member], member, member));
    }
    return container;
}

function jsonToHtml(json) {
    var object = JSON.parse(json);
    var divJSON = objectToDiv(object, "divJSON");
    var inputs = $("input", divJSON);
    inputs.addClass("riTextBox riEnabled");
    inputs.hover(
        function () {
            if (!$(this).is("[readonly]")) {
                $(this).toggleClass("riEnabled");
                $(this).toggleClass("riHover");
            }
        }
    );
    return divJSON;
}

function toggleJsonToHtml(win) {
    var inputs = $("input", win.$('#divJSON')[0]);
    inputs.attr("readonly", !inputs.is("[readonly]"));
    inputs.toggleClass("riRead");
    inputs.toggleClass("riEnabled");
}

function childrenToJson(element) {
    var result = '';
    var child = element.children[0];
    while (child) {
        switch (child.nodeName) {
            case 'FIELDSET':
                result += fieldsetToJson(child) + ',';
                child = child.nextSibling;
                break;
            case 'DIV':
                result += '"' + child.children[1].children[0].id + '":"' + child.children[1].children[0].value + '",';
                child = child.nextSibling;
                break;
            default:
                child = child.nextSibling;
        }
    }
    return result.substr(0, result.length - 1);
}

function fieldsetToJson(fieldset) {
    return '"' + fieldset.children[0].textContent + '":{' + childrenToJson(fieldset) + '}';
}

function htmlToJson(win) {
    return '{' + childrenToJson(win.$('#divJSON')[0]) + '}';
}

function llenaElementoObjeto(element, object) {
    var child = element.children[0];
    while (child) {
        switch (child.nodeName) {
            case 'FIELDSET':
                try {
                    llenaElementoObjeto(child, object[child.children[0].textContent]);
                }
                catch (e) { }
                child = child.nextSibling;
                break;
            case 'DIV':
                try {
                    child.children[1].children[0].value = object[child.children[1].children[0].id];
                }
                catch (e) { }
                child = child.nextSibling;
                break;
            default:
                child = child.nextSibling;
        }
    }
}

function llenaJson(win, json) {
    llenaElementoObjeto(win.$('#divJSON')[0],JSON.parse(json));
}

function WindowDragStart(oWin, args) {
    var resizeExtender = oWin._resizeExtender;
    //DISABLE iframe hiding 
    resizeExtender.set_hideIframes(false);
}



////////-------------------------------Hora del servidor----------------////////////
var date;
var datehere;
function iniciareloj(res, args) {
    datehere = (new Date()).getTime();
    var n = datehere - date.getTime();
    date = new Date('20' + res.substr(0, 2), res.substr(2, 2) - 1, res.substr(4, 2), res.substr(6, 2), res.substr(8, 2), res.substr(10, 2));
    date = date.getTime() + n;
}

function horaServidor() {
    var horaser = (new Date()).getTime() - datehere + date;
    return horaser;
}

function error(e) {
    try {
        CustomerCare.Servicio.logeaCliente(e.get_message(), function (res, args) {
            alert("Ocurrió un error. Código de error:" + res);
        }, function (e) {
            semaforoRojo();
        });
    }
    catch (error) {
        semaforoRojo();
    }
} 



////////////////////////////////////////////////////////
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



 






//--------------------------------------------JSON-----------------------------------------------------//


var JSON;
if (!JSON) {
    JSON = {};
}

(function () {
    "use strict";

    function f(n) {
        // Format integers to have at least two digits.
        return n < 10 ? '0' + n : n;
    }

    if (typeof Date.prototype.toJSON !== 'function') {

        Date.prototype.toJSON = function (key) {

            return isFinite(this.valueOf()) ?
                this.getUTCFullYear() + '-' +
                f(this.getUTCMonth() + 1) + '-' +
                f(this.getUTCDate()) + 'T' +
                f(this.getUTCHours()) + ':' +
                f(this.getUTCMinutes()) + ':' +
                f(this.getUTCSeconds()) + 'Z' : null;
        };

        String.prototype.toJSON =
            Number.prototype.toJSON =
            Boolean.prototype.toJSON = function (key) {
                return this.valueOf();
            };
    }

    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        gap,
        indent,
        meta = {    // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"': '\\"',
            '\\': '\\\\'
        },
        rep;


    function quote(string) {

        // If the string contains no control characters, no quote characters, and no
        // backslash characters, then we can safely slap some quotes around it.
        // Otherwise we must also replace the offending characters with safe escape
        // sequences.

        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable, function (a) {
            var c = meta[a];
            return typeof c === 'string' ? c :
                '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
        }) + '"' : '"' + string + '"';
    }


    function str(key, holder) {

        // Produce a string from holder[key].

        var i,          // The loop counter.
            k,          // The member key.
            v,          // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];

        // If the value has a toJSON method, call it to obtain a replacement value.

        if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }

        // If we were called with a replacer function, then call the replacer to
        // obtain a replacement value.

        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }

        // What happens next depends on the value's type.

        switch (typeof value) {
            case 'string':
                return quote(value);

            case 'number':

                // JSON numbers must be finite. Encode non-finite numbers as null.

                return isFinite(value) ? String(value) : 'null';

            case 'boolean':
            case 'null':

                // If the value is a boolean or null, convert it to a string. Note:
                // typeof null does not produce 'null'. The case is included here in
                // the remote chance that this gets fixed someday.

                return String(value);

                // If the type is 'object', we might be dealing with an object or an array or
                // null.

            case 'object':

                // Due to a specification blunder in ECMAScript, typeof null is 'object',
                // so watch out for that case.

                if (!value) {
                    return 'null';
                }

                // Make an array to hold the partial results of stringifying this object value.

                gap += indent;
                partial = [];

                // Is the value an array?

                if (Object.prototype.toString.apply(value) === '[object Array]') {

                    // The value is an array. Stringify every element. Use null as a placeholder
                    // for non-JSON values.

                    length = value.length;
                    for (i = 0; i < length; i += 1) {
                        partial[i] = str(i, value) || 'null';
                    }

                    // Join all of the elements together, separated with commas, and wrap them in
                    // brackets.

                    v = partial.length === 0 ? '[]' : gap ?
                    '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']' :
                    '[' + partial.join(',') + ']';
                    gap = mind;
                    return v;
                }

                // If the replacer is an array, use it to select the members to be stringified.

                if (rep && typeof rep === 'object') {
                    length = rep.length;
                    for (i = 0; i < length; i += 1) {
                        if (typeof rep[i] === 'string') {
                            k = rep[i];
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                } else {

                    // Otherwise, iterate through all of the keys in the object.

                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                }

                // Join all of the member texts together, separated with commas,
                // and wrap them in braces.

                v = partial.length === 0 ? '{}' : gap ?
                '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}' :
                '{' + partial.join(',') + '}';
                gap = mind;
                return v;
        }
    }

    // If the JSON object does not yet have a stringify method, give it one.

    if (typeof JSON.stringify !== 'function') {
        JSON.stringify = function (value, replacer, space) {

            // The stringify method takes a value and an optional replacer, and an optional
            // space parameter, and returns a JSON text. The replacer can be a function
            // that can replace values, or an array of strings that will select the keys.
            // A default replacer method can be provided. Use of the space parameter can
            // produce text that is more easily readable.

            var i;
            gap = '';
            indent = '';

            // If the space parameter is a number, make an indent string containing that
            // many spaces.

            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }

                // If the space parameter is a string, it will be used as the indent string.

            } else if (typeof space === 'string') {
                indent = space;
            }

            // If there is a replacer, it must be a function or an array.
            // Otherwise, throw an error.

            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                    typeof replacer.length !== 'number')) {
                throw new Error('JSON.stringify');
            }

            // Make a fake root object containing our value under the key of ''.
            // Return the result of stringifying the value.

            return str('', { '': value });
        };
    }


    // If the JSON object does not yet have a parse method, give it one.

    if (typeof JSON.parse !== 'function') {
        JSON.parse = function (text, reviver) {

            // The parse method takes a text and an optional reviver function, and returns
            // a JavaScript value if the text is a valid JSON text.

            var j;

            function walk(holder, key) {

                // The walk method is used to recursively walk the resulting structure so
                // that modifications can be made.

                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }


            // Parsing happens in four stages. In the first stage, we replace certain
            // Unicode characters with escape sequences. JavaScript handles many characters
            // incorrectly, either silently deleting them, or treating them as line endings.

            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }

            // In the second stage, we run the text against regular expressions that look
            // for non-JSON patterns. We are especially concerned with '()' and 'new'
            // because they can cause invocation, and '=' because it can cause mutation.
            // But just to be safe, we want to reject all unexpected forms.

            // We split the second stage into 4 regexp operations in order to work around
            // crippling inefficiencies in IE's and Safari's regexp engines. First we
            // replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
            // replace all simple value tokens with ']' characters. Third, we delete all
            // open brackets that follow a colon or comma or that begin the text. Finally,
            // we look to see that the remaining characters are only whitespace or ']' or
            // ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

            if (/^[\],:{}\s]*$/
                    .test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@')
                        .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
                        .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

                // In the third stage we use the eval function to compile the text into a
                // JavaScript structure. The '{' operator is subject to a syntactic ambiguity
                // in JavaScript: it can begin a block or an object literal. We wrap the text
                // in parens to eliminate the ambiguity.

                j = eval('(' + text + ')');

                // In the optional fourth stage, we recursively walk the new structure, passing
                // each name/value pair to a reviver function for possible transformation.

                return typeof reviver === 'function' ?
                    walk({ '': j }, '') : j;
            }

            // If the text is not JSON parseable, then a SyntaxError is thrown.

            throw new SyntaxError('JSON.parse');
        };
    }
} ());




//--------------------------------------------Browser Detect-----------------------------------------------------//
//Browser name: BrowserDetect.browser
//Browser version: BrowserDetect.version
//OS name: BrowserDetect.OS
var BrowserDetect = {
	init: function () {
		this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
		this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| "an unknown version";
		this.OS = this.searchString(this.dataOS) || "an unknown OS";
	},
	searchString: function (data) {
		for (var i=0;i<data.length;i++)	{
			var dataString = data[i].string;
			var dataProp = data[i].prop;
			this.versionSearchString = data[i].versionSearch || data[i].identity;
			if (dataString) {
				if (dataString.indexOf(data[i].subString) != -1)
					return data[i].identity;
			}
			else if (dataProp)
				return data[i].identity;
		}
	},
	searchVersion: function (dataString) {
		var index = dataString.indexOf(this.versionSearchString);
		if (index == -1) return;
		return parseFloat(dataString.substring(index+this.versionSearchString.length+1));
	},
	dataBrowser: [
		{
			string: navigator.userAgent,
			subString: "Chrome",
			identity: "Chrome"
		},
		{ 	string: navigator.userAgent,
			subString: "OmniWeb",
			versionSearch: "OmniWeb/",
			identity: "OmniWeb"
		},
		{
			string: navigator.vendor,
			subString: "Apple",
			identity: "Safari",
			versionSearch: "Version"
		},
		{
			prop: window.opera,
			identity: "Opera"
		},
		{
			string: navigator.vendor,
			subString: "iCab",
			identity: "iCab"
		},
		{
			string: navigator.vendor,
			subString: "KDE",
			identity: "Konqueror"
		},
		{
			string: navigator.userAgent,
			subString: "Firefox",
			identity: "Firefox"
		},
		{
			string: navigator.vendor,
			subString: "Camino",
			identity: "Camino"
		},
		{		// for newer Netscapes (6+)
			string: navigator.userAgent,
			subString: "Netscape",
			identity: "Netscape"
		},
		{
			string: navigator.userAgent,
			subString: "MSIE",
			identity: "Explorer",
			versionSearch: "MSIE"
		},
		{
			string: navigator.userAgent,
			subString: "Gecko",
			identity: "Mozilla",
			versionSearch: "rv"
		},
		{ 		// for older Netscapes (4-)
			string: navigator.userAgent,
			subString: "Mozilla",
			identity: "Netscape",
			versionSearch: "Mozilla"
		}
	],
	dataOS : [
		{
			string: navigator.platform,
			subString: "Win",
			identity: "Windows"
		},
		{
			string: navigator.platform,
			subString: "Mac",
			identity: "Mac"
		},
		{
			   string: navigator.userAgent,
			   subString: "iPhone",
			   identity: "iPhone/iPod"
		},
		{
		    string: navigator.userAgent,
		    subString: "BlackBerry",
		    identity: "BlackBerry"
		},
		{
			string: navigator.platform,
			subString: "Linux",
			identity: "Linux"
		}
	]

};
BrowserDetect.init();