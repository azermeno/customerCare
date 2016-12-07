using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
//using System.IO.Compression;
using System.IO;

namespace CustomerCare
{
    public partial class Adjunto : System.Web.UI.Page
    {
        private string nombreAdjunto(string adjunto)
        {
           return adjunto.Substring(adjunto.IndexOf("___") + 3);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string rutaAdjuntos = ConfigurationManager.AppSettings["rutaInstalacion"] + "\\Adjuntos\\";
            Response.Clear();
            Response.ClearContent();
            string adjunto = Request.QueryString["arch"];
            string zipNombre = "zip.tmp";
            if (adjunto == null)
            {
                string tarea = Request.QueryString["tar"];
                int count=0;
                while (File.Exists(rutaAdjuntos + zipNombre))
                {
                    count++;
                    if (count == 1)
                        zipNombre = zipNombre.Substring(0, zipNombre.Length - 4) + "(" + count + ")" + ".tmp";
                    else
                        zipNombre = zipNombre.Substring(0, zipNombre.Length - 7) + "(" + count + ")" + ".tmp";
                }
                using (FileStream stream = new FileStream(rutaAdjuntos + zipNombre, FileMode.CreateNew))
                {
                    /* COMENTADO TEMPORALMENTE POR ALM 13102015
                    using (ZipArchive archivoZip = new ZipArchive(stream, ZipArchiveMode.Update))
                    {
                        string[] adjuntos = Directory.GetFiles(rutaAdjuntos, tarea + "*");
                        foreach (string adjuntoArchivo in adjuntos)
                        {
                            adjunto=adjuntoArchivo.Substring(adjuntoArchivo.LastIndexOf('\\')+1);
                            archivoZip.CreateEntryFromFile(adjuntoArchivo, nombreAdjunto(adjunto));
                        }
                    }
                     * */
                }
                Response.WriteFile(rutaAdjuntos + zipNombre);
                Response.AddHeader("Content-Disposition", "attachment; filename=Adjuntos_" + tarea + ".zip");
                adjunto=null;
            }
            else
            {
                Response.WriteFile(rutaAdjuntos + adjunto);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + nombreAdjunto(adjunto));
            }
            Response.AddHeader("Cache-Control", "public");
            Response.AddHeader("Content-Description", "File Transfer");
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "alga/verde";
            Response.Flush();
            if (adjunto == null)
            {
                File.Delete(rutaAdjuntos + zipNombre);
            }
        }
    }
}