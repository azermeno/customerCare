<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.aspx.cs" Inherits="CustomerCare.Administracion.Usuarios" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="Estilos/Usuarios.css" rel="stylesheet" />
    <script type="text/javascript">
        function tam_fram() {
            if (window.innerHeight) {
                //navegadores basados en mozilla 
                espacio_iframe = window.innerHeight - 33
            } else {
                if (document.body.clientHeight) {
                    //Navegadores basados en IExplorer, es que no tengo innerheight 
                    espacio_iframe = document.body.clientHeight - 33
                } else {
                    //otros navegadores 
                    espacio_iframe = 478
                }
            }
            var valor;
            valor = espacio_iframe.toString() + "px";
            document.getElementById("list").style.height = valor;
            //document.getElementById("Lista").style.height = (espacio_iframe-200).To()+"px";
            document.getElementById("det").style.height = valor;
            //document.write('<iframe frameborder="0" src="recursos/PDFs/AntecedentesRevolcucion.pdf" class="pdf" height="' + espacio_iframe + '">')

        }

        function estado() {
            if (document.getElementById("Requiriente").checked == true) {
                document.getElementById("Unidades").style.display = "block";
                document.getElementById("etqUnidad").style.display = "block";
            }
            else {
                document.getElementById("Unidades").style.display = "none";
                document.getElementById("etqUnidad").style.display = "none";
            }
        }

        function oculta() {
            document.getElementById("alerta_fondo").style.display = "none";
            document.getElementById("alerta_mns").style.display = "none";
        }

        function confirmacion() {
            document.getElementById("aef").style.display = "inline";
            document.getElementById("aem").style.display = "inline";
            document.getElementById("emns").textContent = "¿Esta seguro que desea eliminar el registro del usuario " + document.getElementById("Lista").SelectedText + "?";
        }

        function cancela() {
            document.getElementById("aef").style.display = "none";
            document.getElementById("aem").style.display = "none";
        }

        function SearchList() {
            var l = document.getElementById("Lista");
            var tb = document.getElementById("Buscar");
            for (var i = 0; i < l.options.length; i++)
                l.options[i].style.display = "block";

            if (tb.value == "") {
                for (var i = 0; i < l.options.length; i++) {
                    //l.options[i].style.display = "block";
                    l.options[i].className = "normal";
                }
            }
            else {
                for (var i = 0; i < l.options.length; i++) {
                    if (l.options[i].textContent.toLowerCase().indexOf(tb.value.toLowerCase()) != -1) {
                        //l.options[i].style.display = "block";
                        l.options[i].className = "resaltar";
                    }
                    else {
                        l.options[i].style.display = "none";
                        //l.options[i].display = "none";
                        l.options[i].className = "difuminar";
                    }
                }
            }
        }

        function ClearSelection(lb) {
            lb.selectedIndex = -1;
        }
        //function perfil() {
        //    var perf = ","+document.getElementById("Perfiles").value+",";
        //    var CHK = document.getElementById("Permisos");
        //    var checkbox = CHK.getElementsByTagName("input");
        //    var label = CHK.getElementsByTagName("label");
        //    alert(perf);
        //    for (var i = 0; i < checkbox.length; i++) {
        //        alert(checkbox[i].value);
        //        if (perf.contains(","+checkbox[i].value+",")) {
        //            alert("Selected = " + label[i].innerHTML);
        //        }
        //    }
        //    return false;
        //}
    </script>
