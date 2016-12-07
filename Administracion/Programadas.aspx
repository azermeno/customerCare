<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Programadas.aspx.cs" Inherits="CustomerCare.Administracion.Programadas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="Estilos/Programadas.css" rel="stylesheet" />
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

        function anexar() {
            var NuevoRegistro= "<tr><td>Nuevo campo</td><td align=”center”>Campo 2</td><td align=”center”> Campo 3</td></tr>";

$("#Tareas table tbody").append(NuevoRegistro);
        }
    </script>
</head>
<body onclick="tam_fram();" onkeypress="tam_fram();" onload="tam_fram();" onresize="tam_fram();" onchange="tam_fram();" onsubmit="tam_fram();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div id="aef" class="alerta_fondo">
        </div>
        <div id="aem" class="alerta_mns">
            <div class="alerta_texto">
                <asp:Label ID="emns" runat="server" Text="" ClientIDMode="Static"></asp:Label>
            </div>
            <%--<div class="alerta_btn">
                <asp:Button ID="Si" CssClass="btn_mediano" runat="server" Text="Si" OnClick="Si_Click" />
                <input id="No" type="button" value="No" class="btn_mediano" onclick="cancela();" />
            </div>--%>
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
                        <asp:ImageButton ID="Nuevo" runat="server" ImageUrl="~/Administracion/Estilos/Imagenes/nuevo-usuario-icono-8071-32.png" ToolTip="Nueva Unidad" OnClientClick="tamfram();" ClientIDMode="Static" OnClick="Nuevo_Click" />
                        <asp:ImageButton ID="Modificar" runat="server" ImageUrl="~/Administracion/Estilos/Imagenes/editar-un-marcador-de-nombre-icono-8476-32.png" ToolTip="Modificar Datos de la Unidad" OnClick="Modificar_Click" ClientIDMode="Static" />
                        <%--<asp:ImageButton ID="Eliminar" runat="server" ImageUrl="~/Administracion/Estilos/Imagenes/eliminar-usuario-icono-5252-32.png" ToolTip="Eliminar Datos del Usuario" ClientIDMode="Static" OnClientClick="confirmacion();" />--%>
                    </div>
                    <div class="lista">
                        <asp:CheckBox ID="bActivos" runat="server" Text="Mostrar Historial" ClientIDMode="Static" AutoPostBack="True" />
                        <asp:TextBox ID="Buscar" runat="server" onkeyup="SearchList();" Text="" CssClass="busc" ClientIDMode="Static" />
                        <asp:ListBox ID="Lista" CssClass="lista" name="lista" runat="server" ClientIDMode="Static"  AutoPostBack="True"></asp:ListBox>
                    </div>
                </div>
                <div id="det" class="detalle">
                    <div class="campo_boton">
                        <asp:Button ID="Guardar" runat="server" Text="Guardar" CssClass="btn" UseSubmitBehavior="False" OnClick="Guardar_Click" />
                        <asp:Button ID="Cancelar" runat="server" Text="Cancelar" CssClass="btn" UseSubmitBehavior="False" OnClick="Cancelar_Click" />
                    </div>
                    <div class="division">
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Nombre de la tarea programada
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="Nombre_Tarea" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Tarea para
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="Para" runat="server">
                                            <asp:ListItem>Zonas</asp:ListItem>
                                            <asp:ListItem>Centros de Solución</asp:ListItem>
                                            <asp:ListItem>Unidad</asp:ListItem>
                                            <asp:ListItem>Personalizado</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Zonas
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="Zonas" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Centros de Solución
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="CS" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Solicitante
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="Solicitante" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Responsable
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="Internos" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Levanto
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="Levanto" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Incidencia Padre
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="IncPad" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Asunto
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="Asunto" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Detalle
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="Detalle" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Tipificación
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="Tipificacion" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Severidad
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="Severidad" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Hora programada
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox type="time" ID="IniTime" CssClass="elemento invisible dato" min="00:00:00" max="23:59" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo">
                            <table>
                                <tr>
                                    <td>Fecha Limite
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox type="date" ID="FecLim" CssClass="elemento invisible dato" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo" style="width: 350px;">
                            <table>
                                <tr>
                                    <td>Tiempo asignado
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox type="numeric" min="0" ID="dias" CssClass="elemento invisible dato" style="width: 50px; margin-right: 5px;" runat="server" ClientIDMode="Static"></asp:TextBox> Día(s)
                                        <asp:TextBox type="numeric" min="0" ID="horas" CssClass="elemento invisible dato" style="width: 50px; margin-right: 5px;" runat="server" ClientIDMode="Static"></asp:TextBox> Mes(es)
                                        <asp:TextBox type="numeric" min="0" ID="minutos" CssClass="elemento invisible dato" style="width: 50px; margin-right: 5px;" runat="server" ClientIDMode="Static"></asp:TextBox> Año(s)
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo" style="width: 350px;">
                            <table>
                                <tr>
                                    <td>Periodo
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="TipoPeriodo" runat="server">
                                            <asp:ListItem>Día de la Semana</asp:ListItem>
                                            <asp:ListItem>Día del Mes</asp:ListItem>
                                            <asp:ListItem>Día del año</asp:ListItem>
                                            <asp:ListItem>Numero de días</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="campo_boton">
                            <input id="anexar" name="anexar" type="button" value="Añadir Tarea" onclick="anexar();" />
                        </div>
                        <div class="campo" style="width: 99%; margin-left: auto; margin-right: auto;">
                            <asp:GridView ID="Tareas" runat="server" style="width: 100%;" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical">
                                <AlternatingRowStyle BackColor="#DCDCDC" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Codigo"></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nombre"></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Condicion"></asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#000065" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
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
        </asp:UpdateProgress>--%>
    </form>
</body>
</html>
