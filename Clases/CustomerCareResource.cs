using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using Westwind.Web;
using System.Linq;

namespace CustomerCare
{
    public class Extra : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "Extra";
            Controles = new string[] { "ToolBar", "ComboBox", "Calendar", "Input" };
        }
    }

    public class CustomerCareResource : System.Web.UI.Page
    {
        private string version;
        protected ScriptVariables scriptVariables;
        protected string Skin;
        protected string FolderSkin;
        protected string FolderSkinGlobal;
        protected static string FolderEstilos = "Estilos/";
        protected static string FolderEstilosGlobal = "/Estilos/";
        protected string ImagesFolder;
        protected string Ticket;
        protected string MascFem;
        protected bool EmbeddedSkin;
        protected string Tipo { get; set; }
        protected string Nombre { get; set; }
        protected string[] Controles { get; set; }
        protected DAO dao;
        //protected int usu;
        protected Usuario usuario;

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            version = ConfigurationManager.AppSettings["version"];
            Ticket = ConfigurationManager.AppSettings["ticket"];
            MascFem = ConfigurationManager.AppSettings["mascfem"];
            Skin = ConfigurationManager.AppSettings["Telerik.Skin"];
            FolderSkin = FolderEstilos + Skin + "/";
            FolderSkinGlobal = FolderEstilosGlobal + Skin + "/";
            ImagesFolder = FolderEstilosGlobal + Skin + "/Images/";
            EmbeddedSkin = (ConfigurationManager.AppSettings["Telerik.EnableEmbeddedSkins"].Trim().ToLower() == "true");
            Controles = new string[0];
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            dao = new DAO();
            scriptVariables = new ScriptVariables();
        }

        //protected override void OnUnload(EventArgs e)
        //{
        //    base.OnUnload(e);
        //    dao.close();
        //}

        protected override void OnLoad(EventArgs e)
        {
            if (Session["wy"] == null)
            {
                if (Request.Url.PathAndQuery.IndexOf("index.aspx") == -1)
                {
                    Response.Redirect("/index.aspx?cad=si&url=" + Server.UrlEncode(Request.Url.PathAndQuery));
                    return;
                }
            }
            else
            {
                dao.open();
                usuario = dao.getUsuario(Convert.ToInt32(Utilidades.Crip(Session["wy"].ToString())));
                dao.close();
            }
            agregaLink(ImagesFolder + "favicon16x16.png", "icon", "image/png", "16x16");
            agregaLink(ImagesFolder + "favicon32x32.png", "icon", "image/png", "32x32");
            agregaLink(ImagesFolder + "favicon48x48.png", "icon", "image/png", "48x48");
            agregaJavaScriptGlobal("jquery-1.7.2.min.js");
            agregaJavaScript(Nombre + ".js?version=" + version);
            agregaCssSkinGlobal("base");
            agregaCssSkin(Nombre);
            agregaCssGlobal("base.css?version=" + version);
            if (!EmbeddedSkin)
            {
                agregaMultipleCssSkinGlobal(Controles);
            }
            agregaCss(Nombre + ".css?version=" + version);
            base.OnLoad(e);
        }

        protected void agregaJavaScript(string path)
        {
            var script = new HtmlGenericControl("script");
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", "Scripts/" + path);
            Page.Header.Controls.Add(script);
        }

        protected void agregaJavaScriptGlobal(string path)
        {
            var script = new HtmlGenericControl("script");
            script.Attributes.Add("type", "text/javascript");
            script.Attributes.Add("src", "/Scripts/" + path);
            Page.Header.Controls.Add(script);
        }

        private void agregaLink(string href, Dictionary<string, string> atributos)
        {
            var link = new HtmlLink { Href = href };
            foreach (KeyValuePair<string, string> atributo in atributos)
                link.Attributes.Add(atributo.Key, atributo.Value);
            Page.Header.Controls.Add(link);
        }

        protected void agregaLink(string href, string rel)
        {
            Dictionary<string, string> atributos = new Dictionary<string, string>();
            atributos.Add("rel", rel);
            agregaLink(href, atributos);
        }

        protected void agregaLink(string href, string rel, string type)
        {
            Dictionary<string, string> atributos = new Dictionary<string, string>();
            atributos.Add("rel", rel);
            atributos.Add("type", type);
            agregaLink(href, atributos);
        }

        protected void agregaLink(string href, string rel, string type, string sizes)
        {
            Dictionary<string, string> atributos = new Dictionary<string, string>();
            atributos.Add("rel", rel);
            atributos.Add("type", type);
            atributos.Add("sizes", sizes);
            agregaLink(href, atributos);
        }

        protected void agregaCss(string path)
        {
            agregaLink(FolderEstilos + path, "stylesheet", "text/css");
        }

        protected void agregaCssGlobal(string path)
        {
            agregaLink(FolderEstilosGlobal + path, "stylesheet", "text/css");
        }

        protected void agregaCssSkin(string path)
        {
            agregaLink(FolderSkin + path + ".css" + "?version=" + version, "stylesheet", "text/css");
        }

        protected void agregaCssSkinGlobal(string path)
        {
            agregaLink(FolderSkinGlobal + path + ".css" + "?version=" + version, "stylesheet", "text/css");
        }

        protected void agregaMultipleCssSkin(string[] csss)
        {
            foreach (string css in csss)
            {
                agregaCssSkin(css);
            }
        }

        protected void agregaMultipleCssSkinGlobal(string[] csss)
        {
            foreach (string css in csss)
            {
                agregaCssSkinGlobal(css);
            }
        }

        protected string wy;
    }
}