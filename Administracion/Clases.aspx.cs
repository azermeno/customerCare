using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;
using System.Configuration;
using Newtonsoft.Json;

namespace CustomerCare
{
    public partial class Clases1 : CustomerCareResource
    {
        private int clasificacion;
        private string plural;
        private DataSet Rels;


        ////////////////////temporal para la encuesta////////////////////////////////////
        private bool encuesta;
        ////////////////////temporal para la encuesta////////////////////////////////////


        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "Clases";
            Controles = new string[] { "Splitter", "ComboBox" };
            plural = Request.QueryString["clasificacion"];
            dao.open();
            clasificacion = (int)(new SqlCommand("select codigo from Clasificaciones where Plural='" + plural + "'", dao.con)).ExecuteScalar();
            hflTemplate.Value = (string)(new SqlCommand("select top 1 isnull(ce.Campos,'{}') from Clasificaciones c left join CamposClasificaciones cc on c.Codigo= cc.Clasificacion left join CamposExtra ce on cc.Campos=ce.Id where c.Codigo=" + clasificacion.ToString(), dao.con)).ExecuteScalar();
            dao.close();
            SqlClases.SelectCommand = "SELECT c.codigo,c.nombre,c.activa,c.datosextra" +






                ////////temp
                ",'' as idcliente,CAST(0 As bit) As encuestado,'' as txtresponsable,'' as txtdomicilio,'' as txtciudad,'' as txtcontacto, isnull(u.Codigo,0) as unidad " +
                    //////temp





                " FROM [Clases] c " +
                


                
                ////////temp
                "left join unidades u on c.nombre=u.nombre and c.clasificacion=3 " +
                ////////temp
                
                
                
                "where clasificacion=" + clasificacion + " ORDER by c.Nombre";
            dao.open();
            Rels = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("Select rc.Padre, rc.Unica, c.Singular, c.Plural from RelacionesClasificaciones rc join Clasificaciones c on rc.Padre=c.Codigo where Hijo=" + clasificacion, dao.con);
            adapter.Fill(Rels);
            foreach (DataRow row in Rels.Tables[0].Rows)
            {
                Label label = new Label();
                Control control;
                if ((bool)row[1])
                {
                    label.Text = (string)row[2] + ":";
                    RadComboBox combo = new RadComboBox();
                    combo.DataTextField = "Nombre";
                    combo.DataValueField = "Codigo";
                    combo.Width = 400;
                    combo.Filter = RadComboBoxFilter.Contains;
                    combo.DataSource = new SqlDataSource(ConfigurationManager.ConnectionStrings["CustomerCare"].ProviderName, ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString, "Select Nombre, Codigo From Clases Where Clasificacion=" + (int)row[0]);
                    combo.DataBind();
                    control = combo;
                }
                else
                {
                    WebControl div = new WebControl(HtmlTextWriterTag.Div);
                    div.Style[HtmlTextWriterStyle.Width] = "400px";
                    div.Style["border"] = "1px solid black";
                    div.Style[HtmlTextWriterStyle.Height] = "100px";
                    div.Style[HtmlTextWriterStyle.OverflowX] = "visible";
                    div.Style[HtmlTextWriterStyle.OverflowY] = "auto";
                    div.Style[HtmlTextWriterStyle.Display] = "inline-block";
                    label.Text = (string)row[3] + ":";
                    RadTreeView tree = new RadTreeView();
                    tree.DataValueField = "Codigo";
                    tree.DataTextField = "Nombre";
                    tree.DataFieldID = "Codigo";
                    tree.DataFieldParentID = "Padre";
                    tree.CheckBoxes = true;
                    tree.DataSource = new SqlDataSource(ConfigurationManager.ConnectionStrings["CustomerCare"].ProviderName, ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString, "Select Nombre, Codigo, Padre From Clases c left join (select Padre, Hijo from RelacionesClases rc1 join CLases c1 on rc1.Padre=c1.Codigo where c1.Clasificacion=" + (int)row[0] + ") rc on c.Codigo=rc.Hijo Where c.Clasificacion=" + (int)row[0]);
                    tree.DataBind();
                    div.Controls.Add(tree);
                    control = div;
                }
                label.ID = "lbl" + (string)row[3];
                control.ID = "ctl" + (string)row[3];
                Relaciones.Controls.Add(label);
                Relaciones.Controls.Add(control);
            }
            dao.close();





