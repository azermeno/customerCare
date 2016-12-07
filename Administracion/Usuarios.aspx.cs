using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;

namespace CustomerCare.Administracion
{
    public partial class Usuarios : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "Usuarios";
            Controles = new string[] { "Grid", "ToolBar", "ComboBox", "Input" };
        }

        public string separa(string cadena, string separador, Int32 indice)
        {
            string respuesta = "";
            Int32 tam = separador.Length;
            Int32 conta = 0;
            if (cadena.Length > 0)
            {
                if (cadena.Substring(0, tam) == separador)
                {
                    cadena = cadena.Substring(1, cadena.Length - 1);
                }
                if (cadena.Substring(cadena.Length - tam, tam) != separador)
                {
                    cadena += separador;
                }
                Int32 inici = 0;
                for (int aa = 0; aa < cadena.Length; aa++)
                {
                    if (cadena.Substring(aa, separador.Length) == separador)
                    {
                        conta += 1;
                        if (conta == indice)
                        {
                            respuesta = cadena.Substring(inici, aa - inici);
                        }
                        inici = aa + tam;
                    }
                }
            }
            return respuesta;
        }

        public Int32 SeparaCount(string cadena, string separador)
        {
            Int32 respuesta = 0;
            Int32 tam = separador.Length;
            if (cadena.Length > 0)
            {
                if (cadena.Substring(0, tam) == separador)
                {
                    cadena = cadena.Substring(1, cadena.Length - 1);
                }
                if (cadena.Substring(cadena.Length - tam, tam) != separador)
                {
                    cadena += separador;
                }
                for (int aa = 0; aa < cadena.Length; aa++)
                {
                    if (separador == cadena.Substring(aa, separador.Length))
                    {
                        respuesta += 1;
                    }
                }
            }
            return respuesta;
        }

        public string Encrip(string cad)
        {
            int pas;
            pas = 0;
            foreach (char c in cad)
                pas = pas + (int)c;
            pas = 134 * pas;
            return (pas.ToString());
        }

        public string Crip(string wy)
        {
            string texto = "";
            int c0;
            foreach (char c in wy)
            {
                if (c > 47 && c < 58)
                {
                    c0 = c - 48;
                    texto = texto + (char)(48 + ((c0 + 5) % 10));
                }
                else if (c > 64 && c < 91)
                {
                    c0 = c - 65;
                    texto = texto + (char)(65 + ((c0 + 13) % 26));
                }
                else if (c > 96 && c < 123)
                {
                    c0 = c - 97;
                    texto = texto + (char)(97 + ((c0 + 13) % 26));
                }
                else
                    texto = texto + (char)(c);
            }
            return texto;
        }

        private void llenaPerfil()
        {
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
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            DataSet perf = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "SELECT Codigo,Nombre,Permisos FROM perfiles WHERE Activo=1 and codigo=" + Perfiles.SelectedValue;
            SqlConex.Open();
            SqlParellenar.Fill(perf);
            SqlConex.Close();
            string perm = "";
            if (perf.Tables[0].Rows.Count > 0)
            {
                perm = "," + perf.Tables[0].Rows[0]["Permisos"].ToString().Trim()+",";
            }
            for (int i = 0; i < Permisos.Items.Count; i++)
            {
                Permisos.Items[i].Selected = false;
                if (perm.Contains("," + Permisos.Items[i].Value + ","))
                {
                    Permisos.Items[i].Selected = true;
                    Permisos.Items[i].Enabled = false;
                }
                else
                {
                    Permisos.Items[i].Enabled = true;
                }
            }
        }

        private void getUsuarios(bool tInternos, bool tClientes, bool tSolicitante, bool tSClasificar, bool tSActivos)
        {
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
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            DataSet catusu=new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            int cont = 0;
            string sentencia = "";
            sentencia = "SELECT Codigo,TipoUsuario,Nombre,Activo,DCC,Login,Clave,Celular,Correo,Unico,PuedeVer,Permisos,Pruebas,SMS,FiltroDefault,PuedeVerTip,PuedeVerUsu,PuedeVerCon,PuedeVerSol,Perfil FROM Usuarios ";
            if ((!tInternos) && (!tClientes) && (!tSolicitante) && (!tSClasificar))
            {
                sentencia += " WHERE (1=0";
            }
            else
            {
                sentencia += " WHERE (";
            }
            if (tInternos)
            {
                if (cont > 0)
                {
                    sentencia += " or ";
                }
                sentencia += " codigo in (SELECT usuario FROM internos)";
                cont += 1;
            }
            if (tClientes)
            {
                if (cont > 0)
                {
                    sentencia += " or ";
                }
                sentencia += " codigo in (SELECT usuario FROM clientes)";
                cont += 1;
            }
            if (tSolicitante)
            {
                if (cont > 0)
                {
                    sentencia += " or ";
                }
                sentencia += " codigo in (SELECT usuario FROM requirientes)";
                cont += 1;
            }
            if (tSClasificar)
            {
                if (cont > 0)
                {
                    sentencia += " or ";
                }
                sentencia += " ( NOT codigo in (SELECT usuario FROM internos) and NOT codigo in (SELECT usuario FROM clientes) and  NOT codigo in (SELECT usuario FROM requirientes))";
                cont += 1;
            }
            sentencia += ")";
            if (tSActivos)
            {
                if (cont > 0)
                {
                    sentencia += " and ";
                }
                sentencia += " activo=1";
                cont += 1;
            }
            sentencia += " ORDER By Nombre";
            SqlComando.CommandText = sentencia;
            SqlConex.Open();
            SqlParellenar.Fill(catusu);
            SqlConex.Close();
            Lista.DataSource = catusu;
            Lista.DataTextField = "Nombre";
            Lista.DataValueField = "codigo";
            Lista.DataBind();
        }

        private void getUsuario(string tcodigo)
        {
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
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            DataSet usu = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "SELECT Codigo,TipoUsuario,Nombre,Activo,DCC,Login,Clave,Celular,Correo,Unico,PuedeVer,Permisos,Pruebas,SMS,FiltroDefault,PuedeVerTip,PuedeVerUsu,PuedeVerCon,PuedeVerSol,Perfil FROM Usuarios WHERE codigo=" + tcodigo;
            SqlConex.Open();
            SqlParellenar.Fill(usu);
            SqlConex.Close();
            if (usu.Tables[0].Rows.Count > 0)
            {
                Codigo.Text = usu.Tables[0].Rows[0]["Codigo"].ToString().Trim();
                Nombre_Usuario.Text = usu.Tables[0].Rows[0]["Nombre"].ToString().Trim();
                Login.Text = usu.Tables[0].Rows[0]["Login"].ToString().Trim();
                Activo.Checked = Convert.ToBoolean(usu.Tables[0].Rows[0]["Activo"]);
                DCC.Checked = Convert.ToBoolean(usu.Tables[0].Rows[0]["DCC"]);
                SMS.Checked = Convert.ToBoolean(usu.Tables[0].Rows[0]["SMS"]);
                Pruebas.Checked = Convert.ToBoolean(usu.Tables[0].Rows[0]["Pruebas"]);
                Celular.Text = usu.Tables[0].Rows[0]["Celular"].ToString().Trim();
                Correo.Text = usu.Tables[0].Rows[0]["Correo"].ToString().Trim();
                getPerfiles();
                Perfiles.Text = usu.Tables[0].Rows[0]["Perfil"].ToString().Trim();
                Perfil.DataBind();
                if (usu.Tables[0].Rows[0]["PuedeVer"].ToString().Trim()=="1=1")
                {
                    PVer.SelectedIndex=1;
                }
                else
                {
                    PVer.SelectedIndex=0;
                }
                if (usu.Tables[0].Rows[0]["FiltroDefault"].ToString().Trim() == "!!!!")
                {
                    Default.SelectedIndex = 0;
                }
                else
                {
                    if (usu.Tables[0].Rows[0]["FiltroDefault"].ToString().Trim().Contains("!!2!!"))
                    {
                        Default.SelectedIndex = 1;
                    }
                    else
                    {
                        if (usu.Tables[0].Rows[0]["FiltroDefault"].ToString().Trim().Contains("!!6!!"))
                        {
                            Default.SelectedIndex = 2;
                        }
                        else
                        {
                            if (usu.Tables[0].Rows[0]["FiltroDefault"].ToString().Trim().Contains("!!4!!"))
                            {
                                Default.SelectedIndex = 3;
                            }
                        }
                    }
                }
                if (usu.Tables[0].Rows[0]["FiltroDefault"].ToString().Trim().Substring(0, 2) == "!!")
                {
                    Resp.Checked = false;
                }
                else
                {
                    Resp.Checked = true;
                }
                //Default.Text = usu.Tables[0].Rows[0]["FiltroDefault"].ToString().Trim();
                DataSet intern = new DataSet();
                SqlComando.CommandText = "SELECT codigo,usuario FROM internos WHERE Usuario=" + tcodigo;
                SqlConex.Open();
                SqlParellenar.Fill(intern);
                SqlConex.Close();
                if (intern.Tables[0].Rows.Count > 0)
                {
                    Interno.Checked = true;
                }
                else
                {
                    Interno.Checked = false;
                }
                DataSet client = new DataSet();
                SqlComando.CommandText = "SELECT codigo,usuario FROM clientes WHERE Usuario=" + tcodigo;
                SqlConex.Open();
                SqlParellenar.Fill(client);
                SqlConex.Close();
                if (client.Tables[0].Rows.Count > 0)
                {
                    Cliente.Checked = true;
                }
                else
                {
                    Cliente.Checked = false;
                }
                DataSet req = new DataSet();
                SqlComando.CommandText = "SELECT unidad,usuario FROM requirientes WHERE Usuario=" + tcodigo;
                SqlConex.Open();
                SqlParellenar.Fill(req);
                SqlConex.Close();
                if (req.Tables[0].Rows.Count > 0)
                {
                    Requiriente.Checked = true;
                    Unidades.SelectedValue = req.Tables[0].Rows[0]["unidad"].ToString().Trim();
                    Unidades.Style.Add("display", "block");
                    etqUnidad.Style.Add("display", "block");
                }
                else
                {
                    Requiriente.Checked = false;
                    Unidades.Style.Add("display", "none");
                    etqUnidad.Style.Add("display", "none");
                }
                for (int y = 0; y < Permisos.Items.Count; y++)
                {
                    Permisos.Items[y].Selected = false;
                }
                for (int x = 1; x <= SeparaCount(usu.Tables[0].Rows[0]["permisos"].ToString().Trim(), ","); x++)
                {
                    for (int y = 0; y < Permisos.Items.Count; y++)
                    {
                        if (Permisos.Items[y].Value == separa(usu.Tables[0].Rows[0]["permisos"].ToString().Trim(), ",", x))
                        {
                            Permisos.Items[y].Selected = true;
                        }
                    }
                }
                Tipo.SelectedValue = usu.Tables[0].Rows[0]["TipoUsuario"].ToString().Trim();
                llenaPerfil();
            }
        }

        private void getUnidades()
        {
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
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            DataSet unid = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "SELECT Codigo,Nombre FROM Unidades WHERE activa=1 ORDER By nombre";
            SqlConex.Open();
            SqlParellenar.Fill(unid);
            SqlConex.Close();
            Unidades.DataSource = unid;
            Unidades.DataTextField = "Nombre";
            Unidades.DataValueField = "codigo";
            Unidades.DataBind();
        }

        private void getPermisos()
        {
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
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            DataSet perm = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "SELECT Codigo,Descripcion FROM permisos ORDER By codigo";
            SqlConex.Open();
            SqlParellenar.Fill(perm);
            SqlConex.Close();
            Permisos.DataSource = perm;
            Permisos.DataTextField = "Descripcion";
            Permisos.DataValueField = "codigo";
            Permisos.DataBind();
        }

        private void getPerfiles()
        {
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
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            DataSet perf = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "SELECT Codigo,Nombre,Permisos FROM perfiles WHERE Activo=1 ORDER By Nombre";
            SqlConex.Open();
            SqlParellenar.Fill(perf);
            SqlConex.Close();
            DataRow tPerfil = perf.Tables[0].NewRow();
            tPerfil[0] = "0";
            tPerfil[1] = "------";
            perf.Tables[0].Rows.InsertAt(tPerfil, 0);
            Perfiles.DataSource = perf;
            Perfiles.DataTextField = "Nombre";
            Perfiles.DataValueField = "Codigo";
            Perfiles.DataBind();
        }

        private void getTiposUsuario()
        {
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
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            DataSet tu = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "SELECT Codigo,Descripcion FROM TiposUsuario ORDER By Descripcion";
            SqlConex.Open();
            SqlParellenar.Fill(tu);
            SqlConex.Close();
            Tipo.DataSource = tu;
            Tipo.DataTextField = "Descripcion";
            Tipo.DataValueField = "Codigo";
            Tipo.DataBind();
        }

        private void modoEdicion(bool editando)
        {
            Nombre_Usuario.ReadOnly = !editando;
            Login.ReadOnly = !editando;
            Activo.Enabled = editando;
            DCC.Enabled = editando;
            SMS.Enabled = editando;
            Pruebas.Enabled = editando;
            Celular.ReadOnly = !editando;
            Correo.ReadOnly = !editando;
            PVer.Enabled = editando;
            Resp.Enabled = editando;
            Cliente.Enabled = editando;
            Interno.Enabled = editando;
            Requiriente.Enabled = editando;
            Unidades.Enabled = editando;
            Permisos.Enabled = editando;
            Nuevo.Enabled = !editando;
            Modificar.Enabled = !editando;
            Eliminar.Enabled = !editando;
            Lista.Enabled = !editando;
            if (editando)
            {
                Lista.Attributes.Add("disabled", "diasbled");
                Filtrar.Style.Add("display", "none");
                Guardar.Style.Add("display", "inline");
                Cancelar.Style.Add("display", "inline");
                Perfil.Style.Add("display", "none");
                //Perfiles.Style.Add("display", "inline");
            }
            else
            {
                Lista.Attributes.Remove("disabled");
                Filtrar.Style.Add("display", "inline");
                Resetear.Style.Add("display", "none");
                Guardar.Style.Add("display", "none");
                Cancelar.Style.Add("display", "none");
                Resetear.Enabled = editando;
                Perfil.Style.Add("display", "none");
                tCodigo.Style.Add("display", "block");
                Codigo.Style.Add("display", "block");
                //Perfiles.Style.Add("display", "none");
            }
            Internos.Enabled = !editando;
            Clientes.Enabled = !editando;
            Solicitantes.Enabled = !editando;
            sClasificar.Enabled = !editando;
            sActivos.Enabled = !editando;
            Filtrar.Enabled = !editando;
            Guardar.Enabled = editando;
            Cancelar.Enabled = editando;
            Perfil.Visible = false;
            Perfiles.Enabled = editando;
            Tipo.Enabled = editando;
            Default.Enabled = editando;
            Buscar.ReadOnly = editando;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (usuario.tienePermiso(Permiso.VerColumnasUsuariosDesarrollo))
            {
                //rgrUsuarios.Columns.FindByUniqueName("Pruebas").Visible = true;
                //rgrUsuarios.Columns.FindByUniqueName("PuedeVer").Visible = true;
            }
            alerta_fondo.Style.Add("display", "none");
            alerta_mns.Style.Add("display", "none");
            if (!IsPostBack)
            {
                modoEdicion(false);
                getUnidades();
                getPermisos();
                getPerfiles();
                getTiposUsuario();
                getUsuarios(Internos.Checked, Clientes.Checked, Solicitantes.Checked, sClasificar.Checked, sActivos.Checked);
                if (Lista.Items.Count > 0)
                {
                    Lista.SelectedIndex = 0;
                    getUsuario(Lista.SelectedValue);
                }
            }
        }
        protected void rgrUsuarios_ItemUpdated(object source, Telerik.Web.UI.GridUpdatedEventArgs e)
        {
            if (e.Exception != null)
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
            }
        }

        protected void rgrUsuarios_ItemInserted(object source, Telerik.Web.UI.GridInsertedEventArgs e)
        {
            if (e.Exception != null)
            {

                e.ExceptionHandled = true;

            }
        }

        protected void rgrUsuarios_PreRender(object sender, EventArgs e)
        {
        }

        protected void Resetear_Click(object sender, EventArgs e)
        {
            Random r = new Random(DateTime.Now.Millisecond);
            string psw = r.Next(1000, 9999).ToString();
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
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "UPDATE usuarios SET DCC=1, Clave="+Encrip(psw)+" WHERE codigo="+Lista.SelectedValue;
            SqlConex.Open();
            SqlComando.ExecuteNonQuery();
            SqlConex.Close();
            DCC.Checked = true;
            mns.Text = "La nueva contraseña temporal es: " + psw;
            alerta_fondo.Style.Add("display", "block");
            alerta_mns.Style.Add("display", "block");
        }

        protected void Modificar_Click(object sender, ImageClickEventArgs e)
        {
            Operacion.Value = "MODIFICAR";
            modoEdicion(true);
            Resetear.Enabled = true;
            Resetear.Style.Add("display", "inline");
        }

        protected void Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            Nombre_Usuario.Text = "";
            Login.Text = "";
            Activo.Checked = true;
            DCC.Checked = true;
            SMS.Checked = false;
            Pruebas.Checked = false;
            Celular.Text = "";
            Correo.Text = "";
            PVer.SelectedIndex = 0;
            //Default.Text = "";
            Cliente.Checked = false;
            Interno.Checked = false;
            Requiriente.Checked = false;
            Unidades.Style.Add("display", "none");
            for (int x = 0; x < Permisos.Items.Count; x++)
            {
                Permisos.Items[x].Selected = false;
                Permisos.Items[x].Enabled = true;
            }
            Perfiles.SelectedIndex = 0;
            Operacion.Value = "NUEVO";
            Resp.Checked = false;
            modoEdicion(true);
            tCodigo.Style.Add("display", "none");
            Codigo.Style.Add("display", "none");
        }

        protected void Lista_SelectedIndexChanged(object sender, EventArgs e)
        {
            getUsuario(Lista.SelectedValue);
            Buscar.Text = "";
            Lista.DataBind();
        }
                
        protected void Filtrar_Click(object sender, EventArgs e)
        {
            getUsuarios(Internos.Checked, Clientes.Checked, Solicitantes.Checked, sClasificar.Checked, sActivos.Checked);
            if (Lista.Items.Count > 0)
            {
                Lista.SelectedIndex = 0;
                getUsuario(Lista.SelectedValue);
            }
        }

        protected void Perfil_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < Permisos.Items.Count; y++)
            {
                Permisos.Items[y].Selected = false;
            }
            for (int x = 1; x <= SeparaCount(Perfiles.SelectedValue, ","); x++)
            {
                for (int y = 0; y < Permisos.Items.Count; y++)
                {
                    if (Permisos.Items[y].Value == separa(Perfiles.SelectedValue, ",", x))
                    {
                        Permisos.Items[y].Selected = true;
                    }
                }
            }
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            modoEdicion(false);
            getUsuario(Lista.SelectedValue);
        }

        protected void Guardar_Click(object sender, EventArgs e)
        {
            bool libre = true;
            bool ologin = false, onombre = false;
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
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            DataSet usu = new DataSet();
            string sentencia="";
            if (Login.Text.Trim() != "")
            {
                sentencia = "SELECT Codigo,Nombre FROM Usuarios WHERE activo=1 and login='" + Login.Text.Trim() + "'";
                if (Operacion.Value!="NUEVO")
                {
                    sentencia+=" and not codigo = "+Lista.SelectedValue;
                }
                SqlComando.Connection = SqlConex;
                SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
                SqlComando.CommandText = sentencia;
                Utilidades.log(SqlComando.CommandText);
                SqlConex.Open();
                SqlParellenar.Fill(usu);
                SqlConex.Close();
                if (usu.Tables[0].Rows.Count > 0)
                {
                    libre = false;
                    ologin = true;
                }
            }
            sentencia = "SELECT Codigo,Nombre FROM Usuarios WHERE nombre ='" + Nombre_Usuario.Text.Trim() + "'";
            if (Operacion.Value!="NUEVO")
            {
                sentencia+=" and not codigo = "+Lista.SelectedValue;
            }
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = sentencia;
            Utilidades.log(SqlComando.CommandText);
            SqlConex.Open();
            SqlParellenar.Fill(usu);
            SqlConex.Close();
            if (usu.Tables[0].Rows.Count > 0)
            {
                libre = false;
                onombre = true;
            }
            if (libre)
            {
                string act;
                string dc;
                string prb;
                string sm;
                string dft;
                string vr;
                if (PVer.SelectedIndex == 0)
                {
                    vr = "i.Levanto=#";
                }
                else
                {
                    vr = "1=1";
                }
                if (Activo.Checked)
                {
                    act = "1";
                }
                else
                {
                    act = "0";
                }
                if (DCC.Checked)
                {
                    dc = "1";
                }
                else
                {
                    dc = "0";
                }
                if (Pruebas.Checked)
                {
                    prb = "1";
                }
                else
                {
                    prb = "0";
                }
                if (SMS.Checked)
                {
                    sm = "1";
                }
                else
                {
                    sm = "0";
                }
                if (Default.SelectedIndex == 0)
                {
                    dft = "!!!!";
                }
                else
                {

                    if (Default.SelectedIndex == 1)
                    {
                        dft = "!!2!!";
                    }
                    else
                    {
                        if (Default.SelectedIndex == 2)
                        {
                            dft = "!!6!!";
                        }
                        else
                        {
                            dft = "!!4!!";
                        }
                    }
                }
                if (Resp.Checked)
                {
                    dft = "#.1" + dft;
                }
                string perm = "";
                for (int i = 0; i < Permisos.Items.Count; i++)
                {
                    if (Permisos.Items[i].Selected)
                    {
                        if (perm != "")
                        {
                            perm += ",";
                        }
                        perm += Permisos.Items[i].Value;
                    }
                }
                string cg = "";
                if (Operacion.Value == "NUEVO")
                {
                    Random r = new Random(DateTime.Now.Millisecond);
                    string psw = r.Next(1000, 9999).ToString();
                    SqlComando.CommandText = "INSERT INTO Usuarios (TipoUsuario,Nombre,Activo,DCC,Login,Clave,Celular,Correo,PuedeVer,Pruebas,FiltroDefault,sms,Perfil,permisos,PuedeVerTip,PuedeVerUsu,PuedeVerCon,PuedeVerSol) VALUES (" + Tipo.SelectedValue + ",'" + Nombre_Usuario.Text.Trim() + "'," + act + ","+dc+",'" + Login.Text.Trim() + "','" + Encrip(psw) + "','" + Celular.Text.Trim() + "','" + Correo.Text.Trim() + "',''," + prb + ",''," + sm + ","+Perfiles.SelectedValue+",'" + perm + "','1=0','1=1','1=1','1=1')";
                    Utilidades.log(SqlComando.CommandText);
                    SqlConex.Open();
                    SqlComando.ExecuteNonQuery();
                    SqlConex.Close();
                    sentencia = "SELECT Codigo,Nombre FROM Usuarios WHERE nombre= '" + Nombre_Usuario.Text.Trim() + "'";
                    SqlComando.Connection = SqlConex;
                    SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
                    SqlComando.CommandText = sentencia;
                    Utilidades.log(SqlComando.CommandText);
                    SqlConex.Open();
                    SqlParellenar.Fill(usu);
                    SqlConex.Close();
                    if (usu.Tables[0].Rows.Count > 0)
                    {
                        sentencia = "UPDATE usuarios SET PuedeVer='" + vr.Trim().Replace("#", usu.Tables[0].Rows[0]["Codigo"].ToString().Trim()) + "', FiltroDefault='" + dft.Trim().Replace("#", usu.Tables[0].Rows[0]["Codigo"].ToString().Trim()) + "' WHERE codigo=" + usu.Tables[0].Rows[0]["Codigo"].ToString().Trim();
                        SqlComando.CommandText = sentencia;
                        Utilidades.log(SqlComando.CommandText);
                        SqlConex.Open();
                        SqlComando.ExecuteNonQuery();
                        SqlConex.Close();
                    }
                    mns.Text = "La contraseña temporal es: " + psw;
                    alerta_fondo.Style.Add("display", "block");
                    alerta_mns.Style.Add("display", "block");
                    cg = usu.Tables[0].Rows[0]["Codigo"].ToString().Trim();
                }
                else
                {
                    sentencia = "UPDATE usuarios SET TipoUsuario=" + Tipo.SelectedValue + ", Nombre='" + Nombre_Usuario.Text.Trim() + "', Activo=" + act + ", DCC=" + dc + ", Login='" + Login.Text.Trim() + "', Celular='" + Celular.Text.Trim() + "', Correo='" + Correo.Text + "', PuedeVer='" + vr.Trim().Replace("#", Lista.SelectedValue.ToString().Trim()) + "', Pruebas=" + prb + ", FiltroDefault='" + dft.Trim().Replace("#", Lista.SelectedValue.ToString().Trim()) + "', sms=" + sm + ", perfil=" + Perfiles.SelectedValue + ", permisos='" + perm + "', PuedeVerTip='1=0', PuedeVerUsu='1=1', PuedeVerCon='1=1', PuedeVerSol='1=1' WHERE codigo=" + Lista.SelectedValue;
                    SqlComando.CommandText = sentencia;
                    Utilidades.log(SqlComando.CommandText);
                    SqlConex.Open();
                    SqlComando.ExecuteNonQuery();
                    SqlConex.Close();
                    if (!Internos.Checked)
                    {
                        try
                        {
                            SqlComando.CommandText = " DELETE FROM Internos WHERE usuario=" + Lista.SelectedValue;
                            Utilidades.log(SqlComando.CommandText);
                            SqlConex.Open();
                            SqlComando.ExecuteNonQuery();
                        }
                        finally
                        {
                            SqlConex.Close();
                        }
                    }
                    if (!Clientes.Checked)
                    {
                        try
                        {
                            SqlComando.CommandText = " DELETE FROM Clientes WHERE usuario=" + Lista.SelectedValue;
                            Utilidades.log(SqlComando.CommandText);
                            SqlConex.Open();
                            SqlComando.ExecuteNonQuery();
                        }
                        finally
                        {
                            SqlConex.Close();
                        }
                    }
                    SqlComando.CommandText = " DELETE FROM Requirientes WHERE usuario=" + Lista.SelectedValue;
                    Utilidades.log(SqlComando.CommandText);
                    SqlConex.Open();
                    SqlComando.ExecuteNonQuery();
                    SqlConex.Close();
                    cg = Lista.SelectedValue;
                }
                if (cg != "")
                {
                    if (Interno.Checked)
                    {
                        DataSet intern = new DataSet();
                        SqlComando.CommandText = "SELECT codigo,usuario FROM Internos WHERE usuario=" + cg;
                        Utilidades.log(SqlComando.CommandText);
                        SqlConex.Open();
                        SqlParellenar.Fill(intern);
                        SqlConex.Close();
                        if (intern.Tables[0].Rows.Count < 1)
                        {
                            SqlComando.CommandText = "INSERT INTO Internos (Usuario) VALUES (" + cg + ")";
                            Utilidades.log(SqlComando.CommandText);
                            SqlConex.Open();
                            SqlComando.ExecuteNonQuery();
                            SqlConex.Close();
                        }
                        if (System.Configuration.ConfigurationManager.AppSettings["CiberEncuesta"].ToString().Trim().ToUpper() == "TRUE")
                        {
                            SqlComando.CommandText = "SELECT codigo,usuario FROM Internos WHERE usuario=" + cg;
                            Utilidades.log(SqlComando.CommandText);
                            SqlConex.Open();
                            SqlParellenar.Fill(intern);
                            SqlConex.Close();
                            if (intern.Tables[0].Rows.Count > 0)
                            {
                                SqlConnection SqlConexEnc = new SqlConnection();
                                SqlCommand SqlComandoEnc = new SqlCommand();
                                try
                                {
                                    SqlComandoEnc.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                                }
                                catch
                                {
                                    SqlComandoEnc.CommandTimeout = 300;
                                }
                                SqlDataAdapter SqlParellenarEnc = new SqlDataAdapter(SqlComandoEnc);
                                DataSet usuEnc = new DataSet();
                                SqlComandoEnc.Connection = SqlConexEnc;
                                SqlConexEnc.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CiberEncuesta"].ConnectionString;
                                SqlComandoEnc.CommandText = "SELECT usu_codigo,txtCorto FROM usuarios  WHERE usu_codigo=" + cg;
                                Utilidades.log(SqlComandoEnc.CommandText);
                                SqlConexEnc.Open();
                                SqlParellenarEnc.Fill(usuEnc);
                                SqlConexEnc.Close();
                                if (usuEnc.Tables[0].Rows.Count > 0)
                                {
                                    SqlComandoEnc.CommandText = "UPDATE usuarios SET txtFiltroZona='%', txtFiltroCliente='%', txtFiltroProducto='%', debecambiarpwd=0, idtipousuario=" + Tipo.SelectedValue + ", txtfiltrotecnico='" + cg + "', txtcorto='" + Nombre_Usuario.Text.Trim() + "' WHERE usu_codigo=" + cg;
                                }
                                else
                                {
                                    SqlComandoEnc.CommandText = "INSERT INTO usuarios (usu_codigo,txtfiltrozona,txtfiltrocliente,txtfiltroproducto,debecambiarpwd,idtipousuario,txtFiltrotecnico,txtcorto)VALUES("
                                                              + cg + ",'%','%','%',0," + Tipo.SelectedValue + ",'" + cg + "','" + Nombre_Usuario.Text.Trim() + "')";
                                }
                                Utilidades.log(SqlComandoEnc.CommandText);
                                SqlConexEnc.Open();
                                SqlComandoEnc.ExecuteNonQuery();
                                SqlConexEnc.Close();
                            }
                        }
                    }
                    if (Cliente.Checked)
                    {
                        DataSet clien = new DataSet();
                        SqlComando.CommandText = "SELECT codigo,usuario FROM clientes WHERE usuario=" + cg;
                        Utilidades.log(SqlComando.CommandText);
                        SqlConex.Open();
                        SqlParellenar.Fill(clien);
                        SqlConex.Close();
                        if (clien.Tables[0].Rows.Count < 1)
                        {
                            SqlComando.CommandText = "INSERT INTO Clientes (Usuario) VALUES (" + cg + ")";
                            Utilidades.log(SqlComando.CommandText);
                            SqlConex.Open();
                            SqlComando.ExecuteNonQuery();
                            SqlConex.Close();
                        }
                        if (System.Configuration.ConfigurationManager.AppSettings["CiberEncuesta"].ToString().Trim().ToUpper() == "TRUE")
                        {
                            SqlComando.CommandText = "SELECT codigo,usuario FROM clientes WHERE usuario=" + cg;
                            Utilidades.log(SqlComando.CommandText);
                            SqlConex.Open();
                            SqlParellenar.Fill(clien);
                            SqlConex.Close();
                            if (clien.Tables[0].Rows.Count > 0)
                            {
                                SqlConnection SqlConexEnc = new SqlConnection();
                                SqlCommand SqlComandoEnc = new SqlCommand();
                                try
                                {
                                    SqlComandoEnc.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                                }
                                catch
                                {
                                    SqlComandoEnc.CommandTimeout = 300;
                                }
                                SqlDataAdapter SqlParellenarEnc = new SqlDataAdapter(SqlComandoEnc);
                                DataSet usuEnc = new DataSet();
                                SqlComandoEnc.Connection = SqlConexEnc;
                                SqlConexEnc.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CiberEncuesta"].ConnectionString;
                                SqlComandoEnc.CommandText = "SELECT cli_codigo,txtFiltro,txtNombre FROM clientes  WHERE cli_codigo=" + cg;
                                Utilidades.log(SqlComandoEnc.CommandText);
                                SqlConexEnc.Open();
                                SqlParellenarEnc.Fill(usuEnc);
                                SqlConexEnc.Close();
                                if (usuEnc.Tables[0].Rows.Count > 0)
                                {
                                    SqlComandoEnc.CommandText = "UPDATE clientes SET txtFiltro='"+Nombre_Usuario.Text.Trim()+"', txtNombre='"+Nombre_Usuario.Text.Trim()+"' WHERE cli_codigo=" + cg;
                                }
                                else
                                {
                                    SqlComandoEnc.CommandText = "INSERT INTO clientes (cli_codigo,txtfiltro,txtNombre)VALUES("
                                                              + cg + ",'" + Nombre_Usuario.Text.Trim() + "','" + Nombre_Usuario.Text.Trim() + "')";
                                }
                                Utilidades.log(SqlComandoEnc.CommandText);
                                SqlConexEnc.Open();
                                SqlComandoEnc.ExecuteNonQuery();
                                SqlConexEnc.Close();
                            }
                        }
                    }
                    if (Requiriente.Checked)
                    {
                        SqlComando.CommandText = "INSERT INTO Requirientes (Usuario,unidad) VALUES (" + cg + "," + Unidades.SelectedValue + ")";
                        Utilidades.log(SqlComando.CommandText);
                        SqlConex.Open();
                        SqlComando.ExecuteNonQuery();
                        SqlConex.Close();
                    }
                    modoEdicion(false);
                    getUsuarios(Internos.Checked, Clientes.Checked, Solicitantes.Checked, sClasificar.Checked, sActivos.Checked);
                    Lista.SelectedValue = cg;
                    getUsuario(Lista.SelectedValue);
                }
            }
            else
            {
                if ((ologin) && (onombre))
                {
                    mns.Text = "Ya existe un registro con el mismo nombre de usuario y login";
                }
                else
                {
                    if (onombre)
                    {
                        mns.Text = "Ya existe un registro con el mismo nombre de usuario";
                    }
                    else
                    {
                        mns.Text = "Ya existe un registro con el mismo login";
                    }
                }
                alerta_fondo.Style.Add("display", "block");
                alerta_mns.Style.Add("display", "block");
            }
        }

        protected void Si_Click(object sender, EventArgs e)
        {
            //SqlConnection SqlConex = new SqlConnection();
            //SqlCommand SqlComando = new SqlCommand();
            //try
            //{
            //    SqlComando.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            //}
            //catch
            //{
            //    SqlComando.CommandTimeout = 300;
            //}
            //SqlComando.CommandText = " DELETE FROM Internos WHERE usuario=" + Lista.SelectedValue;
            //SqlConex.Open();
            //SqlComando.ExecuteNonQuery();
            //SqlConex.Close();
            //SqlComando.CommandText = " DELETE FROM Requirientes WHERE usuario=" + Lista.SelectedValue;
            //SqlConex.Open();
            //SqlComando.ExecuteNonQuery();
            //SqlConex.Close();
            //SqlComando.CommandText = " DELETE FROM Clientes WHERE usuario=" + Lista.SelectedValue;
            //SqlConex.Open();
            //SqlComando.ExecuteNonQuery();
            //SqlConex.Close();
            //SqlComando.CommandText = " DELETE FROM Usuarios WHERE codigo=" + Lista.SelectedValue;
            //SqlConex.Open();
            //SqlComando.ExecuteNonQuery();
            //SqlConex.Close();
        }

        protected void Perfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenaPerfil();
        }

        protected void Buscar_TextChanged(object sender, EventArgs e)
        {
            Lista.Items[1].Attributes.Add("display", "none");
        }       
    }
}