<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Salir.aspx.cs" Inherits="CustomerCare.Salir" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
	<link href="favicon.ico" rel="shortcut icon" type="image/x-icon"/>
	<link href="favicon.ico" rel="icon" type="image/x-icon"/>
    <title>¡Hasta luego!</title>
</head>
<body>
    <form id="form1" runat="server">
        <img id="background" alt="" src="<%=ImagesFolder%>background.png"/>
        <div id="scroller" align="center"><ASP:Panel id="panDatos" runat="server" height="520px">
			  <table width="100%" align="center" style="font-family:Verdana, 'Bitstream Vera Sans', 'DejaVu Sans', Tahoma, Geneva, Arial, Sans-serif">
				<tr>
					<td style="HEIGHT: 187px; vertical-align:middle;" colspan="4" align="center">
                        <img src="<%=ImagesFolder%>logo.png" alt="Cibernética"/></td></tr>
				<tr>
					<td style="height: 35px; text-align:center;" colspan="4">
                        <asp:Label ID="Label2" runat="server" font-names="Verdana, 'Bitstream Vera Sans', 'DejaVu Sans', Tahoma, Geneva, Arial, Sans-serif" font-size="Larger" 
                            forecolor="White" Text="Gracias por utilizar el sistema"></asp:Label>
                        <asp:HiddenField ID="hflUsuario" runat="server" />
                    </td></tr>
				<tr>
					<td colspan="4" style="HEIGHT: 120px" valign="top" align="center">
						&nbsp;</td></tr>
				<tr>
					<td colspan="4" valign="top" align="center">
				        &nbsp;</td></tr>
				<tr>
					<td colspan="4" valign="top" align="center">
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="index.aspx">Volver a entrar</asp:HyperLink>
				  </td></tr>
			  </table></ASP:Panel>  </div>
    </form>
</body>
</html>
