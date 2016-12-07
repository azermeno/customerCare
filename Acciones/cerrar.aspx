<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cerrar.aspx.cs" Inherits="CustomerCare.cerrar" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <form id="form1" runat="server">
                <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
                </telerik:RadScriptManager>
                <TABLE height="13" cellspacing="1" cellpadding="1" width="500" align="center"><tr><td style="HEIGHT: 23px"><div align="center">
                    <telerik:RadTextBox ID="RadTextBox1" runat="server" EmptyMessage="Solución..." 
                        Height="150px" Skin="Office2007" TextMode="MultiLine" Width="490px">
                    </telerik:RadTextBox></div></td></tr><tr><td><div align="center"><input id="ButCerrar" runat="server" type="button" value="Aceptar" onclick="salirCancelar()" /></div></td></tr></TABLE>
    </form>