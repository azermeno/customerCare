using System.Data.SqlClient;
using CustomerCare;
using System;

namespace CustomerCare
{
    public  class Clase
    {
        public int código { get; private set; }
        public string nombre { get; set; }
        public Usuario responsable {get; set;}
        public Clase(int código)
        {
            this.código = código;
        }
    }

    public partial class DAO
    {
        public Clase getClase(int codigo)
        {
            SqlCommand query = new SqlCommand("Select Nombre, Responsable FROM Clases WHERE Codigo=" + codigo, conex);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            DAO daoAux = new DAO();
            SqlDataReader reader = query.ExecuteReader();
            if (reader.Read())
            {
                Clase result = new Clase(codigo);
                result.nombre = reader.GetString(0);
                daoAux.open();
                result.responsable = daoAux.getUsuario(reader.GetInt32(1));
                daoAux.close();
                return result;
            }
            else
                return null;
        }
    }
}