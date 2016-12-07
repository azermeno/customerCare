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
var frame=window.parent.frames[1]
var Datos;
var tableView;
var rdc;
function selecteditemchanged(s, e) {
    tablabind();
}
function valobtenidas(res, args) {
    alert("hola");
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
};
function VerTicket(n, x) {
    var wy = document.getElementById("hddInterno").value;
    window.open("http://www.redlab.com.mx/cibercalidad/detalleincidencia.html?inc=" + n + "&wy=" + wy + "&asu=" + x, "ticket" + n, "toolbar=no, location=no, directories=no, status=no, menubar=no, resizable=yes, copyhistory=no, width=800, height=600");
};
function RadGrid1_RowDataBound(sender, args) {
    var i = args.get_dataItem()["index"];
    var codigo = args.get_dataItem()["Codigo"];
    var celda = args.get_item().get_cell("codigo");
    var strt = '<a href="javascript:VerTicket(' + codigo + ',\'' + Datos.d[i].ao + '\')">' + codigo + '</a>';
    celda.innerHTML = strt;
    switch (Datos.d[i].eo) {
        case 1: args.get_item().get_element().style.background = "LightBlue";
            break;
        case 2: args.get_item().get_element().style.background = "#fffacd";
            if (Datos.d[i].vs > 0) {
                CustomerCare.Servicio.Validaciones(codigo,
                //                function (ab,cd){alert(ab)}, error);
                                function (res, arg) {
                                    //                                    alert("uorales");
                                    var val = JSON.parse(res);
                                    var j = 0;
                                    strt = '<span style="padding:0px">';
                                    while (j < val.length) {
                                        strt += '<img src="Images/seguimiento2.png" style="float:right; margin-right:2px;" title="' + val[j] + '"/>';
                                        j++;
                                    }
                                    strt += '</span>';
                                    celda.innerHTML += strt;
                                }, error);
                //                                args.get_item().get_cell("codigo").innerHTML += "Hay";
            }
            break;
        case 3: args.get_item().get_element().style.background = "#fffacd";
            if (Datos.d[i].vs > 0) {
                CustomerCare.Servicio.Validaciones(codigo,
                //                function (ab,cd){alert(ab)}, error);
                                function (res, arg) {
                                    //                                    alert("uorales");
                                    var val = JSON.parse(res);
                                    var j = 0;
                                    var strt = '<span style="padding:0px">';
                                    while (j < val.length) {
                                        strt += '<img src="Images/seguimiento2.png" style="float:right; margin-right:2px;" title="' + val[j] + '"/>';
                                        j++;
                                    }
                                    strt += '</span>';
                                    celda.innerHTML += strt;
                                }, error);
                //                                args.get_item().get_cell("codigo").innerHTML += "Hay";
            }
            break;
        case 6: args.get_item().get_element().style.background = "#eea0a0";
            break;
        case 4: args.get_item().get_element().style.background = "LightGray";
            break;
        default: args.get_item().get_element().style.background = "Gray";
    }
    //        alert(args.get_dataItem()["Estado"]);
    //    }



    //    var sb = new Sys.StringBuilder();



    //    sb.appendLine("<b>RadGrid1_RowDataBound</b><br />");



    //    for (var item in args.get_dataItem()) {

    //        sb.appendLine(String.format("{0} : {1}<br />", item, args.get_dataItem()[item]));

    //    }



    //    sb.appendLine("<br />");

    //    sb.appendLine("<br />");



    //    $get("<%= Panel1.ClientID %>").innerHTML += sb.toString();

}

