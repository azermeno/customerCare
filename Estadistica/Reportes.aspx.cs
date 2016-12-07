using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.IO;
using CustomerCare;

namespace CustomerCare
{
    public partial class Reportes : Extra
    {
        //private bool isExport = false;

        private bool permisos = false;

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

        private void ActualizaParametros()
        {
            RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
            if (ListaConsultas.Text != "")
            {
                //Consulta.Text = Consulta.Text + Tablas.SelectedValue;
                TParametros.Rows.Clear();
                if (SeparaCount(Consulta.Text, "#") > 0)
                {
                    List<string> lista_param=new List<string>();
                    string consulta = Consulta.Text;
                    int pos = 0;
                    //Table tp = new Table();
                    int nom = 1;
                    while (pos < SeparaCount(Consulta.Text, "#"))
                    {
                        if (pos % 2 != 0)
                        {
                            consulta += " ";
                            TableRow fila = new TableRow();
                            TableCell campo1 = new TableCell();
                            TableCell campo2 = new TableCell();
                            TableCell campo3 = new TableCell();
                            pos++;
                            //campo1.Text = Consulta.Text.Substring(Consulta.Text.IndexOf("@", pos + 1) + 1, (Consulta.Text.IndexOf(" ", Consulta.Text.IndexOf("@", pos + 1)) - Consulta.Text.IndexOf("@", pos + 1)));
                            //int len = 0;
                            //while (len < pos)
                            //{
                            //    len = consulta.IndexOf(" ", len + 1);
                            //}
                            Label nparam = new Label();
                            nparam.ID = "nparam" + nom.ToString();
                            nparam.Text = separa(Consulta.Text, "#", pos).Trim();
                            campo1.Controls.Add(nparam);
                            RadTextBox valor = new RadTextBox();
                            valor.ID = "valor" + nom.ToString();
                            campo2.Controls.Add(valor);
                            DropDownList tipo = new DropDownList();
                            tipo.ID = "tipo" + nom.ToString();
                            tipo.Items.Add("Numerico");
                            tipo.Items.Add("Texto");
                            tipo.Items.Add("Fecha");
                            tipo.Items.Add("Logico");
                            campo3.Controls.Add(tipo);
                            fila.Cells.Add(campo1);
                            fila.Cells.Add(campo2);
                            fila.Cells.Add(campo3);
                            if (!lista_param.Contains(nparam.Text.Trim().ToUpper()))
                            {
                                TParametros.Rows.Add(fila);
                                lista_param.Add(nparam.Text.Trim().ToUpper());
                            }
                            nom += 1;
                            consulta = consulta.Trim();
                        }
                        else
                        {
                            pos++;
                        }
                    }
                    //Panel1.Controls.Add(tp);
                }
            }
            //else
            //{
            //    //Consulta2.Text = Consulta2.Text + Tablas.SelectedValue;
            //    TParametros.Controls.Clear();
            //    if (Consulta2.Text.IndexOf("@") > 0)
            //    {
            //        List<string> lista_param = new List<string>();
            //        string consulta = Consulta2.Text;
            //        int pos = 0;
            //        //Table tp = new Table();
            //        //tp.ID = "TParametros";
            //        int nom = 1;
            //        while (pos < consulta.LastIndexOf("@"))
            //        {
            //            consulta += " ";
            //            TableRow fila = new TableRow();
            //            TableCell campo1 = new TableCell();
            //            TableCell campo2 = new TableCell();
            //            TableCell campo3 = new TableCell();
            //            pos = consulta.IndexOf("@", pos + 1);
            //            //campo1.Text = Consulta.Text.Substring(Consulta.Text.IndexOf("@", pos + 1) + 1, (Consulta.Text.IndexOf(" ", Consulta.Text.IndexOf("@", pos + 1)) - Consulta.Text.IndexOf("@", pos + 1)));
            //            int len = 0;
            //            while (len < pos)
            //            {
            //                len = consulta.IndexOf(" ", len + 1);
            //            }
            //            Label nparam = new Label();
            //            nparam.ID = "nparam" + nom.ToString();
            //            nparam.Text = consulta.Substring(pos + 1, len - pos);
            //            campo1.Controls.Add(nparam);
            //            RadTextBox valor = new RadTextBox();
            //            valor.ID = "valor" + nom.ToString();
            //            campo2.Controls.Add(valor);
            //            DropDownList tipo = new DropDownList();
            //            tipo.ID = "tipo" + nom.ToString();
            //            tipo.Items.Add("Numerico");
            //            tipo.Items.Add("Texto");
            //            tipo.Items.Add("Fecha");
            //            tipo.Items.Add("Logico");
            //            campo3.Controls.Add(tipo);
            //            fila.Cells.Add(campo1);
            //            fila.Cells.Add(campo2);
            //            fila.Cells.Add(campo3);
            //            if (!lista_param.Contains(nparam.Text.Trim().ToUpper()))
            //            {
            //                TParametros.Rows.Add(fila);
            //                lista_param.Add(nparam.Text.Trim().ToUpper());
            //            }
            //            nom += 1;
            //            consulta = consulta.Trim();
            //        }
            //        //Panel1.Controls.Add(tp);
            //    }
            //}
        }

