using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CustomerCare
{
    public partial class NuevoFormato : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadSkinManager1.Skin = ConfigurationManager.AppSettings["Skin"];
        }

        protected void RadToolBar1_ButtonClick(object sender, Telerik.Web.UI.RadToolBarEventArgs e)
        {

        }
    }
}