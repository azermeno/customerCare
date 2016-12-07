<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tipificaciones.aspx.cs" Inherits="CustomerCare.Tipificaciones" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tipificaciones</title>
    <style type="text/css">
        html, form
        {
            overflow: auto;
            height: 100%;
            padding: 0px;
            margin: 0px;
            border: 0px;
        }
    body   
{
    font-size: .80em;
    font-family: Verdana; 
    margin: 0px 0px 0px 0px;
    padding: 0px;
    color: #ffffff;
    height: 100%;
    width: 100%;
}
</style>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadSkinManager runat="server" ID="rsmTipi" />
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" MinDisplayTime="500" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel1" runat="server" >
        <telerik:RadTreeList ID="RadTreeList1" runat="server" OnChildItemsDataBind="RadTreeList1_ChildItemsDataBind" 
            OnUpdateCommand="RadTreeList1_UpdateCommand" OnInsertCommand="RadTreeList1_InsertCommand"
            OnDeleteCommand="RadTreeList1_DeleteCommand" ParentDataKeyNames="Padre" HideExpandCollapseButtonIfNoChildren="true"
            DataKeyNames="Codigo" AutoGenerateColumns="false" AllowLoadOnDemand="true" OnNeedDataSource="RadTreeList1_NeedDataSource"
            OnItemDataBound="RadTreeList1_ItemDataBound" >
            <EditFormSettings><EditColumn CancelText="Cancelar" UpdateText="Guardar" InsertText="Guardar" /></EditFormSettings>
            <Columns>
                <telerik:TreeListBoundColumn DataField="Descripcion" HeaderText="Descripción" UniqueName="Descripcion"
                    HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Left" />
                <telerik:TreeListEditCommandColumn UniqueName="InsertCommandColumn" ButtonType="ImageButton"
                    ShowEditButton="true" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" AddRecordText="Agregar"
                    EditText="Editar" />
                <%--<telerik:TreeListButtonColumn CommandName="Edit" Text="Edit" UniqueName="EditCommandColumn"
                    ButtonType="ImageButton" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" />--%>
                <telerik:TreeListButtonColumn UniqueName="DeleteCommandColumn" Text="Borrar" CommandName="Delete"
                    ButtonType="ImageButton" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" />
                <telerik:TreeListBoundColumn DataField="Codigo" HeaderText="Código" ReadOnly="true"
                    UniqueName="Codigo" Visible="false" ForceExtractValue="Always" />
                <telerik:TreeListBoundColumn DataField="Padre" HeaderText="Padre" Visible="false"
                    ReadOnly="true" ForceExtractValue="Always" />
                <telerik:TreeListBoundColumn DataField="Hoja" HeaderText="Hoja" Visible="false"
                    ReadOnly="true" ForceExtractValue="Always" UniqueName="Hoja" />
                <%--<telerik:TreeListBoundColumn DataField="FirstName" HeaderText="First Name" UniqueName="FirstName"
                    HeaderStyle-Width="75px" />
                <telerik:TreeListBoundColumn DataField="TitleOfCourtesy" HeaderText="Title" UniqueName="Title"
                    HeaderStyle-Width="60px" />
                <telerik:TreeListBoundColumn DataField="Notes" HeaderText="Notes" UniqueName="Notes"
                    HeaderStyle-Width="280px" />--%>
            </Columns>
        </telerik:RadTreeList>
    </telerik:RadAjaxPanel>
    <br />
    </form>
</body>
</html>
