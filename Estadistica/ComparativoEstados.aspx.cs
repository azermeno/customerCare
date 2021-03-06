﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CustomerCare;
using Telerik.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CustomerCare
{
    public partial class ComparativoEstados : Extra
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
                RadChart1.ChartTitle.TextBlock.Text = ConfigurationManager.AppSettings["ticket"] + "s por estado recibid" + ConfigurationManager.AppSettings["mascfem"] + "s en " + ((DateTime)rmyp.SelectedDate).ToString("MMMM' de 'yyyy") + ".";
                query.CommandText = "select e.Descripcion, COUNT(*) from Incidencias i join Estados e on i.Estado=e.Codigo where Datepart (month, Apertura) = " +
                    ((DateTime)rmyp.SelectedDate).Month.ToString() + " and Datepart (year, Apertura)=" +
                    ((DateTime)rmyp.SelectedDate).Year.ToString() + " group by e.Descripcion";
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

                RadChart1.ChartTitle.TextBlock.Text = ConfigurationManager.AppSettings["ticket"] + "s por estado recibid" + ConfigurationManager.AppSettings["mascfem"] + "s en " + rntb.Text + ".";
                query.CommandText = "select e.Descripcion, COUNT(*) from Incidencias i join Estados e on i.Estado=e.Codigo where Datepart (year, Apertura)=" +
                    A + " group by e.Descripcion";
            }
            dao.open(); //dao.open(); //dao.open();
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
            for (int i = 0; i<RadChart1.Series[0].Items.Count(); i++){
                RadChart1.Series[0].Items[i].Label.TextBlock.Text = RadChart1.Series[0].Items[i].Label.TextBlock.Text + ": #Y " + ConfigurationManager.AppSettings["ticket"].ToLower() + "s, #%";
            }
            //RadStyleSheetManager1 = func.AgregaStyleSheets();
        }
    }
}