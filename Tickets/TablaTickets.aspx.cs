using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Telerik.Web.UI;

namespace CustomerCare
{
    public partial class TablaTickets : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "TablaTickets";
            Controles = new string[] { "Grid", "ToolBar", "TreeView", "ComboBox", "Calendar", "Input", "Dock" };
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //agregaCssSkin("TablaTickets");
            //agregaCss("cssTablaTickets.aspx");
            //if (!EmbeddedSkin)
            //{
            //    string[] controles = { "Grid", "ToolBar", "TreeView" };
            //    agregaMultipleCssSkin(controles);
            //}
            //RadToolBar1.Skin = ConfigurationManager.AppSettings["Skin"];
            RadToolBarDropDown rtbdUsuarios = (RadToolBarDropDown)RadToolBar1.Items[0];
            RadToolBarDropDown rtbdSolicitantes = (RadToolBarDropDown)RadToolBar1.Items[1];
            RadToolBarDropDown rtbdEstados = (RadToolBarDropDown)RadToolBar1.Items[2];
            
            RadToolBarDropDown rtbdFecha = (RadToolBarDropDown)RadToolBar1.Items[3];
            RadToolBarDropDown rtbdTipificacion = (RadToolBarDropDown)RadToolBar1.Items[4];
            RadToolBarDropDown rtbdPrioridad = (RadToolBarDropDown)RadToolBar1.Items[5];
            RadToolBarButton rtbbGuardar = (RadToolBarButton)RadToolBar1.Items[6];
            rtbbGuardar.ImageUrl = ImagesFolder + "guardarFiltro.png";
            RadToolBarButtonCollection checks = rtbdEstados.Buttons;
            checks[0].Text = ConfigurationManager.AppSettings["preticket"] + "s";
            checks[1].Text = ConfigurationManager.AppSettings["activo"] + "s";
            checks[2].Text = ConfigurationManager.AppSettings["enValidacion"];
            checks[3].Text = ConfigurationManager.AppSettings["cerrado"] + "s";
            checks[4].Text = ConfigurationManager.AppSettings["cancelado"] + "s";
            RadComboBox rcbUsuario = (RadComboBox)rtbdUsuarios.Buttons[0].FindControl("rcbUsuario");
            //rcbUsuario.Skin = ConfigurationManager.AppSettings["Skin"];
            RadTreeView rtvSolicitante = (RadTreeView)rtbdSolicitantes.Buttons[0].FindControl("rtvSolicitante");
            //rcbSolicitante.EmptyMessage = ConfigurationManager.AppSettings["solicitante"];
            //rtvSolicitante.Skin = ConfigurationManager.AppSettings["Skin"];
            RadGrid1.MasterTableView.GetColumn("Solicitante").HeaderText = ConfigurationManager.AppSettings["solicitante"];
            ((RadTextBox)rtbTipificar.FindItemByText("tipificacion").FindControl("RadTextBox1")).Attributes.Add("onclick", "showMenu(event)");
            //rdcAcciones.Skin = ConfigurationManager.AppSettings["Skin"];
            //RadComboBox rcbLevantador = (RadComboBox)RadToolBar1.Items[2].FindControl("rcbLevantador");
            //rcbLevantador.Skin = ConfigurationManager.AppSettings["Skin"];
            //RadGrid1.Skin = ConfigurationManager.AppSettings["Skin"];
            RadGrid1.MasterTableView.NoMasterRecordsText = "No hay " + Utilidades.ticket.ToLower() + "s con estos filtros.";
            RadGrid1.MasterTableView.PagerStyle.PagerTextFormat = "Cambiar página: {4} &nbsp;<strong>{5}</strong> " + Utilidades.ticket.ToLower() + "s en <strong>{1}</strong> páginas.";
            //if (Session["wy"] == null && (Request.QueryString["wy"] == null || Request.QueryString["wy"] == "undefined"))
            //    Response.Redirect("index.aspx?url=Tickets/TablaTickets.aspx?");
            //if (Session["wy"] == null)
            //    wy = Request.QueryString["wy"];
            //else
            //    wy = Session["wy"].ToString();
            //string usu = Utilidades.Crip(wy);
            //SqlConnection bdpCon;
            //bdpCon = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
            //bdpCon.Open();
            //SqlCommand Qry;
            //Qry = new SqlCommand();
            //Qry.Connection = bdpCon;
            //Qry.CommandText = "Select u.FiltroDefault from usuarios u where u.codigo=" + usuario.código;
            //SqlDataReader reader = Qry.ExecuteReader();
            //reader.Read();
            try
            {
                hddInterno.Value = Utilidades.Crip(usuario.código.ToString());
            }
            catch
            {
                hddInterno.Value = "";
            }
            //string a = reader.GetString(1);
            if (!usuario.tienePermiso(Permiso.VerTicketsInactivos))
                checks[0].Text = "";
            if (!usuario.tienePermiso(Permiso.VerTicketsCancelados))
                checks[4].Text = "";
            if (!usuario.tienePermiso(Permiso.VerLimiteDeTickets))
                RadGrid1.Columns[4].Visible = false;
            if (!usuario.tienePermiso(Permiso.VerResponsableDeTickets))
                RadGrid1.Columns[6].Visible = false;
            if (usuario.tienePermiso(Permiso.VerColumnaDeEstado))
                RadGrid1.Columns[8].Visible = true;
            if (!usuario.tienePermiso(Permiso.VerColumnaDeIconos))
                RadGrid1.Columns[1].Visible = false;
            hddFiltroDefault.Value = usuario.filtroDefault;
            //bdpCon.Close();
        }

        protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName == RadGrid.ExpandCollapseCommandName && e.Item is GridDataItem)
            {
                ((GridDataItem)e.Item).ChildItem.FindControl("InnerContainer").Visible =
                    !e.Item.Expanded;
            }
        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridNestedViewItem)
            {
                e.Item.FindControl("InnerContainer").Visible = ((GridNestedViewItem)e.Item).ParentItem.Expanded;
            }
        }
    }
}