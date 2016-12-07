using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerCare.Administracion
{
    public partial class Unidades : System.Web.UI.Page
    {
        private void modoEdicion(bool editando)
        {
            Nombre_Unidad.ReadOnly = !editando;
            Alias.ReadOnly = !editando;
            Activo.Enabled = editando;
            Productos.Enabled = editando;
            Clientes.Enabled = editando;
            Sitios.Enabled = editando;
            esEncuestado.Enabled = editando;
            Sesion.ReadOnly = !editando;
            Responsable.ReadOnly = !editando;
            Contacto.ReadOnly = !editando;
            Hora.ReadOnly = !editando;
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
                tCodigo.Style.Add("display", "block");
                Codigo.Style.Add("display", "block");
            }
            bActivos.Enabled = !editando;
            Buscar.ReadOnly = editando;
            esEncuestado.Enabled = editando;
            Sesion.ReadOnly = !editando;
            Responsable.ReadOnly = !editando;
            Contacto.ReadOnly = !editando;
            Hora.ReadOnly = !editando;
            Domicilio.ReadOnly = !editando;
            Ciudad.ReadOnly = !editando;
        }

        private void getUnidades(Boolean activo)
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
            if (activo)
            {
                SqlComando.CommandText = "SELECT Codigo,Nombre FROM Unidades WHERE activa=1 ORDER By nombre";
            }
            else
            {
                SqlComando.CommandText = "SELECT Codigo,Nombre FROM Unidades ORDER By nombre";
            }
            SqlConex.Open();
            SqlParellenar.Fill(unid);
            SqlConex.Close();
            Lista.DataSource = unid;
            Lista.DataTextField = "Nombre";
            Lista.DataValueField = "codigo";
            Lista.DataBind();
        }

        protected void getUnidad(string valor)
        {
            SqlConnection SqlConex = new SqlConnection();
            SqlConnection SqlConexEnc = new SqlConnection();
            SqlCommand SqlComando = new SqlCommand();
            try
            {
                SqlComando.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                SqlComando.CommandTimeout = 300;
            }
            SqlCommand SqlComandoEnc = new SqlCommand();
            try
            {
                SqlComandoEnc.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                SqlComandoEnc.CommandTimeout = 300;
            }
            SqlDataAdapter SqlParellenar = new SqlDataAdapter(SqlComando);
            SqlDataAdapter SqlParellenarEnc = new SqlDataAdapter(SqlComandoEnc);
            DataSet unid = new DataSet();
            DataSet enc = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComandoEnc.Connection = SqlConexEnc;
            SqlConexEnc.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CiberEncuesta"].ConnectionString;
            SqlComando.CommandText = "SELECT sitio,alias,producto,cliente,nombre,activa FROM unidades WHERE codigo=" + valor;
            Utilidades.log(SqlComando.CommandText);
            SqlConex.Open();
            SqlParellenar.Fill(unid);
            SqlConex.Close();
            if (unid.Tables[0].Rows.Count > 0)
            {
                Nombre_Unidad.Text = unid.Tables[0].Rows[0]["nombre"].ToString().Trim();
                Alias.Text = unid.Tables[0].Rows[0]["alias"].ToString().Trim();
                try
                {
                    Productos.SelectedValue = unid.Tables[0].Rows[0]["producto"].ToString().Trim();
                }
                catch
                {
                    Productos.SelectedIndex = -1;
                }
                try
                {
                    Clientes.SelectedValue = unid.Tables[0].Rows[0]["cliente"].ToString().Trim();
                }
                catch
                {
                    Clientes.SelectedIndex = -1;
                }
                try
                {
                    Sitios.SelectedValue = unid.Tables[0].Rows[0]["sitio"].ToString().Trim();
                }
                catch
                {
                    Sitios.SelectedIndex = -1;
                }
                Activo.Checked = Convert.ToBoolean(unid.Tables[0].Rows[0]["activa"]);
                Codigo.Text = valor.Trim();
                //if (unid.Tables[0].Rows[0]["activa"].ToString().Trim() == "1")
                //{
                //    Activo.Checked = true;
                //}
                //else
                //{
                //    Activo.Checked = false;
                //}
            }
            if (System.Configuration.ConfigurationManager.AppSettings["CiberEncuesta"].ToString().Trim().ToUpper() == "TRUE")
            {
                SqlComandoEnc.CommandText = "SELECT idCliente,req_codigo,txtDescrip,esencuestado,txtresponsable,idtipocliente,idproducto,tecnico,idprod,horafinal,txtdomicilio,txtciudad,txtcontacto,txtnombre FROM clienteids WHERE req_codigo=" + valor;
                SqlConexEnc.Open();
                SqlParellenarEnc.Fill(enc);
                SqlConexEnc.Close();
                if (enc.Tables[0].Rows.Count > 0)
                {
                    if (enc.Tables[0].Rows[0]["esencuestado"].ToString().Trim() == "1")
                    {
                        esEncuestado.Checked = true;
                    }
                    else
                    {
                        esEncuestado.Checked = false;
                    }
                    Sesion.Text = enc.Tables[0].Rows[0]["idcliente"].ToString().Trim();
                    Responsable.Text = enc.Tables[0].Rows[0]["txtresponsable"].ToString().Trim();
                    Contacto.Text = enc.Tables[0].Rows[0]["txtcontacto"].ToString().Trim();
                    Hora.Text = enc.Tables[0].Rows[0]["horafinal"].ToString().Trim();
                    Domicilio.Text = enc.Tables[0].Rows[0]["txtdomicilio"].ToString().Trim();
                    Ciudad.Text = enc.Tables[0].Rows[0]["txtciudad"].ToString().Trim();
                }
                else
                {
                    esEncuestado.Checked = false;
                    Sesion.Text = "";
                    Responsable.Text = "";
                    Contacto.Text = "";
                    Hora.Text = "15";
                }
                if (Hora.Text.Trim() == "")
                {
                    Hora.Text = "15";
                }
            }
        }

        protected void getSitios()
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
            DataSet sit = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "SELECT codigo,nombre FROM sitios WHERE activo=1 ORDER by nombre";
            SqlConex.Open();
            SqlParellenar.Fill(sit);
            SqlConex.Close();
            Sitios.DataSource = sit;
            Sitios.DataTextField = "nombre";
            Sitios.DataValueField = "codigo";
            Sitios.DataBind();
        }

        protected void getProductos()
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
            DataSet prod = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "SELECT codigo,nombre FROM productos WHERE activo=1 ORDER By nombre";
            SqlConex.Open();
            SqlParellenar.Fill(prod);
            SqlConex.Close();
            Productos.DataSource = prod;
            Productos.DataTextField = "nombre";
            Productos.DataValueField = "codigo";
            Productos.DataBind();
        }

        protected void getClientes()
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
            DataSet clien = new DataSet();
            SqlComando.Connection = SqlConex;
            SqlConex.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;
            SqlComando.CommandText = "SELECT Clientes.Codigo,Usuarios.Nombre FROM Clientes LEFT JOIN Usuarios ON Clientes.Usuario = Usuarios.Codigo ORDER By Nombre";
            SqlConex.Open();
            SqlParellenar.Fill(clien);
            SqlConex.Close();
            Clientes.DataSource = clien;
            Clientes.DataTextField = "nombre";
            Clientes.DataValueField = "codigo";
            Clientes.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            alerta_fondo.Style.Add("display", "none");
            alerta_mns.Style.Add("display", "none");
            if (!IsPostBack)
            {
                Operacion.Value = "";
                modoEdicion(false);
                getUnidades(true);
                getSitios();
                getProductos();
                getClientes();
                if (Lista.Items.Count > 0)
                {
                    Lista.SelectedIndex = 0;
                    getUnidad(Lista.SelectedValue);
                }
                if (System.Configuration.ConfigurationManager.AppSettings["CiberEncuesta"].ToString().Trim().ToUpper() == "TRUE")
                {
                    esEncuestado.Visible = true;
                    Sesion.Visible = true;
                    Responsable.Visible = true;
                    Contacto.Visible = true;
                    Hora.Visible = true;
                    LSesion.Visible = true;
                    LResponsable.Visible = true;
                    LContacto.Visible = true;
                    LHora.Visible = true;
                    denc.Visible = true;
                }
                else
                {
                    esEncuestado.Visible = false;
                    Sesion.Visible = false;
                    Responsable.Visible = false;
                    Contacto.Visible = false;
                    Hora.Visible = false;
                    LSesion.Visible = false;
                    LResponsable.Visible = false;
                    LContacto.Visible = false;
                    LHora.Visible = false;
                    denc.Visible = false;
                }
                
            }    
        }

        protected void Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            Nombre_Unidad.Text = ""; 
            Alias.Text = "";
            Productos.SelectedIndex = -1;
            Clientes.SelectedIndex = -1;
            Sitios.SelectedIndex = -1;
            esEncuestado.Checked = false;
            Sesion.Text = "";
            Responsable.Text = "";
            Contacto.Text = "";
            Hora.Text = "";
            Ciudad.Text = "";
            Domicilio.Text = "";
            modoEdicion(true);
            Operacion.Value = "NUEVO"; 
            tCodigo.Style.Add("display", "none");
            Codigo.Style.Add("display", "none");
        }

        protected void Modificar_Click(object sender, ImageClickEventArgs e)
        {
            modoEdicion(true);
            Operacion.Value = "MODIFICAR";
        }

        protected void Guardar_Click(object sender, EventArgs e)
        {
            if (Nombre_Unidad.Text.Trim() != "")
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
                SqlComando.CommandText = "SELECT nombre,alias FROM unidades WHERE (nombre='" + Nombre_Unidad.Text.Trim() + "'";
                if (Alias.Text.Trim() != "")
                {
                    SqlComando.CommandText += " or alias='" + Alias.Text.Trim() + "'";
                }
                SqlComando.CommandText += ")";
                if (Operacion.Value.Trim().ToUpper() != "NUEVO")
                {
                    SqlComando.CommandText += " and codigo<>" + Lista.SelectedValue;
                }
                SqlConex.Open();
                SqlParellenar.Fill(unid);
                SqlConex.Close();
                if (unid.Tables[0].Rows.Count == 0)
                {
                    string cg = "";
                    string sitio, producto, cliente;
                    if (Operacion.Value.Trim().ToUpper() == "NUEVO")
                    {
                        SqlComando.CommandText = "INSERT INTO unidades (";
                        if (Sitios.SelectedIndex >= 0)
                        {
                            SqlComando.CommandText += "sitio,";
                            sitio = Sitios.SelectedValue.Trim() + ",";
                        }
                        else
                        {
                            sitio = "";
                        }
                        SqlComando.CommandText += "alias,";
                        if (Productos.SelectedIndex >= 0)
                        {
                            SqlComando.CommandText += "producto,";
                            producto = Productos.SelectedValue.Trim() + ",";
                        }
                        else
                        {
                            producto = "";
                        }
                        if (Clientes.SelectedIndex >= 0)
                        {
                            SqlComando.CommandText += "cliente,";
                            cliente = Clientes.SelectedValue.Trim() + ",";
                        }
                        else
                        {
                            cliente = "";
                        }
                        SqlComando.CommandText += "nombre,activa)VALUES("
                                              + sitio + "'" + Alias.Text.Trim() + "'," + producto
                                              + cliente + "'" + Nombre_Unidad.Text.Trim() + "',";
                        if (Activo.Checked)
                        {
                            SqlComando.CommandText += "1";
                        }
                        else
                        {
                            SqlComando.CommandText += "0";
                        }
                        SqlComando.CommandText += ")";
                    }
                    else
                    {
                        if (Sitios.SelectedIndex >= 0)
                        {
                            SqlComando.CommandText += "sitio,";
                            sitio = "Sitio=" + Sitios.SelectedValue.Trim() + ", ";
                        }
                        else
                        {
                            sitio = "Sitio='', ";
                        }
                        if (Productos.SelectedIndex >= 0)
                        {
                            SqlComando.CommandText += "producto,";
                            producto = "producto=" + Productos.SelectedValue.Trim() + ", ";
                        }
                        else
                        {
                            producto = "producto='', ";
                        }
                        if (Clientes.SelectedIndex >= 0)
                        {
                            SqlComando.CommandText += "cliente,";
                            cliente = "cliente=" + Clientes.SelectedValue.Trim() + ", ";
                        }
                        else
                        {
                            cliente = "cliente='', ";
                        }

                        SqlComando.CommandText = "UPDATE unidades SET " + sitio + " alias='"
                                              + Alias.Text.Trim() + "', " + producto + cliente + " nombre='" + Nombre_Unidad.Text.Trim() + "', activa=";
                        if (Activo.Checked)
                        {
                            SqlComando.CommandText += "1";
                        }
                        else
                        {
                            SqlComando.CommandText += "0";
                        }
                        SqlComando.CommandText += " WHERE codigo=" + Lista.SelectedValue;
                        cg = Lista.SelectedValue;
                    }
                    Utilidades.log(SqlComando.CommandText);
                    SqlConex.Open();
                    SqlComando.ExecuteNonQuery();
                    SqlConex.Close();
                    if (Operacion.Value.Trim().ToUpper() == "NUEVO")
                    {
                        SqlComando.CommandText = "SELECT codigo FROM unidades WHERE alias='" + Alias.Text.Trim() + "' and nombre='" + Nombre_Unidad.Text.Trim() + "'";
                        SqlConex.Open();
                        SqlParellenar.Fill(unid);
                        SqlConex.Close();
                        if (unid.Tables[0].Rows.Count > 0)
                        {
                            cg = unid.Tables[0].Rows[0]["codigo"].ToString().Trim();
                        }
                    }
                    if (cg != "")
                    {
                        if (System.Configuration.ConfigurationManager.AppSettings["CiberEncuesta"].ToString().Trim().ToUpper() == "TRUE")
                        {
                            string idproducto="null";
                            string NombreProducto="null";
                            string tecnico="null";
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
                            DataSet unidEnc = new DataSet();
                            DataSet prodEnc = new DataSet();
                            DataSet tecn = new DataSet();
                            SqlComandoEnc.Connection = SqlConexEnc;
                            SqlConexEnc.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CiberEncuesta"].ConnectionString;
                            SqlComandoEnc.CommandText = "SELECT idcliente,req_codigo FROM clienteids WHERE req_codigo=" + cg;
                            SqlConexEnc.Open();
                            SqlParellenarEnc.Fill(unidEnc);
                            SqlConexEnc.Close();
                            if (Productos.SelectedIndex>=0)
                            {
                                SqlComandoEnc.CommandText="SELECT idproducto,nombreproducto,txtfiltro FROM productos WHERE codcus="+Productos.SelectedValue;
                                SqlConexEnc.Open();
                                SqlParellenarEnc.Fill(prodEnc);
                                SqlConexEnc.Close();
                                if(prodEnc.Tables[0].Rows.Count>0)
                                {
                                    idproducto="'"+prodEnc.Tables[0].Rows[0]["idproducto"].ToString().Trim()+"'";
                                    NombreProducto="'"+prodEnc.Tables[0].Rows[0]["nombreproducto"].ToString().Trim()+"'";
                                }
                            }
                            if (Sitios.SelectedIndex>=0)
                            {
                                SqlComando.CommandText="SELECT responsable FROM sitios WHERE codigo="+Sitios.SelectedValue;
                              //  SqlComando.CommandText = "SELECT idCS FROM ciberencuesta.dbo.CentrosSolucion WHERE CSNombre=" + Sitios.SelectedValue;
                                SqlConex.Open();
                                SqlParellenar.Fill(tecn);
                                SqlConex.Close();
                                if (tecn.Tables[0].Rows.Count>0)
                                {
                                    tecnico=tecn.Tables[0].Rows[0]["responsable"].ToString().Trim();
                                }
                            }
                            string hr="null";
                            if (Hora.Text.Trim() != "")
                            {
                                hr = Hora.Text.Trim();
                            }
                            if (unidEnc.Tables[0].Rows.Count > 0)
                            {
                                SqlComandoEnc.CommandText = "UPDATE clienteids SET idcliente='"+Sesion.Text.Trim()+"', txtdescrip='"+Nombre_Unidad.Text.Trim()+"', esencuestado=";
                                if (esEncuestado.Checked)
                                {
                                    SqlComandoEnc.CommandText+="1";
                                }
                                else
                                {
                                    SqlComandoEnc.CommandText+="0";
                                }

                                SqlComandoEnc.CommandText += ", txtResponsable='" + Responsable.Text.Trim() + "', idtipocliente=1, idproducto=" + NombreProducto + ", tecnico=" + Sitios.SelectedValue + ", idprod=" + idproducto + ", horafinal=" + hr + ", txtdomicilio='" + Domicilio.Text.Trim() + "', txtciudad='" + Ciudad.Text.Trim() + "', txtcontacto='" + Contacto.Text.Trim() + "' WHERE req_codigo=" + cg;
                            }
                            else
                            {
                                SqlComandoEnc.CommandText = "INSERT INTO clienteids (idcliente,req_codigo,txtdescrip,esencuestado,txtresponsable,idtipocliente,idproducto,tecnico,idprod,horafinal,txtdomicilio,txtciudad,txtcontacto)VALUES('"
                                                          + Sesion.Text.Trim() + "'," + cg + ",'" + Nombre_Unidad.Text.Trim() + "',";
                                if (esEncuestado.Checked)
                                {
                                    SqlComandoEnc.CommandText += "1";
                                }
                                else
                                {
                                    SqlComandoEnc.CommandText += "0";
                                }
                                SqlComandoEnc.CommandText += ",'" + Responsable.Text.Trim() + "',1," + NombreProducto + "," + tecnico + "," + idproducto + "," + hr + ",'" + Domicilio.Text.Trim() + "','" + Ciudad.Text.Trim() + "','" + Contacto.Text.Trim() + "')";
                            }
                            SqlConexEnc.Open();
                            SqlComandoEnc.ExecuteNonQuery();
                            SqlConexEnc.Close();
                        }
                    }
                    getUnidades(bActivos.Checked);
                    if (cg != "")
                    {
                        try
                        {
                            Lista.SelectedValue = cg;
                        }
                        catch
                        {
                            Lista.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        Lista.SelectedIndex = 0;
                        for (int x = 0; x < Lista.Items.Count; x++)
                        {
                            if (Lista.Items[x].Text.Trim().ToUpper() == Nombre_Unidad.Text.Trim().ToUpper())
                            {
                                Lista.SelectedIndex = x;
                            }
                        }
                    }
                    getUnidad(Lista.SelectedValue);
                    modoEdicion(false);
                }
                else
                {
                    mns.Text = "El nombre y/o alias especificado ya se encuentra asignado a otra unidad";
                    alerta_fondo.Style.Add("display", "block");
                    alerta_mns.Style.Add("display", "block");
                }
            }
            else
            {
                mns.Text = "Debe de introducir el nombre de la unidad";
                alerta_fondo.Style.Add("display", "block");
                alerta_mns.Style.Add("display", "block");
                Nombre_Unidad.Focus();
            }
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            modoEdicion(false);
            getUnidad(Lista.SelectedValue);
        }

        protected void Lista_SelectedIndexChanged(object sender, EventArgs e)
        {
            getUnidad(Lista.SelectedValue);
        }

        protected void bActivos_CheckedChanged(object sender, EventArgs e)
        {
            getUnidades(bActivos.Checked);
            if (Lista.Items.Count > 0)
            {
                Lista.SelectedIndex = 0;
                getUnidad(Lista.SelectedValue);
            }
        }
    }
}