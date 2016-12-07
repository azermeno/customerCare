using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using Telerik.Web.UI;
using System.Collections;
using Newtonsoft.Json;

namespace CustomerCare
{
    public class Datos
    {
         public DataTable Contenido { get; private set;}
        private DataTable creaContenido() {
            DataTable result = new DataTable();
            result.Columns.Add("Codigo", typeof(Int32));
            result.Columns.Add("Padre", typeof(Int32));
            result.Columns.Add("Nombre", typeof(String));
            result.PrimaryKey = new DataColumn[] {result.Columns["Codigo"]};
            result.Columns["Padre"].AllowDBNull = true;
            result.Columns["Nombre"].AllowDBNull = false;
            return result;
        }
        private DataTable JsonATabla(string JSON)
        {
            DataTable result = creaContenido();
            string buf = JSON;
            List<int> padre = new List<int>();
            int codigo = 0;
            while (buf.Length > 0)
            {
                switch (buf[0])
                {
                    case '{':
                        padre.Add(codigo);
                        buf = buf.Substring(1);
                        break;
                    case '"':
                        codigo++;
                        buf = buf.Substring(1);
                        int comilla = buf.IndexOf('"');
                        if (comilla > 0)
                        {
                            DataRow row=result.NewRow();
                                //result.Insertar(codigo, padre.Count == 1 ? 0 : padre[padre.Count - 1], buf.Substring(0, buf.IndexOf('"')));
                            row["Codigo"] = codigo;
                            if (padre.Count == 1)
                                row["Padre"] = DBNull.Value;
                            else
                                row["Padre"] = padre[padre.Count - 1];
                            row["Nombre"] = buf.Substring(0, buf.IndexOf('"'));
                            buf = buf.Substring(buf.IndexOf('"') + 1);
                            result.Rows.Add(row);
                        }
                        else
                            throw new ArgumentException("Sintaxis JSON de datos extras inválida: los nombres de los campos deben ser no vacíos", "JSON");
                        break;
                    case ',':
                        buf = buf.Substring(1);
                        break;
                    case ':':
                        if (buf[1] == '{')
                            buf = buf.Substring(1);
                        else
                            buf = buf.Substring(3);
                        break;
                    case '}':
                        padre.RemoveAt(padre.Count - 1);
                        buf = buf.Substring(1);
                        break;
                    default:
                        throw new ArgumentException("Sintaxis JSON de datos extras inválida", "JSON");
                }
            }
            return result;
        }
        public Datos()
        {
            this.Contenido = creaContenido();
        }
        public Datos(String JSON)
        {
            this.Contenido = JsonATabla(JSON);
        }
        public void Insertar(object Padre, String Nombre)
        {
            int padre;
            if (Padre is DBNull)
                padre = 0;
            else if (Padre is int)
                padre = (int)Padre;
            else
                throw new ArgumentException("Sólo se aceptan los tipos Int32 y DBNull para el parámetro", "Padre");
            DataTable nuevo = creaContenido();
            DataRowCollection rows = this.Contenido.Rows;
            for (int i = 0; i < this.Contenido.Rows.Count; i++)
            {
                DataRow newrow = nuevo.NewRow();
                newrow["Nombre"] = rows[i]["Nombre"];
                if (i < padre)
                {
                    newrow["Padre"] = rows[i]["Padre"];
                    newrow["Codigo"] = rows[i]["Codigo"];
                }
                else
                {
                    int padrenuevo = rows[i]["Padre"] is DBNull ? 0 : (int)rows[i]["Padre"];
                    newrow["Codigo"] = i + 2;
                    if (padrenuevo == 0)
                        newrow["Padre"] = DBNull.Value;
                    else if (padrenuevo <= padre)
                            newrow["Padre"] = padrenuevo;
                    else
                            newrow["Padre"] = padrenuevo + 1;
                }
                nuevo.Rows.Add(newrow);
            }
            DataRow insercion = nuevo.NewRow();
            insercion["Codigo"] = padre + 1;
            if (padre == 0)
                insercion["Padre"] = DBNull.Value;
            else
                insercion["Padre"] = padre;
            insercion["Nombre"] = Nombre;
            nuevo.Rows.InsertAt(insercion,padre);
            Contenido = nuevo;
        }
        public void Borrar(Int32 Codigo)
        {
            Contenido.Rows.Remove(Contenido.Rows.Find(Codigo));
            int cod;
            foreach (DataRow row in Contenido.Rows)
            {
                cod = (int)row["Codigo"];
                if (cod > Codigo)
                    row["Codigo"] = cod - 1;
            }
        }
        public string AJson()
        {
            string result = "";
            List<int> padre = new List<int> { 0 };
            int nivel = 0;
            foreach (DataRow row in Contenido.Rows)
            {
                int pr = row["Padre"] == DBNull.Value ? 0 : (int)row["Padre"];
                if (pr > padre[nivel])
                {
                    nivel++;
                    padre.Add(pr);
                    result += ":{";
                }
                else if (pr < padre[nivel])
                {
                    for (int i = 1; i <= padre.Count; i++)
                    {
                        nivel--;
                        if (pr == padre[nivel])
                        {
                            result += ":\"\"";
                            result += new String('}', i) + ",";
                            padre.RemoveRange(nivel + 1, i);
                            break;
                        }
                    }
                }
                else
                    result += ":\"\",";
                result += "\"" + ((string)row["Nombre"]) + "\"";
            }
            return "{" + (result == "" ? result : result.Substring(4) + ":\"\"" + (new String('}', padre.Count - 1))) + "}";
        }
    }

