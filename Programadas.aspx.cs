using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using Tickets;
using System.Collections;
using Telerik.Web.UI;

namespace CustomerCare
{
    public partial class Programadas : System.Web.UI.Page
    {
        Funciones fun;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["wy"] == null)
                Response.Write("<script type=\"text/javascript\">parent.location.href = \"/index.aspx\";</script>");
            fun = new Funciones();
            RadSkinManager1.Skin = ConfigurationManager.AppSettings["Skin"];
        }

        protected void rbtAceptar_Click(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString2"].ConnectionString);
            OleDbCommand qry = new OleDbCommand("INSERT Programadas (Nombre, Levanto) VALUES ('" + fun.SQLify(rtbNombre.Text) + "', " + fun.DeCrip(Session["wy"].ToString()) + ")", con);
            con.Open();
            qry.ExecuteNonQuery();
            con.Close();
            RadGrid1.DataBind();
            RadGrid1.Items[RadGrid1.Items.Count - 1].Selected = true;
            RadDock1.Closed = true;
        }

        protected void rbtEliminar_Click(object sender, EventArgs e)
        {
            if (RadGrid1.SelectedItems.Count == 1)
            {
                OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString2"].ConnectionString);
                OleDbCommand qry = new OleDbCommand("DELETE Programadas WHERE Codigo = " + ((GridDataItem)RadGrid1.SelectedItems[0])["Codigo"], con);
                con.Open();
                qry.ExecuteNonQuery();
                con.Close();
                RadGrid1.DataBind();
            }
        }
    }
}