        bool editor = true;

        protected void Page_Load(object sender, EventArgs e)
        {
            btnExcel.ImageUrl = ImagesFolder + "Excel.png";
            btnWord.ImageUrl = ImagesFolder + "Word.jpg";
            btnPDF.ImageUrl = ImagesFolder + "PDF.jpg";
            btnCSV.ImageUrl = ImagesFolder + "CSV.jpg";
            if (!IsPostBack)
            {
                TParametros.Visible = false;
                ControlesReporte.Visible = false;
            }
            else
            {
                TParametros.Visible = true;
                ControlesReporte.Visible = true;
            }
                //func = new Funciones();
                if (Request.Cookies.Get("DatosUsuario") == null || (Request.Cookies["DatosUsuario"]["Recordarme"] == null && Session["wy"] == null))
                    Response.Redirect("index.aspx");
                if (Session["wy"] == null)
                    Session.Add("wy", Request.Cookies["DatosUsuario"]["wy"]);
                wy = Session["wy"].ToString();
                string usu;
                //dao.open(); //dao.open();
                wy = Session["wy"].ToString();
                usu = Utilidades.Crip(wy);
                SqlConnection conex = new SqlConnection();
                DataSet u = new DataSet();
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
                sql.Connection = conex;
                conex.ConnectionString = Datos.ConnectionString;
                sql.CommandText = "SELECT permisos FROM usuarios WHERE codigo=" + usu;
                conex.Open();
                parellenar.Fill(u);
                conex.Close();
                if (!u.Tables[0].Rows[0][0].ToString().Contains("23"))
                {
                    editor = false;
                    ListaConsultas.Width = 500;
                    Modificar.Style.Add("Display", "none");
                    Eliminar.Style.Add("Display", "none");
                    rcbNombre.Style.Add("Display", "none");
                    Guardar.Style.Add("Display", "none");
                    //Consulta2.Enabled = false;
                    //Consulta2.ReadOnly = true;
                    RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
                    //Consulta.Enabled = false;
                    Consulta.Style.Add("Display", "none");
                    Tablas.Style.Add("Display", "none");
                    Campos.Style.Add("Display", "none");
                    TInsertar.Style.Add("Display", "none");
                    CInsertar.Style.Add("Display", "none");
                    Buscar.Style.Add("Display", "none");
                    permisos = false;
                    ListaConsultas.Text = "Seleccione un reporte...";
                    Label1.Text = "Reporte:";
                    Consulta2.Width = 1;
                    Consulta.Width = 1;
                }
            ActualizaParametros();
            Eliminar.Attributes.Add("onclick","return confirm('¿Realmente desea eliminar la consulta?');");
        }

        protected void Ejecutar_Click(object sender, EventArgs e)
        {
            string setencia="";
            Resultado.MasterTableView.SortExpressions.Clear();
            RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
            if (ListaConsultas.Text != "")
            {
                setencia = Consulta.Text;
            }
            else
            {
                setencia = Consulta2.Text;
            }
            if (TParametros.Rows.Count > 0)
            {
                for (int x = 0; x < TParametros.Rows.Count; x++)
                {
                    RadTextBox valor = (RadTextBox)TParametros.FindControl("valor" + (x + 1).ToString());
                    Label nombre = (Label)TParametros.FindControl("nparam" + (x + 1).ToString());
                    DropDownList tipo = (DropDownList)TParametros.FindControl("tipo" + (x + 1).ToString());
                    string viejo = "#" + nombre.Text.Trim() + "#";
                    string nuevo = "";
                    if (tipo.Text.Trim() == "Numerico" || tipo.Text.Trim() == "Logico")
                    {
                        nuevo = valor.Text + " ";
                    }
                    else
                    {
                        nuevo = "'" + valor.Text + "' ";
                    }
                    setencia = setencia.Replace(viejo, nuevo);
                }
            }
            Datos.SelectCommand = setencia;
            try
            {
                Resultado.DataBind();
            }
            catch
            {
                Response.Write("<script language='JavaScript'>alert('La sintaxis de la consulta es incorrecta, favor de verificar.', 'geoflotante', '');</script>");
            }            
        }

