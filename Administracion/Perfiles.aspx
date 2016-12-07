<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Perfiles.aspx.cs" Inherits="CustomerCare.Perfiles" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<! DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Perfiles</title>
    <link href="Estilos/Perfiles.css" rel="stylesheet" />
    <%--<style type="text/css">
    body, form, html   
{
    margin: 0px 0px 0px 0px;
    padding: 0px;
    height: 100%;
    width: 100%;
        font-family:'segoe ui', arial, sans-serif;
        font-size:12px;
        background-color:#FFFDE2;
}
    #RadSplitter1
    {
        display:block;
        width:100%;
        height:100%;
    }
    #ListaPerfiles
    {
float: left;
overflow: auto;
height: 100%;
}
</style>--%>
   <%-- <script type="text/javascript">
<!--
        function mostrar() {
            Unidades.style.display = 'inline';
//            LUnid.Style.diplay='inline';
        }

        function ocultar() {
            Unidades.style.display = 'none';
//            LUnid.Style.diplay='none';
        }
-->
</script>--%>
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
                        //l.options[i].style.display = "none";
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
                <%--<asp:Button ID="Si" CssClass="btn_mediano" runat="server" Text="Si" OnClick="Si_Click" />--%>
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
                <div id="list" class="cont">
                    <div class="botones">
                        <asp:ImageButton ID="Nuevo" runat="server" ImageUrl="~/Administracion/Estilos/Imagenes/nuevo-usuario-icono-8071-32.png" ToolTip="Nuevo Perfil" OnClientClick="tamfram();" ClientIDMode="Static" OnClick="Nuevo_Click" />
                        <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/Administracion/Estilos/Imagenes/editar-un-marcador-de-nombre-icono-8476-32.png" ToolTip="Modificar Datos del Perfil" OnClick="Modificar_Click" ClientIDMode="Static" />
                        <%--<asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/Administracion/Estilos/Imagenes/eliminar-usuario-icono-5252-32.png" ToolTip="Eliminar Datos del Usuario" ClientIDMode="Static" OnClientClick="confirmacion();" />--%>
                    </div>
                    <div class="lista">
                        <asp:TextBox ID="Buscar" runat="server" onkeyup="SearchList();" Text="" CssClass="busc" ClientIDMode="Static" />
                        <asp:ListBox ID="Lista" CssClass="lista" name="lista" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="Lista_SelectedIndexChanged" AutoPostBack="True"></asp:ListBox>
                    </div>
                </div>
                <div id="det" class="detalle">
                    <div class="campo">
                        <table>
                            <tr>
                                <td>Nombre:
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Nombre_Perfil" runat="server" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="campo_check">
                        <asp:CheckBox ID="Activo" runat="server" Text="Activo" ClientIDMode="Static" />
                    </div>
                    <div class="campo_boton">
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
                                    <asp:CheckBoxList ID="Permisos" CssClass="permisos" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static"></asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
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