            ////////////////////temporal para la encuesta////////////////////////////////////
            dao.open();
            encuesta = (bool)(new SqlCommand("if db_id('ciberencuesta') is not null select cast(1 as bit) else select cast(0 as bit)", dao.con)).ExecuteScalar();
            dao.close();
            if (encuesta && clasificacion == 3)
                SqlClases.SelectCommand = "SELECT c.codigo,c.nombre,c.activa,c.datosextra,idcliente,CAST(esencuestado As bit) As encuestado,txtresponsable,txtdomicilio,txtciudad,txtcontacto, isnull(u.Codigo,0) as unidad FROM [Clases] c join unidades u on c.nombre=u.nombre and c.clasificacion=3 INNER JOIN ciberencuesta.dbo.clienteids ON u.codigo=req_codigo ORDER by c.Nombre";
            ////////////////////temporal para la encuesta////////////////////////////////////



        }

        protected void selecciona()
        {
            int codigo = Convert.ToInt32(((RadTextBox)FormClases.FindControl("cod_act")).Text);
            SqlCommand qry = new SqlCommand("select Padre from RelacionesClases rc where Hijo=" + codigo, dao.con);
            try
            {
                qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                qry.CommandTimeout = 300;
            }
            dao.open();
            SqlDataReader reader = qry.ExecuteReader();
            string padres = "";
            while (reader.Read())
                padres += "," + reader.GetInt32(0);
            reader.Close();
            padres = padres + ",";
            for (int i = 0; i < Rels.Tables[0].Rows.Count; i++)
            {
                DataRow row = Rels.Tables[0].Rows[i];
                Control control = Relaciones.FindControl("ctl" + (string)row[3]);
                if (control is RadComboBox)
                {
                    RadComboBox combo = (RadComboBox)control;
                    foreach (RadComboBoxItem item in combo.Items)
                    {
                        if (padres.Contains("," + item.Value + ","))
                            item.Selected = true;
                    }
                }
                else
                {
                    RadTreeView tree = (RadTreeView)(control.Controls[0]);
                    foreach (RadTreeNode node in tree.GetAllNodes())
                    {
                        if (padres.Contains("," + node.Value + ","))
                            node.Checked = true;
                        else
                            node.Checked = false;
                    }
                }
            }
            qry.CommandText = "select c.responsable from clases c join internos i on c.responsable= i.codigo join usuarios u on i.usuario=u.codigo where u.Activo=1 and c.codigo=" + codigo;
            reader = qry.ExecuteReader();
            if (reader.Read())
                rcbResponsable.SelectedValue = reader.GetInt32(0).ToString();
            reader.Close();
            dao.close();
        }


        bool nr = false;
        private void protegido(bool activo)
        {
            bool Enab;
            if (activo)
            {
                Enab = false;
            }
            else
            {
                Enab = true;
            }
            rcbClases.Enabled = activo;
            Nuevo.Enabled = activo;
            Modificar.Enabled = activo;
            Guardar.Enabled = Enab;
            Cancelar.Enabled = Enab;
            //RadComboBox Sitios = (RadComboBox)FormClases.FindControl("Sitios");
            //Sitios.Enabled = Enab;
            RadTextBox Nombre = (RadTextBox)FormClases.FindControl("Nombre");
            Nombre.ReadOnly = activo;
            //RadTextBox Alias = (RadTextBox)FormClases.FindControl("Alias");
            //Alias.ReadOnly = activo;
            //RadComboBox Cliente = (RadComboBox)FormClases.FindControl("Cliente");
            //Cliente.Enabled = Enab;
            //RadComboBox Tecnico = (RadComboBox)FormClases.FindControl("Tecnico");
            //Tecnico.Enabled = Enab;
            //RadComboBox Producto = (RadComboBox)FormClases.FindControl("Producto");
            //Producto.Enabled = Enab;
            CheckBox ActivaCheckBox = (CheckBox)FormClases.FindControl("ActivaCheckBox");
            ActivaCheckBox.Enabled = Enab;


           ///temporal encuesta
                RadTextBox IdCliente = (RadTextBox)FormClases.FindControl("IdCliente");
                IdCliente.ReadOnly = activo;
                CheckBox Encuestado = (CheckBox)FormClases.FindControl("Encuestado");
                Encuestado.Enabled = Enab;
                RadTextBox txtResponsable = (RadTextBox)FormClases.FindControl("txtResponsable");
                txtResponsable.ReadOnly = activo;
                RadTextBox txtDomicilio = (RadTextBox)FormClases.FindControl("txtDomicilio");
                txtDomicilio.ReadOnly = activo;
                RadTextBox txtCiudad = (RadTextBox)FormClases.FindControl("txtCiudad");
                txtCiudad.ReadOnly = activo;
                RadTextBox txtContacto = (RadTextBox)FormClases.FindControl("txtContacto");
                txtContacto.ReadOnly = activo;
           ///////////////temporal encuesta



        }