        protected void Modificar_Click(object sender, EventArgs e)
        {
            if (ListaConsultas.Text != "")
            {
                SqlConnection conex = new SqlConnection();
                DataSet u = new DataSet();
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
                conex.ConnectionString = Datos.ConnectionString;
                sql.Connection = conex;
                RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
                sql.CommandText = "UPDATE consultas SET consulta='" + Consulta.Text.Replace("'","''") + "' WHERE id=" + ListaConsultas.SelectedValue;
                conex.Open();
                sql.ExecuteNonQuery();
                conex.Close();
                FormView1.DataBind();
                ActualizaParametros();
            }
            else
            {
                Response.Write("<script language='JavaScript'>alert('No hay registro para modificar.', 'geoflotante', '');</script>");
            }
        }

        protected void Eliminar_Click(object sender, EventArgs e)
        {
            if (ListaConsultas.Text != "")
            {
                SqlConnection conex = new SqlConnection();
                DataSet u = new DataSet();
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
                conex.ConnectionString = Datos.ConnectionString;
                sql.Connection = conex;
                RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
                sql.CommandText = "DELETE FROM consultas WHERE id=" + ListaConsultas.SelectedValue;
                conex.Open();
                sql.ExecuteNonQuery();
                conex.Close();
                ListaConsultas.DataBind();
                FormView1.DataBind();
            }
            else
            {
                Response.Write("<script language='JavaScript'>alert('No hay registro para eliminar.', 'geoflotante', '');</script>");
            }
        }

        protected void Excel_Click(object sender, ImageClickEventArgs e)
        {
            //Excel._Application ExcelApp = new Excel.Application();
            //ExcelApp.Application.Workbooks.Add(Type.Missing);
            //ExcelApp.Columns.ColumnWidth = 12;
            //for (int i = 0; i < Resultado.Rows.Count; i++)
            //{
            //    GridViewRow Fila = Resultado.Rows[i];
            //    for (int j = 0; j < Fila.Cells.Count; j++)
            //    {
            //        ExcelApp.Cells[i + 1, j + 1] = Fila.Cells[j].Text;
            //    }
            //}
            //string nombre;
            //nombre = @System.Configuration.ConfigurationManager.AppSettings["path_excel"] + "\\Reporte_" + System.DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".xlsx";
            //ExcelApp.ActiveWorkbook.SaveCopyAs(nombre);
            //ExcelApp.ActiveWorkbook.Saved = true;
            //ExcelApp.Quit();
            //Response.Write("<script language='JavaScript'>window.open('" + @System.Configuration.ConfigurationManager.AppSettings["url_excel"] + "/Reporte_" + System.DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss") + ".xlsx" + "', 'geoflotante', '');</script>");
            Resultado.MasterTableView.ExportToExcel();
        }

        protected void Guardar_Click(object sender, EventArgs e)
        {
            if (rcbNombre.Text != "")
            {
                SqlConnection conex = new SqlConnection();
                DataSet u = new DataSet();
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
                conex.ConnectionString = Datos.ConnectionString;
                sql.Connection = conex;
                RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
                if (ListaConsultas.Text != "")
                {
                    sql.CommandText = "INSERT INTO consultas (nombre,consulta) VALUES ('" + rcbNombre.Text + "','" + Consulta.Text.Replace("'","''") + "')";
                }
                else
                {
                    sql.CommandText = "INSERT INTO consultas (nombre,consulta) VALUES ('" + rcbNombre.Text + "','" + Consulta2.Text.Replace("'","''") + "')";
                }
                conex.Open();
                sql.ExecuteNonQuery();
                conex.Close();
                rcbNombre.Text = "";
                ListaConsultas.DataBind();
                FormView1.DataBind();
            }
            else
            {
                Response.Write("<script language='JavaScript'>alert('Escriba primero el nombre que le desea dar a la consulta', 'geoflotante', '');</script>");
            }
        }

