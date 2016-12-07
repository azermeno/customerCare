using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;

namespace CustomerCare
{
    public static class Extensiones 
    {
        public static string toSQL(this DateTime? dateTime)
        {
            if (dateTime == null)
                return "NULL";
            else
                return toSQL((DateTime)dateTime);
        }

        public static string toSQL(this DateTime dateTime)
        {
            Utilidades.log("Formateador de fecha. Fecha: "+dateTime.ToString());
            return "CONVERT(DATETIME,'" + (dateTime).ToString("dd/MM/yyyy HH:mm") + "',103)";
        }

        public static string toSQL(this string texto)
        {
            return texto.Replace("'", "''");
        }
    }

    public partial class DAO
    {
        private SqlConnection conex;

        public DAO()
        {
            conex = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
        }

        public void open()
        {
            conex.Open();
        }

        public void close()
        {
            conex.Close();
        }

        public SqlConnection con
        {
            get { return conex; }
        }

        public string logea(string texto)
        {
            SqlCommand qry = new SqlCommand(
                "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'Errores')) " +
                "INSERT Errores (Texto) VALUES ('" + texto.Replace("'", "''") + "') " +
                "ELSE BEGIN " +
                "CREATE TABLE Errores (Codigo int IDENTITY (1,1), Fecha datetime DEFAULT GETDATE(), Texto varchar(8000)); " +
                "INSERT Errores (Texto) VALUES ('" + texto.Replace("'", "''") + "'); " +
                "END; " +
                "SELECT TOP 1 Codigo FROM Errores ORDER BY Codigo DESC; ", conex);
            try
            {
                qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                qry.CommandTimeout = 300;
            }
            string result = qry.ExecuteScalar().ToString();
            return result;
        }

        public Solicitante getSolicitante(int código, int tipoSolicitante)
        {
            SqlCommand qry = new SqlCommand("", conex);
            try
            {
                qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                qry.CommandTimeout = 300;
            }
            switch (tipoSolicitante){
                case 1:
                    qry.CommandText = "Select Nombre from Unidades where Codigo=" + código;
                    break;
                case 2:
                    qry.CommandText= "Select Nombre from GruposSolucion where codigo=" + código;
                    break;
                case 3:
                      qry.CommandText=  "Select Nombre from Zonas where codigo=" + código;
                    break;
                case 4:
                    qry.CommandText= "Select Login from Usuarios where codigo=" + código;
                    break;
                case 5:
                    qry.CommandText="Select Login from Usuario u join Internos i on u.Codigo=i.Usuario where i.codigo=" +código;
                    break;
                case 6:
                    qry.CommandText="Select NOmbre from Sitios where codigo=" + código;
                    break;
            }
            //conex.Open();
            try
            {
                Solicitante result = new Solicitante(código, tipoSolicitante);
                result.nombre = (string)qry.ExecuteScalar();
                return result;
            }
            catch
            {
                return null;
            }
            //finally { conex.Close(); }
        }

        public List<Ticket> derivados(int códigoSimple)
        {
            DAO daoAux = new DAO();
            List<Ticket> result = new List<Ticket>();

            SqlCommand query = new SqlCommand("Select Derivado from Dependencias where Origen=" + códigoSimple, conex);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            //conex.Open();
            //try
            //{
            //System.Data.DataTable tabla=new System.Data.DataTable();
            //SqlDataAdapter adapter = new SqlDataAdapter(query);
            //adapter.Fill(tabla);
            SqlDataReader reader = query.ExecuteReader();
            daoAux.open();
            //foreach(System.Data.DataRow row in tabla.Rows)
            while (reader.Read())
            {
                result.Add(daoAux.getTicket(reader.GetInt32(0)));
                //result.Add(daoAux.getTicket((int)row[0]));
            }
            daoAux.close();
            //}
            //finally { conex.Close(); }
            return result;
        }
    }
}