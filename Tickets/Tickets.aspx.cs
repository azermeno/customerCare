using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Telerik.Web.UI;

namespace CustomerCare
{     
    public partial class Tickets : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "Tickets";
            Controles = new string[] { "Splitter" };
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //RadSkinManager1.Skin = ConfigurationManager.AppSettings["Skin"];
            //if (!(Request.QueryString["hijas"] == null))
            //    RadPane1.ContentUrl = "TablaTickets.aspx?hijas=" + Request.QueryString["hijas"];
        }
    }
}