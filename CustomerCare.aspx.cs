using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Telerik.Web.UI;
using System.Data.SqlClient;
using CustomerCare;

namespace CustomerCare
{
    public partial class CustomerCare : CustomerCareResource
    {
        protected override void OnLoad(EventArgs e)
        {
            Nombre = "CustomerCare";
            Controles = new string[] { "Menu", "TabStrip", "Window" };
            base.OnLoad(e);
        }
        protected void redireccionar()
        {
            string urlredirect = "customercare.aspx?";
            foreach (string key in Request.QueryString.Keys)
            {
                if (key != "wy")
                    urlredirect += key + "=" + Request.QueryString[key] + "&";
            }
            urlredirect = urlredirect.Remove(urlredirect.Length - 1);
            Response.Redirect("index.aspx?url=" + Server.UrlEncode(urlredirect));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //agregaJavaScript("CustomerCare.js?" + DateTime.Now.Millisecond.ToString());
            //agregaJavaScript("scriptConfig.aspx?" + DateTime.Now.Millisecond.ToString());
            //func = new Funciones();
            Image1.ImageUrl = ImagesFolder + "logo2.png";
            //rwmPrincipal.Skin = Skin;
            rwmPrincipal.IconUrl = ImagesFolder + "favicon16x16.png";
            //rmpPrincipal.Skin = Skin;
            //ralpRelojito.Skin = Skin;
            //rcmTipificacion.Skin = Skin;
            //rcmConclusion.Skin = Skin;
            //RadMenu1.Skin = Skin;
            RadMenu1.GetAllItems()[0].Value = ConfigurationManager.AppSettings["nvotick"];
            //rtsPrincipal.Skin = Skin;
            RadMenu1.Items[0].Text = "Nuev" + ConfigurationManager.AppSettings["mascfem"] + " " + ConfigurationManager.AppSettings["ticket"];
            RadMenu1.Items[1].Items[0].Items[0].Text = ConfigurationManager.AppSettings["zona"] + "s";
            RadMenu1.Items[1].Items[0].Items[1].Text = ConfigurationManager.AppSettings["sitio"] + "s";
            //rtsPrincipal.Tabs[0].Text = ConfigurationManager.AppSettings["ticket"] + "s";
            string usu;
            dao.con.Open(); //dao.open();
            SqlCommand Qry = new SqlCommand("", dao.con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            if (!(Request.QueryString["wy"] == null || Request.QueryString["wy"] == "undefined"))
            {
                wy = Request.QueryString["wy"];
                usu = Utilidades.Crip(wy);
                if (!usuario.tienePermiso(Permiso.AccederPorURL))
                    redireccionar();
                else
                    Session.Add("wy", wy);
            }
            else
            {
                if (Request.Cookies.Get("DatosUsuario") == null || (Request.Cookies["DatosUsuario"]["Recordarme"] == null && Session["wy"] == null))
                    redireccionar();
                if (Session["usrdescrip"] != null)
                    Label1.Text = Session["usrdescrip"].ToString() + " ";
                else
                    Label1.Text = Request.Cookies["DatosUsuario"]["Nombre"] + " ";
                if (Session["wy"] == null)
                    Session.Add("wy",Request.Cookies["DatosUsuario"]["wy"]);
                wy = Session["wy"].ToString();
                usu = Utilidades.Crip(wy);
                try
                {
                    usuario.tienePermiso(Permiso.CancelarTickets);
                }
                catch
                {
                    Session.Abandon();
                    Session.Clear();
                    HttpCookie cook = new HttpCookie("DatosUsuario");
                    cook.Expires = new DateTime(2000,1,1);
                    Response.Cookies.Add(cook);
                    redireccionar();
                    return;
                }
            }
            hddUsuario.Value = wy;
            Qry.CommandText="Select Plural from Clasificaciones where Activa=1";
            SqlDataReader reader=Qry.ExecuteReader();
            while (reader.Read()){
                RadMenuItem item= new RadMenuItem (reader.GetString(0));
                item.Value="clasificacion"+reader.GetString(0);
                RadMenu1.Items[2].Items[3].Items.Add(item);
            }
            if (usuario.tienePermiso(Permiso.VerMenuDeAdministracion))
                RadMenu1.FindItemByText("Administración").Enabled = true;
            if (!usuario.tienePermiso(Permiso.ModificarTicketsProgramados))
                RadMenu1.FindItemByText("Administración").Items.FindItemByValue("programadas").Enabled = false;
            if (!usuario.tienePermiso(Permiso.ModificarFormatos))
                RadMenu1.FindItemByText("Administración").Items.FindItemByValue("formatos").Enabled = false;
            if (!usuario.tienePermiso(Permiso.ModificarUsuarios))
                RadMenu1.FindItemByText("Administración").Items.FindItemByValue("usuarios").Enabled = false;
            if (!usuario.tienePermiso(Permiso.ModificarSolicitantes))
                RadMenu1.FindItemByText("Administración").Items.FindItemByValue("solicitantes").Enabled = false;
            if (!usuario.tienePermiso(Permiso.ModificarTipificaciones))
                RadMenu1.FindItemByText("Administración").Items.FindItemByValue("tipificaciones").Enabled = false;
            if (!usuario.tienePermiso(Permiso.ModificarDatosExtra))
                RadMenu1.FindItemByText("Administración").Items.FindItemByValue("datosextra").Enabled = false;
            if (!usuario.tienePermiso(Permiso.LevantarTickets))
                RadMenu1.Items[0].Enabled = false;
            if (!usuario.tienePermiso(Permiso.VerEstadisticas))
                RadMenu1.Items[1].Enabled = false;
            if (!usuario.tienePermiso(Permiso.VerEncabezadoPrincipal))
            {
                logindisplay.Visible = false;
                logoprincipal.Visible = false;
            }
            dao.con.Close(); //dao.close();
            if (!(Request.QueryString["acc"] == null || Request.QueryString["acc"] == "undefined")){
                hddAccion.Visible=true;
                switch(Request.QueryString["acc"]){
                    case "ticket":
                        hddAccion.Value="ticket:" + Request.QueryString["tic"];
                        break;
                }
            }
            scriptVariables.AddClientIds(form1);
            scriptVariables.Add("imagesFolder", ImagesFolder);
            scriptVariables.Add("valorColumnaCodigo", ConfigurationManager.AppSettings["valorColumnaCodigo"]);
            scriptVariables.Add("preticket", ConfigurationManager.AppSettings["preticket"]);
            scriptVariables.Add("activo", ConfigurationManager.AppSettings["activo"]);
            scriptVariables.Add("enValidacion", ConfigurationManager.AppSettings["enValidacion"]);
            scriptVariables.Add("cerrado", ConfigurationManager.AppSettings["cerrado"]);
            scriptVariables.Add("cancelado", ConfigurationManager.AppSettings["cancelado"]);
            scriptVariables.Add("tituloVentanaValidar", ConfigurationManager.AppSettings["tituloVentanaValidar"]);
            scriptVariables.Add("ticket", ConfigurationManager.AppSettings["ticket"]);
            scriptVariables.Add("zona", ConfigurationManager.AppSettings["zona"]);
            scriptVariables.Add("sitio", ConfigurationManager.AppSettings["sitio"]);
            scriptVariables.Add("mascfem", ConfigurationManager.AppSettings["mascfem"]);
            scriptVariables.Add("Skin", ConfigurationManager.AppSettings["Telerik.Skin"]);
            scriptVariables.Add("tituloVentanaSolicitarValidacion", ConfigurationManager.AppSettings["tituloVentanaSolicitarValidacion"]);
            scriptVariables.Add("urlproyecto", ConfigurationManager.AppSettings["proyecto"]);
            scriptVariables.Add("urlencuestas", ConfigurationManager.AppSettings["encuestas"]);
            scriptVariables.Add("urlmapa", ConfigurationManager.AppSettings["mapa"]);
            scriptVariables.Add("checador", ConfigurationManager.AppSettings["checador"]);
            scriptVariables.Add("checadorI", ConfigurationManager.AppSettings["checadorI"]);

        }
        protected void rcmTipificacion_ItemDataBound(object sender, RadMenuEventArgs e)
        {
            e.Item.Attributes.Add("onkeyup", "cerrarMenu(event.keyCode)");
        }
        protected void rcmConclusion_ItemDataBound(object sender, RadMenuEventArgs e)
        {
            e.Item.Attributes.Add("onkeyup", "cerrarMenuC(event.keyCode)");
        }
    }
}