        protected void Page_Load(object sender, EventArgs e)
        {
            scriptVariables.AddClientIds(FormClases, true);
            Nuevo.Image.ImageUrl=ImagesFolder + "Nuevo.ico";
            Nuevo.Image.IsBackgroundImage=true;
            Modificar.Image.ImageUrl=ImagesFolder + "Modificar.ico";
            Modificar.Image.IsBackgroundImage=true;
            Guardar.Image.ImageUrl=ImagesFolder + "Guardar.ico";
            Guardar.Image.IsBackgroundImage=true;
            Cancelar.Image.ImageUrl=ImagesFolder + "Cancelar.ico";
            Cancelar.Image.IsBackgroundImage=true;
            if (!IsPostBack)
            {
                protegido(true);
                selecciona();
            }

        }

        protected void Nuevo_Click(object sender, EventArgs e)
        {
            nr = true;
            RadTextBox cod_act = (RadTextBox)FormClases.FindControl("cod_act");
            cod_act.Text = "";
            //RadComboBox Sitios = (RadComboBox)FormClases.FindControl("Sitios");
            //Sitios.SelectedIndex = 0;
            RadTextBox Nombre = (RadTextBox)FormClases.FindControl("Nombre");
            Nombre.Text = "";
            //RadTextBox Alias = (RadTextBox)FormClases.FindControl("Alias");
            //Alias.Text = "";
            //RadComboBox Cliente = (RadComboBox)FormClases.FindControl("Cliente");
            //Cliente.SelectedIndex = 0;
            //RadComboBox Producto = (RadComboBox)FormClases.FindControl("Producto");
            //Producto.SelectedIndex = 0;
            CheckBox ActivaCheckBox = (CheckBox)FormClases.FindControl("ActivaCheckBox");
            ActivaCheckBox.Checked = true;



            /////////temp encuesta
            RadTextBox IdCliente = (RadTextBox)FormClases.FindControl("IdCliente");
            IdCliente.Text = "";
            CheckBox Encuestado = (CheckBox)FormClases.FindControl("Encuestado");
            Encuestado.Checked = false;
            RadTextBox txtResponsable = (RadTextBox)FormClases.FindControl("txtResponsable");
            txtResponsable.Text = "";
            RadTextBox txtDomicilio = (RadTextBox)FormClases.FindControl("txtDomicilio");
            txtDomicilio.Text = "";
            RadTextBox txtCiudad = (RadTextBox)FormClases.FindControl("txtCiudad");
            txtCiudad.Text = "";
            RadTextBox txtContacto = (RadTextBox)FormClases.FindControl("txtContacto");
            txtContacto.Text = "";
            /////////temp encuesta



            protegido(false);
        }

        protected void Modificar_Click(object sender, EventArgs e)
        {
            nr = false;
            protegido(false);
        }

        protected void Cancelar_Click(object sender, EventArgs e)
        {
            if (nr)
            {
                RadTextBox cod_act = (RadTextBox)FormClases.FindControl("cod_act");
                try
                {
                    rcbClases.SelectedValue = cod_act.Text;
                }
                catch
                {

                }

            }
            protegido(true);
            nr = false;
            FormClases.DataBind();
        }

