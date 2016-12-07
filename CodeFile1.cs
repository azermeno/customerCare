using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Collections.Generic;

namespace Tickets
{
    public class Funciones
    {
        public Funciones()
        {
            this._con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerCare"].ConnectionString);
        }

        public string SQLify(string texto)
        {
            string result;
            result = texto.Replace("'", "''");
            return result;
        }

        public string MinutosAHumano(int minutos) //convierte un numero de minutos a una cadena más sencilla de entender para el usuario basada en meses, semanas, días y horas
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

        public string DeCrip(string wy)
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
        public string EncodeJsString(string s)
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
        public string Mes(int mes)
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
        private SqlConnection _con;
        public SqlConnection con
        {
            get { return _con; }
            set { _con = value; }
        }
    }
    public class Propiedad
    {
        private int _Codigo;
        private string _Nombre;
        public Propiedad()
        {
        }
        public Propiedad(int cod, string nom)
        {
            this._Codigo = cod;
            this._Nombre = nom;
        }
        public int c
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }
        public string n
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
    }
    
   public class Ticket
    {
        private string _Codigo;
        private int _Estado;
        private string _Asunto;
        private string _Detalle;
        private string _Solucion;
        private string _Apertura;
        private string _Clausura;
        private KeyValuePair<string, string>? _Limite;
        private Propiedad _Responsable;
        private int? _TipoSolicitante;
        private Propiedad _Solicitante;
        private Propiedad _Levantador;
        private KeyValuePair<string,string>? _Tipificacion;
        private int _Validaciones;
        private int _Derivados;

        public Ticket(string asu)
        {
            this._Codigo = null;
            this._Estado = 1;
            this._Asunto = asu;
            this._Detalle = null;
            this._Solucion = null;
            this._Apertura = null;
            this._Clausura = null;
            this._Limite = null;
            this._Responsable = null;
            this._TipoSolicitante = null;
            this._Solicitante = null;
            this._Levantador = null;
            this._Tipificacion = null;
            this._Validaciones = 0;
            this._Derivados = 0;
        }
        public int? ts
        {
            get { return _TipoSolicitante; }
            set { _TipoSolicitante = value; }
        }
        public string co
        {
            get { return _Codigo; }
            set { _Codigo = value; }
        }
        public int eo
        {
            get { return _Estado; }
            set { _Estado = value; }
        }
        public string ao
        {
            get { return _Asunto; }
            set { _Asunto = value; }
        }
        public string de
        {
            get { return _Detalle; }
            set { _Detalle = value; }
        }
        public string sn
        {
            get { return _Solucion; }
            set { _Solucion = value; }
        }
        public string aa
        {
            get { return _Apertura; }
            set { _Apertura = value; }
        }
        public string ca
        {
            get { return _Clausura; }
            set { _Clausura = value; }
        }
        public KeyValuePair<string, string>? le
        {
            get { return _Limite; }
            set { _Limite = value; }
        }
        public Propiedad re
        {
            get { return _Responsable; }
            set { _Responsable = value; }
        }
        public Propiedad se
        {
            get { return _Solicitante; }
            set { _Solicitante = value; }
        }
        public Propiedad lr
        {
            get { return _Levantador; }
            set { _Levantador = value; }
        }
        public KeyValuePair<string,string>? tn
        {
            get { return _Tipificacion; }
            set { _Tipificacion = value; }
        }
        public int vs
        {
            get { return _Validaciones; }
            set { _Validaciones = value; }
        }
       public int hs
       {
           get { return _Derivados;}
           set { _Derivados = value; }
       }
    }
}