function RowSelected(sender, eventArgs) {
    var i = eventArgs.get_itemIndexHierarchical();
    var fram = frame.document
    fram.getElementById("tdCodigo").innerHTML = Datos.d[i].co;
    fram.getElementById("lblAsunto").innerHTML = Datos.d[i].ao;
    fram.getElementById("lblDetalle").innerHTML = Datos.d[i].de;
    fram.getElementById("lblSolucion").innerHTML = Datos.d[i].sn;
    fram.getElementById("lblLevanto").innerHTML = Datos.d[i].lr.n;
    fram.getElementById("lblTipificacion").innerHTML = Datos.d[i].tn.n;
    switch (Datos.d[i].eo) {
        case 1:
            fram.getElementById("icSeguimiento").style.display = "inline";
            fram.getElementById("icCerrar").style.display = "none";
//            fram.getElementById("icRepactar").style.display = "none";
//            fram.getElementById("icImprimir").style.display = "none";
//            fram.getElementById("icEditar").style.display = "inline";
//            fram.getElementById("icEscalar").style.display = "none";
//            fram.getElementById("icAdjuntar").style.display = "inline";
            fram.getElementById("icValidar").style.display = "none";
            break;
        case 2:
            fram.getElementById("icSeguimiento").style.display = "inline";
            fram.getElementById("icCerrar").style.display = "inline";
            fram.getElementById("icCerrar").title = "Cerrar";
            fram.getElementById("icCerrar").setAttribute("onclick","cerrar()");
//            fram.getElementById("icRepactar").style.display = "inline";
//            fram.getElementById("icImprimir").style.display = "inline";
//            fram.getElementById("icEditar").style.display = "inline";
//            fram.getElementById("icEscalar").style.display = "inline";
//            fram.getElementById("icAdjuntar").style.display = "inline";
            fram.getElementById("icValidar").style.display = "inline";
            break;
        case 3:
            fram.getElementById("icSeguimiento").style.display = "inline";
            fram.getElementById("icCerrar").style.display = "inline";
            fram.getElementById("icCerrar").title = "Cerrar";
            fram.getElementById("icCerrar").setAttribute("onclick", "cerrar()");
//            fram.getElementById("icRepactar").style.display = "inline";
//            fram.getElementById("icImprimir").style.display = "inline";
//            fram.getElementById("icEditar").style.display = "inline";
//            fram.getElementById("icEscalar").style.display = "inline";
//            fram.getElementById("icAdjuntar").style.display = "inline";
            fram.getElementById("icValidar").style.display = "inline";
            break;
        case 6:
            fram.getElementById("icSeguimiento").style.display = "inline";
            fram.getElementById("icCerrar").style.display = "inline";
            fram.getElementById("icCerrar").title = "Validar";
            fram.getElementById("icCerrar").setAttribute("onclick", "validar(3)");
//            fram.getElementById("icRepactar").style.display = "none";
//            fram.getElementById("icImprimir").style.display = "inline";
//            fram.getElementById("icEditar").style.display = "inline";
//            fram.getElementById("icEscalar").style.display = "inline";
//            fram.getElementById("icAdjuntar").style.display = "inline";
            fram.getElementById("icValidar").style.display = "none";
            break;
        case 4:
            fram.getElementById("icSeguimiento").style.display = "inline";
            fram.getElementById("icCerrar").style.display = "none";
//            fram.getElementById("icRepactar").style.display = "none";
//            fram.getElementById("icImprimir").style.display = "inline";
//            fram.getElementById("icEditar").style.display = "none";
//            fram.getElementById("icEscalar").style.display = "none";
//            fram.getElementById("icAdjuntar").style.display = "none";
            fram.getElementById("icValidar").style.display = "none";
            break;
        case 5:
            fram.getElementById("icSeguimiento").style.display = "inline";
            fram.getElementById("icCerrar").style.display = "none";
//            fram.getElementById("icRepactar").style.display = "none";
//            fram.getElementById("icImprimir").style.display = "inline";
//            fram.getElementById("icEditar").style.display = "none";
//            fram.getElementById("icEscalar").style.display = "none";
//            fram.getElementById("icAdjuntar").style.display = "none";
            fram.getElementById("icValidar").style.display = "none";
            break;
    }
    frame.vioeve = 0;
    var rts = frame.$find("RadTabStrip1");
    if (rts.get_selectedTab().get_index() == 1)
        frame.evebind();
}
function tablabind() {
//    alert("hola");
    var str="";
    str = str + $find("rcbResponsable").get_value() + "!";
    str = str + $find("rcbEscalador").get_value() + "!";
    str = str + $find("rcbSolicitante").get_value() + "!";
    str = str + $find("rcbLevantador").get_value() + "!";
    var estds = 0;
    if (document.getElementById("cbxPretickets").checked) estds = estds + 1;
    if (document.getElementById("cbxAbiertas").checked) estds = estds + 2;
    if (document.getElementById("cbxValidacion").checked) estds = estds + 4;
    if (document.getElementById("cbxCerradas").checked) estds = estds + 8;
    if (document.getElementById("cbxCanceladas").checked) estds = estds + 16;
    str = str + estds;
    var pageSize = tableView.get_pageSize();
    var currentPageIndex = tableView.get_currentPageIndex();
    $find("RadAjaxLoadingPanel1").show("RadGrid1");
    var usu = document.getElementById("hddUsuario").value;
    CustomerCare.Servicio.GetDataAndCount(currentPageIndex * pageSize, pageSize, str, usu, datosobtenidos, error);
    tableView.clearSelectedItems();
}
function load() {
    tableView = $find("RadGrid1").get_masterTableView();
    document.getElementById("rcbEscalador_DropDown").firstChild.setAttribute("onclick","alert('ya')");
    tablabind();
    rdc=$find("rdcAcciones");
}
function RadGrid1_Command(sender, args) {
    args.set_cancel(true);
    var pageSize = sender.get_masterTableView().get_pageSize();
    var sortExpressions = sender.get_masterTableView().get_sortExpressions();
    var sortExpressionsAsSQL = sortExpressions.toString();
    //    $find("<%= RadAjaxLoadingPanel1.ClientID %>").show("<%= RadGrid1.ClientID %>");
    tablabind();
}

