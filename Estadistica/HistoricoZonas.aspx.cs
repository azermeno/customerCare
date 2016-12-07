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

namespace CustomerCare.Scripts
{
    public partial class HistoricoZonas : Extra
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadMonthYearPicker rmyp = (RadMonthYearPicker)RadToolBar1.Items[3].FindControl("RadMonthYearPicker1");
            RadComboBox rcbz = (RadComboBox)RadToolBar1.Items[1].FindControl("rcbZona");
            if (!IsPostBack)
                ((RadMonthYearPicker)RadToolBar1.Items[3].FindControl("RadMonthYearPicker1")).SelectedDate = DateTime.Now.AddMonths(1-DateTime.Now.Month);
            RadSkinManager1.Skin = ConfigurationManager.AppSettings["Skin"];
            ((Label)RadToolBar1.Items[0].FindControl("lblZona")).Text = ConfigurationManager.AppSettings["zona"] + ":";
            //func = new Funciones();
            DataTable DatosG = new DataTable();
            DatosG.Columns.Add(new DataColumn("Mes", Type.GetType("System.String")));
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
            try
            {
                string zona = Convert.ToInt32(rcbz.SelectedValue).ToString();
                RadChart1.ChartTitle.TextBlock.Text = "Histórico de " + ConfigurationManager.AppSettings["ticket"].ToLower() + "s recibid" + ConfigurationManager.AppSettings["mascfem"] + "s por mes " + ConfigurationManager.AppSettings["zona"].ToLower() + " " + rcbz.SelectedItem.Text +".";
                query.CommandText = "select count(*), DATEPART(month,Apertura), DATEPART(year,apertura) from Incidencias where Estado>1 and Estado<5 " +
                    "and ((TipoSolicitante=1 and dbo.ZonaUnidad(Solicitante)=" + zona + ") or (TipoSolicitante=6 and dbo.ZonaSitio(Solicitante)=" + zona +
                    ") or (TipoSolicitante=3 and Solicitante=" + zona + ")) and ((DATEPART(year,Apertura)>" + selec.Year.ToString() + ") or (DATEPART(year,Apertura)=" + selec.Year.ToString()
                    + " and DATEPART(month,Apertura)>=" + selec.Month.ToString() + ")) " +
                    "group by DATEPART (month, apertura), DATEPART (year, apertura) " +
                    "order by DATEPART (year, apertura), DATEPART(month, apertura)";
            }
            catch
            {
                RadChart1.ChartTitle.TextBlock.Text = "Histórico de " + ConfigurationManager.AppSettings["ticket"].ToLower() + "s recibid" + ConfigurationManager.AppSettings["mascfem"] + "s por mes.";
                query.CommandText = "select count(*), DATEPART(month,Apertura), DATEPART(year,apertura) from Incidencias where Estado>1 and Estado<5 " +
                    "and ((DATEPART(year,Apertura)>" + selec.Year.ToString() + ") or (DATEPART(year,Apertura)=" + selec.Year.ToString()
                    + " and DATEPART(month,Apertura)>=" + selec.Month.ToString() + ")) " +
                    "group by DATEPART (month, apertura), DATEPART (year, apertura) " +
                    "order by DATEPART (year, apertura), DATEPART(month, apertura)";
            }
            dao.open(); //dao.open();
            SqlDataReader reader = query.ExecuteReader();
            int m = ((DateTime)rmyp.SelectedDate).Month;
            int y = ((DateTime)rmyp.SelectedDate).Year;
            reader.Read();
            while (y < DateTime.Now.Year || (y == DateTime.Now.Year && m < DateTime.Now.Month))
            {
                DataRow dr = DatosG.NewRow();
                dr["Mes"] = Utilidades.Mes(m) + " " + y.ToString();
                try
                {
                    if (reader.GetInt32(1) == m && reader.GetInt32(2) == y)
                    {
                        dr["Tickets"] = reader.GetInt32(0);
                        reader.Read();
                    }
                    else
                        dr["Tickets"] = 0;
                }
                catch
                {
                    dr["Tickets"] = 0;
                }
                if (m < 12)
                    m++;
                else
                {
                    m = 1;
                    y++;
                }
                DatosG.Rows.Add(dr);
            }
            dao.close(); //dao.close();
            RadChart1.DataSource = DatosG;
            RadChart1.DataBind();
            for (int i = 0; i < RadChart1.Series[0].Items.Count(); i++)
            {
                RadChart1.Series[0].Items[i].Label.TextBlock.Text = "#Y " + RadChart1.Series[0].Items[i].Label.TextBlock.Text;
            }
        }
    }
}