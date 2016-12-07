<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="CustomerCare.Administracion.WebForm1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <title>Automatic CRUD operations in ASP.NET AJAX Grid | RadGrid demo</title>
    <%--<style type="text/css"">
        .MyImageButton
{
    cursor: hand;
}
.EditFormHeader td
{
    font-size: 14px;
    padding: 4px !important;
    color: #0066cc;
}</style>--%>
</head>
<body>
    <form id="form2" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
<%--    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />
    <telerik:RadFormDecorator ID="QsfFromDecorator" runat="server" DecoratedControls="All" EnableRoundedCorners="false" />
    <%--<telerik:ConfiguratorPanel runat="server" ID="ConfigurationPanel1" Title="Configurator"
        Expanded="true">--%>
       <%-- Switch the edit modes:
        <br />
        <div style="float: left; margin-left: 30px;">
            <asp:RadioButton ID="RadioButton1" AutoPostBack="True" Text="In-forms editing mode"
                runat="server" Checked="True" OnCheckedChanged="RadioCheckedChanged"></asp:RadioButton>
        </div>
        <div style="float: left;">
            <asp:RadioButton ID="RadioButton2" AutoPostBack="True" Text="In-line editing mode"
                runat="server" OnCheckedChanged="RadioCheckedChanged"></asp:RadioButton>
        </div>
        <div style="float: left; margin-left: 20px;">
            <asp:CheckBox ID="CheckBox1" Text="Allow multi-row edit" AutoPostBack="True" runat="server"
                OnCheckedChanged="CheckboxCheckedChanged"></asp:CheckBox>
        </div>--%>
   <%-- </telerik:ConfiguratorPanel>--%>
    <%--<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <script type="text/javascript">
            <!--
            function RowDblClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }

            function gridCreated(sender, args) {
                if (sender.get_editIndexes && sender.get_editIndexes().length > 0) {
                    document.getElementById("OutPut").innerHTML = sender.get_editIndexes().join();
                }
                else {
                    document.getElementById("OutPut").innerHTML = "";
                }
            }
            -->
        </script>
    </telerik:RadCodeBlock>--%>
<%--    <div class="module" style="height: 20px; width: 350px;">
        <span style="font-weight: bold;">Edit indexes: </span><span id="OutPut" style="font-weight: bold;
            color: navy;"></span>
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager1" EnableAJAX="true" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1">
                    </telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadWindowManager1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadioButton1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1">
                    </telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadioButton2"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadioButton2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1">
                    </telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadioButton1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="CheckBox1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1">
                    </telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>--%>
    
   <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" PageSize="5" Skin="Hay"
  DataSourceID="SqlDataSource1" AllowAutomaticInserts="True" AllowAutomaticUpdates="True"
  AllowAutomaticDeletes="True">
  <MasterTableView EditMode="InPlace" CommandItemDisplay="Bottom" DataSourceID="SqlDataSource1"
    DataKeyNames="Codigo" AutoGenerateColumns="False">
    <EditFormSettings>
      <EditColumn UniqueName="EditCommandColumn1" />
      <PopUpSettings ScrollBars="None" />
    </EditFormSettings>
    <Columns>
      <telerik:GridEditCommandColumn />
      <telerik:GridButtonColumn CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" />
      <telerik:GridBoundColumn DataField="Codigo" HeaderText="Codigo" ReadOnly="True"
        UniqueName="Codigo" Display="False" />
      <telerik:GridBoundColumn DataField="Nombre" HeaderText="Nombre" UniqueName="Nombre" />
    </Columns>
  </MasterTableView>
</telerik:RadGrid>
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:CustomerCare %>"
   SelectCommand="SELECT [Codigo], [Nombre] FROM [Usuarios]" 
   DeleteCommand="DELETE FROM [Usuarios] WHERE [Codigo] = @Codigo" 
   InsertCommand="INSERT INTO [Usuarios] ([Nombre]) VALUES (@Nombre)" 
   UpdateCommand="UPDATE [Usuarios] SET [Nombre] = @Nombre WHERE [Codigo] = @Codigo">
   <DeleteParameters>
       <asp:Parameter Name="Codigo" Type="Int32" />
   </DeleteParameters>
   <InsertParameters>
       <asp:Parameter Name="Nombre" Type="String" />
   </InsertParameters>
   <UpdateParameters>
       <asp:Parameter Name="Nombre" Type="String" />
       <asp:Parameter Name="Codigo" Type="Int32" />
    </UpdateParameters>
 </asp:SqlDataSource>

        <asp:SqlDataSource runat="server" ID="sdsUsuarios" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>"
            SelectCommand="
                SELECT 
                    u.Codigo,
                    u.Nombre, 
                    Login, 
                    isnull(Celular,'') as Celular, 
                    isnull(Correo,'') as Correo, 
                    p.Nombre as Perfil, 
                    Activo, 
                    SMS, 
                    Pruebas, 
                    PuedeVer 
                FROM 
                    Usuarios u join Perfiles p on u.Perfil=p.Codigo" 
            InsertCommand="
                INSERT Usuarios (
                    Nombre, 
                    Login, 
                    Celular, 
                    Correo, 
                    Perfil, 
                    Activo, 
                    SMS, 
                    Pruebas, 
                    PuedeVer)
                VALUES (
                    @Nombre,
                    @Login,
                    @Celular,
                    @Correo,
                    (Select Codigo from Perfiles Where Nombre=@Perfil),
                    @Activo,
                    @SMS,
                    @Pruebas,
                    @PuedeVer
                    )"
            UpdateCommand="
                UPDATE Usuarios SET
                    Nombre=@Nombre, 
                    Login=@Login, 
                    Celular=@Celular, 
                    Correo=@Correo, 
                    Perfil=(Select Codigo from Perfiles Where Nombre=@Perfil), 
                    Activo=@Activo, 
                    SMS=@SMS, 
                    Pruebas=@Pruebas, 
                    PuedeVer=@PuedeVer
                WHERE
                    Codigo=@Codigo">
   <InsertParameters>
       <asp:Parameter Name="Nombre" Type="String" />
       <asp:Parameter Name="Login" Type="String" />
       <asp:Parameter Name="Celular" Type="String" />
       <asp:Parameter Name="Correo" Type="String" />
       <asp:Parameter Name="Perfil" Type="String" />
       <asp:Parameter Name="Activo" Type="Boolean" />
       <asp:Parameter Name="SMS" Type="Boolean" />
       <asp:Parameter Name="Pruebas" Type="Boolean" />
       <asp:Parameter Name="PuedeVer" Type="String" />
   </InsertParameters>
   <UpdateParameters>
       <asp:Parameter Name="Codigo" Type="Int32" />
       <asp:Parameter Name="Nombre" Type="String" />
       <asp:Parameter Name="Login" Type="String" />
       <asp:Parameter Name="Celular" Type="String" />
       <asp:Parameter Name="Correo" Type="String" />
       <asp:Parameter Name="Perfil" Type="String" />
       <asp:Parameter Name="Activo" Type="Boolean" />
       <asp:Parameter Name="SMS" Type="Boolean" />
       <asp:Parameter Name="Pruebas" Type="Boolean" />
       <asp:Parameter Name="PuedeVer" Type="String" />
    </UpdateParameters></asp:SqlDataSource>
    </form>
</body>
</html>