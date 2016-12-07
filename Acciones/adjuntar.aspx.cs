using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Telerik.Web.UI;
using System.IO;
using CustomerCare;

namespace CustomerCare
{
    public partial class Adjuntar : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "base";
            Controles = new string[] { "Upload", "Button" };
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["wy"] == null)
            {
                Response.Write("<script type=\"text/javascript\">top.location = \"customercare.aspx\";</script>");
                return;
            }
            RadUpload1.Skin = ConfigurationManager.AppSettings["Skin"];
            RadProgressArea1.Skin = ConfigurationManager.AppSettings["Skin"];
            RadButton1.Skin = ConfigurationManager.AppSettings["Skin"];
            //Funciones //func = new Funciones();
            //RadStyleSheetManager1 = func.AgregaStyleSheets();
        }

        protected void RadButton1_Click(object sender, EventArgs e)
        {
            string inc, pat, existen = "";
            inc = Request.QueryString["inc"];
            int i = 0;

            foreach (UploadedFile uf in RadUpload1.UploadedFiles)
            {
                pat = ConfigurationManager.AppSettings["rutaInstalacion"] + "\\Adjuntos\\" + inc + "u" + Session["wy"].ToString() + "___" + Path.GetFileName(uf.FileName);
                if (!File.Exists(pat))
                    uf.SaveAs(pat);
                else
                {
                    existen += Path.GetFileName(uf.FileName) + ", ";
                    i++;
                }
            }
            if (i > 0)
            {
                existen = existen.Remove(existen.Length - 2);
                if (i > 1)
                    existen = "Ya hay archivos adjuntos con los siguientes nombres: " + existen + ". Porfavor renómbrelos.";
                else
                    existen = "Ya hay un archivo adjunto con el nombre " + existen + ". Porfavor renómbrelo.";
            }
            int inv = RadUpload1.InvalidFiles.Count;
            if (inv > 0)
            {
                if (existen.Length > 0)
                    existen += " ";
                if (inv == 1)
                    existen += "El siguiente archivo es inválido: ";
                else
                    existen += "Los siguientes archivos son inválidos: ";
                foreach (UploadedFile uf in RadUpload1.InvalidFiles)
                {
                    existen += Path.GetFileName(uf.FileName) + ", ";
                    i++;
                }
                existen = existen.Remove(existen.Length - 2);
                existen += ".";
            }
            if (existen.Length > 0)
                Response.Write("<script type=\"text/javascript\">top.radalert(\"" + existen + "\");</script>");
            else
                Response.Write("<script type=\"text/javascript\">top.$find(\"rwmPrincipal\").closeActiveWindow(); top.getSelectedTabWindow().location.reload();</script>");
        }
    }
}