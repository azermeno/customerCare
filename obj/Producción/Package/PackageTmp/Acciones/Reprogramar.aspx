<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reprogramar.aspx.cs" Inherits="CustomerCare.Acciones.Reprogramar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript">

        var Principal = window.top;
        var sender = "<%= Request.QueryString["sen"]%>"
        switch (sender) {
            case "TablaTickets":
                var frame = Principal.$find("rtsPrincipal").findTabByValue(sender + "").get_pageView().get_element().getElementsByTagName("iframe")[0].contentWindow.frames[0];
                break;
            default:
                var frame = Principal.$find("rtsPrincipal").findTabByValue(sender + "").get_pageView().get_element().getElementsByTagName("iframe")[0].contentWindow;
                break;
        }

        function salir() {
            var men = document.getElementById("rtbObs").value;
            var cod = "<%= Request.QueryString["cod"]%>";
            var fechini = $find("rdpInicio").get_selectedDate();
                var fecha = $find("rdpLimite").get_selectedDate();
                var fecstr = (fecha.getDate() < 10 ? "0" + fecha.getDate() : fecha.getDate()) + "/" + (fecha.getMonth() + 1 < 10 ? "0" + (fecha.getMonth() + 1) : fecha.getMonth() + 1) + "/" + fecha.getFullYear() + " " + (fecha.getHours() < 10 ? "0" + fecha.getHours() : fecha.getHours()) + ":" + (fecha.getMinutes() < 10 ? "0" + fecha.getMinutes() : fecha.getMinutes());
                var fecstrini = (fechini.getDate() < 10 ? "0" + fechini.getDate() : fechini.getDate()) + "/" + (fechini.getMonth() + 1 < 10 ? "0" + (fechini.getMonth() + 1) : fechini.getMonth() + 1) + "/" + fechini.getFullYear() + " " + (fechini.getHours() < 10 ? "0" + fechini.getHours() : fechini.getHours()) + ":" + (fechini.getMinutes() < 10 ? "0" + fechini.getMinutes() : fechini.getMinutes());
                Principal.CustomerCare.Servicio.Reprogramar(cod, fecstrini, fecstr, men, Principal.datosusuario.co, realizada, Principal.error);
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
                switch (sender) {
                    case "TablaTickets":
                        frame.document.getElementById("hddCodigo").value = "";
                        frame.tablabind();
                        break;
                    default:
                        frame.location.reload();
                }
            }
    </script>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <div>
        <asp:Label ID="LInicio" runat="server" Text="Inicio"></asp:Label>
        <telerik:RadDateTimePicker ID="rdpInicio" Runat="server">
        </telerik:RadDateTimePicker>
        <asp:Label ID="LLimite" runat="server" Text="Limite"></asp:Label>
        <telerik:RadDateTimePicker ID="rdpLimite" Runat="server">
        </telerik:RadDateTimePicker>
    
        <br />
                            <telerik:RadTextBox ID="rtbObs" runat="server" Height="150px" 
                                TextMode="MultiLine" Width="100%" EmptyMessage="Observaciones">
                            </telerik:RadTextBox>
    
    </div>
    <p>
                        <input type="button" onclick="salir()" value="Guardar" /></p>
    </form>
</body>
</html>