    public partial class DatosExtra : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "DatosExtra";
            Controles = new string[] { "TreeList", "ToolBar", "Input"};
        }

        
        private void creacontroles()
        {
            placeholder.Controls.Clear();
            SqlCommand query = new SqlCommand("SELECT Id, Campos FROM CamposExtra", dao.con);
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
            while (reader.Read())
            {
                Literal br = new Literal();
                br.Text = "<br/>";
                placeholder.Controls.Add(br);
                RadTreeList rtl = new RadTreeList();
                rtl.ID = "rtl" + reader.GetString(0);
                rtl.DataKeyNames = new string[] { "Codigo" };
                rtl.ParentDataKeyNames = new string[] { "Padre" };
                rtl.InsertCommand += new EventHandler<TreeListCommandEventArgs>(Insertar);
                rtl.DeleteCommand += new EventHandler<TreeListCommandEventArgs>(Borrar);
                rtl.ItemDataBound += new EventHandler<TreeListItemDataBoundEventArgs>(ItemDataBound);
                rtl.AutoGenerateColumns = false;
                rtl.EditFormSettings.EditColumn.CancelText = "Cancelar";
                rtl.EditFormSettings.EditColumn.InsertText = "Guardar";
                TreeListBoundColumn nombre = new TreeListBoundColumn();
                nombre.DataField = "Nombre";
                nombre.HeaderText = reader.GetString(0);
                if (reader.GetString(0) == "Incidencias")
                    nombre.HeaderText = Ticket + "s";
                nombre.UniqueName = "Nombre";
                nombre.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                nombre.HeaderStyle.Width = Unit.Pixel(75);
                rtl.Columns.Add(nombre);
                TreeListEditCommandColumn agregar = new TreeListEditCommandColumn();
                agregar.ButtonType = TreeListButtonColumnType.ImageButton;
                agregar.UniqueName = "Agregar";
                agregar.ShowEditButton = false;
                agregar.HeaderStyle.Width = Unit.Pixel(20);
                agregar.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                agregar.AddRecordText = "Agregar";
                rtl.Columns.Add(agregar);
                TreeListButtonColumn borrar = new TreeListButtonColumn();
                borrar.UniqueName = "Borrar";
                borrar.Text = "Borrar";
                borrar.CommandName = "Delete";
                borrar.ButtonType = TreeListButtonColumnType.ImageButton;
                borrar.HeaderStyle.Width = Unit.Pixel(20);
                borrar.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                rtl.Columns.Add(borrar);
                TreeListBoundColumn codigo = new TreeListBoundColumn();
                codigo.Visible = false;
                codigo.DataField = "Codigo";
                codigo.UniqueName = "Codigo";
                codigo.ReadOnly = true;
                rtl.Columns.Add(codigo);
                TreeListBoundColumn padre = new TreeListBoundColumn();
                padre.Visible = false;
                padre.DataField = "Padre";
                padre.UniqueName = "Padre";
                padre.ReadOnly = true;
                rtl.Columns.Add(padre);
                placeholder.Controls.Add(rtl);
                sources[reader.GetString(0)] = new Datos(reader.GetString(1));
                rtl.DataSource = ((Datos)sources[reader.GetString(0)]).Contenido;
                rtl.DataBind();
                rtl.ExpandAllItems();
            }
            reader.Close();
            dao.close(); //dao.close();
        }

        private Hashtable sources = new Hashtable();

        protected void Page_Load(object sender, EventArgs e)
        {
            //func = new Funciones();
            form1.Controls.Add(new RadScriptManager());
            creacontroles();
        }

        protected void Guardar(object sender, EventArgs e)
        {
            SqlCommand query = new SqlCommand("Insert CamposExtra (id, Campos) VALUES ('" + ((RadTextBox)rtbPrincipal.Items[1].FindControl("rtbNombre")).Text + "', '{}')", dao.con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            dao.open(); //dao.open();
            try
            {
                query.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                Response.Write("<script type=\"text/javascript\">alert(\"El nombre ya existe\");</script>");
            }
            finally
            {
                dao.close(); //dao.close();
            }
            creacontroles();
        }

        protected void ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
                TreeListDataItem item = (TreeListDataItem)e.Item;
                Control ec = item.FindControl("ExpandCollapseButton");
                if (ec != null)
                    ec.Visible = false;
                if (item.HierarchyIndex.NestedLevel == 2)
                {
                    ec = item.FindControl("InsertButton_Agregar");
                    ec.Visible = false;
                }
            }
        }

        protected void Insertar(object sender, TreeListCommandEventArgs e)
        {
            Hashtable valores = new Hashtable();
            TreeListEditableItem item = e.Item as TreeListEditableItem;
            item.ExtractValues(valores);
            RadTreeList rtl = (RadTreeList)sender;
            if (!valores.ContainsKey("Padre"))
            {
                valores["Padre"] = DBNull.Value;
            }
            string id=rtl.ID.Substring(3);
            ((Datos)sources[id]).Insertar(valores["Padre"], (String)valores["Nombre"]);
            Guardar(id);
            rtl.DataSource = ((Datos)sources[id]).Contenido;
            rtl.DataBind();
            rtl.ExpandAllItems();
        }

        protected void Borrar(object sender, TreeListCommandEventArgs e)
        {
            TreeListDataItem item = (TreeListDataItem)e.Item;
            if (item.ChildItems.Count!=0)
            {
                e.Canceled = true;
                return;
            }
            RadTreeList rtl = (RadTreeList)sender;
            string id=rtl.ID.Substring(3);
            ((Datos)sources[id]).Borrar((Int32)(item).GetDataKeyValue("Codigo"));
            Guardar(id);
            rtl.DataSource = ((Datos)sources[id]).Contenido;
            rtl.DataBind();
            rtl.ExpandAllItems();
        }

        protected void Guardar(string id)
        {
            SqlCommand query = new SqlCommand("UPDATE CamposExtra SET Campos='" + ((Datos)sources[id]).AJson() + "' WHERE Id='" + id + "'", dao.con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            dao.open(); //dao.open();
            query.ExecuteNonQuery();
            dao.close(); //dao.close();
        }
    }
}