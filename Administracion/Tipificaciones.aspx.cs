using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace CustomerCare
{
    public partial class Tipificaciones : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "Tipificaciones";
            Controles = new string[] {"TreeList"};
        }

        string ConnString = ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString;

        protected void RadTreeList1_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            RadTreeList1.DataSource = GetDataTable("SELECT Codigo, Descripcion, Padre, Hoja FROM Tipificaciones WHERE Activo=1 AND Padre is NULL ORDER BY Descripcion");
        }

        protected void RadTreeList1_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
                TreeListDataItem item = (TreeListDataItem)e.Item;
                if (item["Hoja"].Text=="True")
                    item.FindControl("ExpandCollapseButton").Visible = false;
            }
        }
        
        protected void RadTreeList1_ChildItemsDataBind(object sender, TreeListChildItemsDataBindEventArgs e)
        {
            e.ChildItemsDataSource = GetDataTable("SELECT Codigo, Descripcion, Padre, Hoja FROM Tipificaciones WHERE Activo=1 AND Padre = " + e.ParentDataKeyValues["Codigo"].ToString() + " ORDER BY Descripcion");
        }

        private DataTable GetDataTable(string query)
        {
            SqlConnection conn = new SqlConnection(ConnString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand SqlComando = new SqlCommand(query, conn);
            try
            {
                SqlComando.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                SqlComando.CommandTimeout = 300;
            }
            adapter.SelectCommand = SqlComando;
            DataTable myDataTable = new DataTable();

            conn.Open();
            try
            {
                adapter.Fill(myDataTable);
            }
            finally
            {
                conn.Close();
            }

            return myDataTable;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string skin = ConfigurationManager.AppSettings["Skin"];
            rsmTipi.Skin = skin;
        }

        protected void RadTreeList1_InsertCommand(object sender, TreeListCommandEventArgs e)
        {
            Hashtable table = new Hashtable();
            TreeListEditableItem item = e.Item as TreeListEditableItem;
            item.ExtractValues(table);

            ConvertEmptyValuesToDBNull(table);

            if (!table.ContainsKey("Padre"))
            {
                table["Padre"] = DBNull.Value;
                table["Nivel"] = 1;
            }

            if (table["Descripcion"] != DBNull.Value)
            {
                string commandText = "INSERT INTO [Tipificaciones] ([Descripcion], [Nivel], [Hoja], [Padre], [Activo]) VALUES (@Descripcion, ISNULL((SELECT Nivel FROM Tipificaciones WHERE Codigo=@Padre),0) + 1, 1, @Padre, 1); UPDATE Tipificaciones SET Hoja=0 WHERE Codigo=@Padre";
                ExecuteNonQuery(commandText, table);
            }
        }

        protected void RadTreeList1_UpdateCommand(object sender, TreeListCommandEventArgs e)
        {
            Hashtable table = new Hashtable();
            TreeListEditableItem item = e.Item as TreeListEditableItem;
            item.ExtractValues(table);

            ConvertEmptyValuesToDBNull(table);

            if (table["Descripcion"] != DBNull.Value)
            {
                string commandText = "UPDATE [Tipificaciones] SET [Descripcion] = @Descripcion, [Padre] = @Padre WHERE [Codigo] = @Codigo";
                ExecuteNonQuery(commandText, table);
            }
        }

        protected void RadTreeList1_DeleteCommand(object sender, TreeListCommandEventArgs e)
        {
            TreeListDataItem item = e.Item as TreeListDataItem;
            if (item["Hoja"].Text == "False")
            {
                RadAjaxPanel1.Alert("¡No se puede eliminar, tiene subtipificaciones!");
                e.Canceled = true;
                return;
            }
            string dataKeyValue = item.GetDataKeyValue("Codigo").ToString();
            string commandText = "UPDATE Tipificaciones SET Activo=0 WHERE Codigo = @Codigo";
            Hashtable table = new Hashtable();
            table.Add("Codigo", dataKeyValue);
            ExecuteNonQuery(commandText, table);
        }

        private void ConvertEmptyValuesToDBNull(Hashtable values)
        {
            List<object> keysToDbNull = new List<object>();

            foreach (DictionaryEntry entry in values)
            {
                if (entry.Value == null || (entry.Value is String && String.IsNullOrEmpty((String)entry.Value)))
                {
                    keysToDbNull.Add(entry.Key);
                }
            }

            foreach (object key in keysToDbNull)
            {
                values[key] = DBNull.Value;
            }
        }

        private void ExecuteNonQuery(string commandText, Hashtable parameters)
        {
            SqlConnection conn = new SqlConnection(ConnString);
            SqlCommand command = new SqlCommand(commandText, conn);
            try
            {
                command.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                command.CommandTimeout = 300;
            }
            foreach (DictionaryEntry entry in parameters)
            {
                command.Parameters.AddWithValue(entry.Key.ToString(), entry.Value);
            }

            conn.Open();
            try
            {
                command.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}