</head>
<body onclick="tam_fram();" onkeypress="tam_fram();" onload="tam_fram();" onresize="tam_fram();" onchange="tam_fram();" onsubmit="tam_fram();">
<%--<body >--%>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="aef" class="alerta_fondo">
        </div>
        <div id="aem" class="alerta_mns">
            <div class="alerta_texto">
                <asp:Label ID="emns" runat="server" Text="" ClientIDMode="Static"></asp:Label>
            </div>
            <div class="alerta_btn">
                <asp:Button ID="Si" CssClass="btn_mediano" runat="server" Text="Si" OnClick="Si_Click" />
                <input id="No" type="button" value="No" class="btn_mediano" onclick="cancela();" />
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="Operacion" runat="server" />
                <asp:HiddenField ID="NumPerfil" runat="server" />
                <div id="alerta_fondo" runat="server" style="display: none !important;" class="alerta_fondo">
                </div>
                <div id="alerta_mns" runat="server" style="display: none !important;" class="alerta_mns">
                    <div class="alerta_texto">
                        <asp:Label ID="mns" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                    </div>
                    <div class="alerta_btn">
                        <input id="Aceptar" type="button" value="Aceptar" class="btn_mediano" onclick="oculta();" />
                    </div>
                </div>
                <div class="cont_filtro">
                    <div class="opcion_filtro">
                        <asp:CheckBox ID="Internos" runat="server" Text="Internos" Checked="True" ClientIDMode="Static" />
                        <asp:CheckBox ID="Clientes" runat="server" Text="Clientes" Checked="True" ClientIDMode="Static" />
                        <asp:CheckBox ID="Solicitantes" runat="server" Text="Solicitantes" Checked="True" ClientIDMode="Static" />
                        <asp:CheckBox ID="Propuestos" runat="server" Text="Propuestaos" Checked="True" Visible="False" ClientIDMode="Static" />
                        <asp:CheckBox ID="sClasificar" runat="server" Text="Sin Clasificar" Checked="False" ClientIDMode="Static" />
                        <asp:CheckBox ID="sActivos" runat="server" Text="Solo Activos" Checked="True" ClientIDMode="Static" />
                        <asp:Button ID="Filtrar" CssClass="btn_chico" runat="server" Text="Realizar Filtro" OnClick="Filtrar_Click" />
                    </div>
                </div>
                <div id="list" class="cont">
                    <div class="botones">
                        <asp:ImageButton ID="Nuevo" runat="server" ImageUrl="~/Administracion/Estilos/Imagenes/nuevo-usuario-icono-8071-32.png" ToolTip="Nuevo Usuario" OnClientClick="tamfram();" ClientIDMode="Static" OnClick="Nuevo_Click" />
                        <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/Administracion/Estilos/Imagenes/editar-un-marcador-de-nombre-icono-8476-32.png" ToolTip="Modificar Datos del Usuario" OnClick="Modificar_Click" ClientIDMode="Static" />
                        <asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/Administracion/Estilos/Imagenes/eliminar-usuario-icono-5252-32.png" ToolTip="Eliminar Datos del Usuario" ClientIDMode="Static" OnClientClick="confirmacion();" Visible="False" />
                    </div>
                    <div class="lista">
                        <asp:TextBox ID="Buscar" runat="server" onkeyup="SearchList();" Text="" CssClass="busc" ClientIDMode="Static" />
                        <asp:ListBox ID="Lista" CssClass="lista" name="lista" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="Lista_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                    </div>
                </div>
                <div id="det" class="detalle">
                    <%--<div class="campo">
                        <table>
                            <tr>
                                <td>Código
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Codigo" runat="server" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                    <div class="campo">
                        <table>
                            <tr>
                                <td><asp:Label ID="tCodigo" Text="Código" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Codigo" runat="server" ClientIDMode="Static" ReadOnly="True" style="width: 50px;"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo">
                        <table>
                            <tr>
                                <td>Nombre
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Nombre_Usuario" runat="server" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo">
                        <table>
                            <tr>
                                <td>Login
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Login" runat="server" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo">
                        <table>
                            <tr>
                                <td>Tipo de Usuario Principal
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="Tipo" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo_check">
                        <asp:CheckBox ID="Activo" runat="server" Text="Activo" ClientIDMode="Static" />
                    </div>
                    <div class="campo_check">
                        <asp:CheckBox ID="DCC" runat="server" Text="DCC" ClientIDMode="Static" />
                    </div>
                    <div class="campo_check">
                        <asp:CheckBox ID="SMS" runat="server" Text="SMS" ClientIDMode="Static" />
                    </div>
                    <div class="campo_check">
                        <asp:CheckBox ID="Pruebas" runat="server" Text="Pruebas" ClientIDMode="Static" />
                    </div>
                    <div class="campo">
                        <table>
                            <tr>
                                <td>Celular
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Celular" runat="server" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo">
                        <table>
                            <tr>
                                <td>Correo
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Correo" runat="server" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo">
                        <table>
                            <tr>
                                <td>Puede Ver
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="PVer" runat="server">
                                        <asp:ListItem>Incidencias Levantadas</asp:ListItem>
                                        <asp:ListItem>Todas las Incidencias</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo">
                        <table>
                            <tr>
                                <td>Filtro Default
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="Default" runat="server">
                                        <asp:ListItem>Sin Filtro</asp:ListItem>
                                        <asp:ListItem>Abiertas</asp:ListItem>
                                        <asp:ListItem Selected="True">Abiertas y en Validación</asp:ListItem>
                                        <asp:ListItem>En Validación</asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:CheckBox ID="Resp" runat="server" Text="Solo como responsable" ClientIDMode="Static" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo_check">
                        <asp:CheckBox ID="Cliente" runat="server" Text="Cliente" OnClick="estado();" ClientIDMode="Static" />
                    </div>
                    <div class="campo_check">
                        <asp:CheckBox ID="Interno" runat="server" Text="Interno" OnClick="estado();" ClientIDMode="Static" />
                    </div>
                    <div class="campo_check">
                        <asp:CheckBox ID="Requiriente" runat="server" Text="Requiriente" OnClick="estado();" ClientIDMode="Static" />
                    </div>
                    <div style="empty-cells: hide;" class="campo">
                        <table>
                            <tr>
                                <td style="empty-cells: hide;">
                                    <asp:Label ID="etqUnidad" runat="server" Text="Unidad" ClientIDMode="Static"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="empty-cells: hide;">
                                    <asp:DropDownList ID="Unidades" runat="server" ClientIDMode="Static" Style="display: none;"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo_boton">
                        <asp:Button ID="Resetear" runat="server" Text="Resetear Contraseña" CssClass="btn" OnClick="Resetear_Click" UseSubmitBehavior="False" />
                        <asp:Button ID="Guardar" runat="server" Text="Guardar" CssClass="btn" UseSubmitBehavior="False" OnClick="Guardar_Click" />
                        <asp:Button ID="Cancelar" runat="server" Text="Cancelar" CssClass="btn" UseSubmitBehavior="False" OnClick="Cancelar_Click" />
                    </div>
                    <div class="campo">
                        <table>
                            <tr>
                                <td>
                                    <div class="campo">Permisos</div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="campo">Perfil</div>
                                    <div class="campo">
                                        <asp:DropDownList ID="Perfiles" Style="margin: 0px 10px;" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Perfiles_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="campo">
                                        <asp:Button ID="Perfil" runat="server" Text="Aplicar perfil" CssClass="btn_mediano" UseSubmitBehavior="False" OnClick="Perfil_Click" Visible="False" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="campo">
                                        <asp:CheckBoxList ID="Permisos" CssClass="permisos" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static"></asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <%--<table class="cont">
                    <tr>
                        <td class="barra_izquierda"></td>
                        <td class="detalle"></td>
                    </tr>
                </table>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
            <ProgressTemplate>
                <div id="Background"></div>
                <div id="Progress">
                    <div class="windows8">
                        <div class="wBall" id="wBall_1">
                            <div class="wInnerBall">
                            </div>
                        </div>
                        <div class="wBall" id="wBall_2">
                            <div class="wInnerBall">
                            </div>
                        </div>
                        <div class="wBall" id="wBall_3">
                            <div class="wInnerBall">
                            </div>
                        </div>
                        <div class="wBall" id="wBall_4">
                            <div class="wInnerBall">
                            </div>
                        </div>
                        <div class="wBall" id="wBall_5">
                            <div class="wInnerBall">
                            </div>
                        </div>
                    </div>

                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

    </form>
</body>
</html>

