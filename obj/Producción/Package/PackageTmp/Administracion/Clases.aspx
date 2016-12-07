<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clases.aspx.cs" Inherits="CustomerCare.Clases" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Clases</title>
    <style type="text/css">
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
    #ListaClases
    {
float: left;
overflow: auto;
height: 100%;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
       <%-- <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
        </telerik:RadStyleSheetManager>       
        <telerik:RadSkinManager ID="RadSkinManager1" Runat="server" Skin="Default">
        </telerik:RadSkinManager>--%>
        <telerik:RadScriptManager ID="RadScriptManager1" Runat="server">
        </telerik:RadScriptManager>
    <telerik:RadSplitter ID="RadSplitter1" Runat="server" 
            Width="100%" Height="100%">
        <telerik:RadPane ID="RadPane1" Runat="server" Scrolling="None" Width="330px"><%--
                    <asp:Table runat="server">
                        <asp:TableRow>
                            <asp:TableCell >--%>
                            Sólo Activas:<asp:CheckBox ID="cbxSoloActivos" runat="server" AutoPostBack="true" OnCheckedChanged="cbxSoloActivos_CheckedChanged" Checked="true" /><br />
                            <telerik:RadComboBox ID="rcbClases" runat="server" DataSourceID="SqlClases" DataTextField="Nombre" DataValueField="Codigo" OnSelectedIndexChanged="rcbClases_IndexChanged"  Filter="Contains" AllowCustomText="true" AutoPostBack="true" Width="300px" />
                            <%--</asp:TableCell>
                            <asp:TableCell VerticalAlign="Top" Width="21px">--%>
                            <table>
                            <tr>
                            <td>
                                <telerik:RadButton ID="Nuevo" runat="server" Text=" " Width="20px" 
                    Height="20px" onclick="Nuevo_Click" ToolTip="Dar de alta una nueva entidad">
                            </telerik:RadButton>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <telerik:RadButton ID="Modificar" runat="server" Text=" " 
                    Width="20px" Height="20px" onclick="Modificar_Click" ToolTip="Modificar">
                            </telerik:RadButton>
                            </td>
                            </tr>
                            <tr>
                            <td>
                            <telerik:RadButton ID="Guardar" runat="server" Text="" Enabled="False" 
                    onclick="Guardar_Click" ToolTip="Guardar" Width="20px" Height="20px">
                </telerik:RadButton>
                            </td>
                            </tr>
                            <tr>
                            <td>
                <telerik:RadButton ID="Cancelar" runat="server" Text="" Enabled="False" 
                    onclick="Cancelar_Click" Width="20px" Height="20px" ToolTip="Cancelar">
                </telerik:RadButton>
                            </td>
                            </tr>
                            </table>
                            <%--</asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>--%>
                
            </telerik:RadPane>
        <telerik:RadSplitBar ID="RadSplitBar1" Runat="server">
            </telerik:RadSplitBar>
        <telerik:RadPane ID="RadPane2" Runat="server">
        <asp:HiddenField id="hflTemplate" runat="server" />
                 <asp:PlaceHolder ID="Relaciones" runat="server" />
                    <label for="rcbResponsable">Responsable:</label><telerik:RadComboBox ID="rcbResponsable" runat="server" DataSourceID="SqlUsuarios" DataTextField="Nombre" DataValueField="Codigo" Filter="Contains" EmptyMessage="Vacante..." AllowCustomText="true" Width="300px" />
                     <asp:FormView ID="FormClases" runat="server" DataKeyNames="codigo" 
            DataSourceID="SqlClases" 
            Width="100%">
            
            <ItemTemplate>




            <%--temporal de encuesta--%>
                <asp:HiddenField ID="unidad" runat="server" Value='<%# Eval("unidad") %>' EnableViewState="true" />
               <%-- temporal de encuesta--%>




                <asp:HiddenField ID="hflDatosExtra" runat="server" Value='<%# Eval("datosextra") %>' EnableViewState="true" />
                <telerik:RadTextBox ID="cod_act" runat="server" Text='<%# Eval("codigo") %>'  Width="300px" Display="False">
                </telerik:RadTextBox>
                    <label for="Nombre">Nombre:</label><telerik:RadTextBox ID="Nombre" runat="server" Text='<%# Eval("Nombre") %>' Width="300px"/>
                    <%--<label for="Alias">Alias:</label><telerik:RadTextBox ID="Alias" runat="server" Text='<%# Eval("Alias") %>' MaxLength="15"/>--%>
                    <label for="activaCheckBox">Activo:</label><asp:CheckBox ID="activaCheckBox" runat="server" Checked='<%# Bind("activa") %>' Enabled="false" />
                <fieldset>
                    <legend>Ciberencuesta</legend>
                    <label for="IdCliente">Sesión: </label><telerik:RadTextBox ID="IdCliente" runat="server" Text='<%# Bind("IdCliente") %>' />   <label for="Encuestado">Encuestado: </label><asp:CheckBox ID="Encuestado" runat="server" Checked='<%# Bind("encuestado") %>' />   <br/>
                    <label for="txtResponsable">Saludo: </label><telerik:RadTextBox ID="txtResponsable" runat="server" Text='<%# Bind("txtResponsable") %>'  Width="400px"/><br />  
                </fieldset>  
                <fieldset>
                    <legend>Ciberenvíos</legend>
                    <label for="txtDomicilio">Domicilio: </label><telerik:RadTextBox ID="txtDomicilio" runat="server" Text='<%# Bind("txtDomicilio") %>' />   <label for="txtciudad">Ciudad: </label><telerik:RadTextBox ID="txtciudad" runat="server" Text='<%# Bind("txtciudad") %>' />   <br/>
                    <label for="txtContacto">Contacto: </label><telerik:RadTextBox ID="txtcontacto" runat="server" Text='<%# Bind("txtcontacto") %>' Width="350px"/><br/>
                </fieldset>  
            </ItemTemplate>
                 </asp:FormView>
                </telerik:RadPane>
    </telerik:RadSplitter>

        
        
        <asp:SqlDataSource ID="SqlUsuarios" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="select u.Nombre, i.Codigo from Internos i join Usuarios u on i.Usuario=u.Codigo where u.Activo=1 order by u.Nombre" 
            ></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlClases" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" ></asp:SqlDataSource>
    
    </form>
</body>
</html>