        protected void Guardar_Click(object sender, EventArgs e)
        {
            //RadComboBox Sitios = (RadComboBox)FormClases.FindControl("Sitios");
            RadTextBox cod_act = (RadTextBox)FormClases.FindControl("cod_act");
            RadTextBox Nombre = (RadTextBox)FormClases.FindControl("Nombre");
            //RadTextBox Alias = (RadTextBox)FormClases.FindControl("Alias");
            //RadComboBox Cliente = (RadComboBox)FormClases.FindControl("Cliente");
            //RadComboBox Producto = (RadComboBox)FormClases.FindControl("Producto");
            CheckBox ActivaCheckBox = (CheckBox)FormClases.FindControl("ActivaCheckBox");
            //RadComboBox rcbResponsable = (RadComboBox)FormClases.FindControl("rcbResponsable");


            /////temporal encuesta
            RadTextBox IdCliente = (RadTextBox)FormClases.FindControl("IdCliente");
            CheckBox Encuestado = (CheckBox)FormClases.FindControl("Encuestado");
            RadTextBox txtResponsable = (RadTextBox)FormClases.FindControl("txtResponsable");
            RadTextBox txtDomicilio = (RadTextBox)FormClases.FindControl("txtDomicilio");
            RadTextBox txtCiudad = (RadTextBox)FormClases.FindControl("txtCiudad");
            RadTextBox txtContacto = (RadTextBox)FormClases.FindControl("txtContacto");
            int unidad = Convert.ToInt32(((HiddenField)FormClases.FindControl("unidad")).Value);
            /////////// temporal encuesta



            HiddenField hflDatosExtra = (HiddenField)FormClases.FindControl("hflDatosExtra");
            DataSet u = new DataSet();
            DataSet c = new DataSet();
            SqlCommand sql = new SqlCommand();
            try
            {
                sql.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                sql.CommandTimeout = 300;
            }
            SqlDataAdapter parellenar = new SqlDataAdapter(sql);
            int act = 0;
            if (ActivaCheckBox.Checked)
            {
                act = 1;
            }
            else
            {
                act = 0;
            }
            sql.Connection = dao.con;
            
            


            /////////temp encuesta
            int enc;
            if (Encuestado.Checked)
            {
                enc = 1;
            }
            else
            {
                enc = 0;
            }
            /////////temp encuesta





            if (cod_act.Text == "")
            {
                try
                {
                    dao.open();



                    /////temp encuesta
                    if (clasificacion == 3) {
                        sql.CommandText = "INSERT INTO Unidades (Nombre,activa,datosextra)VALUES('" + Nombre.Text.Trim() + "','" + act + "','" + hflDatosExtra.Value + "'); Select top 1 codigo from unidades order by codigo desc";
                    unidad=(int)sql.ExecuteScalar();
                    }
                    /////////temp encuesta
                    
                    
                    sql.CommandText = "INSERT INTO Clases (Nombre,activa,datosextra,Clasificacion,Responsable)VALUES('" + Nombre.Text.Trim() + "','" + act + "','" + hflDatosExtra.Value + "'," + clasificacion + ", " + rcbResponsable.SelectedValue + "); SELECT top 1 Codigo from Clases order by Codigo desc";
                    int codigo = (int)sql.ExecuteScalar();
                    for (int i = 0; i < Rels.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = Rels.Tables[0].Rows[i];
                        Control control = Relaciones.FindControl("ctl" + (string)row[3]);
                        if (control is RadComboBox)
                        {
                            RadComboBox combo = (RadComboBox)control;
                            foreach (RadComboBoxItem item in combo.Items)
                            {
                                if (item.Selected)
                                {
                                    sql.CommandText = "INSERT RelacionesClases (Padre,Hijo) VALUES(" + item.Value + "," + codigo + ")";
                                    sql.ExecuteNonQuery();
                                    
                                    
                                    
                                    
                                    /////temp encuesta
                    if (clasificacion == 3) {
                        sql.CommandText = "UPDATE Unidades set Sitio=(select codigo from Sitios where Nombre='" + item.Text + "') where Codigo=" + unidad;
                    sql.ExecuteNonQuery();
                    }
                    /////////temp encuesta




                                }
                            }
                        }
                        else
                        {
                            RadTreeView tree = (RadTreeView)(control.Controls[0]);
                            foreach (RadTreeNode node in tree.GetAllNodes())
                            {
                                if (node.Checked)
                                {
                                    sql.CommandText = "INSERT RelacionesClases (Padre,Hijo) VALUES(" + node.Value + "," + codigo + ")";
                                    sql.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    sql.CommandText = "SELECT codigo,Nombre,activa FROM Clases WHERE Clasificacion=" + clasificacion + "Nombre='"
                                     + Nombre.Text.Trim() + "' and activa='"
                                     + act + "'";
                    parellenar.Fill(c);
                    dao.close();
                    if (c.Tables[0].Rows.Count > 0)
                    {



                        /////temp encuesta
                        if (encuesta && clasificacion == 3)
                        {
                            sql.CommandText = "INSERT INTO ciberencuesta.dbo.clienteids (idcliente,req_codigo,txtdescrip,esencuestado,txtresponsable,txtdomicilio,"
                                             + "txtciudad,txtcontacto)VALUES('" + IdCliente.Text.Trim() + "'," + unidad + ",'" + Nombre.Text.Trim() + "',"
                                             + enc + ",'" + txtResponsable.Text.Trim() + "','" + txtDomicilio.Text.Trim() + "','" + txtCiudad.Text.Trim() + "','" + txtContacto.Text.Trim() + "')";
                            dao.open();
                            sql.ExecuteNonQuery();
                            dao.close();
                        }
                        /////////temp encuesta




                        Response.Redirect("Clases.aspx?clasificacion=" + plural);
                    }
                    else
                    {
                        Response.Write("<script language=javascript>alert('Ocurrio un problema y no se pudo guardar la información');</script>");
                    }
                }
                catch
                {
                    Response.Write("<script language=javascript>alert('El nombre de usuario especificado ya esta registrado');</script>");
                }
            }
            else
            {
                try
                {
                    sql.CommandText = "UPDATE Clases Set Nombre='" + Nombre.Text.Trim() + "', activa='" + act + "', datosextra='" + hflDatosExtra.Value + "', Responsable=" + rcbResponsable.SelectedValue + " WHERE codigo=" + cod_act.Text + "; DELETE RelacionesClases Where Hijo=" + cod_act.Text;
                    dao.open();
                    sql.ExecuteNonQuery();
                    int codigo = Convert.ToInt32(cod_act.Text);
                    for (int i = 0; i < Rels.Tables[0].Rows.Count; i++)
                    {
                        DataRow row = Rels.Tables[0].Rows[i];
                        Control control = Relaciones.FindControl("ctl" + (string)row[3]);
                        if (control is RadComboBox)
                        {
                            RadComboBox combo = (RadComboBox)control;
                            foreach (RadComboBoxItem item in combo.Items)
                            {
                                if (item.Selected)
                                {
                                    sql.CommandText = "INSERT RelacionesClases (Padre,Hijo) VALUES(" + item.Value + "," + codigo + ")";
                                    string nombre = item.Text;
                                    nombre = nombre + "-";
                                    sql.ExecuteNonQuery();
                                    
                                    
                                    
                                    
                                    /////temp encuesta
                    if (clasificacion == 3) {
                        sql.CommandText = "UPDATE Unidades set Sitio=(select codigo from Sitios where Nombre='" + item.Text + "') where codigo=" + unidad;
                    sql.ExecuteNonQuery();
                    }
                    /////////temp encuesta




                                
                                }
                            }
                        }
                        else
                        {
                            RadTreeView tree = (RadTreeView)(control.Controls[0]);
                            foreach (RadTreeNode node in tree.GetAllNodes())
                            {
                                if (node.Checked)
                                {
                                    sql.CommandText = "INSERT RelacionesClases (Padre,Hijo) VALUES(" + node.Value + "," + codigo + ")";
                                    sql.ExecuteNonQuery();
                                }
                            }
                        }
                        }
                    dao.close();





                    //////////temp encuesta
                    if (clasificacion==3)
                    sql.CommandText = "UPDATE Unidades Set Nombre='" + Nombre.Text.Trim() + "', activa='" + act + "', datosextra='" + hflDatosExtra.Value + "' WHERE codigo=" + unidad;
                    dao.open();
                    sql.ExecuteNonQuery();
                    dao.close();
                    if (encuesta)
                    {
                        sql.CommandText = "UPDATE ciberencuesta.dbo.clienteids Set idCliente='" + IdCliente.Text.Trim() + "', txtdescrip='" + Nombre.Text.Trim() + "', esencuestado=" + enc
                                         + ", txtresponsable='" + txtResponsable.Text.Trim() + "', txtdomicilio='" + txtDomicilio.Text.Trim() + "', txtciudad='"
                                         + txtCiudad.Text.Trim() + "', txtContacto='" + txtContacto.Text.Trim() + "' WHERE req_codigo=" + unidad;
                        dao.open();
                        sql.ExecuteNonQuery();
                        dao.close();
                    }
                    ////temp encuesta



                    Response.Redirect("Clases.aspx?clasificacion=" + plural);
                }
                catch
                {
                    Response.Write("<script language=javascript>alert('El nombre de usuario especificado ya esta registrado');</script>");
                }
            }
            nr = false;
        }

        protected void rcbClases_IndexChanged(object sender, EventArgs e)
        {
            FormClases.PageIndex = rcbClases.SelectedIndex;
            FormClases.Page.DataBind();
            selecciona();
        }

        protected void cbxSoloActivos_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxSoloActivos.Checked)
                SqlClases.SelectCommand = "SELECT c.codigo,c.nombre,c.activa,c.datosextra" +






                ////////temp
                ",'' as idcliente,CAST(0 As bit) As encuestado,'' as txtresponsable,'' as txtdomicilio,'' as txtciudad,'' as txtcontacto, isnull(u.Codigo,0) as unidad " +
                    //////temp





                " FROM [Clases] c " +
                


                
                ////////temp
                "left join unidades u on c.nombre=u.nombre and c.clasificacion=3 " +
                ////////temp
                
                
                
                "where clasificacion=" + clasificacion + " activa=1 ORDER by c.Nombre";
            else
                SqlClases.SelectCommand = "SELECT c.codigo,c.nombre,c.activa,c.datosextra" +






                ////////temp
                ",'' as idcliente,CAST(0 As bit) As encuestado,'' as txtresponsable,'' as txtdomicilio,'' as txtciudad,'' as txtcontacto, isnull(u.Codigo,0) as unidad " +
                    //////temp





                " FROM [Clases] c " +
                


                
                ////////temp
                "left join unidades u on c.nombre=u.nombre and c.clasificacion=3 " +
                ////////temp
                
                
                
                "where clasificacion=" + clasificacion + " ORDER by c.Nombre";
            



            //////temp encuesta
            if (encuesta && clasificacion==3)
            {
                if (cbxSoloActivos.Checked)
                    SqlClases.SelectCommand = "SELECT c.codigo,c.nombre,c.activa,c.datosextra,idcliente,CAST(esencuestado As bit) As encuestado,txtresponsable,txtdomicilio,txtciudad,txtcontacto, isnull(u.Codigo,0) as unidad FROM [Clases] c join unidades u on c.nombre=u.nombre and c.clasificacion=3 INNER JOIN ciberencuesta.dbo.clienteids ON u.codigo=req_codigo where c.activa=1 ORDER by c.Nombre";
                else
                    SqlClases.SelectCommand = "SELECT c.codigo,c.nombre,c.activa,c.datosextra,idcliente,CAST(esencuestado As bit) As encuestado,txtresponsable,txtdomicilio,txtciudad,txtcontacto, isnull(u.Codigo,0) as unidad FROM [Clases] c join unidades u on c.nombre=u.nombre and c.clasificacion=3 INNER JOIN ciberencuesta.dbo.clienteids ON u.codigo=req_codigo ORDER by c.Nombre";
            }
                ////////temp



           rcbClases.DataBind();
            FormClases.DataBind();
        }
    }
}