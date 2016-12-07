<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="scriptConfig.aspx.cs" Inherits="CustomerCare.Scripts.WebForm2" %>

function valorColumnaCodigo(tickets, i) {
    <%= ConfigurationManager.AppSettings["valorColumnaCodigo"] %>
}
function valorBuscar(texto) {
    return texto;
}
function estado(n) {
    switch (n) {
        case 1: return "<%= ConfigurationManager.AppSettings["preticket"] %>"; break;
        case 2: return "<%= ConfigurationManager.AppSettings["activo"] %>"; break;
        case 3: return "<%= ConfigurationManager.AppSettings["enValidacion"] %>"; break;
        case 4: return "<%= ConfigurationManager.AppSettings["cerrado"] %>"; break;
        case 5: return "<%= ConfigurationManager.AppSettings["cancelado"] %>"; break;
    }
}
var tituloVentanaValidar = "<%= ConfigurationManager.AppSettings["tituloVentanaValidar"] %>";
var ticket = "<%= ConfigurationManager.AppSettings["ticket"] %>";
var zona = "<%= ConfigurationManager.AppSettings["zona"] %>";
var sitio = "<%= ConfigurationManager.AppSettings["sitio"] %>";
var mascfem = "<%= ConfigurationManager.AppSettings["mascfem"] %>";
var tituloVentanaSolicitarValidacion = "<%= ConfigurationManager.AppSettings["tituloVentanaSolicitarValidacion"]%>";
var tituloVentanaValidar = "<%= ConfigurationManager.AppSettings["tituloVentanaValidar"]%>";