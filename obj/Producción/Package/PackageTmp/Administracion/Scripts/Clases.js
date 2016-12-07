var Principal = window.parent;

$(function () {
    $(window).load(function () {
        var hflTemplate = $("#hflTemplate");
        var hflDatosExtra = $("#" + serverVars.hflDatosExtraId);
        hflDatosExtra.parent().append(Principal.jsonToHtml(hflTemplate.val()));
        Principal.llenaJson(window, hflDatosExtra.val());
    });
    $("#Guardar").mousedown(function () {
        console.log("cambiando hflDatosExtra");
        var hflDatosExtra = $("#" + serverVars.hflDatosExtraId);
        hflDatosExtra.val(Principal.htmlToJson(window));
        return true;
    });
});

