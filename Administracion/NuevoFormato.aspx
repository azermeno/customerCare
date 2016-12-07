<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NuevoFormato.aspx.cs" Inherits="CustomerCare.NuevoFormato" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<html>
<head runat="server">
    <title></title>
<link rel="stylesheet" type="text/css" href="<%=ConfigurationManager.AppSettings["Telerik.Web.UI.StyleSheetFolders"] %>/base.css" />
</head>
<body>
    <form id="form1" runat="server">
                <%--<telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
                    <StyleSheets>
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Button.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Calendar.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/ComboBox.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Dock.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Editor.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Grid.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Input.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/ListBox.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Menu.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Slider.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Splitter.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/TabStrip.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/ToolBar.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/ToolTip.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/TreeList.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/TreeView.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Upload.Ayuntamiento.css" />
                        <telerik:StyleSheetReference IsCommonCss="False" Path="Styles/Window.Ayuntamiento.css" />
                    </StyleSheets>
                </telerik:RadStyleSheetManager>--%>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" Runat="server">
    </telerik:RadSkinManager><table style="width: 100%; height: 100%;" cellspacing="0">
    <tr style="height: 26px;"><td>
        <telerik:RadToolBar ID="RadToolBar1" runat="server" Width="100%" 
            AutoPostBack="True" onbuttonclick="RadToolBar1_ButtonClick">
            <Items>
                <telerik:RadToolBarButton runat="server" Enabled="False" Text="NUEVO FORMATO">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" IsSeparator="True" Text="Button 1">
                </telerik:RadToolBarButton>
                <telerik:RadToolBarButton runat="server" Text="Guardar">
                </telerik:RadToolBarButton>
            </Items>
        </telerik:RadToolBar>
        </td></tr>
        <tr valign="top"><td>
    <telerik:RadEditor ID="RadEditor1" Runat="server" Height="600px" 
                                Width="100%">
                            </telerik:RadEditor>
                            </td>
                            </tr>
                            </table>
    </form>
</body>
</html>
