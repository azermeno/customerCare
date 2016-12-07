<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DatosExtra.aspx.cs" Inherits="CustomerCare.DatosExtra" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadToolBar ID="rtbPrincipal" runat="server" OnButtonClick="Guardar" Width="100%" AutoPostBack="true">
    <Items>
    <telerik:RadToolBarButton runat="server" Text="Nuevos datos extra" CommandName="nuevo" PostBack="false">
    </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Nombre" Width="100%" 
                  Value="buscar" PostBack="false">
                    <ItemTemplate>
                        <telerik:RadTextBox ID="rtbNombre" runat="server" 
                            ClientEvents-OnKeyPress="guardar" EmptyMessage="Nombre..."
                            Width="180px" AutoPostBack="false">
                        </telerik:RadTextBox>
                    </ItemTemplate>
                </telerik:RadToolBarButton>
    <telerik:RadToolBarButton runat="server" Text="Guardar" CommandName="guardar" PostBack="true">
    </telerik:RadToolBarButton>
    </Items>
    </telerik:RadToolBar>
    <asp:PlaceHolder ID="placeholder" runat="server"></asp:PlaceHolder>
    </form>
</body>
</html>
