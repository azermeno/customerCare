using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CustomerCare;

namespace CustomerCare
{
    public partial class Accion : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "Accion";
            Controles = new string[] { "Grid", "ToolBar", "TreeView", "ComboBox", "Calendar", "Input" };
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Header.Title = (Request.QueryString["acc"] == "esc" ? "Escalar" : Request.QueryString["acc"] == "rec" ? "Rechazar" : Request.QueryString["acc"] == "cer" ? "Cerrar" : Request.QueryString["acc"] == "pri" ? "Priorizar" : "Validar") + " " + Request.QueryString["cod"];
            rcbResponsable.Skin = System.Configuration.ConfigurationManager.AppSettings["Skin"];
            switch (Request.QueryString["acc"])
            {
                case "esc":
                    rtbConclusion.Visible = false;
                    rblTipoEvento.Visible = false;
                    rcbPrioridad.Visible = false;
                    break;
                case "cer":
                    rcbResponsable.Visible = false;
                    rblTipoEvento.Visible = false;
                    rcbPrioridad.Visible = false;
                    break;
                case "solval":
                    rtbConclusion.Visible = false;
                    rblTipoEvento.Visible = false;
                    rcbPrioridad.Visible = false;
                    break;
                case "pri":
                    if (!usuario.tienePermiso(Permiso.PriorizarCritico))
                    {
                        sdsPrioridad.SelectCommand = "SELECT Codigo, Descripcion FROM Severidades WHERE Codigo>1 ORDER BY Codigo";
                        rcbPrioridad.DataBind();
                    }
                    rcbResponsable.Visible = false;
                    rtbConclusion.Visible = false;
                    rblTipoEvento.Visible = false;
                    break;
                default:
                    rtbConclusion.Visible = false;
                    rcbResponsable.Visible = false;
                    rblTipoEvento.Visible = false;
                    rcbPrioridad.Visible = false;
                    break;
            }
            ////Funciones //func = new Funciones();
            scriptVariables.Add("est", Request.QueryString["est"]);
            scriptVariables.Add("acc", Request.QueryString["acc"]);
            scriptVariables.Add("sen", Request.QueryString["sen"]);
            scriptVariables.Add("cod", Request.QueryString["cod"]);
            //RadStyleSheetManager1=func.AgregaStyleSheets();
        }
    }
}