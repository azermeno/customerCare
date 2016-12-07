
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="CustomerCare.Reportes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function requestStart(sender, args) {
            if (args.get_eventTarget().indexOf("ExportToExcelButton") >= 0) {
                args.set_enableAjax(false);
            }
        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 4px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <%--<telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
    </telerik:RadStyleSheetManager>--%>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.Core.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.jQuery.js">
            </asp:ScriptReference>
            <asp:ScriptReference Assembly="Telerik.Web.UI" 
                Name="Telerik.Web.UI.Common.jQueryInclude.js">
            </asp:ScriptReference>

        </Scripts>
        
    </telerik:RadScriptManager>
    <div>
        <div>
        
            <table>
            <tr>
            <td style="vertical-align: top">
            <asp:Label ID="Label1" runat="server" Text="Consultas Guardadas:"></asp:Label>
            </td>
            <td style="vertical-align: top">
            <telerik:RadComboBox ID="ListaConsultas" Runat="server" 
                DataSourceID="Consultas" DataTextField="Nombre" DataValueField="id" 
                onselectedindexchanged="ListaConsultas_SelectedIndexChanged" 
                    AutoPostBack="True" AllowCustomText="true" Filter="Contains">
            </telerik:RadComboBox>
        </td>
            <td rowspan="3" valign="top" >
             <telerik:RadTextBox ID="Consulta2" Runat="server" TextMode="MultiLine" 
                    Text='<%# Bind("Consulta") %>' Width="400"  
                    onprerender="Consulta2_PreRender">
                    </telerik:RadTextBox>
            <asp:FormView ID="FormView1" runat="server" DataKeyNames="id" 
                DataSourceID="Consultas" Height="100%">
                <EditItemTemplate>
                    id:
                    <asp:Label ID="idLabel1" runat="server" Text='<%# Eval("id") %>' />
                    <br />
                    Nombre:
                    <asp:TextBox ID="NombreTextBox" runat="server" Text='<%# Bind("Nombre") %>' />
                    <br />
                    Consulta:
                    <asp:TextBox ID="ConsultaTextBox" runat="server" 
                        Text='<%# Bind("Consulta") %>' />
                    <br />
                    <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
                        CommandName="Update" Text="Actualizar" />
                    &nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
                        CausesValidation="False" CommandName="Cancel" Text="Cancelar" />
                </EditItemTemplate>
                <InsertItemTemplate>
                    id:
                    <asp:TextBox ID="idTextBox" runat="server" Text='<%# Bind("id") %>' />
                    <br />
                    Nombre:
                    <asp:TextBox ID="NombreTextBox" runat="server" Text='<%# Bind("Nombre") %>' />
                    <br />
                    Consulta:
                    <asp:TextBox ID="ConsultaTextBox" runat="server" 
                        Text='<%# Bind("Consulta") %>' Height="140" />
                    <br />
                    <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
                        CommandName="Insert" Text="Insertar" />
                    &nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
                        CausesValidation="False" CommandName="Cancel" Text="Cancelar" />
                </InsertItemTemplate>
                <ItemTemplate>
                    <telerik:RadTextBox ID="Consulta" Runat="server" TextMode="MultiLine" Text='<%# Bind("Consulta") %>' Width="400" Height="100">
                    </telerik:RadTextBox>

                </ItemTemplate>

            </asp:FormView>
            </td>
            <td rowspan="3" class="style1" valign="top">
                <telerik:RadListBox ID="Tablas" runat="server" DataSourceID="SqlTablas" 
                    DataTextField="TABLE_NAME" DataValueField="TABLE_NAME" AutoPostBack="True" 
                    Height="109px" onselectedindexchanged="Tablas_SelectedIndexChanged">
                </telerik:RadListBox>
                &nbsp;</td>
            <td rowspan="3" valign="top">
                <telerik:RadListBox ID="Campos" runat="server" DataSourceID="SqlCampos" 
                    DataTextField="COLUMN_NAME" DataValueField="COLUMN_NAME" Height="109px">
                </telerik:RadListBox>
                &nbsp;</td>
            <td rowspan="3" valign="top">
                <asp:Table ID="TParametros" runat="server">
                </asp:Table>
                <%--<asp:Panel ID="Panel1" runat="server" Height="109px" ScrollBars="Auto">
                </asp:Panel>--%>
            </td>
            </tr>
            </table>
            <table id="ControlesReporte" runat="server">
            <tr>
            <td valign="top">
                <asp:Button ID="Modificar" runat="server" Text="Modificar" 
                    onclick="Modificar_Click" onprerender="Modificar_PreRender" />
                <asp:Button ID="Eliminar" runat="server" Text="Eliminar" 
                    onclick="Eliminar_Click" onprerender="Eliminar_PreRender" />
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:TextBox ID="rcbNombre" runat="server"></asp:TextBox>
                </td>
                <td valign="top">
                    <asp:Button ID="Guardar" runat="server" Text="Guardar" 
                        onclick="Guardar_Click" />
                </td>
                
            </tr>
            <tr>
            <td valign="top">
                <asp:ImageButton ID="btnExcel" runat="server" onclick="Excel_Click" />
            &nbsp;<asp:ImageButton ID="btnWord" runat="server" onclick="btnWord_Click" />
            &nbsp;<asp:ImageButton ID="btnPDF" runat="server" onclick="btnPDF_Click" />
            &nbsp;<asp:ImageButton ID="btnCSV" runat="server" onclick="btnCSV_Click" />
            </td>
                <td align="right" valign="top" >
                    <asp:Button ID="Ejecutar" runat="server" Text="Realizar Consulta" 
                        onclick="Ejecutar_Click" />
                </td>
                <td>
                    <asp:CheckBox ID="Orden" runat="server" Text="Orden por Multi-columnas" 
                        AutoPostBack="True" oncheckedchanged="Orden_CheckedChanged" />
                </td>
                <td valign="top" >
                    <asp:Button ID="TInsertar" runat="server" Text="Insertar" onclick="Button1_Click" />
                </td>
                <td valign="top">
                <asp:Button ID="CInsertar" runat="server" Text="Insertar" onclick="Button2_Click" />
                </td>
                <td valign="top">
                    <asp:Button ID="Buscar" runat="server" Text="Buscar Parametros" 
                        onclick="Buscar_Click" />   
                </td>
                </tr>
            
        </table>
        </div>
        <div>
        <asp:sqldatasource runat="server" ID="Datos" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
            SelectCommand=""></asp:sqldatasource>

        <asp:sqldatasource runat="server" ID="Consultas" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
            
                SelectCommand="SELECT [id], [Nombre], [Consulta] FROM [Consultas] ORDER BY [Nombre]"></asp:sqldatasource>
        <asp:sqldatasource runat="server" ID="SqlTablas" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
            
                SelectCommand="SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES ORDER BY TABLE_NAME"></asp:sqldatasource>

        <asp:sqldatasource runat="server" ID="SqlCampos" 
            ConnectionString="<%$ ConnectionStrings:CustomerCare %>" 
            ProviderName="<%$ ConnectionStrings:CustomerCare.ProviderName %>" 
            SelectCommand="SELECT TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS">
            <FilterParameters>
                <asp:ControlParameter ControlID="Tablas" Name="TABLE_NAME" 
                    PropertyName="SelectedValue" />
            </FilterParameters>
            </asp:sqldatasource>

                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
            <telerik:RadGrid ID="Resultado" runat="server" CellSpacing="0" Culture="es-ES" DataSourceID="Datos" 
                GridLines="None" AllowSorting="True" 
                onneeddatasource="Resultado_NeedDataSource" 
                onsortcommand="Resultado_SortCommand"  >
                <ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                     <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling>
                </ClientSettings>
                 
<MasterTableView DataSourceID="Datos" AllowMultiColumnSorting="False">

<CommandItemSettings ShowExportToExcelButton="true" ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>
</MasterTableView>
<SortingSettings EnableSkinSortStyles="False" SortedAscToolTip="Ordenado de forma ascedente" SortedDescToolTip="Ordenado de forma descedencte" SortToolTip="Click para ordenar" />
<FilterMenu EnableImageSprites="False"></FilterMenu>

<HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default"></HeaderContextMenu>
            </telerik:RadGrid>
        
        </div>
    </div>
    <telerik:RadAjaxManager runat="server">
    </telerik:RadAjaxManager>
    </form>
</body>
</html>
