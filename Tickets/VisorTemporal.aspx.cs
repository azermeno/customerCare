using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerCare
{
    public partial class VisorTemporal : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "TablaTickets";
            Controles = new string[] { "Grid", "ToolBar", "TreeView", "ComboBox", "Calendar", "Input", "Dock" };
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
        }
    }
}