        protected void Consulta2_PreRender(object sender, EventArgs e)
        {
            if (ListaConsultas.Text != "")
            {
                Consulta2.Display = false;
            }
            else
            {
                Consulta2.Display = true;
            }
        }

        protected void Modificar_PreRender(object sender, EventArgs e)
        {
            if (permisos)
            {
                if (ListaConsultas.Text != "")
                {
                    Modificar.Style.Add("Display", "inline");
                }
                else
                {
                    Modificar.Style.Add("Display", "none");
                }
            }
        }

        protected void Eliminar_PreRender(object sender, EventArgs e)
        {
            if (permisos)
            {
                if (ListaConsultas.Text != "")
                {
                    Eliminar.Style.Add("Display", "inline");
                }
                else
                {
                    Eliminar.Style.Add("Display", "none");
                }
            }
        }
        
        protected void btnPDF_Click(object sender, ImageClickEventArgs e)
        {
            Resultado.MasterTableView.ExportToPdf();
        }

        protected void btnWord_Click(object sender, ImageClickEventArgs e)
        {
            Resultado.MasterTableView.ExportToWord();
        }

        protected void btnCSV_Click(object sender, ImageClickEventArgs e)
        {
            Resultado.MasterTableView.ExportToCSV();
        }

        protected void ListaConsultas_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            FormView1.PageIndex = ListaConsultas.SelectedIndex;
            FormView1.DataBind();
            ActualizaParametros();
            if (!editor)
            {
                ListaConsultas.Width = 500;
                Modificar.Style.Add("Display", "none");
                Eliminar.Style.Add("Display", "none");
                rcbNombre.Style.Add("Display", "none");
                Guardar.Style.Add("Display", "none");
                //Consulta2.Enabled = false;
                //Consulta2.ReadOnly = true;
                RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
                //Consulta.Enabled = false;
                Consulta.Style.Add("Display", "none");
                Tablas.Style.Add("Display", "none");
                Campos.Style.Add("Display", "none");
                TInsertar.Style.Add("Display", "none");
                CInsertar.Style.Add("Display", "none");
                Buscar.Style.Add("Display", "none");
                permisos = false;
                ListaConsultas.Text = "Seleccione un reporte...";
                Label1.Text = "Reporte:";
                Consulta2.Width = 1;
                Consulta.Width = 1;
            }
        }

        protected void Resultado_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
            if (ListaConsultas.Text != "")
            {
                Datos.SelectCommand = Consulta.Text;
            }
            else
            {
                Datos.SelectCommand = Consulta2.Text;
            }
            try
            {
                Resultado.DataBind();
            }
            catch
            {
                Response.Write("<script language='JavaScript'>alert('La sintaxis de la consulta es incorrecta, favor de verificar.', 'geoflotante', '');</script>");
            }     
        }

        protected void Resultado_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            //string gridSortString = this.Resultado.MasterTableView.SortExpressions.GetSortString();
            //string text = "Grid sort expression: " + gridSortString;
            //RadAjaxManager1.ResponseScripts.Add(string.Format("document.getElementById('{0}').innerHTML = '" + text + "';", this.SortLabel.ClientID));

            //DataSourceSelectArguments args = new DataSourceSelectArguments(gridSortString);
            //Resultado.DataSource = this.Datos.Select(args);   
            
        }

        protected void Tablas_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlCampos.FilterExpression = "TABLE_NAME='" + Tablas.SelectedValue + "'";
            Campos.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
            if (ListaConsultas.Text != "")
            {
                Consulta.Text = Consulta.Text + Tablas.SelectedValue;
                
            }
            else
            {
                Consulta2.Text = Consulta2.Text + Tablas.SelectedValue;
                
            }
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            RadTextBox Consulta = (RadTextBox)FormView1.FindControl("Consulta");
            if (ListaConsultas.Text != "")
            {
                Consulta.Text = Consulta.Text + Campos.SelectedValue;

            }
            else
            {
                Consulta2.Text = Consulta2.Text + Campos.SelectedValue;

            }
        }

        protected void Buscar_Click(object sender, EventArgs e)
        {
            ActualizaParametros();
        }

        protected void Orden_CheckedChanged(object sender, EventArgs e)
        {
            Resultado.MasterTableView.AllowMultiColumnSorting = Orden.Checked;
        }
                
    }
}