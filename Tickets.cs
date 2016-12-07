using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerCare
{
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

    public class Usuario
    {
        public Usuario(int código)
        {
            this.código = código;
        }
        public int código { get; set; }
        public string nombre { get; set; }
        public string login { get; set; }
    }

    public class Solicitante
    {
        public Solicitante(int código, int tipoSolicitante)
        {
            this.código = código;
            this.tipoSolicitante = tipoSolicitante;
        }
        public int código { get; set; }
        public string nombre { get; set; }
        public int tipoSolicitante { get; set; }
    }

    public class Tipificación
    {
        public Tipificación(int código)
        {
            this.código = código;
        }
        public int código { get; set; }
        public string descripción { get; set; }
    }

    public class Ticket
    {
        private DAO dao;
        public Ticket()
        {
            dao = new DAO();
            this.código = null;
            this.estado = 1;
            this.asunto = null;
            this.detalle = null;
            this.solución = null;
            this.apertura = null;
            this.clausura = null;
            this.límite = null;
            this.responsable = null;
            this.solicitante = null;
            this.levantador = null;
            this.tipificación = null;
            this.códigoCompuesto = null;
            this.conclusión = null;
            this.resueltoIngresar = false;
        }

        public Ticket(int código): this()
        {
            this.código = código;
        }

        public Ticket(int código, string códigoCompuesto): this(código)
        {
            this.códigoCompuesto = códigoCompuesto;
        }

        public int prioridad { get; set; }
        public int? código { get; private set; }
        public int estado { get; set; }
        public string asunto { get; set; }
        public string detalle { get; set; }
        public string solución { get; set; }
        public DateTime? apertura { get; set; }
        public DateTime? clausura { get; set; }
        public DateTime? límite { get; set; }
        public Usuario responsable { get; set; }
        public Solicitante solicitante { get; set; }
        public Usuario levantador { get; set; }
        public Tipificación tipificación { get; set; }
        public string códigoCompuesto { get; private set; }
        public Tipificación conclusión { get; set; }
        public bool resueltoIngresar { get; set; }

        public List<Ticket> getDerivados()
        {
            if (this.código is int)
            {
                dao.open();
                List<Ticket> result = dao.derivados((int)this.código);
                dao.close();
                return result;
            }
            else
                return new List<Ticket>();
        }

        public int guardar(){
            dao.open();
            int result=dao.guardaTicket(this);
            dao.close();
            return result;
        }
    }

    public class TicketPreJSON
    {
        public TicketPreJSON(string ao)
        {
            this.ao = ao;
        }
        public int? ts {get;set;}
        public string co { get; set; }
        public int eo { get; set; }
        public string ao { get; set; }
        public string de { get; set; }
        public string sn { get; set; }
        public string aa { get; set; }
        public string ca { get; set; }
        public KeyValuePair<string, string>? le { get; set; }
        public Propiedad re { get; set; }
        public Propiedad se { get; set; }
        public Propiedad lr { get; set; }
        public KeyValuePair<string, string>? tn { get; set; }
        public object vs { get; set; }
        public int hs { get; set; }
    }
}