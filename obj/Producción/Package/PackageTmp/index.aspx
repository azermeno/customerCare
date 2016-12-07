<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="CustomerCare.index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"> 
	<meta name="viewport" content="width=device-width, initial-scale=1.0"> 
	
    <title>CustomerCare</title>
    <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="favicon.ico" rel="icon" type="image/x-icon" />
	<meta name="keywords" content="css3, login, form, customer, input, submit, button, html5, placeholder" />
	<meta name="author" content="Jeknel" />
	<link rel="stylesheet" type="text/css" href="css/style.css" />
	
    <style type="text/css">
        .box {
            background-repeat: no-repeat;
            height: 100px;
            width: 100%;
            position: initial !important;
            top: 0 !important;
        }
        .tabla {
            box-shadow: 0 0 1px 1px white; 
            text-align:center; 
            width: 40%; 
            height: 35%; 
            max-height: 250px; 
            background-color: #4b6c9e;
            border:2px solid black; 
            border-radius: 15px 15px;
            position: absolute; 
            top:0; 
            bottom: 0; 
            left: 0; 
            right: 0; 
            margin: auto; 
            padding: 0 0 0 0;
            -webkit-box-reflect: below 0px -webkit-gradient(linear, 
            left top, left bottom, from(transparent), 
            color-stop(70%, transparent), 
            to(rgba(255,255,255,0.5)));
        }
        .label {
            /*display: block;
            font: bold 500 arial, verdana !important;*/
            font-family: arial, verdana;
            font-size: x-large;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
	   <%-- <div><a class="image image-full"><img src="images/logo.png" alt="" /></a></div></div>
			<div class="toolbar1"><a class="image image-full"><img src="images/logo2.png"/></a></div>
					<div class="container">					
						
							

					<div class="support-note">
						<span class="note-ie">Lo siento, esta pagina es sólo compatible con navegadores modernos.</span>
					</div>
							
				<div class="form-1">
						<section class="main">
										   <p class="field">
									<input id="Usuario" type="text" name="login" placeholder="Nombre de Usuario">
									<i class="icon-user icon-large"></i>
								</p>
									<p class="field">
										<input id="Contras" type="password" name="password" placeholder="Contraseña">
										<i class="icon-lock icon-large"></i>
								</p>
								<p class="submit">
										<button id="Boton" onclick="BotonAceptar();"><i class="icon-arrow-right icon-large"></i></button>									
								</p><br><p class="right"><label id="Informativo" style="margin: auto 0px auto 0px" size="9px">Última Actualización</label></p>
						</section>
						</div>
					</div>--%>
        <img id="background" alt="" src="<%=ImagesFolder%>background.png" />
        <div id="scroller" style="text-align: center; ">
            <img id="agua1" alt="" src="<%=ImagesFolder%>agua1.png" />
            <img id="agua2" alt="" src="<%=ImagesFolder%>agua2.png" />
            <img id="logo" alt="" src="<%=ImagesFolder%>logo.png" /><br />
            <br />
            <asp:HiddenField
                ID="hfdUsu" runat="server" />


            <div class="tabla">
                <asp:Panel ID="Panel1" runat="server" CssClass="box" HorizontalAlign="Center">
                    <%-- <ASP:Panel id="panDatos" runat="server" backimageurl="" height="520px" width="540px">--%>
                    <div style="width: 99%; height: auto; vertical-align: bottom; text-align: center;">
                        <asp:CustomValidator ID="Validador" runat="server" ErrorMessage="¡Nombre de usuario o contraseña incorrecta!" ForeColor="White" OnServerValidate="Validador_ServerValidate" Enabled="true" Visible="true" Display="Static" />
                        <asp:CustomValidator ID="ValidadorCambioContrasena" runat="server" ErrorMessage="¡Contraseña incorrecta!" ForeColor="White" OnServerValidate="ValidadorCambioContrasena_ServerValidate" Enabled="false" Visible="false" />
                    </div>
				    <div style="width: 99%; height: auto;">
                        <div style="width: 50%; float: left; text-align:right;">
                            <asp:Label ID="Label1" runat="server" Text="Usuario: " CssClass="label"></asp:Label>
                        </div>
                        <div style="text-align: left; width:50%; float: left;">
                            <asp:TextBox ID="edUsuario" runat="server" Width="100%" MaxLength="20" ToolTip="Escribe tu nombre de usuario"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 99%; height: auto; float:left;">
                        <div style="width: 50%; float: left; text-align:right;">
                            <asp:Label ID="Label4" runat="server" Text="Contraseña: " CssClass="label"></asp:Label>
                        </div>
                        <div style="text-align: left; width:50%; float: left;">
                            <asp:TextBox ID="edPassword" runat="server" Width="100%" MaxLength="30" ToolTip="Introduce tu contraseña" TextMode="Password"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 99%; height: auto; float:left;">
                        <div style="text-align: left; font-size: 12px;">
                            <asp:Label ID="Label5" runat="server" Text="Repetir" Visible="false"></asp:Label>
                        </div>
                        <div>
                            <asp:CheckBox ID="chbRecuerdame" runat="server" Text="Recuérdame" />
                            <asp:TextBox ID="edPassword2" runat="server" Width="150px" MaxLength="30"
                                ToolTip="Repite la contraseña nueva"
                                TextMode="Password" Visible="false"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 100%; height: auto;">
                        <div style="text-align: left; vertical-align: bottom;">
                            <asp:Button ID="btnLogin" runat="server" Text="Continuar" Width="100%" OnClick="btnLogin_Click"></asp:Button>
                        </div>
                    </div>
                    <div style="width: 99%; height: auto; vertical-align: top; text-align: center;">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="¡Escriba su nombre de usuario!" ControlToValidate="edUsuario" ForeColor="White" Font-Size="X-Small" Enabled="true" Visible="true"></asp:RequiredFieldValidator><asp:CompareValidator
                            ID="Iguales" runat="server" Font-Size="X-Small" ErrorMessage="¡Las contraseñas no son iguales!" Enabled="false" ControlToValidate="edPassword2" ControlToCompare="edPassword" Visible="false"></asp:CompareValidator>
                    </div>
                    <div style="width: 99%; height: auto; vertical-align: top; text-align: center;">
                            <asp:Label ID="Label2" runat="server" ForeColor="White" Font-Size="Larger">Su sesión ha caducado</asp:Label><asp:Label ID="Label6" runat="server" ForeColor="White" Font-Size="Larger" Visible="false">Debe cambiar su contraseña</asp:Label>
                    </div>
                    <div style="width: 99%; height: auto; vertical-align: top; text-align: center;">
                        <asp:Label ID="Label3" runat="server" ForeColor="White" Font-Size="X-Small">Por favor reingrese su usuario y contraseña</asp:Label>
                    </div>
                </asp:Panel>
            </div>


        </div>
    </form>
</body>
</html>
