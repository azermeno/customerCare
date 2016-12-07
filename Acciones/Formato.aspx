<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Formato.aspx.cs" Inherits="CustomerCare.Formato" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @media screen {
        html, body, form
        {
            height: 100%;
            margin: 0px;
            overflow: hidden;
        }

.RadToolBarDropDown .rtbChecked .rtbWrap 
{ 
    font-weight: bold; 
}

.rtbIcon
{
    width:13px;
    height:13px;
    background:url(Images/checkbox.png);
    background-position: 13px 0px;
}

.RadToolBarDropDown .rtbChecked .rtbWrap .rtbIcon
{
    background-position: 0px 0px;
}
#divContenido
    {
        width:100%; 
        top:32px; 
        bottom:0px; 
        position:absolute; 
        overflow:auto;
    }
}
@media print, screen and (max-width: 8.5in) 
{
  html, body, form
    {
        width: 100%;
        margin: 0pt;
        overflow: hidden;
    }
    #RadToolBar1 
    {
        display: none;
    }
#divContenido
    {
        width:100%; 
        overflow:hidden;
    }
}
        </style>
</head>
<body>
<script type="text/javascript">
    var Principal = window.top;
    function rtbClicking(sen, args) {
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            console.log(pair)
        }
        if (args.get_item().get_value() == "guardar") {
            Principal.estampaInputs(document);
            Principal.CustomerCare.Servicio.guardaVersionFor(cdg, tic, Principal.datosusuario.co, document.getElementById("divContenido").innerHTML, guardado, Principal.error);
        }
        else if (args.get_item().get_text() == "Version") {
            args.set_cancel(true);
        }
        else if (args.get_item().get_value() == "imprimir") {
            print();
        }
    }
    function guardado(res, args) {
      Principal.radalert(res);
      Principal.ventanaPestanaValor(tic).location.reload();
      location.reload();
    }
</script>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadToolBar ID="RadToolBar1" runat="server" 
        onbuttonclick="RadToolBar1_ButtonClick" Width="100%" 
        onclientbuttonclicking="rtbClicking">
        <Items>
            <telerik:RadToolBarButton runat="server" Text="Guardar" Value="guardar" PostBack="false">
            </telerik:RadToolBarButton>
            <telerik:RadToolBarDropDown runat="server" Text="Versión">
            </telerik:RadToolBarDropDown>
            <telerik:RadToolBarButton runat="server" Text="Imprimir" Value="imprimir" PostBack="false">
            </telerik:RadToolBarButton>
        </Items>
    </telerik:RadToolBar>
    <div id="divContenido" style="width:100%; top:32px; bottom:0px; position:absolute; overflow:auto;" runat="server">
    </div>
    </form>
</body>
</html>
