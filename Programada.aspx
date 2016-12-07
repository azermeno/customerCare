<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Programada.aspx.cs" Inherits="CustomerCare.Programada" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    body
    {
        font-family:'segoe ui', arial, sans-serif;
        font-size:12px;
        background-color:#FFFDE2;
    }
    .titulo
    {
        font-weight:bold;
        font-size:14px;
    }
    </style>
    <script type="text/javascript">
        var Principal = parent.parent;     
    function showMenu(e) {
            var contextMenu = Principal.$find("rcmTipificacion");
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
            var el = frameElement.offsetParent;
            while (el) {
                tempX += el.offsetLeft;
                tempY += el.offsetTop;
                el = el.offsetParent;
            }
            Principal.rcmTipificacion_opener = "programada";
            contextMenu.showAt(tempX, tempY);
        }

        function showMenuK(n) {
            var contextMenu = Principal.$find("rcmTipificacion");
            Principal.rcmTipificacion_opener = "programada";
            var tempX = 0
            var tempY = 0
            var el = frameElement.offsetParent;
            while (el) {
                tempX += el.offsetLeft;
                tempY += el.offsetTop;
                el = el.offsetParent;
            }
            if (n == 9) {
                contextMenu.showAt(tempX, tempY);
                contextMenu.get_items().getItem(0).focus();
            }
        }

        function escribirLabel(sender, args) {
            var texto = ' - ?';
            var item = args.get_item();
            var codigo = item.get_value();
            var n = item.get_level();
            if (codigo.substring(0, 1) == 'h') {
                texto = '';
                codigo = codigo.substring(1)
            }
            for (i = 0; i <= n; i++) {
                texto = ' - ' + item.get_text() + texto;
                item = item.get_parent();
            }
            texto = texto.substring(3);
            var textbox = $find("rtbTipificacion");
            textbox.set_value(texto);
            document.getElementById('hflTipificacion').value = codigo;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" Runat="server">
    </telerik:RadSkinManager>
    <asp:HiddenField ID="hflCodigo" runat="server" />
    <asp:Label ID="lblGeneral" runat="server" Text="General" CssClass="titulo"></asp:Label>
    <br />
    <asp:Label ID="lblAsunto" runat="server" Text="Asunto*:"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="rtbAsunto" ErrorMessage="RequiredFieldValidator" 
                            ForeColor="#FF3300">El asunto es necesario</asp:RequiredFieldValidator>
    <telerik:RadTextBox ID="rtbAsunto" runat="server" Skin="Default" Width="100%">
    </telerik:RadTextBox>
    <br />
    <asp:Label ID="lblDetalle" runat="server" Text="Detalle:"></asp:Label>
    <telerik:RadTextBox ID="rtbDetalle" runat="server" Skin="Default" Width="100%" TextMode="MultiLine" Rows="5">
    </telerik:RadTextBox>
    <br />
    <asp:Label ID="lblTipificacion" runat="server" CssClass="label" 
                            Text="Tipificación*: "></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="rtbTipificacion" ErrorMessage="RequiredFieldValidator" 
                            ForeColor="#FF3300">Favor de tipificar</asp:RequiredFieldValidator>
                        <br />
                        <telerik:RadTextBox ID="rtbTipificacion" Runat="server" EmptyMessage="Seleccione..." 
                            ReadOnly="True" Width="60%" TabIndex="3" Skin="Default">
                        </telerik:RadTextBox>
                    <asp:HiddenField ID="hflTipificacion" runat="server" />
    <hr />
    <asp:Label ID="Label1" runat="server" Text="Asignaciones" CssClass="titulo"></asp:Label>
    <telerik:RadListBox ID="rlbSolicitante" runat="server" 
        DataSourceID="sdsSolicitante" DataTextField="nombre" DataValueField="codigo">
    </telerik:RadListBox>
    <asp:SqlDataSource ID="sdsSolicitante" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString2 %>" 
        ProviderName="<%$ ConnectionStrings:ConnectionString2.ProviderName %>" SelectCommand="select * from (
select nombre, cast(codigo as varchar) + '.1' as codigo from unidades where activa=1
union
select nombre, cast(codigo as varchar) + '.3' as codigo from zonas where activa=1
union
select nombre, cast(codigo as varchar) + '.6' as codigo from sitios where activo=1
union
select nombre, cast(codigo as varchar) + '.4' as codigo from usuarios where codigo in (select usuario from clientes) and activo = 1) sols
order by nombre"></asp:SqlDataSource>
    <telerik:RadListBox ID="rlbResponsable" runat="server">
    </telerik:RadListBox>
    <hr />
    <telerik:RadButton ID="rbtGuardar" runat="server" Text="Guardar" 
        onclick="rbtGuardar_Click">
    </telerik:RadButton>
    </form>
</body>
</html>
