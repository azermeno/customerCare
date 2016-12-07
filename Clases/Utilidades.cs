using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using Westwind.Web;
using System.Linq;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace CustomerCare
{
    public static class Utilidades
    {
        public static bool masculino { get { return ConfigurationManager.AppSettings["mascfem"] == "o"; } }
        public static string mascfem { get { return ConfigurationManager.AppSettings["mascfem"]; } }
        public static string Ticket { get { return ConfigurationManager.AppSettings["Ticket"]; } }
        public static string ticket { get { return Ticket.ToLower(); } }
        public static string domainInstalacion { get { return ConfigurationManager.AppSettings["domainInstalacion"]; } }
        public static string rutaInstalacion { get { return ConfigurationManager.AppSettings["rutaInstalacion"]; } }
        public static string un { get { if (masculino) return "un"; else return "una"; } }
        public static string el { get { if (masculino) return "el"; else return "la"; } }
        public static string Un { get { if (masculino) return "Un"; else return "Una"; } }
        public static string El { get { if (masculino) return "El"; else return "La"; } }
        public static string los { get { if (masculino) return "los"; else return "las"; } }
        public static string Los { get { if (masculino) return "Los"; else return "Las"; } }
        public static string imagesFolder { get { return "Estilos/" + ConfigurationManager.AppSettings["Telerik.Skin"] + "/Images/"; } }

        public static string MinutosAHumano(int minutos) //convierte un numero de minutos a una cadena más sencilla de entender para el usuario basada en meses, semanas, días y horas
        {
            if (minutos < 5)
                return minutos.ToString() + " min";
            else if (minutos < 25)
                return (5 * (minutos / 5)).ToString() + " min";
            else if (minutos < 60)
                return (15 * (minutos / 15)).ToString() + " min";
            else if (minutos < 85)
                return "una hora";
            else if (minutos < 120)
                return "1h " + (15 * (minutos / 15) - 60).ToString() + " min";
            else if (minutos < 150)
                return "2 horas";
            else if (minutos < 180)
                return "2hs 30min";
            else if (minutos < 12 * 60)
                return (minutos / 60).ToString() + " horas";
            else if (minutos < 24 * 60)
                return "+12 horas";
            else if (minutos < 2 * 24 * 60)
                return "un día";
            else if (minutos < 7 * 24 * 60)
                return (minutos / (60 * 24)).ToString() + " días";
            else if (minutos < 2 * 7 * 24 * 60)
                return "una semana";
            else if (minutos < 4 * 7 * 24 * 60)
                return (minutos / (7 * 60 * 24)).ToString() + " sem";
            else if (minutos < 60 * 24 * 60)
                return "un mes";
            else
                return "meses";
        }

        public static string Crip(string wy)
        {
            string texto = "";
            int c0;
            foreach (char c in wy)
            {
                if (c > 47 && c < 58)
                {
                    c0 = c - 48;
                    texto = texto + (char)(48 + ((c0 + 5) % 10));
                }
                else if (c > 64 && c < 91)
                {
                    c0 = c - 65;
                    texto = texto + (char)(65 + ((c0 + 13) % 26));
                }
                else if (c > 96 && c < 123)
                {
                    c0 = c - 97;
                    texto = texto + (char)(97 + ((c0 + 13) % 26));
                }
                else
                    texto = texto + (char)(c);
            }
            return texto;
        }

        public static string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }

        public static string Mes(int mes)
        {
            switch (mes)
            {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Septiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                default:
                    return "Diciembre";
            }
        }

        public static void EnviaMail(MailAddressCollection Para, string asunto, string contenido, bool html)
        {
            if (Para.Count == 0)
                return;
            var mail = new MailMessage();
            mail.From = new MailAddress(ConfigurationManager.AppSettings["De"], ConfigurationManager.AppSettings["DeNombre"]);
            foreach (MailAddress mai in Para)
                if (mai.Address.IndexOf('@') > 0)
                    mail.To.Add(mai);
            mail.Subject = asunto;
            mail.IsBodyHtml = html;
            mail.Body = contenido;
            var mailclient = new SmtpClient();
            mailclient.UseDefaultCredentials = false;
            mailclient.Credentials = new System.Net.NetworkCredential(
                                             ConfigurationManager.AppSettings["usuariosmtp"],
                                             ConfigurationManager.AppSettings["contrasenasmtp"]);
            mailclient.Host = ConfigurationManager.AppSettings["smtp"];
            mailclient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            mailclient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["ssl"]);
/*
            ServicePointManager.ServerCertificateValidationCallback =

             mailclient.delegate(object s

                 , X509Certificate certificate

                 , X509Chain chain

                 , SslPolicyErrors sslPolicyErrors)

             { return true; };
            */
            mailclient.Send(mail);
        }
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            else
            {
                    return true;
                
            }
        }

        public static void EnviaSMS(string numero, string contenido)
        {
            if (numero.Length == 10)
            {
                switch (ConfigurationManager.AppSettings["ServicioSMS"])
                {
                    case "gateway160":
                        try
                        {
                            string URLString = "http://api.gateway160.com/client/sendmessage";
                            string[] param = new string[] { "ceac", "e5c1512d-e874-4421-bb7b-ba7c680b83a2", numero, "MX", contenido, "0" };
                            string postData = string.Format("accountName={0}&key={1}&phoneNumber={2}&countryCode={3}&message={4}&isUnicode={5}", param);
                            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URLString);
                            request.Method = "post";
                            request.ContentType = "application/x-www-form-urlencoded";
                            Stream dataStream = request.GetRequestStream();
                            dataStream.Write(byteArray, 0, byteArray.Length);
                            dataStream.Close();

                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                            if (HttpStatusCode.OK == response.StatusCode)
                            {
                                Stream receiveStream = response.GetResponseStream();
                                StreamReader sr = new StreamReader(receiveStream);
                                string output = sr.ReadLine();
                                response.Close();

                                if (output == "1")
                                {
                                    return;
                                }
                                else if (output == "0")
                                {
                                    throw new Exception("gateway160 cuenta/contraseña inválida");
                                }
                                else
                                {
                                    throw new Exception("gateway160 error");
                                }

                            }
                            else
                            {
                                //http request error
                                throw new Exception("error de http request");
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception("gateway160", e);
                        }
                    case "routo":
                        try
                        {
                            string number = "52" + numero;
                            RoutoSMSTelecom routo = new RoutoSMSTelecom();
                            routo.SetUser("1172680");
                            routo.SetPass("cxn5thyj");
                            routo.SetNumber(number);
                            routo.SetOwnNumber("523339566013");
                            routo.SetType("SMS");
                            routo.SetMessage(contenido);
                            routo.Send();
                            return;
                        }
                        catch (Exception e)
                        {
                            throw new Exception("routo", e);
                        }
                    default:
                        throw new Exception("Proveedor mal configurado");
                }
            }
            else
                throw new Exception("Número de celular inválido");
        }

        public static string MailIncidencia(string informacion, string asunto, string detalle, string codigo)
        {
            string mail = (new System.IO.StreamReader(Utilidades.rutaInstalacion + "\\html\\mail.html")).ReadToEnd();
            mail = mail.Replace("#asunto#", asunto);
            mail = mail.Replace("#informacion#", informacion);
            mail = mail.Replace("#detalle#", detalle);
            mail = mail.Replace("#href#", "http://" + Utilidades.domainInstalacion + "/CustomerCare.aspx?acc=ticket&tic=#codigo#");
            mail = mail.Replace("#images#", "http://" + Utilidades.domainInstalacion + "/" + Utilidades.imagesFolder + "mail");
            mail = mail.Replace("#ticket#", Utilidades.ticket);
            mail = mail.Replace("#Ticket#", Utilidades.Ticket);
            mail = mail.Replace("#codigo#", codigo);
            mail = mail.Replace("#el#", Utilidades.el);
            mail = mail.Replace("#un#", Utilidades.un);
            mail = mail.Replace("#El#", Utilidades.El);
            mail = mail.Replace("#Un#", Utilidades.Un);
            mail = mail.Replace("#los#", Utilidades.los);
            mail = mail.Replace("#Los#", Utilidades.Los);
            mail = mail.Replace("#mascfem#", Utilidades.mascfem);
            mail = mail.Replace("#dia#", DateTime.Now.ToString("dddd d", new System.Globalization.CultureInfo("es-MX")));
            mail = mail.Replace("#mes#", DateTime.Now.ToString("MMMM", new System.Globalization.CultureInfo("es-MX")));
            mail = mail.Replace("#año#", DateTime.Now.ToString("yyyy"));
            return mail;
        }

        public static string MailEstudio(string idStudy, string lab)
        {
            string mail= null;
            if(lab == "dei")
                mail = (new System.IO.StreamReader(Utilidades.rutaInstalacion + "\\html\\mailDEI.html")).ReadToEnd();
            else if (lab == "nucleo")
                mail = (new System.IO.StreamReader(Utilidades.rutaInstalacion + "\\html\\mailNucleoD.html")).ReadToEnd();
            /*
            mail = mail.Replace("#asunto#", asunto);
            mail = mail.Replace("#informacion#", informacion);
            mail = mail.Replace("#detalle#", detalle);
            mail = mail.Replace("#href#", "http://" + Utilidades.domainInstalacion + "/CustomerCare.aspx?acc=ticket&tic=#codigo#");
            
            mail = mail.Replace("#ticket#", Utilidades.ticket);
            mail = mail.Replace("#Ticket#", Utilidades.Ticket);
            mail = mail.Replace("#codigo#", codigo);
            mail = mail.Replace("#el#", Utilidades.el);
            mail = mail.Replace("#un#", Utilidades.un);
            mail = mail.Replace("#El#", Utilidades.El);
            mail = mail.Replace("#Un#", Utilidades.Un);
            mail = mail.Replace("#los#", Utilidades.los);
            mail = mail.Replace("#Los#", Utilidades.Los);
            mail = mail.Replace("#mascfem#", Utilidades.mascfem);
             * */
            mail = mail.Replace("#images#", "http://" + Utilidades.domainInstalacion + "/" + Utilidades.imagesFolder + "mail");
            mail = mail.Replace("#dia#", DateTime.Now.ToString("dddd d", new System.Globalization.CultureInfo("es-MX")));
            mail = mail.Replace("#mes#", DateTime.Now.ToString("MMMM", new System.Globalization.CultureInfo("es-MX")));
            mail = mail.Replace("#año#", DateTime.Now.ToString("yyyy"));
            return mail;
        }
        public static void log(string cadena)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["logear"].Trim().ToUpper() == "TRUE")
            {
                string fileName_Comp = System.Configuration.ConfigurationManager.AppSettings["ruta_log"].Trim() + "\\CustomerCare.log";
                if (File.Exists(fileName_Comp))
                {
                    DateTime dt = File.GetLastWriteTime(fileName_Comp);
                    if (DateTime.Now.Date > dt.Date)
                    {
                        File.Move(fileName_Comp, System.Configuration.ConfigurationManager.AppSettings["ruta_log"].Trim() + "\\CustomerCare_" + dt.ToString("yyyyMMdd") + ".log");
                    }
                }
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName_Comp, true);
                sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss ") + cadena);
                sw.Close();
            }
        }
    }
}
