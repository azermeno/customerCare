using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.OleDb;

namespace CustomerCare
{
    public partial class _Default : System.Web.UI.Page
    {
        public OleDbConnection conCustomer = new OleDbConnection("Provider=SQLOLEDB;Data Source=(local);Persist Security Info=True;Password=C1B3RN3T;User ID=sa;Initial Catalog=CustomerCare");

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
            }
        }

        protected void RadChart1_Click(object sender, Telerik.Charting.ChartClickEventArgs args)
        {

        }
    }
}
