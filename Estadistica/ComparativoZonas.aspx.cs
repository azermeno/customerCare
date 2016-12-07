using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CustomerCare;
using System.Configuration;
using Telerik.Web.UI;

namespace CustomerCare
{
    public partial class ComparativoZonas : Extra
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (((RadToolBarButton)RadToolBar1.Items[0]).Checked)
            {
                RadToolBar1.Items[3].FindControl("RadMonthYearPicker1").Visible = true;
                RadToolBar1.Items[3].FindControl("RadNumericTextBox1").Visible = false;
                ((Label)RadToolBar1.Items[2].FindControl("lblMA")).Text = "Mes:";
            }
            else
            {
                RadToolBar1.Items[3].FindControl("RadMonthYearPicker1").Visible = false;
                RadToolBar1.Items[3].FindControl("RadNumericTextBox1").Visible = true;
                ((Label)RadToolBar1.Items[2].FindControl("lblMA")).Text = "Año:";
            }
            RadMonthYearPicker rmyp = (RadMonthYearPicker)RadToolBar1.Items[3].FindControl("RadMonthYearPicker1");
            RadMaskedTextBox rntb = (RadMaskedTextBox)RadToolBar1.Items[3].FindControl("RadNumericTextBox1");
            if (!IsPostBack)
            {
                ((RadMonthYearPicker)RadToolBar1.Items[3].FindControl("RadMonthYearPicker1")).SelectedDate = DateTime.Now;
                ((RadMaskedTextBox)RadToolBar1.Items[3].FindControl("RadNumericTextBox1")).Text = DateTime.Now.Year.ToString();
            }
            //RadSkinManager1.Skin = ConfigurationManager.AppSettings["Skin"];
            //func = new Funciones();
            DataTable DatosG = new DataTable();
            DatosG.Columns.Add(new DataColumn("Zona", Type.GetType("System.String")));
            DatosG.Columns.Add(new DataColumn("Tickets", Type.GetType("System.Int32")));
            SqlCommand query = new SqlCommand("", dao.con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            DateTime selec = ((DateTime)rmyp.SelectedDate);
            if (((RadToolBarButton)RadToolBar1.Items[0]).Checked)
            {
                RadChart1.ChartTitle.TextBlock.Text = "Comparativo de " + ConfigurationManager.AppSettings["zona"].ToLower() + "s por " + ConfigurationManager.AppSettings["ticket"].ToLower() + "s recibid" + ConfigurationManager.AppSettings["mascfem"] + "s en " + ((DateTime)rmyp.SelectedDate).ToString("MMMM' de 'yyyy") + ".";
                query.CommandText = "select Nombre, ISNULL(Tickets,0) as Tickets from Zonas z join " +
                    "(select COUNT(*) as Tickets, CASE TipoSolicitante WHEN 1 THEN dbo.ZonaUnidad(Solicitante) WHEN 3 then Solicitante WHEN 6 then dbo.ZonaSitio(Solicitante) END as Zona " +
                    "from Incidencias where Estado>1 and Estado<5 and Tiposolicitante in(1,3,6) and Datepart (month, Apertura) = " + 
                    ((DateTime)rmyp.SelectedDate).Month.ToString() + " and Datepart (year, Apertura)=" +
                    ((DateTime)rmyp.SelectedDate).Year.ToString() + " group by CASE TipoSolicitante WHEN 1 THEN dbo.ZonaUnidad(Solicitante) WHEN 3 then Solicitante WHEN 6 then dbo.ZonaSitio(Solicitante) END) t on z.Codigo=t.Zona " +
                    "WHERE z.Activa=1";
            }
            else
            {
                string A = DateTime.Now.Year.ToString();
                try
                {
                    A = (new DateTime(Convert.ToInt32(rntb.Text), 1, 1)).Year.ToString();
                }
                catch
                {
                    ((RadMaskedTextBox)RadToolBar1.Items[3].FindControl("RadNumericTextBox1")).Text = DateTime.Now.Year.ToString();
                }
                    
                RadChart1.ChartTitle.TextBlock.Text = "Comparativo de " + ConfigurationManager.AppSettings["zona"].ToLower() + "s por " + ConfigurationManager.AppSettings["ticket"].ToLower() + "s recibid" + ConfigurationManager.AppSettings["mascfem"] + "s en " + rntb.Text + ".";
                query.CommandText = "select Nombre, ISNULL(Tickets,0) as Tickets from Zonas z join " +
                    "(select COUNT(*) as Tickets, CASE TipoSolicitante WHEN 1 THEN dbo.ZonaUnidad(Solicitante) WHEN 3 then Solicitante WHEN 6 then dbo.ZonaSitio(Solicitante) END as Zona " +
                    "from Incidencias where Estado>1 and Estado<5 and Tiposolicitante in(1,3,6) and Datepart (year, Apertura)=" +
                    A + " group by CASE TipoSolicitante WHEN 1 THEN dbo.ZonaUnidad(Solicitante) WHEN 3 then Solicitante WHEN 6 then dbo.ZonaSitio(Solicitante) END) t on z.Codigo=t.Zona " +
                    "WHERE z.Activa=1";
            }
            dao.open(); //dao.open();
            SqlDataReader reader = query.ExecuteReader();
            while (reader.Read())
            {
                DataRow dr = DatosG.NewRow();
                dr["Zona"] = reader.GetString(0);
                dr["Tickets"] = reader.GetInt32(1);
                DatosG.Rows.Add(dr);
            }
            dao.close(); //dao.close();
            RadChart1.DataSource = DatosG;
            RadChart1.DataBind();
            for (int i = 0; i < RadChart1.Series[0].Items.Count(); i++)
            {
                RadChart1.Series[0].Items[i].Label.TextBlock.Text = "#Y " + RadChart1.Series[0].Items[i].Label.TextBlock.Text;
            }
            //RadStyleSheetManager1 = func.AgregaStyleSheets();
        }
    }
}