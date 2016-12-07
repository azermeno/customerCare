<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Adjuntar.aspx.cs" Inherits="CustomerCare.Adjuntar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Adjuntar archivos</title>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
    </telerik:RadScriptManager>
    
    <telerik:RadProgressManager ID="RadProgressManager1" Runat="server" />
    <asp:Label Font-Names="verdana" Font-Size="Smaller" ID="Label1" runat="server" Text="Puede adjuntar varios archivos a la vez. Los archivos en conjunto no pueden exceder los 4 MB."></asp:Label>
    <hr />
    <telerik:RadUpload ID="RadUpload1" Runat="server" 
        ControlObjectsVisibility="RemoveButtons, AddButton" MaxFileSize="4194304">
        <Localization Add="Agregar" Clear="Borrar" Delete="Eliminar" Remove="Quitar" 
            Select="Examinar" />
    </telerik:RadUpload>
    <hr />
    <telerik:RadButton ID="RadButton1" runat="server" Text="Adjuntar" 
        onclick="RadButton1_Click">
    </telerik:RadButton>
    <telerik:RadProgressArea ID="RadProgressArea1" Runat="server" Culture="es-MX" 
        DisplayCancelButton="True">
<Localization Uploaded="Finalizado" Cancel="Cancelar" 
            CurrentFileName="Subiendo archivo: " ElapsedTime="Tiempo transcurrido" 
            EstimatedTime="Tiempo estimado: " TotalFiles="Número de archivos: " 
            TransferSpeed="Velocidad: " UploadedFiles="Archivos adjuntados:"></Localization>
    </telerik:RadProgressArea>
    </form>
</body>
</html>
