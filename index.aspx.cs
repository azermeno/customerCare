using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
//using MySql.Data.MySqlClient;

namespace CustomerCare
{
    public partial class index : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "index";
        }

        private Boolean debeCambiar;

        private void esCambioContrasena()
        {
            Label1.Text = "Contraseña";
                    edUsuario.ToolTip = "Escribe tu contraseña actual";
                    edUsuario.TextMode = TextBoxMode.Password;
                    edPassword.ToolTip = "Escribe la nueva contraseña";
                    chbRecuerdame.Visible = false;
                    Label4.Text = "Nueva";
                    Label5.Visible = true;
                    edPassword2.Visible = true;
                    ValidadorCambioContrasena.Enabled = true;
                    Validador.Enabled = false;
                    RequiredFieldValidator1.Enabled = false;
                    Iguales.Enabled = true;
                    ValidadorCambioContrasena.Visible = true;
                    Validador.Enabled = false;
                    RequiredFieldValidator1.Visible = false;
                    Iguales.Visible = true;
                    Label6.Visible = true;
                    Label2.Visible = false;
                    Label3.Visible = false;
        }

        private void redireccionar(bool pruebas)
        {
            if (!(Session["wy"] == null))
            {
                if (pruebas)
                {
                    if (Request.QueryString["url"] == null)
                        Response.Redirect("/pruebas/CustomerCare.aspx");
                    else
                        Response.Redirect("/pruebas/" + Request.QueryString["url"]);
                }
                else
                {
                    if (Request.QueryString["url"] == null)
                        Response.Redirect("CustomerCare.aspx");
                    else
                        Response.Redirect(Request.QueryString["url"]);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //agregaLink(ImagesFolder + "favicon16x16.png", "icon", "image/png", "16x16");
            //agregaLink(ImagesFolder + "favicon32x32.png", "icon", "image/png", "32x32");
            //agregaLink(ImagesFolder + "favicon48x48.png", "icon", "image/png", "48x48");
            //agregaCssSkin("index.css");
            Panel1.BackImageUrl = ImagesFolder + "login-box.png";
            Panel1.BackColor = Color.Transparent;
            Panel1.BorderColor = Color.White;
            if (Request.QueryString["cad"] == null)
            {
                Label2.Visible = false;
                Label3.Visible = false;
            }
            if (Request.Cookies["DatosUsuario"] != null)
            {
                if (Request.Cookies["DatosUsuario"]["Recordar"] != null)
                {
					///////////////////////////////////////////MySql/////////////////////////////////
//					MySqlDataReader dbRead;
//					MySqlConnection bdpCon;
//                    bdpCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
//					MySqlCommand Qry;
//                    Qry = new MySqlCommand();
					
					
					/////////////////////////////////SqlServer//////////////////////////////////
                    SqlDataReader dbRead;
                    System.Data.SqlClient.SqlConnection bdpCon;
                    bdpCon = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
					SqlCommand Qry;
                    Qry = new SqlCommand();
                    try
                    {
                        Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                    }
                    catch
                    {
                        Qry.CommandTimeout = 300;
                    }
					
                    Qry.CommandText = "SELECT DCC, rtrim(Login), Pruebas FROM Usuarios WHERE Codigo=" + Utilidades.Crip(Request.Cookies["DatosUsuario"]["wy"]);
                    Qry.Connection = bdpCon;
                    bdpCon.Open();
                    dbRead = Qry.ExecuteReader();
                    if (dbRead.Read())
                    {
                        if (dbRead.GetBoolean(0))
                        {
                            hfdUsu.Value = dbRead.GetString(1);
                            esCambioContrasena();
                        }
                        else
                        {
                            Session.Add("wy", Request.Cookies["DatosUsuario"]["wy"]);
                            Session.Add("usrdescrip", Request.Cookies["DatosUsuario"]["Nombre"]);
                            Session.Add("tipousuario", Request.Cookies["DatosUsuario"]["TipoUsuario"]);
                            redireccionar(dbRead.GetBoolean(2));
                        }
                    }
                    dbRead.Close();
                    bdpCon.Close();
                }
                else
                    Response.Cookies["DatosUsuario"].Expires = DateTime.Now.AddMonths(-1);
            }
        }

        public string Encrip(string cad)
        {
            int pas;
            pas = 0;
            foreach (char c in cad)
                pas = pas + (int)c;
            pas = 134*pas;
            return(pas.ToString());
        }

        //public string Crip(string wy)
        //{
        //    string texto = "";
        //    int c0;
        //    foreach (char c in wy)
        //    {
        //        if (c > 47 && c < 58)
        //        {
        //            c0 = c - 48;
        //            texto = texto + (char)(48 + ((c0 + 5) % 10));
        //        }
        //        else if (c > 64 && c < 91)
        //        {
        //            c0 = c - 65;
        //            texto = texto + (char)(65 + ((c0 + 13) % 26));
        //        }
        //        else if (c > 96 && c < 123)
        //        {
        //            c0 = c - 97;
        //            texto = texto + (char)(97 + ((c0 + 13) % 26));
        //        }
        //        else
        //            texto = texto + (char)(c);
        //    }
        //    return texto;
        //}

        protected void ValidadorCambioContrasena_ServerValidate(object source, ServerValidateEventArgs args)
        {
			///////////////////////////////////////////MySql/////////////////////////////////
//			MySqlDataReader dbRead;
//			MySqlConnection bdpCon;
//            bdpCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
//			MySqlCommand Qry;
//            Qry = new MySqlCommand();
			
			
			/////////////////////////////////SqlServer//////////////////////////////////
            SqlDataReader dbRead;
            System.Data.SqlClient.SqlConnection bdpCon;
            bdpCon = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
			SqlCommand Qry;
            Qry = new SqlCommand();
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
			
            Qry.CommandText = "SELECT DCC, Pruebas FROM Usuarios WHERE rtrim(Login)=rtrim('" + hfdUsu.Value + "') and cast(Clave as" +
                " varchar)='" + Encrip(edUsuario.Text) + "'";
            Qry.Connection = bdpCon;
            bdpCon.Open();
            dbRead = Qry.ExecuteReader();
            if (dbRead.Read() && dbRead.GetBoolean(0))
            {
                pruebas = dbRead.GetBoolean(1);
                dbRead.Close();
                Qry.CommandText = "UPDATE Usuarios SET DCC=0, Clave=" + Encrip(edPassword.Text) + " WHERE rtrim(Login)=rtrim('" + hfdUsu.Value + "') and cast(Clave as" +
                " varchar)='" + Encrip(edUsuario.Text) + "'";
                Qry.ExecuteNonQuery();
                args.IsValid = true;
                debeCambiar = false;
            }
            else
                args.IsValid = false;
            bdpCon.Close();
        }

        private bool pruebas;

        protected void Validador_ServerValidate(object source, ServerValidateEventArgs args)
        {
			///////////////////////////////////////////MySql/////////////////////////////////
//			MySqlDataReader dbRead;
//			MySqlConnection bdpCon;
//            bdpCon = new MySqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
//			MySqlCommand Qry;
//            Qry = new MySqlCommand();
			
			
			/////////////////////////////////SqlServer//////////////////////////////////
            SqlDataReader dbRead;
            System.Data.SqlClient.SqlConnection bdpCon;
            bdpCon = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
			SqlCommand Qry;
            Qry = new SqlCommand();
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
			
            int pc;
            Qry.CommandText = "SELECT Usuarios.Nombre, Usuarios.TipoUsuario, Usuarios.DCC, Usuarios.Codigo, Usuarios.Pruebas, Usuarios.permisos, Perfiles.Permisos As perfperm  FROM Usuarios left JOIN Perfiles ON Usuarios.Perfil=Perfiles.Codigo WHERE rtrim(Login)=rtrim('" + edUsuario.Text + "') and cast(Clave as" +
                " varchar)='" +Encrip(edPassword.Text) +"'";
            Qry.Connection = bdpCon;
            bdpCon.Open();
            dbRead = Qry.ExecuteReader();
            if (dbRead.Read())
            {
                hfdUsu.Value = edUsuario.Text;
                if (dbRead.IsDBNull(0))
                    Session.Add("usrdescrip", "");
                else
                    Session.Add("usrdescrip", dbRead.GetString(0));
                if (dbRead.IsDBNull(1))
                    pc = 0;
                else
                    pc = dbRead.GetInt32(1);
                if (!dbRead.IsDBNull(2))
                    debeCambiar = dbRead.GetBoolean(2);
                args.IsValid = true;
                Session.Add("tipousuario", Utilidades.Crip(pc.ToString()));
                Session.Add("wy", Utilidades.Crip(dbRead.GetInt32(3).ToString()));
                if (chbRecuerdame.Checked == true)
                {
                    Response.Cookies["DatosUsuario"]["Recordar"] = "si";
                    Response.Cookies["DatosUsuario"]["wy"] = Session["wy"].ToString();
                    Response.Cookies["DatosUsuario"]["Nombre"] = Session["usrdescrip"].ToString();
                    Response.Cookies["DatosUsuario"]["TipoUsuario"] = Session["tipousuario"].ToString();
                    Response.Cookies["DatosUsuario"].Expires = DateTime.Now.AddMonths(2);
                }
                else
                    Response.Cookies["DatosUsuario"].Expires = DateTime.Now.AddDays(1);
                string per="";
                try
                {
                    if (dbRead.GetString(5) != null)
                    {
                        per = "," + dbRead.GetString(5) + ",";
                    }
                }
                catch
                {

                }
                try
                {
                    if (dbRead.GetString(6) != null)
                    {
                        per += "," + dbRead.GetString(6) + ",";
                    }
                }
                catch
                {

                }
                per = per.Replace(",,", ",");
                Response.Cookies["DarosUsuario"]["Permisos"] = per;
                pruebas = dbRead.GetBoolean(4);
            }
            else
                args.IsValid = false;
            dbRead.Close();
            bdpCon.Close();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (ValidadorCambioContrasena.Enabled && ValidadorCambioContrasena.IsValid)
                redireccionar(pruebas);
            if (Validador.Enabled && Validador.IsValid)
            {
                if (debeCambiar)
                {
                    esCambioContrasena();
                }
                else
                    redireccionar(pruebas);
            }
        }
    }
}