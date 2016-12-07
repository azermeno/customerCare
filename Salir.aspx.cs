using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomerCare
{
    public partial class Salir : CustomerCareResource
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Nombre = "index";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            Session.Clear();
            HttpCookie cook = new HttpCookie("DatosUsuario");
            cook.Expires = new DateTime(2000, 1, 1);
            Response.Cookies.Add(cook);
        }
    }
}