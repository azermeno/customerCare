using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;
using CustomerCare;
using Telerik.Web.UI;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Net.Mail;

namespace CustomerCare
{
    
    [WebService(Description = "Servicio web para aplicaciones de CustomerCare. Todos los metodos web en caso de error devuelven el texto \"Error \" seguido del numero de error. En caso de exito devuelven texto \"OK\"", Namespace = "http://customercare.com.mx/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class Servicio : System.Web.Services.WebService
    {
        private DAO dao;
        private SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);

        public Servicio()
        {
           dao=new DAO(); 
        }

        private string Encrip(string cad)
        {
            int pas;
            pas = 0;
            foreach (char c in cad)
                pas = pas + (int)c;
            pas = 134 * pas;
            return (pas.ToString());
        }
                
        [WebMethod(Description = "Guarda errores en el log de errores, regresa el numero de error con el que se guardo. Parámetros: error - texto de error")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string logeaCliente(string error)
        {
            dao.open();
            string result=dao.logea("CLIENTE: " + error);
            dao.close();
            return result;
        }

        [WebMethod(Description = "Obtiene las validaciones de un Ticket. Parámetros: cod - codigo de ticket.")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Validaciones(string cod)
        {
                SqlDataReader reader;
                con.Open();
                SqlCommand query = new SqlCommand();
                try
                {
                    query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    query.CommandTimeout = 300;
                }
                List<Dictionary<string, string>> Validaciones = new List<Dictionary<string, string>>();
            try
            {
                query.Connection = con;
                query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + cod + "') SELECT Datos, ISNULL(CONVERT(char(10), Fecha, 103) + ' ' + CONVERT(char(5), Fecha, 114), '') AS Expr1, TipoEvento FROM Eventos e WHERE (Incidencia = @cod) and TipoEvento in (4,5)";
                reader = query.ExecuteReader();
            }
            catch{
                con.Close();
                dao.open();
                string result=dao.logea("Error de sintaxis en Validaciones: " + query.CommandText);
                dao.close();
                return result;
            }
            try
            {
                while (reader.Read())
                {
                    string strjs = reader.GetString(0);
                    JObject datosEv;
                    try
                    {
                        datosEv = JObject.Parse(strjs);
                    }
                    catch
                    {
                        reader.Close();
                        con.Close();
                        dao.open();
                        string result = "Error " + dao.logea("Error JSON en datos de eventos de " + cod + ":" + strjs);
                        dao.close();
                        return result;
                    }
                    int vr = (int)datosEv["usu"];
                    Dictionary<string, string> validacion = new Dictionary<string, string>();
                    validacion.Add("vr", vr.ToString() + "," + reader.GetString(1));
                    if (reader.GetInt32(2) == 4)
                        validacion.Add("ar", "a");
                    else
                        validacion.Add("ar", "r");
                    Validaciones.Add(validacion);
                }
                reader.Close();
                foreach (Dictionary<string, string> validacion in Validaciones)
                {
                    string[] vr = validacion["vr"].Split(',');
                    query.CommandText = "select u.nombre from Usuarios u where u.Codigo=" + vr[0];
                    validacion["vr"] = (string)query.ExecuteScalar() + "," + vr[1];
                }
                con.Close();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string strJSON = js.Serialize(Validaciones);
                return strJSON;
            }
            catch (Exception e)
            {
                reader.Close();
                con.Close();
                dao.open();
                string result="Error " + dao.logea(e.Message);
                dao.close();
                return result;
            }
        }

        [WebMethod(Description = "Guarda el html de un ticket.")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string guardaVersion(string cod, string ver, string usu, string html)
        {
            string rutaTickets = ConfigurationManager.AppSettings["rutaInstalacion"] + "\\HTML\\Tickets\\";
            StreamWriter arch = File.CreateText(rutaTickets + cod + "v" + ver + "u" + usu + ".html");
            arch.Write("<html>" + html + "</html>");
            arch.Close();
            return "OK";
        }

        [WebMethod(Description = "Guarda el html del formato")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string guardaVersionFor(string cdg, string tic, string wy, string archivo)
        {
            //string rutaFormatos = ConfigurationManager.AppSettings["rutaInstalacion"] + "\\HTML\\Formatos\\";
            //string[] versiones = Directory.GetFiles(rutaFormatos, cdg + "i" + tic + "v*u*.html");
            SqlCommand query = new SqlCommand("Select isnull(max(version),0) from versiones where formato=" + cdg + " and ticket=dbo.CodigoSimple('" + tic + "')", con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            int n = (int)query.ExecuteScalar();
            query.CommandText = "Insert Versiones(Formato, Ticket, Version, Usuario, Fecha, Texto) " +
                "VALUES (" + cdg + ",dbo.CodigoSimple('" + tic + "')," + (n+1).ToString() + "," + Utilidades.Crip(wy) + ", getdate(), '<html>" + archivo.Replace("'","''") + "</html>')";
            Utilidades.log("Intentara guardar una nueva version de documento");
            Utilidades.log(query.CommandText);
            query.ExecuteNonQuery();
            con.Close();
            return "guardado";
        }

        [WebMethod(Description = "Modifica el asunto, detalle y datos adjuntos de un ticket")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Edicion(string cod, string asu, string det, string dat, string usu)
        {
            string us = Utilidades.Crip(usu);
            dao.open();
            Usuario usuario = dao.getUsuario(Convert.ToInt32(us));
            con.Open();
            if (!usuario.tienePermiso(Permiso.EditarTickets))
            {
                con.Close();
                return "No tiene permiso";
            }
            SqlCommand query = new SqlCommand(
                "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + cod + "');" +
                "UPDATE Incidencias " +
                "SET Asunto = '" + asu + "', DetalleAsunto = '" + det + "', Datos = '" + dat + "' " +
                "WHERE Codigo = @cod " +
                "INSERT Eventos (Incidencia, TipoEvento, Observacion, Datos, Fecha) " +
                "VALUES (@cod,8,'" + ConfigurationManager.AppSettings["ticket"] + " editad" + ConfigurationManager.AppSettings["mascfem"] + "','{\"usu\"=\"" + us + "\"}', Getdate())"
                , con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.ExecuteNonQuery();
            con.Close();
            dao.close();
            return "Editada";
        }

        [WebMethod(Description = "Te da los Eventos de un Ticket")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ObtieneSolicitantes(string usu)
        {
            List<Dictionary<string, string>> Solicitantes = new List<Dictionary<string, string>>();
            SqlDataReader reader;
            string us = Utilidades.Crip(usu);
            string restric;
            SqlCommand query = new SqlCommand("Select PuedeVerSol From Usuarios where Codigo=" + us, con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            restric = "(" + query.ExecuteScalar().ToString() + ") ";
            query.CommandText = "select * from ( " +
                "select nombre, '1' + cast(codigo as varchar) as codigo from unidades where activa=1 " +
                "union " +
                "select nombre, '3' + cast(codigo as varchar) as codigo from zonas where activa=1 " +
                "union " +
                "select nombre, '6' + cast(codigo as varchar) as codigo from sitios where activo=1 " +
                "union " +
                "select nombre, '4' + cast(codigo as varchar) as codigo from usuarios where codigo in (select usuario from clientes) and activo = 1) sols " +
                "where " + restric +
                "order by nombre";
            reader = query.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> solicitante = new Dictionary<string, string>();
                solicitante.Add("ne", reader.GetString(0));
                solicitante.Add("co", reader.GetString(1));
                Solicitantes.Add(solicitante);
            }
            reader.Close();
            con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(Solicitantes);
            return strJSON;
        }

        [WebMethod(Description = "Te da una lista de Usuarios")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ObtieneUsuarios(string usu)
        {
            List<Dictionary<string, string>> Usuarios = new List<Dictionary<string, string>>();
            SqlDataReader reader;
            string us = Utilidades.Crip(usu);
            string restric;
            SqlCommand query = new SqlCommand("Select PuedeVerUsu From Usuarios where Codigo=" + us, con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            restric = "(" + (string)query.ExecuteScalar() + ") ";
            query.CommandText = "SELECT [Codigo], [Nombre] FROM [Usuarios] WHERE ([Activo] = 1) " +
                "and " + restric +
                "order by nombre";
            reader = query.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> usuario = new Dictionary<string, string>();
                usuario.Add("ne", reader.GetString(1));
                usuario.Add("co", reader.GetInt32(0).ToString());
                Usuarios.Add(usuario);
            }
            reader.Close();
            con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(Usuarios);
            return strJSON;
        }

        [WebMethod(Description = "Te da el catálogo de conclusiones")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ObtieneConclusiones(string usu)
        {
            List<Dictionary<string, string>> Conclusiones = new List<Dictionary<string, string>>();
            SqlDataReader reader;
            string us = Utilidades.Crip(usu);
            string restric;
            SqlCommand query = new SqlCommand("Select PuedeVerCon From Usuarios where Codigo=" + us, con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            restric = "(" + query.ExecuteScalar().ToString() + ") ";
            query.CommandText = "SELECT [Codigo], ISNULL([Padre],-1), [Descripcion] FROM [Conclusiones] where activo=1 " +
                "and " + restric;
            reader = query.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> conclusion = new Dictionary<string, string>();
                conclusion.Add("co", reader.GetInt32(0).ToString());
                conclusion.Add("pe", reader.GetInt32(1).ToString());
                conclusion.Add("dn", reader.GetString(2));
                Conclusiones.Add(conclusion);
            }
            reader.Close();
            con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(Conclusiones);
            return strJSON;
        }

        [WebMethod(Description = "Te da el catálogo de tipificaciones")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ObtieneTipificaciones(string usu)
        {
            List<Dictionary<string, string>> Tipificaciones = new List<Dictionary<string, string>>();
            SqlDataReader reader;
            string us = Utilidades.Crip(usu);
            string restric;
            SqlCommand query = new SqlCommand("Select PuedeVerTip From Usuarios where Codigo=" + us, con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            restric = "(" + query.ExecuteScalar().ToString() + ") ";
            query.CommandText = "SELECT [Codigo], ISNULL([Padre],-1), [Descripcion] FROM [Tipificaciones] where activo=1 " +
                "and " + restric + " ORDER BY [Descripcion] ";
            reader = query.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> tipificacion = new Dictionary<string, string>();
                tipificacion.Add("co", reader.GetInt32(0).ToString());
                tipificacion.Add("pe", reader.GetInt32(1).ToString());
                tipificacion.Add("dn", reader.GetString(2));
                Tipificaciones.Add(tipificacion);
            }
            reader.Close();
            con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(Tipificaciones);
            return strJSON;
        }

        [WebMethod(Description = "Mantiene actualizado todo")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ActualizaDerivados(int startRowIndex, string filtro, string usu, string tick)
        {
            string command = "";
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            con.Open();
            command = "SELECT top 1 " +
                " i.CodigoCompuesto " +
            "FROM 	Incidencias i  " +
            "WHERE i.Codigo NOT IN (SELECT top " + startRowIndex.ToString() + " i.Codigo from Incidencias i  " +
                " where " + FiltroDerivado(filtro, usu, tick) + " order by i.Codigo desc) and " + FiltroDerivado(filtro, usu, tick) +
            " ORDER BY i.Codigo desc";
            query.CommandText = command;
            command = query.ExecuteScalar().ToString();
            con.Close();
            return command;
        }

        [WebMethod(Description = "Te da la hora del servidor en formato yyMMddHHmmss")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Instante()
        {
            return DateTime.Now.ToString("yyMMddHHmmss");
        }

        private string FiltroDerivado(string filtro, string usu, string tick)
        {
            string us = Utilidades.Crip(usu);
            string restric;
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            query.CommandText = "Select PuedeVer From Usuarios where Codigo=" + us;
            restric = "(" + query.ExecuteScalar().ToString() + ") AND i.CodigoCompuesto like '" + tick + "%' ";
            string bus = "";
            string[] filtros = filtro.Split('!');
            if (filtros[0] != "")
                bus += "i.Responsable=" + filtros[0];
            if (filtros[1] != "")
            {
                if (bus != "")
                    bus += " OR ";
                bus += "dbo.Escaladores(i.Codigo) LIKE '%" + filtros[1] + "%'";
            }
            if (filtros[2] != "")
            {
                if (bus != "")
                    bus += " OR ";
                bus += "i.TipoSolicitante=" + filtros[2].Substring(0, 1) + " AND i.Solicitante=" + filtros[2].Substring(1);
            }
            if (filtros[3] != "")
            {
                if (bus != "")
                    bus += " OR ";
                bus += "i.Levanto=" + filtros[3];
            }
            if (filtros[4] != "")
            {
                if (bus != "")
                    bus = "(" + bus + ")";
                else bus = "1=1";
                string bin = Convert.ToString(Convert.ToInt32(filtros[4]), 2);
                string str = new String('0', 5 - bin.Length);
                bin = str + bin;
                str = "";
                for (int i = 1; i <= 5; i++)
                {
                    if (bin[5 - i] == '1')
                        str = str + "," + i.ToString();
                }
                if (str != "")
                {
                    str = str.Substring(1);
                    bus += " AND i.Estado IN (" + str + ")";
                }
                else bus += " AND 1=0";
            }
            if (bus != "")
                restric = restric + " AND (" + bus + ")";
            return restric;
        }

        private string FiltroBusca(string filtro, string usu)
        {
            string us = Utilidades.Crip(usu);
            string restric;
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            query.CommandText = "Select PuedeVer From Usuarios where Codigo=" + us;
            restric = "(" + query.ExecuteScalar().ToString() + ") ";
            string bus = "";
            string[] filtros = filtro.Split('!');
            if (filtros[0] != "")
            {
                string bin = Convert.ToString(Convert.ToInt32(filtros[0]), 2);
                string str = new String('0', 5 - bin.Length);
                bin = str + bin;
                str = "";
                for (int i = 1; i <= 5; i++)
                {
                    if (bin[5 - i] == '1')
                        str = str + "," + i.ToString();
                }
                if (str != "")
                {
                    str = str.Substring(1);
                    bus = " i.Estado IN (" + str + ")";
                }
                else bus = " 1=0";
            }
            restric = restric + " AND " + bus + "";
            return restric;
        }

        [WebMethod(Description = "Repactar Ticket")]
        public string Repactar(string cod, string fecha, string men, string usu)
        {
            SqlCommand query = new SqlCommand("", con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            string[] cods = cod.Split(',');
            foreach (string codigo in cods)
            {
                query.CommandText = "DECLARE @ant datetime SET @ant=(SELECT limite from incidencias where Codigo=dbo.CodigoSimple('" + codigo + "')) INSERT INTO Eventos " +
    "                      (Fecha, Observacion, Incidencia, Datos, TipoEvento) " +
    "VALUES     (GETDATE(), '" + men + "', dbo.CodigoSimple('" + codigo + "'), '{\"usu\":" + Utilidades.Crip(usu) + ", \"ant\":\"' + CONVERT(char(16),@ant,120) + '\"}', 10)";
                query.ExecuteNonQuery();
                query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') UPDATE Incidencias SET Limite=CONVERT(datetime, '" + fecha + "', 103) WHERE Codigo=@cod";
                query.ExecuteNonQuery();
            }
            con.Close();
            return "Repactacion";
        }

        [WebMethod(Description = "Reprogramar Ticket")]
        public string Reprogramar(string cod, string fechini, string fecha, string men, string usu)
        {
            SqlCommand query = new SqlCommand("", con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            string[] cods = cod.Split(',');
            foreach (string codigo in cods)
            {
                query.CommandText = "DECLARE @ant datetime SET @ant=(SELECT limite from incidencias where Codigo=dbo.CodigoSimple('" + codigo + "')) INSERT INTO Eventos " +
    "                      (Fecha, Observacion, Incidencia, Datos, TipoEvento) " +
    "VALUES     (GETDATE(), '" + men + "', dbo.CodigoSimple('" + codigo + "'), '{\"usu\":" + Utilidades.Crip(usu) + ", \"ant\":\"' + CONVERT(char(16),@ant,120) + '\"}', 13)";
                query.ExecuteNonQuery();
                query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') UPDATE Incidencias SET Limite=CONVERT(datetime, '" + fecha + "', 103), Apertura=CONVERT(datetime, '" + fechini + "', 103), estado=1  WHERE Codigo=@cod";
                query.ExecuteNonQuery();
            }
            con.Close();
            return "Reprogramación";
        }

        [WebMethod(Description = "Activar ticket")]
        public string Activar(string cod)
        {
            dao.open();
            Ticket ticket = dao.getTicket(cod);
            if (ticket.asunto == "" || ticket.asunto == null)
                return "No es posible activar, " + Utilidades.el + " " + Utilidades.ticket + " no tiene asunto";
            if (ticket.estado != 1)
                return "No es posible activar, " + Utilidades.el + " " + Utilidades.ticket + " debe estar inactivo";
            if (ticket.límite == null || (DateTime)ticket.límite <= DateTime.Now)
                return "No es posible activar, " + Utilidades.el + " " + Utilidades.ticket + " debe tener un límite válido";
            if (ticket.responsable ==null || !ticket.responsable.activo)
                return "No es posible activar, " + Utilidades.el + " " + Utilidades.ticket + " debe tener un responsable válido";
            try
            {
                DataSet inc = new DataSet();
                SqlCommand query = new SqlCommand("DECLARE @cod int SET @cod=dbo.CodigoSimple('" + cod + "') UPDATE Incidencias SET Estado=2,DetalleSolucion='',Apertura=getdate() WHERE Codigo=@cod", con);
                try
                {
                    query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    query.CommandTimeout = 300;
                }
                SqlDataAdapter parellenar = new SqlDataAdapter(query);
                con.Open();
                query.ExecuteNonQuery();
                con.Close();
                query.CommandText = "  DECLARE @cod int SET @cod=dbo.CodigoSimple('" + cod + "') SELECT @cod As Codigo";
                con.Open();
                parellenar.Fill(inc);
                con.Close();
                if (inc.Tables[0].Rows.Count > 0)
                {
                    notificaActivacion(Convert.ToInt32(inc.Tables[0].Rows[0]["codigo"].ToString().Trim()));
                }
            }
            catch
            {

            }
            dao.close();
            return "activada";
        }

        [WebMethod(Description = "Obtiene el código de usuario a partir de sus credenciales")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Autenticar(string login, string password)
        {
            SqlCommand query = new SqlCommand("SELECT Codigo FROM Usuarios WHERE rtrim(Login)=rtrim('" + login + "') and cast(Clave as" +
                " varchar)='" + Encrip(password) + "'", con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            try
            {
                string usu = Utilidades.Crip(((int)query.ExecuteScalar()).ToString());
                con.Close();
                return usu;
            }
            catch
            {
                con.Close();
                return "usuario o password invalidos";
            }
        }

        private Usuario getUsuario(string usu)
        {
            dao.open();
            Usuario result = dao.getUsuario(Convert.ToInt32(Utilidades.Crip(usu)));
            dao.close();
            return result;
        }

        [WebMethod(Description = "Obtiene los datos de un usuario de acuerdo a su codigo")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DatosUsuario(string usu)
        {
            Usuario usuario = getUsuario(usu);
            //SqlCommand query = new SqlCommand("select Tipousuario, Nombre, Permisos, FiltroDefault FROM Usuarios WHERE Codigo=" + Utilidades.Crip(usu),con);
            //SqlDataReader reader;
            //con.Open();
            //reader = query.ExecuteReader();
            //reader.Read();
            List<string> lpermisos = new List<string>();
            foreach (Permiso permiso in Enum.GetValues(typeof(Permiso)))
            {
                if (usuario.tienePermiso(permiso))
                    lpermisos.Add(((int)permiso).ToString());
            }
            string[] permisos = lpermisos.ToArray();
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("co", usu);
            result.Add("tu", usuario.tipoUsuario);
            result.Add("ne", usuario.nombre);
            result.Add("ps", permisos);
            result.Add("fo", usuario.filtroDefault);
            //reader.Close();
            //con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(result);
            return strJSON;
        }
        
        private int CreaTicket(string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, DateTime lim, string ina, DateTime ini, int pri)
        {
            try
            {
                Ticket ticket = new Ticket();
                ticket.estado = 2;
                ticket.asunto = asu;
                ticket.detalle = dasu;
                ticket.levantador = new Usuario(usu);
                ticket.responsable = new Usuario(res);
                Utilidades.log(sol);
                int tipo = Convert.ToInt32(sol.Substring(0, 1));
                int solicitante = Convert.ToInt32(sol.Substring(1));
                ticket.solicitante = new Solicitante(solicitante, tipo);
                ticket.tipificación = new Tipificación(tip);
                ticket.límite = lim;
                ticket.prioridad = (Prioridad)pri;
                ticket.apertura = DateTime.Now;
                if ((rin == "si")||(rin=="1"))
                {
                    ticket.estado = 4;
                    ticket.resueltoIngresar = true;
                    ticket.solución = solu;
                    ticket.conclusión = new Tipificación(conc);
                    ticket.clausura = DateTime.Now; 
                }
                else if ((ina == "si")||(ina=="1"))
                {
                    ticket.estado = 1;
                    ticket.apertura = ini;
                }
                Utilidades.log("Intentara guardar una nueva incidencia");
                return ticket.guardar();
            }
            catch (InvalidCastException e)
            {
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error de conversion: " + e.ToString());
                dao.close();
                return 500;
            }
            
        }
        /*
        private int CreaTicket(string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, DateTime lim, string ina, DateTime ini, int pri)
        {
        }
        */
        private string notificaTicket(int codigo)
        {
            string result = "No hay ticket con ese código";
            SqlDataReader Reader;
            SqlCommand Qry = new SqlCommand("SELECT  ISNULL(CASE i.TipoSolicitante  " +
                "		WHEN 1 THEN un.Alias  " +
                        "WHEN 2 THEN gs.Nombre  " +
                        "WHEN 3 THEN zo.Nombre  " +
                        "WHEN 4 THEN u3.Login  " +
                        "WHEN 5 THEN u4.Login  " +
                        "WHEN 6 THEN ss.Nombre  " +
                    "END,'') AS SolNombre " +
                    ",ISNULL(u2.Login,'') as LevNombre, i.Responsable, i.Asunto, i.DetalleAsunto, ISNULL(u1.Correo,''), ISNULL(u1.Celular,''), u1.sms, i.CodigoCompuesto " +
            "FROM 	Incidencias i  " +
                "INNER JOIN Usuarios u1 on i.responsable=u1.codigo  " +
                "INNER JOIN Usuarios u2 on i.levanto=u2.codigo  " +
                "LEFT JOIN Unidades un on un.codigo=i.solicitante  " +
                "LEFT JOIN Internos it on it.codigo=i.solicitante  " +
                "LEFT JOIN GruposSolucion gs on gs.codigo=i.solicitante  " +
                "LEFT JOIN Zonas zo on zo.codigo=i.solicitante  " +
                "LEFT JOIN Sitios ss on ss.codigo=i.solicitante  " +
                "LEFT JOIN Usuarios u3 on u3.codigo=i.solicitante  " +
                "LEFT JOIN Usuarios u4 on it.usuario=u4.codigo  " +
                "WHERE i.Codigo=" + codigo.ToString(), con);

            Utilidades.log(Qry.CommandText);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            con.Open();
            try
            {
                Reader = Qry.ExecuteReader();
                if (Reader.Read())
                {
                    string solicitante = Reader.GetString(0).Trim();
                    string levantador = Reader.GetString(1).Trim();
                    int responsable = Reader.GetInt32(2);
                    string asunto = Reader.GetString(3).Trim();
                    string detalle = Reader.GetString(4).Trim();
                    string[] listaMails = Reader.GetString(5).Trim().Split(';');
                    string cels = Reader.GetString(6).Trim();
                    bool sms = Reader.GetBoolean(7);
                    string noinc = Reader.GetString(8);
                    Reader.Close();
                    result = noinc;
                    MailAddressCollection para = new MailAddressCollection();
                    foreach (string mai in listaMails)
                        if (mai.IndexOf('@') > 0)
                            para.Add(new MailAddress(mai.Trim()));

                    if (System.Configuration.ConfigurationManager.AppSettings["SendEmail"].ToString().ToUpper().Trim().Equals("TRUE"))
                    {
                        try
                        {

                            Utilidades.EnviaMail(para, "Se le ha asignado " + Utilidades.un + " " + Utilidades.ticket, Utilidades.MailIncidencia("Estimado usuario, se le ha asignado #un# #ticket#, solicitad#mascfem# por " + solicitante + ", #el# mism#mascfem# fue levantad#mascfem# por " + levantador + ". A continuación se muestra el contenido.", asunto, detalle, noinc), true);
                        }
                        catch (Exception e)
                        {
                            result = "!" + e.Message + "?" + result;
                        }
                    }
                    if (sms)
                    {
                        string contenido;
                        string[] listaCels = cels.Split(';');
                        contenido = solicitante + "-> " + asunto.ToUpper() + " " + detalle.ToLower();
                        if (100 - levantador.Length - 5 < contenido.Length)
                            contenido = contenido.Remove(100 - levantador.Length - 5);
                        contenido += " LEV:" + levantador;
                        foreach (string cel in listaCels)
                            try
                            {
                                Utilidades.EnviaSMS(cel.Trim(), contenido);
                            }
                            catch (Exception e)
                            {
                                result = "!!" + cel.Trim() + "!" + e + "?" + result;
                            }
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        private string notificaActivacion(int codigo)
        {
            string result = "No hay ticket con ese código";
            SqlDataReader Reader;
            SqlCommand Qry = new SqlCommand("SELECT  ISNULL(CASE i.TipoSolicitante  " +
                "		WHEN 1 THEN un.Alias  " +
                        "WHEN 2 THEN gs.Nombre  " +
                        "WHEN 3 THEN zo.Nombre  " +
                        "WHEN 4 THEN u3.Login  " +
                        "WHEN 5 THEN u4.Login  " +
                        "WHEN 6 THEN ss.Nombre  " +
                    "END,'') AS SolNombre " +
                    ",ISNULL(u2.Login,'') as LevNombre, i.Responsable, i.Asunto, i.DetalleAsunto, ISNULL(u1.Correo,''), ISNULL(u1.Celular,''), u1.sms, i.CodigoCompuesto " +
            "FROM 	Incidencias i  " +
                "INNER JOIN Usuarios u1 on i.responsable=u1.codigo  " +
                "INNER JOIN Usuarios u2 on i.levanto=u2.codigo  " +
                "LEFT JOIN Unidades un on un.codigo=i.solicitante  " +
                "LEFT JOIN Internos it on it.codigo=i.solicitante  " +
                "LEFT JOIN GruposSolucion gs on gs.codigo=i.solicitante  " +
                "LEFT JOIN Zonas zo on zo.codigo=i.solicitante  " +
                "LEFT JOIN Sitios ss on ss.codigo=i.solicitante  " +
                "LEFT JOIN Usuarios u3 on u3.codigo=i.solicitante  " +
                "LEFT JOIN Usuarios u4 on it.usuario=u4.codigo  " +
                "WHERE i.Codigo=" + codigo.ToString(), con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            con.Open();
            try
            {
                Reader = Qry.ExecuteReader();
                if (Reader.Read())
                {
                    string solicitante = Reader.GetString(0).Trim();
                    string levantador = Reader.GetString(1).Trim();
                    int responsable = Reader.GetInt32(2);
                    string asunto = Reader.GetString(3).Trim();
                    string detalle = Reader.GetString(4).Trim();
                    string[] listaMails = Reader.GetString(5).Trim().Split(';');
                    string cels = Reader.GetString(6).Trim();
                    bool sms = Reader.GetBoolean(7);
                    string noinc = Reader.GetString(8);
                    Reader.Close();
                    result = noinc;
                    MailAddressCollection para = new MailAddressCollection();
                    foreach (string mai in listaMails)
                        if (mai.IndexOf('@') > 0)
                            para.Add(new MailAddress(mai.Trim()));
                    try
                    {
                        Utilidades.EnviaMail(para, "Se le ha activado " + Utilidades.un + " " + Utilidades.ticket, Utilidades.MailIncidencia("Estimado usuario, se activo #un# #ticket#, solicitad#mascfem# por " + solicitante + ", #el# mism#mascfem# fue levantad#mascfem# por " + levantador + ", y que se dejo en espera hasta esta fecha. A continuación se muestra el contenido.", asunto, detalle, noinc), true);
                    }
                    catch (Exception e)
                    {
                        result = "!" + e.Message + "?" + result;
                    }
                    if (sms)
                    {
                        string contenido;
                        string[] listaCels = cels.Split(';');
                        contenido = solicitante + "-> " + asunto.ToUpper() + " " + detalle.ToLower();
                        if (100 - levantador.Length - 5 < contenido.Length)
                            contenido = contenido.Remove(100 - levantador.Length - 5);
                        contenido += " LEV:" + levantador;
                        foreach (string cel in listaCels)
                            try
                            {
                                Utilidades.EnviaSMS(cel.Trim(), contenido);
                            }
                            catch (Exception e)
                            {
                                result = "!!" + cel.Trim() + "!" + e + "?" + result;
                            }
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        private string respuestaTicket(string num)
        {
            string result = ConfigurationManager.AppSettings["ticket"] + " guardad" + Utilidades.mascfem + " con código " + num.Substring(num.LastIndexOf('?') + 1) + ".";
            if (num[0] != '!')
            {
                return result;
            }
            else if (num[1] != '!')
            {
                dao.open();
                result+=" Disculpe, no pude enviar el correo electrónico. Código de error: " + dao.logea("Error al levantar ticket, error de e-mail: " + num);
                dao.close();
                return result;
            }
            else
            {
                string cels = "";
                int i = 0;
                bool escribe = false;
                while (i < num.Length && num[i] != '?')
                {
                    if (num[i] == '!')
                    {
                        if (i > 0 && num[i - 1] == '!') escribe = true;
                        else
                        {
                            escribe = false;
                            if (i < num.Length - 1 && num[i + 1] != '!') cels += ", ";
                        }
                    }
                    else if (escribe)
                        cels += num[i];
                    i++;
                }
                if (cels.Length > 2) cels = cels.Remove(cels.Length - 2);
                if (num.Contains("?!"))
                {
                    dao.open();
                    result += " Disculpe, no pude enviar el correo electrónico, además no pude enviar mensajes SMS a los siguientes celulares: " + cels + ". Código de error: " + dao.logea("Error al levantar ticket, error de e-mail y SMS: " + num);
                    dao.close();
                    return result;
                }
                else
                {
                    dao.open();
                    result += " Disculpe, no pude enviar mensajes SMS a los siguientes celulares: " + cels + ". Código de error: " + dao.logea("Error al levantar ticket, error de SMS: " + num);
                    dao.close();
                    return result;
                }
            }
        }

        [WebMethod(Description = "Nuevo Ticket según la unidad que se proporciona")]
        public string NuevoTicketUnidad(string asu, string dasu, string login)
        {
            string result = "No hay usuario con ese login";
            SqlDataReader Reader;
            int soli=0,resp=0;
            string qry = "SELECT  u.Codigo as [solicitante],us2.Codigo as [RESP] " +
                          "FROM dbo.Sitios AS s  INNER JOIN " +
                          "    dbo.Unidades AS u ON u.Sitio = s.Codigo INNER JOIN " +
                          "   dbo.Requirientes AS r ON u.Codigo = r.unidad INNER JOIN " +
                          "  dbo.Usuarios AS usuUni ON r.usuario = usuUni.Codigo INNER JOIN " +
                          "    dbo.Clientes AS c ON u.Cliente = c.Codigo INNER JOIN " +
                          "    dbo.Usuarios AS us ON c.Usuario = us.Codigo INNER JOIN " +
                          "    dbo.Internos AS i ON s.Responsable = i.Codigo INNER JOIN " +
                          "    dbo.Usuarios AS us2 ON i.Usuario = us2.Codigo " +
                          "  WHERE  usuUni.Login='" + login + "'";
            SqlCommand Qry = new SqlCommand(qry, con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            con.Open();
            try
            {
                Reader = Qry.ExecuteReader();
                if (Reader.Read())
                {
                    soli = Reader.GetInt32(0);
                    resp = Reader.GetInt32(1);
                    con.Close();
                }
                else{
                    con.Close();
                    return result;
                }
                
            }
            catch (Exception e)
            {
                dao.open();
                result="Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
            }
            //Ahora levantamos el ticket
            try
            {
                return respuestaTicket(notificaTicket(CreaTicket(asu, dasu, resp, resp,"4"+ soli, 1, "0", 0, "", DateTime.Now.AddHours(4), "0",DateTime.Now, 2)));
                //string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, DateTime lim, string ina, DateTime ini, int pri
            }
            catch (Exception e)
            {
                dao.open();
                result="Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
            }

            return result;
            
        }


        [WebMethod(Description = "Nuevo Ticket según el usuario que se proporciona")]
        public string NuevoTicketUsuario(string asu, string dasu, string loginResp, string loginLevanta)
        {
            string result = "No hay usuario con ese login";
            SqlDataReader Reader;
            int resp = 0, levanta = 0;
            string strResponsable = "";
            string qry = "select codigo, nombre from Usuarios  WHERE  Login='" + loginResp + "'";
            SqlCommand Qry = new SqlCommand(qry, con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            // Obtenememos el responsable de la incidencia
            con.Open();
            try
            {
                Reader = Qry.ExecuteReader();
                if (Reader.Read())
                {
                    resp = Reader.GetInt32(0);
                    strResponsable = Reader.GetString(1);
                    con.Close();
                }
                else
                {
                    con.Close();
                    return result;
                }

            }
            catch (Exception e)
            {
                dao.open();
                result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
            }


            //Obtenemos el que levanta la incidencia
            qry = "select codigo from Usuarios  WHERE  Login='" + loginLevanta+ "'";
            Qry = new SqlCommand(qry, con);
            con.Open();
            try
            {
                Reader = Qry.ExecuteReader();
                if (Reader.Read())
                {
                    levanta = Reader.GetInt32(0);
                    con.Close();
                }
                else
                {
                    con.Close();
                    return result;
                }

            }
            catch (Exception e)
            {
                dao.open();
                result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
            }

            if (resp == 0) 
                return "No existe el responsable" ;
            if (levanta == 0)
                return "No existe el solicitante";

            //Ahora levantamos el ticket
            try
            {
                string respuesta = notificaTicket(CreaTicket(asu, dasu, levanta, resp, "4106" , 0, "0", 0, "", DateTime.Now.AddHours(4), "0", DateTime.Now, 2));
                return "creada:"+ respuesta + ":" + strResponsable;
                //string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, DateTime lim, string ina, DateTime ini, int pri
            }
            catch (Exception e)
            {
                dao.open();
                result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
            }

            return result;

        }

        [WebMethod(Description = "Nuevo Ticket")]
        public string NuevoTicket(string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, DateTime lim, string ina, DateTime ini, int pri)
        {
            string result;
            int TipiImplHmtx = 1129;//Este es el código para la implementación de Hmatix
            int TipiImpltPPrivados = 1181;
            int TipiImpltPPublicos = 1185;
            int Codigo = -1;
            try
            {
                if ((TipiImplHmtx == tip) || (TipiImpltPPrivados == tip) || (TipiImpltPPublicos == tip))
                {
                    SqlDataReader reader;
                    con.Open();
                    SqlCommand Qry;

                    string strQry = "Select Nombre from unidades where codigo = " + Convert.ToString(sol).Substring(1);
                    Qry = new SqlCommand(strQry, con);
                    try
                    {
                        Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                    }
                    catch
                    {
                        Qry.CommandTimeout = 300;
                    }
                    Utilidades.log(strQry);
                    reader = Qry.ExecuteReader();
                    string nombreUnidad = "";
                    while (reader.Read())
                    {
                        nombreUnidad = reader.GetString(0);
                    }
                    con.Close();
                    Codigo = CreaTicket("SE SOLICITA REALIZAR INSTALACION EN LA UNIDAD:", nombreUnidad, usu, res, sol, tip, rin, conc, solu, lim, ina, ini, pri);
                    result = respuestaTicket(notificaTicket(Codigo));
                }
                else
                {
                    Codigo = CreaTicket(asu, dasu, usu, res, sol, tip, rin, conc, solu, lim, ina, ini, pri);
                    return respuestaTicket(notificaTicket(Codigo));
                }
            }
            catch (Exception e)
            {
                dao.open();
                result="Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                    dao.close();
            }
            if (TipiImplHmtx == tip) 
            {
                result = Crea_Implementacion(Codigo, usu, res, sol, 34, rin, conc, solu, lim, ina, ini, pri);
            }
            else if   (TipiImpltPPrivados == tip) 
            {
                result = Crea_Implementacion(Codigo, usu, res, sol, 58, rin, conc, solu, lim, ina, ini, pri);
            }
            else if   (TipiImpltPPublicos == tip) 
            {
                result = Crea_Implementacion(Codigo, usu, res, sol, 59, rin, conc, solu, lim, ina, ini, pri);
            }
            return result;
        }

        [WebMethod(Description = "Nuevo Ticket")]
        public string NuevoTicket(string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, DateTime lim, string ina, DateTime ini, int pri, string val, int vdr)
        {
            string result;
            int TipiImplHmtx = 1129;//Este es el código para la implementación de Hmatix
            int TipiImpltPPrivados = 1181;
            int TipiImpltPPublicos = 1185;
            int Codigo = -1;
            try
            {
                if ((TipiImplHmtx == tip) || (TipiImpltPPrivados == tip) || (TipiImpltPPublicos == tip))
                {
                    SqlDataReader reader;
                    con.Open();
                    SqlCommand Qry;

                    string strQry = "Select Nombre from unidades where codigo = " + Convert.ToString(sol).Substring(1);
                    Qry = new SqlCommand(strQry, con);
                    try
                    {
                        Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                    }
                    catch
                    {
                        Qry.CommandTimeout = 300;
                    }
                    Utilidades.log(strQry);
                    reader = Qry.ExecuteReader();
                    string nombreUnidad = "";
                    while (reader.Read())
                    {
                        nombreUnidad = reader.GetString(0);
                    }
                    con.Close();
                    Codigo = CreaTicket("SE SOLICITA REALIZAR INSTALACION EN LA UNIDAD:", nombreUnidad, usu, res, sol, tip, rin, conc, solu, lim, ina, ini, pri);
                    result = respuestaTicket(notificaTicket(Codigo));
                }
                else
                {
                    Codigo = CreaTicket(asu, dasu, usu, res, sol, tip, rin, conc, solu, lim, ina, ini, pri);
                    return respuestaTicket(notificaTicket(Codigo));
                }
            }
            catch (Exception e)
            {
                dao.open();
                result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
            }
            if (TipiImplHmtx == tip)
            {
                result = Crea_Implementacion(Codigo, usu, res, sol, 34, rin, conc, solu, lim, ina, ini, pri);
            }
            else if (TipiImpltPPrivados == tip)
            {
                result = Crea_Implementacion(Codigo, usu, res, sol, 58, rin, conc, solu, lim, ina, ini, pri);
            }
            else if (TipiImpltPPublicos == tip)
            {
                result = Crea_Implementacion(Codigo, usu, res, sol, 59, rin, conc, solu, lim, ina, ini, pri);
            }
            return result;
        }



        //[WebMethod(Description = "Nuevo Ticket")]
        //public string NuevoTicket(string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, DateTime lim, string ina, DateTime ini, int pri, string val, int vdr)
        //{
        //}

        [WebMethod(Description = "Nuevo Ticket Derivado", MessageName = "NuevoTicketDerivado")]
        public string NuevoTicketDerivado(string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, string origen, DateTime lim, string ina, DateTime ini, int pri)
        {
            try
            {
                Utilidades.log("Se recibe apertura: " + ini.ToString());
                Utilidades.log("Se recibe limite: " + lim.ToString());
                int codigo = CreaTicket(asu, dasu, usu, res, sol, tip, rin, conc, solu, lim, ina, ini, pri);
                con.Open();
                SqlCommand Qry;
                Qry = new SqlCommand();
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                }
                Qry.Connection = con;
                Qry.CommandText =
                    "INSERT Dependencias (Origen,Derivado) VALUES(dbo.CodigoSimple('" + origen + "')," + codigo.ToString() + ");";
                Utilidades.log(Qry.CommandText);
                Qry.ExecuteNonQuery();
                con.Close();
                return respuestaTicket(notificaTicket(codigo));
            }
            catch (Exception e)
            {
                try
                {
                    con.Close();
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket derivado:" + e.Message + " StackTrace:" + e.StackTrace);
                    dao.close();
                    Utilidades.log("Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket derivado:" + e.Message + " StackTrace:" + e.StackTrace));
                    return result;
                }
                catch
                {
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket derivado:" + e.Message + " StackTrace:" + e.StackTrace);
                    dao.close();
                    Utilidades.log("Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket derivado:" + e.Message + " StackTrace:" + e.StackTrace));
                    return result;
                }
            }
        }

        [WebMethod(Description = "Obtiene los datos adjuntos de un ticket")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DatosCodigo(string cod, string usu)
        {
            con.Open();
            Usuario usuario = getUsuario(usu);
            if (!usuario.tienePermiso( Permiso.VerDatosExtra))
            {
                con.Close();
                return "No tiene permiso";
            }
            SqlCommand query = new SqlCommand("DECLARE @cod int SET @cod=dbo.CodigoSimple('" + cod + "') Select Datos from Incidencias WHERE Codigo=@cod", con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            string result = query.ExecuteScalar().ToString();
            con.Close();
            return result;
        }

        private string Select(int maximumRows, Usuario usuario)
        {
            SqlCommand query = new SqlCommand("", con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            string det = "''";
            //dao.open();
            if (usuario.tienePermiso(Permiso.VerDetalleDeTickets))
            {
                //dao.close();
                det = "ISNULL(i.DetalleAsunto,'')";
            }
            //else
            //    dao.close();
            string result;
            result = "SELECT top " + maximumRows.ToString() +
                    " i.CodigoCompuesto as rCodigoCompuesto" +
                    ",i.Estado as rEstado" +
                    ",ISNULL(i.Asunto,'') as rAsunto " +
                    "," + det + " as rDetalle" +
                    ",ISNULL(i.DetalleSolucion,'') as rSolucion " +
    ", ISNULL(CONVERT(char(11),i.Apertura,106) + ' ' + CONVERT(char(5),i.Apertura,114),'') as rApertura " +
    ", ISNULL(CONVERT(char(11),i.Clausura,106) + ' ' + CONVERT(char(5),i.Clausura,114),'') as rClausura " +
    ", ISNULL(CONVERT(char(11),i.Limite,106) + ' ' + CONVERT(char(5),i.Limite,114),'') as rLimite " +
                    ",i.Responsable as rResCodigo " +
                    ",ISNULL(u.login,'') as rResNombre " +
                    ",i.TipoSolicitante as rTipoSolicitante" +
                    ",i.Solicitante as rSolCodigo " +
                    ", ISNULL(CASE i.TipoSolicitante  " +
                "		WHEN 1 THEN un.Nombre  " +
                        "WHEN 2 THEN gs.Nombre  " +
                        "WHEN 3 THEN zo.Nombre  " +
                        "WHEN 4 THEN u3.Nombre  " +
                        "WHEN 5 THEN u4.Nombre  " +
                        "WHEN 6 THEN ss.Nombre  " +
                    "END,'') AS rSolNombre " +
                    ",i.Levanto as rLevCodigo " +
                    ",ISNULL(u2.Nombre,'') as rLevNombre " +
                    ",i.Tipificacion as rTipCodigo " +
                    ",ISNULL(dbo.Tipificacion(i.Tipificacion),'') as rTipNombre " +
                //",1 as Validaciones " +
                    ",i.Codigo as rCodigo" +
                    ",dbo.Derivados(i.Codigo) as rDerivados " +
    ", ISNULL(DATEDIFF(minute,GETDATE(),i.Limite),1000000) as rLimitev " +
    ", i.Severidad as rSeveridad " +
            "FROM 	Incidencias i " +
                "INNER JOIN Usuarios u on i.responsable=u.codigo  " +
                "INNER JOIN Usuarios u2 on i.levanto=u2.codigo  " +
                "LEFT JOIN Unidades un on un.codigo=i.solicitante  " +
                "LEFT JOIN Internos it on it.codigo=i.solicitante  " +
                "LEFT JOIN GruposSolucion gs on gs.codigo=i.solicitante  " +
                "LEFT JOIN Zonas zo on zo.codigo=i.solicitante  " +
                "LEFT JOIN Sitios ss on ss.codigo=i.solicitante  " +
                "LEFT JOIN Usuarios u3 on u3.codigo=i.solicitante  " +
                "LEFT JOIN Usuarios u4 on it.usuario=u4.codigo  ";
            return result;
        }

        private string Filtro(string filtro, Usuario usuario)
        {
            //string us = Utilidades.Crip(usu);
            string restric;
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            query.CommandText = "Select PuedeVer From Usuarios where Codigo=" + usuario.código;
            restric = "(" + query.ExecuteScalar().ToString() + ") ";
            string bus = "";
            string bin;
            string str;
            string[] filtros = filtro.Split('!');
            if (filtros[0] != "")
            {
                string[] dropUsu = filtros[0].Split('.');
                if (dropUsu[0] != "")
                {
                    bus += "i.Codigo in (SELECT tr.Incidencia from Traza tr WHERE tr.Usuario=" + dropUsu[0];
                    bin = Convert.ToString(Convert.ToInt32(dropUsu[1]), 2);
                    str = new String('0', 6 - bin.Length);
                    bin = str + bin;
                    str = "";
                    for (int i = 1; i <= 6; i++)
                    {
                        if (bin[6 - i] == '1')
                            str = str + "," + i.ToString();
                    }
                    if (str != "")
                    {
                        str = str.Substring(1);
                        bus += " AND tr.Tipo IN (" + str + "))";
                    }
                    else bus += " AND 1=0)";
                }
            }
            if (filtros[1] != "")
            {
                string[] solicitantes = filtros[1].Split(',');
                if (bus != "")
                    bus += " AND ";
                bus += "(";
                foreach (string solicitante in solicitantes)
                {
                    bus += "i.TipoSolicitante=" + solicitante.Substring(0, 1) + " AND i.Solicitante=" + solicitante.Substring(1) + " OR ";
                }
                bus = bus.Substring(0, bus.Length - 4) + ")";
            }
            if (filtros[2] != "")
            {
                if (bus != "")
                    bus = "(" + bus + ")";
                else bus = "1=1";
                bin = Convert.ToString(Convert.ToInt32(filtros[2]), 2);
                str = new String('0', 5 - bin.Length);
                bin = str + bin;
                str = "";
                for (int i = 1; i <= 5; i++)
                {
                    if (bin[5 - i] == '1')
                        str = str + "," + i.ToString();
                }
                if (str != "")
                {
                    str = str.Substring(1);
                    bus += " AND i.Estado IN (" + str + ")";
                }
                else bus += " AND 1=0";
            }
            if (filtros[3] != "")
            {
                string[] fechas = filtros[3].Split('.');
                if (bus != "")
                    bus += " AND ";
                string between = "BETWEEN convert(datetime,'" + fechas[0] + "',103) AND convert(datetime,'" + fechas[1] + "',103)";
                switch (fechas[2])
                {
                    case "0":
                        bus += "i.Apertura " + between;
                        break;
                    case "1":
                        bus += "i.Clausura " + between;
                        break;
                    default:
                        bus += "i.Limite " + between;
                        break;
                }
            }
            if (filtros[4] != "")
            {
                if (bus != "")
                    bus += " AND ";
                bus += "i.Tipificacion in (" + filtros[4] + ")";
            }
            if (filtros[5] != "")
            {
                if (bus != "")
                    bus = "(" + bus + ")";
                else bus = "1=1";
                bin = Convert.ToString(Convert.ToInt32(filtros[5]), 2);
                str = new String('0', 5 - bin.Length);
                bin = str + bin;
                str = "";
                for (int i = 1; i <= 5; i++)
                {
                    if (bin[5 - i] == '1')
                        str = str + "," + i.ToString();
                }

                if (str != "")
                {
                    str = str.Substring(1);
                    bus += " AND i.Severidad IN (" + str + ")";
                }
                else bus += " AND 1=0";

            }
            if (filtros[6] != "")
            {
                if (bus != "")
                    bus += " AND ";
                string busqueda = filtros[6].Trim();
                string[] busquedas = busqueda.Split(',');
                query.CommandText = "Select";
                foreach (string b in busquedas)
                    query.CommandText += " dbo.CodigoSimple('" + b + "'),";
                query.CommandText = query.CommandText.Remove(query.CommandText.Length - 1);
                SqlDataReader reader = query.ExecuteReader();
                try
                {
                    reader.Read();
                    string codigos = "(";
                    for (int i = 0; i < busquedas.Length; i++)
                    {
                        try { codigos += reader.GetInt32(i).ToString() + ","; }
                        catch { }
                    }
                    reader.Close();
                    string generacion = codigos.Remove(codigos.Length - 1) + ")";
                    do
                    {
                        query.CommandText = "SELECT Derivado from Dependencias WHERE Origen in " + generacion;
                        reader = query.ExecuteReader();
                        generacion = "";
                        while (reader.Read())
                        {
                            generacion += reader.GetInt32(0).ToString() + ",";
                        }
                        reader.Close();
                        codigos = codigos + generacion;
                        if (generacion.Length > 0)
                            generacion = "(" + generacion.Remove(generacion.Length - 1) + ")";
                    }
                    while (generacion.Length > 0);
                    codigos = codigos.Remove(codigos.Length - 1) + ")";
                    bus = "i.Codigo in " + codigos + "";
                }
                catch
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    busqueda = busqueda.Replace("*", "");
                    bus += "(i.Asunto LIKE '%" + busqueda + "%' OR i.DetalleAsunto LIKE '%" + busqueda + "%' OR i.Datos LIKE '%:\"" + busqueda + "%')";
                }
            }
            if (bus != "")
                restric = restric + " AND (" + bus + ")";
            return restric;
        }

        private string order(Usuario usuario)
        {
            if (!usuario.tienePermiso(Permiso.OrdenarTicketsPorCodigo))
            {
                return
                    "i.Severidad, " +
                    "case i.Estado when 3 then 1 when 1 then 3 else i.Estado end, " +
                    "case i.estado " +
                        "when 1 then convert(varchar(20), i.Apertura, 120) " +
                        "when 2 then convert(varchar(20), i.Limite, 120) " +
                        "when 3 then convert(varchar(20), i.Limite, 120) " +
                        "else '0' end, " +
                    "convert(varchar(20),i.Clausura,120) desc, " +
                    "i.Codigo desc";
            }
            else
            {
                return "i.Severidad ASC,i.Apertura desc, i.Codigo desc";
            }
        }

        private string order(Usuario usuario, string sort)
        {
            string result = sort;
            result = result.Replace("rSeveridad", "i.Severidad");
            result = result.Replace("CodigoCompuesto", "i.CodigoCompuesto");
            result = result.Replace("Asunto", "i.Asunto");
            result = result.Replace("Apertura", "i.Apertura");
            result = result.Replace("Clausura", "i.Clausura");
            result = result.Replace("ResNombre", "u.login");
            result = result.Replace("SolNombre", "CASE i.TipoSolicitante  " +
                "		WHEN 1 THEN un.Nombre  " +
                        "WHEN 2 THEN gs.Nombre  " +
                        "WHEN 3 THEN zo.Nombre  " +
                        "WHEN 4 THEN u3.Nombre  " +
                        "WHEN 5 THEN u4.Nombre  " +
                        "WHEN 6 THEN ss.Nombre  " +
                    "END");
            result = result.Replace("Estado", "i.Estado");
            result = result.Replace("Limcla", "CASE i.Estado WHEN 4 THEN i.Clausura WHEN 5 THEN i.Clausura ELSE i.Limite END");
            return result + ", i.Codigo desc";
        }

        private string orderCompuesto(Usuario usuario, string sort)
        {
            string result = sort;
            result = result.Replace("rSeveridad", "i.Severidad");
            result = result.Replace("CodigoCompuesto", "i.CodigoCompuesto");
            result = result.Replace("Asunto", "i.Asunto");
            result = result.Replace("Apertura", "i.Apertura");
            result = result.Replace("Clausura", "i.Clausura");
            result = result.Replace("ResNombre", "u.login");
            result = result.Replace("SolNombre", "CASE i.TipoSolicitante  " +
                "		WHEN 1 THEN un.Nombre  " +
                        "WHEN 2 THEN gs.Nombre  " +
                        "WHEN 3 THEN zo.Nombre  " +
                        "WHEN 4 THEN u3.Nombre  " +
                        "WHEN 5 THEN u4.Nombre  " +
                        "WHEN 6 THEN ss.Nombre  " +
                    "END");
            result = result.Replace("Estado", "i.Estado");
            result = result.Replace("Limcla", "CASE i.Estado WHEN 4 THEN i.Clausura WHEN 5 THEN i.Clausura ELSE i.Limite END");
            return result;
        }

        private string comando(int startRowIndex, int maximumRows, string filtro, Usuario usuario)
        {
            string strRes =
             Select(maximumRows, usuario) +
                "WHERE i.Codigo NOT IN (SELECT top " + startRowIndex.ToString() + " i.Codigo from Incidencias i  " +
                    "where " + Filtro(filtro, usuario) + " order by " + order(usuario) + ") and " + Filtro(filtro, usuario) +
                " ORDER BY " + order(usuario);
            Utilidades.log(strRes);
            return strRes;
        }

        private string comando(int startRowIndex, int maximumRows, string filtro, Usuario usuario, string sort)
        {
            //string us = Utilidades.Crip(usu);
            string strRes = Select(maximumRows, usuario) +
                "WHERE i.Codigo NOT IN (SELECT top " + startRowIndex.ToString() + " i.Codigo from Incidencias i  " +
                    "where " + Filtro(filtro, usuario) + " order by " + order(usuario, sort) + ") and " + Filtro(filtro, usuario) +
                " ORDER BY " + order(usuario, sort);
            Utilidades.log(strRes);
            return strRes;
        }

        private string comandoCompuesto(int startRowIndex, int maximumRows, string filtro, Usuario usuario, string sort)
        {
            //string us = Utilidades.Crip(usu);
            return Select(maximumRows, usuario) +
                "WHERE i.Codigo NOT IN (SELECT top " + startRowIndex.ToString() + " i.Codigo from Incidencias i  " +
                    "where " + Filtro(filtro, usuario) + " order by " + order(usuario, sort) + ") and " + Filtro(filtro, usuario) +
                " ORDER BY " + orderCompuesto(usuario, sort);
        }

        private object Vals(string cod)
        {
            SqlDataReader reader;
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            List<Dictionary<string, string>> Validaciones = new List<Dictionary<string, string>>();
            query.Connection = con;
            query.CommandText = "SELECT Datos, ISNULL(CONVERT(char(10), Fecha, 103) + ' ' + CONVERT(char(5), Fecha, 114), '') AS Expr1, TipoEvento FROM Eventos e WHERE (Incidencia = " + cod + ") and TipoEvento in (4,5)";
            reader = query.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    string strjs = reader.GetString(0);
                    JObject datosEv;
                    try
                    {
                        datosEv = JObject.Parse(strjs);
                    }
                    catch
                    {
                        reader.Close();
                        throw new Exception("Error en método Vals: Error JSON en datos de eventos de ticket " + cod + ":" + strjs);
                    }
                    int vr = (int)datosEv["usu"];
                    Dictionary<string, string> validacion = new Dictionary<string, string>();
                    validacion.Add("vr", vr.ToString() + "," + reader.GetString(1));
                    if (reader.GetInt32(2) == 4)
                        validacion.Add("ar", "a");
                    else
                        validacion.Add("ar", "r");
                    Validaciones.Add(validacion);
                }
                reader.Close();
                foreach (Dictionary<string, string> validacion in Validaciones)
                {
                    string[] vr = validacion["vr"].Split(',');
                    query.CommandText = "select u.nombre from Usuarios u where u.Codigo=" + vr[0];
                    validacion["vr"] = (string)query.ExecuteScalar() + "," + vr[1];
                }
                return Validaciones;
            }
            catch (Exception e)
            {
                reader.Close();
                throw new Exception("Error en método Vals: " + e.Message);
            }
        }

        private List<TicketPreJSON> listaTickets(SqlDataReader reader, Usuario usuario)
        {
            List<TicketPreJSON> result = new List<TicketPreJSON>();
            TicketPreJSON inci;
            while (reader.Read())
            {
                inci = new TicketPreJSON(reader.GetString(2));
                inci.co = reader.GetString(0);
                inci.eo = reader.GetInt32(1);
                inci.de = reader.GetString(3);
                inci.sn = reader.GetString(4);
                if (reader.GetString(5).Length > 9)
                    inci.aa = reader.GetString(5).Substring(0, 2) + reader.GetString(5).Substring(3, 3).ToLower() + reader.GetString(5).Substring(9);
                else
                    inci.aa = "";
                if (reader.GetString(6).Length > 9)
                    inci.ca = reader.GetString(6).Substring(0, 2) + reader.GetString(6).Substring(3, 3).ToLower() + reader.GetString(6).Substring(9);
                else
                    inci.ca = "";
                int minlim = reader.GetInt32(19);
                string vence = "";
                string fecha = "";
                if (minlim > 999999)
                    vence = "sin límite";
                else if (minlim > 0)
                    vence = Utilidades.MinutosAHumano(minlim);
                else
                    vence = "vencid" + ConfigurationManager.AppSettings["mascfem"];
                if (reader.GetString(7).Length > 9)
                    fecha = reader.GetString(7).Substring(0, 2) + reader.GetString(7).Substring(3, 3).ToLower() + reader.GetString(7).Substring(9);
                inci.le = new KeyValuePair<string, string>(fecha, vence);
                string responsable = reader.GetString(9);
                if (usuario.código == reader.GetInt32(8))
                    responsable = responsable + "*";
                inci.re = new Propiedad(reader.GetInt32(8), responsable);
                inci.ts = reader.GetInt32(10);
                inci.se = new Propiedad(reader.GetInt32(11), reader.GetString(12));
                inci.lr = new Propiedad(reader.GetInt32(13), reader.GetString(14));
                inci.tn = new KeyValuePair<string, string>(reader.GetString(15), reader.GetString(16));
                inci.vs = reader.GetInt32(17).ToString();
                inci.hs = reader.GetInt32(18);
                inci.pd = reader.GetInt32(20);
                result.Add(inci);
                //Utilidades.log("se imprem nuno de los rows q encontro");
                Utilidades.log(inci.co);
            }
            reader.Close();
            foreach (TicketPreJSON tick in result)
                tick.vs = Vals((string)tick.vs);
            return result;
        }

        private List<TicketPreJSON> data(int startRowIndex, int maximumRows, string filtro, Usuario usuario)
        {
            //string us = Utilidades.Crip(usu);
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.CommandTimeout = 10;
            query.Connection = con;
            con.Open();
            query.CommandText = comando(startRowIndex, maximumRows, filtro, usuario);
            Utilidades.log("Se manada a ejecutar sin sort:");
            Utilidades.log(query.CommandText);
            SqlDataReader reader;
            try
            {
                reader = query.ExecuteReader();
                return listaTickets(reader, usuario);
            }
            finally
            {
                con.Close();
            }
        }

        private List<TicketPreJSON> data(int startRowIndex, int maximumRows, string filtro, Usuario usuario, string sort)
        {
            //string us = Utilidades.Crip(usu);
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.CommandTimeout = 10;
            query.Connection = con;
            con.Open();
            query.CommandText = comando(startRowIndex, maximumRows, filtro, usuario, sort);
            Utilidades.log("Se manada a ejecutar el sort:");
            Utilidades.log(query.CommandText);
            SqlDataReader reader;
            try
            {
                reader = query.ExecuteReader();
                return listaTickets(reader, usuario);
            }
            finally
            {
                con.Close();
            }
        }

        private List<TicketPreJSON> dataCompuesto(int startRowIndex, int maximumRows, string filtro, Usuario usuario, string sort)
        {
            //string us = Utilidades.Crip(usu);
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            con.Open();
            query.CommandText = comandoCompuesto(startRowIndex, maximumRows, filtro, usuario, sort);
            SqlDataReader reader;
            try
            {
                reader = query.ExecuteReader();
                return listaTickets(reader, usuario);
            }
            finally
            {
                con.Close();
            }
        }

        private int count(string filtro, Usuario usuario)
        {
            con.Open();
            SqlCommand query = new SqlCommand(
                "Select count(*) from Incidencias i  " +
                    "INNER JOIN Usuarios u on i.responsable=u.codigo  " +
                    "INNER JOIN Usuarios u2 on i.levanto=u2.codigo  " +
                    "LEFT JOIN Unidades un on un.codigo=i.solicitante  " +
                    "LEFT JOIN Internos it on it.codigo=i.solicitante  " +
                    "LEFT JOIN GruposSolucion gs on gs.codigo=i.solicitante  " +
                    "LEFT JOIN Zonas zo on zo.codigo=i.solicitante  " +
                    "LEFT JOIN Clientes cl on cl.codigo=i.solicitante  " +
                    "LEFT JOIN Usuarios u3 on cl.usuario=u3.codigo  " +
                    "LEFT JOIN Usuarios u4 on it.usuario=u4.codigo where " + Filtro(filtro, usuario), con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            int result = (int)query.ExecuteScalar();
            con.Close();
            return result;
        }

        [WebMethod(Description = "Te da los Tickets de acuerdo a ciertos filtros")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDataAndCount(int startRowIndex, int maximumRows, string filtro, string usu)
        {
            try
            {
                Usuario usuario = getUsuario(usu);
                Dictionary<string, object> datos = new Dictionary<string, object>();
                try
                {
                    datos.Add("d", data(startRowIndex, maximumRows, filtro, usuario));
                }
                catch (SqlException e)
                {
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error de sintaxis SQL: " + e.Message);
                    dao.close();
                    return result;
                }
                catch (InvalidCastException e)
                {
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error de conversion: " + e.ToString());
                    dao.close();
                    return result;
                }
                datos.Add("c", count(filtro, usuario));
                JavaScriptSerializer js = new JavaScriptSerializer();
                string strJSON = js.Serialize(datos);
                return strJSON;
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }

        [WebMethod(Description = "Te da los Tickets de acuerdo a ciertos filtros")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDataAndCountSort(int startRowIndex, int maximumRows, string filtro, string usu, string sort)
        {
            try
            {
                Usuario usuario = getUsuario(usu);
                dao.close();
                Dictionary<string, object> datos = new Dictionary<string, object>();
                try
                {
                    datos.Add("d", data(startRowIndex, maximumRows, filtro, usuario, sort));
                }
                catch (SqlException e)
                {
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error de sintaxis SQL: " + e.Message);
                    dao.close();
                    return result;
                }
                catch (InvalidCastException e)
                {
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error de conversion: " + e.ToString());
                    dao.close();
                    return result;
                }
                datos.Add("c", count(filtro, usuario));
                JavaScriptSerializer js = new JavaScriptSerializer();
                string strJSON = js.Serialize(datos);
                return strJSON;
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }

        //Función Agregada por andrés Lara Maldonado al 21/10/2014 para traes todas la incidencias según el filtro especificado
        [WebMethod(Description = "Te da los Tickets de acuerdo a ciertos filtros")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDataAndCountCompleto(string filtro, string usu, string sort)
        {
            try
            {
                Usuario usuario = getUsuario(usu);
                dao.close();
                Dictionary<string, object> datos = new Dictionary<string, object>();
                try
                {
                    datos.Add("d", data(filtro, usuario));
                }
                catch (SqlException e)
                {
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error de sintaxis SQL: " + e.Message);
                    dao.close();
                    return result;
                }
                catch (InvalidCastException e)
                {
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error de conversion: " + e.ToString());
                    dao.close();
                    return result;
                }
                datos.Add("c", count(filtro, usuario));
                JavaScriptSerializer js = new JavaScriptSerializer();
                string strJSON = js.Serialize(datos);
                return strJSON;
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }

        private List<TicketPreJSON> data(string filtro, Usuario usuario)
        {
            //string us = Utilidades.Crip(usu);
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.CommandTimeout = 10;
            query.Connection = con;
            con.Open();
            query.CommandText = comando( filtro, usuario);
            Utilidades.log("Hace el selct de lo que se va a traer");
            Utilidades.log(query.CommandText);
            SqlDataReader reader;
            try
            {
                reader = query.ExecuteReader();
                return listaTickets(reader, usuario);
            }
            finally
            {
                con.Close();
            }
        }

        private string comando( string filtro, Usuario usuario)
        {
            string strRes =
             Select( usuario) +
                "WHERE i.Codigo IN (SELECT i.Codigo from Incidencias i  " +
                    "where " + Filtro(filtro, usuario)  + ") and " + Filtro(filtro, usuario) +
                " ORDER BY " + order(usuario);
            return strRes;
        }

        private string Select( Usuario usuario)
        {
            SqlCommand query = new SqlCommand("", con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            string det = "''";
            //dao.open();
            if (usuario.tienePermiso(Permiso.VerDetalleDeTickets))
            {
                //dao.close();
                det = "ISNULL(i.DetalleAsunto,'')";
            }
            //else
            //    dao.close();
            string result;
            result = "SELECT " +
                    " i.CodigoCompuesto as rCodigoCompuesto" +
                    ",i.Estado as rEstado" +
                    ",ISNULL(i.Asunto,'') as rAsunto " +
                    "," + det + " as rDetalle" +
                    ",ISNULL(i.DetalleSolucion,'') as rSolucion " +
                    ", ISNULL(CONVERT(char(11),i.Apertura,106) + ' ' + CONVERT(char(5),i.Apertura,114),'') as rApertura " +
                    ", ISNULL(CONVERT(char(11),i.Clausura,106) + ' ' + CONVERT(char(5),i.Clausura,114),'') as rClausura " +
                    ", ISNULL(CONVERT(char(11),i.Limite,106) + ' ' + CONVERT(char(5),i.Limite,114),'') as rLimite " +
                                    ",i.Responsable as rResCodigo " +
                                    ",ISNULL(u.login,'') as rResNombre " +
                                    ",i.TipoSolicitante as rTipoSolicitante" +
                                    ",i.Solicitante as rSolCodigo " +
                                    ", ISNULL(CASE i.TipoSolicitante  " +
                                "		WHEN 1 THEN un.Nombre  " +
                                        "WHEN 2 THEN gs.Nombre  " +
                                        "WHEN 3 THEN zo.Nombre  " +
                                        "WHEN 4 THEN u3.Nombre  " +
                                        "WHEN 5 THEN u4.Nombre  " +
                                        "WHEN 6 THEN ss.Nombre  " +
                                    "END,'') AS rSolNombre " +
                                    ",i.Levanto as rLevCodigo " +
                                    ",ISNULL(u2.Nombre,'') as rLevNombre " +
                                    ",i.Tipificacion as rTipCodigo " +
                                    ",ISNULL(dbo.Tipificacion(i.Tipificacion),'') as rTipNombre " +
                                //",1 as Validaciones " +
                                    ",i.Codigo as rCodigo" +
                                    ",dbo.Derivados(i.Codigo) as rDerivados " +
                    ", ISNULL(DATEDIFF(minute,GETDATE(),i.Limite),1000000) as rLimitev " +
                    ", i.Severidad as rSeveridad " +
                            "FROM 	Incidencias i " +
                                "INNER JOIN Usuarios u on i.responsable=u.codigo  " +
                                "INNER JOIN Usuarios u2 on i.levanto=u2.codigo  " +
                                "LEFT JOIN Unidades un on un.codigo=i.solicitante  " +
                                "LEFT JOIN Internos it on it.codigo=i.solicitante  " +
                                "LEFT JOIN GruposSolucion gs on gs.codigo=i.solicitante  " +
                                "LEFT JOIN Zonas zo on zo.codigo=i.solicitante  " +
                                "LEFT JOIN Sitios ss on ss.codigo=i.solicitante  " +
                                "LEFT JOIN Usuarios u3 on u3.codigo=i.solicitante  " +
                                "LEFT JOIN Usuarios u4 on it.usuario=u4.codigo  ";
            return result;
        }


        [WebMethod(Description = "Te da los Tickets de acuerdo a ciertos filtros")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDataAndCountCompuesto(int startRowIndex, int maximumRows, string filtro, string usu, string sort)
        {
            try
            {
                Usuario usuario = getUsuario(usu);
                dao.close();
                Dictionary<string, object> datos = new Dictionary<string, object>();
                try
                {
                    datos.Add("d", dataCompuesto(startRowIndex, maximumRows, filtro, usuario, sort));
                }
                catch (SqlException e)
                {
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error de sintaxis SQL: " + e.Message);
                    dao.close();
                    return result;
                }
                catch (InvalidCastException e)
                {
                    dao.open();
                    string result = "Ocurrió un error. Código de error: " + dao.logea("Error de conversion: " + e.ToString());
                    dao.close();
                    return result;
                }
                datos.Add("c", count(filtro, usuario));
                JavaScriptSerializer js = new JavaScriptSerializer();
                string strJSON = js.Serialize(datos);
                return strJSON;
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error general al levantar ticket:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }

        [WebMethod(Description = "Obtiene el codigo de la incidencia más reciente con el filtro indicado.")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string Actualiza(int startRowIndex, string filtro, string usu)
        {
            Usuario usuario = getUsuario(usu);
            string command = "";
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            con.Open();
            command = "SELECT top 1 " +
                " i.CodigoCompuesto " +
            "FROM 	Incidencias i INNER JOIN Usuarios u on i.responsable=u.codigo  INNER JOIN Usuarios u2 on i.levanto=u2.codigo " +
            "WHERE i.Codigo NOT IN (SELECT top " + startRowIndex.ToString() + " i.Codigo from Incidencias i  " +
                " where " + Filtro(filtro, usuario) + " order by " + order(usuario) + ") and " + Filtro(filtro, usuario) +
            " ORDER BY " + order(usuario);
            query.CommandText = command;
            try
            {
                command = query.ExecuteScalar().ToString();
            }
            catch
            {
                command = "-1";
            }
            con.Close();
            return command;
        }

        [WebMethod(Description = "Obtiene el codigo de la incidencia más reciente con el filtro indicado.")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ActualizaSort(int startRowIndex, string filtro, string usu, string sort)
        {
            Usuario usuario = getUsuario(usu);
            string command = "";
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            con.Open();
            command = "SELECT top 1 " +
                " i.CodigoCompuesto " +
            "FROM 	Incidencias i INNER JOIN Usuarios u on i.responsable=u.codigo  INNER JOIN Usuarios u2 on i.levanto=u2.codigo " +
            "WHERE i.Codigo NOT IN (SELECT top " + startRowIndex.ToString() + " i.Codigo from Incidencias i  " +
                " where " + Filtro(filtro, usuario) + " order by " + order(usuario, sort) + ") and " + Filtro(filtro, usuario) +
            " ORDER BY " + order(usuario, sort);
            query.CommandText = command;
            //return command;
            try
            {
                command = query.ExecuteScalar().ToString();
            }
            catch
            {
                command = "-1";
            }
            con.Close();
            return command;
        }

        [WebMethod(Description = "Seguimiento")]
        public string Seguimiento(string cod, string obs, string usu, string env, string eve)
        {
            string[] cods = cod.Split(',');
            string[] Lista;
            string mail, nombre;
            SqlDataReader Reader;
            con.Open();
            foreach (string codigo in cods)
            {
                SqlCommand query = new SqlCommand("INSERT INTO Eventos " +
        "                      (Fecha, Observacion, Incidencia, Datos, TipoEvento) " +
        "VALUES     (GETDATE(), '" + obs + "', dbo.CodigoSimple('" + codigo + "'), '{\"usu\":" + Utilidades.Crip(usu) + "}', " + eve + ")", con);
                try
                {
                    query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    query.CommandTimeout = 300;
                }
                query.ExecuteNonQuery();
                if (env == "si")
                {
                    query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') SELECT     Correo " +
                      "FROM         Usuarios u join Incidencias i on i.Responsable=u.Codigo " +
                      "WHERE     (i.Codigo = @cod)";
                    Reader = query.ExecuteReader();
                    if (Reader.Read())
                    {
                        mail = Reader.GetString(0);
                        Reader.Close();
                        query.CommandText = "SELECT     Login " +
                          "FROM         usuarios u " +
                          "WHERE     (usu_codigo = " + Utilidades.Crip(usu) + ")";
                        Reader = query.ExecuteReader();
                        if (Reader.Read())
                        {
                            nombre = Reader.GetString(0);
                            Reader.Close();
                            Lista = mail.Split(';');
                            MailAddressCollection para = new MailAddressCollection();
                            foreach (string mai in Lista)
                                para.Add(new MailAddress(mai.Trim()));
                            Utilidades.EnviaMail(para, "Seguimiento " + codigo, obs, false);
                        }
                    }
                }
            }
            con.Close();
            return "Hecho";
        }

        [WebMethod(Description = "Cancelar Ticket")]
        public string CancelarTicket(string cod, string sol, string usu)
        {
            con.Open();
            string[] cods=cod.Split(',');
            SqlTransaction tr = con.BeginTransaction();
            try
            {
                foreach (string codigo in cods)
                {
                    SqlCommand query = new SqlCommand("DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') UPDATE Incidencias SET Estado=5,DetalleSolucion='" + sol + "',Clausura=getdate() WHERE Codigo=@cod", con, tr);
                    try
                    {
                        query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                    }
                    catch
                    {
                        query.CommandTimeout = 300;
                    }
                    query.ExecuteNonQuery();
                    query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') INSERT Eventos (Incidencia, TipoEvento, Observacion, Datos, Fecha) VALUES (@cod, 11, '" + sol + "', '{\"usu\":" + Utilidades.Crip(usu) + "}', GETDATE())";
                    query.ExecuteNonQuery();
                }
                tr.Commit();
                con.Close();
                return "Cancelacion";
            }
            catch
            {
                tr.Rollback();
                con.Close();
                return "Error";
            }
        }

        [WebMethod(Description = "Tipificar Ticket")]
        public string TipificarTicket(string cod, string tip)
        {
            string result = "";
            string[] cods = cod.Split(',');
            con.Open();
            foreach (string codigo in cods)
            {
                result += codigo + ": ";
                SqlCommand query = new SqlCommand("DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') UPDATE Incidencias SET Tipificacion=" + tip + " WHERE Codigo=@cod", con);
                try
                {
                    query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    query.CommandTimeout = 300;
                }
                query.ExecuteNonQuery();
                result += "Tipificad" + Utilidades.mascfem + ", ";
            }
            con.Close();
            return result.Substring(0, result.Length - 2);
        }

        [WebMethod(Description = "Guarda un Filtro por default", MessageName = "GuardarFiltro")]
        public string GuardarFiltro(string filtro, string usu)
        {
            string usuario = Utilidades.Crip(usu);
            con.Open();
            SqlCommand Qry;
            Qry = new SqlCommand("UPDATE Usuarios SET FiltroDefault = '" + filtro + "' WHERE Codigo = " + usuario, con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            Qry.ExecuteNonQuery();
            con.Close();
            return "guardado";
        }

        [WebMethod(Description = "Te da los Eventos de un Ticket")]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ObtieneEventos(string cod, string usu)
        {
            try
            {
                Usuario usuario = getUsuario(usu);
                List<Dictionary<string, string>> Eventos = new List<Dictionary<string, string>>();
                SqlCommand query = new SqlCommand();
                try
                {
                    query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    query.CommandTimeout = 300;
                }
                SqlDataReader reader;
                query.Connection = con;
                con.Open();
                string where = "";
                if (!usuario.tienePermiso(Permiso.VerNotasPrivadas))
                    where += "TipoEvento<>6 AND ";
                if (!usuario.tienePermiso(Permiso.VerEventosDeEdicion))
                    where += "TipoEvento<>8 AND ";
                where += "(TipoEvento<>7 OR " + usuario.código + " in (select Usuario from Traza Where Incidencia=@cod and Tipo<>2)) and ";
                where = where.Remove(where.Length - 5);
                query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + cod + "') SELECT TipoEvento, ISNULL(Observacion,'') as Observacion, ISNULL(Datos,'{}') as Datos, ISNULL(CONVERT(char(10),Fecha,103) + ' ' + CONVERT(char(5),Fecha,114),'') as Fecha FROM Eventos WHERE Incidencia=@cod AND (" + where + ") ORDER BY convert(varchar(20), Fecha, 120) DESC";

                reader = query.ExecuteReader();
                Dictionary<string, string> eve;
                while (reader.Read())
                {
                    eve = new Dictionary<string, string>();
                    eve.Add("te", reader.GetInt32(0).ToString());
                    eve.Add("on", reader.GetString(1));
                    eve.Add("ds", reader.GetString(2));
                    eve.Add("fa", reader.GetString(3));
                    Eventos.Add(eve);
                }
                reader.Close();
                string usus = "(";
                foreach (Dictionary<string, string> e in Eventos)
                {
                    if (e["ds"].Contains("\"usu\":"))
                    {
                        string u = e["ds"].Substring(e["ds"].IndexOf("\"usu\"") + 6);
                        usus += u.Remove(u.IndexOf(',') == -1 ? u.IndexOf('}') : u.IndexOf(',')) + ",";
                    }
                }
                usus = usus.Remove(usus.Length - 1);
                usus += ")";
                if (usus.Length >= 3)
                {
                    query.CommandText = "Select Codigo, Login FROM usuarios where Codigo in " + usus;
                    reader = query.ExecuteReader();
                    Dictionary<string, string> usuarios = new Dictionary<string, string>();
                    while (reader.Read())
                    {
                        usuarios.Add(reader.GetInt32(0).ToString(), reader.GetString(1).Trim());
                    }
                    reader.Close();
                    foreach (Dictionary<string, string> e in Eventos)
                    {
                        if (e["ds"].Contains("\"usu\":"))
                        {
                            string u = e["ds"].Substring(e["ds"].IndexOf("\"usu\"") + 6);
                            u = u.Remove(u.IndexOf(',') == -1 ? u.IndexOf('}') : u.IndexOf(','));
                            e["on"] = "<b>" + usuarios[u] + ":</b>&nbsp;" + e["on"];
                        }
                    }
                }
                con.Close();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string strJSON = js.Serialize(Eventos);
                return strJSON;
            }
            catch (Exception e)
            {
                try
                {
                    con.Close();
                    return e.ToString();
                }
                catch
                {
                    return e.ToString();
                }
            }
        }

        [WebMethod(Description = "Cambia el solicitante de un ticket")]
        public string CambiaSol(string cod, string sol, string usu)
        {
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            query.CommandText = 
                "INSERT INTO Eventos " +
                    "(Fecha, Observacion, Incidencia, Datos, TipoEvento) " +
                "VALUES (" + 
                    "GETDATE(), " +
                    "'Cambio de solicitante', " +
                    "dbo.CodigoSimple('" + cod + "'), " +
                    "'{\"usu\":" + Utilidades.Crip(usu) + "}', " +
                    "9" +
                ")";
            con.Open();
            query.ExecuteNonQuery();
            string ts;
            switch (sol.Substring(0, 1))
            {
                case "Z": ts = "3"; break;
                case "S": ts = "6"; break;
                case "C": ts = "4"; break;
                default: ts = "1"; break;
            }
            query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + cod + "') UPDATE Incidencias SET Solicitante=" + sol.Substring(1) + ", TipoSolicitante=" + ts + " WHERE Codigo=@cod";
            query.ExecuteNonQuery();
            con.Close();
            return "Enviada";
        }
        [WebMethod(Description = "Rechazar solicitud de validación")]
        public string Rechazar(string cod, string men, string usu)
        {
            string us = Utilidades.Crip(usu);
            bool notificar = false;
            Int32 ncodig = 0;
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            string result = "";
            if (us.Length == 0)
                return "Volver a ingresar";
            string[] cods = cod.Split(',');
            con.Open();
            foreach (string codigo in cods)
            {
                query.CommandText =
                    "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') " +
                    "SELECT " +
                        "CASE " +
                            "WHEN " +
                                "(SELECT Estado FROM Incidencias WHERE Codigo=@cod)=3 AND " +
                                "(SELECT Responsable FROM Incidencias WHERE Codigo=@cod)=" + us + " " +
                            "THEN (SELECT TOP 1 Usuario FROM Traza WHERE Incidencia=@cod AND Tipo=4 ORDER BY Fecha DESC) " +
                            "ELSE -1 " +
                        "END";
                int res = (int)query.ExecuteScalar();
                if (res != -1)
                {
                    query.CommandText =
                        "INSERT INTO Eventos (Fecha, Observacion, Incidencia, Datos, TipoEvento) " +
                        "VALUES (GETDATE(), '" + men + "', dbo.CodigoSimple('" + codigo + "'), '{\"usu\":" + us + "}', 5); " +
                        "UPDATE Traza SET Tipo=6 WHERE Usuario=" + us + " AND Incidencia=dbo.CodigoSimple('" + codigo + "') AND Tipo=1; " +
                        "INSERT INTO Traza (Usuario, Incidencia, Tipo, Fecha) " +
                        "VALUES (" + res + ", dbo.CodigoSimple('" + codigo + "'), 1, GETDATE()); ";
                    query.ExecuteNonQuery();
                    query.CommandText =
                        "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') " +
                        "UPDATE Incidencias SET Estado=2,Responsable=" + res + " WHERE Codigo=@cod";
                    query.ExecuteNonQuery();
                    query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "')SELECT @cod as codigo";
                    ncodig = (int)query.ExecuteScalar();
                    notificar = true;
                }
                else
                {
                    result += "Error, no es posible rechazar, ";
                    continue;
                }
                result += "Rechazad" + Utilidades.mascfem + ", ";
            }
            con.Close();
            if (notificar)
            {
                notificaTicket(ncodig);
            }
            return result.Substring(0, result.Length - 2);
        }

        [WebMethod(Description = "Validar")]
        public string Validar(string cod, string men, string usu)
        {
            string us = Utilidades.Crip(usu);
            bool notificar = false;
            Int32 ncodig=0;
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            if (us.Length == 0)
                return "Volver a ingresar";
            string[] cods = cod.Split(',');
            string result = "";
            con.Open();
            foreach (string codigo in cods)
            {
                result += codigo + ": ";
                query.CommandText =
                    "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') " +
                    "SELECT " +
                        "CASE " +
                            "WHEN " +
                                "(SELECT Estado FROM Incidencias WHERE Codigo=@cod)=3 AND " +
                                "(SELECT Responsable FROM Incidencias WHERE Codigo=@cod)=" + us + " " +
                            "THEN (SELECT TOP 1 Usuario FROM Traza WHERE Incidencia=@cod AND Tipo=4 ORDER BY Fecha DESC) " +
                            "ELSE -1 " +
                        "END";
                int res = (int)query.ExecuteScalar();
                if (res != -1)
                {
                    query.CommandText =
                        "INSERT INTO Eventos (Fecha, Observacion, Incidencia, Datos, TipoEvento) " +
                        "VALUES (GETDATE(), '" + men.toSQL() + "', dbo.CodigoSimple('" + codigo + "'), '{\"usu\":" + us + "}', 4); " +
                        "UPDATE Traza SET Tipo=5 WHERE Usuario=" + us + " AND Incidencia=dbo.CodigoSimple('" + codigo + "') AND Tipo=1; " +
                        "INSERT INTO Traza (Usuario, Incidencia, Tipo, Fecha) " +
                        "VALUES (" + res + ", dbo.CodigoSimple('" + codigo + "'), 1, GETDATE()); ";
                    query.ExecuteNonQuery();
                    query.CommandText =
                        "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') " +
                        "UPDATE Incidencias SET Estado=2,Responsable=" + res + " WHERE Codigo=@cod";
                    query.ExecuteNonQuery();
                    query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "')SELECT @cod as codigo";
                    ncodig = (int)query.ExecuteScalar();
                    notificar = true;
                    result += "Validad" + Utilidades.mascfem + ", ";
                }
                else
                {
                    result += "Error, validación imposible, ";
                }
            }
            con.Close();
            if (notificar)
            {
                notificaTicket(ncodig);
            }
            return result.Substring(0, result.Length - 2);
        }

        [WebMethod(Description = "Solicitud de Validación")]
        public string solVal(string cod, int res, string men, string usu)
        {
            bool notificar = false;
            Int32 ncodig = 0;
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            if (Utilidades.Crip(usu).Length == 0)
                return "Volver a ingresar";
            string result="";
            string[] cods = cod.Split(',');
            con.Open();
            foreach (string codigo in cods)
            {
                query.CommandText =
                    "INSERT INTO Eventos (Fecha, Observacion, Incidencia, Datos, TipoEvento) " +
                    "VALUES (" +
                        "GETDATE(), " +
                        "'" + men.toSQL() + "', " +
                        "dbo.CodigoSimple('" + codigo + "'), " +
                        "'{\"usu\":" + Utilidades.Crip(usu) + "}'" +
                        ", 3" +
                    "); " +
                    "UPDATE Traza " +
                    "SET Tipo=4 " +
                    "WHERE Incidencia=dbo.CodigoSimple('" + codigo + "') AND Tipo=1; " +
                    "INSERT INTO Traza (Usuario, Incidencia, Tipo, Fecha) " +
                    "VALUES (" + res + ", dbo.CodigoSimple('" + codigo + "'), 1, GETDATE()); ";
                query.ExecuteNonQuery();
                query.CommandText =
                    "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') " +
                    "UPDATE Incidencias SET Estado=3,Responsable=" + res.ToString() + " WHERE Codigo=@cod";
                query.ExecuteNonQuery();
                query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "')SELECT @cod as codigo";
                ncodig = (int)query.ExecuteScalar();
                notificar = true;
                result += "Enviad" + Utilidades.mascfem + ", ";
            }
            con.Close();
            if (notificar)
            {
                notificaTicket(ncodig);
            }
                return result.Substring(0,result.Length-2);
        }

        [WebMethod(Description = "Cerrar Ticket")]
        public string Cerrar(string cod, int conc, string sol)
        {
            string result = "";
            string[] cods = cod.Split(',');
            dao.open();
            bool continuar = true;
            foreach (string codigo in cods)
            {
                result += codigo + ": ";
                Ticket ticket = dao.getTicket(codigo);
                foreach (Ticket derivado in ticket.getDerivados())
                {
                    if (derivado.estado < 4)
                    {
                        result += "No es posible cerrar, " + Utilidades.el + " " + Utilidades.ticket + " " + derivado.códigoCompuesto + " está abiert" + Utilidades.mascfem + ", ";
                        continuar = false;
                        break;
                    }
                }
                if (!continuar)
                {
                    continuar = true;
                    continue;
                }
                ticket.solución = sol;
                ticket.conclusión = dao.getConclusión(conc);
                ticket.estado = 4;
                ticket.clausura = DateTime.Now;
                ticket.guardar();
                result += "Cerrad" + Utilidades.mascfem + ", ";
            }
            dao.close();
            return result.Substring(0, result.Length - 2);
        }

        [WebMethod(Description = "Cambia la prioridad de uno o varios tickets")]
        public string Priorizar(string codigos, int prioridad, string observacion, string usu)
        {
            Usuario usuario = getUsuario(usu);
            if (!usuario.tienePermiso(Permiso.Priorizar))
            {
                return "El usuario no tiene privilegios para priorizar";
            }
            if (prioridad < 2 && !usuario.tienePermiso(Permiso.PriorizarCritico))
            {
                return "El usuario no tiene privilegios para priorizar a este nivel";
            }
            dao.open();
            string[] cods = codigos.Split(',');
            foreach (string cod in cods)
            {
                Ticket ticket = dao.getTicket(cod);
                ticket.prioridad = (Prioridad)prioridad;
                ticket.eventos.Add(new Evento(TipoEvento.Priorización, usuario.código, observacion, DateTime.Now));
                ticket.guardar();
            }
            dao.close();
            return "Exito";
        }

        [WebMethod(Description = "Escalar ticket")]
        public string Escalar(string cod, int res, string men, string usu)
        {
            SqlCommand query = new SqlCommand();
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            query.Connection = con;
            string result = "";
            string[] cods = cod.Split(',');
            con.Open();
            foreach (string codigo in cods)
            {
                result += codigo + ": ";
                SqlTransaction tr = con.BeginTransaction();
                query.CommandText = "INSERT INTO Eventos " +
    "                      (Fecha, Observacion, Incidencia, Datos, TipoEvento) " +
    "VALUES     (GETDATE(), '" + men + "', dbo.CodigoSimple('" + codigo + "'), '{\"usu\":" + Utilidades.Crip(usu) + "}', 1); " +
                    "UPDATE Traza SET Tipo=3 WHERE Incidencia=dbo.CodigoSimple('" + codigo + "') AND Tipo=1; " +
                    "INSERT INTO Traza (Usuario, Incidencia, Tipo, Fecha) VALUES (" + res + ", dbo.CodigoSimple('" + codigo + "'), 1, GETDATE()); ";
                query.Transaction = tr;
                try
                {
                    query.ExecuteNonQuery();
                    query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "') UPDATE Incidencias SET Responsable=" + res.ToString() + " WHERE Codigo=@cod";
                    query.ExecuteNonQuery();
                    tr.Commit();
                    SqlDataReader Reader;
                    //try
                    //{
                        query.CommandText = "SELECT     u.Correo, u.Celular, u.SMS, ue.Nombre, ue.Login " +
                            "FROM         Usuarios u CROSS JOIN Usuarios ue " +
                            "WHERE     (u.codigo = " + res + " AND ue.Codigo = " + Utilidades.Crip(usu) + ")";
                        Reader = query.ExecuteReader();
                    //}
                    //catch
                    //{
                    //    dao.open();
                    //    result += dao.logea("Error de sintaxis en Escalar: " + query.CommandText) + ", ";
                    //    dao.close();
                    //}
                    //try
                    //{
                        if (Reader.Read())
                        {
                            string mails = Reader.GetString(0);
                            string cels = Reader.GetString(1);
                            bool sms = Reader.GetBoolean(2);
                            string escalador = Reader.GetString(3);
                            string esclogin = Reader.GetString(4);
                            Reader.Close();
                            query.CommandText = "DECLARE @cod int SET @cod=dbo.CodigoSimple('" + codigo + "')  SELECT  ISNULL(CASE i.TipoSolicitante  " +
                        "		WHEN 1 THEN un.Alias  " +
                                "WHEN 2 THEN gs.Nombre  " +
                                "WHEN 3 THEN zo.Nombre  " +
                                "WHEN 4 THEN u3.Login  " +
                                "WHEN 5 THEN u4.Login  " +
                                "WHEN 6 THEN ss.Nombre  " +
                            "END,'') AS SolNombre " +
                            ",ISNULL(u2.Login,'') as LevNombre, i.Asunto, i.DetalleAsunto " +
                    "FROM 	Incidencias i  " +
                        "INNER JOIN Usuarios u2 on i.levanto=u2.codigo  " +
                        "LEFT JOIN Unidades un on un.codigo=i.solicitante  " +
                        "LEFT JOIN Internos it on it.codigo=i.solicitante  " +
                        "LEFT JOIN GruposSolucion gs on gs.codigo=i.solicitante  " +
                        "LEFT JOIN Zonas zo on zo.codigo=i.solicitante  " +
                        "LEFT JOIN Sitios ss on ss.codigo=i.solicitante  " +
                        "LEFT JOIN Usuarios u3 on u3.codigo=i.solicitante  " +
                        "LEFT JOIN Usuarios u4 on it.usuario=u4.codigo  " +
                        "WHERE i.Codigo=@cod ";
                            Reader = query.ExecuteReader();
                            if (Reader.Read())
                            {
                                string asu = Reader.GetString(2);
                                string dasu = Reader.GetString(3);
                                string[] Lista = mails.Split(';');
                                string contenido;
                                MailAddressCollection para = new MailAddressCollection();
                                foreach (string mai in Lista)
                                    if (mai.IndexOf('@') > 0)
                                        para.Add(new MailAddress(mai.Trim()));
                                try
                                {
                                    Utilidades.EnviaMail(para, ConfigurationManager.AppSettings["DeNombre"] + " - " + Reader.GetString(0).Trim(), Utilidades.MailIncidencia("Estimado usuario, el usuario " + escalador + " le ha escalado #un# #ticket#. A continuación se muestra el contenido.", asu, dasu, codigo), true);
                                }
                                catch (Exception e)
                                {
                                    Reader.Close();
                                    result += "No pude enviar el correo :(" + e + ", ";
                                    continue;
                                }
                                if (sms)
                                {
                                    Lista = cels.Split(';');
                                    contenido = "ESC " + esclogin.Trim() + ":" + men.Trim() + " -- " + Reader.GetString(0).Trim() + "-> " + asu.Trim().ToUpper() + " " + dasu.Trim().ToLower();
                                    if (100 < contenido.Length)
                                        contenido = contenido.Remove(100);
                                    foreach (string cel in Lista)
                                        Utilidades.EnviaSMS(cel.Trim(), contenido);
                                }
                                Reader.Close();
                            }
                        }
                        else
                        {
                            result += "no hay mail ni celular, ";
                            continue;
                        }
                    //}
                    //catch (Exception e)
                    //{
                    //    Reader.Close();
                    //    result += "Enviada pero con error al enviar mail o SMS: " + e.ToString() + ", ";
                    //}
                    result += "Enviada, ";
                }
                catch (SqlException e)
                {
                    tr.Rollback();
                    result += "sql-" + e.ToString() + ", ";
                }
            }
            con.Close();
            return result.Substring(0, result.Length - 2);
        }

        [WebMethod(Description = "inserta una dependencia")]
        public string InsertaDependencia(string id_incidencia, string dependencia, DateTime fecha_modificacion)
        {
            SqlCommand Qry;
            Qry = new SqlCommand("IF NOT EXISTS(SELECT dependencia FROM tbl_dependencias WHERE id_incidencia='"+id_incidencia+"')"
                +" BEGIN INSERT INTO tbl_dependencias (id_incidencia,dependencia,fecha_modificacion)" +
                                 " VALUES (" + id_incidencia + ",'" + dependencia + "','" + fecha_modificacion.ToString("dd-MM-yyyy hh:mm:ss") + "') END"
                +" ELSE BEGIN"
                +" UPDATE tbl_dependencias SET dependencia='"+dependencia+"', fecha_modificacion='"+fecha_modificacion.ToString("dd-MM-yyyy hh:mm:ss")
                +"' WHERE id_incidencia='"+id_incidencia+"' END", con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            Utilidades.log(Qry.CommandText);
            con.Open();
            Qry.ExecuteNonQuery();
            con.Close();
            return "dependencia guardada";
        }

        [WebMethod(Description = "guarda histórico de proyectos")]
        public string GuardaHistorico(int id_incidencia_maestra, DateTime fecha_modificacion, string informacion)
        {
            con.Open();
            SqlCommand Qry;
            Qry = new SqlCommand("INSERT INTO tbl_historico_proyectos (id_incidencia_maestra, fecha_modificacion, informacion)" +
                                 " VALUES (" + id_incidencia_maestra + ",GetDate(),'" + informacion+"')", con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            Utilidades.log(Qry.CommandText);
            Qry.ExecuteNonQuery();
            con.Close();
            return "histórico guardado";
        }

        [WebMethod(Description = "lee histórico de proyecto")]
        public string LeeHistorico(int id_incidencia_maestra, DateTime fecha_modificacion)
        {
            SqlCommand Qry;
            Qry = new SqlCommand("SELECT informacion from tbl_historico_proyectos where id_incidencia_maestra=" + id_incidencia_maestra + " and fecha_modificacion='" + fecha_modificacion.ToString("dd-MM-yyyy hh:mm:ss") + "'", con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            Utilidades.log(Qry.CommandText);
            con.Open();
            string res = Qry.ExecuteScalar().ToString();
            con.Close();
            return res;
        }

        [WebMethod(Description = "Lee todas las dependencias dada la incidencia maestra")]
        public string  LeeDependencias(int id_incidencia)
        {
            try
            {
                List<Dictionary<string, string>> Dependencias = new List<Dictionary<string, string>>();
                SqlDataReader reader;
                SqlCommand Qry;
                con.Open();
                Qry = new SqlCommand("SELECT id_incidencia, dependencia from tbl_dependencias WHERE id_incidencia like '" + id_incidencia + "%'", con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                }
                Utilidades.log(Qry.CommandText);
                reader = Qry.ExecuteReader();
                while (reader.Read())
                {
                    Dictionary<string, string> dependencia = new Dictionary<string, string>();
                    dependencia.Add("i", reader.GetString(0));
                    dependencia.Add("d", reader.GetString(1));
                    Dependencias.Add(dependencia);
                }
                reader.Close();
                con.Close();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string strJSON = js.Serialize(Dependencias);
                return strJSON;
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error al leer todas las dependencias dada a la incidencia maestras:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }


		[WebMethod(Description = "lee la lista de todos los históricos de los proyectos")]
        public string ListaDeHistoricos(int id_incidencia_maestra)
        {
            try
            {
                List<Dictionary<string, string>> Historicos = new List<Dictionary<string, string>>();
                SqlDataReader reader;
                con.Open();
                SqlCommand Qry;
                Qry = new SqlCommand("SELECT fecha_modificacion from tbl_historico_proyectos where id_incidencia_maestra=" + id_incidencia_maestra + " ORDER BY fecha_modificacion DESC", con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                }
                Utilidades.log(Qry.CommandText);
                reader = Qry.ExecuteReader();
                while (reader.Read())
                {
                    Dictionary<string, string> historico = new Dictionary<string, string>();
                    historico.Add("f", reader.GetDateTime(0).ToString("dd-MM-yyyy hh:mm:ss"));
                    Historicos.Add(historico);
                }
                reader.Close();
                con.Close();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string strJSON = js.Serialize(Historicos);
                return strJSON;
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error al leer todos los Históricos de los proyectos:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }


        [WebMethod(Description = "lee la lista de todos los históricos de los proyectos")]
        public string Checador(string login, string password, string latitud, string longitud,string Altitude,string Accuracy,string AltitudeAcc,string Heading, string Speed,string hora)
        {

            SqlCommand query = new SqlCommand("SELECT Codigo FROM Usuarios WHERE rtrim(Login)=rtrim('" + login + "') and cast(Clave as" +
                " varchar)='" + Encrip(password) + "'", con);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            try
            {
                int id = (int)query.ExecuteScalar();
                string usu = Utilidades.Crip(((int)query.ExecuteScalar()).ToString());
                con.Close();


                con.Open();
                SqlCommand Qry;

                Qry = new SqlCommand("INSERT INTO Checador (idusuario,latitude,longitude,altitude,accuracy,AltitudeAcc,Heading,Speed, hora,horaservidor)" +
                                     " VALUES (" + id + ",'"+ latitud + "','"+ longitud + "','"+ Altitude+ "','"+ Accuracy+ "','"+ AltitudeAcc+ "','"+ Heading+ "','"+ Speed + "','"+hora + "',  GetDate())", con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                    con.Close();
                    return "Error al insertar datos";
                }
                Utilidades.log(Qry.CommandText);
                Qry.ExecuteNonQuery();
                con.Close();

                return "Satisfactorio";
            }
            catch
            {
                con.Close();
                return "usuario o password invalidos";
            }
        }

        [WebMethod(Description = "lee la lista de todos los históricos de los proyectos")]
        public string ChecadorTelefono(string login, string password, string latitud, string longitud, string Altitude, string Accuracy, string AltitudeAcc, string Heading, string Speed, string hora, string telefono)
        {

            SqlCommand query = new SqlCommand("SELECT Codigo FROM Usuarios WHERE rtrim(Login)=rtrim('" + login + "') and cast(Clave as" +
                " varchar)='" + Encrip(password) + "'", con);
            Utilidades.log(query.CommandText);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            try
            {
                int id = (int)query.ExecuteScalar();
                string usu = Utilidades.Crip(((int)query.ExecuteScalar()).ToString());
                con.Close();


                con.Open();
                SqlCommand Qry;

                Qry = new SqlCommand("INSERT INTO Checador (idusuario,latitude,longitude,altitude,accuracy,AltitudeAcc,Heading,Speed, hora,horaservidor,telefono)" +
                                     " VALUES (" + id + ",'" + latitud + "','" + longitud + "','" + Altitude + "','" + Accuracy + "','" + AltitudeAcc + "','" + Heading + "','" + Speed + "','" + hora + "',  GetDate(),'" + telefono + "')", con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                    con.Close();
                    return "Error al insertar datos";
                }
                Utilidades.log(Qry.CommandText);
                Qry.ExecuteNonQuery();
                con.Close();

                return "Satisfactorio";
            }
            catch
            {
                con.Close();
                return "usuario o password invalidos";
            }
        }

        [WebMethod(Description = "lee la lista de todos los históricos de los proyectos")]
        public string ChecadorTelefono2(string login, string password, string latitud, string longitud, string Altitude, string Accuracy, string AltitudeAcc, 
            string Heading, string Speed, string hora, string telefono, int EntradaSalida, string observaciones)
        {

            SqlCommand query = new SqlCommand("SELECT Codigo FROM Usuarios WHERE rtrim(Login)=rtrim('" + login + "') and cast(Clave as" +
                " varchar)='" + Encrip(password) + "'", con);
            Utilidades.log(query.CommandText);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            try
            {
                int id = (int)query.ExecuteScalar();
                string usu = Utilidades.Crip(((int)query.ExecuteScalar()).ToString());
                con.Close();


                con.Open();
                SqlCommand Qry;

                Qry = new SqlCommand("INSERT INTO Checador (idusuario,latitude,longitude,altitude,accuracy,AltitudeAcc,Heading,Speed, hora,horaservidor,telefono,EntradaSalida,  observaciones)" +
                                     " VALUES (" + id + ",'" + latitud + "','" + longitud + "','" + Altitude + "','" + Accuracy + "','" + AltitudeAcc + "','" + Heading + "','" 
                                     + Speed + "','" + hora + "',  GetDate(),'" + telefono + "',"+EntradaSalida+",'"+ observaciones +"' )", con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                    con.Close();
                    return "Error al insertar datos";
                }
                Utilidades.log(Qry.CommandText);
                Qry.ExecuteNonQuery();
                con.Close();

                return "Satisfactorio";
            }
            catch
            {
                con.Close();
                return "usuario o password invalidos";
            }
        }

        [WebMethod(Description = "lee la lista de los checadores")]
        public string ListaChecador(string nombre)
        {
            List<Dictionary<string, string>> ListaC = new List<Dictionary<string, string>>();
            SqlDataReader reader;

            String qry = "select u.Nombre,c.latitude,c.longitude, convert(varchar(17),convert(datetime, c.HoraServidor),113) " +
 "+ ' ' +RIGHT(CONVERT(VARCHAR(26), Convert(datetime,c.HoraServidor), 109), 2) as HoraServidor,c.Accuracy,u.login  from CustomerCare.dbo.checador C " +
                                              " inner join CustomerCare.dbo.Usuarios u on u.Codigo=c.idusuario where horaservidor>DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0) order by u.nombre, c.horaservidor desc;";            con.Open();
            SqlCommand Qry = new SqlCommand(qry, con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            Utilidades.log(Qry.CommandText);
            reader = Qry.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> checador = new Dictionary<string, string>();
                checador.Add("n",reader.GetString(0));
                checador.Add("lat", reader.GetString(1));
                checador.Add("lon", reader.GetString(2));
                checador.Add("f", reader.GetString(3));
                checador.Add("acc", reader.GetString(4));
                checador.Add("log", reader.GetString(5));
                ListaC.Add(checador);
            }
            reader.Close();
            con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(ListaC);
            return strJSON;
        }

        [WebMethod(Description = "Se trae la el histórico de registros por persona en el checador")]
        public string ListaCheca(string nombre)
        {
            List<Dictionary<string, string>> ListaC = new List<Dictionary<string, string>>();
            SqlDataReader reader;

            String qry = "select u.Nombre,c.latitude,c.longitude, convert(varchar(17),convert(datetime, c.HoraServidor),113) " +
 "+ ' ' +RIGHT(CONVERT(VARCHAR(26), Convert(datetime,c.HoraServidor), 109), 2) as HoraServidor,c.Accuracy,u.login  from CustomerCare.dbo.checador C " +
                        " inner join CustomerCare.dbo.Usuarios u on u.Codigo=c.idusuario " +
                        " where Nombre ='" + nombre + "' and " +
                        " horaservidor>DATEADD(day, DATEDIFF(day, 15, GETDATE()), 0) order by c.horaservidor desc;"; con.Open();
            SqlCommand Qry = new SqlCommand(qry, con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            Utilidades.log(Qry.CommandText);
            reader = Qry.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> checador = new Dictionary<string, string>();
                checador.Add("n", reader.GetString(0));
                checador.Add("lat", reader.GetString(1));
                checador.Add("lon", reader.GetString(2));
                checador.Add("f", reader.GetString(3));
                checador.Add("acc", reader.GetString(4));
                checador.Add("log", reader.GetString(5));
                ListaC.Add(checador);
            }
            reader.Close();
            con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(ListaC);
            return strJSON;
        }

        [WebMethod(Description = "Se trae la el histórico de registros usuario y login")]
        public string RegistrosUsuario(string usuario, string password)
        {
            List<Dictionary<string, string>> ListaC = new List<Dictionary<string, string>>();
            SqlDataReader reader;

            SqlCommand query = new SqlCommand("SELECT Codigo FROM Usuarios WHERE rtrim(Login)=rtrim('" + usuario + "') and cast(Clave as" +
                " varchar)='" + Encrip(password) + "'", con);
            Utilidades.log(query.CommandText);
            try
            {
                query.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                query.CommandTimeout = 300;
            }
            con.Open();
            try
            {
                int id = (int)query.ExecuteScalar();
                string usu = Utilidades.Crip(((int)query.ExecuteScalar()).ToString());
                con.Close();
                String qry = "select u.Nombre,c.latitude,c.longitude, convert(varchar(17),convert(datetime, c.HoraServidor),113) " +
                             "+ ' ' +RIGHT(CONVERT(VARCHAR(26), Convert(datetime,c.HoraServidor), 109), 2) as HoraServidor,c.Accuracy,c.EntradaSalida,  c.observaciones  from CustomerCare.dbo.checador C " +
                            " inner join CustomerCare.dbo.Usuarios u on u.Codigo=c.idusuario " +
                            " where codigo ='" + Convert.ToString(id) + "' and " +
                            " horaservidor>DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0) order by c.horaservidor desc;"; con.Open();
                SqlCommand Qry = new SqlCommand(qry, con);
                Utilidades.log(Qry.CommandText);
                reader = Qry.ExecuteReader();
                while (reader.Read())
                {
                    Dictionary<string, string> checador = new Dictionary<string, string>();
                    checador.Add("n", reader.GetString(0));
                    checador.Add("lat", reader.GetString(1));
                    checador.Add("lon", reader.GetString(2));
                    checador.Add("f", reader.GetString(3));
                    checador.Add("acc", reader.GetString(4));
                    checador.Add("ent", Convert.ToString(reader.GetInt32(5)));
                    checador.Add("obs", reader.GetString(6));
                    ListaC.Add(checador);
                }
                reader.Close();
                con.Close();
                JavaScriptSerializer js = new JavaScriptSerializer();
                string strJSON = js.Serialize(ListaC);
                return strJSON;
            }
            catch
            {
                return "[{}]";
            }
        }

        [WebMethod(Description = "Se trae la el histórico de registros DEL CHECADOR EN UNA FECHA EN ESPECÍFICO")]
        public string ListaChecadorPorFecha(string fecha, int codigo)
        {
            List<Dictionary<string, string>> ListaC = new List<Dictionary<string, string>>();
            SqlDataReader reader;

            String qry = "select u.Nombre,c.latitude,c.longitude, convert(varchar(17),convert(datetime, c.HoraServidor),113) " +
                        "+ ' ' +RIGHT(CONVERT(VARCHAR(26), Convert(datetime,c.HoraServidor), 109), 2) as HoraServidor,c.Accuracy,u.login, 'Oficina' as Zona  from CustomerCare.dbo.checador C " +
                        " inner join CustomerCare.dbo.Usuarios u on u.Codigo=c.idusuario " +
                        " where horaservidor>DATEADD(day, DATEDIFF(day, 0, convert(datetime, '" + fecha + "', 103)), 0) " +
                        " and horaservidor<DATEADD(day, DATEDIFF(day, -1, convert(datetime, '" + fecha + "', 103)), 0) order by u.nombre, c.horaservidor desc;";
            if (codigo != 0)
            {
                qry = "select u.Nombre,c.latitude,c.longitude, convert(varchar(17),convert(datetime, c.HoraServidor),113) " +
                        "+ ' ' +RIGHT(CONVERT(VARCHAR(26), Convert(datetime,c.HoraServidor), 109), 2) as HoraServidor,c.Accuracy,u.login, 'Oficina' as Zona  from CustomerCare.dbo.checador C " +
                        " inner join CustomerCare.dbo.Usuarios u on u.Codigo=c.idusuario " +
                        " where u.codigo=" + codigo +" and horaservidor>DATEADD(day, DATEDIFF(day, 0, convert(datetime, '" + fecha + "', 103)), 0) " +
                        " and horaservidor<DATEADD(day, DATEDIFF(day, -1, convert(datetime, '" + fecha + "', 103)), 0) order by u.nombre, c.horaservidor desc;";
            }

            con.Open();
            SqlCommand Qry = new SqlCommand(qry, con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            Utilidades.log(Qry.CommandText);
            reader = Qry.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> checador = new Dictionary<string, string>();
                checador.Add("n", reader.GetString(0));
                checador.Add("lat", reader.GetString(1));
                checador.Add("lon", reader.GetString(2));
                checador.Add("f", reader.GetString(3));
                checador.Add("acc", reader.GetString(4));
                checador.Add("log", reader.GetString(5));
                checador.Add("zona", reader.GetString(6));
                ListaC.Add(checador);
            }
            reader.Close();
            con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(ListaC);
            return strJSON;
        }

        [WebMethod(Description = "SE trae los proyectos (posibles)")]
        public string ListaDeProyectos(int resp)
        {
            List<Dictionary<string, string>> ListaP = new List<Dictionary<string, string>>();
            SqlDataReader reader;

            String qry = "select i.CodigoCompuesto, Asunto,Apertura, Limite, u2.Nombre as UsuResp,t.Descripcion, " +
                        "ISNULL(  " +
                        "    CASE i.TipoSolicitante  " +
                        "        WHEN 1 THEN un.Nombre  " +
                        "        WHEN 3 THEN zo.Nombre  " +
                        "        WHEN 4 THEN u3.Nombre  " +
                        "        WHEN 5 THEN u4.Nombre  " +
                        "        WHEN 6 THEN ss.Nombre  " +
                        "    END,  " +
                        "    '' " +
                        "    ) AS SolNombre ,dbo.Tipificacion(i.Tipificacion) as Tipificacion, DetalleAsunto,Solucion from  " +
                        //"(select origen from Dependencias where Origen in  " +
                        //"(select codigo  from Incidencias where Responsable=168 and Estado<4 and CAST(Codigo as varchar(10))=codigocompuesto) " +
                        //"group by origen) as O left join  " +
                        //"Incidencias i on O.Origen=i.Codigo left join  " +
                        "Incidencias i left join " + 
                        "Unidades un on un.codigo=i.solicitante LEFT JOIN  " +
	                    "Zonas zo on zo.codigo=i.solicitante LEFT JOIN  " +
	                    "Sitios ss on ss.codigo=i.solicitante LEFT JOIN  " +
	                    "Usuarios u3 on u3.codigo=i.solicitante LEFT JOIN " +
	                    "Internos it on it.codigo=i.solicitante LEFT JOIN  " +
	                    "Usuarios u4 on it.usuario=u4.codigo LEFT JOIN " +
                        "Usuarios u2 on i.responsable=u2.codigo LEFT JOIN " +
	                    "TiposSolicitante t on t.Codigo=i.TipoSolicitante " +
                        "where i.Responsable=" + Convert.ToString(resp) + " and i.Estado<4 ; "; 
            con.Open();
            SqlCommand Qry = new SqlCommand(qry, con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            Utilidades.log(Qry.CommandText);
            reader = Qry.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> proyecto = new Dictionary<string, string>();
                proyecto.Add("cod", reader.GetString(0).Trim());
                proyecto.Add("Asu", reader.GetString(1));
                proyecto.Add("Ape", reader.GetDateTime(2).ToString("dd-MM-yyyy"));
                proyecto.Add("Lim", reader.GetDateTime(3).ToString("dd-MM-yyyy"));
                proyecto.Add("Resp",reader.GetString(4));
                proyecto.Add("Tipo",reader.GetString(5));
                proyecto.Add("Soli", reader.GetString(6));
                proyecto.Add("Tipi", reader.GetString(7));
                proyecto.Add("DAsu", reader.GetString(8));
                proyecto.Add("Solu", reader.GetString(9));
                
                ListaP.Add(proyecto);
            }
            reader.Close();
            con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(ListaP);
            return strJSON;

        }

        [WebMethod(Description = "Este reporte consoluta las incidencias de todo el personal y las separa por cerrada,abiertas, al mes etc.")]
        public string ReporteGeneral(int anio, int mes, int codigo)
        {
            List<Dictionary<string, string>> ListaP = new List<Dictionary<string, string>>();
            SqlDataReader reader;


            String qry = "SELECT     Usuario,[AbiertasTotales]as [Activas Totales],[Abiertas] as [Abiertas en el mes], Totales as [Incidencias cerradas en el mes], [En tiempo] as [Cerradas en tiempo], 100 * [En tiempo] / Totales AS Porcentaje, [AbiertasEnValidacion],Codigo, Login, 'Oficina' as Zona " +
                        "FROM         (SELECT  u.login, u.Codigo,  u.Nombre AS Usuario, Totales, " +
                        "                              (SELECT     COUNT(*) " +
                        "                                FROM          incidencias x " +
                        "                                WHERE     x.estado = 4 AND year(x.clausura) = " + anio + " AND month(x.clausura)= " + mes +" AND (x.limite IS NULL OR " +
                        "                               x.clausura IS NOT NULL AND x.clausura <= x.limite) AND x.responsable = a.responsable) AS [En tiempo], " +
                        "                               (SELECT     COUNT(*) " +
                        "                                  FROM          incidencias x " +
                        "                                  WHERE   x.estado = 2 AND x.responsable = a.responsable) AS [AbiertasTotales], " +
                        "                                 (SELECT     COUNT(*) " +
                        "                                  FROM          incidencias x " +
                        "                                  WHERE   x.estado = 3 AND x.responsable = a.responsable) AS [AbiertasEnValidacion],  " +
                        "                               (SELECT     COUNT(*) " +
                        "                                FROM          incidencias x " +
                        "                                WHERE   x.estado > 1 AND x.estado < 5 AND year(x.Apertura) = " + anio +" AND month(x.Apertura)= " + mes + "  " +
                        "                                AND x.responsable = a.responsable) AS [Abiertas], " +
                        "                                1 AS o " +
                        "   FROM          (SELECT     COUNT(*) AS Totales, i.responsable " +
                        "                           FROM          incidencias i " +
                        "                           WHERE    i.estado =4 AND year(clausura) = " + anio + " AND month(clausura) = " + mes +
                        "                           GROUP BY i.responsable) a JOIN " +
                        "                          usuarios u ON a.responsable = u.codigo " +
                        "   ) tabla " +
                        "ORDER BY Porcentaje, [AbiertasTotales] desc;";
            if (codigo != 0)
            {
                qry = "SELECT     Usuario,[AbiertasTotales]as [Activas Totales],[Abiertas] as [Abiertas en el mes], Totales as [Incidencias cerradas en el mes], [En tiempo] as [Cerradas en tiempo], 100 * [En tiempo] / Totales AS Porcentaje, [AbiertasEnValidacion],Codigo, Login, 'Oficina' as Zona " +
                        "FROM         (SELECT  u.login, u.Codigo,  u.Nombre AS Usuario, Totales, " +
                        "                              (SELECT     COUNT(*) " +
                        "                                FROM          incidencias x " +
                        "                                WHERE     x.estado = 4 AND year(x.clausura) = " + anio + " AND month(x.clausura)= " + mes + " AND (x.limite IS NULL OR " +
                        "                               x.clausura IS NOT NULL AND x.clausura <= x.limite) AND x.responsable = a.responsable) AS [En tiempo], " +
                        "                               (SELECT     COUNT(*) " +
                        "                                  FROM          incidencias x " +
                        "                                  WHERE   x.estado = 2 AND x.responsable = a.responsable) AS [AbiertasTotales], " +
                        "                                 (SELECT     COUNT(*) " +
                        "                                  FROM          incidencias x " +
                        "                                  WHERE   x.estado = 3 AND x.responsable = a.responsable) AS [AbiertasEnValidacion],  " +
                        "                               (SELECT     COUNT(*) " +
                        "                                FROM          incidencias x " +
                        "                                WHERE   x.estado > 1 AND x.estado < 5 AND year(x.Apertura) = " + anio + " AND month(x.Apertura)= " + mes + "  " +
                        "                                AND x.responsable = a.responsable) AS [Abiertas], " +
                        "                                1 AS o " +
                        "   FROM          (SELECT     COUNT(*) AS Totales, i.responsable " +
                        "                           FROM          incidencias i " +
                        "                           WHERE    i.estado =4 AND year(clausura) = " + anio + " AND month(clausura) = " + mes +
                        "                           GROUP BY i.responsable) a JOIN " +
                        "                          usuarios u ON a.responsable = u.codigo where u.codigo=" +codigo +
                        "   ) tabla " +
                        "ORDER BY Porcentaje, [AbiertasTotales] desc;";
            }

            con.Open();
            SqlCommand Qry = new SqlCommand(qry, con);
            try
            {
                Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
            }
            catch
            {
                Qry.CommandTimeout = 300;
            }
            Utilidades.log(Qry.CommandText);
            reader = Qry.ExecuteReader();
            while (reader.Read())
            {
                Dictionary<string, string> proyecto = new Dictionary<string, string>();
                proyecto.Add("usu", reader.GetString(0));
                proyecto.Add("at", Convert.ToString(reader.GetInt32(1)));
                proyecto.Add("am", Convert.ToString(reader.GetInt32(2)));
                proyecto.Add("icm", Convert.ToString(reader.GetInt32(3)));
                proyecto.Add("ct", Convert.ToString(reader.GetInt32(4)));
                proyecto.Add("p", Convert.ToString(reader.GetInt32(5)));
                proyecto.Add("av", Convert.ToString(reader.GetInt32(6)));
                proyecto.Add("cod", Convert.ToString(reader.GetInt32(7)));
                proyecto.Add("log", reader.GetString(8));
                proyecto.Add("zona", reader.GetString(9));
                
                ListaP.Add(proyecto);
            }
            reader.Close();
            con.Close();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string strJSON = js.Serialize(ListaP);
            return strJSON;

        }

        [WebMethod(Description = "insertar un check de usuario")]
        public string InsertaChecada(int idChecador, string fechayHora)
        {
            try
            {
                List<Dictionary<string, string>> Historicos = new List<Dictionary<string, string>>();
                SqlDataReader reader;
                con.Open();
                SqlCommand Qry;
                Qry = new SqlCommand("SELECT idCC from relacioneschecador where idchecador=" + idChecador , con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                }
                Utilidades.log(Qry.CommandText);
                reader = Qry.ExecuteReader();
                int idCC = 0;
                while (reader.Read())
                {
                    idCC = reader.GetInt32(0);
                }
                reader.Close();
                Qry.Dispose();
                if (idCC != 0)
                {
                    string strQry = "INSERT INTO [CustomerCare].[dbo].[Checador] ([Latitude] ,[Longitude],[Altitude],[Accuracy],[AltitudeAcc],HORA,[idUsuario] ,[HoraServidor],[Telefono]) " +
                          " VALUES (20.6601624,-103.3820815,NULL,0,NULL,1," + Convert.ToString(idCC) + ", convert(datetime, '" +    fechayHora + "', 103) ,NULL)";
                    Qry = new SqlCommand(strQry, con);
                    reader = Qry.ExecuteReader();
                    Utilidades.log(strQry);

                    // 20.6612166	-103.3832823	null	37.5	null	null	null	1415040871633	1956	2014-11-03 12:44:23.233	NULL
                    con.Close();
                    //JavaScriptSerializer js = new JavaScriptSerializer();
                    return "Dato insertado";
                }
                return "No hay dato";
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error al leer todos los Históricos de los proyectos:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }


        [WebMethod(Description = "Uso temporal de el servio que envia correos de estudios")]
        public string enviaMailRESMD(string strmail, string lab)
        {
            String result = "";
            MailAddressCollection para = new MailAddressCollection();
            para.Add(new MailAddress(strmail));
            try
            {
                Utilidades.EnviaMail(para, "Tiene un nuevo estudio listo para revisar", 
                    Utilidades.MailEstudio("Aquí iría la infromacion", lab), true);
            }
            catch (Exception e)
            {
                result = "!" + e.Message + "?" + result;
            }
            finally
            {
                con.Close();
            }
            return result;
        }


        //LA siguuiente funcio se va a agregar para llevar los registros de HL7
        [WebMethod(Description = "Crea las incidencias concernientes a la Implementación")]
        public string Crea_Implementacion(int Codigo, int usu, int res, string sol, int tip, string rin, int conc, string solu, DateTime lim, string ina, DateTime ini, int pri)
        {
        //    Crea_Implementacion(Codigo, usu, res, sol, 34, rin, conc, solu, lim, ina, ini, pri);
            try
            {
                SqlDataReader reader;
                con.Open();
                SqlCommand Qry;

                string strQry = "Select Nombre from unidades where codigo = " + Convert.ToString(sol).Substring(1);

                //  NuevoTicketDerivado(string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, string origen, DateTime lim, string ina, DateTime ini, int pri)
                Qry = new SqlCommand(strQry, con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                }
                Utilidades.log(strQry);
                reader = Qry.ExecuteReader();
                string  nombreUnidad = "";
                while (reader.Read())
                {
                    nombreUnidad = reader.GetString(0);
                }
                con.Close();

                if (nombreUnidad != "")
                {
                    string detalles;
                    NuevoTicketDerivado("Se solicita realizar  reconocimiento de área en el:", nombreUnidad , usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita preparar sistema, datos para el sistema aplicativo en el:", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    detalles = "Un servidor XXXX , con XXX HHDD SAS DE XXX GB, con XX GB de Ram, XX nucleos, XX fuentes redundantes,  XX nobreack XXX, XX estaciones de trabajo completas, teclado,  mouse, monitor, nobreack,  XX lector de código de barras, XX impresoras de etiquetas de tubo XXX,  XX  impresoras laser HP.";
                    NuevoTicketDerivado("Se solicita el siguiente equipo :", nombreUnidad + " " + detalles, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    detalles = "Instalar  Host, y Hyper-V, Plantillas de virtuales, y configurar dominio. HG:  XX sesiones de usuarios de Pasteur , con XX interfaces";
                    NuevoTicketDerivado("Se solicita preparar servidor para el :", nombreUnidad + " " + detalles, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    detalles = "XX toner HP , XX rollos de etiquetas de tubo, XX  paquetes de hojas t/c.";
                    NuevoTicketDerivado("Se solicita consumibles de inicio, para el:", nombreUnidad + " " + detalles, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita trasladar equipo al :", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita Instalar y configurar el equipos en el:", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita configurar Sistema y realizar pruebas de funcionalidad, en el :", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se requiere apoyo para mejoras y configuración de datos en el sistema aplicativo, en el :", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita apoyo para configurar e instalar interfaces en el:", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    detalles = "Se requiere programar capacitación y arranque, para utilizar el sistema aplicativo , preparar los números de inicio, formatos impresos, firmas en formatos, etc."+
                                "Actas de Entrega de Equipo (F3-PR-S-07) " +
                                "Acta de Arranque (F3-PR-S-09) " +
                                "Acta de Capacitación (F1-PR-S-09) " +
                                "Calendario de Mantenimiento(F1-PR-S-08) " +
                                "Lista de Asistencia de la capacitación (F4-PR-A-01) " +
                                "Evaluación a la Capacitación (F3-PR-A-01)";
                    NuevoTicketDerivado("Se solicita Capacitar y arrancar operaciones :", nombreUnidad + " " + detalles, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    
                }

                return "Se crearon la incidencias correspondientes a la Implementación de " + nombreUnidad+ " con código Principal: " +Convert.ToString(Codigo);
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error al leer todos los Históricos de los proyectos:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }


        //LA siguuiente funcio se va a agregar para llevar los registros de HL7
        [WebMethod(Description = "Crea las incidencias concernientes a la Implementación dado la incidencia padre")]
        public string Crea_Implementacion_Codigo(string Codigo, int usu)
        {
            //    Crea_Implementacion(Codigo, usu, res, sol, 34, rin, conc, solu, lim, ina, ini, pri);
            int res = 0; string sol = ""; int tip = 0; string rin = ""; int conc = 0; string solu = ""; DateTime lim = DateTime.Now; string ina = ""; DateTime ini = DateTime.Now; int pri = 0;
            try
            {
                SqlDataReader reader;

                con.Open();
                SqlCommand Qry;
              
                string strQry = "select * from Incidencias where Codigo= dbo.CodigoSimple('"+Codigo+"')";

                //  NuevoTicketDerivado(string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, string origen, DateTime lim, string ina, DateTime ini, int pri)
                Qry = new SqlCommand(strQry, con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                }
                Utilidades.log(strQry);
                reader = Qry.ExecuteReader(); 
                string nombreUnidad = "";
                while (reader.Read())
                {
                    res = reader.GetInt32(14);
                    sol = "1" + Convert.ToString( reader.GetInt32(5)) ;
                    tip = Convert.ToInt32( reader.GetString(6) );
                    rin = "0";
                    conc = 0;
                    solu = "";
                    lim = reader.GetDateTime(4);
                    ina = Convert.ToString( reader.GetInt32(7));
                    ini = reader.GetDateTime(3);
                    pri = 3;

                       // string sol; int tip; string rin; int conc; string solu; DateTime lim; string ina; DateTime ini; int pri;
                }
                con.Close();
                con.Open();
                strQry = "Select Nombre from unidades where codigo = " + Convert.ToString(sol).Substring(1);

                //  NuevoTicketDerivado(string asu, string dasu, int usu, int res, string sol, int tip, string rin, int conc, string solu, string origen, DateTime lim, string ina, DateTime ini, int pri)
                Qry = new SqlCommand(strQry, con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                }
                Utilidades.log(strQry);
                reader = Qry.ExecuteReader();
                nombreUnidad = "";
                while (reader.Read())
                {
                    nombreUnidad = reader.GetString(0);
                }
                con.Close();

                if (nombreUnidad != "")
                {
                    string detalles;
                    NuevoTicketDerivado("Se solicita realizar  reconocimiento de área en el:", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita preparar sistema, datos para el sistema aplicativo en el:", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    detalles = "Un servidor XXXX , con XXX HHDD SAS DE XXX GB, con XX GB de Ram, XX nucleos, XX fuentes redundantes,  XX nobreack XXX, XX estaciones de trabajo completas, teclado,  mouse, monitor, nobreack,  XX lector de código de barras, XX impresoras de etiquetas de tubo XXX,  XX  impresoras laser HP.";
                    NuevoTicketDerivado("Se solicita el siguiente equipo :", nombreUnidad + " " + detalles, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    detalles = "Instalar  Host, y Hyper-V, Plantillas de virtuales, y configurar dominio. HG:  XX sesiones de usuarios de Pasteur , con XX interfaces";
                    NuevoTicketDerivado("Se solicita preparar servidor para el :", nombreUnidad + " " + detalles, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    detalles = "XX toner HP , XX rollos de etiquetas de tubo, XX  paquetes de hojas t/c.";
                    NuevoTicketDerivado("Se solicita consumibles de inicio, para el:", nombreUnidad + " " + detalles, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita trasladar equipo al :", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita Instalar y configurar el equipos en el:", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita configurar Sistema y realizar pruebas de funcionalidad, en el :", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se requiere apoyo para mejoras y configuración de datos en el sistema aplicativo, en el :", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    NuevoTicketDerivado("Se solicita apoyo para configurar e instalar interfaces en el:", nombreUnidad, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);
                    detalles = "Se requiere programar capacitación y arranque, para utilizar el sistema aplicativo , preparar los números de inicio, formatos impresos, firmas en formatos, etc." +
                                "Actas de Entrega de Equipo (F3-PR-S-07) " +
                                "Acta de Arranque (F3-PR-S-09) " +
                                "Acta de Capacitación (F1-PR-S-09) " +
                                "Calendario de Mantenimiento(F1-PR-S-08) " +
                                "Lista de Asistencia de la capacitación (F4-PR-A-01) " +
                                "Evaluación a la Capacitación (F3-PR-A-01)";
                    NuevoTicketDerivado("Se solicita Capacitar y arrancar operaciones :", nombreUnidad + " " + detalles, usu, res, sol, tip, rin, conc, solu, Convert.ToString(Codigo), lim, ina, ini, pri);

                }

                return "Se crearon la incidencias correspondientes a la Implementación de " + nombreUnidad + " con código Principal: " + Convert.ToString(Codigo);
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error al leer todos los Históricos de los proyectos:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }

        //LA siguuiente funcio se va a agregar para llevar los registros de HL7
        [WebMethod(Description = "Ingresa los registros para HL7 desde pasteur")]
        public string InsertaHL7_Envio(int CodigoUnidad, string fecha, int registros)
        {
            try
            {
                SqlDataReader reader;
                con.Open();
                SqlCommand Qry;
                
                string strQry = "INSERT INTO [CustomerCare].[dbo].[HL7_Envios] (CodigoUnidad, fecha,registros,bitacora) " +
                        " VALUES (" + Convert.ToString(CodigoUnidad) + ", convert(datetime, '" + fecha + "', 103) ," + Convert.ToString(registros) +",getdate())";
                Qry = new SqlCommand(strQry, con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                }
                Utilidades.log(strQry);
                reader = Qry.ExecuteReader();
                con.Close();
                return "Dato insertado";
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error al leer todos los Históricos de los proyectos:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }

        //LA siguuiente funcio se va a agregar para llevar los registros de HL7
        [WebMethod(Description = "Inerta un COMENTARIO a la unidad seleccionada según el AÑO-MES en que se registra")]
        public string InsertaComentario(int CodigoUnidad, string firma, string strComentario)
        {
            try
            {
                SqlDataReader reader;
                con.Open();
                SqlCommand Qry;
                DateTime dt = DateTime.Now;

                string AnioMes = dt.ToString("yyyyMM");// String.Format("{0:yyyyMM}", dt);
                string strQry = "SELECT IDPREGUNTA FROM CIBERENCUESTA.DBO.ENCDETALLE WHERE IDENCUESTA IN (SELECT IDENCUESTA FROM CIBERENCUESTA.DBO.ENCUESTAS WHERE MESENCUESTA='" + AnioMes + "') and txtConcepto='Comentarios'";
                Qry = new SqlCommand(strQry, con);
                try
                {
                    Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                }
                catch
                {
                    Qry.CommandTimeout = 300;
                }
                Utilidades.log(strQry);
                reader = Qry.ExecuteReader();

                string idPregunta = "";
                while (reader.Read())
                {
                    idPregunta = reader.GetString(0);
                }
                con.Close();

                if (idPregunta != "")
                {
                    strQry = "INSERT INTO CIBERENCUESTA.DBO.RESPUESTAS(IDPREGUNTA,REQCODIGO,CODRESPABIERTA)VALUES('" + idPregunta +"',"+ CodigoUnidad + ",'" + firma + ": " + strComentario + "');";
                    con.Open();
                    Qry = new SqlCommand(strQry, con);
                    try
                    {
                        Qry.CommandTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timeout"].ToString().Trim());
                    }
                    catch
                    {
                        Qry.CommandTimeout = 300;
                    }
                    Utilidades.log(strQry);
                    reader = Qry.ExecuteReader();
                    con.Close();
                    return "Registro exitoso";
                }
                
                return "No existe la unidad o la encuesta, favor de revisar";
            }
            catch (Exception e)
            {
                con.Close();
                dao.open();
                string result = "Ocurrió un error. Código de error: " + dao.logea("Error al leer todos los Históricos de los proyectos:" + e.Message + " StackTrace:" + e.StackTrace);
                dao.close();
                return result;
            }
        }


    }
}






