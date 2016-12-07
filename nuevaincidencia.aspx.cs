using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using System.Data.OleDb;

public partial class Default : System.Web.UI.Page 
{
    public string wy;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["wy"] == null && (Request.QueryString["wy"] == null || Request.QueryString["wy"] == "undefined"))
            Response.Redirect("index.aspx?url=nuevaincidencia.aspx?");
        if (Session["wy"]==null) 
            wy = Request.QueryString["wy"];
        else
            wy = Session["wy"].ToString();
        rtbTipificacion.Attributes.Add("onclick", "showMenu(event)");
        rtbTipificacion.Attributes.Add("onkeyup", "showMenuK(event.keyCode)");
        //chkRes.Attributes.Add("onclick", "colapsar()");
        if (! Page.IsPostBack)
                rcbResponsable.SelectedValue = DeCrip(wy);
        rcbSolicitante.Focus();
        HiddenField1.Value = DeCrip(wy);
    }
    protected void rcmTipificacion_ItemDataBound(object sender, RadMenuEventArgs e)
    {
        e.Item.Attributes.Add("onkeyup", "cerrarMenu(event.keyCode)");
    }
    private string DeCrip(string wy)
    {
        string texto = "";
        foreach (char c in wy)
            texto =  texto + (char)(c - 3);
        return texto;
    }
}
