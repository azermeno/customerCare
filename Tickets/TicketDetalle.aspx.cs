using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;
using CustomerCare;
using System.Data.SqlClient;
using Newtonsoft.Json.Serialization;
using Telerik.Web.UI;
using System.Data;

namespace CustomerCare
{
    public partial class TicketDetalle : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "TicketDetalle";
            Controles = new string[] { "ToolBar", "Splitter", "Input", "Grid", "Menu" };
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            rcmSolicitante.Skin = ConfigurationManager.AppSettings["Skin"];
            ((Label)rtbResponsable.Items[3].FindControl("lblSolicitante")).Text = System.Configuration.ConfigurationManager.AppSettings["solicitante"] + ":";
            //func = new Funciones();
            RadToolBar1.FindItemByValue("validacion").Text = ConfigurationManager.AppSettings["tituloVentanaSolicitarValidacion"];
            RadToolBar1.FindItemByValue("validar").Text = ConfigurationManager.AppSettings["tituloVentanaValidar"];
            rpEventos.Height = Convert.ToInt32(ConfigurationManager.AppSettings["altoEve"]);
            RadSkinManager1.Skin = ConfigurationManager.AppSettings["Skin"];
            //string rutaTickets = ConfigurationManager.AppSettings["rutaInstalacion"] + "\\HTML\\Tickets\\";
            string wy;
            if (Session["wy"] == null)
            {
                Response.Write("<script type=\"text/javascript\">parent.location = \"customercare.aspx\";</script>");
                return;
            }
            else
                wy = Session["wy"].ToString();
            string usu = Utilidades.Crip(wy);
            string tic = Request.QueryString["tic"];
            hfCodigo.Value = tic;
            SqlCommand query = new SqlCommand(
                "Declare @tic int Set @tic = dbo.CodigoSimple('" + tic + "'); " +
                "Select " + 
                    "isnull(i.Asunto,''), " + 
                    "isnull(i.DetalleAsunto,''), " + 
                    "i.Datos, " + 
                    "isnull(convert(varchar(32),i.Apertura,100),''), " +
                    "isnull(convert(varchar(32),i.Clausura,100),''), " +
                    "isnull(convert(varchar(32),i.Limite,100),''), " + 
                    "ISNULL(" + 
                        "CASE i.TipoSolicitante  " +
                            "WHEN 1 THEN un.Nombre  " +
                            "WHEN 2 THEN gs.Nombre  " +
                            "WHEN 3 THEN zo.Nombre  " +
                            "WHEN 4 THEN u3.Nombre  " +
                            "WHEN 5 THEN u4.Nombre  " +
                            "WHEN 6 THEN ss.Nombre  " +
                        "END, " + 
                        "''" + 
                    ") AS SolNombre, " + 
                    "isnull(i.DetalleSolucion,''), " + 
                    "dbo.Tipificacion(i.Tipificacion) as Tipificacion, " +
                    "i.Estado, " +
                    "i.Tipificacion as tipi, " +
                    "ISNULL(u2.Nombre, '') AS ResNombre " + 
                "from " +
                    "Incidencias i LEFT JOIN " +
                    "Unidades un on un.codigo=i.solicitante LEFT JOIN " +
                    "GruposSolucion gs on gs.codigo=i.solicitante LEFT JOIN " +
                    "Zonas zo on zo.codigo=i.solicitante LEFT JOIN " +
                    "Sitios ss on ss.codigo=i.solicitante LEFT JOIN " +
                    "Usuarios u3 on u3.codigo=i.solicitante LEFT JOIN " +
                    "Internos it on it.codigo=i.solicitante LEFT JOIN " +
                    "Usuarios u4 on it.usuario=u4.codigo LEFT JOIN " +
                    "Usuarios u2 on i.responsable=u2.codigo " +
                "where i.Codigo=@tic", 
                dao.con
            );
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            dao.open(); //dao.open();
            SqlDataReader reader = query.ExecuteReader();
            string tipi = "0";
            if (reader.Read())
            {
                Label lblTitulo = (Label)(rtbTitulo.Items[0].FindControl("lblTitulo"));
                RadTextBox rtbtip = (RadTextBox)(rtbTipificacion.Items[1].FindControl("rtbTipi"));
                RadTextBox rtbsol = (RadTextBox)(rtbResponsable.Items[4].FindControl("rtbSoli"));
                RadTextBox rtbres = (RadTextBox)(rtbResponsable.Items[1].FindControl("rtbResp"));
                Label rtblim = (Label)(rtbLimite.Items[0].FindControl("rtbLimi"));
                Label rtbini = (Label)(rtbLimite.Items[0].FindControl("rtbIni"));
                Label rtbcla = (Label)(rtbLimite.Items[0].FindControl("rtbCl"));
                lblTitulo.Text = ConfigurationManager.AppSettings["ticket"] + " " + tic;
                tbxAsunto.Text = reader.GetString(0);
                rtbDetalle.Text = reader.GetString(1);
                string ini = reader.GetString(3);
                string cla = reader.GetString(4);
                string lim = reader.GetString(5);
                rtbsol.Text = reader.GetString(6);
                rtbres.Text = reader.GetString(11);
                rtbSolucion.Text = reader.GetString(7);
                rtbtip.Text = reader.GetString(8);
                tipi = reader.GetString(10);
                ini = (ini == "" ? "Sin fecha de inicio" : ini);
                cla = (cla == "" ? "Abiert" + ConfigurationManager.AppSettings["mascfem"] : cla);
                lim = (lim == "" ? "Sin límite" : lim);
                rtbini.Text = ini;
                rtbcla.Text = cla;
                rtblim.Text = lim;
                switch (reader.GetInt32(9))
                {
                    case 1:
                        RadToolBar1.FindItemByValue("validar").Enabled = false;
                        RadToolBar1.FindItemByValue("rechazar").Enabled = false;
                        RadToolBar1.FindItemByValue("validacion").Enabled = false;
                        RadToolBar1.FindItemByValue("cerrar").Enabled = false;
                        rtbTipificacion.Items[2].Enabled = true;
                        rtbResponsable.Items[5].Enabled = true;
                        break;
                    case 2:
                        RadToolBar1.FindItemByValue("validar").Enabled = false;
                        RadToolBar1.FindItemByValue("rechazar").Enabled = false;
                        RadToolBar1.FindItemByValue("validacion").Enabled = true;
                        RadToolBar1.FindItemByValue("cerrar").Enabled = true;
                        rtbTipificacion.Items[2].Enabled = true;
                        rtbResponsable.Items[5].Enabled = true;
                        break;
                    case 3:
                        RadToolBar1.FindItemByValue("validar").Enabled = true;
                        RadToolBar1.FindItemByValue("rechazar").Enabled = true;
                        RadToolBar1.FindItemByValue("validacion").Enabled = false;
                        RadToolBar1.FindItemByValue("cerrar").Enabled = false;
                        rtbTipificacion.Items[2].Enabled = true;
                        rtbResponsable.Items[5].Enabled = true;
                        break;
                    case 4:
                        RadToolBar1.FindItemByValue("validar").Enabled = false;
                        RadToolBar1.FindItemByValue("rechazar").Enabled = false;
                        RadToolBar1.FindItemByValue("validacion").Enabled = false;
                        RadToolBar1.FindItemByValue("cerrar").Enabled = false;
                        rtbTipificacion.Items[2].Enabled = false;
                        rtbResponsable.Items[5].Enabled = false;
                        break;
                    case 5:
                        RadToolBar1.FindItemByValue("validar").Enabled = false;
                        RadToolBar1.FindItemByValue("rechazar").Enabled = false;
                        RadToolBar1.FindItemByValue("validacion").Enabled = false;
                        RadToolBar1.FindItemByValue("cerrar").Enabled = false;
                        rtbTipificacion.Items[2].Enabled = false;
                        rtbResponsable.Items[5].Enabled = false;
                        break;
                }
            }
            reader.Close();
            //SqlCommand Qry = new SqlCommand("Select u.permisos from usuarios u where u.codigo=" + usu, dao.con);
            //string[] permisos = Qry.ExecuteScalar().ToString().Split(',');
            //if (!permisos.Contains("13"))
            if (!usuario.tienePermiso(Permiso.EditarTickets))
                RadToolBar1.Items[0].Enabled = false;
                //if (!permisos.Contains("16"))
            if (!usuario.tienePermiso(Permiso.TipificarTickets))
                rtbTipificacion.Items[2].Enabled = false;
                //if (!permisos.Contains("25"))
            if (!usuario.tienePermiso(Permiso.RepactarTickets))
                rtbLimite.Items[1].Enabled = false;
            DataTable dtAdjuntos = new DataTable();
            dtAdjuntos.Columns.Add(new DataColumn("archivo", Type.GetType("System.String")));
            string rutaAdjuntos = ConfigurationManager.AppSettings["rutaInstalacion"] + "\\Adjuntos\\";
            string[] adjuntos = Directory.GetFiles(rutaAdjuntos, tic + "*");
            foreach (string adjunto in adjuntos)
            {
                DataRow arch = dtAdjuntos.NewRow();
                arch["archivo"] = "<a href=\"/Acciones/Adjunto.aspx?arch=" + adjunto.Replace(rutaAdjuntos, "") + "\">" + adjunto.Substring(adjunto.IndexOf("___") + 3) + "</a>";
                dtAdjuntos.Rows.Add(arch);
            }
            rgrAdjuntos.DataSource = dtAdjuntos;
            rgrAdjuntos.DataBind();
            DataTable dtFormatos = new DataTable();
            dtFormatos.Columns.Add(new DataColumn("formato", Type.GetType("System.String")));
            //string rutaFormatos = ConfigurationManager.AppSettings["rutaInstalacion"] + "\\HTML\\Formatos\\";
            query.CommandText = "Select dbo.rutTip(" + tipi + ")";
            string[] ruta = query.ExecuteScalar().ToString().Split(',');
            foreach (string tip in ruta)
            {
                query.CommandText = "Select f.Nombre, (select count(*) from versiones where Formato=f.codigo and ticket=dbo.CodigoSimple('" + tic + "')) as versiones, codigo from Formatos f where Tipificacion like '%," + tip + ",%'";
                reader = query.ExecuteReader();
                int ver;
                while (reader.Read())
                {
                    ver = reader.GetInt32(1);
                    DataRow form = dtFormatos.NewRow();
                    form["formato"] = reader.GetString(0) + "&nbsp<span onclick=\"Principal.addtab('Acciones/Formato.aspx?cdg=" + reader.GetInt32(2).ToString() + "&tic=" + tic + "','" + reader.GetString(0) + "').select();\" style=\"position:relative; height:20px; width:20px; vertical-align:middle;\"><img src=\"Images/" + (ver > 0 ? "doc" : "edit") + ".png\" style=\"vertical-align:middle;\"/>" + (ver > 0 ? "<div style=\"position:absolute; background:purple; bottom:-6px; left: 10px; font-size:10px; color:white; font-weight:bold; text-align: center;  line-height: 10px; padding: 1px 2px 1px 2px;\">" + ver.ToString() + "</div>" : "") + "</span>";
                    dtFormatos.Rows.Add(form);
                }
                reader.Close();
            }
            rgrFormatos.DataSource = dtFormatos;
            rgrFormatos.DataBind();
            dao.close(); //dao.close();
            sdsTraza.SelectCommand = "Declare @tic int Set @tic = dbo.CodigoSimple('" + tic + "');" + " SELECT u.Nombre as uo, ISNULL(CONVERT(char(10),t.Fecha,103) + ' ' + " +
               " CONVERT(char(5),t.Fecha,114),'') as fa, Tipo as tipo from Traza t join Usuarios u on t.Usuario=u.Codigo where t.Incidencia=@tic ORDER BY Fecha "; //convert(varchar(20), Fecha, 120) ";
            rgrTraza.DataBind();
            string imFolder = ImagesFolder + "30x30/";
            RadToolBar1.Items[0].ImageUrl=imFolder +  "Editar.png";
            RadToolBar1.Items[1].ImageUrl=imFolder +  "Guardar.png";
            RadToolBar1.Items[3].ImageUrl=imFolder +  "SolVal.png";
            RadToolBar1.Items[4].ImageUrl=imFolder +  "Validar.png";
            RadToolBar1.Items[5].ImageUrl=imFolder +  "Rechazar.png";
            RadToolBar1.Items[7].ImageUrl=imFolder +  "Seguimiento.png";
            RadToolBar1.Items[9].ImageUrl=imFolder +  "NuevoDerivado.png";
            RadToolBar1.Items[11].ImageUrl=imFolder +  "Adjuntar.png";
            RadToolBar1.Items[13].ImageUrl=imFolder +  "Imprimir.png";
            RadToolBar1.Items[15].ImageUrl=imFolder +  "Cerrar.png";
            rtbLimite.Items[1].ImageUrl = imFolder + "Repactar.png";
            rtbTipificacion.Items[2].ImageUrl = imFolder + "Tipificar.png";
            rtbResponsable.Items[2].ImageUrl = imFolder + "Escalar.png";
            rtbResponsable.Items[5].ImageUrl = imFolder + "Editar.png";
        }
    }
}