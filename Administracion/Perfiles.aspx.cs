using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Text;
using Telerik.Web.UI;

namespace CustomerCare
{
    public partial class Perfiles : Extra
    {
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

        private void modoEdicion(bool editando)
        {
            Nombre_Perfil.ReadOnly = !editando;
            Activo.Enabled = editando;
            Permisos.Enabled = editando;
            Nuevo.Enabled = !editando;
            Modificar.Enabled = !editando;
            //Eliminar.Enabled = !editando;
            Lista.Enabled = !editando;
            if (editando)
            {
                Lista.Attributes.Add("disabled", "diasbled");
                Guardar.Style.Add("display", "inline");
                Cancelar.Style.Add("display", "inline");
            }
            else
            {
                Lista.Attributes.Remove("disabled");
                Guardar.Style.Add("display", "none");
                Cancelar.Style.Add("display", "none");
            }
            Guardar.Enabled = editando;
            Cancelar.Enabled = editando;
            Buscar.ReadOnly = editando;
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
            SqlComando.CommandText = "SELECT Codigo,Nombre,Permisos FROM perfiles ORDER By Nombre";
            SqlConex.Open();
            SqlParellenar.Fill(perf);
            SqlConex.Close();
            Lista.DataSource = perf;
            Lista.DataTextField = "Nombre";
            Lista.DataValueField = "Codigo";
            Lista.DataBind();
        }

        private void rellena()
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
            SqlComando.CommandText = "SELECT Codigo,Nombre,Permisos, Activo FROM perfiles WHERE codigo=" + Lista.SelectedValue;
            SqlConex.Open();
            SqlParellenar.Fill(perf);
            SqlConex.Close();
            string perm = "";
            if (perf.Tables[0].Rows.Count > 0)
            {
                perm = "," + perf.Tables[0].Rows[0]["Permisos"].ToString().Trim();
                Nombre_Perfil.Text = perf.Tables[0].Rows[0]["Nombre"].ToString().Trim();
                Activo.Checked = Convert.ToBoolean(perf.Tables[0].Rows[0]["Activo"]);
            }
            perm += ",";
            for (int i = 0; i < Permisos.Items.Count; i++)
            {
                if (perm.Contains("," + Permisos.Items[i].Value + ","))
                {
                    Permisos.Items[i].Selected = true;
                }
                else
                {
                    Permisos.Items[i].Selected = false;
                }
            }
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
                getPermisos();
                getPerfiles();
                if (Lista.Items.Count > 0)
                {
                    Lista.SelectedIndex = 0;
                    rellena();
                }
                modoEdicion(false);
            }
        }

        protected void Lista_SelectedIndexChanged(object sender, EventArgs e)
        {
            rellena();
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            modoEdicion(false);
            rellena();
        }

        protected void Modificar_Click(object sender, ImageClickEventArgs e)
        {
            Operacion.Value = "MODIFICAR";
            modoEdicion(true);
        }

        protected void Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            Nombre_Perfil.Text = "";
            Activo.Checked = true;
            for (int i = 0; i < Permisos.Items.Count; i++)
            {
                Permisos.Items[i].Selected = false;
            }
            Operacion.Value = "NUEVO";
            modoEdicion(true);
        }

        protected void Guardar_Click(object sender, EventArgs e)
        {
            string perm="";
            int act=1;
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
            for (int x = 0; x < Permisos.Items.Count; x++)
            {
                if (Permisos.Items[x].Selected)
                {
                    if (perm != "")
                    {
                        perm += ",";
                    }
                    perm += Permisos.Items[x].Value;
                }
            }   
            if(Activo.Checked)
            {
                act=1;
            }
            else
            {
                act=0;
            }
            if (Operacion.Value == "NUEVO")
            {
                try
                {
                    SqlComando.CommandText = "SELECT nombre FROM perfiles WHERE nombre='" + Nombre_Perfil.Text.Trim() + "'";
                    Utilidades.log(SqlComando.CommandText);
                    SqlConex.Open();
                    SqlParellenar.Fill(perf);
                    SqlConex.Close();
                    if (perf.Tables[0].Rows.Count > 0)
                    {
                        mns.Text = "Ya existe un registro con el mismo nombre de perfil";
                        alerta_fondo.Style.Add("display", "block");
                        alerta_mns.Style.Add("display", "block");
                    }
                    else
                    {
                        SqlComando.CommandText = "INSERT INTO perfiles (nombre,permisos,activo)VALUES('"
                                               + Nombre_Perfil.Text.Trim() + "','" + perm + "'," + act.ToString() + ")";
                        Utilidades.log(SqlComando.CommandText);
                        SqlConex.Open();
                        SqlComando.ExecuteNonQuery();
                        SqlConex.Close();
                    }
                    getPerfiles();
                    for (int x = 0; x < Lista.Items.Count; x++)
                    {
                        if (Lista.Items[x].Text.Trim().ToUpper() == Nombre_Perfil.Text.Trim().ToUpper())
                        {
                            Lista.SelectedIndex = x;
                        }
                    }
                }
                catch (Exception exc)
                {
                    try
                    {
                        Utilidades.log("Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket derivado:" + exc.Message + " StackTrace:" + exc.StackTrace));
                    }
                    catch
                    {
                        Utilidades.log("Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket derivado:" + exc.Message + " StackTrace:" + exc.StackTrace));
                    }
                }
            }
            else
            {
                try
                {
                    string cod = Lista.SelectedValue;
                    SqlComando.CommandText = "SELECT nombre FROM perfiles WHERE nombre='" + Nombre_Perfil.Text.Trim() + "' and codigo <>" + Lista.SelectedValue;
                    Utilidades.log(SqlComando.CommandText);
                    SqlConex.Open();
                    SqlParellenar.Fill(perf);
                    SqlConex.Close();
                    if (perf.Tables[0].Rows.Count > 0)
                    {
                        mns.Text = "Ya existe un registro con el mismo nombre de perfil";
                        alerta_fondo.Style.Add("display", "block");
                        alerta_mns.Style.Add("display", "block");
                    }
                    else
                    {
                        SqlComando.CommandText = "UPDATE perfiles SET nombre='" + Nombre_Perfil.Text.Trim()
                                               + "', permisos='" + perm + "', activo=" + act.ToString()
                                               + " WHERE codigo=" + Lista.SelectedValue;
                        Utilidades.log(SqlComando.CommandText);
                        SqlConex.Open();
                        SqlComando.ExecuteNonQuery();
                        SqlConex.Close();
                    }
                    getPerfiles();
                    Lista.SelectedValue = cod;
                }
                catch (Exception exc)
                {
                    try
                    {
                        Utilidades.log("Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket derivado:" + exc.Message + " StackTrace:" + exc.StackTrace));
                    }
                    catch
                    {
                        Utilidades.log("Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket derivado:" + exc.Message + " StackTrace:" + exc.StackTrace));
                    }
                }
            }
            modoEdicion(false);
            rellena();
        }
    }
}