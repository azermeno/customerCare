using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using CustomerCare;
using System.Linq;
using System.Collections.Generic;


public partial class NuevoTicket : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "NuevoTicket";
            Controles = new string[] { "ToolBar", "ComboBox", "Calendar", "Input", "Splitter" };
        }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Header.Title = "Nuev" + Utilidades.mascfem + " " + Utilidades.ticket;
            lblSolicitante.Text = ConfigurationManager.AppSettings["solicitante"];
            rtbSolucion.Skin = ConfigurationManager.AppSettings["Skin"];
            rtbDetalle.Skin = ConfigurationManager.AppSettings["Skin"];
            rtbAsunto.Skin = ConfigurationManager.AppSettings["Skin"];
            rtbTipificacion.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbResponsable.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbValidador.Skin = ConfigurationManager.AppSettings["Skin"];
            rcbSolicitante.Skin = ConfigurationManager.AppSettings["Skin"];
            //RadAjaxLoadingPanel1.Skin = ConfigurationManager.AppSettings["Skin"];
            RadSplitter1.Skin = ConfigurationManager.AppSettings["Skin"];
            RadSkinManager1.Skin = ConfigurationManager.AppSettings["Skin"];
            //string wy;
            //if (Session["wy"] == null && (Request.QueryString["wy"] == null || Request.QueryString["wy"] == "undefined"))
            //{
            //    Response.Write("<script type=\"text/javascript\">parent.location = \"/index.aspx\";</script>");
            //    return;
            //}
            //if (Session["wy"] == null)
            //    wy = Request.QueryString["wy"];
            //else
            //    wy = Session["wy"].ToString();
            //string usu = Utilidades.Crip(wy);
            //SqlConnection conex = new SqlConnection();
            Utilidades.log("Nombre completo: " + usuario.nombre);
            Utilidades.log("Permisos: " + usuario.permisos);
            IniDate.Style.Add("display", "none");
            LInicio.Style.Add("display", "none");
            IniDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            IniDate.Attributes.Add("min", DateTime.Now.ToString("yyyy-MM-dd"));
            IniDate.DataBind();
            IniTime.Text = DateTime.Now.ToString("HH:mm");
            IniTime.DataBind();
            LimDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            LimDate.Attributes.Add("min", DateTime.Now.ToString("yyyy-MM-dd"));
            LimDate.DataBind();
            LimTime.Text = DateTime.Now.AddHours(4).ToString("HH:mm");
            LimDate.DataBind();
            if (!usuario.tienePermiso(Permiso.RepactarTickets))
            {
                LimDate.Style.Add("display", "none");
                LimTime.Style.Add("display", "none");
                LLimite.Style.Add("display", "none");
            }
            else
            {
                LimDate.Style.Add("display", "block");
                LimTime.Style.Add("display", "block");
            }
            if (!usuario.tienePermiso(Permiso.LevantarAjenas))
            {
                Solicitante.Text = usuario.nombre;
                rcbSolicitante.Style.Add("display", "none");
                Solicitante.Visible = true;
                ValueSol.Text = "4" + usuario.código;
                ValueSol.Style.Add("display", "none");
                Solicitante.Style.Add("display", "inline");
            }
            else
            {
                Solicitante.Visible = false;
                ValueSol.Visible = false;
                rcbSolicitante.Focus();
            }
            if (!usuario.tienePermiso(Permiso.TipificarTickets))
            {
                hflTipificacion.Visible = false;
                //rfvTipificar.Enabled = false;
                //rfvTipificar.EnableClientScript = false;
                lblTipificacion.Visible = false;
                rtbTipificacion.Visible = false;
                divTipificacion.Visible = false;
                rtbTipificacion.Display = false;
                //rfvTipificar.Visible = false;
                hflTipificacion.Value = "0";
            }
            if (!usuario.tienePermiso(Permiso.LevantarCerradas))
            {
                lblResueltaIngresar.Visible = false;
                chkRes.Style.Add("display", "none");
            }
            if (!usuario.tienePermiso(Permiso.Inactivar))
            {
                lblInactiva.Visible = false;
                chkIna.Style.Add("display", "none");
            }
            if (!usuario.tienePermiso(Permiso.Priorizar))
            {
                lblPrioridad.Visible = false;
                rcbPrioridad.Visible = false;
            }
            if (!usuario.tienePermiso(Permiso.PriorizarCritico))
            {
                sdsPrioridad.SelectCommand = "SELECT Codigo, Descripcion FROM Severidades WHERE Codigo>1 ORDER BY Codigo";
                rcbPrioridad.DataBind();
            }
            //----- Responsable
            //List<Clase> receptores = usuario.getReceptores();
            //if (receptores.Count == 1)
            //{
            //    Responsable.Text = receptores[0].nombre + "";
            //    rcbResponsable.Style.Add("display", "none");
            //    Responsable.Visible = true;
            //    ValueResp.Text = receptores[0].código + "";
            //    Responsable.Style.Add("display", "inline");
            //}
            //else
            //{
            //    DataTable source = new DataTable();
            //    source.Columns.Add(new DataColumn("codigo", typeof(string)));
            //    source.Columns.Add(new DataColumn("nombre", typeof(string)));
            //    foreach (Clase clase in receptores)
            //    {
            //        DataRow row = source.NewRow();
            //        row["codigo"] = (clase is Usuario ? "u" : "c") + clase.código;
            //        row["nombre"] = clase.nombre;
            //        source.Rows.Add(row);
            //    }
            //    rcbResponsable.DataSource = source;
            //    rcbResponsable.DataTextField = "nombre";
            //    rcbResponsable.DataValueField = "codigo";
            //    rcbResponsable.DataBind();
            //}
            // ------ abajo lo nuevo
            SqlConnection SqlConex = new SqlConnection();
            SqlCommand SqlComando = new SqlCommand();
            try
            {
                SqlComando.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                SqlComando.CommandTimeout = 300;
            }
            SqlComando.Connection = SqlConex;
            DataSet req = new DataSet();
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            SqlConex.ConnectionString = ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "Select codigo,Nombre, login, activo, isnull(r.unidad,0) As requiriente, perfil, filtrodefault, tipoUsuario, permisos from Usuarios u INNER JOIN Requirientes r on u.codigo=r.usuario where u.activo=1 and u.codigo=" + usuario.código;
            Utilidades.log("Se obtinie datos del usuario");
            Utilidades.log(SqlComando.CommandText);
            SqlConex.Open();
            SqlParellenar.Fill(req);
            SqlConex.Close();
            if (req.Tables[0].Rows.Count > 0)
            {
                DataSet resp = new DataSet();
                SqlComando.CommandText = "SELECT usuarios.codigo, usuarios.nombre FROM Unidades INNER JOIN Sitios ON Unidades.Sitio = Sitios.Codigo INNER JOIN internos ON Sitios.Responsable = Internos.Codigo INNER JOIN Usuarios ON Internos.Usuario=Usuarios.Codigo WHERE usuarios.activo=1 and Unidades.Codigo="+req.Tables[0].Rows[0]["requiriente"].ToString().Trim();
                Utilidades.log(SqlComando.CommandText);
                SqlConex.Open();
                SqlParellenar.Fill(resp);
                SqlConex.Close();
                Responsable.Text = resp.Tables[0].Rows[0]["nombre"].ToString().Trim();
                ValueResp.Text = resp.Tables[0].Rows[0]["codigo"].ToString().Trim();
                Responsable.Visible = true;
                ValueResp.Visible = true;
                ValueResp.Style.Add("display", "none");
                rcbResponsable.Visible = false;
            }
            else
            {
                DataSet resp = new DataSet();
                SqlComando.CommandText = "SELECT Usuarios.codigo,usuarios.nombre FROM usuarios INNER JOIN internos ON usuarios.codigo=Internos.Usuario WHERE usuarios.activo=1";
                Utilidades.log(SqlComando.CommandText);
                SqlConex.Open();
                SqlParellenar.Fill(resp);
                SqlConex.Close();
                rcbResponsable.DataSource = resp;
                rcbResponsable.DataTextField = "nombre";
                rcbResponsable.DataValueField = "codigo";
                rcbResponsable.DataBind();
                Responsable.Visible = false;
                ValueResp.Visible = false;
                ValueResp.Style.Add("display", "none");
                rcbResponsable.Visible = true;
                rcbValidador.DataSource = resp;
                rcbValidador.DataTextField = "nombre";
                rcbValidador.DataValueField = "codigo";
                rcbValidador.DataBind();
            }
            //if (reader.Read())
            //{
            //    result = new Usuario(código);
            //    result.nombre = reader.GetString(0);
            //    result.login = reader.GetString(1);
            //    result.activo = reader.GetBoolean(2);
            //    result.interno = reader.GetInt32(3) > 0;
            //    daoAux.open();
            //    result.perfil = daoAux.getPerfil(reader.GetInt32(4), reader.GetString(7));
            //    daoAux.close();
            //    result.filtroDefault = reader.GetString(5);
            //    result.tipoUsuario = reader.GetInt32(6);
            //    result.permisos = reader.GetString(7);
            //    reader.Close();
            //}
            //else
            //{
            //    reader.Close();
            //    return null;
            //}
            //------ Responsable
            divTipificacion.Attributes.Add("onclick", "showMenu(event, 'rcmTipificacion')");
            rtbTipificacion.Attributes.Add("onkeyup", "showMenuK(event.keyCode, 'rcmTipificacion')");
            divConclusion.Attributes.Add("onclick", "showMenu(event, 'rcmConclusion')");
            rtbConclusion.Attributes.Add("onkeyup", "showMenuK(event.keyCode, 'rcmConclusion')");
            rcbPrioridad.SelectedValue = "3";
            HiddenField1.Value = usuario.código + "";
        }
        else
        {
            dialogo.Style.Add("display", "inline");
            //areaboton.Style.Add("display", "inline");
            //Aceptar.Style.Add("display", "inline");
        }
    }
    protected void rcmTipificacion_ItemDataBound(object sender, RadMenuEventArgs e)
    {
        e.Item.Attributes.Add("onkeyup", "cerrarMenu(event.keyCode, 'rtbTipificacion')");
    }
    protected void rcmConclusion_ItemDataBound(object sender, RadMenuEventArgs e)
    {
        e.Item.Attributes.Add("onkeyup", "cerrarMenu(event.keyCode, 'rtbConclusion')");
    }

    protected void Guardar_Click(object sender, EventArgs e)
    {
        bool estaVacio = false;

        string dad = "";
        string sol = "";
        int res = 0;
        int tip = 0;
        string rin = "0";
        int con = 0;
        string solu = "";
        string ina = "no";
        string val = "no";
        int vdr = 0;
        int pri = 3;
        DateTime lim = new DateTime();
        DateTime ini = new DateTime();
        try
        {
            dad = Request.QueryString["dad"].ToString().Trim();
        }
        catch
        {

        }
        if (Solicitante.Visible)
        {
            sol = ValueSol.Text;
            if (sol.Trim().Equals(""))
                estaVacio = true;
        }
        else
        {
            sol = rcbSolicitante.SelectedValue;
            if (sol.Trim().Equals(""))
                estaVacio = true;
        }
        if (Responsable.Visible)
        {
            if (ValueResp.Text.Trim().Equals(""))
                estaVacio = true;
            else
                res = Convert.ToInt32(ValueResp.Text);
        }
        else
        {
            if (rcbResponsable.SelectedValue.Trim().Equals(""))
                estaVacio = true;
            else
                res = Convert.ToInt32(rcbResponsable.SelectedValue);
        }
        if (rtbTipificacion.Visible)
        {
            if (hflTipificacion.Value.Trim() != "")
            {
                tip = Convert.ToInt32(hflTipificacion.Value.Trim());
            }
            else
                estaVacio = true;
        }
        if (chkRes.Checked)
        {
            if (hflConclusion.Value.Trim().Equals(""))
                estaVacio = true;
            else
            {
                rin = "1";
                con = Convert.ToInt32(hflConclusion.Value);
                solu = rtbSolucion.Text.Trim();
            }
        }
        if (LimDate.Visible)
        {
            if (chkIna.Checked)
            {
                try
                {
                    if (Convert.ToDateTime(LimDate.Text + " " + LimTime.Text) >= Convert.ToDateTime(IniDate.Text + " " + IniTime.Text))
                    {
                        lim = Convert.ToDateTime(LimDate.Text + " " + LimTime.Text);
                    }
                    else
                    {
                        lim = Convert.ToDateTime(IniDate.Text + " " + IniTime.Text);
                        lim = lim.AddHours(4);
                    }
                }
                catch
                {
                    lim = DateTime.Now.AddHours(4);
                }
            }
            else
            {
                try
                {
                    if (Convert.ToDateTime(LimDate.Text + " " + LimTime.Text) >= DateTime.Now)
                    {
                        lim = Convert.ToDateTime(LimDate.Text + " " + LimTime.Text);
                    }
                    else
                    {
                        lim = DateTime.Now.AddHours(4);
                    }
                }
                catch
                {
                    lim = DateTime.Now.AddHours(4);
                }
            }
        }
        else
        {
            lim = DateTime.Now.AddHours(4);
        }
        if (chkIna.Checked)
        {
            try
            {
                ina = "si";
                ini = Convert.ToDateTime(IniDate.Text + " " + IniTime.Text);
            }
            catch
            {
                ina = "no";
                ini = DateTime.Now;
            }
        }
        if (rcbPrioridad.Visible)
        {
            pri = Convert.ToInt32(rcbPrioridad.SelectedValue.Trim());
        }
        if (chkVal.Checked)
        {
            try
            {
                val = "si";
                vdr = Convert.ToInt32(rcbValidador.SelectedValue);
            }
            catch
            {
                val = "no";
            }
        }

        if (!estaVacio)
        {
            Servicio ws = new Servicio();
            if (dad == "")
            {
                msg.Text = ws.NuevoTicket(rtbAsunto.Text.Trim(), rtbDetalle.Text.Trim(), usuario.código, res, sol, tip, rin, con, solu, lim, ina, ini, pri, val, vdr);
            }
            else
            {
                msg.Text = ws.NuevoTicketDerivado(rtbAsunto.Text.Trim(), rtbDetalle.Text.Trim(), usuario.código, res, sol, tip, rin, con, solu, dad, lim, ina, ini, pri);
            }
        }
        else
            msg.Text = "Existen campos vacios, intente de nuevo.";
    }
        
}
