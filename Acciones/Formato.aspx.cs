using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Telerik.Web.UI;
using CustomerCare;

namespace CustomerCare
{
    public partial class Formato : System.Web.UI.Page
    {
        private string cdg;
        private string tic;
        //private string rutaFormatos = ConfigurationManager.AppSettings["rutaInstalacion"] + "\\HTML\\Formatos\\";
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
            Utilidades.log("Quiero guardar documento");
            if (Request.QueryString["c"] == null)
            {
                Utilidades.log("Estoy en la parte UNO");
                Response.Write("<script type=\"text/javascript\"> var tic=" + Request.QueryString["tic"] + "; var cdg=" + Request.QueryString["cdg"] + ";</script>");
                cdg = Request.QueryString["cdg"];
                tic = Request.QueryString["tic"];
                Utilidades.log("cdg: " + cdg + " tic:"+tic);
                if (!IsPostBack)
                {
                    //Funciones //func = new Funciones();
                    //string[] vFormato = Directory.GetFiles(rutaFormatos, cdg + "i" + tic + "v*u*.html");
                    SqlCommand query = new SqlCommand("SELECT count(*) FROM versiones WHERE Formato=" + cdg + " AND Ticket=dbo.CodigoSimple('" + tic + "')", con);
                    Utilidades.log(query.CommandText);
                    try
                    {
                        query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                    }
                    catch
                    {
                        query.CommandTimeout = 300;
                    }
                    con.Open();
                    int ver = (int)query.ExecuteScalar();
                    Utilidades.log("versiones: "+ver);
                    if (ver == 0)
                    {
                        RadToolBar1.Items[1].Enabled = false;
                        query.CommandText="select texto from Formatos where Codigo = " + cdg;
                        divContenido.InnerHtml = query.ExecuteScalar().ToString();
                    }
                    else
                    {
                        RadToolBarDropDown Versiones = ((RadToolBarDropDown)RadToolBar1.Items[1]);
                        query.CommandText = "select u.nombre, fecha, version, texto from versiones v join usuarios u on v.usuario=u.codigo " +
                            "where v.formato=" + cdg + " and v.ticket=dbo.CodigoSimple('" + tic + "') order by version asc";
                        SqlDataReader reader = query.ExecuteReader();
                        Utilidades.log(query.CommandText);
                        for (int i=0; i<ver; i++)
                        {
                            Utilidades.log("comienzo por el :" + i);
                            reader.Read();
                            //string usuario = versi.Substring(versi.IndexOf('u') + 1);
                            //usuario = usuario.Remove(usuario.Length - 5);
                            //usuario = Utilidades.Crip(usuario);
                            //query.CommandText = "Select Nombre from Usuarios where Codigo=" + usuario;
                            //FileInfo fi = new FileInfo(vfor);
                            string botontexto = reader.GetString(0);
                            botontexto += " " + reader.GetDateTime(1).ToString("ddMMMyy HH:mm");
                            RadToolBarButton boton = new RadToolBarButton(botontexto);
                            boton.Value = reader.GetInt32(2).ToString();
                            boton.Group = "Versiones";
                            boton.CheckOnClick = true;
                            boton.PostBack = true;
                            Versiones.Buttons.Add(boton);
                            Utilidades.log("paso por el :" + i);
                        }
                        ////Versiones.Buttons.FindButtonByValue(ver.ToString()).Checked = true;
                        divContenido.InnerHtml = reader.GetString(3);
                        reader.Close();
                        Utilidades.log("TErmino");
                    }
                    con.Close();
                }
            }
            else
            {
                Utilidades.log("Estoy en la parte DOS");
                RadToolBar1.Visible = false;
                Response.Clear();
                Response.ClearContent();
                if (Request.QueryString["c"] != null)
                {
                    try
                    {
                        SqlCommand qry = new SqlCommand("select Texto from Formatos where Codigo=" + Request.QueryString["c"], con);
                        try
                        {
                            qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                        }
                        catch
                        {
                            qry.CommandTimeout = 300;
                        }
                        con.Open();
                        Response.Write(qry.ExecuteScalar().ToString());
                        con.Close();
                    }
                    catch
                    {
                        Response.Write("<html><head><title>Error</title></head><body><h1>El formato buscado no existe o no fue especificado</h1></body></html>");
                    }
                }
            }
        }

        protected void RadToolBar1_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
            SqlCommand query = new SqlCommand("SELECT texto FROM versiones WHERE Formato=" + cdg + " AND Ticket=dbo.CodigoSimple('" + tic + "') and version=" + e.Item.Value, con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            //string ver = Directory.GetFiles(rutaFormatos, cdg + "i" + tic + "v" + e.Item.Value + "u*.html")[0];
            con.Open();
            divContenido.InnerHtml = (string)query.ExecuteScalar();
            con.Close();
        }
    }
}