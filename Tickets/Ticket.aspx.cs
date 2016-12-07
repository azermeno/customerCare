using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using CustomerCare;

namespace CustomerCare
{
    public partial class TicketPage : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "Ticket";
            Controles = new string[] { "TabStrip", "Grid" };
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //func = new Funciones();
            //RadTabStrip1.Skin = ConfigurationManager.AppSettings["Skin"];
            //rgrEventos.Skin = ConfigurationManager.AppSettings["Skin"];
            string wy;
            if (Session["wy"] == null)
                wy = Request.QueryString["wy"];
            else
                wy = Session["wy"].ToString();
            string usu = Utilidades.Crip(wy);
            //dao.open(); //dao.open();
            //SqlCommand Qry = new SqlCommand("Select u.permisos from usuarios u where u.codigo=" + usu, dao.con);
            //string[] permisos = Qry.ExecuteScalar().ToString().Split(',');
            //foreach (string s in permisos)
            //    Response.Write(s+"&nbsp;");
            if (usuario.tienePermiso(Permiso.VerDatosExtra))
                RadTabStrip1.FindTabByValue("datosExtras").Visible = true;
            //dao.close(); //dao.close();
            RadTabStrip1.FindTabByValue("datosExtras").Text = ConfigurationManager.AppSettings["datosExtras"];
        }
    }
}