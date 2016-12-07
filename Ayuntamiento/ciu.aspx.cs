using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace CustomerCare
{
    public partial class ciu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rtbDependencia.Attributes.Add("onclick", "showMenu(event)");
            rtbDependencia.Attributes.Add("onkeyup", "showMenuK(event.keyCode)");
            string qa = Request.QueryString["fl"];
            if (qa != null && qa != "")
            {
                Response.Clear();
                Response.Write("<h2>Gracias. Queja guardada con folio OA/" + (Convert.ToInt32(qa) - 113).ToString() + "/2011.</h2>");
                Response.End();
            }
            //Funciones //func = new Funciones();
            //RadStyleSheetManager1 = func.AgregaStyleSheets();
        }
        protected void Guardar(object seneder, EventArgs e)
        {
            string datos;
            string tipsol;
            switch (hflDependencia.Value.Substring(0, 1))
            {
                case "U":
                    tipsol = "1";
                    break;
                case "S":
                    tipsol = "6";
                    break;
                default:
                    tipsol = "3";
                    break;
            }
            datos = "{\"Nombre\":{\"Nombre\":\"" + rtbNombre.Text + "\",\"Apellido\":\"" + rtbApellido.Text + "\"},\"Dirección\":{\"Calle y número\":\"" + rtbDireccion.Text + "\",\"Colonia\":\"" + rtbColonia.Text + "\",\"C.P.\":\"" + rtbCP.Text + "\"},\"Teléfono\":\"" + rtbTelefono.Text + "\",\"Correo Electrónico\":\"" + rtbCorreo.Text + "\",\"Funcionario\":\"" + rtbFuncionario.Text + "\",\"Fecha de los hechos\":\"" + rdpFecha.DateInput.Text + "\"}";
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            query.CommandText = "INSERT Incidencias (Asunto,DetalleAsunto,Apertura,Levanto,TipoSolicitante,Solicitante,Responsable,Limite,Datos,Tipificacion,Estado) " +
                "VALUES('" + rtbHechos.Text + "','" + rtbDetalle.Text + "',GETDATE(),9," + tipsol + "," + hflDependencia.Value.Substring(1) + ",CAST(CEILING(RAND()*3) as int),DATEADD(DAY,1,GETDATE()),'" + datos + "',13,2)";
            con.Open();
            query.ExecuteNonQuery();
            query.CommandText = "select MAX(Codigo) from Incidencias";
            Response.Redirect("ciu.aspx?fl=" +
                //"1234");
                query.ExecuteScalar().ToString());
            con.Close();
        }
    }
}