using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Configuration;

namespace CustomerCare
{
    public partial class Indicadores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadChart3.Skin = ConfigurationManager.AppSettings["Skin"];
            rgTecnicos.Skin = ConfigurationManager.AppSettings["Skin"];
            rsTecnicos.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbZona0.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbCliente0.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbProducto0.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbMes0.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbMes.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbZona.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbTecnico.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbCliente.Skin = ConfigurationManager.AppSettings["Skin"];
            rsUnidades.Skin = ConfigurationManager.AppSettings["Skin"];
            //rgUnidades.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbProducto.Skin = ConfigurationManager.AppSettings["Skin"];
            rtsIndicadores.Skin = ConfigurationManager.AppSettings["Skin"];
            alpPrincipal.Skin = ConfigurationManager.AppSettings["Skin"];
            RadChart2.Skin = ConfigurationManager.AppSettings["Skin"];
            RadChart4.Skin = ConfigurationManager.AppSettings["Skin"];
            string mes, tecnico, zona, cliente, producto;
            if (rcbMes.SelectedValue == "")
                mes = DateTime.Now.ToString("yyyyMM");
            else
                mes = rcbMes.SelectedValue;
            if (rcbTecnico.SelectedValue == "")
                tecnico = "0";
            else
                tecnico = rcbTecnico.SelectedValue;
            if (rcbZona.SelectedValue == "")
                zona = "0";
            else
                zona = rcbZona.SelectedValue;
            if (rcbProducto.SelectedValue == "")
                producto = "0";
            else
                producto = rcbProducto.SelectedValue;
            if (rcbCliente.SelectedValue == "")
                cliente = "0";
            else
                cliente = rcbCliente.SelectedValue;
            SqlConnection con=new SqlConnection();
            con.ConnectionString = sdsClientes.ConnectionString;
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection=con;
            con.Open();
            query.CommandText = "Select Cast(ISNULL(dbo.Promedio('" + mes + "'," + tecnico + "," + zona + "," + producto + "," + cliente + "),0) as Decimal(5,2)) as ISC";
            //Response.Write("<br/>" + query.CommandText);
            lblNSC.Text = query.ExecuteScalar().ToString();
            con.Close();
        }

        protected void rcbProducto_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            //e.Item.Selected = false;
        }
    }
}