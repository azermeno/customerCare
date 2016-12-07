using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CustomerCare
{
    public partial class Unidades2 : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "Unidades";
            Controles = new string[] { "Grid" };
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void RadGrid1_ItemUpdated(object source, Telerik.Web.UI.GridUpdatedEventArgs e)
        {
            string[] item = getItemName(e.Item.OwnerTableView.Name);
            string field = getFieldName(e.Item.OwnerTableView.Name);
            if (e.Exception != null)
            {
                e.KeepInEditMode = true;
                e.ExceptionHandled = true;
                DisplayMessage(item[0] + " " + e.Item[field].Text + " no se puede guardar. Error: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(item[1] + " " + e.Item[field].Text + " modificad" + item[2]);
            }
        }

        protected void RadGrid1_ItemInserted(object source, GridInsertedEventArgs e)
        {
            string[] item = getItemName(e.Item.OwnerTableView.Name);
            if (e.Exception != null)
            {
                e.ExceptionHandled = true;
                DisplayMessage(item[0] + " no se puede agregar. Error: " + e.Exception.Message);
            }
            else
            {
                DisplayMessage(item[1] + " agregad" + item[2]);
            }
        }

        protected void RadGrid1_InsertCommand(object source, GridCommandEventArgs e)
        {
            if ("Sitios".Equals(e.Item.OwnerTableView.Name))
            {
                GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                sdsSitios.InsertParameters["Zona"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["Codigo"].ToString();
            }
            else if ("Unidades".Equals(e.Item.OwnerTableView.Name))
            {
                GridDataItem parentItem = (GridDataItem)e.Item.OwnerTableView.ParentItem;
                sdsUnidades.InsertParameters["Sitio"].DefaultValue = parentItem.OwnerTableView.DataKeyValues[parentItem.ItemIndex]["Codigo"].ToString();
            }
        }

        protected void RadGrid1_ItemCreated(object sender, GridItemEventArgs e)
        {
            GridEditableItem item = e.Item as GridEditableItem;
            if (item != null && item.IsInEditMode && item.ItemIndex != -1)
            {
                if (item.OwnerTableView.Name == ((RadGrid)sender).MasterTableView.Name)
                {
                    (item.EditManager.GetColumnEditor("Zona").ContainerControl.Controls[0] as TextBox).Enabled = false;
                }
                else if (item.OwnerTableView.Name == "Details")
                {
                    (item.EditManager.GetColumnEditor("Sitio").ContainerControl.Controls[0] as TextBox).Enabled = false;
                }
            }
        }

        private String[] getItemName(string tableName)
        {
            switch (tableName)
            {
                case ("Zonas"):
                    {
                        return new string[3] {"La zona","Zona","a"};
                    }
                case ("Unidades"):
                    {
                        return new string[3] { "La unidad", "Unidad" , "a"};
                    }
                case ("Sitios"):
                    {
                        return new string[3] { "El sitio", "Sitio" , "o"};
                    }
                default: return new string[3];
            }
        }

        private String getFieldName(string tableName)
        {
            return "Codigo";
        }

        private void DisplayMessage(string text)
        {
            RadGrid1.Controls.Add(new LiteralControl(string.Format("<span style='color:red'>{0}</span>", text)));
        }
    }
}