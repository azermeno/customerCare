<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Accion.aspx.cs" Inherits="CustomerCare.Accion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
                <asp:Panel ID="Panel1" runat="server">
                        <div>
                          <telerik:RadAjaxLoadingPanel ID="ralp" runat="server" Skin="Default">
                          </telerik:RadAjaxLoadingPanel>
         <asp:SqlDataSource ID="sdsResponsable" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="SELECT [Codigo], [Nombre] FROM [Usuarios] WHERE ([Activo] = 1) ORDER BY [Nombre]"></asp:SqlDataSource>

        <telerik:RadComboBox ID="rcbResponsable" Runat="server" AllowCustomText="True" 
            DataSourceID="sdsResponsable" DataTextField="nombre" DataValueField="codigo" 
            EmptyMessage="Responsable..." Filter="Contains" 
            DropDownWidth="200px" Width="220px">
        </telerik:RadComboBox>
         <asp:SqlDataSource ID="sdsPrioridad" runat="server" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" SelectCommand="SELECT Codigo, Descripcion FROM Severidades ORDER BY Codigo"></asp:SqlDataSource>

        <telerik:RadComboBox ID="rcbPrioridad" Runat="server" AllowCustomText="True" 
            DataSourceID="sdsPrioridad" DataTextField="Descripcion" DataValueField="codigo" 
            EmptyMessage="Prioridad..." Filter="Contains" 
            DropDownWidth="200px" Width="220px">
        </telerik:RadComboBox>
        
                        <telerik:RadTextBox ID="rtbConclusion" Runat="server" EmptyMessage="Seleccione..." 
                            ReadOnly="True" Width="300px" TabIndex="3">
                        </telerik:RadTextBox>
                            <telerik:RadTextBox ID="rtbObs" runat="server" Height="150px" 
                                TextMode="MultiLine" Width="100%" EmptyMessage="Observaciones">
                            </telerik:RadTextBox>
<div align="right">
    <asp:radiobuttonlist runat="server" ID="rblTipoEvento" 
        RepeatDirection="Horizontal">
        <asp:ListItem Value="7"></asp:ListItem>
        <asp:ListItem Value="6"></asp:ListItem>
        <asp:ListItem Value="2" Selected="True"></asp:ListItem>
    </asp:radiobuttonlist> <span style="display:none"><asp:CheckBox id="chkEnviarMail" runat="server" textalign="Left" text="Enviar Mail (SMS)"></ASP:CheckBox></span></div>
                        </div>
                        <div>
                        <input type="button" onclick="salir()" value="Guardar" />
                            <%-- <telerik:RadButton ID="btnGuardar" runat="server" Text="Guardar" OnClientClicked="guardarSeguimiento()">
                            </telerik:RadButton>--%>
                            <asp:RequiredFieldValidator id="txtObsSegui" runat="server" controltovalidate="rtbObs" errormessage="Las observaciones son necesarias"></ASP:RequiredFieldValidator>
                            </div>
                            </asp:Panel>
    </form>
</body>
</html>
