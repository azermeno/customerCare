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
        /// <summary>
        /// Autor: Andres Lara y Cristos Ruiz
        /// Fecha de Creación: 
        /// Descripción: Método encargado de registrar los errores en la tabla dbo.Errores que son son controlados por la aplicación.
        ///         Bajo este mecanismo se podrá verificar donde ocurré el problema.
        /// </summary>
        /// <param name="texto">Parámetro de tipo cadena que provee el mensaje de error generardo por el aplicativo</param>
        /// <returns>Devuelve el último valor generado por la columna identity de la tabla dbo.Errores</returns>
        public string logea(string texto)
        {
            //Se genera las variables locales con la finalidad de reservar las direcciones de memoría.
            //Esto se lográ a tráves de asignarles un valor específico.
            int intTimeOut = 0;
            string strQuery = string.Empty;
            string strResult = string.Empty;
            SqlCommand qry = null;
            try
            {
                //Se obtiene el parámentro de timeout almacenado en el archivo de configuración web.config.
                //Se utiliza el método TryParse del objeto int para convertir el valor, en caso de que esto no se logrará 
                //se le asigna el valor.
                if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim(), out intTimeOut))
                    intTimeOut = 300;
                
                //Se arma la sentencia DML y DDL con el fin de procesar los datos del mensaje, en este caso se utiliza el esquema
                //bajo parámetros de SQL, para evitar el SQL Injection
                strQuery = "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Errores'))";
                strQuery += " INSERT dbo.Errores (Texto) VALUES (@texto)";
                strQuery += " ELSE BEGIN";
                strQuery += " CREATE TABLE dbo.Errores (Codigo int IDENTITY (1,1), Fecha datetime DEFAULT GETDATE(), Texto varchar(8000)); ";
                strQuery += " INSERT Errores (Texto) VALUES (@texto); ";
                strQuery += " END;";
                strQuery += " SELECT TOP 1 Codigo FROM Errores ORDER BY Codigo DESC;";
                /*
                 * NOTA: En lugar de manejar el tipo de dato VARCHAR(8000) es preferible manejar el tipo de dato VARCHAR(MAX), este
                 *      tipo de dato puede albergar hasta 2,147,483,647 caracteres.
                 */
                //Se valida la conexión. No tiene caso continuar si la conexión no está abierta
                if (conex.State == System.Data.ConnectionState.Open)
                {
                    //Se prepara el comando para el procesamiento de la cadena DML y DLL, a través del using.
                    //El using tratará de cerrar la instancia del comando de manera natural.
                    using (qry = new SqlCommand())
                    {
                        SqlParameter parTexto = qry.Parameters.Add("@texto", System.Data.SqlDbType.VarChar, 8000);
                        parTexto.Value = texto.Trim();
                        qry.CommandTimeout = intTimeOut;
                        qry.CommandText = strQuery;
                        qry.CommandType = System.Data.CommandType.Text; //Aunque por defecto se le asigna el valor es mejor especificarlo
                        qry.Connection = conex;
                        strResult = qry.ExecuteScalar().ToString();
                    }
                }
            }
            catch
            {
                //Seria bueno poner aquí algo de registrar los errores criticos en el Visor de Sucesos del sistema operativo.
                //https://support.microsoft.com/es-mx/kb/307024
            }
            finally
            {
                /* PASE LO QUE PASE SE EJECUTA LAS SIGUIENTES LINEAS*/
                //Se destruyen las instancias creadas
                strQuery = string.Empty;
                //en caso de que el using no destruya la instancia, se obliga dicha destrucción.
                if (qry != null)
                {
                    qry.Dispose(); //Se llama el dispose para obligar el GC para que lo tome en el siguiente ciclo de limpieza
                    qry = null;
                }
            }
            return strResult;
            /*
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
             * */
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
    }
}