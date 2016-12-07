<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="seguimiento.aspx.cs" Inherits="CustomerCare.seguimiento" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
                        <div>
                            <telerik:RadTextBox ID="rtbObs" runat="server" Height="150px" 
                                TextMode="MultiLine" Width="100%" EmptyMessage="Observaciones">
                            </telerik:RadTextBox>
                            
						
<div align="right">Tipo de Evento:&nbsp;
    <asp:radiobuttonlist runat="server" ID="rblTipoEvento" 
        RepeatDirection="Horizontal">
        <asp:ListItem Value="7"></asp:ListItem>
        <asp:ListItem Value="6"></asp:ListItem>
        <asp:ListItem Value="2" Selected="True"></asp:ListItem>
    </asp:radiobuttonlist> <span style="display:none"><asp:CheckBox id="chkEnviarMail" runat="server" textalign="Left" text="Enviar Mail (SMS)"></ASP:CheckBox></span></div>
                        </div>
                        <div>
                        <input type="button" onclick="guardarSeguimiento()" value="Guardar" />
                            <%-- <telerik:RadButton ID="btnGuardar" runat="server" Text="Guardar" OnClientClicked="guardarSeguimiento()">
                            </telerik:RadButton>--%>
                            <asp:RequiredFieldValidator id="txtObsSegui" runat="server" controltovalidate="rtbObs" errormessage="Las observaciones son necesarias"></ASP:RequiredFieldValidator>
                            </div>
    
   
    </form>