function datosobtenidos(res, args) {
//    alert(res);
    Datos = JSON.parse(res)
    cont = new Array()
    cont2 = new Array()
    var i = 0;
    while (i < Datos.d.length) {
        str = '{"index":'+i+',"Codigo":' + Datos.d[i].co + ',"Asunto":"' + Datos.d[i].ao.replace(/"/g, '\\\"') + '","Apertura":"' + Datos.d[i].aa + '","Limite":"' + Datos.d[i].le + '","Clausura":"' + Datos.d[i].ca + '","Responsable":"' + Datos.d[i].re.n.replace(/"/g, '\\\"') + '","Solicitante":"' + Datos.d[i].se.n.replace(/"/g, '\\\"') + '"}';
//                        alert(str);
        a = JSON.parse(str);
        cont.push(a);
        i++;
    }
    //            alert(JSON.stringify(JSON.parse('{"Codigo":1,"Asunto}')))
    tableView.set_dataSource(cont);
    tableView.set_virtualItemCount(Datos.c);
    tableView.dataBind();
    $find("RadAjaxLoadingPanel1").hide("RadGrid1");
}

function guardarSeguimiento() {
    cod = document.getElementById("hddCodigo").value;
    obs = document.getElementById("txtObs").value;
    usu = document.getElementById("hddUsuario").value;
    x = document.getElementById("chkEnviarMail");
    if (x.checked)
        env = "si";
    else
        env = "";
    CustomerCare.Servicio.Seguimiento(cod, obs, usu, env, segGuardado, error);
}

function segGuardado(res, e) {
    rdc.set_closed(true);
    document.getElementById("hddCodigo").value = "";
    frame.evebind();
}

function valEnviada(res, e) {
    rdc.set_closed(true);
    document.getElementById("hddCodigo").value = "";
    tablabind();
}

function salirCerrar() {
    cod = document.getElementById("hddCodigo").value;
    sol = document.getElementById("TextBox3").value;
    CustomerCare.Servicio.CerrarTicket(cod,sol,incCerrada,error);
}

function incCerrada(res, e) {
    alert("Ticket cerrado");
    rdc.set_closed(true);
    document.getElementById("hddCodigo").value = "";
}

function error(e) {
    alert(e.get_message());
    $find("RadAjaxLoadingPanel1").hide("RadGrid1");
}
