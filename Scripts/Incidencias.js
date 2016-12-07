function load() {
}

function cerrar(a) {
    x = $find("rdcCerrar");
    x.set_title("Cerrar incidencia " + a);
    x.set_closed(false);
    document.getElementById("hddCodigo").value = a;
}

function seguimiento(a) {
    x = $find("rdcSeguimiento");
    x.set_title("Seguimiento incidencia " + a);
    document.getElementById("hddCodigo").value = a;
    x.set_closed(false);
}

function guardarSeguimiento() {
    cod = document.getElementById("hddCodigo").value;
    obs = document.getElementById("rdcSeguimiento_C_txtObs").value;
    usu = document.getElementById("hddUsuario").value;
    x = document.getElementById("rdcSeguimiento_C_chkEnviarMail");
    if (x.checked)
        env = "si";
    else
        env = "";
    CustomerCare.Servicio.Seguimiento(cod, obs, usu, env, segGuardado, error);
}

function segGuardado(res, e) {
    x = $find("rdcSeguimiento");
    x.set_closed(true);
    document.getElementById("rdcSeguimiento_C_txtObs").value = "";
    document.getElementById("hddCodigo").value = "";
}


function salirCerrar() {
    cod = document.getElementById("hddCodigo").value;
    sol = document.getElementById("rdcCerrar_C_TextBox2").value;
    det = document.getElementById("rdcCerrar_C_TextBox3").value;
    CustomerCare.Servicio.CerrarIncidencia(cod,sol,det,incCerrada,error);
}

function incCerrada(res, e) {
    alert("Incidencia cerrada");
    x = $find("rdcCerrar");
    x.set_closed(true);
    document.getElementById("rdcCerrar_C_TextBox2").value = "";
    document.getElementById("rdcCerrar_C_TextBox3").value = "";
    document.getElementById("hddCodigo").value = "";
}

function error(e) {
    alert("Uy! hubo un error, porfavor vuelve a intentarlo.");
}
