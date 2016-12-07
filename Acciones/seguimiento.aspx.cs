using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CustomerCare
{
    public partial class seguimiento : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            rtbObs.Skin = ConfigurationManager.AppSettings["Skin"];
            rblTipoEvento.Items[0].Text = ConfigurationManager.AppSettings["notaPrivada"];
            rblTipoEvento.Items[1].Text = ConfigurationManager.AppSettings["notaInterna"];
            rblTipoEvento.Items[2].Text = ConfigurationManager.AppSettings["notaPublica"];
        }
    }
}