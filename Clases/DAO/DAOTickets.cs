using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace CustomerCare
{

    public partial class DAO
    {
        public int códigoSimple(string códigoCompuesto)//Devuelve el Código Simple dado el Código Compuesto si este existe, si no existe devuelve cero
        {
            SqlCommand qry = new SqlCommand("SELECT dbo.CodigoSimple('" + códigoCompuesto + "')", conex);
            try
            {
                qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                qry.CommandTimeout = 300;
            }
            int result = 0;
            result = (int)qry.ExecuteScalar();
            return result;
        }

        public Tipificación getConclusión(int código)
        {
            SqlCommand qry = new SqlCommand("Select Descripcion from Conclusiones where codigo=" + código, conex);
            try
            {
                qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                qry.CommandTimeout = 300;
            }
            try
            {
                Tipificación result = new Tipificación(código);
                result.descripción = (string)qry.ExecuteScalar();
                return result;
            }
            catch
            {
                return null;
            }
        }

        public Tipificación getTipificación(int código)
        {

            SqlCommand qry = new SqlCommand("Select Descripcion from Tipificaciones where codigo=" + código, conex);
            try
            {
                qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                qry.CommandTimeout = 300;
            }
            try
            {
                Tipificación result = new Tipificación(código);
                result.descripción = (string)qry.ExecuteScalar();
                return result;
            }
            catch
            {
                return null;
            }
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

        public Ticket getTicket(int códigoSimple)
        {
            DAO daoAux = new DAO();
            Ticket result;
            SqlCommand qry = new SqlCommand("Select Asunto, DetalleAsunto, Apertura, Clausura, Limite, Responsable, DetalleSolucion, Levanto, Solicitante, TipoSolicitante, CONVERT(int,Tipificacion), Conclusion, Severidad, Estado, CodigoCompuesto from Incidencias where codigo=" + códigoSimple, conex);
            try
            {
                qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                qry.CommandTimeout = 300;
            }
            SqlDataReader reader = qry.ExecuteReader();
            if (reader.Read())
            {
                daoAux.open();
                //result = new Ticket(códigoSimple, reader.GetString(14));
                result = new Ticket();
                result.asunto = reader.GetString(0);
                result.detalle = reader.GetString(1);
                result.apertura = reader.IsDBNull(2) ? null : (DateTime?)reader.GetDateTime(2);
                result.clausura = reader.IsDBNull(3) ? null : (DateTime?)reader.GetDateTime(3);
                result.límite = reader.IsDBNull(4) ? null : (DateTime?)reader.GetDateTime(4);
                result.responsable = daoAux.getUsuario(reader.GetInt32(5));
                result.solución = reader.GetString(6);
                result.levantador = daoAux.getUsuario(reader.GetInt32(7));
                result.solicitante = daoAux.getSolicitante(reader.GetInt32(8), reader.GetInt32(9));
                result.tipificación = reader.IsDBNull(10) ? null : daoAux.getTipificación(reader.GetInt32(10));
                result.conclusión = reader.IsDBNull(11) ? null : daoAux.getConclusión(reader.GetInt32(11));
                result.prioridad = (Prioridad)reader.GetInt32(12);
                result.estado = reader.GetInt32(13);
                daoAux.close();
                reader.Close();
            }
            else
            {
                return null;
            }
            qry.CommandText = "Select TipoEvento, Observacion, Fecha from Eventos Where Incidencia=" + códigoSimple;
            reader = qry.ExecuteReader();
            while (reader.Read())
            {
                result.eventos.Add(new Evento((TipoEvento)reader.GetInt32(0), reader.GetString(1), reader.GetDateTime(2)));
            }
            reader.Close();
            return result;
        }

        public Ticket getTicket(string códigoCompuesto)
        {
            return getTicket(códigoSimple(códigoCompuesto));
        }

        public void actualizaTicket(Ticket ticket, int código)
        {
            try
            {
                Utilidades.log("Apertura: " + ticket.apertura);
                Utilidades.log("Limite: " + ticket.límite);
                SqlCommand qry = new SqlCommand("", conex);
                try
                {
                    qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    qry.CommandTimeout = 300;
                }
                DAO daoAux = new DAO();
                daoAux.open();
                Ticket previo = this.getTicket(código);
                daoAux.close();
                string command = "";
                if (previo.asunto != ticket.asunto)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Asunto= " + "'" + ticket.asunto + "', ";
                if (previo.detalle != ticket.detalle)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "DetalleAsunto= " + "'" + ticket.detalle + "', ";
                if (previo.solución != ticket.solución)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "DetalleSolucion= " + "'" + ticket.solución + "', ";
                if (previo.apertura != ticket.apertura)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Apertura= " + ticket.apertura.toSQL() + ", ";
                if (previo.clausura != ticket.clausura)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Clausura= " + ticket.clausura.toSQL() + ", ";
                if (previo.límite != ticket.límite)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Limite= " + ticket.límite.toSQL() + ", ";
                if (previo.responsable != ticket.responsable)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Responsable= " + (ticket.responsable == null ? "NULL" : ticket.responsable.código.ToString()) + ", ";
                if (previo.levantador != ticket.levantador)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Levanto= " + (ticket.levantador == null ? "NULL" : ticket.levantador.código.ToString()) + ", ";
                if (previo.solicitante != ticket.solicitante)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Solicitante= " + (ticket.solicitante == null ? "NULL" : ticket.solicitante.código.ToString()) + ", " +
                        "TipoSolicitante= " + (ticket.solicitante == null ? "NULL" : ticket.solicitante.tipoSolicitante.ToString()) + ", ";
                if (previo.tipificación != ticket.tipificación)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Tipificacion=" + (ticket.tipificación == null ? "NULL" : ticket.tipificación.código.ToString()) + ", ";
                if (previo.conclusión != ticket.conclusión)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Conclusion= " + (ticket.conclusión == null ? "NULL" : ticket.conclusión.código.ToString()) + ", ";
                if (previo.prioridad != ticket.prioridad)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Severidad= " + (int)ticket.prioridad + ", ";
                if (previo.estado != ticket.estado)
                    command += (command == "" ? "UPDATE Incidencias SET " : "") + "Estado= " + ticket.estado.ToString() + ", ";
                if (command != "")
                {
                    qry.CommandText = command.Substring(0, command.Length - 2) + " WHERE codigo=" + código;
                    Utilidades.log(qry.CommandText);
                    qry.ExecuteNonQuery();
                }
                if (previo.eventos.Count != ticket.eventos.Count)
                {
                    Evento evento;
                    for (int i = previo.eventos.Count; i < ticket.eventos.Count; i++)
                    {
                        evento = ticket.eventos[i];
                        string result = ticket.código.ToString();
                        qry.CommandText = "INSERT INTO Eventos (Incidencia, Observacion, TipoEvento, Datos, Fecha) VALUES (" + result + ", '" + evento.texto + "', " + (int)evento.tipo + ", '" + (evento.usuario == null ? "{}" : "{\"usu\":" + evento.usuario + "}") + "', " + evento.fecha.toSQL() + ")";
                        Utilidades.log(qry.CommandText);
                        qry.ExecuteNonQuery();
                    }
                }
            }
            catch (InvalidCastException e)
            {
                Utilidades.log("Error: " + e.Message);
            }
        }

        public int guardaTicket(Ticket ticket)
        {
            int result = 0;
            try
            {
                Utilidades.log("Apertura: " + ticket.apertura);
                Utilidades.log("Limite: " + ticket.límite);
                SqlCommand qry = new SqlCommand("", conex);
                try
                {
                    qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    qry.CommandTimeout = 300;
                }
                    qry.CommandText = "INSERT Incidencias " +
                        "(" +
                            "Asunto, " +
                            "DetalleAsunto, " +
                            "DetalleSolucion, " +
                            "Apertura, " +
                            "Clausura, " +
                            "Limite, " +
                            "Responsable, " +
                            "Levanto, " +
                            "Solicitante, " +
                            "TipoSolicitante, " +
                            "Tipificacion," +
                            "Conclusion, " +
                            "Severidad, " +
                            "Estado, " +
                            "CerradaIngresar" +
                        ") " +
                        "VALUES (" +
                            "'" + ticket.asunto.toSQL() + "', " +
                            "'" + ticket.detalle.toSQL() + "', " +
                            "'" + ticket.solución.toSQL() + "', " +
                            ticket.apertura.toSQL() + ", " +
                            ticket.clausura.toSQL() + ", " +
                            ticket.límite.toSQL() + ", " +
                            (ticket.responsable == null ? "NULL" : ticket.responsable.código.ToString()) + ", " +
                            (ticket.levantador == null ? "NULL" : ticket.levantador.código.ToString()) + ", " +
                            (ticket.solicitante == null ? "NULL" : ticket.solicitante.código.ToString()) + ", " +
                            (ticket.solicitante == null ? "NULL" : ticket.solicitante.tipoSolicitante.ToString()) + ", " +
                            (ticket.tipificación == null ? "NULL" : ticket.tipificación.código.ToString()) + ", " +
                            (ticket.conclusión == null ? "NULL" : ticket.conclusión.código.ToString()) + ", " +
                            ((int)ticket.prioridad).ToString() + ", " +
                            ticket.estado.ToString() + ", " +
                            "CAST(" + (ticket.resueltoIngresar ? "1" : "0") + " AS bit)" +
                        "); SELECT Top 1 Codigo from Incidencias ORDER BY Codigo DESC";
                    Utilidades.log(qry.CommandText);
                    result = (int)qry.ExecuteScalar();
                    Utilidades.log(result.ToString());
                    if (ticket.levantador != null)
                    {
                        qry.CommandText = "INSERT INTO Traza (Usuario, Incidencia, Tipo, Fecha) VALUES (" + ticket.levantador.código + ", " + result + ", 2, GETDATE())";
                        Utilidades.log(qry.CommandText);
                        qry.ExecuteNonQuery();
                    }
                    if (ticket.responsable != null)
                    {
                        qry.CommandText = "INSERT INTO Traza (Usuario, Incidencia, Tipo, Fecha) VALUES (" + ticket.responsable.código + ", " + result + ", 1, GETDATE())";
                        Utilidades.log(qry.CommandText);
                        qry.ExecuteNonQuery();
                    }
                    foreach (Evento evento in ticket.eventos)
                    {
                        qry.CommandText = "INSERT INTO Eventos (Incidencia, Observacion, TipoEvento, Datos, Fecha) VALUES (" + result + ", '" + evento.texto + "', " + (int)evento.tipo + ", '" + (evento.usuario == null ? "{}" : "{\"usu\":" + evento.usuario + "}") + "', " + evento.fecha.toSQL() + ")";
                        Utilidades.log(qry.CommandText);
                        qry.ExecuteNonQuery();
                    }
                Utilidades.log(result.ToString());
            }
            catch (InvalidCastException e)
            {
                Utilidades.log("Error: " + e.Message);
            }
            return result;
        }
    }
}