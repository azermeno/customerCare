using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using Tickets;

namespace CustomerCare
{
    public partial class Programada : System.Web.UI.Page
    {
        Funciones fun;

        protected void Page_Load(object sender, EventArgs e)
        {
            fun=new Funciones();
            RadSkinManager1.Skin = ConfigurationManager.AppSettings["Skin"];
            rtbTipificacion.Attributes.Add("onclick", "showMenu(event)");
            rtbTipificacion.Attributes.Add("onkeyup", "showMenuK(event.keyCode)");
        }

        protected void rbtGuardar_Click(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString2"].ConnectionString);
            OleDbCommand qry = new OleDbCommand("UPDATE Programadas SET Asunto = '" + fun.SQLify(rtbAsunto.Text) + "', DetalleAsunto = '" + fun.SQLify(rtbDetalle.Text) + "' WHERE Codigo = " + hflCodigo.Value, con);
            con.Open();
            qry.ExecuteNonQuery();
            con.Close();
            Response.Write("<script type=\"text/javascript\">parent.location.reload();</script>");
        }
    }
}