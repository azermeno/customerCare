<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Repactar.aspx.cs" Inherits="CustomerCare.Scripts.Repactar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript">

        var Principal = window.top;
        var sender = "<%= Request.QueryString["sen"]%>"
        switch (sender) {
        case "TablaTickets":
         var frame = Principal.$find("rtsPrincipal").findTabByValue(sender+"").get_pageView().get_element().getElementsByTagName("iframe")[0].contentWindow.frames[0];
         break;
         default:
         var frame = Principal.$find("rtsPrincipal").findTabByValue(sender+"").get_pageView().get_element().getElementsByTagName("iframe")[0].contentWindow;
         break;
         }
         
        function salir() {
                var men = document.getElementById("rtbObs").value;
                var cod = "<%= Request.QueryString["cod"]%>";
                var fecha = $find("rdpRepactar").get_selectedDate();
                var fecstr = (fecha.getDate()<10?"0"+fecha.getDate():fecha.getDate()) + "/" + (fecha.getMonth()+1<10?"0"+(fecha.getMonth()+1):fecha.getMonth()+1) + "/" + fecha.getFullYear() + " " + (fecha.getHours()<10?"0"+fecha.getHours():fecha.getHours())+":"+(fecha.getMinutes()<10?"0"+fecha.getMinutes():fecha.getMinutes());
                    Principal.CustomerCare.Servicio.Repactar(cod, fecstr, men, Principal.datosusuario.co, realizada, Principal.error);
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
    
        <telerik:RadDateTimePicker ID="rdpRepactar" Runat="server">
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
