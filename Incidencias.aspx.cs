using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using Telerik.Web.UI;

namespace CustomerCare
{
    public partial class Incidencias : System.Web.UI.Page
    {
        public string wy;
        public string DeCrip(string cad)
        {
            string Result = "";
            foreach (char c in cad)
                Result = Result + (char)((int)c - 3);
            return (Result);
        }

        protected void RecargaFiltros()
        {
            string res = "0";
            if (rcbResponsable.SelectedItem != null)
                res = rcbResponsable.SelectedValue;
            string lev = "0";
            if (rcbLevantador.SelectedItem != null)
                lev = rcbLevantador.SelectedValue;
            string esc = "";
            if (rcbEscalador.SelectedItem != null)
                esc = rcbEscalador.SelectedValue;
            string sol = "0";
            if (rcbSolicitante.SelectedItem != null)
                sol = rcbSolicitante.SelectedValue;
            string pre = "0";
            if (cbxPreincidencias.Checked)
                pre = "1";
            string abi = "0";
            if (cbxAbiertas.Checked)
                abi = "1";
            string cer = "0";
            if (cbxCerradas.Checked)
                cer = "1";
            string can = "0";
            if (cbxCanceladas.Checked)
                can = "1";
            //string ade = "01/01/1900";
            //if (rdpAperturaDesde.DateInput.Text != "")
            //    ade = rdpAperturaDesde.SelectedDate.Value.ToString("dd/MM/yyyy");
            //string aha = "01/01/2100";
            //if (rdpAperturaHasta.DateInput.Text != "")
            //    aha = rdpAperturaHasta.SelectedDate.Value.ToString("dd/MM/yyyy");
            //string lde = "01/01/1900";
            //if (rdpLimiteDesde.DateInput.Text != "")
            //    lde = rdpLimiteDesde.SelectedDate.Value.ToString("dd/MM/yyyy");
            //string lha = "01/01/2100";
            //if (rdpLimiteHasta.DateInput.Text != "")
            //    lha = rdpLimiteHasta.SelectedDate.Value.ToString("dd/MM/yyyy");
            //string cld = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy");
            //if (rdpClausuraDesde.DateInput.Text != "")
            //    cld = rdpClausuraDesde.SelectedDate.Value.ToString("dd/MM/yyyy");
            //string clh = "01/01/2100";
            //if (rdpClausuraHasta.DateInput.Text != "")
            //    clh = rdpClausuraHasta.SelectedDate.Value.ToString("dd/MM/yyyy");
            //string cad = DateTime.Now.AddMonths(-3).ToString("dd/MM/yyyy");
            //if (rdpCancelaDesde.DateInput.Text != "")
            //    cad = rdpCancelaDesde.SelectedDate.Value.ToString("dd/MM/yyyy");
            //string cah = "01/01/2100";
            //if (rdpCancelaHasta.DateInput.Text != "")
            //    cah = rdpCancelaHasta.SelectedDate.Value.ToString("dd/MM/yyyy");
            //string tipificacion = "";
            //foreach (RadTreeNode nodo in RadTreeView1.GetAllNodes())
            //{
            //    if (!nodo.Checked)
            //        tipificacion = tipificacion + " AND Tipificacion<>" + nodo.Value;
            //}
            RadGrid1.MasterTableView.FilterExpression =
                "(res = " + res + " OR " + res + "=0) " +
                "       AND (lev = " + lev + " OR " + lev + "=0) " +
                "       AND (esc LIKE '%" + esc + "%') " +
                "       AND (sol='" + sol + "' OR " + sol + "=0) " +
                "       AND ( " +
                "           (Estado=2" + 
                //"                AND Apertura > '" + ade + "' AND Apertura < '" + aha + "' AND Limite > '" + lde + "' AND Limite < '" + lha + "'" +
                "                AND 1 = " + abi + ") " +
                "           OR (Estado = 4" + 
                //"                AND Clausura > '" + cld + "' AND Clausura < '" + clh + "'" +
                "                AND 1 = " + cer + ") " +
                "           OR (Estado = 5" +
                //"                AND Clausura > '" + cad + "' AND Clausura < '" + cad + "'" +
                "                AND 1 = " + can + ") " +
                "           OR (Estado = 1 AND 1 = " + pre + ") )";
                //"       ) " + tipificacion;
            RadGrid1.MasterTableView.Rebind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["wy"] == null && (Request.QueryString["wy"] == null || Request.QueryString["wy"] == "undefined"))
                Response.Redirect("index.aspx?url=nuevaincidencia.aspx?");
            if (Session["wy"] == null)
                wy = Request.QueryString["wy"];
            else
                wy = Session["wy"].ToString();
            if (!Page.IsPostBack)
            {
                foreach (RadTreeNode nodo in RadTreeView1.GetAllNodes())
                    nodo.Checked = true;
            }    
        string usucodigo=DeCrip(wy);
        hddUsuario.Value = usucodigo;
        }

        protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
        {
            
            //if (e.Item.ItemType == GridItemType.AlternatingItem || e.Item.ItemType == GridItemType.Item)
            //{
            //    //Response.Write("<br/>");
            //    //foreach (TableCell c in e.Item.Cells)
            //    //    Response.Write(c.Text + " ");
            //    int[] a;
            //    int i = 0;
            //    if (visible.Length != 0)
            //    {
            //        while (visible[i] < Convert.ToInt32(e.Item.Cells[2].Text))
            //        {
            //            i++;
            //            if (i == visible.Length)
            //                break;
            //        }
            //    }
            //    if (i == visible.Length)
            //    {
            //        e.Item.Display = false;
            //        visible = new int[0];
            //    }
            //    else
            //    {
            //        if (visible[i] > Convert.ToInt32(e.Item.Cells[2].Text))
            //            e.Item.Display = false;
            //        a = new int[visible.Length - i];
            //        Array.Copy(visible, i, a, 0, visible.Length - i);
            //        visible = a;
            //    }
            //}
        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                RecargaFiltros();
            }
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

        protected void cbxAbiertas_CheckedChanged(object sender, EventArgs e)
        {
            RecargaFiltros();
        }

        protected void cbxCerradas_CheckedChanged(object sender, EventArgs e)
        {
            RecargaFiltros();
        }

        protected void cbxPreincidencias_CheckedChanged(object sender, EventArgs e)
        {
            RecargaFiltros();
        }

        protected void cbxCanceladas_CheckedChanged(object sender, EventArgs e)
        {
            RecargaFiltros();
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            RecargaFiltros();
        }

        protected void rcbResponsable_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RecargaFiltros();
        }

        protected void rcbEscalador_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RecargaFiltros();
        }

        protected void rcbSolicitante_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RecargaFiltros();
        }

        protected void rcbLevantador_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RecargaFiltros();
        }
    }
}